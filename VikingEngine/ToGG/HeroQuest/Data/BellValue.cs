using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG
{
    struct BellValue
    {
        const float CenterPercChance = 0.68f;
        const float LeftPercChance = (1f - CenterPercChance) / 2f;

        public int value;

        public BellValue(int value)
        {
            this.value = value;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)value);
        }

        public static BellValue Read(System.IO.BinaryReader r)
        {
            return new BellValue(r.ReadByte());
        }

        public Damage NextDamage()
        {
            int result = Next();

            return new Damage(result, DamageApplyType.Direct);
        }

        public Damage NextDamage(DiceRoll dice)
        {
            int result = Next(dice);

            return new Damage(result, DamageApplyType.Direct);
        }


        public int Next(DiceRoll dice)
        {
            return fromRndValue(dice.next());
        }

        public int Next()
        {
            return fromRndValue(Ref.rnd.Float());
        }

        int fromRndValue(float rnd)
        {
            if (rnd < CenterPercChance)
            {
                return value;
            }
            else if (rnd < CenterPercChance + LeftPercChance)
            {
                return value - 1;
            }
            else
            {
                return value + 1;
            }
        }

        public float MedianValue()
        {
            return value;
        }

        public string IntervalToString()
        {
            return (value - 1).ToString() + "-" + (value + 1).ToString();
        }
        
        public static bool operator ==(BellValue value1, BellValue value2)
        {
            return value1.value == value2.value;
        }
        public static bool operator !=(BellValue value1, BellValue value2)
        {
            return value1.value != value2.value;
        }

        public string ValueToString()
        {
            return "~" + value.ToString();
        }

        public override string ToString()
        {
            return "Bell value (" + value.ToString() + ")";
        }
    }
}
