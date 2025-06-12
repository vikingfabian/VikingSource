using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map.HDvoxel
{
    enum MaterialProperty
    {
        Empty = BlockHD.EmptyBlockMaterial,
        Default,
        AntiBlock,

        Layer_AboveAll,
        Layer_Above1,
        Layer_Below1,
        Layer_BelowAll,

        //JointUp,
        //JointForward,

        Terrain,
        TerrainWontBurt,
        Building,
        BuildingWontBurt,
        //Damage,
        //Living,

        //SoftFoliage,
        //Wood, //Slå lätt sönder, trä ljud
        //Glass,
        //SolidWood,
        //SolidStone,

        BlockPattern = BlockHD.BlockPatternMaterial,//15
    }
}
