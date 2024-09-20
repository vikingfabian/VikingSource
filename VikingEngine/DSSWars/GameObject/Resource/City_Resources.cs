﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public static readonly ItemResourceType[] MovableCityResourceTypes =
        {
             ItemResourceType.Wood_Group,
             ItemResourceType.Stone_G,
             ItemResourceType.RawFood_Group,
             ItemResourceType.Food_G,
             ItemResourceType.SkinLinnen_Group,
             ItemResourceType.IronOre_G,
             ItemResourceType.Iron_G,

             ItemResourceType.SharpStick,
             ItemResourceType.Sword,
             ItemResourceType.Bow,

             ItemResourceType.LightArmor,
             ItemResourceType.MediumArmor,
             ItemResourceType.HeavyArmor,
        };

        MinuteStats blackMarketCosts_food = new MinuteStats();
        public MinuteStats foodProduction = new MinuteStats();
        public MinuteStats foodSpending = new MinuteStats();
        public MinuteStats soldResources = new MinuteStats();

        //public int water = Maxwater;
        int waterBuffer = 2;
        int waterSpendOrders = 0;

        public GroupedResource water = new GroupedResource() { amount = DssConst.Maxwater, goalBuffer = DssConst.Maxwater };
        public GroupedResource wood = new GroupedResource() { amount = 20,  goalBuffer = 300 };
        public GroupedResource stone = new GroupedResource() { amount = 20, goalBuffer = 100 };
        public GroupedResource rawFood = new GroupedResource() { amount = 50, goalBuffer = 200 };
        public GroupedResource food = new GroupedResource() { amount = 200, goalBuffer = 500 };
        public GroupedResource skinLinnen = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource ore = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource iron = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource sharpstick = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource sword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource bow = new GroupedResource() { amount = 0, goalBuffer = 100 };

        public GroupedResource lightArmor = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource mediumArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource heavyArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };

        //bool useLocalTrade
        public TradeTemplate tradeTemplate = new TradeTemplate();

        //int tradeGold = 0;

        public GroupedResource GetGroupedResource(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.Water_G: return water;
                case ItemResourceType.IronOre_G: return ore;
                case ItemResourceType.Iron_G: return iron;
                case ItemResourceType.Food_G: return food;
                case ItemResourceType.Stone_G: return stone;
                case ItemResourceType.Wood_Group: return wood;
                case ItemResourceType.RawFood_Group: return rawFood;
                case ItemResourceType.SkinLinnen_Group: return skinLinnen;

                case ItemResourceType.SharpStick: return sharpstick;
                case ItemResourceType.Sword: return sword;
                case ItemResourceType.Bow: return bow;

                case ItemResourceType.LightArmor: return lightArmor;
                case ItemResourceType.MediumArmor: return mediumArmor;
                case ItemResourceType.HeavyArmor: return heavyArmor;

                default:
                    throw new NotImplementedException();
            }
        }

        public void SetGroupedResource(ItemResourceType type, GroupedResource resource)
        {
            switch (type)
            {
                case ItemResourceType.Water_G:
                    water = resource;
                    break;
                case ItemResourceType.IronOre_G:
                    ore = resource;
                    break;
                case ItemResourceType.Iron_G:
                    iron = resource;
                    break;
                case ItemResourceType.Food_G:
                    food = resource;
                    break;
                case ItemResourceType.Stone_G:
                    stone = resource;
                    break;
                case ItemResourceType.Wood_Group:
                    wood = resource;
                    break;
                case ItemResourceType.RawFood_Group:
                    rawFood = resource;
                    break;
                case ItemResourceType.SkinLinnen_Group:
                    skinLinnen = resource;
                    break;

                case ItemResourceType.SharpStick:
                    sharpstick = resource;
                    break;
                case ItemResourceType.Sword:
                    sword = resource;
                    break;
                case ItemResourceType.Bow:
                    bow = resource;
                    break;
                case ItemResourceType.LightArmor:
                    lightArmor = resource;
                    break;
                case ItemResourceType.MediumArmor:
                    mediumArmor = resource;
                    break;
                case ItemResourceType.HeavyArmor:
                    heavyArmor = resource;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public int SellCost(ItemResourceType itemResourceType)
        {
            TradeResource resource;
            switch (itemResourceType)
            {
                case ItemResourceType.HardWood:
                case ItemResourceType.SoftWood:
                    resource = tradeTemplate.wood;
                    break;
                case ItemResourceType.Stone_G:
                    resource = tradeTemplate.stone;
                    break;
                case ItemResourceType.Food_G:
                    resource = tradeTemplate.food;
                    break;
                case ItemResourceType.Iron_G:
                    resource = tradeTemplate.iron;
                    break;

                default:
                    throw new NotImplementedException();
            }

            int goldCost = (int)Math.Ceiling( ItemPropertyColl.CarryAmount(itemResourceType) * resource.price);

            return goldCost;
        }

        public ItemResource MakeTrade(ItemResourceType itemResourceType, int payment, float maxWeight = 1f)
        {
            int carry = ItemPropertyColl.CarryAmount(itemResourceType, maxWeight);
            switch (itemResourceType)
            {
                case ItemResourceType.SoftWood:
                    wood.amount -= carry;
                    break;
                case ItemResourceType.Stone_G:
                    stone.amount -= carry;
                    break;
                case ItemResourceType.Food_G:
                    food.amount -= carry;
                    break;
                case ItemResourceType.Iron_G:
                    iron.amount -= carry;
                    break;

                default:
                    throw new NotImplementedException();
            }

            return new ItemResource(itemResourceType, 1, payment, carry);
        }

        public void dropOffItem(ItemResource item)
        {
            switch (item.type)
            {
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    wood.add(item);
                    break;

                case ItemResourceType.Stone_G:
                    stone.add(item);
                    break;

                case ItemResourceType.Wheat:
                    rawFood.add(item, DssConst.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                    animalResourceBonus(ref item);
                    rawFood.add(item, DssConst.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Pig:
                    animalResourceBonus(ref item);
                    rawFood.add(item, DssConst.PigRawFoodAmout);
                    skinLinnen.add(item);
                    break;

                case ItemResourceType.Linnen:
                    skinLinnen.add(item);
                    break;

                case ItemResourceType.IronOre_G:
                    {
                        var price = item.amount * DssConst.IronSellValue;
                        faction.gold += price;
                        soldResources.add(price);
                    }
                    break;

                case ItemResourceType.GoldOre:
                    {
                        var price = item.amount * DssConst.GoldOreSellValue;
                        faction.gold += price;
                        soldResources.add(price);
                    }
                    break;
            }
        }

        void animalResourceBonus(ref ItemResource item)
        {
            if (Culture == CityCulture.AnimalBreeder)
            {
                item.amount *= 2;
            }
        }
        //public void craftItem(TerrainBuildingType building, out bool canCraftAgain)
        //{
        //    canCraftAgain = false;

        //    switch (building)
        //    {
        //        case TerrainBuildingType.Work_Cook:
        //            var addFood = ResourceLib.CraftFood.craft(this);
        //            food.amount += addFood;
        //            foodProduction.add(addFood);
        //            canCraftAgain = ResourceLib.CraftFood.canCraft(this);
        //            break;
        //        case TerrainBuildingType.Work_Smith:
        //            iron.amount += ResourceLib.CraftIron.craft(this);
        //            canCraftAgain = ResourceLib.CraftFood.canCraft(this);
        //            break;
        //    }
        //}

        //public void craftBuild(TerrainBuildingType building)
        //{

        //}

        public void tradeTab()
        { 
            
        }

        
    }   


    //struct SimplifiedResourceCollection
    //{ 
        
    //}

    struct GroupedResource
    {
        public int amount;
        //public float saleValue;
        public int backOrder;
        public int goalBuffer;
        public int orderQueCount;
        public int trade;
        public int blacktrade;

        public int freeAmount()
        { 
            return amount - backOrder;
        }

        public bool needMore()
        {
            return (amount + orderQueCount - backOrder) < goalBuffer;
        }

        public bool needToImport()
        {
            return amount < goalBuffer;
        }

        public bool canTradeAway()
        {
            return (amount - backOrder) >= goalBuffer;
        }

        public void add(ItemResource item, int multiply = 1)
        {
            amount += item.amount * multiply;
        }

        public void toMenu(RichBoxContent content, string name)
        {
            content.text(name + ": " + amount.ToString());
        }

        public void clearOrders()
        { 
            backOrder = 0;
            orderQueCount = 0;
        }
    }

    
}
