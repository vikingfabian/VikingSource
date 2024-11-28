using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.Resource
{
    static class ResourceLib
    {
        public static readonly CraftBlueprint ConvertGoldOre = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.GoldOre, 1),
           },
            Work.WorkExperienceType.CraftMetal
       );

        public static readonly CraftBlueprint CupperCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           5,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Cupper, 5),
           },
            Work.WorkExperienceType.NONE
       );

        public static readonly CraftBlueprint BronzeCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           10,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Bronze, 5),
           },
            Work.WorkExperienceType.NONE
       );

        public static readonly CraftBlueprint SilverCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          20,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Silver, 5),
          },
           Work.WorkExperienceType.NONE
      );

        public static readonly CraftBlueprint ElfCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          100,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Mithril, 1),
          },
           Work.WorkExperienceType.NONE
      );

        // public static readonly CraftBlueprint CraftRecruitment = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Recruitment,
        //     1,
        //     new UseResource[]
        //     {
        // new UseResource(ItemResourceType.Wood_Group, 50),
        // new UseResource(ItemResourceType.SkinLinen_Group, 10)
        //     }
        // );

        // public static readonly CraftBlueprint CraftBarracks = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Barracks,
        //     1,
        //     new UseResource[]
        //     {
        // new UseResource(ItemResourceType.Wood_Group, 100),
        // new UseResource(ItemResourceType.Stone_G, 20)
        //     }
        // );

        // public static readonly CraftBlueprint CraftPigPen = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.PigPen,
        //     1,
        //     new UseResource[]
        //     {
        // new UseResource(ItemResourceType.Water_G, 4),
        // new UseResource(ItemResourceType.Wood_Group, 20),
        // new UseResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount)
        //     }
        // );

        // public static readonly CraftBlueprint CraftHenPen = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.HenPen,
        //     1,
        //     new UseResource[]
        //     {
        // new UseResource(ItemResourceType.Water_G, 2),
        // new UseResource(ItemResourceType.Wood_Group, 20),
        // new UseResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount)
        //     }
        // );

        // static readonly UseResource[] FarmResources = new UseResource[]
        //     {
        //         new UseResource(ItemResourceType.RawFood_Group, 4),
        //         new UseResource(ItemResourceType.Water_G, 2),
        //     };

        // public static readonly CraftBlueprint CraftWheatFarm = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.WheatFarm,
        //     1,
        //     FarmResources
        // );

        // public static readonly CraftBlueprint CraftLinenFarm = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.LinenFarm,
        //     1,
        //     FarmResources
        // );

        // public static readonly CraftBlueprint CraftHempFarm = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.HempFarm,
        //     1,
        //     FarmResources
        // );

        // public static readonly CraftBlueprint CraftRapeseedFarm = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.RapeSeedFarm,
        //     1,
        //     FarmResources
        // );

        // public const int CraftSmith_IronUse = 10;
        // public static readonly CraftBlueprint CraftSmith = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Smith,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Wood_Group, 10),
        //        new UseResource(ItemResourceType.Iron_G, CraftSmith_IronUse),
        //    }
        //);

        // public static readonly CraftBlueprint CraftCook = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Cook,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Wood_Group, 10),
        //        new UseResource(ItemResourceType.Stone_G, 10),
        //        new UseResource(ItemResourceType.Iron_G, 5),
        //    }
        // );

        // public static readonly CraftBlueprint CraftWorkBench = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.WorkBench,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Wood_Group, 10),
        //        new UseResource(ItemResourceType.Iron_G, 2),
        //    }
        // );

        // public static readonly CraftBlueprint CraftCoalPit = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.CoalPit,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Stone_G, 30),
        //    }
        // );

        // public static readonly CraftBlueprint CraftCarpenter = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Carpenter,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Wood_Group, 20),
        //        new UseResource(ItemResourceType.Iron_G, 8),
        //    }
        // );

        // public static readonly CraftBlueprint CraftNobelHouse = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Nobelhouse,
        //     1,
        //     new UseResource[]
        //     {
        //         new UseResource(ItemResourceType.Gold, 5000),
        //         new UseResource(ItemResourceType.Wood_Group, 100),
        //         new UseResource(ItemResourceType.Stone_G, 200)
        //     }
        // );

        // public static readonly CraftBlueprint CraftPavement = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Pavement,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Stone_G, 20),
        //    }
        //);
        // public static readonly CraftBlueprint CraftPavementFlower = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.PavementFlower,
        //    1,
        //    new UseResource[]
        //    {
        //         new UseResource(ItemResourceType.RawFood_Group, 5),
        //        new UseResource(ItemResourceType.Stone_G, 20),
        //    }
        //);

        // public static readonly CraftBlueprint CraftStatue = new CraftBlueprint(
        //     CraftResultType.Building,
        //     (int)Build.BuildAndExpandType.Statue_ThePlayer,
        //    1,
        //    new UseResource[]
        //    {
        //        new UseResource(ItemResourceType.Stone_G, 500),
        //        new UseResource(ItemResourceType.Iron_G, 50),
        //    }
        //);




        public static string Name(ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Gold:
                    return DssRef.lang.ResourceType_Gold;

                case ResourceType.Worker:
                    return DssRef.lang.ResourceType_Workers;

                case ResourceType.DiplomaticPoint:
                    return DssRef.lang.ResourceType_DiplomacyPoints;

                case ResourceType.MercenaryOnMarket:
                    return DssRef.lang.Hud_MercenaryMarket;


                default:
                    return "Unknown resource";
            }
        }
        public static SpriteName PayIcon(ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Gold:
                    return SpriteName.rtsUpkeep;

                case ResourceType.Worker:
                    return SpriteName.WarsWorker;

                case ResourceType.DiplomaticPoint:
                    return SpriteName.WarsDiplomaticSub;

                case ResourceType.MercenaryOnMarket:
                    return SpriteName.WarsGroupIcon;

                default:
                    return SpriteName.NO_IMAGE;
            }
        }



        public static SpriteName Icon(ItemResourceType resource)
        {
            switch (resource)
            {
                case ItemResourceType.Gold:
                    return SpriteName.rtsMoney;

                case ItemResourceType.Ballista:
                    return SpriteName.WarsResource_Ballista;
                case ItemResourceType.Beer:
                    return SpriteName.WarsResource_Beer;
                case ItemResourceType.Bow:
                    return SpriteName.WarsResource_Bow;
                case ItemResourceType.Crossbow:
                    return SpriteName.WarsResource_Crossbow;
                case ItemResourceType.Egg:
                    return SpriteName.WarsResource_Egg;
                case ItemResourceType.Food_G:
                    return SpriteName.WarsResource_Food;
                case ItemResourceType.GoldOre:
                    return SpriteName.WarsResource_GoldOre;
                case ItemResourceType.HeavyIronArmor:
                    return SpriteName.WarsResource_HeavyIronArmor;
                case ItemResourceType.Iron_G:
                    return SpriteName.WarsResource_Iron;
                case ItemResourceType.IronOre_G:
                    return SpriteName.WarsResource_IronOre;
                case ItemResourceType.IronArmor:
                    return SpriteName.WarsResource_IronArmor;
                case ItemResourceType.PaddedArmor:
                    return SpriteName.WarsResource_PaddedArmor;
                case ItemResourceType.Linen:
                    return SpriteName.WarsResource_Linen;
                case ItemResourceType.LongBow:
                    return SpriteName.WarsResource_Longbow;
                case ItemResourceType.Hemp:
                    return SpriteName.WarsResource_Hemp;
                case ItemResourceType.Hen:
                case ItemResourceType.Pig:
                    return SpriteName.WarsResource_RawMeat;
                case ItemResourceType.Rapeseed:
                    return SpriteName.WarsResource_Rapeseed;
                case ItemResourceType.RawFood_Group:
                    return SpriteName.WarsResource_RawFood;
                case ItemResourceType.SharpStick:
                    return SpriteName.WarsResource_Sharpstick;
                case ItemResourceType.SkinLinen_Group:
                    return SpriteName.WarsResource_LinenCloth;
                case ItemResourceType.Stone_G:
                    return SpriteName.WarsResource_Stone;
                case ItemResourceType.Sword:
                    return SpriteName.WarsResource_Sword;
                case ItemResourceType.Water_G:
                    return SpriteName.WarsResource_Water;
                case ItemResourceType.Wheat:
                    return SpriteName.WarsResource_Wheat;
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group:
                    return SpriteName.WarsResource_Wood;
                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G:
                    return SpriteName.WarsResource_Fuel;
                case ItemResourceType.TwoHandSword:
                    return SpriteName.WarsResource_TwoHandSword;
                case ItemResourceType.KnightsLance:
                    return SpriteName.WarsResource_KnightsLance;


                case ItemResourceType.Wagon2Wheel:
                    return SpriteName.WarsResource_Wagon2Wheel;
                case ItemResourceType.Wagon4Wheel:
                    return SpriteName.WarsResource_Wagon4Wheel;
                case ItemResourceType.Tin:
                    return SpriteName.WarsResource_Tin;
                case ItemResourceType.TinOre:
                    return SpriteName.WarsResource_TinOre;
                case ItemResourceType.Bronze:
                    return SpriteName.WarsResource_Bronze;
                case ItemResourceType.Cupper:
                    return SpriteName.WarsResource_Cupper;
                case ItemResourceType.CupperOre:
                    return SpriteName.WarsResource_CupperOre;
                case ItemResourceType.Silver:
                    return SpriteName.WarsResource_Silver;
                case ItemResourceType.SilverOre:
                    return SpriteName.WarsResource_SilverOre;
                case ItemResourceType.Mithril:
                    return SpriteName.WarsResource_MithrilAlloy;
                case ItemResourceType.RawMithril:
                    return SpriteName.WarsResource_Mithril;

                case ItemResourceType.BronzeSword:
                    return SpriteName.WarsResource_BronzeSword;
                case ItemResourceType.ShortSword:
                    return SpriteName.WarsResource_ShortSword;
                case ItemResourceType.LongSword:
                    return SpriteName.WarsResource_Longsword;
                case ItemResourceType.HandSpear:
                    return SpriteName.WarsResource_HandSpear;
                case ItemResourceType.Warhammer:
                    return SpriteName.WarsResource_Warhammer;
                case ItemResourceType.MithrilSword:
                    return SpriteName.WarsResource_MithrilSword;
                case ItemResourceType.SlingShot:
                    return SpriteName.WarsResource_Slingshot;
                case ItemResourceType.ThrowingSpear:
                    return SpriteName.WarsResource_ThrowSpear;
                case ItemResourceType.MithrilBow:
                    return SpriteName.WarsResource_Mithrilbow;

                case ItemResourceType.Toolkit:
                    return SpriteName.WarsResource_Toolkit;

                case ItemResourceType.Sulfur:
                    return SpriteName.WarsResource_Sulfur;
                case ItemResourceType.LeadOre:
                    return SpriteName.WarsResource_LeadOre;
                case ItemResourceType.Lead:
                    return SpriteName.WarsResource_Lead;
                case ItemResourceType.BloomeryIron:
                    return SpriteName.WarsResource_BloomeryIron;
                case ItemResourceType.Steel:
                    return SpriteName.WarsResource_Steel;
                case ItemResourceType.CastIron:
                    return SpriteName.WarsResource_CastIron;

                case ItemResourceType.BlackPowder:
                    return SpriteName.WarsResource_BlackPowder;
                case ItemResourceType.GunPowder:
                    return SpriteName.WarsResource_GunPowder;
                case ItemResourceType.LedBullet:
                    return SpriteName.WarsResource_Bullets;

                case ItemResourceType.HandCannon:
                    return SpriteName.WarsResource_BronzeRifle;
                case ItemResourceType.HandCulverin:
                    return SpriteName.WarsResource_BronzeShotgun;
                case ItemResourceType.Rifle:
                    return SpriteName.WarsResource_IronRifle;
                case ItemResourceType.Blunderbus:
                    return SpriteName.WarsResource_IronShotgun;

                case ItemResourceType.Manuballista:
                    return SpriteName.WarsResource_Manuballista;
                case ItemResourceType.Catapult:
                    return SpriteName.WarsResource_Catapult;
                case ItemResourceType.SiegeCannonBronze:
                    return SpriteName.WarsResource_BronzeSiegeCannon;
                case ItemResourceType.ManCannonBronze:
                    return SpriteName.WarsResource_BronzeManCannon;
                case ItemResourceType.SiegeCannonIron:
                    return SpriteName.WarsResource_IronSiegeCannon;
                case ItemResourceType.ManCannonIron:
                    return SpriteName.WarsResource_IronManCannon;

                case ItemResourceType.HeavyPaddedArmor:
                    return SpriteName.WarsResource_HeavyPaddedArmor;

               
                case ItemResourceType.BronzeArmor:
                    return SpriteName.WarsResource_BronzeArmor;

                case ItemResourceType.LightPlateArmor:
                    return SpriteName.WarsResource_LightPlateArmor;
                case ItemResourceType.FullPlateArmor:
                    return SpriteName.WarsResource_FullPlateArmor;

                case ItemResourceType.MithrilArmor:
                    return SpriteName.WarsResource_MithrilArmor;



                default:
                    return SpriteName.NO_IMAGE;
            }
        }

        
    }

    enum ResourceType
    {
        Gold,
        Worker,
        DiplomaticPoint,
        Item,
        MercenaryOnMarket,
        NUM
    }
}
