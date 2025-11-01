using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Resource
{
    struct Money
    {
        public static readonly Money Zero = new Money(0);

        public const long GoldToCopper = 100;
        public const double CopperToGold = 1.0 / GoldToCopper;

        public long copper;

        //public Money(int copper)
        //{
        //    this.copper = copper;
        //}

        public Money(long copper)
        {
            this.copper = copper;
        }

        public Money(float copperF)
        {
            this.copper = Convert.ToInt64(copperF);
        }

        public void AddGold(long add)
        {
            //Debug.CrashCorruptValue(add);
            copper += add * GoldToCopper;
        }

        public void SetGold(long gold)
        {
            //Debug.CrashCorruptValue(add);
            copper = gold * GoldToCopper;
        }

        public void AddCopper(long add)
        {
            copper += add;
        }

        public long payGold_MuchAsPossible(long goldCost)
        {
            if (copper >= CopperToGold)
            {
                long canPay = Math.Min((long)(copper * CopperToGold), goldCost);
                goldCost -= canPay;
                return canPay;
            }
            return 0;
        }

        public long GetGold()
        { return (long)(copper * CopperToGold); }

        public bool PayGold(float payGold, bool allowDept)
        {
            if (allowDept)
            {
                copper -= Convert.ToInt64(payGold * GoldToCopper);
                return true;
            }
            else
            {
                return PayUpkeep(payGold * GoldToCopper);
            }
            
        }

        public bool PayUpkeep(float payCopper)
        {
            if (payCopper == 0)
                return true;

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

        //public int GetGold()
        //{ return (int)(copper * CopperToGold); }

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

        public void write(System.IO.BinaryWriter w)
        { 
            w.Write(copper);
        }

        public void read(System.IO.BinaryReader r)
        {
            copper = r.ReadInt64();
        }

        public static Money operator +(Money a, Money b)
        {
            return new Money(a.copper + b.copper);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a.copper - b.copper);
        }

        public override string ToString()
        {
            return "Gold: " + GetGold().ToString();
        }
    }
}
