using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using VikingEngine.DataStream;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine
{
 

    static class StreamLib
    {
        public const string BackUpName = "_bak";

        public static void WriteFloatAsPercentU16(BinaryWriter w, float value, float max)
        {
            value = Math.Clamp(value, 0f, max);
            ushort encoded = (ushort)(value / max * ushort.MaxValue);
            w.Write(encoded);
        }

        public static void FloatAsPercentU16_WriteEmpty(BinaryWriter w)
        {
            w.Write(ushort.MinValue);
        }

        public static float ReadFloatFromPercentU16(BinaryReader r, float max)
        {
            ushort encoded = r.ReadUInt16();
            return (float)encoded / ushort.MaxValue * max;
        }

        /// <summary>
        /// Writes as 3 bytes
        /// </summary>
        public static void WriteColorStream_3B(System.IO.BinaryWriter w, Color col)
        {
            w.Write(col.R);
            w.Write(col.G);
            w.Write(col.B);
        }
        public static Color ReadColorStream_3B(System.IO.BinaryReader r)
        {
            return new Color(
                r.ReadByte(), r.ReadByte(), r.ReadByte());
        }
        public static void WriteString(System.IO.BinaryWriter w, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                w.Write((byte)0);
                return;
            }
            w.Write((byte)value.Length);
            w.Write(value.ToCharArray());
        }
        public static string ReadString_safe(System.IO.BinaryReader r)
        {
            try
            {
                byte length = r.ReadByte();
                if (length == 0)
                    return TextLib.EmptyString;
                return new string(r.ReadChars(length));
            }
            catch
            {

                return null;
            }
        }

        public static string ReadString(System.IO.BinaryReader r)
        {
            byte length = r.ReadByte();
            if (length == 0)
                return null;
            return new string(r.ReadChars(length));
        }

        public static void WriteVector(System.IO.BinaryWriter w, Vector3 value)
        {
            w.Write(value.X);
            w.Write(value.Y);
            w.Write(value.Z);
        }
        public static void WriteVector(System.IO.BinaryWriter w, Vector2 value)
        {
            w.Write(value.X);
            w.Write(value.Y);
        }
        public static Vector3 ReadVector3(System.IO.BinaryReader r)
        {
            return new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }

        public static Vector2 ReadVector2(System.IO.BinaryReader r)
        {
            return new Vector2(r.ReadSingle(), r.ReadSingle());
        }


        public static void WriteUInt24(System.IO.BinaryWriter w, int value)
        {
            //maximum value of 16,777,215
            // Write 3 bytes for X
            w.Write((byte)(value & 0xFF));         // 1st byte (Lowest 8 bits)
            w.Write((byte)((value >> 8) & 0xFF));  // 2nd byte (Middle 8 bits)
            w.Write((byte)((value >> 16) & 0xFF)); // 3rd byte (Highest 8 bits of our 24)
        }

        public static int ReadUInt24(System.IO.BinaryReader r)
        {
            // Reconstruct X from 3 bytes
            return r.ReadByte() | (r.ReadByte() << 8) | (r.ReadByte() << 16);
        }


        public static void ValueIO(ref float value, System.IO.BinaryWriter w, System.IO.BinaryReader r)
        {
            if (w != null)
            {
                w.Write(value);
            }
            else
            {
                value = r.ReadSingle();
            }
        }
        public static void ValueIO(ref Vector2 value, System.IO.BinaryWriter w, System.IO.BinaryReader r)
        {
            if (w != null)
            {
                w.Write(value.X);
                w.Write(value.Y);
            }
            else
            {
                value.X = r.ReadSingle();
                value.Y = r.ReadSingle();
            }
        }
        public static void ValueIO(ref Vector3 value, System.IO.BinaryWriter w, System.IO.BinaryReader r)
        {
            if (w != null)
            {
                w.Write(value.X);
                w.Write(value.Y);
                w.Write(value.Z);
            }
            else
            {
                value.X = r.ReadSingle();
                value.Y = r.ReadSingle();
                value.Z = r.ReadSingle();
            }
        }

        public static int StringSimpleHash(string text)
        {
            int hash = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                hash += (int)text[i] * (i + 1); 
            }

            return hash;
        }

        public static void WriteDir(int dir, System.IO.BinaryWriter w)
        {
            sbyte byteDir = (sbyte)dir;
            w.Write(byteDir);
        }

        public static int ReadDir(System.IO.BinaryReader r)
        {
            return (int)r.ReadSByte();
        }

        const float FloatMultiplier = 50; //Accuracy of 2%

        public static byte WriteFloatMultiplier(float value, System.IO.BinaryWriter w)
        {
           byte byteVal = (byte)(value * FloatMultiplier);
            w.Write(byteVal);
            return byteVal;
        }

        public static float ReadFloatMultiplier(System.IO.BinaryReader r)
        {
            return r.ReadByte() / FloatMultiplier;
        }

        const int GrowingAddValueMaxByte = byte.MaxValue - 1;
        /// <summary>
        /// Best for values that mostly are between 0-200 and rarely larger than 600
        /// </summary>
        public static void WriteGrowing_Byte_Add(System.IO.BinaryWriter w, int value)
        {//REMARK, can't write negative values
            while (value > GrowingAddValueMaxByte)
            {
                w.Write(byte.MaxValue);
                value -= GrowingAddValueMaxByte;
            }
            w.Write((byte)value);
        }
        public static int ReadGrowing_Byte_Add(System.IO.BinaryReader r)
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
        public static void WriteGrowing_Byte_Bit(System.IO.BinaryWriter w, int value)
        {//REMARK, can't write negative values
            while (value > GrowingBitShiftValueMaxByte)
            {
                w.Write((byte)((value & GrowingBitShiftValueMaxByte) + EightBit));
                value = value >> GrowingBitShift;
            }
            w.Write((byte)(value));
        }
        public static int ReadGrowing_Byte_Bit(System.IO.BinaryReader r)
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

            WriteGrowing_Byte_Bit(w, value);
            WriteGrowing_Byte_Add(w, value);

            System.IO.BinaryReader r = new System.IO.BinaryReader(s);
            r.BaseStream.Position = 0;
            int shiftRes = ReadGrowing_Byte_Bit(r);
            int addRes = ReadGrowing_Byte_Add(r);

            if (value != shiftRes || addRes != value)
            {
                throw new Exception();
            }

            //test 512
            s = new System.IO.MemoryStream();
            w = new System.IO.BinaryWriter(s);
            WriteGrowing_Byte_Bit(w, 512);
            byte[] test = new byte[(int)(w.BaseStream.Length)];
            w.BaseStream.Position = 0;
            w.BaseStream.Read(test, 0, test.Length);
        }

        const int GrowingAddValueMaxUShort = ushort.MaxValue - 1; // 65534

        /// <summary>
        /// Best for values that mostly are between 0-65534 and rarely larger than 130000
        /// </summary>
        public static void WriteGrowing_UShort_Add(System.IO.BinaryWriter w, int value)
        {
            // REMARK, can't write negative values
            while (value > GrowingAddValueMaxUShort)
            {
                w.Write(ushort.MaxValue);
                value -= GrowingAddValueMaxUShort;
            }
            w.Write((ushort)value);
        }

        public static int ReadGrowing_UShort_Add(System.IO.BinaryReader r)
        {
            int result = 0;
            while (true)
            {
                ushort value = r.ReadUInt16();
                if (value == ushort.MaxValue)
                {
                    result += GrowingAddValueMaxUShort;
                }
                else
                {
                    result += value;
                    return result;
                }
            }
        }


        const int GrowingBitShift_UShort = 15;
        const int GrowingBitShiftValueMaxUShort = 32767; // 0x7FFF (15 bits of data)
        const int SixteenthBit = 32768;                  // 0x8000 (Continuation flag)

        /// <summary>
        /// Best for values that mostly are between 0-32767 but frequently extend beyond 65000
        /// </summary>
        public static void WriteGrowing_UShort_Bit(System.IO.BinaryWriter w, int value)
        {
            // REMARK, can't write negative values
            while (value > GrowingBitShiftValueMaxUShort)
            {
                w.Write((ushort)((value & GrowingBitShiftValueMaxUShort) + SixteenthBit));
                value = value >> GrowingBitShift_UShort;
            }
            w.Write((ushort)(value));
        }

        public static int ReadGrowing_UShort_Bit(System.IO.BinaryReader r)
        {
            int result = 0;
            int numShifts = 0;
            while (true)
            {
                ushort value = r.ReadUInt16();

                // If the 16th bit is set, it means we have more ushorts to read
                if (value > GrowingBitShiftValueMaxUShort)
                {
                    result += (value & GrowingBitShiftValueMaxUShort) << numShifts;
                }
                else
                {
                    return result + (value << numShifts);
                }
                numShifts += GrowingBitShift_UShort;
            }
        }

        /// <summary>
        /// anything below zero is considered empty
        /// </summary>
        public static void WriteGrowing_UShort_Bit_MayEmpty(System.IO.BinaryWriter w, int value)
        {
            if (value < 0)
            {
                value = -1;
            }
            WriteGrowing_UShort_Bit(w, value +1);
        }

        public static int ReadGrowing_UShort_Bit_MayEmpty(System.IO.BinaryReader r)
        {
            return ReadGrowing_UShort_Bit(r) -1;
        }

        public static void TestGrowingWriter_UShort(int value)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);

            WriteGrowing_UShort_Bit(w, value);
            WriteGrowing_UShort_Add(w, value);

            System.IO.BinaryReader r = new System.IO.BinaryReader(s);
            r.BaseStream.Position = 0;

            int shiftRes = ReadGrowing_UShort_Bit(r);
            int addRes = ReadGrowing_UShort_Add(r);

            if (value != shiftRes || addRes != value)
            {
                throw new System.Exception("Deserialized value did not match the input!");
            }

            // test 70000 (A value large enough to trigger growth in a ushort implementation)
            s = new System.IO.MemoryStream();
            w = new System.IO.BinaryWriter(s);
            WriteGrowing_UShort_Bit(w, 70000);

            byte[] test = new byte[(int)(w.BaseStream.Length)];
            s.Position = 0;
            s.Read(test, 0, test.Length);
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

