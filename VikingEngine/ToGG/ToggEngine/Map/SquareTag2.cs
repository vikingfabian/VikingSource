using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DataStream;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    struct SquareTag2
    {
        public static readonly SquareTag2 None = new Map.SquareTag2();

        public SquareTagType tagType;
        public byte tagId;

        public bool boardText(out string text, out Color color)
        {
            text = null;
            color = Color.Black;

            switch (tagType)
            {
                case SquareTagType.None:
                    return false;

                case SquareTagType.HeroStart:
                    text = "Enter" + tagId.ToString();
                    return true;

                case SquareTagType.Tag:
                    text = "Tag" + tagId.ToString();
                    return true;

                case SquareTagType.RoomDivider:
                    text = "Room div";
                    return true;

                case SquareTagType.MapEnter:
                    text = "Map enter" + tagId.ToString();
                    return true;

                default:
                    text = TextLib.Error;
                    color = Color.Red;
                    return true;
            }
        }

        public override bool Equals(object obj)
        {
            var other = (SquareTag2)obj;
            return other.tagType == this.tagType &&
                other.tagId == this.tagId;
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)tagType);
            w.Write((byte)tagId);
        }

        public void Read(System.IO.BinaryReader r, FileVersion version)
        {
            tagType = (SquareTagType)r.ReadByte();
            tagId = r.ReadByte();
        }

        public override string ToString()
        {
            return "Square tag (" + tagType.ToString() + tagId.ToString() + ")";
        }

        public bool HasValue => tagType != SquareTagType.None;
    }

    enum SquareTagType
    {
        None,
        Tag,
        HeroStart,
        Exit,
        EnemySpawn,
        RoomDivider,
        MapEnter,
    }
}
