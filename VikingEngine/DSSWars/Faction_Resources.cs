using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars
{
    //RESOURCES
    partial class Faction
    {
        public int gold = 40;
        public int totalWorkForce, cityIncome, armyUpkeep;
        public int nobelHouseCount = 0;

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
            if (player.aggressionLevel > AbsPlayer.AggressionLevel0_Passive && player.IsAi())
            { 
                income=MathExt.MultiplyInt(AiPlayer.EconomyMultiplier, income);
            }
            gold += income;
        }

        public void resources_updateAsynch()
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
            var armiesC = armies.counter();
            while (armiesC.Next())
            {
                float armyUpkeep = 0;

                var groups = armiesC.sel.groups.counter();
                while (groups.Next())
                {
                    armyUpkeep += groups.sel.Upkeep();
                    //var soldiers = groups.sel.soldiers.counter();
                    //while (soldiers.Next())
                    //{
                    //    armyUpkeep += soldiers.sel.upkeep;
                    //}
                }

                totalArmiesUpkeep += armyUpkeep;
                armiesC.sel.upkeep = Convert.ToInt32(armyUpkeep);
            }

            armyUpkeep = Convert.ToInt32(totalArmiesUpkeep);
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
