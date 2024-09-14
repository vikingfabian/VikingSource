using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        public WorkTemplate workTemplate = new WorkTemplate();

        const int NoSubWork = -1;
        public const int WorkTeamSize = 8;
        TimeStamp previousWorkQueUpdate = TimeStamp.None;
        List<WorkQueMember> workQue = new List<WorkQueMember>();

        public void workTab(Players.LocalPlayer player,RichBoxContent content)
        {
            workTemplate.toHud(player, content, faction, this);
        }

        public void async_workUpdate()
        {
            CityStructure.Singleton.newCity = true;

            //if (parentArrayIndex == 160 || debugTagged)
            //{ 
            //    lib.DoNothing();
            //}

            async_blackMarketUpdate();

            int idleCount = 0;
            IntVector2 minpos = WP.ToSubTilePos_Centered(tilePos);
            IntVector2 maxpos = minpos;
            wood.clearOrders();
            stone.clearOrders();
            rawFood.clearOrders();
            skin.clearOrders();
            ore.clearOrders();
            waterSpendOrders = 0;

            for (int i = 0; i < workerStatuses.Count; i++)
            {
                var status = workerStatuses[i];

                switch (status.work)
                { 
                    case WorkType.Idle: 
                        idleCount++; 
                        break;

                    case WorkType.Building:
                        var blueprint = ResourceLib.Blueprint((TerrainBuildingType)status.workSubType);
                        blueprint.createBackOrder(this);
                        break;

                    case WorkType.Craft:
                        switch (status.workSubType)
                        {
                            case WorkerStatus.Subwork_Craft_Food:
                                ResourceLib.CraftFood.createBackOrder(this);
                                break;

                            case WorkerStatus.Subwork_Craft_Iron:
                                ResourceLib.CraftIron.createBackOrder(this);
                                break;
                        }
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

            cullingTopLeft = WP.SubtileToTilePos(minpos);
            cullingBottomRight = WP.SubtileToTilePos(maxpos);

            int workTeamCount = workForce.Int() / WorkTeamSize;

            if (workerStatuses.Count < workTeamCount)
            {
                int newWorkers = workTeamCount - workerStatuses.Count;
                IntVector2 startPos = WP.ToSubTilePos_Centered(tilePos);
                for (int i = 0; i < newWorkers; i++)
                {
                    workerStatuses.Add(new WorkerStatus()
                    { 
                        work = WorkType.Idle,
                        processTimeStartStampSec = Ref.TotalGameTimeSec,
                        energy = WorkerStatus.MaxEnergy,
                        subTileEnd = startPos,
                        subTileStart = startPos,
                    });

                    ++idleCount;
                }
            }            

            if (idleCount > 0 && previousWorkQueUpdate.secPassed(10))
            {
                if (parentArrayIndex == 160 || debugTagged)
                {
                    lib.DoNothing();
                }
                buildWorkQue();
                //Last position = highest priority
                workQue.Sort((a, b) => a.priority.CompareTo(b.priority));

                previousWorkQueUpdate.setNow();
            }

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatuses[i].carry.amount > 0)
                    {
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.DropOff, -1, -1, WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0)
                    {
                        CityStructure.Singleton.updateIfNew(this);
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Eat, -1, -1, CityStructure.Singleton.eatPosition(status.subTileEnd));//WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workQue.Count > 0)
                    {
                        var work = arraylib.PullLastMember(workQue);

                        var status = workerStatuses[i];
                        status.createWorkOrder(work.work, work.subWork, work.orderId, work.subTile);
                        workerStatuses[i] = status;

                        if (work.orderId >= 0)
                        {
                            faction.player.StartOrderId(work.orderId);
                        }
                    }
                    else
                    {
                        var worker = workerStatuses[i];
                        worker.energy -= (Ref.TotalGameTimeSec - worker.processTimeStartStampSec) * ResourceLib.WorkTeamEnergyCost_WhenIdle;
                        worker.processTimeStartStampSec = Ref.TotalGameTimeSec;
                    }
                }
            }

            if (!inRender_detailLayer)
            {
                processAsynchWork(workerStatuses);
            }

            void buildWorkQue()
            {
                IntVector2 topleft;
                ForXYLoop subTileLoop;
                                
                workQue.Clear();

                //Find orders
                lock (faction.player.orders)
                {
                    for (int i = 0; i < faction.player.orders.Count; ++i)
                    {
                       var workOrder = faction.player.orders[i].GetWorkOrder(this);
                        if (workOrder != null)
                        {
                            workQue.Add(workOrder.createWorkQue(out CraftBlueprint orderBluePrint));
                            //orderBluePrint.createBackOrder(this);
                            waterSpendOrders += orderBluePrint.useWater;
                        }
                    }
                }

                //Look for city work
                topleft = WP.ToSubTilePos_TopLeft(tilePos);
                subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);
                while (subTileLoop.Next())
                {
                    var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                    if (subTile.mainTerrain == TerrainMainType.Building)
                    {
                        switch ((TerrainBuildingType)subTile.subTerrain)
                        {
                            case TerrainBuildingType.Work_Cook:
                                if (ResourceLib.CraftFood.available(this) &&
                                    isFreeTile(subTileLoop.Position))
                                {
                                    //ResourceLib.CraftFood.createBackOrder(this);
                                    workQue.Add(new WorkQueMember(WorkType.Craft, NoSubWork, subTileLoop.Position, workTemplate.craft_food.value, 0));
                                }
                                //else
                                //{
                                //    var b1 = ResourceLib.CraftFood.available(this);
                                //    var b2 = isFreeTile(subTileLoop.Position);
                                //}
                                break;
                            case TerrainBuildingType.Work_Smith:
                                if (ResourceLib.CraftIron.available(this) &&
                                    isFreeTile(subTileLoop.Position))
                                {
                                    //ResourceLib.CraftIron.createBackOrder(this);
                                    workQue.Add(new WorkQueMember(WorkType.Craft, NoSubWork, subTileLoop.Position, workTemplate.craft_iron.value, 0));
                                }
                                break;
                        }
                    }
                }

                //Cirkle outward from city to find resources
                for (int radius = 1; radius <= cityTileRadius; ++radius)
                {
                    int distanceValue = -radius;
                    ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, radius));
                    
                    while (cirkleLoop.Next())
                    {
                        if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                        {
                            var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                            if (tile.CityIndex == this.parentArrayIndex && tile.IsLand())
                            {
                                topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                                subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                                while (subTileLoop.Next())
                                {
                                    var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                    if (subTile.collectionPointer >= 0)
                                    {
                                        if (isFreeTile(subTileLoop.Position))
                                        {
                                            workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, subTileLoop.Position, workTemplate.move.value, distanceValue));
                                        }
                                    }
                                    else
                                    {
                                        switch (subTile.mainTerrain)
                                        {
                                            case TerrainMainType.Foil:
                                                var foil = (TerrainSubFoilType)subTile.subTerrain;

                                                switch (foil)
                                                {
                                                    case Map.TerrainSubFoilType.TreeSoft:
                                                    case Map.TerrainSubFoilType.TreeHard:
                                                        if (wood.needMore() &&
                                                            subTile.terrainAmount >= TerrainContent.TreeReadySize)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.wood.value, distanceValue));
                                                                wood.orderQueCount += subTile.terrainAmount;
                                                            }
                                                        }
                                                        break;

                                                    case Map.TerrainSubFoilType.StoneBlock:
                                                    case Map.TerrainSubFoilType.Stones:
                                                        if (stone.needMore() &&
                                                            isFreeTile(subTileLoop.Position))
                                                        {
                                                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.stone.value, distanceValue));
                                                            stone.orderQueCount += ItemPropertyColl.CarryStones;
                                                        }
                                                        break;

                                                    case TerrainSubFoilType.FarmCulture:
                                                        if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty &&
                                                            rawFood.needMore())
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.Plant, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
                                                            }
                                                        }
                                                        else if (rawFood.needMore() &&
                                                            subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
                                                                rawFood.orderQueCount += subTile.terrainAmount;
                                                            }
                                                        }
                                                        break;
                                                }

                                                break;

                                            case TerrainMainType.Mine:
                                                if (ore.needMore() &&
                                                    isFreeTile(subTileLoop.Position))
                                                {
                                                    workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, subTileLoop.Position, workTemplate.mining.value, distanceValue));
                                                    ore.orderQueCount += TerrainContent.MineAmount;
                                                }
                                                break;

                                            case TerrainMainType.Building:
                                                var building = (TerrainBuildingType)subTile.subTerrain;

                                                switch (building)
                                                {
                                                    case TerrainBuildingType.HenPen:
                                                        if (rawFood.needMore() && subTile.terrainAmount > TerrainContent.HenReady)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
                                                                rawFood.orderQueCount += TerrainContent.HenMaxSize;
                                                            }
                                                        }
                                                        break;
                                                    case TerrainBuildingType.PigPen:
                                                        if ((rawFood.needMore() || skin.needMore()) && subTile.terrainAmount > TerrainContent.PigReady)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workTemplate.farming.value, distanceValue));
                                                                rawFood.orderQueCount += TerrainContent.PigMaxSize;
                                                            }
                                                        }
                                                        break;
                                                    //case TerrainBuildingType.Tavern:
                                                    //    FoodSpots_workupdate.Add(subTileLoop.Position);
                                                    //    break;
                                                }

                                                break;

                                            case TerrainMainType.Destroyed:
                                            case TerrainMainType.DefaultLand:
                                                if (waterBuffer + waterSpendOrders < water)
                                                {
                                                    if (rawFood.needMore() && isFreeTile(subTileLoop.Position))
                                                    {
                                                        workQue.Add(new WorkQueMember(WorkType.Till, NoSubWork, subTileLoop.Position, workTemplate.expand_farms.value, distanceValue));
                                                        waterSpendOrders += 10;
                                                    }
                                                    else if (workForce.value >= workForceMax && 
                                                        //idleCount < 5 && 
                                                        ResourceLib.CraftWorkerHut.available(this) &&
                                                        isFreeTile(subTileLoop.Position))
                                                    {
                                                        //worker hut
                                                        //ResourceLib.CraftWorkerHut.createBackOrder(this);
                                                        workQue.Add(new WorkQueMember(WorkType.Building, (int)TerrainBuildingType.WorkerHut, subTileLoop.Position, workTemplate.expand_housing.value, distanceValue));
                                                        waterSpendOrders += 10;
                                                    }
                                                }
                                                break;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                const int CostPrioValue = -1000;
                const int RelationPrioValue = 100;

                WorkQueMember woodTrade = WorkQueMember.NoPrio;
                WorkQueMember stoneTrade = WorkQueMember.NoPrio;
                WorkQueMember foodTrade = WorkQueMember.NoPrio;


                //Trade with neighbor cities
                foreach (var n in neighborCities)
                {
                    var nCity = DssRef.world.cities[n];

                    //priority
                    // check trade block
                    //1. price
                    //2. buy in faction/ally
                    //3. distance
                    int distanceValue = -tilePos.SideLength(nCity.tilePos);

                    if (DssRef.diplomacy.MayTrade(nCity.faction, faction, out var relation))
                    {
                        if (nCity.faction == faction)
                        {
                            distanceValue += 8 * RelationPrioValue;
                        }
                        else
                        {
                            distanceValue += (int)relation * RelationPrioValue;
                        }

                        if (wood.needToImport() && nCity.wood.canTradeAway())
                        {
                            int value = distanceValue + (int)(nCity.tradeTemplate.wood.price * CostPrioValue);
                            if (value > woodTrade.priority)
                            {
                                woodTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.SoftWood, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
                            }                           
                        }
                        if (stone.needToImport() && nCity.stone.canTradeAway())
                        {
                            int value = distanceValue + (int)(nCity.tradeTemplate.stone.price * CostPrioValue);
                            if (value > stoneTrade.priority)
                            {
                                stoneTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Stone, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
                            }
                        }
                        if (food.needToImport() && nCity.food.canTradeAway())
                        {
                            int value = distanceValue + (int)(nCity.tradeTemplate.food.price * CostPrioValue);
                            if (value > foodTrade.priority)
                            {
                                foodTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Food, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
                            }
                        }
                    }
                }

                if (woodTrade.work != WorkType.IsDeleted)
                {
                    workQue.Add(woodTrade);
                }
                if (stoneTrade.work != WorkType.IsDeleted)
                {
                    workQue.Add(stoneTrade);
                }
                if (foodTrade.work != WorkType.IsDeleted)
                {
                    workQue.Add(foodTrade);
                }

            }

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

        protected override void onAsynchWorkComplete(ref WorkerStatus status)
        {
            status.WorkComplete(this);
        }

        void async_blackMarketUpdate()
        {
            //float foodUpkeep = 0;

            if (food.amount <= -40)
            {
                int buyFood = -food.amount;

                int cost = (int)(buyFood * FoodGoldValue_BlackMarket);
                faction.payMoney(cost, true);
                blackMarketCosts_food.add(cost);
                food.amount += buyFood;
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
        Eat,

        Till,
        Plant,
        GatherFoil,
        GatherCityProduce,
        Mine,
        PickUpResource,
        PickUpProduce,
        DropOff,        
        Craft,
        Building,
        LocalTrade,

        TrossCityTrade,
        TrossReturnToArmy,
    }
}
