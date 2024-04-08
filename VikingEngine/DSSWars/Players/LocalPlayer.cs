using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Players;
using VikingEngine.LootFest.Players;
using VikingEngine.HUD.RichBox;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.GameState;
using System;

namespace VikingEngine.DSSWars.Players
{    
    class LocalPlayer : AbsPlayer
    {   
        public Engine.PlayerData playerData;

        public GameHud hud;
        public InputMap input;
        
        public GameMenuSystem menuSystem;
        public MapControls mapControls;
        public ArmyControls armyControls = null;

        public MapDetailLayerManager drawUnitsView;
        public bool bUnitDetailLayer_buffer;
        public bool bUnitDetailLayer;

        public Rectangle2 cullingTileArea = Rectangle2.ZeroOne;
        public DiplomacyMap diplomacyMap = null;

        public FloatingInt_Max commandPoints = new FloatingInt_Max();
        public FloatingInt_Max diplomaticPoints = new FloatingInt_Max();
        public int diplomaticPoints_softMax;

        //public int servantFactions = 0;
        //public int warsStarted = 0;
        public Data.Statistics statistics = new Data.Statistics();

        public PlayerToPlayerDiplomacy[] toPlayerDiplomacies = null;
        public Automation automation;

        public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(4);

        int tabCity = -1;
        SpottedArrayCounter<Army> tabArmy;
        SpottedArrayCounter<BattleGroup> tabBattle;

        public LocalPlayer(Faction faction, int playerindex, int numPlayers)
            :base(faction)
        {
            faction.factiontype = FactionType.Player;
            var pStorage = DssRef.storage.localPlayers[playerindex];
            faction.SetProfile(DssRef.storage.flagStorage.flagDesigns[pStorage.profile]);
            faction.diplomaticSide = DiplomaticSide.Light;

            tabArmy = faction.armiesCounter.Clone();
            tabBattle = new SpottedArrayCounter<BattleGroup>(battles);

            input = new InputMap(playerindex);
            input.setInputSource(pStorage.inputSource.sourceType, pStorage.inputSource.controllerIndex);

            faction.profile.gameStartInit();
            faction.displayInFullOverview = true;

            hud = new GameHud(this);
            automation = new Automation(this);

            playerData = Engine.XGuide.GetPlayer(playerindex);
            playerData.Tag = this;
            playerData.view.SetDrawArea(numPlayers, pStorage.screenIndex, false, null);

            mapControls = new Players.MapControls(this);
            mapControls.setCameraPos(faction.mainCity.tilePos);

            Ref.draw.AddPlayerScreen(playerData);
            drawUnitsView = new MapDetailLayerManager(playerData);

            menuSystem = new GameMenuSystem(input, IsLocalHost());

            new AsynchUpdateable(interactAsynchUpdate, "DSS player interact", 0);

            refreshNeihgborAggression();
            toPlayerDiplomacies = new PlayerToPlayerDiplomacy[numPlayers];
            //initPlayerToPlayer(playerindex, numPlayers);
        }

        public void initPlayerToPlayer(int playerindex, int numPlayers)
        {
            
            for (int i = 0; i < numPlayers; i++)
            {
                if (i != playerindex)
                {
                    if (toPlayerDiplomacies[i] == null)
                    {
                        var PtoP = new PlayerToPlayerDiplomacy()
                        { index = i, };

                        toPlayerDiplomacies[i] = PtoP;
                        var otherP = DssRef.state.localPlayers[i].toPlayerDiplomacies[playerindex] = PtoP;
                    }
                }
            }
        }

        public override void createStartUnits()
        {
            IntVector2 onTile = DssRef.world.GetFreeTile(faction.mainCity.tilePos);

            var mainArmy = faction.NewArmy(onTile);
            for (int i = 0; i < 5; ++i)
            {
                new SoldierGroup(mainArmy, UnitType.Soldier, false);
            }

            if (IsPlayer() && DssRef.difficulty.honorGuard)
            {
                int guardCount = 12;

                var citiesC = faction.cities.counter();
                while (citiesC.Next())
                {
                    if (citiesC.sel != faction.mainCity)
                    {
                        onTile = DssRef.world.GetFreeTile(citiesC.sel.tilePos);
                        var army = faction.NewArmy(onTile);
                        for (int i = 0; i < 3; ++i)
                        {
                            new SoldierGroup(army, UnitType.HonorGuard, false);
                            --guardCount;
                        }
                        army.OnSoldierPurchaseCompleted();
                        if (guardCount <= 3)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < guardCount; ++i)
                {
                    new SoldierGroup(mainArmy, UnitType.HonorGuard, false);
                }
            }
            mainArmy.OnSoldierPurchaseCompleted();
        }

        void refreshNeihgborAggression()
        {
            if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium)
            {
                var citiesC = faction.cities.counter();
                while (citiesC.Next())
                {
                    foreach (var n in citiesC.sel.neighborCities)
                    {
                        DssRef.world.cities[n].faction.player.onPlayerNeighborCapture(this);
                    }
                }
            }
        }

        public override void OnCityCapture(City city)
        {
            if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium)
            {
                foreach (var n in city.neighborCities)
                {
                    DssRef.world.cities[n].faction.player.onPlayerNeighborCapture(this);
                }                
            }
        }

        public override void onNewRelation(Faction otherFaction, DiplomaticRelation rel, RelationType previousRelation)
        {
            base.onNewRelation(otherFaction, rel, previousRelation);

            if (rel.Relation == RelationType.RelationType3_Ally)
            {
                DssRef.achieve.onAlly(faction, otherFaction);
            }
        }

        //public void loadedAndReady()
        //{ }

        override public void Update()
        { }

        public void userUpdate()
        {
            bool openMenySystem = menuSystem.update();
            
            if (!openMenySystem)
            {
                
                hud.update();

                if (hud.menuFocus)
                {
                    hud.updateMenuFocus();
                }
                else
                {
                    mapControls.update(hud.mouseOver);

                    if (input.AutomationSetting.DownEvent)
                    {
                        hud.OpenAutomationMenu();
                    }
                }

                updateDiplomacy();

                if (armyControls != null)
                {                    
                    armyControls.update();
                    
                }

                if (PlatformSettings.DevBuild)
                {
                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                    {
                        //cityBuilderTest();
                        DssRef.state.events.TestNextEvent();
                        
                        //battleLineUpTest(true);

                        //battleLineUpTest(true);
                        new Display.CutScene.EndScene(true);
                    }

                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                    {
                        
                        //hud.messages.Add("message!", "Hello hello");
                        //battleLineUpTest(false);
                        //mapControls.FocusObject()?.tagObject();
                    }
                }

                if (input.inputSource.IsController)
                {
                    if (!mapControls.focusedObjectMenuState() && 
                        !hud.menuFocus &&
                        input.Select.DownEvent)    
                    {
                        if (mapControls.selection.obj == null)
                        {
                            mapSelect();
                        }
                        else
                        {
                            mapExecute();
                        }
                    }                    
                }
                else
                {
                    if (!hud.mouseOver)
                    {
                        if (input.Select.DownEvent)
                        {
                            mapSelect();
                        }
                        if (input.Execute.DownEvent)
                        {
                            mapExecute();
                        }
                    }
                }

               

                updateGameSpeed();
            }

            updateObjectTabbing();

            //DssRef.state.detailMap.PlayerUpdate(mapControls.playerPointerPos, bUnitDetailLayer);
            drawUnitsView.Update();
        }

        public void asyncUserUpdate()
        {
            diplomacyMap?.asynchUpdate();

            automation.asyncUpdate();
        }

        void updateObjectTabbing()
        {
            //CITY
            if (input.NextCity.DownEvent && faction.cities.Count > 0)
            {
                tabCity++;
                if (tabCity >= faction.cities.Count)
                {
                    tabCity = 0;
                }

                int current = 0;
                var citiesC = faction.cities.counter();
                while (citiesC.Next())
                {
                    if (current == tabCity)
                    { 
                        //focus on city
                        mapControls.cameraFocus = citiesC.sel;
                        mapSelect(citiesC.sel);

                        return;
                    }
                    current++;
                }
            }

            //ARMY
            if (input.NextArmy.DownEvent)
            {
                if (tabArmy.Next_Rollover())
                {
                    mapControls.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }

            //BATTLE
            if (input.NextBattle.DownEvent)
            {
                if (tabBattle.Next_Rollover())
                {
                    mapControls.cameraFocus = tabBattle.sel;
                    return;
                }
            }
        }

        void updateDiplomacy()
        {
            if (drawUnitsView.current.DrawOverview)
            {
                if (diplomacyMap == null)
                {
                    diplomacyMap = new DiplomacyMap(this);
                }

                diplomacyMap.update();
            }
            else
            {
                if (diplomacyMap != null)
                {
                    diplomacyMap.DeleteMe();
                    diplomacyMap = null;
                }
            }
        }

        void updateGameSpeed()
        {
           
            if (DssRef.difficulty.allowPauseCommand && input.PauseGame.DownEvent && IsLocalHost())
            {
                DssRef.state.pauseAction();
            }

            if (DssRef.state.IsSinglePlayer() && input.GameSpeed.DownEvent)
            {
                if (Ref.isPaused)
                {
                    Ref.isPaused = false;
                    Ref.GameTimeSpeed = 1f;
                }
                else
                {
                    for (int i = 0; i < DssLib.GameSpeedOptions.Length; i++)
                    {
                        if (DssLib.GameSpeedOptions[i] == Ref.GameTimeSpeed)
                        {
                            int next = Bound.SetRollover(i + 1, 0, DssLib.GameSpeedOptions.Length - 1);
                            Ref.GameTimeSpeed = DssLib.GameSpeedOptions[next];
                            hud.needRefresh = true;
                            break;
                        }
                    }
                }
            }           
        }


        void cityBuilderTest()
        {
            //IntVector2 position = mapControls.subTilePosition;

            //var model = DssRef.models.ModelInstance( LootFest.VoxelModelName.city_tower24, WorldData.SubTileWidth * 1.4f, false);//1.4f
            //model.AddToRender(DrawGame.UnitDetailLayer);
            //model.position = WP.ToSubTilePos_Centered(position);
           
        }



        void battleLineUpTest(bool friendly)
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(45));
            Rotation1D playerRot = enemyRot;//enemyRot.getInvert();

            Faction enemyFac = DssRef.state.darkLordPlayer.faction;
            DssRef.state.darkLordPlayer.faction.hasDeserters = false;
            //DssRef.world.factionsCounter.Reset();

            //enemyFac = DssRef.world.factions.Array[DssRef.world.evilFactionIndex];
            ////while (DssRef.world.factionsCounter.Next())
            ////{
            ////    if (DssRef.world.factionsCounter.sel.player.IsAi())
            ////    {
            ////        enemyFac = DssRef.world.factionsCounter.sel;
            ////        break;
            ////    }
            ////}

            IntVector2 position = mapControls.tilePosition;

            //if (friendly)
            {
                var army = faction.NewArmy(position);
                army.rotation = playerRot;

                //int count = Ref.rnd.Int(4, 8);
                for (int i = 0; i < 15; ++i)
                {
                    new SoldierGroup(army, UnitType.Viking, false).completeTransform(SoldierTransformType.ToShip);
                }
                for (int i = 0; i < 10; ++i)
                {
                    new SoldierGroup(army, UnitType.Soldier, false).completeTransform(SoldierTransformType.ToShip);
                }

                army.refreshPositions(true);
            }
            //else
            {
                //{
                //    var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                //    army.rotation = enemyRot;
                //    //int count = 4;//Ref.rnd.Int(4, 8);
                //    for (int i = 0; i < 5; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Trollcannon, false);
                //    }
                //    for (int i = 0; i < 9; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Pikeman, false);
                //    }
                //    //for (int i = 0; i < 5; ++i)
                //    //{
                //    //    new SoldierGroup(army, UnitType.Ballista, false);
                //    //}
                //    army.refreshPositions(true);
                //}


                //{
                //    var army = enemyFac.NewArmy(VectorExt.AddY(position, 2));
                //    army.rotation = enemyRot;
                //    //int count = 4;//Ref.rnd.Int(4, 8);
                //    for (int i = 0; i < 5; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Soldier, false);
                //    }
                //    for (int i = 0; i < 5; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Ballista, false);
                //    }
                //    army.refreshPositions(true);
                //}
                //count = Ref.rnd.Int(4, 8);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Pikeman, false);
                //}


                //count = Ref.rnd.Int(0, 8);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.CrossBow, false);
                //}

                //count = Ref.rnd.Int(0, 16);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Trollcannon, false);
                //}
                //army.refreshPositions(true);
            }

        }

        void mapSelect()
        { 
            bool oldselection = clearSelection();
            
            bool newselection = clickHover();

            if (oldselection && !newselection)
            {
                SoundLib.back.Play();
            }
        }

        void mapSelect(AbsWorldObject mapObject)
        {
            clearSelection();

            mapControls.hover.obj = mapObject;
            clickHover();

        }
        
        public bool clearSelection()
        {
            bool bClear=false;

            if (armyControls != null)
            {
                armyControls.clearState();
                armyControls = null;
                
            }

            bClear = mapControls.clearSelection();
            hud.clearState();

            return bClear;
        }

        void mapExecute()
        {
            if (armyControls != null)
            {
                armyControls.mapExecute();
                armyControls.moveOrderEffect();

                if (input.inputSource.IsController)
                {
                    clearSelection();
                }
            }
        }

        public bool interactAsynchUpdate(int id, float time)
        {
            //mapControls.asynchUpdate();
            armyControls?.asynchUpdate();

            return false;
        }

        bool clickHover()
        {
            if (mapControls.hover.obj != null &&
                mapControls.hover.obj.GetFaction() == this.faction)
            {
                SoundLib.click.Play();

                //mapControls.selection.obj = mapControls.hover.obj;
                mapControls.onSelect();

                if (mapControls.selection.obj.gameobjectType() == GameObjectType.Army)
                {
                    armyControls = new ArmyControls(this, (Army)mapControls.selection.obj);
                }

                return true;
            }

            return false;
        }

        public override void onGameStart()
        {
            base.onGameStart();
            oneSecUpdate();
            commandPoints.value = commandPoints.max * 0.5;
            diplomaticPoints.value = diplomaticPoints.max * 0.6;

        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            commandPoints.setMax(DssLib.DefaultMaxCommand + DssLib.NobelHouseAddMaxCommand * faction.nobelHouseCount);
            commandPoints.add(DssLib.DefaultCommandPerSecond + DssLib.NobelHouseAddCommand * faction.nobelHouseCount);

            double max = DssRef.diplomacy.DefaultMaxDiplomacy + DssRef.diplomacy.NobelHouseAddMaxDiplomacy * faction.nobelHouseCount;
            diplomaticPoints_softMax = (int)Math.Floor(max);
            diplomaticPoints.setMax(max + DssRef.diplomacy.Diplomacy_HardMax_Add);

            if (diplomaticPoints.value < diplomaticPoints_softMax)
            {
                diplomaticPoints.add(DssRef.diplomacy.DefaultDiplomacyPerSecond + DssRef.diplomacy.NobelHouseAddDiplomacy * faction.nobelHouseCount, diplomaticPoints_softMax);
            }
            else
            {
                diplomaticPoints.add(DssRef.diplomacy.AddDiplomacy_AfterSoftlock_PerSecond);
            }

            if (StartupSettings.EndlessResources)
            {
                faction.gold += 1000;
            }

            if (StartupSettings.EndlessDiplomacy)
            {
                diplomaticPoints.max = 10;
                diplomaticPoints.value = 10;
            }

            automation.oneSecondUpdate();
        }

        public bool IsLocalHost()
        { 
            return playerData.localPlayerIndex == 0;
        }

        public override bool IsLocal => true;

        public override bool IsAi()
        {
            return false;
        }

        public override bool IsPlayer()
        {
            return true;
        }

        public override LocalPlayer GetLocalPlayer()
        {
            return this;
        }
        public override string Name => playerData.PublicName(LoadedFont.Regular);
    }
}
