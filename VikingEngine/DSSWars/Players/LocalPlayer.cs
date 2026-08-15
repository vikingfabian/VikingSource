using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Interface.MapObjMenu;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Stockpile;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Network;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer : AbsHumanPlayer
    {
        public Engine.PlayerData playerData;

        public GameHud hud;

        public GameControls gameControls;
        protected BattleSetupManager setupManager = null;

        public MapLayerManager mapLayersManager;

        public Rectangle2 cullingTileArea = Rectangle2.ZeroOne;

        public CityTagMap cityTagMap = null;

        public FloatingInt_Max commandPoints = new FloatingInt_Max();
        public FloatingInt_Max diplomaticPoints = new FloatingInt_Max();
        
        public int warCount = 0;
        public int diplomaticPoints_softMax;

       
           

        public Data.Statistics statistics = new Data.Statistics();

        /// <summary>
        /// Faction is key
        /// </summary>
        public Dictionary<PFaction, PlayerToPlayerDiplomacy> toPlayerDiplomacies = new Dictionary<PFaction, PlayerToPlayerDiplomacy>();
        

        public List<PFaction> alliedFactions = new List<PFaction>();
        public List<PFaction> alliedFactions_build = new List<PFaction>();
        public bool netFirstTimeEnter;

        public PlayerToPlayerDiplomacy GetOrCreateToPlayerDiplomacy(AbsHumanPlayer player)
        {
            PlayerToPlayerDiplomacy result = null;
            if (toPlayerDiplomacies.TryGetValue(player.pfaction, out result) == false)
            {
                result = new PlayerToPlayerDiplomacy(player.pfaction);
                toPlayerDiplomacies.Add(result.pfaction, result);
            }

            return result;
        }

        public Automation automation;

        const int MercenaryMarketSoftLock1 = DssLib.MercenaryPurchaseCount * 5;
        const double MercenaryMarketAddPerSec_Speed1 = 0.5;
        const double MercenaryMarketAddPerSec_Speed2 = 0.3;
        public FloatingInt mercenaryMarket = new FloatingInt() { value = DssLib.MercenaryPurchaseCount * 2 };

        public MenuTab factionTab = MenuTab.NUM_NONE;
        public MenuTab cityTab;
        public MenuTab armyTab = MapObjMenu.ArmyTabs[0];
        public MenuTab pinTab = MenuTab.Info;
        public ResourcesSubTab resourcesSubTab = new ResourcesSubTab();
        public HashSet<ItemResourceType> armyFilterItems = new HashSet<ItemResourceType>();
        public UnitFilter armyFilterClasses = new UnitFilter();
        public MovingGroupsCollection movingGroupsCollection = null;

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
        public List<CesspitStatus> cesspitsCopy = new List<CesspitStatus>();
        public GroupedResource[] stockPileCopy = null;

        public PlayerControls.Tutorial tutorial = null;
        CityBorders cityBorders = new CityBorders();

        public FactionPixelTexture factionPixelTexture;
        public FactionPixelTexture minimapPixelTexture;

        public UnitsPixelTexture unitsPixelTexture;
        public int BlackMarketCount = 500;
        public int PrevCityCount = 1;

        public Profile.ObjectHudSettings cityHudSettings = new Profile.ObjectHudSettings();
        public Profile.ObjectHudSettings armyHudSettings = new Profile.ObjectHudSettings();
        public Profile.ObjectHudSettings pinHudSettings = new Profile.ObjectHudSettings();


        // public int firstAttacker = ushort.MaxValue;
        public PFaction firstAttacker = PFaction.Empty;
        public int nextDominationSize;
        public int factionsTerminated = 0;
        public bool barbarianKiller = false;
        public bool cohalitionEvent = false;
        public bool cohalitionWarning = false;

        static readonly Vector3 ThemeNorth_Blue = new Vector3(0f, 0f, 0.3f);
        static readonly Vector3 ThemeMid_Yellow = new Vector3(0.15f, 0.15f, 0);
        static readonly Vector3 ThemeSouth_Red = new Vector3(0.2f, 0.05f, 0f);

        public Vector3 ShaderThemeColor = ThemeMid_Yellow;
        
        List<MessagePosition> battleMessages = new List<MessagePosition>(8);
        public bool isDropInPlayer = false;

        public StoredCameraPos storedCameraPos;
        public PlayerNetState playerNetState = PlayerNetState.InMenu;

        public int sendGold = 1000;
       
        public LocalPlayer()
        {
            baseInit();
        }
        public void EnterBattleLab()
        {
            BattleLabStorage.Singleton = new BattleLabStorage();
            setupManager = new BattleSetupManager();
        }

        public bool DisplayBattleLab(RichBoxContent content, RichMenu menu)
        {
            if (setupManager != null)
            {
                setupManager.updateObjectDisplay(content, menu);
                return true;
            }

            return false;
        }

        public void openPlayerToPlayerDisplay(AbsHumanPlayer selected)
        { 
            gameControls.clearSelection();

            if (selected.IsLocal)
            {

            }
            else
            {
                hud.objMenu.netSessionDisplay.selectedPlayer = selected.GetRemotePlayer();
            }
            hud.needRefresh = true;
        }

        public override void refreshFlag()
        {
            base.refreshFlag();
            hud.head.RefreshFlag(this);
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

        void baseInit()
        {
            orders = new Orders.Orders();
            automation = new Automation(this);
            schoolCopy = new SchoolStatus();
            schoolCopy.defaulSetup();

            localAiAggressivity = DssRef.difficulty.aiAggressivity;
            localTooPeacefulPercentage = DssRef.difficulty.tooPeacefulPercentage;
            if (DssRef.difficulty.extremeAggression)
            {
                localAiAggressivity = AiAggressivity.Extreme;
            }
            warManagerGear = new WarManagerGear(WarManagerGear.StartGear, localAiAggressivity);
        }

        public LocalPlayer(Faction faction, bool newGame)
           : base(faction, newGame)
        {
            baseInit();
            if (newGame)
            {
                startingResources();

            }
            setPlayerFaction(faction);

            faction.technology = new XP.TechnologyTemplate();
            faction.technology.iron.points = XP.TechnologyTemplate.FactionUnlock;
        }
        public override void AssignFaction(Faction faction)
        {
            base.AssignFaction(faction);
            gameControls.refreshFaction();
        }
        public override void SetColor(Color selected, bool netShare)
        {
            var clone = profile.flag.Clone();
            profile.flag = clone;
            base.SetColor(selected, netShare);
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
            var profile = DssRef.storage.profileStorage.profiles[pStorage.profileIndex];
            if (DssRef.state.PlayType() == PlayStateType.Play && DssRef.state.playstate().recolor.HasValue)
            {
                profile.flag = profile.flag.Clone();
                profile.flag.col0_Main = DssRef.state.playstate().recolor.Value;
            }
            SetProfile(profile);
            if (!DssRef.state.host)
            {
                profile.casualControls &= Ref.netsett.remoteHostSettings.hostSettings.allowCasualControls;
            }
            
            pfaction.GetFaction().diplomaticSide = DiplomaticSide.Light;

            InputMap input = new InputMap(playerindex);
            input.setInputSource(pStorage.inputSource);

            if (pStorage.inputSource.IsXnaController)
            {
                input.copyDataFrom(Ref.gamesett.controllerMap);
            }
            else if (pStorage.inputSource.HasKeyBoard)
            {
                input.copyDataFrom(Ref.gamesett.keyboardMap);
            }

            //inputConnected = input.Connected;

            //faction.displayInFullOverview = true;

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
            //if (numPlayers > 1)
            //{
            //    toPlayerDiplomacies = new PlayerToPlayerDiplomacy[numPlayers];
            //}

            menDeliveryCopy = new DeliveryStatus();
            menDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Men);

            itemDeliveryCopy = new DeliveryStatus();
            itemDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Resource);

            goldDeliveryCopy = new DeliveryStatus();
            goldDeliveryCopy.defaultSetup(DeliveryStatus.DeliveryType_Gold);

            soldierConscriptCopy = new BarracksStatus(BuildAndExpandType.SoldierBarracks);

            archerConscriptCopy = new BarracksStatus(BuildAndExpandType.ArcherBarracks);

            warmachineConscriptCopy = new BarracksStatus(BuildAndExpandType.WarmachineBarracks);

            //knightConscriptCopy = new BarracksStatus(BuildAndExpandType.KnightsBarracks);

            gunConscriptCopy = new BarracksStatus(BuildAndExpandType.GunBarracks);

            cannonConscriptCopy = new BarracksStatus(BuildAndExpandType.CannonBarracks);

        }

        public void NetUpdate(bool bSlowUpdate)
        {
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssPlayerStatus, Network.PacketReliability.Unrelyable, SendPacketTo.All, 0, playerData.localPlayerIndex);
                DssRef.state.culling.players[playerData.localPlayerIndex].GetState().writeNet(w);

                RemotePlayerPointer.netWrite(w, this);

                w.Write((int)timePlayed.TotalSeconds);

                EightBit bits = new EightBit(Ref.steam.recordingOn, DssRef.DlcSupporter.owned);
                bits.write(w);
            }

            if (bSlowUpdate)
            {
                netWritePinUpdate();
            }   
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write((short)diplomaticPoints.Int());

            statistics.writeGameState(w);


            foreach (var kv in toPlayerDiplomacies)
            {
                kv.Key.write(w);
                kv.Value.writeGameState(w);
            }
            w.Write(ushort.MaxValue);

            automation.writeGameState(w);

            Debug.WriteCheck(w);

            tutorial_writeGameState(w);
            orders.writeGameState(w);

            cityHudSettings.write(w);   
            armyHudSettings.write(w);

            firstAttacker.write(w);
            //w.Write((ushort)firstAttacker);
            w.Write((ushort)nextDominationSize);
            w.Write(cohalitionEvent);
            w.Write(barbarianKiller);
            w.Write((ushort)factionsTerminated);

            writePins(w);
            

            storedCameraPos.writeGameState(w);

            hud.pins.writeGameState(w);

            tooPeacefulCheckTimer.write(w);
            //gameControls.build.buildPriority.writeGameState(w, false);

            writeBlackMarket(w);

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

            statistics.readGameState(r, subversion);

            if (subversion >= 59)
            {
                while (true)
                {

                    var rpfaction = PFaction.Empty;
                    //int factionIndex = -1;

                    if (subversion >= 114)
                    {
                        rpfaction.read(r);
                        //factionIndex = r.ReadUInt16();
                    }
                    else
                    {
                        int player = r.ReadInt16();

                        if (arraylib.InBound(DssRef.state.localPlayers, player))
                        {
                            rpfaction = DssRef.state.localPlayers[player].pfaction;
                        }
                    }

                    if (rpfaction.HasValue())
                    {
                        PlayerToPlayerDiplomacy tp = new PlayerToPlayerDiplomacy(rpfaction);

                        tp.readGameState(r, subversion);
                        toPlayerDiplomacies.Add(rpfaction, tp);                        
                    }
                    else
                    {
                        break;
                    }
                }
            }

            automation.readGameState(r, subversion);

            // Debug.ReadCheck(r);//TEMP!
            if (subversion >= 133)
            {
                Debug.ReadCheck(r);
            }
            else
            {
                var none1 = r.ReadInt32();
            }
            tutorial_readGameState(r, subversion);

            orders.readGameState(playerData.localPlayerIndex, r, subversion, pointers);

            if (subversion < 103)
            {
                var viewCityTagsOnMap = r.ReadBoolean();
                var viewArmyTagsOnMap = r.ReadBoolean();
            }
            else
            {
                cityHudSettings.read(r, subversion);
                armyHudSettings.read(r, subversion);
            }

            if (subversion >= 73)
            {
                firstAttacker.read(r);
                //firstAttacker = r.ReadUInt16();
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
                readPins(r, subversion);
            }

            if (subversion >= 66)
            {
                storedCameraPos.readGameState(r, subversion);
            }
            if (subversion >= 69)
            {
                hud.pins.readGameState(r, subversion);
            }

            if (subversion >= 85)
            {
                tooPeacefulCheckTimer.read(r);
            }

            readBlackMarket(r, subversion);

            Debug.ReadCheck(r);
        }

        public void writeBlackMarket(BinaryWriter w)
        {
            w.Write(Bound.UShort(BlackMarketCount));
            w.Write(Bound.UShort(PrevCityCount));
        }
        public void readBlackMarket(BinaryReader r, int subversion)
        {
            if (subversion >= 132)
            {
                BlackMarketCount = r.ReadUInt16();
                PrevCityCount = r.ReadUInt16();
            }
        }
        struct CityClientSaveMeta
        {
            public int cityIndex;
            public long memoryStart;
            public long memoryLength;

            public void write(System.IO.BinaryWriter w)
            {
                w.Write((ushort)cityIndex);
                w.Write((ushort)memoryLength);
            }

            public void read(System.IO.BinaryReader r)
            {
                cityIndex = r.ReadUInt16();
                memoryLength = r.ReadUInt16();
            }
        }
        public void writeClientSave(BinaryWriter w)
        {
            orders.writeGameState(w);

            writePins(w);
            Debug.WriteCheck(w);

            pfaction.GetFaction().writeClientState(w);
            Debug.WriteCheck(w);

            var f = pfaction.GetFaction();
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();

            List<CityClientSaveMeta> clientSaveMetas = new List<CityClientSaveMeta>(f.cities.Count);
            MemoryStreamHandler memory = new MemoryStreamHandler();
            var memW = memory.GetWriter();

            while (citiesC.Next(ref f.cities, DssRef.world.cities, out City city))
            {
                var meta = new CityClientSaveMeta() { cityIndex = city.myIndex, memoryStart = memW.BaseStream.Position };

                city.writeClientState(memW);
                meta.memoryLength = memW.BaseStream.Position - meta.memoryStart;
                clientSaveMetas.Add(meta);
                //city.workTemplate.onFactionChange(city, workTemplate, true);
            }


            w.Write((ushort)clientSaveMetas.Count);
            foreach (var c in clientSaveMetas)
            {
                c.write(w);
            }

            memory.WriteSaveFile(w);

            writeBlackMarket(w);
            Debug.WriteCheck(w);
        }
        public void readClientSave(int playerIx, BinaryReader r, int subversion)
        {
            orders.readGameState(playerIx, r, subversion, null);

            readPins(r, subversion);
            Debug.ReadCheck(r);
                        
            var f = pfaction.GetFaction();
            f.readClientState(r, subversion);
            Debug.ReadCheck(r);

            int clientSaveMetasCount = r.ReadUInt16();
            List<CityClientSaveMeta> clientSaveMetas = new List<CityClientSaveMeta>(clientSaveMetasCount);
            for (int metaIx = 0; metaIx < clientSaveMetasCount; metaIx++)
            {
                CityClientSaveMeta meta = new CityClientSaveMeta();
                meta.read(r);
                clientSaveMetas.Add(meta);
            }

            MemoryStreamHandler memory = new MemoryStreamHandler();
            memory.ReadSaveFile(r);
            var memR = memory.GetReader();
            long readStart = 0;

            for (int metaIx = 0; metaIx < clientSaveMetasCount; metaIx++)
            {
                memR.BaseStream.Position = readStart;
                CityClientSaveMeta meta = clientSaveMetas[metaIx];
                var city = DssRef.world.cities[meta.cityIndex];

                if (city.pfaction == pfaction)
                {
                    city.readClientState(memR, subversion);
                }

                readStart += meta.memoryLength;
            }

            readBlackMarket(r, subversion);
            Debug.ReadCheck(r);

            //Apply faction setup
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref f.cities, DssRef.world.cities, out City city))
            {
                city.workTemplate.onFactionChange(city, f.workTemplate, true);
            }
            
        }

        public void InitTutorial(bool newGame)
        {
            if ((newGame || PlatformSettings.STEAM_DEMO) &&
                DssRef.storage.runTutorial &&
                DssRef.state.PlayType() == PlayStateType.Play &&
                Difficulty.ModeSupportsTutorial(DssRef.difficulty.setting_gameMode, DssRef.storage.ruleset.factionStartSize))
            {
                new PlayerControls.Tutorial(this);
            }
        }

        public bool IntutorialMode()
        {
            if (tutorial != null)
            {
                return tutorial.TutorialMode();
            }
            return false;
        }

        public void tutorial_writeGameState(BinaryWriter w)
        {
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
            bool inTutorialMode = r.ReadBoolean();
            
            if (inTutorialMode)
            {
                new PlayerControls.Tutorial(this);
                tutorial.readGameState(r, subversion);
            }            
        }
        public void cityTabClick(int tab)
        {
            MenuTab newTab = AvailableCityTabs()[tab];
            if (newTab != cityTab)
            {
                cityTab = newTab;
                if (newTab == MenuTab.Resources)
                {
                    SoundLib.SubTab(resourcesSubTab.managementType);
                }
                else
                {
                    SoundLib.Tab(newTab);
                }
            }
        }
        public void armyTabClick(int tab)
        {
            MenuTab newTab = AvailableArmyTabs()[tab];
            if (newTab != armyTab)
            {
                armyTab = newTab;
                SoundLib.Tab(newTab);
            }
        }

        public void pinTabClick(int tab)
        {
            pinTab = tab == 0? MenuTab.Info : MenuTab.Tag;
        }

        public List<MenuTab> AvailableCityTabs()
        {
            if (profile.casualControls)
            {
                return MapObjMenu.CasualTabs;
            }
            return tutorial != null && tutorial.TutorialMode() ? tutorial.cityTabs : MapObjMenu.CityTabs;
        }

        public List<MenuTab> AvailableArmyTabs()
        {
            return MapObjMenu.ArmyTabs;
        }

        public void beginCreatePin()
        { 
            
        }
        public LocationPin createPin()
        {
            LocationPin pin = new LocationPin(this, gameControls.map.pointerPosWP);
            pin.myIndex = pins.Add(pin);
            pin.basicInit();

            gameControls.map.selection.obj = pin;
            hud.needRefresh = true;

            if (Ref.steam.isInitialized)
            {
                SteamTimeline.AddInstantaneousTimelineEvent(
                            DssRef.lang.ObjectType_LocationPin_Ping,               // Title in UI
                            DssRef.lang.ObjectType_LocationPin, // Description in UI
                            "steam_marker",                 // Built-in Steam icon 
                            4,                              // Priority (0 = default, max= 1000)
                            0f,                             // Offset in seconds
                            ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard // Clip suggestion priority
                        );
            }

            return pin;
        }        

        public override void createStartUnits(double unitCountMulti, bool settlerGuard)
        {
            playerStartUnits(unitCountMulti, settlerGuard, DssRef.difficulty.honorGuardCount());
           
        }

        void refreshNeihgborAggression()
        {
            if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium)
            {
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref pfaction.GetFaction().cities, DssRef.world.cities, out City citySel))
                {
                    EcsStaticArrayCounter neighbors = citySel.CityNeighbors();
                    while (neighbors.Next(DssRef.world.cities, out City nCity))//foreach (var n in citySel.neighborCities)
                    {
                        nCity.pfaction.GetPlayer()?.onPlayerNeighborCapture(this);
                    }
                }
            }
        }

        public override void OnCityCapture(City city)
        {
            if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium)
            {
                EcsStaticArrayCounter neighbors = city.CityNeighbors();
                while (neighbors.Next(DssRef.world.cities, out City nCity))
                {
                    nCity.pfaction.GetPlayer()?.onPlayerNeighborCapture(this);
                }
            }

            var f = pfaction.GetFaction();
            if (f.cities.Count > PrevCityCount)
            {
                BlackMarketCount += 50 * f.cities.Count - PrevCityCount;
                PrevCityCount = f.cities.Count;
            }

            if (!profile.casualControls)
            { 
                city.automateCity = true;
            }
        }

        public override void onNewRelation(bool isActuator, PFaction otherPFaction, DiplomaticRelation rel, RelationType previousRelation, bool fromAllianceTrade, bool localAction)
        {
            base.onNewRelation(isActuator, otherPFaction, rel, previousRelation, fromAllianceTrade, localAction);
       
            //Faction otherFaction = otherPFaction.GetFaction();

            if (otherPFaction.TryGetFaction(out var otherFaction))
            {
                if (otherFaction.factiontype != FactionType.SouthHara)
                //(rel.Relation <= RelationType.RelationTypeN3_Mobilization &&
                //otherFaction.factiontype != FactionType.SouthHara)
                //||
                //(otherFaction.player != null && otherFaction.player.IsHumanPlayer()))
                {

                    switch (rel.Relation)
                    {
                        case RelationType.RelationType3_Ally:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationAlly.Play());
                            break;
                        case RelationType.RelationType2_Good:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationGood.Play());
                            break;
                        case RelationType.RelationType1_Peace:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationGood.Play());
                            break;
                        case RelationType.RelationType0_Neutral:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationEnemy.Play());
                            break;
                        case RelationType.RelationTypeN1_Enemies:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationEnemy.Play());
                            break;
                        case RelationType.RelationTypeN2_Truce:
                            message(DssRef.lang.Diplomacy_RelationType, SoundLib.eventRelationEnemy.Play());
                            break;
                        case RelationType.RelationTypeN3_Mobilization:
                        case RelationType.RelationTypeN4_War:
                            if (previousRelation == RelationType.RelationTypeN2_Truce)
                            {
                                message(DssRef.lang.Diplomacy_TruceEndTitle, SoundLib.eventRelationWar.Play());
                            }
                            else
                            {
                                message(DssRef.lang.Diplomacy_WarDeclarationTitle, SoundLib.eventRelationWar.Play());
                                Ref.music.OnGameEvent();
                            }
                            break;
                       
                        case RelationType.RelationTypeN5_TotalWar:
                            if (previousRelation == RelationType.RelationTypeN2_Truce)
                            {
                                message(DssRef.lang.Diplomacy_TruceEndTitle, SoundLib.eventRelationTotalWar.Play());
                            }
                            else
                            {
                                message(DssRef.lang.Diplomacy_WarDeclarationTitle, SoundLib.eventRelationTotalWar.Play());
                                Ref.music.OnGameEvent();
                            }
                            break;

                    }

                    //if (rel.Relation >= RelationType.RelationType0_Neutral)
                    //{
                    //    message(DssRef.lang.Diplomacy_RelationType);
                    //}
                    //else if (rel.Relation <= RelationType.RelationTypeN3_Mobilization)
                    //{
                    //    if (previousRelation == RelationType.RelationTypeN2_Truce)
                    //    {
                    //        message(DssRef.lang.Diplomacy_TruceEndTitle);
                    //    }
                    //    else
                    //    {
                    //        message(DssRef.lang.Diplomacy_WarDeclarationTitle);
                    //        Ref.music.OnGameEvent();
                    //    }
                    //}

                    void message(string title, SoundContainerBase sound)
                    {
                        if (Ref.steam.isInitialized)
                        {
                            SteamTimeline.AddInstantaneousTimelineEvent(
                                        title,               // Title in UI
                                        DssRef.lang.Diplomacy_RelationType, // Description in UI
                                        "steam_scroll",                 // Built-in Steam icon 
                                        (uint)(-(int)rel.Relation + 5),                              // Priority (0 = default, max= 1000)
                                        0f,                             // Offset in seconds
                                        ETimelineEventClipPriority.k_ETimelineEventClipPriority_Standard // Clip suggestion priority
                                    );
                        }
                        //SoundContainerBase sound = 
                        RichBoxContent content = new RichBoxContent();
                        MessageGroup_Ingame.Title(content, title);
                        DiplomacyDisplay.FactionRelationDisplay(otherFaction, rel.Relation, content, true);
                        Ref.update.AddSyncAction(new SyncAction3Arg<RichBoxContent, SoundContainerBase, bool>(hud.messages.Add, content, sound, true));
                    }

                }
            }
        }

        public void userUpdate(bool cityUpdate)
        {
            gameControls.update();

            debugUpdate();

            mapLayersManager.Update();
            playerData.view.Camera.RecalculateMatrices();

            if (cityUpdate)
            {
                updateMapOverlays();
                cityBorders.update(this);
            }

            updatePlayer();
        }

        public void uiUpdateOnly()
        { 
            gameControls.UiUpdateOnly();
        }

        void debugUpdate()
        {
#if DEBUG
            if (Input.Keyboard.Ctrl && Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                RichBoxContent c = new RichBoxContent();
                c.text(gameControls.map.tilePosition.ToString());
                hud.messages.Add(c);
            }

            
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Z))
                {
                    //var armiesC = faction.armies.counter();
                    //while (armiesC.Next())
                    //{
                    //    armiesC.sel.food = -100;
                    //    armiesC.sel.conservedFood = -120;

                    //}
                    //DssRef.state.events.victory(Event.VictoryType.DefeatBoss);
                    //DssRef.state.events.TestNextEvent();
                    //DssRef.state.events.TestNextEvent();
                    //hud.objMenu.diplomacy?.makeServant();
                    //if (gameControls.map.hover.obj is City)
                    //{
                    //    gameControls.map.hover.obj.GetCity().setFaction(faction, false, false, true);
                    //}
                    //if (gameControls.map.hover.obj is Army)
                    //{
                    //    gameControls.map.hover.obj.GetArmy().DeleteMe(DeleteReason.Desert, true);
                    //}
                    //if (gameControls.map.hover.obj is Army)
                    //{
                    //    gameControls.map.hover.obj.GetArmy().DeleteMe(DeleteReason.Desert, true);
                    //}
                    //debugKillCityLess();
                    //fleeingArmyTest();
                }
                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    //faction.money.copper = -100000000000;
                    //DssRef.state.events.victory(Event.VictoryType.DefeatBoss);
                    //DssRef.state.events.TestNextEvent();
                    //hud.messages.Add(new RichBoxContent() { new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("message test") }, null) });
                    battleLineUpTest2(true);
                    //DssRef.state.events.TestNextEvent();
                    //DssRef.state.events.testTooPeacefulCheck();
                    //Ref.steam.StartRecording();
                }
                else if (Input.Keyboard.KeyUpEvent(Microsoft.Xna.Framework.Input.Keys.Y))
                {
                    //Ref.steam.StopRecording();
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.X))
                {
                    //battleLineUpTest3_friendly_only();
                    //battleLineUpTest2(false);

                    //var tile = DssRef.world.tileGrid.Get(gameControls.mapControls.tilePosition);
                    //Debug.Log(tile.ToString());
                }

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.N) && !Input.Keyboard.Ctrl)
                {
                    AbsWorldObject obj = gameControls.map.hover.obj as AbsWorldObject;
                    obj?.AddDebugTag();
                }


            if (Input.Keyboard.KeyDownEvent(Keys.B) && Input.Keyboard.Ctrl)
            {
                DssRef.state.menuSystem.debugMenu();
            }
#endif 
        }


        public bool mayAttackObj(AbsGameObject obj)
        {
            if (obj != null)
            {
                switch (obj.gameobjectType())
                {
                    case GameObjectType.City:
                        if (obj.GetCity().cityType == CityType.UnClaimed)
                        {
                            return false;
                        }
                        break;
                    case GameObjectType.LocationPin:
                        return false;
                }
                //if (hover.obj.gameobjectType() == GameObjectType.City &&
                //    hover.obj.GetCity().cityType == CityType.UnClaimed)
                //{
                //    return false;
                //}
                return obj.pfaction.GetFaction() != pfaction.GetFaction();

            }

            return false;
        }


        //public void debugMenu(GuiLayout layout)
        //{
        //    new GuiTextButton("Next event", "skip forward in the event timer", new GuiAction(new Action(DssRef.state.events.TestNextEvent) + DssRef.state.menuSystem.closeMenu), false, layout);
        //    new GuiTextButton("1000 resources", "add 1000 of all resources to all cities", new GuiAction(new Action(debugAddResources) + DssRef.state.menuSystem.closeMenu), false, layout);



        //    //new GuiTextButton("Enemy alliance", "when the player grow to fast", new GuiAction(new Action(()=> { DssRef.state.events.collectAllianceAgainstPlayerDomination(this); }) + DssRef.state.menuSystem.closeMenu), false, layout);

        //    //UnitType[] unitTypes = DssLib.AvailableUnitTypes;
        //    //foreach (var type in unitTypes)
        //    //{ 
        //    //    new GuiTextButton("Battle test - " + type.ToString() + " (Land)", null, 
        //    //        new GuiAction2Arg<UnitType, bool>(battleLineTest,type,false), false, layout);
        //    //}

        //    //foreach (var type in unitTypes)
        //    //{
        //    //    new GuiTextButton("Battle test - " + type.ToString() + " (Sea)", null,
        //    //        new GuiAction2Arg<UnitType, bool>(battleLineTest, type, true), false, layout);
        //    //}
        //}

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

            var city = pfaction.GetFaction().cities.GetRandom(Ref.rnd, DssRef.world.cities);
            if (city != null &&
                city.automateCity &&
                city.automationFocus == AutomationFocus.Military)
            {
                if (buySoldiers(city, false, city.warAutoArmyType, false))
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        buySoldiers(city, false, city.warAutoArmyType, true);
                    }));
                }
            }

            pfaction.GetFaction().updateResourceOverview_async();

            float z = gameControls.map.camera.LookTarget.Z / DssRef.world.Size.Y;
            if (z < 0.5)
            {
                setThemeColor(z / 0.5f, ThemeNorth_Blue, ThemeMid_Yellow);
            }
            else
            {
                setThemeColor((z - 0.5f) / 0.5f, ThemeMid_Yellow, ThemeSouth_Red);
            }

            void setThemeColor(float percSouth, Vector3 north, Vector3 south)
            {
                ShaderThemeColor = VectorExt.AddX(north * (1f - percSouth) + south * percSouth, DssRef.time.ShaderDayLight_RedTint);
            }

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref pfaction.GetFaction().cities, DssRef.world.cities, out City citySel))
            {
                citySel.WorkerStats_StuckBuildings_Process = 0;
            }

            lock (orders.orders)
            {
                bool differentPrio = false;
                int prio = city.workTemplate.Get(WorkPriorityType.buildOrders).value;

                for (int i = 0; i < orders.orders.Count; ++i)
                {
                    var buildOrder = orders.orders[i].GetBuild();
                    if (buildOrder != null)
                    {
                        if (!Build.BuildLib.BuildOptions[(int)buildOrder.buildingType].blueprint.hasResources_buildAndUpgrade(buildOrder.city))
                        {
                            buildOrder.city.WorkerStats_StuckBuildings_Process++;
                        }

                        differentPrio |= buildOrder.priority != prio;
                    }
                }

                gameControls.build.ordersDifferFromPriority = differentPrio;
            }

            citiesC.Reset();
            while (citiesC.Next(ref pfaction.GetFaction().cities, DssRef.world.cities, out City citySel))
            {
                citySel.WorkerStats_StuckBuildings = citySel.WorkerStats_StuckBuildings_Process;
            }
        }

        public void ApplyBuildPrioToAll(City city)
        {
            lock (orders.orders)
            {
                int prio = city.workTemplate.Get(WorkPriorityType.buildOrders).value;

                for (int i = 0; i < orders.orders.Count; ++i)
                {
                    var buildOrder = orders.orders[i].GetBuild();
                    if (buildOrder != null)
                    {
                        buildOrder.priority = prio;
                    }
                }

                gameControls.build.ordersDifferFromPriority = false;
            }
        }

        void updateMapOverlays()
        {
            if (mapLayersManager.current.DrawFar)
            {
                gameControls.map.hover.subTile.clear();
                //gameControls.map.selection.subTile.clear();
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
                gameControls.map.hover.subTile.clear();
                //gameControls.map.selection.subTile.clear();
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
                
        void battleLineUpTest3_friendly_only()
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.pfaction.GetFaction();
            DssRef.settings.darkLordPlayer.pfaction.GetFaction().hasDeserters = false;
            DssRef.world.diplomacy.declareWar(pfaction, enemyFac.pfaction, false);


            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = pfaction.GetFaction().NewArmy(position);
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

            Faction enemyFac = DssRef.settings.Faction_Barbarian.GetFaction();//DssRef.world.factions[DssRef.settings.Faction_DarkFollower];
            enemyFac.hasDeserters = false;
            //DssRef.world.diplomacy.declareWar(faction, enemyFac);


            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            if (friendly)
            {
                var army = pfaction.GetFaction().NewArmy(position);
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
                            weapon = Resource.ItemResourceType.Ballista,
                            armorLevel = Resource.ItemResourceType.PaddedArmor,
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
                            //weapon = Resource.ItemResourceType.KnightsLance,
                            //armorLevel = Resource.ItemResourceType.IronArmor,
                            //training = TrainingLevel.Basic,
                            //specialization = SpecializationType.Traditional,


                            man = Resource.ItemResourceType.NobleMen,
                            weapon = Resource.ItemResourceType.HandSpear,
                            armorLevel = Resource.ItemResourceType.IronArmor,
                            animal = Resource.ItemResourceType.WarHorse,
                            mountArmor = Resource.ItemResourceType.MountIronArmor,
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
                            weapon = Resource.ItemResourceType.Ballista,
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
        void fleeingArmyTest()
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.Faction_Barbarian.GetFaction();
            enemyFac.money.copper = -10000;
            enemyFac.hasDeserters = true;
            enemyFac.player.protectedFromDelete = false;
            DssRef.world.diplomacy.declareWar(pfaction, enemyFac.pfaction, false);


            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = pfaction.GetFaction().NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;

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

                army.setAsStartArmy();
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
                

                army.refreshPositions(true);
                army.setAsStartArmy();
                army.food = 0;
            }

        }

        void battleLineUpTest(bool friendly)
        {
            Rotation1D enemyRot = Rotation1D.FromDegrees(-90 + Ref.rnd.Plus_Minus(1));
            Rotation1D playerRot = enemyRot.getInvert();

            Faction enemyFac = DssRef.settings.darkLordPlayer.pfaction.GetFaction();
            enemyFac.hasDeserters = false;
            DssRef.world.diplomacy.declareWar(pfaction, enemyFac.pfaction, false);


            IntVector2 position = gameControls.map.tilePosition;

            Army friendlyArmy, enemyArmy;


            //if (friendly)
            {
                var army = pfaction.GetFaction().NewArmy(position);
                friendlyArmy = army;
                army.rotation = playerRot;

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
                //{
                //    SoldierConscriptProfile SoldierProfile = new SoldierConscriptProfile()
                //    {
                //        conscript = new ConscriptProfile()
                //        {
                //            weapon = Resource.ItemResourceType.KnightsLance,
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
              
                army.refreshPositions(true);
                army.setAsStartArmy();
            }

        }




        public void asyncPlayerPathUpdate(float time)
        {
            gameControls.army?.asynchPathUpdate();

            //return false;
        }

        override public void asynchCullingUpdate(float time, bool bStateA)
        {
            base.asynchCullingUpdate(time, bStateA);

            orders.cullingUpdate(bStateA, playerData.localPlayerIndex);
        }

        public void baseOnGameStart()
        {
            factionPixelTexture = new FactionPixelTexture(playerData.localPlayerIndex, true,
                (DssRef.settings.playType == GameState.PlayStateType.Play || DssRef.settings.playType == GameState.PlayStateType.MapEditor) ?
                FactionMapFilter.FactionCols : FactionMapFilter.Terrain);
            minimapPixelTexture = new FactionPixelTexture(playerData.localPlayerIndex, true, FactionMapFilter.Minimap);
            unitsPixelTexture = new UnitsPixelTexture(playerData.localPlayerIndex);
        }

        public override void onGameStart(bool newGame)
        {
            base.onGameStart(newGame);

            baseOnGameStart();

            hud.messages.onGameStart();
            oneSecUpdate();

            if (newGame)
            {
                commandPoints.value = commandPoints.max * 0.5;
                diplomaticPoints.value = diplomaticPoints.max * 0.6;


                if (DssRef.difficulty.resourcesStartHelp)
                {
                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref pfaction.GetFaction().cities, DssRef.world.cities, out City citySel))
                    {
                        citySel.checkPlayerFuelAccess_OnGamestart_async();
                    }
                }
            }

            pfaction.GetFaction().refreshMainCity();
            if (pfaction.GetFaction().mainCity != null)
            {
                if (newGame)
                {
                    pfaction.GetFaction().mainCity.Tag = new MapObjectTag(CityTagBack.Carton, MapObjectTag.Tag_Faction);

                    if (profile.casualControls)
                    {
                        pfaction.GetFaction().mainCity.FinishCasualBuild(PlayerControls.Casual.CasualBuildType.StartUpBarracks);
                    }

                    if (DssRef.storage.ruleset.factionStartSize == FactionStartSize.Settler)
                    {   
                        for (int i = 0; i < CityResourceIndex.COUNT; i++)
                        {
                            ref GroupedResource resources = ref pfaction.GetFaction().mainCity.GetRefGroupedResource(i);
                            resources.hardSetLimit(100);
                        }
                    }
                }

                gameControls.map.setCameraPos(pfaction.GetFaction().mainCity.tilePos);
            }
            else
            {
                gameControls.map.setCameraPos(DssRef.world.Size / 2);
            }

            nextDominationSize = pfaction.GetFaction().cities.Count + DssConst.DominationSizeIncrease.GetRandom();

            if (DssRef.state.host)
            {
                timePlayed = DssRef.time.TotalIngameTime();
            }
        }

        public double diplomacyAddPerSec()
        {
            return DssRef.world.diplomacy.DefaultDiplomacyPerSecond + DssRef.world.diplomacy.EmbassyAddDiplomacy * pfaction.GetFaction().embassyCount;
        }

        public MapDetailLayerType mapLayer()
        {
            if (Map.MapLayerManager.CameraIndexToView == null ||
                Map.MapLayerManager.CameraIndexToView[playerData.view.ScreenIndex] == null)
            {
                return MapDetailLayerType.TerrainOverview2;
            }

            return Map.MapLayerManager.CameraIndexToView[playerData.view.ScreenIndex].current.type;
        }
        public double diplomacyAddPerSec_CapIncluded()
        {
            if (diplomaticPoints.value < diplomaticPoints_softMax)
            {
                return diplomacyAddPerSec();
            }
            else
            {
                return DssRef.world.diplomacy.AddDiplomacy_AfterSoftlock_PerSecond;
            }
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            if (DssRef.time.oneMinute)
            {
                pfaction.GetFaction().resourceOverviewOneMinuteUpdate();
            }
            double max = DssRef.world.diplomacy.DefaultMaxDiplomacy + DssRef.world.diplomacy.EmbassyAddMaxDiplomacy * pfaction.GetFaction().embassyCount;
            diplomaticPoints_softMax = (int)Math.Floor(max);
            diplomaticPoints.setMax(max + DssRef.world.diplomacy.Diplomacy_HardMax_Add);

            if (diplomaticPoints.value < diplomaticPoints_softMax)
            {
                diplomaticPoints.add(diplomacyAddPerSec(), diplomaticPoints_softMax);
            }
            else
            {
                diplomaticPoints.add(DssRef.world.diplomacy.AddDiplomacy_AfterSoftlock_PerSecond);
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
                pfaction.GetFaction().addGold_factionWide(1000);
            }

            if (StartupSettings.EndlessDiplomacy)
            {
                diplomaticPoints.max = 10;
                diplomaticPoints.value = 10;
            }

            automation.oneSecondUpdate();
            //hud.oneSecondUpdate(this);
        }

        //public override void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType farm, out bool intelligent)
        //{
        //    intelligent = true;
        //    city.AutoExpandType(out work, out farm);
        //}
       
        public bool IsLocalHost()
        {
            return playerData.localPlayerIndex == 0;
        }

        virtual public bool updateObjectDisplay()
        {
            return false;
        }



        public override bool IsLocal => true;

        

        public override bool IsLocalPlayer()
        {
            return true;
        }

        public override LocalPlayer GetLocalPlayer()
        {
            return this;
        }
        public override bool HasSupportDLC()
        {
            return DssRef.DlcSupporter.owned;
        }
        public override string Name
        {

            get
            {
                if (string.IsNullOrEmpty(profile.name))
                {
                    return playerData.PublicName(LoadedFont.Regular);
                }
                else
                {
                    return LoadContent.CheckCharsSafety(profile.name, LoadedFont.Regular);
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

