using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map;
using VikingEngine.ToGG.MoonFall;
//

namespace VikingEngine.DSSWars
{
    class PlayState : Engine.GameState
    {
        WorldResources resources = new WorldResources();

        Map.MapLayer_Factions factionsMap;
        Map.MapLayer_Overview overviewMap;
        public Map.MapLayer_Detail detailMap;

        public Culling culling;
        public PathFindingPool pathFindingPool = new PathFindingPool();
        
        public int nextGroupId = 0;
        public List<Players.LocalPlayer> localPlayers;
        public Players.DarkLordPlayer darkLordPlayer;
        public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(64);

        bool host;
        bool isReady= false;
        public bool PartyMode = false;   
        bool exitThreads = false;
        public GameEvents events;
        public AbsCutScene cutScene=null;

        bool bResourceUpdate = false;

        public PlayState(bool host)
            : base(true)
        {
            DssRef.state = this;

            new Diplomacy();
            new Achievements();
            new GameTime();
            HudLib.Init();
            initPlayers();
            culling = new Culling();

            this.host = host;

            factionsMap = new MapLayer_Factions();
            overviewMap = new Map.MapLayer_Overview(factionsMap);
            detailMap = new Map.MapLayer_Detail();

            Engine.Update.SetFrameRate(60);

            onGameStart();
            initStartUnits();
            
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

            if (StartupSettings.RunResoursesUpdate)
            {
                new AsynchUpdateable_TryCatch(asyncResourcesUpdate, "DSS resources update", 61);
            }
            isReady = host;
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
        public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            events.readGameState(r, version, pointers);
        }

        void initPlayers()
        {
            Players.AiPlayer.EconomyMultiplier = Difficulty.AiEconomyLevel[DssRef.difficulty.aiEconomyLevel] / 100.0; ;

            new Faction(DssRef.world, FactionType.DarkLord);
            new Faction(DssRef.world, FactionType.SouthHara);

            DssRef.world.factionsCounter.Reset();
            while (DssRef.world.factionsCounter.Next())
            {

                DssRef.world.factionsCounter.sel.initDiplomacy(DssRef.world);
                if (DssRef.world.factionsCounter.sel.factiontype == FactionType.DarkLord)
                {
                    darkLordPlayer = new Players.DarkLordPlayer(DssRef.world.factionsCounter.sel);
                }
                else
                {
                    new Players.AiPlayer(DssRef.world.factionsCounter.sel);
                }
            }

            int playerCount = DssRef.storage.playerCount;
            localPlayers = new List<Players.LocalPlayer>(playerCount);
            Engine.Screen.SetupSplitScreen(playerCount, !DssRef.storage.verticalScreenSplit);

            for (var i = 0; i < playerCount; ++i)
            {
                var startFaction = DssRef.world.getNextFreeFaction(i == 0);
                var local = new Players.LocalPlayer(startFaction, i, playerCount);
                localPlayers.Add(local);
            }

            for (var i = 0; i < playerCount; ++i)
            {
                localPlayers[i].initPlayerToPlayer(i, playerCount);
            }
        }

        void onGameStart()
        {
            events = new GameEvents();

            DssRef.world.factionsCounter.Reset();
            while (DssRef.world.factionsCounter.Next())
            {
                DssRef.world.factionsCounter.sel.onGameStart();
            }

            foreach (var m in DssRef.world.cities)
            {
                m.onGameStart();
            }
        }

        void initStartUnits()
        {
            if (StartupSettings.SpawnStartingArmies)
            {
                DssRef.world.factionsCounter.Reset();
                while (DssRef.world.factionsCounter.Next())
                {
                    DssRef.world.factionsCounter.sel.player.createStartUnits();
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

            if (cutScene != null)
            {
                cutScene.Time_Update(time);
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


            if (DssRef.time.halfSecond)
            {
                overviewMap.HalfSecondUpdate();
            }
            detailMap.update();

            foreach (var local in localPlayers)
            {
                local.userUpdate();
            }

            Engine.ParticleHandler.Update(time);

        }

        public void OneMinute_Update()
        { 
            bResourceUpdate = true;
        }

        public override void OnDestroy()
        {
            exitThreads = true;
            base.OnDestroy();
        }

        public void pauseAction()
        {
            Ref.SetPause(!Ref.isPaused);
            //Ref.isPaused = !Ref.isPaused;

            //if (Ref.isPaused)
            //{
            //    Ref.GameTimeSpeed = 0f;
            //}
            //else
            //{
            //    Ref.GameTimeSpeed = 1f;
            //}
        }

        

        public void exit()
        {
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
            DssRef.world.factionsCounter.Reset();
            while (DssRef.world.factionsCounter.Next())
            {
                DssRef.world.factionsCounter.sel.shareAllHostedObjects(sender);
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
                events.asyncUpdate();
            }
            return exitThreads;
        }

        bool asyncUserUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                foreach (var local in localPlayers)
                {
                    local.asyncUserUpdate();
                }
            }
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
                    //factions.sel.asynchSleepObjectsUpdate(time);
                    if (factions.sel.factiontype == FactionType.SouthHara)
                    { 
                        lib.DoNothing();
                    }
                    factions.sel.asynchSleepObjectsUpdate(time);
                    
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
