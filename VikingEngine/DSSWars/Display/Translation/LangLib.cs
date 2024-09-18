using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Display.Translation
{
    static class LangLib
    {
        public static string Armor(ArmorLevel level)
        {
            switch (level)
            {
                case ArmorLevel.None:
                    return "None";
                case ArmorLevel.Light: return DssRef.todoLang.Resource_TypeName_LightArmor;
                case ArmorLevel.Medium: return DssRef.todoLang.Resource_TypeName_MediumArmor;
                case ArmorLevel.Heavy: return DssRef.todoLang.Resource_TypeName_HeavyArmor;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string Weapon(MainWeapon weapon)
        {
            switch (weapon)
            {
                case MainWeapon.SharpStick: return DssRef.todoLang.Resource_TypeName_SharpStick;
                case MainWeapon.Sword: return DssRef.todoLang.Resource_TypeName_Sword;
                case MainWeapon.Bow: return DssRef.todoLang.Resource_TypeName_Bow;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string Training(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return "Minimal";
                case TrainingLevel.Basic: return "Basic";
                case TrainingLevel.Skillful: return "Skillful";
                case TrainingLevel.Professional: return "Professional";


                default:
                    throw new NotImplementedException();
            }
        }

        public static string Tab(MenuTab tab)
        {
            switch (tab)
            {
                case MenuTab.Info:
                    return DssRef.todoLang.MenuTab_Info;
                case MenuTab.Build:
                    return DssRef.todoLang.MenuTab_Build;
                case MenuTab.Conscript:
                    return DssRef.todoLang.Conscription_Title;
                case MenuTab.Recruit:
                    return DssRef.todoLang.MenuTab_Recruit;
                case MenuTab.Trade:
                    return DssRef.todoLang.MenuTab_Trade;
                case MenuTab.Economy:
                    return DssRef.todoLang.MenuTab_Economy;
                case MenuTab.Delivery:
                    return DssRef.todoLang.MenuTab_Delivery;
                case MenuTab.Resources:
                    return DssRef.todoLang.MenuTab_Resources;
                case MenuTab.Work:
                    return DssRef.todoLang.MenuTab_Work;
                case MenuTab.Automation:
                    return DssRef.lang.Automation_Title;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string BuildingDescription(TerrainBuildingType buildingType)
        {
            switch (buildingType)
            { 
                case TerrainBuildingType.Tavern:
                    return DssRef.todoLang.BuildingType_Tavern_Description;
                case TerrainBuildingType.PigPen:
                    return DssRef.todoLang.BuildingType_PigPen_Description;
                case TerrainBuildingType.HenPen:
                    return DssRef.todoLang.BuildingType_HenPen_Description;
                case TerrainBuildingType.WorkerHut:
                    return string.Format(DssRef.todoLang.BuildingType_WorkerHut_DescriptionLimitX, GameObject.CityDetail.WorkersPerHut);
                case TerrainBuildingType.Postal:
                    return DssRef.todoLang.BuildingType_Postal_Description;
                case TerrainBuildingType.Recruitment:
                    return DssRef.todoLang.BuildingType_Recruitment_Description;
                case TerrainBuildingType.Barracks:
                    return DssRef.todoLang.BuildingType_Barracks_Description;

                default:
                    throw new NotImplementedException();
            }
        }


        public static string Item(ItemResourceType item)
        {
            switch (item)
            {
                case ItemResourceType.Water_G: return DssRef.todoLang.Resource_TypeName_Water;
                case ItemResourceType.IronOre_G: return DssRef.todoLang.Resource_TypeName_Ore;
                case ItemResourceType.Iron_G: return DssRef.todoLang.Resource_TypeName_Iron;
                case ItemResourceType.Food_G: return DssRef.todoLang.Resource_TypeName_Food;
                case ItemResourceType.Stone_G: return DssRef.todoLang.Resource_TypeName_Stone;
                case ItemResourceType.Wood_Group: return DssRef.todoLang.Resource_TypeName_Wood;
                case ItemResourceType.RawFood_Group: return DssRef.todoLang.Resource_TypeName_RawFood;
                case ItemResourceType.SkinLinnen_Group: return DssRef.todoLang.Resource_TypeName_SkinAndLinnen;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
