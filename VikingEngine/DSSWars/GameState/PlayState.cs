//#define DEBUG_CLIENT


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;


using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.MoonFall;
//

namespace VikingEngine.DSSWars
{
    class PlayState : AbsPlayState
    {
        public const int PathThreadCount = 4;

        

        public int nextGroupId = 0;
        public List<Players.LocalPlayer> localPlayers;
        public SpottedArray<Players.RemotePlayer> remotePlayers = new SpottedArray<Players.RemotePlayer>();
        
        //public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(64);

        
        bool isReady= false;
        public bool PartyMode = false;   
        
        public GameEvents events;
        public Progress progress = new Progress();
        TechnologyManager technologyManager = new TechnologyManager();
        

        bool bResourceMinuteUpdate = true;
        public int NextArmyId = 0;
        public GameMenuSystem menuSystem;
        bool slowMinuteUpdate = true;   
                     
        bool netMapUpdate = false;

        public PlayState(bool host, SaveStateMeta loadMeta, System.IO.BinaryReader readWorld)
            : base()
        {
#if DEBUG_CLIENT
            host = false;
            var file = new DataStream.MemoryStreamHandler();
            var w = file.GetWriter();

            var meta = new SaveStateMeta();
            meta.netSetup();
            var saveGamestate = new SaveGamestate(meta);
            saveGamestate.writeNet(w);

            readWorld = file.GetReader();
#endif
            DssRef.state = this;
            this.host = host;
            Engine.Update.SetFrameRate(60);

            if (readWorld != null)
            {
                initGameState_client();
                new LoadScene(readWorld);
            }
            else if (loadMeta == null)
            {
                initGameState(true, null);
                onGameStart(true);
            }
            else
            {
                new LoadScene(loadMeta);
            }
        }

        public void initGameState_client()
        {
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            new Diplomacy();
            new Achievements();
            new GameTime();
            HudLib.Init();

            DssRef.world.factions.Array[0] = new Faction(DssRef.world, FactionType.Player);
            var local = new Players.LocalPlayer(DssRef.world.factions.Array[0]);
            localPlayers = new List<Players.LocalPlayer>(1);
            localPlayers.Add(local);
            local.assignPlayer(0, 1, false);

            //culling = new Culling();

            //factionsMap = new MapLayer_Factions();
            //overviewMap = new Map.MapLayer_Overview(factionsMap);
            //detailMap = new Map.MapLayer_Detail();
            baseInit();
            technologyManager.initGame(false);

            events = new GameEvents();
        }

        public void initGameState(bool newGame, ObjectPointerCollection pointers)
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            new Diplomacy();
            
            new GameTime();
            HudLib.Init();

            //Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            initPlayers(newGame, pointers);

            //culling = new Culling();

            //factionsMap = new MapLayer_Factions();
            //overviewMap = new Map.MapLayer_Overview(factionsMap);
            //detailMap = new Map.MapLayer_Detail();
            baseInit();
            technologyManager.initGame(newGame);
            
            events = new GameEvents();

             
        }

        public void OnLoadComplete()
        {
            onGameStart(false);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            resources.writeGameState(w);
            events.writeGameState(w);
            
            progress.writeGameState(w);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            resources.readGameState(r, subversion);
            events.readGameState(r, subversion, pointers);

            if (subversion >= 16)
            {
                progress.readGameState(r, subversion, pointers);
            }
        }

        void initPlayers(bool newGame, ObjectPointerCollection pointers)
        {
            //Players.AiPlayer.EconomyMultiplier = Difficulty.AiEconomyLevel[DssRef.difficulty.aiEconomyLevel] / 100.0;

            new Faction(DssRef.world, FactionType.DarkLord);
            new Faction(DssRef.world, FactionType.SouthHara);

            int playerCount = DssRef.storage.playerCount;
            //int playerIndex = 0;
            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount, !DssRef.storage.verticalScreenSplit);


            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                factionsCounter.sel.initDiplomacy(DssRef.world);
                if (factionsCounter.sel.factiontype == FactionType.DarkLord)
                {
                    DssRef.settings.darkLordPlayer = new Players.DarkLordPlayer(factionsCounter.sel);
                }
                else if (factionsCounter.sel.factiontype == FactionType.Player)
                {
                    var local = new Players.LocalPlayer(factionsCounter.sel);
                    //var local = arraylib.PullFirstMember(pointers.localPlayers);//new Players.LocalPlayer(factionsCounter.sel, 
                    
                    localPlayers.Add(local);
                }
                else
                {
                    new Players.AiPlayer(factionsCounter.sel);
                }

#if DEBUG
                if (factionsCounter.sel.player == null)
                {
                    throw new Exception();
                }
#endif
            }


            if (newGame)
            {
                for (var i = 0; i < playerCount; ++i)
                {
                    var startFaction = DssRef.world.getPlayerAvailableFaction(i == 0, localPlayers);
                    var local = new Players.LocalPlayer(startFaction);
                    local.assignPlayer(i, playerCount, newGame);
                    localPlayers.Add(local);
                }
            }
            else
            {
                for (var i = 0; i < playerCount; ++i)
                {
                    localPlayers[i].assignPlayer(i, playerCount, newGame);
                }
            }

            for (var i = 0; i < playerCount; ++i)
            {
                localPlayers[i].initPlayerToPlayer(i, playerCount);
            }


        }

        void onGameStart(bool newGame)
        {
            Ref.music.OnGameStart();

            if (host)
            {
                DssRef.difficulty.refreshSettings();
                events.onGameStart(newGame);


                var factionsCounter = DssRef.world.factions.counter();
                while (factionsCounter.Next())
                {
                    factionsCounter.sel.onGameStart(newGame);
                }

                foreach (var m in DssRef.world.cities)
                {
                    m.onGameStart(newGame);
                }

                if (newGame && !DssRef.storage.runTutorial)
                {
                    initStartUnits();
                }

                //System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
                new AsynchUpdateable_TryCatch(asynchGameObjectsUpdate, "DSS gameobjects update", 51, System.Threading.ThreadPriority.BelowNormal);
                new AsynchUpdateable_TryCatch(asynchAiPlayersUpdate, "DSS ai player update", 52, System.Threading.ThreadPriority.BelowNormal);
                new AsynchUpdateable_TryCatch(asynchArmyAiUpdate, "DSS army ai update", 53, System.Threading.ThreadPriority.BelowNormal);
                //new AsynchUpdateable_TryCatch(asynchCullingUpdate, "DSS culling update", 54, System.Threading.ThreadPriority.BelowNormal);
                new AsynchUpdateable_TryCatch(asynchSleepObjectsUpdate, "DSS sleep objects update", 55, System.Threading.ThreadPriority.BelowNormal);
                new AsynchUpdateable_TryCatch(asynchNearObjectsUpdate, "DSS near objects update", 56, System.Threading.ThreadPriority.BelowNormal);
            }

            startMapThreads();
            //new AsynchUpdateable_TryCatch(asynchMapGenerating, "DSS map gen", 57, System.Threading.ThreadPriority.Normal);
            //new AsynchUpdateable_TryCatch(asyncMapBorders, "DSS map borders update", 59, System.Threading.ThreadPriority.Lowest);

            if (host)
            {
                new AsynchUpdateable_TryCatch(asyncUserUpdate, "DSS user update", 58, System.Threading.ThreadPriority.Normal);
                
                new AsynchUpdateable_TryCatch(asyncDiplomacyUpdate, "DSS diplomacy update", 60, System.Threading.ThreadPriority.Lowest);
                new AsynchUpdateable_TryCatch(asyncBattlesUpdate, "DSS battles update", 62, System.Threading.ThreadPriority.Normal);
                new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63, System.Threading.ThreadPriority.Lowest);
                new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61, System.Threading.ThreadPriority.Lowest);
                new AsynchUpdateable_TryCatch(asyncSlowUpdate, "DSS slow update", 62, System.Threading.ThreadPriority.Lowest);
                
                new AsynchUpdateable_TryCatch(asynchHostNetUpdate, "DSS host net update", 62, System.Threading.ThreadPriority.Lowest);

                //new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63);
                //new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61);


                if (localPlayers.Count > 1)
                {
                    Ref.SetGameSpeed(DssRef.storage.multiplayerGameSpeed);
                }

                pathUpdates = new PathUpdateThread[PathThreadCount + 1];
                int startIx = 0;
                int factionLength = DssRef.world.factions.Count / PathThreadCount;
                for (int i = 0; i < PathThreadCount; i++)
                {
                    int end = startIx + factionLength;
                    if (i == PathThreadCount - 1)
                    {
                        //last
                        end = DssRef.world.factions.Count - 1;
                    }
                    pathUpdates[i] = new PathUpdateThread(i, startIx, end);
                    startIx = end + 1;
                }
                pathUpdates[PathThreadCount] = new PathUpdateThread_Player(PathThreadCount);

                
            }

            isReady = true;
        }

        void initStartUnits()
        {
            if (StartupSettings.SpawnStartingArmies)
            {
                var factionsCounter = DssRef.world.factions.counter();
                while (factionsCounter.Next())
                {
                    factionsCounter.sel.player.createStartUnits();
                }
            }
        }

        protected override void createDrawManager()
        {
            draw = new DSSWars.DrawGame();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            Sound.SoundStackManager.Update();

            if (Ref.music != null)
            {
                Ref.music.Update();
            }

            if (Ref.steam.inOverlay)
            {
                return;
            }

            if (cutScene != null)
            {
                cutScene.Time_Update(time);
                return;
            }

            if (pauseMenuUpdate())
            {
                return;
            }

            
            if (Ref.DeltaGameTimeMs > 0)
            {
                DssRef.time.update();
                
                if (isReady)
                {
                    if (host)
                    {
                        foreach (var m in DssRef.world.cities)
                        {
                            m.update();
                        }

                        var factions = DssRef.world.factions.counter();
                        while (factions.Next())
                        {
                            factions.sel.update();

                            if (DssRef.time.oneSecond)
                            {
                                factions.sel.oneSecUpdate();
                            }
                        }
                    }
                    else
                    {
                        foreach (var m in DssRef.world.cities)
                        {
                            m.update_client();
                        }
                    }
                }
                
            }
            else
            {
                if (host)
                {
                    if (isReady)
                    {
                        foreach (var m in DssRef.world.cities)
                        {
                            m.PauseUpdate();
                        }

                        var factions = DssRef.world.factions.counter();
                        while (factions.Next())
                        {
                            factions.sel.PauseUpdate();
                        }
                    }
                }
            }
            
            if (DssRef.time.halfSecond)
            {
                overviewMap.HalfSecondUpdate();
            }
            if (subTileReloadTimer.Update())
            {
                detailMap.oneSecondUpdate = true;
                overviewMap.bRefreshTimer = true;
            }

            detailMap.update();

            if (localPlayers != null)
            {
                foreach (var local in localPlayers)
                {
                    local.userUpdate();
                    if (local.input.Menu.DownEvent)
                    {
                        menuSystem.pauseMenu();
                    }
                    if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.B) && Keyboard.Ctrl)
                    {
                        menuSystem.debugMenu();
                    }
                }
            }

            if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape) && !menuSystem.Open)
            {
                menuSystem.pauseMenu();
            }

            Engine.ParticleHandler.Update(time);

        }

        bool pauseMenuUpdate()
        {
            if (menuSystem.Open)
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

        const float AutoSaveTimeSec = 15 * TimeExt.MinuteInSeconds;
        float LastAutoSaveTime_TotalSec = 0;

        
        public void OneMinute_Update()
        { 
            bResourceMinuteUpdate = true;

            slowMinuteUpdate = true;

            if (host && DssRef.storage.autoSave && 
                Ref.TotalTimeSec > LastAutoSaveTime_TotalSec + AutoSaveTimeSec)
            {
                if (cutScene == null)
                {
                    new SaveScene(true);
                }
                LastAutoSaveTime_TotalSec = Ref.TotalTimeSec;
            }            
        }

        

        public override void OnDestroy()
        {
            exitThreads = true;
            base.OnDestroy();
        }

        public void pauseAction()
        {
            Ref.SetPause(!Ref.isPaused);
        }

        

        public void exit()
        {
            Ref.music.stop(true);
            exitThreads = true;
            
            new ExitGamePlay();
        }

        
        public override void NetEvent_ConnectionLost(string reason)
        {
            base.NetEvent_ConnectionLost(reason);
            if (!this.host)
            {
                new GameState.ExitGamePlay();
            }
        }
        
        void shareAllHostedObjects(Network.AbsNetworkPeer sender)
        {
            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                factionsCounter.sel.shareAllHostedObjects(sender);
            }
        }

        bool asyncBattlesUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                //var battlesC = battles.counter();
                //while (battlesC.Next())
                //{
                //    bool deleted = battlesC.sel.async_update(time);
                //    if (deleted)
                //    {
                //        battlesC.RemoveAtCurrent();
                //    }
                //}
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

        bool asyncWorkUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                float seconds = DssRef.time.pullAsyncWork_Seconds();

                if (!Ref.isPaused)
                {
                    foreach (var m in DssRef.world.cities)
                    {
                        m.async_workUpdate();
                        m.async_conscriptUpdate(time);
                        m.async_deliveryUpdate();
                    }

                    var factions = DssRef.world.factions.counter();
                    while (factions.Next())
                    {
                        var armiesC = factions.sel.armies.counter();
                        while (armiesC.Next())
                        {
                            armiesC.sel.async_workUpdate(seconds);
                        }
                    }
                }
            }
            return exitThreads;
        }

        bool asyncResourcesUpdate(int id, float time)
        {
            //This thread is the only thay may edit subtiles
            if (cutScene == null)
            {
                resources.asyncEditTiles();
                //Runs every minute to upate any resource progression: trees grow, food spoil, etc
                if (bResourceMinuteUpdate || StartupSettings.DebugResoursesSuperSpeed)
                {
                    bResourceMinuteUpdate = false;

                    resources.asyncGrowUpdate();
                }
            }
            return exitThreads;
        }

        bool asyncSlowUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                if (slowMinuteUpdate)
                { 
                    slowMinuteUpdate = false;
                    technologyManager.asyncOneMinuteUpdate(true);
                }

            }
            return exitThreads;
        }

        bool asyncDiplomacyUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                DssRef.diplomacy.async_update();
                events.asyncUpdate(time);
            }
            return exitThreads;
        }

        int doubleTaskTest = 0;

        bool asyncUserUpdate(int id, float time)
        {
            doubleTaskTest++;

            if (doubleTaskTest > 1)
            {
                throw new Exception("Double task error");
            }

            if (cutScene == null)
            {
                foreach (var local in localPlayers)
                {
                    local.asyncUserUpdate();
                }
            }

            doubleTaskTest--;

            return exitThreads;

        }

        

        bool asynchHostNetUpdate(int id, float time)
        {
            if (remotePlayers.Count > 0)
            {
                if (!sendMap())
                {
                    //TODO, rotate user update
                    //Map sent, start updating units
                    var remoteC = remotePlayers.counter();
                    while (remoteC.Next())
                    {
                        remoteC.sel.Net_HostObjectsUpdate_async();
                    }
                }

                bool sendMap()
                {
                    var remoteC = remotePlayers.counter();
                    while (remoteC.Next())
                    {
                        if (remoteC.sel.Net_HostMapUpdate_async())
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            return exitThreads;
        }

        

       

        int asynchGameObjectsMinutes = 0;
        bool asynchGameObjectsUpdate(int id, float time)
        {
            float seconds = DssRef.time.pullAsyncGameObjects_Seconds();

            if (cutScene == null)
            {
                bool minute = DssRef.time.pullMinute(ref asynchGameObjectsMinutes);

                foreach (var m in DssRef.world.cities)
                {
                    m.asynchGameObjectsUpdate(minute);
                }

                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    factions.sel.asynchGameObjectsUpdate(time, seconds, minute);
                }

            }
            return exitThreads;
        }

        bool asynchAiPlayersUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    factions.sel.asynchAiPlayersUpdate(time);
                }
            }
            
            return exitThreads;
        }

        bool asynchArmyAiUpdate(int id, float time)
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

       

        //bool asynchCullingUpdate(int id, float time)
        //{
        //    if (cutScene == null)
        //    {
        //        //culling.asynch_update(time);
        //    }
        //    return exitThreads;
        //}

        bool asynchSleepObjectsUpdate(int id, float time)
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

        bool asynchNearObjectsUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                DssRef.world.unitCollAreaGrid.asynchUpdate();

                foreach (var m in DssRef.world.cities)
                {
                    m.asynchNearObjectsUpdate();
                }

                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    var armiesC = factions.sel.armies.counter();
                    while (armiesC.Next())
                    {
                        armiesC.sel.asyncNearObjectsUpdate();
                    }
                }
            }
            return exitThreads;
        }

        public bool IsSinglePlayer()
        { 
            return localPlayers.Count == 1;
        }

        public bool IsLocalMultiplayer()
        {
            return localPlayers.Count >= 2;
        }

        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case PacketType.DssJoined_WantWorld:
                    var w = Ref.netSession.BeginWritingPacket(PacketType.DssSendWorld, SendPacketTo.OneSpecific, packet.sender.fullId, PacketReliability.Reliable, null);
                    var meta = new SaveStateMeta();
                    meta.netSetup();
                    var saveGamestate = new SaveGamestate(meta);
                    saveGamestate.writeNet(w);

                    //new SteamLargePacketWriter(file, SendPacketTo.OneSpecific, packet.sender.fullId, PacketType.DssSendWorld).begin();
                    break;

                case PacketType.DssPlayerStatus:
                    GetRemotePlayer(packet).Net_readStatus(packet.r);
                    break;

                case PacketType.DssWorldTiles:                    
                    DssRef.world.readNet_Tile(packet.r);//l 32 * 4 * 4
                    overviewMap.bRefreshDataRecieved = true;
                    break;

                case PacketType.DssWorldSubTiles:
                    DssRef.world.readNet_SubTile(packet.r);//l 522
                    break;

                case PacketType.DssFactions:
                    DssRef.world.readNet_Factions(packet.r);
                    break;

                case PacketType.DssCities:
                    DssRef.world.readNet_Cities(packet.r);
                    break;
            }
        }

        public ConcurrentStack<Graphics.VoxelModelInstance> modelPool(bool detail)
        { 
            return detail? voxelModelInstancesPool_detail : voxelModelInstancesPool_overview;
        }

        public Players.RemotePlayer GetRemotePlayer(ReceivedPacket packet)
        {
            return (Players.RemotePlayer)packet.sender.instancePeers[packet.senderLocalIndex].Tag;
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

        public override void NetUpdate()
        {
            foreach (var player in localPlayers)
            {
                player.NetUpdate();
            }
        }

        public override void NetEvent_PeerJoined(AbsNetworkPeer peer)
        {
            base.NetEvent_PeerJoined(peer);
            GetOrCreateRemotePlayer(peer, 0);
            //peer.initInstancePeers();
            //foreach (var ins in peer.instancePeers)
            //{
            //    remotePlayers.Add(new Players.RemotePlayer(ins));
            //}
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer peer)
        {
            var remotePlayerC = remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                if (remotePlayerC.sel.networkPeer.peer == peer)
                {
                    //TODO return region to AI
                    remotePlayerC.RemoveAtCurrent();
                }
            }
        }
    }

    struct AsynchUpdateArgs
    {
        public float time;
        public int weeks;

        public AsynchUpdateArgs(float time, int weeks)
        {
            this.time = time;
            this.weeks = weeks;
        }
    }
}
