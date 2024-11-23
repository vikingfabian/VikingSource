using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;

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
        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static List<BuildAndExpandType> AvailableBuildTypes(GameObject.City city)
        {
            List<BuildAndExpandType> result = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

            if (city.buildingLevel_logistics == 0)
            {
                result.Add(BuildAndExpandType.Logistics);
            }
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.WaterResovoir);
            }
            
            result.Add(BuildAndExpandType.WorkerHuts);
            result.Add(BuildAndExpandType.SoldierBarracks);
            result.Add(BuildAndExpandType.ArcherBarracks);
            result.Add(BuildAndExpandType.WarmashineBarracks);
            result.Add(BuildAndExpandType.GunBarracks);
            result.Add(BuildAndExpandType.CannonBarracks);

            if (city.buildingLevel_logistics >= 1)
            {
                if (city.buildingCount_nobelHouse > 0)
                {
                    result.Add(BuildAndExpandType.KnightsBarracks);
                }

                result.Add(BuildAndExpandType.Nobelhouse);

                if (city.buildingCount_nobelHouse > 0)
                {
                    result.Add(BuildAndExpandType.Embassy);
                }
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
            result.Add(BuildAndExpandType.Smelter);
            result.Add(BuildAndExpandType.Smith);
            result.Add(BuildAndExpandType.Foundry);
            result.Add(BuildAndExpandType.Armory);

            result.Add(BuildAndExpandType.Carpenter);
            result.Add(BuildAndExpandType.Chemist);
            result.Add(BuildAndExpandType.Gunmaker);

            result.Add(BuildAndExpandType.PigPen);
            result.Add(BuildAndExpandType.HenPen);
            result.Add(BuildAndExpandType.WheatFarm);
            result.Add(BuildAndExpandType.LinenFarm);
            result.Add(BuildAndExpandType.RapeSeedFarm);
            if (city.buildingLevel_logistics >= 1)
            {
                result.Add(BuildAndExpandType.HempFarm);

                result.Add(BuildAndExpandType.WoodCutter);
                result.Add(BuildAndExpandType.StoneCutter);

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
            new BuildOption(BuildAndExpandType.Logistics, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, SpriteName.WarsBuild_Logistics, CraftBuildingLib.CraftLogistics)
            {
                uniqueBuilding = true
            };

            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, CraftBuildingLib.Postal);
            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, CraftBuildingLib.Recruitment);
            new BuildOption(BuildAndExpandType.SoldierBarracks, TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, SpriteName.WarsBuild_Barracks, CraftBuildingLib.Barracks);
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

            new BuildOption(BuildAndExpandType.WheatFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, SpriteName.WarsBuild_WheatFarms, CraftBuildingLib.WheatFarm );
            new BuildOption(BuildAndExpandType.LinenFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.LinenFarm, SpriteName.WarsBuild_LinenFarms, CraftBuildingLib.LinenFarm);
            new BuildOption(BuildAndExpandType.HempFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.HempFarm, SpriteName.WarsBuild_HempFarms, CraftBuildingLib.HempFarm);
            new BuildOption(BuildAndExpandType.RapeSeedFarm, TerrainMainType.Foil, (int)TerrainSubFoilType.RapeSeedFarm, SpriteName.WarsBuild_RapeseedFarms, CraftBuildingLib.RapeseedFarm);

            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, CraftBuildingLib.Pavement);
            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, CraftBuildingLib.PavementFlower);
            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, CraftBuildingLib.Statue);

            new BuildOption(BuildAndExpandType.Smelter, TerrainMainType.Building, (int)TerrainBuildingType.Smelter, SpriteName.WarsBuild_Smelter, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.Armory, TerrainMainType.Building, (int)TerrainBuildingType.Armory, SpriteName.WarsBuild_Smelter, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.WoodCutter, TerrainMainType.Building, (int)TerrainBuildingType.WoodCutter, SpriteName.WarsBuild_WoodCutter, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.StoneCutter, TerrainMainType.Building, (int)TerrainBuildingType.StoneCutter, SpriteName.WarsBuild_StoneCutter, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.Embassy, TerrainMainType.Building, (int)TerrainBuildingType.Embassy, SpriteName.WarsBuild_Embassy, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.WaterResovoir, TerrainMainType.Building, (int)TerrainBuildingType.WaterResovoir, SpriteName.WarsBuild_WaterReservoir, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.ArcherBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.WarmashineBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.GunBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.KnightsBarracks, TerrainMainType.Building, (int)TerrainBuildingType.KnightsBarracks, SpriteName.WarsBuild_KnightBarrack, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.Foundry, TerrainMainType.Building, (int)TerrainBuildingType.Foundry, SpriteName.WarsBuild_Foundry, CraftBuildingLib.WorkerHut);
            new BuildOption(BuildAndExpandType.Chemist, TerrainMainType.Building, (int)TerrainBuildingType.Chemist, SpriteName.WarsBuild_Chemist, CraftBuildingLib.Chemist);
            new BuildOption(BuildAndExpandType.Gunmaker, TerrainMainType.Building, (int)TerrainBuildingType.Gunmaker, SpriteName.WarsBuild_Chemist, CraftBuildingLib.Gunmaker);
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

        public static bool TryAutoBuild(IntVector2 subTilePos, TerrainMainType mainType, int terrainSubType)
        {
            SubTile subTile;
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out subTile))
            {
                if (CanAutoBuildHere(ref subTile))
                {
                    subTile.SetType(mainType, terrainSubType, 1);
                    EditSubTile edit = new EditSubTile(subTilePos, subTile, true, false, false);
                    edit.Submit();
                    //DssRef.world.subTileGrid.Set(subTilePos, subTile);
                    return true;
                }
            }

            return false;
        }
    }


}
