using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars
{
    //RESOURCES
    partial class Faction
    {
        public int gold = 40;
        public int totalWorkForce, cityIncome, armyUpkeep, armyFoodUpkeep;
        public int nobelHouseCount = 0;

        public TradeTemplate tradeTemplate = new TradeTemplate();
        public WorkTemplate workTemplate = new WorkTemplate();

        public void workTab(RichBoxContent content)
        {
            workTemplate.toHud(player.GetLocalPlayer(), content, this, null);
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
        public void changeWorkPrio(int change, WorkPriorityType priorityType, City city)
        {
            if (city != null)
            {
                city.workTemplate.changeWorkPrio(change, priorityType);
            }
            else
            {
                workTemplate.changeWorkPrio(change, priorityType);
                var cityCounter = cities.counter();
                while (cityCounter.Next())
                {
                    cityCounter.sel.workTemplate.onFactionChange(workTemplate);
                }
                //tradeTemplate.changeResourcePrice(change, resourceType);
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
            int income = NetIncome();
            if ( player.IsAi())
            {                
                if (DssRef.settings.AiDelay)
                {
                    income = MathExt.MultiplyInt(0.05, income);
                }
                else if (player.aggressionLevel > AbsPlayer.AggressionLevel0_Passive)
                {
                    income = MathExt.MultiplyInt(AiPlayer.EconomyMultiplier, income);
                }
            }
            gold += income;
        }

        public void resources_updateAsynch(float oneSecondUpdate)
        {
            int cityIncomeCount = 0;
            int workForceCount = 0;
            int nobel = 0;
            var citiesC = cities.counter();
            while (citiesC.Next())
            {
                citiesC.sel.updateIncome_asynch();
                cityIncomeCount += citiesC.sel.income;
                workForceCount += citiesC.sel.workForce.Int();

                if (citiesC.sel.nobelHouse)
                {
                    ++nobel;
                }
            }

            totalWorkForce = workForceCount;
            cityIncome = cityIncomeCount;
            nobelHouseCount = nobel;

            float totalArmiesUpkeep = 0;
            float totalArmiesFoodUpkeep = 0;
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                float energyUpkeep = 0;
                float armyUpkeep = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    groups.sel.Upkeep(ref energyUpkeep);
                }

                float foodUpkeep = energyUpkeep / ResourceLib.FoodEnergy;

                totalArmiesUpkeep += armyUpkeep;
                totalArmiesFoodUpkeep += foodUpkeep;
         
                armiesC.sel.foodUpkeep = foodUpkeep;
                armiesC.sel.food -= foodUpkeep * oneSecondUpdate;
                if (armiesC.sel.food < -foodUpkeep * 60)
                {
                    Ref.update.AddSyncAction(new SyncAction(armiesC.sel.hungerDeserters));
                }
            }
            
            armyUpkeep = Convert.ToInt32(totalArmiesUpkeep);
            armyFoodUpkeep = Convert.ToInt32(totalArmiesFoodUpkeep);
        }

        

        public int NetIncome()
        {
            return cityIncome - TotalUpkeep();
        }
        public int TotalUpkeep()
        { 
            int total = armyUpkeep + Convert.ToInt32(DssLib.NobleHouseUpkeep * nobelHouseCount);
            return total;
        }
    }
}
