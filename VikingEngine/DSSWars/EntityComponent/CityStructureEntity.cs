using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Build;

namespace VikingEngine.Core.DSSWars.EntityComponent
{
    struct CityBuildingStructureData
    {
        public static readonly int COUNT = (int)BuildStructureType.NUM_NONE;
        public int count;
        public IntVector2 position;

        public bool HasOne()
        {
            return count > 0;
        }

    }

    enum BuildStructureType
    {       
        Logistics,
        Economics,
        ManorLord,
        GreatHall,
        TingsHouse,
        
        WorkerHut,
        ServiceHouse,

        SoldiersQuarter,
        GuardHouse,

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
        LinenFarm,
        HempFarm,
        RapeSeedFarm,

       
        PavementStreet,

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

        //KnightsBarracks,

        Smelter,
        Foundry,
        Armory,
        Chemist,
        Gunmaker,
        School,


        Postal,
        Recruitment,

        GoldDelivery,

        DirtRoad,
        GravelRoad,
        CobblestoneRoad,
        PavedRoad,

        Wall,

        Palisade,
        ImmigrationTent,
        ResearchCenter,
        BookPress,

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
       
        NUM_NONE,

    }
}
