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

        public BiomType biom1, biom2;
        public float secondBiomWeight;

    }

    struct BiomeWeight
    {
        public BiomType type;
        public float weight;
    }
}
