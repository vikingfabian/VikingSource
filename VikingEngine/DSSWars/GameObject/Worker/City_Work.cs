﻿using System;
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
        const int WorkTeamSize = 8;

        List<WorkerStatus> workerStatuses = new List<WorkerStatus>();
        List<WorkQueMember> workQue = new List<WorkQueMember>();
        public List<WorkerUnit> workerUnits = null;

        public void async_workUpdate()
        {        
            if (parentArrayIndex == 52 || debugTagged)
            { 
                lib.DoNothing();
            }

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
                        energy = 50,
                        subTileEnd = startPos,
                        subTileStart = startPos,
                    });

                    ++idleCount;
                }
            }            

            if (idleCount > workQue.Count)
            {
                buildWorkQue();
                //Last position = highest priority
                workQue.Sort((a, b) => a.priority.CompareTo(b.priority));
            }

            //Give orders to idle workers
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                if (workerStatuses[i].work == WorkType.Idle)
                {
                    if (workerStatuses[i].carry.amount > 0)
                    {
                        var status = workerStatuses[i];
                        status.createWorkOrder(this,WorkType.DropOff, -1, WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workerStatuses[i].energy < 0)
                    {
                        var status = workerStatuses[i];
                        status.createWorkOrder(this,WorkType.Eat, -1, WP.ToSubTilePos_Centered(tilePos));
                        workerStatuses[i] = status;
                    }
                    else if (workQue.Count > 0)
                    {
                        var work = arraylib.PullLastMember(workQue);

                        var status = workerStatuses[i];
                        status.createWorkOrder(this,work.work, work.subWork, work.subTile);
                        workerStatuses[i] = status;
                    }
                    //else
                    //{
                    //    break;
                    //}
                }
            }

            if (!inRender)
            {
                processAsynchWork();
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
                                    workQue.Add(new WorkQueMember(WorkType.Craft, subTileLoop.Position, 8));
                                }
                                break;
                            case TerrainBuildingType.Work_Smith:
                                if (CraftIron.available(this) &&
                                    isFreeTile(subTileLoop.Position))
                                {
                                    CraftIron.createBackOrder(this);
                                    workQue.Add(new WorkQueMember(WorkType.Craft, subTileLoop.Position, 6));
                                }
                                break;
                        }
                    }
                }

                //Cirkle outward from city to find resources
                for (int radius = 1; radius < 12; ++radius)
                {
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
                                            workQue.Add(new WorkQueMember(WorkType.PickUpResource, subTileLoop.Position, 6));
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
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, subTileLoop.Position, 4));
                                                                wood.orderQueCount += subTile.terrainAmount;
                                                            }
                                                        }
                                                        break;

                                                    case Map.TerrainSubFoilType.StoneBlock:
                                                    case Map.TerrainSubFoilType.Stones:
                                                        if (stone.needMore() &&
                                                            isFreeTile(subTileLoop.Position))
                                                        {
                                                            workQue.Add(new WorkQueMember(WorkType.GatherFoil, subTileLoop.Position, 4));
                                                            stone.orderQueCount += ItemPropertyColl.CarryStones;
                                                        }
                                                        break;

                                                    case TerrainSubFoilType.FarmCulture:
                                                        if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty &&
                                                            rawFood.needMore())
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.Plant, subTileLoop.Position, 5));
                                                            }
                                                        }
                                                        else if (rawFood.needMore() &&
                                                            subTile.terrainAmount >= TerrainContent.FarmCulture_ReadySize)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.GatherFoil, subTileLoop.Position, 5));
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
                                                    workQue.Add(new WorkQueMember(WorkType.Mine, subTileLoop.Position, 5));
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
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, subTileLoop.Position, 5));
                                                                rawFood.orderQueCount += TerrainContent.HenMaxSize;
                                                            }
                                                        }
                                                        break;
                                                    case TerrainBuildingType.PigPen:
                                                        if ((rawFood.needMore() || skin.needMore()) && subTile.terrainAmount > TerrainContent.PigReady)
                                                        {
                                                            if (isFreeTile(subTileLoop.Position))
                                                            {
                                                                workQue.Add(new WorkQueMember(WorkType.PickUpProduce, subTileLoop.Position, 5));
                                                                rawFood.orderQueCount += TerrainContent.PigMaxSize;
                                                            }
                                                        }
                                                        break;
                                                }

                                                break;

                                            case TerrainMainType.DefaultLand:
                                                if (waterBuffer + waterSpendOrders < water)
                                                {
                                                    if (rawFood.needMore() && isFreeTile(subTileLoop.Position))
                                                    {
                                                        workQue.Add(new WorkQueMember(WorkType.Till, subTileLoop.Position, 3));
                                                        waterSpendOrders += 10;
                                                    }
                                                    else if (workForce.value >= workForceMax && 
                                                        //idleCount < 5 && 
                                                        CraftWorkerHut.available(this) &&
                                                        isFreeTile(subTileLoop.Position))
                                                    {
                                                        //worker hut
                                                        CraftWorkerHut.createBackOrder(this);
                                                        workQue.Add(new WorkQueMember(WorkType.Building, subTileLoop.Position, 3));
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

                //Trade with neighbor cities
                foreach (var n in neighborCities)
                {
                    var nCity = DssRef.world.cities[n];
                    if (!DssRef.diplomacy.InWar(nCity.faction, faction))
                    {
                        if (wood.needToImport() && nCity.wood.canTradeAway())
                        {
                            workQue.Add(new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.SoftWood, WP.ToSubTilePos_Centered(nCity.tilePos), 5));
                        }
                        if (stone.needToImport() && nCity.stone.canTradeAway())
                        {
                            workQue.Add(new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Stone, WP.ToSubTilePos_Centered(nCity.tilePos), 5));
                        }
                        if (food.needToImport() && nCity.food.canTradeAway())
                        {
                            workQue.Add(new WorkQueMember(WorkType.LocalTrade, (int)ItemResourceType.Food, WP.ToSubTilePos_Centered(nCity.tilePos), 5));
                        }
                    }
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

            void processAsynchWork()
            {
                for (int i = 0; i < workerStatuses.Count; i++)
                {
                    var status = workerStatuses[i];
                    if (status.work != WorkType.Idle &&
                        Ref.TotalGameTimeSec > status.processTimeStartStampSec + status.processTimeLengthSec)
                    {
                        //Work complete
                        status.WorkComplete(this);
                        workerStatuses[i] = status;
                    }

                }
            }
        }

        void updateWorkerUnits()
        {
            if (workerUnits != null)
            {
                if (workerUnits.Count < workerStatuses.Count)
                {
                    for (int i = workerUnits.Count; i < workerStatuses.Count; i++)
                    {
                        workerUnits.Add(new WorkerUnit(this, workerStatuses[i], i));
                    }
                }

                foreach (var w in workerUnits)
                {
                    w.update();
                }
            }
        }

        public void setWorkersInRenderState()
        {
            if (inRender)
            {
                if (workerUnits == null)
                {
                    workerUnits = new List<WorkerUnit>(workerStatuses.Count);
                }
            }
            else
            {
                if (workerUnits != null)
                {
                    foreach (var w in workerUnits)
                    { 
                        w.DeleteMe();
                    }

                    workerUnits = null;
                }
            }
        }

        public void getWorkerStatus(int index, ref WorkerStatus status)
        { 
            status = workerStatuses[index];
        }

        public void setWorkerStatus(int index, ref WorkerStatus status)
        { 
            workerStatuses[index] = status;
        }
    }

    

    struct WorkQueMember
    {
        
        public WorkType work;
        public int subWork;
        public IntVector2 subTile;

        /// <summary>
        /// Goes from 1:lowest to 10: highest
        /// </summary>
        public int priority;

        public WorkQueMember(WorkType work, IntVector2 subTile, int priority)
        {
            this.work = work;
            this.subWork = -1;
            this.subTile = subTile;
            this.priority = priority;
        }

        public WorkQueMember(WorkType work, int subWork, IntVector2 subTile, int priority)
        {
            this.work = work;
            this.subWork = subWork;
            this.subTile = subTile;
            this.priority = priority;
        }
    }

    enum WorkType
    { 
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
    }
}
