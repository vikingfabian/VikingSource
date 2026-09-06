using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct GenTile
    {
        public Color color;
        public float groundY;

        byte heightValue;

        public BiomType biom1, biom2;
        public float secondBiomWeight;

        public void writeIcon(System.IO.BinaryWriter w)
        {
            heightValue = Map2Generator.Height_Interval.GetValueBytePercentPos_WithBound(groundY);

            w.Write(heightValue);
            w.Write((byte)biom1);
        }

        public void readIcon(System.IO.BinaryReader r)
        {
            heightValue = r.ReadByte();
            biom1 = (BiomType)r.ReadByte();

            groundY = Map2Generator.Height_Interval.GetFromBytePercent(heightValue);
        }
    }

    struct BiomeWeight
    {
        public BiomType type;
        public float weight;
    }
}
