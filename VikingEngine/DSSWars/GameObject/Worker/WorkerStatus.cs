﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkerStatus
    {
        const int MaxEnergy = 400;
        const int FoodEnergy = 100;

        public const int Subwork_Craft_Food = 0;
        public const int Subwork_Craft_Iron = 1;

        public WorkType work;
        public int workSubType;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;

        public ItemResource carry;
        public float energy;

        public override string ToString()
        {
            return "Worker (" + work.ToString() + "), carry (" + carry.ToString() + ")";
        }

        public void WorkComplete(City city)
        {
            energy -= processTimeLengthSec;
            SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);

            bool tryRepeatWork = false;

            switch (work)
            {
                case WorkType.Eat:
                    int eatAmount = (int)Math.Floor((MaxEnergy - energy) / FoodEnergy);
                    city.food.amount -= eatAmount;
                    energy += eatAmount * FoodEnergy;
                    //work = WorkType.Idle;
                    break;

                case WorkType.GatherFoil:
                    {
                        //Resource.ItemResourceType resourceType;

                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.TreeSoft:
                                gatherWood(Resource.ItemResourceType.SoftWood, ref subTile);
                                break;

                            case TerrainSubFoilType.TreeHard:
                                gatherWood(Resource.ItemResourceType.HardWood, ref subTile);
                                break;

                            case TerrainSubFoilType.FarmCulture:
                                DssRef.state.resources.addItem(
                                    new Resource.ItemResource(
                                        ItemResourceType.Wheat,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        subTile.terrainAmount),
                                    ref subTile.collectionPointer);

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                                break;

                            case TerrainSubFoilType.StoneBlock:
                            case TerrainSubFoilType.Stones:
                                carry = new ItemResource(ItemResourceType.Stone, 1, Convert.ToInt32(processTimeLengthSec), ItemPropertyColl.CarryStones);
                                break;
                        }

                        //work = WorkType.Idle;                        
                    }
                    break;

                case WorkType.Till:
                    if (subTile.mainTerrain == TerrainMainType.DefaultLand)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.FarmCulture, 0);
                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                    }

                   // work = WorkType.Idle;
                    break;

                case WorkType.Plant:
                    if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty)
                    {
                        subTile.terrainAmount++;
                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                    }

                    //work = WorkType.Idle;
                    break;

                case WorkType.PickUpResource:
                    if (subTile.collectionPointer >= 0)
                    {
                        var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                        carry = chunk.pickUp(1f);

                        if (carry.type != ItemResourceType.NONE)
                        {
                            DssRef.state.resources.update(subTile.collectionPointer, ref chunk);

                            if (chunk.count <= 0)
                            {
                                subTile.collectionPointer = -1;

                                if (subTile.mainTerrain == TerrainMainType.Resourses)
                                {
                                    subTile.mainTerrain = TerrainMainType.DefaultLand;
                                }
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                            }
                        }
                    }
                    //work = WorkType.Idle;
                    break;

                case WorkType.PickUpProduce:
                    {
                        var building = (TerrainBuildingType)subTile.subTerrain;

                        int min, size;
                        Resource.ItemResourceType resourceType;

                        if (building == TerrainBuildingType.PigPen)
                        {
                            resourceType = Resource.ItemResourceType.Pig;
                            min = TerrainContent.PigReady;
                            size = TerrainContent.PigMaxSize;
                        }
                        else
                        {
                            resourceType = Resource.ItemResourceType.Hen;
                            min = TerrainContent.HenReady;
                            size = TerrainContent.HenMaxSize;
                        }

                        if (subTile.terrainAmount >= min)
                        {
                            subTile.terrainAmount -= size;
                            DssRef.world.subTileGrid.Set(subTileEnd, subTile);

                            carry = new ItemResource(resourceType, 1, Convert.ToInt32(processTimeLengthSec), 1);
                        }
                    }
                    //work = WorkType.Idle;
                    break;

                case WorkType.DropOff:
                    city.dropOffItem(carry);
                    carry = ItemResource.Empty;
                    //work = WorkType.Idle;
                    break;

                case WorkType.Mine:
                    {
                        var mineType = (TerrainMineType)subTile.subTerrain;
                        Resource.ItemResourceType resourceType = ItemResourceType.NONE;
                        switch (mineType)
                        {
                            case TerrainMineType.IronOre:
                                resourceType = ItemResourceType.IronOre;
                                break;
                            case TerrainMineType.GoldOre:
                                resourceType = ItemResourceType.GoldOre;
                                break;
                        }

                        DssRef.state.resources.addItem(
                                new Resource.ItemResource(
                                    resourceType,
                                    subTile.terrainQuality,
                                    Convert.ToInt32(processTimeLengthSec),
                                    TerrainContent.MineAmount),
                                ref subTile.collectionPointer);

                        tryRepeatWork = true;
                        //work = WorkType.Idle;
                    }
                    break;
                case WorkType.Craft:
                    {
                        var building = (TerrainBuildingType)subTile.subTerrain;

                        city.craftItem(building, out tryRepeatWork);
                        //work = WorkType.Idle;
                    }
                    break;

                case WorkType.Building:
                    {

                        subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, 1);
                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                        bool non = false;
                        city.craftItem(TerrainBuildingType.WorkerHut, out _);

                        //work = WorkType.Idle;
                    }
                    break;
            }

            if (tryRepeatWork && energy > 0)
            {
                processTimeStartStampSec = Ref.TotalGameTimeSec;
                processTimeLengthSec = finalizeWorkTime();
                subTileStart = subTileEnd;
            }
            else
            {
                work = WorkType.Idle;
            }
        }

        void gatherWood(Resource.ItemResourceType resourceType, ref SubTile subTile)
        {
            DssRef.state.resources.addItem(
                new Resource.ItemResource(
                    resourceType,
                    subTile.terrainQuality,
                    Convert.ToInt32(processTimeLengthSec),
                    subTile.terrainAmount),
                ref subTile.collectionPointer);

            subTile.SetType(TerrainMainType.Resourses, (int)TerrainResourcesType.Wood, 1);

            DssRef.world.subTileGrid.Set(subTileEnd, subTile);
        }

        public void createWorkOrder(WorkType work, IntVector2 targetSubTile)
        {
            this.work = work;
            subTileStart = subTileEnd;
            subTileEnd = targetSubTile;
            processTimeStartStampSec = Ref.TotalGameTimeSec;
            float dist = VectorExt.Length(subTileEnd.X - subTileStart.X, subTileEnd.Y - subTileStart.Y) / WorldData.TileSubDivitions;
            
            processTimeLengthSec = finalizeWorkTime() + 
                dist / (AbsDetailUnitData.StandardWalkingSpeed * 1000);

            switch (work)
            {
                case WorkType.Craft:
                    SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);
                    var building = (TerrainBuildingType)subTile.subTerrain;
                    switch (building)
                    { 
                        case TerrainBuildingType.Work_Cook:
                            workSubType = Subwork_Craft_Food;
                            break;
                        case TerrainBuildingType.Work_Smith:
                            workSubType = Subwork_Craft_Iron;
                            break;
                    }
                    break;
            }
        }

        public float finalizeWorkTime()
        {
            switch (work)
            {
                case WorkType.Eat:
                    return 20;
                case WorkType.PickUpResource:
                    return 2f;
                case WorkType.PickUpProduce:
                    return 3f;
                case WorkType.DropOff:
                    return 1f;
                case WorkType.GatherFoil:
                    SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);
                    switch ((TerrainSubFoilType)subTile.subTerrain)
                    { 
                        case TerrainSubFoilType.TreeSoft:
                            return 10;
                        case TerrainSubFoilType.TreeHard:
                            return 12;
                        case TerrainSubFoilType.FarmCulture:
                            return 20;
                        case TerrainSubFoilType.Stones:
                        case TerrainSubFoilType.StoneBlock:
                            return 5;
                        default:
                            throw new NotImplementedException();
                    }
                case WorkType.Till:
                    return 30;
                case WorkType.Plant:
                    return 20;
                case WorkType.Mine:
                    return 30;
                case WorkType.Craft:
                    return 5f;
                case WorkType.Building:
                    return 40;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
