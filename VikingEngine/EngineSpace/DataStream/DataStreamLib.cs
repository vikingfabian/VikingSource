using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    static class DataStreamLib
    {

        const int GrowingAddValueMaxByte = byte.MaxValue - 1;
        /// <summary>
        /// Best for values that mostly are between 0-200 and rarely larger than 600
        /// </summary>
        public static void WriteGrowingAddValue(System.IO.BinaryWriter w, int value)
        {//REMARK, can't write negative values
            while (value > GrowingAddValueMaxByte)
            {
                w.Write(byte.MaxValue);
                value -= GrowingAddValueMaxByte;
            }
            w.Write((byte)value);
        }
        public static int ReadGrowingAddValue(System.IO.BinaryReader r)
        {
            int result = 0;
            while (true)
            {
                byte value = r.ReadByte();
                if (value == byte.MaxValue)
                {
                    result += GrowingAddValueMaxByte;
                }
                else
                {
                    result += value;
                    return result;
                }
            }
        }


        const int GrowingBitShift = 7;
        const int GrowingBitShiftValueMaxByte = 127;
        const int EightBit = 128;
        /// <summary>
        /// Best for values that mostly are between 0-100 but frequently extend beyond 600
        /// </summary>
        public static void WriteGrowingBitShiftValue(System.IO.BinaryWriter w, int value)
        {//REMARK, can't write negative values
            while (value > GrowingBitShiftValueMaxByte)
            {
                w.Write((byte)((value & GrowingBitShiftValueMaxByte) + EightBit));
                value = value >> GrowingBitShift;
            }
            w.Write((byte)(value));
        }
        public static int ReadGrowingBitShiftValue(System.IO.BinaryReader r)
        {
            int result = 0;
            int numShifts = 0;
            while (true)
            {
                byte value = r.ReadByte();
                if (value > GrowingBitShiftValueMaxByte)
                {
                    result += (value & GrowingBitShiftValueMaxByte) << numShifts;
                }
                else
                {
                    return result + (value << numShifts);
                }
                numShifts += GrowingBitShift;
            }
        }

        public static void TestGrowingWriter(int value)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);

            WriteGrowingBitShiftValue(w, value);
            WriteGrowingAddValue(w, value);

            System.IO.BinaryReader r = new System.IO.BinaryReader(s);
            r.BaseStream.Position = 0;
            int shiftRes = ReadGrowingBitShiftValue(r);
            int addRes = ReadGrowingAddValue(r);

            if (value != shiftRes || addRes != value)
            {
                throw new Exception();
            }

            //test 512
            s = new System.IO.MemoryStream();
            w = new System.IO.BinaryWriter(s);
            WriteGrowingBitShiftValue(w, 512);
            byte[] test = new byte[(int)(w.BaseStream.Length)];
            w.BaseStream.Position = 0;
            w.BaseStream.Read(test, 0, test.Length);
        }

        public static void WritePercent(float percent, System.IO.BinaryWriter w)
        {
            w.Write((byte)(percent * 100f));
        }
        public static float ReadPercent(System.IO.BinaryReader r)
        {
            return r.ReadByte() * 0.01f;
        }

        static string removePathDir(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == FilePath.Dir)
                {
                    return path.Remove(0, i);
                }
            }
            return path;
        }
    }

    interface IStreamIOCallback
    {
        void SaveComplete(bool save, int player, bool completed, byte[] value);
    }

    
}
