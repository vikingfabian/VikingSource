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
        const int FoodWaterUsage = 8;
        const int FoodFuelUsage = 3;
        const int FoodCraftAmount = 20;
       

        //ORE
        public static readonly CraftBlueprint Copper = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Copper,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.CopperOre, 2)
            },
            XP.WorkExperienceType.Smelting,
            XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint Cupper_AndCooling = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Copper,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.CoolingFluid, 2),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.CopperOre, 2)
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
           XP.ExperienceLevel.Practitioner_2,
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
                new UseResource(ItemResourceType.Copper, 4),
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
                new UseResource(ItemResourceType.Fuel_G, FoodFuelUsage),
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
                new UseResource(ItemResourceType.Fuel_G, FoodFuelUsage),
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
               XP.WorkExperienceType.WoodWork,

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
               XP.WorkExperienceType.WoodWork,

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
            XP.ExperienceLevel.Expert_3,
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
