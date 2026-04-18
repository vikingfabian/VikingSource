using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct TileGenerateData
    {
        public BiomType biom = BiomType.Green;
        public float secondaryBiomStrength = 0;
        public BiomType secondaryBiom = BiomType.Green;
        public int heightLevel;

        public TileGenerateData()
        { }
    }
}
