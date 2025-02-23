﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Work
{
    struct WorkerStatus
    {

        public WorkType work;
        public int workSubType;
        public int orderId;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;

        public ItemResource carry;
        public float energy;
        //public bool isDeleted;

        public override string ToString()
        {
            return "Worker (" + work.ToString() + "), carry (" + carry.ToString() + ")";
        }

        public string workString()
        {
            switch (work)
            {
                case WorkType.Build:
                    return string.Format(DssRef.lang.WorkerStatus_BuildX, BuildLib.BuildOptions[workSubType].Label());
                case WorkType.Craft:
                    return string.Format(DssRef.lang.Work_CraftX, LangLib.Item((ItemResourceType)workSubType));

                case WorkType.DropOff:
                    return DssRef.lang.WorkerStatus_DropOff;

                case WorkType.Eat:
                    return DssRef.lang.WorkerStatus_Eat;
                case WorkType.GatherFoil:
                    return DssRef.lang.WorkerStatus_Gather;
                case WorkType.Idle:
                    return DssRef.lang.Hud_Idle;
                case WorkType.Mine:
                    return DssRef.lang.Work_Mining;
                case WorkType.PickUpProduce:
                case WorkType.PickUpResource:
                    return DssRef.lang.WorkerStatus_PickUpResource;
                case WorkType.Plant:
                    return DssRef.lang.WorkerStatus_Plant;
                case WorkType.Till:
                    return DssRef.lang.WorkerStatus_Till;
                case WorkType.Starving:
                case WorkType.Exit:
                    return DssRef.lang.WorkerStatus_Exit;
                case WorkType.TrossReturnToArmy:
                    return DssRef.lang.WorkerStatus_TrossReturnToArmy;
                case WorkType.Demolish:
                    return DssRef.lang.Build_DestroyBuilding;

                default:
                    return TextLib.Error;
            }
        }

        void workComplete(Army army)
        {
            switch (work)
            {
                case WorkType.TrossCityTrade:
                    var toCity = DssRef.world.tileGrid.Get(subTileEnd / WorldData.TileSubDivitions).City();
                    ItemResource recieved = toCity.MakeTrade(ItemResourceType.Food_G, carry.amount, DssConst.Worker_TrossWorkerCarryWeight);
                    carry = recieved;

                    createWorkOrder(WorkType.TrossReturnToArmy, 0, -1, WP.ToSubTilePos_Centered(army.tilePos), null);
                    break;
                case WorkType.TrossReturnToArmy:
                    army.food += carry.amount;
                    work = WorkType.IsDeleted;
                    break;
            }

        }

        int farmGrowthMultiplier(int terrainAmount, City city)
        {
            terrainAmount *= 5;
            if (city.Culture == CityCulture.FertileGround)
            {
                return terrainAmount * 2;
            }
            return terrainAmount;
        }

        void workComplete(City city, bool visualUnit)
        {
            float energyCost = processTimeLengthSec * DssConst.WorkTeamEnergyCost;
            if (city.Culture == CityCulture.CrabMentality)
            {
                energyCost *= 0.5f;
            }
            energy -= energyCost;
            SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);

            bool tryRepeatWork = false;

            switch (work)
            {
                case WorkType.Eat:
                    int eatAmount = (int)Math.Floor((DssConst.Worker_MaxEnergy - energy) / DssRef.difficulty.FoodEnergySett);
                    city.res_food.amount -= eatAmount;
                    city.foodSpending.add(eatAmount);
                    energy += eatAmount * DssRef.difficulty.FoodEnergySett;
                    break;

                case WorkType.GatherFoil:
                    {
                        //Resource.ItemResourceType resourceType;

                        switch (subTile.GetFoilType())
                        {
                            case TerrainSubFoilType.TreeSoft:
                                gatherWood(Resource.ItemResourceType.SoftWood, ref subTile, city);
                                break;

                            case TerrainSubFoilType.TreeHard:
                                gatherWood(Resource.ItemResourceType.HardWood, ref subTile, city);
                                break;

                            case TerrainSubFoilType.DryWood:
                                gatherWood(Resource.ItemResourceType.DryWood, ref subTile, city);
                                break;

                            case TerrainSubFoilType.WheatFarm:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Wheat,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(subTile.terrainAmount, city));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                                break;

                            case TerrainSubFoilType.LinenFarm:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Linen,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(subTile.terrainAmount, city));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                                break;

                            case TerrainSubFoilType.RapeSeedFarm:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Rapeseed,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(subTile.terrainAmount, city));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                                break;

                            case TerrainSubFoilType.HempFarm:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Hemp,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(subTile.terrainAmount, city));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                                break;

                            case TerrainSubFoilType.StoneBlock:
                            case TerrainSubFoilType.Stones:
                                carry = new ItemResource(ItemResourceType.Stone_G, city.Culture == CityCulture.Stonemason ? 8 : 4, Convert.ToInt32(processTimeLengthSec), ItemPropertyColl.CarryStones);
                                break;

                            case TerrainSubFoilType.BogIron:
                                carry = new ItemResource(ItemResourceType.IronOre_G, 1, Convert.ToInt32(processTimeLengthSec), TerrainContent.MineAmount);
                                break;


                        }

                        //work = WorkType.Idle;                        
                    }
                    break;

                case WorkType.Till:
                    if (subTile.mainTerrain == TerrainMainType.DefaultLand ||
                        subTile.mainTerrain == TerrainMainType.Destroyed)
                    {
                        subTile.SetType(TerrainMainType.Foil, (int)TerrainSubFoilType.WheatFarm, 0);
                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                    }

                    // work = WorkType.Idle;
                    break;

                case WorkType.Plant:
                    if (subTile.terrainAmount == TerrainContent.FarmCulture_Empty)
                    {
                        subTile.terrainAmount++;
                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                        city.res_water.amount -= DssConst.PlantWaterCost;
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
                                EditSubTile editTile = new EditSubTile(subTileEnd, subTile, false, false, true);
                                editTile.value.collectionPointer = -1;

                                if (subTile.mainTerrain == TerrainMainType.Resourses)
                                {
                                    editTile.value.mainTerrain = TerrainMainType.DefaultLand;
                                    editTile.editTerrain = true;
                                }
                                editTile.Submit();
                                //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
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

                            EditSubTile editTile = new EditSubTile(subTileEnd, subTile, false, true, false);
                            editTile.Submit();
                            
                            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);


                            carry = new ItemResource(resourceType, 1, Convert.ToInt32(processTimeLengthSec), 1);
                        }
                    }
                    //work = WorkType.Idle;
                    break;

                case WorkType.DropOff:
                    city.dropOffItem(carry, out ItemResource convert1, out ItemResource convert2);
                    carry = ItemResource.Empty;

                    if (visualUnit)
                    {
                        Vector3 pos = VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(subTileEnd), 0.08f);
                        new ResourceEffect(convert1.type, convert1.amount, pos, ResourceEffectType.Add);
                        if (convert2.amount > 0)
                        {
                            new ResourceEffect(convert2.type, convert2.amount, VectorExt.AddY(pos, 0.08f), ResourceEffectType.Add);
                        }
                    }
                    break;

                case WorkType.LocalTrade:
                    ItemResourceType tradeForItem = (ItemResourceType)workSubType;
                    var toCity = DssRef.world.tileGrid.Get(subTileEnd / WorldData.TileSubDivitions).City();
                    int payment = carry.amount;
                    ItemResource recieved = toCity.MakeTrade(tradeForItem, payment);

                    if (city.faction != toCity.faction)
                    {
                        city.faction.CityTradeImportCounting += payment;
                        toCity.faction.CityTradeExportCounting += payment;
                    }

                    carry = recieved;
                    break;

                case WorkType.Mine:
                    {
                        //TODO placera mining i en deposit
                        var mineType = (TerrainMineType)subTile.subTerrain;
                        Resource.ItemResourceType resourceType = ItemResourceType.NONE;
                        switch (mineType)
                        {
                            case TerrainMineType.IronOre:
                                resourceType = ItemResourceType.IronOre_G;
                                break;
                            case TerrainMineType.Coal:
                                resourceType = ItemResourceType.Coal;
                                break;
                            case TerrainMineType.GoldOre:
                                resourceType = ItemResourceType.GoldOre;
                                break;
                        }

                        int amount = TerrainContent.MineAmount;
                        if (city.Culture == CityCulture.Miners)
                        {
                            amount *= 2;
                        }

                        carry = new ItemResource(
                            resourceType,
                            subTile.terrainQuality,
                            Convert.ToInt32(processTimeLengthSec),
                            amount);

                    }
                    break;
                case WorkType.Craft:
                    {

                        ItemResourceType item = (ItemResourceType)workSubType;
                        ResourceLib.Blueprint(item, out var bp1, out var bp2);

                        int add = bp1.tryPayResources(city);
                        if (add == 0 && bp2 != null)
                        {
                            add = bp2.tryPayResources(city);
                        }

                        if (add > 0)
                        {
                            switch (item)
                            {
                                case ItemResourceType.Food_G:
                                    city.foodProduction.add(add);
                                    break;

                                case ItemResourceType.Fuel_G:
                                case ItemResourceType.Coal:
                                    item = ItemResourceType.Fuel_G;
                                    if (city.Culture == CityCulture.PitMasters)
                                    {
                                        add *= 2;
                                    }
                                    break;

                                case ItemResourceType.Beer:
                                    if (city.Culture == CityCulture.Brewmaster)
                                    {
                                        add += add / 2;
                                    }
                                    break;

                                case ItemResourceType.LightArmor:
                                    if (city.Culture == CityCulture.Weavers)
                                    {
                                        add += 1;
                                    }
                                    break;

                                case ItemResourceType.MediumArmor:
                                case ItemResourceType.HeavyArmor:
                                    if (city.Culture == CityCulture.Armorsmith)
                                    {
                                        add += 1;
                                    }
                                    break;

                            }

                            //if (item == ItemResourceType.Food_G)
                            //{
                            //    city.foodProduction.add(add);
                            //}
                            //if (item == ItemResourceType.Fuel_G || item == ItemResourceType.Coal)
                            //{ 
                            //    item = ItemResourceType.Fuel_G;
                            //    if (city.Culture == CityCulture.PitMasters)
                            //    {
                            //        add *= 2;
                            //    }
                            //}

                            city.AddGroupedResource(item, add);


                            if (city.debugTagged && item == ItemResourceType.Food_G)
                            {
                                lib.DoNothing();
                            }

                            tryRepeatWork = false;

                            if (city.GetGroupedResource(item).needMore())
                            {
                                if (bp1.hasResources(city))
                                {
                                    tryRepeatWork = true;
                                }
                                else if (bp2 != null && bp2.hasResources(city))
                                {
                                    tryRepeatWork = true;
                                }
                            }



                            if (visualUnit)
                            {
                                new ResourceEffect(item, add, VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(subTileEnd), 0.08f), ResourceEffectType.Add);
                            }
                        }
                    }
                    break;

                case WorkType.Build:
                    {
                        if (orderIsActive(city))
                        {
                            BuildLib.BuildOptions[workSubType].execute_async(city, subTileEnd, ref subTile);
                            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                            EditSubTile edit = new EditSubTile(subTileEnd, subTile, true, true, false);
                            edit.Submit();
                        }
                    }
                    break;
                case WorkType.Demolish:
                    {
                        if (orderIsActive(city))
                        {
                            BuildLib.Demolish(city, subTileEnd);
                            //BuildLib.BuildOptions[workSubType].execute_async(city, subTileEnd, ref subTile);
                            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                        }
                    }
                    break;
                case WorkType.Exit:
                    work = WorkType.IsDeleted;
                    break;
            }

            if (tryRepeatWork && energy > 0)
            {
                processTimeLengthSec = finalizeWorkTime(city);
                subTileStart = subTileEnd;
            }
            else
            {
                work = WorkType.Idle;

                if (orderId >= 0)
                {
                    city.faction.player.orders?.CompleteOrderId(orderId);
                }
            }

            processTimeStartStampSec = Ref.TotalGameTimeSec;

        }

        public void cancelWork()
        {
            work = WorkType.Idle;
            processTimeStartStampSec = Ref.TotalGameTimeSec;
        }

        public bool orderIsActive(City city)
        {
            if (orderId >= 0)
            {
                if (city.faction.player.orders != null)
                {
                    return city.faction.player.orders.GetFromId(orderId) != null;
                }
            }

            return true;

        }

        public void WorkComplete(AbsMapObject mapObject, bool visualUnit)
        {
            switch (mapObject.gameobjectType())
            {
                case GameObjectType.City:
                    workComplete(mapObject.GetCity(), visualUnit);
                    break;

                case GameObjectType.Army:
                    workComplete(mapObject.GetArmy());
                    break;
            }
        }

        void gatherWood(Resource.ItemResourceType resourceType, ref SubTile subTile, City city)
        {
            int amount = subTile.terrainAmount;
            if (city.Culture == CityCulture.Woodcutters)
            {
                amount *= 2;
            }

            DssRef.state.resources.addItem(
                new Resource.ItemResource(
                    resourceType,
                    subTile.terrainQuality,
                    Convert.ToInt32(processTimeLengthSec),
                    amount),
                ref subTile.collectionPointer);

            subTile.SetType(TerrainMainType.Resourses, (int)TerrainResourcesType.Wood, 1);
            EditSubTile editSubTile = new EditSubTile(subTileEnd, subTile, true, true, true);
            editSubTile.Submit();
            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
        }


        public void createWorkOrder(WorkType work, int subWork, int order, IntVector2 targetSubTile, City city)
        {
            this.work = work;
            workSubType = subWork;
            orderId = order;
            subTileStart = subTileEnd;
            subTileEnd = targetSubTile;
            processTimeStartStampSec = Ref.TotalGameTimeSec;
            float dist = VectorExt.Length(subTileEnd.X - subTileStart.X, subTileEnd.Y - subTileStart.Y) / WorldData.TileSubDivitions; //Convrst to WP length

            processTimeLengthSec = finalizeWorkTime(city) +
                dist / DssVar.Men_StandardWalkingSpeed_PerSec;

            switch (work)
            {
                case WorkType.Craft:
                    //SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);
                    //var building = (TerrainBuildingType)subTile.subTerrain;
                    //switch (building)
                    //{ 
                    //    case TerrainBuildingType.Work_Cook:
                    //        workSubType = Subwork_Craft_Food;
                    //        break;
                    //    case TerrainBuildingType.Work_Smith:
                    //        workSubType = Subwork_Craft_Iron;
                    //        break;
                    //}
                    break;

                case WorkType.LocalTrade:
                    {
                        ItemResourceType tradeForItem = (ItemResourceType)workSubType;
                        var toCity = DssRef.world.tileGrid.Get(targetSubTile / WorldData.TileSubDivitions).City();
                        int goldCost = toCity.SellCost(tradeForItem);

                        carry = new ItemResource(ItemResourceType.Gold, 1, 1, goldCost * DssConst.Worker_TrossWorkerCarryWeight);
                    }
                    break;

                case WorkType.TrossCityTrade:
                    {
                        var toCity = DssRef.world.tileGrid.Get(targetSubTile / WorldData.TileSubDivitions).City();
                        int goldCost = toCity.SellCost(ItemResourceType.Food_G);

                        carry = new ItemResource(ItemResourceType.Gold, 1, 1, goldCost);
                    }
                    break;
            }
        }

        public float finalizeWorkTime(City city)
        {
            switch (work)
            {
                case WorkType.Eat:
                    return DssConst.WorkTime_Eat;
                case WorkType.PickUpResource:
                    return DssConst.WorkTime_PickUpResource;
                case WorkType.PickUpProduce:
                    return DssConst.WorkTime_PickUpProduce;
                case WorkType.TrossCityTrade:
                    return DssConst.WorkTime_TrossCityTrade;
                case WorkType.LocalTrade:
                    return DssConst.WorkTime_LocalTrade;
                case WorkType.GatherFoil:
                    SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);
                    switch ((TerrainSubFoilType)subTile.subTerrain)
                    {
                        case TerrainSubFoilType.TreeSoft:
                            return DssConst.WorkTime_GatherFoil_TreeSoft;
                        case TerrainSubFoilType.TreeHard:
                            return DssConst.WorkTime_GatherFoil_TreeHard;
                        case TerrainSubFoilType.DryWood:
                            return DssConst.WorkTime_GatherFoil_DryWood;
                        case TerrainSubFoilType.WheatFarm:
                        case TerrainSubFoilType.LinenFarm:
                        case TerrainSubFoilType.RapeSeedFarm:
                        case TerrainSubFoilType.HempFarm:
                            return DssConst.WorkTime_GatherFoil_FarmCulture;
                        case TerrainSubFoilType.Stones:
                        case TerrainSubFoilType.StoneBlock:
                            return DssConst.WorkTime_GatherFoil_Stones;

                        case TerrainSubFoilType.BogIron:
                            return DssConst.WorkTime_BogIron;
                        default:
                            return -1;//throw new NotImplementedException();
                    }
                case WorkType.Till:
                    return DssConst.WorkTime_Till;
                case WorkType.Plant:
                    return DssConst.WorkTime_Plant;
                case WorkType.Mine:
                    return DssConst.WorkTime_Mine;
                case WorkType.Craft:
                    return DssConst.WorkTime_Craft;

                case WorkType.Build:
                    if (city.Culture == CityCulture.Builders)
                    {
                        return DssConst.WorkTime_Building * 0.5f;
                    }
                    return DssConst.WorkTime_Building;

                case WorkType.Demolish:
                    return DssConst.WorkTime_Demolish;

                case WorkType.TrossReturnToArmy:
                case WorkType.DropOff:
                case WorkType.Exit:
                case WorkType.Starving:
                    return 1f;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
