using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Build
{
    static class CraftBuildingLib
    {
        public const int CraftSmith_IronUse = 10;

        public static readonly ItemResourceType[] SmelterCraftTypes = {
            ItemResourceType.Copper, ItemResourceType.Tin, ItemResourceType.Lead, ItemResourceType.Iron_G,
            ItemResourceType.BloomeryIron, ItemResourceType.Silver, ItemResourceType.Mithril };

        public static readonly ItemResourceType[] SmithCraftTypes = {
            ItemResourceType.ShortSword, ItemResourceType.Sword, ItemResourceType.LongSword,
            ItemResourceType.HandSpear,
            ItemResourceType.Warhammer, ItemResourceType.TwoHandSword, ItemResourceType.KnightsLance,
            ItemResourceType.IronArmor,
        };

        public static readonly ItemResourceType[] GunmakerCraftTypes = {
            ItemResourceType.HandCannon, ItemResourceType.HandCulverin,
            ItemResourceType.Rifle, ItemResourceType.Blunderbus,
            ItemResourceType.SiegeCannonBronze, ItemResourceType.ManCannonBronze,
            ItemResourceType.SiegeCannonIron, ItemResourceType.ManCannonIron,
        };

        public static readonly ItemResourceType[] ArmoryCraftTypes = {
            ItemResourceType.PaddedArmor, ItemResourceType.HeavyPaddedArmor, ItemResourceType.BronzeArmor, ItemResourceType.IronArmor, ItemResourceType.HeavyIronArmor, ItemResourceType.LightPlateArmor, ItemResourceType.FullPlateArmor, ItemResourceType.MithrilArmor
        };

        public static readonly ItemResourceType[] FoundryCraftTypes = {
            ItemResourceType.Bronze, ItemResourceType.CastIron, ItemResourceType.BloomeryIron, ItemResourceType.Mithril };

        public static readonly ItemResourceType[] BenchCraftTypes = {
            ItemResourceType.Fuel_G, ItemResourceType.PaddedArmor, ItemResourceType.SharpStick, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear };

        public static readonly ItemResourceType[] CarpenterCraftTypes = {
            ItemResourceType.SharpStick, ItemResourceType.Bow, ItemResourceType.LongBow, ItemResourceType.Crossbow,
            ItemResourceType.MithrilBow,
            ItemResourceType.Ballista, ItemResourceType.Manuballista, ItemResourceType.Catapult };

        public static readonly ItemResourceType[] ChemistCraftTypes = {
            ItemResourceType.BlackPowder, ItemResourceType.GunPowder };



        public static readonly CraftBlueprint CraftLogistics = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Logistics,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Stone_G, 30)
            },
            XP.WorkExperienceType.HouseBuilding,
             XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Logistics1
        );
        public static readonly CraftBlueprint CraftLogisticsLevel2 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Logistics,
            2,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 10),
                new UseResource(ItemResourceType.Stone_G, 10)
            }, XP.WorkExperienceType.HouseBuilding,
             XP.ExperienceLevel.Beginner_1,
            CraftRequirement.Logistics2
        );

        public static readonly CraftBlueprint WorkerHut = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkerHuts,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 200),
        new UseResource(ItemResourceType.Stone_G, 40)
            },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint Tavern = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Tavern,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 20)
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );

        public static readonly CraftBlueprint Storehouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Storehouse,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Stone_G, 40)
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
       );

        public static readonly CraftBlueprint Brewery = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Brewery,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Iron_G, 5)
           },
            XP.WorkExperienceType.Cook, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint Postal = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Postal,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint Postal_Level2 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PostalLevel2,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wagon2Wheel, 1),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
       )
        { upgradeFrom = Postal };

        public static readonly CraftBlueprint Postal_Level3 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PostalLevel3,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wagon4Wheel, 1),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
       )
        { upgradeFrom = Postal };

        public static readonly CraftBlueprint Recruitment = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Recruitment,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 50),
        new UseResource(ItemResourceType.SkinLinen_Group, 10)
            },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint Recruitment_Level2 = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.RecruitmentLevel2,
           1,
          new UseResource[]
           {
                new UseResource(ItemResourceType.Wagon2Wheel, 1),
           },
           XP.WorkExperienceType.HouseBuilding
        )
        { upgradeFrom = Recruitment };


        public static readonly CraftBlueprint GoldDelivery = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.GoldDeliveryLvl1,
           1,
           new UseResource[]
           {
            new UseResource(ItemResourceType.Iron_G, 10),
            new UseResource(ItemResourceType.Wood_Group, 20),
            new UseResource(ItemResourceType.Stone_G, 40),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
       );

        public static readonly CraftBlueprint GoldDelivery_Level2 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.GoldDeliveryLvl2,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Iron_G, 5),
                new UseResource(ItemResourceType.Wagon2Wheel, 1),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
       )
        {  upgradeFrom = GoldDelivery };

        public static readonly CraftBlueprint GoldDelivery_Level3 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.GoldDeliveryLvl3,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Steel, 5),
                new UseResource(ItemResourceType.Wagon4Wheel, 1),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
       )
        { upgradeFrom = GoldDelivery };


        public static readonly CraftBlueprint Recruitment_Level3 = new CraftBlueprint(
          CraftResultType.Building,
          (int)Build.BuildAndExpandType.RecruitmentLevel3,
          1,
         new UseResource[]
          {
                new UseResource(ItemResourceType.Wagon4Wheel, 1),
          },
          XP.WorkExperienceType.HouseBuilding
       )
        { upgradeFrom = Recruitment };

        public static readonly CraftBlueprint SoldierBarracks = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.SoldierBarracks,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 80),
        new UseResource(ItemResourceType.Stone_G, 20)
            },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint ArcherBarracks = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.ArcherBarracks,
                    1,
                    new UseResource[]
                    {
        new UseResource(ItemResourceType.Wood_Group, 80),
        new UseResource(ItemResourceType.Stone_G, 20)
                    },
                    XP.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint WarmashineBarracks = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WarmashineBarracks,
                    1,
                    new UseResource[]
                    {
        new UseResource(ItemResourceType.Wood_Group, 80),
        new UseResource(ItemResourceType.Stone_G, 20)
                    },
                    XP.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint GunBarracks = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.GunBarracks,
                    1,
                    new UseResource[]
                    {
        new UseResource(ItemResourceType.Wood_Group, 40),
        new UseResource(ItemResourceType.Stone_G, 40)
                    },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                );
        public static readonly CraftBlueprint CannonBarracks = new CraftBlueprint(
                            CraftResultType.Building,
                            (int)Build.BuildAndExpandType.CannonBarracks,
                            1,
                            new UseResource[]
                            {
        new UseResource(ItemResourceType.Wood_Group, 30),
        new UseResource(ItemResourceType.Stone_G, 60)
                            },
                            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                        );

        public static readonly CraftBlueprint KnightsBarracks = new CraftBlueprint(
                  CraftResultType.Building,
                  (int)Build.BuildAndExpandType.KnightsBarracks,
                 1,
                 new UseResource[]
                 {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 100)
                 },
                  XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
            );

        //public static readonly CraftBlueprint PigPen = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.PigPen,
        //    1,
        //    new UseResource[]
        //    {
        //new UseResource(ItemResourceType.Water_G, 4),
        //new UseResource(ItemResourceType.Wood_Group, 20),
        //new UseResource(ItemResourceType.RawFood_Group, DssConst.PigRawFoodAmout)
        //    },
        //    XP.WorkExperienceType.AnimalCare
        //);

        //public static readonly CraftBlueprint HenPen = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.HenPen,
        //    1,
        //    new UseResource[]
        //    {
        //new UseResource(ItemResourceType.Water_G, 2),
        //new UseResource(ItemResourceType.Wood_Group, 20),
        //new UseResource(ItemResourceType.RawFood_Group, DssConst.DefaultItemRawFoodAmount)
        //    },
        //    XP.WorkExperienceType.AnimalCare
        //);

        //static readonly UseResource[] FarmResources = new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.RawFood_Group, 4),
        //        new UseResource(ItemResourceType.Water_G, 2),
        //    };

        //public static readonly CraftBlueprint WheatFarm = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.WheatFarm,
        //    1,
        //    FarmResources,
        //    XP.WorkExperienceType.Farm
        //);

        //public static readonly CraftBlueprint LinenFarm = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.LinenFarm,
        //    1,
        //    FarmResources,
        //    XP.WorkExperienceType.Farm
        //);

        //public static readonly CraftBlueprint HempFarm = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.HempFarm,
        //    1,
        //    FarmResources,
        //    XP.WorkExperienceType.Farm
        //);

        //public static readonly CraftBlueprint RapeseedFarm = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.RapeSeedFarm,
        //    1,
        //    FarmResources,
        //    XP.WorkExperienceType.Farm
        //);
        public static readonly CraftBlueprint PigPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PigPen,
           1,
           new UseResource[]
           {
        new UseResource(ItemResourceType.Water_G, 4),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount)
           }, XP.WorkExperienceType.AnimalCare
       );

        public static readonly CraftBlueprint HenPen = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HenPen,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Water_G, 2),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount)
            }, XP.WorkExperienceType.AnimalCare
        );

        static readonly UseResource[] FarmResources = new UseResource[]
            {
                new UseResource(ItemResourceType.RawFood_Group, 4),
                new UseResource(ItemResourceType.Water_G, 2),
            };

        public static readonly CraftBlueprint WheatFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WheatFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint WheatFarmUpgrade = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WheatFarmUpgraded,
            1, new UseResource[] { new UseResource(ItemResourceType.Toolkit, 1) },
            XP.WorkExperienceType.Farm, XP.ExperienceLevel.Practitioner_2
        )
        { upgradeFrom = WheatFarm };

        public static readonly CraftBlueprint LinenFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint LinenFarmUpgrade = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarmUpgraded,
            1, new UseResource[] { new UseResource(ItemResourceType.Toolkit, 1) },
            XP.WorkExperienceType.Farm, XP.ExperienceLevel.Practitioner_2
        )
        { upgradeFrom = LinenFarm };

        public static readonly CraftBlueprint HempFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint HempFarmUpgrade = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarmUpgraded,
            1, new UseResource[] { new UseResource(ItemResourceType.Toolkit, 1) },
            XP.WorkExperienceType.Farm, XP.ExperienceLevel.Practitioner_2
        )
        { upgradeFrom = HempFarm };

        public static readonly CraftBlueprint RapeseedFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint RapeseedFarmUpgrade = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1, new UseResource[] { new UseResource(ItemResourceType.Toolkit, 1) },
            XP.WorkExperienceType.Farm, XP.ExperienceLevel.Practitioner_2
        )
        { upgradeFrom = RapeseedFarm };

        public static readonly CraftBlueprint Smith = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Smith,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, CraftSmith_IronUse),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint Cook = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Cook,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 10),
               new UseResource(ItemResourceType.Iron_G, 5),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint WorkBench = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkBench,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, 2),
           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CoalPit = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CoalPit,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 30),
           },
            XP.WorkExperienceType.CraftFuel
        );

        public static readonly CraftBlueprint Carpenter = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Carpenter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 8),
           },
            XP.WorkExperienceType.HouseBuilding
        );


        public static readonly CraftBlueprint Armory = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Armory,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 30),
               
           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint Smelter = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Smelter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 30),
               new UseResource(ItemResourceType.Iron_G, 5),
           },
            XP.WorkExperienceType.HouseBuilding
        );




        public static readonly CraftBlueprint WoodCutter = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WoodCutter,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 30),
               new UseResource(ItemResourceType.Stone_G, 5),
                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                );

        public static readonly CraftBlueprint StoneCutter = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.StoneCutter,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 20),
                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                );

        public static readonly CraftBlueprint Bank = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Bank,
                   1,
                   new UseResource[]
                   {
                        new UseResource(ItemResourceType.Gold, 1000),
                        new UseResource(ItemResourceType.Wood_Group, 50),
                        new UseResource(ItemResourceType.Stone_G, 100)
                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
                );

        public static readonly CraftBlueprint CoinMinter = new CraftBlueprint(
               CraftResultType.Building,
               (int)Build.BuildAndExpandType.CoinMinter,
              1,
              new UseResource[]
              {
                   new UseResource(ItemResourceType.Wood_Group, 10),
                   new UseResource(ItemResourceType.Iron_G, CraftSmith_IronUse),
              },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
      );


        public static readonly CraftBlueprint Embassy = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Embassy,
                   1,
                   new UseResource[]
                   {
                        new UseResource(ItemResourceType.Gold, 1000),
                        new UseResource(ItemResourceType.Wood_Group, 50),
                        new UseResource(ItemResourceType.Stone_G, 100)
                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
                );
        public static readonly CraftBlueprint WaterResovoir = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WaterResovoir,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 40),
                   },
                    XP.WorkExperienceType.HouseBuilding
                );
      
                
        public static readonly CraftBlueprint Foundry = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Foundry,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Stone_G, 40),
               new UseResource(ItemResourceType.Iron_G, 5),
                   },
                    XP.WorkExperienceType.HouseBuilding
                );
        public static readonly CraftBlueprint Chemist = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Chemist,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 5),
                    new UseResource(ItemResourceType.Bronze, 30),

                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                );

        public static readonly CraftBlueprint Gunmaker = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Gunmaker,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 20),
                    new UseResource(ItemResourceType.Iron_G, 10),

                   },
                    XP.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint School = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.School,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 100),
                    new UseResource(ItemResourceType.Stone_G, 20),
                    new UseResource(ItemResourceType.Iron_G, 5),

                   },
                    XP.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint NobelHouse = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Nobelhouse,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Gold, 5000),
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 200)
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );


        public static readonly CraftBlueprint DirtRoad = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.DirtRoad,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 1),
           },
           XP.WorkExperienceType.StoneCutter
       );

        public static readonly CraftBlueprint Pavement = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Pavement,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 20),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Practitioner_2
       );
        public static readonly CraftBlueprint PavementFlower = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PavementFlower,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.RawFood_Group, 5),
               new UseResource(ItemResourceType.Stone_G, 20),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Practitioner_2
       );

        public static readonly CraftBlueprint Statue = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Statue_ThePlayer,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 500),
               new UseResource(ItemResourceType.Iron_G, 50),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Master_4
       );



        public static readonly CraftBlueprint DirtWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DirtWall,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint DirtTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DirtTower,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint WoodWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.WoodWall,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint WoodTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.WoodTower,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWall,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneTower,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneWallGreen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallGreen,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneWallBlueRoof = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallBlueRoof,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneWallWoodHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallWoodHouse,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneGate = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneGate,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint StoneHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneHouse,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint PavementLamp = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavementLamp,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint PavemenFountain = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavemenFountain,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint PavementRectFlower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavementRectFlower,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint GardenFourBushes = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenFourBushes,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint GardenLongTree = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenLongTree,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint GardenWalledBush = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenWalledBush,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint SmallCityHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.SmallCityHouse,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint BigCityHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.BigCityHouse,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint CitySquare = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.CitySquare,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint CobbleStones = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.CobbleStones,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Gold, 1),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint GardenGrass = new CraftBlueprint(
          CraftResultType.Building,
          (int)Build.BuildAndExpandType.GardenGrass,
         1,
         new UseResource[]
         {
               new UseResource(ItemResourceType.Gold, 1),
         },
          XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint GardenBird = new CraftBlueprint(
          CraftResultType.Building,
          (int)Build.BuildAndExpandType.GardenBird,
         1,
         new UseResource[]
         {
               new UseResource(ItemResourceType.Gold, 1),
         },
          XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
       );


        public static readonly CraftBlueprint GardenMemoryStone = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.GardenMemoryStone,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
        public static readonly CraftBlueprint Statue_Leader = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Leader,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
        public static readonly CraftBlueprint Statue_Lion = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Lion,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
        public static readonly CraftBlueprint Statue_Horse = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Horse,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
        public static readonly CraftBlueprint Statue_Pillar = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Pillar,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );


      
       public static readonly CraftBlueprint FlagPole_LongBanner = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_LongBanner,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
        public static readonly CraftBlueprint FlagPole_Flag = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
               new UseResource(ItemResourceType.Gold, 1),
        },
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
      );
    }
}
