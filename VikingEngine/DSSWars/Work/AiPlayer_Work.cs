using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer
    {
        const int ResourceLowBuffer = 60;

        protected void refreshWorkPriority_async(bool inWar)
        {
            ref var autoBuild = ref pfaction.GetFaction().workTemplate.GetRefWorkPriority(WorkPriorityType.autoBuild);
            autoBuild.value = (byte)(4 - aggressionLevel);
            if (inWar && autoBuild.value > 1)
            {
                autoBuild.value -= 1;
            }
            pfaction.GetFaction().refreshCityWork();

            int count = Bound.Min(pfaction.GetFaction().cities.Count / 4, 1);
            for (int i = 0; i < count; i++)
            {
                City city = pfaction.GetFaction().cities.GetRandom(Ref.rnd, DssRef.world.cities);

                if (city != null)
                {
                    //bool prepareSettle = false;
                    //EcsStaticArrayCounter neighbors = city.CityNeighbors();
                    //while (neighbors.Next(DssRef.world.cities, out City nCity))
                    //{
                    //    if (nCity.cityType == CityType.UnClaimed)
                    //    {
                    //        prepareSettle = true;
                    //        break;
                    //    }
                    //}

                    //city.autoAdjustResourcesToCitySize(prepareSettle);

                    //DOES NOT WORK - will reset in auto_updateWorkPrio()

                    ref var woodPrio = ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.wood);
                    adjustWorkToBuffer(city.resourceComponentStartIndex + CityResourceIndex.wood, ref woodPrio);

                    ref var movePrio = ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.move);
                    movePrio.set(lib.LargestValue(woodPrio.value, 3));

                    adjustWorkToBuffer(city.resourceComponentStartIndex + CityResourceIndex.stone, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.stone));
                   
                    adjustWorkToBuffer_2(city.resourceComponentStartIndex + CityResourceIndex.food, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.farmFood), ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.craftFood));

                    if (city.cityType == CityType.Campsite)
                    {
                        city.workTemplate.GetRefWorkPriority(WorkPriorityType.collectClay).set(1);
                        city.workTemplate.GetRefWorkPriority(WorkPriorityType.miningSalt).set(1);
                        city.workTemplate.GetRefWorkPriority(WorkPriorityType.craftConservedFood).set(1);

                        if (city.GetGroupedResource(CityResourceIndex.stone).useStockLimit == false)
                        {
                            DssRef.world.setCityStockPile(city, 100);
                            city.GetRefGroupedResource(CityResourceIndex.wood).setLimit(200);
                            city.GetRefGroupedResource(CityResourceIndex.food).setLimit(int.MaxValue);
                        }
                    }
                    else
                    {
                        if (city.GetGroupedResource(CityResourceIndex.stone).useStockLimit)
                        {
                            DssRef.world.setCityStockPile(city, int.MaxValue);
                        }

                        adjustWorkToBuffer(city.resourceComponentStartIndex + CityResourceIndex.Clay, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.collectClay), false);
                        city.workTemplate.GetRefWorkPriority(WorkPriorityType.miningSalt).set(3);
                        adjustWorkToBuffer_2(city.resourceComponentStartIndex + CityResourceIndex.ConservedFood, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.craftConservedFood), ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.miningSalt), false);
                    }

                    adjustWorkToBuffer_2(city.resourceComponentStartIndex + CityResourceIndex.fuel, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.craftFuel), ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.miningCoal));

                    adjustWorkToBuffer_2(city.resourceComponentStartIndex + CityResourceIndex.ironore, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.bogiron), ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.miningIron));
                    adjustWorkToBuffer(city.resourceComponentStartIndex + CityResourceIndex.iron, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.smeltIron));

                    adjustWorkToBuffer(city.resourceComponentStartIndex + CityResourceIndex.rawFood, ref city.workTemplate.GetRefWorkPriority(WorkPriorityType.farmRawFood));


                    if (city.resourceAmount(CityResourceIndex.food) <= 0)
                    {
                        city.workTemplate.setWorkPrio(WorkPriorityType.craftFood, 5);
                        city.workTemplate.setWorkPrio(WorkPriorityType.farmFood, 5);
                        city.workTemplate.setWorkPrio(WorkPriorityType.farmRawFood, 4);
                    }
                    //if (city.resourceAmount(CityResoureIndex.wood) <= 0)
                    //{
                    //    BlackMarketResources.AiPurchaseWood(city, faction);
                    //}


                    bool hasBetterCraft = false;
                    foreach (var weaponType in ConscriptWeaponPrioOrder)
                    {
                        var work = city.workTemplate.GetWorkPriority(weaponType.item, out _);
                        if (adjustWorkToMilitaryCrafting(city, ItemPropertyColl.Get(weaponType.item).bp1, ref work, hasBetterCraft, out bool available))
                        {
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
                        var work = city.workTemplate.GetWorkPriority(armorType, out _);
                        if (adjustWorkToMilitaryCrafting(city, ItemPropertyColl.Get(armorType).bp1, ref work, hasBetterCraft, out hasBetterCraft))
                        {
                            city.workTemplate.SetWorkPriority(armorType, work);
                        }
                    }


                }
            }

            void adjustWorkToBuffer(int resourceCompex, ref WorkPriority workPriority, bool highPrio = true)
            {
                GroupedResource resource = DssRef.world.cityResouces[resourceCompex];

                if (resource.amount < ResourceLowBuffer)
                {
                    if (Ref.peRnd.Chance(highPrio? 0.5 : 0.3))
                    {
                        workPriority.addPrio_belowMax(1);
                    }
                }
                else if (resource.amount >= resource.MaxLimit() / 2)
                {
                    if (Ref.peRnd.Chance(0.3))
                    {
                        workPriority.addPrio(-1);
                    }
                }
            }

            void adjustWorkToBuffer_2(int resourceCompex, ref WorkPriority workPriority1, ref WorkPriority workPriority2, bool highPrio = true)
            {
                GroupedResource resource = DssRef.world.cityResouces[resourceCompex];

                if (resource.amount < ResourceLowBuffer)
                {
                    double chance = highPrio ? 0.5 : 0.3;
                    if (Ref.peRnd.Chance(chance))
                    {
                        workPriority1.addPrio_belowMax(1);
                    }
                    if (Ref.peRnd.Chance(chance))
                    {
                        workPriority2.addPrio_belowMax(1);
                    }
                }
                else if (resource.amount >= resource.MaxLimit() / 2)
                {
                    if (Ref.peRnd.Chance(0.3))
                    {
                        workPriority1.addPrio(-1);
                    }
                    if (Ref.peRnd.Chance(0.3))
                    {
                        workPriority2.addPrio(-1);
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

        //public void AutoExpandType(City city, out bool work, out Build.BuildAndExpandType building, out bool intelligent)
        //{
        //    building = BuildAndExpandType.NUM_NONE;
        //    intelligent = false;
        //    work = false;

        //    if (city.needMore(CityResoureIndex.rawFood) && Ref.peRnd.Chance(0.6))
        //    {
        //        building = BuildAndExpandType.WheatFarm;
        //    }
        //    else if (city.resourceAmount(CityResoureIndex.fuel) < ResourceLowBuffer && city.resourceAmount(CityResoureIndex.wood)/*res_wood.amount*/ > ResourceLowBuffer && Ref.rnd.Chance(0.6))
        //    {
        //        building = BuildAndExpandType.CoalPit;
        //    }
        //    else if (city.needMore(CityResoureIndex.skinLinnen) && Ref.peRnd.Chance(0.6))
        //    {
        //        building = BuildAndExpandType.LinenFarm;
        //    }
        //    else if (city.conscriptBuildings.Count < 2 && Ref.peRnd.Chance(0.6))
        //    {
        //        building = BuildAndExpandType.SoldierBarracks;
        //    }
        //    else
        //    {
        //        var res_ironore = city.GetGroupedResource(CityResoureIndex.ironore);
        //        var res_iron = city.GetGroupedResource(CityResoureIndex.iron);

        //        if (((city.buildingStructure.Smith_count == 0 && city.resourceAmount(CityResoureIndex.ironore)/*res_ironore.amount*/ > ResourceLowBuffer) ||
        //            (res_ironore.amount >= res_ironore.MaxLimit())
        //                && Ref.peRnd.Chance(0.02))
        //            )
        //        {
        //            if (res_iron.amount < CraftBuildingLib.CraftSmith_IronUse)
        //            {
        //                if (!BlackMarketResources.AiPurchaseIron(city, faction))
        //                {

        //                    intelligent = true;
        //                    work = true;

        //                    return;
        //                }
        //            }
        //            building = BuildAndExpandType.Smith;
        //        }
        //        else if (city.deliveryServices.Count < 2 && Ref.peRnd.Chance(0.2))
        //        {
        //            building = BuildAndExpandType.Postal;
        //        }
        //        else
        //        {
        //            intelligent = true;
        //            work = true;
        //        }
        //    }
        //}
    }
}
