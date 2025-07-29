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

        public const int GoldToCopper = 100;
        public const float CopperToGold = 1f / GoldToCopper;

        public int copper;

        public Money(int copper)
        {
            this.copper = copper;
        }

        public void AddGold(int add)
        {
            copper += add * GoldToCopper;
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

        public bool PayUpkeep(float payCopper)
        {
            if (copper >= payCopper)
            {
                copper -= (int)payCopper;
                return true;
            }
            else
            {
                if (copper > 0)
                {
                    copper = 0;
                }

                return false;
            }
        }

        public int GetGold()
        { return (int)(copper * CopperToGold); }

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
            float gold = copper * CopperToGold;
            if (gold < 10)
            {
                return TextLib.TwoDecimal(gold);
            }
            else
            {
                return TextLib.LargeNumber((int)gold);
            }
        }
    }
}
