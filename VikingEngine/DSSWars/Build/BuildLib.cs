using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

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
        Brewery,
        Cook,
        CoalPit,
        WorkBench,
        Smith,
        Carpenter,
        WheatFarm,
        LinenFarm,
        PigPen,
        HenPen,
        Statue_ThePlayer,
        Pavement,
        PavementFlower,

        NUM_NONE,
    }
    static class BuildLib
    {
       
        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static readonly List<BuildAndExpandType> AvailableBuildTypes = new List<BuildAndExpandType> {
            BuildAndExpandType.WorkerHuts,
            BuildAndExpandType.Barracks,
            BuildAndExpandType.Nobelhouse,
            BuildAndExpandType.Postal,
            BuildAndExpandType.Recruitment,
            BuildAndExpandType.Brewery,
            BuildAndExpandType.Cook,
            BuildAndExpandType.CoalPit,
            BuildAndExpandType.WorkBench,
            BuildAndExpandType.Smith,
            BuildAndExpandType.Carpenter,
            BuildAndExpandType.PigPen,
            BuildAndExpandType.HenPen,
            BuildAndExpandType.WheatFarm,
            BuildAndExpandType.LinenFarm,
            BuildAndExpandType.Pavement,
            BuildAndExpandType.PavementFlower,
            BuildAndExpandType.Statue_ThePlayer
        };
       

        public static void Init()
        {
            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, SpriteName.WarsBuild_WorkerHuts, ResourceLib.CraftWorkerHut);
            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, SpriteName.WarsBuild_Postal, ResourceLib.CraftPostal);
            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, SpriteName.WarsBuild_Recruitment, ResourceLib.CraftRecruitment);
            new BuildOption(BuildAndExpandType.Barracks, TerrainMainType.Building, (int)TerrainBuildingType.Barracks, SpriteName.WarsBuild_Barracks, ResourceLib.CraftBarracks);
            new BuildOption(BuildAndExpandType.Nobelhouse, TerrainMainType.Building, (int)TerrainBuildingType.Nobelhouse, SpriteName.WarsBuild_Nobelhouse, ResourceLib.CraftNobelHouse);
            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, SpriteName.WarsBuild_Tavern, ResourceLib.CraftTavern);
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

            new BuildOption(BuildAndExpandType.Pavement, TerrainMainType.Decor, (int)TerrainDecorType.Pavement, SpriteName.WarsBuild_Pavement, ResourceLib.CraftPavement);
            new BuildOption(BuildAndExpandType.PavementFlower, TerrainMainType.Decor, (int)TerrainDecorType.PavementFlower, SpriteName.WarsBuild_PavementFlowers, ResourceLib.CraftPavementFlower);
            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, SpriteName.WarsBuild_Statue, ResourceLib.CraftStatue);

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
                    DssRef.world.subTileGrid.Set(subTilePos, subTile);
                    return true;
                }
            }

            return false;
        }
    }


}
