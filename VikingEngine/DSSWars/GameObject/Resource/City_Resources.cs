using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
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
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    wood.add(item);
                    break;

                case ItemResourceType.Stone_G:
                    stone.add(item);
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                case ItemResourceType.Wheat:
                    rawFood.add(item, DssConst.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Pig:
                    rawFood.add(item, DssConst.PigRawFoodAmout);
                    skinLinnen.add(item);
                    break;

                case ItemResourceType.Linnen:
                    skinLinnen.add(item);
                    break;

                case ItemResourceType.IronOre_G:
                    //ore.add(item);
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

        public void craftItem(TerrainBuildingType building, out bool canCraftAgain)
        {
            canCraftAgain = false;

            switch (building)
            {
                case TerrainBuildingType.Work_Cook:
                    var addFood = ResourceLib.CraftFood.craft(this);
                    food.amount += addFood;
                    foodProduction.add(addFood);
                    canCraftAgain = ResourceLib.CraftFood.canCraft(this);
                    break;
                case TerrainBuildingType.Work_Smith:
                    iron.amount += ResourceLib.CraftIron.craft(this);
                    canCraftAgain = ResourceLib.CraftFood.canCraft(this);
                    break;
                //case TerrainBuildingType.WorkerHut:
                //    ResourceLib.CraftWorkerHut.craft(this);
                //    onWorkHutBuild();
                //    break;
            }
        }

        public void tradeTab()
        { 
            
        }

        public void resourcesToMenu(RichBoxContent content)
        {
            content.Add(new RichBoxSeperationLine());

            content.h1("Resources");
            content.newLine();

            // content.text("Water: " + water.ToString());
            water.toMenu(content, DssRef.todoLang.Resource_TypeName_Water);
            wood.toMenu(content, DssRef.todoLang.Resource_TypeName_Wood);
            stone.toMenu(content, DssRef.todoLang.Resource_TypeName_Stone);
            rawFood.toMenu(content, DssRef.todoLang.Resource_TypeName_RawFood);
            food.toMenu(content, DssRef.todoLang.Resource_TypeName_Food);
            skinLinnen.toMenu(content, DssRef.todoLang.Resource_TypeName_SkinAndLinnen);
            ore.toMenu(content, DssRef.todoLang.Resource_TypeName_Ore);
            
            content.newParagraph();
            content.h2("Crafting blueprints");
            ResourceLib.CraftFood.toMenu(content);
            ResourceLib.CraftIron.toMenu(content);
            ResourceLib.CraftSharpStick.toMenu(content);
            ResourceLib.CraftSword.toMenu(content);
            ResourceLib.CraftBow.toMenu(content);
            ResourceLib.CraftLightArmor.toMenu(content);
            ResourceLib.CraftMediumArmor.toMenu(content);
            ResourceLib.CraftHeavyArmor.toMenu(content);

            ResourceLib.CraftWorkerHut.toMenu(content);
            //content.text("1 iron => " + DssConst.IronSellValue.ToString() + "gold");
            content.text("1 gold ore => " + DssConst.GoldOreSellValue.ToString() + "gold");
            content.text("1 food => " + DssConst.FoodEnergy + " energy (seconds of work)");
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
