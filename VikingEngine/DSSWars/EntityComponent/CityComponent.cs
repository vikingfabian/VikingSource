using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars
{  
    partial class WorldData
    {

        public EcsStaticArray neighborCities;
        public GroupedResource[] cityResouces;

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

            int startIndex = 0;
            for (int cityIx = 0; cityIx < cityCount; cityIx++)
            {
                //cities[cityIx].resourceComponentStartIndex = startIndex;
                //int multiplyDefault = cities[cityIx].cityType == CityType.UnClaimed ? 0 : 1;
                // Basics
                cityResouces[startIndex + CityResoureIndex.wood] = new GroupedResource { amount = 20, stockPileLimit = DefaultBuffer_Wood };
                cityResouces[startIndex + CityResoureIndex.fuel] = new GroupedResource { amount = 100, stockPileLimit = 400 };
                cityResouces[startIndex + CityResoureIndex.water] = new GroupedResource { stockPileLimit = 0 }; // (no default given)
                cityResouces[startIndex + CityResoureIndex.stone] = new GroupedResource { amount = 20, stockPileLimit = 300 };
                cityResouces[startIndex + CityResoureIndex.rawFood] = new GroupedResource { amount = 0, stockPileLimit = 200 };
                cityResouces[startIndex + CityResoureIndex.food] = new GroupedResource { amount = 200, stockPileLimit = 500 };
                cityResouces[startIndex + CityResoureIndex.beer] = new GroupedResource { amount = 0, stockPileLimit = 200 };
                cityResouces[startIndex + CityResoureIndex.coolingfluid] = new GroupedResource { amount = 0, stockPileLimit = 200 };
                cityResouces[startIndex + CityResoureIndex.skinLinnen] = new GroupedResource { amount = 20, stockPileLimit = DefaultBuffer_SkinLinnen };

                // Ores
                cityResouces[startIndex + CityResoureIndex.ironore] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.TinOre] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.CopperOre] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.LeadOre] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.SilverOre] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.GoldOre] = new GroupedResource { stockPileLimit = 100 };

                // Refined / materials
                cityResouces[startIndex + CityResoureIndex.iron] = new GroupedResource { amount = 20, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Tin] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Copper] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Lead] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Silver] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.RawMithril] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Sulfur] = new GroupedResource { stockPileLimit = 100 };

                // Alloys / specials
                cityResouces[startIndex + CityResoureIndex.Bronze] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Steel] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.CastIron] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.BloomeryIron] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Mithril] = new GroupedResource { stockPileLimit = 100 };

                // Tools / components / melee
                cityResouces[startIndex + CityResoureIndex.Palisade] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Toolkit] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Wagon2Wheel] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Wagon4Wheel] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.BlackPowder] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.GunPowder] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.LedBullet] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.sharpstick] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.BronzeSword] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.shortsword] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Sword] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.LongSword] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.HandSpear] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilSword] = new GroupedResource { stockPileLimit = 100 };

                // More weapons
                cityResouces[startIndex + CityResoureIndex.Warhammer] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.twohandsword] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.knightslance] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.SlingShot] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.ThrowingSpear] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.bow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.longbow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.crossbow] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilBow] = new GroupedResource { stockPileLimit = 100 };

                // Early firearms
                cityResouces[startIndex + CityResoureIndex.HandCannon] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.HandCulvertin] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Rifle] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Blunderbuss] = new GroupedResource { stockPileLimit = 100 };

                // Siege
                cityResouces[startIndex + CityResoureIndex.BatteringRam] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.ballista] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Manuballista] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.Catapult] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.SiegeCannonBronze] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.ManCannonBronze] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.SiegeCannonIron] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.ManCannonIron] = new GroupedResource { stockPileLimit = 100 };

                // Armor
                cityResouces[startIndex + CityResoureIndex.paddedArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.HeavyPaddedArmor] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.BronzeArmor] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.mailArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.heavyMailArmor] = new GroupedResource { amount = 0, stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.LightPlateArmor] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.FullPlateArmor] = new GroupedResource { stockPileLimit = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilArmor] = new GroupedResource { stockPileLimit = 100 };

                startIndex += CityResoureIndex.COUNT;
            }
        }

    }
}
