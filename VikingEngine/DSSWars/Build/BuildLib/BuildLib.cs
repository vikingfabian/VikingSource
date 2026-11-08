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
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.PJ.GameState;
using VikingEngine.ToGG;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;
using VikingEngine.ToGG.HeroQuest.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Build
{
    
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
            BuildAndExpandType.PavementFountain,
            BuildAndExpandType.Statue_Leader,
        };

        public static List<BuildAndExpandType> ManorUnlockBuildings = new List<BuildAndExpandType>
        {
            BuildAndExpandType.WheatFarm,
            BuildAndExpandType.HenPen,

            BuildAndExpandType.Cook,
        };

        public static BuildOption[] BuildOptions = new BuildOption[(int)BuildAndExpandType.NUM_NONE];

        public static Dictionary<int, BuildAndExpandType> TerrainToBuildType = new Dictionary<int, BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);


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

        public static bool CanAutoBuildHere(ref MapTile1_1 subTile)
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

        public static bool TryAutoBuild(PFaction pfaction, IntVector2 subTilePos, TerrainMainType mainType, int terrainSubType, int amount)
        {
            MapTile1_1 subTile;
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out subTile))
            {
                if (CanAutoBuildHere(ref subTile))
                {
                    subTile.SetType(mainType, terrainSubType, amount);
                    EditSubTile edit = new EditSubTile(pfaction, true, subTilePos, subTile, true, true, false);
                    edit.Submit();
                    return true;
                }
            }

            return false;
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

        public static int TerrainHash(TerrainMainType main, int subType)
        {
            return (int)main * 1000 + subType;
        }
    }

   
}
