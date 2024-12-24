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
            BuildAndExpandType.CoalPit,
            BuildAndExpandType.Foundry,
            BuildAndExpandType.School,

        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static void AvailableBuildTypes(List<BuildAndExpandType> list, GameObject.City city)
        {
            //List<BuildAndExpandType> result = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
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
            
            //if (city.buildingStructure.buildingLevel_logistics >= 1 ||
            //    StartupSettings.UnlockAllProgress)
            //{
                list.Add(BuildAndExpandType.CoalPit);
            //}
            
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

            //return list;
        }

        public static void Init()
        {
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, SpriteName.WarsBuild_Logistics, CraftBuildingLib.CraftLogistics)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, CraftBuildingLib.WorkerHut);

            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal);
            new BuildOption(BuildAndExpandType.PostalLevel2, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel2, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal_Level2);
            new BuildOption(BuildAndExpandType.PostalLevel3, TerrainMainType.Building, (int)TerrainBuildingType.PostalLevel3, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal_Level3);

            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment);
            new BuildOption(BuildAndExpandType.RecruitmentLevel2, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel2, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment_Level2);
            new BuildOption(BuildAndExpandType.RecruitmentLevel3, TerrainMainType.Building, (int)TerrainBuildingType.RecruitmentLevel3, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment_Level3);

            new BuildOption(BuildAndExpandType.GoldDeliveryLvl1, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel1, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery);
            new BuildOption(BuildAndExpandType.GoldDeliveryLvl2, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel2, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery_Level2);
            new BuildOption(BuildAndExpandType.GoldDeliveryLvl3, TerrainMainType.Building, (int)TerrainBuildingType.GoldDeliveryLevel3, SpriteName.WarsBuild_Postal, CraftBuildingLib.GoldDelivery_Level3);


            new BuildOption(BuildAndExpandType.SoldierBarracks, TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, SpriteName.WarsBuild_SoldierBarracks, CraftBuildingLib.SoldierBarracks);
            new BuildOption(BuildAndExpandType.Nobelhouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, SpriteName.WarsBuild_Nobelhouse, CraftBuildingLib.NobelHouse);
            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, SpriteName.WarsBuild_Tavern, CraftBuildingLib.Tavern);
            new BuildOption(BuildAndExpandType.Storehouse, TerrainMainType.Building, (int)TerrainBuildingType.Storehouse, SpriteName.WarsBuild_Storehouse, CraftBuildingLib.Storehouse);
            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, SpriteName.WarsBuild_Brewery, CraftBuildingLib.Brewery);
            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.Building, (int)TerrainBuildingType.PigPen, SpriteName.WarsBuild_PigPen, CraftBuildingLib.PigPen);
            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.Building, (int)TerrainBuildingType.HenPen, SpriteName.WarsBuild_HenPen, CraftBuildingLib.HenPen);
            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, SpriteName.WarsBuild_Cook, CraftBuildingLib.Cook);
            new BuildOption(BuildAndExpandType.CoalPit, TerrainMainType.Building, (int)TerrainBuildingType.Work_CoalPit, SpriteName.WarsBuild_CoalPit, CraftBuildingLib.CoalPit);
            new BuildOption(BuildAndExpandType.WorkBench, TerrainMainType.Building, (int)TerrainBuildingType.Work_Bench, SpriteName.WarsBuild_WorkBench, CraftBuildingLib.WorkBench);
            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, SpriteName.WarsBuild_Smith, CraftBuildingLib.Smith);
            new BuildOption(BuildAndExpandType.Carpenter, TerrainMainType.Building, (int)TerrainBuildingType.Carpenter, SpriteName.WarsBuild_Carpenter, CraftBuildingLib.Carpenter);

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarm);
            new BuildOption(BuildAndExpandType.WheatFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarmUpgraded, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarmUpgrade);
            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarm, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarm);
            new BuildOption(BuildAndExpandType.LinenFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarmUpgraded, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarmUpgrade);
            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarm, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarm);
            new BuildOption(BuildAndExpandType.HempFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarmUpgraded, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarmUpgrade);
            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarm, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarm);
            new BuildOption(BuildAndExpandType.RapeSeedFarmUpgraded, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarmUpgraded, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarmUpgrade);

            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, CraftBuildingLib.Pavement);
            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, CraftBuildingLib.PavementFlower);
            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, CraftBuildingLib.Statue);

            new BuildOption(BuildAndExpandType.Smelter, TerrainMainType.Building, (int)TerrainBuildingType.Smelter, SpriteName.WarsBuild_Smelter, CraftBuildingLib.Smelter);
            new BuildOption(BuildAndExpandType.Armory, TerrainMainType.Building, (int)TerrainBuildingType.Armory, SpriteName.WarsBuild_Armory, CraftBuildingLib.Armory);
            new BuildOption(BuildAndExpandType.WoodCutter, TerrainMainType.Building, (int)TerrainBuildingType.WoodCutter, SpriteName.WarsBuild_WoodCutter, CraftBuildingLib. WoodCutter);
            new BuildOption(BuildAndExpandType.StoneCutter, TerrainMainType.Building, (int)TerrainBuildingType.StoneCutter, SpriteName.WarsBuild_StoneCutter, CraftBuildingLib.StoneCutter);
            new BuildOption(BuildAndExpandType.Bank, TerrainMainType.Building, (int)TerrainBuildingType.Bank, SpriteName.WarsBuild_Bank, CraftBuildingLib.Bank);
            new BuildOption(BuildAndExpandType.CoinMinter, TerrainMainType.Building, (int)TerrainBuildingType.CoinMinter, SpriteName.WarsBuild_Coinminter, CraftBuildingLib.CoinMinter);
            new BuildOption(BuildAndExpandType.Embassy, TerrainMainType.Building, (int)TerrainBuildingType.Embassy, SpriteName.WarsBuild_Embassy, CraftBuildingLib.Embassy);
            new BuildOption(BuildAndExpandType.WaterResovoir, TerrainMainType.Building, (int)TerrainBuildingType.WaterResovoir, SpriteName.WarsBuild_WaterReservoir, CraftBuildingLib.WaterResovoir);
            new BuildOption(BuildAndExpandType.ArcherBarracks, TerrainMainType.Building, (int)TerrainBuildingType.ArcherBarracks, SpriteName.WarsBuild_ArcherBarracks, CraftBuildingLib.ArcherBarracks);
            new BuildOption(BuildAndExpandType.WarmashineBarracks, TerrainMainType.Building, (int)TerrainBuildingType.WarmashineBarracks, SpriteName.WarsBuild_WarmashineBarracks, CraftBuildingLib.WarmashineBarracks);
            new BuildOption(BuildAndExpandType.GunBarracks, TerrainMainType.Building, (int)TerrainBuildingType.GunBarracks, SpriteName.WarsBuild_GunBarracks, CraftBuildingLib.GunBarracks);
            new BuildOption(BuildAndExpandType.CannonBarracks, TerrainMainType.Building, (int)TerrainBuildingType.CannonBarracks, SpriteName.WarsBuild_CannonBarracks, CraftBuildingLib.CannonBarracks);
            new BuildOption(BuildAndExpandType.KnightsBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.KnightsBarracks);
            new BuildOption(BuildAndExpandType.Foundry, TerrainMainType.Building, (int)TerrainBuildingType.Foundry, SpriteName.WarsBuild_Foundry, CraftBuildingLib.Foundry);
            new BuildOption(BuildAndExpandType.Chemist, TerrainMainType.Building, (int)TerrainBuildingType.Chemist, SpriteName.WarsBuild_Chemist, CraftBuildingLib.Chemist);
            new BuildOption(BuildAndExpandType.Gunmaker, TerrainMainType.Building, (int)TerrainBuildingType.Gunmaker, SpriteName.WarsBuild_Gunmaker, CraftBuildingLib.Gunmaker);
            new BuildOption(BuildAndExpandType.School, TerrainMainType.Building, (int)TerrainBuildingType.School, SpriteName.WarsBuild_School, CraftBuildingLib.School);

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


}
