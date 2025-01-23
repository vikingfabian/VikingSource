using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.LootFest.GO.Characters.CastleEnemy;

namespace VikingEngine.DSSWars.GameState
{
    abstract class AbsPlayState : AbsDssState
    {
        public WorldResources resources = new WorldResources();
        protected Map.MapLayer_Factions factionsMap;
        protected Map.MapLayer_Overview overviewMap;
        public Map.MapLayer_Detail detailMap;
        public Culling culling;

        public PathUpdateThread[] pathUpdates;
        protected ConcurrentStack<Graphics.VoxelModelInstance> voxelModelInstancesPool_detail = new ConcurrentStack<VoxelModelInstance>();
        protected ConcurrentStack<Graphics.VoxelModelInstance> voxelModelInstancesPool_overview = new ConcurrentStack<VoxelModelInstance>();
        public bool exitThreads = false;
        protected Timer.Basic subTileReloadTimer = new Timer.Basic(1000, true);

        public AbsCutScene cutScene = null;
        protected bool host = true;

        public AbsPlayState() 
            :base() 
        { 
            
        }

        protected void startMapThreads()
        {
            new AsynchUpdateable_TryCatch(asynchMapGenerating, "DSS map gen", 57, System.Threading.ThreadPriority.Normal);
            new AsynchUpdateable_TryCatch(asyncMapBorders, "DSS map borders update", 59, System.Threading.ThreadPriority.Lowest);
        }

        protected void baseInit()
        {
            culling = new Culling();

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();
        }

        bool asynchMapGenerating(int id, float time)
        {
            if (cutScene == null)
            {
                if (!host)
                {
                    overviewMap.refresh_async();
                }

                culling.asynch_update(time);
                DssRef.state.detailMap.asynchUpdate();
                overviewMap.unitMiniModels.asynchUpdate();


            }
            return exitThreads;
        }

        bool asyncMapBorders(int id, float time)
        {
            if (cutScene == null)
            {
                overviewMap.runAsyncTask();
            }
            return exitThreads;
        }
    }
}
