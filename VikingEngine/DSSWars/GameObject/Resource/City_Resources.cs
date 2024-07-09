using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        static readonly CraftBlueprint CraftFood = new CraftBlueprint("Food", 5, 5, 1, 0, 10);
        static readonly CraftBlueprint CraftIron = new CraftBlueprint("Iron", 1, 20, 0, 2, 4);


        //Simplified resources
        const int Maxwater = 100;
        int water = Maxwater;
        int waterBuffer = 10;
        int waterSpendOrders = 0;
        
        SimplifiedResource wood = new SimplifiedResource() { goalBuffer = 10 };
        SimplifiedResource rawFood = new SimplifiedResource() { goalBuffer = 20 };
        SimplifiedResource food = new SimplifiedResource() { goalBuffer = 100 };
        SimplifiedResource skin = new SimplifiedResource() { goalBuffer = 10 };
        SimplifiedResource ore = new SimplifiedResource() { goalBuffer = 10 };
        SimplifiedResource iron = new SimplifiedResource() { goalBuffer = 10 };

        public void dropOffItem(ItemResource item)
        {
            switch (item.type)
            {
                case ItemResourceType.SoftWood:
                case  ItemResourceType.HardWood:
                    wood.add(item);
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
            }
        }

        public void craftItem(TerrainBuildingType building)
        {
            switch (building)
            {
                case TerrainBuildingType.Work_Cook:
                    food.amount += CraftFood.craft(ref water, ref wood.amount, ref rawFood.amount, ref ore.amount);
                    break;
                case TerrainBuildingType.Work_Smith:
                    iron.amount += CraftIron.craft(ref water, ref wood.amount, ref rawFood.amount, ref ore.amount);
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
            rawFood.toMenu(content, "rawFood");
            food.toMenu(content, "food");
            skin.toMenu(content, "skin");
            ore.toMenu(content, "ore");
            iron.toMenu(content, "iron");

            content.newParagraph();
            content.h2("Crafting blueprints");
            CraftFood.toMenu(content);
            CraftIron.toMenu(content);
        }
    }

    struct SimplifiedResource
    {
        public int amount;
        public int goalBuffer;
        public int orderCount;
        public int trade;
        public int blacktrade;

        public bool needMore()
        {
            return amount + orderCount < goalBuffer;
        }

        public void add(ItemResource item)
        {
            amount += item.amount;
        }

        public void toMenu(RichBoxContent content, string name)
        {
            content.text(name + ": " + amount.ToString());
        }
    }

    class CraftBlueprint
    {
        string name;

        int useWater;
        int useWood;
        int useRawFood;
        int useOre;

        int resultCount;

        public CraftBlueprint(string name,int water, int wood, int rawfood, int ore, int result)
        { 
            this.name = name;
            this.useWater = water;
            this.useWood = wood;
            this.useRawFood = rawfood;
            this.useOre = ore;
            this.resultCount = result;
        }

        public bool available(int water, int wood, int rawfood, int ore)
        {
            return water >= useWater &&
                wood >= useWood &&
                rawfood >= useRawFood &&
                ore >= useOre;
        }

        public int craft(ref int water,ref int wood, ref int rawfood, ref int ore)
        {
            water -= useWater;
            wood-=useWood;
            rawfood-=useRawFood;
            ore-=useOre;

            return resultCount;
        }

        public void toMenu(RichBoxContent content)
        {
            string resources = string.Empty;

            addResources(useWater, "water", ref resources);
            addResources(useWood, "wood", ref resources);
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
