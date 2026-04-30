//#define DEBUG_CLIENT


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
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
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.MoonFall;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
//

namespace VikingEngine.DSSWars
{

    partial class PlayState : AbsPlayState
    {
        public int nextGroupId = 0;
        public bool PartyMode = false;   
        
        TechnologyManager technologyManager = new TechnologyManager();
        bool bResourceMinuteUpdate = true;
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
            
            this.host = host;
            Engine.Update.SetFrameRate(Ref.gamesett.FrameRate);

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
            new GameTime();
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

                localPlayers[i].initPlayerToPlayer(i, playerCount);
            }

            postPlayerInit();
            technologyManager.initGame(false);

            events = new Event.EventManager();

            local.onGameStart(false);

            Ref.p2p.localPeer.Tag = LocalHost();
            netPresentYourself(ReceivedPacket.Empty);
        }

        public void initGameState(bool newGame, ObjectPointerCollection pointers)
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            //new Diplomacy();
            
            new GameTime();
            HudLib.Init();

            prePlayerInit();
            //Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            initPlayers(newGame, pointers);

            //culling = new Culling();

            //factionsMap = new MapLayer_Factions();
            //overviewMap = new Map.MapLayer_Overview(factionsMap);
            //detailMap = new Map.MapLayer_Detail();
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
            onGameStart(false);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            resources.writeGameState(w);
            SaveGamestate.MainProgress++;
            events.writeGameState(w);
            SaveGamestate.MainProgress++;
            w.Write(NextArmyId);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            resources.readGameState(r, subversion);
            events.readGameState(r, subversion, pointers);

            if (subversion >= 16)
            {
                progress.readGameState(r, subversion, pointers);
            }
            if (subversion >= 105)
            { 
                NextArmyId = r.ReadInt32();
            }
        }

        void initPlayers(bool newGame, ObjectPointerCollection pointers)
        {
            
            if (DssRef.difficulty.setting_gameMode == GameModeMainType.FullStory)
            {
                new Faction(DssRef.world, FactionType.DarkLord);
                new Faction(DssRef.world, FactionType.SouthHara);
                new Faction(DssRef.world, FactionType.Barbarians);
            }
            else if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickBoss)
            {
                new Faction(DssRef.world, FactionType.DarkLord);
            }

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

                    Debug.Log("Add player " + localPlayers[i].ToString() + ", to " + localPlayers[i].faction.ToString());
                }
            }

            for (var i = 0; i < playerCount; ++i)
            {
                var pdata = localPlayers[i].playerData;
                Mouse.AddPlayer(pdata, playerCount, localPlayers[i].gameControls.input.moveCursor, localPlayers[i].gameControls.input.menuInput.cursor);

                localPlayers[i].initPlayerToPlayer(i, playerCount);
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


                    DssRef.world.diplomacy.SetRelationType(matchFactions[i], matchFactions[j], 
                        ally ? RelationType.RelationType3_Ally : RelationType.RelationTypeN4_TotalWar, null, 
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

                if (LocalHost().faction.player.IsBot())
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
        }

       

        public void initStartUnits(bool barracks = false)
        {
            if (StartupSettings.SpawnStartingArmies)
            {
                double unitCountMulti = 1;
                bool settlerGuard = false;

                switch (DssRef.storage.gameRuleset.factionStartSize)
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

        
        public override void Time_Update(float time)
        {
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
                cutScene.Time_Update(time);
                setPlayerNetState(PlayerNetState.InMenu);
                return;
            }

            if (pauseMenuUpdate())
            {
                setPlayerNetState(PlayerNetState.InMenu);
                return;
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

                    if (host)
                    {
                        var factionsC = DssRef.world.factions.counter();
                        while (factionsC.Next())
                        {
                            factionsC.sel.update();

                            if (DssRef.time.oneSecond)
                            {
                                factionsC.sel.oneSecUpdate(DssRef.time.oneMinute);
                            }
                        }
                    }
                    else
                    {
                        var factionsC = DssRef.world.factions.counter();
                        while (factionsC.Next())
                        {
                            factionsC.sel.update_client(culling.playerInDetailView);
                        }

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
                if (detailMap != null)
                {
                    detailMap.oneSecondUpdate = true;
                }
             
                if (overviewMap != null)
                {
                    overviewMap.bRefreshTimer = true;
                }
            }

            overviewMap.update();

            updatePauseInput();

            Engine.ParticleHandler.Update(time);
        }

        const float AutoSaveTimeSec = 15 * TimeExt.MinuteInSeconds;
        float LastAutoSaveTime_TotalSec = 0;


        void setPlayerNetState(PlayerNetState netState)
        {
            foreach (var local in localPlayers)
            {
                local.playerNetState = netState;
            }
        }

        protected void updatePauseInput()
        {
            if (localPlayers != null)
            {
                foreach (var local in localPlayers)
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
            if (!PlatformSettings.STEAM_DEMO)
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
            if (cutScene == null)
            {
                float seconds = DssRef.time.pullAsyncWork_Seconds();

                if (!Ref.isPaused)
                {
                    foreach (var m in DssRef.world.cities)
                    {
                        m.async_workUpdate((int)Ref.TargetGameTimeSpeed);
                        m.async_conscriptUpdate(time);
                        m.async_deliveryUpdate();
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

            if (cutScene == null)
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
            if (cutScene == null)
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
                DssRef.world.diplomacy.async_update();
                events.asyncUpdate(time);
            }
            return exitThreads;
        }

        int doubleTaskTest = 0;

        bool asyncUserUpdate(int id, float time)
        {
            //doubleTaskTest++;

            //if (doubleTaskTest > 1)
            //{
            //    throw new Exception("Double task error");
            //}

            DssRef.ambience.update_async();

            if (cutScene == null)
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
        public override PlayState Game()
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
