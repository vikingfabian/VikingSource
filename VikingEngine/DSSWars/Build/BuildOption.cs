using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;

namespace VikingEngine.DSSWars.Build
{
    class BuildOption
    {   
        public BuildAndExpandType buildType;
        public CraftBlueprint blueprint;
        public CraftBlueprint altBlueprint = null;
        
        public MapTile1_1 terrainType;
        public SpriteName icon;
        public string name, description;
        public bool uniqueBuilding = false;
        public bool canAutoBuild;
        public BuildCategoryTab buildCategory;
        public MapPaintToolCategory paintToolCategory;
        public float buildTimeSec;

        public BuildFilterTag filterTag1;
        public BuildFilterTag filterTag2;
        public BuildFilterTag filterTag3;

        public ItemResource upkeep = ItemResource.Empty;

        public BuildInAreaOptions buildInArea = BuildInAreaOptions.WithInOnly;
        public SpecialBuildRequirement specialBuildRequirement = SpecialBuildRequirement.None;
        public BuildingGroundRequirement groundRequirement;

        public BuildOption(BuildAndExpandType buildType, TerrainMainType mainType, int subType, BuildingGroundRequirement groundRequirement, 
            SpriteName sprite, 
            string name, string description,
            CraftBlueprint blueprint, 
            bool canAutoBuild, BuildCategoryTab buildCategory,
            BuildFilterTag filterTag1,
            BuildFilterTag filterTag2,
            BuildFilterTag filterTag3,
            MapPaintToolCategory paintToolCategory, float buildTimeSec)
        {
            this.name = name;
            this.description = description;

            this.groundRequirement = groundRequirement;
            this.canAutoBuild = canAutoBuild;
            this.icon = sprite;
            this.buildType = buildType;
            this.blueprint = blueprint;
            terrainType = new MapTile1_1(mainType, subType) { terrainAmount = 1 };
            //this.experienceType = experienceType;
            this.buildCategory = buildCategory;
            this.filterTag1 = filterTag1;
            this.filterTag2 = filterTag2;
            this.filterTag3 = filterTag3;
            
            this.paintToolCategory = paintToolCategory;
            this.buildTimeSec = buildTimeSec;

            BuildLib.BuildOptions[(int)buildType] = this;
            BuildLib.TerrainToBuildType.Add(BuildLib.TerrainHash(mainType, subType), buildType);
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
            return name != null? name : $"Missing name for {buildType}";
        }
        //public string Description()
        //{
        //    switch (terrainType.mainTerrain)
        //    {
        //        case TerrainMainType.Building:
        //            return LangLib.BuildingDescription((TerrainBuildingType)terrainType.subTerrain);
        //        case TerrainMainType.Foil:
        //            return DssRef.lang.BuildingType_Farm_Description;
        //        case TerrainMainType.Decor:
        //        case TerrainMainType.Road:
        //            return DssRef.lang.BuildingType_Decor_Description;
        //        case TerrainMainType.Wall:
        //            switch ((TerrainWallType)terrainType.subTerrain)
        //            {
        //                case TerrainWallType.StoneHouse:
        //                    return DssRef.lang.Defence_WallDescription_Movement;
        //                default:
        //                    return DssRef.lang.Defence_WallDescription_Movement + " " + DssRef.lang.Defence_WallDescription_GuardPost;
        //            }
                    
        //    }

        //    return TextLib.Error;
        //}

        public void destroy_async(City city, IntVector2 subPos)
        {
            var sutile = new MapTile1_1();
            city.executeBuildEffectsOnCity(false, subPos, ref sutile, terrainType.mainTerrain, terrainType.subTerrain);

        }

        public bool execute_async(City city, IntVector2 subPos, ref MapTile1_1 subTile, out bool bTerraform, bool upgrade, bool payResources = true)
        {
            bTerraform = false;
            if (city.executeBuildEffectsOnCity(true, subPos, ref subTile, terrainType.mainTerrain, terrainType.subTerrain))
            {
                var ground = DssRef.world.subTileGrid.Get(subPos).groundType;
                GroundPropertiesLib.MayBuild_GroundCheck(ground, groundRequirement, out var mustTerraform);
                if (payResources)
                {
                    CraftBlueprint bp;
                    if (altBlueprint != null && altBlueprint.available(city))
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
                        city.unreserveItems(bp);
                    }

                    if (mustTerraform.HasValue)
                    {
                        var terraformBp = GroundPropertiesLib.Get(mustTerraform.Value).TerraformToCost;
                        if (terraformBp != null)
                        {
                            terraformBp.payResources_BuildAndUpgrade(city);
                            city.unreserveItems(terraformBp);
                        }
                    }
                }
                subTile.SetType(terrainType.mainTerrain, terrainType.subTerrain, 1);
                if (mustTerraform.HasValue)
                {
                    subTile.groundType = mustTerraform.Value;
                    bTerraform = true;
                }
                return true;

            }
            return false;
        }

        public bool availableBlueprintResources(City city)
        {
            return blueprint.available(city) || (altBlueprint != null && altBlueprint.available(city));
        }

        public bool availableBlueprintResources_ignorewater(City city)
        {
            return blueprint.hasResources_ignorewater(city) || (altBlueprint != null && altBlueprint.hasResources_ignorewater(city));
        }

        public MayBuildResult mayBuild_regionCheck(PCity tilePCity, IntVector2 mapTilePos, PFaction pFaction, City fromCity)
        {
            bool insideRegion = tilePCity.cityIndex == fromCity.myIndex;

            if (insideRegion)
            {
                if (buildInArea.WithInBorders)
                {
                    return MayBuildResult.Yes;
                }
                else
                {
                    return MayBuildResult.No_InsideRegion;
                }
            }
            if (tilePCity.HasValue())
            {
                City tilecity = tilePCity.City();
                if (tilecity.pfaction == pFaction)
                {
                    //Expecting only players to do this
                    if (pFaction.TryGetLocalPlayer(out var player))
                    {
                        List<BuildAndExpandType> available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
                        AvailableBuildTypes.Available(available, tilecity, false);

                        if (!available.Contains(player.gameControls.build.placeBuildingType))
                        {
                            return MayBuildResult.Yes_ChangeCity;
                        }
                        else
                        {
                            return MayBuildResult.No_OutsideRegion;
                        }
                    }
                    //return MayBuildResult.Yes_ChangeCity;
                }
                else
                {
                    return MayBuildResult.No_OutsideRegion;
                }
            }
            if (DssRef.state.GodPowers())
            { 
                return MayBuildResult.Yes;
            }
            if (buildInArea.OutsideBorders)
            {
                if ((mapTilePos - fromCity.maptilePos).Length() < fromCity.buildOutsideRegionRadius)
                {
                    return MayBuildResult.Yes;
                }
            }
            return MayBuildResult.No_OutsideRegion;
        }
    }

    enum SpecialBuildRequirement
    { 
        None,
        FoodStorage,
        MaxSoldierQuarters, 
        MaxExpansions,
    }
}
