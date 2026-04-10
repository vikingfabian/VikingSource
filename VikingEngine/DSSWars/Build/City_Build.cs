using System;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.GameObject
{
    //AUTO BUILD
    partial class City
    {
        public bool executeBuildEffectsOnCity(bool build, IntVector2 subPos, ref SubTile subTile, TerrainMainType mainType, int subType)
        {
            if (!build)
            {
                lib.DoNothing();
            }
            switch (mainType)
            {
                case TerrainMainType.Building:
                    {
                        switch ((TerrainBuildingType)subType)
                        {
                            case TerrainBuildingType.Logistics:
                                if (build)
                                {
                                    if (buildingStructure.buildingLevel_logistics > 0)
                                    {
                                        //Already built
                                        return false;
                                    }

                                    if (CanBuildLogistics(2))
                                    {
                                        subTile.terrainAmount = 2;
                                    }
                                    buildingStructure.buildingLevel_logistics = subTile.terrainAmount;
                                }
                                break;

                            case TerrainBuildingType.ManorLord:
                                if (build && buildingStructure.manorLord)
                                {
                                    //Already built
                                    return false;
                                }
                                buildingStructure.manorLord = build;
                                break;
                            case TerrainBuildingType.GreatHall:
                                if (build && buildingStructure.greatHall)
                                {
                                    //Already built
                                    return false;
                                }
                                buildingStructure.greatHall = build;
                                break;

                            case TerrainBuildingType.WorkerTent:
                                onWorkHutBuild(build, DssConst.HousingCount_WorkerTent);
                                break;
                            case TerrainBuildingType.WorkerHut:
                                onWorkHutBuild(build, DssConst.HousingCount_WorkerHut);
                                break;
                            case TerrainBuildingType.WorkerHutLarge:
                                onWorkHutBuild(build, DssConst.HousingCount_WorkerHutLarge);
                                break;

                            case TerrainBuildingType.ServiceMenHouse_small:
                                onServiceHouseBuild(build, false);
                                break;
                            case TerrainBuildingType.ServiceMenHouse_Large:
                                onServiceHouseBuild(build, true);
                                break;

                            case TerrainBuildingType.GuardHouse_Small:
                                onGuardHouseBuild(build, false);
                                break;
                            case TerrainBuildingType.GuardHouse_Large:
                                onGuardHouseBuild(build, true);
                                break;

                            case TerrainBuildingType.Nobelhouse:
                                onNobelHouseBuild(build, DssConst.NobelHouseMenCount);
                                break;

                            case TerrainBuildingType.MaterialStorage:
                                addStorageBuilding(StorageType.MaterialStorage, build);
                                break;
                            case TerrainBuildingType.FoodStorage:
                                addStorageBuilding(StorageType.FoodStorage, build);
                                break;
                            case TerrainBuildingType.WeaponStorage:
                                addStorageBuilding(StorageType.WeaponStorage, build);
                                break;
                            case TerrainBuildingType.ArmorStorage:
                                addStorageBuilding(StorageType.ArmorStorage, build);
                                break;
                            case TerrainBuildingType.AnimalStorage:
                                addStorageBuilding(StorageType.ArmorStorage, build);
                                break;

                            case TerrainBuildingType.SoldierBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.SoldierBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;
                            case TerrainBuildingType.ArcherBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.ArcherBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;
                            case TerrainBuildingType.WarmachineBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.WarmachineBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;
                            //case TerrainBuildingType.KnightsBarracks:
                            //    if (build)
                            //    {
                            //        Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.KnightsBarracks));
                            //    }
                            //    else
                            //    {
                            //        destroyBarracks(subPos);
                            //    }
                            //    break;
                            case TerrainBuildingType.GunBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.GunBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;
                            case TerrainBuildingType.CannonBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.CannonBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;

                            case TerrainBuildingType.Postal:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Resource));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.PostalLevel2:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Resource));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.PostalLevel3:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Resource));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;

                            case TerrainBuildingType.Recruitment:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Men));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.RecruitmentLevel2:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Men));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.RecruitmentLevel3:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Men));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;

                            case TerrainBuildingType.GoldDeliveryLevel1:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 1, DeliveryStatus.DeliveryType_Gold));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.GoldDeliveryLevel2:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 2, DeliveryStatus.DeliveryType_Gold));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;
                            case TerrainBuildingType.GoldDeliveryLevel3:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction3Arg<IntVector2, int, ItemResourceType>(addDelivery, subPos, 3, DeliveryStatus.DeliveryType_Gold));
                                }
                                else
                                {
                                    destroyDelivery(subPos);
                                }
                                break;


                            case TerrainBuildingType.School:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction1Arg<IntVector2>(addSchool, subPos));
                                }
                                else
                                {
                                    destroySchool(subPos);
                                }
                                break;

                            case TerrainBuildingType.ResearchCenter:
                                if (build)
                                {
                                    addResearchBuilding(subPos, true);
                                }
                                else
                                {
                                    destroyResearchBuilding(subPos);
                                }
                                break;
                            case TerrainBuildingType.BookPress:
                                if (build)
                                {
                                    addResearchBuilding(subPos, false);
                                }
                                else
                                {
                                    destroyResearchBuilding(subPos);
                                }
                                break;

                            case TerrainBuildingType.HenPen:
                            case TerrainBuildingType.PigPen:
                            case TerrainBuildingType.OxenPen:
                            case TerrainBuildingType.KineOxenPen:

                            case TerrainBuildingType.DogCage:
                            case TerrainBuildingType.HoundCage:

                            case TerrainBuildingType.PonyPen:
                            case TerrainBuildingType.HorsePen:
                            case TerrainBuildingType.WarHorsePen:
                            case TerrainBuildingType.DraftHorsePen:
                            case TerrainBuildingType.WildPigPen:
                            case TerrainBuildingType.WildHogPen:
                            case TerrainBuildingType.WarHogPen:
                            case TerrainBuildingType.StagHogPen:
                            case TerrainBuildingType.WolfCage:
                            case TerrainBuildingType.WargCage:
                            case TerrainBuildingType.AlphaWargCage:
                            case TerrainBuildingType.WildCatCage:
                            case TerrainBuildingType.LionCage:
                            case TerrainBuildingType.WarLionCage:
                            case TerrainBuildingType.ElephantCage:
                            case TerrainBuildingType.WarElephantCage:
                            case TerrainBuildingType.OliphantCage:
                                var upkeep = Build.BuildLib.Get(mainType, subType).upkeep;
                                if (upkeep.type == ItemResourceType.RawFood_Group)
                                {
                                    PenFoodUpkeep_minute += lib.BoolToLeftRight(build) * upkeep.amount;
                                }
                                break;

                            case TerrainBuildingType.Cesspit:
                                if (build)
                                {
                                    addCesspit(subPos);
                                }
                                else
                                {
                                    destroyCesspit(subPos);
                                }
                                break;

                        }
                    }
                    break;

                case TerrainMainType.Wall:
                    if (build)
                    {
                        bool tower = false;
                        switch ((TerrainWallType)subType)
                        {
                            case TerrainWallType.DirtTower:
                            case TerrainWallType.WoodTower:
                            case TerrainWallType.StoneTower:
                                tower = true;
                                break;
                        }
                        addDefenceBuilding_async(subPos, tower);
                    }
                    else
                    {
                        destroyDefenceBuilding_async(subPos);
                    }
                    break;

                case TerrainMainType.Decor:
                    if (build)
                    {
                        var cityPlayer = GetPlayer();
                        if (cityPlayer.IsLocalPlayer())
                        {
                            cityPlayer.GetLocalPlayer().statistics.onDecorBuild_async((TerrainDecorType)subType);
                        }
                    }
                    break;
            }

            return true;
        }
        public bool MayAutoBuildHere(IntVector2 subTilePos)
        {
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out var subtile))
            {
                switch (subtile.mainTerrain)
                {
                    case TerrainMainType.Destroyed:
                    case TerrainMainType.DefaultLand:
                        var tile = DssRef.world.tileGrid.Get(WP.SubtileToTilePos(subTilePos));
                        return tile.MayBuild() && tile.CityIndex == myIndex;

                }
            }
            return false;
        }
    }
}
