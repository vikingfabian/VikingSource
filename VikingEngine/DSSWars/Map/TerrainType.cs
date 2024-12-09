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
        Wall,
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
        BogIron,
        HempFarm,
        RapeSeedFarm,
        NUM_NONE
    }

    /// <summary>
    /// Creates a stockpile of resources
    /// </summary>
    enum TerrainResourcesType
    {
        Wood,
        Rubble,
        Storage,
        NUM_NONE
    }

    enum TerrainBuildingType
    {
        _RESERVE1,//DirtWall,
        _RESERVE2,//DirtTower,
        _RESERVE3,//WoodWall,
        _RESERVE4,//WoodTower,
        _RESERVE5,//StoneWall,
        _RESERVE6,//StoneTower,
        StoneHall,
        SmallHouse,
        BigHouse,
        Square,
        CobbleStones,
        WorkerHut,
        Brewery,
        Work_Cook,
        Work_Bench,
        Work_CoalPit,
        Work_Smith,
        PigPen,
        HenPen,
        Tavern,
        Postal,
        Recruitment,
        SoldierBarracks,
        Carpenter,
        Nobelhouse,
        Storehouse,
        Bank,
        CoinMinter,

        Logistics,
        Smelter,
        WoodCutter,
        StoneCutter,
        Embassy,
        WaterResovoir,
        ArcherBarracks,
        WarmashineBarracks,
        GunBarracks,
        CannonBarracks,
        KnightsBarracks,
        Foundry,
        Armory,
        Chemist,
        Gunmaker,
        School,

        NUM_NONE
    }

    enum TerrainWallType
    {
        DirtWall,
        DirtTower,
        WoodWall,
        WoodTower,
        StoneWall,
        StoneTower,

        NUM_NONE
    }

    enum TerrainDecorType
    {
        Pavement,
        PavementFlower,
        Statue_ThePlayer,
        NUM_NONE
    }

    enum TerrainMineType
    {
        Stones,
        Coal,
        StoneBlock,
        IronOre,
        TinOre,
        CupperOre,
        SilverOre,
        GoldOre,
        LeadOre,
        Mithril,
        Sulfur,
    }

    enum TerrainSeaType
    {       
        Deep,
        Low,
    }

    enum TerrainDefaultLandType
    {
        Flat,
        Mountain,
    }
}
