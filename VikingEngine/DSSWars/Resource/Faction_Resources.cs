using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    //RESOURCES
    partial class Faction
    {
        public Money money = new Money(4000);
        Money storeMoney = Money.Zero;
        Money previuosMoney = Money.Zero;
        public int totalWorkForce, /*armyFoodUpkeep, */armyFoodImportCost, armyFoodBlackMarketCost;
        public SoldierUpkeep totalArmiesUpkeep = new SoldierUpkeep();
        public int embassyCount = 0;

        public TradeTemplate tradeTemplate = new TradeTemplate();
        public WorkTemplate workTemplate = new WorkTemplate();
        public CityEconomyData citiesEconomy;
        public int CityTradeExport = 0;
        public int CityTradeImport = 0;

        public int CityTradeExportCounting = 0;
        public int CityTradeImportCounting = 0;

        public int CityFoodProduction = 0;
        public int CityFoodSpending = 0;
        public int CitySoldResources = 0;

        public int resourceComponentStartIndex;
        //SoldierUpkeep totalArmiesUpkeep = new SoldierUpkeep();
        public int WorkForceInCityCount()
        { 
            return totalWorkForce / DssConst.HeadCityStartMaxWorkForce;
        }
        public bool craftOnFullStockProperty(object tag, bool set, bool value)
        {
            WorkPriorityType work = (WorkPriorityType)tag;

            ref var prio = ref workTemplate.GetRefWorkPriority(work);

            if (set)
            {
                prio.waitForStockpile = value;
            }
            return prio.waitForStockpile;
        }

        /// <summary>
        /// To measure the strength a faction could muster
        /// </summary>
        /// <returns></returns>
        public float PotensialMilitaryStrength()
        {
            return militaryStrength + totalWorkForce / DssConst.SoldierGroup_DefaultCount;
        }

        public long GoldSecDiff()
        {
            return storeMoney.GetGold() - previuosMoney.GetGold();
        }

        public void resourceTab(LocalPlayer player, RichBoxContent content)
        {
            //if (player.resourcesSubTab > ResourcesSubTab.Overview_Armor)
            //{
            //    player.resourcesSubTab = 0;
            //}
            player.resourcesSubTab.managementType = ResourceManagementType.Overview;

            content.newLine();
            for (ResourceGroupType resourceGroup = 0; resourceGroup < ResourceGroupType.Mint; resourceGroup++)//ResourcesSubTab resourcesSubTab = 0; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
            {

                //var tabContent = new RichBoxContent();

                //switch (resourceGroup)
                //{
                //    case ResourceGroupType.Resources:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                //        break;

                //    case ResourceGroupType.Metals:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                //        break;
                //    case ResourceGroupType.Weapons:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                //        break;

                //    case ResourceGroupType.Projectile:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                //        break;

                //    case ResourceGroupType.Armor:
                //        tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
                //        break;

                //    //case ResourcesSubTab.Stockpile_Resources:
                //    //    tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Stockpile));
                //    //    tabContent.space();
                //    //    tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                //    //    break;
                //}
                IconName.Tab(resourceGroup, out SpriteName groupIcon, out string groupName);
                var subTab = new ArtButton(player.resourcesSubTab.resourceGroup == resourceGroup? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, 
                    new List<AbsRichBoxMember> { new RbImage(groupIcon) },
                    new RbAction1Arg<ResourceGroupType>((ResourceGroupType resourceGroup) =>
                    {
                        player.resourcesSubTab.resourceGroup = resourceGroup;
                    }, resourceGroup, RbSoundType.Tab));

                content.Add(subTab);
            }

            ItemResourceType[] items = ResourceLib.ResourceGroupList(player.resourcesSubTab.resourceGroup);

            //switch (player.resourcesSubTab)
            //{
            //    case ResourcesSubTab.Overview_Resources:
            //        items = City.MovableCityResource_Misc;
            //        break;

            //    case ResourcesSubTab.Overview_Metals:
            //        items = City.MovableCityResource_Metals;
            //        break;

            //    case ResourcesSubTab.Overview_Weapons:
            //        items = City.MovableCityResource_WeaponMelee;
            //        break;

            //    case ResourcesSubTab.Overview_Projectile:
            //        items = City.MovableCityResource_WeaponRanged;
            //        break;

            //    case ResourcesSubTab.Overview_Armor:
            //        items = City.MovableCityResource_Armor;
            //        break;

            //}

            
            CircleCounterUp lineCounter = new CircleCounterUp(1, 1);
            foreach (var item in items)
            {
                if (lineCounter.Next_IsReset())
                {
                    content.Add(new RbSeperationLine());
                }
                int itemIndex = ItemPropertyColl.CityIndex(item);
                var resource = DssRef.world.factionResourceOverviews[resourceComponentStartIndex + itemIndex];

                resource.toFactionOverViewMenu(content, item);
            }
        }

        public void stockPileTab(LocalPlayer player, RichBoxContent content)
        {
            player.resourcesSubTab.managementType = ResourceManagementType.Stockpile;

            //switch (player.resourcesSubTab)
            //{
                
            //    case ResourcesSubTab.Overview_Resources:
            //    case ResourcesSubTab.Work_Resources:
            //        player.resourcesSubTab = ResourcesSubTab.Stockpile_Resources;
            //        break;

            //    case ResourcesSubTab.Overview_Metals:
            //    case ResourcesSubTab.Work_Metals:
            //        player.resourcesSubTab = ResourcesSubTab.Stockpile_Metals;
            //        break;

            //    case ResourcesSubTab.Overview_Weapons:
            //    case ResourcesSubTab.Work_Weapons:
            //        player.resourcesSubTab = ResourcesSubTab.Stockpile_Weapons;
            //        break;

            //    case ResourcesSubTab.Overview_Projectile:
            //    case ResourcesSubTab.Work_Projectile:
            //        player.resourcesSubTab = ResourcesSubTab.Stockpile_Projectile;
            //        break;

            //    case ResourcesSubTab.Overview_Armor:
            //    case ResourcesSubTab.Work_Armor:
            //        player.resourcesSubTab = ResourcesSubTab.Stockpile_Armor;
            //        break;
            //}
            

            content.newLine();
            content.h2(DssRef.lang.Resource_Tab_Stockpile, HudLib.TitleColor_Head);
            content.newLine();

            for (ResourceGroupType resourceGroup = 0; resourceGroup < ResourceGroupType.NUM; resourceGroup++)//ResourcesSubTab resourcesSubTab = ResourcesSubTab.Stockpile_Resources; resourcesSubTab <= ResourcesSubTab.Stockpile_Armor; ++resourcesSubTab)
            {
                //var tabContent = new RichBoxContent();

                //switch (resourceGroup)
                //{
                //    case ResourceGroup.:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                //        break;

                //    case ResourcesSubTab.Stockpile_Metals:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                //        break;
                //    case ResourcesSubTab.Stockpile_Weapons:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                //        break;

                //    case ResourcesSubTab.Stockpile_Projectile:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                //        break;

                //    case ResourcesSubTab.Stockpile_Armor:
                //        tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
                //        break;

                //}
                IconName.Tab(resourceGroup, out var icon, out var name);


                var subTab = new ArtButton(player.resourcesSubTab.resourceGroup == resourceGroup ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, 
                    new List<AbsRichBoxMember> { new RbImage(icon) },
                    new RbAction1Arg<ResourceGroupType>((ResourceGroupType resourceGroup) =>
                    {
                        player.resourcesSubTab.resourceGroup = resourceGroup;
                    }, resourceGroup, RbSoundType.Tab));

                content.Add(subTab);
            }

            new StockPileMenu(content, null, this).toHud(player, player.resourcesSubTab.resourceGroup);
            //ItemResourceType[] items = null;

            //switch (player.resourcesSubTab)
            //{
            //    case ResourcesSubTab.Stockpile_Resources:
            //        items = City.MovableCityResource_Misc;
            //        break;

            //    case ResourcesSubTab.Stockpile_Metals:
            //        items = City.MovableCityResource_Metals;
            //        break;

            //    case ResourcesSubTab.Stockpile_Weapons:
            //        items = City.MovableCityResource_WeaponMelee;
            //        break;

            //    case ResourcesSubTab.Stockpile_Projectile:
            //        items = City.MovableCityResource_WeaponRanged;
            //        break;

            //    case ResourcesSubTab.Stockpile_Armor:
            //        items = City.MovableCityResource_Armor;
            //        break;

            //}


            ////CircleCounterUp lineCounter = new CircleCounterUp(1, 1);
            //foreach (var item in items)
            //{
            //    //if (lineCounter.Next_IsReset())
            //    //{
            //    //    content.Add(new RbSeperationLine());
            //    //}
            //    int itemIndex = ItemPropertyColl.CityIndex(item);
            //    ResourceOverview resource = DssRef.world.factionResourceOverviews[resourceComponentStartIndex + itemIndex];

            //    resource.toMenu(content, item);
            //}
        }

        public void workTab(RichBoxContent content)
        {
            var p = player.GetLocalPlayer();

            //if (p.resourcesSubTab < ResourcesSubTab.Work_Resources || p.resourcesSubTab > ResourcesSubTab.Work_Armor)
            //{
            //    p.resourcesSubTab = ResourcesSubTab.Work_Resources;
            //}
            p.resourcesSubTab.managementType = ResourceManagementType.Work;

            content.h2(DssRef.lang.Work_OrderPrioTitle, HudLib.TitleColor_Head);
            content.newLine();
            //for (ResourcesSubTab resourcesSubTab = ResourcesSubTab.Work_Resources; resourcesSubTab <= ResourcesSubTab.Work_Armor; ++resourcesSubTab)
            //{
            for (ResourceGroupType resourceGroup = 0; resourceGroup < ResourceGroupType.NUM; resourceGroup++)//ResourcesSubTab resourcesSubTab = ResourcesSubTab.Stockpile_Resources; resourcesSubTab <= ResourcesSubTab.Stockpile_Armor; ++resourcesSubTab)
            {
                //var tabContent = new RichBoxContent();
                ////string text = null;
                //switch (resourcesSubTab)
                //{
                //    case ResourcesSubTab.Work_Resources:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                //        break;

                //    case ResourcesSubTab.Work_Metals:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                //        break;
                //    case ResourcesSubTab.Work_Weapons:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                //        break;
                //    case ResourcesSubTab.Work_Projectile:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                //        break;
                //    case ResourcesSubTab.Work_Armor:
                //        tabContent.Add(new RbImage(SpriteName.WarsResource_IronArmor));
                //        break;
                //}

                IconName.Tab(resourceGroup, out var icon, out var name);

                var subTab = new ArtButton(p.resourcesSubTab.resourceGroup == resourceGroup ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                    new List<AbsRichBoxMember> { new RbImage(icon) }, 
                    new RbAction1Arg<ResourceGroupType>((ResourceGroupType resourceGroup) =>
                    {
                        p.resourcesSubTab.resourceGroup = resourceGroup;
                    }, resourceGroup, RbSoundType.Tab));

                content.Add(subTab);
            }
            
            content.Add(new RbSeperationLine());
           
            workTemplate.toHud(p, content, p.resourcesSubTab.resourceGroup, this, null);
        }

        //public void tradeTab(RichBoxContent content)
        //{
        //    tradeTemplate.toHud(player.GetLocalPlayer(), content, this, null);
        //}

        //public void changeResourcePrice(float change, ItemResourceType resourceType, City city)
        //{
        //    if (city != null)
        //    {
        //        city.tradeTemplate.changeResourcePrice(change, resourceType);
        //    }
        //    else
        //    { 
        //        tradeTemplate.changeResourcePrice(change, resourceType);
        //        //var cityCounter = cities.counter();
        //        //while (cityCounter.Next())
        //        //{
        //        SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
        //        while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
        //        {
        //            citySel.tradeTemplate.onFactionValueChange(tradeTemplate);
        //        }
        //    }
        //}
        public void setWorkPrio(int set, WorkPriorityType priorityType, City city)
        {
            if (city != null)
            {
                city.workTemplate.setWorkPrio(priorityType, (byte)set);
            }
            else
            {
                //if (priorityType == WorkPriorityType.buildOrders)
                //{
                //    player.GetLocalPlayer().gameControls.build.buildPriority.value = set;
                //}
                //else
                //{
                    workTemplate.setWorkPrio(priorityType, (byte)set);
                    refreshCityWork();
                //}
            }
        }

        public void refreshCityWork()
        {
            //var cityCounter = cities.counter();
            //while (cityCounter.Next())
            //{
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                citySel.workTemplate.onFactionChange(citySel, workTemplate);
            }
        }

        //public void tradeFollowFactionClick(ItemResourceType resourceType, City city)
        //{
        //    city.tradeTemplate.followFactionClick(resourceType, tradeTemplate);
        //}

        public void workFollowFactionClick(WorkPriorityType prioType, City city)
        {
            city.workTemplate.followFactionClick(city, prioType, workTemplate);
        }

        //public bool calcCost(int cost, ref int totalCost, City city) {
        //    totalCost += cost;

        //    if (DssRef.storage.centralGold)
        //    {
        //        return gold >= totalCost;
        //    }
        //    else
        //    {
        //        return city.gold >= totalCost;
        //    }
        //}

        public long GetGold(City city)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                return money.GetGold();
            }
            else
            {
                return city.money.GetGold();
            }
        }

        public bool hasGold(int cost, AbsMapObject mapObj)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                return money.GetGold() >= cost;
            }
            else
            {
                return mapObj.money.GetGold() >= cost;
            }
        }

        public bool hasMoney(Money cost, AbsMapObject mapObj)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                return money >= cost;
            }
            else
            {
                return mapObj.money >= cost;
            }
        }

        public ref Money GetRefMoney(AbsMapObject mapObj)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                return ref money;
            }
            else
            {
                return ref mapObj.money;
            }

        }

        public bool payGold(int cost, bool allowDept, City city)
        {
#if DEBUG
            if (player.IsLocalPlayer() && StartupSettings.EndlessResources)
            {
                return true;
            }
#endif

            if (DssRef.storage.gameRuleset.centralGold)
            { 
                return money.PayGold(cost, allowDept);                
            }
            else
            {
                return city.money.PayGold(cost, allowDept);                
            }
        }
        public int payGold_MuchAsPossible(int cost, City city)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                return (int)money.payGold_MuchAsPossible(cost);//pay(ref gold);
            }
            else
            {
                return (int)city.money.payGold_MuchAsPossible(cost);
            }

            //int pay(ref int gold)
            //{
            //    if (gold > 0)
            //    {
            //        int canPay = lib.SmallestValue(gold, cost);
            //        gold-= canPay;
            //        return canPay;
            //    }
            //    return 0;
            //}
        }


        public void addGold(long value, City city)
        {
            if (DssRef.storage.gameRuleset.centralGold)
            {
                money.AddGold(value);
            }
            else
            { 
                city.money.AddGold(value);        
            }
        }

        public void addGold_factionWide(int value)
        {   
            money.AddGold(value);
            int cityCount = cities.Count;
            if (cityCount > 0)
            {
                int perCity = value / cityCount;

                //var citiesC = cities.counter();
                //while (citiesC.Next())
                //{
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
                {
                    citySel.money.AddGold(perCity);
                }
            }           
        }

        

        public void resources_updateAsynch(float oneSecondUpdate, out float citiesMilitaryStrenght)
        {
            //int cityIncomeCount = 0;
            int workForceCount = 0;
            //int nobel = 0;
            
            CityEconomyData newCitiesEconomy = new CityEconomyData();
            float citiesFoodProduce = 0;
            float citiesFoodSpend = 0;
            float soldResources = 0;
            citiesMilitaryStrenght = 0;

            //var citiesC = cities.counter();
            //            while (citiesC.Next())
            //            {
            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                //citiesC.sel.updateIncome_asynch();
                //CityEconomyData data = citiesC.sel.calcIncome_async();
                CityEconomyData data = new CityEconomyData(citySel);
                newCitiesEconomy.Add(data);
                //cityIncomeCount += data.total();
                workForceCount += citySel.workForce.amount;
                citiesFoodProduce += citySel.foodProduction.displayValue_gold_sec;
                citiesFoodSpend += citySel.foodSpending.displayValue_gold_sec;
                soldResources += citySel.soldResources.displayValue_gold_sec;
                citiesMilitaryStrenght += citySel.strengthValue;
                //if (citiesC.sel.nobelHouse)
                //{
                //    ++nobel;
                //}
            }

            totalWorkForce = workForceCount;
            citiesEconomy = newCitiesEconomy;
            //cityIncome = newCitiesEconomy.total();
            //nobelHouseCount = nobel;

#if DEBUG
            if (Debug.CorruptValue(citiesFoodSpend))
            {
                lib.DoNothing();
            }
#endif

            CityFoodProduction = Convert.ToInt32(citiesFoodProduce);
            CityFoodSpending = Convert.ToInt32(citiesFoodSpend);
            CitySoldResources = Convert.ToInt32(soldResources);

            //float totalArmiesUpkeep = 0;
            float foodImport = 0;
            float foodBlackMarket = 0;

            bool casual = player.profile.casualControls;
            SoldierUpkeep _totalArmiesUpkeep = new SoldierUpkeep();
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                if (armiesC.sel.debugTagged)
                {
                    lib.DoNothing();
                }

                //SoldierUpkeep upkeep = new SoldierUpkeep();
                SoldierUpkeep armyUpkeep = new SoldierUpkeep();
                float moneyCarry = 0;
                //float armyUpkeep = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    groups.sel.Upkeep(casual, ref armyUpkeep, ref moneyCarry);
                }
                //armyUpkeep += upkeep;
                //_totalArmiesUpkeep += upkeep;

                //float goldUpkeep = manUpkeepCount;

                //totalArmiesUpkeep += armyUpkeep;
                foodImport += armiesC.sel.foodCosts_import.displayValue_gold_sec;
                foodBlackMarket += armiesC.sel.foodCosts_blackmarket.displayValue_gold_sec;

                armiesC.sel.goldCarryCapacity = Convert.ToInt32(moneyCarry);
                armiesC.sel.totalUpkeep = armyUpkeep;
                _totalArmiesUpkeep += armyUpkeep;


                float copperUpkeep = armyUpkeep.copper * oneSecondUpdate; // DssRef.difficulty.setting_foodMulti;
                if (!money.PayUpkeep(copperUpkeep))
                {
                    Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                }

              
                if (!casual)
                {
                    
                    armiesC.sel.food -= armyUpkeep.food * oneSecondUpdate;
                    if (armiesC.sel.food < 0)
                    {
                        float rest = armiesC.sel.food;
                        armiesC.sel.food = 0;
                        armiesC.sel.conservedFood += rest;

                        if (armiesC.sel.conservedFood < -armyUpkeep.food * 60)
                        {
                            if (hasDeserters)
                            {
                                Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                            }
                            else
                            {
                                armiesC.sel.setMaxFood();
                            }
                        }
                    }
                }
            }
            
            armyFoodImportCost = Convert.ToInt32(foodImport);
            armyFoodBlackMarketCost = Convert.ToInt32(foodBlackMarket);

            totalArmiesUpkeep = _totalArmiesUpkeep;
        }

        public ref GroupedResource GetRefResourceOverview(ItemResourceType item)
        {
            int itemIndex = ItemPropertyColl.CityIndex(item);
            return ref DssRef.world.factionResourceOverviews[resourceComponentStartIndex + itemIndex];
        }

        public GroupedResource GetResourceOverview(ItemResourceType item)
        {
            int itemIndex = ItemPropertyColl.CityIndex(item);
            return DssRef.world.factionResourceOverviews[resourceComponentStartIndex + itemIndex];
        }

        int resourceSecondUpdates = 0;

        public void resourceOverviewOneSecondUpdate()
        {
            resourceSecondUpdates++;
            

            //int end = resourceComponentStartIndex + CityResoureIndex.COUNT;
            //for (int itemIx = resourceComponentStartIndex; itemIx < end; itemIx++)
            //{
            //    //ref ResourceOverview overview = ref DssRef.world.factionResourceOverviews[itemIx];
            //    //overview.oneSecondUpdate();
            //    DssRef.world.factionResourceOverviews[itemIx].changeRate.oneSecondUpdate();
            //}
            //res_wood.oneSecondUpdate();
            //res_fuel.oneSecondUpdate();
            //res_stone.oneSecondUpdate();
            //res_rawFood.oneSecondUpdate();
            //res_food.oneSecondUpdate();
            //res_beer.oneSecondUpdate();
            //res_coolingfluid.oneSecondUpdate();
            //res_skinLinnen.oneSecondUpdate();

            //res_ironore.oneSecondUpdate();
            //res_TinOre.oneSecondUpdate();
            //res_CupperOre.oneSecondUpdate();
            //res_LeadOre.oneSecondUpdate();
            //res_SilverOre.oneSecondUpdate();
            //res_GoldOre.oneSecondUpdate();

            //res_iron.oneSecondUpdate();
            //res_Tin.oneSecondUpdate();
            //res_Cupper.oneSecondUpdate();
            //res_Lead.oneSecondUpdate();
            //res_Silver.oneSecondUpdate();
            //res_RawMithril.oneSecondUpdate();
            //res_Sulfur.oneSecondUpdate();

            //res_Bronze.oneSecondUpdate();
            //res_Steel.oneSecondUpdate();
            //res_CastIron.oneSecondUpdate();
            //res_BloomeryIron.oneSecondUpdate();
            //res_Mithril.oneSecondUpdate();

            //res_Palisade.oneSecondUpdate();
            //res_Toolkit.oneSecondUpdate();
            //res_Wagon2Wheel.oneSecondUpdate();
            //res_Wagon4Wheel.oneSecondUpdate();
            //res_BlackPowder.oneSecondUpdate();
            //res_GunPowder.oneSecondUpdate();
            //res_LedBullet.oneSecondUpdate();

            //res_sharpstick.oneSecondUpdate();
            //res_BronzeSword.oneSecondUpdate();
            //res_shortsword.oneSecondUpdate();
            //res_Sword.oneSecondUpdate();
            //res_LongSword.oneSecondUpdate();
            //res_HandSpear.oneSecondUpdate();
            //res_MithrilSword.oneSecondUpdate();

            //res_Warhammer.oneSecondUpdate();
            //res_twohandsword.oneSecondUpdate();
            //res_knightslance.oneSecondUpdate();
            //res_SlingShot.oneSecondUpdate();
            //res_ThrowingSpear.oneSecondUpdate();
            //res_bow.oneSecondUpdate();
            //res_longbow.oneSecondUpdate();
            //res_crossbow.oneSecondUpdate();
            //res_MithrilBow.oneSecondUpdate();

            //res_HandCannon.oneSecondUpdate();
            //res_HandCulvertin.oneSecondUpdate();
            //res_Rifle.oneSecondUpdate();
            //res_Blunderbuss.oneSecondUpdate();

            //res_BatteringRam.oneSecondUpdate();
            //res_ballista.oneSecondUpdate();
            //res_Manuballista.oneSecondUpdate();
            //res_Catapult.oneSecondUpdate();
            //res_SiegeCannonBronze.oneSecondUpdate();
            //res_ManCannonBronze.oneSecondUpdate();
            //res_SiegeCannonIron.oneSecondUpdate();
            //res_ManCannonIron.oneSecondUpdate();

            //res_paddedArmor.oneSecondUpdate();
            //res_HeavyPaddedArmor.oneSecondUpdate();
            //res_BronzeArmor.oneSecondUpdate();
            //res_mailArmor.oneSecondUpdate();
            //res_heavyMailArmor.oneSecondUpdate();
            //res_LightPlateArmor.oneSecondUpdate();
            //res_FullPlateArmor.oneSecondUpdate();
            //res_MithrilArmor.oneSecondUpdate();
        }

        public void updateResourceOverview_async()
        {
            if (resourceSecondUpdates > 0)
            {
                resourceSecondUpdates--;

                int end = resourceComponentStartIndex + CityResoureIndex.COUNT;
                for (int itemIx = resourceComponentStartIndex; itemIx < end; itemIx++)
                {
                    DssRef.world.factionResourceOverviews[itemIx].clearFactionOverView();
                }

                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities))
                {
                    int start = citiesC.sel * CityResoureIndex.COUNT;

                    for (int index = 0; index < CityResoureIndex.COUNT; index++)
                    {
                        ref var cityResource = ref DssRef.world.cityResouces[start + index];
                        cityResource.changeRate.oneSecondUpdate();

                        ref var factionOverview = ref DssRef.world.factionResourceOverviews[resourceComponentStartIndex + index];
                        factionOverview.amount += cityResource.amount;
                        factionOverview.changeRate.prevConsumed += cityResource.changeRate.prevConsumed;
                        factionOverview.changeRate.prevProduced += cityResource.changeRate.prevProduced;
                    }
                }
            }

            //var citiesC = cities.counter();

            //for (int itemIx = 0; itemIx < CityResoureIndex.COUNT; itemIx++)
            //{
            //    ref ResourceFactionOverview overview = ref DssRef.world.factionResourceOverviews[resourceComponentStartIndex + itemIx];
            //    overview.clearCurrent();

            //    //citiesC.Reset();
            //    //while (citiesC.Next())
            //    //{
            //    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            //    while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            //    {
            //        overview.current += DssRef.world.cityResouces[citySel.resourceComponentStartIndex + itemIx].amount;
            //    }
            //}


            //res_wood.clearCurrent();
            //res_fuel.clearCurrent();
            //res_stone.clearCurrent();
            //res_rawFood.clearCurrent();
            //res_food.clearCurrent();
            //res_beer.clearCurrent();
            //res_coolingfluid.clearCurrent();
            //res_skinLinnen.clearCurrent();

            //res_ironore.clearCurrent();
            //res_TinOre.clearCurrent();
            //res_CupperOre.clearCurrent();
            //res_LeadOre.clearCurrent();
            //res_SilverOre.clearCurrent();
            //res_GoldOre.clearCurrent();

            //res_iron.clearCurrent();
            //res_Tin.clearCurrent();
            //res_Cupper.clearCurrent();
            //res_Lead.clearCurrent();
            //res_Silver.clearCurrent();
            //res_RawMithril.clearCurrent();
            //res_Sulfur.clearCurrent();

            //res_Bronze.clearCurrent();
            //res_Steel.clearCurrent();
            //res_CastIron.clearCurrent();
            //res_BloomeryIron.clearCurrent();
            //res_Mithril.clearCurrent();

            //res_Palisade.clearCurrent();
            //res_Toolkit.clearCurrent();
            //res_Wagon2Wheel.clearCurrent();
            //res_Wagon4Wheel.clearCurrent();
            //res_BlackPowder.clearCurrent();
            //res_GunPowder.clearCurrent();
            //res_LedBullet.clearCurrent();

            //res_sharpstick.clearCurrent();
            //res_BronzeSword.clearCurrent();
            //res_shortsword.clearCurrent();
            //res_Sword.clearCurrent();
            //res_LongSword.clearCurrent();
            //res_HandSpear.clearCurrent();
            //res_MithrilSword.clearCurrent();

            //res_Warhammer.clearCurrent();
            //res_twohandsword.clearCurrent();
            //res_knightslance.clearCurrent();
            //res_SlingShot.clearCurrent();
            //res_ThrowingSpear.clearCurrent();
            //res_bow.clearCurrent();
            //res_longbow.clearCurrent();
            //res_crossbow.clearCurrent();
            //res_MithrilBow.clearCurrent();

            //res_HandCannon.clearCurrent();
            //res_HandCulvertin.clearCurrent();
            //res_Rifle.clearCurrent();
            //res_Blunderbuss.clearCurrent();

            //res_BatteringRam.clearCurrent();
            //res_ballista.clearCurrent();
            //res_Manuballista.clearCurrent();
            //res_Catapult.clearCurrent();
            //res_SiegeCannonBronze.clearCurrent();
            //res_ManCannonBronze.clearCurrent();
            //res_SiegeCannonIron.clearCurrent();
            //res_ManCannonIron.clearCurrent();

            //res_paddedArmor.clearCurrent();
            //res_HeavyPaddedArmor.clearCurrent();
            //res_BronzeArmor.clearCurrent();
            //res_mailArmor.clearCurrent();
            //res_heavyMailArmor.clearCurrent();
            //res_LightPlateArmor.clearCurrent();
            //res_FullPlateArmor.clearCurrent();
            //res_MithrilArmor.clearCurrent();

            //var citiesC = cities.counter();
            //while (citiesC.Next())
            //{
            //    res_wood.current += citiesC.sel.res_wood.amount;
            //    res_fuel.current += citiesC.sel.res_fuel.amount;
            //    res_stone.current += citiesC.sel.res_stone.amount;
            //    res_rawFood.current += citiesC.sel.res_rawFood.amount;
            //    res_food.current += citiesC.sel.res_food.amount;
            //    res_beer.current += citiesC.sel.res_beer.amount;
            //    res_coolingfluid.current += citiesC.sel.res_coolingfluid.amount;
            //    res_skinLinnen.current += citiesC.sel.res_skinLinnen.amount;

            //    res_ironore.current += citiesC.sel.res_ironore.amount;
            //    res_TinOre.current += citiesC.sel.res_TinOre.amount;
            //    res_CupperOre.current += citiesC.sel.res_CupperOre.amount;
            //    res_LeadOre.current += citiesC.sel.res_LeadOre.amount;
            //    res_SilverOre.current += citiesC.sel.res_SilverOre.amount;
            //    res_GoldOre.current += citiesC.sel.res_GoldOre.amount;

            //    res_iron.current += citiesC.sel.res_iron.amount;
            //    res_Tin.current += citiesC.sel.res_Tin.amount;
            //    res_Cupper.current += citiesC.sel.res_Cupper.amount;
            //    res_Lead.current += citiesC.sel.res_Lead.amount;
            //    res_Silver.current += citiesC.sel.res_Silver.amount;
            //    res_RawMithril.current += citiesC.sel.res_RawMithril.amount;
            //    res_Sulfur.current += citiesC.sel.res_Sulfur.amount;

            //    res_Bronze.current += citiesC.sel.res_Bronze.amount;
            //    res_Steel.current += citiesC.sel.res_Steel.amount;
            //    res_CastIron.current += citiesC.sel.res_CastIron.amount;
            //    res_BloomeryIron.current += citiesC.sel.res_BloomeryIron.amount;
            //    res_Mithril.current += citiesC.sel.res_Mithril.amount;

            //    res_Palisade.current += citiesC.sel.res_Palisade.amount;
            //    res_Toolkit.current += citiesC.sel.res_Toolkit.amount;
            //    res_Wagon2Wheel.current += citiesC.sel.res_Wagon2Wheel.amount;
            //    res_Wagon4Wheel.current += citiesC.sel.res_Wagon4Wheel.amount;
            //    res_BlackPowder.current += citiesC.sel.res_BlackPowder.amount;
            //    res_GunPowder.current += citiesC.sel.res_GunPowder.amount;
            //    res_LedBullet.current += citiesC.sel.res_LedBullet.amount;

            //    res_sharpstick.current += citiesC.sel.res_sharpstick.amount;
            //    res_BronzeSword.current += citiesC.sel.res_BronzeSword.amount;
            //    res_shortsword.current += citiesC.sel.res_shortsword.amount;
            //    res_Sword.current += citiesC.sel.res_Sword.amount;
            //    res_LongSword.current += citiesC.sel.res_LongSword.amount;
            //    res_HandSpear.current += citiesC.sel.res_HandSpear.amount;
            //    res_MithrilSword.current += citiesC.sel.res_MithrilSword.amount;

            //    res_Warhammer.current += citiesC.sel.res_Warhammer.amount;
            //    res_twohandsword.current += citiesC.sel.res_twohandsword.amount;
            //    res_knightslance.current += citiesC.sel.res_knightslance.amount;
            //    res_SlingShot.current += citiesC.sel.res_SlingShot.amount;
            //    res_ThrowingSpear.current += citiesC.sel.res_ThrowingSpear.amount;
            //    res_bow.current += citiesC.sel.res_bow.amount;
            //    res_longbow.current += citiesC.sel.res_longbow.amount;
            //    res_crossbow.current += citiesC.sel.res_crossbow.amount;
            //    res_MithrilBow.current += citiesC.sel.res_MithrilBow.amount;

            //    res_HandCannon.current += citiesC.sel.res_HandCannon.amount;
            //    res_HandCulvertin.current += citiesC.sel.res_HandCulvertin.amount;
            //    res_Rifle.current += citiesC.sel.res_Rifle.amount;
            //    res_Blunderbuss.current += citiesC.sel.res_Blunderbuss.amount;

            //    res_BatteringRam.current += citiesC.sel.res_BatteringRam.amount;
            //    res_ballista.current += citiesC.sel.res_ballista.amount;
            //    res_Manuballista.current += citiesC.sel.res_Manuballista.amount;
            //    res_Catapult.current += citiesC.sel.res_Catapult.amount;
            //    res_SiegeCannonBronze.current += citiesC.sel.res_SiegeCannonBronze.amount;
            //    res_ManCannonBronze.current += citiesC.sel.res_ManCannonBronze.amount;
            //    res_SiegeCannonIron.current += citiesC.sel.res_SiegeCannonIron.amount;
            //    res_ManCannonIron.current += citiesC.sel.res_ManCannonIron.amount;

            //    res_paddedArmor.current += citiesC.sel.res_paddedArmor.amount;
            //    res_HeavyPaddedArmor.current += citiesC.sel.res_HeavyPaddedArmor.amount;
            //    res_BronzeArmor.current += citiesC.sel.res_BronzeArmor.amount;
            //    res_mailArmor.current += citiesC.sel.res_mailArmor.amount;
            //    res_heavyMailArmor.current += citiesC.sel.res_heavyMailArmor.amount;
            //    res_LightPlateArmor.current += citiesC.sel.res_LightPlateArmor.amount;
            //    res_FullPlateArmor.current += citiesC.sel.res_FullPlateArmor.amount;
            //    res_MithrilArmor.current += citiesC.sel.res_MithrilArmor.amount;

            //}
        }
    }
}
