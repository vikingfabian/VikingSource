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
        //Terrain,
        Building,
        NUM
    }

    enum TerrainSubFoilType
    {
        TreeHardSprout,
        TreeSoftSprout,
        TreeHard,
        TreeSoft,
        Bush,
        Herbs,
        TallGrass,
        Stones,
        StoneBlock,
        WheatFarm,
        LinnenFarm,
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
        Work_Cook,
        Work_Smith,
        PigPen,
        HenPen,
        Tavern,
        Barracks,
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
