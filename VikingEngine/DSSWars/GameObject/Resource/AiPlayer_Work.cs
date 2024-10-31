using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;

namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer
    {
        const int ResourceLowBuffer = 60;

        protected void refreshWorkPriority_async(bool inWar)
        {

            faction.workTemplate.autoBuild.value = 4 - aggressionLevel;
            if (inWar && faction.workTemplate.autoBuild.value > 1)
            {
                faction.workTemplate.autoBuild.value -= 1;
            }
            faction.refreshCityWork();

            int count = Bound.Min(faction.cities.Count / 4, 1);
            for (int i = 0; i < count; i++)
            {
                City city = faction.cities.GetRandomSafe(Ref.rnd);

                if (city != null)
                {
                    //adjustWorkToBuffer(ref city.res_wood, ref city.workTemplate.wood);

                    adjustWorkToBuffer(ref city.res_stone, ref city.workTemplate.stone);

                    adjustWorkToBuffer(ref city.res_food, ref city.workTemplate.craft_food);

                    adjustWorkToBuffer(ref city.res_fuel, ref city.workTemplate.craft_fuel);

                    adjustWorkToBuffer(ref city.res_iron, ref city.workTemplate.craft_iron);

                    adjustWorkToBuffer(ref city.res_rawFood, ref city.workTemplate.farm_food);

                    //adjustWorkToBuffer(ref city.res_wood, ref city.workTemplate.wood);

                    if (city.res_food.amount <= 0)
                    {
                        city.workTemplate.craft_food.value = 5;
                        city.workTemplate.farm_food.value = 4;
                    }
                    if (city.res_wood.amount <= 0)
                    {
                        BlackMarketResources.AiPurchaseWood(city, faction);
                    }

                    bool craftWeapon = adjustWorkToCrafting(city, ResourceLib.CraftSword, ref city.workTemplate.craft_sword, false);
                    craftWeapon = adjustWorkToCrafting(city, ResourceLib.CraftBow, ref city.workTemplate.craft_bow, craftWeapon);
                    adjustWorkToCrafting(city, ResourceLib.CraftSharpStick, ref city.workTemplate.craft_sharpstick, craftWeapon);
                    
                    bool craftArmour= adjustWorkToCrafting(city, ResourceLib.CraftHeavyArmor, ref city.workTemplate.craft_heavyarmor, false);
                    craftArmour = adjustWorkToCrafting(city, ResourceLib.CraftMediumArmor, ref city.workTemplate.craft_mediumarmor, craftArmour);
                    adjustWorkToCrafting(city, ResourceLib.CraftLightArmor, ref city.workTemplate.craft_lightarmor, craftArmour);
                    
                }
            }

            void adjustWorkToBuffer(ref GroupedResource resource, ref WorkPriority workPriority)
            {
                if (resource.amount < ResourceLowBuffer)
                {
                    if (Ref.rnd.Chance(0.5))
                    {
                        workPriority.addPrio_belowMax(1);
                    }
                }
                else if (resource.amount >= resource.goalBuffer / 2)
                {
                    if (Ref.rnd.Chance(0.3))
                    {
                        workPriority.addPrio(-1);
                    }
                }
            }

            bool adjustWorkToCrafting(City city, CraftBlueprint blueprint, ref WorkPriority workPriority, bool lowPrio)
            {
                int count = blueprint.canCraftCount(city);
                if (!lowPrio && count >= ResourceLowBuffer)
                {
                    if (Ref.rnd.Chance(0.8))
                    {
                        workPriority.addPrio_belowMax(1);
                        return true;
                    }
                }
                else
                {
                    if (Ref.rnd.Chance(0.4))
                    {
                        workPriority.addPrio(-1);
                    }
                }

                return false;
            }
        }

        public override void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType building, out bool intelligent)
        {
            building = BuildAndExpandType.NUM_NONE;
            intelligent = false;
            work = false;

            if (city.res_rawFood.needMore() && Ref.rnd.Chance(0.6))
            {
                building = BuildAndExpandType.WheatFarm;
            }
            else if (city.res_fuel.amount < ResourceLowBuffer && city.res_wood.amount > ResourceLowBuffer && Ref.rnd.Chance(0.6))
            {
                building = BuildAndExpandType.CoalPit;
            }
            else if (city.res_skinLinnen.needMore() && Ref.rnd.Chance(0.6))
            {
                building = BuildAndExpandType.LinenFarm;
            }
            else if (city.conscriptBuildings.Count < 2 && Ref.rnd.Chance(0.6))
            {
                building = BuildAndExpandType.Barracks;
            }
            else if (((!city.hasBuilding_smith && city.res_ironore.amount > ResourceLowBuffer) ||
                (city.res_ironore.amount >= city.res_ironore.goalBuffer)
                    && Ref.rnd.Chance(0.02))
                )
            {
                if (city.res_iron.amount < ResourceLib.CraftSmith_IronUse)
                {
                    if (!BlackMarketResources.AiPurchaseIron(city, faction))
                    {

                        intelligent = true;
                        work = true;

                        return;
                    }
                }
                building = BuildAndExpandType.Smith;
            }
            else if (city.deliveryServices.Count < 2 && Ref.rnd.Chance(0.2))
            {
                building = BuildAndExpandType.Postal;
            }
            else
            {
                intelligent = true;
                work = true;
            }
        }
    }
}
