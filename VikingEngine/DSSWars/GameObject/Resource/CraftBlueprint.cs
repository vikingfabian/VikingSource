using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Resource
{
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

            addResources(useRawFood, DssRef.todoLang.Resource_TypeName_RawFood, ref resources);
            addResources(useOre, DssRef.todoLang.Resource_TypeName_Ore, ref resources);
            addResources(useWood, DssRef.todoLang.Resource_TypeName_Wood, ref resources);
            addResources(useStone, DssRef.todoLang.Resource_TypeName_Stone, ref resources);
            addResources(useWater, DssRef.todoLang.Resource_TypeName_Water, ref resources);

            content.text(resources + " => " + resultCount.ToString() + " " + name);

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
