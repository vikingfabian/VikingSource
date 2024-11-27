using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
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

        public int MoneySecDiff()
        {
            return storeGold - previuosGold;
        }

        public void workTab(RichBoxContent content)
        {
            var p = player.GetLocalPlayer();
            workTemplate.toHud(p, content, p.workSubTab, this, null);
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

        //public void setWorkPrioSafeGuard(bool set, WorkPriorityType priorityType, City city)
        //{
        //    if (city != null)
        //    {
        //        city.workTemplate.setWorkPrioSafeGuard(set, priorityType);
        //    }
        //    else
        //    {
        //        workTemplate.setWorkPrioSafeGuard(set, priorityType);
        //        refreshCityWork();
        //    }
        //}

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

        public bool calcCost(int cost, ref int totalCost) {
            totalCost += cost;

            return gold >= totalCost;
        }

        public bool payMoney(int cost, bool allowDept)
        {
            if (player.IsPlayer() && StartupSettings.EndlessResources)
            {
                return true;
            }

            if (allowDept || gold >= cost)
            {
                gold -= cost;
                return true;
            }
            return false;
        }

        public void resources_oneSecUpdate()
        {
            

            CityTradeImport = CityTradeImportCounting;
            CityTradeExport = CityTradeExportCounting;
            CityTradeImportCounting -= CityTradeImport;
            CityTradeExportCounting -= CityTradeExport;

            double tax = citiesEconomy.tax(null);
            if (player.IsAi())
            {
                if (DssRef.settings.AiDelay)
                {
                    tax *= 0.05;
                }
                else if (player.aggressionLevel > AbsPlayer.AggressionLevel0_Passive)
                {
                    tax *= DssRef.difficulty.aiEconomyMultiplier;
                }
            }
            int income = Convert.ToInt32(tax - citiesEconomy.cityGuardUpkeep - DssLib.NobleHouseUpkeep * nobelHouseCount);

            
            gold += income;
            
            previuosGold = storeGold;
            storeGold = gold;
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
                //float armyUpkeep = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    groups.sel.Upkeep(ref energyUpkeep);
                }

                float foodUpkeep = energyUpkeep / DssRef.difficulty.FoodEnergySett;

                //totalArmiesUpkeep += armyUpkeep;
                foodImport += armiesC.sel.foodCosts_import.displayValue_sec;
                foodBlackMarket += armiesC.sel.foodCosts_blackmarket.displayValue_sec;

                totalArmiesFoodUpkeep += foodUpkeep;
         
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

        

        //public int NetIncome()
        //{
        //    return cityIncome - Convert.ToInt32(DssLib.NobleHouseUpkeep * nobelHouseCount);
        //}
        //public int TotalUpkeep()
        //{ 
        //    int total = Convert.ToInt32(DssLib.NobleHouseUpkeep * nobelHouseCount);
        //    return total;
        //}
    }
}
