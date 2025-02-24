using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.LootFest.GO.Characters.CastleEnemy;
using VikingEngine.Network;

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
        public GameMenuSystem menuSystem;
        public SpottedArray<Players.RemotePlayer> remotePlayers = new SpottedArray<Players.RemotePlayer>();
        public List<Players.LocalPlayer> localPlayers;
        public GameEvents events;
        public Progress progress = new Progress();
        public int NextArmyId = 0;
        protected int stepFramesCount = 0;
        public Ambience ambience;

        public AbsPlayState() 
            :base() 
        {
            DssRef.state = this;
           
        }

        public void stepFrames(int frameCount)
        {
            stepFramesCount = frameCount;
            Ref.SetGameSpeed(1f);
            Ref.SetPause(false);
        }

        protected void updateStepFrames()
        {
            if (stepFramesCount > 0)
            {
                if (--stepFramesCount <= 0)
                {
                    Ref.SetPause(true);
                }
            }
        }

        protected void startMapThreads()
        {
            new AsynchUpdateable_TryCatch(asynchMapGenerating, "DSS map gen", 57, System.Threading.ThreadPriority.Normal);
            new AsynchUpdateable_TryCatch(asyncMapBorders, "DSS map borders update", 59, System.Threading.ThreadPriority.Lowest);
        }

        protected void baseInit()
        {
            DssRef.ambience.gameStart();
            culling = new Culling();

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();
        }

        public ConcurrentStack<Graphics.VoxelModelInstance> modelPool(bool detail)
        {
            return detail ? voxelModelInstancesPool_detail : voxelModelInstancesPool_overview;
        }


        virtual public void OnLoadComplete()
        {
           
        }

        protected bool pauseMenuUpdate()
        {
            if (menuSystem.IsOpen())
            {
                menuSystem.menuUpdate();

                if (closeMenuInput_AnyPlayer())
                {
                    menuSystem.closeMenu();
                }

                return true;
            }
            return false;
        }

        public bool closeMenuInput_AnyPlayer()
        {
            foreach (var local in localPlayers)
            {
                if (local.input.Menu.DownEvent)
                {
                    return true;
                }
            }
            return false;
        }

        

        protected bool asynchArmyAiUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    var armiesC = factions.sel.armies.counter();
                    while (armiesC.Next())
                    {
                        armiesC.sel.asynchAiUpdate(time);
                    }
                }
            }
            return exitThreads;
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

        protected bool asynchSleepObjectsUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                if (time > 0)
                {
                    var factions = DssRef.world.factions.counter();
                    while (factions.Next())
                    {
                        if (factions.sel.factiontype == FactionType.SouthHara)
                        {
                            lib.DoNothing();
                        }
                        factions.sel.asynchSleepObjectsUpdate(time);
                    }
                }
            }
            return exitThreads;
        }

        protected override void createDrawManager()
        {
            draw = new DSSWars.DrawGame();
        }


        protected bool asyncBattlesUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    var armiesC = factions.sel.armies.counter();
                    while (armiesC.Next())
                    {
                        var groupsC = armiesC.sel.groups.counter();
                        while (groupsC.Next())
                        {
                            groupsC.sel.asyncBattleUpdate();
                        }
                    }
                }
            }
            return exitThreads;
        }

        protected void initPathFindingThreads(int count)
        {
            pathUpdates = new PathUpdateThread[count + 1];
            int startIx = 0;
            int factionLength = DssRef.world.factions.Count / count;
            for (int i = 0; i < count; i++)
            {
                int end = startIx + factionLength;
                if (i == count - 1)
                {
                    //last
                    end = DssRef.world.factions.Count - 1;
                }
                pathUpdates[i] = new PathUpdateThread(i, startIx, end);
                startIx = end + 1;
            }
            pathUpdates[count] = new PathUpdateThread_Player(count);

        }

        public void exit()
        {
            Ref.music.stop(true);
            exitThreads = true;
            DssRef.ambience.gameEnd();
            new ExitGamePlay();
        }

        public Players.RemotePlayer GetOrCreateRemotePlayer(AbsNetworkPeer peer, int SplitScreenIndex)
        {
            var remotePlayerC = remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                if (remotePlayerC.sel.networkPeer.peer == peer)
                {
                    //TODO return region to AI
                    return remotePlayerC.sel;
                }
            }

            //No found
            peer.initInstancePeers();
            foreach (var ins in peer.instancePeers)
            {
                remotePlayers.Add(new Players.RemotePlayer(ins));
            }
            return (Players.RemotePlayer)peer.instancePeers[SplitScreenIndex].Tag;
        }
        virtual public void OneMinute_Update()
        { }
        public bool IsSinglePlayer()
        {
            return localPlayers.Count == 1;
        }
        public bool IsLocalMultiplayer()
        {
            return localPlayers.Count >= 2;
        }

        virtual public PlayState Game()
        {
            throw new NotImplementedException();
        }

        abstract public PlayStateType PlayType();
    }

    enum PlayStateType
    { 
        Play,
        BattleLab,
    }
}
