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

        public static readonly CraftBlueprint TreeSoft = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.TreeSoft,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
);

        public static readonly CraftBlueprint TreeHard = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.TreeHard,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        //public static readonly CraftBlueprint StonesMine = new CraftBlueprint(
        //    CraftResultType.Building,
        //    (int)Build.BuildAndExpandType.StonesMine,
        //    1,
        //    new UseResource[] { },
        //    XP.WorkExperienceType.Farm
        //);

        public static readonly CraftBlueprint CoalMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CoalMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint StoneBlockMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.StoneBlockMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint IronOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.IronOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint TinOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.TinOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint CopperOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CopperOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint SilverOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.SilverOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint GoldOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.GoldOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint LeadOreMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LeadOreMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint MithrilMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.MithrilMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint SulfurMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.SulfurMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint SaltMine = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.SaltMine,
            1,
            new UseResource[] { },
            XP.WorkExperienceType.Farm
        );



        //public static readonly ItemResourceType[] CoinMinterCraftTypes = {
        //     ItemResourceType.CoolingFluid, ItemResourceType.BlackPowder, ItemResourceType.GunPowder };

        public static readonly CraftBlueprint CraftLogistics = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Logistics,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, 2),
                new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Stone_G, 30)
            },
            XP.WorkExperienceType.HouseBuilding,
             XP.ExperienceLevel.Beginner_1
            //,
             //BuildAndExpandType.f
        );
        public static readonly CraftBlueprint CraftLogisticsLevel2 = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Logistics,
            2,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, 4),
                new UseResource(ItemResourceType.Container, 10),
                new UseResource(ItemResourceType.Brick, 10)
            }, XP.WorkExperienceType.HouseBuilding,
             XP.ExperienceLevel.Beginner_1,
            Build.BuildAndExpandType.Logistics
        );

        public static readonly CraftBlueprint WorkerTent = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkerTent,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.SkinLinen_Group, 30),
            },
            XP.WorkExperienceType.Transport
        );

        public static readonly CraftBlueprint WorkerHut = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkerHut,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 200),
                new UseResource(ItemResourceType.Stone_G, 40)
            },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint WorkerHutLarge = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.WorkerHutLarge,
           1,
           new UseResource[]
           {
            new UseResource(ItemResourceType.Wood_Group, 300),
            new UseResource(ItemResourceType.Stone_G, 60)
           },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
       );

        public static readonly CraftBlueprint ManorLord = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.ManorLord,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 40),
                new UseResource(ItemResourceType.Stone_G, 40),
           },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint GreatHall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GreatHall,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Stone_G, 80),
                new UseResource(ItemResourceType.Iron_G, 10)
           },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
       );

        public static readonly CraftBlueprint Tavern = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Tavern,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, 2),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
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
        public static readonly CraftBlueprint Brewery_Bronze = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Brewery,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Bronze, 5)
           },
            XP.WorkExperienceType.Cook, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint Postal = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Postal,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
        new UseResource(ItemResourceType.Wood_Group, 80),
        new UseResource(ItemResourceType.Stone_G, 20)
            },
                    XP.WorkExperienceType.HouseBuilding
            );

        public static readonly CraftBlueprint WarmachineBarracks = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WarmachineBarracks,
                    1,
                    new UseResource[]
                    {
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
        new UseResource(ItemResourceType.Wood_Group, 40),
        new UseResource(ItemResourceType.Brick, 20)
                    },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                );
        public static readonly CraftBlueprint CannonBarracks = new CraftBlueprint(
                            CraftResultType.Building,
                            (int)Build.BuildAndExpandType.CannonBarracks,
                            1,
                            new UseResource[]
                            {
               new UseResource(ItemResourceType.ServiceMen, 1),
        new UseResource(ItemResourceType.Wood_Group, 30),
        new UseResource(ItemResourceType.Brick, 30)
                            },
                            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
                        );

        //public static readonly CraftBlueprint KnightsBarracks = new CraftBlueprint(
        //          CraftResultType.Building,
        //          (int)Build.BuildAndExpandType.KnightsBarracks,
        //         1,
        //         new UseResource[]
        //         {
        //       new UseResource(ItemResourceType.ServiceMen, 1),
        //            new UseResource(ItemResourceType.Wood_Group, 20),
        //            new UseResource(ItemResourceType.Stone_G, 100)
        //         },
        //          XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        //    );
        public static readonly CraftBlueprint BoarPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.BoarPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Boar, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
           }, XP.WorkExperienceType.AnimalCare
       );

        public static readonly CraftBlueprint FowlPen = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.FowlPen,
            1,
            new UseResource[]
            {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Fowl, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
            }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint PigPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PigPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Pig, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
           }, XP.WorkExperienceType.AnimalCare
       );

        public static readonly CraftBlueprint HenPen = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HenPen,
            1,
            new UseResource[]
            {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Hen, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
            }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint DogCage = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DogCage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Dog, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Iron_G, 10),
           }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint HoundCage = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.HoundCage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Hound, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Iron_G, 20),
           }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint OxenPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.OxenPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Oxen, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
           }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint KineOxenPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.KineOxenPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.KineOxen, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 40),
           }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint PonyPen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PonyPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Pony, DssConst.PenBreedingStockCount),
                new UseResource(ItemResourceType.Wood_Group, 20),
           }, XP.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint HorsePen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.HorsePen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Horse, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WarHorsePen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WarHorsePen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 2),
               new UseResource(ItemResourceType.WarHorse, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint DraftHorsePen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.DraftHorsePen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 2),
               new UseResource(ItemResourceType.DraftHorse, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WildPigPen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WildPigPen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.WildPig, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WildHogPen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WildHogPen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.WildHog, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WarHogPen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WarHogPen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 2),
               new UseResource(ItemResourceType.WarHog, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint StagHogPen = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.StagHogPen,
                1,
                new UseResource[]
                {
               new UseResource(ItemResourceType.ServiceMen, 2),
               new UseResource(ItemResourceType.StagHog, DssConst.PenBreedingStockCount),
            new UseResource(ItemResourceType.Wood_Group, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WolfCage = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WolfCage,
                1,
                new UseResource[]
                {
                    new UseResource(ItemResourceType.ServiceMen, 1),
                    new UseResource(ItemResourceType.Wolf, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Iron_G, 10),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint WargCage = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WargCage,
                1,
                new UseResource[]
                {
                   new UseResource(ItemResourceType.ServiceMen, 2),
                   new UseResource(ItemResourceType.Warg, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Iron_G, 20),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint AlphaWargCage = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.AlphaWargCage,
                1,
                new UseResource[]
                {
                   new UseResource(ItemResourceType.ServiceMen, 4),
                   new UseResource(ItemResourceType.AlphaWarg, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Iron_G, 40),
                }, XP.WorkExperienceType.AnimalCare
            );


        public static readonly CraftBlueprint WildCatCage = new CraftBlueprint(
                 CraftResultType.Building,
                 (int)Build.BuildAndExpandType.WildCatCage,
                 1,
                 new UseResource[]
                 {
                    new UseResource(ItemResourceType.ServiceMen, 1),
                    new UseResource(ItemResourceType.WildCat, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Wood_Group, 20),
                 }, XP.WorkExperienceType.AnimalCare
             );

        public static readonly CraftBlueprint LionCage = new CraftBlueprint(
                 CraftResultType.Building,
                 (int)Build.BuildAndExpandType.LionCage,
                 1,
                 new UseResource[]
                 {
                    new UseResource(ItemResourceType.ServiceMen, 2),
                    new UseResource(ItemResourceType.Lion, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Iron_G, 20),
                 }, XP.WorkExperienceType.AnimalCare
             );

        public static readonly CraftBlueprint WarLionCage = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.WarLionCage,
                1,
                new UseResource[]
                {
                   new UseResource(ItemResourceType.ServiceMen, 4),
                   new UseResource(ItemResourceType.WarLion, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Iron_G, 40),
                }, XP.WorkExperienceType.AnimalCare
            );

        public static readonly CraftBlueprint ElephantCage = new CraftBlueprint(
                 CraftResultType.Building,
                 (int)Build.BuildAndExpandType.ElephantCage,
                 1,
                 new UseResource[]
                 {
                    new UseResource(ItemResourceType.ServiceMen, 1),
                    new UseResource(ItemResourceType.Elephant, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Stone_G, 10),
                 }, XP.WorkExperienceType.AnimalCare
             );

        public static readonly CraftBlueprint WarElephantCage = new CraftBlueprint(
                 CraftResultType.Building,
                 (int)Build.BuildAndExpandType.WarElephantCage,
                 1,
                 new UseResource[]
                 {
                    new UseResource(ItemResourceType.ServiceMen, 2),
                    new UseResource(ItemResourceType.WarElephant, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Stone_G, 20),
                 }, XP.WorkExperienceType.AnimalCare
             );

        public static readonly CraftBlueprint OliphantCage = new CraftBlueprint(
                CraftResultType.Building,
                (int)Build.BuildAndExpandType.OliphantCage,
                1,
                new UseResource[]
                {
                   new UseResource(ItemResourceType.ServiceMen, 4),
                   new UseResource(ItemResourceType.Oliphant, DssConst.PenBreedingStockCount),
                    new UseResource(ItemResourceType.Brick, 20),
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

        public static readonly CraftBlueprint WheatFarm_Gold = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WheatFarm,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Gold, DssConst.FoodGoldValue_BlackMarket * 3),
                new UseResource(ItemResourceType.Water_G, 2),
            }, XP.WorkExperienceType.Farm
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
            [
                new UseResource(ItemResourceType.SkinLinen_Group, 4),
                new UseResource(ItemResourceType.Water_G, 2)
            ], 
             XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint LinenFarm_Gold = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarm,
            1,
            [
                new UseResource(ItemResourceType.Gold, 20),
                new UseResource(ItemResourceType.Water_G, 2)
            ],
             XP.WorkExperienceType.Farm
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
            [
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
                new UseResource(ItemResourceType.Fuel_G, 2),
                new UseResource(ItemResourceType.Water_G, 2)
            ], XP.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint HempFarm_Gold = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarm,
            1,
            [
                new UseResource(ItemResourceType.Gold, 25),
                new UseResource(ItemResourceType.Water_G, 2)
            ], XP.WorkExperienceType.Farm
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
            [new UseResource(ItemResourceType.Fuel_G, 4), new UseResource(ItemResourceType.Water_G, 2)],
            XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint RapeseedFarm_Gold = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1,
            [   new UseResource(ItemResourceType.Gold, 20),
                new UseResource(ItemResourceType.Water_G, 2)],
            XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint RapeseedFarmUpgrade = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1, new UseResource[] { new UseResource(ItemResourceType.Toolkit, 1) },
            XP.WorkExperienceType.Farm, XP.ExperienceLevel.Practitioner_2
        )
        { upgradeFrom = RapeseedFarm };


        public static readonly CraftBlueprint TreeSeedlingSoft = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.TreeSeedlingSoft,
            1,
            new UseResource[] 
            {
                new UseResource(ItemResourceType.Wood_Group, 2),
                new UseResource(ItemResourceType.Water_G, 20),
            }, XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint TreeSeedlingSoft_Gold = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.TreeSeedlingSoft,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Gold, 10),
                new UseResource(ItemResourceType.Water_G, 20),
           }, XP.WorkExperienceType.Farm
       );

        public static readonly CraftBlueprint TreeSeedlingHard = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.TreeSeedlingHard,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 2),
                new UseResource(ItemResourceType.Water_G, 50),
            }, XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint TreeSeedlingHard_Gold = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.TreeSeedlingHard,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Gold, 10),
                new UseResource(ItemResourceType.Water_G, 50),
           }, XP.WorkExperienceType.Farm
       );

        public static readonly CraftBlueprint Orchard = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.OrchardApple,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Food_G, 6),
                new UseResource(ItemResourceType.Water_G, 30),
            }, XP.WorkExperienceType.Farm
        );
        public static readonly CraftBlueprint Orchard_Gold = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.OrchardApple,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Gold, DssConst.FoodGoldValue_BlackMarket * 6),
                new UseResource(ItemResourceType.Water_G, 30),
            }, XP.WorkExperienceType.Farm
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
               new UseResource(ItemResourceType.Wood_Group, 20),
               //new UseResource(ItemResourceType.Stone_G, 10),
               new UseResource(ItemResourceType.Iron_G, 5),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint Cook_Copper = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Cook,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 10),
               new UseResource(ItemResourceType.Copper, 5),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint WorkBench = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkBench,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 2),
           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint WorkBench_Bronze = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkBench,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Bronze, 2),
           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CoalPit = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CoalPit,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 15),
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
        public static readonly CraftBlueprint Carpenter_Bronze = new CraftBlueprint(
                   CraftResultType.Building,
                   (int)Build.BuildAndExpandType.Carpenter,
                  1,
                  new UseResource[]
                  {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Bronze, 8),
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

            public static readonly CraftBlueprint ShieldMaker = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.ShieldMaker,
            1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 30),

           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint Pottery = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Pottery,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Stone_G, 8),
          },
           XP.WorkExperienceType.HouseBuilding
       );
        public static readonly CraftBlueprint DryingPan = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DryingPan,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Stone_G, 20),
          },
           XP.WorkExperienceType.HouseBuilding
       );
        public static readonly CraftBlueprint Butcher = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Butcher,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 8),
          },
           XP.WorkExperienceType.HouseBuilding
       );
        public static readonly CraftBlueprint Smoker = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Smoker,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 8),
          },
           XP.WorkExperienceType.HouseBuilding
       );


        public static readonly CraftBlueprint Dryer = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Dryer,
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
               new UseResource(ItemResourceType.Brick, 15),
           },
            XP.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint TrapperHut = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.TrapperHut,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.ServiceMen, 1),
               new UseResource(ItemResourceType.Wood_Group, 30),
               new UseResource(ItemResourceType.SkinLinen_Group, 5),
                   },
                    XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
                );

        public static readonly CraftBlueprint WoodCutter = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WoodCutter,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
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
               new UseResource(ItemResourceType.ServiceMen, 4),
                        new UseResource(ItemResourceType.Gold, 1000),
                        new UseResource(ItemResourceType.Wood_Group, 50),
                        new UseResource(ItemResourceType.Brick, 50)
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
               new UseResource(ItemResourceType.ServiceMen, 4),
                        new UseResource(ItemResourceType.Gold, 1000),
                        new UseResource(ItemResourceType.Wood_Group, 50),
                        new UseResource(ItemResourceType.Brick, 50)
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
               new UseResource(ItemResourceType.Brick, 20),
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
               new UseResource(ItemResourceType.ServiceMen, 1),
                    new UseResource(ItemResourceType.Wood_Group, 100),
                    new UseResource(ItemResourceType.Stone_G, 20),

                   },
                    XP.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint NobelHouse = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Nobelhouse,
            1,
            new UseResource[]
            {
               new UseResource(ItemResourceType.ServiceMen, 20),
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Brick, 100)
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
               new UseResource(ItemResourceType.Brick, 10),
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
               new UseResource(ItemResourceType.Brick, 10),
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

        const int StorageContainerCost = 20;
        const int StorageQuarterCost = 5;

        public static readonly CraftBlueprint MaterialStorage = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.MaterialStorage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Container, StorageContainerCost),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint FoodStorage = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.FoodStorage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Container, StorageContainerCost),
               new UseResource(ItemResourceType.Salt, StorageQuarterCost),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint WeaponStorage = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WeaponStorage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Container, StorageContainerCost),
               new UseResource(ItemResourceType.Iron_G, StorageQuarterCost),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint ArmorStorage = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.ArmorStorage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Container, StorageContainerCost),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint AnimalStorage = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.AnimalStorage,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Container, StorageContainerCost),
               new UseResource(ItemResourceType.RawFood_Group, StorageContainerCost),
           },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint Cesspit = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Cesspit,
          1,
          new UseResource[]
          {
                new UseResource(ItemResourceType.Brick, 10),
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint DirtWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DirtWall,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Stone_G, 20),
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint DirtTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.DirtTower,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 25),
                new UseResource(ItemResourceType.Stone_G, 25),
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint Palisade = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Palisade,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Palisade, 4),

          },
           XP.WorkExperienceType.Transport, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint WoodWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.WoodWall,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 80),
                new UseResource(ItemResourceType.Stone_G, 20),

          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
        );
        public static readonly CraftBlueprint WoodTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.WoodTower,
          1,
         new UseResource[]
          {
               new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 30),

          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
        );
        public static readonly CraftBlueprint StoneWall = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWall,
          1,
          new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Brick, 50)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint StoneTower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneTower,
          1,
          new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 40),
                new UseResource(ItemResourceType.Stone_G, 120)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint StoneWallGreen = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallGreen,
          1,
           new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 40),
                new UseResource(ItemResourceType.Stone_G, 120)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint StoneWallBlueRoof = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallBlueRoof,
          1,
           new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 40),
                new UseResource(ItemResourceType.Stone_G, 120)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint StoneWallWoodHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneWallWoodHouse,
          1,
           new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 40),
                new UseResource(ItemResourceType.Stone_G, 120)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint StoneGate = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneGate,
          1,
           new UseResource[]
          {
              new UseResource(ItemResourceType.ServiceMen, 1),
              new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.Brick, 50)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
          
        );
        public static readonly CraftBlueprint StoneHouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.StoneHouse,
          1,
          new UseResource[]
          {
              new UseResource(ItemResourceType.Wood_Group, 20),
              new UseResource(ItemResourceType.Brick, 50)
          },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint PavementLamp = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavementLamp,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Brick, 10),
                new UseResource(ItemResourceType.Fuel_G, 20),

          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint PavemenFountain = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavemenFountain,
          1,
          new UseResource[]
          {
                new UseResource(ItemResourceType.Brick, 10),
                new UseResource(ItemResourceType.Water_G, 8),

          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Master_4
        );
        public static readonly CraftBlueprint PavementRectFlower = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.PavementRectFlower,
          1,
          new UseResource[]
          {
                new UseResource(ItemResourceType.Stone_G, 20),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Expert_3
        );
        public static readonly CraftBlueprint GardenFourBushes = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenFourBushes,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Water_G, 4),
                new UseResource(ItemResourceType.RawFood_Group, 2),
               new UseResource(ItemResourceType.Wood_Group, 6),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint GardenLongTree = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenLongTree,
          1,
          new UseResource[]
          {
              new UseResource(ItemResourceType.Water_G, 4),
                new UseResource(ItemResourceType.RawFood_Group, 2),
               new UseResource(ItemResourceType.Wood_Group, 6),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );
        public static readonly CraftBlueprint GardenWalledBush = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GardenWalledBush,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Water_G, 4),
                new UseResource(ItemResourceType.RawFood_Group, 2),
               new UseResource(ItemResourceType.Wood_Group, 6),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Practitioner_2
        );
        //public static readonly CraftBlueprint SmallCityHouse = new CraftBlueprint(
        //   CraftResultType.Building,
        //   (int)Build.BuildAndExpandType.ServiceHouse_Small,
        //  1,
        //  new UseResource[]
        //  {
        //       new UseResource(ItemResourceType.Gold, 1),
        //  },
        //   XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        //);
        //public static readonly CraftBlueprint BigCityHouse = new CraftBlueprint(
        //   CraftResultType.Building,
        //   (int)Build.BuildAndExpandType.ServiceHouse_Large,
        //  1,
        //  new UseResource[]
        //  {
        //       new UseResource(ItemResourceType.Gold, 1),
        //  },
        //   XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        //);
        public static readonly CraftBlueprint CitySquare = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.CitySquare,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Brick, 10),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Practitioner_2
        );
        public static readonly CraftBlueprint CobbleStones = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.CobbleStones,
          1,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Stone_G, 5),
          },
           XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
        );

        public static readonly CraftBlueprint GardenGrass = new CraftBlueprint(
          CraftResultType.Building,
          (int)Build.BuildAndExpandType.GardenGrass,
         1,
         new UseResource[]
         {
               new UseResource(ItemResourceType.Water_G, 4),
                new UseResource(ItemResourceType.RawFood_Group, 2),

         },
          XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Beginner_1
       );

        public static readonly CraftBlueprint GardenBird = new CraftBlueprint(
          CraftResultType.Building,
          (int)Build.BuildAndExpandType.GardenBird,
         1,
         new UseResource[]
         {
               new UseResource(ItemResourceType.Gold, 10),
               new UseResource(ItemResourceType.Water_G, 4),
               new UseResource(ItemResourceType.RawFood_Group, 8),
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
         XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Practitioner_2
      );
        public static readonly CraftBlueprint Statue_Leader = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Leader,
        1,
         new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 400),
               new UseResource(ItemResourceType.Iron_G, 50),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Master_4
       );
        public static readonly CraftBlueprint Statue_Lion = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Lion,
        1,
         new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 300),
               new UseResource(ItemResourceType.Iron_G, 10),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Master_4
       );
        public static readonly CraftBlueprint Statue_Horse = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Horse,
        1,
         new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 300),
               new UseResource(ItemResourceType.Iron_G, 10),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Master_4
       );
        public static readonly CraftBlueprint Statue_Pillar = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.Statue_Pillar,
        1,
        new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 300),
               new UseResource(ItemResourceType.Iron_G, 10),
           },
            XP.WorkExperienceType.StoneCutter, XP.ExperienceLevel.Expert_3
       );



        public static readonly CraftBlueprint FlagPole_LongBanner = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_LongBanner,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );

        public static readonly CraftBlueprint FlagPole_Flag = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );



        public static readonly CraftBlueprint FlagPole_Banner = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );

        public static readonly CraftBlueprint FlagPole_SlimBanner = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
       new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );

        public static readonly CraftBlueprint FlagPole_FlagRound = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );
        public static readonly CraftBlueprint FlagPole_FlagLarge = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );
        public static readonly CraftBlueprint FlagPole_Streamer = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),
        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );
        public static readonly CraftBlueprint FlagPole_Triangle = new CraftBlueprint(
         CraftResultType.Building,
         (int)Build.BuildAndExpandType.FlagPole_Flag,
        1,
        new UseResource[]
        {
                new UseResource(ItemResourceType.Bronze, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 5),
                new UseResource(ItemResourceType.Wood_Group, 6),

        },
         XP.WorkExperienceType.CraftArmor, XP.ExperienceLevel.Expert_3
      );

        public static readonly CraftBlueprint CityHall_Village = new CraftBlueprint(
            CraftResultType.NoSet,
            0,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, DssConst.VillageHall_RequiredStaff),
                new UseResource(ItemResourceType.Gold, 500),
                new UseResource(ItemResourceType.Wood_Group, 50),
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
            );

        public static readonly CraftBlueprint CityHall_Town = new CraftBlueprint(
            CraftResultType.NoSet,
            0,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, DssConst.TownHall_RequiredStaff - DssConst.VillageHall_RequiredStaff),
                new UseResource(ItemResourceType.Gold, 1000),
                new UseResource(ItemResourceType.Wood_Group, 30),
                new UseResource(ItemResourceType.Brick, 50)
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
            );

        public static readonly CraftBlueprint CityHall_Capital = new CraftBlueprint(
           CraftResultType.NoSet,
           0,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.ServiceMen, DssConst.CapitalHall_RequiredStaff - DssConst.TownHall_RequiredStaff),
                new UseResource(ItemResourceType.Gold, 1500),
                new UseResource(ItemResourceType.Wood_Group, 10),
                new UseResource(ItemResourceType.Brick, 75)
           },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
           );


        public static readonly CraftBlueprint ServiceHouse_Small = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.ServiceHouse_Small,
            1,
            new UseResource[]
                {
                    new UseResource(ItemResourceType.Men, DssConst.HousingCount_ServiceHouse_Small),
                    new UseResource(ItemResourceType.Wood_Group, 200),
                    new UseResource(ItemResourceType.Brick, 20)
                },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
            );

        public static readonly CraftBlueprint ServiceHouse_Large = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.ServiceHouse_Large,
            1,
            new UseResource[]
                {
                    new UseResource(ItemResourceType.Men, DssConst.HousingCount_ServiceHouse_Large),
                    new UseResource(ItemResourceType.Wood_Group, 100),
                    new UseResource(ItemResourceType.Brick, 100)
                },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
            );

        public static readonly CraftBlueprint GuardHouse_Small = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.GuardHouse_Small,
            1,
            new UseResource[]
                {
                    new UseResource(ItemResourceType.Wood_Group, 140),
                    new UseResource(ItemResourceType.Brick, 30)
                },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
            );

        public static readonly CraftBlueprint GuardHouse_Large = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.GuardHouse_Large,
           1,
           new UseResource[]
               {
                    new UseResource(ItemResourceType.Wood_Group, 60),
                    new UseResource(ItemResourceType.Brick, 60)
               },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Expert_3
           );

        public static readonly CraftBlueprint ImmigrationTent = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.ImmigrationTent,
           1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 80),
                new UseResource(ItemResourceType.Wood_Group, 60)
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
           );

        public static readonly CraftBlueprint ResearchCenter = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.ResearchCenter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.ServiceMen, 4),
               new UseResource(ItemResourceType.Bronze, 40),
                new UseResource(ItemResourceType.Brick, 150)
           },
           XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Practitioner_2
        );
        
        public static readonly CraftBlueprint BookPress = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.ResearchCenter,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.ServiceMen, 4),
                new UseResource(ItemResourceType.Bronze, 50),
                new UseResource(ItemResourceType.Wood_Group, 100),
           
            },
            XP.WorkExperienceType.HouseBuilding, XP.ExperienceLevel.Beginner_1
        );

    }
}
