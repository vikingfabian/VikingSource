using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Build
{
    static class BuildingCraftList
    {
        public static readonly ItemResourceType[] SmelterCraftTypes = {
            ItemResourceType.Copper, ItemResourceType.Tin, ItemResourceType.Lead, ItemResourceType.Iron_G,
            ItemResourceType.BloomeryIron, ItemResourceType.Silver, ItemResourceType.Gold, ItemResourceType.Mithril };

        public static readonly ItemResourceType[] SmithCraftTypes = {
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword, ItemResourceType.Sword, ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
            ItemResourceType.Warhammer, ItemResourceType.TwoHandSword, ItemResourceType.KnightsLance,
            ItemResourceType.MithrilSword,
            ItemResourceType.Steel,
            ItemResourceType.Toolkit,
        };

        public static readonly ItemResourceType[] GunmakerCraftTypes = {
            ItemResourceType.HandCannon, ItemResourceType.HandCulverin,
            ItemResourceType.Rifle, ItemResourceType.Blunderbuss,
            ItemResourceType.SiegeCannonBronze, ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron, ItemResourceType.ManCannonIron,
        };

        public static readonly ItemResourceType[] ArmoryCraftTypes = {
            ItemResourceType.PaddedArmor, ItemResourceType.HeavyPaddedArmor, ItemResourceType.BronzeArmor, ItemResourceType.IronArmor, ItemResourceType.HeavyIronArmor, ItemResourceType.LightPlateArmor, ItemResourceType.FullPlateArmor, ItemResourceType.MithrilArmor
        };

        public static readonly ItemResourceType[] FoundryCraftTypes = {
            ItemResourceType.Bronze, ItemResourceType.CastIron, ItemResourceType.LedBullet, ItemResourceType.BloomeryIron, ItemResourceType.Mithril };

        public static readonly ItemResourceType[] BenchCraftTypes = {
            ItemResourceType.Fuel_G, ItemResourceType.PaddedArmor, ItemResourceType.SharpStick, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear };

        public static readonly ItemResourceType[] CarpenterCraftTypes = {
            ItemResourceType.Palisade,
            ItemResourceType.SharpStick, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear, ItemResourceType.Bow, ItemResourceType.LongBow, ItemResourceType.Crossbow,
            ItemResourceType.MithrilBow,
            ItemResourceType.Ballista, ItemResourceType.Manuballista, ItemResourceType.Catapult,
            ItemResourceType.Wagon2Wheel, ItemResourceType.Wagon4Wheel };

        public static readonly ItemResourceType[] ChemistCraftTypes = {
             ItemResourceType.CoolingFluid, ItemResourceType.BlackPowder, ItemResourceType.GunPowder };

        public static List<KeyValuePair<BuildAndExpandType, ItemResourceType[]>> AllBuidings()
        {
            return new List<KeyValuePair<BuildAndExpandType, ItemResourceType[]>>
            {
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Cook, [ItemResourceType.Food_G]),

                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Smelter, SmelterCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Smith, SmithCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Gunmaker, GunmakerCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Armory, ArmoryCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Foundry, FoundryCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.WorkBench, BenchCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Carpenter, CarpenterCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Chemist, ChemistCraftTypes ),
            };
        }
    }
}
