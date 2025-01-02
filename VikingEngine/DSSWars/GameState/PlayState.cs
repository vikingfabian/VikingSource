using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
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
using VikingEngine.Input;
using VikingEngine.ToGG.MoonFall;
//

namespace VikingEngine.DSSWars
{
    class PlayState : AbsDssState
    {
        public const int PathThreadCount = 4;
        public WorldResources resources = new WorldResources();

        Map.MapLayer_Factions factionsMap;
        Map.MapLayer_Overview overviewMap;
        public Map.MapLayer_Detail detailMap;

        public Culling culling;
        public PathUpdateThread[] pathUpdates; 
        //public PathFindingPool pathFindingPool = new PathFindingPool();
        //public DetailPathFindingPool detailPathFindingPool = new DetailPathFindingPool();

        public int nextGroupId = 0;
        public List<Players.LocalPlayer> localPlayers;
        
        //public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(64);

        bool host;
        bool isReady= false;
        public bool PartyMode = false;   
        public bool exitThreads = false;
        public GameEvents events;
        public Progress progress = new Progress();
        TechnologyManager technologyManager = new TechnologyManager();
        public AbsCutScene cutScene=null;

        bool bResourceMinuteUpdate = true;
        public int NextArmyId = 0;
        public GameMenuSystem menuSystem;
        bool slowMinuteUpdate = true;   
        Timer.Basic subTileReloadTimer = new Timer.Basic(1000, true);                

        public PlayState(bool host, SaveStateMeta loadMeta)
            : base()
        {
            
            DssRef.state = this;
            this.host = host;
            Engine.Update.SetFrameRate(60);

            //int seed;
            //if (loadMeta == null)
            //{
            //    seed = DssRef.world.metaData.seed;
            //}
            //else
            //{
            //    seed = loadMeta.worldmeta.seed;
            //}


            if (loadMeta == null)
            {
                initGameState(true, null);
                onGameStart(true);
            }
            else
            {
                new LoadScene(loadMeta);
            }
        }

        public void initGameState(bool newGame, ObjectPointerCollection pointers)
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            new Diplomacy();
            new Achievements();
            new GameTime();
            HudLib.Init();

            //Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            initPlayers(newGame, pointers);

            culling = new Culling();

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();
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
            DssRef.difficulty.refreshSettings();
            events.onGameStart(newGame);
            Ref.music.OnGameStart();

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
            new AsynchUpdateable_TryCatch(asynchCullingUpdate, "DSS culling update", 54, System.Threading.ThreadPriority.BelowNormal);
            new AsynchUpdateable_TryCatch(asynchSleepObjectsUpdate, "DSS sleep objects update", 55, System.Threading.ThreadPriority.BelowNormal);
            new AsynchUpdateable_TryCatch(asynchNearObjectsUpdate, "DSS near objects update", 56, System.Threading.ThreadPriority.BelowNormal);
            new AsynchUpdateable_TryCatch(asynchMapGenerating, "DSS map gen", 57, System.Threading.ThreadPriority.Normal);
            new AsynchUpdateable_TryCatch(asyncUserUpdate, "DSS user update", 58, System.Threading.ThreadPriority.Normal);
            new AsynchUpdateable_TryCatch(asyncMapBorders, "DSS map borders update", 59, System.Threading.ThreadPriority.Lowest);
            new AsynchUpdateable_TryCatch(asyncDiplomacyUpdate, "DSS diplomacy update", 60, System.Threading.ThreadPriority.Lowest);
            new AsynchUpdateable_TryCatch(asyncBattlesUpdate, "DSS battles update", 62, System.Threading.ThreadPriority.Normal);
            new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63, System.Threading.ThreadPriority.Lowest);
            new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61, System.Threading.ThreadPriority.Lowest);
            new AsynchUpdateable_TryCatch(asyncSlowUpdate, "DSS slow update", 62, System.Threading.ThreadPriority.Lowest);

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
                if (i == PathThreadCount -1)
                {
                    //last
                    end = DssRef.world.factions.Count -1;
                }
                pathUpdates[i] = new PathUpdateThread(i, startIx, end);
                startIx = end + 1;
            }
            pathUpdates[PathThreadCount] = new PathUpdateThread_Player(PathThreadCount);

            isReady = host;
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
            }
            else 
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

            //if (DssRef.time.oneSecond)
            //{ 
            //    DssRef.settings.OneSecondUpdate();                
            //}    
            if (DssRef.time.halfSecond)
            {
                overviewMap.HalfSecondUpdate();
            }
            if (subTileReloadTimer.Update())
            {
                detailMap.onSecondUpdate = true;
            }

            detailMap.update();

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

            if (DssRef.storage.autoSave && 
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

        bool asynchMapGenerating(int id, float time)
        {
            if (cutScene == null)
            {
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

       

        bool asynchCullingUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                //culling.asynch_update(time);
            }
            return exitThreads;
        }

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
