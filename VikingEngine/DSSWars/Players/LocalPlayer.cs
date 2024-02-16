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

        public int servantFactions = 0;
        public int warsStarted = 0;

        public PlayerToPlayerDiplomacy[] toPlayerDiplomacies = null;

        public LocalPlayer(Faction faction, int playerindex, int numPlayers)
            :base(faction)
        {
            var pStorage = DssRef.storage.localPlayers[playerindex];
            faction.SetProfile(DssRef.storage.profiles[pStorage.profile]);
            faction.diplomaticSide = DiplomaticSide.Light;

            input = new InputMap(playerindex);
            input.setInputSource(pStorage.inputSource.sourceType, pStorage.inputSource.controllerIndex);

            faction.profile.gameStartInit();
            faction.displayInFullOverview = true;

            hud = new GameHud(this);

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

            if (IsPlayer() && DssRef.storage.honorGuard)
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
            if (DssRef.storage.aiAggressivity >= AiAggressivity.Medium)
            {
                faction.cityCounter.Reset();
                while (faction.cityCounter.Next())
                {
                    foreach (var n in faction.cityCounter.sel.neighborCities)
                    {
                        DssRef.world.cities[n].faction.player.onPlayerNeighborCapture(this);
                    }
                }
            }
        }

        public override void OnCityCapture()
        {
            if (DssRef.storage.aiAggressivity >= AiAggressivity.Medium)
            {
                var cityC = faction.cityCounter.Clone();

                while (cityC.Next())
                {
                    foreach (var n in cityC.sel.neighborCities)
                    {
                        DssRef.world.cities[n].faction.player.onPlayerNeighborCapture(this);
                    }
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
            //if (menuSystem.Open)
            //{
            //    if (!menuSystem.menuUpdate())
            //    {
            //        menuSystem.closeMenu();
            //        mapControls.clearSelection();
            //    }
            //}
            //else
            if (!openMenySystem)
            {
                //if (!StartupSettings.Trailer)
                //{
                hud.update();
                //}
                mapControls.update(hud.mouseOver);

                updateDiplomacy();

                if (armyControls != null)
                {                    
                    armyControls.update();
                    
                }

                if (PlatformSettings.DevBuild)
                {
                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                    {
                        DssRef.state.events.TestNextEvent();
                        //battleLineUpTest(true);
                    }

                    if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                    {
                        
                        //hud.messages.Add("message!", "Hello hello");
                        battleLineUpTest(false);
                        //mapControls.FocusObject()?.tagObject();
                    }
                }

                if (input.inputSource.IsController)
                {
                    if (!mapControls.focusedObjectMenuState() && 
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

            

            //DssRef.detailMap.PlayerUpdate(mapControls.playerPointerPos, bUnitDetailLayer);
            drawUnitsView.Update();
        }

        public void asyncUserUpdate()
        {
            diplomacyMap?.asynchUpdate();
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
           
            if (input.PauseGame.DownEvent && IsLocalHost())
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




        

        void battleLineUpTest(bool friendly)
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(220 + Ref.rnd.Plus_Minus(45));
            Rotation1D playerRot = Rotation1D.FromDegrees(180 - 45); //enemyRot.getInvert();

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
                for (int i = 0; i < 20; ++i)
                {
                    new SoldierGroup(army, UnitType.Sailor, false).completeTransform(SoldierTransformType.ToShip);
                }

                //count = Ref.rnd.Int(0, 8);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Soldier, false);
                //}

                //count = Ref.rnd.Int(4, 8);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Archer, false);
                //}


                //count = Ref.rnd.Int(0, 8);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Knight, false);
                //}

                //count = Ref.rnd.Int(0, 16);
                //for (int i = 0; i < count; ++i)
                //{
                //    new SoldierGroup(army, UnitType.Ballista, false);
                //}
                army.refreshPositions(true);
            }
            //else
            {
                //{
                //    var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                //    army.rotation = enemyRot;
                //    //int count = 4;//Ref.rnd.Int(4, 8);
                //    for (int i = 0; i < 3; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Soldier, false).completeTransform(SoldierTransformType.ToShip);
                //    }
                //    for (int i = 0; i < 3; ++i)
                //    {
                //        new SoldierGroup(army, UnitType.Ballista, false).completeTransform(SoldierTransformType.ToShip);
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

        public bool clearSelection()
        {
            bool bClear=false;

            if (armyControls != null)
            {
                armyControls.clearState();
                armyControls = null;
                
            }

            bClear = mapControls.clearSelection();

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
                mapControls.hover.obj.Faction() == this.faction)
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
            diplomaticPoints.value = diplomaticPoints.max * 0.5;

        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            commandPoints.setMax(DssLib.DefaultMaxCommand + DssLib.NobelHouseAddMaxCommand * faction.nobelHouseCount);
            commandPoints.add(DssLib.DefaultCommandPerSecond + DssLib.NobelHouseAddCommand * faction.nobelHouseCount);

            diplomaticPoints.setMax(DssRef.diplomacy.DefaultMaxDiplomacy + DssRef.diplomacy.NobelHouseAddMaxDiplomacy * faction.nobelHouseCount);
            diplomaticPoints.add(DssRef.diplomacy.DefaultDiplomacyPerSecond + DssRef.diplomacy.NobelHouseAddDiplomacy * faction.nobelHouseCount);

            if (StartupSettings.EndlessResources)
            {
                faction.gold += 1000;
            }

            if (StartupSettings.EndlessDiplomacy)
            {
                diplomaticPoints.max = 10;
                diplomaticPoints.value = 10;
            }
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
