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
        public MinuteStats foodProduction = new MinuteStats();
        public MinuteStats foodSpending = new MinuteStats();
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

        //public int CityFoodProduction = 0;
        //public int CityFoodSpending = 0;
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
                citySel.workTemplate.onFactionChange(citySel, workTemplate, false);
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
            if (DssRef.storage.ruleset_instance.centralGold)
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
            if (cost <= 0)
            {
                return true;
            }

            if (DssRef.storage.ruleset_instance.centralGold)
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
            if (DssRef.storage.ruleset_instance.centralGold)
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
            if (DssRef.storage.ruleset_instance.centralGold)
            {
                return ref money;
            }
            else
            {
                return ref mapObj.money;
            }

        }

        public bool payGold(int cost, bool allowDept, AbsArmy city)
        {
#if DEBUG
            if (player != null && player.IsLocalPlayer() && StartupSettings.EndlessResources)
            {
                return true;
            }
#endif

            if (DssRef.storage.ruleset_instance.centralGold)
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
            if (DssRef.storage.ruleset_instance.centralGold)
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
            if (DssRef.storage.ruleset_instance.centralGold)
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

                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
                {
                    citySel.money.AddGold(perCity);
                }
            }           
        }

        

        public void resources_updateAsynch(float oneSecondUpdate, out float citiesMilitaryStrenght)
        {
            int workForceCount = 0;
            
            CityEconomyData newCitiesEconomy = new CityEconomyData();
            //float citiesFoodProduce = 0;
            //float citiesFoodSpend = 0;
            float soldResources = 0;
            citiesMilitaryStrenght = 0;

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref cities, DssRef.world.cities, out City citySel))
            {
                CityEconomyData data = new CityEconomyData(citySel);
                newCitiesEconomy.Add(data);
                workForceCount += citySel.workForce.amount;
                //citiesFoodProduce += citySel.foodProduction.displayValue_gold_sec;
                //citiesFoodSpend += citySel.foodSpending.displayValue_gold_sec;
                soldResources += citySel.soldResources.displayValue_gold_sec;
                citiesMilitaryStrenght += citySel.strengthValue;
              
            }

            totalWorkForce = workForceCount;
            citiesEconomy = newCitiesEconomy;

//#if DEBUG
//            if (Debug.CorruptValue(citiesFoodSpend))
//            {
//                lib.DoNothing();
//            }
//#endif

//            CityFoodProduction = Convert.ToInt32(citiesFoodProduce);
//            CityFoodSpending = Convert.ToInt32(citiesFoodSpend);
            CitySoldResources = Convert.ToInt32(soldResources);

            float foodImport = 0;
            float foodBlackMarket = 0;

            bool casual = player.profile.casualControls;
            SoldierUpkeep _totalArmiesUpkeep = new SoldierUpkeep();
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                

                bool missingUpkeep = false;
                SoldierUpkeep armyUpkeep = new SoldierUpkeep();
                float moneyCarry = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    groups.sel.Upkeep(casual, ref armyUpkeep, ref moneyCarry);
                }
                
                foodImport += armiesC.sel.foodCosts_import.displayValue_gold_sec;
                foodBlackMarket += armiesC.sel.foodCosts_blackmarket.displayValue_gold_sec;

                armiesC.sel.goldCarryCapacity = Convert.ToInt32(moneyCarry);
                armiesC.sel.totalUpkeep = armyUpkeep;
                _totalArmiesUpkeep += armyUpkeep;

                if (oneSecondUpdate > 0)
                {
                    //if (armiesC.sel.debugTagged)
                    //{
                    //    lib.DoNothing();
                    //}
                    if (this.myIndex == 72)
                    {
                        lib.DoNothing();
                    }
                    float copperUpkeep = armyUpkeep.copper * oneSecondUpdate; // DssRef.difficulty.setting_foodMulti;
                    money.PayUpkeep(copperUpkeep, true);
                    //if (! && hasDeserters)
                    //{
                    //    missingUpkeep = true;
                    //    //Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                    //}


                    if (!casual)
                    {
                        armiesC.sel.food -= armyUpkeep.food * oneSecondUpdate;
                        if (armiesC.sel.food < 0 && armiesC.sel.conservedFood > 0)
                        {
                            float rest = lib.SmallestValue(armiesC.sel.conservedFood, - armiesC.sel.food);
                            armiesC.sel.conservedFood -= rest;
                            armiesC.sel.food += rest;
                        }

                        if (armiesC.sel.food < -armyUpkeep.food * 60)
                        {
                            armiesC.sel.foodMarketCheck();
                            if (hasDeserters)
                            {
                                missingUpkeep = true;
                                //Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                            }
                            else
                            {
                                armiesC.sel.setMaxFood();
                            }
                        }
                    }

                    if (missingUpkeep)
                    {
                        armiesC.sel.missingUpkeepSeconds++;
                        if (armiesC.sel.missingUpkeepSeconds > 20)
                        {
                            Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                        }
                    }
                    else
                    {
                        armiesC.sel.missingUpkeepSeconds = 0;
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

        int resourceMinuteUpdates = 0;

        public void resourceOverviewOneMinuteUpdate()
        {
            resourceMinuteUpdates++;            
        }
        
        public void updateResourceOverview_async()
        {
            if (resourceMinuteUpdates > 0)
            {
                resourceMinuteUpdates--;
                
                int end = resourceComponentStartIndex + CityResourceIndex.COUNT;
                for (int itemIx = resourceComponentStartIndex; itemIx < end; itemIx++)
                {
                    DssRef.world.factionResourceOverviews[itemIx].clearFactionOverView();
                }

                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref cities))
                {
                    int start = citiesC.sel * CityResourceIndex.COUNT;

                    for (int index = 0; index < CityResourceIndex.COUNT; index++)
                    {
                        ref var cityResource = ref DssRef.world.cityResouces[start + index];
                        cityResource.changeRate.oneMinuteUpdate();

                        ref var factionOverview = ref DssRef.world.factionResourceOverviews[resourceComponentStartIndex + index];
                        factionOverview.amount += cityResource.amount;
                        factionOverview.changeRate.prevConsumed += cityResource.changeRate.prevConsumed;
                        factionOverview.changeRate.prevProduced += cityResource.changeRate.prevProduced;
                    }
                }

                ResourceChangeRate foodChange = DssRef.world.factionResourceOverviews[resourceComponentStartIndex + CityResourceIndex.food].changeRate;
                foodProduction.add(foodChange.prevProduced);
                foodSpending.add(foodChange.prevConsumed);
            }

            
        }
    }
}
