using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.LootFest.Display;

namespace VikingEngine.DSSWars
{  
    partial class WorldData
    {

        public EcsStaticArray neighborCities;
        public GroupedResource[] cityResouces;
        public WorkPriority[] cityWork;
        public StorageSize[] cityStorage;

        public void InitCity(City city)
        { 
            city.resourceComponentStartIndex = CityResoureIndex.COUNT * city.myIndex;
           

        }

        public void clearCityResources(City city)
        {
            int ex_end = city.resourceComponentStartIndex + CityResoureIndex.COUNT;
            for (int i = city.resourceComponentStartIndex; i < ex_end; i++)
            {
                cityResouces[i].amount = 0;
            }
        }

        public const int DefaultBuffer_Wood = 300;
        public const int DefaultBuffer_SkinLinnen = 300;

        public void Init_CityComponents(int cityCount)
        {
            cityResouces = new GroupedResource[CityResoureIndex.COUNT * cityCount];
            neighborCities = new EcsStaticArray(14, cityCount);
            cityWork = new WorkPriority[WorkTemplate.COUNT * cityCount];
            cityStorage = new StorageSize[StorageSize.COUNT * cityCount];

            int resourceStart = 0;
            //int workStart = 0;

            for (int cityIx = 0; cityIx < cityCount; cityIx++)
            {
                //cities[cityIx].resourceComponentStartIndex = startIndex;
                //int multiplyDefault = cities[cityIx].cityType == CityType.UnClaimed ? 0 : 1;
                // Basics
                //cityResouces[startIndex + CityResoureIndex.wood] = new GroupedResource { amount = 20, goalBuffer = DefaultBuffer_Wood };
                //cityResouces[startIndex + CityResoureIndex.fuel] = new GroupedResource { amount = 100, goalBuffer = 400 };
                //cityResouces[startIndex + CityResoureIndex.water] = new GroupedResource { goalBuffer = 0 }; // (no default given)
                //cityResouces[startIndex + CityResoureIndex.stone] = new GroupedResource { amount = 20, goalBuffer = 300 };
                //cityResouces[startIndex + CityResoureIndex.rawFood] = new GroupedResource { amount = 0, goalBuffer = 200 };
                //cityResouces[startIndex + CityResoureIndex.food] = new GroupedResource { amount = 200, goalBuffer = 500 };
                //cityResouces[startIndex + CityResoureIndex.beer] = new GroupedResource { amount = 0, goalBuffer = 200 };
                //cityResouces[startIndex + CityResoureIndex.coolingfluid] = new GroupedResource { amount = 0, goalBuffer = 200 };
                //cityResouces[startIndex + CityResoureIndex.skinLinnen] = new GroupedResource { amount = 20, goalBuffer = DefaultBuffer_SkinLinnen };
                

                //// Ores
                //cityResouces[resourceStart + CityResoureIndex.ironore] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.TinOre] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.CopperOre] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.LeadOre] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.SilverOre] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.GoldOre] = new GroupedResource { stockPileLimit = 100 };

                // Refined / materials
                //cityResouces[startIndex + CityResoureIndex.iron] = new GroupedResource { amount = 20, goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.Tin] = new GroupedResource { goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.Copper] = new GroupedResource { goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.Lead] = new GroupedResource { goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.Silver] = new GroupedResource { goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.RawMithril] = new GroupedResource { goalBuffer = 100 };
                //cityResouces[startIndex + CityResoureIndex.Sulfur] = new GroupedResource { goalBuffer = 100 };
                

                //// Alloys / specials
                //cityResouces[resourceStart + CityResoureIndex.Bronze] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Steel] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.CastIron] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.BloomeryIron] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Mithril] = new GroupedResource { stockPileLimit = 100 };

                //// Tools / components / melee
                //cityResouces[resourceStart + CityResoureIndex.Palisade] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Toolkit] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Wagon2Wheel] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Wagon4Wheel] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.BlackPowder] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.GunPowder] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.LedBullet] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.sharpstick] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.BronzeSword] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.shortsword] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Sword] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.LongSword] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.HandSpear] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.MithrilSword] = new GroupedResource { stockPileLimit = 100 };

                //// More weapons
                //cityResouces[resourceStart + CityResoureIndex.Warhammer] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.twohandsword] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                ////cityResouces[resourceStart + CityResoureIndex.knightslance] = new GroupedResource { amount = 0, goalBuffer = 100 };
                //cityResouces[resourceStart + CityResoureIndex.SlingShot] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.ThrowingSpear] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.bow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.longbow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.crossbow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.MithrilBow] = new GroupedResource { stockPileLimit = 100 };

                //// Early firearms
                //cityResouces[resourceStart + CityResoureIndex.HandCannon] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.HandCulvertin] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Rifle] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Blunderbuss] = new GroupedResource { stockPileLimit = 100 };

                //// Siege
                //cityResouces[resourceStart + CityResoureIndex.BatteringRam] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.ballista] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Manuballista] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.Catapult] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.SiegeCannonBronze] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.ManCannonBronze] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.SiegeCannonIron] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.ManCannonIron] = new GroupedResource { stockPileLimit = 100 };

                //// Armor
                //cityResouces[resourceStart + CityResoureIndex.paddedArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.HeavyPaddedArmor] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.BronzeArmor] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.mailArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.heavyMailArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.LightPlateArmor] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.FullPlateArmor] = new GroupedResource { stockPileLimit = 100 };
                //cityResouces[resourceStart + CityResoureIndex.MithrilArmor] = new GroupedResource { stockPileLimit = 100 };

                for (int resourceIx = 0; resourceIx < CityResoureIndex.COUNT; ++resourceIx)
                {
                    cityResouces[resourceStart + resourceIx] = new GroupedResource();
                }

                resourceStart += CityResoureIndex.COUNT;
                //workStart += WorkTemplate.COUNT;

                
            }

            for (int i = 0; i < cityStorage.Length; i++)
            {
                cityStorage[i] = new StorageSize();
            }

        }

    }
}
