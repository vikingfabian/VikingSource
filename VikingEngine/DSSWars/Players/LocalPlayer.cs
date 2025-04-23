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
using VikingEngine.Input;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer : AbsHumanPlayer
    {
        
        public Engine.PlayerData playerData;

        public GameHud hud;
        
        bool inputConnected;

        public GameControls gameControls;
        

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
       

        //public int mercenaryCost = DssRef.difficulty.MercenaryPurchaseCost_Start;

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
        public BuildCategoryTab buildCategoryTab = 0;

        public DeliveryStatus menDeliveryCopy, itemDeliveryCopy, goldDeliveryCopy;
        public ConscriptProfile soldierConscriptCopy, archerConscriptCopy, warmashineConscriptCopy, knightConscriptCopy, gunConscriptCopy, cannonConscriptCopy;

        public PlayerControls.Tutorial tutorial = null;
        CityBorders cityBorders = new CityBorders();
        public bool viewCityTagsOnMap = true;
        public bool viewArmyTagsOnMap = true;
        
        public int nextDominationSize;
        public int dominationEvents = 0;

        static readonly Vector3 ThemeNorth_Blue = new Vector3(0f, 0f, 0.3f);
        static readonly Vector3 ThemeMid_Yellow = new Vector3(0.15f, 0.15f, 0);
        static readonly Vector3 ThemeSouth_Red = new Vector3(0.2f, 0.05f, 0f);

        public Vector3 ShaderThemeColor = ThemeMid_Yellow;
        public float opposingSizePerc = 0;

        SpottedArray<LocationPin> pins = new SpottedArray<LocationPin>();

        public LocalPlayer(Faction faction)
           : base(faction)
        {
            faction.addGold_factionWide( DssRef.difficulty.PlayerBonusGold);
            orders = new Orders.Orders();

            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;

            automation = new Automation(this);
            

            faction.technology = new XP.TechnologyTemplate();
            faction.technology.iron = XP.TechnologyTemplate.FactionUnlock;

            faction.addGold_factionWide(10000);
        }

        public void assignPlayer(int playerindex, int numPlayers, bool newGame)
        {
            var pStorage = DssRef.storage.localPlayers[playerindex];
            faction.SetProfile(DssRef.storage.flagStorage.flagDesigns[pStorage.flagDesignIndex]);
            faction.diplomaticSide = DiplomaticSide.Light;

            InputMap input = new InputMap(playerindex);
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


            if (Ref.netSession.HasInternet)
            {
                var peer = Ref.netSession.LocalPeer();
                if (peer != null)
                {
                    networkPeer = new Network.NetworkInstancePeer(peer,playerindex);
                }
            }
            

            faction.profile.gameStartInit();
            faction.displayInFullOverview = true;

            playerData = Engine.XGuide.GetPlayer(playerindex);
            playerData.Tag = this;
            playerData.view.SetDrawArea(numPlayers, pStorage.screenIndex, false, null);

            new GameControls(this, input);

            new GameHud(this, numPlayers);

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

            w.Write(int.MinValue);//none

            tutorial_writeGameState(w);

            orders.writeGameState(w);

            w.Write(viewCityTagsOnMap);
            w.Write(viewArmyTagsOnMap);

            w.Write((ushort)nextDominationSize);
            w.Write((ushort)dominationEvents);


            w.Write((ushort)pins.Count);
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.writeGameState(w);
            }

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

            var  none1 = r.ReadInt32();
            
            tutorial_readGameState(r, subversion);

            orders.readGameState(r, subversion, pointers);

            viewCityTagsOnMap = r.ReadBoolean();
            viewArmyTagsOnMap = r.ReadBoolean();
                        
            nextDominationSize = r.ReadUInt16();
            dominationEvents = r.ReadUInt16();

            if (subversion > 53)
            { 
                int pinsCount = r.ReadUInt16();
                for (int i = 0; i < pinsCount; ++i)
                {
                    LocationPin pin = new LocationPin(this, r, subversion);
                    pin.parentArrayIndex = pins.Add(pin);
                    pin.basicInit();
                }
            }

            Debug.ReadCheck(r);
        }

        public void InitTutorial(bool newGame)
        {
            if (newGame && DssRef.storage.runTutorial_1short_2normal != 0)
            {
                tutorial = new PlayerControls.Tutorial(this);
            }
            
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
                tutorial.writeGameState(w);
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
                    tutorial.readGameState(r, subversion);
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

        public void createPin()
        {
#if DEBUG
            LocationPin pin = new LocationPin(this,gameControls.mapControls.mousePosition);
            pin.parentArrayIndex = pins.Add(pin);
            pin.basicInit();
#endif
        }
        public void clearPins()
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.DeleteMe(DeleteReason.Disband, false);
            }

            pins.Clear();
        }

        public LocationPin getPin(string name)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                if (pinsC.sel.Name(out _) == name)
                {
                    return pinsC.sel;
                }
            }

            return null;
        }
        public void deletePin(int index)
        {
            var pin = pins.PullIndex_Safe(index);
            pin?.DeleteMe(DeleteReason.Disband, false);            
        }

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
                    //float opposingSizePerc;

                    opposingSizePerc = opposingSize / faction.citiesEconomy.tax(null);

                    toPeaceful = opposingSizePerc <= DssRef.difficulty.toPeacefulPercentage;
                }
                else
                {
                    opposingSizePerc = 0;
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

            //if (tutorial != null)
            //{
            //    tutorial.update();
            //}

            gameControls.update();

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    //hud.messages.Add(new RichBoxContent() { new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("message test") }, null) });
                    battleLineUpTest(true);
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    var tile = DssRef.world.tileGrid.Get(gameControls.mapControls.tilePosition);
                    Debug.Log(tile.ToString());
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.N) && !Input.Keyboard.Ctrl)
                {
                    AbsWorldObject obj = gameControls.mapControls.hover.obj as AbsWorldObject;
                    obj?.AddDebugTag();
                }
            }

            drawUnitsView.Update();
            playerData.view.Camera.RecalculateMatrices();
            

            if (cityUpdate)
            {
                updateMapOverlays();
                cityBorders.update(this);
            }

            //if (Ref.peRnd.Chance(0.1))
            //{
                var pinsC = pins.counter();
                while (pinsC.Next())
                {
                    pinsC.sel.update();
                }
            //}

#if DEBUG
            if (Input.Keyboard.Ctrl && Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                RichBoxContent c = new RichBoxContent();
                c.text(gameControls.mapControls.tilePosition.ToString());
                hud.messages.Add(c);
            }
#endif 
        }

        public LocationPin rayCollisionWithPin(Ray ray)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                if (pinsC.sel.rayCollision(ray))
                { 
                    return pinsC.sel;
                }
            }

            return null;
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

            float z = gameControls.mapControls.camera.LookTarget.Z / DssRef.world.Size.Y;
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
           

            IntVector2 position = gameControls.mapControls.tilePosition;

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
                            weapon = Resource.ItemResourceType.HandSpear,
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
                            weapon = Resource.ItemResourceType.ShortSword,
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
                            weapon = Resource.ItemResourceType.HandCannon,
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
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.ManCannonBronze,
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
                            weapon = Resource.ItemResourceType.ShortSword,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 16; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.LongBow,
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
                            weapon = Resource.ItemResourceType.Pike,
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
                            weapon = Resource.ItemResourceType.KnightsLance,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 2; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.TwoHandSword,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 5; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.RoseWarrior_tank,
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

                army.refreshPositions(true);
                army.setAsStartArmy();
            }

            //friendlyArmy.Order_Attack(enemyArmy);

        }

        
        

        public void asyncPlayerPathUpdate(float time)
        {
            gameControls.armyControls?.asynchUpdate();

            //return false;
        }

        public void asynchCullingUpdate(float time, bool bStateA)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.asynchCullingUpdate(time, bStateA);
            }
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
                if (faction.mainCity != null)
                {
                    faction.mainCity.tagBack = CityTagBack.Carton;
                    faction.mainCity.tagArt = CityTagArt.IconFaction;
                }
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
                faction.addGold_factionWide(1000);
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
