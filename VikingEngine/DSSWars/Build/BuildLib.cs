using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Build
{
    static class BuildLib
    {
        public static readonly BuildOption BuildWorkerHut = new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.WorkerHut, ResourceLib.CraftWorkerHut);

        public static readonly BuildOption[] BuildOptions =
            {
                BuildWorkerHut,
                new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Barracks, ResourceLib.CraftBarracks),
                new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.Tavern, ResourceLib.CraftTavern),
                
                new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.PigPen, ResourceLib.CraftPigPen),
                new BuildOption(BuildOptionType.Building, (int)TerrainBuildingType.HenPen, ResourceLib.CraftHenPen),
                new BuildOption(BuildOptionType.Farm, (int)TerrainSubFoilType.WheatFarm, ResourceLib.CraftFarm),
                new BuildOption(BuildOptionType.Farm, (int)TerrainSubFoilType.LinnenFarm, ResourceLib.CraftFarm),
            };
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
