using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap
{
    struct BlockMapSquare
    {
        public static readonly BlockMapSquare Empty = new BlockMapSquare();

        public MapBlockType type;
        public MapBlockSpecialType special;
        public byte specialIndex;
        public byte specialDir;
        public byte lockId;

        public byte terrain;

        public BlockMapSquare(BinaryReader r, byte version)
            : this()
        {
            read(r, version);
        }

        public Color typeColor()
        {
            switch (type)
            {
                case MapBlockType.Occupied:
                    return Color.DarkGray;
                case MapBlockType.OverrideToOccupied:
                    return ColorExt.VeryDarkGray;
                case MapBlockType.Wall:
                    return Color.OrangeRed;
                case MapBlockType.Water:
                    return Color.LightBlue;

                case MapBlockType.Open:
                    return Color.Green;
                case MapBlockType.Road:
                    return Color.SaddleBrown;
                default:
                    return ColorExt.Error;
            }
        }
        public Color debugColor()
        {
            if (special == MapBlockSpecialType.None)
            {
                return typeColor();
            }
            else
            {
                return Color.Yellow;
            }
        }

        public void write(BinaryWriter w)
        {
            w.Write((byte)type);
            w.Write((byte)special);
            w.Write(specialIndex);
            w.Write(specialDir);
        }
        void read(BinaryReader r, byte version)
        {
            type = (MapBlockType)r.ReadByte();
            special = (MapBlockSpecialType)r.ReadByte();
            if (version >= 1)
            {
                specialIndex = r.ReadByte();
                specialDir = r.ReadByte();
            }
        }

        public Dir4 Dir4
        {
            get { return (VikingEngine.Dir4)specialDir; }
            set { specialDir = (byte)value; }
        }

        public override string ToString()
        {
            string result = type.ToString();
            if (special != MapBlockSpecialType.None)
            {
                result += ", special(" + special.ToString() + ", Ix" + specialIndex.ToString() + ", Dir" + specialDir.ToString() + ")";
            }
            return result;
        }

        public bool isEmpty()
        {
            return type == Empty.type && special == Empty.special;
        }
    }
}
