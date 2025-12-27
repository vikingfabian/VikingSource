using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Data;
using VikingEngine.Network;
using VikingEngine.ToGG.MoonFall;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.Players.Profile;
using System.Collections.Concurrent;

namespace VikingEngine.DSSWars
{
    partial class Faction : AbsGameObject
    {
        public Players.AbsPlayer player = null;
        public GameObject.City mainCity;
        public Vector3 SelectionCenter { get; private set; }


        //public ConcurrentBag<int> cityPointers;
        //public SpottedArray<GameObject.City> cities;
       public SpottedPointerArray cities;

        public int previousWarAgainstFaction = -1;
        //public DiplomaticRelation[] diplomaticRelations = null;
        public DiplomaticSide diplomaticSide = DiplomaticSide.None;

        public bool textureLoaded = false;

        public ModelTextureSettings FlagTexture = ModelTextureSettings.Default;

        public SpottedArray<Army> armies;

        ushort nextUnitId = 0;
        public int nextArmyId = 1;

        public bool isAlive = true;
        public bool availableForPlayer = false;
        public FactionType factiontype;
        public FactionGroupType grouptype = FactionGroupType.Other;
        public bool displayInFullOverview = false;
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

            cities = new SpottedPointerArray(8);
            //cities = new SpottedArray<GameObject.City>(8);
            //cityPointers = new ConcurrentBag<int>();
            armies = new SpottedArray<Army>(16);
        }

        public Faction(WorldData addTo, FactionType factiontype, int arrayIndex = -1)
        {
            if (factiontype == FactionType.DefaultAi)
            {
                if (addTo.availableGenericAiTypes.Count > 0)
                {
                    factiontype = arraylib.RandomListMemberPop(addTo.availableGenericAiTypes, addTo.metaData.objRnd);
                }
            }

            this.factiontype = factiontype;

            if (arrayIndex >= 0)
            {
                this.myIndex = arrayIndex;
                addTo.factions.HardSet(this, arrayIndex);
            }
            else
            {
                this.myIndex = addTo.factions.Add(this);
            }
            factionIndex = myIndex;
            addTo.factionComponentsAdd(this);
            initVisuals(addTo.metaData);

            cities = new SpottedPointerArray(8);//new SpottedArray<City>(8);
            armies = new SpottedArray<Army>(16);
        }

        public void initClient(WorldData world)
        {
            initDiplomacy(world);
        }
       
        public void onGameStart(bool newGame)
        {
            player?.onGameStart(newGame);
        }

        public void initDiplomacy(WorldData world)
        {
            diplomaticRelations = new DiplomaticRelation[world.factions.Array.Length];
        }

        public void initVisuals(WorldMetaData worldMeta)
        {
            worldMeta.setObjSeed(myIndex);
        }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {            
            w.Write((ushort)factiontype);
            player.writeGameState(w);

            w.Write(money.copper);
            Debug.WriteCheck(w);

            cities.write_ushort(w);
            //var cityList = cities.toList(DssRef.world.cities);
            //w.Write((ushort)cityList.Count);
            //foreach(var city in cityList)
            //{
            //    w.Write((ushort)city.myIndex);
            //}
            Debug.WriteCheck(w);

            var armyList = armies.toList();
            w.Write((ushort)armyList.Count);
            foreach (var army in armyList)
            {
                army.writeGameState(w);
                Debug.WriteCheck(w);
            }

            writeRelations(w);

            workTemplate.writeGameState(w, false);
        }
        virtual public void readGameState(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            factiontype = (FactionType)r.ReadUInt16();
            if (player != null && player.IsLocalPlayer() && player.GetLocalPlayer().isDropInPlayer)
            {
                factiontype = FactionType.Player;
            }

            if (subVersion >= 81)
            {
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

            }

            if (subVersion < 53)
            {
                int gold = r.ReadInt32();
                money.copper = gold * 100;
            }
            else if (subVersion < 67)
            {
                money.copper = r.ReadInt32();
            }
            else
            {
                money.copper = r.ReadInt64();
            }
            if (subVersion >= 77)
            {
                Debug.ReadCheck(r);
            }

            //int citiesCount = r.ReadUInt16();
            //for (int i = 0; i < citiesCount; i++)
            //{
            //    int cityIx = r.ReadUInt16();
            //    var city = DssRef.world.cities[cityIx];
                    
            //    city.setFaction(this, true, false);
                
            //}
            cities.read_ushort(r);
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                //int cityIx = r.ReadUInt16();
                //if (arraylib.InBound(DssRef.world.cities, cityIx))
                //{
                //    //var city = DssRef.world.cities[cityIx];
                //    //cities.Add(city);
                //    city.setFaction(this, true, false);
                //}
                city.setFaction(this, true, false);
            }

            if (subVersion >= 76)
            { 
                Debug.ReadCheck(r);
            }

            int armiesCount = r.ReadUInt16();
            for (int i = 0; i < armiesCount; i++)
            {
                var army = new Army();
                army.readGameState(this, r, subVersion, pointers);
                
                if (subVersion >= 76)
                {
                    Debug.ReadCheck(r);
                }
            }

            readRelations(r, subVersion);

            if (subVersion < 81)
            {
                if ((factiontype == FactionType.Player) != player.IsLocalPlayer())
                {
                    throw new Exception();
                }

                player.readGameState(r, subVersion, pointers);
            }

            workTemplate.readGameState(r, subVersion, false);

            //var cities_c = cities.counter();
            //while (cities_c.Next())
            //{
            //    cities_c.sel.workTemplate.onFactionChange(cities_c.sel, workTemplate);
            //}
            citiesC.Reset();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
            {
                city.workTemplate.onFactionChange(city, workTemplate);
            }
        }

        void writeRelations(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < diplomaticRelations.Length; ++i)
            {
                if (diplomaticRelations[i] != null &&
                    diplomaticRelations[i].IsFactionOne(this))
                {
                    diplomaticRelations[i].write(w);
                }
            }
            w.Write(short.MinValue);
        }

        void readRelations(System.IO.BinaryReader r, int subVersion)
        {
            while (true)
            {
                DiplomaticRelation relation = new DiplomaticRelation();
                if (relation.read(r, subVersion))
                {
                    relation.addToFactions();
                }
                else
                {
                    break;
                }
            }
        }

        virtual public void writeNet(System.IO.BinaryWriter w)
        {
            w.Write((ushort)factiontype);
            //player.profile.flag.write(w);
            player.profile.write(w, true);

            writeRelations(w);

            if (factiontype == FactionType.Player)
            {
                player.GetHumanPlayer().networkPeer.writeNetID(w);
            }
        }

        virtual public void readNet(System.IO.BinaryReader r)
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

            player.profile.read(r);
            //FlagAndColor profile = new FlagAndColor(r);
            //SetProfile(profile);

            readRelations(r, int.MaxValue);

            if (factiontype == FactionType.Player)
            {
                Network.NetworkInstancePeer.ReadNetID(r, out AbsNetworkPeer peer, out int SplitScreenIndex);
                var player = DssRef.state.GetOrCreateRemotePlayer(peer, SplitScreenIndex);
                this.player = player;
                player.faction = this;
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
            cities.write_ushort(w);

            w.Write(availableForPlayer);
        }

        public void readMapFile(System.IO.BinaryReader r, int mapVersion, WorldData world)
        {
            int cityCount = r.ReadUInt16();

            for (int i = 0; i < cityCount; ++i)
            {
                int cityIx = r.ReadUInt16();
                AddCity(world.cities[cityIx], true);
            }

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
                city.OnNewOwner(newFaction, false);
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
                armies.HardSet(army, overrideIx);
            }
            army.factionIndex = myIndex;
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
                city.setFaction(this, duringStartUp, false);
            }
            else
            {

                if (!cities.Contains(city.myIndex))
                {
                    cities.Add(city.myIndex);
                    city.setFaction(this, duringStartUp, false);
                    if (!duringStartUp)
                    {
                        player.OnCityCapture(city);

                        city.workTemplate.setAllToFollowFaction();
                        city.workTemplate.onFactionChange(city, workTemplate);
                        city.defaultResourceBuffer(DssRef.world);

                        if (mainCity == null || mainCity.factionIndex != myIndex)
                        {
                            refreshMainCity();
                        }
                    }
                }
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
            
            while (armiesCounter.Next())
            {
                armiesCounter.sel.PauseUpdate();
            }
        }

        
        public void oneSecUpdate()
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
                else
                {
                    lib.DoNothing();
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
                    if (city.factionIndex == myIndex)
                    {
                        city.oneSecUpdate();
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


                if (DssRef.storage.gameRuleset.centralGold)
                {
                    money.copper += Convert.ToInt32(income);
                }
                else
                {
                    money = citiesTotalCopper;
                }

                previuosMoney = storeMoney;
                storeMoney = money;

                if (cities.Count == 0 && !player.protectedFromDelete)
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
            player.aiPlayerAsynchUpdate(time);
        }
        
        public void asynchGameObjectsUpdate(float time, float oneSecondUpdate, bool oneMinute)
        {
            float armiesStrength = 0;

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
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
                    p.unitsPixelTexture.updateColorProfile(this);
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

        //public bool canBuyMercenay(int count)
        //{
        //    return (workForce.max + ExpandWorkForce * count) <= maxEpandWorkSize;
        //}

        public void remove(Army army)
        {
            Debug.CrashIfThreaded();
            armies.RemoveAt_EqualSafeCheck(army, army.myIndex);
        }

        public void remove(City city)
        {   
            cities.Remove(city.myIndex);
            if (city == mainCity ||
               mainCity == null || mainCity.factionIndex != myIndex)
            {
                refreshMainCity();                     
            }

            if (player != null && player.IsLocalPlayer())
            {
                player.orders.refreshAvailable(this);

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    var localplayer = player.GetLocalPlayer();
                    if (localplayer.battleMessageCheck(city.tilePos))
                    {
                        MessageGroup_Ingame.Title(content, DssRef.lang.Message_LostCity);

                        var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
                        MessageGroup_Ingame.ControllerInputIcons(localplayer, gotoBattleButtonContent);
                        gotoBattleButtonContent.Add(new RbText(city.TypeName()));

                        content.Add(new ArtButton(RbButtonStyle.Primary, gotoBattleButtonContent,
                            new RbAction1Arg<AbsGameObject>(localplayer.hud.messages.goToMapObject, city)));

                        localplayer.hud.messages.Add(content);
                    }
                }));
            }
        }

        public void refreshMainCity()
        {
            if (mainCity != null && mainCity.factionIndex != myIndex)
            {
                mainCity = null;
            }

            if (mainCity == null || mainCity.cityType < CityType.Capital)
            {
                City largest = null;

                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
                {
                    
                    if (largest == null || city.HousingCount_Workers > largest.HousingCount_Workers)
                    {
                        largest = city;
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

        //void updateAreaCenter()
        //{
        //    if (mainCity != null)
        //    {
        //        landAreaCenter = mainCity.tilePos - IntVector2.One;
        //    }
        //    //IntVector2 center = IntVector2.Zero;
        //    //SpottedArrayCounter<City> cityCounter = new SpottedArrayCounter<City>(cities);
        //    //while (cityCounter.Next())
        //    //{
        //    //    center.Add(cityCounter.sel.tilePos);
        //    //}

        //    //int cityCount = cities.Count;
        //    //if (cityCount > 0f)
        //    //{
        //    //    center.X = Convert.ToInt32((float)center.X / cityCount);
        //    //    center.Y = Convert.ToInt32((float)center.Y / cityCount);
        //    //    center.Y -= 2;

        //    //    landAreaCenter = center;
        //    //    //IntVector2 newlandSymbolStart = center - DssLib.UserHeraldicHalfWidth;
        //    //    //newlandSymbolStart.X %= DssLib.UserHeraldicWidth;
        //    //    //newlandSymbolStart.Y %= DssLib.UserHeraldicWidth;
        //    //    //landSymbolStart = newlandSymbolStart;
        //    //}
        //}

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

        //public GameObject.AbsArmyUnit selectObject(Vector3 screenCenterPos)
        //{
        //    GameObject.AbsArmyUnit closestObj = null;
        //    float closestObjDistance = float.MaxValue;
        //    //foreach (City c in cities)
        //    cityCounter.Reset();

        //    while (cityCounter.Next())
        //    {
        //        distanceCheck(cityCounter.sel, screenCenterPos, ref closestObj, ref closestObjDistance);
        //    }

        //    armyCounter.Reset();
        //    while(armyCounter.Next())
        //    {
        //        distanceCheck(armyCounter.sel, screenCenterPos, ref closestObj, ref closestObjDistance);
        //    }

        //    return closestObj;
        //}

        void distanceCheck(GameObject.AbsMapObject obj, Vector3 screenCenterPos, 
            ref GameObject.AbsMapObject closestObj, ref float closestObjDistance)
        {
            float l = (obj.position - screenCenterPos).Length();
            if (l < closestObjDistance)
            {
                closestObj = obj;
                closestObjDistance = l;
            }
        }

//        public void WeeklyUpdate(UpdateArgs args)
//        {
//            //income = owner.ExtraIncome;
//            //if (owner is AbsHumanPlayer)
//            //    income += RTSlib.HumanPlayerExtaIncome;

//            cityCounter.Reset();

//            while (cityCounter.Next())
//            {
//                income += cityCounter.sel.GetWeekIncome();
//                cityCounter.sel.WeeklyUpdate(args);
//            }
//            upkeep = 0;
//            armyCounter.Reset();
//            while (armyCounter.Next())
//            {
//#if PCGAME
//                if (armyCounter.sel.faction != this)
//                    throw new Exception();
//#endif
//                upkeep += armyCounter.sel.Upkeep();
//                armyCounter.sel.WeeklyUpdate(args);
//            }

//            money += income - upkeep;

//            if (money < DssLib.MaxDept)
//            { 
//                //A part of the army will quit
//                armyCounter.Reset();
//                while (armyCounter.Next())
//                {
//                    armyCounter.sel.QuitFromDept();
//                }
//                //owner.OnDeptDeserters();
//            }

//            updateBanner();
//        }



        //public void NetUpdate()
        //{
        //    armyCounter.Reset();
        //    while (armyCounter.Next())
        //    {
        //        armyCounter.sel.NetUpdate();
        //    }
        //}

        //public void AsynchUpdate(AsynchUpdateArgs args)
        //{
        //    localArmyAsynchCounter.Reset();
        //    while (localArmyAsynchCounter.Next())
        //    {
        //        localArmyAsynchCounter.sel.AsynchUpdate(args);
        //    }
        //}

        //public bool payMoney(int cost)
        //{
        //    if (money >= cost)
        //    {
        //        //if (owner is LocalPlayer)
        //        //    LootFest.Music.SoundManager.PlayFlatSound(LoadedSound.buy);
        //        //money -= cost;
        //        //owner.MoneyChangeEvent();
        //        return true;
        //    }
        //    return false;
        //}

        //public GameObject.Army BuySoldiers(TroopType type, City city, int chunkCount, LocalPlayer p)
        //{
        //    if (payMoney(city.ArmyUnitCost.Get(type) * chunkCount))
        //    {
        //        const float AutoMergeLenght = DssLib.ArmyAttackRadius + 0.2f;
        //        Vector3 pos = city.SelectionCenter;

        //        armyCounter.Reset();
        //        while (armyCounter.Next())
        //        {
        //            float l = (pos - armyCounter.sel.SelectionCenter).Length();
        //            if ((pos - armyCounter.sel.SelectionCenter).Length() <= AutoMergeLenght)
        //            { //found a close by army to add soldiers to
        //                armyCounter.sel.addSoldiers(type, chunkCount);
        //                return armyCounter.sel;
        //            }
        //        }

        //        if (p != null && p.CheckUnitCountLimit())
        //        {
        //            GameObject.Army newArmy = new GameObject.Army(DssLib.TileToDrawPos_centered(DssRef.world.GetFreeTile(city.position)), this, type);
        //            return newArmy;
        //        }
        //    }
        //    return null;
        //}

        public void shareAllHostedObjects(Network.AbsNetworkPeer sender)
        {
            //if (owner != null && owner.LocalMember)
            //{
            //    armyCounter.Reset();
            //    while (armyCounter.Next())
            //    {
            //        armyCounter.sel.NetShare(sender.Id);
            //    }
            //}
        }

        //public void BattleEndResult(BattleCalculation2 battle, bool isWinner)
        //{
        //    //if (owner != null)
        //    //    owner.BattleResult(battle);

        //    if (isWinner)
        //    {
        //        bool human = battle.loser.faction.player is AbsHumanPlayer;
        //        foreach (AbsArmyUnit enemy in battle.loser.group)
        //        {
        //            if (enemy.Type == ObjectType.City)
        //            {
        //                VictoryPoints += human ? DssLib.VP_DefeatPlayerCity : DssLib.VP_DefeatCity;
        //            }
        //            else
        //            {
        //                VictoryPoints += DssLib.VP_DefeatArmy;
        //            }
        //        }
        //    }
        //}

        public void tradeAllianceWars(Faction otherFaction)
        {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        foreach (var m in otherFaction.diplomaticRelations)
                        {
                            if (m != null)
                            {
                                if (m.Relation <= RelationType.RelationTypeN3_War)
                                {
                                    var thirdFaction = m.opponent(otherFaction);

                                    var thisAndThirdRelation = diplomaticRelations[thirdFaction.myIndex];
                                    if (thisAndThirdRelation == null)
                                    {
                                        //Gain bad relation
                                        DssRef.diplomacy.SetRelationType(this, thirdFaction, m.Relation);
                                    }
                                    else
                                    {
                                        if (thisAndThirdRelation.Relation < RelationType.RelationType3_Ally)
                                        {
                                            //share worst relation
                                            RelationType worst = (RelationType)Math.Min((int)m.Relation, (int)thisAndThirdRelation.Relation);
                                            if (worst <= RelationType.RelationTypeN3_War)
                                            {
                                                DssRef.diplomacy.declareWar(this, thirdFaction);
                                            }
                                            else
                                            {
                                                DssRef.diplomacy.SetRelationType(this, thirdFaction, worst);
                                            }
                                        }
                                    }
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

        public void shareRelationWithAllAllies(Faction relationTo, RelationType relationType)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    DssRef.diplomacy.SetRelationType(this, relationTo, relationType);

                    for (int relIndex = 0; relIndex < diplomaticRelations.Length; relIndex++)//each (var m in diplomaticRelations)
                    {
                        if (diplomaticRelations[relIndex] != null)
                        {
                            if (diplomaticRelations[relIndex].Relation >= RelationType.RelationType3_Ally && relIndex != this.factionIndex)
                            {
                                Faction ally = DssRef.world.faction(relIndex);

                                if (ally != null)
                                {
                                    DssRef.diplomacy.SetRelationType(ally, relationTo, relationType);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            });
        }

        public void stopAllAttacksAgainst(Faction otherFaction)
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

        public void mergeTo(Faction masterFaction)
        {
            DeleteMe();

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.setFaction(masterFaction, false, true);
            }

            armies.Clear();

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                citySel.setFaction(masterFaction, false, true);                
            }

            cities.Clear();

            DssRef.world.BordersUpdated = true;
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
                DssRef.diplomacy.onFactionDeath(this);

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
                return player.Name;
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

        public SpeakTerms DefaultSpeakingTerms()
        {
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

        public List<Faction> CollectWars()
        {
            List<Faction> opponents = new List<Faction>();
            for (int relIx = 0; relIx < diplomaticRelations.Length; ++relIx)
            {
                if (diplomaticRelations[relIx] != null &&
                    relIx != myIndex &&
                   diplomaticRelations[relIx].Relation <= RelationType.RelationTypeN3_War)
                {
                    opponents.Add(DssRef.world.faction(relIx));
                }
            }

            return opponents;
        }

        public int CountWars()
        {
            int count = 0;
            for (int relIx = 0; relIx < diplomaticRelations.Length; ++relIx)
            {
                if (diplomaticRelations[relIx] != null &&
                    relIx != myIndex &&
                   diplomaticRelations[relIx].Relation <= RelationType.RelationTypeN3_War)
                {
                    ++count;
                }
            }

            return count;
        }


        /// <returns>Combined strength of allied nations (myself not included)</returns>
        public float CollectAllianceStrength()
        {
            float result = 0;

            for (int relIx = 0; relIx < diplomaticRelations.Length; ++relIx)
            {
                if (diplomaticRelations[relIx] != null &&
                    relIx != myIndex &&
                   diplomaticRelations[relIx].Relation >= RelationType.RelationType3_Ally)
                {
                    var ally = DssRef.world.faction(relIx);
                    if (ally != null)
                    {
                        result += ally.militaryStrength;
                    }
                }
            }

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

        public override bool aliveAndBelongTo(Faction faction)
        {
            return faction == this;
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
