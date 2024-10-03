using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map
{
    enum TerrainMainType
    {
        DefaultLand,
        DefaultSea,
        Destroyed,

        Foil,
        Mine,
        Resourses,
        Building,
        Decor,
        NUM
    }

    enum TerrainSubFoilType
    {
        TreeHardSprout,
        TreeSoftSprout,
        TreeHard,
        TreeSoft,
        DryWood,
        Bush,
        Herbs,
        TallGrass,
        Stones,
        StoneBlock,
        WheatFarm,
        LinenFarm,
        NUM_NONE
    }

    enum TerrainResourcesType
    {
        Wood,
        Storage,
        NUM_NONE
    }

    enum TerrainBuildingType
    {
        DirtWall,
        DirtTower,
        WoodWall,
        WoodTower,
        StoneWall,
        StoneTower,
        StoneHall,
        SmallHouse,
        BigHouse,
        Square,
        CobbleStones,
        WorkerHut,
        Brewery,
        Work_Cook,
        Work_Smith,
        PigPen,
        HenPen,
        Tavern,
        Postal,
        Recruitment,
        Barracks,
        Carpenter,
        Nobelhouse,
        
        NUM_NONE
    }

    enum TerrainDecorType
    {
        Statue_ThePlayer,
        NUM_NONE
    }

    enum TerrainMineType
    {
        Stones,
        StoneBlock,
        IronOre,
        TinOre,
        CupperOre,
        SilverOre,
        GoldOre,
        MithrilOre,
    }
}
