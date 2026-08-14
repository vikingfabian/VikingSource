using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.GameState;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    class PlaySettings
    {
        public Players.DarkLordPlayer darkLordPlayer;

        public PFaction Faction_SouthHara = PFaction.Empty;
        public PFaction Faction_GreenWood = PFaction.Empty;
        public PFaction Faction_DarkFollower = PFaction.Empty;
        public PFaction Faction_Barbarian = PFaction.Empty;
        public PFaction Faction_UnitedKingdom = PFaction.Empty;

        public PFaction Faction_DyingMonger = PFaction.Empty;
        public PFaction Faction_DyingHate = PFaction.Empty;
        public PFaction Faction_DyingDestru = PFaction.Empty;        

        public int AiArmyPurchase_MoneyMin;
        public int AiArmyPurchase_IncomeMin;

        public int AiArmyPurchase_MoneyMin_Aggresive;
        public int AiArmyPurchase_IncomeMin_Aggresive;

        int aiDelayTimeSec = 0;

        public PlayStateType playType = PlayStateType.Play;

        public List<string> returnFromEditorMenuStack = null;
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
            if (subversion < 44)
            {
                var aiDelayTimeSec = r.ReadInt32();

            }
        }
    }
}
