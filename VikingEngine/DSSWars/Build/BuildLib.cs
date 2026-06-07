using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.PJ.GameState;
using VikingEngine.ToGG;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;
using VikingEngine.ToGG.HeroQuest.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Build
{
    enum BuildAndExpandType
    {
        WorkerHut,
        WorkerHutLarge,

        ServiceHouse_Small,
        ServiceHouse_Large,

        GuardHouse_Small,
        GuardHouse_Large,

        Postal,
        Recruitment,
        SoldierBarracks,
        Noblehouse,
        Tavern,
        Storehouse,
        Brewery,
        Cook,
        CoalPit,
        WorkBench,
        Smith,
        Carpenter,
        WheatFarm,
        LinenFarm,
        HempFarm,
        RapeSeedFarm,
        PigPen,
        HenPen,
        Statue_ThePlayer,
        Pavement,
        PavementFlower,

        Logistics,
        Bank,
        CoinMinter,

        WoodCutter,
        StoneCutter,
        Embassy,
        WaterResovoir,
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

        WheatFarmUpgraded,
        LinenFarmUpgraded,
        HempFarmUpgraded,
        RapeSeedFarmUpgraded,
        PostalLevel2,
        PostalLevel3,
        RecruitmentLevel2,
        RecruitmentLevel3,

        GoldDeliveryLvl1,
        GoldDeliveryLvl2,
        GoldDeliveryLvl3,

        DirtRoad,

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
        PavementLamp,
        PavemenFountain,
        PavementRectFlower,
        GardenGrass,
        GardenFourBushes,
        GardenLongTree,
        GardenWalledBush,
        
        CitySquare,
        CobbleStones,
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

        ManorLord,
        GreatHall,

        OrchardApple,
        OrchidBanana,

        //New
        SaltMine,

        Pottery,
        DryingPan,
        Butcher,
        Smoker,
        Dryer,

        MaterialStorage, FoodStorage,  WeaponStorage, ArmorStorage, AnimalStorage,
        Cesspit,

        ShieldMaker,

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

        BoarPen,
        FowlPen,
        TrapperHut,
        DiplomaticStatue_ThumbsUpWest,
        DiplomaticStatue_ThumbsUpEast,
        DiplomaticStatue_InsultWest,
        DiplomaticStatue_InsultEast,
        DiplomaticStatue_GoldenPoop,


        NUM_NONE,
        ALL,
        DEMOLISH,

        LogisticsLevel1,
        LogisticsLevel2,

    }
    static class BuildLib
    {
        public static List<BuildAndExpandType> LogisticsUnlockBuildings = new List<BuildAndExpandType>
        {
            BuildAndExpandType.CoalPit,
            //BuildAndExpandType.Brewery,

            BuildAndExpandType.ImmigrationTent,
            BuildAndExpandType.Recruitment,
            BuildAndExpandType.Storehouse,
            BuildAndExpandType.Tavern,

            BuildAndExpandType.WoodWall,

            BuildAndExpandType.School,
            BuildAndExpandType.ResearchCenter,
        };

        public static List<BuildAndExpandType> LogisticsUnlockBuildings_Level2 = new List<BuildAndExpandType>
        {
            BuildAndExpandType.GardenGrass,
            BuildAndExpandType.PavemenFountain,
            BuildAndExpandType.Statue_Leader,
        };

        public static List<BuildAndExpandType> ManorUnlockBuildings = new List<BuildAndExpandType>
        {
            BuildAndExpandType.WheatFarm,
            BuildAndExpandType.HenPen,

            BuildAndExpandType.Cook,
        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static void AvailableBuildTypes(List<BuildAndExpandType> list, City city, bool autoBuild)
        {
            bool godPowers = (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator || (StartupSettings.UnlockAllProgress && city.GetPlayer().IsLocalPlayer())) && !autoBuild;

            bool devUnlockAll = StartupSettings.UnlockAllProgress;

            var unlocks = city.technology.GetUnlocks(false);

            bool logistics1 = city.buildingStructure.buildingLevel_logistics >= 1 ||
                godPowers;
            bool logistics2 = city.buildingStructure.buildingLevel_logistics >= 2 ||
                godPowers;

            bool manor = city.buildingStructure.manorLord || godPowers;

            bool campSite = city.cityType == CityType.Campsite;

            if (!campSite && city.buildingStructure.buildingLevel_logistics == 0)
            {
                list.Add(BuildAndExpandType.Logistics);
            }

            if (logistics1)
            {
                if (!manor)
                {
                    list.Add(BuildAndExpandType.ManorLord);
                }
                if (!city.buildingStructure.greatHall)
                {
                    list.Add(BuildAndExpandType.GreatHall);
                }
                list.Add(BuildAndExpandType.School);

                list.Add(BuildAndExpandType.ResearchCenter);
                list.Add(BuildAndExpandType.BookPress);
            }

            if (campSite)
            {
                list.Add(BuildAndExpandType.WorkerTent);
            }
            else
            {
                if (godPowers)
                {
                    list.Add(BuildAndExpandType.WorkerTent);
                }
                list.Add(BuildAndExpandType.WorkerHut);
                if (logistics1)
                {
                    list.Add(BuildAndExpandType.WorkerHutLarge);
                }
            }

            list.Add(BuildAndExpandType.ServiceHouse_Small);
            if (logistics1)
            {
                list.Add(BuildAndExpandType.ServiceHouse_Large);
            }

            list.Add(BuildAndExpandType.GuardHouse_Small);
            if (logistics1)
            {
                list.Add(BuildAndExpandType.GuardHouse_Large);

                list.Add(BuildAndExpandType.ImmigrationTent);

            }

            list.Add(BuildAndExpandType.OrchardApple);

            if (manor)
            {
                list.Add(BuildAndExpandType.WheatFarm);
                if (unlocks.building_upgradedFarm || godPowers)
                {
                    list.Add(BuildAndExpandType.WheatFarmUpgraded);
                }
            }

            list.Add(BuildAndExpandType.LinenFarm);
            if (unlocks.building_upgradedFarm)
            {
                list.Add(BuildAndExpandType.LinenFarmUpgraded);
            }

            list.Add(BuildAndExpandType.RapeSeedFarm);
            if (unlocks.building_upgradedFarm)
            {
                list.Add(BuildAndExpandType.RapeSeedFarmUpgraded);
            }

            if (manor)
            {
                if (unlocks.building_mixedFarms)
                {
                    list.Add(BuildAndExpandType.HempFarm);
                    if (unlocks.building_upgradedFarm)
                    {
                        list.Add(BuildAndExpandType.HempFarmUpgraded);
                    }                   
                }                
            }

            if (logistics1)
            {
                list.Add(BuildAndExpandType.TrapperHut);

                addAnimalPen(BuildAndExpandType.FowlPen, CityResoureIndex.Fowl, city.buildingStructure.FowlPen_count);
                addAnimalPen(BuildAndExpandType.HenPen, CityResoureIndex.Hen, city.buildingStructure.HenPen_count);

                addAnimalPen(BuildAndExpandType.BoarPen, CityResoureIndex.Boar, city.buildingStructure.BoarPen_count);
                addAnimalPen(BuildAndExpandType.PigPen, CityResoureIndex.Pig, city.buildingStructure.PigPen_count);

                // --- Wild Pigs / Hogs ---
                if (biomRequirement(CityBiome.Mountain))
                {
                    addAnimalPen(BuildAndExpandType.WildPigPen, CityResoureIndex.WildPig, city.buildingStructure.WildPigPen_count);
                    addAnimalPen(BuildAndExpandType.WildHogPen, CityResoureIndex.WildHog, city.buildingStructure.WildHogPen_count);
                    addAnimalPen(BuildAndExpandType.WarHogPen, CityResoureIndex.WarHog, city.buildingStructure.WarHogPen_count);
                    addAnimalPen(BuildAndExpandType.StagHogPen, CityResoureIndex.StagHog, city.buildingStructure.StagHogPen_count);
                }

                // --- Oxen ---
                addAnimalPen(BuildAndExpandType.OxenPen, CityResoureIndex.Oxen, city.buildingStructure.OxenPen_count);
                addAnimalPen(BuildAndExpandType.KineOxenPen, CityResoureIndex.KineOxen, city.buildingStructure.KineOxenPen_count);

                // --- Dogs ---
                addAnimalPen(BuildAndExpandType.DogCage, CityResoureIndex.Dog, city.buildingStructure.DogCage_count);
                addAnimalPen(BuildAndExpandType.HoundCage, CityResoureIndex.Hound, city.buildingStructure.HoundCage_count);

                // --- Horses ---
                addAnimalPen(BuildAndExpandType.PonyPen, CityResoureIndex.Pony, city.buildingStructure.PonyPen_count);
                addAnimalPen(BuildAndExpandType.HorsePen, CityResoureIndex.Horse, city.buildingStructure.HorsePen_count);
                addAnimalPen(BuildAndExpandType.WarHorsePen, CityResoureIndex.WarHorse, city.buildingStructure.WarHorsePen_count);
                addAnimalPen(BuildAndExpandType.DraftHorsePen, CityResoureIndex.DraftHorse, city.buildingStructure.DraftHorsePen_count);
                               

                // --- Wolves ---
                if (biomRequirement(CityBiome.Desolate))
                {
                    addAnimalPen(BuildAndExpandType.WolfCage, CityResoureIndex.Wolf, city.buildingStructure.WolfCage_count);
                    addAnimalPen(BuildAndExpandType.WargCage, CityResoureIndex.Warg, city.buildingStructure.WargCage_count);
                    addAnimalPen(BuildAndExpandType.AlphaWargCage, CityResoureIndex.AlphaWarg, city.buildingStructure.AlphaWargCage_count);
                }

                // --- Cats ---
                if (biomRequirement(CityBiome.Forest))
                {
                    addAnimalPen(BuildAndExpandType.WildCatCage, CityResoureIndex.WildCat, city.buildingStructure.WildCatCage_count);
                    addAnimalPen(BuildAndExpandType.LionCage, CityResoureIndex.Lion, city.buildingStructure.LionCage_count);
                    addAnimalPen(BuildAndExpandType.WarLionCage, CityResoureIndex.WarLion, city.buildingStructure.WarLionCage_count);
                }
                // --- Elephants ---
                if (biomRequirement(CityBiome.Desert))
                {
                    addAnimalPen(BuildAndExpandType.ElephantCage, CityResoureIndex.Elephant, city.buildingStructure.ElephantCage_count);
                    addAnimalPen(BuildAndExpandType.WarElephantCage, CityResoureIndex.WarElephant, city.buildingStructure.WarElephantCage_count);
                    addAnimalPen(BuildAndExpandType.OliphantCage, CityResoureIndex.Oliphant, city.buildingStructure.OliphantCage_count);
                }
            }


            if (city.buildingStructure.WoodCutter_count > 0 ||
                godPowers)
            {
                list.Add(BuildAndExpandType.TreeSeedlingSoft);
                list.Add(BuildAndExpandType.TreeSeedlingHard);
            }

            

            list.Add(BuildAndExpandType.Postal);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.PostalLevel2);
                list.Add(BuildAndExpandType.PostalLevel3);
                list.Add(BuildAndExpandType.Recruitment);
                list.Add(BuildAndExpandType.RecruitmentLevel2);
                list.Add(BuildAndExpandType.RecruitmentLevel3);

                if (!DssRef.storage.gameRuleset.centralGold)
                {
                    list.Add(BuildAndExpandType.GoldDeliveryLvl1);
                    list.Add(BuildAndExpandType.GoldDeliveryLvl2);
                    list.Add(BuildAndExpandType.GoldDeliveryLvl3);
                }

                list.Add(BuildAndExpandType.MaterialStorage);
                list.Add(BuildAndExpandType.FoodStorage);
                list.Add(BuildAndExpandType.WeaponStorage);
                list.Add(BuildAndExpandType.ArmorStorage);
                list.Add(BuildAndExpandType.AnimalStorage);
                list.Add(BuildAndExpandType.Cesspit);

                list.Add(BuildAndExpandType.Storehouse);
                list.Add(BuildAndExpandType.Tavern);
                if (manor)
                {
                    list.Add(BuildAndExpandType.Brewery);
                }
                list.Add(BuildAndExpandType.WaterResovoir);

                list.Add(BuildAndExpandType.CoalPit);
            }
            //else
            //{
            //    list.Add(BuildAndExpandType.FoodStorage);
            //}

            list.Add(BuildAndExpandType.WorkBench);
            if (manor)
            {
                list.Add(BuildAndExpandType.Cook);
            }

            if (logistics1)
            {
                list.Add(BuildAndExpandType.Butcher);
                list.Add(BuildAndExpandType.Smoker);

                if (biomRequirement(CityBiome.Desert))
                {
                    list.Add(BuildAndExpandType.Dryer);
                    list.Add(BuildAndExpandType.DryingPan);
                }
            }
            //list.Add(BuildAndExpandType.Cook);
            list.Add(BuildAndExpandType.Smelter);
            list.Add(BuildAndExpandType.Foundry);
            list.Add(BuildAndExpandType.Smith);

            list.Add(BuildAndExpandType.Carpenter);
            
            list.Add(BuildAndExpandType.Pottery);
            
            

            if (logistics1)
            {
                list.Add(BuildAndExpandType.Armory);
                list.Add(BuildAndExpandType.ShieldMaker);
            }
            if (unlocks.building_chemist)
            {
                list.Add(BuildAndExpandType.Chemist);
            }
            if (unlocks.building_gunmaker)
            {
                list.Add(BuildAndExpandType.Gunmaker);
            }

            list.Add(BuildAndExpandType.SoldierBarracks);
            list.Add(BuildAndExpandType.ArcherBarracks);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.WarmachineBarracks);
            }
            if (unlocks.building_gunBarrack || godPowers)
            {
                list.Add(BuildAndExpandType.GunBarracks);
            }
            if (unlocks.building_cannonBarrack ||
                godPowers)
            {
                list.Add(BuildAndExpandType.CannonBarracks);
            }

            if (logistics1)
            {                    
                list.Add(BuildAndExpandType.WoodCutter);
                list.Add(BuildAndExpandType.StoneCutter);
            }

            if (!campSite && unlocks.building_stoneBuildings)
            {
                list.Add(BuildAndExpandType.Noblehouse);

                if (city.buildingStructure.Noblehouse_count > 0 ||
                    godPowers)
                {
                    list.Add(BuildAndExpandType.Embassy);
                }

                list.Add(BuildAndExpandType.Bank);
                if (city.buildingStructure.Bank_count > 0 ||
                    godPowers)
                {
                    list.Add(BuildAndExpandType.CoinMinter);
                }
            }

            list.Add(BuildAndExpandType.DirtRoad);

            if (logistics2)
            {
                list.Add(BuildAndExpandType.Pavement);
                list.Add(BuildAndExpandType.PavementFlower);
                list.Add(BuildAndExpandType.PavementRectFlower);
                list.Add(BuildAndExpandType.PavementLamp);
                list.Add(BuildAndExpandType.PavemenFountain);

                list.Add(BuildAndExpandType.GardenGrass);
                list.Add(BuildAndExpandType.GardenBird);
                list.Add(BuildAndExpandType.GardenFourBushes);
                list.Add(BuildAndExpandType.GardenLongTree);
                list.Add(BuildAndExpandType.GardenWalledBush);

                list.Add(BuildAndExpandType.Statue_Leader);
                list.Add(BuildAndExpandType.Statue_Lion);
                list.Add(BuildAndExpandType.Statue_Horse);
                list.Add(BuildAndExpandType.Statue_Pillar);

            }
            if (unlocks.building_stoneBuildings ||
                godPowers)
            {
                list.Add(BuildAndExpandType.Statue_ThePlayer);
            }

            if (city.GetGroupedResource(ItemResourceType.Palisade).amount > 0)
            {
                list.Add(BuildAndExpandType.Palisade);
            }

            list.Add(BuildAndExpandType.DirtWall);
            list.Add(BuildAndExpandType.DirtTower);            

            if (logistics1)
            {
                list.Add(BuildAndExpandType.WoodWall);
                list.Add(BuildAndExpandType.WoodTower);
                list.Add(BuildAndExpandType.StoneWall);
                list.Add(BuildAndExpandType.StoneTower);
                list.Add(BuildAndExpandType.StoneWallGreen);
                list.Add(BuildAndExpandType.StoneWallBlueRoof);
                list.Add(BuildAndExpandType.StoneWallWoodHouse);
                list.Add(BuildAndExpandType.StoneGate);
                list.Add(BuildAndExpandType.StoneHouse);
                list.Add(BuildAndExpandType.CitySquare);
            }
           
            list.Add(BuildAndExpandType.CobbleStones);
            list.Add(BuildAndExpandType.GardenMemoryStone);

            if (logistics1)
            {
                list.Add(BuildAndExpandType.FlagPole_LongBanner);
                list.Add(BuildAndExpandType.FlagPole_Banner);
                list.Add(BuildAndExpandType.FlagPole_SlimBanner);

                list.Add(BuildAndExpandType.FlagPole_Flag);
                list.Add(BuildAndExpandType.FlagPole_FlagRound);
                list.Add(BuildAndExpandType.FlagPole_FlagLarge);
                list.Add(BuildAndExpandType.FlagPole_Streamer);
                list.Add(BuildAndExpandType.FlagPole_Triangle);

                if (DssRef.DlcSupporter.owned)
                {
                    list.Add(BuildAndExpandType.DiplomaticStatue_ThumbsUpWest);
                    list.Add(BuildAndExpandType.DiplomaticStatue_ThumbsUpEast);
                    list.Add(BuildAndExpandType.DiplomaticStatue_InsultWest);
                    list.Add(BuildAndExpandType.DiplomaticStatue_InsultEast);
                    list.Add(BuildAndExpandType.DiplomaticStatue_GoldenPoop);
                }
            }


            if (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator)
            {
                list.Add(BuildAndExpandType.TreeSoft);
                list.Add(BuildAndExpandType.TreeHard);
                list.Add(BuildAndExpandType.CoalMine);
                list.Add(BuildAndExpandType.IronOreMine);
                list.Add(BuildAndExpandType.TinOreMine);
                list.Add(BuildAndExpandType.CopperOreMine);
                list.Add(BuildAndExpandType.SilverOreMine);
                list.Add(BuildAndExpandType.GoldOreMine);
                list.Add(BuildAndExpandType.LeadOreMine);
                list.Add(BuildAndExpandType.MithrilMine);
                list.Add(BuildAndExpandType.SulfurMine);
            }


            void addAnimalPen(BuildAndExpandType build, int cityResourceType, int buildingCount) 
            {
                if (
                    (logistics1 && (buildingCount > 0 || city.GetGroupedResource(cityResourceType).amount > 0)) || 
                    StartupSettings.UnlockAllProgress)
                {
                    list.Add(build);
                }
            }

            bool biomRequirement(CityBiome biom)
            {
                return biom == CityBiome.Default_Fields || city.cityBiome == biom || StartupSettings.UnlockAllProgress;
            }
        }



        public static void Init()
        {
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, SpriteName.WarsBuild_Logistics, CraftBuildingLib.CraftLogistics, true, 
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.ManorLord, TerrainMainType.Building, (int)TerrainBuildingType.ManorLord, SpriteName.WarsBuild_ManorLord, CraftBuildingLib.ManorLord, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Farm, BuildFilterTag.Food, 
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.GreatHall, TerrainMainType.Building, (int)TerrainBuildingType.GreatHall, SpriteName.WarsBuild_GreatHall, CraftBuildingLib.GreatHall, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Upgrade, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.JustOne, DssConst.WorkTime_Building_Default)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.WorkerTent, TerrainMainType.Building, (int)TerrainBuildingType.WorkerTent, SpriteName.WarsBuild_TentHut, CraftBuildingLib.WorkerTent, true,
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_WorkerTent);

            new BuildOption(BuildAndExpandType.WorkerHut, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, CraftBuildingLib.WorkerHut, true, 
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WorkerHutLarge, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHutLarge, SpriteName.WarsBuild_WorkerHutLarge, CraftBuildingLib.WorkerHutLarge, false, 
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.ServiceHouse_Small, TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_small, SpriteName.WarsBuild_SmallServiceHouse, CraftBuildingLib.ServiceHouse_Small, true, 
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ServiceHouse_Large, TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_Large, SpriteName.WarsBuild_BigServiceHouse, CraftBuildingLib.ServiceHouse_Large, false, 
                BuildCategoryTab.General, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.GuardHouse_Small, TerrainMainType.Building, (int)TerrainBuildingType.GuardHouse_Small, SpriteName.WarsBuild_GuardOffice, CraftBuildingLib.GuardHouse_Small, true, 
                BuildCategoryTab.Military, BuildFilterTag.Guards, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            
            new BuildOption(BuildAndExpandType.GuardHouse_Large, TerrainMainType.Building, (int)TerrainBuildingType.GuardHouse_Large, SpriteName.WarsBuild_GuardOfficeLarge, CraftBuildingLib.GuardHouse_Large, false, 
                BuildCategoryTab.Military, BuildFilterTag.Guards, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.ImmigrationTent, TerrainMainType.Building, (int)TerrainBuildingType.ImmigrationTent, SpriteName.WarsBuild_Tent, CraftBuildingLib.ImmigrationTent, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Workers, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PostalLevel2, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel2, SpriteName.WarsBuild_PostalLevel2, CraftBuildingLib.Postal_Level2, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.PostalLevel3, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel3, SpriteName.WarsBuild_PostalLevel3, CraftBuildingLib.Postal_Level3, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment,
                true, BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.RecruitmentLevel2, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel2, SpriteName.WarsBuild_RecruitmentLevel2, CraftBuildingLib.Recruitment_Level2, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.RecruitmentLevel3, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel3, SpriteName.WarsBuild_RecruitmentLevel3, CraftBuildingLib.Recruitment_Level3, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl1, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel1, SpriteName.WarsBuild_GoldDeliver, CraftBuildingLib.GoldDelivery, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl2, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel2, SpriteName.WarsBuild_GoldDeliverLevel2, CraftBuildingLib.GoldDelivery_Level2, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);
            
            new BuildOption(BuildAndExpandType.GoldDeliveryLvl3, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel3, SpriteName.WarsBuild_GoldDeliverLevel3, CraftBuildingLib.GoldDelivery_Level3, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Transport, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.SoldierBarracks, TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, SpriteName.WarsBuild_SoldierBarracks, CraftBuildingLib.SoldierBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Noblehouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, SpriteName.WarsBuild_Nobelhouse, CraftBuildingLib.NobleHouse, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Upgrade, BuildFilterTag.Military, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, SpriteName.WarsBuild_Tavern, CraftBuildingLib.Tavern, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Food, BuildFilterTag.Workers, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Storehouse, TerrainMainType.Building, (int)TerrainBuildingType.Storehouse, SpriteName.WarsBuild_Storehouse, CraftBuildingLib.Storehouse, false,
                BuildCategoryTab.Advanced, BuildFilterTag.Resources, BuildFilterTag.Workers, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, SpriteName.WarsBuild_Brewery, CraftBuildingLib.Brewery, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Water, BuildFilterTag.Optimize,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
                { altBlueprint = CraftBuildingLib.Brewery_Bronze };


            new BuildOption(BuildAndExpandType.TrapperHut, TerrainMainType.Building, (int)TerrainBuildingType.TrappersHut, SpriteName.WarsBuild_Trapper, CraftBuildingLib.TrapperHut, false,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            

            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, SpriteName.WarsBuild_Cook, CraftBuildingLib.Cook, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
                { altBlueprint = CraftBuildingLib.Cook_Copper };

            new BuildOption(BuildAndExpandType.CoalPit, TerrainMainType.Building, (int)TerrainBuildingType.Work_CoalPit, SpriteName.WarsBuild_CoalPit, CraftBuildingLib.CoalPit, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WorkBench, TerrainMainType.Building, (int)TerrainBuildingType.Work_Bench, SpriteName.WarsBuild_WorkBench, CraftBuildingLib.WorkBench, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default){ altBlueprint = CraftBuildingLib.WorkBench_Bronze };

            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, SpriteName.WarsBuild_Smith, CraftBuildingLib.Smith, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Metals,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Carpenter, TerrainMainType.Building, (int)TerrainBuildingType.Carpenter, SpriteName.WarsBuild_Carpenter, CraftBuildingLib.Carpenter, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default) { altBlueprint = CraftBuildingLib.Carpenter_Bronze };

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.WheatFarm_Gold };

            new BuildOption(BuildAndExpandType.WheatFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarmUpgraded, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarmUpgrade, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarm, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarm, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.LinenFarm_Gold };
            

            new BuildOption(BuildAndExpandType.LinenFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarmUpgraded, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarmUpgrade, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarm, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarm, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.Fuel, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.HempFarm_Gold };

            new BuildOption(BuildAndExpandType.HempFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarmUpgraded, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarmUpgrade, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.Fuel, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarm, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarm, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.RapeseedFarm_Gold };

            new BuildOption(BuildAndExpandType.RapeSeedFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarmUpgraded, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarmUpgrade, true, 
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Fuel, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TreeSeedlingSoft, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoftSprout, SpriteName.WarsBuild_TreeSeedlingSoft, CraftBuildingLib.TreeSeedlingSoft, false,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.TreeSeedlingSoft_Gold };

            new BuildOption(BuildAndExpandType.TreeSeedlingHard, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHardSprout, SpriteName.WarsBuild_TreeSeedlingHard, CraftBuildingLib.TreeSeedlingHard, false,
               BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
               MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.TreeSeedlingHard_Gold };

            new BuildOption(BuildAndExpandType.OrchardApple, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeApple, SpriteName.WarsBuild_TreeApple, CraftBuildingLib.Orchard, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Orchard_Gold };

            new BuildOption(BuildAndExpandType.OrchidBanana, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeBanana, SpriteName.WarsBuild_TreeBanana, CraftBuildingLib.Orchard, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { altBlueprint = CraftBuildingLib.Orchard_Gold };

            new BuildOption(BuildAndExpandType.DirtRoad, TerrainMainType.Road, (int)TerrainRoadType.DirtRoad, SpriteName.warsFoliageDirtRoad, CraftBuildingLib.DirtRoad, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, CraftBuildingLib.Pavement, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, CraftBuildingLib.PavementFlower, false, //B
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PavementRectFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementRectFlower, SpriteName.WarsBuild_PavementRectFlower, CraftBuildingLib.PavementRectFlower, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);
            
            new BuildOption(BuildAndExpandType.PavementLamp, TerrainMainType.Decor, (int)TerrainDecorType.PavementLamp, SpriteName.WarsBuild_PavementLamp, CraftBuildingLib.PavementLamp, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
               MapPaintToolCategory.Road, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.PavemenFountain, TerrainMainType.Decor, (int)TerrainDecorType.PavemenFountain, SpriteName.WarsBuild_PavemenFountain, CraftBuildingLib.PavemenFountain, false,
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Road, DssConst.WorkTime_Building_Large);

            

            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, CraftBuildingLib.Statue, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.Smelter, TerrainMainType.Building, (int)TerrainBuildingType.Smelter, SpriteName.WarsBuild_Smelter, CraftBuildingLib.Smelter, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Metals, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Armory, TerrainMainType.Building, (int)TerrainBuildingType.Armory, SpriteName.WarsBuild_Armory, CraftBuildingLib.Armory, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Soldiers, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WoodCutter, TerrainMainType.Building, (int)TerrainBuildingType.WoodCutter, SpriteName.WarsBuild_WoodCutter, CraftBuildingLib.WoodCutter, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Optimize, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.StoneCutter, TerrainMainType.Building, (int)TerrainBuildingType.StoneCutter, SpriteName.WarsBuild_StoneCutter, CraftBuildingLib.StoneCutter, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.Optimize, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Bank, TerrainMainType.Building, (int)TerrainBuildingType.Bank, SpriteName.WarsBuild_Bank, CraftBuildingLib.Bank, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Upgrade, BuildFilterTag.Gold, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.CoinMinter, TerrainMainType.Building, (int)TerrainBuildingType.CoinMinter, SpriteName.WarsBuild_Coinminter, CraftBuildingLib.CoinMinter, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Gold, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Embassy, TerrainMainType.Building, (int)TerrainBuildingType.Embassy, SpriteName.WarsBuild_Embassy, CraftBuildingLib.Embassy, false, 
                BuildCategoryTab.Advanced, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.WaterResovoir, TerrainMainType.Building, (int)TerrainBuildingType.WaterResovoir, SpriteName.WarsBuild_WaterReservoir, CraftBuildingLib.WaterResovoir, true, 
                BuildCategoryTab.Advanced, BuildFilterTag.Optimize, BuildFilterTag.Water, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.ArcherBarracks, TerrainMainType.Building, (int)TerrainBuildingType.ArcherBarracks, SpriteName.WarsBuild_ArcherBarracks, CraftBuildingLib.ArcherBarracks, true,
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.WarmachineBarracks, TerrainMainType.Building, (int)TerrainBuildingType.WarmachineBarracks, SpriteName.WarsBuild_WarmachineBarracks, CraftBuildingLib.WarmachineBarracks, true, 
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.GunBarracks, TerrainMainType.Building, (int)TerrainBuildingType.GunBarracks, SpriteName.WarsBuild_GunBarracks, CraftBuildingLib.GunBarracks, true, 
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CannonBarracks, TerrainMainType.Building, (int)TerrainBuildingType.CannonBarracks, SpriteName.WarsBuild_CannonBarracks, CraftBuildingLib.CannonBarracks, true, 
                BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //new BuildOption(BuildAndExpandType.KnightsBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.KnightsBarracks, true, 
            //    BuildCategoryTab.Military, BuildFilterTag.Soldiers, BuildFilterTag.Military, BuildFilterTag.NUM_NONE, 
            //    MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Foundry, TerrainMainType.Building, (int)TerrainBuildingType.Foundry, SpriteName.WarsBuild_Foundry, CraftBuildingLib.Foundry, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Metals, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Chemist, TerrainMainType.Building, (int)TerrainBuildingType.Chemist, SpriteName.WarsBuild_Chemist, CraftBuildingLib.Chemist, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Gunmaker, TerrainMainType.Building, (int)TerrainBuildingType.Gunmaker, SpriteName.WarsBuild_Gunmaker, CraftBuildingLib.Gunmaker, true, 
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.School, TerrainMainType.Building, (int)TerrainBuildingType.School, SpriteName.WarsBuild_School, CraftBuildingLib.School, true, 
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Workers, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ResearchCenter, TerrainMainType.Building, (int)TerrainBuildingType.ResearchCenter, SpriteName.WarsBuild_ResearchCenter, CraftBuildingLib.ResearchCenter, false, 
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Research, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.BookPress, TerrainMainType.Building, (int)TerrainBuildingType.BookPress, SpriteName.WarsBuild_Bookpress, CraftBuildingLib.BookPress, false,
                BuildCategoryTab.Upgrade, BuildFilterTag.Optimize, BuildFilterTag.Research, BuildFilterTag.NUM_NONE, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Palisade, TerrainMainType.Wall, (int)TerrainWallType.Palisade, SpriteName.WarsBuild_Palisade, CraftBuildingLib.Palisade, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards, 
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Palisade);

            new BuildOption(BuildAndExpandType.DirtWall, TerrainMainType.Wall, (int)TerrainWallType.DirtWall, SpriteName.WarsBuild_DirtWall, CraftBuildingLib.DirtWall, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards, 
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.DirtTower,TerrainMainType.Wall, (int)TerrainWallType.DirtTower, SpriteName.WarsBuild_DirtTower, CraftBuildingLib.DirtTower, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.WoodWall,TerrainMainType.Wall, (int)TerrainWallType.WoodWall, SpriteName.WarsBuild_WoodWall, CraftBuildingLib.WoodWall, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.WoodTower,TerrainMainType.Wall, (int)TerrainWallType.WoodTower, SpriteName.WarsBuild_WoodTower, CraftBuildingLib.WoodTower, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.StoneWall,TerrainMainType.Wall, (int)TerrainWallType.StoneWall, SpriteName.WarsBuild_StoneWall, CraftBuildingLib.StoneWall, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneTower,TerrainMainType.Wall, (int)TerrainWallType.StoneTower, SpriteName.WarsBuild_StoneTower, CraftBuildingLib.StoneTower, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.StoneWallGreen,TerrainMainType.Wall, (int)TerrainWallType.StoneWallGreen, SpriteName.WarsBuild_StoneWallGreen, CraftBuildingLib.StoneWallGreen, false, 
                BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

             new BuildOption(BuildAndExpandType.StoneWallBlueRoof,TerrainMainType.Wall, (int)TerrainWallType.StoneWallBlueRoof, SpriteName.WarsBuild_StoneWallBlueRoof, CraftBuildingLib.StoneWallBlueRoof, false, 
                 BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

             new BuildOption(BuildAndExpandType.StoneWallWoodHouse,TerrainMainType.Wall, (int)TerrainWallType.StoneWallWoodHouse, SpriteName.WarsBuild_StoneWallWoodHouse, CraftBuildingLib.StoneWallWoodHouse, false, 
                 BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

             new BuildOption(BuildAndExpandType.StoneGate,TerrainMainType.Wall, (int)TerrainWallType.StoneGate, SpriteName.WarsBuild_StoneGate, CraftBuildingLib.StoneGate, false, 
                 BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                 MapPaintToolCategory.Wall, DssConst.WorkTime_Building_Epic);

             new BuildOption(BuildAndExpandType.StoneHouse,TerrainMainType.Wall, (int)TerrainWallType.StoneHouse, SpriteName.WarsBuild_StoneHouse, CraftBuildingLib.StoneHouse, false, 
                 BuildCategoryTab.Military, BuildFilterTag.Walls, BuildFilterTag.Military, BuildFilterTag.Guards,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            

            new BuildOption(BuildAndExpandType.GardenGrass, TerrainMainType.Decor, (int)TerrainDecorType.GardenGrass, SpriteName.WarsBuild_GardenGrass, CraftBuildingLib.GardenGrass, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenBird, TerrainMainType.Decor, (int)TerrainDecorType.GardenBird, SpriteName.WarsBuild_GardenBird, CraftBuildingLib.GardenBird, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenFourBushes, TerrainMainType.Decor, (int)TerrainDecorType.GardenFourBushes, SpriteName.WarsBuild_GardenFourBushes, CraftBuildingLib.GardenFourBushes, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenLongTree,TerrainMainType.Decor, (int)TerrainDecorType.GardenLongTree, SpriteName.WarsBuild_GardenLongTree, CraftBuildingLib.GardenLongTree, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GardenWalledBush,TerrainMainType.Decor, (int)TerrainDecorType.GardenWalledBush, SpriteName.WarsBuild_GardenWalledBush, CraftBuildingLib.GardenWalledBush, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //new BuildOption(BuildAndExpandType.ServiceHouse_Small,TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_small, SpriteName.MissingImage, CraftBuildingLib.SmallCityHouse, false, BuildCategoryTab.ExpandAndCraft, MapPaintToolCategory.Default);
            //new BuildOption(BuildAndExpandType.BigCityHouse,TerrainMainType.Building, (int)TerrainBuildingType.ServiceMenHouse_Large, SpriteName.MissingImage, CraftBuildingLib.BigCityHouse, false, BuildCategoryTab.ExpandAndCraft, MapPaintToolCategory.Default);
            
            new BuildOption(BuildAndExpandType.CitySquare,TerrainMainType.Decor, (int)TerrainDecorType.Square, SpriteName.WarsBuild_CitySquare, CraftBuildingLib.CitySquare, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CobbleStones,TerrainMainType.Decor, (int)TerrainDecorType.CobbleStones, SpriteName.WarsBuild_CobbleStones, CraftBuildingLib.CobbleStones, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Road, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.GardenMemoryStone, TerrainMainType.Decor, (int)TerrainDecorType.GardenMemoryStone, SpriteName.WarsBuild_GardenMemoryStone, CraftBuildingLib.GardenMemoryStone, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Garden, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.Statue_Leader, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Leader, SpriteName.WarsBuild_Statue_Leader, CraftBuildingLib.Statue_Leader, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);

            new BuildOption(BuildAndExpandType.Statue_Lion, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Lion, SpriteName.WarsBuild_Statue_Lion, CraftBuildingLib.Statue_Lion, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Statue_Horse, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Horse, SpriteName.WarsBuild_Statue_Horse, CraftBuildingLib.Statue_Horse, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Large);

            new BuildOption(BuildAndExpandType.Statue_Pillar, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Pillar, SpriteName.WarsBuild_Statue_Pillar, CraftBuildingLib.Statue_Pillar, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);


            //    DiplomaticStatue_ThumbsUpWest,

            new BuildOption(BuildAndExpandType.DiplomaticStatue_ThumbsUpWest, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_ThumbsUpWest, SpriteName.WarsBuild_DiplomaticStatue_ThumbsUpWest, CraftBuildingLib.DiplomaticStatueUpW, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);
            //DiplomaticStatue_ThumbsUpEast,
            new BuildOption(BuildAndExpandType.DiplomaticStatue_ThumbsUpEast, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_ThumbsUpEast, SpriteName.WarsBuild_DiplomaticStatue_ThumbsUpEast, CraftBuildingLib.DiplomaticStatueUpE, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);
            //DiplomaticStatue_InsultWest,
            new BuildOption(BuildAndExpandType.DiplomaticStatue_InsultWest, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_InsultWest, SpriteName.WarsBuild_DiplomaticStatue_InsultWest, CraftBuildingLib.DiplomaticStatueInsultW, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);
            //DiplomaticStatue_InsultEast,
            new BuildOption(BuildAndExpandType.DiplomaticStatue_InsultEast, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_InsultEast, SpriteName.WarsBuild_DiplomaticStatue_InsultEast, CraftBuildingLib.DiplomaticStatueInsultE, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);
            //DiplomaticStatue_GoldenPoop,
            new BuildOption(BuildAndExpandType.DiplomaticStatue_GoldenPoop, TerrainMainType.Decor, (int)TerrainDecorType.DiplomaticStatue_GoldenPoop, SpriteName.WarsBuild_DiplomaticStatue_GoldenPoop, CraftBuildingLib.DiplomaticStatue_GoldenPoop, false,
                BuildCategoryTab.Decor, BuildFilterTag.Statue, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Epic);


            new BuildOption(BuildAndExpandType.FlagPole_LongBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_LongBanner, SpriteName.WarsFlagType_LongBanner, CraftBuildingLib.FlagPole_LongBanner, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Banner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Banner, SpriteName.WarsFlagType_Banner, CraftBuildingLib.FlagPole_Banner, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_SlimBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_SlimBanner, SpriteName.WarsFlagType_SlimBanner, CraftBuildingLib.FlagPole_SlimBanner, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Flag, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Flag, SpriteName.WarsFlagType_Flag, CraftBuildingLib.FlagPole_Flag, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_FlagRound, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagRound, SpriteName.WarsFlagType_FlagRound, CraftBuildingLib.FlagPole_FlagRound, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_FlagLarge, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagLarge, SpriteName.WarsFlagType_FlagLarge, CraftBuildingLib.FlagPole_FlagLarge, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default , DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Streamer, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Streamer, SpriteName.WarsFlagType_Streamer, CraftBuildingLib.FlagPole_Streamer, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.FlagPole_Triangle, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Triangle, SpriteName.WarsFlagType_Triangle, CraftBuildingLib.FlagPole_Triangle, false, 
                BuildCategoryTab.Decor, BuildFilterTag.Flag, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
                 MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);


            new BuildOption(BuildAndExpandType.TreeSoft, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeSoft, SpriteName.WarsBuild_TreeSoft, CraftBuildingLib.TreeSoft, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TreeHard, TerrainMainType.Foil, (int)TerrainSubFoilType.TreeHard, SpriteName.WarsBuild_TreeHard, CraftBuildingLib.TreeHard, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Farm, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            // Mines
            //new BuildOption(BuildAndExpandType.StonesMine, TerrainMainType.Mine, (int)TerrainMineType.Stones, SpriteName.WarsResource_Stone, CraftBuildingLib.StonesMine, false,
            //  BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
            //  MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CoalMine, TerrainMainType.Mine, (int)TerrainMineType.Coal, SpriteName.WarsResource_Fuel, CraftBuildingLib.CoalMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.StoneBlockMine, TerrainMainType.Mine, (int)TerrainMineType.StoneBlock, SpriteName.WarsResource_Stone, CraftBuildingLib.StoneBlockMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.IronOreMine, TerrainMainType.Mine, (int)TerrainMineType.IronOre, SpriteName.WarsResource_Iron, CraftBuildingLib.IronOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.TinOreMine, TerrainMainType.Mine, (int)TerrainMineType.TinOre, SpriteName.WarsResource_Tin, CraftBuildingLib.TinOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.CopperOreMine, TerrainMainType.Mine, (int)TerrainMineType.CopperOre, SpriteName.WarsResource_Copper, CraftBuildingLib.CopperOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.SilverOreMine, TerrainMainType.Mine, (int)TerrainMineType.SilverOre, SpriteName.WarsResource_Silver, CraftBuildingLib.SilverOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.GoldOreMine, TerrainMainType.Mine, (int)TerrainMineType.GoldOre, SpriteName.WarsResource_Gold, CraftBuildingLib.GoldOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.LeadOreMine, TerrainMainType.Mine, (int)TerrainMineType.LeadOre, SpriteName.WarsResource_Lead, CraftBuildingLib.LeadOreMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.MithrilMine, TerrainMainType.Mine, (int)TerrainMineType.Mithril, SpriteName.WarsResource_Mithril, CraftBuildingLib.MithrilMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.SulfurMine, TerrainMainType.Mine, (int)TerrainMineType.Sulfur, SpriteName.WarsResource_Sulfur, CraftBuildingLib.SulfurMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);


            // SaltMine
            new BuildOption(BuildAndExpandType.SaltMine, TerrainMainType.Mine, (int)TerrainMineType.Salt, SpriteName.WarsResource_Salt, CraftBuildingLib.SaltMine, false,
              BuildCategoryTab.GodPower, BuildFilterTag.Resources, BuildFilterTag.NUM_NONE, BuildFilterTag.NUM_NONE,
              MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            //    Pottery,
            new BuildOption(BuildAndExpandType.Pottery, TerrainMainType.Building, (int)TerrainBuildingType.Pottery, SpriteName.WarsBuild_Pottery, CraftBuildingLib.Pottery, true,
                BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Storage, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.ShieldMaker, TerrainMainType.Building, (int)TerrainBuildingType.ShieldMaker, SpriteName.WarsBuild_Shieldmaker, CraftBuildingLib.ShieldMaker, true,
               BuildCategoryTab.General, BuildFilterTag.Craft, BuildFilterTag.Weapons, BuildFilterTag.Soldiers,
               MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            //DryingPan,
            new BuildOption(BuildAndExpandType.DryingPan, TerrainMainType.Building, (int)TerrainBuildingType.DryingPan, SpriteName.WarsBuild_DryingPan, CraftBuildingLib.DryingPan, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //Butcher,
            new BuildOption(BuildAndExpandType.Butcher, TerrainMainType.Building, (int)TerrainBuildingType.Butcher, SpriteName.WarsBuild_Butcher, CraftBuildingLib.Butcher, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.Animals,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            //Smoker,
            new BuildOption(BuildAndExpandType.Smoker, TerrainMainType.Building, (int)TerrainBuildingType.Smoker, SpriteName.WarsBuild_Smoker, CraftBuildingLib.Smoker, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            //Dryer,
            new BuildOption(BuildAndExpandType.Dryer, TerrainMainType.Building, (int)TerrainBuildingType.Dryer, SpriteName.WarsBuild_Dryer, CraftBuildingLib.Dryer, true,
                BuildCategoryTab.Advanced, BuildFilterTag.Craft, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            new BuildOption(BuildAndExpandType.MaterialStorage, TerrainMainType.Building, (int)TerrainBuildingType.MaterialStorage, SpriteName.WarsBuild_MaterialStorage, CraftBuildingLib.MaterialStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //FoodStorage
            new BuildOption(BuildAndExpandType.FoodStorage, TerrainMainType.Building, (int)TerrainBuildingType.FoodStorage, SpriteName.WarsBuild_FoodStorage, CraftBuildingLib.FoodStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //WeaponStorage
            new BuildOption(BuildAndExpandType.WeaponStorage, TerrainMainType.Building, (int)TerrainBuildingType.WeaponStorage, SpriteName.WarsBuild_WeaponStorage, CraftBuildingLib.WeaponStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //ArmorStorage
            new BuildOption(BuildAndExpandType.ArmorStorage, TerrainMainType.Building, (int)TerrainBuildingType.ArmorStorage, SpriteName.WarsBuild_ArmorStorage, CraftBuildingLib.ArmorStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);

            //AnimalStorage
            new BuildOption(BuildAndExpandType.AnimalStorage, TerrainMainType.Building, (int)TerrainBuildingType.AnimalStorage, SpriteName.WarsBuild_AnimalStorage, CraftBuildingLib.AnimalStorage, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default);
            
            new BuildOption(BuildAndExpandType.Cesspit, TerrainMainType.Building, (int)TerrainBuildingType.Cesspit, SpriteName.WarsBuild_Cesspit, CraftBuildingLib.Cesspit, true,
                BuildCategoryTab.Upgrade, BuildFilterTag.Storage, BuildFilterTag.Optimize, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Small);

            new BuildOption(BuildAndExpandType.BoarPen, TerrainMainType.Building, (int)TerrainBuildingType.BoarPen, SpriteName.WarsBuild_BoarPen, CraftBuildingLib.BoarPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            new BuildOption(BuildAndExpandType.FowlPen, TerrainMainType.Building, (int)TerrainBuildingType.FowlPen, SpriteName.WarsBuild_FowlPen, CraftBuildingLib.FowlPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 1) };

            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.Building, (int)TerrainBuildingType.PigPen, SpriteName.WarsBuild_PigPen, CraftBuildingLib.PigPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.Resources,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.Building, (int)TerrainBuildingType.HenPen, SpriteName.WarsBuild_HenPen, CraftBuildingLib.HenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Food, BuildFilterTag.NUM_NONE,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 1) };

            //OxenPen
            new BuildOption(BuildAndExpandType.OxenPen, TerrainMainType.Building, (int)TerrainBuildingType.OxenPen, SpriteName.WarsBuild_OxenPen, CraftBuildingLib.OxenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //KineOxenPen
            new BuildOption(BuildAndExpandType.KineOxenPen, TerrainMainType.Building, (int)TerrainBuildingType.KineOxenPen, SpriteName.WarsBuild_KineOxenPen, CraftBuildingLib.KineOxenPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 3) };

            //DogCage
            new BuildOption(BuildAndExpandType.DogCage, TerrainMainType.Building, (int)TerrainBuildingType.DogCage, SpriteName.WarsBuild_DogCage, CraftBuildingLib.DogCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 1) };

            //HoundCage
            new BuildOption(BuildAndExpandType.HoundCage, TerrainMainType.Building, (int)TerrainBuildingType.HoundCage, SpriteName.WarsBuild_HoundCage, CraftBuildingLib.HoundCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 1) };

            //PonyPen
            new BuildOption(BuildAndExpandType.PonyPen, TerrainMainType.Building, (int)TerrainBuildingType.PonyPen, SpriteName.WarsBuild_PonyPen, CraftBuildingLib.PonyPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //HorsePen
            new BuildOption(BuildAndExpandType.HorsePen, TerrainMainType.Building, (int)TerrainBuildingType.HorsePen, SpriteName.WarsBuild_HorsePen, CraftBuildingLib.HorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WarHorsePen
            new BuildOption(BuildAndExpandType.WarHorsePen, TerrainMainType.Building, (int)TerrainBuildingType.WarHorsePen, SpriteName.WarsBuild_WarHorsePen, CraftBuildingLib.WarHorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //DraftHorsePen
            new BuildOption(BuildAndExpandType.DraftHorsePen, TerrainMainType.Building, (int)TerrainBuildingType.DraftHorsePen, SpriteName.WarsBuild_DraftHorsePen, CraftBuildingLib.DraftHorsePen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WildPigPen
            new BuildOption(BuildAndExpandType.WildPigPen, TerrainMainType.Building, (int)TerrainBuildingType.WildPigPen, SpriteName.WarsBuild_WildPigPen, CraftBuildingLib.WildPigPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WildHogPen
            new BuildOption(BuildAndExpandType.WildHogPen, TerrainMainType.Building, (int)TerrainBuildingType.WildHogPen, SpriteName.WarsBuild_WildHogPen, CraftBuildingLib.WildHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WarHogPen
            new BuildOption(BuildAndExpandType.WarHogPen, TerrainMainType.Building, (int)TerrainBuildingType.WarHogPen, SpriteName.WarsBuild_WarHogPen, CraftBuildingLib.WarHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //StagHogPen
            new BuildOption(BuildAndExpandType.StagHogPen, TerrainMainType.Building, (int)TerrainBuildingType.StagHogPen, SpriteName.WarsBuild_StagHogPen, CraftBuildingLib.StagHogPen, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WolfCage
            new BuildOption(BuildAndExpandType.WolfCage, TerrainMainType.Building, (int)TerrainBuildingType.WolfCage, SpriteName.WarsBuild_WolfPen, CraftBuildingLib.WolfCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //WargCage
            new BuildOption(BuildAndExpandType.WargCage, TerrainMainType.Building, (int)TerrainBuildingType.WargCage, SpriteName.WarsBuild_WargPen, CraftBuildingLib.WargCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 3) };

            //AlphaWargCage
            new BuildOption(BuildAndExpandType.AlphaWargCage, TerrainMainType.Building, (int)TerrainBuildingType.AlphaWargCage, SpriteName.WarsBuild_AlphaWargPen, CraftBuildingLib.AlphaWargCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 3) };

            //WildCatCage
            new BuildOption(BuildAndExpandType.WildCatCage, TerrainMainType.Building, (int)TerrainBuildingType.WildCatCage, SpriteName.WarsBuild_WildCatPen, CraftBuildingLib.WildCatCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 2) };

            //LionCage
            new BuildOption(BuildAndExpandType.LionCage, TerrainMainType.Building, (int)TerrainBuildingType.LionCage, SpriteName.WarsBuild_LionPen, CraftBuildingLib.LionCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 3) };

            //WarLionCage
            new BuildOption(BuildAndExpandType.WarLionCage, TerrainMainType.Building, (int)TerrainBuildingType.WarLionCage, SpriteName.WarsBuild_WarLionPen, CraftBuildingLib.WarLionCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 3) };

            //ElephantCage
            new BuildOption(BuildAndExpandType.ElephantCage, TerrainMainType.Building, (int)TerrainBuildingType.ElephantCage, SpriteName.WarsBuild_ElephantPen, CraftBuildingLib.ElephantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 8) };

            //WarElephantCage
            new BuildOption(BuildAndExpandType.WarElephantCage, TerrainMainType.Building, (int)TerrainBuildingType.WarElephantCage, SpriteName.WarsBuild_WarElephantPen, CraftBuildingLib.WarElephantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 10) };

            //OliphantCage
            new BuildOption(BuildAndExpandType.OliphantCage, TerrainMainType.Building, (int)TerrainBuildingType.OliphantCage, SpriteName.WarsBuild_OliphantPen, CraftBuildingLib.OliphantCage, true,
                BuildCategoryTab.Farming, BuildFilterTag.Farm, BuildFilterTag.Animals, BuildFilterTag.Soldiers,
                MapPaintToolCategory.Default, DssConst.WorkTime_Building_Default)
            { upkeep = new ItemResource(ItemResourceType.RawFood_Group, 20) };
        }

        public static BuildAndExpandType BuildTypeFromTerrain(TerrainMainType main, int sub)
        { 
            foreach (BuildOption buildOption in BuildOptions)
            {
                if (buildOption != null && buildOption.terrainType.EqualTerrain(main, sub))
                { 
                    return buildOption.buildType;
                }
            }

            return BuildAndExpandType.NUM_NONE;
        }

        public static bool CanAutoBuildHere(ref SubTile subTile)
        {
            switch (subTile.mainTerrain)
            {
                case TerrainMainType.DefaultLand:
                case TerrainMainType.Destroyed:
                    return true;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subTile.subTerrain)
                    {
                        case TerrainSubFoilType.Bush:
                        case TerrainSubFoilType.Herbs:
                        case TerrainSubFoilType.TallGrass:
                            return true;
                    }
                    break;
            }

            return false;
        }

        public static bool TryAutoBuild(Faction faction, IntVector2 subTilePos, TerrainMainType mainType, int terrainSubType, int amount)
        {
            SubTile subTile;
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out subTile))
            {
                if (CanAutoBuildHere(ref subTile))
                {
                    subTile.SetType(mainType, terrainSubType, amount);
                    EditSubTile edit = new EditSubTile(faction, true,subTilePos, subTile, true, true, false);
                    edit.Submit();
                    return true;
                }
            }

            return false;
        }

        public static void Demolish(City city, IntVector2 subTilePos)
        {
            var subTile = DssRef.world.subTileGrid.Get(subTilePos);
            var buildingType = BuildLib.GetType(subTile.mainTerrain, subTile.subTerrain);
            if (buildingType != BuildAndExpandType.NUM_NONE)
            {
                var opt = BuildOptions[(int)buildingType];
                opt.destroy_async(city, subTilePos);

                bool addRubble = false;
                var bp = opt.blueprint;
                foreach (var r in bp.resources)
                {
                    if (r.type == ItemResourceType.ServiceMen)
                    {
                        city.freeServiceMen.amount += r.amount;
                    }
                    else if (r.type != ItemResourceType.Water_G)
                    {
                        int returnAmount = r.amount / 2;
                        if (returnAmount > 4)
                        {
                            addRubble = true;

                            DssRef.state.resources.addItem(
                                new Resource.ItemResource(
                                  r.type,
                                  subTile.terrainQuality,
                                  0,
                                  returnAmount),
                              ref subTile.collectionPointer);
                        }
                    }
                }

                if (addRubble)
                {
                    subTile.mainTerrain = TerrainMainType.Resourses;
                    subTile.subTerrain = (int)TerrainResourcesType.Rubble;
                }
                else
                {
                    subTile.mainTerrain = TerrainMainType.Destroyed;
                    subTile.subTerrain = 0;
                }
            
                EditSubTile edit = new EditSubTile(city.GetFaction(), true, subTilePos, subTile, true, true, true);
                edit.Submit();
            }
        }

        public static BuildOption Get(BuildAndExpandType option)
        {
            return BuildOptions[(int)option];
        }
        public static BuildOption Get(TerrainMainType main, int subType)
        {
            return BuildOptions[(int)GetType(main, subType)];
        }

        public static BuildAndExpandType GetType(TerrainMainType main, int subType)
        {
            if (main == TerrainMainType.DefaultLand || main == TerrainMainType.DefaultSea)
            { 
                return BuildAndExpandType.NUM_NONE;
            }

            foreach (var opt in BuildOptions)
            {
                if (opt != null && opt.terrainType.EqualTerrain(main, subType))//opt.mainType == main && opt.subType == subType)
                { 
                    return opt.buildType;
                }
            }

            return BuildAndExpandType.NUM_NONE;
        }
    }

    enum MapPaintToolShape
    { 
        Free,
        Line,
        LShape,
        Area,
        Path,
    }

    enum MapPaintToolCategory
    { 
        Default,
        JustOne,
        Road,
        Wall,
    }

    enum BuildCategoryTab
    {  
        General,
        Advanced,
        Farming,
        Military,
        Decor,
        Upgrade,
        Automation,
        Filter,
        GodPower,
        NUM
    }

    enum BuildFilterTag
    { 
        
        Workers,
        
        Gold,
        Resources,
        Water,
        Food,
        Fuel,
        Metals,
        Craft,
        Farm,
        Animals,
        Storage,
        Transport,

        Weapons,
        Military,
        Soldiers,
        Guards,
        Walls,

        Road,
        Garden,
        Statue,
        Flag,

        Optimize,
        Upgrade,
        Research,

        NUM_NONE,
    }

    enum LShapeDir
    { 
        NoSet,
        StartX,
        StartY,
    }
}
