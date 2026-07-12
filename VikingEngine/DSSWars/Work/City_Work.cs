using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        static WorkerSkillCollector SkillCollector = new WorkerSkillCollector();

        public WorkTemplate workTemplate = new WorkTemplate();

        const int NoSubWork = -1;
        public const int WorkTeamSize = 6;
        TimeStamp previousWorkQueUpdate = TimeStamp.None;
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        static List<WorkQueMember> WaitingHighSkillJobs = new List<WorkQueMember>(16);
        bool starving = false;
        static List<int> idleWorkers = new List<int>(64);


        public int WorkerStats_IdleCount = 0;
        public int WorkerStats_WorkQueueLength => workQue.Count;
        public int WorkerStats_TotalUnits = -1; /*=> workerStatuses.Count*/

        //public bool mintOnFullStockProperty(object tag, bool set, bool value)
        
        public int WorkerStats_StuckBuildings_Process = 0;
        public int WorkerStats_StuckBuildings = 0;

        FlatArray_Three<int> markedForExit = new FlatArray_Three<int>();

        public bool craftOnFullStockProperty(object tag, bool set, bool value)
        {
            WorkPriorityType work = (WorkPriorityType)tag;

            ref var prio = ref workTemplate.GetRefWorkPriority(work);

            if (set)
            {
                prio.waitForStockpile = value;
            }
            return prio.waitForStockpile;
        }

        public void async_workUpdate(int updateSpeed)
        {
            if (factionIndex < 0 || cityType == CityType.UnClaimed)
            {
                CityStructure.WorkInstance.update(DssRef.world, this, 0);
                return; 
            }

            var faction = pfaction.GetFaction();
            if (faction == null || faction.player == null)
            {
                return;
            }

            lock (workerStatuses.array)
            {
                bool hostUpdate = DssRef.state.host || faction.player.IsLocalPlayer();

                CityStructure.WorkInstance.newCity = true;
               
                async_blackMarketUpdate();

                int workTeamsTotalCount = workerStatuses.Count;
                int deletedCount = 0;
                int idleCount = 0;
                int mayExitCount = 0;
             
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    var status = workerStatuses[i];
                    SkillCollector.Add(ref status);

                    switch (status.work)
                    {
                        case WorkType.IsDeleted:
                            ++deletedCount;
                            --workTeamsTotalCount;
                            break;

                        case WorkType.Starving:
                        case WorkType.Exit:
                            //--workTeamsTotalCount;
                            ++mayExitCount;
                            break;

                        case WorkType.Idle:
                            idleCount++;
                            break;
                        default:
                            checkAvailable(status.work, status.workSubType);
                            break;

                    }
                    //minMax_workerCulling.Next(ref status.subTileEnd);
                 
                }

                cityExperienceLevels = SkillCollector.ExportData();
                
                int workTeamGoalCount = Bound.Min(workForce.amount / WorkTeamSize, 1);
                int exitCount = (workTeamsTotalCount - mayExitCount) - (workTeamGoalCount/* + 1*/);

                if (myIndex == 383)
                {
                    lib.DoNothing();
                }

                if (workTeamsTotalCount < workTeamGoalCount)
                {
                    
                    int deletedIx = 0;
                    int newWorkers = workTeamGoalCount - workTeamsTotalCount;
                    IntVector2 startPos = citySquareSubtilePos;
                    for (int i = 0; i < newWorkers; i++)
                    {
                        var newWorker = new WorkerStatus(true)
                        {
                            work = WorkType.Idle,
                            processTimeStartStampSec = Ref.TotalGameTimeSec,
                            energy = DssConst.Worker_MaxEnergy,
                            subTileEnd = startPos,
                            subTileStart = startPos,
                        };

                        if (DssRef.time.totalMinutes < 1)
                        {
                            newWorker = newGameWorkerSkills(newWorker);
                        }
                        else if (cityCulture == CityCulture.Apprentices)
                        {
                            for (int xpIx = 0; xpIx <= 1; ++xpIx)
                            {
                                var exp = arraylib.RandomListMember(XpLib.ExperienceTypes);
                                var lvl = (ExperienceLevel)cityExperienceLevels.Get(exp).maxLevel;
                                if (lvl >= ExperienceLevel.Expert_3)
                                {
                                    newWorker.setXpFor(exp, DssConst.WorkXpToLevel);
                                }
                            }
                        }

                        if (deletedCount > 0)
                        {
                            for (int di = deletedIx; di < workerStatuses.Count; ++di)
                            {
                                if (workerStatuses[di].work == WorkType.IsDeleted)
                                {
                                    workerStatuses[di] = newWorker;
                                    --deletedCount;
                                    deletedIx = di;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            workerStatuses.Add(newWorker);
                        }
                        ++workTeamsTotalCount;
                        ++idleCount;
                    }
                }
                else if (exitCount > 0)//workTeamsTotalCount - mayExitCount > workTeamGoalCount + 1)
                {
                    if (myIndex == 54)
                    {
                        lib.DoNothing();
                    }

                    findLowXpWorkers();
                }
              

                if (idleCount > 0 && IsNetHosted && previousWorkQueUpdate.secPassed(10))
                {
                    CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                    buildWorkQue2();
                    //Last position = highest priority
                    if (workQue.Count > 1)
                    {
                        workQue.Sort((a, b) => a.priority.CompareTo(b.priority));
                    }
                    previousWorkQueUpdate.setNow();
                }

                idleWorkers.Clear();
                int maxWorkerOrderCount = (1 + workerStatuses.Count / 100) * updateSpeed;

                //Collect idle workers
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    ref WorkerStatus status = ref workerStatuses.array[i];

                    if (status.work == WorkType.Idle)
                    {
                        if (exitCount > 0 &&
                            (status.GetXpScore() < DssConst.WorkXpToLevel_Squared ||
                            markedForExit.Contains(i) || 
                            exitCount >= 4)
                            )
                        {
                            
                            exitCount--;
                            mayExitCount++;
                            status.createWorkOrder(WorkType.Exit, -1, 0, WorkExperienceType.NUM_NONE, -1, citySquareSubtilePos/*WP.ToSubTilePos_Centered(tilePos)*/, this);
                        }
                        else if (status.carry.amount > 0)
                        {
                            CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                            status.createWorkOrder(WorkType.DropOff, -1, 0, WorkExperienceType.Transport, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        }
                        else if (status.energy < 0 && workTeamsTotalCount <= 1)
                        {
                            status.energy = DssConst.Worker_MaxEnergy / 2;
                        }
                        else if (status.energy < 0 && (resourceAmount(CityResourceIndex.food) > 0 || faction.hasGold(1, this)))
                        {
                            CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                            status.createWorkOrder(WorkType.Eat, -1, 0, WorkExperienceType.NUM_NONE, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        }
                        else if (status.energy <= DssConst.Worker_Starvation)
                        {
                            --workTeamsTotalCount;
                            --workForce.amount;

                            status.createWorkOrder(WorkType.Starving, -1, 0, WorkExperienceType.NUM_NONE, -1, citySquareSubtilePos/*WP.ToSubTilePos_Centered(tilePos)*/, this);
                        }
                        else
                        {
                            idleWorkers.Add(i);
                        }
                    }
                }

                WorkerStats_TotalUnits = workTeamsTotalCount;
                WorkerStats_IdleCount = idleWorkers.Count;

                int distanceValue;
                int experienceValue;

                switch (experenceOrDistance)
                {
                    case ExperienceOrDistancePrio.Mix:
                        distanceValue = 8;
                        experienceValue = 5;
                        break;
                    case ExperienceOrDistancePrio.Distance:
                        distanceValue = 256;
                        experienceValue = 10;
                        break;
                    case ExperienceOrDistancePrio.Experience:
                        distanceValue = 8;
                        experienceValue = 256;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                while (workQue.Count > 0 && idleWorkers.Count > 0 && maxWorkerOrderCount > 0)
                {
                    var work = arraylib.PullLastMember(workQue);

                    if (checkAvailable(work.work, work.subWork) &&
                        work_isFreeTile(work.subTile))
                    {
                        WorkExperienceType experienceType = WorkLib.WorkToExperienceType(work.work, work.subWork, work.workBonus, work.subTile, this,
                           out ExperienceLevel requiredLvl, out int xpRequired, out int maxXp);

                        if (requiredLvl == ExperienceLevel.Beginner_1 || requiredLvl <= (ExperienceLevel)cityExperienceLevels.Get(experienceType).maxLevel)
                        {

                            int bestWorkerListIx = -1;
                            int bestvalue = int.MaxValue;

                            for (int i = 0; i < idleWorkers.Count; ++i)
                            {
                                var worderIx = idleWorkers[i];
                                var worker = workerStatuses.array[worderIx];

                                var xp = worker.getXpFor(experienceType);

                                if (xp.InBound(xpRequired, maxXp))
                                {
                                    var distance = work.subTile.SideLength(worker.subTileEnd);
                                    int value = distance * distanceValue - xp.xp * experienceValue;

                                    if (value < bestvalue)
                                    {
                                        bestvalue = value;
                                        bestWorkerListIx = i;
                                    }
                                }
                            }

                            if (bestWorkerListIx >= 0)
                            {//Assign job
                                var worderIx = idleWorkers[bestWorkerListIx];
                                idleWorkers.RemoveAt(bestWorkerListIx);

                                ref var status = ref workerStatuses.array[worderIx];
                                status.createWorkOrder(work.work, work.subWork, work.workBonus, experienceType, work.orderId, work.subTile, this);
                                
                                --maxWorkerOrderCount;

                                if (work.orderId >= 0)
                                {
                                    faction.player.orders?.StartOrderId(work.orderId);
                                }
                            }
                            else if (requiredLvl > ExperienceLevel.Beginner_1)
                            {
                                //put back experiece required job
                                WaitingHighSkillJobs.Add(work);
                            }
                        }

                    }
                }

                workQue.AddRange(WaitingHighSkillJobs);
                WaitingHighSkillJobs.Clear();

                //Set remaning workers to wait
                foreach (var workerIx in idleWorkers)
                {
                    ref var worker = ref workerStatuses.array[workerIx];
                    worker.energy -= (Ref.TotalGameTimeSec - worker.processTimeStartStampSec) * DssConst.WorkTeamEnergyCost_WhenIdle;
                    worker.processTimeStartStampSec = Ref.TotalGameTimeSec;
                    //workerStatuses[workerIx] = worker;
                }

                if (!inRender_detailLayer)
                {
                    processAsynchWork(ref workerStatuses);
                }

                void buildWorkQue2()
                {
                    if (faction == null)
                        return;

                    IntVector2 center = citySquareSubtilePos;
                    workQue.Clear();

                    if (debugTagged || myIndex == 45)
                    {
                        lib.DoNothing();
                    }

                    //bool foodSafeGuard = foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

                    var orders_sp = faction.player?.orders;

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

                    //SCHOOL
                    lock (schoolBuildings)
                    {
                        for (int i = 0; i < schoolBuildings.Count; ++i)
                        {
                            var school = schoolBuildings[i];
                            if (school.que > 0)
                            {
                                workQue.Add(new WorkQueMember(WorkType.School, (int)school.learnExperience, (byte)school.toLevel, conv.IntToIntVector2(school.idAndPosition), WorkTemplate.MaxPrio, 0, 0));
                            }
                        }
                    }

                    //PICK UP
                    if (hostUpdate)
                    {
                        if (workTemplate.Get(WorkPriorityType.move).HasPrio())
                        {
                            foreach (var pos in CityStructure.WorkInstance.ResourceOnGround)
                            {
                                var subTile = DssRef.world.subTileGrid.Get(pos);

                                if (subTile.collectionPointer >= 0)
                                {
                                    var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                                    var resource = chunk.peek();

                                    if (needMore(resource.type) && work_isFreeTile(pos))
                                    {
                                        int distanceValue = -center.SideLength(pos);
                                        workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, 0, pos, workTemplate.Get(WorkPriorityType.move).value, 0, distanceValue));
                                    }
                                }
                            }
                        }
                    }

                    //WOOD
                    if (workTemplate.Get(WorkPriorityType.wood).HasPrio() && needMore(CityResourceIndex.wood))
                    {
                        foreach (var pos in CityStructure.WorkInstance.Trees)
                        {
                            if (work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);

                                byte bonus = 0;
                                if (CityStructure.WorkInstance.inBonusRadius(pos, CityStructure.WorkInstance.WoodCutter, DssConst.WoodCutter_BonusRadius))
                                {
                                    bonus = DssConst.WoodCutter_WoodBonus;
                                }
                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, bonus, pos, workTemplate.Get(WorkPriorityType.wood).value, bonus, distanceValue));
                            }
                        }
                    }

                    //STONE
                    if (workTemplate.Get(WorkPriorityType.stone).HasPrio() &&
                        needMore(CityResourceIndex.stone)/*res_stone.needMore()*/)
                    {
                        foreach (var pos in CityStructure.WorkInstance.Stones)
                        {
                            if (work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                byte bonus = 0;
                                if (CityStructure.WorkInstance.inBonusRadius(pos, CityStructure.WorkInstance.StoneCutter, DssConst.StoneCutter_BonusRadius))
                                {
                                    bonus = DssConst.StoneCutter_StoneBonus;
                                }
                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, bonus, pos, workTemplate.Get(WorkPriorityType.stone).value, bonus, distanceValue));
                            }
                        }
                    }

                    //FARMS
                    foreach (var tilework in CityStructure.WorkInstance.Farms)
                    {
                        bool bNeedMore = false;
                        //bool safeGuard = false;
                        var subTile = DssRef.world.subTileGrid.Get(tilework.subtile);
                        int prio = 0;
                        byte bonus = 0;
                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.TreeApple:
                            case TerrainSubFoilType.TreeBanana:
                                //safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.food);
                                prio = workTemplate.Get(WorkPriorityType.farmFood).value;
                                break;

                            case TerrainSubFoilType.LinenFarm:
                                bNeedMore = needMore(CityResourceIndex.skinLinnen);//res_skinLinnen.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmlinen).value;
                                break;
                            case TerrainSubFoilType.LinenFarmUpgraded:
                                bNeedMore = needMore(CityResourceIndex.skinLinnen);//res_skinLinnen.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmlinen).value;

                                break;
                            case TerrainSubFoilType.WheatFarm:
                                //safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.rawFood);
                                prio = workTemplate.Get(WorkPriorityType.farmRawFood).value;
                                break;
                            case TerrainSubFoilType.WheatFarmUpgraded:
                                //safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.rawFood);//res_rawFood.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmRawFood).value;
                                bonus = 1;
                                break;
                            case TerrainSubFoilType.RapeSeedFarm:
                                //safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.fuel);//res_fuel.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmfuel).value;
                                break;
                            case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                //safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.fuel);////res_fuel.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmfuel).value;
                                break;
                            case TerrainSubFoilType.HempFarm:
                                //safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.fuel);////res_fuel.needMore() || res_skinLinnen.needMore() || fuelSafeGuard;
                                prio = Math.Max(workTemplate.Get(WorkPriorityType.farmlinen).value, workTemplate.Get(WorkPriorityType.farmfuel).value);
                                break;
                            case TerrainSubFoilType.HempFarmUpgraded:
                                //safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResourceIndex.fuel) || needMore(CityResourceIndex.skinLinnen)/*res_fuel.needMore() || res_skinLinnen.needMore()*/ ;
                                prio = Math.Max(workTemplate.Get(WorkPriorityType.farmlinen).value, workTemplate.Get(WorkPriorityType.farmfuel).value);
                                break;
                        }

                        if ((bNeedMore && prio > WorkTemplate.NoPrio) && work_isFreeTile(tilework.subtile))
                        {
                            int distanceValue = -center.SideLength(tilework.subtile);
                            workQue.Add(new WorkQueMember(tilework.workType, NoSubWork, bonus, tilework.subtile, prio, 0, distanceValue));
                        }
                    }

                    //MINING
                    if (workTemplate.Get(WorkPriorityType.bogiron).HasPrio() &&
                        needMore(CityResourceIndex.ironore))
                    {
                        foreach (var pos in CityStructure.WorkInstance.BogIron)
                        {
                            if (work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, 0, pos, workTemplate.Get(WorkPriorityType.bogiron).value, 0, distanceValue));
                            }
                        }
                    }
                    if (workTemplate.Get(WorkPriorityType.collectClay).HasPrio() &&
                        needMore(CityResourceIndex.Clay))
                    {
                        foreach (var pos in CityStructure.WorkInstance.ClayPit)
                        {
                            if (work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, 0, pos, workTemplate.Get(WorkPriorityType.collectClay).value, 0, distanceValue));
                            }
                        }
                    }

                    //if (fuelSafeGuard)
                    //{
                    foreach (var pos in CityStructure.WorkInstance.Mines)
                    {
                        bool bNeedMore = true;
                        //bool safeGuard = false;

                        WorkPriority priority;
                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch ((TerrainMineType)subTile.subTerrain)
                        {
                            default:
                            case TerrainMineType.IronOre:
                                bNeedMore = needMore(CityResourceIndex.ironore);
                                priority = workTemplate.Get(WorkPriorityType.miningIron);
                                break;
                            case TerrainMineType.TinOre:
                                bNeedMore = needMore(CityResourceIndex.TinOre);
                                priority = workTemplate.Get(WorkPriorityType.miningTin);
                                break;
                            case TerrainMineType.CopperOre:
                                bNeedMore = needMore(CityResourceIndex.CopperOre);
                                priority = workTemplate.Get(WorkPriorityType.miningCopper);
                                break;
                            case TerrainMineType.LeadOre:
                                bNeedMore = needMore(CityResourceIndex.LeadOre);
                                priority = workTemplate.Get(WorkPriorityType.miningLead);
                                break;
                            case TerrainMineType.SilverOre:
                                bNeedMore = needMore(CityResourceIndex.SilverOre);
                                priority = workTemplate.Get(WorkPriorityType.miningSilver);
                                break;
                            case TerrainMineType.Salt:
                                bNeedMore = needMore(CityResourceIndex.Salt);
                                priority = workTemplate.Get(WorkPriorityType.miningSalt);
                                break;
                            case TerrainMineType.StoneBlock:
                                bNeedMore = needMore(CityResourceIndex.Brick);
                                priority = workTemplate.Get(WorkPriorityType.miningBrick);
                                break;
                            case TerrainMineType.Sulfur:
                                bNeedMore = needMore(CityResourceIndex.Sulfur);
                                priority = workTemplate.Get(WorkPriorityType.miningSulfur);
                                break;
                            case TerrainMineType.GoldOre:
                                bNeedMore = true;
                                priority = workTemplate.Get(WorkPriorityType.miningGold);
                                break;
                            case TerrainMineType.Mithril:
                                bNeedMore = needMore(CityResourceIndex.RawMithril);
                                priority = workTemplate.Get(WorkPriorityType.miningMithril);
                                break;
                            case TerrainMineType.Coal:
                                bNeedMore = needMore(CityResourceIndex.fuel);
                                priority = workTemplate.Get(WorkPriorityType.miningCoal);
                                break;
                        }

                        if (priority.HasPrio() && bNeedMore && work_isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, 0, pos,  priority.value, 0, distanceValue));
                        }
                    }


                    //ANIMALS
                    if (workTemplate.Get(WorkPriorityType.move).HasPrio())
                    {
                        foreach (var pos in CityStructure.WorkInstance.AnimalPens)
                        {
                            bool bNeedMore = true;

                            var subTile = DssRef.world.subTileGrid.Get(pos);
                            switch (subTile.GetBuildingType())
                            {
                                case TerrainBuildingType.FowlHabitat:
                                case TerrainBuildingType.FowlPen:
                                    bNeedMore = needMore(CityResourceIndex.Fowl);
                                    break;
                                case TerrainBuildingType.HenPen:
                                    bNeedMore = needMore(CityResourceIndex.Hen);
                                    break;
                                case TerrainBuildingType.PigPen:
                                    bNeedMore = needMore(CityResourceIndex.Pig);
                                    break;

                                // Oxen
                                case TerrainBuildingType.OxHabitat:
                                case TerrainBuildingType.OxenPen:
                                    bNeedMore = needMore(CityResourceIndex.Oxen);
                                    break;
                                case TerrainBuildingType.KineOxenPen:
                                    bNeedMore = needMore(CityResourceIndex.KineOxen);
                                    break;

                                // Dogs
                                case TerrainBuildingType.DogHabitat:
                                case TerrainBuildingType.DogCage:
                                    bNeedMore = needMore(CityResourceIndex.Dog);
                                    break;
                                case TerrainBuildingType.HoundCage:
                                    bNeedMore = needMore(CityResourceIndex.Hound);
                                    break;

                                // Horses
                                case TerrainBuildingType.PonyHabitat:
                                case TerrainBuildingType.PonyPen:
                                    bNeedMore = needMore(CityResourceIndex.Pony);
                                    break;
                                case TerrainBuildingType.HorsePen:
                                    bNeedMore = needMore(CityResourceIndex.Horse);
                                    break;
                                case TerrainBuildingType.WarHorsePen:
                                    bNeedMore = needMore(CityResourceIndex.WarHorse);
                                    break;
                                case TerrainBuildingType.DraftHorsePen:
                                    bNeedMore = needMore(CityResourceIndex.DraftHorse);
                                    break;

                                // Wild Pigs/Hogs
                                case TerrainBuildingType.BoarHabitat:
                                case TerrainBuildingType.BoarPen:
                                    bNeedMore = needMore(CityResourceIndex.Boar);
                                    break;
                                case TerrainBuildingType.WildPigPen:
                                    bNeedMore = needMore(CityResourceIndex.WildPig);
                                    break;
                                case TerrainBuildingType.WildHogPen:
                                    bNeedMore = needMore(CityResourceIndex.WildHog);
                                    break;
                                case TerrainBuildingType.WarHogPen:
                                    bNeedMore = needMore(CityResourceIndex.WarHog);
                                    break;
                                case TerrainBuildingType.StagHogPen:
                                    bNeedMore = needMore(CityResourceIndex.StagHog);
                                    break;

                                // Wolves
                                case TerrainBuildingType.WolfHabitat:
                                case TerrainBuildingType.WolfCage:
                                    bNeedMore = needMore(CityResourceIndex.Wolf);
                                    break;
                                case TerrainBuildingType.WargCage:
                                    bNeedMore = needMore(CityResourceIndex.Warg);
                                    break;
                                case TerrainBuildingType.AlphaWargCage:
                                    bNeedMore = needMore(CityResourceIndex.AlphaWarg);
                                    break;

                                // Cats
                                case TerrainBuildingType.CatHabitat:
                                case TerrainBuildingType.WildCatCage:
                                    bNeedMore = needMore(CityResourceIndex.WildCat);
                                    break;
                                case TerrainBuildingType.LionCage:
                                    bNeedMore = needMore(CityResourceIndex.Lion);
                                    break;
                                case TerrainBuildingType.WarLionCage:
                                    bNeedMore = needMore(CityResourceIndex.WarLion);
                                    break;

                                // Elephants
                                case TerrainBuildingType.ElephantHabitat:
                                case TerrainBuildingType.ElephantCage:
                                    bNeedMore = needMore(CityResourceIndex.Elephant);
                                    break;
                                case TerrainBuildingType.WarElephantCage:
                                    bNeedMore = needMore(CityResourceIndex.WarElephant);
                                    break;
                                case TerrainBuildingType.OliphantCage:
                                    bNeedMore = needMore(CityResourceIndex.Oliphant);
                                    break;
                            }

                            if (bNeedMore && work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, 0, pos, workTemplate.Get(WorkPriorityType.move).value, 0, distanceValue));
                            }
                        }
                    }

                    {
                        //CRAFT
                        byte prio;
                        foreach (var pos in CityStructure.WorkInstance.CraftStation)
                        {
                            int distanceValue = -center.SideLength(pos);
                            var subTile = DssRef.world.subTileGrid.Get(pos);
                            var building = subTile.GetBuildingType();
                            switch (building)
                            {
                                case TerrainBuildingType.Work_Cook:
                                    if (
                                        workTemplate.Get(WorkPriorityType.craftFood).HasPrio_r(out prio) && needMore(CityResourceIndex.food) &&
                                        (CraftResourceLib.Food2.hasResources(this) || CraftResourceLib.Food1.hasResources(this)) &&
                                        work_isFreeTile(pos))
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, 0, pos, prio, 0, distanceValue));
                                    }

                                    if (
                                        workTemplate.Get(WorkPriorityType.craftConservedFood).HasPrio_r(out prio) && needMore(CityResourceIndex.ConservedFood) &&
                                        CraftResourceLib.ConservedFood_Barrel.hasResources(this) &&
                                        work_isFreeTile(pos))
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.ConservedFood, 0, pos, prio, 0, distanceValue));
                                    }
                                    break;

                                case TerrainBuildingType.Work_Bench:
                                    craftBench(pos, distanceValue, CraftList.BenchCraftTypes, -5);
                                    break;
                                case TerrainBuildingType.Work_Smith:

                                    craftBench(pos, distanceValue, CraftList.SmithCraftTypes);
                                    break;

                                case TerrainBuildingType.Work_CoalPit:
                                    if (
                                       workTemplate.Get(WorkPriorityType.craftFuel).HasPrio() && needMore(CityResourceIndex.food) &&
                                       CraftResourceLib.Charcoal.hasResources(this) &&
                                       work_isFreeTile(pos))
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, 0, pos, workTemplate.Get(WorkPriorityType.craftFuel).value, 0, distanceValue));
                                    }
                                    break;

                                case TerrainBuildingType.Brewery:
                                    if (workTemplate.Get(WorkPriorityType.craftBeer).HasPrio() &&
                                        needMore(CityResourceIndex.beer) &&//res_beer.needMore() &&
                                        CraftResourceLib.Beer.hasResources(this) &&
                                        work_isFreeTile(pos))
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Beer, 0, pos, workTemplate.Get(WorkPriorityType.craftBeer).value, 0, distanceValue));
                                    }
                                    break;

                                case TerrainBuildingType.Carpenter:
                                    craftBench(pos, distanceValue, CraftList.CarpenterCraftTypes);
                                    break;
                                case TerrainBuildingType.Armory:
                                    craftBench(pos, distanceValue, CraftList.ArmoryCraftTypes);
                                    break;
                                case TerrainBuildingType.ShieldMaker:
                                    craftBench(pos, distanceValue, CraftList.ShieldCraftTypes);
                                    break;
                                case TerrainBuildingType.Pottery:
                                    craftBench(pos, distanceValue, CraftList.PotteryCraftTypes);
                                    break;
                                case TerrainBuildingType.Smelter:
                                    craftBench(pos, distanceValue, CraftList.SmelterCraftTypes);
                                    break;
                                case TerrainBuildingType.Foundry:
                                    craftBench(pos, distanceValue, CraftList.FoundryCraftTypes);
                                    break;
                                //case TerrainBuildingType.Butcher:
                                //    craftBench(pos, distanceValue, CraftList.ButcherCraftTypes);
                                //    break;
                                case TerrainBuildingType.Chemist:
                                    craftBench(pos, distanceValue, CraftList.ChemistCraftTypes);
                                    break;
                                case TerrainBuildingType.Gunmaker:
                                    craftBench(pos, distanceValue, CraftList.GunmakerCraftTypes);
                                    break;
                                case TerrainBuildingType.Smoker:
                                    if (
                                        workTemplate.Get(WorkPriorityType.craftConservedFood).HasPrio() && needMore(CityResourceIndex.ConservedFood) &&
                                        CraftResourceLib.ConservedFood_Smoked.hasResources(this) &&
                                        work_isFreeTile(pos)
                                        )
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.ConservedFood, 0, pos, workTemplate.Get(WorkPriorityType.craftConservedFood).value, 0, distanceValue));
                                    }
                                    break;
                                case TerrainBuildingType.Dryer:
                                    if (
                                        workTemplate.Get(WorkPriorityType.craftConservedFood).HasPrio() && needMore(CityResourceIndex.ConservedFood) &&
                                        CraftResourceLib.ConservedFood_Dried.hasResources(this) &&
                                        work_isFreeTile(pos)
                                        )
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.ConservedFood, 0, pos, workTemplate.Get(WorkPriorityType.craftConservedFood).value, 0, distanceValue));
                                    }
                                    break;
                                case TerrainBuildingType.DryingPan:
                                    if (
                                        workTemplate.Get(WorkPriorityType.miningSalt).HasPrio() && needMore(CityResourceIndex.Salt) &&
                                        work_isFreeTile(pos)
                                        )
                                    {
                                        workQue.Add(new WorkQueMember(WorkType.Mine, (int)ItemResourceType.Salt, 0, pos, workTemplate.Get(WorkPriorityType.miningSalt).value, 0, distanceValue));
                                    }
                                    break;
                                case TerrainBuildingType.Butcher:
                                    if (debugTagged || myIndex == 270)
                                    {
                                        lib.DoNothing();
                                    }
                                    itemConvert(pos, distanceValue, false);
                                    break;
                                case TerrainBuildingType.CoinMinter:
                                    itemConvert(pos, distanceValue, true);
                                    break;
                            }
                        }
                    }
                    //COINS
                    if (CityStructure.WorkInstance.CoinMinting.Count > 0)//foreach (var pos in CityStructure.WorkInstance.CoinMinting)
                    {
                        ItemResourceType topItem = ItemResourceType.NONE;
                        int topPrio = 0;

                        getMintPriority(workTemplate.Get(WorkPriorityType.coinmaker_copper), ItemResourceType.CopperCoin, Minting.CopperCoin);
                        getMintPriority(workTemplate.Get(WorkPriorityType.coinmaker_bronze), ItemResourceType.BronzeCoin, Minting.BronzeCoin);
                        getMintPriority(workTemplate.Get(WorkPriorityType.coinmaker_silver), ItemResourceType.SilverCoin, Minting.SilverCoin);
                        getMintPriority(workTemplate.Get(WorkPriorityType.coinmaker_mithril), ItemResourceType.ElfCoin, Minting.ElfCoin);

                        void getMintPriority(WorkPriority priority, ItemResourceType item, CraftBlueprint blueprint)
                        {
                            if (priority.value > topPrio && blueprint.hasResources(this))
                            {
                                topPrio = priority.value;
                                topItem = item;
                            }
                        }

                        if (topPrio > 0)
                        {
                            foreach (var pos in CityStructure.WorkInstance.CoinMinting)
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, 0, pos, topPrio, 0, distanceValue));
                            }
                        }
                    }

                    if (hostUpdate && (faction.player.IsBot() || automateCity))
                    {
                        workAutoBuild();
                    }

                    void craftBench(IntVector2 pos, int distanceValue, ItemResourceType[] types, int prioAdd = 0)
                    {
                        //int topPrioValue = WorkTemplate.NoPrio;
                        //ItemResourceType topItem = ItemResourceType.NONE;
                        //WorkPriority topPrio = WorkPriority.Empty;
                        //bool waitForFullStock = false;

                        foreach (var item in types)
                        {
                            WorkPriority template = workTemplate.GetWorkPriority(item, out _);
                            

                            if (template.unlocked &&  template.value > WorkTemplate.NoPrio/*&& template.value > topPrioValue*/)
                            {
                                //if (item == ItemResourceType.Gold)
                                //{
                                //    lib.DoNothing();
                                //}

                                ItemPropertyColl.Blueprint(item, out var bp1, out var bp2);
                                bool available = bp1.available(this);

                                if (!available && bp2 != null)
                                {
                                    available = bp2.available(this);
                                }

                                if (available && 
                                    GetGroupedResource(item).needMore() &&
                                    work_isFreeTile(pos))
                                {
                                    //topPrioValue = template.value;
                                    //topItem = item;
                                    //topPrio = template;
                                    workQue.Add(new WorkQueMember(WorkType.Craft, (int)item, 0, pos, template.value, prioAdd, distanceValue));
                                }

                                //if (topPrioValue > WorkTemplate.NoPrio &&
                                //    work_isFreeTile(pos))
                                //{
                                   
                                //}
                            }
                        }

                        
                    }

                    void itemConvert(IntVector2 pos, int distanceValue, bool coinMint)
                    {
                        int topPrioValue = WorkTemplate.NoPrio;
                        int topItem = -1;
                        WorkPriority topPrio = WorkPriority.Empty;

                        CraftBlueprint[] crafts = coinMint ? Minting.CoinCraftTypes : CraftList.ButcherAnimalCraftTypes;

                        foreach (var bp in crafts)
                        {
                            WorkPriority template = workTemplate.GetWorkPriority((ItemResourceType)bp.workTag, out _);
                            if (template.value > topPrioValue && (!template.waitForStockpile || bp.hasFullStock(this)) && bp.available(this))
                            {
                                topPrioValue = template.value;
                                topItem = bp.workTag;
                                topPrio = template;
                            }
                        }

                        if (topPrioValue > WorkTemplate.NoPrio &&
                            work_isFreeTile(pos))
                        {
                            workQue.Add(new WorkQueMember(WorkType.Craft, topItem, 0, pos, topPrioValue, 0, distanceValue));
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

                //            if (DssRef.world.diplomacy.MayTrade(nCity.faction, faction, out var relation))
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


            }
        }

        WorkerStatus newGameWorkerSkills(WorkerStatus newWorker)
        {
            newWorker.setXpFor(WorkExperienceType.Farm, DssConst.WorkXpToLevel);
            //newWorker.xpType1 = WorkExperienceType.Farm;
            //newWorker.xp1 = DssConst.WorkXpToLevel;

            if (Bound.IsWithin(workerStatuses.Count, 1, 2))
            {
                if (Ref.rnd.Chance(0.4f))
                {
                    //newWorker.xpType2 = arraylib.RandomListMember(XpLib.ExperienceTypes);
                    //newWorker.xp2 = DssConst.WorkXpToLevel;
                    newWorker.setXpFor(arraylib.RandomListMember(XpLib.ExperienceTypes), DssConst.WorkXpToLevel);
                }
            }
            else if (workerStatuses.Count == 3 && pfaction.TryGetFaction(out var f) && f.mainCity == this)
            {
                //newWorker.xpType2 = WorkExperienceType.HouseBuilding;
                //newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
                newWorker.setXpFor(WorkExperienceType.HouseBuilding, DssConst.WorkXpToLevel);
            }
            else if (workerStatuses.Count == 4)
            {
                WorkExperienceType cultureWork = WorkExperienceType.NUM_NONE;
                switch (cityCulture)
                {
                    case CityCulture.FertileGround:
                        cultureWork = WorkExperienceType.Farm;
                        break;
                    //case CityCulture.:
                    //    cultureWork = WorkExperienceType.AnimalCare;
                    //    break;
                    case CityCulture.Armorsmith:
                        cultureWork = WorkExperienceType.CraftArmor;
                        break;
                    case CityCulture.Brewmaster:
                        cultureWork = WorkExperienceType.Chemistry;
                        break;
                    case CityCulture.PitMasters:
                        cultureWork = WorkExperienceType.CraftFuel;
                        break;
                    case CityCulture.BronzeCasters:
                    case CityCulture.Smelters:
                        cultureWork = WorkExperienceType.Smelting;
                        break;
                    case CityCulture.Builders:
                        cultureWork = WorkExperienceType.HouseBuilding;
                        break;
                    case CityCulture.Miners:
                        cultureWork = WorkExperienceType.Mining;
                        break;
                    case CityCulture.Networker:
                        cultureWork = WorkExperienceType.Transport;
                        break;
                    case CityCulture.Archers:
                        cultureWork = WorkExperienceType.Fletcher;
                        break;
                    case CityCulture.Stonemason:
                        cultureWork = WorkExperienceType.StoneCutter;
                        break;
                    case CityCulture.SiegeEngineer:
                    case CityCulture.Woodcutters:
                        cultureWork = WorkExperienceType.WoodWork;
                        break;

                }

                if (cultureWork != WorkExperienceType.NUM_NONE)
                {
                    if (Ref.rnd.Chance(0.5f))
                    {
                        //newWorker.xpType2 = cultureWork;
                        //newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
                        newWorker.setXpFor(cultureWork, DssConst.WorkXpToLevel);
                    }
                }
                else
                {
                    if (Ref.rnd.Chance(0.05f))
                    {
                        //newWorker.xpType2 = arraylib.RandomListMember(XpLib.ExperienceTypes);
                        //newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
                        newWorker.setXpFor(arraylib.RandomListMember(XpLib.ExperienceTypes), DssConst.WorkXpToLevel);
                    }
                }
            }

            return newWorker;
        }

        bool work_isFreeTile(IntVector2 subtile)
            {
                for (int i = 0; i < workerStatuses.Count; ++i)
                {
                    var status = workerStatuses.array[i];
                    if (status.work != WorkType.Idle &&
                        status.subTileEnd == subtile)
                    {
                        return false;
                    }
                }

                return true;
            }

        public void checkPlayerFuelAccess_OnGamestart_async()
        {
            const int FuelFarmCount = 10;
            int fuelType = (int)TerrainSubFoilType.RapeSeedFarm;

            CityStructure structure = new CityStructure();
            structure.update(DssRef.world, this, 32, FuelFarmCount);
            if (structure.fuelSpots <= 8)
            {
                //int count = Math.Min(structure.EmptyLand.Count, FuelFarmCount);
                for (int i = 0; i < FuelFarmCount; ++i)
                {
                    if (structure.NextEmptyLand(this, Ref.peRnd.Int(64), out var freeSubTilePos))
                    {
                        BuildLib.TryAutoBuild(pfaction.GetFaction(), freeSubTilePos, TerrainMainType.Foil, fuelType, Ref.peRnd.Int(1, TerrainContent.FarmCulture_MaxSize));
                    }
                }
            }
        }


        void findLowXpWorkers()
        {
            

            // Value1 = Index
            // Value2 = Score
            TwoInts lowest1 = new TwoInts(-1, int.MaxValue);
            TwoInts lowest2 = new TwoInts(-1, int.MaxValue);
            TwoInts lowest3 = new TwoInts(-1, int.MaxValue);

            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses.array[i].work != WorkType.IsDeleted)
                {
                    int score = workerStatuses.array[i].GetXpScore();

                    // Check against lowest1
                    if (lowest1.Value1 < 0 || score < lowest1.Value2)
                    {
                        // Shift down
                        lowest3 = lowest2;
                        lowest2 = lowest1;

                        // Assign new lowest1
                        lowest1.Value1 = i;
                        lowest1.Value2 = score;
                    }
                    // Check against lowest2
                    else if (lowest2.Value1 < 0 || score < lowest2.Value2)
                    {
                        // Shift down
                        lowest3 = lowest2;

                        // Assign new lowest2
                        lowest2.Value1 = i;
                        lowest2.Value2 = score;
                    }
                    // Check against lowest3
                    else if (lowest3.Value1 < 0 || score < lowest3.Value2)
                    {
                        // Assign new lowest3
                        lowest3.Value1 = i;
                        lowest3.Value2 = score;
                    }
                }
            }

            markedForExit.Clear();
            if (lowest3.Value1 >= 0)
            {
                markedForExit.Add(lowest3.Value1);
            }
            if (lowest2.Value1 >= 0)
            {
                markedForExit.Add(lowest2.Value1);
            }
            if (lowest1.Value1 >= 0)
            {
                markedForExit.Add(lowest1.Value1);
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }

        void async_blackMarketUpdate()
        {
            if (pfaction.GetFaction() == null)
            { return; }

            ref var food = ref GetRefGroupedResource(CityResourceIndex.food);

            if (food.amount <= -10)
            {
                //if (GetPlayer().IsLocalPlayer())
                //{
                //    lib.DoNothing();
                //}

                if (GetCasual())
                {
                    food.amount = 100;
                    return;
                }

                int buyFood = -food.amount;

                int cost = (int)(buyFood * DssConst.FoodGoldValue_BlackMarket);
                pfaction.GetFaction().payGold(cost, true, this);
                blackMarketCosts_food.add(cost);
                food.amount += buyFood;

                starving = true;
            }
            else if (food.amount > 10)
            {
                starving = false;
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
                        ItemPropertyColl.Blueprint(item, out var bp1, out var bp2);
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

                case WorkType.Upgrade:
                case WorkType.Build:
                    {
                        //if (work == WorkType.Upgrade)
                        //{
                        //    lib.DoNothing();
                        //}
                        return BuildLib.BuildOptions[subWork].availableBlueprintResources(this);
                    }

                default:
                    return true;
            }
        }

        public bool workerInSchoolCheckup(int idAndPosition, out float completeTimeSec)
        {
            IntVector2 pos = conv.IntToIntVector2(idAndPosition);
            //Warning! this checkup is badly optimized, only use it for player info
            for (int i = 0; i < workerStatuses.Count; ++i)
            {
                var status = workerStatuses.array[i];
                if (status.work == WorkType.School &&
                    status.subTileEnd == pos)
                {
                    completeTimeSec = status.processTimeStartStampSec + status.processTimeLengthSec;
                    return true;
                }
            }
            completeTimeSec = -1;
            return false;
        }

    } 

    struct WorkQueMember
    {
        public static readonly WorkQueMember NoPrio = new WorkQueMember() { priority = int.MinValue };

        public WorkType work;
        public int subWork;
        public IntVector2 subTile;
        public byte workBonus;
        public int orderId = -1;

        /// <summary>
        /// Goes from 1:lowest to 10: highest
        /// </summary>
        public int priority;

        public WorkQueMember(WorkType work, int subWork, byte workBonus, IntVector2 subTile, int priority, int midPrio, int subPrio)
        {
            this.work = work;
            this.subWork = subWork;
            this.workBonus = workBonus;
            this.subTile = subTile;
            this.priority = priority * 1000000 + midPrio * 1000 + subPrio;
        }
    }
        
}
