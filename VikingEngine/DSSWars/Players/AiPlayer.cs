using System;
using System.Collections.Generic;
using System.IO;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;


namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer : AbsPlayer
    {
        public Time nextDecisionTimer = new Time(1000);

        const int PurchaseOrderType_None = 0;
        const int PurchaseOrderType_Army = 1;
        const int PurchaseOrderType_CityWorkers = 2;
        const int PurchaseOrderType_CityGuard = 3;
        const int PurchaseOrderType_MergeArmies = 4;

        const int PurchaseOrderFocus_None = 0;
        const int PurchaseOrderFocus_Defend = 1;
        const int PurchaseOrderFocus_QuickDefend = 2;
        const int PurchaseOrderFocus_AttackCity = 3;
        const int PurchaseOrderFocus_SeaTravel = 4;

        int purchaseOrder = PurchaseOrderType_None;
        int purchaseOrderFocus = PurchaseOrderFocus_None;
        int purchaseOrderIndex1 = -1;
        int purchaseOrderIndex2 = -1;
        bool purchaseIsMainArmy = false;

        int purchaseCount =-1;
        //public static double EconomyMultiplier = 1.0;
        string name;

        //Center attack focus and buy focus on the main army
        Army mainArmy = null;
        const int MainArmyState_StartNew = 0;
        const int MainArmyState_BuySoldiers = 1;
        const int MainArmyState_CollectSupport = 2;
        const int MainArmyState_Defend = 3;
        const int MainArmyState_Attack = 4;
        int mainArmyState = MainArmyState_StartNew;
        int mainArmyWar = -1;


        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write(IsPlayerNeighbor);
            w.Write((byte)aggressionLevel);
            w.Write(protectedPlayer);

        }
        public override void readGameState(BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            base.readGameState(r, version, pointers);

            IsPlayerNeighbor = r.ReadBoolean();
            aggressionLevel = r.ReadByte();
            protectedPlayer = r.ReadBoolean();
        }

        public AiPlayer(Faction faction)
            : base(faction)
        {
            
            faction.profile.gameStartInit();

            switch (faction.factiontype)
            {
                case FactionType.DefaultAi:
                    var chance = Ref.rnd.Double();
                    if (chance < 0.08)
                    {
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                    }
                    else if (chance < 0.25)
                    {
                        aggressionLevel = AggressionLevel2_RandomAttacks;
                    }
                    else if (chance < 0.4)
                    {
                        aggressionLevel = AggressionLevel1_RevengeOnly;
                    }
                    else
                    {
                        aggressionLevel = AggressionLevel0_Passive;
                    }
                    
                    name = string.Format(DssRef.lang.FactionName_GenericAi, faction.parentArrayIndex);
                    break;

                case FactionType.DarkLord:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.5f;
                    name = DssRef.lang.FactionName_DarkLord;
                    faction.displayInFullOverview = true;
                    break;

                case FactionType.DarkFollower:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_DarkFollower = faction.parentArrayIndex;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.5f;
                    name = DssRef.lang.FactionName_DarkFollower;
                    faction.displayInFullOverview = true;
                    faction.gold += DssConst.HeadCityStartMaxWorkForce * 10;
                    break;

                case FactionType.UnitedKingdom:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_UnitedKingdom = faction.parentArrayIndex;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    name = DssRef.lang.FactionName_UnitedKingdom;
                    faction.displayInFullOverview = true;
                    break;

                case FactionType.GreenWood:
                    faction.diplomaticSide = DiplomaticSide.Light;
                    DssRef.settings.Faction_GreenWood = faction.parentArrayIndex;

                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.growthMultiplier = 0.75f;
                    name = DssRef.lang.FactionName_Greenwood;
                    //addStartCitiesBuyOption(UnitType.GreenSoldier);
                    break;

                case FactionType.EasternEmpire:
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    name = DssRef.lang.FactionName_EasternEmpire;
                    break;

                case FactionType.NordicRealm:
                    faction.grouptype = FactionGroupType.Nordic;
                    faction.diplomaticSide = DiplomaticSide.Light;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_NordicRealm;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    break;

                case FactionType.BearClaw:
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_BearClaw;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    break;

                case FactionType.NordicSpur:
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_NordicSpur;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    break;

                case FactionType.IceRaven:
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_IceRaven;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    break;

                case FactionType.DragonSlayer:
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = Ref.rnd.Chance(0.4) ? AggressionLevel2_RandomAttacks : AggressionLevel1_RevengeOnly;
                    name = DssRef.lang.FactionName_Dragonslayer;
                    //addStartCitiesBuyOption(UnitType.CrossBow);
                    break;

                case FactionType.SouthHara:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_SouthHara = faction.parentArrayIndex;

                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.1f;
                    faction.hasDeserters = false;
                    name = DssRef.lang.FactionName_SouthHara;
                    faction.displayInFullOverview = true;
                    faction.gold += DssConst.HeadCityStartMaxWorkForce * 5;
                    break;

                case FactionType.DyingMonger:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_DyingMonger = faction.parentArrayIndex;

                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.growthMultiplier = 4f;
                    faction.hasDeserters = false;
                    name = "Monger";
                    faction.gold += DssConst.HeadCityStartMaxWorkForce * 1000;
                    break;

                case FactionType.DyingHate:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_DyingHate = faction.parentArrayIndex;

                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.growthMultiplier = 4f;
                    faction.hasDeserters = false;
                    name = "Hatu";
                    faction.gold += DssConst.HeadCityStartMaxWorkForce * 1000;
                    break;

                case FactionType.DyingDestru:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_DyingDestru = faction.parentArrayIndex;

                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.growthMultiplier = 4f;
                    faction.hasDeserters = false;
                    name = "Destru";
                    faction.gold += DssConst.HeadCityStartMaxWorkForce * 1000;
                    break;


                default:
                    throw new NotImplementedException("ai player " + faction.factiontype);
            }

            refreshAggression();
        }

        public void refreshAggression()
        {
            int prioAdd = 0;
            if (aggressionLevel >= AggressionLevel2_RandomAttacks)
            {
                faction.workTemplate.craft_heavyarmor.value = 5;
            }
            else if (aggressionLevel == AggressionLevel1_RevengeOnly)
            {
                prioAdd = -1;
            }
            else
            {
                prioAdd = -2;
            }

            faction.workTemplate.craft_mediumarmor.value = 4 + prioAdd;
            faction.workTemplate.craft_lightarmor.value = 3 + prioAdd;

            faction.workTemplate.craft_sword.value = 5 + prioAdd;
            faction.workTemplate.craft_bow.value = 4 + prioAdd;
            faction.workTemplate.craft_sharpstick.value = 3 + prioAdd;
        }



        //void addStartCitiesBuyOption(UnitType unitType)
        //{
        //    var typeData = DssRef.profile.Get(unitType);
        //    var citiesC = faction.cities.counter();

        //    while (citiesC.Next())
        //    {
        //        citiesC.sel.cityPurchaseOptions.Add(new CityPurchaseOption()
        //        {
        //            unitType = unitType,
        //            goldCost = typeData.goldCost,
        //        });
        //    }
        //}
        
        public override void createStartUnits()
        {
            if (faction.cities.Count > 0)
            {

                Army mainArmy = null;

                switch (faction.factiontype)
                {
                    default:
                        mainArmy = startMainArmy();
                        for (int i = 0; i < 5; ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_Standard);//, UnitType.Soldier, false);
                        }
                        break;

                    case FactionType.UnitedKingdom:
                        {
                            var citiesC = faction.cities.counter();
                            while (citiesC.Next())
                            {
                                if (citiesC.sel.CityType == CityType.Large)
                                {
                                    IntVector2 pos = DssRef.world.GetFreeTile(citiesC.sel.tilePos);
                                    var army = faction.NewArmy(pos);

                                    for (int i = 0; i < 10; ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard);
                                    }

                                    army.OnSoldierPurchaseCompleted();
                                    army.setMaxFood();
                                }
                            }
                        }
                        break;

                    case FactionType.GreenWood:
                        mainArmy = startMainArmy();
                        for (int i = 0; i < 5; ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_GreenSoldier);//UnitType.GreenSoldier, false);
                        }
                        break;

                    case FactionType.NordicRealm:
                    case FactionType.BearClaw:
                    case FactionType.NordicSpur:
                    case FactionType.IceRaven:
                        mainArmy = startMainArmy();
                        for (int i = 0; i < 5; ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_Viking);
                        }
                        break;

                    case FactionType.DyingMonger:
                    case FactionType.DyingHate:
                    case FactionType.DyingDestru:
                        {
                            var citiesC = faction.cities.counter();
                            while (citiesC.Next())
                            {
                                if (citiesC.sel.CityType == CityType.Large)
                                {
                                    IntVector2 pos = DssRef.world.GetFreeTile(citiesC.sel.tilePos);
                                    var army = faction.NewArmy(pos);

                                    for (int i = 0; i < 10; ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard);//UnitType.HonorGuard, false);
                                    }
                                    for (int i = 0; i < 30; ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_StandardArcher); //UnitType.Archer, false);
                                    }
                                    //for (int i = 0; i < 20; ++i)
                                    //{
                                    //    new SoldierGroup(army, UnitType.Ballista, false);
                                    //}
                                    for (int i = 0; i < 100; ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_Standard); //UnitType.Soldier, false);
                                    }
                                    //for (int i = 0; i < 20; ++i)
                                    //{
                                    //    new SoldierGroup(army, UnitType.Knight, false);
                                    //}

                                    army.OnSoldierPurchaseCompleted();
                                    army.setMaxFood();
                                }
                            }
                        }
                        break;
                }

                if (mainArmy != null)
                {
                    mainArmy.OnSoldierPurchaseCompleted();
                    mainArmy.setMaxFood();
                }
            }

            Army startMainArmy()
            {
                IntVector2 onTile = DssRef.world.GetFreeTile(faction.mainCity.tilePos);
                return faction.NewArmy(onTile);
            }
        }

        public override void Update()
        {
            base.Update();

            if (IsLocal)
            {
                if (faction.factiontype == FactionType.SouthHara)
                {
                    lib.DoNothing();
                }
                if (purchaseOrder !=  PurchaseOrderType_None)
                {
                    

                    if (purchaseOrder == PurchaseOrderType_MergeArmies)
                    {
                        var armiesCounter = faction.armies.counter();
                        int found = 0;
                        Army army1 = null;
                        Army army2 = null;

                        while (armiesCounter.Next() && found < 2)
                        {
                            if (armiesCounter.sel.parentArrayIndex == purchaseOrderIndex1)
                            {
                                army1 = armiesCounter.sel;
                                ++found;
                            }
                            else if (armiesCounter.sel.parentArrayIndex == purchaseOrderIndex2)
                            {
                                army2 = armiesCounter.sel;
                                ++found;
                            }
                        }

                        if (army1!= null && army2 != null)
                        {
                            army1.mergeArmies(army2);
                        }
                    }
                    else
                    {
                        var city = DssRef.world.cities[purchaseOrderIndex1];
                        if (city.faction.player == this)
                        {
                            switch (purchaseOrder)
                            {
                                case PurchaseOrderType_Army:
                                    buySoldiers(city, true, true);
                                    break;
                                case PurchaseOrderType_CityWorkers:
                                    if (city.damages.HasValue())
                                    {
                                        city.buyRepair(true, true);
                                    }
                                    //else
                                    //{
                                    //    city.buyWorkforce(true, 1);
                                    //}
                                    break;
                                case PurchaseOrderType_CityGuard:
                                    city.buyCityGuards(true, 1);
                                    break;
                            }

                        }
                    }
                    purchaseOrder = PurchaseOrderType_None;
                }
            }
        }

        public override void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType building, out bool intelligent)
        {
            building = BuildAndExpandType.NUM_NONE;
            intelligent = false;
            work = false;

            if (city.res_rawFood.needMore())
            {
                building = BuildAndExpandType.WheatFarm;
            }
            else if (city.res_skinLinnen.needMore())
            {
                building = BuildAndExpandType.LinenFarm;
            }
            else if (city.barracks.Count < 2)
            {
                building = BuildAndExpandType.Barracks;
            }
            else if (city.deliveryServices.Count < 2)
            {
                building = BuildAndExpandType.Postal;
            }
            else 
            {
                intelligent = true;
                work = true;
            }

            //return result;
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();
            ignorePlayerCapture = false;
        }

        //void buySoldiers(City city)
        //{
        //    MainWeapon[] conscriptWeaponPrioOrder =
        //    {
        //        MainWeapon.Sword,
        //        MainWeapon.Bow,
        //        MainWeapon.SharpStick
        //    };

        //    ArmorLevel[] conscriptArmorPrioOrder =
        //    {
        //        ArmorLevel.Heavy,
        //        ArmorLevel.Medium,
        //        ArmorLevel.Light,
        //    };
        //    if (city.barracks.Count > 0)
        //    {
        //        ItemResourceType weaponItem = ConscriptProfile.WeaponItem(status.inProgress.weapon);
        //    }

        //    return;

        //    if (faction.MoneySecDiff() > DssLib.SoldierDefaultUpkeep * purchaseCount)
        //    {
        //        Army army;
        //        switch (faction.factiontype)
        //        {
        //            case FactionType.DarkLord:
        //                {
        //                    int cannons = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.6), purchaseCount);
        //                    int archers = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.4), purchaseCount);
        //                    int soldiers = purchaseCount - cannons * 2 - archers;

        //                    city.buySoldiers(UnitType.Trollcannon, cannons, true, out army, true);
        //                    city.buySoldiers(UnitType.CrossBow, archers, true, out army, true);
        //                    city.buySoldiers(UnitType.Pikeman, soldiers, true, out army, true);

        //                    if (DssRef.state.events.nextEvent == EventType.DarkLordInPerson)
        //                    {
        //                        city.buySoldiers(UnitType.HonorGuard, 6, true, out army, true);
        //                        city.buySoldiers(UnitType.DarkLord, 1, true, out army, true);
        //                    }
        //                }
        //                break;

        //            case FactionType.GreenWood:
        //                city.buySoldiers(UnitType.GreenSoldier, purchaseCount, true, out army, true);
        //                break;

        //            case FactionType.NordicRealm:
        //            case FactionType.BearClaw:
        //            case FactionType.NordicSpur:
        //            case FactionType.IceRaven:
        //                city.buySoldiers(UnitType.Viking, purchaseCount, true, out army, true);
        //                break;

        //            default:
        //                switch (purchaseOrderFocus)
        //                {
        //                    case PurchaseOrderFocus_QuickDefend:
        //                        city.buySoldiers(UnitType.Folkman, purchaseCount, true, out army);
        //                        break;

        //                    case PurchaseOrderFocus_Defend:
        //                        if (!city.buySoldiers(UnitType.Knight, purchaseCount / 2, true, out army))
        //                        {
        //                            city.buySoldiers(UnitType.Soldier, purchaseCount, true, out army);
        //                        }
        //                        break;

        //                    case PurchaseOrderFocus_AttackCity:
        //                        {
        //                            int ballistas = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.6), purchaseCount);
        //                            int archers = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.4), purchaseCount);
        //                            int soldiers = purchaseCount - ballistas - archers;

        //                            city.buySoldiers(UnitType.Ballista, ballistas, true, out army);
        //                            city.buySoldiers(UnitType.Archer, archers, true, out army);
        //                            city.buySoldiers(UnitType.Soldier, soldiers, true, out army);
        //                        }
        //                        break;

        //                    default:
        //                        {
        //                            int knights = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.8), purchaseCount);
        //                            int ballistas = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.2), purchaseCount);
        //                            int archers = MathExt.MultiplyInt(Ref.rnd.Double(0.0, 0.2), purchaseCount);
        //                            int soldiers = purchaseCount - ballistas - archers - knights * 2;

        //                            city.buySoldiers(UnitType.Knight, knights, true, out army);
        //                            city.buySoldiers(UnitType.Ballista, ballistas, true, out army);
        //                            city.buySoldiers(UnitType.Archer, archers, true, out army);
        //                            city.buySoldiers(UnitType.Soldier, soldiers, true, out army);
        //                        }
        //                        break;
        //                }
        //                break;
        //        }
        //        purchaseOrderFocus = PurchaseOrderFocus_None;

        //        if (purchaseIsMainArmy)
        //        {
        //            mainArmy = army;
        //            purchaseIsMainArmy = false;
        //        }
        //    }
        //}

        //bool haveIncomeForArmyPurchase(bool aggresive)
        //{
        //    if (aggresive)
        //    {
        //        return faction.gold >= DssRef.settings.AiArmyPurchase_MoneyMin_Aggresive &&
        //            faction.MoneySecDiff() >= DssRef.settings.AiArmyPurchase_IncomeMin_Aggresive;
        //    }
        //    else
        //    {
        //        return faction.gold >= DssRef.settings.AiArmyPurchase_MoneyMin &&
        //            faction.MoneySecDiff() >= DssRef.settings.AiArmyPurchase_IncomeMin;
        //    }
        //}

        override public void aiPlayerAsynchUpdate(float time)
        {
            if (faction.factiontype == FactionType.SouthHara)
            {
                lib.DoNothing();
            }

            if (StartupSettings.RunAI && nextDecisionTimer.CountDownGameTime(time))
            {
                if (faction.cities.Count == 0)
                {
                    mainArmy = null;
                    if (faction.armies.Count == 0)
                    {
                        nextDecisionTimer.MilliSeconds = 10000;
                        return;
                    }
                    
                }
                if (faction.factiontype == FactionType.SouthHara)
                { 
                    lib.DoNothing();
                }

                nextDecisionTimer.MilliSeconds = Ref.rnd.Float(2000, 5000);
                
                bool protect = Ref.rnd.Chance(0.6);

                var wars = DssRef.diplomacy.aiPlayerAsynchUpdate_collectWars(faction);
                bool inWar = aggressionLevel >= AggressionLevel2_RandomAttacks ||
                    (aggressionLevel == AggressionLevel1_RevengeOnly && wars.Count > 0);

                
                if (inWar && Ref.rnd.Chance(aggressionLevel == AggressionLevel2_RandomAttacks ? 0.05 : 0.3) &&
                    !mainArmyLockedInTravel())
                {
                    mainArmy_AsyncUpdate(wars);
                }
                else if (protect) //&& buySoldiers(//haveIncomeForArmyPurchase(inWar))
                {
                    City city = faction.cities.GetRandomSafe(Ref.rnd);

                    if (city != null && buySoldiers(city, inWar, false))
                    {
                        int maxPurchaseCount = 30;
                        if (inWar)
                        {
                            maxPurchaseCount = MathExt.MultiplyInt(DssRef.difficulty.aiEconomyMultiplier, maxPurchaseCount);
                        }
                        createPurcaseOrder(city, maxPurchaseCount);
                    }
                }
                else if (inWar)
                {
                    searchAttackTarget(wars);
                }
                

                async_buildUpCheck();

                //Merge armies check

                MergeArmiesCheck();

                decisionTimerSizeCheck();

            }
        }

        void decisionTimerSizeCheck()
        {
            float multiply = 1;

            switch (faction.Size())
            {
                case FactionSize.Tiny:
                    multiply = 1.6f;
                    break;
                case FactionSize.Big:
                    multiply = 0.6f;
                    break;
                case FactionSize.Giant:
                    multiply = 0.2f;
                    break;

            }

            nextDecisionTimer.MilliSeconds*= multiply;
        }


        void async_buildUpCheck()
        {
            if (purchaseOrder == PurchaseOrderType_None &&
                faction.gold > DssLib.GroupDefaultCost * 10)
            { 
                var city = faction.cities.GetRandomUnsafe(Ref.rnd);

                if (city != null && !city.InBattle())
                {
                    int friendCount = 0;
                    int enemyCount = 0;

                    foreach (var n in city.neighborCities)
                    {
                        if (DssRef.world.cities[n].faction == faction)
                        {
                            friendCount++;
                        }
                        else
                        { 
                            enemyCount++;
                        }
                    }

                    if (friendCount > enemyCount)
                    {
                        purchaseOrder = PurchaseOrderType_CityWorkers;
                    }
                    else
                    {
                        purchaseOrder = PurchaseOrderType_CityGuard;
                    }

                    purchaseOrderIndex1 = city.parentArrayIndex;
                }
            }
        }

        private void createPurcaseOrder(City city, int maxPurchaseCount)
        {
            purchaseCount = Ref.rnd.Int(5, maxPurchaseCount);
            purchaseOrder = PurchaseOrderType_Army;
            purchaseOrderIndex1 = city.parentArrayIndex;

        }

        void mainArmy_AsyncUpdate(List<int> wars)
        {

            if (emptyMainArmy() ||
                mainArmy.InBattle())
            {
                mainArmyState = MainArmyState_StartNew;
                if (faction.armies.Count > 0)
                {
                    //Try find large army
                    const int Trials = 3;
                    for (int i = 0; i < Trials; i++)
                    {
                        var army = faction.armies.GetRandomUnsafe(Ref.rnd);
                        if (army != null && army.IdleObjetive() && army.groups.Count >= 5)
                        {
                            mainArmy = army;
                            mainArmyState = MainArmyState_CollectSupport;
                            break;
                        }
                    }
                }
            }


            if (mainArmyState == MainArmyState_StartNew)
            {
                //bool haveIncome = faction.NetIncome() >= 0 &&
                //    faction.gold >= DssLib.GroupDefaultCost * 5;
                City city = null;
                city = cityCloseToCityInDanger(cityInDanger());

                if (city != null)
                {
                    purchaseOrderFocus = PurchaseOrderFocus_Defend;
                }
                else
                {
                    int war = findMainWar(wars);

                    if (war >= 0)
                    {
                        //find close city
                        city = cityCloseToOpponent(war);
                    }
                    else
                    {
                        city = cityCloseToNewTarget();
                    }
                    purchaseOrderFocus = PurchaseOrderFocus_AttackCity;
                }

                //if (haveIncomeForArmyPurchase(true))
                if (city != null && buySoldiers(city, true, false))
                {
                    //Start fresh
                    mainArmy = null;
                    
                    nextDecisionTimer.MilliSeconds += Ref.rnd.Int(4000, 15000);
                    mainArmyBuyAtCity(city);
                    
                }
                else
                {
                    Army army = StrongIdleArmy();

                    if (army != null)
                    {
                        mainArmy = army;
                        mainArmyState = MainArmyState_BuySoldiers;
                    }
                }
            }
            else if (mainArmyState == MainArmyState_BuySoldiers)
            {
                AbsMapObject city = null;

                //Begin with defence check
                city = cityInDanger();

                if (city != null)
                {
                    //Purchase some support for the city
                    buyDefenceAtCity((City)city);

                    float l = city.distanceTo(mainArmy);
                    float percDist = 1f - l / 64;
                    double chance = 0.2 + percDist;

                    if (Ref.rnd.Chance(chance))
                    {
                        mainArmyState = MainArmyState_Defend;

                        nextDecisionTimer.MilliSeconds += 4000;
                        if (city.distanceTo(mainArmy) > 2)
                        {
                            mainArmy.Order_MoveTo(city.tilePos);
                        }
                    }
                    else
                    {
                        city = null;
                    }                    
                }

                if (city == null)
                {
                    int war = findMainWar(wars);

                    if (war < 0)
                    {
                        //Start new war
                        city = AttackRamdom(mainArmy);
                        if (city != null)
                        {
                            mainArmyWar = city.faction.parentArrayIndex;
                        }
                    }
                    else
                    {
                        var opponent = DssRef.world.factions.Array[war];
                        city = AttackFaction(mainArmy, opponent);
                    }

                    mainArmyState = MainArmyState_Attack;
                }

                if (city != null)
                {
                    collectLooseArmies(city.tilePos);
                }
                else
                {
                    mainArmyState = MainArmyState_CollectSupport;
                }
            }
            else if (mainArmyState == MainArmyState_Attack ||
                mainArmyState == MainArmyState_Defend)
            {
                if (mainArmy.IdleObjetive())
                {
                    mainArmyState = MainArmyState_CollectSupport;
                }
            }
            else if (mainArmyState == MainArmyState_CollectSupport)
            {
                if (Ref.rnd.Chance(0.2))
                {
                    mainArmy = null;
                }
                else
                {
                    Tile tile;
                    if (DssRef.world.tileGrid.TryGet(mainArmy.tilePos, out tile))
                    {
                        var city = tile.City();
                        if (city.faction == faction)
                        {
                            if (city.distanceTo(mainArmy) <= 2)
                            {
                                collectLooseArmies(city.tilePos);
                                mainArmyBuyAtCity(city);
                                mainArmyState = MainArmyState_BuySoldiers;
                            }
                            else
                            {
                                mainArmy.Order_MoveTo(city.tilePos);
                            }
                        }
                    }

                }
            }
        }

        private int findMainWar(List<int> wars)
        {
            int war = -1;
            if (wars.Count > 0)
            {
                if (wars.Contains(mainArmyWar))
                {
                    war = mainArmyWar;
                }
                else
                {
                    war = arraylib.RandomListMember(wars);
                }
            }

            return war;
        }

        bool emptyMainArmy()
        {
            if (mainArmy == null ||
                 mainArmy.isDeleted ||
                 mainArmy.groups.Count < 4)
            {
                mainArmy = null;
                return true;
            }
            return false;
        }

        bool mainArmyLockedInTravel()
        {
            if (emptyMainArmy())
            {
                return false;
            }
            if (mainArmyState == MainArmyState_Attack || mainArmyState == MainArmyState_Defend)
            {
                return !mainArmy.IdleObjetive();
            }
            return false;
        }

        private void mainArmyBuyAtCity(City city)
        {
            //int max = Math.Min(faction.gold / DssLib.GroupDefaultCost, city.workForce / DssConst.SoldierGroup_DefaultCount);
            
            //if (max >= 4)
            //{
            //    purchaseCount = Ref.rnd.Int(MathExt.MultiplyInt(0.5, max), max);

            //    if (purchaseCount >= 4)
            //    {
                    mainArmyState = MainArmyState_BuySoldiers;

                    purchaseIsMainArmy = true;
                    purchaseOrder = PurchaseOrderType_Army;
                    purchaseOrderIndex1 = city.parentArrayIndex;

                    collectLooseArmies(city.tilePos);
            //    }

            //}
        }

        void buyDefenceAtCity(City city)
        {   
            //int max = Math.Min(faction.gold / DssLib.GroupDefaultCost, city.workForce / DssConst.SoldierGroup_DefaultCount);

            //if (max >= 4)
            //{
            //    purchaseCount = Ref.rnd.Int(MathExt.MultiplyInt(0.3, max), MathExt.MultiplyInt(0.6, max));

            //    if (purchaseCount >= 4)
            //    {
                if (buySoldiers(city, true, false))
                {
                    purchaseOrder = PurchaseOrderType_Army;
                    purchaseOrderFocus = PurchaseOrderFocus_QuickDefend;
                    purchaseOrderIndex1 = city.parentArrayIndex;
                }

            //}
        }

        void collectLooseArmies(IntVector2 toPos)
        {
            DssRef.world.unitCollAreaGrid.collectArmies(faction, toPos, 2, DssRef.world.unitCollAreaGrid.armies_aiUpdate);

            foreach (var a in DssRef.world.unitCollAreaGrid.armies_aiUpdate)
            {
                double chance = a.objective == ArmyObjective.None ? 0.8 : 0.1;
                if (a != mainArmy && Ref.rnd.Chance(chance))
                {
                    a.Order_MoveTo(toPos);
                }
            }
        }

        City cityCloseToCityInDanger(City inDanger)
        {
            if (inDanger == null)
            {
                return null;
            }

            City city = null;

            foreach (int m in inDanger.neighborCities)
            {
                City c = DssRef.world.cities[m];
                if (c.faction == faction)
                {
                    if (city == null)
                    {
                        city = c;
                    }
                    else if (c.workForce > city.workForce)
                    {
                        city = c;
                    }
                }
            }

            return city;
        }

        City cityInDanger()
        { 
            City checkCity1 = null;
            City checkCity2 = null;

            checkCity1 = faction.cities.GetRandomUnsafe(Ref.rnd);
            if (checkCity1 == null)
            {
                return null;
            }

            if (check(checkCity1))
            {
                return checkCity1;
            }

            checkCity2 = faction.cities.GetRandomUnsafe(Ref.rnd);
            if (checkCity2 == null)
            {
                return null;
            }

            if (checkCity2 != checkCity1 && check(checkCity2))
            {
                return checkCity2;
            }

            return null;

            bool check(City city)
            {
                DssRef.world.unitCollAreaGrid.collectOpponentArmies(faction, city.tilePos, 1, DssRef.world.unitCollAreaGrid.armies_aiUpdate);

                foreach (var army in DssRef.world.unitCollAreaGrid.armies_aiUpdate)
                { 
                    float dist = city.distanceTo(army);

                    if (dist <= 4)
                    {
                        return true;
                    }

                    if (DssRef.diplomacy.InWar(faction, army.faction))
                    {
                        if (dist <= 8)
                        {
                            return true;
                        }

                        if (army.attackTarget == city ||
                            city.distanceTo(army.walkGoal) <= 4)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        City cityCloseToOpponent(int opponent)
        {
            Faction otherFaction = DssRef.world.factions[opponent];
            City myClosestCity = null;
            float closestDistance = float.MaxValue;
           
            const int TrialCount = 3;
            for (int i = 0; i < TrialCount; ++i)
            {
                City myCity = faction.cities.GetRandomUnsafe(Ref.rnd);
                if (myCity == null)
                {
                    return null;
                }

                for (int j = 0; j < TrialCount; ++j)
                {
                    City otherCity = otherFaction.cities.GetRandomUnsafe(Ref.rnd);
                    if (otherCity == null)
                    {
                        return null;
                    }

                    float l = (otherCity.tilePos - myCity.tilePos).Length();

                    if (l < closestDistance)
                    { 
                        closestDistance = l;
                        myClosestCity = myCity;
                    }
                }
                
            }

            return myClosestCity;
        }

        City cityCloseToNewTarget()
        {
            City city=null;
            Faction weakestOpponent = null;

            const int TrialCount = 3;
            for (int i = 0; i < TrialCount; ++i)
            {
                City myCity = faction.cities.GetRandomUnsafe(Ref.rnd);
                if (myCity == null)
                {
                    return null;
                }

                foreach (var m in myCity.neighborCities)
                {
                    City c = DssRef.world.cities[m];
                    if (c.faction != faction && c.faction != weakestOpponent)
                    {
                        if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium &&
                            c.faction.player.IsPlayer())
                        {
                            return myCity;
                        }

                        if (weakestOpponent == null)
                        {
                            city = myCity;
                            weakestOpponent = c.faction;
                        }
                        else if (c.faction.militaryStrength < weakestOpponent.militaryStrength)
                        {
                            city = myCity;
                            weakestOpponent = c.faction;
                        }
                    }
                }
            }

            return city;
        }

        private void MergeArmiesCheck()
        {
            var armyC = faction.armies.counter();
            while (armyC.Next())
            {
                //if (armyC.sel.ai.objective == ArmyObjective.None)
                //{
                    var otherArmy = DssRef.world.unitCollAreaGrid.AdjacenToArmy(faction, armyC.sel, armyC.sel.tilePos, Army.MaxTradeDistance +1);
                    if (otherArmy != null)
                    {
                        purchaseOrder = PurchaseOrderType_MergeArmies;

                        if (armyC.sel.groups.Count > otherArmy.groups.Count)
                        {
                            purchaseOrderIndex2 = armyC.sel.parentArrayIndex;
                            purchaseOrderIndex1 = otherArmy.parentArrayIndex;
                        }
                        else
                        {
                            purchaseOrderIndex1 = armyC.sel.parentArrayIndex;
                            purchaseOrderIndex2 = otherArmy.parentArrayIndex;
                        }

                        break;
                    }
                //}
            }
        }

        void searchAttackTarget(List<int> wars)
        {
            
            if (faction.armies.Count > 0)
            {
                Army army = StrongIdleArmy();

                if (faction.cities.Count == 0 && Ref.rnd.Chance(0.5))
                {
                    AttackRamdom(army);
                }
                else if (army != null &&
                    (army != mainArmy || Ref.rnd.Chance(0.25)))
                {
                    if (
                        wars.Count == 0 ||
                        (aggressionLevel == AggressionLevel2_RandomAttacks && Ref.rnd.Chance(0.5))
                        )
                    {
                        //Start new war
                        AttackRamdom(army);
                    }
                    else
                    {
                        var opponent = DssRef.world.factions.Array[arraylib.RandomListMember(wars)];
                        if (opponent != null)
                        {
                            AttackFaction(army, opponent);
                        }
                    }
                }
            }
        }

        private Army StrongIdleArmy()
        {
            if (faction.armies.Count == 0)
            {
                return null;
            }
            
            Army army = null;
           
            for (int i = 0; i < 3; i++)
            {
                var army2 = faction.armies.GetRandomUnsafe(Ref.rnd);
                if (army2 != null && army2.IdleObjetive())
                {
                    if (army == null ||
                        army2.strengthValue > army.strengthValue)
                    {
                        army = army2;
                    }
                }
            }
            

            return army;
        }

        AbsMapObject AttackFaction(Army army, Faction opponent)
        {
            if (army != null)
            {
                var areaPos = UnitCollAreaGrid.ToAreaPos(army.tilePos);
                DssRef.world.unitCollAreaGrid.collectCitiesAndArmies(areaPos, 2, army.strengthValue * 0.8f, DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate,
                    null, opponent);
                if (DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate.Count > 0)
                {
                    AbsMapObject result = arraylib.RandomListMember(DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate);
                    army.Order_Attack(result);
                    return result;
                }
            }
            return null;
        }

        City AttackRamdom(Army army)
        {
            if (army != null)
            {
                var areaPos = UnitCollAreaGrid.ToAreaPos(army.tilePos);

                int compareCityCount = 4;

                DssRef.world.unitCollAreaGrid.collectCities_fromArea(areaPos,
                    compareCityCount, DssRef.world.unitCollAreaGrid.cities_aiUpdate,
                    faction, null);

                //TODO pick random city
                foreach (var city in DssRef.world.unitCollAreaGrid.cities_aiUpdate)
                {
                    if (army.strengthValue > city.strengthValue + city.ai_armyDefenceValue)
                    {
                        if (mayAttackFaction(city.faction))
                        {
                            army.Order_Attack(city);
                            return city;
                        }
                    }
                }
            }
            return null;
        }

        bool mayAttackFaction(Faction otherFaction)
        {
            if (otherFaction.player.protectedPlayer)
            {
                if (faction.Size() >= FactionSize.Big && Ref.rnd.Chance(0.25))
                { 
                    return true;
                }
                return false;
            }

            RelationType rel = DssRef.diplomacy.GetRelationType(faction, otherFaction);

            if (rel <= RelationType.RelationType0_Neutral)
            {
                if (rel == RelationType.RelationTypeN2_Truce)
                {
                    return false;
                }
                return true;
            }
            else if (rel == RelationType.RelationType1_Peace ||
                rel == RelationType.RelationType2_Good) 
            {
                return Ref.rnd.Chance(0.05);
            }
            return false;
        }

        
        public override void onNewRelation(Faction otherFaction, DiplomaticRelation rel, RelationType previousRelation)
        {
            base.onNewRelation(otherFaction, rel, previousRelation);
            if (rel.Relation == RelationType.RelationTypeN3_War)
            {
                if (otherFaction.factiontype == FactionType.Player &&
                    DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                {
                    protectedPlayer = true;
                }
                else if (otherFaction.factiontype == FactionType.DarkLord &&
                    faction.diplomaticSide == DiplomaticSide.None)
                {
                    faction.diplomaticSide = DiplomaticSide.Light;
                }
            }
        }

        public override bool IsLocal => true;
        public override bool IsAi()
        {
            return true;
        }
        public override bool IsPlayer()
        {
            return false;
        }
        public override AiPlayer GetAiPlayer()
        {
            return this;
        }
        public override string Name
        {
            get {
               return name;
            }
        }
    }

   
}
