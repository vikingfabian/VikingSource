using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Build
{
    enum BuildAndExpandType
    {
        WorkerHuts,
        Postal,
        Recruitment,
        SoldierBarracks,
        Nobelhouse,
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
        WarmashineBarracks,
        GunBarracks,
        CannonBarracks,

        KnightsBarracks,

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
        SmallCityHouse,
        BigCityHouse,
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


        NUM_NONE,
    }
    static class BuildLib
    {
        public static List<BuildAndExpandType> LogisticsUnlockBuildings = new List<BuildAndExpandType>
        {
            BuildAndExpandType.Nobelhouse,
            BuildAndExpandType.Recruitment,
            BuildAndExpandType.Storehouse,
            BuildAndExpandType.Tavern,
            BuildAndExpandType.Brewery,
            //BuildAndExpandType.CoalPit,
            //BuildAndExpandType.Foundry,
            BuildAndExpandType.School,

        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static void AvailableBuildTypes(List<BuildAndExpandType> list, GameObject.City city)
        {
            var unlocks = city.technology.GetUnlocks(false);

            if (StartupSettings.UnlockAllProgress)
            { 
                unlocks.unlockAll();
            }

            if (city.buildingStructure.buildingLevel_logistics == 0 ||
                StartupSettings.UnlockAllProgress)
            {
                list.Add(BuildAndExpandType.Logistics);
            }
            if (city.buildingStructure.buildingLevel_logistics >= 1 ||
                StartupSettings.UnlockAllProgress)
            {
                list.Add(BuildAndExpandType.School);
            }

            list.Add(BuildAndExpandType.WorkerHuts);

            list.Add(BuildAndExpandType.WheatFarm);
            if (unlocks.building_upgradedFarm)
            {
                list.Add(BuildAndExpandType.WheatFarmUpgraded);
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
            if (unlocks.building_mixedFarms)
            {
                list.Add(BuildAndExpandType.HempFarm);
                if (unlocks.building_upgradedFarm)
                {
                    list.Add(BuildAndExpandType.HempFarmUpgraded);
                }
                list.Add(BuildAndExpandType.PigPen);
            }

            list.Add(BuildAndExpandType.HenPen);
                       
            if (unlocks.building_stoneBuildings)
            {
                list.Add(BuildAndExpandType.Nobelhouse);

                if (city.buildingStructure.Nobelhouse_count > 0 ||
                    StartupSettings.UnlockAllProgress)
                {
                    list.Add(BuildAndExpandType.Embassy);
                }

                list.Add(BuildAndExpandType.Bank);
                if (city.buildingStructure.Bank_count > 0 ||
                    StartupSettings.UnlockAllProgress)
                {
                    if (!DssRef.storage.centralGold)
                    {
                        list.Add(BuildAndExpandType.GoldDeliveryLvl1);
                        if (city.buildingStructure.buildingLevel_logistics >= 1 ||
                            StartupSettings.UnlockAllProgress)
                        {
                            list.Add(BuildAndExpandType.GoldDeliveryLvl2);
                            list.Add(BuildAndExpandType.GoldDeliveryLvl3);
                        }
                    }
                    list.Add(BuildAndExpandType.CoinMinter);

                }
            }

            list.Add(BuildAndExpandType.Postal);

            if (city.buildingStructure.buildingLevel_logistics >= 1 ||
                StartupSettings.UnlockAllProgress)
            {
                list.Add(BuildAndExpandType.PostalLevel2);
                list.Add(BuildAndExpandType.PostalLevel3);
                list.Add(BuildAndExpandType.Recruitment);
                list.Add(BuildAndExpandType.RecruitmentLevel2);
                list.Add(BuildAndExpandType.RecruitmentLevel3);

                list.Add(BuildAndExpandType.Storehouse);
                list.Add(BuildAndExpandType.Tavern);
                list.Add(BuildAndExpandType.Brewery);
                list.Add(BuildAndExpandType.WaterResovoir);
            }
            
            list.Add(BuildAndExpandType.CoalPit);
            list.Add(BuildAndExpandType.WorkBench);
            list.Add(BuildAndExpandType.Cook);
            list.Add(BuildAndExpandType.Smelter);
            list.Add(BuildAndExpandType.Foundry);
            list.Add(BuildAndExpandType.Smith);

            list.Add(BuildAndExpandType.Carpenter);
            if (unlocks.building_chemist)
            {
                list.Add(BuildAndExpandType.Chemist);
                list.Add(BuildAndExpandType.Gunmaker);
            }

            list.Add(BuildAndExpandType.Armory);

           
            list.Add(BuildAndExpandType.SoldierBarracks);
            list.Add(BuildAndExpandType.ArcherBarracks);
            list.Add(BuildAndExpandType.WarmashineBarracks);
            if (unlocks.building_gunBarrack)
            {
                list.Add(BuildAndExpandType.GunBarracks);
            }
            if (unlocks.building_cannonBarrack)
            {
                list.Add(BuildAndExpandType.CannonBarracks);
            }

            if (city.buildingStructure.buildingLevel_logistics >= 1 ||
                StartupSettings.UnlockAllProgress)
            {
                if (city.buildingStructure.Nobelhouse_count > 0 ||
                    StartupSettings.UnlockAllProgress)
                {
                    list.Add(BuildAndExpandType.KnightsBarracks);
                }
            }

            if (city.buildingStructure.buildingLevel_logistics >= 1 ||
                StartupSettings.UnlockAllProgress)
            {                
                list.Add(BuildAndExpandType.WoodCutter);
                list.Add(BuildAndExpandType.StoneCutter);
            }

            list.Add(BuildAndExpandType.DirtRoad);
            if (city.buildingStructure.buildingLevel_logistics >= 2 ||
                StartupSettings.UnlockAllProgress)
            {
                list.Add(BuildAndExpandType.Pavement);
                list.Add(BuildAndExpandType.PavementFlower);
                
            }
            if (unlocks.building_stoneBuildings)
            {
                list.Add(BuildAndExpandType.Statue_ThePlayer);
            }

            list.Add(BuildAndExpandType.DirtWall);
            list.Add(BuildAndExpandType.DirtTower);
            list.Add(BuildAndExpandType.WoodWall);
            list.Add(BuildAndExpandType.WoodTower);
            list.Add(BuildAndExpandType.StoneWall);
            list.Add(BuildAndExpandType.StoneTower);
            list.Add(BuildAndExpandType.StoneWallGreen);
            list.Add(BuildAndExpandType.StoneWallBlueRoof);
            list.Add(BuildAndExpandType.StoneWallWoodHouse);
            list.Add(BuildAndExpandType.StoneGate);
            list.Add(BuildAndExpandType.StoneHouse);
            list.Add(BuildAndExpandType.PavementLamp);
            list.Add(BuildAndExpandType.PavemenFountain);
            list.Add(BuildAndExpandType.PavementRectFlower);
            list.Add(BuildAndExpandType.GardenGrass);
            list.Add(BuildAndExpandType.GardenBird);
            list.Add(BuildAndExpandType.GardenFourBushes);
            list.Add(BuildAndExpandType.GardenLongTree);
            list.Add(BuildAndExpandType.GardenWalledBush);
            list.Add(BuildAndExpandType.SmallCityHouse);
            list.Add(BuildAndExpandType.BigCityHouse);
            list.Add(BuildAndExpandType.CitySquare);
            list.Add(BuildAndExpandType.CobbleStones);

            list.Add(BuildAndExpandType.GardenMemoryStone);
            list.Add(BuildAndExpandType.Statue_Leader);
            list.Add(BuildAndExpandType.Statue_Lion);
            list.Add(BuildAndExpandType.Statue_Horse);
            list.Add(BuildAndExpandType.Statue_Pillar);
            
            list.Add(BuildAndExpandType.FlagPole_LongBanner);
            list.Add(BuildAndExpandType.FlagPole_Banner);
            list.Add(BuildAndExpandType.FlagPole_SlimBanner);

            list.Add(BuildAndExpandType.FlagPole_Flag);
            list.Add(BuildAndExpandType.FlagPole_FlagRound);
            list.Add(BuildAndExpandType.FlagPole_FlagLarge);
            list.Add(BuildAndExpandType.FlagPole_Streamer);
            list.Add(BuildAndExpandType.FlagPole_Triangle);
        }

        public static void Init()
        {
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, SpriteName.WarsBuild_Logistics, CraftBuildingLib.CraftLogistics, true, MapPaintToolCategory.JustOne)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, CraftBuildingLib.WorkerHut, true, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.PostalLevel2, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel2, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal_Level2, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.PostalLevel3, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel3, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal_Level3, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.RecruitmentLevel2, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel2, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment_Level2, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.RecruitmentLevel3, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel3, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment_Level3, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl1, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel1, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GoldDeliveryLvl2, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel2, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery_Level2, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GoldDeliveryLvl3, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel3, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery_Level3, false, MapPaintToolCategory.Default);


            new BuildOption(BuildAndExpandType.SoldierBarracks, TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, SpriteName.WarsBuild_SoldierBarracks, CraftBuildingLib.SoldierBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Nobelhouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, SpriteName.WarsBuild_Nobelhouse, CraftBuildingLib.NobelHouse, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, SpriteName.WarsBuild_Tavern, CraftBuildingLib.Tavern, false   , MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Storehouse, TerrainMainType.Building, (int)TerrainBuildingType.Storehouse, SpriteName.WarsBuild_Storehouse, CraftBuildingLib.Storehouse, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, SpriteName.WarsBuild_Brewery, CraftBuildingLib.Brewery, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.Building, (int)TerrainBuildingType.PigPen, SpriteName.WarsBuild_PigPen, CraftBuildingLib.PigPen, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.Building, (int)TerrainBuildingType.HenPen, SpriteName.WarsBuild_HenPen, CraftBuildingLib.HenPen, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, SpriteName.WarsBuild_Cook, CraftBuildingLib.Cook, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.CoalPit, TerrainMainType.Building, (int)TerrainBuildingType.Work_CoalPit, SpriteName.WarsBuild_CoalPit, CraftBuildingLib.CoalPit, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WorkBench, TerrainMainType.Building, (int)TerrainBuildingType.Work_Bench, SpriteName.WarsBuild_WorkBench, CraftBuildingLib.WorkBench, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, SpriteName.WarsBuild_Smith, CraftBuildingLib.Smith, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Carpenter, TerrainMainType.Building, (int)TerrainBuildingType.Carpenter, SpriteName.WarsBuild_Carpenter, CraftBuildingLib.Carpenter, true, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarm, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WheatFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarmUpgraded, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarmUpgrade, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarm, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarm, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.LinenFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarmUpgraded, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarmUpgrade, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarm, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarm, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.HempFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarmUpgraded, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarmUpgrade, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarm, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarm, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.RapeSeedFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarmUpgraded, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarmUpgrade, true, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.DirtRoad, TerrainMainType.Road, (int)TerrainRoadType.DirtRoad, SpriteName.warsFoliageDirtRoad, CraftBuildingLib.DirtRoad, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, CraftBuildingLib.Pavement, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, CraftBuildingLib.PavementFlower, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, CraftBuildingLib.Statue, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.Smelter, TerrainMainType.Building, (int)TerrainBuildingType.Smelter, SpriteName.WarsBuild_Smelter, CraftBuildingLib.Smelter, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Armory, TerrainMainType.Building, (int)TerrainBuildingType.Armory, SpriteName.WarsBuild_Armory, CraftBuildingLib.Armory, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WoodCutter, TerrainMainType.Building, (int)TerrainBuildingType.WoodCutter, SpriteName.WarsBuild_WoodCutter, CraftBuildingLib. WoodCutter, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.StoneCutter, TerrainMainType.Building, (int)TerrainBuildingType.StoneCutter, SpriteName.WarsBuild_StoneCutter, CraftBuildingLib.StoneCutter, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Bank, TerrainMainType.Building, (int)TerrainBuildingType.Bank, SpriteName.WarsBuild_Bank, CraftBuildingLib.Bank, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.CoinMinter, TerrainMainType.Building, (int)TerrainBuildingType.CoinMinter, SpriteName.WarsBuild_Coinminter, CraftBuildingLib.CoinMinter, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Embassy, TerrainMainType.Building, (int)TerrainBuildingType.Embassy, SpriteName.WarsBuild_Embassy, CraftBuildingLib.Embassy, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WaterResovoir, TerrainMainType.Building, (int)TerrainBuildingType.WaterResovoir, SpriteName.WarsBuild_WaterReservoir, CraftBuildingLib.WaterResovoir, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.ArcherBarracks, TerrainMainType.Building, (int)TerrainBuildingType.ArcherBarracks, SpriteName.WarsBuild_ArcherBarracks, CraftBuildingLib.ArcherBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WarmashineBarracks, TerrainMainType.Building, (int)TerrainBuildingType.WarmashineBarracks, SpriteName.WarsBuild_WarmashineBarracks, CraftBuildingLib.WarmashineBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GunBarracks, TerrainMainType.Building, (int)TerrainBuildingType.GunBarracks, SpriteName.WarsBuild_GunBarracks, CraftBuildingLib.GunBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.CannonBarracks, TerrainMainType.Building, (int)TerrainBuildingType.CannonBarracks, SpriteName.WarsBuild_CannonBarracks, CraftBuildingLib.CannonBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.KnightsBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.KnightsBarracks, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Foundry, TerrainMainType.Building, (int)TerrainBuildingType.Foundry, SpriteName.WarsBuild_Foundry, CraftBuildingLib.Foundry, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Chemist, TerrainMainType.Building, (int)TerrainBuildingType.Chemist, SpriteName.WarsBuild_Chemist, CraftBuildingLib.Chemist, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Gunmaker, TerrainMainType.Building, (int)TerrainBuildingType.Gunmaker, SpriteName.WarsBuild_Gunmaker, CraftBuildingLib.Gunmaker, true, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.School, TerrainMainType.Building, (int)TerrainBuildingType.School, SpriteName.WarsBuild_School, CraftBuildingLib.School, true, MapPaintToolCategory.Default);
            
            new BuildOption(BuildAndExpandType.DirtWall, TerrainMainType.Wall, (int)TerrainWallType.DirtWall, SpriteName.MissingImage, CraftBuildingLib.DirtWall, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.DirtTower,TerrainMainType.Wall, (int)TerrainWallType.DirtTower, SpriteName.MissingImage, CraftBuildingLib.DirtTower, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.WoodWall,TerrainMainType.Wall, (int)TerrainWallType.WoodWall, SpriteName.MissingImage, CraftBuildingLib.WoodWall, false, MapPaintToolCategory.Wall);
            new BuildOption(BuildAndExpandType.WoodTower,TerrainMainType.Wall, (int)TerrainWallType.WoodTower, SpriteName.MissingImage, CraftBuildingLib.WoodTower, false, MapPaintToolCategory.Wall);
            new BuildOption(BuildAndExpandType.StoneWall,TerrainMainType.Wall, (int)TerrainWallType.StoneWall, SpriteName.MissingImage, CraftBuildingLib.StoneWall, false, MapPaintToolCategory.Wall);
            new BuildOption(BuildAndExpandType.StoneTower,TerrainMainType.Wall, (int)TerrainWallType.StoneTower, SpriteName.WarsBuild_StoneTower, CraftBuildingLib.StoneTower, false, MapPaintToolCategory.Wall);
            new BuildOption(BuildAndExpandType.StoneWallGreen,TerrainMainType.Wall, (int)TerrainWallType.StoneWallGreen, SpriteName.WarsBuild_StoneWallGreen, CraftBuildingLib.StoneWallGreen, false, MapPaintToolCategory.Wall);
             new BuildOption(BuildAndExpandType.StoneWallBlueRoof,TerrainMainType.Wall, (int)TerrainWallType.StoneWallBlueRoof, SpriteName.WarsBuild_StoneWallBlueRoof, CraftBuildingLib.StoneWallBlueRoof, false, MapPaintToolCategory.Wall);
             new BuildOption(BuildAndExpandType.StoneWallWoodHouse,TerrainMainType.Wall, (int)TerrainWallType.StoneWallWoodHouse, SpriteName.WarsBuild_StoneWallWoodHouse, CraftBuildingLib.StoneWallWoodHouse, false, MapPaintToolCategory.Wall);
             new BuildOption(BuildAndExpandType.StoneGate,TerrainMainType.Wall, (int)TerrainWallType.StoneGate, SpriteName.cmdTileStoneGate, CraftBuildingLib.StoneGate, false, MapPaintToolCategory.Wall);
             new BuildOption(BuildAndExpandType.StoneHouse,TerrainMainType.Wall, (int)TerrainWallType.StoneHouse, SpriteName.WarsBuild_StoneHouse, CraftBuildingLib.StoneHouse, false, MapPaintToolCategory.Default);
             new BuildOption(BuildAndExpandType.PavementLamp,TerrainMainType.Decor, (int)TerrainDecorType.PavementLamp, SpriteName.WarsBuild_PavementLamp, CraftBuildingLib.PavementLamp, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.PavemenFountain,TerrainMainType.Decor, (int)TerrainDecorType.PavemenFountain, SpriteName.WarsBuild_PavemenFountain, CraftBuildingLib.PavemenFountain, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.PavementRectFlower,TerrainMainType.Decor, (int)TerrainDecorType.PavementRectFlower, SpriteName.WarsBuild_PavementFlowers, CraftBuildingLib.PavementRectFlower, false, MapPaintToolCategory.Road);
            new BuildOption(BuildAndExpandType.GardenGrass, TerrainMainType.Decor, (int)TerrainDecorType.GardenGrass, SpriteName.WarsBuild_GardenGrass, CraftBuildingLib.GardenGrass, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GardenBird, TerrainMainType.Decor, (int)TerrainDecorType.GardenBird, SpriteName.WarsBuild_GardenBird, CraftBuildingLib.GardenBird, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GardenFourBushes, TerrainMainType.Decor, (int)TerrainDecorType.GardenFourBushes, SpriteName.WarsBuild_GardenFourBushes, CraftBuildingLib.GardenFourBushes, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GardenLongTree,TerrainMainType.Decor, (int)TerrainDecorType.GardenLongTree, SpriteName.WarsBuild_GardenLongTree, CraftBuildingLib.GardenLongTree, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.GardenWalledBush,TerrainMainType.Decor, (int)TerrainDecorType.GardenWalledBush, SpriteName.WarsBuild_GardenWalledBush, CraftBuildingLib.GardenWalledBush, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.SmallCityHouse,TerrainMainType.Building, (int)TerrainBuildingType.SmallHouse, SpriteName.MissingImage, CraftBuildingLib.SmallCityHouse, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.BigCityHouse,TerrainMainType.Building, (int)TerrainBuildingType.BigHouse, SpriteName.MissingImage, CraftBuildingLib.BigCityHouse, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.CitySquare,TerrainMainType.Building, (int)TerrainBuildingType.Square, SpriteName.MissingImage, CraftBuildingLib.CitySquare, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.CobbleStones,TerrainMainType.Building, (int)TerrainBuildingType.CobbleStones, SpriteName.MissingImage, CraftBuildingLib.CobbleStones, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.GardenMemoryStone, TerrainMainType.Decor, (int)TerrainDecorType.GardenMemoryStone, SpriteName.WarsBuild_GardenMemoryStone, CraftBuildingLib.GardenMemoryStone, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Statue_Leader, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Leader, SpriteName.WarsBuild_Statue_Leader, CraftBuildingLib.Statue_Leader, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Statue_Lion, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Lion, SpriteName.WarsBuild_Statue_Lion, CraftBuildingLib.Statue_Lion, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Statue_Horse, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Horse, SpriteName.WarsBuild_Statue_Horse, CraftBuildingLib.Statue_Horse, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.Statue_Pillar, TerrainMainType.Decor, (int)TerrainDecorType.Statue_Pillar, SpriteName.WarsBuild_Statue_Pillar, CraftBuildingLib.Statue_Pillar, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.FlagPole_LongBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_LongBanner, SpriteName.WarsFlagType_LongBanner, CraftBuildingLib.FlagPole_LongBanner, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_Banner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Banner, SpriteName.WarsFlagType_Banner, CraftBuildingLib.FlagPole_Banner, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_SlimBanner, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_SlimBanner, SpriteName.WarsFlagType_SlimBanner, CraftBuildingLib.FlagPole_SlimBanner, false, MapPaintToolCategory.Default);

            new BuildOption(BuildAndExpandType.FlagPole_Flag, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Flag, SpriteName.WarsFlagType_Flag, CraftBuildingLib.FlagPole_Flag, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_FlagRound, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagRound, SpriteName.WarsFlagType_FlagRound, CraftBuildingLib.FlagPole_FlagRound, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_FlagLarge, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_FlagLarge, SpriteName.WarsFlagType_FlagLarge, CraftBuildingLib.FlagPole_FlagLarge, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_Streamer, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Streamer, SpriteName.WarsFlagType_Streamer, CraftBuildingLib.FlagPole_Streamer, false, MapPaintToolCategory.Default);
            new BuildOption(BuildAndExpandType.FlagPole_Triangle, TerrainMainType.Decor, (int)TerrainDecorType.FlagPole_Triangle, SpriteName.WarsFlagType_Triangle, CraftBuildingLib.FlagPole_Triangle, false, MapPaintToolCategory.Default);


        }

        public static BuildAndExpandType BuildTypeFromTerrain(TerrainMainType main, int sub)
        { 
            foreach (BuildOption buildOption in BuildOptions)
            {
                if (buildOption != null && buildOption.mainType == main && buildOption.subType == sub)
                { 
                    return buildOption.buildType;
                }
            }

            return BuildAndExpandType.NUM_NONE;
        }

        public static bool CanAutoBuildHere(ref SubTile subTile)
        {
            if (subTile.mainTerrain == TerrainMainType.DefaultLand ||
                subTile.mainTerrain == TerrainMainType.Destroyed)
            {
                return true;
            }

            if (subTile.mainTerrain == TerrainMainType.Foil)
            {
                TerrainSubFoilType foil = (TerrainSubFoilType)subTile.subTerrain;
                return foil != TerrainSubFoilType.WheatFarm;
            }

            return false;
        }

        public static bool TryAutoBuild(IntVector2 subTilePos, TerrainMainType mainType, int terrainSubType, int amount)
        {
            SubTile subTile;
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out subTile))
            {
                if (CanAutoBuildHere(ref subTile))
                {
                    subTile.SetType(mainType, terrainSubType, amount);
                    EditSubTile edit = new EditSubTile(subTilePos, subTile, true, true, false);
                    edit.Submit();
                    //DssRef.world.subTileGrid.Set(subTilePos, subTile);
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
                
                var bp = opt.blueprint;
                foreach (var r in bp.resources)
                {
                    int returnAmount = r.amount / 2;
                    if (returnAmount > 0)
                    {
                        DssRef.state.resources.addItem(
                            new Resource.ItemResource(
                              r.type,
                              subTile.terrainQuality,
                              0,
                              returnAmount),
                          ref subTile.collectionPointer);
                    }
                }               

                subTile.mainTerrain = TerrainMainType.Resourses;
                subTile.subTerrain = (int)TerrainResourcesType.Rubble;

                EditSubTile edit = new EditSubTile(subTilePos, subTile, true, true, true);
                edit.Submit();
            }
        }

        public static BuildAndExpandType GetType(TerrainMainType main, int subType)
        {
            if (main == TerrainMainType.DefaultLand || main == TerrainMainType.DefaultSea)
            { 
                return BuildAndExpandType.NUM_NONE;
            }

            foreach (var opt in BuildOptions)
            {
                if (opt.mainType == main && opt.subType == subType)
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

    enum LShapeDir
    { 
        NoSet,
        StartX,
        StartY,
    }
}
