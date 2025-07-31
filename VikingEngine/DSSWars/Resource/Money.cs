using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Resource
{
    struct Money
    {
        public static readonly Money Zero = new Money(0);

        const long GoldToCopper = 100;
        const double CopperToGold = 1.0 / GoldToCopper;

        public long copper;

        public Money(int copper)
        {
            this.copper = copper;
        }

        public void AddGold(int add)
        {
            //Debug.CrashCorruptValue(add);
            copper += add * GoldToCopper;
        }

        public void AddCopper(int add)
        {
            copper += add;
        }

        public int payGold_MuchAsPossible(int goldCost)
        {
            if (copper >= CopperToGold)
            {
                int canPay = lib.SmallestValue((int)(copper * CopperToGold), goldCost);
                goldCost -= canPay;
                return canPay;
            }
            return 0;
        }

        public long GetGold()
        { return (long)(copper * CopperToGold); }

        public static string CopperToGoldString_Decimal(int copper)
        {
            return TextLib.TwoDecimal(copper * CopperToGold);
        }

        public static string CopperToGoldString_Large(int copper)
        {
            return TextLib.LargeNumber((int)(copper * CopperToGold));
        }

        public static string CopperToGoldString_Dynamic(int copper)
        {
            double gold = copper * CopperToGold;
            if (gold < 10)
            {
                return TextLib.TwoDecimal(gold);
            }
            else
            {
                return TextLib.LargeNumber((long)gold);
            }
        }
    }
}
