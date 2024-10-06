﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.LootFest.GO;

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
                case MainWeapon.TwoHandSword: return DssRef.todoLang.Resource_TypeName_TwoHandSword;
                case MainWeapon.KnightsLance: return DssRef.todoLang.Resource_TypeName_KnightsLance;
                case MainWeapon.Bow: return DssRef.todoLang.Resource_TypeName_Bow;
                case MainWeapon.Ballista: return DssRef.lang.UnitType_Ballista;

                default:
                    return TextLib.Error;
            }
        }

        public static string Training(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssRef.todoLang.Conscript_Training_Minimal;
                case TrainingLevel.Basic: return DssRef.todoLang.Conscript_Training_Basic;
                case TrainingLevel.Skillful: return DssRef.todoLang.Conscript_Training_Skillful;
                case TrainingLevel.Professional: return DssRef.todoLang.Conscript_Training_Professional;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string SpecializationTypeName(SpecializationType specialization)
        {
            switch (specialization)
            {
                case SpecializationType.None: return DssRef.todoLang.Hud_None;
                case SpecializationType.Field: return DssRef.todoLang.Conscript_Specialization_Field;
                case SpecializationType.Sea:
                    return DssRef.todoLang.Conscript_Specialization_Sea;
                case SpecializationType.Siege:
                    return DssRef.todoLang.Conscript_Specialization_Siege;
                case SpecializationType.Viking:
                    return DssRef.lang.UnitType_Viking;
                case SpecializationType.HonorGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case SpecializationType.Green: return DssRef.lang.UnitType_GreenSoldier;
                case SpecializationType.Traditional:
                    return DssRef.todoLang.Conscript_Specialization_Traditional;
                case SpecializationType.AntiCavalry:
                    return DssRef.todoLang.Conscript_Specialization_AntiCavalry;

                default:
                    return TextLib.Error;
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
                case MenuTab.BlackMarket:
                    return DssRef.todoLang.Hud_BlackMarket;
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

        //public static string BuildingName(BuildAndExpandType buildingType)
        //{
        //    switch (buildingType)
        //    {
        //        case BuildAndExpandType.Barracks:
        //             return DssRef.todoLang.BuildingType_Barracks;

        //        case BuildAndExpandType.Brewery:
        //            return DssRef.todoLang.BuildingType_Brewery;
        //        case BuildAndExpandType.Carpenter:
        //            return DssRef.todoLang.BuildingType_Carpenter;
        //        case BuildAndExpandType.CoalPit:
        //            return DssRef.todoLang.BuildingType_CoalPit;
        //        case BuildAndExpandType.Cook:
        //            return DssRef.todoLang.BuildingType_Cook;
        //        case BuildAndExpandType.HenPen:
        //            return DssRef.todoLang.BuildingType_HenPen;
        //        case BuildAndExpandType.LinenFarm:
        //            return string.Format(DssRef.todoLang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Linen);
        //        case BuildAndExpandType.Nobelhouse:
        //            return DssRef.lang.Building_NobleHouse;
        //        case BuildAndExpandType.Pavement:
        //            return DssRef.todoLang.DecorType_Pavement + " A";
        //        case BuildAndExpandType.PavementFlower:
        //            return DssRef.todoLang.DecorType_Pavement + " B";
        //        case BuildAndExpandType.PigPen:
        //            return DssRef.todoLang.BuildingType_PigPen;
        //        case BuildAndExpandType.Postal:
        //            return DssRef.todoLang.BuildingType_Postal;
        //        case BuildAndExpandType.Recruitment:
        //            return DssRef.todoLang.BuildingType_Recruitment;
        //        case BuildAndExpandType.Smith:
        //            return DssRef.todoLang.BuildingType_Smith;
        //        case BuildAndExpandType.Statue_ThePlayer:
        //            return DssRef.todoLang.DecorType_Statue;
        //        case BuildAndExpandType.Tavern:
        //            return DssRef.todoLang.BuildingType_Tavern;
        //        case BuildAndExpandType.WheatFarm:
        //            return string.Format(DssRef.todoLang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Wheat);
        //        case BuildAndExpandType.WorkBench:
        //            return DssRef.todoLang.BuildingType_WorkBench;
        //        case BuildAndExpandType.WorkerHuts:
        //            return DssRef.todoLang.BuildingType_WorkerHut;

        //        default:
        //            return TextLib.Error;
        //    }
        //}

        public static string TerrainName(TerrainMainType mainType, int subType)
        {
            switch (mainType)
            {
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subType)
                    { 
                        case TerrainBuildingType.Barracks:
                            return DssRef.todoLang.BuildingType_Barracks;
                        case TerrainBuildingType.Brewery:
                            return DssRef.todoLang.BuildingType_Brewery;
                        case TerrainBuildingType.Carpenter:
                            return DssRef.todoLang.BuildingType_Carpenter;
                        case TerrainBuildingType.Work_CoalPit:
                            return DssRef.todoLang.BuildingType_CoalPit;
                        case TerrainBuildingType.Work_Cook:
                            return DssRef.todoLang.BuildingType_Cook;
                        case TerrainBuildingType.HenPen:
                            return DssRef.todoLang.BuildingType_HenPen;
                        case TerrainBuildingType.Nobelhouse:
                            return DssRef.lang.Building_NobleHouse;
                        case TerrainBuildingType.PigPen:
                            return DssRef.todoLang.BuildingType_PigPen;
                        case TerrainBuildingType.Postal:
                            return DssRef.todoLang.BuildingType_Postal;
                        case TerrainBuildingType.Recruitment:
                            return DssRef.todoLang.BuildingType_Recruitment;
                        case TerrainBuildingType.Work_Smith:
                            return DssRef.todoLang.BuildingType_Smith;
                        case TerrainBuildingType.Tavern:
                            return DssRef.todoLang.BuildingType_Tavern;
                        case TerrainBuildingType.Work_Bench:
                            return DssRef.todoLang.BuildingType_WorkBench;
                        case TerrainBuildingType.WorkerHut:
                            return DssRef.todoLang.BuildingType_WorkerHut;

                        default:
                            return DssRef.todoLang.BuildingType_DefaultName;
                    }

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subType)
                    {
                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            return DssRef.todoLang.Resource_TypeName_Wood;

                        case TerrainSubFoilType.StoneBlock:
                        case TerrainSubFoilType.Stones:
                            return DssRef.todoLang.Resource_TypeName_Stone;

                        case TerrainSubFoilType.LinenFarm:
                            return string.Format(DssRef.todoLang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Linen);

                        case TerrainSubFoilType.WheatFarm:
                            return string.Format(DssRef.todoLang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Wheat);
                    }
                    break;

                case TerrainMainType.Decor:
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Pavement:
                            return DssRef.todoLang.DecorType_Pavement + " A";
                        case TerrainDecorType.PavementFlower:
                            return DssRef.todoLang.DecorType_Pavement + " B";
                        case TerrainDecorType.Statue_ThePlayer:
                            return DssRef.todoLang.DecorType_Statue;
                    }
                    break;
            }


            return TextLib.Error;
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
                case TerrainBuildingType.Work_Cook:
                    return DssRef.todoLang.BuildingType_Cook_Description;
                case TerrainBuildingType.Work_Bench:
                    return DssRef.todoLang.BuildingType_Bench_Description;
                case TerrainBuildingType.Work_Smith:
                    return DssRef.todoLang.BuildingType_Smith_Description;
                case TerrainBuildingType.Carpenter:
                    return DssRef.todoLang.BuildingType_Carpenter_Description;
                case TerrainBuildingType.Nobelhouse:
                    return DssRef.todoLang.BuildingType_Nobelhouse_Description;
                case TerrainBuildingType.Brewery:
                    return DssRef.todoLang.BuildingType_Tavern_Brewery;
                case TerrainBuildingType.Work_CoalPit:
                    return DssRef.todoLang.BuildingType_CoalPit_Description;

                default:
                    return TextLib.Error;
            }
        }


        public static string Item(ItemResourceType item)
        {
            switch (item)
            {
                case ItemResourceType.GoldOre: return DssRef.todoLang.Resource_TypeName_GoldOre;
                case ItemResourceType.Gold: return DssRef.lang.ResourceType_Gold;

                case ItemResourceType.Water_G: return DssRef.todoLang.Resource_TypeName_Water;
                case ItemResourceType.Beer: return DssRef.todoLang.Resource_TypeName_Beer;
                case ItemResourceType.IronOre_G: return DssRef.todoLang.Resource_TypeName_IronOre;
                case ItemResourceType.Iron_G: return DssRef.todoLang.Resource_TypeName_Iron;
                case ItemResourceType.Food_G: return DssRef.todoLang.Resource_TypeName_Food;
                case ItemResourceType.Stone_G: return DssRef.todoLang.Resource_TypeName_Stone;
                case ItemResourceType.Wood_Group: return DssRef.todoLang.Resource_TypeName_Wood;
                case ItemResourceType.Fuel_G: return DssRef.todoLang.Resource_TypeName_Fuel;
                case ItemResourceType.RawFood_Group: return DssRef.todoLang.Resource_TypeName_RawFood;
                case ItemResourceType.SkinLinen_Group: return DssRef.todoLang.Resource_TypeName_SkinAndLinen;

                case ItemResourceType.SharpStick:
                    return DssRef.todoLang.Resource_TypeName_SharpStick;
                case ItemResourceType.Sword:
                    return DssRef.todoLang.Resource_TypeName_Sword;
                case ItemResourceType.TwoHandSword:
                    return DssRef.todoLang.Resource_TypeName_TwoHandSword;
                case ItemResourceType.KnightsLance:
                    return DssRef.todoLang.Resource_TypeName_KnightsLance;
                case ItemResourceType.Bow:
                    return DssRef.todoLang.Resource_TypeName_Bow;
                case ItemResourceType.Ballista:
                    return DssRef.lang.UnitType_Ballista;

                case ItemResourceType.LightArmor:
                    return DssRef.todoLang.Resource_TypeName_LightArmor;
                case ItemResourceType.MediumArmor:
                    return DssRef.todoLang.Resource_TypeName_MediumArmor;
                case ItemResourceType.HeavyArmor:
                    return DssRef.todoLang.Resource_TypeName_HeavyArmor;

                default:
                    return TextLib.Error;
            }
        }

        public static string CityCulture(CityCulture cityCulture, bool title)
        {
            switch (cityCulture)
            {
                case DSSWars.CityCulture.AnimalBreeder: return title ? DssRef.todoLang.CityCulture_AnimalBreeder : DssRef.todoLang.CityCulture_AnimalBreeder_Description;
                case DSSWars.CityCulture.Archers: return title ? DssRef.todoLang.CityCulture_Archers : DssRef.todoLang.CityCulture_Archers_Description;
                case DSSWars.CityCulture.Builders: return title ? DssRef.todoLang.CityCulture_Builders : DssRef.todoLang.CityCulture_Builders_Description;
                case DSSWars.CityCulture.CrabMentality: return title ? DssRef.todoLang.CityCulture_CrabMentality : DssRef.todoLang.CityCulture_CrabMentality_Description;
                case DSSWars.CityCulture.DeepWell: return title ? DssRef.todoLang.CityCulture_DeepWell : DssRef.todoLang.CityCulture_DeepWell_Description;
                case DSSWars.CityCulture.FertileGround: return title ? DssRef.todoLang.CityCulture_FertileGround : DssRef.todoLang.CityCulture_FertileGround_Description;
                case DSSWars.CityCulture.LargeFamilies: return title ? DssRef.todoLang.CityCulture_LargeFamilies : DssRef.todoLang.CityCulture_LargeFamilies_Description;
                case DSSWars.CityCulture.Miners: return title ? DssRef.todoLang.CityCulture_Miners : DssRef.todoLang.CityCulture_Miners_Description;
                case DSSWars.CityCulture.Warriors: return title ? DssRef.todoLang.CityCulture_Warriors : DssRef.todoLang.CityCulture_Warriors_Description;
                case DSSWars.CityCulture.Woodcutters: return title ? DssRef.todoLang.CityCulture_Woodcutters : DssRef.todoLang.CityCulture_Woodcutters_Description;
                case DSSWars.CityCulture.Networker: return title ? DssRef.todoLang.CityCulture_Networker : DssRef.todoLang.CityCulture_Networker_Description;
                case DSSWars.CityCulture.PitMasters: return title ? DssRef.todoLang.CityCulture_PitMasters : DssRef.todoLang.CityCulture_PitMasters_Description;


                default: return TextLib.Error;
            }
        }
    }
}
