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
            return LangLib.TerrainName(mainType, subType);
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

            //switch (mainType)
            //{
            //    case TerrainMainType.Building:
            //        {
            //            switch ((TerrainBuildingType)subType)
            //            {

            //                case TerrainBuildingType.WorkerHut:
            //                    city.onWorkHutBuild(false, false);
            //                    break;
            //                case TerrainBuildingType.WorkerHutLarge:
            //                    city.onWorkHutBuild(false, true);
            //                    break;

            //                case TerrainBuildingType.ServiceMenHouse_small:
            //                    city.onServiceHouseBuild(false, false);
            //                    break;
            //                case TerrainBuildingType.ServiceMenHouse_Large:
            //                    city.onServiceHouseBuild(false, true);
            //                    break;

            //                case TerrainBuildingType.GuardHouse_Small:
            //                    city.onGuardHouseBuild(false, false);
            //                    break;
            //                case TerrainBuildingType.GuardHouse_Large:
            //                    city.onGuardHouseBuild(false, true);
            //                    break;

            //                case TerrainBuildingType.SoldierBarracks:
            //                case TerrainBuildingType.ArcherBarracks:
            //                case TerrainBuildingType.WarmachineBarracks:
            //                case TerrainBuildingType.KnightsBarracks:
            //                case TerrainBuildingType.GunBarracks:
            //                    city.destroyBarracks(subPos);
            //                    break;

            //                case TerrainBuildingType.Recruitment:
            //                case TerrainBuildingType.RecruitmentLevel2:
            //                case TerrainBuildingType.RecruitmentLevel3:
            //                case TerrainBuildingType.Postal:
            //                case TerrainBuildingType.PostalLevel2:
            //                case TerrainBuildingType.PostalLevel3:
            //                case TerrainBuildingType.GoldDeliveryLevel1:
            //                case TerrainBuildingType.GoldDeliveryLevel2:
            //                case TerrainBuildingType.GoldDeliveryLevel3:
            //                    city.destroyDelivery(subPos);
            //                    break;

            //                case TerrainBuildingType.School:
            //                    city.destroySchool(subPos);
            //                    break;
            //                case TerrainBuildingType.ResearchCenter:
            //                case TerrainBuildingType.BookPress:
            //                    city.destroyResearchBuilding(subPos);
            //                    break;
            //            }
            //        }
            //        break;
            //    case TerrainMainType.Wall:
            //        city.destroyDefenceBuilding_async(subPos);
            //        break;
            //}
        }

        public bool execute_async(City city, IntVector2 subPos, ref SubTile subTile, bool upgrade)
        {
            //TODO handle upgrades


            //switch (mainType)
            //{
            //    case TerrainMainType.Building:
            //        {
            //            switch ((TerrainBuildingType)subType)
            //            {
            //                case TerrainBuildingType.Logistics:
            //                    if (city.buildingStructure.buildingLevel_logistics > 0)
            //                    {
            //                        //Already built
            //                        return false;
            //                    }

            //                    if (city.CanBuildLogistics(2))
            //                    {
            //                        subTile.terrainAmount = 2;
            //                    }
            //                    city.buildingStructure.buildingLevel_logistics = subTile.terrainAmount;
            //                    break;


            //                case TerrainBuildingType.WorkerHut:
            //                    city.onWorkHutBuild(true, false);
            //                    break;
            //                case TerrainBuildingType.WorkerHutLarge:
            //                    city.onWorkHutBuild(true, true);
            //                    break;

            //                case TerrainBuildingType.ServiceMenHouse_small:
            //                    city.onServiceHouseBuild(true, false);
            //                    break;
            //                case TerrainBuildingType.ServiceMenHouse_Large:
            //                    city.onServiceHouseBuild(true, true);
            //                    break;

            //                case TerrainBuildingType.GuardHouse_Small:
            //                    city.onGuardHouseBuild(true, false);
            //                    break;
            //                case TerrainBuildingType.GuardHouse_Large:
            //                    city.onGuardHouseBuild(true, true);
            //                    break;



            //                case TerrainBuildingType.SoldierBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.SoldierBarracks));
            //                    break;
            //                case TerrainBuildingType.ArcherBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.ArcherBarracks));
            //                    break;
            //                case TerrainBuildingType.WarmachineBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.WarmachineBarracks));
            //                    break;
            //                case TerrainBuildingType.KnightsBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.KnightsBarracks));
            //                    break;
            //                case TerrainBuildingType.GunBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.GunBarracks));
            //                    break;
            //                case TerrainBuildingType.CannonBarracks:
            //                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(city.addBarracks, subPos, Build.BuildAndExpandType.CannonBarracks));
            //                    break;

            //                case TerrainBuildingType.Postal:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Resource));
            //                    break;
            //                case TerrainBuildingType.PostalLevel2:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Resource));
            //                    break;
            //                case TerrainBuildingType.PostalLevel3:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Resource));
            //                    break;

            //                case TerrainBuildingType.Recruitment:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Men));
            //                    break;
            //                case TerrainBuildingType.RecruitmentLevel2:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Men));
            //                    break;
            //                case TerrainBuildingType.RecruitmentLevel3:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Men));
            //                    break;

            //                case TerrainBuildingType.GoldDeliveryLevel1:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Gold));
            //                    break;
            //                case TerrainBuildingType.GoldDeliveryLevel2:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Gold));
            //                    break;
            //                case TerrainBuildingType.GoldDeliveryLevel3:
            //                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(city.addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Gold));
            //                    break;


            //                case TerrainBuildingType.School:
            //                    Ref.update.AddSyncAction(new SyncAction1Arg<IntVector2>(city.addSchool, subPos));
            //                    break;

            //                case TerrainBuildingType.ResearchCenter:
            //                    city.addResearchBuilding(subPos, true);
            //                    break;
            //                case TerrainBuildingType.BookPress:
            //                    city.addResearchBuilding(subPos, false);
            //                    break;

            //            }
            //        }
            //        break;

            //    case TerrainMainType.Wall:
            //        city.addDefenceBuilding_async(subPos);
            //        break;

            //    case TerrainMainType.Decor:
            //        bool statue = false;
            //        switch ((TerrainDecorType)subType)
            //        {
            //            case TerrainDecorType.Statue_ThePlayer:
            //                statue = true;
            //                break;
            //        }

            //        var cityPlayer = city.GetPlayer();
            //        if (cityPlayer.IsLocalPlayer())
            //        {
            //            city.GetPlayer().GetLocalPlayer().statistics.onDecorBuild_async(statue);
            //        }
            //        break;
            //}
            if (city.executeBuildEffectsOnCity(true, subPos, ref subTile, mainType, subType))
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
