﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Build
{
    enum BuildAndExpandType
    {
        WorkerHuts,
        Postal,
        Recruitment,
        Barracks,
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
        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static List<BuildAndExpandType> AvailableBuildTypes(GameObject.City city)
        {
            List<BuildAndExpandType> result = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

            if (city.buildingLevel_logistics == 0)
            {
                result.Add(BuildAndExpandType.Logistics);
            }

            result.Add(BuildAndExpandType.WorkerHuts);
            result.Add(BuildAndExpandType.Barracks);
            
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.Nobelhouse);
            }
            
            result.Add(BuildAndExpandType.Postal);
            
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.Recruitment);
                result.Add(BuildAndExpandType.Storehouse);
                result.Add(BuildAndExpandType.Tavern);
                result.Add(BuildAndExpandType.Brewery);
            }
            
            result.Add(BuildAndExpandType.Cook);
            
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.CoalPit);
            }
            
            result.Add(BuildAndExpandType.WorkBench);
            result.Add(BuildAndExpandType.Smith);
            result.Add(BuildAndExpandType.Carpenter);
            result.Add(BuildAndExpandType.PigPen);
            result.Add(BuildAndExpandType.HenPen);
            result.Add(BuildAndExpandType.WheatFarm);
            result.Add(BuildAndExpandType.LinenFarm);
            result.Add(BuildAndExpandType.RapeSeedFarm);
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.HempFarm);
            }

            if (city.buildingLevel_logistics >= 2)
            {
                result.Add(BuildAndExpandType.Pavement);
                result.Add(BuildAndExpandType.PavementFlower);
                result.Add(BuildAndExpandType.Statue_ThePlayer);
            }

            return result;
        }

        public static void Init()
        {
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, SpriteName.WarsBuild_Logistics, ResourceLib.CraftLogistics)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, ResourceLib.CraftWorkerHut);
            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, ResourceLib.CraftPostal);
            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, ResourceLib.CraftRecruitment);
            new BuildOption(BuildAndExpandType.Barracks, TerrainMainType.Building, (int)TerrainBuildingType.Barracks, SpriteName.WarsBuild_Barracks, ResourceLib.CraftBarracks);
            new BuildOption(BuildAndExpandType.Nobelhouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, SpriteName.WarsBuild_Nobelhouse, ResourceLib.CraftNobelHouse);
            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, SpriteName.WarsBuild_Tavern, ResourceLib.CraftTavern);
            new BuildOption(BuildAndExpandType.Storehouse, TerrainMainType.Building, (int)TerrainBuildingType.Storehouse, SpriteName.WarsBuild_Storehouse, ResourceLib.CraftStorehouse);
            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, SpriteName.WarsBuild_Brewery, ResourceLib.CraftBrewery);
            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.Building, (int)TerrainBuildingType.PigPen, SpriteName.WarsBuild_PigPen, ResourceLib.CraftPigPen);
            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.Building, (int)TerrainBuildingType.HenPen, SpriteName.WarsBuild_HenPen, ResourceLib.CraftHenPen);
            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, SpriteName.WarsBuild_Cook, ResourceLib.CraftCook);
            new BuildOption(BuildAndExpandType.CoalPit, TerrainMainType.Building, (int)TerrainBuildingType.Work_CoalPit, SpriteName.WarsBuild_CoalPit, ResourceLib.CraftCoalPit);
            new BuildOption(BuildAndExpandType.WorkBench, TerrainMainType.Building, (int)TerrainBuildingType.Work_Bench, SpriteName.WarsBuild_WorkBench, ResourceLib.CraftWorkBench);
            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, SpriteName.WarsBuild_Smith, ResourceLib.CraftSmith);
            new BuildOption(BuildAndExpandType.Carpenter, TerrainMainType.Building, (int)TerrainBuildingType.Carpenter, SpriteName.WarsBuild_Carpenter, ResourceLib.CraftCarpenter);

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, SpriteName.WarsBuild_WheatFarms, ResourceLib.CraftWheatFarm);
            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarm, SpriteName.WarsBuild_LinenFarms, ResourceLib.CraftLinenFarm);
            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarm, SpriteName.WarsBuild_HempFarms, ResourceLib.CraftHempFarm);
            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarm, SpriteName.WarsBuild_RapeseedFarms, ResourceLib.CraftRapeseedFarm);

            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, ResourceLib.CraftPavement);
            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, ResourceLib.CraftPavementFlower);
            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, ResourceLib.CraftStatue);

        }

        public static BuildAndExpandType BuildTypeFromTerrain(TerrainMainType main, int sub)
        { 
            foreach (BuildOption buildOption in BuildOptions)
            {
                if (buildOption.mainType == main && buildOption.subType == sub)
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
