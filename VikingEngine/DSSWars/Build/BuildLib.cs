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
        Tavern,
        Brewery,
        Cook,
        Smith,
        WheatFarms,
        LinnenFarms,
        PigPen,
        HenPen,
        Statue_ThePlayer,
        
        NUM_NONE,
    }
    static class BuildLib
    {
        //public static readonly BuildOption BuildWorkerHut = new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.WorkerHut, ResourceLib.CraftWorkerHut);

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];
        public static readonly BuildAndExpandType[] AvailableBuildTypes = {
            BuildAndExpandType.WorkerHuts,
            BuildAndExpandType.Barracks,
            BuildAndExpandType.Postal,
            BuildAndExpandType.Recruitment,
            BuildAndExpandType.Tavern,
            BuildAndExpandType.Brewery,
            BuildAndExpandType.Cook,
            BuildAndExpandType.Smith,
            BuildAndExpandType.PigPen,
            BuildAndExpandType.HenPen,
            BuildAndExpandType.WheatFarms,
            BuildAndExpandType.LinnenFarms,
            BuildAndExpandType.Statue_ThePlayer
        };
        //{
        //    BuildWorkerHut,
        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Postal, ResourceLib.CraftPostal),
        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Recruitment, ResourceLib.CraftRecruitment),

        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Barracks, ResourceLib.CraftBarracks),
        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Tavern, ResourceLib.CraftTavern),

        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.PigPen, ResourceLib.CraftPigPen),
        //    new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.HenPen, ResourceLib.CraftHenPen),
        //    new BuildOption(BuildOptionType.Farm, (int)TerrainSubFoilType.WheatFarm, ResourceLib.CraftFarm),
        //    new BuildOption(BuildOptionType.Farm, (int)TerrainSubFoilType.LinnenFarm, ResourceLib.CraftFarm),
        //};

        public static void Init()
        {
            new BuildOption(BuildAndExpandType.WorkerHuts, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, ResourceLib.CraftWorkerHut);
            new BuildOption(BuildAndExpandType.Postal, TerrainMainType.Building, (int)TerrainBuildingType.Postal, ResourceLib.CraftPostal);
            new BuildOption(BuildAndExpandType.Recruitment, TerrainMainType.Building, (int)TerrainBuildingType.Recruitment, ResourceLib.CraftRecruitment);            
            new BuildOption(BuildAndExpandType.Barracks, TerrainMainType.Building, (int)TerrainBuildingType.Barracks, ResourceLib.CraftBarracks);
            new BuildOption(BuildAndExpandType.Tavern, TerrainMainType.Building, (int)TerrainBuildingType.Tavern, ResourceLib.CraftTavern);
            new BuildOption(BuildAndExpandType.Brewery, TerrainMainType.Building, (int)TerrainBuildingType.Brewery, ResourceLib.CraftBrewery);
            new BuildOption(BuildAndExpandType.PigPen, TerrainMainType.Building, (int)TerrainBuildingType.PigPen, ResourceLib.CraftPigPen);
            new BuildOption(BuildAndExpandType.HenPen, TerrainMainType.Building, (int)TerrainBuildingType.HenPen, ResourceLib.CraftHenPen);
            new BuildOption(BuildAndExpandType.Cook, TerrainMainType.Building, (int)TerrainBuildingType.Work_Cook, ResourceLib.CraftCook);
            new BuildOption(BuildAndExpandType.Smith, TerrainMainType.Building, (int)TerrainBuildingType.Work_Smith, ResourceLib.CraftSmith);

            new BuildOption(BuildAndExpandType.WheatFarms, TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, ResourceLib.CraftFarm);
            new BuildOption(BuildAndExpandType.LinnenFarms, TerrainMainType.Foil, (int)TerrainSubFoilType.LinnenFarm, ResourceLib.CraftFarm);

            new BuildOption(BuildAndExpandType.Statue_ThePlayer, TerrainMainType.Decor, (int)TerrainDecorType.Statue_ThePlayer, ResourceLib.CraftStatue);

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
