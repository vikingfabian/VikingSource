using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            int idleCount = 0;
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    ++idleCount;
                }
            }

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
            }

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workQue.Count > 0)
                    {

                        var work = arraylib.PullLastMember(workQue);

                        var status = workerStatuses[i];
                        {
                            status.work = work.work;
                            status.subTileStart = status.subTileEnd;
                            status.subTileEnd = work.subTile;
                            status.processTimeLengthSec = 100;
                            status.processTimeStartStampSec = Ref.TotalGameTimeSec;
                        }
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
                //while (workQue.Count < workTeamCount)
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
                                    var foil = subTile.GetFoilType();

                                    if (foil == Map.TerrainSubFoilType.TreeSoft ||
                                        foil == Map.TerrainSubFoilType.TreeHard)
                                    {
                                        if (subTile.terrainValue >= TerrainContent.TreeReadySize)
                                        {
                                            workQue.Add(new WorkQueMember(WorkType.Gather, subTileLoop.Position));
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
                        status.WorkComplete();
                        workerStatuses[i] = status;
                    }

                }
            }
        }

        void updateWorkerUnits()
        {
            if (workerUnits != null)
            {
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
                    for (int i = 0; i < workerStatuses.Count; i++)
                    {
                        workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
                    }
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

    struct WorkerStatus
    {
        public WorkType work;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;

        public void WorkComplete()
        {
            var subTile = DssRef.world.subTileGrid.Get(subTileEnd);
            subTile.SetType(TerrainMainType.DefaultLand, 0, 0);
            DssRef.world.subTileGrid.Set(subTileEnd, subTile);

            work = WorkType.Idle;
        }
    }

    struct WorkQueMember
    {
        public WorkType work;
        public IntVector2 subTile;

        public WorkQueMember(WorkType work, IntVector2 subTile)
        {
            this.work = work;
            this.subTile = subTile;
        }
    }

    enum WorkType
    { 
        Idle,
        Gather,
        Carry,
    }
}
