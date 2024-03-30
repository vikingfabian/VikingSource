using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class PlaySettings
    {
        public int Faction_SouthHara;
        public int Faction_GreenWood;

        public int AiArmyPurchase_MoneyMin;
        public int AiArmyPurchase_IncomeMin;

        public int AiArmyPurchase_MoneyMin_Aggresive;
        public int AiArmyPurchase_IncomeMin_Aggresive;

        public PlaySettings() 
        {
            DssRef.settings = this;

            switch (DssRef.difficulty.aiAggressivity)
            {
                case AiAggressivity.Low:
                    AiArmyPurchase_MoneyMin = DssLib.GroupDefaultCost * 20;
                    AiArmyPurchase_IncomeMin = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 15);

                    AiArmyPurchase_MoneyMin_Aggresive = DssLib.GroupDefaultCost * 20;
                    AiArmyPurchase_IncomeMin_Aggresive = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 15);
                    break;
                case AiAggressivity.Medium:
                    AiArmyPurchase_MoneyMin = DssLib.GroupDefaultCost * 20;
                    AiArmyPurchase_IncomeMin = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 15);

                    AiArmyPurchase_MoneyMin_Aggresive = DssLib.GroupDefaultCost * 10;
                    AiArmyPurchase_IncomeMin_Aggresive = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 5);
                    break;
                case AiAggressivity.High:
                    AiArmyPurchase_MoneyMin = DssLib.GroupDefaultCost * 40;
                    AiArmyPurchase_IncomeMin = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 20);

                    AiArmyPurchase_MoneyMin_Aggresive = DssLib.GroupDefaultCost * 5;
                    AiArmyPurchase_IncomeMin_Aggresive = Convert.ToInt32(DssLib.GroupDefaultUpkeep * 5);
                    break;

            }
            //bool haveMoney = faction.gold >= DssLib.GroupDefaultCost * 20;
            //bool haveIncome = faction.NetIncome() >= DssLib.GroupDefaultCost * (aggresive ? 5 : 15);
        }
    }
}
