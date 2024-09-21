﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    static class ResourceLib
    {
        public static readonly CraftBlueprint CraftFood1 = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_Food,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.Wood_Group, 5),
                new UseResource(ItemResourceType.RawFood_Group, 25)
            }
        );

        public static readonly CraftBlueprint CraftFood2 = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_Food,
            25,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Beer, 5),
                new UseResource(ItemResourceType.Wood_Group, 5),
                new UseResource(ItemResourceType.RawFood_Group, 25)
            }
        );

        public static readonly CraftBlueprint CraftBeer = new CraftBlueprint(
           DssRef.todoLang.Resource_TypeName_Beer,
           10,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Water_G, 5),
                new UseResource(ItemResourceType.Wood_Group, 1),
                new UseResource(ItemResourceType.RawFood_Group, 1)
           }
       );

        public static readonly CraftBlueprint CraftIron = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_Iron,
            4,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Water_G, 1),
                new UseResource(ItemResourceType.Wood_Group, 20),
                new UseResource(ItemResourceType.IronOre_G, 2)
            }
        );

        public static readonly CraftBlueprint CraftSharpStick = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_SharpStick,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 1),
                new UseResource(ItemResourceType.Stone_G, 1),
            }
        );

        public static readonly CraftBlueprint CraftSword = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_Sword,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.SkinLinnen_Group, 1),
                new UseResource(ItemResourceType.Iron_G, 3),
            }
        );

        public static readonly CraftBlueprint CraftBow = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_Bow,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 2),
                new UseResource(ItemResourceType.SkinLinnen_Group, 2),
            }
        );

        public static readonly CraftBlueprint CraftLightArmor = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_LightArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinnen_Group, 4),
            }
        );

        public static readonly CraftBlueprint CraftMediumArmor = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_MediumArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinnen_Group, 4),
        new UseResource(ItemResourceType.Iron_G, 2),
            }
        );

        public static readonly CraftBlueprint CraftHeavyArmor = new CraftBlueprint(
            DssRef.todoLang.Resource_TypeName_HeavyArmor,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.SkinLinnen_Group, 2),
        new UseResource(ItemResourceType.Iron_G, 6),
            }
        );

        public static readonly CraftBlueprint CraftWorkerHut = new CraftBlueprint(
            DssRef.todoLang.BuildingType_WorkerHut,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 200),
        new UseResource(ItemResourceType.Stone_G, 40)
            }
        );

        public static readonly CraftBlueprint CraftTavern = new CraftBlueprint(
            DssRef.todoLang.BuildingType_Tavern,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Wood_Group, 100),
                new UseResource(ItemResourceType.Stone_G, 20)
            }
        );

        public static readonly CraftBlueprint CraftBrewery = new CraftBlueprint(
           DssRef.todoLang.BuildingType_Brewery,
           1,
           new UseResource[]
           {
                new UseResource(ItemResourceType.Wood_Group, 60),
                new UseResource(ItemResourceType.Iron_G, 5)
           }
       );

        public static readonly CraftBlueprint CraftPostal = new CraftBlueprint(
           DssRef.todoLang.BuildingType_Postal,
           1,
           new UseResource[]
           {
        new UseResource(ItemResourceType.Wood_Group, 60),
           }
       ); 

        public static readonly CraftBlueprint CraftRecruitment = new CraftBlueprint(
            DssRef.todoLang.BuildingType_Recruitment,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 50),
        new UseResource(ItemResourceType.SkinLinnen_Group, 10)
            }
        );

        public static readonly CraftBlueprint CraftBarracks = new CraftBlueprint(
            DssRef.todoLang.BuildingType_Barracks,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Wood_Group, 100),
        new UseResource(ItemResourceType.Stone_G, 20)
            }
        );

        public static readonly CraftBlueprint CraftPigPen = new CraftBlueprint(
            DssRef.todoLang.BuildingType_PigPen,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Water_G, 4),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.PigRawFoodAmout)
            }
        );

        public static readonly CraftBlueprint CraftHenPen = new CraftBlueprint(
            DssRef.todoLang.BuildingType_HenPen,
            1,
            new UseResource[]
            {
        new UseResource(ItemResourceType.Water_G, 2),
        new UseResource(ItemResourceType.Wood_Group, 20),
        new UseResource(ItemResourceType.RawFood_Group, DssConst.DefaultItemRawFoodAmout)
            }
        );

        public static readonly CraftBlueprint CraftFarm = new CraftBlueprint(
            DssRef.todoLang.BuildingType_Barracks,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.RawFood_Group, 4),
                new UseResource(ItemResourceType.Water_G, 5),
            }
        );

        public static readonly CraftBlueprint CraftSmith = new CraftBlueprint(
           DssRef.todoLang.BuildingType_HenPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Iron_G, 10),
           }
       );

        public static readonly CraftBlueprint CraftCook = new CraftBlueprint(
           DssRef.todoLang.BuildingType_HenPen,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Wood_Group, 10),
               new UseResource(ItemResourceType.Stone_G, 10),
               new UseResource(ItemResourceType.Iron_G, 5),
           }
       );

        public static readonly CraftBlueprint CraftStatue = new CraftBlueprint(
           DssRef.todoLang.DecorType_Statue,
           1,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Stone_G, 500),
               new UseResource(ItemResourceType.Iron_G, 50),
           }
       );
        //public const int DefaultItemRawFoodAmout = 8;
        //public const int PigRawFoodAmout = 5 * DefaultItemRawFoodAmout;
        //public const float ManDefaultEnergyCost = 1f;
        //public const float WorkTeamEnergyCost = ManDefaultEnergyCost * City.WorkTeamSize;
        //public const float WorkTeamEnergyCost_WhenIdle = WorkTeamEnergyCost * 0.5f;
        //public const int FoodEnergy = 100;
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
                    return SpriteName.WarsWorkerSub;

                case ResourceType.DiplomaticPoint:
                    return SpriteName.WarsDiplomaticSub;

                case ResourceType.MercenaryOnMarket:
                    return SpriteName.WarsGroupIcon;

                default:
                    return SpriteName.NO_IMAGE;
            }
        }

        public static void Blueprint(ItemResourceType item, out CraftBlueprint bp1, out CraftBlueprint bp2)
        {
            switch (item)
            {
                case ItemResourceType.Food_G: bp1 = CraftFood1; bp2 = CraftFood2; break;
                case ItemResourceType.Beer: bp1 = CraftBeer; bp2 = null; break;

                case ItemResourceType.Iron_G: bp1 = CraftIron; bp2 = null; break;
                case ItemResourceType.LightArmor: bp1 = CraftLightArmor; bp2 = null; break;
                case ItemResourceType.MediumArmor: bp1 = CraftMediumArmor; bp2 = null; break;
                case ItemResourceType.HeavyArmor: bp1 = CraftHeavyArmor; bp2 = null; break;

                case ItemResourceType.SharpStick: bp1 = CraftSharpStick; bp2 = null; break;
                case ItemResourceType.Sword: bp1 = CraftSword; bp2 = null; break;
                case ItemResourceType.Bow: bp1 = CraftBow; bp2 = null; break;

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
