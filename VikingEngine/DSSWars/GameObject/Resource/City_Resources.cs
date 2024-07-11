using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        static readonly CraftBlueprint CraftFood = new CraftBlueprint("Food", 10) { useWater = 5, useWood = 5, useRawFood = 1 };
        static readonly CraftBlueprint CraftIron = new CraftBlueprint("Iron", 4) { useWater = 1, useWood = 20, useOre = 2 } ;
        static readonly CraftBlueprint CraftWorkerHut = new CraftBlueprint("Worker hut", 1) { useWater = 0, useWood = 200, useStone = 40 };

        const int GoldOreValue = 10;

        //Simplified resources
        const int Maxwater = 100;
        public int water = Maxwater;
        int waterBuffer = 10;
        int waterSpendOrders = 0;
        
        public SimplifiedResource wood = new SimplifiedResource() { goalBuffer = 300 };
        public SimplifiedResource stone = new SimplifiedResource() { goalBuffer = 100 };
        public SimplifiedResource rawFood = new SimplifiedResource() { goalBuffer = 200 };
        public SimplifiedResource food = new SimplifiedResource() { goalBuffer = 500 };
        public SimplifiedResource skin = new SimplifiedResource() { goalBuffer = 100 };
        public SimplifiedResource ore = new SimplifiedResource() { goalBuffer = 100 };
        public SimplifiedResource iron = new SimplifiedResource() { goalBuffer = 100 };

        int tradeGold = 0;



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
                    rawFood.add(item);
                    break;

                case ItemResourceType.Pig:
                    rawFood.add(item);
                    skin.add(item);
                    break;

                case ItemResourceType.IronOre:
                    ore.add(item);
                    break;

                case ItemResourceType.GoldOre:
                    tradeGold += item.amount * GoldOreValue;
                    break;
            }
        }

        public void craftItem(TerrainBuildingType building, out bool canCraftAgain)
        {
            canCraftAgain = false;

            switch (building)
            {
                case TerrainBuildingType.Work_Cook:
                    food.amount += CraftFood.craft(this);
                    canCraftAgain = CraftFood.canCraft(this);
                    break;
                case TerrainBuildingType.Work_Smith:
                    iron.amount += CraftIron.craft(this);
                    canCraftAgain = CraftFood.canCraft(this);
                    break;
                case TerrainBuildingType.WorkerHut:
                    CraftWorkerHut.craft(this);
                    onWorkHutBuild();
                    break;
            }
        }

        public void resourcesToMenu(RichBoxContent content)
        {
            content.Add(new RichBoxSeperationLine());

            content.h1("Resources");
            content.newLine();

            content.text("Water: " + water.ToString());
            wood.toMenu(content, "wood");
            stone.toMenu(content, "stone");
            rawFood.toMenu(content, "rawFood");
            food.toMenu(content, "food");
            skin.toMenu(content, "skin");
            ore.toMenu(content, "ore");
            iron.toMenu(content, "iron");

            content.newParagraph();
            content.h2("Crafting blueprints");
            CraftFood.toMenu(content);
            CraftIron.toMenu(content);
            CraftWorkerHut.toMenu(content);
            content.text("1 gold ore => " + GoldOreValue.ToString() + "gold");
        }
    }

    struct SimplifiedResourceCollection
    { 
        
    }

    struct SimplifiedResource
    {
        public int amount;
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

        public bool canTradeAway()
        {
            return (amount - backOrder) >= goalBuffer;
        }

        public void add(ItemResource item)
        {
            amount += item.amount;
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

    class CraftBlueprint
    {
        string name;

        public int useWater;
        public int useStone;
        public int useWood;
        public int useRawFood;
        public int useOre;

        int resultCount;

        public CraftBlueprint(string name, int result)
        { 
            this.name = name;
            //this.useWater = water;
            //this.useStone = stone;
            //this.useWood = wood;
            //this.useRawFood = rawfood;
            //this.useOre = ore;
            this.resultCount = result;
        }

        public void createBackOrder(City city)
        {
            city.wood.backOrder += useWood;
            city.stone.backOrder += useStone;
            city.rawFood.backOrder += useRawFood;
            city.ore.backOrder += useOre;
        }

        public bool available(City city)
        {
            bool result = city.water >= useWater &&
                city.wood.freeAmount() >= useWood &&
                city.stone.freeAmount() >= useStone &&
                city.rawFood.freeAmount() >= useRawFood &&
                city.ore.freeAmount() >= useOre;

            return result;
        }

        public bool canCraft(City city)
        {
            bool result = city.water >= useWater &&
                city.wood.amount >= useWood &&
                city.stone.amount >= useStone &&
                city.rawFood.amount >= useRawFood &&
                city.ore.amount >= useOre;

            return result;
        }

        public int craft(City city)//ref int water,ref int wood, ref int stone, ref int rawfood, ref int ore)
        {
            city.water -= useWater;
            city.wood.amount -= useWood;
            city.stone.amount -= useStone;
            city.rawFood.amount -= useRawFood;
            city.ore.amount -= useOre;

            if (city.stone.amount < 0 || city.wood.amount < 0)
            { 
                lib.DoNothing();
            }

            return resultCount;
        }

        public void toMenu(RichBoxContent content)
        {
            string resources = string.Empty;

            addResources(useWater, "water", ref resources);
            addResources(useWood, "wood", ref resources);
            addResources(useWood, "stone", ref resources);
            addResources(useRawFood, "raw food", ref resources);
            addResources(useOre, "ore", ref resources);


            content.text(resources + " => " + resultCount.ToString());

            void addResources(int count, string name, ref string resources)
            {
                if (count > 0)
                {
                    if (resources.Length > 0)
                    {
                        resources += " + ";
                    }
                    resources += count.ToString() + " " + name;
                }
            }
        }
    }
}
