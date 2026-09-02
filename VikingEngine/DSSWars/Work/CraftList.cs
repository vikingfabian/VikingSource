using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.GO.Characters.Monsters;

namespace VikingEngine.DSSWars.Work
{
    static class CraftList
    {
        public static readonly ItemResourceType[] SmelterCraftTypes = {
            ItemResourceType.Copper, ItemResourceType.Tin, ItemResourceType.Lead, ItemResourceType.Iron_G,
            ItemResourceType.BloomeryIron, ItemResourceType.Silver, ItemResourceType.Gold, ItemResourceType.Mithril 
        };

        public static readonly ItemResourceType[] SmithCraftTypes = {
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword, ItemResourceType.Sword, ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
            ItemResourceType.Warhammer, ItemResourceType.TwoHandSword, /*ItemResourceType.KnightsLance,*/
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
            ItemResourceType.PaddedArmor, ItemResourceType.HeavyPaddedArmor, ItemResourceType.BronzeArmor, ItemResourceType.IronArmor, ItemResourceType.HeavyIronArmor, ItemResourceType.LightPlateArmor, ItemResourceType.FullPlateArmor, ItemResourceType.MithrilArmor,
        ItemResourceType.MountPaddedArmor, ItemResourceType.MountHeavyPaddedArmor, ItemResourceType.MountBronzeArmor, ItemResourceType.MountIronArmor, ItemResourceType.MountHeavyIronArmor, ItemResourceType.MountLightPlateArmor, ItemResourceType.MountFullPlateArmor, ItemResourceType.MountMithrilArmor,
        };

        public static readonly ItemResourceType[] ShieldCraftTypes = { 
             ItemResourceType.BucklerShield, ItemResourceType.RoundShield, ItemResourceType.HeaterShield, ItemResourceType.TowerShield
        };

        public static readonly ItemResourceType[] FoundryCraftTypes = {
            ItemResourceType.Bronze, ItemResourceType.CastIron, ItemResourceType.LedBullet, ItemResourceType.BloomeryIron, ItemResourceType.Mithril 
        };

        public static readonly ItemResourceType[] BenchCraftTypes = {
            ItemResourceType.Fuel_G, ItemResourceType.PaddedArmor, ItemResourceType.SharpStick, ItemResourceType.HandSpear, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear 
        };

        public static readonly ItemResourceType[] CarpenterCraftTypes = {
            ItemResourceType.Palisade,
            ItemResourceType.WoodContainer,
            ItemResourceType.SharpStick, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear, ItemResourceType.Bow, ItemResourceType.LongBow, ItemResourceType.Crossbow,
            ItemResourceType.MithrilBow,
            ItemResourceType.Ballista, ItemResourceType.Manuballista, ItemResourceType.Catapult,
            ItemResourceType.Wagon2Wheel, ItemResourceType.Wagon4Wheel, ItemResourceType.WagonClosed, ItemResourceType.WagonIron, ItemResourceType.WagonSteel 
        };

        public static readonly ItemResourceType[] ChemistCraftTypes = {
             ItemResourceType.CoolingFluid, ItemResourceType.BlackPowder, ItemResourceType.GunPowder 
        };

        public static readonly ItemResourceType[] PotteryCraftTypes = {
             ItemResourceType.PotContainer, ItemResourceType.Brick 
        };

        public static readonly ItemResourceType[] ShieldMakerCraftTypes = {
             ItemResourceType.BucklerShield, ItemResourceType.RoundShield, ItemResourceType.HeaterShield, ItemResourceType.TowerShield
        };

        //public static readonly ItemResourceType[] ButcherAnimalTypes = {
        //    ItemResourceType.Hen,
        //    ItemResourceType.Pig,
        //    ItemResourceType.Oxen,
        //    ItemResourceType.KineOxen,

        //    ItemResourceType.Pony,
        //    ItemResourceType.Horse,
        //    ItemResourceType.WarHorse,
        //    ItemResourceType.DraftHorse,

        //    ItemResourceType.WildPig,
        //    ItemResourceType.WildHog,
        //    ItemResourceType.WarHog,
        //    ItemResourceType.StagHog,

        //    ItemResourceType.Wolf,
        //    ItemResourceType.Warg,
        //    ItemResourceType.AlphaWarg,

        //    ItemResourceType.WildCat,
        //    ItemResourceType.Lion,
        //    ItemResourceType.WarLion,

        //    ItemResourceType.Elephant,
        //    ItemResourceType.WarElephant,
        //    ItemResourceType.Oliphant,
        //};

        public static readonly CraftBlueprint[] ButcherAnimalCraftTypes = {
            CraftResourceLib.SlaughterFowl,
            CraftResourceLib.SlaughterBoar,
            CraftResourceLib.SlaughterHen,
            CraftResourceLib.SlaughterPig,
            CraftResourceLib.SlaughterOxen,
            CraftResourceLib.SlaughterKineOxen,

            CraftResourceLib.SlaughterPony,
            CraftResourceLib.SlaughterHorse,
            CraftResourceLib.SlaughterWarHorse,
            CraftResourceLib.SlaughterDraftHorse,

            CraftResourceLib.SlaughterWildPig,
            CraftResourceLib.SlaughterWildHog,
            CraftResourceLib.SlaughterWarHog,
            CraftResourceLib.SlaughterStagHog,

            CraftResourceLib.SlaughterWolf,
            CraftResourceLib.SlaughterWarg,
            CraftResourceLib.SlaughterAlphaWarg,

            CraftResourceLib.SlaughterWildCat,
            CraftResourceLib.SlaughterLion,
            CraftResourceLib.SlaughterWarLion,

            CraftResourceLib.SlaughterElephant,
            CraftResourceLib.SlaughterWarElephant,
            CraftResourceLib.SlaughterOliphant,
        };

        public static List<KeyValuePair<BuildAndExpandType, ItemResourceType[]>> AllBuidings()
        {
            return new List<KeyValuePair<BuildAndExpandType, ItemResourceType[]>>
            {
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Cook, [ItemResourceType.Food_G]),

                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Smelter, SmelterCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Smith, SmithCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.ShieldMaker, ShieldMakerCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Gunmaker, GunmakerCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Armory, ArmoryCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Foundry, FoundryCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.WorkBench, BenchCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Pottery, PotteryCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Carpenter, CarpenterCraftTypes ),
                new KeyValuePair<BuildAndExpandType, ItemResourceType[]>( BuildAndExpandType.Chemist, ChemistCraftTypes ),
            };
        }
    }
}
