using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
   
    struct ThreeBytes
    {
        public int Values;

        public ThreeBytes(int values)
        {
            this.Values = values;
        }
        public ThreeBytes(int value1, int value2, int value3)
        {
            Values = value1 + value2 << PublicConstants.ByteBitLenght * 1 + value3 << PublicConstants.ByteBitLenght * 2;
        }
        public int Get(int index)
        {
            return (Values >> (PublicConstants.ByteBitLenght * index)) & byte.MaxValue;
        }
        public void Set(int index, int value)
        {
            Values = Values & (int.MaxValue - (byte.MaxValue << (PublicConstants.ByteBitLenght * index)));
            Values += value << (PublicConstants.ByteBitLenght * index);
        }

        public static void Test()
        {
            ThreeBytes test = new ThreeBytes();
            const int val1 = 20;
            const int val2 = 244;
            const int val3 = 33;

            test.Set(0, val1);
            test.Set(1, val2);
            test.Set(2, val3);

            if (test.Get(0) != val1)
                throw new Exception();
            if (test.Get(1) != val2)
                throw new Exception();
            if (test.Get(2) != val3)
                throw new Exception();

        }
      
    }

    struct TwoShorts
    {
        public int Values;

        public TwoShorts(int values)
        {
            this.Values = values;
        }
        public TwoShorts(int value1, int value2)
        {
            Values = 0;
            Value1 = value1;
            Value2 = value2;
        }

        public int Value1
        {
            get
            {
                return Values & short.MaxValue;
            }
            set
            {
                Values = Values & (int.MaxValue - short.MaxValue);
                Values += value;
            }

        }

        public int Value2
        {
            get
            {
                return Values >> PublicConstants.ShortBitLenght;
            }
            set
            {
                Values = Values & short.MaxValue;
                Values += value << PublicConstants.ShortBitLenght;
            }
        }
    }


    ///// <summary>
    ///// Useful for an enum of properties
    ///// </summary>
    //struct Int32Bits
    //{
    //    int value;

    //    public bool Get(int index)
    //    {
    //        int pow = indexToPower(index);
    //        return (value & pow) == pow;
    //    }
    //    public void Set(int index, bool bit)
    //    {
    //        int pow = indexToPower(index);
    //        if (bit)
    //        {
    //            this.value |= pow; 
    //        }
    //        else
    //        {
    //            pow = int.MaxValue - pow; //set the selected bit to zero
    //            value &= pow;
    //        }
    //    }

    //    int indexToPower(int index)
    //    {
    //        return 1 << index;
    //    }
    //}

    struct TwoHalfByte : IBinaryIOobj
    {
        public const byte MaxValue = 15;
        byte bitArray;
        const byte FirstHalf = MaxValue;
        const byte SecHalf = byte.MaxValue - MaxValue;

        public TwoHalfByte(byte value)
        {
            bitArray = value;
        }
       
        public TwoHalfByte(byte value1, byte value2)
        {
            bitArray = value1;
            Value2 = value2;
        }
        public TwoHalfByte(int value1, int value2)
        {
            bitArray = (byte)value1;
            Value2 = (byte)value2;
        }

        public byte Value1
        {
            get
            {
                return (byte)(bitArray & FirstHalf);
            }
            set
            {
                bitArray |= value;
            }
        }

        const byte HalfByteLenght = 4;
        public byte Value2
        {
            get
            {
                return (byte)((bitArray & SecHalf) >> HalfByteLenght);
            }
            set
            {
                value <<= HalfByteLenght;
                bitArray |= value;
            }
        }

        public float Value1Percent
        {
            get
            {
                return (float)Value1 / MaxValue;
            }
        }
        public float Value2Percent
        {
            get
            {
                return (float)Value2 / MaxValue;
            }
        }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(bitArray);
        }
        public void read(System.IO.BinaryReader r)
        {
            bitArray = r.ReadByte();
        }
        public static TwoHalfByte FromStream(System.IO.BinaryReader r)
        {
            return new TwoHalfByte(r.ReadByte());
        }

        public override string ToString()
        {
            return Value1.ToString() + ", " + Value2.ToString();
        }
    }

    struct IntPair
    {
        public int key, value;

        public IntPair(int key, int value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
