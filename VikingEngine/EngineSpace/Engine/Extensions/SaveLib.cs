using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine
{
    static class SaveLib
    {
        public const string BackUpName = "_bak";

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
            if (value == null)
            {
                w.Write((byte)0);
                return;
            }
            w.Write((byte)value.Length);
            w.Write(value.ToCharArray());
        }
        public static String ReadString(System.IO.BinaryReader r)
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

        public static void WriteFloatMultiplier(float value, System.IO.BinaryWriter w)
        {            
            w.Write((byte) (value * FloatMultiplier));
        }

        public static float ReadFloatMultiplier(System.IO.BinaryReader r)
        {
            return r.ReadByte() / FloatMultiplier;
        }
    }
}
