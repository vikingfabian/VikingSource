using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Build
{
    class BuildOption
    {
        //public BuildOptionType type;
        public int subType;
        public BuildAndExpandType buildType;
        public CraftBlueprint blueprint;
        public TerrainMainType mainType;
        //static int NextIndex = 0;
        //public int index;

        public BuildOption(BuildAndExpandType buildType, TerrainMainType mainType, int subType, CraftBlueprint blueprint)
        {
            this.buildType = buildType;
            this.blueprint = blueprint;
            this.mainType = mainType;
            this.subType = subType;

            BuildLib.BuildOptions[(int)buildType] = this;
        }
        public string Label()
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    return ((TerrainBuildingType)subType).ToString();
                case TerrainMainType.Foil:
                    return ((TerrainSubFoilType)subType).ToString();
            }

            return TextLib.Error;
        }
        public string Description()
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    return LangLib.BuildingDescription((TerrainBuildingType)subType);
                case TerrainMainType.Foil:
                    return "Grow a resource";
            }

            return TextLib.Error;
        }

        public void execute_async(City city, IntVector2 subPos, ref SubTile subTile)
        {
            //switch (type)
            //{
            //    case BuildOptionType.Building:
                    subTile.SetType(mainType, subType, 1);

            if (mainType == TerrainMainType.Building)
            {
                switch ((TerrainBuildingType)subType)
                {
                    case TerrainBuildingType.WorkerHut:
                        city.onWorkHutBuild();
                        break;

                    case TerrainBuildingType.Barracks:
                        Ref.update.AddSyncAction(new SyncAction1Arg<IntVector2>(city.addBarracks, subPos));
                        break;

                    case TerrainBuildingType.Postal:
                        Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, bool>(city.addDelivery, subPos, false));
                        break;

                    case TerrainBuildingType.Recruitment:
                        Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, bool>(city.addDelivery, subPos, true));
                        break;
                }
            }
            //    case BuildOptionType.Farm:
            //        subTile.SetType(TerrainMainType.Foil, subType, 1);
            //        break;
            //}
            blueprint.craft(city);
        }
    }

    //enum BuildOptionType
    //{
    //    Building,
    //    Farm,
    //}
}
