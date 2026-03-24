
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
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.GameState;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.GO;

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
            else if (terrainStructure.resourceCount_wood <= 2 && GetGroupedResource(CityResoureIndex.wood).amount <= 10)
            {
                safeGuardBuild = BuildAndExpandType.TreeSeedlingHard;
            }
            else if (buildingStructure.LinenFarm_count < 2)
            {
                safeGuardBuild = BuildAndExpandType.LinenFarm;
                safeGuardBuildCount = 2;
            }
            else if (buildingStructure.Orchard_count < 6)
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

            AutoUpgradeCityHall();
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

        private void AutoUpgradeCityHall()
        {
            if (homeUsers() >= WorkersMaxLimit - 10 || cityType == CityType.Campsite)
            {
                double upgradeChance;
                switch (cityType)
                {
                    default:
                    case CityType.Campsite:
                        upgradeChance = 1;
                        break;
                    case CityType.Village:
                        upgradeChance = 0.2;
                        break;
                    case CityType.Town:
                        upgradeChance = 0.05;
                        break;
                    case CityType.Capital:
                        upgradeChance = 0.0;
                        break;

                }
                if (automationFocus == AutomationFocus.Grow)
                {
                    upgradeChance += 0.5;
                }
                if (Ref.rnd.Chance(upgradeChance))
                {
                    upgradeCityHall();
                }
            }
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

            BuildLib.AvailableBuildTypes(AutoBuild_available, this, true);

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
                    //if (homeUsers() >= WorkersMaxLimit - 10)
                    //{
                    //    upgradeCityHall();
                    //}
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
                    case BuildAndExpandType.ImmigrationTent:
                        maxCount = 2;
                        chance = 10;
                        break;

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
                        maxCount = 2;
                        chance = automationFocus == AutomationFocus.Military ? 100 : 40;
                        break;

                    case BuildAndExpandType.WarmachineBarracks:
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
                        bBuild = GetGroupedResource(CityResoureIndex.rawFood).amount > 50 &&
                            workTemplate.Get(WorkPriorityType.craftFood).value > WorkTemplate.NoPrio;
                        maxCount = 8;
                        break;

                    case BuildAndExpandType.Dryer:
                        maxCount = 2;
                        bBuild = haveResourcesToCraftItem_Bp(WorkPriorityType.craftConservedFood, CraftResourceLib.ConservedFood_Dried);
                        break;

                    case BuildAndExpandType.Smoker:
                        maxCount = 2;
                        bBuild = haveResourcesToCraftItem_Bp(WorkPriorityType.craftConservedFood, CraftResourceLib.ConservedFood_Smoked);
                        break;

                    case BuildAndExpandType.WorkBench:
                        chance = 200;
                        break;

                    case BuildAndExpandType.OrchardApple:
                    case BuildAndExpandType.OrchidBanana:
                        chance = automationFocus == AutomationFocus.Grow ? 2000 : 1000;
                        maxCount = cityType < CityType.Town ? 60 : 200;
                        break;

                    case BuildAndExpandType.WheatFarm:
                    case BuildAndExpandType.LinenFarm:
                    case BuildAndExpandType.HempFarm:
                        chance = automationFocus == AutomationFocus.Grow ? 200 : 100;
                        maxCount = 24;
                        break;

                    case BuildAndExpandType.RapeSeedFarm:
                        chance = automationFocus == AutomationFocus.Grow ? 50 : 20;
                        maxCount = 8;
                        break;

                    case BuildAndExpandType.Postal:
                    case BuildAndExpandType.PostalLevel2:
                    case BuildAndExpandType.PostalLevel3:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 60;
                            maxCount = 10;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 4;
                        }
                        break;

                    case BuildAndExpandType.Recruitment:
                    case BuildAndExpandType.RecruitmentLevel2:
                    case BuildAndExpandType.RecruitmentLevel3:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 60;
                            maxCount = 2;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 1;
                        }
                        break;

                    case BuildAndExpandType.CoalPit:
                        int fuelUsingCount = buildingStructure.Foundry_count + buildingStructure.Smelter_count + buildingStructure.Cook_count;
                        bBuild = fuelUsingCount > 2;
                        chance = 10 + fuelUsingCount * 10;
                        break;

                    case BuildAndExpandType.Carpenter:
                        maxCount = 2;
                        bBuild = haveResourcesToCraft(buildingStructure.Carpenter_count, maxCount, CraftList.CarpenterCraftTypes);
                        break;

                    case BuildAndExpandType.Gunmaker:
                        maxCount = 2;
                        bBuild = haveResourcesToCraft(buildingStructure.Gunmaker_count, maxCount, CraftList.GunmakerCraftTypes);
                        break;

                    case BuildAndExpandType.Foundry:
                        chance = 20;
                        maxCount = 2;
                        bBuild = haveResourcesToCraft(buildingStructure.Foundry_count, maxCount, CraftList.FoundryCraftTypes);
                        break;

                    case BuildAndExpandType.Chemist:
                        maxCount = 2;
                        bBuild = haveResourcesToCraft(buildingStructure.Chemist_count, maxCount, CraftList.ChemistCraftTypes);
                        break;

                    case BuildAndExpandType.Smelter:
                        bBuild = haveResourcesToCraft(buildingStructure.Smelter_count, maxCount, CraftList.SmelterCraftTypes);
                        break;

                    case BuildAndExpandType.Smith:
                        bBuild = haveResourcesToCraft(buildingStructure.Smith_count, maxCount, CraftList.SmithCraftTypes);
                        break;

                    case BuildAndExpandType.Armory:
                        maxCount = 2;
                        bBuild = haveResourcesToCraft(buildingStructure.Armory_count, maxCount, CraftList.ArmoryCraftTypes);
                        break;

                    case BuildAndExpandType.ShieldMaker:
                        chance = 20;
                        maxCount = 1;
                        bBuild = haveResourcesToCraft(buildingStructure.ShieldMaker_count, maxCount, CraftList.ShieldMakerCraftTypes);
                        break;

                    case BuildAndExpandType.Pottery:
                        bBuild = haveResourcesToCraft(buildingStructure.Pottery_count, maxCount, CraftList.PotteryCraftTypes);
                        maxCount = 2;
                        break;

                    case BuildAndExpandType.Butcher:
                        bBuild = haveAnimalsToSlaughter();//haveResourcesToCraft(buildingStructure.Butcher_count, maxCount, CraftList.ButcherAnimalTypes);
                        maxCount = 1;
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

                    case BuildAndExpandType.GreatHall:
                        bBuild = buildingStructure.AllBarracksCount() >= 2;
                        chance = 20;
                        maxCount = 1;
                        break;

                    case BuildAndExpandType.Bank:
                        maxCount = 1;
                        break;
                    case BuildAndExpandType.CoinMinter:
                        maxCount = 1;
                        break;



                    case BuildAndExpandType.Nobelhouse:
                        chance = 20;
                        bBuild = buildingStructure.AllBarracksCount() >= 4 && Money.ToGold(previousIncome_copp) > 10;
                        break;

                    case BuildAndExpandType.School:
                        chance = 5;
                        maxCount = 2;
                        break;

                    case BuildAndExpandType.MaterialStorage:
                    case BuildAndExpandType.FoodStorage:
                    case BuildAndExpandType.WeaponStorage:
                    case BuildAndExpandType.ArmorStorage:
                    case BuildAndExpandType.AnimalStorage:
                        chance = 10;
                        maxCount = 3;
                        break;

                    case BuildAndExpandType.Cesspit:
                        chance = 5;
                        maxCount = 1;
                        break;

                    case BuildAndExpandType.WaterResovoir:
                        chance = 10;
                        break;


                    //case BuildAndExpandType.HenPen:
                    //case BuildAndExpandType.PigPen:
                        
                    //    break;
                       

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

            bool haveResourcesToCraft(int hasCount, int maxCount, ItemResourceType[] craftList)
            {
                if (hasCount >= maxCount)
                {
                    return false;
                }
                

                foreach (ItemResourceType item in craftList)
                {
                    if (haveResourcesToCraftItem(item))
                    {
                        return true;
                    }
                }

                return false;
            }

            bool haveResourcesToCraftItem(ItemResourceType item)
            {
                const int MinCraftCount = 30;

                ItemProperties properties = ItemPropertyColl.Get(item);

                if (properties.work != WorkPriorityType.NUM_NONE)
                {
                    if (workTemplate.Get(properties.work).value == WorkTemplate.NoPrio)
                    {
                        return false;
                    }
                }

                int available = properties.bp1.canCraftCount(this);

                if (available >= MinCraftCount)
                {
                    return true;
                }

                if (properties.bp2 != null)
                {
                    available = properties.bp2.canCraftCount(this);
                    if (available >= MinCraftCount)
                    {
                        return true;
                    }
                }

                return false;
            }

            bool haveResourcesToCraftItem_Bp(WorkPriorityType workPriority, CraftBlueprint blueprint)
            {
                const int MinCraftCount = 30;

                if (workTemplate.Get(workPriority).value == WorkTemplate.NoPrio)
                {
                    return false;
                }                

                int available = blueprint.canCraftCount(this);

                if (available >= MinCraftCount)
                {
                    return true;
                }

                return false;
            }

            bool haveAnimalsToSlaughter()
            {
                foreach (var craft in CraftList.ButcherAnimalCraftTypes)
                {
                    if (craft.hasFullStock(this))
                    {
                        return true;
                    }
                }

                return false;
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
            workTemplate.setWorkPrio(WorkPriorityType.craftBrick, 2);

            // Crafting: Consumables
            workTemplate.setWorkPrio(WorkPriorityType.craftFuel, 4);
            workTemplate.setWorkPrio(WorkPriorityType.craftFood, 4);
            workTemplate.setWorkPrio(WorkPriorityType.craftConservedFood, 2);
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
            workTemplate.setWorkPrio(WorkPriorityType.craftPalisade, 0);
            workTemplate.setWorkPrio(WorkPriorityType.craftToolkit, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftContainer, 1);

            // Crafting: Wagons
            // Mapped 'wagonlight' -> 2Wheel, 'wagonheavy' -> 4Wheel
            workTemplate.setWorkPrio(WorkPriorityType.craftWagon2Wheel, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftWagon4Wheel, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftWagonClosed, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftWagonIron, 1);
            workTemplate.setWorkPrio(WorkPriorityType.craftWagonSteel, 1);            

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
            workTemplate.setWorkPrio(WorkPriorityType.collectClay, 3);

            // Mining
            workTemplate.setWorkPrio(WorkPriorityType.miningSalt, 2);
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

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterHen, 2, false);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterPig, 2, false);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterOxen, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterKineOxen, 1, true);

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterPony, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterHorse, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWarHorse, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterDraftHorse, 1, true);

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWildPig, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWildHog, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWarHog, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterStagHog, 1, true);

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWolf, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWarg, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterAlphaWarg, 1, true);

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWildCat, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterLion, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWarLion, 1, true);

            workTemplate.setWorkPrio(WorkPriorityType.SlaughterElephant, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterWarElephant, 1, true);
            workTemplate.setWorkPrio(WorkPriorityType.SlaughterOliphant, 1, true);
        }

        public bool AutomateCityProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                automateCity = value;
                if (automateCity == false)
                {
                    //Pull faction settings
                    var player = GetPlayer().GetLocalPlayer();
                    if (player != null)
                    {
                        DssRef.world.copyStockPile(player, player.faction, this, CopyPasteOption.FactionToCity, ResourceGroupType.NUM);
                        workTemplate.setAllToFollowFactionAndUpdate(this, player.faction.workTemplate);
                    }
                }
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return automateCity;
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
