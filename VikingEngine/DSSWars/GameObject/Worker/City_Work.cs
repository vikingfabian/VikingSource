using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        const int WorkTeamSize = 8;

        List<WorkerStatus> workerStatuses = new List<WorkerStatus>();
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        public void async_workUpdate()
        {
            int workTeamCount = workForce.Int() / WorkTeamSize;

            if (workerStatuses.Count < workTeamCount)
            {
                int newWorkers = workTeamCount - workerStatuses.Count;
                IntVector2 startPos = WP.ToSubTilePos_Centered(tilePos);
                for (int i = 0; i < newWorkers; i++)
                {
                    workerStatuses.Add(new WorkerStatus()
                    { 
                        subTileEnd = tilePos,
                    });
                }
            }

            int idleCount = 0;
            foreach (WorkerStatus workerStatus in workerStatuses)
            {
                if (workerStatus.work == WorkType.Idle)
                { 
                    ++idleCount;
                }
            }

            if (idleCount > workQue.Count)
            {
                buildWorkQue();
            }

            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    var work = arraylib.PullLastMember(workQue);

                    var status = workerStatuses[i];

                    status.work = work.work;
                    status.subTileStart = status.subTileEnd;
                    status.subTileEnd = work.subTile;
                    status.processTimeLengthSec = 0;
                    status.processTimeStartStampSec = Ref.TotalGameTimeSec;
                }
            }


            void buildWorkQue()
            {
                workQue.Clear();
                //Priority of collect

                //Cirkle outward from city to find resources
                while (workQue.Count < workTeamCount)
                {
                    ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, 1));
                    while (cirkleLoop.Next())
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
        }

        
    }

    struct WorkerStatus
    {
        public WorkType work;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;
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
