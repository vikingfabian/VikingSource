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

        const int GoldOreSellValue = 100;
        const int IronSellValue = 5;
        public const float FoodGoldValue = 2f;
        public const float FoodGoldValue_BlackMarket = FoodGoldValue * 5;

        //Simplified resources
        const int Maxwater = 10;
        public int water = Maxwater;
        int waterBuffer = 2;
        int waterSpendOrders = 0;
        
        public SimplifiedResource wood = new SimplifiedResource() { amount = 20,  goalBuffer = 300 };
        public SimplifiedResource stone = new SimplifiedResource() { amount = 20, goalBuffer = 100 };
        public SimplifiedResource rawFood = new SimplifiedResource() { amount = 50, goalBuffer = 200 };
        public SimplifiedResource food = new SimplifiedResource() { amount = 200, goalBuffer = 500 };
        public SimplifiedResource skin = new SimplifiedResource() { goalBuffer = 100 };
        public SimplifiedResource ore = new SimplifiedResource() { goalBuffer = 100 };
        public SimplifiedResource iron = new SimplifiedResource() { goalBuffer = 100 };

        //bool useLocalTrade
        public TradeTemplate tradeTemplate = new TradeTemplate();

        //int tradeGold = 0;

        public int SellCost(ItemResourceType itemResourceType)
        {
            TradeResource resource;
            switch (itemResourceType)
            {
                case ItemResourceType.HardWood:
                case ItemResourceType.SoftWood:
                    resource = tradeTemplate.wood;
                    break;
                case ItemResourceType.Stone:
                    resource = tradeTemplate.stone;
                    break;
                case ItemResourceType.Food:
                    resource = tradeTemplate.food;
                    break;
                case ItemResourceType.Iron:
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
                case ItemResourceType.Stone:
                    stone.amount -= carry;
                    break;
                case ItemResourceType.Food:
                    food.amount -= carry;
                    break;
                case ItemResourceType.Iron:
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

                case ItemResourceType.Stone:
                    stone.add(item);
                    break;

                case ItemResourceType.Egg:                                   
                case ItemResourceType.Hen:
                case ItemResourceType.Wheat:
                    rawFood.add(item, ResourceLib.DefaultItemRawFoodAmout);
                    break;

                case ItemResourceType.Pig:
                    rawFood.add(item, ResourceLib.PigRawFoodAmout);
                    skin.add(item);
                    break;

                case ItemResourceType.IronOre:
                    //ore.add(item);
                    {
                        var price = item.amount * IronSellValue;
                        faction.gold += price;
                        soldResources.add(price);
                    }
                    break;

                case ItemResourceType.GoldOre:
                    {
                        var price = item.amount * GoldOreSellValue;
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
                case TerrainBuildingType.WorkerHut:
                    ResourceLib.CraftWorkerHut.craft(this);
                    onWorkHutBuild();
                    break;
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

            content.text("Water: " + water.ToString());
            wood.toMenu(content, DssRef.todoLang.Resource_TypeName_Wood);
            stone.toMenu(content, DssRef.todoLang.Resource_TypeName_Stone);
            rawFood.toMenu(content, DssRef.todoLang.Resource_TypeName_RawFood);
            food.toMenu(content, DssRef.todoLang.Resource_TypeName_Food);
            skin.toMenu(content, DssRef.todoLang.Resource_TypeName_Skin);
            ore.toMenu(content, DssRef.todoLang.Resource_TypeName_Ore);
            //iron.toMenu(content, "iron");

            content.newParagraph();
            content.h2("Crafting blueprints");
            ResourceLib.CraftFood.toMenu(content);
            ResourceLib.CraftIron.toMenu(content);
            ResourceLib.CraftWorkerHut.toMenu(content);
            content.text("1 iron => " + IronSellValue.ToString() + "gold");
            content.text("1 gold ore => " + GoldOreSellValue.ToString() + "gold");
            content.text("1 food => " + ResourceLib.FoodEnergy + " energy (seconds of work)");
        }
    }

    //struct SimplifiedResourceCollection
    //{ 
        
    //}

    struct SimplifiedResource
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
