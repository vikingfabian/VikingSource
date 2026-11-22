using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Build
{
    enum BuildAndExpandType : byte
    {
        ExpansionHall_Lvl1,
        ExpansionHall_Lvl2,
        ExpansionHall_Lvl3,
        Logistics,
        Economics,
        ManorLord,
        GreatHall,
        TingsHouse,

        WorkerHut,
        WorkerHutLarge,

        ServiceHouse_Small,
        ServiceHouse_Large,


        SoldiersQuarter_Small,
        SoldiersQuarter_Large,
        GuardHouse_Small,
        GuardHouse_Large,


        Noblehouse,
        Tavern,
        Storehouse,
        Brewery,
        Cook,
        CoalPit,
        WorkBench,
        Smith,
        Carpenter,

        OrchardApple,
        OrchidBanana,
        WheatFarm,
        WheatFarmUpgraded,
        LinenFarm,
        LinenFarmUpgraded,
        HempFarm,
        HempFarmUpgraded,
        RapeSeedFarm,
        RapeSeedFarmUpgraded,

        Statue_ThePlayer,
        PavementStreet,
        PavementFlower,
        PavementLamp,
        PavementFountain,
        PavementRectFlower,

        Bank,
        CoinMinter,

        WoodCutter,
        StoneCutter,
        Embassy,

        WaterWell,
        WaterResovoir,

        SoldierBarracks,
        ArcherBarracks,
        WarmachineBarracks,
        GunBarracks,
        CannonBarracks,
        AnimalTrainer,
        //KnightsBarracks,

        Smelter,
        Foundry,
        Armory,
        Chemist,
        Gunmaker,
        School,


        Postal,
        PostalLevel2,
        PostalLevel3,
        Recruitment,
        RecruitmentLevel2,
        RecruitmentLevel3,

        GoldDeliveryLvl1,
        GoldDeliveryLvl2,
        GoldDeliveryLvl3,

        DirtRoad,
        GravelRoad,
        CobblestoneRoad,
        PavedRoad,

        Stakes,
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

        GardenGrass,
        GardenFourBushes,
        GardenLongTree,
        GardenWalledBush,

        CitySquare,
        //CobbleStones,
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

        Palisade,
        ImmigrationTent,
        ResearchCenter,
        BookPress,

        TreeSeedlingSoft,
        TreeSeedlingHard,

        TreeSoft,
        TreeHard,

        DigSite_Lvl1,
        //MineShaft

        StonesMine,
        CoalMine,
        StoneBlockMine,
        IronOreMine,
        TinOreMine,
        CopperOreMine,
        SilverOreMine,
        GoldOreMine,
        LeadOreMine,
        MithrilMine,
        SulfurMine,
        WorkerTent,
        SaltMine,

        Pottery,
        DryingPan,
        Butcher,
        Smoker,
        Dryer,

        MaterialStorage, FoodStorage, WeaponStorage, ArmorStorage, AnimalStorage,
        Cesspit,

        ShieldMaker,

        BoarPen,
        FowlPen,
        PigPen,
        HenPen,
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


        TrapperHut,
        DiplomaticStatue_ThumbsUpWest,
        DiplomaticStatue_ThumbsUpEast,
        DiplomaticStatue_InsultWest,
        DiplomaticStatue_InsultEast,
        DiplomaticStatue_GoldenPoop,


        NUM_NONE,
        ALL,
        TERRAFORM,
        DEMOLISH,

        LogisticsLevel1,
        LogisticsLevel2,

        GovernentBuildings,

        

    }

    struct BuildInAreaOptions
    {
        public static readonly BuildInAreaOptions WithInOnly = new BuildInAreaOptions() { WithInBorders = true };
        public static readonly BuildInAreaOptions OutsideOnly = new BuildInAreaOptions() { OutsideBorders = true };
        public static readonly BuildInAreaOptions Anywhere = new BuildInAreaOptions() { WithInBorders = true, OutsideBorders = true };

        public bool WithInBorders, OutsideBorders;
    }
    //enum BuildInAreaOptions
    //{
    //    WithInBorders,
    //    OutsideBorders,
    //    Both,
    //}

}
