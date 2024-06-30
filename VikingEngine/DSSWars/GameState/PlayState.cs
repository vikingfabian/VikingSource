using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map;
using VikingEngine.Input;
using VikingEngine.ToGG.MoonFall;
//

namespace VikingEngine.DSSWars
{
    class PlayState : Engine.GameState
    {
        public WorldResources resources = new WorldResources();

        Map.MapLayer_Factions factionsMap;
        Map.MapLayer_Overview overviewMap;
        public Map.MapLayer_Detail detailMap;

        public Culling culling;
        public PathFindingPool pathFindingPool = new PathFindingPool();
        
        public int nextGroupId = 0;
        public List<Players.LocalPlayer> localPlayers;
        
        public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(64);

        bool host;
        bool isReady= false;
        public bool PartyMode = false;   
        bool exitThreads = false;
        public GameEvents events;
        public AbsCutScene cutScene=null;

        bool bResourceUpdate = true;
        public int NextArmyId = 0;
        public GameMenuSystem menuSystem;

        public PlayState(bool host, SaveStateMeta loadMeta)
            : base(true)
        {
            

            DssRef.state = this;
            Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            menuSystem = new GameMenuSystem();

            new Diplomacy();
            new Achievements();
            new GameTime();
            HudLib.Init();

            //Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            initPlayers();
            culling = new Culling();

            this.host = host;

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();

            Engine.Update.SetFrameRate(60);
            events = new GameEvents();

            if (loadMeta == null)
            {
                onGameStart(true);
            }
            else
            {
                new LoadScene(loadMeta);
            }
        }

        public void OnLoadComplete()
        {
            onGameStart(false);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            events.writeGameState(w);

            w.Write((ushort)battles.Count);
            var battlesC = battles.counter();
            while (battlesC.Next())
            {
                battlesC.sel.writeGameState(w);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            events.readGameState(r, subversion, pointers);
        }

        void initPlayers()
        {
            Players.AiPlayer.EconomyMultiplier = Difficulty.AiEconomyLevel[DssRef.difficulty.aiEconomyLevel] / 100.0; ;

            new Faction(DssRef.world, FactionType.DarkLord);
            new Faction(DssRef.world, FactionType.SouthHara);

            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                factionsCounter.sel.initDiplomacy(DssRef.world);
                if (factionsCounter.sel.factiontype == FactionType.DarkLord)
                {
                    DssRef.settings.darkLordPlayer = new Players.DarkLordPlayer(factionsCounter.sel);
                }
                else
                {
                    new Players.AiPlayer(factionsCounter.sel);
                }
            }

            int playerCount = DssRef.storage.playerCount;
            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount, !DssRef.storage.verticalScreenSplit);

            for (var i = 0; i < playerCount; ++i)
            {
                var startFaction = DssRef.world.getPlayerAvailableFaction(i == 0, localPlayers);
                var local = new Players.LocalPlayer(startFaction, i, playerCount);
                localPlayers.Add(local);
            }

            for (var i = 0; i < playerCount; ++i)
            {
                localPlayers[i].initPlayerToPlayer(i, playerCount);
            }
        }

        void onGameStart(bool newGame)
        {
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

            if (newGame)
            {
                initStartUnits();
            }

            new AsynchUpdateable_TryCatch(asynchGameObjectsUpdate, "DSS gameobjects update", 51);
            new AsynchUpdateable_TryCatch(asynchAiPlayersUpdate, "DSS ai player update", 52);
            new AsynchUpdateable_TryCatch(asynchArmyAiUpdate, "DSS army ai update", 53);
            new AsynchUpdateable_TryCatch(asynchCullingUpdate, "DSS culling update", 54);
            new AsynchUpdateable_TryCatch(asynchSleepObjectsUpdate, "DSS sleep objects update", 55);
            new AsynchUpdateable_TryCatch(asynchNearObjectsUpdate, "DSS near objects update", 56);
            new AsynchUpdateable_TryCatch(asynchMapGenerating, "DSS map gen", 57);
            new AsynchUpdateable_TryCatch(asyncUserUpdate, "DSS user update", 58);
            new AsynchUpdateable_TryCatch(asyncMapBorders, "DSS map borders update", 59);
            new AsynchUpdateable_TryCatch(asyncDiplomacyUpdate, "DSS diplomacy update", 60);
            new AsynchUpdateable_TryCatch(asyncBattlesUpdate, "DSS battles update", 62);
            new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63);

            if (StartupSettings.RunResoursesUpdate)
            {
                new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61);
            }

            if (localPlayers.Count > 1)
            {
                Ref.SetGameSpeed(DssRef.storage.multiplayerGameSpeed);
            }

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

            if (DssRef.time.oneSecond)
            { 
                DssRef.settings.OneSecondUpdate();
                detailMap.onSecondUpdate = true;
            }    
            if (DssRef.time.halfSecond)
            {
                overviewMap.HalfSecondUpdate();
            }
            detailMap.update();

            foreach (var local in localPlayers)
            {
                local.userUpdate();
                if (local.input.Menu.DownEvent)
                {
                    menuSystem.pauseMenu();
                }
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

        const float AutoSaveTimeSec = 10 * 60;
        float LastAutoSaveTime_TotalSec = 0;

        
        public void OneMinute_Update()
        { 
            bResourceUpdate = true;

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
                var battlesC = battles.counter();
                while (battlesC.Next())
                {
                    bool deleted = battlesC.sel.async_update(time);
                    if (deleted)
                    {
                        battlesC.RemoveAtCurrent();
                    }
                }
            }
            return exitThreads;
        }

        bool asyncWorkUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                foreach (var m in DssRef.world.cities)
                {
                    m.async_workUpdate();
                }
            }
            return exitThreads;
        }

        bool asyncResourcesUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                //Runs every minute to upate any resource progression: trees grow, food spoil, etc
                if (bResourceUpdate || StartupSettings.DebugResoursesSuperSpeed)
                {
                    bResourceUpdate = false;

                    resources.asyncUpdate();
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
        
        bool asynchGameObjectsUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                foreach (var m in DssRef.world.cities)
                {
                    m.asynchGameObjectsUpdate();
                }

                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    factions.sel.asynchGameObjectsUpdate(time);
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
                culling.asynch_update(time);
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
