using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;

namespace VikingEngine.DSSWars.Resource
{
    static class CraftResourceLib
    {
        const int FoodWaterUsage = 20;
        const int FoodCraftAmount = 20;
        //public static void Blueprint(ItemResourceType item, out CraftBlueprint bp1, out CraftBlueprint bp2)
        //{
        //    switch (item)
        //    {
        //        case ItemResourceType.Fuel_G: bp1 = Fuel1; bp2 = null; break;
        //        case ItemResourceType.Coal: bp1 = Charcoal; bp2 = null; break;
        //        case ItemResourceType.Food_G: bp1 = Food1; bp2 = Food2; break;
        //        case ItemResourceType.Beer: bp1 = Beer; bp2 = null; break;

        //        case ItemResourceType.Cupper: bp1 = Cupper; bp2 = null; break;

        //        case ItemResourceType.Tin: bp1 = Tin; bp2 = null; break;
        //        case ItemResourceType.Lead: bp1 = Lead; bp2 = null; break;
        //        case ItemResourceType.Iron_G: bp1 = Iron; bp2 = null; break;
        //        case ItemResourceType.Silver: bp1 = Silver; bp2 = null; break;

        //        case ItemResourceType.Bronze: bp1 = Bronze; bp2 = null; break;
        //        case ItemResourceType.CastIron: bp1 = CastIron; bp2 = null; break;
        //        case ItemResourceType.BloomeryIron: bp1 = BloomeryIron; bp2 = null; break;
        //        case ItemResourceType.Mithril: bp1 = Mithril; bp2 = null; break;


        //        case ItemResourceType.PaddedArmor: bp1 = PaddedArmor; bp2 = null; break;
        //        case ItemResourceType.HeavyPaddedArmor: bp1 = HeavyPaddedArmor; bp2 = null; break;
        //        case ItemResourceType.BronzeArmor: bp1 = BronzeArmor; bp2 = null; break;
        //        case ItemResourceType.IronArmor: bp1 = MailArmor; bp2 = null; break;
        //        case ItemResourceType.HeavyIronArmor: bp1 = HeavyMailArmor; bp2 = null; break;
        //        case ItemResourceType.LightPlateArmor: bp1 = PlateArmor; bp2 = null; break;
        //        case ItemResourceType.FullPlateArmor: bp1 = FullPlateArmor; bp2 = null; break;

        //        case ItemResourceType.Toolkit: bp1 = Beer; bp2 = null; break;
        //        case ItemResourceType.Wagon2Wheel: bp1 = WagonLight; bp2 = null; break;
        //        case ItemResourceType.Wagon4Wheel: bp1 = WagonHeavy; bp2 = null; break;
        //        case ItemResourceType.BlackPowder: bp1 = BlackPowder; bp2 = null; break;
        //        case ItemResourceType.GunPowder: bp1 = GunPowder; bp2 = null; break;
        //        case ItemResourceType.LedBullet: bp1 = LedBullets; bp2 = null; break;

        //        case ItemResourceType.SharpStick: bp1 = SharpStick; bp2 = null; break;
        //        case ItemResourceType.BronzeSword: bp1 = BronzeSword; bp2 = null; break;
        //        case ItemResourceType.ShortSword: bp1 = ShortSword; bp2 = null; break;
        //        case ItemResourceType.Sword: bp1 = Sword; bp2 = null; break;
        //        case ItemResourceType.LongSword: bp1 = LongSword; bp2 = null; break;
        //        case ItemResourceType.MithrilSword: bp1 = MithrilSword; bp2 = null; break;

        //        case ItemResourceType.Warhammer: bp1 = WarhammerIron; bp2 = WarhammerBronze; break;
        //        case ItemResourceType.TwoHandSword: bp1 = TwoHandSword; bp2 = null; break;
        //        case ItemResourceType.KnightsLance: bp1 = KnightsLance; bp2 = null; break;

        //        case ItemResourceType.SlingShot: bp1 = Slingshot; bp2 = null; break;
        //        case ItemResourceType.ThrowingSpear: bp1 = ThrowingSpear1; bp2 = ThrowingSpear2; break;
        //        case ItemResourceType.Bow: bp1 = Bow; bp2 = null; break;
        //        case ItemResourceType.LongBow: bp1 = LongBow; bp2 = null; break;
        //        case ItemResourceType.Crossbow: bp1 = CrossBow; bp2 = null; break;
        //        case ItemResourceType.MithrilBow: bp1 = MithrilBow; bp2 = null; break;

        //        case ItemResourceType.HandCannon: bp1 = BronzeHandCannon; bp2 = null; break;
        //        case ItemResourceType.HandCulverin: bp1 = BronzeHandCulverin; bp2 = null; break;
        //        case ItemResourceType.Rifle: bp1 = Rifle; bp2 = null; break;
        //        case ItemResourceType.Blunderbus: bp1 = Blunderbus; bp2 = null; break;

        //        case ItemResourceType.Ballista: bp1 = Ballista; bp2 = null; break;
        //        case ItemResourceType.Manuballista: bp1 = ManuBallista; bp2 = null; break;
        //        case ItemResourceType.Catapult: bp1 = Catapult; bp2 = null; break;
        //        case ItemResourceType.SiegeCannonBronze: bp1 = SiegeCannonBronze; bp2 = null; break;
        //        case ItemResourceType.ManCannonBronze: bp1 = ManCannonBronze; bp2 = null; break;
        //        case ItemResourceType.SiegeCannonIron: bp1 = SiegeCannonIron; bp2 = null; break;
        //        case ItemResourceType.ManCannonIron: bp1 = ManCannonIron; bp2 = null; break;

        //        default: throw new NotImplementedException();
        //    }
        //}

        //ORE
        public static readonly CraftBlueprint Cupper = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Cupper,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.CupperOre, 2)
            },
            XP.WorkExperienceType.Smelting,
            XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Cupper_AndCooling = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Cupper,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.CoolingFluid, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.CupperOre, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Iron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Iron_G,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.IronOre_G, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Iron_AndCooling = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Iron_G,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.CoolingFluid, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.IronOre_G, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Silver = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Silver,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 1),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.SilverOre, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Silver_AndCooling = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Silver,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.CoolingFluid, 1),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.SilverOre, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Tin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Tin,
            4,
            new UseResource[]
            {
                //new UseResource(ItemResourceType.Water_G, 1),
                new UseResource(ItemResourceType.Fuel_G, 20),
                new UseResource(ItemResourceType.TinOre, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Lead = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Lead,
            4,
            new UseResource[]
            {

                new UseResource(ItemResourceType.Fuel_G, 10),
                new UseResource(ItemResourceType.LeadOre, 2)
            },
           XP.WorkExperienceType.Smelting,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );


        //ALLOY
        public static readonly CraftBlueprint BloomeryIron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.BloomeryIron,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.Iron_G, 5)
            },
           XP.WorkExperienceType.CastMetal,
           XP.ExperienceLevel.Expert_3,
            CraftRequirement.Foundry
        );

        public static readonly CraftBlueprint Steel = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Steel,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 2),
                new UseResource(ItemResourceType.BloomeryIron, 4)
            },
           XP.WorkExperienceType.CraftMetal,
           XP.ExperienceLevel.Expert_3,
           CraftRequirement.Smith
        );

        public static readonly CraftBlueprint Steel_AndCooling = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Steel,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.CoolingFluid, 2),
                new UseResource(ItemResourceType.BloomeryIron, 4)
            },
           XP.WorkExperienceType.CraftMetal,
           XP.ExperienceLevel.Expert_3,
           CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CastIron = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.CastIron,
           4,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Fuel_G, 40),
                new UseResource(ItemResourceType.Stone_G, 2),
                new UseResource(ItemResourceType.Iron_G, 4),               
           },
          XP.WorkExperienceType.CastMetal,
          XP.ExperienceLevel.Expert_3,
           CraftRequirement.Foundry
       );

        public static readonly CraftBlueprint Bronze = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Bronze,
            6,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Stone_G, 1),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.Cupper, 4),
                new UseResource(ItemResourceType.Tin, 2),
            },
           XP.WorkExperienceType.CastMetal,
           XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Foundry
        );

        public static readonly CraftBlueprint Mithril = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Mithril,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Fuel_G, 100),
                new UseResource(ItemResourceType.Silver, 2),
                new UseResource(ItemResourceType.RawMithril, 2)
            },
           XP.WorkExperienceType.CastMetal,
           XP.ExperienceLevel.Master_4,
            CraftRequirement.Foundry
        );

        //NON METAL
        public static readonly CraftBlueprint Fuel1 = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Fuel_G,
            10,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 10),
            },
            XP.WorkExperienceType.CraftFuel
        );

        public static readonly CraftBlueprint Charcoal = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Fuel_G,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Fuel_G, 10),
                new UseResource(ItemResourceType.Wood_Group, 10),
            },
            XP.WorkExperienceType.CraftFuel,
            XP.ExperienceLevel.Practitioner_2,
             CraftRequirement.CoalPit
        );

       


        //public static readonly CraftBlueprint Food1 = new CraftBlueprint(
        //    CraftResultType.Resource,
        //    (int)ItemResourceType.Food_G,
        //    FoodCraftAmount,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Water_G, FoodWaterUsage),
        //        new UseResource(ItemResourceType.Fuel_G, FoodWaterUsage),
        //        new UseResource(ItemResourceType.RawFood_Group, FoodCraftAmount)
        //    },
        //    XP.WorkExperienceType.Cook
        //)
        //{ tooltipId = Tooltip.Food_BlueprintId };

        //public static readonly CraftBlueprint Food2 = new CraftBlueprint(
        //    CraftResultType.Resource,
        //    (int)ItemResourceType.Food_G,
        //    FoodCraftAmount,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Beer, FoodWaterUsage),
        //        new UseResource(ItemResourceType.Fuel_G, FoodWaterUsage),
        //        new UseResource(ItemResourceType.RawFood_Group, FoodCraftAmount)
        //    },
        //   XP.WorkExperienceType.Cook
        //)
        //{ tooltipId = Tooltip.Food_BlueprintId };
        public static readonly CraftBlueprint Food1 = new CraftBlueprint(
          CraftResultType.Resource,
          (int)ItemResourceType.Food_G,
          FoodCraftAmount,
          new UseResource[]
          {
                new UseResource(ItemResourceType.Water_G, FoodWaterUsage),
                new UseResource(ItemResourceType.Fuel_G, FoodWaterUsage),
                new UseResource(ItemResourceType.RawFood_Group, FoodCraftAmount)
          },XP.WorkExperienceType.Cook
      )
        { tooltipId = Tooltip.Food_BlueprintId };

        public static readonly CraftBlueprint Food2 = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Food_G,
            FoodCraftAmount,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Beer, FoodWaterUsage),
                new UseResource(ItemResourceType.Fuel_G, FoodWaterUsage),
                new UseResource(ItemResourceType.RawFood_Group, FoodCraftAmount)
            },XP.WorkExperienceType.Cook
        )
        { tooltipId = Tooltip.Food_BlueprintId };



        public static readonly CraftBlueprint Beer = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.Beer,
               10,
               new UseResource[]
               {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.Fuel_G, 1),
                new UseResource(ItemResourceType.RawFood_Group, 1)
               },
            XP.WorkExperienceType.Chemistry,
            XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Brewery

           );

        public static readonly CraftBlueprint CoolingFluid = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.CoolingFluid,
               10,
               new UseResource[]
               {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.RawFood_Group, 3)
               },
            XP.WorkExperienceType.Chemistry

           );

        public static readonly CraftBlueprint BlackPowder = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.BlackPowder,
               4,
               new UseResource[]
               {
                new UseResource(ItemResourceType.Fuel_G, 2),
                new UseResource(ItemResourceType.Sulfur, 2),
               },
               XP.WorkExperienceType.Chemistry,
               XP.ExperienceLevel.Practitioner_2,
                CraftRequirement.Chemist
           );

        public static readonly CraftBlueprint Toolkit = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.Toolkit,
               1,
               new UseResource[]
               {
                    new UseResource(ItemResourceType.Steel, 1),
                    new UseResource(ItemResourceType.Wood_Group, 4),
               },
               XP.WorkExperienceType.CraftMetal,
               XP.ExperienceLevel.Beginner_1,
                CraftRequirement.Smith
           );

        public static readonly CraftBlueprint WagonLight = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.Wagon2Wheel,
               1,
               new UseResource[]
               {
                    new UseResource(ItemResourceType.SkinLinen_Group, 4),
                    new UseResource(ItemResourceType.Wood_Group, 8),
               },
               XP.WorkExperienceType.WoodCutter,

               XP.ExperienceLevel.Practitioner_2,
                CraftRequirement.Carpenter

           );
        public static readonly CraftBlueprint WagonHeavy = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.Wagon2Wheel,
               1,
               new UseResource[]
               {
                    new UseResource(ItemResourceType.SkinLinen_Group, 2),
                    new UseResource(ItemResourceType.Wood_Group, 16),
                    new UseResource(ItemResourceType.Iron_G, 2),
               },
               XP.WorkExperienceType.WoodCutter,

               XP.ExperienceLevel.Expert_3,
                CraftRequirement.Carpenter

           );

        public static readonly CraftBlueprint GunPowder = new CraftBlueprint(
                CraftResultType.Resource,
                (int)ItemResourceType.GunPowder,
               4,
               new UseResource[]
               {
                new UseResource(ItemResourceType.Fuel_G, 2),
                new UseResource(ItemResourceType.Sulfur, 2),
               },
               XP.WorkExperienceType.Chemistry,

               XP.ExperienceLevel.Expert_3,
                CraftRequirement.Chemist
           );
        public static readonly CraftBlueprint LedBullets = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.LedBullet,
           5,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Fuel_G, 10),
                new UseResource(ItemResourceType.Tin, 1),
                new UseResource(ItemResourceType.Lead, 4),
           },
          XP.WorkExperienceType.CastMetal,
          XP.ExperienceLevel.Beginner_1,
           CraftRequirement.Foundry
       );

        //WEAPONS
        public static readonly CraftBlueprint SharpStick = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.SharpStick,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 1),
                new UseResource(ItemResourceType.Stone_G, 1),
            },
            XP.WorkExperienceType.CraftWeapon
        );

        public static readonly CraftBlueprint BronzeSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.BronzeSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Bronze, 3),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );
        
        public static readonly CraftBlueprint ShortSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.ShortSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Iron_G, 3),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint Sword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Sword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Iron_G, 4),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint LongSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.LongSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Steel, 4),
            },
            XP.WorkExperienceType.CraftWeapon,
             XP.ExperienceLevel.Expert_3,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint HandSpearIron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.HandSpear,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 5),
                new UseResource(ItemResourceType.Iron_G, 1),
            },
            XP.WorkExperienceType.CraftWeapon,
             XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint HandSpearBronze = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.HandSpear,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 5),
                new UseResource(ItemResourceType.Bronze, 1),
           },
           XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Beginner_1,
           CraftRequirement.Smith
       );

        public static readonly CraftBlueprint MithrilSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.MithrilSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Bronze, 1),
                new UseResource(ItemResourceType.Mithril, 3),
            },
            XP.WorkExperienceType.CraftWeapon,
             XP.ExperienceLevel.Master_4,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint WarhammerIron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Warhammer,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
                new UseResource(ItemResourceType.Wood_Group, 4),
                new UseResource(ItemResourceType.Iron_G, 2),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );
        public static readonly CraftBlueprint WarhammerBronze = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Warhammer,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
                new UseResource(ItemResourceType.Wood_Group, 4),
                new UseResource(ItemResourceType.Bronze, 2),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint KnightsLance = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.KnightsLance,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Gold, 60),
                new UseResource(ItemResourceType.RawFood_Group, 20),
                new UseResource(ItemResourceType.Wood_Group, 5),
                new UseResource(ItemResourceType.Iron_G, 5),
            },
           XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint TwoHandSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.TwoHandSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Steel, 5),
            },
            XP.WorkExperienceType.CraftWeapon,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint Slingshot = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.SlingShot,
           4,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Stone_G, 4),
                new UseResource(ItemResourceType.SkinLinen_Group, 4),
           },
           XP.WorkExperienceType.Fletcher
       );

        public static readonly CraftBlueprint ThrowingSpear1 = new CraftBlueprint(
          CraftResultType.Resource,
          (int)ItemResourceType.ThrowingSpear,
          4,
          new UseResource[]
          {
                 new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.Iron_G, 1),
          },
          XP.WorkExperienceType.Fletcher
      );
        public static readonly CraftBlueprint ThrowingSpear2 = new CraftBlueprint(
          CraftResultType.Resource,
          (int)ItemResourceType.ThrowingSpear,
          4,
          new UseResource[]
          {
                 new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.Bronze, 1),
          },
          XP.WorkExperienceType.Fletcher
      );

        public static readonly CraftBlueprint Bow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Bow,
            2,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 4),
                new UseResource(ItemResourceType.Iron_G, 1),
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
            },
            XP.WorkExperienceType.Fletcher, 
            XP.ExperienceLevel.Practitioner_2

        );
        public static readonly CraftBlueprint LongBow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.LongBow,
            2,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.Iron_G, 1),
                new UseResource(ItemResourceType.SkinLinen_Group, 3),
            },
           XP.WorkExperienceType.Fletcher,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint CrossBow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Crossbow,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 4),
                new UseResource(ItemResourceType.Steel, 1),
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
            },
           XP.WorkExperienceType.Fletcher,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint MithrilBow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.MithrilBow,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.Bronze, 1),
                new UseResource(ItemResourceType.Mithril, 1),
                new UseResource(ItemResourceType.SkinLinen_Group, 3),
            },
           XP.WorkExperienceType.Fletcher,
           XP.ExperienceLevel.Master_4,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint BronzeHandCannon = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.HandCannon,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.BlackPowder, 1),
                new UseResource(ItemResourceType.LedBullet, 1),
                new UseResource(ItemResourceType.Bronze, 2),
            },
           XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint BronzeHandCulverin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.HandCulverin,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.BlackPowder, 1),
                new UseResource(ItemResourceType.LedBullet, 1),
                new UseResource(ItemResourceType.Bronze, 2),
           },
          XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Practitioner_2,
           CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint Rifle = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Rifle,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.GunPowder, 1),
                new UseResource(ItemResourceType.LedBullet, 1),
                new UseResource(ItemResourceType.CastIron, 2),
            },
           XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint Blunderbus = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Blunderbus,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.GunPowder, 1),
                new UseResource(ItemResourceType.LedBullet, 1),
                new UseResource(ItemResourceType.CastIron, 2),
            },
           XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint Ballista_Iron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Ballista,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.SkinLinen_Group, 4),
                new UseResource(ItemResourceType.Iron_G, 1),
            },
           XP.WorkExperienceType.Fletcher,
            XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint Ballista_Bronze = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Ballista,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.SkinLinen_Group, 4),
                new UseResource(ItemResourceType.Bronze, 1),
           },
          XP.WorkExperienceType.Fletcher,
           XP.ExperienceLevel.Practitioner_2,
           CraftRequirement.Carpenter
       );

        public static readonly CraftBlueprint ManuBallista = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Manuballista,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.Iron_G, 1),
            },
           XP.WorkExperienceType.Fletcher,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint Catapult = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Catapult,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 10),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Iron_G, 2),
            },
           XP.WorkExperienceType.Fletcher,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint SiegeCannonBronze = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.SiegeCannonBronze,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.BlackPowder, 40),
                new UseResource(ItemResourceType.Stone_G, 40),
                new UseResource(ItemResourceType.Bronze, 50),
            },
           XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Master_4,
            CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint ManCannonBronze = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.ManCannonBronze,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Bronze, 20),
            },
           XP.WorkExperienceType.CastMetal,
            XP.ExperienceLevel.Expert_3,
            CraftRequirement.Gunmaker
        );

        public static readonly CraftBlueprint SiegeCannonIron = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.SiegeCannonIron,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
                new UseResource(ItemResourceType.GunPowder, 10),
                new UseResource(ItemResourceType.CastIron, 20),
           },
          XP.WorkExperienceType.CraftMetal,
           XP.ExperienceLevel.Master_4,
           CraftRequirement.Gunmaker
       );

        public static readonly CraftBlueprint ManCannonIron = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.ManCannonIron,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
                new UseResource(ItemResourceType.GunPowder, 10),
                new UseResource(ItemResourceType.CastIron, 20),
           },
          XP.WorkExperienceType.CraftMetal,
           XP.ExperienceLevel.Master_4,
           CraftRequirement.Gunmaker
       );

        public static readonly CraftBlueprint PaddedArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.PaddedArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint HeavyPaddedArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.HeavyPaddedArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 8),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Practitioner_2
        );

        public static readonly CraftBlueprint BronzeArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.BronzeArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
        new UseResource(ItemResourceType.Bronze, 2),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Practitioner_2
        );

        public static readonly CraftBlueprint MailArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.IronArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
        new UseResource(ItemResourceType.Iron_G, 2),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint HeavyMailArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.HeavyIronArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 2),
        new UseResource(ItemResourceType.Iron_G, 6),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint PlateArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.IronArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
        new UseResource(ItemResourceType.Steel, 2),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Practitioner_2,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint FullPlateArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.FullPlateArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 2),
        new UseResource(ItemResourceType.Steel, 6),
            },
           XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3,
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint MithrilArmor = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.MithrilArmor,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.PaddedArmor, 1),
                new UseResource(ItemResourceType.Mithril, 3),
           },
          XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Master_4,
           CraftRequirement.Smith
       );

        
    }
}
