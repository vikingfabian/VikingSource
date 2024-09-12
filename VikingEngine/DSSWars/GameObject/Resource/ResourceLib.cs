using System;
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
        public static readonly CraftBlueprint CraftFood = new CraftBlueprint(DssRef.todoLang.Resource_TypeName_Food, 40) { useWater = 5, useWood = 5, useRawFood = 1 };
        public static readonly CraftBlueprint CraftIron = new CraftBlueprint(DssRef.todoLang.Resource_TypeName_Iron, 4) { useWater = 1, useWood = 20, useOre = 2 };
        public static readonly CraftBlueprint CraftWorkerHut = new CraftBlueprint(DssRef.todoLang.BuildingType_WorkerHut, 1) { useWater = 0, useWood = 200, useStone = 40 };
        public static readonly CraftBlueprint CraftTavern = new CraftBlueprint(DssRef.todoLang.BuildingType_Tavern, 1) { useWater = 0, useWood = 100, useStone = 20 };
        public static readonly CraftBlueprint CraftPigPen = new CraftBlueprint(DssRef.todoLang.BuildingType_PigPen, 1) { useWater = 4, useWood = 20, useRawFood = PigRawFoodAmout };
        public static readonly CraftBlueprint CraftHenPen = new CraftBlueprint(DssRef.todoLang.BuildingType_HenPen, 1) { useWater = 2, useWood = 20, useRawFood = DefaultItemRawFoodAmout };

        public const int PigRawFoodAmout = 50;
        public const int DefaultItemRawFoodAmout = 10;
        public const float ManDefaultEnergyCost = 1f;
        public const float WorkTeamEnergyCost = ManDefaultEnergyCost * City.WorkTeamSize;
        public const float WorkTeamEnergyCost_WhenIdle = WorkTeamEnergyCost * 0.5f;
        public const int FoodEnergy = 100;
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

        public static CraftBlueprint Blueprint(Map.TerrainBuildingType buildingType)
        {
            switch (buildingType)
            {
                case Map.TerrainBuildingType.HenPen: return CraftHenPen;
                case Map.TerrainBuildingType.PigPen: return CraftPigPen;
                case Map.TerrainBuildingType.Tavern: return CraftTavern;
                case Map.TerrainBuildingType.WorkerHut: return CraftWorkerHut;

                    default: throw new NotImplementedException();
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
