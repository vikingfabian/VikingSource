using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.Map.Path
{
    class PathUpdateThread
    {
        int index;
        int startFaction;
        int endFaction;

        public PathFindingPool pathFindingPool = new PathFindingPool();
        public DetailPathFindingPool detailPathFindingPool = new DetailPathFindingPool();

        public List<GameObject.Army> ArmiesColl_asyncupdate = new List<Army>();

        public PathUpdateThread(int index, int startFaction, int endFaction, System.Threading.ThreadPriority prio = System.Threading.ThreadPriority.BelowNormal)
        {              
            this.index = index;
            this.startFaction = startFaction;
            this.endFaction = endFaction;

            new AsynchUpdateable_TryCatch(asyncPathUpdate, $"DSS path update {index}", index, prio);
        }

        virtual protected bool asyncPathUpdate(int id, float time)
        {
            //var factions = DssRef.world.factions.counter();
            //while (factions.Next())
            //{
            //    factions.sel.asyncPathUpdate();
            //}
            for (int i = startFaction; i <= endFaction; ++i)
            {
                DssRef.world.factions.Array[i]?.asyncPathUpdate(index);
            }

            return DssRef.state.exitThreads;
        }
    }

    class PathUpdateThread_Player : PathUpdateThread
    {
        public PathUpdateThread_Player(int index)
            :base(index, -1, -1, System.Threading.ThreadPriority.Normal)
        { }

        protected override bool asyncPathUpdate(int id, float time)
        {
            foreach (var p in DssRef.state.localPlayers)
            { 
                p.asyncPlayerPathUpdate(time);
            }

            return DssRef.state.exitThreads;
        }
    }
}
