using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class CraftBlueprint
    {
        string name;

        //public int useWater;
        //public int useStone;
        //public int useWood;
        //public int useRawFood;
        //public int useOre;
        UseResource[] resources;

        int resultCount;

        public CraftBlueprint(string name, int result, UseResource[] resources)
        {
            this.name = name;
            this.resultCount = result;
            this.resources = resources;
        }

        public void createBackOrder(City city)
        {
            //OLD
            //city.wood.backOrder += useWood;
            //city.stone.backOrder += useStone;
            //city.rawFood.backOrder += useRawFood;
            //city.ore.backOrder += useOre;

            //NEW
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                res.backOrder += r.amount;
                city.SetGroupedResource(r.type, res);
            }
            
        }

        //public bool available(City city)
        //{
        //    bool result = city.water >= useWater &&
        //        city.wood.freeAmount() >= useWood &&
        //        city.stone.freeAmount() >= useStone &&
        //        city.rawFood.freeAmount() >= useRawFood &&
        //        city.ore.freeAmount() >= useOre;

        //    return result;
        //}

        //public bool canCraft(City city)
        //{
        //    bool result = city.water >= useWater &&
        //        city.wood.amount >= useWood &&
        //        city.stone.amount >= useStone &&
        //        city.rawFood.amount >= useRawFood &&
        //        city.ore.amount >= useOre;

        //    return result;
        //}

        //public int craft(City city)//ref int water,ref int wood, ref int stone, ref int rawfood, ref int ore)
        //{
        //    city.water -= useWater;
        //    city.wood.amount -= useWood;
        //    city.stone.amount -= useStone;
        //    city.rawFood.amount -= useRawFood;
        //    city.ore.amount -= useOre;

        //    if (city.stone.amount < 0 || city.wood.amount < 0)
        //    {
        //        lib.DoNothing();
        //    }

        //    return resultCount;
        //}
        public bool available(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.freeAmount() < r.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public bool canCraft(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                if (res.amount < r.amount)
                {
                    return false;
                }
            }
            return true;
        }

        public int craft(City city)
        {
            foreach (var r in resources)
            {
                var res = city.GetGroupedResource(r.type);
                res.amount -= r.amount;
                city.SetGroupedResource(r.type, res);
            }

            return resultCount;
        }

        public void toMenu(RichBoxContent content)
        {
            string resourcesString = string.Empty;

            foreach(var r in resources)
            {
                addResources(r.amount, LangLib.Item(r.type), ref resourcesString);
            }
            //addResources(useRawFood, DssRef.todoLang.Resource_TypeName_RawFood, ref resources);
            //addResources(useOre, DssRef.todoLang.Resource_TypeName_Ore, ref resources);
            //addResources(useWood, DssRef.todoLang.Resource_TypeName_Wood, ref resources);
            //addResources(useStone, DssRef.todoLang.Resource_TypeName_Stone, ref resources);
            //addResources(useWater, DssRef.todoLang.Resource_TypeName_Water, ref resources);

            content.text(resourcesString + " => " + resultCount.ToString() + " " + name);

            void addResources(int count, string name, ref string resourcesString)
            {
                if (count > 0)
                {
                    if (resourcesString.Length > 0)
                    {
                        resourcesString += " + ";
                    }
                    resourcesString += count.ToString() + " " + name;
                }
            }
        }
    }

    struct UseResource
    {
        public ItemResourceType type;
        public int amount;

        public UseResource(ItemResourceType type, int amount)
        { 
            this.type = type;
            this.amount = amount;
        }

       
    }

    
}
