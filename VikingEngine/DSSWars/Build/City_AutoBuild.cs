using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    //AUTO BUILD
    partial class City
    {
        static ForXYEdgeLoopRandomPicker Auto_EdgeRandomizer = new ForXYEdgeLoopRandomPicker();
        static ForXYEdgeLoopRandomPicker Casual_EdgeRandomizer = new ForXYEdgeLoopRandomPicker();
        static List<BuildAndExpandType> AutoBuildList = new List<BuildAndExpandType>(4);
        static RandomObjects_Int AutoBuild_RandomBuild = new RandomObjects_Int();
        static List<BuildAndExpandType> AutoBuild_available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
        //static List<BuildAndExpandType> AutoBuild_available_mustInclude = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

        public bool automateCity = false;
        public AutomationFocus automationFocus = AutomationFocus.NoFocus;
        public WarAutoQuality warAutoQuality = WarAutoQuality.Medium;
        public WarAutoWeaponType warAutoWeaponType = WarAutoWeaponType.Mix;

        public ExportAutoType exportAutoType = ExportAutoType.Resources;

        public bool autoExport_weapons = false;
        public GameTimeStamp nextAutoConscriptTime = GameTimeStamp.None;

        

        protected void workAutoBuild(bool fuelSafeGuard, bool rawFoodSafeGuard)
        {
            var player = GetPlayer();

            //EMPTY
            //if (checkAutoBuildAvailable())
            {
               

                AutoBuildList.Clear();

                BuildAndExpandType safeGuardBuild = BuildAndExpandType.NUM_NONE;
                if (fuelSafeGuard && CityStructure.WorkInstance.fuelSpots < 4)
                {
                    ++CityStructure.WorkInstance.fuelSpots;
                    safeGuardBuild = BuildAndExpandType.RapeSeedFarm;
                }
                else if (rawFoodSafeGuard && CityStructure.WorkInstance.foodspots < 4)
                {
                    ++CityStructure.WorkInstance.foodspots;
                    safeGuardBuild = BuildAndExpandType.WheatFarm;
                }

                if (safeGuardBuild != BuildAndExpandType.NUM_NONE)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        AutoBuildList.Add(safeGuardBuild);
                    }
                }
                else if (player.IsBot())
                {
                    var aiPlayer = player.GetAiPlayer();
                    automationFocus = AutomationFocus.NoFocus;

                    bool warCity = aiPlayer.IsWarBorderCity(this, aiPlayer.aggressionLevel < AbsPlayer.AggressionLevel2_RandomAttacks);
                    if (warCity)
                    {
                        automationFocus = AutomationFocus.Military;
                    }
                    commit_automateCityBuilding();

                }
                else if (automateCity)
                {
                    commit_automateCityBuilding();
                }
                else //Player default
                {

                    AutoExpandType(out bool work, out Build.BuildAndExpandType buildType);
                    if (work)
                    {
                        buildType = autoBuild_Farm ? autoExpandFarmType : Build.BuildAndExpandType.NUM_NONE;
                        
                        if (work && workForce.amount >= HousingCount_Workers)
                        {
                            buildType = BuildAndExpandType.WorkerHut;
                        }
                        
                        if (buildType != BuildAndExpandType.NUM_NONE)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                AutoBuildList.Add(buildType);
                            }
                        }
                    }
                }

                int buildCount = lib.SmallestValue(AutoBuildList.Count, CityStructure.WorkInstance.EmptyLand.Count);

                for (int i = 0; i < buildCount; ++i)
                {
                    var buildType = AutoBuildList[i];
                    
                    var pos = CityStructure.WorkInstance.EmptyLand[i];
                    if (this.buildingStructure.getCount(buildType) > 0)
                    {
                        var prevPos = CityStructure.WorkInstance.buildingPosition.getPos(buildType);
                        if (prevPos.X > 0)
                        {
                            findAdjacentFreeSpot(Auto_EdgeRandomizer, prevPos, ref pos);
                        }
                    }
                    
                    if (BuildLib.BuildOptions[(int)buildType].availableBlueprintResources(this) &&
                        work_isFreeTile(pos))
                    {
                        workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.autoBuild.value, 0, 0));
                    }
                    
                }
            }

            

            //bool checkAutoBuildAvailable()
            //{
            //    if (buildingStructure.buildingLevel_logistics < 2)
            //    {
            //        var p = player.GetLocalPlayer();
            //        if (p != null)
            //        {
            //            return p.orders.buildQueue(this) + 1 < MaxBuildQueue();
            //        }
            //    }
            //    return true;
            //}
        }

        void findAdjacentFreeSpot(ForXYEdgeLoopRandomPicker edgeRandomizer, IntVector2 center, ref IntVector2 result)
        {
            for (int r = 1; r <= 2; r++)
            {
                edgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(center, r));

                while (edgeRandomizer.Next())
                {
                    if (CityStructure.WorkInstance.MayAutoBuildHere(this, edgeRandomizer.Position))
                    {
                        result = edgeRandomizer.Position;
                        return;
                    }
                }
            }
        }

        private void commit_automateCityBuilding()
        {
            auto_updateWorkPrio();

            AutoBuild_available.Clear();
            AutoBuild_RandomBuild.clear();

            BuildLib.AvailableBuildTypes(AutoBuild_available, this);

            int pickCount = lib.SmallestValue(AutoBuild_available.Count, 4);

            switch (automationFocus)
            {
                case AutomationFocus.Food:
                    auto_addBuildingType(BuildAndExpandType.WheatFarm);
                    auto_addBuildingType(BuildAndExpandType.Cook);
                    auto_addBuildingType(BuildAndExpandType.CoalPit);
                    auto_addBuildingType(BuildAndExpandType.Postal);
                    break;
                case AutomationFocus.Grow:
                    auto_addBuildingType(BuildAndExpandType.WorkerHut);
                    auto_addBuildingType(BuildAndExpandType.WheatFarm);
                    auto_addBuildingType(BuildAndExpandType.WorkBench);
                    auto_addBuildingType(BuildAndExpandType.ServiceHouse_Small);
                    break;
                case AutomationFocus.Export:
                    auto_addBuildingType(BuildAndExpandType.Postal);
                    auto_addBuildingType(BuildAndExpandType.Recruitment);
                    break;
                case AutomationFocus.Military:
                    auto_addBuildingType(BuildAndExpandType.SoldierBarracks);
                    auto_addBuildingType(BuildAndExpandType.ArcherBarracks);
                    auto_addBuildingType(BuildAndExpandType.WarmachineBarracks);
                    auto_addBuildingType(BuildAndExpandType.GuardHouse_Small);
                    break;
            }

            while (AutoBuild_available.Count > 0 && AutoBuild_RandomBuild.members.Count < 8)
            {
                var buildType = arraylib.RandomListMemberPop(AutoBuild_available);
                auto_addBuildingType(buildType);
            }

            pickCount = lib.SmallestValue(AutoBuild_RandomBuild.members.Count, 2);

            for (int i = 0; i < pickCount; ++i)
            {
                AutoBuildList.Add((BuildAndExpandType)AutoBuild_RandomBuild.PullRandom());
            }
        }

        private void auto_addBuildingType(BuildAndExpandType buildType)
        {
            const int NoMaxLimit = 500;
            bool bBuild = true;
            int chance = 100;
            int maxCount = 4;
            int repeat = 1;

            if (BuildLib.BuildOptions[(int)buildType].canAutoBuild)
            {
                switch (buildType)
                {
                    case BuildAndExpandType.WorkerHutLarge:
                    case BuildAndExpandType.WorkerHut:
                        bBuild = WorkersMaxLimit < HousingCount_Workers;
                        maxCount = 100;
                        chance = 200;
                        repeat = 4;
                        break;

                    case BuildAndExpandType.SoldierBarracks:
                    case BuildAndExpandType.ArcherBarracks:
                    case BuildAndExpandType.WarmachineBarracks:
                    case BuildAndExpandType.KnightsBarracks:
                    case BuildAndExpandType.GunBarracks:
                    case BuildAndExpandType.CannonBarracks:
                        maxCount = 2;
                        chance = automationFocus == AutomationFocus.Military ? 100 : 5;
                        break;

                    case BuildAndExpandType.GuardHouse_Small:
                    case BuildAndExpandType.GuardHouse_Large:
                        chance = automationFocus == AutomationFocus.Military ? 300 : 50;
                        maxCount = NoMaxLimit;
                        bBuild = AvailableGuardHousing() < 10;
                        break;

                    case BuildAndExpandType.ServiceHouse_Small:
                    case BuildAndExpandType.ServiceHouse_Large:
                        chance = automationFocus == AutomationFocus.Grow ? 300 : 50;
                        maxCount = NoMaxLimit;
                        int goalNumber = 5;
                        if (cityType < CityType.Capital)
                        {
                            canUpgradeCityHall(out _, out _, out int nextUpgradeRequirement, out _);
                            goalNumber = lib.LargestValue(goalNumber, nextUpgradeRequirement);
                        }
                        bBuild = freeServiceMen.amount < goalNumber;
                        break;

                    case BuildAndExpandType.CoalPit:
                    case BuildAndExpandType.WorkBench:
                        chance = 200;
                        break;

                    case BuildAndExpandType.WheatFarm:
                    case BuildAndExpandType.LinenFarm:
                    case BuildAndExpandType.HenPen:
                    case BuildAndExpandType.PigPen:
                    case BuildAndExpandType.RapeSeedFarm:
                    case BuildAndExpandType.HempFarm:
                        chance = automationFocus == AutomationFocus.Grow ? 300 : 150;
                        maxCount = 24;
                        break;

                    case BuildAndExpandType.Postal:
                    case BuildAndExpandType.PostalLevel2:
                    case BuildAndExpandType.PostalLevel3:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 60;
                            maxCount = 24;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 8;
                        }
                        break;
                    case BuildAndExpandType.Recruitment:
                    case BuildAndExpandType.RecruitmentLevel2:
                    case BuildAndExpandType.RecruitmentLevel3:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 200;
                            maxCount = 12;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 4;
                        }
                        break;

                    case BuildAndExpandType.Foundry:
                        chance = 20;
                        maxCount = 2;
                        break;
                }

                if (bBuild)
                {
                    var opt = BuildLib.BuildOptions[(int)buildType];
                    if (opt.blueprint.hasResources_buildAndUpgrade(this))
                    {
                        int currentCount = this.buildingStructure.getCount(buildType);

                        if (currentCount == 0)
                        {
                            chance /= 4;
                        }

                        if (currentCount < maxCount)
                        {
                            repeat = Ref.peRnd.Int(repeat) + 1;
                            for (int i = 0; i < repeat; ++i)
                            {
                                AutoBuild_RandomBuild.AddItem((int)buildType, chance);
                            }
                        }
                    }
                }
            }
        }

        public void AutoExpandType(out bool work, out Build.BuildAndExpandType farm)
        {
            work = autoBuild_Work;

            if (buildingStructure.buildingLevel_logistics == 0)
            {
                farm = Build.BuildAndExpandType.NUM_NONE;
                return;
            }

            farm = autoBuild_Farm ? autoExpandFarmType : Build.BuildAndExpandType.NUM_NONE;
        }

        void auto_updateWorkPrio()
        {
            int weaponPrio = automationFocus== AutomationFocus.Military ? 2 : 1;

            workTemplate.move.set(3);
            workTemplate.wood.set(2);
            workTemplate.stone.set(2);
            workTemplate.craft_fuel.set(4);
            workTemplate.craft_food.set(4);
            workTemplate.craft_beer.set(1);
            workTemplate.craft_coolingfluid.set(1);

             workTemplate.craft_iron.set(1);
            workTemplate.craft_tin.set(1);
            workTemplate.craft_cupper.set(1);
            workTemplate.craft_lead.set(1);
            workTemplate.craft_silver.set(1);

            workTemplate.craft_bronze.set(1);
            workTemplate.craft_castiron.set(1);
            workTemplate.craft_bloomeryiron.set(1);
            workTemplate.craft_steel.set(1);
            workTemplate.craft_mithril.set(1);

            workTemplate.craft_palisade.set(1);
            workTemplate.craft_toolkit.set(1);
            workTemplate.craft_wagonlight.set(1);
            workTemplate.craft_wagonheavy.set(1);
            workTemplate.craft_blackpowder.set(1);
            workTemplate.craft_gunpowder.set(1);
            workTemplate.craft_bullet.set(1);

            workTemplate.craft_sharpstick.set(weaponPrio);
            workTemplate.craft_bronzesword.set(weaponPrio);
            workTemplate.craft_shortsword.set(weaponPrio);
            workTemplate.craft_sword.set(weaponPrio);
            workTemplate.craft_longsword.set(weaponPrio);
            workTemplate.craft_handspear.set(weaponPrio);
            workTemplate.craft_mithrilsword.set(weaponPrio);
            workTemplate.craft_warhammer.set(weaponPrio);
            workTemplate.craft_twohandsword.set(weaponPrio);
            workTemplate.craft_knightslance.set(weaponPrio);

            workTemplate.craft_slingshot.set(weaponPrio);
            workTemplate.craft_throwingspear.set(weaponPrio);
            workTemplate.craft_bow.set(weaponPrio);
            workTemplate.craft_longbow.set(weaponPrio);
            workTemplate.craft_crossbow.set(weaponPrio);
            workTemplate.craft_mithrilbow.set(weaponPrio);

            workTemplate.craft_handcannon.set(weaponPrio);
            workTemplate.craft_handculverin.set(weaponPrio);
            workTemplate.craft_rifle.set(weaponPrio);
            workTemplate.craft_blunderbus.set(weaponPrio);

            workTemplate.craft_ballista.set(weaponPrio);
            workTemplate.craft_manuballista.set(weaponPrio);
            workTemplate.craft_catapult.set(weaponPrio);
            workTemplate.craft_batteringram.set(weaponPrio);

            workTemplate.craft_siegecannonbronze.set(weaponPrio);
            workTemplate.craft_mancannonbronze.set(weaponPrio);
            workTemplate.craft_siegecannoniron.set(weaponPrio);
            workTemplate.craft_mancannoniron.set(weaponPrio);

            workTemplate.craft_paddedarmor.set(weaponPrio);
            workTemplate.craft_heavypaddedarmor.set(weaponPrio);
            workTemplate.craft_bronzearmor.set(weaponPrio);
            workTemplate.craft_mailarmor.set(weaponPrio);
            workTemplate.craft_heavymailarmor.set(weaponPrio);
            workTemplate.craft_platearmor.set(weaponPrio);
            workTemplate.craft_fullplatearmor.set(weaponPrio);
            workTemplate.craft_mithrilarmor.set(weaponPrio);

            workTemplate.farm_food.set(4);
            workTemplate.farm_fuel.set(3);
            workTemplate.farm_linen.set(weaponPrio);
            workTemplate.bogiron.set(1);
            workTemplate.mining_iron.set(3);
            workTemplate.mining_tin.set(1);
            workTemplate.mining_cupper.set(1);
            workTemplate.mining_lead.set(1);
            workTemplate.mining_silver.set(2);
            workTemplate.mining_gold.set(2);
            workTemplate.mining_mithril.set(2);
            workTemplate.mining_sulfur.set(1);
            workTemplate.mining_coal.set(1);

            workTemplate.autoBuild.set(1);

            workTemplate.smeltgold.set(4);
            workTemplate.coinmaker_copper.set(3);
            workTemplate.coinmaker_copper_fullStock = true;
            workTemplate.coinmaker_bronze.set(3);
            workTemplate.coinmaker_bronze_fullStock = true;
            workTemplate.coinmaker_silver.set(3);
            workTemplate.coinmaker_silver_fullStock = true;
            workTemplate.coinmaker_mithril.set(3);
            workTemplate.coinmaker_mithril_fullStock = true;


            AutoConscriptLib.WorkPriority(this, ref workTemplate);
        }

        public bool AutomateCityProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                automateCity = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return automateCity;
        }

        public bool executeBuildEffectsOnCity(bool build, IntVector2 subPos, ref SubTile subTile, TerrainMainType mainType, int subType)
        {
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


                            case TerrainBuildingType.WorkerHut:
                                onWorkHutBuild(build, false);
                                break;
                            case TerrainBuildingType.WorkerHutLarge:
                                onWorkHutBuild(build, true);
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
                            case TerrainBuildingType.KnightsBarracks:
                                if (build)
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<IntVector2, Build.BuildAndExpandType>(addBarracks, subPos, Build.BuildAndExpandType.KnightsBarracks));
                                }
                                else
                                {
                                    destroyBarracks(subPos);
                                }
                                break;
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
                        //bool statue = false;
                        //switch ((TerrainDecorType)subType)
                        //{
                        //    case TerrainDecorType.Statue_ThePlayer:
                        //        statue = true;
                        //        break;
                        //}

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
    }

    enum AutomationFocus
    { 
        NO_AUTO,
        NoFocus,
        Grow,
        Export,
        Military,
        Food,
        //LevelUp,
    }
    enum WarAutoQuality
    {
        Low,
        Medium,
        High,
        NUM
    }

    enum WarAutoWeaponType
    { 
        Mix,
        Melee,
        Ranged,
        Warmachine,
        NUM
    }

    enum ExportAutoType
    {
        Resources,
        //Food,
        Weapons,
        NUM
    }
}
