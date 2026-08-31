using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    struct ThirtyTwoBit : IBinaryIOobj
    {
        public static readonly ThirtyTwoBit Zero = new ThirtyTwoBit(uint.MinValue);
        public static readonly ThirtyTwoBit AllTrue = new ThirtyTwoBit(uint.MaxValue);

        
        static readonly uint[] indexToBitValue = new uint[]
        {
        0x1u, 0x2u, 0x4u, 0x8u,
        0x10u, 0x20u, 0x40u, 0x80u,
        0x100u, 0x200u, 0x400u, 0x800u,
        0x1000u, 0x2000u, 0x4000u, 0x8000u,
        0x10000u, 0x20000u, 0x40000u, 0x80000u,
        0x100000u, 0x200000u, 0x400000u, 0x800000u,
        0x1000000u, 0x2000000u, 0x4000000u, 0x8000000u,
        0x10000000u, 0x20000000u, 0x40000000u, 0x80000000u
        };

        public uint bitArray;

        public ThirtyTwoBit(
            bool value1 = false, bool value2 = false, bool value3 = false, bool value4 = false,
            bool value5 = false, bool value6 = false, bool value7 = false, bool value8 = false,
            bool value9 = false, bool value10 = false, bool value11 = false, bool value12 = false,
            bool value13 = false, bool value14 = false, bool value15 = false, bool value16 = false,
            bool value17 = false, bool value18 = false, bool value19 = false, bool value20 = false,
            bool value21 = false, bool value22 = false, bool value23 = false, bool value24 = false,
            bool value25 = false, bool value26 = false, bool value27 = false, bool value28 = false,
            bool value29 = false, bool value30 = false, bool value31 = false, bool value32 = false)
        {
            bitArray = 0;

            if (value1) bitArray |= indexToBitValue[0];
            if (value2) bitArray |= indexToBitValue[1];
            if (value3) bitArray |= indexToBitValue[2];
            if (value4) bitArray |= indexToBitValue[3];
            if (value5) bitArray |= indexToBitValue[4];
            if (value6) bitArray |= indexToBitValue[5];
            if (value7) bitArray |= indexToBitValue[6];
            if (value8) bitArray |= indexToBitValue[7];
            if (value9) bitArray |= indexToBitValue[8];
            if (value10) bitArray |= indexToBitValue[9];
            if (value11) bitArray |= indexToBitValue[10];
            if (value12) bitArray |= indexToBitValue[11];
            if (value13) bitArray |= indexToBitValue[12];
            if (value14) bitArray |= indexToBitValue[13];
            if (value15) bitArray |= indexToBitValue[14];
            if (value16) bitArray |= indexToBitValue[15];
            if (value17) bitArray |= indexToBitValue[16];
            if (value18) bitArray |= indexToBitValue[17];
            if (value19) bitArray |= indexToBitValue[18];
            if (value20) bitArray |= indexToBitValue[19];
            if (value21) bitArray |= indexToBitValue[20];
            if (value22) bitArray |= indexToBitValue[21];
            if (value23) bitArray |= indexToBitValue[22];
            if (value24) bitArray |= indexToBitValue[23];
            if (value25) bitArray |= indexToBitValue[24];
            if (value26) bitArray |= indexToBitValue[25];
            if (value27) bitArray |= indexToBitValue[26];
            if (value28) bitArray |= indexToBitValue[27];
            if (value29) bitArray |= indexToBitValue[28];
            if (value30) bitArray |= indexToBitValue[29];
            if (value31) bitArray |= indexToBitValue[30];
            if (value32) bitArray |= indexToBitValue[31];
        }

        public ThirtyTwoBit(uint bitArray)
        {
            this.bitArray = bitArray;
        }

        public bool Get(int index)
        {
            if (index == int.MaxValue || index < 0 || index >= 32)
                return false;
            return (bitArray & indexToBitValue[index]) != 0;
        }

        public void Get(out bool value1)
        {
            value1 = Get(0);
        }

        public void Get(out bool value1, out bool value2)
        {
            value1 = Get(0);
            value2 = Get(1);
        }

        public void Get(out bool value1, out bool value2, out bool value3)
        {
            value1 = Get(0);
            value2 = Get(1);
            value3 = Get(2);
        }

        public void Get(out bool value1, out bool value2, out bool value3, out bool value4)
        {
            value1 = Get(0);
            value2 = Get(1);
            value3 = Get(2);
            value4 = Get(3);
        }

        public void Get(out bool value1, out bool value2, out bool value3, out bool value4, out bool value5)
        {
            value1 = Get(0);
            value2 = Get(1);
            value3 = Get(2);
            value4 = Get(3);
            value5 = Get(4);
        }

        public void Get(out bool value1, out bool value2, out bool value3, out bool value4, out bool value5, out bool value6)
        {
            value1 = Get(0);
            value2 = Get(1);
            value3 = Get(2);
            value4 = Get(3);
            value5 = Get(4);
            value6 = Get(5);
        }

        public void Set(int index, bool value)
        {
            if (value)
            {
                bitArray |= indexToBitValue[index];
            }
            else
            {
                bitArray &= ~indexToBitValue[index];
            }
        }

        public void Set_Safe(int index, bool value)
        {
            if (index >= 0 && index < indexToBitValue.Length)
            {
                Set(index, value);
            }
        }

        public static bool GetBit(uint bitArray, int index)
        {
            if (index == int.MaxValue || index < 0 || index >= 32)
                return false;
            return (bitArray & indexToBitValue[index]) != 0;
        }

        public static uint SetBit(uint bitArray, int index, bool value)
        {
            if (value)
            {
                bitArray |= indexToBitValue[index];
            }
            else
            {
                bitArray &= ~indexToBitValue[index];
            }

            return bitArray;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(bitArray);
        }

        public void read(System.IO.BinaryReader r)
        {
            bitArray = r.ReadUInt32();
        }

        public ThirtyTwoBit(System.IO.BinaryReader r)
        {
            bitArray = 0;
            read(r);
        }

        public static ThirtyTwoBit FromStream(System.IO.BinaryReader r)
        {
            ThirtyTwoBit result = ThirtyTwoBit.Zero;
            result.read(r);
            return result;
        }

        public bool InFilter(ThirtyTwoBit filter)
        {
            // A bitwise AND will return a non-zero value if 
            // there is at least one matching 'true' bit in both arrays.
            return (this.bitArray & filter.bitArray) != 0;
        }

        /// <summary>
        /// Bitwise OR combines the bits, keeping all 'true' bits from both
        /// </summary>
        public void Combine(ThirtyTwoBit other)
        {
            
            this.bitArray |= other.bitArray;
        }
        

        public override string ToString()
        {
            string result = "{";
            for (int i = 0; i < indexToBitValue.Length; ++i)
            {
                if (i != 0) result += ", ";
                result += Get(i).ToString();

            }
            return result + "}";
        }

        public bool HasValue()
        {
            return bitArray != uint.MinValue;
        }
        public bool IsEmpty()
        {
            return bitArray == uint.MinValue;
        }

    }
}
