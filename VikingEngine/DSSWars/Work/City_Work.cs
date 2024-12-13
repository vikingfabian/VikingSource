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
using VikingEngine.DSSWars.XP;
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
             
        public void async_workUpdate()
        {
            CityStructure.WorkInstance.newCity = true;

            async_blackMarketUpdate();

            int workerStatusActiveCount = workerStatuses.Count;
            int deletedCount = 0;
            int idleCount = 0;
            IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
            IntVector2 maxpos = minpos;

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

            topskill_Farm = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Farm]);
            topskill_AnimalCare = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.AnimalCare]);
            topskill_HouseBuilding = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.HouseBuilding]);
            topskill_WoodCutter = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.WoodWork]);
            topskill_StoneCutter = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.StoneCutter]);
            topskill_Mining = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Mining]);
            topskill_Transport = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Transport]);
            topskill_Cook = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Cook]);
            topskill_Fletcher = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.Fletcher]);
            topskill_CraftMetal = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftMetal]);
            topskill_CraftArmor = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftArmor]);
            topskill_CraftWeapon = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftWeapon]);
            topskill_CraftFuel = XpLib.ToLevel(MaxSkill[(int)WorkExperienceType.CraftFuel]);

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
                    else if (Culture == CityCulture.Apprentices)
                    {
                        for (int xpIx = 0; xpIx <= 1; ++xpIx)
                        {
                            var exp = arraylib.RandomListMember(XpLib.ExperienceTypes);
                            var lvl = XpLib.ToLevel(MaxSkill[(int)exp]);
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

            //Collect idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatusActiveCount > workForce.amount)
                    {
                        --workerStatusActiveCount;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Exit, -1, 0, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].carry.amount > 0)
                    {
                        CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        //status.createWorkOrder(WorkType.DropOff, -1, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        status.createWorkOrder(WorkType.DropOff, -1, 0, WorkExperienceType.Transport, -1, CityStructure.WorkInstance.storePosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0 && (res_food.amount > 0 || faction.gold > 0))
                    {
                        CityStructure.WorkInstance.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        //status.createWorkOrder(WorkType.Eat, -1, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        status.createWorkOrder(WorkType.Eat, -1, 0, WorkExperienceType.NONE, -1, CityStructure.WorkInstance.eatPosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy <= DssConst.Worker_Starvation)
                    {
                        --workerStatusActiveCount;
                        --workForce.amount;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Starving, -1, 0, WorkExperienceType.NONE, -1, WP.ToSubTilePos_Centered(tilePos), this);
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

            while (workQue.Count > 0 && idleWorkers.Count > 0)
            {
                var work = arraylib.PullLastMember(workQue);

                if (checkAvailable(work.work, work.subWork) &&
                    isFreeTile(work.subTile))
                {                    
                    WorkExperienceType experienceType = WorkLib.WorkToExperienceType(work.work, work.subWork, work.workBonus, work.subTile, this,
                        out int xpRequired, out int maxXp);

                    int bestWorkerListIx = -1;
                    int bestvalue = int.MaxValue;

                    for (int i = 0; i < idleWorkers.Count; ++i)
                    {
                        var worderIx = idleWorkers[i];
                        var worker = workerStatuses[worderIx];
                        
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

//                    if (bestWorkerListIx == -1)
//                    {
//#if DEBUG
//                        throw new Exception();
//#else
//                        return;
//#endif
//                    }

                    if (bestWorkerListIx >= 0)
                    {//Assign job
                        var worderIx = idleWorkers[bestWorkerListIx];
                        idleWorkers.RemoveAt(bestWorkerListIx);

                        var status = workerStatuses[worderIx];
                        status.createWorkOrder(work.work, work.subWork, work.workBonus, experienceType, work.orderId, work.subTile, this);
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

                if (debugTagged)
                {
                    lib.DoNothing();
                }

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
                                workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, 0, pos, usesSafeGuard? WorkTemplate.SafeGuardPrio : workTemplate.move.value, 0, distanceValue));
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

                            byte bonus = 0;
                            if (CityStructure.WorkInstance.inBonusRadius(pos, CityStructure.WorkInstance.WoodCutter, DssConst.WoodCutter_BonusRadius))
                            {
                                bonus = DssConst.WoodCutter_WoodBonus;
                            }
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, bonus, pos, woodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.wood.value, bonus, distanceValue));
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
                            byte bonus = 0;
                            if (CityStructure.WorkInstance.inBonusRadius(pos, CityStructure.WorkInstance.StoneCutter, DssConst.StoneCutter_BonusRadius))
                            {
                                bonus = DssConst.StoneCutter_StoneBonus;
                            }
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, bonus, pos, workTemplate.stone.value, bonus, distanceValue));
                        }
                    }
                }

                //FARMS
                foreach (var tilework in CityStructure.WorkInstance.Farms)
                {
                    bool needMore = false;
                    bool safeGuard = false;
                    var subTile = DssRef.world.subTileGrid.Get(tilework.subtile);
                    int prio = 0;
                    byte bonus = 0;
                    switch (subTile.GetFoilType())
                    {
                        case TerrainSubFoilType.LinenFarm:
                            needMore = res_skinLinnen.needMore();
                            prio = workTemplate.farm_linen.value;
                            break;
                        case TerrainSubFoilType.LinenFarmUpgraded:
                            needMore = res_skinLinnen.needMore();
                            prio = workTemplate.farm_linen.value;
                            
                            break;
                        case TerrainSubFoilType.WheatFarm:
                            safeGuard = rawFoodSafeGuard;
                            needMore = res_rawFood.needMore();
                            prio = workTemplate.farm_food.value;
                            break;
                        case TerrainSubFoilType.WheatFarmUpgraded:
                            safeGuard = rawFoodSafeGuard;
                            needMore = res_rawFood.needMore();
                            prio = workTemplate.farm_food.value;
                            bonus = 1;
                            break;
                        case TerrainSubFoilType.RapeSeedFarm:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore();
                            prio = workTemplate.farm_fuel.value;
                            break;
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore();
                            prio = workTemplate.farm_fuel.value;
                            break;
                        case TerrainSubFoilType.HempFarm:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore() || res_skinLinnen.needMore() || fuelSafeGuard;
                            prio = Math.Max(workTemplate.farm_linen.value, workTemplate.farm_fuel.value);
                            break;
                        case TerrainSubFoilType.HempFarmUpgraded:
                            safeGuard = fuelSafeGuard;
                            needMore = res_fuel.needMore() || res_skinLinnen.needMore() || fuelSafeGuard;
                            prio = Math.Max(workTemplate.farm_linen.value, workTemplate.farm_fuel.value);
                            break;
                    }

                    if (((needMore && prio > WorkTemplate.NoPrio) || safeGuard) && isFreeTile(tilework.subtile))
                    {
                        int distanceValue = -center.SideLength(tilework.subtile);
                        workQue.Add(new WorkQueMember(tilework.workType, NoSubWork, bonus, tilework.subtile, safeGuard? WorkTemplate.SafeGuardPrio : workTemplate.farm_food.value,0, distanceValue));
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
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, 0, pos, workTemplate.bogiron.value,0, distanceValue));
                        }
                    }
                }

                if (fuelSafeGuard)
                {
                    foreach (var pos in CityStructure.WorkInstance.Mines)
                    {
                        bool needMore = true;
                        bool safeGuard = false;

                        WorkPriority priority;
                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch ((TerrainMineType)subTile.subTerrain)
                        {
                            default://case TerrainMineType.IronOre:
                                needMore = res_ironore.needMore();
                                priority = workTemplate.mining_iron;
                                break;
                            case TerrainMineType.TinOre:
                                needMore = res_TinOre.needMore();
                                priority = workTemplate.mining_tin;
                                break;
                            case TerrainMineType.CupperOre:
                                needMore = res_CupperOre.needMore();
                                priority = workTemplate.mining_cupper;
                                break;
                            case TerrainMineType.LeadOre:
                                needMore = res_LeadOre.needMore();
                                priority = workTemplate.mining_lead;
                                break;
                            case TerrainMineType.SilverOre:
                                needMore = res_ironore.needMore();
                                priority = workTemplate.mining_silver;
                                break;
                            case TerrainMineType.Sulfur:
                                needMore = res_Sulfur.needMore();
                                priority = workTemplate.mining_sulfur;
                                break;
                            case TerrainMineType.GoldOre:
                                needMore = true;
                                priority = workTemplate.mining_gold;
                                break;
                            case TerrainMineType.Mithril:
                                needMore = res_RawMithril.needMore();
                                priority = workTemplate.mining_mithril;
                                break;
                            case TerrainMineType.Coal:
                                //++fuelSpots;
                                safeGuard = fuelSafeGuard;
                                needMore = res_fuel.needMore();
                                priority = workTemplate.mining_coal;
                                break;
                        }

                        if (priority.HasPrio() && (needMore || safeGuard) && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, 0, pos, safeGuard ? WorkTemplate.SafeGuardPrio : priority.value, 0, distanceValue));
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
                            workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, 0, pos, rawFoodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.farm_food.value,0, distanceValue));
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
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, 0, pos, foodSafeGuard ? WorkTemplate.SafeGuardPrio : workTemplate.craft_food.value,0, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Work_Bench:
                            craftBench(pos, distanceValue, CraftBuildingLib.BenchCraftTypes, -5);
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
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, 0, pos, fuelSafeGuard? WorkTemplate.SafeGuardPrio : workTemplate.craft_fuel.value,0, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Brewery:
                            if (workTemplate.craft_beer.HasPrio() &&
                                res_beer.needMore() &&
                                CraftResourceLib.Beer.hasResources(this) &&
                                isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Beer, 0, pos, workTemplate.craft_beer.value,0, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Carpenter:
                            craftBench(pos, distanceValue, CraftBuildingLib.CarpenterCraftTypes);
                            break;
                    }
                }

                //COINS
                if (CityStructure.WorkInstance.CoinMinting.Count > 0)//foreach (var pos in CityStructure.WorkInstance.CoinMinting)
                {
                    ItemResourceType topItem = ItemResourceType.NONE;
                    int topPrio = 0;

                    getMintPriority(workTemplate.coinmaker_cupper, ItemResourceType.CupperCoin, ResourceLib.CupperCoin);
                    getMintPriority(workTemplate.coinmaker_bronze, ItemResourceType.BronzeCoin, ResourceLib.BronzeCoin);
                    getMintPriority(workTemplate.coinmaker_silver, ItemResourceType.SilverCoin, ResourceLib.SilverCoin);
                    getMintPriority(workTemplate.coinmaker_mithril, ItemResourceType.ElfCoin, ResourceLib.ElfCoin);

                    void getMintPriority(WorkPriority priority, ItemResourceType  item, CraftBlueprint blueprint)
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
                                workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.autoBuild.value,0, distanceValue));
                            }
                        }
                    }
                }


                bool checkAutoBuildAvailable()
                {
                    if (buildingStructure.buildingLevel_logistics < 2)
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
                            ItemPropertyColl.Blueprint(item, out var bp1, out var bp2);
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
                        workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, 0, pos, topPrioValue, prioAdd, distanceValue));
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
