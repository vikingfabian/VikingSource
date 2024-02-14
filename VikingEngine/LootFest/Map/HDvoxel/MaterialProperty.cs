using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map.HDvoxel
{
    enum MaterialProperty
    {
        Empty = BlockHD.EmptyBlockMaterial,
        Unknown,
        AntiBlock,
        Damage,
        Living,
        
        SoftFoliage,
        Wood, //Slå lätt sönder, trä ljud
        Glass,
        SolidWood,
        SolidStone,

        BlockPattern = BlockHD.BlockPatternMaterial,
    }
}
