using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class FieldSquare
    {
        public FieldTerrainType terrain = FieldTerrainType.Open;
        public FieldSquareType type = FieldSquareType.None;
        public int typeIndex = 0;

        public Graphics.TextG editorText = null;
        public const float DefaultFriction = 0.985f;
        const float SandFriction = DefaultFriction * 0.8f;

        public string SquareText()
        {
            switch (type)
            {
                default: return "";

                case FieldSquareType.Area:
                    return "A" + typeIndex.ToString();

                case FieldSquareType.ChokeStart:
                    return "Ck_A" + typeIndex.ToString();

                case FieldSquareType.ChokeEnd:
                    return "Ck_B" + typeIndex.ToString();

                case FieldSquareType.LaunchCannon:
                    return "Canon";
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)type);
            w.Write((byte)typeIndex);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            type = (FieldSquareType)r.ReadByte();
            typeIndex = r.ReadByte();
        }

        public void Clear()
        {
            type = FieldSquareType.None;
            terrain = FieldTerrainType.Open;
        }

        public float friction()
        {
            if (terrain == FieldTerrainType.Sand)
            {
                return SandFriction;
            }
            else
            {
                return DefaultFriction;
            }
        }

        public override string ToString()
        {
            return type.ToString() + typeIndex.ToString();
        }
    }

    enum FieldSquareType
    {
        None,
        Area,
        ChokeStart,
        ChokeEnd,
        LaunchCannon,
    }

    enum FieldTerrainType
    {
        Open,
        Sand,
        Spikes,
    }

}
