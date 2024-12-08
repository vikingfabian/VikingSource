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
using System.IO;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Data;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.HeroQuest.Net;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer : AbsPlayer
    {
        public const int MaxSpeedOption = 5;
        public Engine.PlayerData playerData;

        public GameHud hud;
        public InputMap input;
        bool inputConnected;
        
        public MapControls mapControls;
        public ArmyControls armyControls = null;

        public MapDetailLayerManager drawUnitsView;
        public bool bUnitDetailLayer_buffer;
        public bool bUnitDetailLayer;

        public Rectangle2 cullingTileArea = Rectangle2.ZeroOne;
        public DiplomacyMap diplomacyMap = null;
        public CityTagMap cityTagMap = null;

        public FloatingInt_Max commandPoints = new FloatingInt_Max();
        public FloatingInt_Max diplomaticPoints = new FloatingInt_Max();
        public int diplomaticPoints_softMax;

        public Data.Statistics statistics = new Data.Statistics();

        public PlayerToPlayerDiplomacy[] toPlayerDiplomacies = null;
        public Automation automation;
        public Build.BuildControls buildControls;

        public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(4);

        int tabCity = -1;
        SpottedArrayCounter<Army> tabArmy;
        SpottedArrayCounter<BattleGroup> tabBattle;

        public int mercenaryCost = DssRef.difficulty.MercenaryPurchaseCost_Start;

        const int MercenaryMarketSoftLock1 = DssLib.MercenaryPurchaseCount * 5;
        const double MercenaryMarketAddPerSec_Speed1 = 0.5;
        const double MercenaryMarketAddPerSec_Speed2 = 0.3;
        public FloatingInt mercenaryMarket = new FloatingInt() { value = DssLib.MercenaryPurchaseCount * 2 };

        public MenuTab factionTab = HeadDisplay.Tabs[0];
        public MenuTab cityTab = CityMenu.Tabs[0];
        public MenuTab armyTab = ArmyMenu.Tabs[0];
        public ResourcesSubTab resourcesSubTab = ResourcesSubTab.Overview_Resources;
        public WorkSubTab workSubTab = WorkSubTab.Priority_Resources;
        public ProgressSubTab progressSubTab = 0;
        public MixTabEditType mixTabEditType = MixTabEditType.None;
        public ItemResourceType mixTabItem = ItemResourceType.NONE;

        public DeliveryStatus menDeliveryCopy, itemDeliveryCopy;
        public ConscriptProfile soldierConscriptCopy, archerConscriptCopy, warmashineConscriptCopy, knightConscriptCopy, gunConscriptCopy, cannonConscriptCopy;

        public PlayerControls.Tutorial tutorial = null;
        CityBorders cityBorders = new CityBorders();
        public bool viewCityTagsOnMap = true;
        public bool viewArmyTagsOnMap = true;
        public int[] GameSpeedOptions;

        public LocalPlayer(Faction faction)
           : base(faction)
        {
            orders = new Orders.Orders();

            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;

            automation = new Automation(this);
            if (DssRef.storage.speed5x)
            {
                GameSpeedOptions = new int[] { 1, 2, MaxSpeedOption };
            }
            else
            {
                GameSpeedOptions = new int[] { 1, 2 };
            }

            faction.technology = new XP.TechnologyTemplate();
            faction.technology.iron = XP.TechnologyTemplate.FactionUnlock;
        }

        public void assignPlayer(int playerindex, int numPlayers, bool newGame)
        {
            var pStorage = DssRef.storage.localPlayers[playerindex];
            faction.SetProfile(DssRef.storage.flagStorage.flagDesigns[pStorage.profile]);
            faction.diplomaticSide = DiplomaticSide.Light;

            tabArmy = faction.armies.counter();
            tabBattle = new SpottedArrayCounter<BattleGroup>(battles);

            input = new InputMap(playerindex);
            input.setInputSource(pStorage.inputSource.sourceType, pStorage.inputSource.controllerIndex);
            if (pStorage.inputSource.IsController)
            {
                input.copyDataFrom(Ref.gamesett.controllerMap);
            }
            else
            {
                input.copyDataFrom(Ref.gamesett.keyboardMap);
            }

            inputConnected = input.Connected;

            faction.profile.gameStartInit();
            faction.displayInFullOverview = true;

            hud = new GameHud(this, numPlayers);
            buildControls = new Build.BuildControls(this);


            playerData = Engine.XGuide.GetPlayer(playerindex);
            playerData.Tag = this;
            playerData.view.SetDrawArea(numPlayers, pStorage.screenIndex, false, null);

            mapControls = new Players.MapControls(this);
            mapControls.setCameraPos(faction.mainCity.tilePos);

            Ref.draw.AddPlayerScreen(playerData);
            drawUnitsView = new MapDetailLayerManager(playerData);
            InitTutorial(newGame);

            new AsynchUpdateable(interactAsynchUpdate, "DSS player interact", playerindex);

            refreshNeihgborAggression();
            if (numPlayers > 1)
            {
                toPlayerDiplomacies = new PlayerToPlayerDiplomacy[numPlayers];
            }

            if (StartupSettings.EndlessResources)
            {
                foreach (var c in faction.cities.Array)
                {
                    if (c != null)
                    {
                        foreach (var type in City.MovableCityResourceTypes)
                        {
                            var res = c.GetGroupedResource(type);
                            res.amount += 1000;
                            c.SetGroupedResource(type, res);
                        }
                    }
                }
            }

            menDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(true);

            itemDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(false);

            soldierConscriptCopy = new ConscriptProfile();
            soldierConscriptCopy.defaultSetup(BarracksType.Soldier);

            archerConscriptCopy = new ConscriptProfile();
            archerConscriptCopy.defaultSetup(BarracksType.Archer);

            warmashineConscriptCopy = new ConscriptProfile();
            warmashineConscriptCopy.defaultSetup(BarracksType.Warmashine);

            knightConscriptCopy = new ConscriptProfile();
            knightConscriptCopy.defaultSetup(BarracksType.Knight);

            gunConscriptCopy = new ConscriptProfile();
            gunConscriptCopy.defaultSetup(BarracksType.Gun);

            cannonConscriptCopy = new ConscriptProfile();
            cannonConscriptCopy.defaultSetup(BarracksType.Cannon);


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

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write((short)diplomaticPoints.Int());
            statistics.writeGameState(w);
            if (toPlayerDiplomacies != null)
            {
                foreach (var tp in toPlayerDiplomacies)
                {
                    if (tp == null)
                    {
                        w.Write(false);
                    }
                    else
                    {
                        w.Write(true);
                        tp.writeGameState(w);
                    }
                }
            }
            automation.writeGameState(w);

            w.Write(mercenaryCost);

            tutorial_writeGameState(w);

            orders.writeGameState(w);

            w.Write(viewCityTagsOnMap);
            w.Write(viewArmyTagsOnMap);

            Debug.WriteCheck(w);
        }

        public override void readGameState(BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subversion, pointers);

            diplomaticPoints.value = r.ReadInt16();
            statistics.readGameState(r, subversion);
            if (toPlayerDiplomacies != null)
            {
                for (int i = 0; i < toPlayerDiplomacies.Length; ++i)
                {
                    if (r.ReadBoolean())
                    {
                        PlayerToPlayerDiplomacy tp = new PlayerToPlayerDiplomacy();
                        tp.readGameState(r, subversion);
                        toPlayerDiplomacies[i] = tp;
                    }
                }
            }
            automation.readGameState(r, subversion);

            if (subversion >= 2)
            {
                mercenaryCost = r.ReadInt32();
            }

            tutorial_readGameState(r, subversion);

            orders.readGameState(r, subversion, pointers);

            viewCityTagsOnMap = r.ReadBoolean();
            viewArmyTagsOnMap = r.ReadBoolean();

            Debug.ReadCheck(r);
        }

        public void InitTutorial(bool newGame)
        {
            if (newGame && DssRef.storage.runTutorial)
            {
                tutorial = new PlayerControls.Tutorial(this);
            }
            //inTutorialMode = false;
            //mapControls.setZoomRange(inTutorialMode);
        }

        public void tutorial_writeGameState(BinaryWriter w)
        {
            //w.Write(inTutorialMode);
            //w.Write((int)tutorialMission);
            //w.Write(tutorialMission_BuySoldier);
            //w.Write(tutorialMission_MoveArmy);
            if (tutorial != null)
            {
                w.Write(true);
                tutorial.tutorial_writeGameState(w);
            }
            else
            { w.Write(false); }
        }

        public void tutorial_readGameState(BinaryReader r, int subversion)
        {
            if (subversion >= 7)
            {
                bool inTutorialMode = r.ReadBoolean();
                if (subversion < 15)
                {
                    bool non1 = r.ReadBoolean();
                    bool non2 = r.ReadBoolean();
                }

                if (inTutorialMode)
                {
                    tutorial = new PlayerControls.Tutorial(this);
                    tutorial.tutorial_readGameState(r, subversion);
                }
            }
        }
        public void factionTabClick(int tab)
        {
            factionTab = HeadDisplay.Tabs[tab];
        }
        public void cityTabClick(int tab)
        {
            cityTab = AvailableCityTabs()[tab];
        }
        public void armyTabClick(int tab)
        {
            armyTab = AvailableArmyTabs()[tab];
        }

        public List<MenuTab> AvailableCityTabs()
        { 
            return tutorial != null ? tutorial.cityTabs : CityMenu.Tabs;
        }

        public List<MenuTab> AvailableArmyTabs()
        {
            return ArmyMenu.Tabs;
        }

        public bool InBuildOrdersMode()
        {
            return cityTab == Display.MenuTab.Build &&
                mapControls.selection.obj != null &&
                mapControls.selection.obj.gameobjectType() == GameObjectType.City &&
                buildControls.buildMode != SelectTileResult.None;                        
        }

        public void childrenTooltip(City city)
        {
            RichBoxContent content = new RichBoxContent();
            content.text(string.Format(DssRef.lang.WorkForce_ChildToManTime, 2));

            content.newParagraph();
            content.h2(DssRef.lang.WorkForce_ChildBirthRequirements);
            content.text(string.Format(DssRef.lang.WorkForce_AvailableHomes, city.homesUnused())).overrideColor = HudLib.ResourceCostColor(city.homesUnused() > 0);
            content.text(DssRef.lang.WorkForce_Peace).overrideColor = HudLib.ResourceCostColor(city.battleGroup == null);
            HudLib.ItemCount(content, DssRef.lang.Resource_TypeName_Food, city.res_food.amount.ToString()).overrideColor = HudLib.ResourceCostColor(city.res_food.amount > 0);

            hud.tooltip.create(this, content, true);
        }

        //public RbAction WorkSafeguardTooltip;
        

        public void followFactionTooltip(bool follows, double currentFactionValue)
        {
            RichBoxContent content = new RichBoxContent();

            content.h2(DssRef.lang.Hud_ToggleFollowFaction).overrideColor = HudLib.TitleColor_Action;
            content.newParagraph();

            string current;
            if (follows)
            {
                current = DssRef.lang.Hud_FollowFaction_Yes;
            }
            else
            { 
                current = string.Format(DssRef.lang.Hud_FollowFaction_No, currentFactionValue);
            }
            content.text(current).overrideColor = HudLib.InfoYellow_Light;

            hud.tooltip.create(this, content, true);
        }

        public void perSecondTooltip(bool minuteAverage)
        {
            RichBoxContent content = new RichBoxContent();
            content.text(DssRef.lang.Info_PerSecond);
            if (minuteAverage)
            {
                content.text(DssRef.lang.Info_MinuteAverage);
            }

            hud.tooltip.create(this, content, true);
        }

        

       

        public override void createStartUnits()
        {
            IntVector2 onTile = DssRef.world.GetFreeTile(faction.mainCity.tilePos);

            var mainArmy = faction.NewArmy(onTile);
            mainArmy.tagBack = CityTagBack.Blue;
            mainArmy.tagArt = ArmyTagArt.Specialize_Tradition;

            for (int i = 0; i < 5; ++i)
            {
                new SoldierGroup(mainArmy, DssLib.SoldierProfile_Standard);//mainArmy, UnitType.Soldier, false);
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
                        for (int i = 0; i < 4; ++i)
                        {
                            new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard);//UnitType.HonorGuard, false);
                            --guardCount;
                        }
                        army.OnSoldierPurchaseCompleted();
                        army.setMaxFood();
                        if (guardCount <= 3)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < guardCount; ++i)
                {
                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_HonorGuard);
                }
            }
            mainArmy.OnSoldierPurchaseCompleted();
            mainArmy.setMaxFood();
        }

        public void toPeacefulCheck_asynch()
        {
            if (faction.citiesEconomy.tax(null) > 0 && !DssRef.settings.AiDelay)
            {
                int warCount = 0;
                float opposingSize = 0;

                for (int relIx = 0; relIx < faction.diplomaticRelations.Length; ++relIx)
                {
                    if (faction.diplomaticRelations[relIx] != null &&
                        faction.diplomaticRelations[relIx].Relation <= RelationType.RelationTypeN2_Truce)
                    {
                        var opponent = faction.diplomaticRelations[relIx].opponent(faction);
                        if (opponent.player.IsAi())
                        {
                            ++warCount;
                            opposingSize += opponent.citiesEconomy.tax(null);
                        }
                    }
                }

                bool toPeaceful = true;

                if (opposingSize > 0)
                {
                    float opposingSizePerc;
                    
                    opposingSizePerc = opposingSize / faction.citiesEconomy.tax(null);
                    
                    toPeaceful = opposingSizePerc <= DssRef.difficulty.toPeacefulPercentage;
                }

                if (toPeaceful)
                {
                    //start a war
                    const int MaxTrials = 10;

                    for (int i = 0; i < MaxTrials; ++i)
                    {
                        var city = faction.cities.GetRandomUnsafe(Ref.rnd);
                        if (city != null)
                        {
                            foreach (var cindex in city.neighborCities)
                            {
                                var otherfaction = DssRef.world.cities[cindex].faction;
                                if ((otherfaction.factiontype == FactionType.DefaultAi ||  otherfaction.factiontype == FactionType.DarkFollower) &&
                                    otherfaction.armies.Count > 0)
                                {
                                    var rel = DssRef.diplomacy.GetRelationType(faction, otherfaction);
                                    if (rel >= RelationType.RelationTypeN1_Enemies && rel <= RelationType.RelationType1_Peace)
                                    {
                                        var aiPlayer = otherfaction.player.GetAiPlayer();
                                        if (aiPlayer.aggressionLevel <= AiPlayer.AggressionLevel1_RevengeOnly)
                                        {
                                            aiPlayer.aggressionLevel = AiPlayer.AggressionLevel2_RandomAttacks;
                                            aiPlayer.refreshAggression();
                                        }
                                        DssRef.diplomacy.declareWar(otherfaction, faction);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

            if (faction.cities.Count >= DssRef.world.cities.Count - 5)
            {
                DssRef.state.events.onWorldDomination();
            }
        }

        public void enterBattle(Battle.BattleGroup battleGroup, AbsMapObject playerUnit)
        {
            battles.Add(battleGroup);
            RichBoxContent content = new RichBoxContent();
            hud.messages.Title(content, DssRef.lang.Hud_Battle);

            var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
            hud.messages.ControllerInputIcons(gotoBattleButtonContent);
            gotoBattleButtonContent.Add(new RichBoxText(playerUnit.TypeName() + " - " + battleGroup.TypeName()));

            content.Add(new RichboxButton(gotoBattleButtonContent,
                new RbAction1Arg<Battle.BattleGroup>(goToBattle, battleGroup)));
            hud.messages.Add(content);
        }

        void goToBattle(Battle.BattleGroup battleGroup)
        {
            mapControls.cameraFocus = battleGroup;
        }

        public override void onNewRelation(Faction otherFaction, DiplomaticRelation rel, RelationType previousRelation)
        {
            base.onNewRelation(otherFaction, rel, previousRelation);

            if (rel.Relation == RelationType.RelationType3_Ally)
            {
                DssRef.achieve.onAlly(faction, otherFaction);
            }

            if ((rel.Relation <= RelationType.RelationTypeN3_War &&
                otherFaction.factiontype != FactionType.SouthHara)
                ||
                otherFaction.player.IsPlayer())
            {
                string title;
                if (rel.Relation >= RelationType.RelationType2_Good)
                {
                    title = DssRef.lang.Diplomacy_RelationType;
                }
                else if (previousRelation == RelationType.RelationTypeN2_Truce)
                {
                    title = DssRef.lang.Diplomacy_TruceEndTitle;
                }
                else
                {
                    title = DssRef.lang.Diplomacy_WarDeclarationTitle;
                    Ref.music.OnGameEvent();
                }

                RichBoxContent content = new RichBoxContent();
                hud.messages.Title(content, title);
                DiplomacyDisplay.FactionRelationDisplay(otherFaction, rel.Relation, content);
                Ref.update.AddSyncAction(new SyncAction1Arg<RichBoxContent>(hud.messages.Add, content));
            }
        }

        //public void loadedAndReady()
        //{ }

        //override public void Update()
        //{
        //    if (tutorial != null)
        //    {
        //        tutorial.update();
        //    }
        //}

        public void userUpdate()
        {

            if (tutorial != null)
            {
                tutorial.update();
            }

            bool menuFocusState = mapControls.focusedObjectMenuState();
           
                
            hud.update();

                
            mapControls.update(hud.mouseOver);

            if (input.AutomationSetting.DownEvent)
            {
                hud.OpenAutomationMenu();
            }                

            

            if (armyControls != null)
            {
                armyControls.update();
            }
            else
            {
                updateMapShortCuts();
            }

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    battleLineUpTest(true);
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    var tile = DssRef.world.tileGrid.Get(mapControls.tilePosition);
                    Debug.Log(tile.ToString() );
                }
            }

            if (input.inputSource.IsController)
            {

                bool friendlyHoverObj = mapControls.hover.obj != null && mapControls.hover.obj.GetFaction() == faction;
                    if (!menuFocusState && 
                        !hud.menuFocus &&
                        (input.Select.DownEvent || (friendlyHoverObj && input.ControllerFocus.DownEvent)))    
                    {
                        if (armyControls != null && 
                            (mapControls.hover.obj == null || mapControls.armyMayAttackHoverObj()))
                        {
                            mapExecute();
                        }
                        else
                        {
                            mapSelect();
                        }
                    }

                    if (input.ControllerMessageClick.DownEvent)
                    {
                        hud.messages.onControllerClick();
                    }

                    if (inputConnected && !input.Connected)
                    {
                        DssRef.state.menuSystem.controllerLost();
                    }
                    inputConnected = input.Connected;
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

                if (input.ControllerCancel.DownEvent && InBuildOrdersMode())
                {
                    buildControls.buildMode = SelectTileResult.None;
                }
            

                updateGameSpeed();
            

            updateObjectTabbing();

            

            //DssRef.state.detailMap.PlayerUpdate(mapControls.playerPointerPos, bUnitDetailLayer);
            drawUnitsView.Update();
            playerData.view.Camera.RecalculateMatrices();
            updateMapOverlays();

            cityBorders.update(this);
        }

        void setBuildMode(City city, BuildAndExpandType type)
        {
            mapSelect(city);
            cityTab = MenuTab.Build;
            if (type != BuildAndExpandType.NUM_NONE)
            {
                buildControls.buildMode = SelectTileResult.Build;
                buildControls.placeBuildingType = type;
            }
        }

        void updateMapShortCuts()
        {
            if (drawUnitsView.current.DrawDetailLayer)
            {
                if (input.Build.DownEvent)
                {
                    var order = orders.orderOnSubTile(mapControls.hover.subTile.subTilePos) as BuildOrder;
                    if ( order != null)
                    {
                        setBuildMode(mapControls.hover.subTile.city, order.buildingType);
                        return;
                    }                    

                    var build = BuildLib.BuildTypeFromTerrain(mapControls.hover.subTile.subTile.mainTerrain, mapControls.hover.subTile.subTile.subTerrain);
                    setBuildMode(mapControls.hover.subTile.city, build);
                    return;
                }

                bool inHotkeyRepeceptiveMenu = mapControls.selection.obj != null &&
                    mapControls.selection.obj.gameobjectType() == GameObjectType.City &&
                    (cityTab == MenuTab.Delivery || cityTab == MenuTab.Conscript);

                if (!inHotkeyRepeceptiveMenu)
                {
                    switch (mapControls.hover.subTile.subTile.mainTerrain)
                    {
                        case TerrainMainType.Building:

                            switch ((TerrainBuildingType)mapControls.hover.subTile.subTile.subTerrain)
                            {
                                case TerrainBuildingType.Recruitment:
                                case TerrainBuildingType.Postal:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.copyDelivery(this, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.pasteDelivery(this, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.Stop.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.deliveryIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        bool start = mapControls.hover.subTile.city.toggleDeliveryStop(ix);
                                        (start? SoundLib.start : SoundLib.stop).Play();
                                    }
                                    break;

                                case TerrainBuildingType.Nobelhouse:
                                case TerrainBuildingType.SoldierBarracks:
                                    if (input.Copy.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.copyConscript(this, ix);
                                        SoundLib.copy.Play();
                                    }
                                    if (input.Paste.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        mapControls.hover.subTile.city.pasteConscript(this, ix);
                                        SoundLib.paste.Play();
                                    }
                                    if (input.Stop.DownEvent)
                                    {
                                        int ix = mapControls.hover.subTile.city.conscriptIxFromSubTile(mapControls.hover.subTile.subTilePos);
                                        bool start = mapControls.hover.subTile.city.toggleConscriptStop(ix);
                                        (start ? SoundLib.start : SoundLib.stop).Play();
                                        hud.needRefresh = true;
                                    }
                                    break;
                            }
                            break;
                    }
                }
                //            //    case TerrainBuildingType.Recruitment:
                //            //        build = BuildAndExpandType.Recruitment;
                //            //        break;
                //            //    case TerrainBuildingType.PigPen:
                //            //        build = BuildAndExpandType.PigPen;
                //            //        break;
                //            //    case TerrainBuildingType.HenPen:
                //            //        build = BuildAndExpandType.HenPen;
                //            //        break;
                //            //    case TerrainBuildingType.Barracks:
                //            //        build = BuildAndExpandType.Barracks;
                //            //        break;
                //            //    case TerrainBuildingType.Brewery:
                //            //        build = BuildAndExpandType.Brewery;
                //            //        break;
                //            //    case TerrainBuildingType.Carpenter:
                //            //        build = BuildAndExpandType.Carpenter;
                //            //        break;
                //            //    case TerrainBuildingType.Nobelhouse:
                //            //        build = BuildAndExpandType.Nobelhouse;
                //            //        break;
                //            //    case TerrainBuildingType.Storehouse:
                //            //        build = BuildAndExpandType.Storehouse;
                //            //        break;
                //            //    case TerrainBuildingType.Tavern:
                //            //        build = BuildAndExpandType.Tavern;
                //            //        break;
                //            //    case TerrainBuildingType.WorkerHut:
                //            //        build = BuildAndExpandType.WorkerHuts;
                //            //        break;
                //            //    case TerrainBuildingType.Work_Bench:
                //            //        build = BuildAndExpandType.WorkBench;
                //            //        break;

                //            //}

                //            setBuildMode(mapControls.hover.subTile.city, build);
                //        }
                //}

            }

            if (mapControls.selection.obj != null && 
                mapControls.selection.obj.gameobjectType() == GameObjectType.City)
            {
                var city = mapControls.selection.obj.GetCity();
                switch (cityTab)
                {
                    case MenuTab.Delivery:
                        if (input.Stop.DownEvent)
                        {
                            bool start = city.toggleDeliveryStop(city.selectedDelivery);
                            hud.needRefresh = true;
                            (start ? SoundLib.start : SoundLib.stop).Play();
                        }
                        if (input.Copy.DownEvent)
                        {
                            city.copyDelivery(this);
                            SoundLib.copy.Play();
                        }
                        if (input.Paste.DownEvent)
                        {
                            city.pasteDelivery(this);
                            SoundLib.paste.Play();
                            hud.needRefresh = true;
                        }
                        break;
                    case MenuTab.Conscript:
                        if (input.Stop.DownEvent)
                        {
                            bool start = city.toggleConscriptStop(city.selectedConscript);
                            hud.needRefresh = true;
                            (start ? SoundLib.start : SoundLib.stop).Play();
                        }
                        if (input.Copy.DownEvent)
                        {
                            city.copyConscript(this);
                            SoundLib.copy.Play();
                        }
                        if (input.Paste.DownEvent)
                        {
                            city.pasteConscript(this);
                            SoundLib.paste.Play();
                            hud.needRefresh = true;
                        }
                        break;
                }
            }
        }

        public void debugMenu(GuiLayout layout)
        {
            new GuiTextButton("Next event", "skip forward in the event timer", new GuiAction(new Action(DssRef.state.events.TestNextEvent) + DssRef.state.menuSystem.closeMenu), false, layout);
            new GuiTextButton("1000 resources", "add 1000 of all resources to all cities", new GuiAction(new Action(debugAddResources) + DssRef.state.menuSystem.closeMenu), false, layout);

            //UnitType[] unitTypes = DssLib.AvailableUnitTypes;
            //foreach (var type in unitTypes)
            //{ 
            //    new GuiTextButton("Battle test - " + type.ToString() + " (Land)", null, 
            //        new GuiAction2Arg<UnitType, bool>(battleLineTest,type,false), false, layout);
            //}

            //foreach (var type in unitTypes)
            //{
            //    new GuiTextButton("Battle test - " + type.ToString() + " (Sea)", null,
            //        new GuiAction2Arg<UnitType, bool>(battleLineTest, type, true), false, layout);
            //}
        }

        void debugAddResources()
        {
            foreach (var c in DssRef.world.cities)
            {
                foreach (var type in City.MovableCityResourceTypes)
                {
                    var res = c.GetGroupedResource(type);
                    res.amount += 1000;
                    c.SetGroupedResource(type, res);
                }
            } 
        }

        //void battleLineTest(UnitType type, bool sea)
        //{
        //    DssRef.state.menuSystem.closeMenu();

        //    Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(45));
        //    Rotation1D playerRot = enemyRot.getInvert();

        //    Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
        //    DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            
        //    IntVector2 position = mapControls.tilePosition;

            
        //    {
        //        var army = faction.NewArmy(position);
        //        army.rotation = playerRot;
                
        //        for (int i = 0; i < 5; ++i)
        //        {
        //           var group =  new SoldierGroup(army, UnitType.Soldier, false);
        //            if (sea)
        //            { 
        //                group.completeTransform(SoldierTransformType.ToShip);
        //            }
        //        }

        //        army.refreshPositions(true);
        //    }
        //    {
                
        //        var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
        //        army.rotation = enemyRot;
        //        int count = type == UnitType.Ballista ? 10 : 5;

        //        for (int i = 0; i < count; ++i)
        //        {
        //            var group = new SoldierGroup(army, type, false);
        //            if (sea)
        //            {
        //                group.completeTransform(SoldierTransformType.ToShip);
        //            }
        //        }
                    
        //        army.refreshPositions(true);               

        //    }
        //}

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
                if (Input.Keyboard.Shift)
                {
                    tabCity--;
                    if (tabCity < 0)
                    {
                        tabCity = faction.cities.Count-1;
                    }
                }
                else
                {
                    tabCity++;
                    if (tabCity >= faction.cities.Count)
                    {
                        tabCity = 0;
                    }
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
                if (Input.Keyboard.Shift)
                {
                    if (tabArmy.Prev_Rollover())
                    {
                        mapControls.cameraFocus = tabArmy.sel;
                        mapSelect(tabArmy.sel);

                        return;
                    }
                }
                else
                {
                    if (tabArmy.Next_Rollover())
                    {
                        mapControls.cameraFocus = tabArmy.sel;
                        mapSelect(tabArmy.sel);

                        return;
                    }
                }
            }

            //BATTLE
            if (input.NextBattle.DownEvent)
            {
                if (Input.Keyboard.Shift)
                {
                    if (tabBattle.Prev_Rollover())
                    {
                        mapControls.cameraFocus = tabBattle.sel;
                        return;
                    }
                }
                else
                {
                    if (tabBattle.Next_Rollover())
                    {
                        mapControls.cameraFocus = tabBattle.sel;
                        return;
                    }
                }
            }
        }

        void updateMapOverlays()
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

            if (drawUnitsView.current.DrawNormal)
            {
                if (cityTagMap == null)
                { 
                    cityTagMap = new CityTagMap(this);
                }
                cityTagMap.update();
            }
            else
            {
                if (cityTagMap != null)
                { 
                    cityTagMap.DeleteMe();
                    cityTagMap = null;
                }
            }
        }

        void updateGameSpeed()
        {
           
            if (DssRef.difficulty.setting_allowPauseCommand && 
                input.PauseGame.DownEvent && 
                DssRef.state.localPlayers.Count == 1)//IsLocalHost())
            {
                DssRef.state.pauseAction();
            }

            if (DssRef.state.IsSinglePlayer() && input.GameSpeed.DownEvent)
            {
                if (Ref.isPaused)
                {
                    //Ref.isPaused = false;
                    //Ref.GameTimeSpeed = 1f;
                    Ref.SetPause(false);
                }
                else
                {
                    for (int i = 0; i < GameSpeedOptions.Length; i++)
                    {
                        if (GameSpeedOptions[i] == Ref.GameTimeSpeed)
                        {
                            int next = Bound.SetRollover(i + 1, 0, GameSpeedOptions.Length - 1);
                            Ref.SetGameSpeed( GameSpeedOptions[next]);
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

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            

            IntVector2 position = mapControls.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = faction.NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;

                SoldierConscriptProfile SoldierProfile1 = new SoldierConscriptProfile()
                {
                    conscript = new ConscriptProfile()
                    {
                        weapon = Resource.ItemResourceType.HandCulverin,
                        armorLevel = Resource.ItemResourceType.IronArmor,
                        training = TrainingLevel.Basic,
                        specialization = SpecializationType.Traditional,
                    }
                };

                for (int i = 0; i < 2; ++i)
                {
                    new SoldierGroup(army, SoldierProfile1);
                }
                army.refreshPositions(true);
            }
            //else
            {
                
                var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                enemyArmy = army;
                army.rotation = enemyRot;

                SoldierConscriptProfile SoldierProfile1 = new SoldierConscriptProfile()
                {
                    conscript = new ConscriptProfile()
                    {
                        weapon = Resource.ItemResourceType.Sword,
                        armorLevel = Resource.ItemResourceType.NONE,
                        training = TrainingLevel.Basic,
                        specialization = SpecializationType.Traditional,
                    }
                };
                for (int i = 0; i < 2; ++i)
                {
                    new SoldierGroup(army, SoldierProfile1);
                }

                army.refreshPositions(true);
            }

            friendlyArmy.Order_Attack(enemyArmy);

        }

        void mapSelect()
        {
            if (mapControls.hover.subTile.hasSelection && InBuildOrdersMode())
            {
                buildControls.onTileSelect(mapControls.hover.subTile);
            }
            else
            {
                bool sameMapObject = mapControls.selection.obj != null;
                if (mapControls.hover.subTile.hasSelection)
                {
                    sameMapObject &= mapControls.selection.obj == mapControls.hover.subTile.city;
                }
                else
                {
                    sameMapObject &= mapControls.hover.obj == mapControls.selection.obj;
                }
                bool oldselection = clearSelection();

                bool newselection = clickHover(sameMapObject);

                if (newselection && input.inputSource.IsController)
                {
                    if (input.ControllerFocus.DownEvent || mapControls.focusedObjectMenuState())
                    {
                        mapControls.setObjectMenuFocus(true);
                    }
                }


                if (oldselection && !newselection)
                {
                    SoundLib.back.Play();
                } 
            }
        }

        void mapSelect(AbsWorldObject mapObject)
        {
            bool sameMapObject = mapControls.selection.obj != null && mapObject == mapControls.selection.obj;
            clearSelection();

            mapControls.hover.obj = mapObject;
            clickHover(sameMapObject);

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
            armyControls?.asynchUpdate();

            return false;
        }

        bool clickHover(bool sameMapObject)
        {
            if (mapControls.hover.subTile.hasSelection)//.selectable(faction, out var city))
            {

                SoundLib.click.Play();

                mapControls.onTileSelect(mapControls.hover.subTile, sameMapObject);

                return true;
            }

            if (mapControls.hover.obj != null &&
                mapControls.hover.obj.GetFaction() == this.faction)
            {
                SoundLib.click.Play();
                mapControls.onSelect();

                switch (mapControls.selection.obj.gameobjectType())
                {
                    case GameObjectType.Army:
                        SoundLib.select_army.Play();
                        {
                            armyControls = new ArmyControls(this, new List<AbsMapObject> { mapControls.selection.obj.GetArmy() });
                        }
                        break;
                    case GameObjectType.City:
                        SoundLib.select_city.Play();
                        break;

                    //case GameObjectType.Faction:
                    //    SoundLib.select_faction.Play();
                    //    break;
                }

                return true;
            }

            

            return false;
        }

        public override void onGameStart(bool newGame)
        {
            base.onGameStart(newGame);
            hud.messages.onGameStart();
            oneSecUpdate();

            if (newGame)
            {
                commandPoints.value = commandPoints.max * 0.5;
                diplomaticPoints.value = diplomaticPoints.max * 0.6;
            }

            if (DssRef.difficulty.resourcesStartHelp)
            {
                Task.Factory.StartNew(() =>
                {
                    var citiesC = faction.cities.counter();
                    while (citiesC.Next())
                    {
                        citiesC.sel.checkPlayerFuelAccess_OnGamestart_async();
                    }
                });
            }

            if (newGame)
            { 
                faction.mainCity.tagBack = CityTagBack.Carton;
                faction.mainCity.tagArt = CityTagArt.IconFaction;
            }
        }

        public double diplomacyAddPerSec()
        {
            return DssRef.diplomacy.DefaultDiplomacyPerSecond + DssRef.diplomacy.NobelHouseAddDiplomacy * faction.nobelHouseCount;
        }

        public double diplomacyAddPerSec_CapIncluded()
        {
            if (diplomaticPoints.value < diplomaticPoints_softMax)
            {
                return diplomacyAddPerSec();
            }
            else
            {
                return DssRef.diplomacy.AddDiplomacy_AfterSoftlock_PerSecond;
            }
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            double max = DssRef.diplomacy.DefaultMaxDiplomacy + DssRef.diplomacy.NobelHouseAddMaxDiplomacy * faction.nobelHouseCount;
            diplomaticPoints_softMax = (int)Math.Floor(max);
            diplomaticPoints.setMax(max + DssRef.diplomacy.Diplomacy_HardMax_Add);

            if (diplomaticPoints.value < diplomaticPoints_softMax)
            {
                diplomaticPoints.add(diplomacyAddPerSec(), diplomaticPoints_softMax);
            }
            else
            {
                diplomaticPoints.add(DssRef.diplomacy.AddDiplomacy_AfterSoftlock_PerSecond);
            }

            if (mercenaryMarket.value < MercenaryMarketSoftLock1)
            {
                mercenaryMarket.value += MercenaryMarketAddPerSec_Speed1;
            }
            else 
            {
                mercenaryMarket.value += MercenaryMarketAddPerSec_Speed2;
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

        public override void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType farm, out bool intelligent)
        {
            intelligent = true;
            city.AutoExpandType(out work, out farm);
        }
        public bool CityTagsOnMapProperty(int index, bool set, bool value)
        {
            if (set)
            {
                viewCityTagsOnMap = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return viewCityTagsOnMap;
        }

        public bool ArmyTagsOnMapProperty(int index, bool set, bool value)
        {
            if (set)
            {
                viewArmyTagsOnMap = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return viewArmyTagsOnMap;
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
