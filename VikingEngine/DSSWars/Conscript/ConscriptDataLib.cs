using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Conscript
{
    static class ConscriptDataLib
    {
        public static readonly ItemResourceType[] SoldierWeapons = {
            ItemResourceType.SharpStick,
            ItemResourceType.BronzeSword,
            ItemResourceType.ShortSword,
            ItemResourceType.Sword,
            ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
            ItemResourceType.Warhammer,
            ItemResourceType.TwoHandSword,
        };

        public static readonly ItemResourceType[] ArcherWeapons = {
            ItemResourceType.SlingShot,
            ItemResourceType.ThrowingSpear,
            ItemResourceType.Bow,
            ItemResourceType.LongBow,
            ItemResourceType.Crossbow,
            ItemResourceType.MithrilBow,
        };

        public static readonly ItemResourceType[] ArcherGuardWeapons = {
            ItemResourceType.Stone_G,
            ItemResourceType.ThrowingSpear,
            ItemResourceType.Bow,
            ItemResourceType.LongBow,
            ItemResourceType.Crossbow,
        };

        public static readonly ItemResourceType[] WarmachineWeapons = {

            ItemResourceType.Ballista,
            ItemResourceType.Manuballista,
            ItemResourceType.Catapult,
        };



        //static readonly ItemResourceType[] NobelWeapons = {
        //    ItemResourceType.Warhammer,
        //    ItemResourceType.TwoHandSword,
        //    ItemResourceType.KnightsLance,
        //    ItemResourceType.MithrilSword,
        //    ItemResourceType.MithrilBow,
        //};

        public static readonly ItemResourceType[] GunWeapons = {
            ItemResourceType.HandCannon,
            ItemResourceType.HandCulverin,
            ItemResourceType.Rifle,
            ItemResourceType.Blunderbuss,
        };

        public static readonly ItemResourceType[] CannonWeapons = {
           ItemResourceType.SiegeCannonBronze,
            ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron,
            ItemResourceType.ManCannonIron,
        };

        public static readonly ItemResourceType[] SideShield = {
            ItemResourceType.NONE,
            ItemResourceType.BucklerShield
        };
        public static readonly ItemResourceType[] AllShields = {
            ItemResourceType.NONE,
            ItemResourceType.BucklerShield,
            ItemResourceType.RoundShield,
            ItemResourceType.HeaterShield,
            ItemResourceType.TowerShield,
        };

        public static readonly ItemResourceType[] MenTypes = {
            ItemResourceType.Men,
            ItemResourceType.NobelMen,
        };

        public static readonly List<ItemResourceType> ArmorOptions = new List<ItemResourceType>
        {
            ItemResourceType.NONE,
            ItemResourceType.PaddedArmor,
            ItemResourceType.HeavyPaddedArmor,
            ItemResourceType.BronzeArmor,
            ItemResourceType.IronArmor,
            ItemResourceType.HeavyIronArmor,
            ItemResourceType.LightPlateArmor,
            ItemResourceType.FullPlateArmor,
            ItemResourceType.MithrilArmor,
        };

        public static readonly ItemResourceType[] AnimalTypes = {
            ItemResourceType.NONE,
            ItemResourceType.Pig,
            ItemResourceType.Oxen,
            ItemResourceType.KineOxen,

            ItemResourceType.Dog,
            ItemResourceType.Hound,

            ItemResourceType.Pony,
            ItemResourceType.Horse,
            ItemResourceType.WarHorse,
            ItemResourceType.DraftHorse,

            ItemResourceType.WildPig,
            ItemResourceType.WildHog,
            ItemResourceType.WarHog,
            ItemResourceType.StagHog,

            ItemResourceType.Wolf,
            ItemResourceType.Warg,
            ItemResourceType.AlphaWarg,

            ItemResourceType.WildCat,
            ItemResourceType.Lion,
            ItemResourceType.WarLion,

            ItemResourceType.Elephant,
            ItemResourceType.WarElephant,
            ItemResourceType.Oliphant,
        };

        public static readonly ItemResourceType[] MountArmorTypesLight = {
            ItemResourceType.NONE,
            ItemResourceType.MountPaddedArmor,
            ItemResourceType.MountHeavyPaddedArmor,
        };

        public static readonly ItemResourceType[] MountArmorTypes = {
            ItemResourceType.NONE,
            ItemResourceType.MountBronzeArmor,
            ItemResourceType.MountPaddedArmor,
            ItemResourceType.MountHeavyPaddedArmor,
            ItemResourceType.MountIronArmor,
            ItemResourceType.MountHeavyIronArmor,
            ItemResourceType.MountLightPlateArmor,
            ItemResourceType.MountFullPlateArmor,
            ItemResourceType.MountMithrilArmor,
        };


        public static readonly ItemResourceType[] VehicleTypesLight = {
            ItemResourceType.NONE,
            ItemResourceType.Wagon2Wheel,
        };

        public static readonly ItemResourceType[] VehicleTypes = {
            ItemResourceType.NONE,
            ItemResourceType.Wagon2Wheel,
            ItemResourceType.Wagon4Wheel,
            ItemResourceType.WagonClosed,
            ItemResourceType.WagonIron,
            ItemResourceType.WagonSteel,
        };

        public static List<ItemResourceType[]> AllConstriptWeapons()
        {
            return new List<ItemResourceType[]>
            {
                SoldierWeapons,
                ArcherWeapons,
                WarmachineWeapons,
                //NobelWeapons,
                GunWeapons,
                CannonWeapons,
            };
        }
        public static List<ItemResourceType[]> AllHandWeapons()
        {
            return new List<ItemResourceType[]>
            {
                SoldierWeapons,
                ArcherWeapons,
                //NobelWeapons,
                GunWeapons,
            };
        }


        public static readonly BuildAndExpandType[] BarrackTypes = new BuildAndExpandType[]
            {
                BuildAndExpandType.SoldierBarracks,
                BuildAndExpandType.ArcherBarracks,
                BuildAndExpandType.WarmachineBarracks,
                BuildAndExpandType.GunBarracks,
                BuildAndExpandType.CannonBarracks,
                //BuildAndExpandType.KnightsBarracks,
            };
        public static Dictionary<BuildAndExpandType, int> TypeToBarrackTypeIx;

        public static void Init()
        {
            TypeToBarrackTypeIx = new Dictionary<BuildAndExpandType, int>(BarrackTypes.Length);
            for (int i = 0; i < BarrackTypes.Length; i++)
            {
                TypeToBarrackTypeIx.Add(BarrackTypes[i], i);
            }
        }

        public const int CraftSettlerFood = 300;
        public const int CraftSettlerWood = 150;
        public const int CraftSettlerSkinLinen = 200;

        public static readonly CraftBlueprint CraftSettler = new CraftBlueprint(
            CraftResultType.NoSet,
            0,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Men, 60),
                new UseResource(ItemResourceType.Food_G, CraftSettlerFood),
                new UseResource(ItemResourceType.Wood_Group, CraftSettlerWood),
                new UseResource(ItemResourceType.SkinLinen_Group, CraftSettlerSkinLinen)
            },
            XP.WorkExperienceType.NUM_NONE,
            XP.ExperienceLevel.Beginner_1,
             BuildAndExpandType.NUM_NONE
        );

        public static readonly CraftBlueprint CraftNomadSettler = new CraftBlueprint(
            CraftResultType.NoSet,
            0,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Men, 60),
                new UseResource(ItemResourceType.Food_G, CraftSettlerFood /2),
                new UseResource(ItemResourceType.Wood_Group, CraftSettlerWood /2),
                new UseResource(ItemResourceType.SkinLinen_Group, CraftSettlerSkinLinen /2)
            },
            XP.WorkExperienceType.NUM_NONE,
            XP.ExperienceLevel.Beginner_1,
             BuildAndExpandType.NUM_NONE
        );
    }

}
