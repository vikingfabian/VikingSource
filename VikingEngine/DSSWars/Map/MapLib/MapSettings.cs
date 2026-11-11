using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Settings
{
    class MapSettings
    {
        public static readonly Color DeepWaterCol1 = new Color(71, 95, 214);
        public static readonly Color DeepWaterCol2 = ColorExt.Multiply(DeepWaterCol1, 1.1f);

        public BiomHeightColor[] heigts;
        public MapLib.WorldBioms bioms;

        public MapSettings()
        {
            DssRef.map = this;

            bioms = new MapLib.WorldBioms();
            heigts = new BiomHeightColor[BiomHeightColor.MaxHeight + 1];

            for (int height = 0; height <= BiomHeightColor.MaxHeight; ++height)
            {
                heigts[height] = new BiomHeightColor(height);
            }            
        }


    }
}
