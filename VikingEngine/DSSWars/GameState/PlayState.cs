//#define DEBUG_CLIENT


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.EngineSpace;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.ToggEngine;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
//

namespace VikingEngine.DSSWars
{

    partial class PlayState : AbsPlayState
    {
        public int nextGroupId = 0;
        //public bool PartyMode = false;   
        public bool casualControls;

        TechnologyManager technologyManager = new TechnologyManager();
        bool bResourceMinuteUpdate = true;
        bool slowMinuteUpdate = true;
        bool netMapUpdate = false;

        public Dictionary<int, PlayerMapHistory> previousRemotePlayers = new Dictionary<int, PlayerMapHistory>();

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
            
            this.host = host;
            Engine.Update.SetFrameRate(Ref.gamesett.FrameRate);

            if (host)
            {
                Ref.netsett.remoteHostSettings = new NetSharedHostSettings();
                DssRef.storage.ruleset_instance = DssRef.storage.ruleset;
                DssRef.storage.ruleset_instance.refreshSettings();
                Ref.steamlobby?.refreshMetaData();
            }

            if (readWorld != null)
            {   
                new LoadScene(readWorld);

                initGameState_client();
            }
            else if (loadMeta == null)
            {
                initGameState(true, null);
                onGameStart(true);
            }
            else
            {
                new LoadScene(loadMeta);
                Ref.netsett.SendStats(false);
            }

            if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
            {
                BattleLabStorage.Singleton = new BattleLabStorage();
            }

            DssRef.achieve.UnlockAchievement(AchievementIndex.first_game);
        }

        public void initGameState_client()
        {
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            //new Diplomacy();
            new Achievements();
            new Event.GameTime();
            HudLib.Init();

            prePlayerInit();

            var playerFaction = new Faction(DssRef.world, FactionType.Player);
            DssRef.world.factions.Array[0] = playerFaction;
            //playerFaction.initClient(DssRef.world);
            var local = new Players.LocalPlayer(playerFaction, false);
            localPlayers = new List<Players.LocalPlayer>(1);
            localPlayers.Add(local);
            local.assignPlayer(0, 1, false);

            int playerCount = 1;
            for (var i = 0; i < playerCount; ++i)
            {
                var pdata = localPlayers[i].playerData;
                Mouse.AddPlayer(pdata, playerCount, localPlayers[i].gameControls.input.moveCursor, localPlayers[i].gameControls.input.menuInput.cursor);

                //localPlayers[i].initPlayerToPlayer(i, playerCount);
            }

            postPlayerInit();
            technologyManager.initGame(false);

            events = new Event.EventManager();

            local.onGameStart(false);

            Ref.p2p.localPeer.Tag = LocalHost();
            //netPresentYourself(ReceivedPacket.Empty);
        }

        public void initGameState(bool newGame, ObjectPointerCollection pointers)
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.worldId.seed);
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            //new Diplomacy();
            
            new Event.GameTime();
            HudLib.Init();

            prePlayerInit();
            //Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            initPlayers(newGame, pointers);

            
            postPlayerInit();
            technologyManager.initGame(newGame);

            if (PlatformSettings.STEAM_DEMO &&
               (DssRef.storage.runTutorial == false || LocalHost().profile.casualControls))
            {
                events = new Event.GameEventsDemo();
            }
            else
            {
                events = new Event.EventManager();
            }

        }

        public override void OnLoadComplete()
        {
            //if (!host)
            //{ 
            //    DssRef.storage.profileStorage.Selected().casualControls &= Ref.netsett.remoteHostSettings.hostSettings.allowCasualControls;
            //}

            onGameStart(false);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            resources.writeGameState(w);
            SaveGamestate.MainProgress++;
            events.writeGameState(w);
            SaveGamestate.MainProgress++;
            w.Write(NextArmyId);
            SaveGamestate.MainProgress++;

            var remoteStores = previousRemotePlayers.Values.ToList();
            remotePlayersCounter.Reset();
            while (remotePlayersCounter.Next())
            {
                remoteStores.Add( remotePlayersCounter.sel.GetMapHistory());
            }

            w.Write((ushort)remoteStores.Count);
            foreach (var m in remoteStores)
            { 
                m.write(w);
            }

            w.Write(casualControls);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            resources.readGameState(r, subversion);
            events.readGameState(r, subversion, pointers);

            //if (subversion >= 16)
            //{
            //    progress.readGameState(r, subversion, pointers);
            //}
            if (subversion >= 105)
            {
                NextArmyId = r.ReadInt32();
            }
            if (subversion >= 115)
            {
                int remoteStoresCount = r.ReadUInt16();
                for (int i = 0; i < remoteStoresCount; i++)
                {
                    PlayerMapHistory mapHistory = new PlayerMapHistory();
                    mapHistory.read(r, subversion);
                    previousRemotePlayers.TryAdd(mapHistory.GetHashCode(), mapHistory);
                }
            }
            if (subversion >= 132)
            {
                casualControls |= r.ReadBoolean();
            }
        }

        void initPlayers(bool newGame, ObjectPointerCollection pointers)
        {
            
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
            {
                new Faction(DssRef.world, FactionType.DarkLord);
                new Faction(DssRef.world, FactionType.SouthHara);                
            }
            else if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickBoss)
            {
                new Faction(DssRef.world, FactionType.DarkLord);
            }

            new Faction(DssRef.world, FactionType.Barbarians);

            int playerCount = DssRef.storage.playerCount;


            Stack<Faction> spectatorFaction = new Stack<Faction>(playerCount);
            if (newGame)
            {
                if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
                {
                    for (int i = 0; i < playerCount; ++i)
                    {
                        spectatorFaction.Push(new Faction(DssRef.world, FactionType.Player));
                    }
                }
            }

            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount);


            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                switch (factionsCounter.sel.factiontype)
                {
                    case FactionType.DarkLord:
                        {
                            new Players.DarkLordPlayer(factionsCounter.sel, newGame);
                        }
                        break;
                    case FactionType.Player:
                        {
                            var local = new Players.LocalPlayer(factionsCounter.sel, newGame);
                            
                            localPlayers.Add(local);
                        }
                        break;
                    default:
                        {
                            new Players.AiPlayer(factionsCounter.sel, newGame);
                        }
                        break;
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
                    Players.LocalPlayer local;
                    Faction startFaction;
                    if (DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
                    {
                        startFaction = spectatorFaction.Pop();
                        local = startFaction.player.GetLocalPlayer();
                    }
                    else
                    {
                        startFaction = DssRef.world.getPlayerAvailableFaction2(localPlayers, i == 0, false);
                        local = new Players.LocalPlayer(startFaction, newGame);
                        localPlayers.Add(local);
                    }
                    
                    local.assignPlayer(i, playerCount, newGame);                  
                    
                }
            }
            else
            {                

                for (var i = 0; i < playerCount; ++i)
                {
                    if (localPlayers.Count <= i)
                    {
                        //Drop in support
                        Faction startFaction = DssRef.world.getPlayerAvailableFaction2(localPlayers, i == 0, false);
                        Players.LocalPlayer local = new Players.LocalPlayer(startFaction, newGame) { isDropInPlayer = true };
                        
                        localPlayers.Add(local);
                    }

                    localPlayers[i].assignPlayer(i, playerCount, newGame);

                    //Debug.Log("Add player " + localPlayers[i].ToString() + ", to " + localPlayers[i].pfaction.GetFaction().ToString());
                }
            }

            for (var i = 0; i < playerCount; ++i)
            {
                var pdata = localPlayers[i].playerData;
                Mouse.AddPlayer(pdata, playerCount, localPlayers[i].gameControls.input.moveCursor, localPlayers[i].gameControls.input.menuInput.cursor);
                
                casualControls |= localPlayers[i].profile.casualControls;
                
            }

            if (newGame && DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
            {
                initQuickMatch();
            }
        }

        void initQuickMatch()
        {
            List<Faction> matchFactions = StoryEvent_QuickMatch.Factions();
                        
            int team1Count = (int)Math.Ceiling(matchFactions.Count / 2.0);
            for (var i = 0; i < matchFactions.Count; ++i)
            {
                for (var j = i + 1; j < matchFactions.Count; ++j)
                {
                    bool ally = DssRef.difficulty.setting_QuickMatch_TwoTeams && (i < team1Count == j < team1Count);


                    DssRef.world.diplomacy.SetRelationType(matchFactions[i].pfaction, matchFactions[j].pfaction, PFaction.Empty, 
                        ally ? RelationType.RelationType3_Ally : RelationType.RelationTypeN5_TotalWar, null, 
                        SpeakTerms.SpeakTermsN2_None);

                    //var relation = DssRef.world.diplomacy.GetOrCreateRelation(matchFactions[i], matchFactions[j]);

                    //relation.Relation = ally ? RelationType.RelationType3_Ally : RelationType.RelationTypeN4_TotalWar;
                    //relation.SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                }
            }
            
        }

        override protected void onGameStart(bool newGame)
        {
            base.onGameStart(newGame);

            updateMouseVisible();
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

                if (LocalHost().pfaction.GetFaction().player.IsBot())
                {
                    LocalHost().baseOnGameStart();
                }

                foreach (var m in DssRef.world.cities)
                {
                    m.onGameStart(newGame);
                }

                if (newGame && (DssRef.storage.runTutorial == false || DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator))
                {
                    initStartUnits();
                }
                
                new AsynchUpdateable_TryCatch(asynchAiPlayersUpdate, "DSS ai player update", 52, System.Threading.ThreadPriority.BelowNormal);                
            }

            new AsynchUpdateable_TryCatch(asynchGameObjectsUpdate, "DSS gameobjects update", 51, System.Threading.ThreadPriority.BelowNormal);

            new AsynchUpdateable_TryCatch(asynchArmyAiUpdate, "DSS army ai update", 53, System.Threading.ThreadPriority.BelowNormal);

            new AsynchUpdateable_TryCatch(asynchSleepObjectsUpdate, "DSS sleep objects update", 55, System.Threading.ThreadPriority.BelowNormal);

            new AsynchUpdateable_TryCatch(asynchNearObjectsUpdate, "DSS near objects update", 56, System.Threading.ThreadPriority.BelowNormal);

            startMapThreads();

            new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63, System.Threading.ThreadPriority.Lowest);

            new AsynchUpdateable_TryCatch(asyncUserUpdate, "DSS user update", 58, System.Threading.ThreadPriority.Normal);

            new AsynchUpdateable_TryCatch(asyncDiplomacyUpdate, "DSS diplomacy update", 60, System.Threading.ThreadPriority.Lowest);//only truce timers

            new AsynchUpdateable_TryCatch(asyncBattlesUpdate, "DSS battles update", 62, System.Threading.ThreadPriority.Normal);

            new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61, System.Threading.ThreadPriority.Lowest);

            new AsynchUpdateable_TryCatch(asyncSlowUpdate, "DSS slow update", 62, System.Threading.ThreadPriority.Lowest);

            if (host)
            { 
                new AsynchUpdateable_TryCatch(asynchHostNetUpdate, "DSS host net update", 62, System.Threading.ThreadPriority.Lowest);

                if (localPlayers.Count > 1)
                {
                    Ref.SetGameSpeed(DssRef.storage.multiplayerGameSpeed);
                }
            }
            else
            {
                new AsynchUpdateable_TryCatch(asynchClientNetUpdate, "DSS client net update", 65, System.Threading.ThreadPriority.Lowest);
            }

            initPathFindingThreads();

            isReady = true;
            LastAutoSaveTime_TotalSec = Ref.TotalTimeSec;
            events.onGameStarted();

            if ((newGame && LocalHost().IntutorialMode()) == false)
            {
                menuSystem.pauseMenu();
            }
        }

        public void initStartUnits(bool barracks = false)
        {
            if (StartupSettings.SpawnStartingArmies)
            {
                startingArmySizes(out double unitCountMulti, out bool settlerGuard);

                var factionsCounter = DssRef.world.factions.counter();
                while (factionsCounter.Next())
                {
                    if (barracks)
                    {
                        factionsCounter.sel.player.createStartupBarracks();
                    }
                    factionsCounter.sel.player.createStartUnits(unitCountMulti, settlerGuard);
                }
            }
        }

        public void startingArmySizes(out double unitCountMulti, out bool settlerGuard)
        {
            unitCountMulti = 1;
            settlerGuard = false;
            switch (DssRef.storage.ruleset.factionStartSize)
            {
                case FactionStartSize.OneCity:
                    unitCountMulti = 0.4;
                    settlerGuard = DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch;
                    break;
                case FactionStartSize.Settler:
                    unitCountMulti = 0.25;
                    settlerGuard = true;
                    break;

            }
        }


        public override void Time_Update(float time)
        {
            bool bUserMapUpdate = true;

            base.Time_Update(time);
            Sound.SoundStackManager.Update();

            if (Ref.music != null)
            {
                Ref.music.Update();
            }            

            if (Ref.steam.InOffGameOverlay())
            {
                if (!menuSystem.IsOpen())
                {
                    menuSystem.pauseMenu();
                }
                setPlayerNetState(PlayerNetState.InMenu);
                return;
            }

            if (cutScene != null)
            {
                setPlayerNetState(cutScene.NetState());
                cutScene.Time_Update(time);
                
                return;
            }

            if (pauseMenuUpdate(out bool blockInput))
            {
                setPlayerNetState(PlayerNetState.InMenu);
                bUserMapUpdate = false;
                if (Ref.isPaused || blockInput)
                {
                    return;
                }
            }

            if (exitGameStateThreads != null)
            {
                new ExitScene(exitGameStateThreads);
                setPlayerNetState(PlayerNetState.InMenu);
                return;
            }
            
            if (Ref.DeltaGameTimeMs > 0)
            {
                DssRef.time.update();
                
                if (isReady)
                {
                    foreach (var m in DssRef.world.cities)
                    {
                        m.update();
                    }

                    var factionsC = DssRef.world.factions.counter();
                    while (factionsC.Next())
                    {
                        if (factionsC.sel.IsNetHosted())
                        {
                            factionsC.sel.update();

                            if (DssRef.time.oneSecond)
                            {
                                factionsC.sel.oneSecUpdate(DssRef.time.oneMinute);
                            }
                        }
                        else if (factionsC.sel.pfaction.TryGetPlayer(out _))
                        {
                            factionsC.sel.update_client(culling.playerInDetailView);

                            if (DssRef.time.oneSecond)
                            {
                                factionsC.sel.client_oneSecUpdate(DssRef.time.oneMinute);
                            }

                        }
                    }
                }
                
            }
            else //PAUSE UPDATE
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

            switch (processTime.update())
            {
                case ProcessEvent.OverviewMap:
                    overviewMap.HalfSecondUpdate();
                    break;
                case ProcessEvent.SubTileReload:
                    if (detailMap != null)
                    {
                        detailMap.oneSecondUpdate = true;
                    }

                    if (overviewMap != null)
                    {
                        overviewMap.bRefreshTimer = true;
                    }
                    break;
            }
            
            overviewMap.update();

            //if (bUserUpdate)
            //{
                updateUserInput(bUserMapUpdate);
            //}
            //else
            //{ 
                
            //}

            Engine.ParticleHandler.Update(time);
        }

        const float AutoSaveTimeSec = 15 * TimeExt.MinuteInSeconds;
        float LastAutoSaveTime_TotalSec = 0;


        void setPlayerNetState(PlayerNetState netState)
        {
            if (localPlayers != null)
            {
                foreach (var local in localPlayers)
                {
                    local.playerNetState = netState;
                }
            }
        }

        protected void updateUserInput(bool bUserMapUpdate)
        {
            if (localPlayers != null)
            {
                foreach (var local in localPlayers)
                {
                    if (bUserMapUpdate)
                    {
                        local.userUpdate(true);

                        if (local.gameControls.input.Menu.DownEvent)
                        {
                            menuSystem.pauseMenu();
                        }

                        if (local.playerData.LostController)
                        {
                            local.playerData.IgnoreLostController = true;
                            menuSystem.controllerDisconnectMenu(); //todo lost menu
                        }
                    }
                    else
                    {
                        local.uiUpdateOnly();
                    }

                    remotePlayersCounter.Reset();
                    while (remotePlayersCounter.Next())
                    {
                        remotePlayersCounter.sel.UpdateClient(local);
                    }

                    local.gameControls.map.camera.prevLookTarget = local.gameControls.map.camera.LookTarget;
                }
            }

           

            if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape) && !menuSystem.IsOpen())
            {
                menuSystem.pauseMenu();
            }
        }

        public void speedUpGrowing()
        {
            if (DssRef.time.oneSecond)
            {
                bResourceMinuteUpdate = true;
            }
        }

        override public void OneMinute_Update()
        { 
            bResourceMinuteUpdate = true;

            slowMinuteUpdate = true;

            if (host && DssRef.storage.autoSave && 
                DssRef.storage.runTutorial == false &&
                Ref.TotalTimeSec > LastAutoSaveTime_TotalSec + AutoSaveTimeSec)
            {
                AutoSave();
            }            
        }

        public void AutoSave()
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                if (Ref.netSession.IsHost)
                {
                    RequestClientGamestates(true);
                }
            }
            else if (!PlatformSettings.STEAM_DEMO)
            {
                if (cutScene == null)
                {
                    new SaveScene(true);
                }
            }
            LastAutoSaveTime_TotalSec = Ref.TotalTimeSec;
        }
                

        public override void OnDestroy()
        {
            exitThreads = true;
            
            base.OnDestroy();
        }

        override public bool UpdateReady()
        {
            return cutScene == null && (host || factionHandOverComplete);
        }


        //void shareAllHostedObjects(Network.AbsNetworkPeer sender)
        //{
        //    var factionsCounter = DssRef.world.factions.counter();
        //    while (factionsCounter.Next())
        //    {
        //        factionsCounter.sel.shareAllHostedObjects(sender);
        //    }
        //}



        bool asyncWorkUpdate(int id, float time)
        {
            if (UpdateReady())
            {
                float seconds = DssRef.time.pullAsyncWork_Seconds();

                if (!Ref.isPaused)
                {
                    foreach (var m in DssRef.world.cities)
                    {
                        if (m.IsNetHosted)
                        {
                            m.async_workUpdate((int)Ref.TargetGameTimeSpeed);
                            m.async_conscriptUpdate(time);
                            m.async_deliveryUpdate();
                        }
                        else
                        {
                            if (arraylib.HasMembers(m.workerUnits))
                            {
                                m.async_workUpdate((int)Ref.TargetGameTimeSpeed);
                            }
                        }
                    }

                    var factions = DssRef.world.factions.counter();
                    while (factions.Next())
                    {
                        var armiesC = factions.sel.armies.counter();
                        while (armiesC.Next())
                        {
                            armiesC.sel.async_workUpdate(factions.sel, seconds);
                        }
                    }
                }
            }
            return exitThreads;
        }

        int asynchGameObjectsMinutes = 0;
        protected bool asynchGameObjectsUpdate(int id, float time)
        {
            float seconds = DssRef.time.pullAsyncGameObjects_Seconds();

            if (UpdateReady())
            {
                bool minute = DssRef.time.pullMinute(ref asynchGameObjectsMinutes);

                foreach (var m in DssRef.world.cities)
                {
                    //if (m.IsNetHosted)
                    //{
                        m.asynchGameObjectsUpdate(minute);
                    //}
                }

                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    //if (factions.sel.IsNetHosted())
                    //{
                    if (factions.sel.player != null)
                    {
                        factions.sel.asynchGameObjectsUpdate(time, seconds, minute);
                    }
                    //}
                }

            }
            return exitThreads;
        }

        protected bool asynchNearObjectsUpdate(int id, float time)
        {
            if (UpdateReady())
            {
                DssRef.world.unitCollAreaGrid.asynchUpdate();

                foreach (var m in DssRef.world.cities)
                {
                    m.asyncNearObjectsUpdate();
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

        bool asyncResourcesUpdate(int id, float time)
        {
            //This thread is the only thay may edit subtiles
            if (UpdateReady())
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
            if (UpdateReady())
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
            if (UpdateReady())
            {
                DssRef.world.diplomacy.async_update();
                events.asyncUpdate(time);
                
            }
            return exitThreads;
        }

        int doubleTaskTest = 0;

        bool asyncUserUpdate(int id, float time)
        {
            DssRef.ambience.update_async();

            if (UpdateReady())
            {
                foreach (var local in localPlayers)
                {
                    local.asyncUserUpdate();
                }
            }

            DssRef.achieve.asyncUpdate();

            return exitThreads;

        }


        public override int PathThreadCount()
        {
            return 4;
        }
        public override PlayState playstate()
        {
            return this;
        }

        public override PlayStateType PlayType()
        {
            return PlayStateType.Play;
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
