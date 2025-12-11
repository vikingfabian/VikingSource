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
        Road,
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
        WheatFarmUpgraded,
        LinenFarm,
        LinenFarmUpgraded,
        HempFarm,
        HempFarmUpgraded,
        RapeSeedFarm,
        RapeSeedFarmUpgraded,

        BogIron,
        ClayPit,
       
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
        CityHall_Village,
        CityHall_Town,
        CityHall_Capital,

        WorkerHut,
        WorkerHutLarge,

        ServiceMenHouse_small,
        ServiceMenHouse_Large,

        GuardHouse_Small,
        GuardHouse_Large,

        Brewery,
        Work_Cook,
        Work_Bench,
        Work_CoalPit,
        Work_Smith,
        PigPen,
        HenPen,
        Tavern,
        
        Postal,
        PostalLevel2,
        PostalLevel3,

        Recruitment,
        RecruitmentLevel2,
        RecruitmentLevel3,

        GoldDeliveryLevel1,
        GoldDeliveryLevel2,
        GoldDeliveryLevel3,

        SoldierBarracks,
        ArcherBarracks,
        WarmachineBarracks,
        GunBarracks,
        CannonBarracks,
        KnightsBarracks,

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
        
        Foundry,
        Armory,
        Chemist,
        Gunmaker,
        School,
        ResearchCenter,
        BookPress,
        ImmigrationTent,

        WorkerTent,
        CityHall_Unclaimed,
        CityHall_Tent,

        //NEW
        Pottery,
        DryingPan,
        Butcher,
        Smoker,
        Dryer,

        MaterialStorage, FoodStorage, WeaponStorage, ArmorStorage, AnimalStorage,

        OxenPen,
        KineOxenPen,

        DogCage,
        HoundCage,

        PonyPen,
        HorsePen,
        WarHorsePen,
        DraftHorsePen,
        WildPigPen,
        WildHogPen,
        WarHogPen,
        StagHogPen,
        WolfCage,
        WargCage,
        AlphaWargCage,
        WildCatCage,
        LionCage,
        WarLionCage,
        ElephantCage,
        WarElephantCage,
        OliphantCage,

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
        StoneWallGreen,
        StoneWallBlueRoof,
        StoneWallWoodHouse,
        StoneGate,
        StoneHouse,
        Palisade,
        NUM_NONE
    }

    enum TerrainDecorType
    {
        Square,
        CobbleStones,

        Pavement,
        PavementFlower,
        Statue_ThePlayer,
        PavementLamp,
        PavemenFountain,
        PavementRectFlower,
        GardenFourBushes,
        GardenLongTree,
        GardenWalledBush,
        GardenGrass,
        GardenBird,
        GardenMemoryStone,
        Statue_Leader,
        Statue_Lion,
        Statue_Horse,
        Statue_Pillar,

        FlagPole_LongBanner,
        FlagPole_Banner,
        FlagPole_SlimBanner,

        FlagPole_Flag,
        FlagPole_FlagRound,
        FlagPole_FlagLarge,
        FlagPole_Streamer,
        FlagPole_Triangle,
        NUM_NONE
    }

    enum TerrainRoadType
    {
        DirtRoad,
        NUM_NONE
    }

    enum TerrainMineType
    {
        Salt,
        Coal,
        StoneBlock,
        IronOre,
        TinOre,
        CopperOre,
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
