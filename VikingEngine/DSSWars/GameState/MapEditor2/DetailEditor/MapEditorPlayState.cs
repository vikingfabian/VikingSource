using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.Players;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class MapEditorPlayState : AbsPlayState
    {
        bool isReady = false;

        public MapEditorPlayState()
            : base()
        {
            initGameState();
            onGameStart();
            DssRef.stats.start_battle_lab.addOne_ifUnset();
        }

        public override PlayStateType PlayType()
        {
            return PlayStateType.MapEditor;
        }

        public void initGameState()
        {
            Ref.rnd.SetSeed(DssRef.world.metaData.worldId.seed);
            foreach (var tile in DssRef.world.subTileGrid.array)
            {
                if (tile.IsLand())
                {
                    lib.DoNothing();
                    break;
                }
            }
            menuSystem = new GameMenuSystem();

            new GameObject.AllUnits();
            //new Diplomacy();

            new GameTime();
            HudLib.Init();

            prePlayerInit();
            initPlayers();
            postPlayerInit();
            //initScenario();

            //LocalHost().gameControls.map.battleModeCamBound();
        }

        virtual protected void initPlayers()
        {
            new Faction(DssRef.world, FactionType.DarkLord);

            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                //factionsCounter.sel.initDiplomacy(DssRef.world);
                if (factionsCounter.sel.factiontype == FactionType.DarkLord)
                {
                    DssRef.settings.darkLordPlayer = new Players.DarkLordPlayer(factionsCounter.sel, true);
                }
                else
                {
                    new Players.AiPlayer(factionsCounter.sel, true);
                }
            }

            int playerCount = 1;

            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount);
            for (var i = 0; i < playerCount; ++i)
            {
                var startFaction = DssRef.world.getPlayerAvailableFaction2(localPlayers, i == 0, false);
                var local = createLocalPlayer(startFaction);
                local.assignPlayer(i, playerCount, true);
                localPlayers.Add(local);
                local.baseOnGameStart();

                Mouse.AddPlayer(local.playerData, playerCount, local.gameControls.input.moveCursor, local.gameControls.input.menuInput.cursor);
            }
        }

        virtual protected LocalPlayer createLocalPlayer(Faction faction)
        {
            return new MapEditorPlayer(faction);
        }

        virtual protected void initScenario()
        { }

        void onGameStart()
        {
            Ref.music.OnGameStart();
            
            startMapThreads();

            foreach (var m in DssRef.world.cities)
            {
                m.onEditorStart();
            }

            isReady = true;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            //MayChangeDetail_OnNewUpdate();
            updateStepFrames();
            Sound.SoundStackManager.Update();

            if (Ref.music != null)
            {
                Ref.music.Update();
            }

            if (Ref.steam.inOverlay)
            {
                if (!menuSystem.IsOpen())
                {
                    menuSystem.pauseMenu();
                }
                return;
            }

            if (pauseMenuUpdate(out _))
            {
                return;
            }

            if (exitGameStateThreads != null)
            {
                if (cutScene == null)
                {
                    new ExitScene(exitGameStateThreads);
                }
                return;
            }


            //if (Ref.DeltaGameTimeMs > 0)
            //{
            //    DssRef.time.update();

            //    if (isReady)
            //    {
            //        foreach (var m in DssRef.world.cities)
            //        {
            //            m.PauseUpdate();
            //        }

            //        var factions = DssRef.world.factions.counter();
            //        while (factions.Next())
            //        {
            //            if (factions.sel.player != null)
            //            {
            //                factions.sel.update();
            //            }
            //        }
            //    }
            //}
            //else
            //{
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
            //}

            if (DssRef.time.halfSecond)
            {
                overviewMap.HalfSecondUpdate();
            }
            switch (processTime.update())
            {
                case ProcessEvent.OverviewMap:
                    overviewMap.HalfSecondUpdate();
                    break;
                case ProcessEvent.SubTileReload:
                    detailMap.oneSecondUpdate = true;
                    overviewMap.bRefreshTimer = true;
                    break;
            }

            //detailMap.update();

            overviewMap.update();

            if (localPlayers != null)
            {
                foreach (var local in localPlayers)
                {
                    local.userUpdate(false);
                    if (local.gameControls.input.Menu.DownEvent)
                    {
                        menuSystem.pauseMenu();
                    }
                }
            }

            if (Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape) && !menuSystem.IsOpen())
            {
                menuSystem.pauseMenu();
            }

            //Engine.ParticleHandler.Update(time);

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

        public override int PathThreadCount()
        {
            return 1;
        }
    }
}
