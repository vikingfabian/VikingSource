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
        

        public GroupedResource[] cityResouces;

        public void Init_CityComponents()
        {
            cityResouces = new GroupedResource[CityResoureIndex.COUNT * cities.Count];

            int startIndex = 0;
            for (int cityIx = 0; cityIx < cities.Count; cityIx++)
            {
                cities[cityIx].resourceComponentStartIndex = startIndex;

                // Basics
                cityResouces[startIndex + CityResoureIndex.wood] = new GroupedResource { amount = 20, goalBuffer = 300 };
                cityResouces[startIndex + CityResoureIndex.fuel] = new GroupedResource { amount = 100, goalBuffer = 400 };
                cityResouces[startIndex + CityResoureIndex.water] = new GroupedResource { goalBuffer = 0 }; // (no default given)
                cityResouces[startIndex + CityResoureIndex.stone] = new GroupedResource { amount = 20, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.rawFood] = new GroupedResource { amount = 50, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex.food] = new GroupedResource { amount = 200, goalBuffer = 500 };
                cityResouces[startIndex + CityResoureIndex.beer] = new GroupedResource { amount = 0, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex.coolingfluid] = new GroupedResource { amount = 0, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex.skinLinnen] = new GroupedResource { goalBuffer = 100 };

                // Ores
                cityResouces[startIndex + CityResoureIndex.ironore] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.TinOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.CupperOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.LeadOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.SilverOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.GoldOre] = new GroupedResource { goalBuffer = 100 };

                // Refined / materials
                cityResouces[startIndex + CityResoureIndex.iron] = new GroupedResource { amount = 10, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Tin] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Cupper] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Lead] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Silver] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.RawMithril] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Sulfur] = new GroupedResource { goalBuffer = 100 };

                // Alloys / specials
                cityResouces[startIndex + CityResoureIndex.Bronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Steel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.CastIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.BloomeryIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Mithril] = new GroupedResource { goalBuffer = 100 };

                // Tools / components / melee
                cityResouces[startIndex + CityResoureIndex.Palisade] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Toolkit] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Wagon2Wheel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Wagon4Wheel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.BlackPowder] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.GunPowder] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.LedBullet] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.sharpstick] = new GroupedResource { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.BronzeSword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.shortsword] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Sword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.LongSword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.HandSpear] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilSword] = new GroupedResource { goalBuffer = 100 };

                // More weapons
                cityResouces[startIndex + CityResoureIndex.Warhammer] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.twohandsword] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.knightslance] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.SlingShot] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.ThrowingSpear] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.bow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.longbow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.crossbow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilBow] = new GroupedResource { goalBuffer = 100 };

                // Early firearms
                cityResouces[startIndex + CityResoureIndex.HandCannon] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.HandCulvertin] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Rifle] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Blunderbuss] = new GroupedResource { goalBuffer = 100 };

                // Siege
                cityResouces[startIndex + CityResoureIndex.BatteringRam] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.ballista] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Manuballista] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.Catapult] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.SiegeCannonBronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.ManCannonBronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.SiegeCannonIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.ManCannonIron] = new GroupedResource { goalBuffer = 100 };

                // Armor
                cityResouces[startIndex + CityResoureIndex.paddedArmor] = new GroupedResource { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.HeavyPaddedArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.BronzeArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.mailArmor] = new GroupedResource { amount = 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.heavyMailArmor] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.LightPlateArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.FullPlateArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex.MithrilArmor] = new GroupedResource { goalBuffer = 100 };

                startIndex += CityResoureIndex.COUNT;
            }
        }

    }
}
