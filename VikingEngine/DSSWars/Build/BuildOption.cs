using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;

namespace VikingEngine.DSSWars.Build
{
    class BuildOption
    {   
        public BuildAndExpandType buildType;
        public CraftBlueprint blueprint;
        public CraftBlueprint altBlueprint = null;
        public TerrainMainType mainType;
        public int subType;
        public SpriteName sprite;
        public bool uniqueBuilding = false;
        public bool canAutoBuild;
        public BuildCategoryTab buildCategory;
        public MapPaintToolCategory paintToolCategory;
        public float buildTimeSec;

        public BuildFilterTag filterTag1;
        public BuildFilterTag filterTag2;
        public BuildFilterTag filterTag3;


        public BuildOption(BuildAndExpandType buildType, TerrainMainType mainType, int subType, SpriteName sprite, CraftBlueprint blueprint, 
            bool canAutoBuild, BuildCategoryTab buildCategory,
            BuildFilterTag filterTag1,
            BuildFilterTag filterTag2,
            BuildFilterTag filterTag3,
            MapPaintToolCategory paintToolCategory, float buildTimeSec)
        {
            this.canAutoBuild = canAutoBuild;
            this.sprite = sprite;
            this.buildType = buildType;
            this.blueprint = blueprint;
            this.mainType = mainType;
            this.subType = subType;
            //this.experienceType = experienceType;
            this.buildCategory = buildCategory;
            this.filterTag1 = filterTag1;
            this.filterTag2 = filterTag2;
            this.filterTag3 = filterTag3;
            BuildLib.BuildOptions[(int)buildType] = this;
            this.paintToolCategory = paintToolCategory;
            this.buildTimeSec = buildTimeSec;
        }

        public bool Contains(BuildFilterTag filterTag)
        { 
            return filterTag == filterTag1 ||  filterTag == filterTag2 || filterTag ==filterTag3;
        }

        public WorkExperienceType experienceType() 
        {
            return blueprint.experienceType;
        }
        public string Label()
        {
            IconName.Building(buildType, out _, out string name);//LangLib.TerrainName(mainType, subType);
            return name;
        }
        public string Description()
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    return LangLib.BuildingDescription((TerrainBuildingType)subType);
                case TerrainMainType.Foil:
                    return DssRef.lang.BuildingType_Farm_Description;
                case TerrainMainType.Decor:
                case TerrainMainType.Road:
                    return DssRef.lang.BuildingType_Decor_Description;
                case TerrainMainType.Wall:
                    switch ((TerrainWallType)subType)
                    {
                        case TerrainWallType.StoneHouse:
                            return DssRef.lang.Defence_WallDescription_Movement;
                        default:
                            return DssRef.lang.Defence_WallDescription_Movement + " " + DssRef.lang.Defence_WallDescription_GuardPost;
                    }
                    
            }

            return TextLib.Error;
        }

        public void destroy_async(City city, IntVector2 subPos)
        {
            var sutile = new SubTile();
            city.executeBuildEffectsOnCity(false, subPos, ref sutile, mainType, subType);

        }

        public bool execute_async(City city, IntVector2 subPos, ref SubTile subTile, bool upgrade, bool payResources = true)
        {
            
            if (city.executeBuildEffectsOnCity(true, subPos, ref subTile, mainType, subType))
            {
                if (payResources)
                {
                    CraftBlueprint bp;
                    if (altBlueprint != null && altBlueprint.hasResources(city))
                    {
                        bp = altBlueprint;
                    }
                    else
                    {
                        bp = blueprint;
                    }

                    if (upgrade)
                    {
                        bp.payResources(city);
                    }
                    else
                    {
                        bp.payResources_BuildAndUpgrade(city);
                    }
                }
                subTile.SetType(mainType, subType, 1);
                return true;

            }
            return false;
        }

        public bool availableBlueprintResources(City city)
        {
            return blueprint.hasResources(city) || (altBlueprint != null && altBlueprint.hasResources(city));
        }
    }
}
