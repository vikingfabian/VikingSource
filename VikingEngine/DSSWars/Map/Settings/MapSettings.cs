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

        public Height[] heigts;
        public WorldBioms bioms;

        public MapSettings()
        {
            DssRef.map = this;

            bioms = new WorldBioms();
            heigts = new Height[Height.MaxHeight + 1];

            for (int height = 0; height <= Height.MaxHeight; ++height)
            {
                heigts[height] = new Height(height);
            }            
        }


    }
}
