using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    class CaveTerrain : AbsLevelTerrain
    {
        public CaveTerrain(AbsLevel level)
            : base(level)
        {
            wallMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.black, Data.MaterialType.gray_70, 6, Data.MaterialType.dark_cool_brown);
            groundMaterials = HeightMapMaterials.Test; // new HeightMapMaterials(Data.MaterialType.dirt_black, Data.MaterialType.stones_black, 4, Data.MaterialType.mountain_black);
            roadMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.gray_60, Data.MaterialType.stones_black, 4, Data.MaterialType.mountain_black);

            backgroundScenery = new Map.BackgroundSceneryData(true);
        }
    }
}
