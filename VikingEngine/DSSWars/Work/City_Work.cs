using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
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
        public int WorkerStats_TotalUnits => workerStatuses.Count;

        //public bool mintOnFullStockProperty(object tag, bool set, bool value)
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
            if (factionIndex < 0)
            {
                CityStructure.WorkInstance.update(this, 0);
                return; 
            }

            var faction = GetFaction();
            if (faction == null)
            {
                return;
            }

            lock (workerStatuses.array)
            {
                bool hostUpdate = DssRef.state.host || faction.player.IsLocalPlayer();

                CityStructure.WorkInstance.newCity = true;
                //WaitingHighSkillJobs.Clear();

                async_blackMarketUpdate();

                int workerStatusActiveCount = workerStatuses.Count;
                int deletedCount = 0;
                int idleCount = 0;
                //IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
                //IntVector2 maxpos = minpos;
                Intvector2MinMax minMax = new Intvector2MinMax(WP.ToSubTilePos_Centered(tilePos));

                //for (int i = 0; i < MaxSkill.Length; ++i)
                //{
                //    MaxSkill[i] = 0;
                //}

                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    var status = workerStatuses[i];
                    SkillCollector.Add(ref status);

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
                    minMax.Next(ref status.subTileEnd);
                 
                }

                cityExperienceLevels = SkillCollector.ExportData();
                //topskill_Farm = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Farm]);
                //topskill_AnimalCare = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.AnimalCare]);
                //topskill_HouseBuilding = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.HouseBuilding]);
                //topskill_WoodCutter = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.WoodWork]);
                //topskill_StoneCutter = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.StoneCutter]);
                //topskill_Mining = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Mining]);
                //topskill_Transport = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Transport]);
                //topskill_Cook = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Cook]);
                //topskill_Fletcher = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Fletcher]);
                //topskill_Smelting = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Smelting]);
                //topskill_Casting = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CastMetal]);
                //topskill_CraftMetal = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftMetal]);
                //topskill_CraftArmor = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftArmor]);
                ////topskill_CraftWeapon = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftWeapon]);
                //topskill_CraftFuel = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftFuel]);
                //topskill_Chemistry = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Chemistry]);

                //cullingTopLeft = WP.SubtileToTilePos(minMax.min);
                //cullingBottomRight = WP.SubtileToTilePos(minMax.max);
                workerCullingMinMax = new Intvector2MinMax(WP.SubtileToTilePos(minMax.min), WP.SubtileToTilePos(minMax.max));

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
                            newWorker = newGameWorkerSkills(newWorker);
                        }
                        else if (Culture == CityCulture.Apprentices)
                        {
                            for (int xpIx = 0; xpIx <= 1; ++xpIx)
                            {
                                var exp = arraylib.RandomListMember(XpLib.ExperienceTypes);
                                var lvl = (ExperienceLevel)cityExperienceLevels.Get(exp).maxLevel;//XpLib.ToLevel(MaxSkill[(int)exp]);
                                if (lvl >= ExperienceLevel.Expert_3)
                                {
                                    if (xpIx == 0)
                                    {
                                        newWorker.xpType1 = exp;
                                        newWorker.xp1 = DssConst.WorkXpToLevel;
                                    }
                                    else
                                    {
                                        if (exp != newWorker.xpType1)
                                        {
                                            newWorker.xpType2 = exp;
                                            newWorker.xp2 = DssConst.WorkXpToLevel;
                                        }
                                    }
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
                                    deletedIx = di - 1;
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
                    if (myIndex == 715 || debugTagged)
                    {
                        lib.DoNothing();
                    }

                    CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                    buildWorkQue2();
                    //Last position = highest priority
                    if (workQue.Count > 1)
                    {
                        workQue.Sort((a, b) => a.priority.CompareTo(b.priority));
                    }
                    //WorkerStats_WorkQueueLength = workQue.Count;
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
                        if (workerStatusActiveCount > workForce.amount)
                        {
                            --workerStatusActiveCount;
                            
                            status.createWorkOrder(WorkType.Exit, -1, 0, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        }
                        else if (status.carry.amount > 0)
                        {
                            CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                            status.createWorkOrder(WorkType.DropOff, -1, 0, WorkExperienceType.Transport, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        }
                        else if (status.energy < 0 && (resourceAmount(CityResoureIndex.food)/*res_food.amount*/ > 0 || faction.hasGold(1, this)))
                        {
                            CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                            status.createWorkOrder(WorkType.Eat, -1, 0, WorkExperienceType.NONE, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        }
                        else if (status.energy <= DssConst.Worker_Starvation)
                        {
                            --workerStatusActiveCount;
                            --workForce.amount;

                            status.createWorkOrder(WorkType.Starving, -1, 0, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        }
                        else
                        {
                            idleWorkers.Add(i);
                        }
                    }
                }

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

                        if (requiredLvl == ExperienceLevel.Beginner_1 || requiredLvl <= (ExperienceLevel)cityExperienceLevels.Get(experienceType).maxLevel/*GetTopSkill(experienceType)*/)
                        {

                            int bestWorkerListIx = -1;
                            int bestvalue = int.MaxValue;

                            for (int i = 0; i < idleWorkers.Count; ++i)
                            {
                                var worderIx = idleWorkers[i];
                                var worker = workerStatuses.array[worderIx];

                                var xp = worker.getXpFor(experienceType);

                                if (xp >= xpRequired && xp < maxXp)
                                {
                                    var distance = work.subTile.SideLength(worker.subTileEnd);
                                    int value = distance * distanceValue - xp * experienceValue;

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
                                //workerStatuses[worderIx] = status;
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

                    IntVector2 center = WP.ToSubTilePos_Centered(tilePos);
                    workQue.Clear();

                    //if (debugTagged || parentArrayIndex == 218)
                    //{
                    //    lib.DoNothing();
                    //}

                    bool foodSafeGuard = foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);

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
                        if (workTemplate.Get(WorkPriorityType.move).HasPrio() || woodSafeGuard)
                        {
                            foreach (var pos in CityStructure.WorkInstance.ResourceOnGround)
                            {
                                var subTile = DssRef.world.subTileGrid.Get(pos);

                                if (subTile.collectionPointer >= 0)
                                {
                                    var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                                    var resource = chunk.peek();

                                    if (needMore(resource.type, rawFoodSafeGuard, woodSafeGuard, out bool usesSafeGuard) && work_isFreeTile(pos))
                                    {
                                        int distanceValue = -center.SideLength(pos);
                                        workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, 0, pos, usesSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get(WorkPriorityType.move).value, 0, distanceValue));
                                    }
                                }
                            }
                        }
                    }

                    //WOOD
                    if ((workTemplate.Get(WorkPriorityType.wood).HasPrio() && needMore(CityResoureIndex.wood)/*res_wood.needMore()*/) || woodSafeGuard)
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
                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, bonus, pos, woodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get(WorkPriorityType.wood).value, bonus, distanceValue));
                            }
                        }
                    }

                    //STONE
                    if (workTemplate.Get(WorkPriorityType.stone).HasPrio() &&
                        needMore(CityResoureIndex.stone)/*res_stone.needMore()*/)
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
                        bool safeGuard = false;
                        var subTile = DssRef.world.subTileGrid.Get(tilework.subtile);
                        int prio = 0;
                        byte bonus = 0;
                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.TreeApple:
                            case TerrainSubFoilType.TreeBanana:
                                safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.food);
                                prio = workTemplate.Get(WorkPriorityType.farmfood).value;
                                break;

                            case TerrainSubFoilType.LinenFarm:
                                bNeedMore = needMore(CityResoureIndex.skinLinnen);//res_skinLinnen.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmlinen).value;
                                break;
                            case TerrainSubFoilType.LinenFarmUpgraded:
                                bNeedMore = needMore(CityResoureIndex.skinLinnen);//res_skinLinnen.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmlinen).value;

                                break;
                            case TerrainSubFoilType.WheatFarm:
                                safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.rawFood);
                                prio = workTemplate.Get(WorkPriorityType.farmfood).value;
                                break;
                            case TerrainSubFoilType.WheatFarmUpgraded:
                                safeGuard = rawFoodSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.rawFood);//res_rawFood.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmfood).value;
                                bonus = 1;
                                break;
                            case TerrainSubFoilType.RapeSeedFarm:
                                safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.fuel);//res_fuel.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmfuel).value;
                                break;
                            case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.fuel);////res_fuel.needMore();
                                prio = workTemplate.Get(WorkPriorityType.farmfuel).value;
                                break;
                            case TerrainSubFoilType.HempFarm:
                                safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.fuel);////res_fuel.needMore() || res_skinLinnen.needMore() || fuelSafeGuard;
                                prio = Math.Max(workTemplate.Get(WorkPriorityType.farmlinen).value, workTemplate.Get(WorkPriorityType.farmfuel).value);
                                break;
                            case TerrainSubFoilType.HempFarmUpgraded:
                                safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.fuel) || needMore(CityResoureIndex.skinLinnen)/*res_fuel.needMore() || res_skinLinnen.needMore()*/ || fuelSafeGuard;
                                prio = Math.Max(workTemplate.Get(WorkPriorityType.farmlinen).value, workTemplate.Get(WorkPriorityType.farmfuel).value);
                                break;
                        }

                        if (((bNeedMore && prio > WorkTemplate.NoPrio) || safeGuard) && work_isFreeTile(tilework.subtile))
                        {
                            int distanceValue = -center.SideLength(tilework.subtile);
                            workQue.Add(new WorkQueMember(tilework.workType, NoSubWork, bonus, tilework.subtile, safeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get( WorkPriorityType.farmfood).value, 0, distanceValue));
                        }
                    }

                    //MINING
                    if (workTemplate.Get(WorkPriorityType.bogiron).HasPrio() &&
                        needMore(CityResoureIndex.ironore)/*res_ironore.needMore()*/)
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

                    //if (fuelSafeGuard)
                    //{
                    foreach (var pos in CityStructure.WorkInstance.Mines)
                    {
                        bool bNeedMore = true;
                        bool safeGuard = false;

                        WorkPriority priority;
                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch ((TerrainMineType)subTile.subTerrain)
                        {
                            default://case TerrainMineType.IronOre:
                                bNeedMore = needMore(CityResoureIndex.ironore);//res_ironore.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningIron);
                                break;
                            case TerrainMineType.TinOre:
                                bNeedMore = needMore(CityResoureIndex.TinOre);
                                priority = workTemplate.Get(WorkPriorityType.miningTin);
                                break;
                            case TerrainMineType.CopperOre:
                                bNeedMore = needMore(CityResoureIndex.CopperOre);//res_CupperOre.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningCopper);
                                break;
                            case TerrainMineType.LeadOre:
                                bNeedMore = needMore(CityResoureIndex.LeadOre);//res_LeadOre.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningLead);
                                break;
                            case TerrainMineType.SilverOre:
                                bNeedMore = needMore(CityResoureIndex.SilverOre);//res_Silver.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningSilver);
                                break;
                            case TerrainMineType.Salt:
                                bNeedMore = needMore(CityResoureIndex.Salt);
                                priority = workTemplate.Get(WorkPriorityType.miningSalt);
                                break;
                            case TerrainMineType.Sulfur:
                                bNeedMore = needMore(CityResoureIndex.Sulfur);
                                priority = workTemplate.Get(WorkPriorityType.miningSulfur);
                                break;
                            case TerrainMineType.GoldOre:
                                bNeedMore = true;
                                priority = workTemplate.Get(WorkPriorityType.miningGold);
                                break;
                            case TerrainMineType.Mithril:
                                bNeedMore = needMore(CityResoureIndex.RawMithril);//res_RawMithril.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningMithril);
                                break;
                            case TerrainMineType.Coal:
                                //++fuelSpots;
                                safeGuard = fuelSafeGuard;
                                bNeedMore = needMore(CityResoureIndex.fuel);//res_fuel.needMore();
                                priority = workTemplate.Get(WorkPriorityType.miningCoal);
                                break;
                        }

                        if (priority.HasPrio() && (bNeedMore || safeGuard) && work_isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, 0, pos, safeGuard ? WorkTemplate.SafeGuardPrio : priority.value, 0, distanceValue));
                        }
                    }


                    //ANIMALS
                    if (workTemplate.Get(WorkPriorityType.farmfood).HasPrio() || rawFoodSafeGuard)
                    {
                        foreach (var pos in CityStructure.WorkInstance.AnimalPens)
                        {
                            bool bNeedMore = true;

                            var subTile = DssRef.world.subTileGrid.Get(pos);
                            switch (subTile.GetBuildingType())
                            {
                                case TerrainBuildingType.HenPen:
                                    bNeedMore = needMore(CityResoureIndex.rawFood);//res_rawFood.needMore();
                                    break;
                                case TerrainBuildingType.PigPen:
                                    bNeedMore = needMore(CityResoureIndex.rawFood) || needMore(CityResoureIndex.skinLinnen);// res_rawFood.needMore() || res_skinLinnen.needMore();
                                    break;
                            }

                            if ((bNeedMore || rawFoodSafeGuard) && work_isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, 0, pos, rawFoodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get(WorkPriorityType.farmfood).value, 0, distanceValue));
                            }
                        }
                    }

                    //CRAFT
                    foreach (var pos in CityStructure.WorkInstance.CraftStation)
                    {
                        int distanceValue = -center.SideLength(pos);
                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        var building = subTile.GetBuildingType();
                        switch (building)
                        {
                            case TerrainBuildingType.Work_Cook:
                                if (
                                    ((workTemplate.Get(WorkPriorityType.craftFood).HasPrio() && needMore(CityResoureIndex.food)/*res_food.needMore()*/) || foodSafeGuard) &&
                                    (CraftResourceLib.Food2.hasResources(this) || CraftResourceLib.Food1.hasResources(this)) &&
                                    work_isFreeTile(pos))
                                {
                                    workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, 0, pos, foodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get(WorkPriorityType.craftFood).value, 0, distanceValue));
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
                                    ((workTemplate.Get(WorkPriorityType.craftFuel).HasPrio() && needMore(CityResoureIndex.food)/*res_fuel.needMore()*/) || fuelSafeGuard) &&
                                   CraftResourceLib.Charcoal.hasResources(this) &&
                                   work_isFreeTile(pos))
                                {
                                    workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, 0, pos, fuelSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.Get(WorkPriorityType.craftFuel).value, 0, distanceValue));
                                }
                                break;

                            case TerrainBuildingType.Brewery:
                                if (workTemplate.Get(WorkPriorityType.craftBeer).HasPrio() &&
                                    needMore(CityResoureIndex.beer) &&//res_beer.needMore() &&
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
                            case TerrainBuildingType.Butcher:
                                itemConvert(pos, distanceValue, false);
                                break;
                            case TerrainBuildingType.CoinMinter:
                                itemConvert(pos, distanceValue, true);
                                break;
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

                    if (hostUpdate)
                    {
                        workAutoBuild(fuelSafeGuard, rawFoodSafeGuard);
                    }

                    void craftBench(IntVector2 pos, int distanceValue, ItemResourceType[] types, int prioAdd = 0)
                    {
                        int topPrioValue = WorkTemplate.NoPrio;
                        ItemResourceType topItem = ItemResourceType.NONE;
                        WorkPriority topPrio = WorkPriority.Empty;
                        //bool waitForFullStock = false;

                        foreach (var item in types)
                        {
                            WorkPriority template = workTemplate.GetWorkPriority(item, out _);
                            

                            if (template.unlocked && template.value > topPrioValue)
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

                                if (available && GetGroupedResource(item).needMore())
                                {
                                    topPrioValue = template.value;
                                    topItem = item;
                                    topPrio = template;
                                }
                            }
                        }

                        if (topPrioValue > WorkTemplate.NoPrio &&
                            work_isFreeTile(pos))
                        {
                            workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, 0, pos, topPrioValue, prioAdd, distanceValue));
                        }
                    }

                    void itemConvert(IntVector2 pos, int distanceValue, bool coinMint)
                    {
                        int topPrioValue = WorkTemplate.NoPrio;
                        int topItem = -1;
                        WorkPriority topPrio = WorkPriority.Empty;

                        CraftBlueprint[] crafts = coinMint ? Minting.CoinCraftTypes : CraftList.ButcherAnimalTypes;

                        foreach (var bp in crafts)
                        {
                            WorkPriority template = workTemplate.GetWorkPriority((ItemResourceType)bp.workTag, out _);
                            if ((!template.waitForStockpile || bp.hasFullStock(this)) && bp.available(this))
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


            }
        }

        private WorkerStatus newGameWorkerSkills(WorkerStatus newWorker)
        {
            newWorker.xpType1 = WorkExperienceType.Farm;
            newWorker.xp1 = DssConst.WorkXpToLevel;

            if (Bound.IsWithin(workerStatuses.Count, 1, 2))
            {
                if (Ref.rnd.Chance(0.4f))
                {
                    newWorker.xpType2 = arraylib.RandomListMember(XpLib.ExperienceTypes);
                    newWorker.xp2 = DssConst.WorkXpToLevel;
                }
            }
            else if (workerStatuses.Count == 3 && TryGetFaction(out var f) && f.mainCity == this)
            {
                newWorker.xpType2 = WorkExperienceType.HouseBuilding;
                newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
            }
            else if (workerStatuses.Count == 4)
            {
                WorkExperienceType cultureWork = WorkExperienceType.NONE;
                switch (Culture)
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

                if (cultureWork != WorkExperienceType.NONE)
                {
                    if (Ref.rnd.Chance(0.5f))
                    {
                        newWorker.xpType2 = cultureWork;
                        newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
                    }
                }
                else
                {
                    if (Ref.rnd.Chance(0.05f))
                    {
                        newWorker.xpType2 = arraylib.RandomListMember(XpLib.ExperienceTypes);
                        newWorker.xp2 = (byte)DssConst.WorkLevel_Expert;
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
            structure.update(this, 32, FuelFarmCount);
            if (structure.fuelSpots <= 8)
            {
                //int count = Math.Min(structure.EmptyLand.Count, FuelFarmCount);
                for (int i = 0; i < FuelFarmCount; ++i)
                {
                    if (structure.NextEmptyLand(this, Ref.peRnd.Int(64), out var freeSubTilePos))
                    {
                        BuildLib.TryAutoBuild(freeSubTilePos, TerrainMainType.Foil, fuelType, Ref.peRnd.Int(1, TerrainContent.FarmCulture_MaxSize));
                    }
                }
            }
        }

        protected override void onWorkComplete_async(ref WorkerStatus status)
        {
            status.WorkComplete(this, false);
        }

        void async_blackMarketUpdate()
        {
            if (GetFaction() == null)
            { return; }

            ref var food = ref GetRefGroupedResource(CityResoureIndex.food);

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
                GetFaction().payGold(cost, true, this);
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
