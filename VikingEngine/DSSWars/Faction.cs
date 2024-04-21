using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;

using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    partial class Faction : AbsGameObject
    {
        //public int index;
        public Players.AbsPlayer player = null;
        public FlagAndColor profile;

        public GameObject.City mainCity;
        public Vector3 SelectionCenter { get; private set; }

        //public SpottedArrayCounter<GameObject.City> cityCounter;

        //public SpottedArrayCounter<GameObject.City> cityAsynchMainCounter;
        //public SpottedArrayCounter<GameObject.City> cityAsynchAiCounter;

        public SpottedArray<GameObject.City> cities;

        public Texture2D flagTexture;
        public int previousWarAgainstFaction = -1;
        public DiplomaticRelation[] diplomaticRelations = null;
        public DiplomaticSide diplomaticSide = DiplomaticSide.None;

        public bool textureLoaded = false;
        public Vector2 FlagTextureTargetSheetPos;
        public ModelTextureSettings FlagTexture = ModelTextureSettings.Default;

        public SpottedArray<Army> armies;
        public SpottedArrayCounter<Army> armiesCounter;

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

        public int mercenaryCost = DssRef.difficulty.MercenaryPurchaseCost_Start;

        public Faction()
        { }

        public Faction(WorldData addTo, FactionType factiontype)
        {
            this.factiontype = factiontype;

            this.parentArrayIndex = addTo.factions.Add(this);

            initVisuals(addTo.metaData);

            cities = new SpottedArray<GameObject.City>(8);

            //cityCounter = new SpottedArrayCounter<City>(cities);

            //cityAsynchMainCounter = new SpottedArrayCounter<City>(cities);
            //cityAsynchAiCounter = new SpottedArrayCounter<City>(cities);

            armies = new SpottedArray<Army>(16);
            armiesCounter = armies.counter();

            
        }

        public void onGameStart(bool newGame)
        {
            player.onGameStart(newGame);
        }

        public void initDiplomacy(WorldData world)
        {

            diplomaticRelations = new DiplomaticRelation[world.factions.Array.Length];
        }

        public void initVisuals(WorldMetaData worldMeta)
        {
            worldMeta.setObjSeed(parentArrayIndex);
            SetProfile(new FlagAndColor(factiontype, -1, worldMeta));
        }

        public void SetProfile(FlagAndColor profile)
        {
            this.profile = profile;
            flagTexture = profile.flagDesign.CreateTexture(profile);
        }
        virtual public void writeGameState(System.IO.BinaryWriter w)
        {
            //profile.write(w);

            w.Write(gold);

            w.Write((ushort)cities.Count);
            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                w.Write((ushort)citiesC.sel.parentArrayIndex);
            }
                       

            w.Write((ushort)armies.Count); 
            var armiesC = armiesCounter.Clone();
            while(armiesC.Next())
            { 
                armiesC.sel.writeGameState(w); 
            }
                        

            for (int i = 0; i < diplomaticRelations.Length; ++i)
            {
                if (diplomaticRelations[i] != null &&
                    diplomaticRelations[i].IsFactionOne(this))
                {
                    diplomaticRelations[i].write(w);
                }
            }
            w.Write(short.MinValue);//write end
                        

            player.writeGameState(w);

            
        }
        virtual public void readGameState(System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            //profile = new FlagAndColor(r);

            gold = r.ReadInt32();

            int citiesCount = r.ReadUInt16();
            for (int i = 0; i < citiesCount; i++)
            {
                int cityIx = r.ReadUInt16();
                var city = DssRef.world.cities[cityIx];
                //cities.Add(city);
                city.setFaction(this);
            }

            

            int armiesCount = r.ReadUInt16();
            for (int i = 0; i < armiesCount; i++)
            {
                var army = new Army();
                army.readGameState(this, r, version, pointers);
                //armies.Add(army);
            }

           
            while (true)
            { 
                DiplomaticRelation relation = new DiplomaticRelation();
                if (relation.read(r, version))
                {
                    relation.addToFactions();
                }
                else
                {
                    break;
                }
            }

            

            player.readGameState(r, version);

        }

        virtual public void writeNet(System.IO.BinaryWriter w)
        {

        }
        virtual public void readNet(System.IO.BinaryReader r)
        {

        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            w.Write((ushort)cities.Count);
            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                w.Write((ushort)citiesC.sel.parentArrayIndex);
            }

            w.Write(availableForPlayer);
        }

        public void readMapFile(System.IO.BinaryReader r, int version, WorldData world)
        {
            int cityCount = r.ReadUInt16();

            for (int i = 0; i < cityCount; ++i)
            {
                int cityIx = r.ReadUInt16();
                AddCity(world.cities[cityIx], true);
            }

            availableForPlayer= r.ReadBoolean();
        }

        public void OnFlagtextureLoaded()
        {
            if (!textureLoaded)
            {
                FlagTexture.SetSpriteName(SpriteName.NO_IMAGE);
                textureLoaded = true;
                onNewOwner();
            }
        }

        void onNewOwner()
        {
            if (!textureLoaded)
                FlagTexture.ColorAndAlpha = profile.getColor(ProfileColorType.Main).ToVector4();

            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                citiesC.sel.OnNewOwner();
            }
        }

        

        public Army NewArmy(IntVector2 startPos)
        {
            if (DssRef.state.PartyMode)
            {
                var army = new GameObject.Party.PartyArmy(this, startPos);
                return army;
            }
            else
            {
                var army = new Army(this, startPos);
                return army;
            }
        }

        public void AddArmy(Army army)
        { 
            army.parentArrayIndex = armies.Add(army);
            army.faction = this;
        }

        public void AddCity(City city, bool duringStartUp)
        {
            if (duringStartUp)
            {
                if (mainCity == null)
                    mainCity = city;
                else if (city.CityType > mainCity.CityType)
                {//larger city
                    mainCity = city;
                }
            }

            if (factiontype == FactionType.SouthHara)
            {
                lib.DoNothing();
            }

            if (!cities.Contains(city))
            {
                cities.Add(city);
                city.setFaction(this);
                if (!duringStartUp)
                {
                    player.OnCityCapture(city);

                    if (mainCity == null || mainCity.faction != this)
                    {
                        refreshMainCity();
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
            var armyC = armiesCounter.Clone();
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
            //if (player is Players.LocalPlayer)
            //{
            //    lib.DoNothing();
            //}

            armiesCounter.Reset();
            while (armiesCounter.Next())
            {
                armiesCounter.sel.update();
            }

            player.Update();
        }

        public void PauseUpdate()
        {
            armiesCounter.Reset();
            while (armiesCounter.Next())
            {
                armiesCounter.sel.PauseUpdate();
            }
        }

        public void oneSecUpdate()
        {
            resources_oneSecUpdate();
            player.oneSecUpdate();

            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                if (citiesC.sel.faction == this)
                {
                    citiesC.sel.oneSecUpdate();
                }
                else
                {
                    citiesC.RemoveAtCurrent();
                    refreshMainCity();
                }
            }

            if (!player.IsAi())
            {
                lib.DoNothing();
            }
            if (Ref.rnd.Chance(0.2))
            {
                desertersUpdate();
            }

            if (factiontype == FactionType.SouthHara)
            {
                lib.DoNothing();
            }

            if (armies.Count == 0 && cities.Count == 0)
            {
                bool protectedFaction = factiontype == FactionType.DarkLord ||
                    (factiontype == FactionType.SouthHara && DssRef.state.events.nextEvent <= EventType.SouthShips);
                                    
                if (!protectedFaction)
                {
                    DeleteMe();
                }
            }
        
        }

        void desertersUpdate()
        {
            if ( gold < 0)
            {
                //if (factiontype == FactionType.SouthHara)
                //{
                //    lib.DoNothing();
                //}
                int payDiff = -gold + armyUpkeep;

                if (hasDeserters && payDiff >= armyUpkeep * 5)
                {
                    
                    //Gain a portion of deserters on all armies
                    int totalDeserters = 0;

                    armiesCounter.Reset();
                    while (armiesCounter.Next())
                    {
                        totalDeserters += armiesCounter.sel.desertSoldiers();
                    }

                    if (player.IsPlayer())
                    {
                        player.GetLocalPlayer().hud.messages.Add("Deserters!", "Unpaid soldiers are deserting from your armies");
                        player.GetLocalPlayer().statistics.SoldiersDeserted += totalDeserters;
                    }
                }
            }
        }

        public void asynchAiPlayersUpdate(float time)
        {
            player.aiPlayerAsynchUpdate(time);
        }

        public void asynchGameObjectsUpdate(float time)
        {
            float totalStrength = 0;

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.asynchGameObjectsUpdate(time);
                totalStrength += armiesC.sel.strengthValue;
            }

            militaryStrength = totalStrength;

            resources_updateAsynch();
        }

        public void asynchSleepObjectsUpdate(float time)
        {
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.asynchSleepObjectsUpdate(time);
            }
        }

        public void asynchCullingUpdate(float time, bool bStateA)
        {
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
            armies.RemoveAt_EqualSafeCheck(army, army.parentArrayIndex);
        }

        public void remove(City city)
        {   
            cities.Remove(city);
            if (city == mainCity ||
               mainCity == null || mainCity.faction != this)
            {
                refreshMainCity();                     
            }
        }

        public void refreshMainCity()
        {
            if (mainCity != null && mainCity.faction != this)
            {
                mainCity = null;
            }

            if (mainCity == null || mainCity.CityType < CityType.Head)
            {
                City largest = null;

                var citiesC = cities.counter();

                while (citiesC.Next())
                {
                    if (largest == null || citiesC.sel.workForce.max > largest.workForce.max)
                    {
                        largest = citiesC.sel;
                    }
                }

                mainCity = largest;
            }
        
        }

        public IntVector2 landAreaCenter(out bool cityPosition)
        {
            if (mainCity != null)
            {
                cityPosition = true;
                return mainCity.tilePos - IntVector2.One;
            }
            else if (armies.Count > 0)
            {
                cityPosition = false;
                return armies.First().tilePos;
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

            armiesCounter.Reset();
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
            System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    foreach (var m in otherFaction.diplomaticRelations)
                    {
                        if (m != null)
                        {
                            if (m.Relation <= RelationType.RelationTypeN3_War)
                            {
                                var thirdFaction = m.opponent(otherFaction);

                                var thisAndThirdRelation = diplomaticRelations[thirdFaction.parentArrayIndex];
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
                                        DssRef.diplomacy.SetRelationType(this, thirdFaction, worst);
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }

        public void stopAllAttacksAgainst(Faction otherFaction)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                var armiesC = armies.counter();
                while (armiesC.Next())
                {
                    armiesC.sel.stopAllAttacksAgainst(otherFaction);
                }
            });
        }

        public void mergeTo(Faction masterFaction)
        {
            DeleteMe();

            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                armiesC.sel.setFaction(masterFaction);
            }

            armies.Clear();

            var citiesC = cities.counter();
            while (citiesC.Next())
            { 
                citiesC.sel.setFaction(masterFaction);
            }

            cities.Clear();

            DssRef.world.BordersUpdated = true;
        }

        public void SetNeighborToPlayer()
        {
            var citiesC = cities.counter();

            while (citiesC.Next())
            {
                citiesC.sel.SetNeighborToPlayer();
            }
        }
        public bool HasPlayerNeighbor()
        {
            var citiesC = cities.counter();

            while (citiesC.Next())
            {
                if (citiesC.sel.HasPlayerNeighbor())
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
                DssRef.diplomacy.onFactionDeath(this);

                if (factiontype == FactionType.SouthHara &&
                    DssRef.state.events.nextEvent <= EventType.DarkLord &&
                    DssRef.difficulty.bossTimeSettings <= BossTimeSettings.Early)
                {
                    DssRef.achieve.UnlockAchievement(AchievementIndex.early_hara);
                }
                else if (factiontype == FactionType.Player)
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
            if (player is Players.LocalPlayer)
            {
                return Owner.Name;
            }
            return "Faction " + parentArrayIndex.ToString();
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
            w.Write((byte)parentArrayIndex);
        }
        public Players.AbsPlayer Owner
        {
            get
            {
                return player;
            }
            set
            {
                if (player != value)
                {
                    player = value;
                    onNewOwner();
                }
            }
        }

        public FactionSize Size()
        {
            if (cityIncome <= DssLib.LargeCityMaxWorkForce * 2)
            {
                return FactionSize.Tiny;
            }
            else if (cityIncome <= DssLib.LargeCityMaxWorkForce * 6)
            {
                return FactionSize.Normal;
            }
            else if (cityIncome <= DssLib.LargeCityMaxWorkForce * 30)
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

        public RichBoxTexture FlagTextureToHud()
        {
            return new RichBoxTexture(flagTexture, 1f, 0, 0.2f);
        }

        public Color Color()
        {
                if (player == null)
                    return ColorExt.Error;
                return player.faction.profile.getColor(ProfileColorType.Main);
            
        }

        public SpeakTerms DefaultSpeakingTerms()
        {
            switch (factiontype)
            { 
                default: return SpeakTerms.SpeakTerms0_Normal;

                case FactionType.DarkLord:
                case FactionType.SouthHara:
                case FactionType.DarkFollower:
                case FactionType.GreenWood:
                    return SpeakTerms.SpeakTermsN2_None;

                case FactionType.UnitedKingdom:
                case FactionType.EasternEmpire:
                    return SpeakTerms.SpeakTermsN1_Bad;
            }
        }

        public bool WantToAllyAgainstDark()
        {
            return diplomaticSide == DiplomaticSide.Light &&
                DssRef.state.events.nextEvent >= EventType.DarkLord;
        }
        
        public override Faction GetFaction()
        {
            return this;
        }

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
    }

    enum FactionGroupType
    {
        Other,
        Nordic,
    }

    enum DiplomaticSide
    {
        None,
        Light,
        Dark,
    }
}
