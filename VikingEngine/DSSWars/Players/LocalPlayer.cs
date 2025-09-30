using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;
using VikingEngine.ToGG.Commander.LevelSetup;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;
using VikingEngine.ToGG.HeroQuest.Net;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer : AbsHumanPlayer
    {
        
        public Engine.PlayerData playerData;

        public GameHud hud;
        
        bool inputConnected;

        public GameControls gameControls;
        

        public MapLayerManager mapLayersManager;
      
        public Rectangle2 cullingTileArea = Rectangle2.ZeroOne;
        
        public CityTagMap cityTagMap = null;

        public FloatingInt_Max commandPoints = new FloatingInt_Max();
        public FloatingInt_Max diplomaticPoints = new FloatingInt_Max();
        public int allyCount = 0;
        public int warCount = 0;
        public int diplomaticPoints_softMax;

        public Data.Statistics statistics = new Data.Statistics();

        public PlayerToPlayerDiplomacy[] toPlayerDiplomacies = null;
        public Automation automation;

        const int MercenaryMarketSoftLock1 = DssLib.MercenaryPurchaseCount * 5;
        const double MercenaryMarketAddPerSec_Speed1 = 0.5;
        const double MercenaryMarketAddPerSec_Speed2 = 0.3;
        public FloatingInt mercenaryMarket = new FloatingInt() { value = DssLib.MercenaryPurchaseCount * 2 };

        public MenuTab factionTab = MenuTab.NUM_NONE;
        public MenuTab cityTab;
        public MenuTab armyTab = ArmyMenu.Tabs[0];
        public ResourcesSubTab resourcesSubTab = ResourcesSubTab.Overview_Resources;

        public ProgressSubTab progressSubTab = 0;
        public TagSubTab tagSubTab = 0;
        public BuildAndExpandType conscriptSubTab = BuildAndExpandType.ALL;
        public ItemResourceType deliverySupTab = ItemResourceType.NUM;
        public MixTabEditType mixTabEditType = MixTabEditType.None;
        public WorkPriorityType mixWorkType = WorkPriorityType.NUM_NONE;
        public ItemResourceType mixTabItem = ItemResourceType.NONE;
        public BuildCategoryTab buildCategoryTab = 0;
        public BuildFilterTag buildFilterTag = 0;
        public XP.TechnologyTreeType selectedTech = 0;

        public DeliveryStatus menDeliveryCopy, itemDeliveryCopy, goldDeliveryCopy;
        public BarracksStatus soldierConscriptCopy, archerConscriptCopy, warmachineConscriptCopy, knightConscriptCopy, gunConscriptCopy, cannonConscriptCopy;
        public SchoolStatus schoolCopy;

        public PlayerControls.Tutorial tutorial = null;
        CityBorders cityBorders = new CityBorders();
        public bool viewCityTagsOnMap = true;
        public bool viewArmyTagsOnMap = true;
        
        public int firstAttacker = ushort.MaxValue;
        public int nextDominationSize;
        public int factionsTerminated = 0;
        public bool barbarianKiller = false;
        public bool cohalitionEvent = false;
        public bool cohalitionWarning = false;

        static readonly Vector3 ThemeNorth_Blue = new Vector3(0f, 0f, 0.3f);
        static readonly Vector3 ThemeMid_Yellow = new Vector3(0.15f, 0.15f, 0);
        static readonly Vector3 ThemeSouth_Red = new Vector3(0.2f, 0.05f, 0f);

        public Vector3 ShaderThemeColor = ThemeMid_Yellow;
        public float opposingSizePerc = 0;

        SpottedArray<LocationPin> pins = new SpottedArray<LocationPin>();

        List<MessagePosition> battleMessages = new List<MessagePosition>(8);
        public bool isDropInPlayer = false;

        public StoredCameraPos storedCameraPos;

        public LocalPlayer()
        {
            baseInit();
        }

        public void DrawDetalLayer_Mesh(int cameraIndex)
        {
            gameControls.map.hover.groupModels_detail.Draw(cameraIndex);
            gameControls.map.selection.groupModels_detail.Draw(cameraIndex);
        }
        public void DrawMidLayer_Mesh(int cameraIndex)
        {
            gameControls.map.hover.groupModels_terrian.Draw(cameraIndex);
            gameControls.map.selection.groupModels_terrian.Draw(cameraIndex);
        }


        public void setPlayerFaction(Faction faction)
        { 
            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;
        }

        void baseInit()
        { 
            orders = new Orders.Orders();
            automation = new Automation(this);
            schoolCopy = new SchoolStatus();
            schoolCopy.defaulSetup();
        }

        public LocalPlayer(Faction faction, bool newGame)
           : base(faction, newGame)
        {
            baseInit();
            faction.addGold_factionWide( DssRef.difficulty.PlayerBonusGold);

            setPlayerFaction(faction);

            faction.technology = new XP.TechnologyTemplate();
            faction.technology.iron.points = XP.TechnologyTemplate.FactionUnlock;

            faction.addGold_factionWide(10000);
        }

        public bool battleMessageCheck(IntVector2 tilepos)
        {
            for (int i = battleMessages.Count - 1; i >= 0; --i)
            {
                if (battleMessages[i].time.secPassed(20))
                {
                    battleMessages.RemoveAt(i);
                }
                else if (battleMessages[i].tilePos.SideLength(tilepos) <= 5)
                {
                    return false;
                }
            }

            battleMessages.Add(new MessagePosition(tilepos));
            return true;
        }

        public void initNetwork()
        {
            var peer = Ref.netSession.LocalPeer();
            if (peer != null)
            {
                networkPeer = new Network.NetworkInstancePeer(peer, playerData.localPlayerIndex);
            }
        }

        public void assignPlayer(int playerindex, int numPlayers, bool newGame)
        {
            var pStorage = DssRef.storage.localPlayers[playerindex];
            SetProfile(DssRef.storage.profileStorage.profiles[pStorage.profileIndex]);
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

            faction.displayInFullOverview = true;

            playerData = Engine.XGuide.GetPlayer(playerindex);
            playerData.Tag = this;
            playerData.view.SetDrawArea(numPlayers, pStorage.screenIndex, false, null);

            if (Ref.netSession.HasInternet)
            {
                initNetwork();
            }

            if (!Bound.IsWithin(playerData.view.ScreenIndex, 0, 3))
            {
                throw new Exception("Screen index error: " + playerData.view.ScreenIndex.ToString());
            }

            new GameControls(this, input);

            new GameHud(this, numPlayers);

            cityTab = AvailableCityTabs()[0];

            Ref.draw.AddPlayerScreen(playerData);
            mapLayersManager = new MapLayerManager(playerData);
            InitTutorial(newGame);

            refreshNeihgborAggression();
            if (numPlayers > 1)
            {
                toPlayerDiplomacies = new PlayerToPlayerDiplomacy[numPlayers];
            }

            menDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Men);

            itemDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Resource);
            
            goldDeliveryCopy = new DeliveryStatus();
            goldDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Gold);

            soldierConscriptCopy = new BarracksStatus(BuildAndExpandType.SoldierBarracks);

            archerConscriptCopy = new BarracksStatus(BuildAndExpandType.ArcherBarracks);

            warmachineConscriptCopy = new BarracksStatus(BuildAndExpandType.WarmachineBarracks);

            knightConscriptCopy = new BarracksStatus(BuildAndExpandType.KnightsBarracks);

            gunConscriptCopy = new BarracksStatus(BuildAndExpandType.GunBarracks);

            cannonConscriptCopy = new BarracksStatus(BuildAndExpandType.CannonBarracks);

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

            //Debug.WriteCheck(w);//TEMP!

            statistics.writeGameState(w);

            //Debug.WriteCheck(w);//TEMP!

            if (toPlayerDiplomacies == null)
            {
                w.Write(short.MinValue);
            }
            else
            {
                for (int i = 0; i < toPlayerDiplomacies.Length; ++i)//each (var tp in toPlayerDiplomacies)
                {
                    var tp = toPlayerDiplomacies[i];
                    if (tp != null)
                    {
                        w.Write((short)i);
                        tp.writeGameState(w);
                    }
                }

                w.Write(short.MinValue);
            }
            //Debug.WriteCheck(w);//TEMP!

            automation.writeGameState(w);

            //Debug.WriteCheck(w);//TEMP!

            w.Write(int.MinValue);//none

            tutorial_writeGameState(w);

            //Debug.WriteCheck(w);//TEMP!
            orders.writeGameState(w);

            w.Write(viewCityTagsOnMap);
            w.Write(viewArmyTagsOnMap);

            w.Write((ushort)firstAttacker);
            w.Write((ushort)nextDominationSize);
            w.Write(cohalitionEvent);
            w.Write(barbarianKiller);
            w.Write((ushort)factionsTerminated);


            w.Write((ushort)pins.Count);
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.writeGameState(w);
            }

            storedCameraPos.writeGameState(w);

            hud.pins.writeGameState(w);


            //gameControls.build.buildPriority.writeGameState(w, false);

            Debug.WriteCheck(w);
        }

        public override void readGameState(BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subversion, pointers);

            if (isDropInPlayer)
            {
                readAiPlayerGameState(r, subversion);
                return;
            }

            diplomaticPoints.value = r.ReadInt16();

            //Debug.ReadCheck(r);//TEMP!

            statistics.readGameState(r, subversion);


            //Debug.ReadCheck(r);//TEMP!

            if (subversion >= 59)
            {
                while (true)//if (toPlayerDiplomacies != null)
                {
                    int index = r.ReadInt16();
                    if (index >= 0)
                    {
                        PlayerToPlayerDiplomacy tp = new PlayerToPlayerDiplomacy();

                        tp.readGameState(r, subversion);
                        if (arraylib.InBound(toPlayerDiplomacies, index))
                        {
                            toPlayerDiplomacies[index] = tp;
                        }
                        //for (int i = 0; i < toPlayerDiplomacies.Length; ++i)
                        //{

                        //    if (!DssRef.state.localPlayers[i].isDropInPlayer)
                        //    {
                        //        if (r.ReadBoolean())
                        //        {
                        //            PlayerToPlayerDiplomacy tp = new PlayerToPlayerDiplomacy();

                        //            tp.readGameState(r, subversion);
                        //            toPlayerDiplomacies[i] = tp;
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        break;
                    }
                }
            }

           // Debug.ReadCheck(r);//TEMP!

            automation.readGameState(r, subversion);

           // Debug.ReadCheck(r);//TEMP!

            var  none1 = r.ReadInt32();
            
            tutorial_readGameState(r, subversion);

           // Debug.ReadCheck(r);//TEMP!
            orders.readGameState(playerData.localPlayerIndex , r, subversion, pointers);

            viewCityTagsOnMap = r.ReadBoolean();
            viewArmyTagsOnMap = r.ReadBoolean();

            if (subversion >= 73)
            { 
                firstAttacker = r.ReadUInt16();
            }
            nextDominationSize = r.ReadUInt16();
            if (subversion < 72)
            {
                r.ReadUInt16();
            }
            else
            {
                cohalitionEvent = r.ReadBoolean();
                barbarianKiller = r.ReadBoolean();
                factionsTerminated = r.ReadUInt16();
            }

            if (subversion > 53)
            { 
                int pinsCount = r.ReadUInt16();
                for (int i = 0; i < pinsCount; ++i)
                {
                    LocationPin pin = new LocationPin(this, r, subversion);
                    pin.myIndex = pins.Add(pin);
                    pin.basicInit();
                }
            }

            if (subversion >= 66)
            {
                storedCameraPos.readGameState(r, subversion);
            }
            if (subversion >= 69)
            {
                hud.pins.readGameState(r, subversion);
            }
            //if (subversion >= 70)
            //{
            //    gameControls.build.buildPriority.readGameState(r, subversion, false);
            //}
            Debug.ReadCheck(r);
        }

        public void InitTutorial(bool newGame)
        {
            if ((newGame || PlatformSettings.STEAM_DEMO) && 
                DssRef.storage.runTutorial_1short_2normal != 0 &&
                DssRef.state.PlayType() == PlayStateType.Play &&
                DssRef.difficulty.setting_gameMode != GameModeMainType.Spectator)
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
        //public void factionTabClick(int tab)
        //{
        //    factionTab = PlayerHud_Head.Tabs[tab];
        //}
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
            if (profile.casualControls)
            {
                return new List<MenuTab> { MenuTab.Info, MenuTab.Casual_Recruit, MenuTab.Casual_Build, MenuTab.Tag };
            }
            return tutorial != null ? tutorial.cityTabs : CityMenu.Tabs;
        }

        public List<MenuTab> AvailableArmyTabs()
        {
            return ArmyMenu.Tabs;
        }

        public void createPin()
        {
#if DEBUG
            LocationPin pin = new LocationPin(this,gameControls.map.pointerPosWP);
            pin.myIndex = pins.Add(pin);
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
            if (faction.cities.Count > 0)
            {
                IntVector2 onTile = faction.mainCity.ArmySpawnTilePos();
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
                            onTile = citiesC.sel.ArmySpawnTilePos();
                            var army = faction.NewArmy(onTile);
                            for (int i = 0; i < 4; ++i)
                            {
                                new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard, army.position);
                                --guardCount;
                            }

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

                mainArmy.setAsStartArmy();
            }
        }

        public void toPeacefulCheck_asynch()
        {
            if (faction.totalWorkForce > 0)
            {
                int warCount = 0;
                float opposingSize = 0;

                for (int relIx = 0; relIx < faction.diplomaticRelations.Length; ++relIx)
                {
                    if (faction.diplomaticRelations[relIx] != null &&
                        faction.diplomaticRelations[relIx].Relation <= RelationType.RelationTypeN2_Truce)
                    {
                        var opponent = faction.diplomaticRelations[relIx].opponent(faction);
                        if (opponent.player.IsBot())
                        {
                            ++warCount;
                            opposingSize += opponent.PotensialMilitaryStrength();
                        }
                    }
                }

                bool toPeaceful = true;
                int maxChecks = Ref.rnd.Int(1, 5);

                while (toPeaceful && maxChecks > 0)
                {
                    maxChecks--;

                    if (opposingSize > 0)
                    {
                        opposingSizePerc = opposingSize / faction.PotensialMilitaryStrength();

                        toPeaceful = opposingSizePerc <= DssRef.difficulty.toPeacefulPercentage;
                    }
                    else
                    {
                        opposingSizePerc = 0;
                    }

                    if (toPeaceful)
                    {
                        //start a war
                        var attacker = DssRef.state.events.findAttackingNeighborFaction(faction);

                        if (attacker == null && Ref.rnd.Chance(0.6))
                        {
                            attacker = DssRef.state.events.findAttackingNeighborFaction_keepExpanding(faction);

                            //See if can gank any of the players friendlies, since they are not neihbor to the player
                            var friend = DssRef.state.events.findFriendsToDefender(attacker, this.faction);
                            if (friend != null)
                            {
                                DssRef.diplomacy.declareWar(attacker, friend);
                            }
                        }

                        if (attacker != null)
                        {
                            opposingSize += attacker.PotensialMilitaryStrength();

                            attacker.player.setMinimumAggression(AbsPlayer.AggressionLevel2_RandomAttacks);
                            DssRef.diplomacy.declareWar(attacker, faction);
                        }

                    }
                    else
                    {
                        return;
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
                        DssRef.world.cities[n].GetPlayer().onPlayerNeighborCapture(this);
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
                    DssRef.world.cities[n].GetPlayer().onPlayerNeighborCapture(this);
                }                
            }

            //if (faction.cities.Count >= DssRef.world.cities.Count - 5)
            //{
            //    DssRef.state.events.onWorldDomination();
            //}
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

            //if (rel.Relation == RelationType.RelationType3_Ally)
            //{
            //    DssRef.achieve.onAlly(faction, otherFaction);
            //}

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
                MessageGroup_Ingame.Title(content, title);
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

#if DEBUG
            if (Input.Keyboard.Ctrl && Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                RichBoxContent c = new RichBoxContent();
                c.text(gameControls.map.tilePosition.ToString());
                hud.messages.Add(c);
            }
#endif 
            gameControls.update();

            if (PlatformSettings.DevBuild)
            {
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    //DssRef.state.events.victory(Event.VictoryType.DefeatBoss);
                    //DssRef.state.events.TestNextEvent();
                    //DssRef.state.events.TestNextEvent();
                    hud.objMenu.diplomacy?.makeServant();
                }
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    //DssRef.state.events.TestNextEvent();
                    //hud.messages.Add(new RichBoxContent() { new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("message test") }, null) });
                    battleLineUpTest2(true);
                    //DssRef.state.events.TestNextEvent();
                    //DssRef.state.events.testToPeacefulCheck();
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    //battleLineUpTest3_friendly_only();
                    battleLineUpTest2(false);

                    //var tile = DssRef.world.tileGrid.Get(gameControls.mapControls.tilePosition);
                    //Debug.Log(tile.ToString());
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.N) && !Input.Keyboard.Ctrl)
                {
                    AbsWorldObject obj = gameControls.map.hover.obj as AbsWorldObject;
                    obj?.AddDebugTag();
                }
            }

            mapLayersManager.Update();
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
            //new GuiTextButton("Enemy alliance", "when the player grow to fast", new GuiAction(new Action(()=> { DssRef.state.events.collectAllianceAgainstPlayerDomination(this); }) + DssRef.state.menuSystem.closeMenu), false, layout);

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
            gameControls.diplomacy?.asynchUpdate();

            automation.asyncUpdate();

            var city = faction.cities.GetRandomUnsafe(Ref.rnd);
            if (city != null && 
                city.automateCity && 
                city.automationFocus == AutomationFocus.Military)
            {
                if (buySoldiers(city, false, false, false))
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        buySoldiers(city, false, false, true);
                    }));
                }
            }

            faction.updateResourceOverview_async();

            float z = gameControls.map.camera.LookTarget.Z / DssRef.world.Size.Y;
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
            
            if (mapLayersManager.current.DrawFar)
            {
                if (gameControls.diplomacy == null)
                {
                    gameControls.diplomacy = new DiplomacyMap(this);
                }

                gameControls.diplomacy.update();
            }
            else
            {
                if (gameControls.diplomacy != null)
                {
                    gameControls.diplomacy.DeleteMe();
                    gameControls.diplomacy = null;
                }
            }

            if (mapLayersManager.current.DrawMid)
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
        void battleLineUpTest3_friendly_only()
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(faction, enemyFac);


            IntVector2 position = gameControls.map.tilePosition;

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

                    for (int i = 0; i < 64; ++i)
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
                            weapon = Resource.ItemResourceType.Ballista,
                            armorLevel = Resource.ItemResourceType.PaddedArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 16; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }

                army.setAsStartArmy();
                //army.(true);
            }
            
        }

        void battleLineUpTest2(bool friendly)
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(180 + Ref.rnd.Plus_Minus(10));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.world.factions[DssRef.settings.Faction_DarkFollower];
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            //DssRef.diplomacy.declareWar(faction, enemyFac);


            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            if (friendly)
            {
                var army = faction.NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;
                army.armyColumnWidth = 6;

                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.ShortSword,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 8; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
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
                            weapon = Resource.ItemResourceType.ThrowingSpear,
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
                            weapon = Resource.ItemResourceType.Bow,
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
                            weapon = Resource.ItemResourceType.SiegeCannonBronze,
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


                army.setAsStartArmy();
                //army.(true);
            }
            else
            {

                var army = enemyFac.NewArmy(VectorExt.AddX(position, 2));
                enemyArmy = army;
                army.rotation = enemyRot;

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

                    for (int i = 0; i < 12; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Crossbow,
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
                            weapon = Resource.ItemResourceType.TwoHandSword,
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
                            weapon = Resource.ItemResourceType.Catapult,
                            armorLevel = Resource.ItemResourceType.PaddedArmor,
                            training = TrainingLevel.Basic,
                            specialization = SpecializationType.Traditional,
                        }
                    };

                    for (int i = 0; i < 8; ++i)
                    {
                        new SoldierGroup(army, SoldierProfile, army.position);
                    }
                }

                army.refreshPositions(true);
                army.setAsStartArmy();
            }
        }

        void battleLineUpTest(bool friendly)
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot =enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.faction;
            DssRef.settings.darkLordPlayer.faction.hasDeserters = false;
            DssRef.diplomacy.declareWar(faction, enemyFac);
           

            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = faction.NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;

                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.HandSpear,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 16; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.ShortSword,
                //            armorLevel = Resource.ItemResourceType.IronArmor,
                //            training = TrainingLevel.Basic,
                //            specialization = SpecializationType.Traditional,
                //        }
                //    };

                //    for (int i = 0; i < 8; ++i)
                //    {
                //        new SoldierGroup(army, SoldierProfile, army.position);
                //    }
                //}
                {
                    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                    {
                        conscript = new ConscriptProfile()
                        {
                            weapon = Resource.ItemResourceType.Crossbow,
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
                            weapon = Resource.ItemResourceType.Ballista,
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
            gameControls.army?.asynchPathUpdate();

            //return false;
        }

        public void asynchCullingUpdate(float time, bool bStateA)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.asynchCullingUpdate(time, bStateA);
            }

            orders.cullingUpdate(bStateA, playerData.localPlayerIndex);
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
                    try
                    {
                        var citiesC = faction.cities.counter();
                        while (citiesC.Next())
                        {
                            citiesC.sel.checkPlayerFuelAccess_OnGamestart_async();
                        }
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                    
                });
            }

            if (newGame)
            {
                if (faction.mainCity != null)
                {
                    faction.mainCity.tagBack = CityTagBack.Carton;
                    faction.mainCity.tagArt = CityTagArt.IconFaction;

                    if (profile.casualControls)
                    { 
                        faction.mainCity.FinishCasualBuild( PlayerControls.Casual.CasualBuildType.StartUpBarracks);
                    }
                }
            }

            nextDominationSize = faction.cities.Count + DssConst.DominationSizeIncrease.GetRandom();
        }

        public double diplomacyAddPerSec()
        {
            return DssRef.diplomacy.DefaultDiplomacyPerSecond + DssRef.diplomacy.EmbassyAddDiplomacy * faction.embassyCount;
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

            double max = DssRef.diplomacy.DefaultMaxDiplomacy + DssRef.diplomacy.EmbassyAddMaxDiplomacy * faction.embassyCount;
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
        public bool CityTagsOnMapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                viewCityTagsOnMap = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return viewCityTagsOnMap;
        }

        public bool ArmyTagsOnMapProperty(object tag, bool set, bool value)
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

        public override bool IsBot()
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
        public override string Name {

            get
            {
                if (string.IsNullOrEmpty(profile.name))
                {
                    return playerData.PublicName(LoadedFont.Regular);
                }
                else
                {
                    return LoadContent.CheckCharsSafety( profile.name, LoadedFont.Regular);
                }
            }
        } 

        public override string ToString()
        {
            return $"Local Player ({playerData.PublicName(LoadedFont.Regular)})";
        }
    }

    struct MessagePosition
    {
        public IntVector2 tilePos;
        public GameTimeStamp time;

        public MessagePosition(IntVector2 tilePos)
        {
            this.tilePos = tilePos;
            time = GameTimeStamp.Now();
        }
    }
}
