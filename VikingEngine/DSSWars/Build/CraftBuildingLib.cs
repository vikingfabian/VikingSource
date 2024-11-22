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
        public static readonly ItemResourceType[] SmelterCraftTypes = { 
            ItemResourceType.Cupper, ItemResourceType.Tin, ItemResourceType.Lead, ItemResourceType.Iron_G, 
            ItemResourceType.BloomeryIron, ItemResourceType.Silver, ItemResourceType.Mithril };
        
        public static readonly ItemResourceType[] SmithCraftTypes = { 
            ItemResourceType.ShortSword, ItemResourceType.Sword, ItemResourceType.LongSword, 
            ItemResourceType.Warhammer, ItemResourceType.TwoHandSword, ItemResourceType.KnightsLance, 
            ItemResourceType.IronArmor,            
        };

        public static readonly ItemResourceType[] GunmakerCraftTypes = {
            ItemResourceType.HandCannon, ItemResourceType.HandCulverin,
            ItemResourceType.Rifle, ItemResourceType.Blunderbus,
            ItemResourceType.SiegeCannonIron, ItemResourceType.ManCannonIron,
        };

        public static readonly ItemResourceType[] ArmoryCraftTypes = {
            ItemResourceType.IronArmor, ItemResourceType.HeavyIronArmor, ItemResourceType.LightPlateArmor, ItemResourceType.FullPlateArmor,
        };

        public static readonly ItemResourceType[] FoundryCraftTypes = { 
            ItemResourceType.Bronze, ItemResourceType.CastIron, ItemResourceType.BloomeryIron, ItemResourceType.Mithril };

        public static readonly ItemResourceType[] BenchCraftTypes = { 
            ItemResourceType.Fuel_G, ItemResourceType.PaddedArmor, ItemResourceType.HeavyPaddedArmor, ItemResourceType.SharpStick, ItemResourceType.SlingShot, ItemResourceType.ThrowingSpear };
        
        public static readonly ItemResourceType[] CarpenterCraftTypes = { 
            ItemResourceType.SharpStick, ItemResourceType.Bow, ItemResourceType.LongBow, ItemResourceType.Crossbow, 
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
            Work.WorkExperienceType.HouseBuilding,
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
            }, Work.WorkExperienceType.HouseBuilding,
            CraftRequirement.Logistics2
        );

        public static readonly CraftBlueprint CraftWorkerHut = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkerHuts,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 200),
        new UseResource(ItemResourceType.Stone_G, 40)
            },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftTavern = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Tavern,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 20)
            },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftStorehouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Storehouse,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Stone_G, 40)
           },
            Work.WorkExperienceType.HouseBuilding
       );

        public static readonly CraftBlueprint CraftBrewery = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Brewery,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Iron_G, 5)
           },
            Work.WorkExperienceType.Cook
       );

        public static readonly CraftBlueprint CraftPostal = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Postal,
           1,
           new UseResource[]
           {
        new UseResource(ItemResourceType.Wood_Group, 60),
           },
            Work.WorkExperienceType.HouseBuilding
       );

        public static readonly CraftBlueprint CraftRecruitment = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Recruitment,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 50),
        new UseResource(ItemResourceType.SkinLinen_Group, 10)
            },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftBarracks = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Barracks,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 100),
        new UseResource(ItemResourceType.Stone_G, 20)
            },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftPigPen = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PigPen,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Water_G, 4),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.PigRawFoodAmout)
            },
            Work.WorkExperienceType.AnimalCare
        );

        public static readonly CraftBlueprint CraftHenPen = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HenPen,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Water_G, 2),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.DefaultItemRawFoodAmount)
            },
            Work.WorkExperienceType.AnimalCare
        );

        static readonly UseResource[] FarmResources = new UseResource[]
            {
                new UseResource(ItemResourceType.RawFood_Group, 4),
                new UseResource(ItemResourceType.Water_G, 2),
            };

        public static readonly CraftBlueprint CraftWheatFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WheatFarm,
            1,
            FarmResources,
            Work.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint CraftLinenFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarm,
            1,
            FarmResources,
            Work.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint CraftHempFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarm,
            1,
            FarmResources,
            Work.WorkExperienceType.Farm
        );

        public static readonly CraftBlueprint CraftRapeseedFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1,
            FarmResources,
            Work.WorkExperienceType.Farm
        );

        public const int CraftSmith_IronUse = 10;
        public static readonly CraftBlueprint CraftSmith = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Smith,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, CraftSmith_IronUse),
           },
            Work.WorkExperienceType.HouseBuilding
       );

        public static readonly CraftBlueprint CraftCook = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Cook,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 10),
               new UseResource(ItemResourceType.Iron_G, 5),
           },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftWorkBench = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkBench,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, 2),
           },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftCoalPit = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CoalPit,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 30),
           },
            Work.WorkExperienceType.CraftFuel
        );

        public static readonly CraftBlueprint CraftCarpenter = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Carpenter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 8),
           },
            Work.WorkExperienceType.HouseBuilding
        );


        public static readonly CraftBlueprint CraftBuilding_Smelter = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Smelter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 30),
               new UseResource(ItemResourceType.Iron_G, 5),
           },
            Work.WorkExperienceType.HouseBuilding
        );


        public static readonly CraftBlueprint CraftBuilding_WoodCutter = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WoodCutter,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 30),
               new UseResource(ItemResourceType.Stone_G, 5),
                   },
                    Work.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint CraftBuilding_StoneCutter = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.StoneCutter,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 20),
                   },
                    Work.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint CraftBuilding_Embassy = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Embassy,
                   1,
                   new UseResource[]
                   {
                        new UseResource(ItemResourceType.Gold, 1000),
                        new UseResource(ItemResourceType.Wood_Group, 50),
                        new UseResource(ItemResourceType.Stone_G, 100)
                   },
                    Work.WorkExperienceType.HouseBuilding
                );
        public static readonly CraftBlueprint CraftBuilding_WaterResovoir = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.WaterResovoir,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Wood_Group, 40),
                   },
                    Work.WorkExperienceType.HouseBuilding
                );
        public static readonly CraftBlueprint CraftBuilding_KnightsBarracks = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.KnightsBarracks,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 100)
                   },
                    Work.WorkExperienceType.HouseBuilding
                );
        public static readonly CraftBlueprint CraftBuilding_Foundry = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Foundry,
                   1,
                   new UseResource[]
                   {
               new UseResource(ItemResourceType.Stone_G, 40),
               new UseResource(ItemResourceType.Iron_G, 5),
                   },
                    Work.WorkExperienceType.HouseBuilding
                );
        public static readonly CraftBlueprint CraftBuilding_Chemist = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Chemist,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 5),
                    new UseResource(ItemResourceType.Bronze, 30),

                   },
                    Work.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint CraftBuilding_Gunmaker = new CraftBlueprint(
                    CraftResultType.Building,
                    (int)Build.BuildAndExpandType.Gunmaker,
                   1,
                   new UseResource[]
                   {
                    new UseResource(ItemResourceType.Wood_Group, 20),
                    new UseResource(ItemResourceType.Stone_G, 20),
                    new UseResource(ItemResourceType.Iron_G, 10),

                   },
                    Work.WorkExperienceType.HouseBuilding
                );

        public static readonly CraftBlueprint CraftNobelHouse = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Nobelhouse,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Gold, 5000),
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 200)
            },
            Work.WorkExperienceType.HouseBuilding
        );

        public static readonly CraftBlueprint CraftPavement = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Pavement,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 20),
           },
            Work.WorkExperienceType.StoneCutter
       );
        public static readonly CraftBlueprint CraftPavementFlower = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PavementFlower,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.RawFood_Group, 5),
               new UseResource(ItemResourceType.Stone_G, 20),
           },
            Work.WorkExperienceType.StoneCutter
       );

        public static readonly CraftBlueprint CraftStatue = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Statue_ThePlayer,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 500),
               new UseResource(ItemResourceType.Iron_G, 50),
           },
            Work.WorkExperienceType.StoneCutter
       );
    }
}
