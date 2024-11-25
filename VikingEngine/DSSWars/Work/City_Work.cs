using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        static byte[] MaxSkill = new byte[(int)WorkExperienceType.NUM];

        public WorkTemplate workTemplate = new WorkTemplate();

        const int NoSubWork = -1;
        public const int WorkTeamSize = 6;
        TimeStamp previousWorkQueUpdate = TimeStamp.None;
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        bool starving = false;
        static List<int> idleWorkers = new List<int>(64);

        public ExperienceLevel topskill_Farm = 0;
        public ExperienceLevel topskill_AnimalCare = 0;
        public ExperienceLevel topskill_HouseBuilding = 0;
        public ExperienceLevel topskill_WoodCutter = 0;
        public ExperienceLevel topskill_StoneCutter = 0;
        public ExperienceLevel topskill_Mining = 0;
        public ExperienceLevel topskill_Transport = 0;
        public ExperienceLevel topskill_Cook = 0;
        public ExperienceLevel topskill_Fletcher = 0;
        public ExperienceLevel topskill_Smelting = 0;
        public ExperienceLevel topskill_Casting = 0;
        public ExperienceLevel topskill_CraftMetal = 0;
        public ExperienceLevel topskill_CraftArmor = 0;
        public ExperienceLevel topskill_CraftWeapon = 0;
        public ExperienceLevel topskill_CraftFuel = 0;
        public ExperienceLevel topskill_Chemistry = 0;

        public ExperenceOrDistancePrio experenceOrDistance = ExperenceOrDistancePrio.Mix;

        public void async_workUpdate()
        {
            CityStructure.WorkInstance.newCity = true;

            async_blackMarketUpdate();

            int workerStatusActiveCount = workerStatuses.Count;
            int deletedCount = 0;
            int idleCount = 0;
            IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
            IntVector2 maxpos = minpos;

            //res_water.clearOrders();
            //res_wood.clearOrders();
            //res_stone.clearOrders();
            //res_rawFood.clearOrders();
            //res_skinLinnen.clearOrders();
            //res_ironore.clearOrders();

            for (int i = 0; i < MaxSkill.Length; ++i)
            {
                MaxSkill[i] = 0;
            }

            for (int i = 0; i < workerStatuses.Count; i++)
            {
                var status = workerStatuses[i];

                if (status.xp1 > MaxSkill[(int)status.xpType1])
                {
                    MaxSkill[(int)status.xpType1] = status.xp1;
                }
                if (status.xp2 > MaxSkill[(int)status.xpType2])
                {
                    MaxSkill[(int)status.xpType2] = status.xp2;
                }
                if (status.xp3 > MaxSkill[(int)status.xpType3])
                {
                    MaxSkill[(int)status.xpType3] = status.xp3;
                }


                switch (status.work)
                {
                    case WorkType.IsDeleted:
                        ++deletedCount;
                        --workerStatusActiveCount;
                        break;

                    case WorkType.Starving:
                    case WorkType.Exit:
                        --workerStatusActiveCount;
                        break;

                    case WorkType.Idle: 
                        idleCount++; 
                        break;
                    default:
                        checkAvailable(status.work, status.workSubType);
                        break;
                    
                }

                IntVector2 pos = status.subTileEnd;
                if (pos.X < minpos.X)
                {
                    minpos.X = pos.X;
                }
                if (pos.X > maxpos.X)
                {
                    maxpos.X = pos.X;
                }

                if (pos.Y < minpos.Y)
                {
                    minpos.Y = pos.Y;
                }
                if (pos.Y > maxpos.Y)
                {
                    maxpos.Y = pos.Y;
                }
            }

            topskill_Farm = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.Farm]);
            topskill_AnimalCare = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.AnimalCare]);
            topskill_HouseBuilding = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.HouseBuilding]);
            topskill_WoodCutter = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.WoodCutter]);
            topskill_StoneCutter = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.StoneCutter]);
            topskill_Mining = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.Mining]);
            topskill_Transport = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.Transport]);
            topskill_Cook = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.Cook]);
            topskill_Fletcher = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.Fletcher]);
            topskill_CraftMetal = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftMetal]);
            topskill_CraftArmor = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftArmor]);
            topskill_CraftWeapon = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftWeapon]);
            topskill_CraftFuel = WorkLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftFuel]);

            cullingTopLeft = WP.SubtileToTilePos(minpos);
            cullingBottomRight = WP.SubtileToTilePos(maxpos);

            int workTeamCount = workForce.amount / WorkTeamSize;

            if (workerStatusActiveCount < workTeamCount)
            {
                int deletedIx = 0;
                int newWorkers = workTeamCount - workerStatusActiveCount;
                IntVector2 startPos = WP.ToSubTilePos_Centered(tilePos);
                for (int i = 0; i < newWorkers; i++)
                {
                    var newWorker = new WorkerStatus()
                    {
                        work = WorkType.Idle,
                        processTimeStartStampSec = Ref.TotalGameTimeSec,
                        energy = DssConst.Worker_MaxEnergy,
                        subTileEnd = startPos,
                        subTileStart = startPos,
                    };

                    if (DssRef.time.totalMinutes < 1)
                    {
                        newWorker.xpType1 = WorkExperienceType.Farm;
                        newWorker.xp1 = DssConst.WorkXpToLevel;
                    }

                    if (deletedCount > 0)
                    {
                        for (int di = deletedIx; di < workerStatuses.Count; ++di)
                        {
                            if (workerStatuses[di].work == WorkType.IsDeleted)
                            {
                                workerStatuses[di] = newWorker;
                                --deletedCount;
                                deletedIx = di-1;
                            }
                        }
                    }
                    else
                    {
                        workerStatuses.Add(newWorker);
                    }
                    ++idleCount;
                }
            }            

            if (idleCount > 0 && previousWorkQueUpdate.secPassed(10))
            {
                if (parentArrayIndex == -1 || debugTagged)
                {
                    lib.DoNothing();
                }

                CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                buildWorkQue2();
                //Last position = highest priority
                workQue.Sort((a, b) => a.priority.CompareTo(b.priority));

                previousWorkQueUpdate.setNow();
            }

            idleWorkers.Clear();

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatusActiveCount > workForce.amount)
                    {
                        --workerStatusActiveCount;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Exit, -1, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].carry.amount > 0)
                    {
                        CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        //status.createWorkOrder(WorkType.DropOff, -1, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        status.createWorkOrder(WorkType.DropOff, -1, WorkExperienceType.Transport, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0 && (res_food.amount > 0 || faction.gold > 0))
                    {
                        CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        //status.createWorkOrder(WorkType.Eat, -1, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        status.createWorkOrder(WorkType.Eat, -1, WorkExperienceType.NONE, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy <= DssConst.Worker_Starvation)
                    {
                        --workerStatusActiveCount;
                        --workForce.amount;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Starving, -1, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        workerStatuses[i] = status;
                    }
                    else//else if (workQue.Count > 0)
                    {
                        idleWorkers.Add(i);
                        var status = workerStatuses[i];
                    }
                    //else
                    //{
                    //    var worker = workerStatuses[i];
                    //    worker.energy -= (Ref.TotalGameTimeSec - worker.processTimeStartStampSec) * DssConst.WorkTeamEnergyCost_WhenIdle;
                    //    worker.processTimeStartStampSec = Ref.TotalGameTimeSec;
                    //}
                }
            }

            int distanceValue;
            int experienceValue;

            switch (experenceOrDistance)
            {
                case ExperenceOrDistancePrio.Mix:
                    distanceValue = 8;
                    experienceValue = 5;
                    break;
                case ExperenceOrDistancePrio.Distance:
                    distanceValue = 256;
                    experienceValue = 10;
                    break;
               case ExperenceOrDistancePrio.Experience:
                    distanceValue = 8;
                    experienceValue = 256;
                    break;

                default:
                    throw new NotImplementedException();
            }

            while (workQue.Count > 0 && idleWorkers.Count > 0)
            {
                var work = arraylib.PullLastMember(workQue);

                if (checkAvailable(work.work, work.subWork) &&
                    isFreeTile(work.subTile))
                {
                    WorkExperienceType experienceType = WorkLib.WorkToExperienceType(work.work, work.subWork, work.subTile);

                    int bestWorkerListIx = -1;
                    int bestvalue = int.MaxValue;

                    for (int i = 0; i < idleWorkers.Count; ++i)
                    {
                        var worderIx = idleWorkers[i];
                        var worker = workerStatuses[worderIx];

                        var distance =  work.subTile.SideLength(worker.subTileEnd);
                        var xp = worker.getXpFor(experienceType);

                        int value = distance * distanceValue - xp * experienceValue;

                        if (value < bestvalue)
                        { 
                            bestvalue = value;
                            bestWorkerListIx = i;
                        }
                    }

                    if (bestWorkerListIx == -1)
                    {
#if DEBUG
                        throw new Exception();
#else
                        return;
#endif
                    }

                    {//Assign job
                        var worderIx = idleWorkers[bestWorkerListIx];
                        idleWorkers.RemoveAt(bestWorkerListIx);

                        var status = workerStatuses[worderIx];
                        status.createWorkOrder(work.work, work.subWork, experienceType, work.orderId, work.subTile, this);
                        workerStatuses[worderIx] = status;

                        if (work.orderId >= 0)
                        {
                            faction.player.orders?.StartOrderId(work.orderId);
                        }
                    }
                   
                }
            }

            //Set remaning workers to wait
            foreach (var workerIx in idleWorkers)
            {
                var worker = workerStatuses[workerIx];
                worker.energy -= (Ref.TotalGameTimeSec - worker.processTimeStartStampSec) * DssConst.WorkTeamEnergyCost_WhenIdle;
                worker.processTimeStartStampSec = Ref.TotalGameTimeSec;
                workerStatuses[workerIx] = worker;  
            }

            if (!inRender_detailLayer)
            {
                processAsynchWork(workerStatuses);
            }
                        
            void buildWorkQue2()
            {
                IntVector2 center = WP.ToSubTilePos_Centered(tilePos);
                workQue.Clear();


                bool foodSafeGuard = foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
               
                var orders_sp = faction.player.orders;

                //ORDERS
                if (orders_sp != null)
                {
                    lock (orders_sp)
                    {
                        for (int i = 0; i < orders_sp.orders.Count; ++i)
                        {
                            var order = orders_sp.orders[i];
                            switch (order.GetWorkType(this))
                            {
                                case OrderType.Build:
                                    var workOrder = order.GetBuild();
                                    workQue.Add(workOrder.createWorkQue(out CraftBlueprint orderBluePrint));                                    
                                    break;
                                case OrderType.Demolish:
                                    var demolishOrder = order.GetDemolish();
                                    workQue.Add(demolishOrder.createWorkQue());  
                                    break;
                            }
                            
                        }
                    }
                }

                //PICK UP
                if (workTemplate.move.HasPrio() || woodSafeGuard )
                {
                    foreach (var pos in CityStructure.WorkInstance.ResourceOnGround)
                    {
                        var subTile = DssRef.world.subTileGrid.Get(pos);

                        if (subTile.collectionPointer >= 0)
                        {
                            var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                            var resource = chunk.peek();

                            if (needMore(resource.type, rawFoodSafeGuard, woodSafeGuard, out bool usesSafeGuard) && isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, pos, usesSafeGuard? WorkTemplate.SafeGuardPrio : workTemplate.move.value, distanceValue));
                            }
                        }
                    }
                }

                //WOOD
                if ( (workTemplate.wood.HasPrio() && res_wood.needMore()) || woodSafeGuard)
                {
                    foreach (var pos in CityStructure.WorkInstance.Trees)
                    {
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, woodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.wood.value, distanceValue));
                        }
                    }
                }

                //STONE
                if (workTemplate.stone.HasPrio() &&
                    res_stone.needMore())
                {
                    foreach (var pos in CityStructure.WorkInstance.Stones)
                    {
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.stone.value, distanceValue));
                        }
                    }
                }

                //FARMS
                //if (workTemplate.farm_food.HasPrio() || rawFoodSafeGuard || fuelSafeGuard)
                //{
                foreach (var tilework in CityStructure.WorkInstance.Farms)
                {
                    bool needMore = false;
                    bool safeGuard = false;
                    var subTile = DssRef.world.subTileGrid.Get(tilework.subtile);
                    int prio = 0;
                    switch (subTile.GetFoilType())
                    {
                        case TerrainSubFoilType.LinenFarm:
                            needMore = res_skinLinnen.needMore();
                            prio = workTemplate.farm_linen.value;
                            break;
                        case TerrainSubFoilType.WheatFarm:                                
                            safeGuard = rawFoodSafeGuard;
                            needMore = res_rawFood.needMore();
                            prio = workTemplate.farm_food.value;
                            break;
                        case TerrainSubFoilType.RapeSeedFarm:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore();
                            prio = workTemplate.farm_fuel.value;
                            break;
                        case TerrainSubFoilType.HempFarm:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore() || res_skinLinnen.needMore() || fuelSafeGuard;
                            prio = Math.Max(workTemplate.farm_linen.value, workTemplate.farm_fuel.value);
                            break;
                    }

                    if (((needMore && prio > WorkTemplate.NoPrio) || safeGuard) && isFreeTile(tilework.subtile))
                    {
                        int distanceValue = -center.SideLength(tilework.subtile);
                        workQue.Add(new WorkQueMember(tilework.workType, NoSubWork, tilework.subtile, safeGuard? WorkTemplate.SafeGuardPrio : workTemplate.farm_food.value, distanceValue));
                    }
                }

                //MINING
                if (workTemplate.bogiron.HasPrio() &&
                    res_ironore.needMore())
                {
                    foreach (var pos in CityStructure.WorkInstance.BogIron)
                    {                       
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.bogiron.value, distanceValue));
                        }
                    }
                }

                if (workTemplate.mining_iron.HasPrio() || fuelSafeGuard)
                {
                    foreach (var pos in CityStructure.WorkInstance.Mines)
                    {
                        bool needMore = true;
                        bool safeGuard = false;

                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch ((TerrainMineType)subTile.subTerrain)
                        {
                            case TerrainMineType.IronOre:
                                needMore = res_ironore.needMore();
                                break;
                            case TerrainMineType.Coal:
                                //++fuelSpots;
                                safeGuard = fuelSafeGuard;
                                needMore = res_fuel.needMore();
                                break;
                        }

                        if ((needMore || safeGuard) && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, pos, safeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.mining_iron.value, distanceValue));
                        }
                    }
                }

                //ANIMALS
                if (workTemplate.farm_food.HasPrio() || rawFoodSafeGuard)
                {
                    foreach (var pos in CityStructure.WorkInstance.AnimalPens)
                    {
                        bool needMore = true;

                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch (subTile.GeBuildingType())
                        {
                            case TerrainBuildingType.HenPen:
                                needMore = res_rawFood.needMore();
                                break;
                            case TerrainBuildingType.PigPen:
                                needMore = res_rawFood.needMore() || res_skinLinnen.needMore();
                                break;
                        }

                        if ((needMore || rawFoodSafeGuard) && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, pos, rawFoodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.farm_food.value, distanceValue));
                        }
                    }
                }

                //CRAFT
                foreach (var pos in CityStructure.WorkInstance.CraftStation)
                {
                    int distanceValue = -center.SideLength(pos);
                    var subTile = DssRef.world.subTileGrid.Get(pos);
                    var building = subTile.GeBuildingType();
                    switch (building)
                    {
                        case TerrainBuildingType.Work_Cook:
                            if (
                                ((workTemplate.craft_food.HasPrio() && res_food.needMore()) ||  foodSafeGuard) &&
                                (CraftResourceLib.Food2.hasResources(this) || CraftResourceLib.Food1.hasResources(this)) &&
                                isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, pos, foodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.craft_food.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Work_Bench:
                            craftBench(pos, distanceValue, CraftBuildingLib.BenchCraftTypes, -5000);
                            break;
                        case TerrainBuildingType.Work_Smith:

                            craftBench(pos, distanceValue, CraftBuildingLib.SmithCraftTypes);
                            break;

                        case TerrainBuildingType.Work_CoalPit:
                            if (
                                ((workTemplate.craft_fuel.HasPrio() && res_fuel.needMore()) || fuelSafeGuard) &&
                               CraftResourceLib.Charcoal.hasResources(this) &&
                               isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, pos, fuelSafeGuard? WorkTemplate.SafeGuardPrio : workTemplate.craft_fuel.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Brewery:
                            if (workTemplate.craft_beer.HasPrio() &&
                                res_beer.needMore() &&
                                CraftBuildingLib.Brewery.hasResources(this) &&
                                isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Beer, pos, workTemplate.craft_beer.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Carpenter:
                            craftBench(pos, distanceValue, CraftBuildingLib.CarpenterCraftTypes);
                            break;
                    }
                }


                //EMPTY
                if (checkAutoBuildAvailable())
                {
                    foreach (var pos in CityStructure.WorkInstance.EmptyLand)
                    {
                        faction.player.AutoExpandType(this, out bool work, out var buildType, out bool intelligent);

                        bool intelligentCheck = true;

                        if (fuelSafeGuard && CityStructure.WorkInstance.fuelSpots < 4)
                        {
                            ++CityStructure.WorkInstance.fuelSpots;
                            buildType = BuildAndExpandType.RapeSeedFarm;
                        }
                        else if (rawFoodSafeGuard && CityStructure.WorkInstance.foodspots < 4)
                        {
                            ++CityStructure.WorkInstance.foodspots;
                            buildType = BuildAndExpandType.WheatFarm;
                        }
                        else if (work && workForce.amount >= workForceMax)
                        {
                            buildType = BuildAndExpandType.WorkerHuts;
                        }
                        else if (intelligent)
                        {
                            switch (buildType)
                            {
                                case BuildAndExpandType.WheatFarm:
                                case BuildAndExpandType.HenPen:
                                    intelligentCheck = res_rawFood.needMore();
                                    break;
                                case BuildAndExpandType.PigPen:
                                    intelligentCheck = res_rawFood.needMore() || res_skinLinnen.needMore();
                                    break;
                                case BuildAndExpandType.LinenFarm:
                                    intelligentCheck = res_skinLinnen.needMore();
                                    break;
                                case BuildAndExpandType.RapeSeedFarm:
                                    intelligentCheck = res_fuel.needMore();
                                    break;
                            }
                        }

                        if (buildType != BuildAndExpandType.NUM_NONE)
                        {
                            if (BuildLib.BuildOptions[(int)buildType].blueprint.available(this) &&
                                isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, pos, workTemplate.autoBuild.value, distanceValue));
                            }
                        }
                    }
                }


                bool checkAutoBuildAvailable()
                {
                    if (buildingLevel_logistics < 2)
                    {
                        var p = faction.player.GetLocalPlayer();
                        if (p != null)
                        {
                            return p.orders.buildQueue(this) + 1 < MaxBuildQueue();
                        }
                    }
                    return true;
                }

                void craftBench(IntVector2 pos, int distanceValue, ItemResourceType[] types, int prioAdd = 0)
                {
                    int topPrioValue = WorkTemplate.NoPrio;
                    ItemResourceType topItem = ItemResourceType.NONE;
                    WorkPriority topPrio = WorkPriority.Empty;

                    foreach (var item in types)
                    {
                        var template = workTemplate.GetWorkPriority(item);
                        if (template.value > topPrioValue)
                        {
                            CraftResourceLib.Blueprint(item, out var bp1, out var bp2);
                            if (bp1.available(this) && GetGroupedResource(item).needMore())
                            {
                                topPrioValue = template.value;
                                topItem = item;
                                topPrio = template;
                            }
                        }
                    }

                    if (topPrioValue > WorkTemplate.NoPrio &&
                        isFreeTile(pos))
                    {
                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, pos, topPrioValue, distanceValue + prioAdd));
                    }
                }

            }



            //    if (DssLib.UseLocalTrading)
            //    {

            //        const int CostPrioValue = -1000;
            //        const int RelationPrioValue = 100;

            //        WorkQueMember woodTrade = WorkQueMember.NoPrio;
            //        WorkQueMember stoneTrade = WorkQueMember.NoPrio;
            //        WorkQueMember foodTrade = WorkQueMember.NoPrio;


            //        //Trade with neighbor cities
            //        foreach (var n in neighborCities)
            //        {
            //            var nCity = DssRef.world.cities[n];

            //            //priority
            //            // check trade block
            //            //1. price
            //            //2. buy in faction/ally
            //            //3. distance
            //            int distanceValue = -tilePos.SideLength(nCity.tilePos);

            //            if (DssRef.diplomacy.MayTrade(nCity.faction, faction, out var relation))
            //            {
            //                if (nCity.faction == faction)
            //                {
            //                    distanceValue += 8 * RelationPrioValue;
            //                }
            //                else
            //                {
            //                    distanceValue += (int)relation * RelationPrioValue;
            //                }

            //                if (res_wood.needToImport() && nCity.res_wood.canTradeAway())
            //                {
            //                    int value = distanceValue + (int)(nCity.tradeTemplate.wood.price * CostPrioValue);
            //                    if (value > woodTrade.priority)
            //                    {
            //                        woodTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.SoftWood, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
            //                    }
            //                }
            //                if (res_stone.needToImport() && nCity.res_stone.canTradeAway())
            //                {
            //                    int value = distanceValue + (int)(nCity.tradeTemplate.stone.price * CostPrioValue);
            //                    if (value > stoneTrade.priority)
            //                    {
            //                        stoneTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Stone_G, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
            //                    }
            //                }
            //                if (res_food.needToImport() && nCity.res_food.canTradeAway())
            //                {
            //                    int value = distanceValue + (int)(nCity.tradeTemplate.food.price * CostPrioValue);
            //                    if (value > foodTrade.priority)
            //                    {
            //                        foodTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Food_G, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
            //                    }
            //                }
            //            }
            //        }

            //        if (woodTrade.work != WorkType.IsDeleted)
            //        {
            //            workQue.Add(woodTrade);
            //        }
            //        if (stoneTrade.work != WorkType.IsDeleted)
            //        {
            //            workQue.Add(stoneTrade);
            //        }
            //        if (foodTrade.work != WorkType.IsDeleted)
            //        {
            //            workQue.Add(foodTrade);
            //        }
            //    }
            //}

            bool isFreeTile(IntVector2 subtile)
            {
                for (int i = 0; i < workerStatuses.Count; ++i)
                { 
                    var status = workerStatuses[i];
                    if (status.work != WorkType.Idle &&
                        status.subTileEnd == subtile)
                    { 
                        return false; 
                    }
                }

                return true;
            }

            
        }

        public void checkPlayerFuelAccess_OnGamestart_async()
        {
            const int FuelFarmCount = 10;
            int fuelType = (int)TerrainSubFoilType.RapeSeedFarm;

            CityStructure structure = new CityStructure();
            structure.update(this, 32, FuelFarmCount);
            if (structure.fuelSpots <= 8)
            {
                int count = Math.Min(structure.EmptyLand.Count, FuelFarmCount);
                for (int i = 0; i < count; ++i) 
                {
                    BuildLib.TryAutoBuild(structure.EmptyLand[i], TerrainMainType.Foil, fuelType, Ref.rnd.Int(1, TerrainContent.FarmCulture_MaxSize));
                }                
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }

        void async_blackMarketUpdate()
        {
            //float foodUpkeep = 0;

            if (res_food.amount <= -10)
            {
                int buyFood = -res_food.amount;

                int cost = (int)(buyFood * DssConst.FoodGoldValue_BlackMarket);
                faction.payMoney(cost, true);
                blackMarketCosts_food.add(cost);
                res_food.amount += buyFood;

                starving = true;
            }
        }

        bool checkAvailable(WorkType work, int subWork)
        {
            switch (work)
            {
                case WorkType.Plant:
                    return res_water.amount >= DssConst.PlantWaterCost;

                case WorkType.Craft:
                    {
                        ItemResourceType item = (ItemResourceType)subWork;
                        CraftResourceLib.Blueprint(item, out var bp1, out var bp2);
                        if (bp1.available(this))
                        {
                            //bp1.createBackOrder(this);
                            return true;
                        }
                        else if (bp2 != null && bp2.available(this))
                        {
                            //bp2.createBackOrder(this);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                case WorkType.Build:
                    {
                        var bp = BuildLib.BuildOptions[subWork].blueprint;
                        if (bp.available(this))
                        {
                            //bp.createBackOrder(this);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                default:
                    return true;
            }
        }

    }

    

    struct WorkQueMember
    {
        public static readonly WorkQueMember NoPrio = new WorkQueMember() { priority = int.MinValue };

        public WorkType work;
        public int subWork;
        public IntVector2 subTile;
        public int orderId = -1;

        /// <summary>
        /// Goes from 1:lowest to 10: highest
        /// </summary>
        public int priority;

        public WorkQueMember(WorkType work, int subWork, IntVector2 subTile, int priority, int subPrio)
        {
            this.work = work;
            this.subWork = subWork;
            this.subTile = subTile;
            this.priority = priority * 1000000 + subPrio;
        }
    }

    

    
}
