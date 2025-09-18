using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;

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

                    city.res_food.goalBuffer = city.workForce.amount / 100 * 100 + 200;
                    city.res_rawFood.goalBuffer = city.workForce.amount / 300 * 100 + 100;

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


                    bool hasBetterCraft = false;
                    foreach (var weaponType in ConscriptWeaponPrioOrder)
                    {
                        var work = city.workTemplate.GetWorkPriority(weaponType.item);
                        if (adjustWorkToMilitaryCrafting(city, ItemPropertyColl.Get(weaponType.item).bp1, ref work, hasBetterCraft, out bool available))
                        {
                            //if (hasBetterCraft && weaponType.item != ItemResourceType.SharpStick)
                            //{
                            //    lib.DoNothing();
                            //}
                            city.workTemplate.SetWorkPriority(weaponType.item, work);
                        }

                        if (available && city.buildingStructure.getBarracksCount(weaponType.barracks) > 0)
                        { 
                            hasBetterCraft = true;
                        }
                    }

                    hasBetterCraft = false;
                    foreach (var armorType in conscriptArmorPrioOrder)
                    {
                        var work = city.workTemplate.GetWorkPriority(armorType);
                        if (adjustWorkToMilitaryCrafting(city, ItemPropertyColl.Get(armorType).bp1, ref work, hasBetterCraft, out hasBetterCraft))
                        {
                            city.workTemplate.SetWorkPriority(armorType, work);
                        }
                    }

                    //bool craftWeapon = adjustWorkToCrafting(city, CraftResourceLib.Sword, ref city.workTemplate.craft_sword, false);
                    //craftWeapon = adjustWorkToCrafting(city, CraftResourceLib.Bow, ref city.workTemplate.craft_bow, craftWeapon);
                    //adjustWorkToCrafting(city, CraftResourceLib.SharpStick, ref city.workTemplate.craft_sharpstick, craftWeapon);

                    //bool craftArmour= adjustWorkToCrafting(city, CraftResourceLib.HeavyMailArmor, ref city.workTemplate.craft_heavymailarmor, false);
                    //craftArmour = adjustWorkToCrafting(city, CraftResourceLib.MailArmor, ref city.workTemplate.craft_mailarmor, craftArmour);
                    //adjustWorkToCrafting(city, CraftResourceLib.PaddedArmor, ref city.workTemplate.craft_paddedarmor, craftArmour);

                }
            }

            void adjustWorkToBuffer(ref GroupedResource resource, ref WorkPriority workPriority)
            {
                if (resource.amount < ResourceLowBuffer)
                {
                    if (Ref.peRnd.Chance(0.5))
                    {
                        workPriority.addPrio_belowMax(1);
                    }
                }
                else if (resource.amount >= resource.goalBuffer / 2)
                {
                    if (Ref.peRnd.Chance(0.3))
                    {
                        workPriority.addPrio(-1);
                    }
                }
            }

            bool adjustWorkToMilitaryCrafting(City city, CraftBlueprint blueprint, ref WorkPriority workPriority, bool lowPrio, out bool available)
            {
                int count = blueprint.canCraftCount(city);
                if (!lowPrio && count >= ResourceLowBuffer)
                {
                    available = true;
                    if (Ref.peRnd.Chance(0.8))
                    {
                        workPriority.addPrio_belowMax(changeWeight());
                        return true;
                    }
                }
                else
                {
                    available = false;
                    if (Ref.peRnd.Chance(0.4))
                    {
                        workPriority.addPrio(-changeWeight());
                        return true;
                    }
                }

                return false;

                int changeWeight()
                {
                    return aggressionLevel >= AggressionLevel2_RandomAttacks? 2 : 1;
                }
            }
        }

        public override void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType building, out bool intelligent)
        {
            building = BuildAndExpandType.NUM_NONE;
            intelligent = false;
            work = false;

            if (city.res_rawFood.needMore() && Ref.peRnd.Chance(0.6))
            {
                building = BuildAndExpandType.WheatFarm;
            }
            else if (city.res_fuel.amount < ResourceLowBuffer && city.res_wood.amount > ResourceLowBuffer && Ref.rnd.Chance(0.6))
            {
                building = BuildAndExpandType.CoalPit;
            }
            else if (city.res_skinLinnen.needMore() && Ref.peRnd.Chance(0.6))
            {
                building = BuildAndExpandType.LinenFarm;
            }
            else if (city.conscriptBuildings.Count < 2 && Ref.peRnd.Chance(0.6))
            {
                building = BuildAndExpandType.SoldierBarracks;
            }
            else if (((city.buildingStructure.Smith_count == 0 && city.res_ironore.amount > ResourceLowBuffer) ||
                (city.res_ironore.amount >= city.res_ironore.goalBuffer)
                    && Ref.peRnd.Chance(0.02))
                )
            {
                if (city.res_iron.amount < CraftBuildingLib.CraftSmith_IronUse)
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
            else if (city.deliveryServices.Count < 2 && Ref.peRnd.Chance(0.2))
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
