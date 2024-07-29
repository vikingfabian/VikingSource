using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        int workPriority_move = 6;
        int workPriority_wood = 4;
        int workPriority_stone = 4;
        int workPriority_craft_food = 8;
        int workPriority_craft_iron = 6;
        int workPriority_farming = 5;
        int workPriority_mining = 5;
        int workPriority_trading = 5;
        int workPriority_expand_housing = 3;
        int workPriority_expand_farms = 3;


        const int NoSubWork = -1;
        public const int WorkTeamSize = 8;
        TimeStamp previousWorkQueUpdate = TimeStamp.None;
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        
        public void async_workUpdate()
        {        
            if (parentArrayIndex == 52 || debugTagged)
            { 
                lib.DoNothing();
            }

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
                        CraftWorkerHut.createBackOrder(this);
                        break;

                    case WorkType.Craft:
                        switch (status.workSubType)
                        {
                            case WorkerStatus.Subwork_Craft_Food:
                                CraftFood.createBackOrder(this);
                                break;

                            case WorkerStatus.Subwork_Craft_Iron:
                                CraftIron.createBackOrder(this);
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
                        energy = WorkerStatus.MaxEnergy,
                        subTileEnd = startPos,
                        subTileStart = startPos,
                    });

                    ++idleCount;
                }
            }            

            if (idleCount > workQue.Count && previousWorkQueUpdate.secPassed(20))
            {
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
                        status.createWorkOrder(WorkType.DropOff, -1, WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0)
                    {
                        var status = workerStatuses[i];
                        status.createWorkOrder(WorkType.Eat, -1, WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workQue.Count > 0)
                    {
                        var work = arraylib.PullLastMember(workQue);

                        var status = workerStatuses[i];
                        status.createWorkOrder(work.work, work.subWork, work.subTile);
                        workerStatuses[i] = status;
                    }
                    //else
                    //{
                    //    break;
                    //}
                }
            }

            if (!inRender_overviewLayer)
            {
                processAsynchWork(workerStatuses);
            }

            void buildWorkQue()
            {
                IntVector2 topleft;
                ForXYLoop subTileLoop;

                workQue.Clear();

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
                                if (CraftFood.available(this) && 
                                    isFreeTile(subTileLoop.Position))
                                {
                                    CraftFood.createBackOrder(this);
                                    workQue.Add(new WorkQueMember(WorkType.Craft, NoSubWork, subTileLoop.Position, workPriority_craft_food, 0));
                                }
                                break;
                            case TerrainBuildingType.Work_Smith:
                                if (CraftIron.available(this) &&
                                    isFreeTile(subTileLoop.Position))
                                {
                                    CraftIron.createBackOrder(this);
                                    workQue.Add(new WorkQueMember(WorkType.Craft, NoSubWork, subTileLoop.Position, workPriority_craft_iron, 0));
                                }
                                break;
                        }
                    }
                }

                //Cirkle outward from city to find resources
                for (int radius = 1; radius < 12; ++radius)
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
                                            workQue.Add(new WorkQueMember(WorkType.PickUpResource, NoSubWork, subTileLoop.Position, workPriority_move, distanceValue));
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
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workPriority_wood, distanceValue));
                                                                wood.orderQueCount += subTile.terrainAmount;
                                                            }
                                                        }
                                                        break;

                                                    case Map.TerrainSubFoilType.StoneBlock:
                                                    case Map.TerrainSubFoilType.Stones:
                                                        if (stone.needMore() &&
                                                            isFreeTile(subTileLoop.Position))
                                                        {
                                                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workPriority_stone, distanceValue));
                                                            stone.orderQueCount += ItemPropertyColl.CarryStones;
                                                        }
                                                        break;

                                                    case TerrainSubFoilType.FarmCulture:
                                                        if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty &&
                                                            rawFood.needMore())
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.Plant, NoSubWork, subTileLoop.Position, workPriority_farming, distanceValue));
                                                            }
                                                        }
                                                        else if (rawFood.needMore() &&
                                                            subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, NoSubWork, subTileLoop.Position, workPriority_farming, distanceValue));
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
                                                    workQue.Add(new WorkQueMember(WorkType.Mine, NoSubWork, subTileLoop.Position, workPriority_mining, distanceValue));
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
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workPriority_farming, distanceValue));
                                                                rawFood.orderQueCount += TerrainContent.HenMaxSize;
                                                            }
                                                        }
                                                        break;
                                                    case TerrainBuildingType.PigPen:
                                                        if ((rawFood.needMore() || skin.needMore()) && subTile.terrainAmount > TerrainContent.PigReady)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, NoSubWork, subTileLoop.Position, workPriority_farming, distanceValue));
                                                                rawFood.orderQueCount += TerrainContent.PigMaxSize;
                                                            }
                                                        }
                                                        break;
                                                }

                                                break;

                                            case TerrainMainType.Destroyed:
                                            case TerrainMainType.DefaultLand:
                                                if (waterBuffer + waterSpendOrders < water)
                                                {
                                                    if (rawFood.needMore() && isFreeTile(subTileLoop.Position))
                                                    {
                                                        workQue.Add(new WorkQueMember(WorkType.Till, NoSubWork, subTileLoop.Position, 3, distanceValue));
                                                        waterSpendOrders += 10;
                                                    }
                                                    else if (workForce.value >= workForceMax && 
                                                        //idleCount < 5 && 
                                                        CraftWorkerHut.available(this) &&
                                                        isFreeTile(subTileLoop.Position))
                                                    {
                                                        //worker hut
                                                        CraftWorkerHut.createBackOrder(this);
                                                        workQue.Add(new WorkQueMember(WorkType.Building, NoSubWork, subTileLoop.Position, 3, distanceValue));
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
                            int value = distanceValue + (int)(nCity.wood.goldValue * CostPrioValue);
                            if (value > woodTrade.priority)
                            {
                                woodTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.SoftWood, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
                            }                           
                        }
                        if (stone.needToImport() && nCity.stone.canTradeAway())
                        {
                            int value = distanceValue + (int)(nCity.stone.goldValue * CostPrioValue);
                            if (value > stoneTrade.priority)
                            {
                                stoneTrade = new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Stone, WP.ToSubTilePos_Centered(nCity.tilePos), 5, value);
                            }
                        }
                        if (food.needToImport() && nCity.food.canTradeAway())
                        {
                            int value = distanceValue + (int)(nCity.food.goldValue * CostPrioValue);
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
                    { return false; }
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
                blackMarketCosts.add(cost);
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

        /// <summary>
        /// Goes from 1:lowest to 10: highest
        /// </summary>
        public int priority;

        //public int distance;

        //public WorkQueMember(WorkType work, IntVector2 subTile, int distance, int priority)
        //{
        //    this.work = work;
        //    this.subWork = -1;
        //    this.subTile = subTile;
        //    this.priority = priority;
        //    this.distance = distance;
        //}
        
        public WorkQueMember(WorkType work, int subWork, IntVector2 subTile, int priority, int subPrio)
        {
            this.work = work;
            this.subWork = subWork;
            this.subTile = subTile;
            this.priority = priority * 1000000 + subPrio;
            //this.distance = distance;
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
