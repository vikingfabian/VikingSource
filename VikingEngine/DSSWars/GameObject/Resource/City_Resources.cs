using Microsoft.Xna.Framework;
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
             ItemResourceType.Beer,
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

        public int maxWater;
        static readonly GroupedResource Res_Nothing = new GroupedResource() { amount = 100000 };

        public GroupedResource res_water = new GroupedResource();
        public GroupedResource res_wood = new GroupedResource() { amount = 20,  goalBuffer = 300 };
        public GroupedResource res_stone = new GroupedResource() { amount = 20, goalBuffer = 100 };
        public GroupedResource res_rawFood = new GroupedResource() { amount = 50, goalBuffer = 200 };
        public GroupedResource res_food = new GroupedResource() { amount = 200, goalBuffer = 500 };
        public GroupedResource res_beer = new GroupedResource() { amount = 0, goalBuffer = 200 };
        public GroupedResource res_skinLinnen = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ore = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_iron = new GroupedResource() { goalBuffer = 100 };

        public GroupedResource res_sharpstick = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_sword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_bow = new GroupedResource() { amount = 0, goalBuffer = 100 };

        public GroupedResource res_lightArmor = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_mediumArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_heavyArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };

        //bool useLocalTrade
        public TradeTemplate tradeTemplate = new TradeTemplate();

        //int tradeGold = 0;

        public void AddGroupedResource(ItemResourceType type, int add)
        {
            switch (type)
            {
                case ItemResourceType.Water_G:
                    res_water.amount += add;
                    break;
                case ItemResourceType.IronOre_G:
                    res_ore.amount += add;
                    break;
                case ItemResourceType.Iron_G:
                    res_iron.amount += add;
                    break;
                case ItemResourceType.Food_G:
                    res_food.amount += add;
                    break;
                case ItemResourceType.Beer:
                    res_beer.amount += add;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone.amount += add;
                    break;
                case ItemResourceType.Wood_Group:
                    res_wood.amount += add;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood.amount += add;
                    break;
                case ItemResourceType.SkinLinnen_Group:
                    res_skinLinnen.amount += add;
                    break;
                case ItemResourceType.SharpStick:
                    res_sharpstick.amount += add;
                    break;
                case ItemResourceType.Sword:
                    res_sword.amount += add;
                    break;
                case ItemResourceType.Bow:
                    res_bow.amount += add;
                    break;
                case ItemResourceType.LightArmor:
                    res_lightArmor.amount += add;
                    break;
                case ItemResourceType.MediumArmor:
                    res_mediumArmor.amount += add;
                    break;
                case ItemResourceType.HeavyArmor:
                    res_heavyArmor.amount += add;
                    break;
                case ItemResourceType.NONE:
                    return;

                default:
                    throw new NotImplementedException();
            }
        }

        public GroupedResource GetGroupedResource(ItemResourceType type)
        {
            switch (type)
            {
                case ItemResourceType.Water_G: return res_water;
                case ItemResourceType.IronOre_G: return res_ore;
                case ItemResourceType.Iron_G: return res_iron;
                case ItemResourceType.Beer: return res_beer;
                case ItemResourceType.Food_G: return res_food;
                case ItemResourceType.Stone_G: return res_stone;
                case ItemResourceType.Wood_Group: return res_wood;
                case ItemResourceType.RawFood_Group: return res_rawFood;
                case ItemResourceType.SkinLinnen_Group: return res_skinLinnen;

                case ItemResourceType.SharpStick: return res_sharpstick;
                case ItemResourceType.Sword: return res_sword;
                case ItemResourceType.Bow: return res_bow;

                case ItemResourceType.LightArmor: return res_lightArmor;
                case ItemResourceType.MediumArmor: return res_mediumArmor;
                case ItemResourceType.HeavyArmor: return res_heavyArmor;

                case ItemResourceType.NONE: return Res_Nothing;

                default:
                    throw new NotImplementedException();
            }
        }

        public void SetGroupedResource(ItemResourceType type, GroupedResource resource)
        {
            switch (type)
            {
                case ItemResourceType.Water_G:
                    res_water = resource;
                    break;
                case ItemResourceType.IronOre_G:
                    res_ore = resource;
                    break;
                case ItemResourceType.Iron_G:
                    res_iron = resource;
                    break;
                case ItemResourceType.Food_G:
                    res_food = resource;
                    break;
                case ItemResourceType.Beer:
                    res_beer = resource;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone = resource;
                    break;
                case ItemResourceType.Wood_Group:
                    res_wood = resource;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood = resource;
                    break;
                case ItemResourceType.SkinLinnen_Group:
                    res_skinLinnen = resource;
                    break;

                case ItemResourceType.SharpStick:
                    res_sharpstick = resource;
                    break;
                case ItemResourceType.Sword:
                    res_sword = resource;
                    break;
                case ItemResourceType.Bow:
                    res_bow = resource;
                    break;
                case ItemResourceType.LightArmor:
                    res_lightArmor = resource;
                    break;
                case ItemResourceType.MediumArmor:
                    res_mediumArmor = resource;
                    break;
                case ItemResourceType.HeavyArmor:
                    res_heavyArmor = resource;
                    break;

                case ItemResourceType.NONE:
                    return;

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
                    res_wood.amount -= carry;
                    break;
                case ItemResourceType.Stone_G:
                    res_stone.amount -= carry;
                    break;
                case ItemResourceType.Food_G:
                    res_food.amount -= carry;
                    break;
                case ItemResourceType.Iron_G:
                    res_iron.amount -= carry;
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
                    res_wood.add(item);
                    break;

                case ItemResourceType.Stone_G:
                    res_stone.add(item);
                    break;

                case ItemResourceType.Wheat:
                    res_rawFood.add(item, DssConst.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                    animalResourceBonus(ref item);
                    res_rawFood.add(item, DssConst.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Pig:
                    animalResourceBonus(ref item);
                    res_rawFood.add(item, DssConst.PigRawFoodAmout);
                    res_skinLinnen.add(item);
                    break;

                case ItemResourceType.Linnen:
                    res_skinLinnen.add(item);
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



        public void blackMarketPurchase(ItemResourceType resourceType, int count, int cost)
        {
            if (faction.payMoney(cost * count, false))
            {
                AddGroupedResource(resourceType, count);
            }
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
        //public int trade;
        //public int blacktrade;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(amount);
            w.Write((ushort)goalBuffer);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            amount = r.ReadInt32();
            goalBuffer = r.ReadUInt16();
        }

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
