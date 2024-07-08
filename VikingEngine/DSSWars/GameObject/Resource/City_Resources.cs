using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        //Simplified resources
        int water = 100;
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

        public void resourcesToMenu(RichBoxContent content)
        {
            content.h1("Resources");
            content.newLine();

            content.text("Water: " + water.ToString());
            wood.toMenu(content, "wood");
            rawFood.toMenu(content, "rawFood");
            food.toMenu(content, "food");
            skin.toMenu(content, "skin");
            ore.toMenu(content, "ore");
            iron.toMenu(content, "iron");

        }
    }

    struct SimplifiedResource
    {
        public int amount;
        public int goalBuffer;
        public int trade;
        public int blacktrade;

        public void add(ItemResource item)
        {
            amount += item.amount;
        }

        public void toMenu(RichBoxContent content, string name)
        {
            content.text(name + ": " + amount.ToString());
        }
    }
}
