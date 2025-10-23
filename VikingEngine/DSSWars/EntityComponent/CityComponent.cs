using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars
{  
    partial class WorldData
    {
        // Indices for per-city resources
        public const int CityResoureIndex_wood = 0;
        public const int CityResoureIndex_fuel = 1;
        public const int CityResoureIndex_water = 2;
        public const int CityResoureIndex_stone = 3;
        public const int CityResoureIndex_rawFood = 4;
        public const int CityResoureIndex_food = 5;
        public const int CityResoureIndex_beer = 6;
        public const int CityResoureIndex_coolingfluid = 7;
        public const int CityResoureIndex_skinLinnen = 8;

        // Ores
        public const int CityResoureIndex_ironore = 9;
        public const int CityResoureIndex_TinOre = 10;
        public const int CityResoureIndex_CupperOre = 11;
        public const int CityResoureIndex_LeadOre = 12;
        public const int CityResoureIndex_SilverOre = 13;
        public const int CityResoureIndex_GoldOre = 14;

        // Refined / materials
        public const int CityResoureIndex_iron = 15;
        public const int CityResoureIndex_Tin = 16;
        public const int CityResoureIndex_Cupper = 17;
        public const int CityResoureIndex_Lead = 18;
        public const int CityResoureIndex_Silver = 19;
        public const int CityResoureIndex_RawMithril = 20;
        public const int CityResoureIndex_Sulfur = 21;

        // Alloys / specials
        public const int CityResoureIndex_Bronze = 22;
        public const int CityResoureIndex_Steel = 23;
        public const int CityResoureIndex_CastIron = 24;
        public const int CityResoureIndex_BloomeryIron = 25;
        public const int CityResoureIndex_Mithril = 26;

        // Tools / components / melee
        public const int CityResoureIndex_Palisade = 27;
        public const int CityResoureIndex_Toolkit = 28;
        public const int CityResoureIndex_Wagon2Wheel = 29;
        public const int CityResoureIndex_Wagon4Wheel = 30;
        public const int CityResoureIndex_BlackPowder = 31;
        public const int CityResoureIndex_GunPowder = 32;
        public const int CityResoureIndex_LedBullet = 33;
        public const int CityResoureIndex_sharpstick = 34;
        public const int CityResoureIndex_BronzeSword = 35;
        public const int CityResoureIndex_shortsword = 36;
        public const int CityResoureIndex_Sword = 37;
        public const int CityResoureIndex_LongSword = 38;
        public const int CityResoureIndex_HandSpear = 39;
        public const int CityResoureIndex_MithrilSword = 40;

        // More weapons (melee/ranged)
        public const int CityResoureIndex_Warhammer = 41;
        public const int CityResoureIndex_twohandsword = 42;
        public const int CityResoureIndex_knightslance = 43;
        public const int CityResoureIndex_SlingShot = 44;
        public const int CityResoureIndex_ThrowingSpear = 45;
        public const int CityResoureIndex_bow = 46;
        public const int CityResoureIndex_longbow = 47;
        public const int CityResoureIndex_crossbow = 48;
        public const int CityResoureIndex_MithrilBow = 49;

        // Early firearms
        public const int CityResoureIndex_HandCannon = 50;
        public const int CityResoureIndex_HandCulvertin = 51;
        public const int CityResoureIndex_Rifle = 52;
        public const int CityResoureIndex_Blunderbuss = 53;

        // Siege
        public const int CityResoureIndex_BatteringRam = 54;
        public const int CityResoureIndex_ballista = 55;
        public const int CityResoureIndex_Manuballista = 56;
        public const int CityResoureIndex_Catapult = 57;
        public const int CityResoureIndex_SiegeCannonBronze = 58;
        public const int CityResoureIndex_ManCannonBronze = 59;
        public const int CityResoureIndex_SiegeCannonIron = 60;
        public const int CityResoureIndex_ManCannonIron = 61;

        // Armor
        public const int CityResoureIndex_paddedArmor = 62;
        public const int CityResoureIndex_HeavyPaddedArmor = 63;
        public const int CityResoureIndex_BronzeArmor = 64;
        public const int CityResoureIndex_mailArmor = 65;
        public const int CityResoureIndex_heavyMailArmor = 66;
        public const int CityResoureIndex_LightPlateArmor = 67;
        public const int CityResoureIndex_FullPlateArmor = 68;
        public const int CityResoureIndex_MithrilArmor = 69;

        // Reserve some indices
        public const int CityResoure_Count = 80;

        public GroupedResource[] cityResouces;

        public void Init_CityComponents()
        {
            cityResouces = new GroupedResource[CityResoure_Count * cities.Count];

            int startIndex = 0;
            for (int cityIx = 0; cityIx < cities.Count; cityIx++)
            {
                cities[cityIx].resourceComponentIndex = startIndex;

                // Basics
                cityResouces[startIndex + CityResoureIndex_wood] = new GroupedResource { amount = 20, goalBuffer = 300 };
                cityResouces[startIndex + CityResoureIndex_fuel] = new GroupedResource { amount = 100, goalBuffer = 400 };
                cityResouces[startIndex + CityResoureIndex_water] = new GroupedResource { goalBuffer = 0 }; // (no default given)
                cityResouces[startIndex + CityResoureIndex_stone] = new GroupedResource { amount = 20, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_rawFood] = new GroupedResource { amount = 50, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex_food] = new GroupedResource { amount = 200, goalBuffer = 500 };
                cityResouces[startIndex + CityResoureIndex_beer] = new GroupedResource { amount = 0, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex_coolingfluid] = new GroupedResource { amount = 0, goalBuffer = 200 };
                cityResouces[startIndex + CityResoureIndex_skinLinnen] = new GroupedResource { goalBuffer = 100 };

                // Ores
                cityResouces[startIndex + CityResoureIndex_ironore] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_TinOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_CupperOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_LeadOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_SilverOre] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_GoldOre] = new GroupedResource { goalBuffer = 100 };

                // Refined / materials
                cityResouces[startIndex + CityResoureIndex_iron] = new GroupedResource { amount = 10, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Tin] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Cupper] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Lead] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Silver] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_RawMithril] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Sulfur] = new GroupedResource { goalBuffer = 100 };

                // Alloys / specials
                cityResouces[startIndex + CityResoureIndex_Bronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Steel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_CastIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_BloomeryIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Mithril] = new GroupedResource { goalBuffer = 100 };

                // Tools / components / melee
                cityResouces[startIndex + CityResoureIndex_Palisade] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Toolkit] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Wagon2Wheel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Wagon4Wheel] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_BlackPowder] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_GunPowder] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_LedBullet] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_sharpstick] = new GroupedResource { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_BronzeSword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_shortsword] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Sword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_LongSword] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_HandSpear] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_MithrilSword] = new GroupedResource { goalBuffer = 100 };

                // More weapons
                cityResouces[startIndex + CityResoureIndex_Warhammer] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_twohandsword] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_knightslance] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_SlingShot] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_ThrowingSpear] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_bow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_longbow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_crossbow] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_MithrilBow] = new GroupedResource { goalBuffer = 100 };

                // Early firearms
                cityResouces[startIndex + CityResoureIndex_HandCannon] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_HandCulvertin] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Rifle] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Blunderbuss] = new GroupedResource { goalBuffer = 100 };

                // Siege
                cityResouces[startIndex + CityResoureIndex_BatteringRam] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_ballista] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Manuballista] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_Catapult] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_SiegeCannonBronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_ManCannonBronze] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_SiegeCannonIron] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_ManCannonIron] = new GroupedResource { goalBuffer = 100 };

                // Armor
                cityResouces[startIndex + CityResoureIndex_paddedArmor] = new GroupedResource { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_HeavyPaddedArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_BronzeArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_mailArmor] = new GroupedResource { amount = 2, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_heavyMailArmor] = new GroupedResource { amount = 0, goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_LightPlateArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_FullPlateArmor] = new GroupedResource { goalBuffer = 100 };
                cityResouces[startIndex + CityResoureIndex_MithrilArmor] = new GroupedResource { goalBuffer = 100 };

                startIndex += CityResoure_Count;
            }
        }





        //public GroupedResource res_water = new GroupedResource();
        //public GroupedResource res_wood = new GroupedResource() { amount = 20, goalBuffer = 300 };
        //public GroupedResource res_fuel = new GroupedResource() { amount = 100, goalBuffer = 400 };
        //public GroupedResource res_stone = new GroupedResource() { amount = 20, goalBuffer = 100 };
        //public GroupedResource res_rawFood = new GroupedResource() { amount = 50, goalBuffer = 200 };
        //public GroupedResource res_food = new GroupedResource() { amount = 200, goalBuffer = 500 };
        //public GroupedResource res_beer = new GroupedResource() { amount = 0, goalBuffer = 200 };
        //public GroupedResource res_coolingfluid = new GroupedResource() { amount = 0, goalBuffer = 200 };
        //public GroupedResource res_skinLinnen = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_ironore = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_TinOre = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_CupperOre = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_LeadOre = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_SilverOre = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_GoldOre = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_iron = new GroupedResource() { amount = 10, goalBuffer = 100 };
        //public GroupedResource res_Tin = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Cupper = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Lead = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Silver = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_RawMithril = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Sulfur = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_Bronze = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Steel = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_CastIron = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_BloomeryIron = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Mithril = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_Palisade = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Toolkit = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Wagon2Wheel = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Wagon4Wheel = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_BlackPowder = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_GunPowder = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_LedBullet = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_sharpstick = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        //public GroupedResource res_BronzeSword = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_shortsword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_Sword = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_LongSword = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_HandSpear = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_MithrilSword = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_Warhammer = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_twohandsword = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_knightslance = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_SlingShot = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_ThrowingSpear = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_bow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_longbow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_crossbow = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_MithrilBow = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_HandCannon = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_HandCulvertin = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Rifle = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Blunderbuss = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_BatteringRam = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_ballista = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_Manuballista = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_Catapult = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_SiegeCannonBronze = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_ManCannonBronze = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_SiegeCannonIron = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_ManCannonIron = new GroupedResource() { goalBuffer = 100 };

        //public GroupedResource res_paddedArmor = new GroupedResource() { amount = DssConst.SoldierGroup_DefaultCount * 2, goalBuffer = 100 };
        //public GroupedResource res_HeavyPaddedArmor = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_BronzeArmor = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_mailArmor = new GroupedResource() { amount = 2, goalBuffer = 100 };
        //public GroupedResource res_heavyMailArmor = new GroupedResource() { amount = 0, goalBuffer = 100 };
        //public GroupedResource res_LightPlateArmor = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_FullPlateArmor = new GroupedResource() { goalBuffer = 100 };
        //public GroupedResource res_MithrilArmor = new GroupedResource() { goalBuffer = 100 };

    }
}
