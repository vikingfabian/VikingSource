using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    struct ListStatistics
    {
        float MaxValue;
        public int MaxValueIndex;
        float MinValue;
        public int MinValueIndex;

        public ListStatistics(float max, float min)
        {
            MaxValue = max;
            MinValue = min;
            MaxValueIndex = -1;
            MinValueIndex = -1;
        }
        public static readonly ListStatistics StartSetup = new ListStatistics(float.MinValue, float.MaxValue);

        public void NextMember(int index, float value)
        {
            if (value < MinValue)
            {
                MinValue = value;
                MinValueIndex = index;
            }
            if (value < MaxValue)
            {
                MaxValue = value;
                MaxValueIndex = index;
            }

        }
        
    }

    struct FourBytes
    {
        public int Value;

        public FourBytes(byte val1, byte val2)
            :this(val1, val2, byte.MinValue, byte.MinValue)
        {

        }
        public FourBytes(byte val1, byte val2, byte val3)
            : this(val1, val2, val3, byte.MinValue)
        {

        }
        public FourBytes(byte val1, byte val2, byte val3, byte val4)
        {
            Value = BitConverter.ToInt32(new byte[] { val1, val2, val3, val4 }, 0);
        }
 
        public FourBytes(int value)
        {
            this.Value = value;
        }

        public byte Get(int index)
        {
            return BitConverter.GetBytes(Value)[index];
        }
        public void Set(int index, int value)
        {
            this.Set(index, (byte)value);
        }
        public void Set(int index, byte value)
        {
            byte[] array = BitConverter.GetBytes(Value);
            array[index] = value;
            Value = BitConverter.ToInt32(array, 0);
        }

        public override string ToString()
        {
            byte[] array = BitConverter.GetBytes(Value);
            return array[0].ToString() + ", " + array[1].ToString() + ", " + array[2].ToString() + ", " + array[3].ToString();
        }
    }
}
