using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class PlaySettings
    {
        public Players.DarkLordPlayer darkLordPlayer;
        public int Faction_SouthHara;
        public int Faction_GreenWood;
        public int Faction_DarkFollower;
        public int Faction_UnitedKingdom;

        public int Faction_DyingMonger;
        public int Faction_DyingHate;
        public int Faction_DyingDestru;


        public int AiArmyPurchase_MoneyMin;
        public int AiArmyPurchase_IncomeMin;

        public int AiArmyPurchase_MoneyMin_Aggresive;
        public int AiArmyPurchase_IncomeMin_Aggresive;

        int aiDelayTimeSec = 0;
      
        //public bool AiDelay = true;

        public PlaySettings() 
        {
            DssRef.settings = this;

            aiDelayTimeSec = DssRef.difficulty.aiDelayTimeSec;

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

        //public void OneSecondUpdate()
        //{
        //    if (DssRef.state.localPlayers[0].tutorial == null)
        //    {
        //        if (--aiDelayTimeSec <= 0)
        //        {
        //            AiDelay = false;
        //        }
        //    }            
        //}

        

        public void writeGameState(System.IO.BinaryWriter w)
        {
            //w.Write(aiDelayTimeSec);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            if (subversion < 31)
            {
                var aiDelayTimeSec = r.ReadInt32();

            }
        }
    }
}
