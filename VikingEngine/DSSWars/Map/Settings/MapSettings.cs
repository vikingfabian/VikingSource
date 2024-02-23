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
        //static readonly Color LowWaterCol1 = new Color(99, 125, 253);
        //static readonly Color LowWaterCol2 = ColorExt.Multiply(LowWaterCol1, 1.1f);
        //static readonly Color SeaBottomCol = new Color(253, 198, 137);

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

            //    TypeToHeight_aboveWater = new float[TypeToHeight.Length];
            //for (int i = 0; i < TypeToHeight.Length; i++)
            //{
            //    TypeToHeight_aboveWater[i] = Math.Max(TypeToHeight[i], 0);
            //}

            //TerrainTypes = new Height[Height.BiomCount, MaxHeight + 1];
            //for (int biom = 0; biom < Height.BiomCount; ++biom)
            //{
            //    for (int height = 0; height <= MaxHeight; ++height)
            //    {
            //        TerrainTypes[biom, height] = new Height(biom, height);
            //    }
            //}

            
        }


    }
}
