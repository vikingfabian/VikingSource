using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.XP;
using VikingEngine.Input;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class BattleLabPlayState : AbsPlayState
    {
        bool isReady = false;

        public BattleLabPlayState()
            : base()
        {
            initGameState();
            onGameStart();
        }

        public void initGameState()
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.seed);
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            new Diplomacy();

            new GameTime();
            HudLib.Init();

            initPlayers();
            baseInit();
        }

        void initPlayers()
        {
            new Faction(DssRef.world, FactionType.DarkLord);


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

            int playerCount = 1;

            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount, !DssRef.storage.verticalScreenSplit);
            for (var i = 0; i < playerCount; ++i)
            {
                var startFaction = DssRef.world.getPlayerAvailableFaction(i == 0, localPlayers);
                var local = new BattleLabPlayer(startFaction);
                local.assignPlayer(i, playerCount, true);
                localPlayers.Add(local);
            }
        }

        void onGameStart()
        {
            Ref.music.OnGameStart();

            if (host)
            {
                var factionsCounter = DssRef.world.factions.counter();
                while (factionsCounter.Next())
                {
                    factionsCounter.sel.onGameStart(true);
                }

                //System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
                new AsynchUpdateable_TryCatch(asynchGameObjectsUpdate, "DSS gameobjects update", 51, System.Threading.ThreadPriority.BelowNormal);
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
                
                new AsynchUpdateable_TryCatch(asyncBattlesUpdate, "DSS battles update", 62, System.Threading.ThreadPriority.Normal);
               
                //new AsynchUpdateable_TryCatch(asyncWorkUpdate, "DSS work update", 63);
                //new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61);


                if (localPlayers.Count > 1)
                {
                    Ref.SetGameSpeed(DssRef.storage.multiplayerGameSpeed);
                }

                initPathFindingThreads(1);
            }

            isReady = true;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            updateStepFrames();
            Sound.SoundStackManager.Update();

            if (Ref.music != null)
            {
                Ref.music.Update();
            }

            if (Ref.steam.inOverlay)
            {
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
                    var factions = DssRef.world.factions.counter();
                    while (factions.Next())
                    {
                        factions.sel.update();

                        //if (DssRef.time.oneSecond)
                        //{
                        //    factions.sel.oneSecUpdate();
                        //}
                    }                    
                }
            }
            else
            {
                
                if (isReady)
                {                        
                    var factions = DssRef.world.factions.counter();
                    while (factions.Next())
                    {
                        factions.sel.PauseUpdate();
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
                    local.userUpdate(false);
                    if (local.input.Menu.DownEvent)
                    {
                        menuSystem.pauseMenu();
                    }
                }
            }

            if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape) && !menuSystem.IsOpen())
            {
                menuSystem.pauseMenu();
            }

            Engine.ParticleHandler.Update(time);

        }

        int asynchGameObjectsMinutes = 0;
        protected bool asynchGameObjectsUpdate(int id, float time)
        {
            float seconds = DssRef.time.pullAsyncGameObjects_Seconds();

            if (cutScene == null)
            {
                bool minute = DssRef.time.pullMinute(ref asynchGameObjectsMinutes);

                //foreach (var m in DssRef.world.cities)
                //{
                //    m.asynchGameObjectsUpdate(minute);
                //}

                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    factions.sel.asynchGameObjectsUpdate(time, seconds, minute);
                }

            }
            return exitThreads;
        }

        protected bool asynchNearObjectsUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                DssRef.world.unitCollAreaGrid.asynchUpdate();

                //foreach (var m in DssRef.world.cities)
                //{
                //    m.asynchNearObjectsUpdate();
                //}

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

        public override PlayStateType PlayType()
        {
            return PlayStateType.BattleLab;
        }
    }
}
