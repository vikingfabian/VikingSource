﻿using Microsoft.Xna.Framework;
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
//using VikingEngine.DSSWars.Battle;
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
using VikingEngine.DSSWars.Work;
using System.Net.Http.Headers;
using System.Drawing;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.DSSWars.Players.PlayerControls;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer : AbsHumanPlayer
    {
        public const int MaxSpeedOption = 5;
        public Engine.PlayerData playerData;

        public GameHud hud;
        public InputMap input;
        bool inputConnected;
        
        public MapControls mapControls;
        public ArmyControls armyControls = null;
        public SoldierControls soldierControls = null;

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

        //public SpottedArray<Battle.BattleGroup> battles = new SpottedArray<Battle.BattleGroup>(4);

        int tabCity = -1;
        SpottedArrayCounter<Army> tabArmy;
        //SpottedArrayCounter<BattleGroup> tabBattle;

        public int mercenaryCost = DssRef.difficulty.MercenaryPurchaseCost_Start;

        const int MercenaryMarketSoftLock1 = DssLib.MercenaryPurchaseCount * 5;
        const double MercenaryMarketAddPerSec_Speed1 = 0.5;
        const double MercenaryMarketAddPerSec_Speed2 = 0.3;
        public FloatingInt mercenaryMarket = new FloatingInt() { value = DssLib.MercenaryPurchaseCount * 2 };

        public MenuTab factionTab = MenuTab.NUM_NONE;//HeadDisplay.Tabs[0];
        public MenuTab cityTab = CityMenu.Tabs[0];
        public MenuTab armyTab = ArmyMenu.Tabs[0];
        public ResourcesSubTab resourcesSubTab = ResourcesSubTab.Overview_Resources;
        //public WorkSubTab workSubTab = WorkSubTab.Priority_Resources;
        public ProgressSubTab progressSubTab = 0;
        public MixTabEditType mixTabEditType = MixTabEditType.None;
        public WorkPriorityType mixWorkType = WorkPriorityType.NUM_NONE;
        public ItemResourceType mixTabItem = ItemResourceType.NONE;

        public DeliveryStatus menDeliveryCopy, itemDeliveryCopy, goldDeliveryCopy;
        public ConscriptProfile soldierConscriptCopy, archerConscriptCopy, warmashineConscriptCopy, knightConscriptCopy, gunConscriptCopy, cannonConscriptCopy;

        public PlayerControls.Tutorial tutorial = null;
        CityBorders cityBorders = new CityBorders();
        public bool viewCityTagsOnMap = true;
        public bool viewArmyTagsOnMap = true;
        public int[] GameSpeedOptions;

        public int nextDominationSize;
        public int dominationEvents = 0;

        static readonly Vector3 ThemeNorth_Blue = new Vector3(0f, 0f, 0.3f);
        static readonly Vector3 ThemeMid_Yellow = new Vector3(0.15f, 0.15f, 0);
        static readonly Vector3 ThemeSouth_Red = new Vector3(0.2f, 0.05f, 0f);

        public Vector3 ShaderThemeColor = ThemeMid_Yellow;

        public LocalPlayer(Faction faction)
           : base(faction)
        {
            faction.addMoney_factionWide( DssRef.difficulty.PlayerBonusGold);
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

            faction.addMoney_factionWide(10000);
        }

        
        

        

        public void assignPlayer(int playerindex, int numPlayers, bool newGame)
        {
            var pStorage = DssRef.storage.localPlayers[playerindex];
            faction.SetProfile(DssRef.storage.flagStorage.flagDesigns[pStorage.flagDesignIndex]);
            faction.diplomaticSide = DiplomaticSide.Light;

            tabArmy = faction.armies.counter();
            //tabBattle = new SpottedArrayCounter<BattleGroup>(battles);

            input = new InputMap(playerindex);
            if (Ref.netSession.HasInternet)
            {
                var peer = Ref.netSession.LocalPeer();
                if (peer != null)
                {
                    networkPeer = new Network.NetworkInstancePeer(peer,playerindex);
                }
            }
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

            playerData = Engine.XGuide.GetPlayer(playerindex);
            playerData.Tag = this;
            playerData.view.SetDrawArea(numPlayers, pStorage.screenIndex, false, null);


            new GameHud(this, numPlayers);
            buildControls = new Build.BuildControls(this);


            
            mapControls = new Players.MapControls(this);
            if (faction.mainCity != null)
            {
                mapControls.setCameraPos(faction.mainCity.tilePos);
            }
            Ref.draw.AddPlayerScreen(playerData);
            drawUnitsView = new MapDetailLayerManager(playerData);
            InitTutorial(newGame);

            //new AsynchUpdateable(interactAsynchUpdate, "DSS player interact", playerindex);

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
                        //foreach (var type in City.MovableCityResourceTypes)
                        //{
                        //    var res = c.GetGroupedResource(type);
                        //    res.amount += 1000;
                        //    c.SetGroupedResource(type, res);
                        //}
                    }
                }
            }

            menDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Men);

            itemDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Resource);
            
            goldDeliveryCopy = new DeliveryStatus();
            goldDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Gold);

            soldierConscriptCopy = new ConscriptProfile();
            soldierConscriptCopy.defaultSetup(Build.BuildAndExpandType.SoldierBarracks);

            archerConscriptCopy = new ConscriptProfile();
            archerConscriptCopy.defaultSetup(Build.BuildAndExpandType.ArcherBarracks);

            warmashineConscriptCopy = new ConscriptProfile();
            warmashineConscriptCopy.defaultSetup(Build.BuildAndExpandType.WarmashineBarracks);

            knightConscriptCopy = new ConscriptProfile();
            knightConscriptCopy.defaultSetup(Build.BuildAndExpandType.KnightsBarracks);

            gunConscriptCopy = new ConscriptProfile();
            gunConscriptCopy.defaultSetup(Build.BuildAndExpandType.GunBarracks);

            cannonConscriptCopy = new ConscriptProfile();
            cannonConscriptCopy.defaultSetup(Build.BuildAndExpandType.CannonBarracks);

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

        public void NetUpdate()
        {
            if (Ref.netSession.IsClient)
            {
                var w = Ref.netSession.BeginWritingPacketToHost(Network.PacketType.DssPlayerStatus, Network.PacketReliability.Unrelyable, playerData.localPlayerIndex);
                DssRef.state.culling.players[playerData.localPlayerIndex].GetState().writeNet(w);
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

            w.Write((ushort)nextDominationSize);
            w.Write((ushort)dominationEvents);
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

            mercenaryCost = r.ReadInt32();
            
            tutorial_readGameState(r, subversion);

            orders.readGameState(r, subversion, pointers);

            viewCityTagsOnMap = r.ReadBoolean();
            viewArmyTagsOnMap = r.ReadBoolean();

            if (subversion >= 45)
            {
                nextDominationSize = r.ReadUInt16();
                dominationEvents = r.ReadUInt16();
            }

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

        

        //public RbAction WorkSafeguardTooltip;
        

       

       

        public override void createStartUnits()
        {
            IntVector2 onTile = DssRef.world.GetFreeTile(faction.mainCity.tilePos);

            var mainArmy = faction.NewArmy(onTile);
            mainArmy.tagBack = CityTagBack.Blue;
            mainArmy.tagArt = ArmyTagArt.Specialize_Tradition;

            for (int i = 0; i < 5; ++i)
            {
                new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);
            }

            if (IsLocalPlayer() && DssRef.difficulty.honorGuard)
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
                            new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard, army.position);
                            --guardCount;
                        }
                        //army.OnSoldierPurchaseCompleted();
                        army.setAsStartArmy();
                        if (guardCount <= 3)
                        {
                            break;
                        }
                    }
                }

                for (int i = 0; i < guardCount; ++i)
                {
                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_HonorGuard, mainArmy.position);
                }
            }
            //mainArmy.OnSoldierPurchaseCompleted();
            mainArmy.setAsStartArmy();
        }

        public void toPeacefulCheck_asynch()
        {
            if (faction.citiesEconomy.tax(null) > 0 && !DssRef.state.events.AiDelay())
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
                    //const int MaxTrials = 10;

                    //for (int i = 0; i < MaxTrials; ++i)
                    //{
                    //    var city = faction.cities.GetRandomUnsafe(Ref.rnd);
                    //    if (city != null)
                    //    {
                    //        foreach (var cindex in city.neighborCities)
                    //        {
                    //            var otherfaction = DssRef.world.cities[cindex].faction;
                    //            if ((otherfaction.factiontype == FactionType.DefaultAi ||  otherfaction.factiontype == FactionType.DarkFollower) &&
                    //                otherfaction.armies.Count > 0)
                    //            {
                    //                var rel = DssRef.diplomacy.GetRelationType(faction, otherfaction);
                    //                if (rel >= RelationType.RelationTypeN1_Enemies && rel <= RelationType.RelationType1_Peace)
                    //                {
                    //                    var aiPlayer = otherfaction.player.GetAiPlayer();
                    //                    if (aiPlayer.aggressionLevel <= AiPlayer.AggressionLevel1_RevengeOnly)
                    //                    {
                    //                        aiPlayer.aggressionLevel = AiPlayer.AggressionLevel2_RandomAttacks;
                    //                        aiPlayer.refreshAggression();
                    //                    }
                    //                    DssRef.diplomacy.declareWar(otherfaction, faction);
                    //                    return;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    var attacker = DssRef.state.events.findAttackingNeighborFaction(faction);
                    attacker.player.setMinimumAggression(AbsPlayer.AggressionLevel2_RandomAttacks);
                    DssRef.diplomacy.declareWar(attacker, faction);
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

        //public void enterBattle(Battle.BattleGroup battleGroup, AbsMapObject playerUnit)
        //{
        //    battles.Add(battleGroup);
        //    RichBoxContent content = new RichBoxContent();
        //    hud.messages.Title(content, DssRef.lang.Hud_Battle);

        //    var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
        //    hud.messages.ControllerInputIcons(gotoBattleButtonContent);
        //    gotoBattleButtonContent.Add(new RichBoxText(playerUnit.TypeName() + " - " + battleGroup.TypeName()));

        //    content.Add(new RichboxButton(gotoBattleButtonContent,
        //        new RbAction1Arg<Battle.BattleGroup>(goToBattle, battleGroup)));
        //    hud.messages.Add(content);
        //}

        //void goToBattle(Battle.BattleGroup battleGroup)
        //{
        //    mapControls.cameraFocus = battleGroup;
        //}

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
                otherFaction.player.IsLocalPlayer())
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
       

        public void userUpdate(bool cityUpdate)
        {

            if (tutorial != null)
            {
                tutorial.update();
            }

            bool menuFocusState = mapControls.focusedObjectMenuState();
            hud.update();

            mapControls.update(hud.mouseOver);

            if (cityUpdate && input.AutomationSetting.DownEvent)
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
                    hud.messages.Add(new RichBoxContent() { new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("message test") }, null) });
                    //battleLineUpTest(true);
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    var tile = DssRef.world.tileGrid.Get(mapControls.tilePosition);
                    Debug.Log(tile.ToString());
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.N) && !Input.Keyboard.Ctrl)
                {
                    AbsWorldObject obj = mapControls.hover.obj as AbsWorldObject;
                    obj?.AddDebugTag();
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
                    if ((mapControls.hover.subTile.hasSelection && InBuildOrdersMode()) || buildControls.buildKeyDown)
                    {
                        buildControls.updateBuildMode();
                    }
                    else
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
            }

            if (cityUpdate && input.ControllerCancel.DownEvent && InBuildOrdersMode())
            {
                buildControls.buildMode = SelectTileResult.None;
            }


            updateGameSpeed();

            updateObjectTabbing();

            drawUnitsView.Update();
            playerData.view.Camera.RecalculateMatrices();
            

            if (cityUpdate)
            {
                updateMapOverlays();
                cityBorders.update(this);
            }
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
                if (input.Build.DownEvent && mapControls.hover.subTile.city.faction == this.faction)
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

#if DEBUG
                if (VikingEngine.Input.Keyboard.KeyDownEvent(Keys.P))
                {
                    var subtile = DssRef.world.subTileGrid.Get(mapControls.hover.subTile.subTilePos);
                    subtile.SetType(TerrainMainType.Wall, (int)TerrainWallType.StoneWall, 1);
                    DssRef.world.subTileGrid.Set(mapControls.hover.subTile.subTilePos, subtile);
                }
#endif

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
            new GuiTextButton("Enemy alliance", "when the player grow to fast", new GuiAction(new Action(()=> { DssRef.state.events.collectAllianceAgainstPlayerDomination(this); }) + DssRef.state.menuSystem.closeMenu), false, layout);

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
                //foreach (var type in City.MovableCityResourceTypes)
                //{
                //    var res = c.GetGroupedResource(type);
                //    res.amount += 1000;
                //    c.SetGroupedResource(type, res);
                //}
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

            var city = faction.cities.GetRandomUnsafe(Ref.rnd);
            if (city != null && 
                city.automateCity && 
                city.automationFocus == AutomationFocus.Military)
            {
                if (buySoldiers(city, true, false))
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        buySoldiers(city, true, true);
                    }));
                }
            }

            faction.updateResourceOverview_async();

            float z = mapControls.camera.LookTarget.Z / DssRef.world.Size.Y;
            if (z < 0.5)
            {
                setThemeColor(z / 0.5f, ThemeNorth_Blue, ThemeMid_Yellow);
            }
            else
            {
                setThemeColor((z - 0.5f)/ 0.5f, ThemeMid_Yellow, ThemeSouth_Red);
            }

            void setThemeColor(float percSouth, Vector3 north, Vector3 south)
            {
                ShaderThemeColor = VectorExt.AddX( north * (1f - percSouth) + south * percSouth, DssRef.time.ShaderDayLight_RedTint);
            }
        }

        void updateObjectTabbing()
        {
            //CITY
            if (input.NextCity.DownEvent && faction.cities.Count > 0)
            {
                nextCity(!Input.Keyboard.Shift);
                //if (Input.Keyboard.Shift)
                //{
                //    tabCity--;
                //    if (tabCity < 0)
                //    {
                //        tabCity = faction.cities.Count-1;
                //    }
                //}
                //else
                //{
                //    tabCity++;
                //    if (tabCity >= faction.cities.Count)
                //    {
                //        tabCity = 0;
                //    }
                //}


                //int current = 0;
                //var citiesC = faction.cities.counter();
                //while (citiesC.Next())
                //{
                //    if (current == tabCity)
                //    { 
                //        //focus on city
                //        mapControls.cameraFocus = citiesC.sel;
                //        mapSelect(citiesC.sel);

                //        return;
                //    }
                //    current++;
                //}
            }

            //ARMY
            if (input.NextArmy.DownEvent)
            {
                nextArmy(!Input.Keyboard.Shift);
                //if (Input.Keyboard.Shift)
                //{
                //    if (tabArmy.Prev_Rollover())
                //    {
                //        mapControls.cameraFocus = tabArmy.sel;
                //        mapSelect(tabArmy.sel);

                //        return;
                //    }
                //}
                //else
                //{
                //    if (tabArmy.Next_Rollover())
                //    {
                //        mapControls.cameraFocus = tabArmy.sel;
                //        mapSelect(tabArmy.sel);

                //        return;
                //    }
                //}
            }

        }

        public void nextCity(bool forward)
        {
            if (forward)
            {
                tabCity++;
                if (tabCity >= faction.cities.Count)
                {
                    tabCity = 0;
                }
            }
            else
            {
                tabCity--;
                if (tabCity < 0)
                {
                    tabCity = faction.cities.Count - 1;
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
        public void nextArmy(bool forward)
        {
            if (forward)
            {
               if (tabArmy.Next_Rollover())
                {
                    mapControls.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
                }
            }
            else
            {
                if (tabArmy.Prev_Rollover())
                {
                    mapControls.cameraFocus = tabArmy.sel;
                    mapSelect(tabArmy.sel);

                    return;
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
                hud.headOptions.pauseAction();
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
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot =enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(faction, enemyFac);
           

            IntVector2 position = mapControls.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = faction.NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;

                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Sword,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 8; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Bow,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 4; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.HandCulverin,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 4; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.ManCannonBronze,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 2; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
                army.setAsStartArmy();
                //army.(true);
            }
            //else
            {

                var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                enemyArmy = army;
                army.rotation = enemyRot;

                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Sword,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 6; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Bow,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 4; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.HandCulverin,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 4; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.ManCannonBronze,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 2; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}

                //army.refreshPositions(true);
                army.setAsStartArmy();
            }

            friendlyArmy.Order_Attack(enemyArmy);

        }

        
        void mapSelect()
        {

            //if (mapControls.hover.subTile.hasSelection && InBuildOrdersMode())
            //{
            //    buildControls.onTileSelect(mapControls.hover.subTile);
            //}
            //else if (downEvent)
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

        public void mapSelect(AbsWorldObject mapObject)
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

            if (soldierControls != null)
            { 
                soldierControls = null;
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

            if (soldierControls != null)
            {
                soldierControls.mapExecute(this);
            }
        }

        public void asyncPlayerPathUpdate(float time)
        {
            armyControls?.asynchUpdate();

            //return false;
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

                    case GameObjectType.Soldier:
                        SoundLib.select_army.Play();
                        {
                            soldierControls = new SoldierControls(new List<SoldierGroup> { mapControls.selection.obj.GetSoldierGroup() });
                        }
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

            nextDominationSize = faction.cities.Count + DssConst.DominationSizeIncrease.GetRandom();
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

            faction.resourceOverviewOneSecondUpdate();

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
                faction.addMoney_factionWide(1000);
            }

            if (StartupSettings.EndlessDiplomacy)
            {
                diplomaticPoints.max = 10;
                diplomaticPoints.value = 10;
            }

            automation.oneSecondUpdate();
            //hud.oneSecondUpdate(this);
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

        virtual public bool updateObjectDisplay()
        {
            return false;
        }

        public override bool IsLocal => true;

        public override bool IsAi()
        {
            return false;
        }

        public override bool IsLocalPlayer()
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
