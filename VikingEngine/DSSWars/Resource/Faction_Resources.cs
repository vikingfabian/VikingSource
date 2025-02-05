using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
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
        public int gold = 40;
        int storeGold = 0;
        int previuosGold = 0;
        public int totalWorkForce, armyFoodUpkeep, armyFoodImportCost, armyFoodBlackMarketCost;
        public int nobelHouseCount = 0;

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

        public ResourceOverview res_wood = new ResourceOverview();
        public ResourceOverview res_fuel = new ResourceOverview();
        public ResourceOverview res_stone = new ResourceOverview();
        public ResourceOverview res_rawFood = new ResourceOverview();
        public ResourceOverview res_food = new ResourceOverview();
        public ResourceOverview res_beer = new ResourceOverview();
        public ResourceOverview res_coolingfluid = new ResourceOverview();
        public ResourceOverview res_skinLinnen = new ResourceOverview();

        public ResourceOverview res_ironore = new ResourceOverview();
        public ResourceOverview res_TinOre = new ResourceOverview();
        public ResourceOverview res_CupperOre = new ResourceOverview();
        public ResourceOverview res_LeadOre = new ResourceOverview();
        public ResourceOverview res_SilverOre = new ResourceOverview();

        public ResourceOverview res_iron = new ResourceOverview();
        public ResourceOverview res_Tin = new ResourceOverview();
        public ResourceOverview res_Cupper = new ResourceOverview();
        public ResourceOverview res_Lead = new ResourceOverview();
        public ResourceOverview res_Silver = new ResourceOverview();
        public ResourceOverview res_RawMithril = new ResourceOverview();
        public ResourceOverview res_Sulfur = new ResourceOverview();

        public ResourceOverview res_Bronze = new ResourceOverview();
        public ResourceOverview res_Steel = new ResourceOverview();
        public ResourceOverview res_CastIron = new ResourceOverview();
        public ResourceOverview res_BloomeryIron = new ResourceOverview();
        public ResourceOverview res_Mithril = new ResourceOverview();

        public ResourceOverview res_Toolkit = new ResourceOverview();
        public ResourceOverview res_Wagon2Wheel = new ResourceOverview();
        public ResourceOverview res_Wagon4Wheel = new ResourceOverview();
        public ResourceOverview res_BlackPowder = new ResourceOverview();
        public ResourceOverview res_GunPowder = new ResourceOverview();
        public ResourceOverview res_LedBullet = new ResourceOverview();

        public ResourceOverview res_sharpstick = new ResourceOverview();
        public ResourceOverview res_BronzeSword = new ResourceOverview();
        public ResourceOverview res_shortsword = new ResourceOverview();
        public ResourceOverview res_Sword = new ResourceOverview();
        public ResourceOverview res_LongSword = new ResourceOverview();
        public ResourceOverview res_HandSpear = new ResourceOverview();
        public ResourceOverview res_MithrilSword = new ResourceOverview();

        public ResourceOverview res_Warhammer = new ResourceOverview();
        public ResourceOverview res_twohandsword = new ResourceOverview();
        public ResourceOverview res_knightslance = new ResourceOverview();
        public ResourceOverview res_SlingShot = new ResourceOverview();
        public ResourceOverview res_ThrowingSpear = new ResourceOverview();
        public ResourceOverview res_bow = new ResourceOverview();
        public ResourceOverview res_longbow = new ResourceOverview();
        public ResourceOverview res_crossbow = new ResourceOverview();
        public ResourceOverview res_MithrilBow = new ResourceOverview();

        public ResourceOverview res_HandCannon = new ResourceOverview();
        public ResourceOverview res_HandCulvertin = new ResourceOverview();
        public ResourceOverview res_Rifle = new ResourceOverview();
        public ResourceOverview res_Blunderbus = new ResourceOverview();

        public ResourceOverview res_BatteringRam = new ResourceOverview();
        public ResourceOverview res_ballista = new ResourceOverview();
        public ResourceOverview res_Manuballista = new ResourceOverview();
        public ResourceOverview res_Catapult = new ResourceOverview();
        public ResourceOverview res_SiegeCannonBronze = new ResourceOverview();
        public ResourceOverview res_ManCannonBronze = new ResourceOverview();
        public ResourceOverview res_SiegeCannonIron = new ResourceOverview();
        public ResourceOverview res_ManCannonIron = new ResourceOverview();

        public ResourceOverview res_paddedArmor = new ResourceOverview();
        public ResourceOverview res_HeavyPaddedArmor = new ResourceOverview();
        public ResourceOverview res_BronzeArmor = new ResourceOverview();
        public ResourceOverview res_mailArmor = new ResourceOverview();
        public ResourceOverview res_heavyMailArmor = new ResourceOverview();
        public ResourceOverview res_LightPlateArmor = new ResourceOverview();
        public ResourceOverview res_FullPlateArmor = new ResourceOverview();
        public ResourceOverview res_MithrilArmor = new ResourceOverview();

        public int MoneySecDiff()
        {
            return storeGold - previuosGold;
        }

        public void resourceTab(LocalPlayer player, RichBoxContent content)
        {
            if (player.resourcesSubTab > ResourcesSubTab.Overview_Armor)
            {
                player.resourcesSubTab = 0;
            }

            content.newLine();
            for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab <= ResourcesSubTab.Overview_Armor; ++resourcesSubTab)
            {
                var tabContent = new RichBoxContent();
                //string text = null;
                switch (resourcesSubTab)
                {
                    case ResourcesSubTab.Overview_Resources:
                        //tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Overview));
                        //tabContent.space();
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                        break;

                    case ResourcesSubTab.Overview_Metals:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                        break;
                    case ResourcesSubTab.Overview_Weapons:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                        break;

                    case ResourcesSubTab.Overview_Projectile:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                        break;

                    case ResourcesSubTab.Overview_Armor:
                        tabContent.Add(new RbImage(SpriteName.cmdMailArmor));
                        break;

                    case ResourcesSubTab.Stockpile_Resources:
                        tabContent.Add(new RbText(DssRef.lang.Resource_Tab_Stockpile));
                        tabContent.space();
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                        break;
                }
                var subTab = new ArtButton(player.resourcesSubTab == resourcesSubTab? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, tabContent,
                    new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                    {
                        player.resourcesSubTab = resourcesSubTab;
                    }, resourcesSubTab, SoundLib.menutab));
                //subTab.setGroupSelectionColor(HudLib.RbSettings, );
                content.Add(subTab);
                //content.space();
            }

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview_Resources:
                    content.Add(new RbSeperationLine());
                    res_wood.toMenu(content, ItemResourceType.Wood_Group);
                    res_fuel.toMenu(content, ItemResourceType.Fuel_G);
                    content.Add(new RbSeperationLine());
                    res_stone.toMenu(content, ItemResourceType.Stone_G);
                    res_rawFood.toMenu(content, ItemResourceType.RawFood_Group);
                    content.Add(new RbSeperationLine());
                    res_food.toMenu(content, ItemResourceType.Food_G);
                    res_beer.toMenu(content, ItemResourceType.Beer);
                    content.Add(new RbSeperationLine());
                    res_coolingfluid.toMenu(content, ItemResourceType.CoolingFluid);
                    res_skinLinnen.toMenu(content, ItemResourceType.SkinLinen_Group);
                    content.Add(new RbSeperationLine());

                    res_Toolkit.toMenu(content, ItemResourceType.Toolkit);
                    res_Wagon2Wheel.toMenu(content, ItemResourceType.Wagon2Wheel);
                    content.Add(new RbSeperationLine());
                    res_Wagon4Wheel.toMenu(content, ItemResourceType.Wagon4Wheel);
                    res_BlackPowder.toMenu(content, ItemResourceType.BlackPowder);
                    content.Add(new RbSeperationLine());
                    res_GunPowder.toMenu(content, ItemResourceType.GunPowder);
                    res_LedBullet.toMenu(content, ItemResourceType.LedBullet);
                    break;

                case ResourcesSubTab.Overview_Metals:
                    content.Add(new RbSeperationLine());
                    res_ironore.toMenu(content, ItemResourceType.IronOre_G);
                    res_TinOre.toMenu(content, ItemResourceType.TinOre);
                    content.Add(new RbSeperationLine());
                    res_CupperOre.toMenu(content, ItemResourceType.CopperOre);
                    res_LeadOre.toMenu(content, ItemResourceType.LeadOre);
                    content.Add(new RbSeperationLine());
                    res_SilverOre.toMenu(content, ItemResourceType.SilverOre);

                    res_iron.toMenu(content, ItemResourceType.Iron_G);
                    content.Add(new RbSeperationLine());
                    res_Tin.toMenu(content, ItemResourceType.Tin);
                    res_Cupper.toMenu(content, ItemResourceType.Copper);
                    content.Add(new RbSeperationLine());
                    res_Lead.toMenu(content, ItemResourceType.Lead);
                    res_Silver.toMenu(content, ItemResourceType.Silver);
                    content.Add(new RbSeperationLine());
                    res_RawMithril.toMenu(content, ItemResourceType.RawMithril);
                    res_Sulfur.toMenu(content, ItemResourceType.Sulfur);
                    content.Add(new RbSeperationLine());

                    res_Bronze.toMenu(content, ItemResourceType.Bronze);
                    res_Steel.toMenu(content, ItemResourceType.Steel);
                    content.Add(new RbSeperationLine());
                    res_CastIron.toMenu(content, ItemResourceType.CastIron);
                    res_BloomeryIron.toMenu(content, ItemResourceType.BloomeryIron);
                    content.Add(new RbSeperationLine());
                    res_Mithril.toMenu(content, ItemResourceType.Mithril);
                    break;

                case ResourcesSubTab.Overview_Weapons:
                    content.Add(new RbSeperationLine());
                    res_sharpstick.toMenu(content, ItemResourceType.SharpStick);
                    res_BronzeSword.toMenu(content, ItemResourceType.BronzeSword);
                    content.Add(new RbSeperationLine());
                    res_shortsword.toMenu(content, ItemResourceType.ShortSword);
                    res_Sword.toMenu(content, ItemResourceType.Sword);
                    content.Add(new RbSeperationLine());
                    res_LongSword.toMenu(content, ItemResourceType.LongSword);
                    res_HandSpear.toMenu(content, ItemResourceType.HandSpear);
                    content.Add(new RbSeperationLine());
                    res_MithrilSword.toMenu(content, ItemResourceType.MithrilSword);

                    res_Warhammer.toMenu(content, ItemResourceType.Warhammer);
                    content.Add(new RbSeperationLine());
                    res_twohandsword.toMenu(content, ItemResourceType.TwoHandSword);
                    res_knightslance.toMenu(content, ItemResourceType.KnightsLance);
                    break;

                case ResourcesSubTab.Overview_Projectile:
                    content.Add(new RbSeperationLine());
                    res_SlingShot.toMenu(content, ItemResourceType.SlingShot);
                    res_ThrowingSpear.toMenu(content, ItemResourceType.ThrowingSpear);
                    content.Add(new RbSeperationLine());
                    res_bow.toMenu(content, ItemResourceType.Bow);
                    res_longbow.toMenu(content, ItemResourceType.LongBow);
                    content.Add(new RbSeperationLine());
                    res_crossbow.toMenu(content, ItemResourceType.Crossbow);
                    res_MithrilBow.toMenu(content, ItemResourceType.MithrilBow);
                    content.Add(new RbSeperationLine());

                    res_HandCannon.toMenu(content, ItemResourceType.HandCannon);
                    res_HandCulvertin.toMenu(content, ItemResourceType.HandCulverin);
                    content.Add(new RbSeperationLine());
                    res_Rifle.toMenu(content, ItemResourceType.Rifle);
                    res_Blunderbus.toMenu(content, ItemResourceType.Blunderbus);
                    content.Add(new RbSeperationLine());

                    //res_BatteringRam.toMenu(content, ItemResourceType.UN_BatteringRam);
                    res_ballista.toMenu(content, ItemResourceType.Ballista);
                    res_Manuballista.toMenu(content, ItemResourceType.Manuballista);
                    content.Add(new RbSeperationLine());
                    res_Catapult.toMenu(content, ItemResourceType.Catapult);
                    res_SiegeCannonBronze.toMenu(content, ItemResourceType.SiegeCannonBronze);
                    content.Add(new RbSeperationLine());
                    res_ManCannonBronze.toMenu(content, ItemResourceType.ManCannonBronze);
                    res_SiegeCannonIron.toMenu(content, ItemResourceType.SiegeCannonIron);
                    content.Add(new RbSeperationLine());
                    res_ManCannonIron.toMenu(content, ItemResourceType.ManCannonIron);
                    break;

                case ResourcesSubTab.Overview_Armor:
                    content.Add(new RbSeperationLine());
                    res_paddedArmor.toMenu(content, ItemResourceType.PaddedArmor);
                    res_HeavyPaddedArmor.toMenu(content, ItemResourceType.HeavyPaddedArmor);
                    content.Add(new RbSeperationLine());
                    res_BronzeArmor.toMenu(content, ItemResourceType.BronzeArmor);
                    res_mailArmor.toMenu(content, ItemResourceType.IronArmor);
                    content.Add(new RbSeperationLine());
                    res_heavyMailArmor.toMenu(content, ItemResourceType.HeavyIronArmor);
                    res_LightPlateArmor.toMenu(content, ItemResourceType.LightPlateArmor);
                    content.Add(new RbSeperationLine());
                    res_FullPlateArmor.toMenu(content, ItemResourceType.FullPlateArmor);
                    res_MithrilArmor.toMenu(content, ItemResourceType.MithrilArmor);
                    break;

            }
        }

        public void workTab(RichBoxContent content)
        {
            var p = player.GetLocalPlayer();

            if (p.resourcesSubTab < ResourcesSubTab.Work_Resources || p.resourcesSubTab > ResourcesSubTab.Work_Armor)
            {
                p.resourcesSubTab = ResourcesSubTab.Work_Resources;
            }

            content.h2(DssRef.lang.Work_OrderPrioTitle, HudLib.TitleColor_Head);
            content.newLine();
            for (ResourcesSubTab resourcesSubTab = ResourcesSubTab.Work_Resources; resourcesSubTab <= ResourcesSubTab.Work_Armor; ++resourcesSubTab)
            {
                var tabContent = new RichBoxContent();
                //string text = null;
                switch (resourcesSubTab)
                {
                    case ResourcesSubTab.Work_Resources:
                        //tabContent.Add(new RbText(DssRef.lang.Work_OrderPrioTitle));
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                        break;

                    case ResourcesSubTab.Work_Metals:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Iron));
                        break;
                    case ResourcesSubTab.Work_Weapons:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                        break;
                    case ResourcesSubTab.Work_Projectile:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                        break;
                    case ResourcesSubTab.Work_Armor:
                        tabContent.Add(new RbImage(SpriteName.WarsResource_IronArmor));
                        break;
                }
                var subTab = new ArtButton(p.resourcesSubTab == resourcesSubTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected, 
                    tabContent,
                    new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                    {
                        p.resourcesSubTab = resourcesSubTab;
                    }, resourcesSubTab, SoundLib.menutab));
                //subTab.setGroupSelectionColor(HudLib.RbSettings, p.resourcesSubTab == resourcesSubTab);
                content.Add(subTab);
                //content.space(resourcesSubTab == ResourcesSubTab.Work_Armor ? 2 : 1);
            }
            
            content.Add(new RbSeperationLine());
           
            workTemplate.toHud(p, content, p.resourcesSubTab, this, null);
        }

        public void tradeTab(RichBoxContent content)
        {
            tradeTemplate.toHud(player.GetLocalPlayer(), content, this, null);
        }

        public void changeResourcePrice(float change, ItemResourceType resourceType, City city)
        {
            if (city != null)
            {
                city.tradeTemplate.changeResourcePrice(change, resourceType);
            }
            else
            { 
                tradeTemplate.changeResourcePrice(change, resourceType);
                var cityCounter = cities.counter();
                while (cityCounter.Next())
                {
                    cityCounter.sel.tradeTemplate.onFactionValueChange(tradeTemplate);
                }
            }
        }
        public void setWorkPrio(int set, WorkPriorityType priorityType, City city)
        {
            if (city != null)
            {
                city.workTemplate.setWorkPrio(set, priorityType);
            }
            else
            {
                workTemplate.setWorkPrio(set, priorityType);
                refreshCityWork();
            }
        }

        public void refreshCityWork()
        { 
            var cityCounter = cities.counter();
            while (cityCounter.Next())
            {
                cityCounter.sel.workTemplate.onFactionChange(workTemplate);
            }
        }

        public void tradeFollowFactionClick(ItemResourceType resourceType, City city)
        {
            city.tradeTemplate.followFactionClick(resourceType, tradeTemplate);
        }

        public void workFollowFactionClick(WorkPriorityType prioType, City city)
        {
            city.workTemplate.followFactionClick(prioType, workTemplate);
        }

        public bool calcCost(int cost, ref int totalCost, City city) {
            totalCost += cost;

            if (DssRef.storage.centralGold)
            {
                return gold >= totalCost;
            }
            else
            {
                return city.gold >= totalCost;
            }
        }

        public bool hasMoney(int cost, City city)
        {
            if (DssRef.storage.centralGold)
            {
                return gold >= cost;
            }
            else
            {
                return city.gold >= cost;
            }
        }

        public bool payMoney(int cost, bool allowDept, City city)
        {
            if (player.IsLocalPlayer() && StartupSettings.EndlessResources)
            {
                return true;
            }

            if (DssRef.storage.centralGold)
            {
                if (allowDept || gold >= cost)
                {
                    gold -= cost;
                    return true;
                }
            }
            else
            {
                if (allowDept || city.gold >= cost)
                {
                    city.gold -= cost;
                    return true;
                }
            }
            return false;
        }
        public int payMoney_MuchAsPossible(int cost, City city)
        {
            if (DssRef.storage.centralGold)
            {
               return  pay(ref gold);
            }
            else
            {
               return pay(ref city.gold);
            }

            int pay(ref int gold)
            {
                if (gold > 0)
                {
                    int canPay = lib.SmallestValue(gold, cost);
                    gold-= canPay;
                    return canPay;
                }
                return 0;
            }
        }


        public void gainMoney(int value, City city)
        {
            if (DssRef.storage.centralGold)
            {
                gold += value;
            }
            else
            { 
                city.gold += value;        
            }
        }

        public void addMoney_factionWide(int value)
        {   
            gold += value;

            if (cities.Count > 0)
            {
                int perCity = value / cities.Count;

                var citiesC = cities.counter();
                while (citiesC.Next())
                {
                    citiesC.sel.gold += perCity;
                }
            }           
        }

        

        public void resources_updateAsynch(float oneSecondUpdate)
        {
            //int cityIncomeCount = 0;
            int workForceCount = 0;
            //int nobel = 0;
            var citiesC = cities.counter();
            CityEconomyData newCitiesEconomy = new CityEconomyData();
            float citiesFoodProduce = 0;
            float citiesFoodSpend = 0;
            float soldResources = 0;

            while (citiesC.Next())
            {
                //citiesC.sel.updateIncome_asynch();
                CityEconomyData data = citiesC.sel.calcIncome_async();
                newCitiesEconomy.Add(data);
                //cityIncomeCount += data.total();
                workForceCount += citiesC.sel.workForce.amount;
                citiesFoodProduce += citiesC.sel.foodProduction.displayValue_sec;
                citiesFoodSpend += citiesC.sel.foodSpending.displayValue_sec;
                soldResources += citiesC.sel.soldResources.displayValue_sec;

                //if (citiesC.sel.nobelHouse)
                //{
                //    ++nobel;
                //}
            }

            totalWorkForce = workForceCount;
            citiesEconomy = newCitiesEconomy;
            //cityIncome = newCitiesEconomy.total();
            //nobelHouseCount = nobel;

            CityFoodProduction = Convert.ToInt32(citiesFoodProduce);
            CityFoodSpending = Convert.ToInt32(citiesFoodSpend);
            CitySoldResources = Convert.ToInt32(soldResources);

            //float totalArmiesUpkeep = 0;
            float foodImport = 0;
            float foodBlackMarket = 0;

            float totalArmiesFoodUpkeep = 0;
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                float energyUpkeep = 0;
                float moneyCarry = 0;
                //float armyUpkeep = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    groups.sel.Upkeep(ref energyUpkeep, ref moneyCarry);
                }

                float foodUpkeep = energyUpkeep / DssRef.difficulty.FoodEnergySett;

                //totalArmiesUpkeep += armyUpkeep;
                foodImport += armiesC.sel.foodCosts_import.displayValue_sec;
                foodBlackMarket += armiesC.sel.foodCosts_blackmarket.displayValue_sec;

                totalArmiesFoodUpkeep += foodUpkeep;
         
                armiesC.sel.goldCarryCapacity = Convert.ToInt32(moneyCarry);
                armiesC.sel.foodUpkeep = foodUpkeep;
                armiesC.sel.food -= foodUpkeep * oneSecondUpdate;
                if (armiesC.sel.food < -foodUpkeep * 60)
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
            
            armyFoodImportCost = Convert.ToInt32(foodImport);
            armyFoodBlackMarketCost = Convert.ToInt32(foodBlackMarket);
            armyFoodUpkeep = Convert.ToInt32(totalArmiesFoodUpkeep);
        }

        public void resourceOverviewOneSecondUpdate()
        {
            res_wood.oneSecondUpdate();
            res_fuel.oneSecondUpdate();
            res_stone.oneSecondUpdate();
            res_rawFood.oneSecondUpdate();
            res_food.oneSecondUpdate();
            res_beer.oneSecondUpdate();
            res_coolingfluid.oneSecondUpdate();
            res_skinLinnen.oneSecondUpdate();

            res_ironore.oneSecondUpdate();
            res_TinOre.oneSecondUpdate();
            res_CupperOre.oneSecondUpdate();
            res_LeadOre.oneSecondUpdate();
            res_SilverOre.oneSecondUpdate();

            res_iron.oneSecondUpdate();
            res_Tin.oneSecondUpdate();
            res_Cupper.oneSecondUpdate();
            res_Lead.oneSecondUpdate();
            res_Silver.oneSecondUpdate();
            res_RawMithril.oneSecondUpdate();
            res_Sulfur.oneSecondUpdate();

            res_Bronze.oneSecondUpdate();
            res_Steel.oneSecondUpdate();
            res_CastIron.oneSecondUpdate();
            res_BloomeryIron.oneSecondUpdate();
            res_Mithril.oneSecondUpdate();

            res_Toolkit.oneSecondUpdate();
            res_Wagon2Wheel.oneSecondUpdate();
            res_Wagon4Wheel.oneSecondUpdate();
            res_BlackPowder.oneSecondUpdate();
            res_GunPowder.oneSecondUpdate();
            res_LedBullet.oneSecondUpdate();

            res_sharpstick.oneSecondUpdate();
            res_BronzeSword.oneSecondUpdate();
            res_shortsword.oneSecondUpdate();
            res_Sword.oneSecondUpdate();
            res_LongSword.oneSecondUpdate();
            res_HandSpear.oneSecondUpdate();
            res_MithrilSword.oneSecondUpdate();

            res_Warhammer.oneSecondUpdate();
            res_twohandsword.oneSecondUpdate();
            res_knightslance.oneSecondUpdate();
            res_SlingShot.oneSecondUpdate();
            res_ThrowingSpear.oneSecondUpdate();
            res_bow.oneSecondUpdate();
            res_longbow.oneSecondUpdate();
            res_crossbow.oneSecondUpdate();
            res_MithrilBow.oneSecondUpdate();

            res_HandCannon.oneSecondUpdate();
            res_HandCulvertin.oneSecondUpdate();
            res_Rifle.oneSecondUpdate();
            res_Blunderbus.oneSecondUpdate();

            res_BatteringRam.oneSecondUpdate();
            res_ballista.oneSecondUpdate();
            res_Manuballista.oneSecondUpdate();
            res_Catapult.oneSecondUpdate();
            res_SiegeCannonBronze.oneSecondUpdate();
            res_ManCannonBronze.oneSecondUpdate();
            res_SiegeCannonIron.oneSecondUpdate();
            res_ManCannonIron.oneSecondUpdate();

            res_paddedArmor.oneSecondUpdate();
            res_HeavyPaddedArmor.oneSecondUpdate();
            res_BronzeArmor.oneSecondUpdate();
            res_mailArmor.oneSecondUpdate();
            res_heavyMailArmor.oneSecondUpdate();
            res_LightPlateArmor.oneSecondUpdate();
            res_FullPlateArmor.oneSecondUpdate();
            res_MithrilArmor.oneSecondUpdate();
        }

        public void updateResourceOverview_async()
        {
            res_wood.clearCurrent();
            res_fuel.clearCurrent();
            res_stone.clearCurrent();
            res_rawFood.clearCurrent();
            res_food.clearCurrent();
            res_beer.clearCurrent();
            res_coolingfluid.clearCurrent();
            res_skinLinnen.clearCurrent();

            res_ironore.clearCurrent();
            res_TinOre.clearCurrent();
            res_CupperOre.clearCurrent();
            res_LeadOre.clearCurrent();
            res_SilverOre.clearCurrent();

            res_iron.clearCurrent();
            res_Tin.clearCurrent();
            res_Cupper.clearCurrent();
            res_Lead.clearCurrent();
            res_Silver.clearCurrent();
            res_RawMithril.clearCurrent();
            res_Sulfur.clearCurrent();

            res_Bronze.clearCurrent();
            res_Steel.clearCurrent();
            res_CastIron.clearCurrent();
            res_BloomeryIron.clearCurrent();
            res_Mithril.clearCurrent();

            res_Toolkit.clearCurrent();
            res_Wagon2Wheel.clearCurrent();
            res_Wagon4Wheel.clearCurrent();
            res_BlackPowder.clearCurrent();
            res_GunPowder.clearCurrent();
            res_LedBullet.clearCurrent();

            res_sharpstick.clearCurrent();
            res_BronzeSword.clearCurrent();
            res_shortsword.clearCurrent();
            res_Sword.clearCurrent();
            res_LongSword.clearCurrent();
            res_HandSpear.clearCurrent();
            res_MithrilSword.clearCurrent();

            res_Warhammer.clearCurrent();
            res_twohandsword.clearCurrent();
            res_knightslance.clearCurrent();
            res_SlingShot.clearCurrent();
            res_ThrowingSpear.clearCurrent();
            res_bow.clearCurrent();
            res_longbow.clearCurrent();
            res_crossbow.clearCurrent();
            res_MithrilBow.clearCurrent();

            res_HandCannon.clearCurrent();
            res_HandCulvertin.clearCurrent();
            res_Rifle.clearCurrent();
            res_Blunderbus.clearCurrent();

            res_BatteringRam.clearCurrent();
            res_ballista.clearCurrent();
            res_Manuballista.clearCurrent();
            res_Catapult.clearCurrent();
            res_SiegeCannonBronze.clearCurrent();
            res_ManCannonBronze.clearCurrent();
            res_SiegeCannonIron.clearCurrent();
            res_ManCannonIron.clearCurrent();

            res_paddedArmor.clearCurrent();
            res_HeavyPaddedArmor.clearCurrent();
            res_BronzeArmor.clearCurrent();
            res_mailArmor.clearCurrent();
            res_heavyMailArmor.clearCurrent();
            res_LightPlateArmor.clearCurrent();
            res_FullPlateArmor.clearCurrent();
            res_MithrilArmor.clearCurrent();

            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                res_wood.current += citiesC.sel.res_wood.amount;
                res_fuel.current += citiesC.sel.res_fuel.amount;
                res_stone.current += citiesC.sel.res_stone.amount;
                res_rawFood.current += citiesC.sel.res_rawFood.amount;
                res_food.current += citiesC.sel.res_food.amount;
                res_beer.current += citiesC.sel.res_beer.amount;
                res_coolingfluid.current += citiesC.sel.res_coolingfluid.amount;
                res_skinLinnen.current += citiesC.sel.res_skinLinnen.amount;

                res_ironore.current += citiesC.sel.res_ironore.amount;
                res_TinOre.current += citiesC.sel.res_TinOre.amount;
                res_CupperOre.current += citiesC.sel.res_CupperOre.amount;
                res_LeadOre.current += citiesC.sel.res_LeadOre.amount;
                res_SilverOre.current += citiesC.sel.res_SilverOre.amount;

                res_iron.current += citiesC.sel.res_iron.amount;
                res_Tin.current += citiesC.sel.res_Tin.amount;
                res_Cupper.current += citiesC.sel.res_Cupper.amount;
                res_Lead.current += citiesC.sel.res_Lead.amount;
                res_Silver.current += citiesC.sel.res_Silver.amount;
                res_RawMithril.current += citiesC.sel.res_RawMithril.amount;
                res_Sulfur.current += citiesC.sel.res_Sulfur.amount;

                res_Bronze.current += citiesC.sel.res_Bronze.amount;
                res_Steel.current += citiesC.sel.res_Steel.amount;
                res_CastIron.current += citiesC.sel.res_CastIron.amount;
                res_BloomeryIron.current += citiesC.sel.res_BloomeryIron.amount;
                res_Mithril.current += citiesC.sel.res_Mithril.amount;

                res_Toolkit.current += citiesC.sel.res_Toolkit.amount;
                res_Wagon2Wheel.current += citiesC.sel.res_Wagon2Wheel.amount;
                res_Wagon4Wheel.current += citiesC.sel.res_Wagon4Wheel.amount;
                res_BlackPowder.current += citiesC.sel.res_BlackPowder.amount;
                res_GunPowder.current += citiesC.sel.res_GunPowder.amount;
                res_LedBullet.current += citiesC.sel.res_LedBullet.amount;

                res_sharpstick.current += citiesC.sel.res_sharpstick.amount;
                res_BronzeSword.current += citiesC.sel.res_BronzeSword.amount;
                res_shortsword.current += citiesC.sel.res_shortsword.amount;
                res_Sword.current += citiesC.sel.res_Sword.amount;
                res_LongSword.current += citiesC.sel.res_LongSword.amount;
                res_HandSpear.current += citiesC.sel.res_HandSpear.amount;
                res_MithrilSword.current += citiesC.sel.res_MithrilSword.amount;

                res_Warhammer.current += citiesC.sel.res_Warhammer.amount;
                res_twohandsword.current += citiesC.sel.res_twohandsword.amount;
                res_knightslance.current += citiesC.sel.res_knightslance.amount;
                res_SlingShot.current += citiesC.sel.res_SlingShot.amount;
                res_ThrowingSpear.current += citiesC.sel.res_ThrowingSpear.amount;
                res_bow.current += citiesC.sel.res_bow.amount;
                res_longbow.current += citiesC.sel.res_longbow.amount;
                res_crossbow.current += citiesC.sel.res_crossbow.amount;
                res_MithrilBow.current += citiesC.sel.res_MithrilBow.amount;

                res_HandCannon.current += citiesC.sel.res_HandCannon.amount;
                res_HandCulvertin.current += citiesC.sel.res_HandCulvertin.amount;
                res_Rifle.current += citiesC.sel.res_Rifle.amount;
                res_Blunderbus.current += citiesC.sel.res_Blunderbus.amount;

                res_BatteringRam.current += citiesC.sel.res_BatteringRam.amount;
                res_ballista.current += citiesC.sel.res_ballista.amount;
                res_Manuballista.current += citiesC.sel.res_Manuballista.amount;
                res_Catapult.current += citiesC.sel.res_Catapult.amount;
                res_SiegeCannonBronze.current += citiesC.sel.res_SiegeCannonBronze.amount;
                res_ManCannonBronze.current += citiesC.sel.res_ManCannonBronze.amount;
                res_SiegeCannonIron.current += citiesC.sel.res_SiegeCannonIron.amount;
                res_ManCannonIron.current += citiesC.sel.res_ManCannonIron.amount;

                res_paddedArmor.current += citiesC.sel.res_paddedArmor.amount;
                res_HeavyPaddedArmor.current += citiesC.sel.res_HeavyPaddedArmor.amount;
                res_BronzeArmor.current += citiesC.sel.res_BronzeArmor.amount;
                res_mailArmor.current += citiesC.sel.res_mailArmor.amount;
                res_heavyMailArmor.current += citiesC.sel.res_heavyMailArmor.amount;
                res_LightPlateArmor.current += citiesC.sel.res_LightPlateArmor.amount;
                res_FullPlateArmor.current += citiesC.sel.res_FullPlateArmor.amount;
                res_MithrilArmor.current += citiesC.sel.res_MithrilArmor.amount;

            }
        }
    }
}
