using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        static readonly ItemResourceType[] IronCraftTypes = { ItemResourceType.Iron_G, ItemResourceType.MediumArmor, ItemResourceType.HeavyArmor, ItemResourceType.Sword, ItemResourceType.TwoHandSword, ItemResourceType.KnightsLance };
        static readonly ItemResourceType[] BenchCraftTypes = { ItemResourceType.Fuel_G, ItemResourceType.LightArmor, ItemResourceType.SharpStick, ItemResourceType.Bow };
        static readonly ItemResourceType[] CarpenterCraftTypes = { ItemResourceType.SharpStick, ItemResourceType.Bow, ItemResourceType.LongBow, ItemResourceType.Ballista };
        public WorkTemplate workTemplate = new WorkTemplate();

        const int NoSubWork = -1;
        public const int WorkTeamSize = 8;
        TimeStamp previousWorkQueUpdate = TimeStamp.None;
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        bool starving = false;
        //public void workTab(Players.LocalPlayer player,RichBoxContent content)
        //{
            
        //}

        public void async_workUpdate()
        {
            CityStructure.Singleton.newCity = true;

            async_blackMarketUpdate();

            int workerStatusActiveCount = workerStatuses.Count;
            int deletedCount = 0;
            int idleCount = 0;
            IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
            IntVector2 maxpos = minpos;

            res_water.clearOrders();
            res_wood.clearOrders();
            res_stone.clearOrders();
            res_rawFood.clearOrders();
            res_skinLinnen.clearOrders();
            res_ironore.clearOrders();
            //waterSpendOrders = 0;
            

            for (int i = 0; i < workerStatuses.Count; i++)
            {
                var status = workerStatuses[i];

                
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
                        status.checkAvailableAndBackOrder(status.work, status.workSubType, this);
                        break;
                    //case WorkType.Build:
                    //    var build= BuildLib.BuildOptions[status.workSubType];
                    //    //var blueprint = ResourceLib.Blueprint((TerrainBuildingType)status.workSubType);
                    //    build.blueprint.createBackOrder(this);
                    //    break;

                    //case WorkType.Craft:
                    //    switch (status.workSubType)
                    //    {
                    //        case WorkerStatus.Subwork_Craft_Food:
                    //            ResourceLib.CraftFood.createBackOrder(this);
                    //            break;

                    //        case WorkerStatus.Subwork_Craft_Iron:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;

                    //        case WorkerStatus.Subwork_Craft_SharpStick:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;
                    //        case WorkerStatus.Subwork_Craft_Sword:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;
                    //        case WorkerStatus.Subwork_Craft_Bow:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;

                    //        case WorkerStatus.Subwork_Craft_LightArmor:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;
                    //        case WorkerStatus.Subwork_Craft_MediumArmor:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;
                    //        case WorkerStatus.Subwork_Craft_HeavyArmor:
                    //            ResourceLib.CraftIron.createBackOrder(this);
                    //            break;
                    //    }
                    //    break;
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

            cullingTopLeft = WP.SubtileToTilePos(minpos);
            cullingBottomRight = WP.SubtileToTilePos(maxpos);

            int workTeamCount = workForce / WorkTeamSize;

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

                CityStructure.Singleton.updateIfNew(this, workerStatuses.Count);
                buildWorkQue2();
                //Last position = highest priority
                workQue.Sort((a, b) => a.priority.CompareTo(b.priority));

                previousWorkQueUpdate.setNow();
            }

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatusActiveCount > workForce)
                    {
                        --workerStatusActiveCount;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Exit, -1, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].carry.amount > 0)
                    {
                        CityStructure.Singleton.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.DropOff, -1, -1, CityStructure.Singleton.storePosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0 && (res_food.amount > 0 || faction.gold > 0))
                    {
                        CityStructure.Singleton.updateIfNew(this, workerStatuses.Count);
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Eat, -1, -1, CityStructure.Singleton.eatPosition(status.subTileEnd), this);
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy <= DssConst.Worker_Starvation)
                    {
                        --workerStatusActiveCount;
                        --workForce;
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Starving, -1, -1, WP.ToSubTilePos_Centered(tilePos), this);
                        workerStatuses[i] = status;
                    }
                    else if (workQue.Count > 0)
                    {
                        var status = workerStatuses[i];

                        do
                        {
                            var work = arraylib.PullLastMember(workQue);

                            if (status.checkAvailableAndBackOrder(work.work, work.subWork, this))
                            {
                                status.createWorkOrder(work.work, work.subWork, work.orderId, work.subTile, this);
                                workerStatuses[i] = status;

                                if (work.orderId >= 0)
                                {
                                    faction.player.orders?.StartOrderId(work.orderId);
                                }
                                break;
                            }
                        }
                        while (workQue.Count > 0);
                    }
                    else
                    {
                        var worker = workerStatuses[i];
                        worker.energy -= (Ref.TotalGameTimeSec - worker.processTimeStartStampSec) * DssConst.WorkTeamEnergyCost_WhenIdle;
                        worker.processTimeStartStampSec = Ref.TotalGameTimeSec;
                    }
                }
            }

            if (!inRender_detailLayer)
            {
                processAsynchWork(workerStatuses);
            }
                        
            void buildWorkQue2()
            {
                IntVector2 center = WP.ToSubTilePos_Centered(tilePos);

                workQue.Clear();

                var orders_sp = faction.player.orders;
                //ORDERS
                if (orders_sp != null)
                {
                    lock (orders_sp)
                    {
                        for (int i = 0; i < orders_sp.orders.Count; ++i)
                        {
                            var workOrder = orders_sp.orders[i].GetWorkOrder(this);
                            if (workOrder != null)
                            {
                                workQue.Add(workOrder.createWorkQue(out CraftBlueprint orderBluePrint));
                            }
                        }
                    }
                }

                //PICK UP
                if (workTemplate.move.HasPrio())
                {
                    foreach (var pos in CityStructure.Singleton.ResourceOnGround)
                    {
                        var subTile = DssRef.world.subTileGrid.Get(pos);

                        if (subTile.collectionPointer >= 0)
                        {
                            var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                            var resource = chunk.peek();

                            //var stockpile = GetGroupedResource(resource.type);

                            if (needMore(resource.type) && isFreeTile(pos))
                            {
                                int distanceValue = -center.SideLength(pos);
                                workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, pos, workTemplate.move.value, distanceValue));
                            }
                        }
                    }
                }

                //WOOD
                if (workTemplate.wood.HasPrio() &&
                        res_wood.needMore())
                {
                    foreach (var pos in CityStructure.Singleton.Trees)
                    {
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.wood.value, distanceValue));
                        }
                    }
                }

                //STONE
                if (workTemplate.stone.HasPrio() &&
                    res_stone.needMore())
                {
                    foreach (var pos in CityStructure.Singleton.Stones)
                    {
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.stone.value, distanceValue));
                            //res_stone.orderQueCount += ItemPropertyColl.CarryStones;
                        }
                    }
                }

                //FARMS
                if (workTemplate.farming.HasPrio())
                {
                    foreach (var pos in CityStructure.Singleton.FarmPlant)
                    {
                        bool needMore = false;

                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.LinenFarm:
                                needMore = res_skinLinnen.needMore();
                                break;
                            case TerrainSubFoilType.WheatFarm:
                                needMore = res_rawFood.needMore();
                                break;
                        }

                        if (needMore && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Plant, NoSubWork, pos, workTemplate.farming.value, distanceValue));
                        }
                    }

                    foreach (var pos in CityStructure.Singleton.FarmGather)
                    {
                        bool needMore = false;

                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.LinenFarm:
                                needMore = res_skinLinnen.needMore();
                                break;
                            case TerrainSubFoilType.WheatFarm:
                                needMore = res_rawFood.needMore();
                                break;
                        }

                        if (needMore && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.farming.value, distanceValue));
                        }
                    }
                }

                //MINING
                if (workTemplate.bogiron.HasPrio() &&
                    res_ironore.needMore())
                {
                    foreach (var pos in CityStructure.Singleton.BogIron)
                    {                       
                        if (isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, pos, workTemplate.bogiron.value, distanceValue));
                        }
                    }
                }

                if (workTemplate.mining.HasPrio())
                {
                    foreach (var pos in CityStructure.Singleton.Mines)
                    {
                        bool needMore = true;

                        var subTile = DssRef.world.subTileGrid.Get(pos);
                        switch ((TerrainMineType)subTile.subTerrain)
                        {
                            case TerrainMineType.IronOre:
                                needMore = res_ironore.needMore();
                                break;
                            case TerrainMineType.Coal:
                                needMore = res_fuel.needMore();
                                break;
                        }

                        if (needMore && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, pos, workTemplate.mining.value, distanceValue));
                        }
                    }
                }

                //ANIMALS
                if (workTemplate.farming.HasPrio())
                {
                    foreach (var pos in CityStructure.Singleton.AnimalPens)
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

                        if (needMore && isFreeTile(pos))
                        {
                            int distanceValue = -center.SideLength(pos);
                            workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, pos, workTemplate.farming.value, distanceValue));
                        }
                    }
                }

                //CRAFT
                foreach (var pos in CityStructure.Singleton.CraftStation)
                {
                    int distanceValue = -center.SideLength(pos);
                    var subTile = DssRef.world.subTileGrid.Get(pos);
                    var building = subTile.GeBuildingType();
                    switch (building)
                    {
                        case TerrainBuildingType.Work_Cook:
                            if (workTemplate.craft_food.HasPrio() &&
                                res_food.needMore() &&
                                (ResourceLib.CraftFood2.canCraft(this) || ResourceLib.CraftFood1.canCraft(this)) &&
                                isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, pos, workTemplate.craft_food.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Work_Bench:
                            craftBench(pos, distanceValue, BenchCraftTypes, -5000);
                            break;
                        case TerrainBuildingType.Work_Smith:

                            craftBench(pos, distanceValue, IronCraftTypes);
                            //int topPrioValue = WorkTemplate.NoPrio;
                            //ItemResourceType topItem = ItemResourceType.NONE;
                            //WorkPriority topPrio = WorkPriority.Empty;

                            //int prioAdd = 0;
                            //ItemResourceType[] types;

                            //switch  (building)
                            //{
                            //    case TerrainBuildingType.Work_Bench:
                            //{
                            //    types = BenchCraftTypes;
                            //    prioAdd = -5000;
                            //}
                            //        break;
                            // case 
                            //{
                            //    types = IronCraftTypes;
                            //}

                            //foreach (var item in types)
                            //{
                            //    var template = workTemplate.GetWorkPriority(item);
                            //    if (template.value > topPrioValue)
                            //    {
                            //        ResourceLib.Blueprint(item, out var bp1, out var bp2);
                            //        if (bp1.available(this) && GetGroupedResource(item).needMore())
                            //        {
                            //            topPrioValue = template.value;
                            //            topItem = item;
                            //            topPrio = template;
                            //            //res_fuel
                            //        }
                            //    }
                            //}

                            //if (topPrioValue > WorkTemplate.NoPrio &&
                            //    isFreeTile(pos))
                            //{
                            //    workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, pos, topPrioValue, distanceValue + prioAdd));
                            //}
                            break;

                        case TerrainBuildingType.Work_CoalPit:
                            if (workTemplate.craft_fuel.HasPrio() &&
                               res_fuel.needMore() &&
                               ResourceLib.CraftCharcoal.canCraft(this) &&
                               isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, pos, workTemplate.craft_fuel.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Brewery:
                            if (workTemplate.craft_beer.HasPrio() &&
                                res_beer.needMore() &&
                                ResourceLib.CraftBrewery.canCraft(this) &&
                                isFreeTile(pos))
                            {
                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Beer, pos, workTemplate.craft_beer.value, distanceValue));
                            }
                            break;

                        case TerrainBuildingType.Carpenter:
                            craftBench(pos, distanceValue, CarpenterCraftTypes);
                            //if (workTemplate.craft_ballista.HasPrio() &&
                            //    res_ballista.needMore()&&
                            //    ResourceLib.CraftBallista.canCraft(this) &&
                            //    isFreeTile(pos))
                            //{
                            //    workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Ballista, pos, workTemplate.craft_ballista.value, distanceValue));
                            //}
                            break;
                    }
                }


                //EMPTY
                foreach (var pos in CityStructure.Singleton.EmptyLand)
                {
                    faction.player.AutoExpandType(this, out bool work, out var buildType, out bool intelligent);

                    bool intelligentCheck = true;

                    if (work && workForce >= workForceMax)
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
                            ResourceLib.Blueprint(item, out var bp1, out var bp2);
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


            //void buildWorkQue()
            //{
            //    IntVector2 topleft;
            //    ForXYLoop subTileLoop;
                                
            //    workQue.Clear();
            //    int emptyLandExpansions = 2;// workerStatuses.Count / 8; 
            //    //Find orders
            //    lock (faction.player.orders)
            //    {
            //        for (int i = 0; i < faction.player.orders.orders.Count; ++i)
            //        {
            //           var workOrder = faction.player.orders.orders[i].GetWorkOrder(this);
            //            if (workOrder != null)
            //            {
            //                workQue.Add(workOrder.createWorkQue(out CraftBlueprint orderBluePrint));
            //                //orderBluePrint.createBackOrder(this);
            //                //waterSpendOrders += orderBluePrint.useWater;
            //            }
            //        }
            //    }

            //    //Look for city work
            //    //topleft = WP.ToSubTilePos_TopLeft(tilePos);
            //    //subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);
            //    //while (subTileLoop.Next())
            //    //{
            //    //    var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

            //    //    if (subTile.mainTerrain == TerrainMainType.Building)
            //    //    {
            //    //        switch ((TerrainBuildingType)subTile.subTerrain)
            //    //        {
            //    //            case TerrainBuildingType.Work_Cook:
            //    //                if (ResourceLib.CraftFood.canCraft(this) &&
            //    //                    isFreeTile(subTileLoop.Position))
            //    //                {
            //    //                    //ResourceLib.CraftFood.createBackOrder(this);
            //    //                    workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, subTileLoop.Position, workTemplate.craft_food.value, 0));
            //    //                }
            //    //                else
            //    //                {
            //    //                    var b1 = ResourceLib.CraftFood.available(this);
            //    //                    var b2 = isFreeTile(subTileLoop.Position);
            //    //                }
            //    //                break;
            //    //            case TerrainBuildingType.Work_Smith:
                                
            //    //                int topPrioValue = WorkTemplate.NoPrio;
            //    //                ItemResourceType topItem = ItemResourceType.NONE;
            //    //                WorkPriority topPrio = WorkPriority.Empty;

            //    //                foreach (var item in SmithTypes)
            //    //                {
            //    //                    var template = workTemplate.GetWorkPriority(item);
            //    //                    if (template.value > topPrioValue &&
            //    //                         ResourceLib.Blueprint(item).available(this))
            //    //                    {
            //    //                        topPrioValue = template.value;
            //    //                        topItem = item;
            //    //                        topPrio = template;
            //    //                    }
            //    //                }

            //    //                if (topPrioValue > WorkTemplate.NoPrio &&
            //    //                    isFreeTile(subTileLoop.Position))
            //    //                {
            //    //                    workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, subTileLoop.Position, topPrioValue, 0));
            //    //                }


            //    //                break;
            //    //        }
            //    //    }
            //    //}

            //    //Cirkle outward from city to find resources
            //    for (int radius = 0; radius <= cityTileRadius; ++radius)
            //    {
            //        int distanceValue = -radius;
            //        ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, radius));
                    
            //        while (cirkleLoop.Next())
            //        {
            //            if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
            //            {
            //                var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
            //                if (tile.CityIndex == this.parentArrayIndex && tile.IsLand())
            //                {
            //                    topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
            //                    subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

            //                    while (subTileLoop.Next())
            //                    {
            //                        var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

            //                        if (subTile.collectionPointer >= 0 &&
            //                            isFreeTile(subTileLoop.Position))
            //                        {
            //                            workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, subTileLoop.Position, workTemplate.move.value, distanceValue));
            //                        }
            //                        else
            //                        {
            //                            switch (subTile.mainTerrain)
            //                            {
            //                                case TerrainMainType.Foil:
            //                                    var foil = (TerrainSubFoilType)subTile.subTerrain;

            //                                    switch (foil)
            //                                    {
            //                                        case Map.TerrainSubFoilType.TreeSoft:
            //                                        case Map.TerrainSubFoilType.TreeHard:
            //                                        case Map.TerrainSubFoilType.DryWood:
            //                                            if (workTemplate.wood.HasPrio() &&
            //                                                res_wood.needMore() &&
            //                                                (foil == TerrainSubFoilType.DryWood || subTile.terrainAmount >= TerrainContent.TreeReadySize))
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.wood.value, distanceValue));
            //                                                    res_wood.orderQueCount += subTile.terrainAmount;
            //                                                }
            //                                            }
            //                                            break;

            //                                        case Map.TerrainSubFoilType.StoneBlock:
            //                                        case Map.TerrainSubFoilType.Stones:
            //                                            if (workTemplate.stone.HasPrio() &&
            //                                                res_stone.needMore() &&
            //                                                isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.stone.value, distanceValue));
            //                                                res_stone.orderQueCount += ItemPropertyColl.CarryStones;
            //                                            }
            //                                            break;

            //                                        case TerrainSubFoilType.WheatFarm:
            //                                            if (workTemplate.farming.HasPrio() && 
            //                                                subTile.terrainAmount == TerrainContent.FarmCulture_Empty &&
            //                                                res_rawFood.needMore())
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.Plant, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                }
            //                                            }
            //                                            else if (workTemplate.farming.HasPrio() && 
            //                                                res_rawFood.needMore() &&
            //                                                subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                    res_rawFood.orderQueCount += subTile.terrainAmount;
            //                                                }
            //                                            }
            //                                            break;

            //                                        case TerrainSubFoilType.LinenFarm:
            //                                            if (workTemplate.farming.HasPrio() &&
            //                                                subTile.terrainAmount == TerrainContent.FarmCulture_Empty &&
            //                                                res_skinLinnen.needMore())
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.Plant, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                }
            //                                            }
            //                                            else if (workTemplate.farming.HasPrio() &&
            //                                                res_skinLinnen.needMore() &&
            //                                                subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                    res_rawFood.orderQueCount += subTile.terrainAmount;
            //                                                }
            //                                            }
            //                                            break;
            //                                    }

            //                                    break;

            //                                case TerrainMainType.Mine:
            //                                    if (workTemplate.mining.HasPrio() &&
            //                                        res_ironore.needMore() &&
            //                                        isFreeTile(subTileLoop.Position))
            //                                    {
            //                                        workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, subTileLoop.Position, workTemplate.mining.value, distanceValue));
            //                                        res_ironore.orderQueCount += TerrainContent.MineAmount;
            //                                    }
            //                                    break;

            //                                case TerrainMainType.Building:
            //                                    var building = (TerrainBuildingType)subTile.subTerrain;

            //                                    switch (building)
            //                                    {
            //                                        case TerrainBuildingType.HenPen:
            //                                            if (workTemplate.farming.HasPrio() && res_rawFood.needMore() && subTile.terrainAmount > TerrainContent.HenReady)
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                    res_rawFood.orderQueCount += TerrainContent.HenMaxSize;
            //                                                }
            //                                            }
            //                                            break;
            //                                        case TerrainBuildingType.PigPen:
            //                                            if (workTemplate.farming.HasPrio() && (res_rawFood.needMore() || res_skinLinnen.needMore()) && subTile.terrainAmount > TerrainContent.PigReady)
            //                                            {
            //                                                if (isFreeTile(subTileLoop.Position))
            //                                                {
            //                                                    workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
            //                                                    res_rawFood.orderQueCount += TerrainContent.PigMaxSize;
            //                                                }
            //                                            }
            //                                            break;
            //                                        case TerrainBuildingType.Work_Cook:
            //                                            if (workTemplate.craft_food.HasPrio() &&
            //                                                (ResourceLib.CraftFood2.canCraft(this) ||ResourceLib.CraftFood1.canCraft(this)) &&
            //                                                isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                //ResourceLib.CraftFood.createBackOrder(this);
            //                                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Food_G, subTileLoop.Position, workTemplate.craft_food.value, distanceValue));
            //                                            }
            //                                            break;
            //                                        case TerrainBuildingType.Work_Bench:                                                        
            //                                        case TerrainBuildingType.Work_Smith:

            //                                            int topPrioValue = WorkTemplate.NoPrio;
            //                                            ItemResourceType topItem = ItemResourceType.NONE;
            //                                            WorkPriority topPrio = WorkPriority.Empty;

            //                                            int prioAdd = 0;
            //                                            ItemResourceType[] types;

            //                                            if (building == TerrainBuildingType.Work_Bench)
            //                                            {
            //                                                types = BenchCraftTypes;
            //                                                prioAdd = -5000;
            //                                            }
            //                                            else
            //                                            {
            //                                                types = IronCraftTypes;
            //                                            }

            //                                            foreach (var item in types)
            //                                            {
            //                                                var template = workTemplate.GetWorkPriority(item);
            //                                                if (template.value > topPrioValue)
            //                                                {
            //                                                    ResourceLib.Blueprint(item, out var bp1, out var bp2);
            //                                                    if (bp1.available(this) && GetGroupedResource(item).needMore())
            //                                                    {
            //                                                        topPrioValue = template.value;
            //                                                        topItem = item;
            //                                                        topPrio = template;
            //                                                    }
            //                                                }
            //                                            }

            //                                            if (topPrioValue > WorkTemplate.NoPrio &&
            //                                                isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)topItem, subTileLoop.Position, topPrioValue, distanceValue + prioAdd));
            //                                            }
            //                                            break;

            //                                        case TerrainBuildingType.Work_CoalPit:
            //                                            if (workTemplate.craft_fuel.HasPrio() &&
            //                                               res_fuel.needMore() &&
            //                                               ResourceLib.CraftCharcoal.canCraft(this) &&
            //                                               isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Coal, subTileLoop.Position, workTemplate.craft_fuel.value, distanceValue));
            //                                            }
            //                                            break;

            //                                        case TerrainBuildingType.Brewery:
            //                                            if (workTemplate.craft_beer.HasPrio() &&
            //                                                res_beer.needMore() &&
            //                                                ResourceLib.CraftBrewery.canCraft(this) &&
            //                                                isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                workQue.Add(new WorkQueMember(WorkType.Craft, (int)ItemResourceType.Beer, subTileLoop.Position, workTemplate.craft_beer.value, distanceValue));
            //                                            }
            //                                            break;
            //                                    }

            //                                    break;

            //                                case TerrainMainType.Destroyed:
            //                                case TerrainMainType.DefaultLand:
            //                                    if (emptyLandExpansions > 0)
            //                                    {
            //                                        --emptyLandExpansions;

            //                                       faction.player.AutoExpandType(this, out bool work, out var buildType, out bool intelligent);
                                                    
            //                                        bool intelligentCheck = true;

            //                                        if (work && workForce >= workForceMax)
            //                                        {
            //                                            buildType = BuildAndExpandType.WorkerHuts;
            //                                        }
            //                                        else if (intelligent)
            //                                        {
            //                                            switch (buildType)
            //                                            {
            //                                                case BuildAndExpandType.WheatFarm:
            //                                                case BuildAndExpandType.HenPen:
            //                                                    intelligentCheck = res_rawFood.needMore();
            //                                                    break;
            //                                                case BuildAndExpandType.PigPen:
            //                                                    intelligentCheck = res_rawFood.needMore() || res_skinLinnen.needMore();
            //                                                    break;
            //                                                case BuildAndExpandType.LinenFarm:
            //                                                    intelligentCheck = res_skinLinnen.needMore();
            //                                                    break;
            //                                            }
            //                                        }

            //                                        if (buildType != BuildAndExpandType.NUM_NONE)
            //                                        {
            //                                            if (BuildLib.BuildOptions[(int)buildType].blueprint.available(this) &&
            //                                                isFreeTile(subTileLoop.Position))
            //                                            {
            //                                                workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, subTileLoop.Position, workTemplate.autoBuild.value, distanceValue));
            //                                            }
            //                                        }

            //                                    }
            //                                    break;

            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

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

    enum WorkType
    { 
        IsDeleted,
        Idle,
        Exit,
        Starving,
        Eat,

        Till,
        Plant,
        GatherFoil,
        //GatherCityProduce,
        Mine,
        PickUpResource,
        PickUpProduce,
        DropOff,        
        Craft,
        Build,
        LocalTrade,

        TrossCityTrade,
        TrossReturnToArmy,
    }

    
}
