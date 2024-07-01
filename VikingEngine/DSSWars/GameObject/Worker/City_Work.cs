using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        const int WorkTeamSize = 8;

        List<WorkerStatus> workerStatuses = new List<WorkerStatus>();
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        List<WorkerUnit> workerUnits = null;
        public void async_workUpdate()
        {
            if (parentArrayIndex == -1 || debugTagged)
            { 
                lib.DoNothing();
            }

            int idleCount = 0;
            IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
            IntVector2 maxpos = minpos;
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    ++idleCount;
                }

                IntVector2 pos = workerStatuses[i].subTileEnd;
                if (pos.X < minpos.X)
                {
                    minpos.X = pos.X;
                }
                if (pos.X > maxpos.X)
                {
                    maxpos.X = pos.X;
                }

                if (pos.Y < minpos.Y)
                {
                    minpos.Y = pos.Y;
                }
                if (pos.Y > maxpos.Y)
                {
                    maxpos.Y = pos.Y;
                }
            }

            cullingTopLeft = WP.SubtileToTilePos(minpos);
            cullingBottomRight = WP.SubtileToTilePos(maxpos);

            int workTeamCount = workForce.Int() / WorkTeamSize;

            if (workerStatuses.Count < workTeamCount)
            {
                int newWorkers = workTeamCount - workerStatuses.Count;
                IntVector2 startPos = WP.ToSubTilePos_Centered(tilePos);
                for (int i = 0; i < newWorkers; i++)
                {
                    workerStatuses.Add(new WorkerStatus()
                    { 
                        subTileEnd = startPos,
                        subTileStart = startPos,
                    });

                    ++idleCount;
                }
            }            

            if (idleCount > workQue.Count)
            {
                buildWorkQue();
                workQue.Sort((a, b) => a.priority.CompareTo(b.priority));
            }

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatuses[i].carry.amount > 0)
                    {
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.DropOff, WP.ToSubTilePos_Centered(tilePos));
                        //{
                        //    status.work = WorkType.DropOff;
                        //    status.subTileStart = status.subTileEnd;
                        //    status.subTileEnd = WP.ToSubTilePos_Centered(tilePos);
                        //    status.processTimeLengthSec = 100;
                        //    status.processTimeStartStampSec = Ref.TotalGameTimeSec;
                        //}
                        workerStatuses[i] = status;
                    }
                    else if (workQue.Count > 0)
                    {
                        var work = arraylib.PullLastMember(workQue);

                        var status = workerStatuses[i];
                        status.createWorkOrder(work.work, work.subTile);
                        //{
                        //    status.work = work.work;
                        //    status.subTileStart = status.subTileEnd;
                        //    status.subTileEnd = work.subTile;
                        //    status.processTimeLengthSec = 100;
                        //    status.processTimeStartStampSec = Ref.TotalGameTimeSec;
                        //}
                        workerStatuses[i] = status;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (!inRender)
            {
                processAsynchWork();
            }

            void buildWorkQue()
            {
                workQue.Clear();
                //Priority of collect

                //Cirkle outward from city to find resources
                for (int radius = 1; radius < 12; ++radius)
                {
                    ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, radius));
                    
                    while (cirkleLoop.Next())
                    {
                        if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                        {
                            var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                            if (tile.IsLand())
                            {
                                IntVector2 topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                                ForXYLoop subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                                while (subTileLoop.Next())
                                {
                                    var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                    if (subTile.mainTerrain == TerrainMainType.Resourses)
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.PickUp, subTileLoop.Position, 5));
                                    }
                                    else
                                    {
                                        var foil = subTile.GetFoilType();

                                        if (foil == Map.TerrainSubFoilType.TreeSoft ||
                                            foil == Map.TerrainSubFoilType.TreeHard)
                                        {
                                            if (subTile.terrainAmount >= TerrainContent.TreeReadySize)
                                            {
                                                workQue.Add(new WorkQueMember(WorkType.Gather, subTileLoop.Position, 4));
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (workQue.Count >= workTeamCount)
                    {
                        return;
                    }
                }
            }

            void processAsynchWork()
            {
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    var status = workerStatuses[i];
                    if (status.work != WorkType.Idle &&
                        Ref.TotalGameTimeSec > status.processTimeStartStampSec + status.processTimeLengthSec)
                    {
                        //Work complete
                        status.WorkComplete(this);
                        workerStatuses[i] = status;
                    }

                }
            }
        }

        void updateWorkerUnits()
        {
            if (workerUnits != null)
            {
                if (workerUnits.Count < workerStatuses.Count)
                {
                    for (int i = workerUnits.Count; i < workerStatuses.Count; i++)
                    {
                        workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
                    }
                }

                foreach (var w in workerUnits)
                {
                    w.update();
                }
            }
        }

        public void setWorkersInRenderState()
        {
            if (inRender)
            {
                if (workerUnits == null)
                {
                    workerUnits = new List<WorkerUnit>(workerStatuses.Count);
                }
            }
            else
            {
                if (workerUnits != null)
                {
                    foreach (var w in workerUnits)
                    { 
                        w.DeleteMe();
                    }

                    workerUnits = null;
                }
            }
        }

        public void getWorkerStatus(int index, ref WorkerStatus status)
        { 
            status = workerStatuses[index];
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        { 
            workerStatuses[index] = status;
        }
    }

    

    struct WorkQueMember
    {
        public WorkType work;
        public IntVector2 subTile;

        /// <summary>
        /// Goes from 1:lowest to 10: highest
        /// </summary>
        public int priority;

        public WorkQueMember(WorkType work, IntVector2 subTile, int priority)
        {
            this.work = work;
            this.subTile = subTile;
            this.priority = priority;
        }
    }

    enum WorkType
    { 
        Idle,
        Gather,
        PickUp,
        DropOff,
    }
}
