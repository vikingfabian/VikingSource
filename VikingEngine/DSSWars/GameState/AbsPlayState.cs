using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.Characters.CastleEnemy;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.GameState
{
    abstract class AbsPlayState : AbsDssState
    {
        public bool isReady = false;
        public bool hasManorLords = false;
        public WorldResources resources = new WorldResources();
        public Map.MapLayer_Factions factionsMap;
        protected Map.MapLayer_Overview overviewMap;
        public Map.MapLayer_Detail detailMap;
        public Culling culling;

        public PathUpdateThread[] pathUpdates;
        protected ConcurrentStack<Graphics.VoxelModelInstance> voxelModelInstancesPool_detail = new ConcurrentStack<VoxelModelInstance>();
        protected ConcurrentStack<Graphics.VoxelModelInstance> voxelModelInstancesPool_overview = new ConcurrentStack<VoxelModelInstance>();

        public ConcurrentStack<VoxelModelInstance_Pooled> voxelModelInstancesPooled = new ConcurrentStack<VoxelModelInstance_Pooled>();

        public bool exitThreads = false;
        protected Timer.Basic subTileReloadTimer = new Timer.Basic(1000, true);

        public AbsCutScene cutScene = null;
        public bool host = true;
        public GameMenuSystem menuSystem;
        public SpottedArray<Players.RemotePlayer> remotePlayers;
        protected SpottedArrayCounter<Players.RemotePlayer> remotePlayersCounter;
        public List<Players.LocalPlayer> localPlayers;
        public EventManager events;
        public Progress progress = new Progress();
        public int NextArmyId = 0;
        protected int stepFramesCount = 0;
        public Ambience ambience;
        public bool importedWorld = false;

        public Stack<SpriteText3D> Text3DPool = new Stack<SpriteText3D>();
       
        protected ExitGameStateThreads exitGameStateThreads;

        TimeStamp gameStartTime = TimeStamp.Now();

        public AbsPlayState() 
            :base() 
        {
            remotePlayers = new SpottedArray<Players.RemotePlayer>();
            remotePlayersCounter = new SpottedArrayCounter<RemotePlayer>(remotePlayers);

            DssRef.state = this;
            
        }

        virtual protected void onGameStart(bool newGame)
        {
            gameStartTime = TimeStamp.Now();
            Input.Mouse.SetMenuMode(SteamWrapping.SteamActionSet.InGameControls);
        }

        public bool resourceCheckTime()
        {
            return gameStartTime.secPassed(5);
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

        protected void prePlayerInit()
        {
            XpLib.Unlock = new TechnologyUnlock(DssRef.storage.ruleset_instance.setting_techMulti);
            DssRef.storage.profileStorage.refreshProfiles();
            CityMenu.InitGame();
        }

        protected void postPlayerInit()
        {
            DssRef.ambience.gameStart();
            culling = new Culling();

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();
            ((DrawGame)draw).initMapShaders();

            foreach (var p in localPlayers)
            {
                p.hud.initMap();
            }
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
                    if (Ref.netsett.settingsHasChanged)
                    {
                        Ref.netsett.settingsHasChanged = false;
                        DssRef.storage.Save(null);
                    }
                    menuSystem.closeMenu();
                }

                return true;
            }
            return false;
        }

        public void TogglePause()
        {   
            Ref.TogglePause();
            menuSystem.gameWasPaused = Ref.isPaused;
            onSpeedChange();
        }

        public void GameSpeedClick(int toSpeed)
        {            
            Ref.SetPause(false);
            Ref.SetGameSpeed(toSpeed);
            onSpeedChange();
        }

        protected int gameSpeedValue()
        {
            int speed = Ref.isPaused ? 0 : (int)Ref.GameTimeSpeed;
            return speed;
        }

        

        public void onSpeedChange()
        {
            if (Ref.netSession.IsHostingMultiplayer)
            {
                var w = Ref.netSession.BeginWritingPacket(PacketType.PlayPause, PacketReliability.Reliable);                
                w.Write((byte)gameSpeedValue());
            }

            if (Ref.isPaused)
            {
                SoundLib.speed_down.Play(Pan.Right);
            }
            else
            {
                SoundLib.speed_up.Play(Pan.Right, -0.4f + Ref.GameTimeSpeed * 0.26f);
            }
        }

        public bool closeMenuInput_AnyPlayer()
        {
            foreach (var local in localPlayers)
            {
                if (local.gameControls.input.menuInput.openCloseInputEvent())
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
                    if (factions.sel.IsNetHosted())
                    {
                        var armiesC = factions.sel.armies.counter();
                        while (armiesC.Next())
                        {
                            armiesC.sel.asynchAiUpdate(time);
                        }
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
                        //if (factions.sel.IsNetHosted())
                        //{
                            factions.sel.asynchSleepObjectsUpdate(time);
                        //}
                    }


                    foreach (var m in DssRef.world.cities)
                    {
                        if (m.IsNetHosted)
                        {
                            m.async_sleepUpate(time);
                        }
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
                    if (factions.sel.IsNetHosted())
                    {
                        var armiesC = factions.sel.armies.counter();
                        while (armiesC.Next())
                        {
                            armiesC.sel.asyncBattleUpdate();
                        }
                    }
                }

                foreach (var m in DssRef.world.cities)
                {
                    if (m.IsNetHosted)
                    {
                        m.asyncBattleUpdate();
                    }
                }
               
            }
            return exitThreads;
        }

        protected void initPathFindingThreads()
        {
            int count = PathThreadCount();

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

        public void updateMouseVisible()
        {
            Input.Mouse.SetMenuMode(menuSystem != null && menuSystem.IsOpen());

            //Mouse.Hide();
            
        }

        public void beginExit()
        {
            Ref.music.stop(true);
            exitThreads = true;
            DssRef.ambience.gameEnd();

            if (cutScene is EndScene)
            {
                cutScene.Close();
            }

            exitGameStateThreads = new ExitGameStateThreads(exit);
        }
        void exit()
        {            
            new ExitToLobby(false);
        }

        public Players.AbsHumanPlayer GetOrCreateRemotePlayer(AbsNetworkPeer peer, int SplitScreenIndex)
        {
            Players.AbsHumanPlayer player = peer.instancePeers?[SplitScreenIndex].Tag as Players.AbsHumanPlayer;
            if (player != null)
            {
                return player;
            }

            var remotePlayerC = remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                if (remotePlayerC.sel.networkPeer == null)
                {
                    remotePlayerC.RemoveAtCurrent();
                }
                else if (remotePlayerC.sel.networkPeer.peer == peer)
                {
                    //TODO return region to AI
                    return remotePlayerC.sel;
                }
            }

            if (peer.fullId == Ref.netSession.LocalPeer().fullId)
            {
                return LocalHost();
            }

            //No found
            peer.initInstancePeers();
            foreach (var ins in peer.instancePeers)
            {
                remotePlayers.Add(new Players.RemotePlayer(ins));
            }
            return (Players.AbsHumanPlayer)peer.instancePeers[SplitScreenIndex].Tag;
        }
        virtual public void OneMinute_Update()
        { }

        public bool IsSinglePlayer_LocalAndOnline()
        { 
            return DssRef.storage.playerCount == 1 && remotePlayers.Count == 0;
        }
        public bool IsSinglePlayer_Local()
        {
            return DssRef.storage.playerCount == 1;
        }
        public bool IsLocalMultiplayer()
        {
            return localPlayers.Count >= 2;
        }
        public LocalPlayer LocalHost()
        {
            return localPlayers[0];
        }
        virtual public PlayState Game()
        {
            throw new NotImplementedException();
        }

        public override bool MayUseLowLatencyGC()
        {
            return true;
        }
        abstract public PlayStateType PlayType();

        abstract public int PathThreadCount();
    }

    enum PlayStateType
    { 
        Play,
        BattleLab,
        BattleTrials,
        MapEditor,
    }
}
