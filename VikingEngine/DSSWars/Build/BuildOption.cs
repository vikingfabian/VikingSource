﻿using Microsoft.CodeAnalysis.CSharp;
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
        public BuildOptionType type;
        public int subType;
        public CraftBlueprint blueprint;
        static int NextIndex = 0;
        public int index;

        public BuildOption(BuildOptionType type, int subType, CraftBlueprint blueprint)
        {
            index = NextIndex;
            ++NextIndex;

            this.type = type;
            this.subType = subType;
            this.blueprint = blueprint;
        }
        public string Label()
        {
            switch (type)
            {
                case BuildOptionType.Building:
                    return ((TerrainBuildingType)subType).ToString();
                case BuildOptionType.Farm:
                    return ((TerrainSubFoilType)subType).ToString();
            }

            return TextLib.Error;
        }
        public string Description()
        {
            switch (type)
            {
                case BuildOptionType.Building:
                    return LangLib.BuildingDescription((TerrainBuildingType)subType);
                case BuildOptionType.Farm:
                    return "Grow a resource";
            }

            return TextLib.Error;
        }

        public void execute_async(City city, IntVector2 subPos, ref SubTile subTile)
        {
            switch (type)
            {
                case BuildOptionType.Building:
                    subTile.SetType(TerrainMainType.Building, subType, 1);

                    switch ((TerrainBuildingType)subType)
                    {
                        case TerrainBuildingType.WorkerHut:
                            city.onWorkHutBuild();
                            break;

                        case TerrainBuildingType.Barracks:
                            Ref.update.AddSyncAction(new SyncAction1Arg<IntVector2>(city.addBarracks, subPos));
                            //BarracksStatus consriptProfile = new BarracksStatus()
                            //{
                            //    idAndPosition = conv.IntVector2ToInt(subPos),
                            //};

                            //city.barracks.Add(consriptProfile);
                            break;
                    }
                    break;
                case BuildOptionType.Farm:
                    subTile.SetType(TerrainMainType.Foil, subType, 1);
                    break;
            }
            blueprint.craft(city);
        }
    }

    enum BuildOptionType
    {
        Building,
        Farm,
    }
}