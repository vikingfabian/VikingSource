﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        /// <remarks>
        /// Changing the list will corrupt the save files!
        /// </remarks>
        public static readonly ItemResourceType[] MovableCityResourceTypes =
        {
             ItemResourceType.Wood_Group,
             ItemResourceType.Fuel_G,
             ItemResourceType.Stone_G,
             ItemResourceType.RawFood_Group,
             ItemResourceType.Food_G,
             ItemResourceType.Beer,
             ItemResourceType.SkinLinen_Group,
             ItemResourceType.IronOre_G,
             ItemResourceType.Iron_G,

             ItemResourceType.Sword,
             ItemResourceType.SharpStick,
             ItemResourceType.TwoHandSword,
             ItemResourceType.KnightsLance,
             ItemResourceType.Bow,
             ItemResourceType.LongBow,
             ItemResourceType.Ballista,            

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

        public int maxWater = DssConst.Maxwater;
        FloatingInt nextWater = new FloatingInt();
        public float waterAddPerSec;
        static readonly GroupedResource Res_Nothing = new GroupedResource() { amount = 100000 };

        public GroupedResource res_water = new GroupedResource();
        public GroupedResource res_wood = new GroupedResource() { amount = 20, goalBuffer = 300 };
        public GroupedResource res_fuel = new GroupedResource() { amount = 20, goalBuffer = 300 };
        public GroupedResource res_stone = new GroupedResource() { amount = 20, goalBuffer = 100 };
        public GroupedResource res_rawFood = new GroupedResource() { amount = 50, goalBuffer = 200 };
        public GroupedResource res_food = new GroupedResource() { amount = 200, goalBuffer = 500 };
        public GroupedResource res_beer = new GroupedResource() { amount = 0, goalBuffer = 200 };
        public GroupedResource res_skinLinnen = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_ironore = new GroupedResource() { goalBuffer = 100 };
        public GroupedResource res_iron = new GroupedResource() { amount = 10, goalBuffer = 100 };

        public GroupedResource res_sharpstick = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_sword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_twohandsword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_knightslance = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_bow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_longbow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        public GroupedResource res_ballista = new GroupedResource() { amount = 0, goalBuffer = 100 };

        public GroupedResource res_lightArmor = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        public GroupedResource res_mediumArmor = new GroupedResource() { amount = 2, goalBuffer = 100 };
        public GroupedResource res_heavyArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };

        public bool res_food_safeguard = true;

        public bool foodSafeGuardIsActive(ItemResourceType item)
        {
            bool food = foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard);
            switch (item)
            {
                case ItemResourceType.Food_G:
                    return food;
                case ItemResourceType.Fuel_G:
                    return fuelSafeGuard;
                case ItemResourceType.RawFood_Group:
                    return rawFoodSafeGuard;
                case ItemResourceType.Wood_Group:
                    return woodSafeGuard;
            }

            return false;
        }

        public bool foodSafeGuardIsActive(out bool fuelSafeGuard, out bool rawFoodSafeGuard, out bool woodSafeGuard)
        {
            if (res_food_safeguard && res_food.amount <= DssConst.WorkSafeGuardAmount)
            {
                fuelSafeGuard = res_fuel.amount <= DssConst.WorkSafeGuardAmount;
                rawFoodSafeGuard = res_rawFood.amount <= DssConst.WorkSafeGuardAmount;
                woodSafeGuard = fuelSafeGuard && res_wood.amount <= DssConst.WorkSafeGuardAmount;
                return true;
            }
            else
            {
                fuelSafeGuard = false;
                rawFoodSafeGuard = false;
                woodSafeGuard = false;
                return false;
            }
        }


        public TradeTemplate tradeTemplate = new TradeTemplate();
        public const int DefaultFoodBuffer = 500;
        public const int Logistics1FoodStorage = 300;
        public void defaultResourceBuffer()
        {
            res_wood.goalBuffer = 300;
            res_fuel.goalBuffer = 300;
            res_stone.goalBuffer = 200;
            res_rawFood.goalBuffer = 200;
            res_food.goalBuffer = DefaultFoodBuffer;
            res_beer.goalBuffer = 200;
            res_skinLinnen.goalBuffer = 100;
            res_ironore.goalBuffer = 100;
            res_iron.goalBuffer = 100;
            res_sharpstick.goalBuffer = 100;
            res_sword.goalBuffer = 100;
            res_twohandsword.goalBuffer = 100;
            res_knightslance.goalBuffer = 100;
            res_bow.goalBuffer = 100;
            res_longbow.goalBuffer = 100;
            res_ballista.goalBuffer = 100;
            res_lightArmor.goalBuffer = 100;
            res_mediumArmor.goalBuffer = 100;
            res_heavyArmor.goalBuffer = 100;

        }

        public void AddGroupedResource(ItemResourceType type, int add)
        {
            if (add == 0)
            { 
                lib.DoNothing();
            }

            switch (type)
            {
                case ItemResourceType.Gold:
                    faction.gold += add;
                    break;
                case ItemResourceType.Water_G:
                    res_water.amount += add;
                    break;
                case ItemResourceType.IronOre_G:
                    res_ironore.amount += add;
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
                case ItemResourceType.Fuel_G:
                    res_fuel.amount += add;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood.amount += add;
                    break;
                case ItemResourceType.SkinLinen_Group:
                    res_skinLinnen.amount += add;
                    break;
                case ItemResourceType.SharpStick:
                    res_sharpstick.amount += add;
                    break;
                case ItemResourceType.Sword:
                    res_sword.amount += add;
                    break;
                case ItemResourceType.TwoHandSword:
                    res_twohandsword.amount += add;
                    break;
                case ItemResourceType.KnightsLance:
                    res_knightslance.amount += add;
                    break;
                case ItemResourceType.Bow:
                    res_bow.amount += add;
                    break;
                case ItemResourceType.LongBow:
                    res_longbow.amount += add;
                    break;
                case ItemResourceType.Ballista:
                    res_ballista.amount += add;
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
                case ItemResourceType.Gold:
                    return new GroupedResource() { amount = faction.gold };
                case ItemResourceType.GoldOre:
                    return new GroupedResource() { amount = 1 };
                case ItemResourceType.Men:
                    return workForce;

                case ItemResourceType.Water_G: return res_water;
                case ItemResourceType.IronOre_G: return res_ironore;
                case ItemResourceType.Iron_G: return res_iron;
                case ItemResourceType.Beer: return res_beer;
                case ItemResourceType.Food_G: return res_food;
                case ItemResourceType.Stone_G: return res_stone;
                case ItemResourceType.Wood_Group: return res_wood;
                case ItemResourceType.Fuel_G: return res_fuel;
                case ItemResourceType.RawFood_Group: return res_rawFood;
                case ItemResourceType.SkinLinen_Group: return res_skinLinnen;

                case ItemResourceType.SharpStick: return res_sharpstick;
                case ItemResourceType.Sword: return res_sword;
                case ItemResourceType.TwoHandSword: return res_twohandsword;
                case ItemResourceType.KnightsLance: return res_knightslance;
                case ItemResourceType.Bow: return res_bow;
                case ItemResourceType.LongBow: return res_longbow;
                case ItemResourceType.Ballista: return res_ballista;

                case ItemResourceType.LightArmor: return res_lightArmor;
                case ItemResourceType.MediumArmor: return res_mediumArmor;
                case ItemResourceType.HeavyArmor: return res_heavyArmor;

                case ItemResourceType.NONE: return Res_Nothing;

                default:
                    throw new NotImplementedException();
            }
        }

        public bool needMore(ItemResourceType type, bool rawfoodSafeGuard, bool woodSafeGuard, out bool usesSafeGuard)
        {
            usesSafeGuard = false;
            switch (type)
            {
                case ItemResourceType.RawFood_Group:
                case ItemResourceType.Wheat:
                case ItemResourceType.Egg:
                case ItemResourceType.Hen:
                    if (rawfoodSafeGuard)
                    { 
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_rawFood.needMore();

                case ItemResourceType.Pig:
                    if (rawfoodSafeGuard)
                    {
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_food.needMore() || res_skinLinnen.needMore();

                case ItemResourceType.Wood_Group:
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                    if (woodSafeGuard)
                    {
                        usesSafeGuard = true;
                        return true;
                    }
                    return res_wood.needMore();

                case ItemResourceType.NONE:
                    return false;

                default:
//#if DEBUG
                    return GetGroupedResource(type).needMore();
                    //throw new NotImplementedException();
//#else
//                    return false;
//#endif
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
                    res_ironore = resource;
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
                case ItemResourceType.Fuel_G:
                    res_fuel = resource;
                    break;
                case ItemResourceType.RawFood_Group:
                    res_rawFood = resource;
                    break;
                case ItemResourceType.SkinLinen_Group:
                    res_skinLinnen = resource;
                    break;

                case ItemResourceType.SharpStick:
                    res_sharpstick = resource;
                    break;
                case ItemResourceType.Sword:
                    res_sword = resource;
                    break;
                case ItemResourceType.TwoHandSword:
                    res_twohandsword = resource;
                    break;
                case ItemResourceType.KnightsLance:
                    res_knightslance = resource;
                    break;
                case ItemResourceType.Bow:
                    res_bow = resource;
                    break;
                case ItemResourceType.LongBow:
                    res_longbow = resource;
                    break;
                case ItemResourceType.Ballista:
                    res_ballista = resource;
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

                case ItemResourceType.Gold:
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

        public void dropOffItem(ItemResource item, out ItemResource convert1, out ItemResource convert2)
        {
            convert1 = item;
            convert2 = ItemResource.Empty;

            switch (item.type)
            {
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    convert1.type = ItemResourceType.Wood_Group;
                    break;
                
                case ItemResourceType.Coal:
                    convert1.type = ItemResourceType.Fuel_G;
                    break;

                case ItemResourceType.Wheat:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.WheatFoodAmount;
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.HenRawFoodAmout;
                    animalResourceBonus(ref item);
                    break;

                case ItemResourceType.Pig:
                    convert1.type = ItemResourceType.RawFood_Group;
                    convert1.amount = DssConst.PigRawFoodAmout;
                    animalResourceBonus(ref item);

                    convert2 = new ItemResource(ItemResourceType.SkinLinen_Group, 1, 1, DssConst.PigSkinAmount);
                    break;

                case ItemResourceType.Linen:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    convert1.amount = DssConst.LinenHarvestAmount;
                    break;

                case ItemResourceType.Rapeseed:
                    convert1.type = ItemResourceType.Fuel_G;
                    convert1.amount = DssConst.RapeSeedFuelAmount;
                    break;

                case ItemResourceType.Hemp:
                    convert1.type = ItemResourceType.SkinLinen_Group;
                    convert1.amount = DssConst.HempLinenAndFuelAmount;

                    convert2.type = ItemResourceType.Fuel_G;
                    convert2.amount = DssConst.HempLinenAndFuelAmount;
                    break;

                case ItemResourceType.GoldOre:
                    {
                        var price = convert1.amount * DssConst.GoldOreSellValue;
                        faction.gold += price;
                        soldResources.add(price);

                        convert1.type = ItemResourceType.Gold;
                        convert1.amount = price;
                    }
                    break;
            }

            if (Ref.rnd.Chance(DssRef.difficulty.resourceMultiplyChance) &&
                faction.player.IsAi())
            {
                if (DssRef.difficulty.resourceMultiplyDecrease)
                {
                    return;
                }
                else
                {
                    convert1.amount *= 2;
                    convert2.amount *= 2;
                }
            }

            AddGroupedResource(convert1.type, convert1.amount);
            if (convert2.amount > 0)
            {
                AddGroupedResource(convert2.type, convert2.amount);
            }
        }

        void animalResourceBonus(ref ItemResource item)
        {
            if (Culture == CityCulture.AnimalBreeder)
            {
                item.amount *= 2;
            }
        }
        
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


    struct GroupedResource
    {
        public int amount;
        //public int backOrder;
        public int goalBuffer;
        //public int orderQueCount;
        public int deliverCount;

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

        //public int freeAmount()
        //{ 
        //    return amount - backOrder;
        //}

        public bool needMore()
        {
            return amount < goalBuffer;
        }

        public bool needToImport()
        {
            return amount < goalBuffer;
        }

        public bool canTradeAway()
        {
            return amount >= goalBuffer;
        }

        public int amountPlusDelivery()
        {
            return amount + deliverCount;
        }

        public void add(ItemResource item, int multiply = 1)
        {
            amount += item.amount * multiply;
        }

        public void toMenu(RichBoxContent content, ItemResourceType item, bool safeGuard, ref bool reachedBuffer)
        {
            content.newLine();
            
            content.Add(new RichBoxImage(ResourceLib.Icon(item)));
            content.Add(new RichBoxText( LangLib.Item(item) + ": " + TextLib.LargeNumber(amount)));

            if (item != ItemResourceType.Water_G && 
                item != ItemResourceType.Gold &&
                item != ItemResourceType.Men)
            {
                bool reached = amount >= goalBuffer;
                reachedBuffer |= reached;
                SpriteName stockIcon;
                if (safeGuard)
                {
                    stockIcon = SpriteName.WarsStockpileAdd_Protected;
                }
                else if (reached)
                {
                    stockIcon = SpriteName.WarsStockpileStop;
                }
                else
                {
                    stockIcon = SpriteName.WarsStockpileAdd;
                }
                var icon = new RichBoxImage(stockIcon);
                content.Add(icon);
            }
            
        }

        public static void BufferIconInfo(RichBoxContent content, bool safeguard)
        {
            content.newLine();
            SpriteName sprite;
            string textstring;
            if (safeguard)
            {
                sprite = SpriteName.WarsStockpileAdd_Protected;
                textstring = DssRef.lang.Resource_FoodSafeGuard_Active;
            }
            else
            {
                sprite = SpriteName.WarsStockpileStop;
                textstring = DssRef.lang.Resource_ReachedStockpile;
            }


            var icon = new RichBoxImage(sprite);
            content.Add(icon);

            var text = new RichBoxText(": " + textstring);
            text.overrideColor = HudLib.InfoYellow_Light;
            content.Add(text);
        }

        //public void clearOrders()
        //{ 
        //    backOrder = 0;
        //    //orderQueCount = 0;
        //}

        public override string ToString()
        {
            return $"Grouped resource {amount}/{goalBuffer}";
        }
    }

    
}
