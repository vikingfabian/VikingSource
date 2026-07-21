using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace.DataStream;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Data;
using VikingEngine.Network;
using VikingEngine.ToGG.MoonFall;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars
{
    partial class Faction : AbsGameObject
    {
        public Players.AbsPlayer player = null;
        public GameObject.City mainCity;
        public Vector3 SelectionCenter { get; private set; }

        public SpottedPointerArray cities;

        public int previousWarAgainstFaction = -1;
        //public DiplomaticRelation[] diplomaticRelations = null;
        public bool viewOnLargeMap = false;
        public bool storyProtectedFaction = false;
        public DiplomaticSide diplomaticSide = DiplomaticSide.None;

        public bool textureLoaded = false;

        public ModelTextureSettings FlagTexture = ModelTextureSettings.Default;

        public SpottedArray<Army> armies;

        ushort nextUnitId = 0;
        public int nextArmyId = 1;

        public bool isAlive = true;
        public bool availableForPlayer = false;
        public int availableForPlayerScore = -1;
        public FactionType factiontype;
        public FactionGroupType grouptype = FactionGroupType.Other;
        
        public float growthMultiplier = 1f;

        public float militaryStrength = 0;
        public bool hasDeserters = true;

        public int lostCity_Time0 = -1;
        public int lostCity_Time1 = -1;
        public bool quickMatchFaction = false;

        public XP.TechnologyTemplate technology;

        public Faction(int index)
        {
            this.myIndex = index;
            pfaction = new PFaction(myIndex);
            workTemplate = new Work.WorkTemplate(false, index);

            cities = new SpottedPointerArray(8);
            armies = new SpottedArray<Army>(16);
        }

        public PFaction Pointer()
        { 
            return pfaction;
        }

        public Faction(WorldData world, FactionType factiontype, int arrayIndex = -1)
        {
            if (factiontype == FactionType.DefaultAi)
            {
                if (world.availableGenericAiTypes.Count > 0)
                {
                    factiontype = arraylib.RandomListMemberPop(world.availableGenericAiTypes, world.metaData.objRnd);
                }
            }

            this.factiontype = factiontype;

            if (arrayIndex >= 0)
            {
                this.myIndex = arrayIndex;
                world.factions.HardSet(this, arrayIndex);
            }
            else
            {
                this.myIndex = world.factions.Add(this);
            }
            pfaction = new PFaction(myIndex);
            workTemplate = new Work.WorkTemplate(false, myIndex);
            world.factionComponentsAdd(this);
            initVisuals(world.metaData);

            cities = new SpottedPointerArray(8);
            armies = new SpottedArray<Army>(16);
        }

        public FactionType StoredFactionType()
        {
            return player != null && player.IsRemotePlayer() ? player.GetRemotePlayer().previousFactionType : factiontype;
        }
        
        public bool displayInFullOverview()
        {
            return viewOnLargeMap || player.IsHumanPlayer() || quickMatchFaction;
        }

        public bool IsNetHosted()
        {
            return player != null && (!Ref.netSession.InMultiplayerSession || (Ref.netSession.IsHost && player.IsLocal) || player.IsLocalPlayer());
        }

        public AbsNetworkPeer HostingPeer()
        {
            if (player == null)
            {
                return Ref.netSession.Host();
            }

            if (player.IsHumanPlayer())
            {
                return player.GetHumanPlayer().networkPeer.peer;
            }

            if (DssRef.state.host)
            {
                return Ref.netSession.LocalPeer();
            }
            else
            {
                return Ref.netSession.Host();
            }
        }
        //public void initClient(WorldData world)
        //{
        //    initDiplomacy(world);
        //}
       
        public void onGameStart(bool newGame)
        {
            player?.onGameStart(newGame);

            SpeakTerms speakTerms = DefaultSpeakingTerms();
            if (speakTerms != SpeakTerms.SpeakTerms0_Normal)
            {

                DssRef.world.diplomacy.SetDefaultSpeakTerms(this.pfaction, speakTerms);
            }
        }

        public SpeakTerms DefaultSpeakingTerms()
        {
            //Todo init all relations at start
            switch (factiontype)
            {
                default:
                    if (diplomaticSide == DiplomaticSide.Dark)
                    {
                        return SpeakTerms.SpeakTermsN1_Bad;
                    }
                    return SpeakTerms.SpeakTerms0_Normal;

                case FactionType.DarkLord:
                case FactionType.SouthHara:
                case FactionType.DarkFollower:
                case FactionType.Barbarians:
                case FactionType.GreenWood:
                case FactionType.UnitedKingdom:
                    return SpeakTerms.SpeakTermsN2_None;


                case FactionType.EasternEmpire:
                    return SpeakTerms.SpeakTermsN1_Bad;
            }
        }

        public void initMidGameEnter()
        {            
            new Players.AiPlayer(this, false);
            //initDiplomacy(DssRef.world);
        }

        public void initVisuals(WorldMetaData worldMeta)
        {
            worldMeta.setObjSeed(myIndex);
        }

        public void writeNet_Status(System.IO.BinaryWriter w)
        {
            w.Write(money.copper);
            Debug.WriteCheck(w);

            if (mainCity == null)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)mainCity.myIndex);
            }
        }

        public void readNet_Status(System.IO.BinaryReader r)
        {
            money.copper = r.ReadInt64();

            Debug.ReadCheck(r);

            int mainIndex = r.ReadUInt16();
            if (mainIndex == ushort.MaxValue)
            {
                refreshMainCity();
            }
            else
            {
                mainCity = DssRef.world.cities[mainIndex];
            }
        }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {
            if (player.IsRemotePlayer())
            {
                w.Write((ushort)player.GetRemotePlayer().previousFactionType);
                player.GetRemotePlayer().previousPlayer.writeGameState(w);
            }
            else
            {
                w.Write((ushort)factiontype);
                player.writeGameState(w);
            }            

            w.Write(money.copper);
            Debug.WriteCheck(w);

            if (mainCity == null)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)mainCity.myIndex);
            }
            
            Debug.WriteCheck(w);

            var armyList = armies.toList();
            w.Write((ushort)armyList.Count);
            foreach (var army in armyList)
            {
                army.writeGameState(w);
                Debug.WriteCheck(w);
            }

            workTemplate.writeGameState(w);

            Debug.WriteCheck(w);
            writeResources(w);

        }
        virtual public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            factiontype = (FactionType)r.ReadUInt16();
            if (player != null && player.IsLocalPlayer() && player.GetLocalPlayer().isDropInPlayer)
            {
                factiontype = FactionType.Player;
            }

            
                switch (factiontype)
                {
                    case FactionType.Player:
                        if (!player.IsLocalPlayer())
                        {
                            throw new Exception();
                        }
                        break;

                    case FactionType.DarkLord:
                        new DarkLordPlayer(this, false);
                        break;

                    default:
                        new AiPlayer(this, false);
                        break;
                }

                player.readGameState(r, subVersion, pointers);

            

            
                money.copper = r.ReadInt64();
            
            
                Debug.ReadCheck(r);
            

            
                int mainIndex = r.ReadUInt16();
                if (mainIndex == ushort.MaxValue)
                {
                    refreshMainCity();
                }
                else
                {
                    mainCity = DssRef.world.cities[mainIndex];
                }
            

            //int citiesCount = r.ReadUInt16();
            //for (int i = 0; i < citiesCount; i++)
            //{
            //    int cityIx = r.ReadUInt16();
            //    var city = DssRef.world.cities[cityIx];

            //    city.setFaction(this, true, false);

            //}

            //if (subVersion < 105)
            //{
            //    cities.read_ushort_compressed(r);
            //}
            
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                city.setFaction(this, true, false, ConvertReason.Assigned, false);
            }

            Debug.ReadCheck(r);
            

            int armiesCount = r.ReadUInt16();
            for (int i = 0; i < armiesCount; i++)
            {
                var army = new Army();
                army.readGameState(this, r, subVersion, pointers);
                
                Debug.ReadCheck(r);
                
            }

            //readRelations(r, subVersion);

            workTemplate.readGameState(r, subVersion, false);

            if (subVersion >= 110)
            {
                Debug.ReadCheck(r);
                readResources(r, subVersion);
            }
            
            citiesC.Reset();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                city.workTemplate.onFactionChange(city, workTemplate, true);
            }


        }

        //void writeRelations(System.IO.BinaryWriter w)
        //{
        //    for (int i = 0; i < diplomaticRelations.Length; ++i)
        //    {
        //        if (diplomaticRelations[i] != null &&
        //            diplomaticRelations[i].IsFactionOne(this))
        //        {
        //            diplomaticRelations[i].write(w);
        //        }
        //    }
        //    w.Write(short.MinValue);
        //}

        //void readRelations(System.IO.BinaryReader r, int subVersion)
        //{
        //    while (true)
        //    {
        //        DiplomaticRelation relation = new DiplomaticRelation();
        //        if (relation.read(r, subVersion))
        //        {
        //            relation.addToFactions();
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //}

        void writeResources(System.IO.BinaryWriter w)
        {
            BoolRegister boolRegister = new BoolRegister(CityResourceIndex.COUNT * 1);
            {
                for (int i = 0; i < CityResourceIndex.COUNT; ++i)
                {
                    DssRef.world.factionResourceOverviews[resourceComponentStartIndex + i].writeFaction( boolRegister);
                }
            } boolRegister.finalizeWrite(w);
        }

        public void readResources(System.IO.BinaryReader r, int subversion)
        {
            BoolRegister boolRegister = new BoolRegister(r);
            for (int i = 0; i < CityResourceIndex.COUNT; ++i)
            {
                DssRef.world.factionResourceOverviews[resourceComponentStartIndex + i].readFaction(boolRegister,r, subversion);
            }
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

            w.Write((ushort)factiontype);
            player.profile.writeNet(w);

            if (factiontype == FactionType.Player)
            {
                player.GetHumanPlayer().networkPeer.writeNetID(w);
            }
        }

        public void readNet(System.IO.BinaryReader r)
        {            

            factiontype = (FactionType)r.ReadUInt16();

            switch (factiontype)
            {
                case FactionType.DarkLord:
                    new DarkLordPlayer(this, false);
                    break;

                default:
                    new AiPlayer(this, false);
                    break;
            }

            player.profile.readNet(r);
            //FlagAndColor profile = new FlagAndColor(r);
            //SetProfile(profile);

            //readRelations(r, int.MaxValue);

            if (factiontype == FactionType.Player)
            {
                Network.NetworkInstancePeer.ReadNetID(r, out AbsNetworkPeer peer, out int SplitScreenIndex);
                var player = DssRef.state.GetOrCreateRemotePlayer(peer, SplitScreenIndex);
                this.player = player;
                player.pfaction = this.pfaction;
            }
            else
            {
                new Players.AiPlayer(this, false);
            }
        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            //var cityList = cities.toList();

            //w.Write((ushort)Debug.Ushort_OrCrash(cityList.Count));

            //foreach(var c in cityList)
            //{
            //    w.Write((ushort)c.myIndex);
            //}
            cities.write_ushort_compressed(w);

            w.Write(availableForPlayer);
        }

        public void readMapFile(System.IO.BinaryReader r, int mapVersion, WorldData world)
        {
            cities.read_ushort_compressed(r/*, myIndex == 4? -1 : 0*/);
            //int cityCount = r.ReadUInt16();

            //for (int i = 0; i < cityCount; ++i)
            //{
            //    int cityIx = r.ReadUInt16();
            //    AddCity(world.cities[cityIx], true);
            //}

            availableForPlayer= r.ReadBoolean();
        }

        public void OnFlagtextureLoaded(Faction newFaction)
        {
            if (!textureLoaded)
            {
                FlagTexture.SetSpriteName(SpriteName.NO_IMAGE);
                textureLoaded = true;
                onNewOwner(newFaction);
            }
        }

        void onNewOwner(Faction newFaction)
        {
            if (!textureLoaded)
                FlagTexture.ColorAndAlpha = player.profile.flag.col0_Main.ToVector4();

            //var citiesC = cities.counter();
            //while (citiesC.Next())
            //{
            //    citiesC.sel.OnNewOwner(newFaction);
            //}
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                city.OnNewOwner(newFaction, false, ConvertReason.Assigned);
            }
        }
        
        public Army NewArmy(IntVector2 startPos)
        {
            //if (DssRef.state.PartyMode)
            //{
            //    var army = new GameObject.Party.PartyArmy(this, startPos);
            //    return army;
            //}
            //else
            //{
                var army = new Army(this, startPos);
                return army;
            //}
        }

        public void AddArmy(Army army, int overrideIx = -1)
        {
            if (overrideIx < 0)
            {
                army.myIndex = armies.Add(army);
            }
            else
            {
                army.myIndex = overrideIx;
                armies.HardSet(army, overrideIx);
            }
            army.pfaction = pfaction;
        }

        public void AddCity(City city, bool duringStartUp)
        {
            if (duringStartUp)
            {
                if (mainCity == null)
                {
                    mainCity = city;
                }
                else if (city.HousingCount_Workers > mainCity.HousingCount_Workers)
                {//larger city
                    mainCity = city;
                }
                cities.Add(city.myIndex);
                city.setFaction(this, duringStartUp, false, ConvertReason.Assigned, false);
            }
            else
            {

                if (!cities.Contains(city.myIndex))
                {
                    cities.Add(city.myIndex);
                    city.setFaction(this, duringStartUp, false, ConvertReason.WarCapture, false);
                    if (!duringStartUp)
                    {
                        player?.OnCityCapture(city);

                        city.workTemplate.setAllToFollowFaction();
                        city.workTemplate.onFactionChange(city, workTemplate, duringStartUp);
                        city.defaultResourceBuffer(DssRef.world);

                        if (mainCity == null || mainCity.pfaction != pfaction)
                        {
                            refreshMainCity();

                        }
                    }
                }
            }
        }

        public void Net_AddCity(City city)
        {
            if (cities.AddIfNotExists(city.myIndex))
            {
                refreshMainCity();
            }
        }

        public AbsMapObject GetUnit(System.IO.BinaryReader r)
        {
            ushort id = r.ReadUInt16();
            AbsMapObject result = null;

            return result;
        }

        public bool HasArmyBlockingPosition(IntVector2 tilepos)
        {
            var armyC = armies.counter();
            while (armyC.Next())
            {
                if ((armyC.sel.objective == ArmyObjective.None || armyC.sel.objective == ArmyObjective.Halt) &&
                    armyC.sel.tilePos == tilepos)
                { 
                    return true;
                }
            }

            return false;
        }

        public void update()
        {
            var armiesCounter = armies.counter();

            while (armiesCounter.Next())
            {
                armiesCounter.sel.update();
            }

            player?.Update();
        }

        public void update_client(bool playerDetailView)
        {
            var armiesCounter = armies.counter();

            while (armiesCounter.Next())
            {
                armiesCounter.sel.net_updateclient(playerDetailView);
            }
        }

        public void PauseUpdate()
        {
            var armiesCounter = armies.counter();

            if (IsNetHosted())
            {
                while (armiesCounter.Next())
                {
                    armiesCounter.sel.PauseUpdate();
                }
            }
            else
            {
                while (armiesCounter.Next())
                {
                    armiesCounter.sel.clientPauseUpdate();
                }
            }
        }
        public void client_oneSecUpdate(bool minute)
        {
            if (isAlive)
            {
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
                {
                    if (city.pfaction == pfaction)
                    {
                        city.oneSecondCaptureCheck();
                    }
                    else
                    {
                        citiesC.RemoveAtCurrent(ref cities);
                        refreshMainCity();
                    }
                }
            }
        }
        public void oneSecUpdate(bool minute)
        {
            if (isAlive)
            {

                CityTradeImport = CityTradeImportCounting;
                CityTradeExport = CityTradeExportCounting;
                CityTradeImportCounting -= CityTradeImport;
                CityTradeExportCounting -= CityTradeExport;

                double incomeMultiplier = 1;
                if (player.IsBot())
                {
                    if (DssRef.state.events.RunAi() == false)
                    {
                        incomeMultiplier = 0.1;
                    }
                    else if (player.aggressionLevel > AbsPlayer.AggressionLevel0_Passive)
                    {
                        incomeMultiplier = DssRef.difficulty.aiEconomyMultiplier;
                    }
                }
                else if (player.IsRemotePlayer())
                {
                    incomeMultiplier = player.GetRemotePlayer().incomeMultiplier;
                }

                double income = 0;
                Money citiesTotalCopper = Money.Zero;

                player.oneSecUpdate();

                embassyCount = 0;
                //var citiesC = cities.counter();
                //while (citiesC.Next())
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
                {
                    if (city.pfaction.factionIndex == myIndex)
                    {
                        city.oneSecUpdate(minute);
                        embassyCount += city.buildingStructure.Embassy_count;

                        income += city.income_oneSecUpdate(incomeMultiplier);
                        citiesTotalCopper.copper += city.money.copper;
                    }
                    else
                    {
                        citiesC.RemoveAtCurrent(ref cities);
                        refreshMainCity();
                    }
                }


                if (DssRef.storage.ruleset_instance.centralGold)
                {
                    money.copper += Convert.ToInt32(income);
                }
                else
                {
                    money = citiesTotalCopper;
                }

                previuosMoney = storeMoney;
                storeMoney = money;

                if (cities.Count == 0 && !player.protectedFromDelete && Ref.netSession.IsHost)
                {
                    if (armies.Count == 0)
                    {
                        DeleteMe();
                    }
                    else if (militaryStrength < 0.4f)
                    {
                        var armiesC = armies.counter();
                        while (armiesC.Next())
                        {
                            armiesC.sel.DeleteMe(DeleteReason.Desert, true);
                        }

                        DeleteMe();
                    }
                }
            }
        }

        public void asynchAiPlayersUpdate(float time)
        {
            player?.aiPlayerAsynchUpdate(time);
        }
        
        public void asynchGameObjectsUpdate(float time, float oneSecondUpdate, bool oneMinute)
        {
            if (oneMinute)
            {
                foodProduction.minuteUpdate();
                foodSpending.minuteUpdate();
            }

            bool netHosted = IsNetHosted();

            float armiesStrength = 0;

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.IsNetHosted = netHosted;
                armiesC.sel.asynchGameObjectsUpdate(time, oneMinute);
                armiesStrength += armiesC.sel.strengthValue;
            }
            
            resources_updateAsynch(oneSecondUpdate, out float citiesStrength);

            militaryStrength = armiesStrength + citiesStrength;
        }

        public void asynchSleepObjectsUpdate(float time)
        {
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.asynchSleepObjectsUpdate(time);
            }
        }

        public void asyncPathUpdate(int pathThreadIndex)
        {
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.asyncPathUpdate(pathThreadIndex);
            }

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                city.asyncPathUpdate(pathThreadIndex);
            }
        }

        public void asynchCullingUpdate(float time, bool bStateA)
        {
            
            foreach (var p in DssRef.state.localPlayers)
            {
                p.unitsPixelTexture.updateColorProfile(pfaction);
            }
            
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.asynchCullingUpdate(time, bStateA);
            }
        }

        public int pickNextUnitId()
        {
            ++nextUnitId;

            return nextUnitId;
        }

        public void remove(Army army)
        {
            Debug.CrashIfThreaded();
            armies.RemoveAt_EqualSafeCheck(army, army.myIndex);
        }

        public void remove(City city)
        {
            if (cities.Remove(city.myIndex))
            {
                if (city == mainCity ||
                   mainCity == null || mainCity.pfaction.factionIndex != myIndex)
                {
                    refreshMainCity();
                }

                if (player != null && player.IsLocalPlayer())
                {
                    var localplayer = player.GetLocalPlayer();

                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        player.orders?.refreshAvailable(this.pfaction);

                        RichBoxContent content = new RichBoxContent();
                        
                        if (localplayer.battleMessageCheck(city.tilePos))
                        {
                            MessageGroup_Ingame.Title(content, DssRef.lang.Message_LostCity);

                            var gotoButtonContent = new RichBoxContent();
                            MessageGroup_Ingame.ControllerInputIcons(localplayer, gotoButtonContent);

                            city.toButtonContent(gotoButtonContent, true);

                            content.Add(new ArtButton(RbButtonStyle.Primary, gotoButtonContent,
                                new RbAction1Arg<AbsGameObject>(localplayer.hud.messages.goToMapObject, city, RbSoundType.Default))
                            { fillWidth = true });

                            localplayer.hud.messages.Add(content);
                        }
                    }));
                }
            }
        }

        public void refreshMainCity()
        {
            if (mainCity != null && mainCity.pfaction.factionIndex != myIndex)
            {
                mainCity = null;
            }

            if (mainCity == null || mainCity.cityType < CityType.Capital)
            {
                City largest = null;

                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
                {
                    
                    if (largest == null || citySel.HousingCount_Workers > largest.HousingCount_Workers)
                    {
                        largest = citySel;
                    }
                }

                mainCity = largest;
            }
        
        }

        public IntVector2 landAreaCenter(out bool cityPosition)
        {
            var mainCity_sp = mainCity;
            if (mainCity_sp != null)
            {
                cityPosition = true;
                return mainCity_sp.tilePos - IntVector2.One;
            }
            else if (armies.Count > 0)
            {
                var first = armies.First();

                if (first != null)
                {
                    cityPosition = false;
                    return first.tilePos;
                }
            }

            cityPosition = false;
            return IntVector2.Zero;
        }

        public Army ClosestFriendlyArmy(Vector3 position, float maxDist)
        {
            Army closestArmy = null;
            float closestLenght = float.MaxValue;

            var armiesCounter = armies.counter();
            while (armiesCounter.Next())
            {
                Vector3 diff = armiesCounter.sel.position - position;
                float l = diff.Length();
                if (l < maxDist)
                {
                    if (l < closestLenght)
                    {
                        closestLenght = l;
                        closestArmy = armiesCounter.sel;
                    }
                }                
            }

            return closestArmy;
        }
               
        public void tradeAllianceWars(bool isActuator, PFaction alliedFaction)
        {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        bool protectedFromWars = player.IsLocalPlayer() && DssRef.difficulty.setting_gameMode == GameModeMainType.Peaceful && !isActuator;

                        RelationsLoop loop = new RelationsLoop(pfaction);
                        while (loop.Next())
                        {
                            var myRelation = loop.Relation();
                            {
                                if (myRelation.Relation <= RelationType.RelationTypeN4_War)
                                {
                                    var thirdParty = loop.OtherFaction_P();
                                        var allyToEnemyRelation = DssRef.world.diplomacy.GetRelation(alliedFaction, thirdParty);

                                        if (allyToEnemyRelation.Relation < RelationType.RelationType3_Ally)
                                        {
                                            //share worst relation
                                            RelationType worst = (RelationType)Math.Min((int)myRelation.Relation, (int)allyToEnemyRelation.Relation);
                                            if (worst <= RelationType.RelationTypeN3_Mobilization)
                                            {
                                                if (protectedFromWars)
                                                {
                                                    DssRef.world.diplomacy.SetRelationType(alliedFaction, thirdParty, alliedFaction, RelationType.RelationTypeN1_Enemies, null, null, false, true);
                                                }
                                                else
                                                {
                                                    DssRef.world.diplomacy.declareWar(alliedFaction, thirdParty, true);
                                                }
                                            }
                                            else
                                            {
                                                DssRef.world.diplomacy.SetRelationType(alliedFaction, thirdParty, alliedFaction, worst, null, null, false, true);
                                            }
                                        }
                                    //}
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }                    
                }
            );
        }

        public void shareRelationWithAllAllies(PFaction relationTo, RelationType relationType)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    DssRef.world.diplomacy.SetRelationType(this.pfaction, relationTo, this.pfaction, relationType);

                    RelationsLoop loop = new RelationsLoop(pfaction);
                    while (loop.Next())
                    {
                        
                            if (loop.Relation().Relation >= RelationType.RelationType3_Ally)
                            {
                            //if (loop.OtherFaction(out var ally))
                            //{
                                var ally = loop.OtherFaction_P();
                                DssRef.world.diplomacy.SetRelationType(ally, relationTo, ally, relationType);
                                //}
                            }
                        
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });
        }

        public void stopAllAttacksAgainst(PFaction otherFaction)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var armiesC = armies.counter();
                    while (armiesC.Next())
                    {
                        armiesC.sel.stopAllAttacksAgainst(otherFaction);
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
                
            });
        }

       
        public void toHud(RichBoxContent content, RelationType relation, bool flag, bool dark)
        {
            if (flag)
            {
                content.Add(FlagTextureToHud());
                content.hspace();
            }
            else
            {
                content.Add(new RbImage(SpriteName.WarsGovernmentIcon));
                content.space(0.5f);
            }
            if (relation != RelationType.NONE)
            {
                IconName.Relation(relation, out SpriteName relIcon, out string relName);
                content.Add(new RbImage(relIcon));
            }
            if (player.IsRemotePlayer())
            {
                content.space(0.5f);
                content.Add(new RbGamerIcon(((RemotePlayer)player).networkPeer.peer, 0.8f));
            }

            content.space(0.5f);
            content.Add(new RbText(PlayerName, dark ? HudLib.TitleColor_Name_Dark : HudLib.TitleColor_Name));
        }

        public void mergeTo(Faction masterFaction)
        {
            DeleteMe();

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.setFaction(masterFaction, false, true, ConvertReason.Diplomacy, true);
            }

            armies.Clear();

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                citySel.setFaction(masterFaction, false, true, ConvertReason.Diplomacy, true);                
            }

            cities.Clear();

            DssRef.world.BordersUpdated = true;
        }

        public List<Faction> adjacentFactions(bool botsOnly)
        {
            //List<Faction> factions = new List<Faction>();
            HashSet<Faction> factions = new HashSet<Faction>();

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                EcsStaticArrayCounter neighbors = citySel.CityNeighbors();
                while (neighbors.Next(DssRef.world.cities, out City nCity))
                {
                    var nCityFaction = nCity.pfaction.GetFaction();

                    if (nCityFaction != null &&
                        nCityFaction != this &&
                        (!botsOnly || nCityFaction.player.IsBot()))
                        //&&
                        //!factions.Contains(nCityFaction))
                    {
                        factions.Add(nCityFaction);
                    }
                }
            }

            return factions.ToList();
        }

        public void SetNeighborToPlayer()
        {
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                
                city.SetNeighborToPlayer();
            }
        }
        public bool HasPlayerNeighbor()
        {
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                if (city.HasPlayerNeighbor())
                {
                    return true;
                }
            }
            return false;
        }

        public void DeleteMe()
        {
            if (isAlive)
            {
                isAlive = false;
                DssRef.state.events.onFactionDestroyed(this);
                DssRef.world.diplomacy.onFactionDeath(this.pfaction);

                if (factiontype == FactionType.Player)
                {
                    DssRef.state.events.onPlayerDeath();
                }
            }
        }

        public bool HasZeroUnits()
        { 
            return cities.Count == 0 &&  armies.Count == 0;
        }

        public override string ToString()
        {
            //if (player is Players.LocalPlayer)
            //{
            //    return Owner.Name;
            //}

            return $"Faction ({myIndex}) - Owner ({player?.Name}), Type({factiontype})";
        }

        public string PlayerName
        {
            get
            {
                if (player == null)
                {
                    return "NONE";
                }
                return  player.Name;
            }
        }

        public void WriteNetId(System.IO.BinaryWriter w)
        {
            w.Write((byte)myIndex);
        }
        //public Players.AbsPlayer Owner
        //{
        //    get
        //    {
        //        return player;
        //    }
        //    set
        //    {
        //        if (player != value)
        //        {
        //            player = value;
        //            onNewOwner();
        //        }
        //    }
        //}


        public FactionSize Size()
        {
            if (citiesEconomy.workerCount <= DssConst.LargeCityStartMaxWorkForce * 2)
            {
                return FactionSize.Tiny;
            }
            else if (citiesEconomy.workerCount <= DssConst.LargeCityStartMaxWorkForce * 6)
            {
                return FactionSize.Normal;
            }
            else if (citiesEconomy.workerCount <= DssConst.LargeCityStartMaxWorkForce * 30)
            {
                return FactionSize.Big;
            }
            else 
            {
                return FactionSize.Giant;
            }
        }

        public void SetStartOwner(Players.AbsPlayer owner)
        {
            this.player = owner;
        }

        public RbTexture FlagTextureToHud()
        {
            return new RbTexture(player.flagTexture, 1f, 0, 0.2f);
        }
        Color tempColor = FlagAndColor.AiColorRange.GetRandom();

        public Color Color()
        {
                if (player == null || player.profile.flag == null)
                    return tempColor;
                return player.profile.flag.col0_Main;            
        }

        public List<Faction> CollectWars()
        {
            List<Faction> opponents = new List<Faction>();
            //for (int relIx = 0; relIx < diplomaticRelations.Length; ++relIx)
            //{
            RelationsLoop loop = new RelationsLoop(pfaction);
            while (loop.Next())
            {   
                if (loop.Relation().InWar() && loop.OtherFaction(out var opponent))
                {
                    opponents.Add(opponent);
                }
            }

            return opponents;
        }

        public int CountWars(out int playerWars)
        {
            int count = 0;
            playerWars = 0;
            
            RelationsLoop loop = new RelationsLoop(pfaction);
            while (loop.Next())
            {
                if (loop.Relation().InWar())
                {
                    if (loop.OtherFaction(out var opponent) && opponent.player.IsHumanPlayer())
                    {
                        ++playerWars;
                    }
                    ++count;
                }
            }

            return count;
        }


        /// <returns>Combined strength of allied nations (myself not included)</returns>
        public float CollectAllianceStrength()
        {
            float result = 0;

            RelationsLoop loop = new RelationsLoop(pfaction);
            while (loop.Next())
            {
                if (loop.Relation().InAlliance() && loop.OtherFaction(out var ally))
                {
                    result += ally.militaryStrength;
                }
            }
            //for (int relIx = 0; relIx < diplomaticRelations.Length; ++relIx)
            //{
            //    if (diplomaticRelations[relIx] != null &&
            //        relIx != myIndex &&
            //       diplomaticRelations[relIx].Relation >= RelationType.RelationType3_Ally)
            //    {
            //        var ally = DssRef.world.faction(relIx);
            //        if (ally != null)
            //        {
            //            result += ally.militaryStrength;
            //        }
            //    }
            //}

            return result;
        }

        public float MyPlusAllianceStrengthValue()
        {
            return militaryStrength + CollectAllianceStrength() * 0.5f;
        }

        public bool WantToAllyAgainstDark()
        {
            return diplomaticSide == DiplomaticSide.Light &&
                DssRef.state.events.StoryIndex() >= EventsOrder.DarkLord;
        }

        public bool SameOrNeutralSide(DiplomaticSide otherFaction)
        {
            return this.diplomaticSide == DiplomaticSide.None || otherFaction == DiplomaticSide.None || diplomaticSide == otherFaction;
        }
        
        //public override Faction GetFaction()
        //{
        //    return this;
        //}

        public Army GetArmyFromId(int id)
        {
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                if (armiesC.sel.id == id)
                { 
                    return armiesC.sel;
                }
            }

            return null;    
        }

        public override bool aliveAndBelongTo(PFaction faction)
        {
            return faction == this.pfaction;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.Faction;
        }
    }

    enum FactionSize
    {
        Tiny,
        Normal,
        Big,
        Giant,
    }

    enum FactionType
    {
        DefaultAi = 0,
        Player = 1,
        DarkLord = 2,
        DarkFollower = 3,
        UnitedKingdom = 4,
        GreenWood = 5,
        EasternEmpire = 6,
        NordicRealm = 7,
        BearClaw = 8,
        NordicSpur = 9,
        IceRaven = 10,
        DragonSlayer = 11,
        SouthHara = 12,

        DyingMonger,
        NewMonger,
        DyingHate,
        NewHate,
        DyingDestru,
        NewDestu,

        //Generic ai
        Starshield,
        Bluepeak,
        Hoft,
        RiverStallion,
        Sivo,

        AelthrenConclave,
        VrakasundEnclave,
        Tormürd,
        ElderysFyrd,
        Hólmgar,
        RûnothalOrder,
        GrimwardEotain,
        SkaeldraHaim,
        MordwynnCompact,
        AethmireSovren,

        ThurlanKin,
        ValestennOrder,
        Mournfold,
        OrentharTribes,
        SkarnVael,
        Glimmerfell,
        BleakwaterFold,
        Oathmaeren,
        Elderforge,
        MarhollowCartel,
        
        TharvaniDominion,
        KystraAscendancy,
        GildenmarkUnion,
        AurecanEmpire,
        BronzeReach,
        ElbrethGuild,
        ValosianSenate,
        IronmarchCompact,
        KaranthCollective,
        VerdicAlliance,

        OrokhCircles,
        TannagHorde,
        BraghkRaiders,
        ThurvanniStonekeepers,
        KolvrenHunters,
        JorathBloodbound,
        UlrethSkycallers,
        GharjaRavagers,
        RavkanShield,
        FenskaarTidewalkers,

        HroldaniStormguard,
        SkirnirWolfkin,
        ThalgarBearclaw,
        VarnokRimeguard,
        KorrakFirehand,
        MoongladeGat,
        DraskarSons,
        YrdenFlamekeepers,
        BrundirWarhorns,
        OltunBonecarvers,

        HaskariEmber,
        ZalfrikThunderborn,
        BjorunStonetender,
        MyrdarrIcewalkers,
        SkelvikSpear,
        VaragThroatcallers,
        Durakai,
        FjornfellWarhowl,
        AshgroveWard,
        HragmarHorncarvers,

        Barbarians,

        /// <summary>
        /// Wood-elves who guard enchanted forests. Secretive, druidic, tied to nature spirits.
        /// </summary>
        SylvaranGlade,

        /// <summary>
        /// Marsh-dwellers, human clans who thrive in bogs and waterways, masters of ambush.
        /// </summary>
        DrelmirePact,

        /// <summary>
        /// Stubborn mountain dwarves, famed for masterwork steel and siegecraft.
        /// </summary>
        KhazrunForgeclan,

        /// <summary>
        /// Nomadic steppe riders, swift raiders and proud cavalry culture.
        /// </summary>
        VeylanHorselords,

        /// <summary>
        /// A human religious order devoted to the Eternal Flame. Zealous and uncompromising.
        /// </summary>
        ThalosCovenant,

        /// <summary>
        /// Coastal defenders, human mariners and sea-watchers, sworn to protect against pirates.
        /// </summary>
        NerathianTideguard,

        /// <summary>
        /// Desert-dwellers, scarred nomads once driven from their homeland. Fierce survivalists.
        /// </summary>
        SkaruunExiles,

        /// <summary>
        /// Dragon-worshipping cult/kingdom, ruled by dragonblooded warlords.
        /// </summary>
        DraktharDominion,

        /// <summary>
        /// Brutal mercenary brotherhood, sellswords bound by strict contracts.
        /// </summary>
        MalrekIronbound,

        /// <summary>
        /// A modest barony nestled in fertile valleys, proud of its ancient stone keeps.
        /// </summary>
        BranthollowBarony,

        /// <summary>
        /// Grain-rich plains kingdom, known for horse-breeding and wheat harvests.
        /// </summary>
        DunwadeHold,

        /// <summary>
        /// Borderland march-lords, stern folk living in fortified towns along contested lands.
        /// </summary>
        CaerwynMarches,

        /// <summary>
        /// Mining folk in a rugged valley, semi-independent but loyal to their lords.
        /// </summary>
        StonevaleFreehold,

        /// <summary>
        /// Small forested domain, famed for herbalists and bowmen.
        /// </summary>
        GlenmereLordship,

        /// <summary>
        /// A minor princely house clinging to its old glory, proud but weakened.
        /// </summary>
        ArveldonPrincipality,

        /// <summary>
        /// Coastal duchy of fisherfolk and shipwrights, always at odds with pirates.
        /// </summary>
        WestmereReaches,

        /// <summary>
        /// Small marcher state, thorny hedges and palisades mark their borders.
        /// </summary>
        ThornwickWardens,

        /// <summary>
        /// A sleepy lakeside domain, romanticized in ballads but of little power.
        /// </summary>
        EvermereFief,

        /// <summary>
        /// Forest hillfolk, stubborn and hearty, famed for boar-hunting feasts.
        /// </summary>
        BryndralHollow,

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        Mendog,

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        Minde,

        /// <summary>
        /// A proud family of royal knights
        /// </summary>
        FloKingdom,

        /// <summary>
        /// A macon family with the secrets to advanced buildings
        /// </summary>
        CarolusKeksenmark,

        /// <summary>
        /// Theme: A confederation of hobbit villages along winding streams, known for gardens, festivals, and fiercely defended borders when threatened.
        /// </summary>
        BramblebrookHill,

        /// <summary>
        /// Theme: Hill-dwelling hobbits in cozy burrows, famous for cider, storytelling, and their legendary hospitality (and occasional trickery).
        /// </summary>
        Tumblehill,

        /// <summary>
        /// Theme: A democracy run house with focus on politics and military might. Looks down on any outsiders.
        /// </summary>
        Etheleorthe,

        /// <summary>
        /// Theme: Four headed dragon symbol. Known for having an unpenetrable castle.
        /// </summary>
        DragonGem,

        /// <summary>
        /// Theme: Easter egg for december. "Tomten" is an old nordic name for father christmas
        /// </summary>
        Tomten,

        /// <summary>
        /// Theme: The blessed folk. A horde like farmers faction.
        /// </summary>
        Hælfolc,

        /// <summary>
        /// The Iron Saints, people who guard a mountain pass against evil.
        /// </summary>
        AerimAngren,

        /// <summary>
        /// Faction of city elves who grow purple flowers, and specialize in medicine
        /// </summary>
        Ellium,

        /// <summary>
        /// An independant group that are masters of tricks and illusions, their name means "filthy trick"
        /// </summary>
        GrakPushdug,

        /// <summary>
        /// Nobel household in a rough part of the world
        /// </summary>
        Draugost,

        NUM
    }

    enum FactionGroupType
    {
        Other,
        Nordic,
    }

    enum FactionFlavorType
    {
        Other,
        Horse,
        Mountain,
        Noble,
        Sea,
        Forest,
        Mystical,
        Warrior,
        People,
        Desert,
        City,
    }

    enum DiplomaticSide
    {
        None,
        Light,
        Dark,
    }
}
