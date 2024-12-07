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
            ItemResourceType.Cupper, ItemResourceType.Tin, ItemResourceType.Lead, ItemResourceType.Iron_G, 
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

        public static readonly CraftBlueprint LinenFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint HempFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint RapeseedFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1,
            FarmResources, XP.WorkExperienceType.Farm
        );

       
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
    }
}
