using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    static class ResourceLib
    {
        public static readonly CraftBlueprint CraftFuel1 = new CraftBlueprint(  
            CraftResultType.Resource,
            (int)ItemResourceType.Fuel_G,
            5,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 5),
            }
        );

        public static readonly CraftBlueprint CraftCharcoal = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Fuel_G,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Fuel_G, 10),
                new UseResource(ItemResourceType.Wood_Group, 10),
            }, CraftRequirement.CoalPit
        );

        public static readonly CraftBlueprint CraftFood1 = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Food_G,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.Fuel_G, 5),
                new UseResource(ItemResourceType.RawFood_Group, 25)
            }
        ) { tooltipId = Tooltip.Food_BlueprintId };

        public static readonly CraftBlueprint CraftFood2 = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Food_G,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Beer, 5),
                new UseResource(ItemResourceType.Fuel_G, 5),
                new UseResource(ItemResourceType.RawFood_Group, 25)
            }
        ) { tooltipId = Tooltip.Food_BlueprintId };

    public static readonly CraftBlueprint CraftBeer = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Beer,
           10,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.Fuel_G, 1),
                new UseResource(ItemResourceType.RawFood_Group, 1)
           }, 
           CraftRequirement.Brewery
       );

        public static readonly CraftBlueprint CraftIron = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Iron_G,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 1),
                new UseResource(ItemResourceType.Fuel_G, 30),
                new UseResource(ItemResourceType.IronOre_G, 2)
            },
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftSharpStick = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.SharpStick,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 1),
                new UseResource(ItemResourceType.Stone_G, 1),
            }
        );

        public static readonly CraftBlueprint CraftSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Sword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Iron_G, 3),
            },
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftKnightsLance = new CraftBlueprint(
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
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftTwoHandSword = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.TwoHandSword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinen_Group, 1),
                new UseResource(ItemResourceType.Iron_G, 6),
            },
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftBow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Bow,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 2),
                new UseResource(ItemResourceType.SkinLinen_Group, 2),
            }
        );
        public static readonly CraftBlueprint CraftLongBow = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Bow,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 4),
                new UseResource(ItemResourceType.SkinLinen_Group, 3),
            }, CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint CraftBallista = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Ballista,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 8),
                new UseResource(ItemResourceType.SkinLinen_Group, 4),
                new UseResource(ItemResourceType.Iron_G, 1),
            },
            CraftRequirement.Carpenter
        );

        public static readonly CraftBlueprint CraftLightArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.LightArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
            }
        );

        public static readonly CraftBlueprint CraftMediumArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.MediumArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 4),
        new UseResource(ItemResourceType.Iron_G, 2),
            },
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftHeavyArmor = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.HeavyArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinen_Group, 2),
        new UseResource(ItemResourceType.Iron_G, 6),
            },
            CraftRequirement.Smith
        );

        public static readonly CraftBlueprint CraftWorkerHut = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkerHuts,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 200),
        new UseResource(ItemResourceType.Stone_G, 40)
            }
        );

        public static readonly CraftBlueprint CraftTavern = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Tavern,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 20)
            }
        );

        public static readonly CraftBlueprint CraftStorehouse = new CraftBlueprint(
           CraftResultType.Building,
           (int)Build.BuildAndExpandType.Storehouse,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Stone_G, 40)
           }
       );

        public static readonly CraftBlueprint CraftBrewery = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Brewery,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Iron_G, 5)
           }
       );

        public static readonly CraftBlueprint CraftPostal = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Postal,
           1,
           new UseResource[]
           {
        new UseResource(ItemResourceType.Wood_Group, 60),
           }
       ); 

        public static readonly CraftBlueprint CraftRecruitment = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Recruitment,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 50),
        new UseResource(ItemResourceType.SkinLinen_Group, 10)
            }
        );

        public static readonly CraftBlueprint CraftBarracks = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Barracks,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 100),
        new UseResource(ItemResourceType.Stone_G, 20)
            }
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
            }
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
            }
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
            FarmResources
        );

        public static readonly CraftBlueprint CraftLinenFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.LinenFarm,
            1,
            FarmResources
        );

        public static readonly CraftBlueprint CraftHempFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.HempFarm,
            1,
            FarmResources
        );

        public static readonly CraftBlueprint CraftRapeseedFarm = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.RapeSeedFarm,
            1,
            FarmResources
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
           }
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
           }
        );

        public static readonly CraftBlueprint CraftWorkBench = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.WorkBench,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, 2),
           }
        );

        public static readonly CraftBlueprint CraftCoalPit = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.CoalPit,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 30),
           }
        );

        public static readonly CraftBlueprint CraftCarpenter = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Carpenter,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 20),
               new UseResource(ItemResourceType.Iron_G, 8),
           }
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
            }
        );

        public static readonly CraftBlueprint CraftPavement = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Pavement,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 20),
           }
       );
        public static readonly CraftBlueprint CraftPavementFlower = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.PavementFlower,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.RawFood_Group, 5),
               new UseResource(ItemResourceType.Stone_G, 20),
           }
       );

        public static readonly CraftBlueprint CraftStatue = new CraftBlueprint(
            CraftResultType.Building,
            (int)Build.BuildAndExpandType.Statue_ThePlayer,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 500),
               new UseResource(ItemResourceType.Iron_G, 50),
           }
       );


        public static readonly CraftBlueprint ConvertGoldOre = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.GoldOre, 1),
           }
       );

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
                case ItemResourceType.Egg:
                    return SpriteName.WarsResource_Egg;
                case ItemResourceType.Food_G:
                    return SpriteName.WarsResource_Food;
                case ItemResourceType.GoldOre:
                    return SpriteName.WarsResource_GoldOre;
                case ItemResourceType.HeavyArmor:
                    return SpriteName.WarsResource_HeavyArmor;
                case ItemResourceType.Iron_G:
                    return SpriteName.WarsResource_Iron;
                case ItemResourceType.IronOre_G:
                    return SpriteName.WarsResource_IronOre;
                case ItemResourceType.MediumArmor:
                    return SpriteName.WarsResource_MediumArmor;
                case ItemResourceType.LightArmor:
                    return SpriteName.WarsResource_LightArmor;
                case ItemResourceType.Linen:
                    return SpriteName.WarsResource_Linen;
                case ItemResourceType.LongBow:
                    return SpriteName.WarsResource_Longbow;
                case ItemResourceType.Hen:
                case ItemResourceType.Pig:
                    return SpriteName.WarsResource_RawMeat;
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

                default:
                    return SpriteName.NO_IMAGE;
            }
        }

        public static void Blueprint(ItemResourceType item, out CraftBlueprint bp1, out CraftBlueprint bp2)
        {
            switch (item)
            {
                case ItemResourceType.Fuel_G: bp1 = CraftFuel1; bp2 = null; break;
                case ItemResourceType.Coal: bp1 = CraftCharcoal; bp2 = null; break;
                case ItemResourceType.Food_G: bp1 = CraftFood1; bp2 = CraftFood2; break;
                case ItemResourceType.Beer: bp1 = CraftBeer; bp2 = null; break;

                case ItemResourceType.Iron_G: bp1 = CraftIron; bp2 = null; break;
                case ItemResourceType.LightArmor: bp1 = CraftLightArmor; bp2 = null; break;
                case ItemResourceType.MediumArmor: bp1 = CraftMediumArmor; bp2 = null; break;
                case ItemResourceType.HeavyArmor: bp1 = CraftHeavyArmor; bp2 = null; break;

                case ItemResourceType.SharpStick: bp1 = CraftSharpStick; bp2 = null; break;
                case ItemResourceType.Sword: bp1 = CraftSword; bp2 = null; break;
                case ItemResourceType.TwoHandSword: bp1 = CraftTwoHandSword; bp2 = null; break;
                case ItemResourceType.KnightsLance: bp1 = CraftKnightsLance; bp2 = null; break;
                case ItemResourceType.Bow: bp1 = CraftBow; bp2 = null; break;
                case ItemResourceType.LongBow: bp1 = CraftLongBow; bp2 = null; break;
                case ItemResourceType.Ballista: bp1 = CraftBallista; bp2 = null; break;

                default: throw new NotImplementedException();
            }
        }
        //public static CraftBlueprint Blueprint(Map.TerrainBuildingType buildingType)
        //{
        //    switch (buildingType)
        //    {
        //        case Map.TerrainBuildingType.HenPen: return CraftHenPen;
        //        case Map.TerrainBuildingType.PigPen: return CraftPigPen;
        //        case Map.TerrainBuildingType.Tavern: return CraftTavern;
        //        case Map.TerrainBuildingType.Barracks: return CraftBarracks;
        //        case Map.TerrainBuildingType.WorkerHut: return CraftWorkerHut;

        //            default: throw new NotImplementedException();
        //    }              
        //}
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
