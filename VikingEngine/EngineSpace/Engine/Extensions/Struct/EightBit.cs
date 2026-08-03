using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{

    struct EightBit : IBinaryIOobj
    {
        public static readonly EightBit Zero = new EightBit(byte.MinValue);
        public static readonly EightBit AllTrue = new EightBit(byte.MaxValue);
        static readonly byte[] indexToBitValue = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

        public byte bitArray;

        public EightBit(
            bool value1,
            bool value2 = false,
            bool value3 = false,
            bool value4 = false,
            bool value5 = false,
            bool value6 = false,
            bool value7 = false,
            bool value8 = false)
        {
            bitArray = 0;

            if (value1)
                bitArray |= indexToBitValue[0];
            if (value2)
                bitArray |= indexToBitValue[1];
            if (value3)
                bitArray |= indexToBitValue[2];
            if (value4)
                bitArray |= indexToBitValue[3];
            if (value5)
                bitArray |= indexToBitValue[4];
            if (value6)
                bitArray |= indexToBitValue[5];
            if (value7)
                bitArray |= indexToBitValue[6];
            if (value8)
                bitArray |= indexToBitValue[7];

        }

        public EightBit(byte bitArray)
        {
            this.bitArray = bitArray;
        }

        public bool Get(int index)
        {
            if (index == byte.MaxValue)
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
                bitArray &= (byte)~indexToBitValue[index];
            }
        }

        public void Set_Safe(int index, bool value)
        {
            if (index >= 0 && index < indexToBitValue.Length)
            {
                Set(index, value);
            }
        }

        public static bool GetBit(byte bitArray, int index)
        {
            if (index == byte.MaxValue)
                return false;
            return (bitArray & indexToBitValue[index]) != 0;
        }

        public static byte SetBit(byte bitArray, int index, bool value)
        {
            if (value)
            {
                bitArray |= indexToBitValue[index];
            }
            else
            {
                bitArray &= (byte)~indexToBitValue[index];
            }

            return bitArray;
        }


        public void write(System.IO.BinaryWriter w)
        {
            w.Write(bitArray);
        }
        public void read(System.IO.BinaryReader r)
        {
            bitArray = r.ReadByte();
        }

        public EightBit(System.IO.BinaryReader r)
        {
            bitArray = 0;
            read(r);
        }

        public static EightBit FromStream(System.IO.BinaryReader r)
        {
            EightBit result = EightBit.Zero;
            result.read(r);
            return result;
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
    }

}
