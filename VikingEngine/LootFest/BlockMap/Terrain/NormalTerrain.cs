using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    class NormalTerrain : AbsLevelTerrain
    {
        public NormalTerrain(AbsLevel level)
            : base(level)
        {
            wallMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.grass_mossy, Data.MaterialType.dirt_brown, 6, Data.MaterialType.mountain_red);
            groundMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.grass_green, Data.MaterialType.dirt_brown, 4, Data.MaterialType.mountain_blue);
            roadMaterials = HeightMapMaterials.Test; //new HeightMapMaterials(Data.MaterialType.dirt_sand, Data.MaterialType.dirt_brown, 4, Data.MaterialType.mountain_blue);

            backgroundScenery = new Map.BackgroundSceneryData(false);
        }
    }
}
