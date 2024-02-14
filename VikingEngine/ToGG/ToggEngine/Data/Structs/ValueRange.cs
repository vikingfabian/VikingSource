using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG
{
    struct ValueRange
    {
        public ValueRangeType range;
        public int value;
        public int maxValue;

        public ValueRange(BellValue bellValue)
        {
            this.value = bellValue.value;
            this.maxValue = value;

            range = ValueRangeType.BellValue;
        }

        public ValueRange(int min, int max)
        {
            this.value = min;
            this.maxValue = max;

            range = ValueRangeType.Interval;
        }
        
        public ValueRange(int value)
        {
            this.value = value;
            this.maxValue = value;

            range = ValueRangeType.None;
        }

        public ValueRange(int value, ValueRangeType range)
        {
            this.value = value;
            this.maxValue = value;

            this.range = range;
        }

        

        void valueCheck()
        {
            value = Bound.Byte(value);
            Bound.Set(ref maxValue, value, byte.MaxValue);
        }

        public void set(int value)
        {
            this.value = value;
            valueCheck();
        }

        public void set(int value, ValueRangeType range)
        {
            this.value = value;
            this.range = range;
            valueCheck();
        }

        public void set(int min, int max)
        {
            value = min;
            maxValue = max;
            range = ValueRangeType.Interval;
            valueCheck();
        }

        public void Add(int add)
        {
            value += add;
            maxValue += add;
            valueCheck();
        }

        public void Add(ValueRange add)
        {
            value += add.value;
            maxValue += add.maxValue;
            valueCheck();
        }

        public int MedianValue()
        {
            return value;
        }

        public static bool operator ==(ValueRange value1, ValueRange value2)
        {
            if (value1.value == value2.value && value1.range == value2.range)
            {
                if (value1.range == ValueRangeType.Interval)
                {
                    return value1.maxValue == value2.maxValue;
                }
                return true;
            }

            return false;
        }
        public static bool operator !=(ValueRange value1, ValueRange value2)
        {
            if (value1.value != value2.value || value1.range != value2.range)
            {
                return true;
            }

            if (value1.range == ValueRangeType.Interval)
            {
                return value1.maxValue != value2.maxValue;
            }

            return false;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public string IntervalToString()
        {
            switch (range)
            {
                default:
                    return value.ToString();

                case ValueRangeType.BellValue:
                    return BellValue.IntervalToString();
            }
        }

        public int Next()
        {
            switch (range)
            {
                case ValueRangeType.BellValue:
                    return new BellValue(value).Next();
                default:
                    return value;
            }
        }

        public int Next(DiceRoll dice)
        {
            switch (range)
            {
                case ValueRangeType.BellValue:
                    return new BellValue(value).Next(dice);
                default:
                    return value;
            }
        }

        public string BellValueToString()
        {
            return "~" + value.ToString();
        }

        public BellValue BellValue => new BellValue(value);

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)value);
            w.Write((byte)maxValue);
            w.Write((byte)range);
        }

        public void read(System.IO.BinaryReader r)
        {
            value = r.ReadByte();
            maxValue = r.ReadByte();
            range = (ValueRangeType)r.ReadByte();
        }


    }

    enum ValueRangeType
    {
        None,
        BellValue,
        Interval,
    }
}
