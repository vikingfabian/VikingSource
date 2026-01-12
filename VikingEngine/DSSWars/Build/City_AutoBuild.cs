using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.GameState;

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
        
        public bool automateCity = false;
        public AutomationFocus automationFocus = AutomationFocus.NoFocus;
        public WarAutoQuality warAutoQuality = WarAutoQuality.Medium;
        public WarAutoWeaponType warAutoWeaponType = WarAutoWeaponType.Mix;

        public ExportAutoType exportAutoType = ExportAutoType.Resources;

        public bool autoExport_weapons = false;
        public GameTimeStamp nextAutoConscriptTime = GameTimeStamp.None;

        int currentWallRadius = 0;

        protected void workAutoBuild(/*bool fuelSafeGuard, bool rawFoodSafeGuard*/)
        {

            var player = GetPlayer();

            //EMPTY
            //if (checkAutoBuildAvailable())
            //{
               

                AutoBuildList.Clear();
                int safeGuardBuildCount = 1;

                BuildAndExpandType safeGuardBuild = BuildAndExpandType.NUM_NONE;
                //if (fuelSafeGuard && CityStructure.WorkInstance.fuelSpots < 4)
                //{
                //    ++CityStructure.WorkInstance.fuelSpots;
                //    safeGuardBuild = BuildAndExpandType.RapeSeedFarm;
                //    safeGuardBuildCount = 2;
                //}
                //else if (rawFoodSafeGuard && CityStructure.WorkInstance.foodspots < 4)
                //{
                //    ++CityStructure.WorkInstance.foodspots;
                //    safeGuardBuild = BuildAndExpandType.OrchardApple;
                //    safeGuardBuildCount = 4;
                //}

                if (buildingStructure.Orchard_count + buildingStructure.WheatFarm_count + buildingStructure.HenPen_count < 2)
                {
                    safeGuardBuild = BuildAndExpandType.OrchardApple;
                    safeGuardBuildCount = 2;
                }
                else if (cityType == CityType.Campsite && buildingStructure.TentHuts_count < 2)
                {
                    safeGuardBuild = BuildAndExpandType.WorkerTent;
                }
                else if (cityType == CityType.Campsite && buildingStructure.LinenFarm_count < 2)
                {
                    safeGuardBuild = BuildAndExpandType.LinenFarm;
                }
                else if (cityType == CityType.Campsite && buildingStructure.Orchard_count < 6)
                {
                    safeGuardBuild = BuildAndExpandType.OrchardApple;
                    safeGuardBuildCount = 2;
                }
                else if (buildingStructure.WorkBench_count < 1)
                {
                    safeGuardBuild = BuildAndExpandType.WorkBench;
                }
                else if (cityType == CityType.Campsite && TryGetFaction(out var faction) && faction.cities.Count == 1 && 
                    buildingStructure.SoldierBarracks_count + buildingStructure.ArcherBarracks_count < 1)
                {
                    if (freeServiceMen.amount < 1)
                    {
                        safeGuardBuild = BuildAndExpandType.ServiceHouse_Small;
                    }
                    else if (GetGroupedResource(EntityComponent.CityResoureIndex.sharpstick).amount >
                        GetGroupedResource(EntityComponent.CityResoureIndex.ThrowingSpear).amount)
                    {
                        safeGuardBuild = BuildAndExpandType.SoldierBarracks;
                    }
                    else
                    {
                        safeGuardBuild = BuildAndExpandType.ArcherBarracks;
                    }
                }
                else if (CityStructure.WorkInstance.fuelSpots < 2)
                {
                    ++CityStructure.WorkInstance.fuelSpots;
                    safeGuardBuild = BuildAndExpandType.RapeSeedFarm;
                    safeGuardBuildCount = 2;
                }

            if (safeGuardBuild != BuildAndExpandType.NUM_NONE)
                {
                    for (int i = 0; i < safeGuardBuildCount; i++)
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
                    autoAdjustResourcesToCitySize(false);
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

            //int buildCount = lib.SmallestValue(AutoBuildList.Count, CityStructure.WorkInstance.EmptyLand.Count);

            for (int i = 0; i < AutoBuildList.Count; ++i)
            {
                var buildType = AutoBuildList[i];

#if DEBUG
                if (buildType == BuildAndExpandType.OrchardApple)
                {
                    lib.DoNothing();

                }
#endif

                bool foundPos = false;
                IntVector2 pos = IntVector2.Zero;

                if (this.buildingStructure.getCount(buildType) > 0)
                {
                    var prevPos = CityStructure.WorkInstance.buildingPosition.getPos(buildType);
                    if (prevPos.X > 0)
                    {
                        foundPos = findAdjacentFreeSpot(Auto_EdgeRandomizer, prevPos, ref pos);
                    }
                }

                if (!foundPos && CityStructure.WorkInstance.NextEmptyLand(this, Ref.peRnd.Int(32), out pos))//.EmptyLand[i];
                {
                    foundPos = true;
                }

                if (foundPos)
                {
                    if (BuildLib.BuildOptions[(int)buildType].availableBlueprintResources_ignorewater(this) &&
                        work_isFreeTile(pos))
                    {
                        //workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.autoBuild.value, 0, 0));
                        workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.Get(WorkPriorityType.autoBuild).value, 0, 0));
                    }
                }
                else
                {
                    break;
                }
            }

            if (safeGuardBuild == BuildAndExpandType.NUM_NONE)
            {
                int freeWalls = buildingStructure.wallCount - groups.Count;
                if (freeWalls < 2 && currentWallRadius < 32)
                {
                    if (currentWallRadius == 0)
                    {
                        currentWallRadius = Ref.rnd.Int(4, 8);
                    }

                    int addCount = 2;
                    BuildAndExpandType wallType, towerType;
                    switch (cityType)
                    {
                        default:
                            wallType = BuildAndExpandType.DirtWall;
                            towerType = BuildAndExpandType.DirtTower;
                            break;
                        case CityType.Town:
                            wallType = BuildAndExpandType.WoodWall;
                            towerType = BuildAndExpandType.WoodTower;
                            break;
                        case CityType.Capital:
                            wallType = BuildAndExpandType.StoneWall;
                            towerType = BuildAndExpandType.StoneTower;
                            break;
                    }

                    if (BuildLib.BuildOptions[(int)wallType].availableBlueprintResources(this))
                    {
                        ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(cityHallSubtilePos, currentWallRadius));
                        while (loop.Next())
                        {
                            if (!(loop.AtBottom && loop.AtCenterX) && //place for opening
                                MayAutoBuildHere(loop.Position) && work_isFreeTile(loop.Position))
                            {                                
                                workQue.Add(new WorkQueMember(WorkType.Build, (int)(loop.AtCorner? towerType : wallType), 0, loop.Position, workTemplate.Get(WorkPriorityType.autoBuild).value, 0, 0));
                                addCount--;
                                if (addCount <= 0)
                                {
                                    break;
                                }
                            }
                        }

                        if (addCount > 0)
                        {
                            currentWallRadius += Ref.rnd.Int(5, 9);
                        }
                            //    workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.Get(WorkPriorityType.autoBuild).value, 0, 0));
                            //}
                        }
                    //else
                    //{
                    //    workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.Get(WorkPriorityType.autoBuild).value, 0, 0));
                    //    break;
                    //}
                }
            }
            //}

        }

        bool findAdjacentFreeSpot(ForXYEdgeLoopRandomPicker edgeRandomizer, IntVector2 center, ref IntVector2 result)
        {
            for (int r = 1; r <= 2; r++)
            {
                edgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(center, r));

                while (edgeRandomizer.Next())
                {
                    if (MayAutoBuildHere(edgeRandomizer.Position))
                    {
                        result = edgeRandomizer.Position;
                        return true;
                    }
                }
            }
            return false;
        }

        private void commit_automateCityBuilding()
        {
            auto_updateWorkPrio();

            AutoBuild_available.Clear();
            AutoBuild_RandomBuild.clear();

            BuildLib.AvailableBuildTypes(AutoBuild_available, this);

            int pickCount = lib.SmallestValue(AutoBuild_available.Count, 4);

            auto_addBuildingType(BuildAndExpandType.Logistics);

            switch (automationFocus)
            {
                case AutomationFocus.Food:
                    auto_addBuildingType(BuildAndExpandType.OrchardApple);
                    auto_addBuildingType(BuildAndExpandType.WheatFarm);
                    auto_addBuildingType(BuildAndExpandType.Cook);
                    auto_addBuildingType(BuildAndExpandType.CoalPit);
                    auto_addBuildingType(BuildAndExpandType.Postal);
                    break;
                case AutomationFocus.Grow:

                    auto_addBuildingType(BuildAndExpandType.WorkerTent);
                    auto_addBuildingType(BuildAndExpandType.WorkerHut);
                    auto_addBuildingType(BuildAndExpandType.OrchardApple);
                    auto_addBuildingType(BuildAndExpandType.WheatFarm);
                    auto_addBuildingType(BuildAndExpandType.WorkBench);
                    auto_addBuildingType(BuildAndExpandType.ServiceHouse_Small);
                    if (homeUsers() >= WorkersMaxLimit - 10)
                    {
                        upgradeCityHall();
                    }
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

        public void autoAdjustResourcesToCitySize(bool prepareSettle)
        {
            int multi = automationFocus == AutomationFocus.Food ? 5 : 1;

            ref var res_food = ref GetRefGroupedResource(CityResoureIndex.food);
            ref var res_rawFood = ref GetRefGroupedResource(CityResoureIndex.rawFood);
            ref var res_fuel = ref GetRefGroupedResource(CityResoureIndex.fuel);

            res_food.stockPileLimit = Bound.Min(workForce.amount / 100 * 100 + 200, DssConst.Logistics1FoodStorage) * multi;
            res_rawFood.stockPileLimit = (workForce.amount / 300 * 100 + 100) * multi;
            res_fuel.stockPileLimit = res_rawFood.stockPileLimit;


            ref var res_wood = ref GetRefGroupedResource(CityResoureIndex.wood);
            ref var res_skin = ref GetRefGroupedResource(CityResoureIndex.skinLinnen);

            res_wood.stockPileLimit = WorldData.DefaultBuffer_Wood;
            res_skin.stockPileLimit = WorldData.DefaultBuffer_SkinLinnen;

            if (prepareSettle)
            {
                res_food.stockPileLimit += Conscript.ConscriptDataLib.CraftSettlerFood;
                res_wood.stockPileLimit += Conscript.ConscriptDataLib.CraftSettlerWood;
                res_skin.stockPileLimit += Conscript.ConscriptDataLib.CraftSettlerSkinLinen;
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
                    case BuildAndExpandType.WorkerTent:
                        bBuild = WorkersMaxLimit > HousingCount_Workers;
                        maxCount = 20;
                        chance = automationFocus == AutomationFocus.Grow ? 4000 : 200;
                        repeat = 4;
                        break;

                    case BuildAndExpandType.WorkerHutLarge:
                    case BuildAndExpandType.WorkerHut:
                        bBuild = WorkersMaxLimit > HousingCount_Workers;
                        maxCount = 100;
                        chance = automationFocus == AutomationFocus.Grow ? 4000 : 200;
                        repeat = 4;
                        break;

                    case BuildAndExpandType.ArcherBarracks:
                        maxCount = 3;
                        chance = automationFocus == AutomationFocus.Military ? 150 : 100;
                        break;

                    case BuildAndExpandType.SoldierBarracks:
                    case BuildAndExpandType.WarmachineBarracks:
                    //case BuildAndExpandType.KnightsBarracks:
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

                    case BuildAndExpandType.Cook:
                        maxCount = 8;
                        break;

                    case BuildAndExpandType.CoalPit:
                    case BuildAndExpandType.WorkBench:
                        chance = 200;
                        break;

                    case BuildAndExpandType.OrchardApple:
                    case BuildAndExpandType.OrchidBanana:
                        chance = automationFocus == AutomationFocus.Grow ? 2000 : 1000;
                        maxCount = 200;
                        break;

                    case BuildAndExpandType.WheatFarm:
                    case BuildAndExpandType.LinenFarm:
                    case BuildAndExpandType.HenPen:
                    case BuildAndExpandType.PigPen:
                    case BuildAndExpandType.RapeSeedFarm:
                    case BuildAndExpandType.HempFarm:
                        chance = automationFocus == AutomationFocus.Grow ? 2000 : 1000;
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

                    case BuildAndExpandType.Logistics:
                        chance = automationFocus == AutomationFocus.Grow ? 300 : 150;
                        maxCount = 1;
                        break;
                    case BuildAndExpandType.ManorLord:
                        if (DssRef.state.hasManorLords)
                        {
                            chance = 100;
                            maxCount = 1;
                        }
                        else
                        {
                            bBuild = false;
                        }
                        
                        break;

                    case BuildAndExpandType.School:
                        chance = 5;
                        maxCount = 2;
                        break;
                }

                if (bBuild)
                {
                    var opt = BuildLib.BuildOptions[(int)buildType];
                    if (opt.blueprint.hasResources_buildAndUpgrade_IgnoreWater(this))
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
            // Basic Resources & Movement
            workTemplate.setWorkPrio(WorkPriorityType.move, 3);
            workTemplate.setWorkPrio(WorkPriorityType.wood, 2);
            workTemplate.setWorkPrio(WorkPriorityType.stone, 2);

            // Crafting: Consumables
            workTemplate.setWorkPrio(WorkPriorityType.craftFuel, 4);
            workTemplate.setWorkPrio(WorkPriorityType.craftFood, 4);
            workTemplate.setWorkPrio(WorkPriorityType.craftBeer, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftCoolingFluid, 1);

            // Smelting: Base Metals
            // Mapped 'craft_iron' to 'smeltIron' based on context (vs bloomery/cast iron)
            workTemplate.setWorkPrio(WorkPriorityType.smeltIron, 2);
            workTemplate.setWorkPrio(WorkPriorityType.smeltTin, 1);
            workTemplate.setWorkPrio(WorkPriorityType.smeltCopper, 1); // Mapped from 'craft_cupper'
            workTemplate.setWorkPrio(WorkPriorityType.smeltLead, 1);
            workTemplate.setWorkPrio(WorkPriorityType.smeltSilver, 1);

            // Crafting: Alloys & Advanced Metals
            workTemplate.setWorkPrio(WorkPriorityType.craftBronze, 2);
            workTemplate.setWorkPrio(WorkPriorityType.craftCastIron, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftBloomeryIron, 2);
            workTemplate.setWorkPrio(WorkPriorityType.craftSteel, 3);
            workTemplate.setWorkPrio(WorkPriorityType.craftMithril, 4);

            // Crafting: Construction & Tools
            workTemplate.setWorkPrio(WorkPriorityType.craftPalisade, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftToolkit, 1);

            // Crafting: Wagons
            // Mapped 'wagonlight' -> 2Wheel, 'wagonheavy' -> 4Wheel
            workTemplate.setWorkPrio(WorkPriorityType.craftWagon2Wheel, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftWagon4Wheel, 1);

            // Crafting: Ammo
            workTemplate.setWorkPrio(WorkPriorityType.craftBlackPowder, 2);
            workTemplate.setWorkPrio(WorkPriorityType.craftGunPowder, 3);
            workTemplate.setWorkPrio(WorkPriorityType.craftBullet, 3);

            // Farming & Gathering
            workTemplate.setWorkPrio(WorkPriorityType.farmFood, 4);
            workTemplate.setWorkPrio(WorkPriorityType.farmRawFood, 3);
            workTemplate.setWorkPrio(WorkPriorityType.farmfuel, 3);
            workTemplate.setWorkPrio(WorkPriorityType.farmlinen, 3);
            workTemplate.setWorkPrio(WorkPriorityType.bogiron, 1);

            // Mining
            workTemplate.setWorkPrio(WorkPriorityType.miningIron, 3);
            workTemplate.setWorkPrio(WorkPriorityType.miningTin, 2);
            workTemplate.setWorkPrio(WorkPriorityType.miningCopper, 2);
            workTemplate.setWorkPrio(WorkPriorityType.miningLead, 1);
            workTemplate.setWorkPrio(WorkPriorityType.miningSilver, 2);
            workTemplate.setWorkPrio(WorkPriorityType.miningGold, 2);
            workTemplate.setWorkPrio(WorkPriorityType.miningMithril, 3);
            workTemplate.setWorkPrio(WorkPriorityType.miningSulfur, 1);
            workTemplate.setWorkPrio(WorkPriorityType.miningCoal, 1);

            // Building
            workTemplate.setWorkPrio(WorkPriorityType.autoBuild, 1);

            // Coinage (Using the new 3-argument overload for Full Stock)
            workTemplate.setWorkPrio(WorkPriorityType.smeltGold, 2);
            workTemplate.setWorkPrio(WorkPriorityType.coinmaker_copper, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.coinmaker_bronze, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.coinmaker_silver, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.coinmaker_mithril, 1, true);
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
                            case TerrainBuildingType.ManorLord:
                                if (build)
                                {
                                    if (buildingStructure.manorLord)
                                    {
                                        //Already built
                                        return false;
                                    }
                                    buildingStructure.manorLord = true;
                                }
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

    enum AutomationFocus
    { 
        NO_AUTO,
        NoFocus,
        Grow,
        Export,
        Military,
        Food,
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
