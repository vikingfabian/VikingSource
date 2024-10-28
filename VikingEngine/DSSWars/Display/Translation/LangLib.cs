using System;
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
                case ArmorLevel.Light: return DssRef.lang.Resource_TypeName_LightArmor;
                case ArmorLevel.Medium: return DssRef.lang.Resource_TypeName_MediumArmor;
                case ArmorLevel.Heavy: return DssRef.lang.Resource_TypeName_HeavyArmor;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string Weapon(MainWeapon weapon)
        {
            switch (weapon)
            {
                case MainWeapon.SharpStick: return DssRef.lang.Resource_TypeName_SharpStick;
                case MainWeapon.Sword: return DssRef.lang.Resource_TypeName_Sword;
                case MainWeapon.TwoHandSword: return DssRef.lang.Resource_TypeName_TwoHandSword;
                case MainWeapon.KnightsLance: return DssRef.lang.Resource_TypeName_KnightsLance;
                case MainWeapon.Bow: return DssRef.lang.Resource_TypeName_Bow;
                case MainWeapon.Longbow: return DssRef.todoLang.Resource_TypeName_Longbow;
                case MainWeapon.Ballista: return DssRef.lang.UnitType_Ballista;

                default:
                    return TextLib.Error;
            }
        }

        public static string Training(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssRef.lang.Conscript_Training_Minimal;
                case TrainingLevel.Basic: return DssRef.lang.Conscript_Training_Basic;
                case TrainingLevel.Skillful: return DssRef.lang.Conscript_Training_Skillful;
                case TrainingLevel.Professional: return DssRef.lang.Conscript_Training_Professional;

                default:
                    throw new NotImplementedException();
            }
        }

        public static string SpecializationTypeName(SpecializationType specialization)
        {
            switch (specialization)
            {
                case SpecializationType.None: return DssRef.lang.Hud_None;
                case SpecializationType.Field: return DssRef.lang.Conscript_Specialization_Field;
                case SpecializationType.Sea:
                    return DssRef.lang.Conscript_Specialization_Sea;
                case SpecializationType.Siege:
                    return DssRef.lang.Conscript_Specialization_Siege;
                case SpecializationType.Viking:
                    return DssRef.lang.UnitType_Viking;
                case SpecializationType.HonorGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case SpecializationType.Green: return DssRef.lang.UnitType_GreenSoldier;
                case SpecializationType.Traditional:
                    return DssRef.lang.Conscript_Specialization_Traditional;
                case SpecializationType.AntiCavalry:
                    return DssRef.lang.Conscript_Specialization_AntiCavalry;

                default:
                    return TextLib.Error;
            }
        }


       

        public static string Tab(MenuTab tab, out string description)
        {
            switch (tab)
            {
                case MenuTab.Info:
                    description = null;
                    return DssRef.lang.MenuTab_Info;
                case MenuTab.Build:
                    description = DssRef.lang.MenuTab_Build_Description;
                    return DssRef.lang.MenuTab_Build;
                case MenuTab.Conscript:
                    description = DssRef.lang.BuildingType_Barracks_Description;
                    return DssRef.lang.Conscription_Title;
                case MenuTab.Trade:
                    description = null;
                    return DssRef.lang.MenuTab_Trade;
                case MenuTab.BlackMarket:
                    description = DssRef.lang.MenuTab_BlackMarket_Description;
                    return DssRef.lang.Hud_BlackMarket;
                case MenuTab.Economy:
                    description = null;
                    return DssRef.lang.MenuTab_Economy;
                case MenuTab.Delivery:
                    description = DssRef.lang.BuildingType_Postal_Description;
                    return DssRef.lang.MenuTab_Delivery;
                case MenuTab.Resources:
                    description = DssRef.lang.MenuTab_Resources_Description;
                    return DssRef.lang.MenuTab_Resources;
                case MenuTab.Work:
                    description = DssRef.lang.MenuTab_Work_Description;
                    return DssRef.lang.MenuTab_Work;
                case MenuTab.Automation:
                    description = DssRef.lang.MenuTab_Automation_Description;
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
        //             return DssRef.lang.BuildingType_Barracks;

        //        case BuildAndExpandType.Brewery:
        //            return DssRef.lang.BuildingType_Brewery;
        //        case BuildAndExpandType.Carpenter:
        //            return DssRef.lang.BuildingType_Carpenter;
        //        case BuildAndExpandType.CoalPit:
        //            return DssRef.lang.BuildingType_CoalPit;
        //        case BuildAndExpandType.Cook:
        //            return DssRef.lang.BuildingType_Cook;
        //        case BuildAndExpandType.HenPen:
        //            return DssRef.lang.BuildingType_HenPen;
        //        case BuildAndExpandType.LinenFarm:
        //            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen);
        //        case BuildAndExpandType.Nobelhouse:
        //            return DssRef.lang.Building_NobleHouse;
        //        case BuildAndExpandType.Pavement:
        //            return DssRef.lang.DecorType_Pavement + " A";
        //        case BuildAndExpandType.PavementFlower:
        //            return DssRef.lang.DecorType_Pavement + " B";
        //        case BuildAndExpandType.PigPen:
        //            return DssRef.lang.BuildingType_PigPen;
        //        case BuildAndExpandType.Postal:
        //            return DssRef.lang.BuildingType_Postal;
        //        case BuildAndExpandType.Recruitment:
        //            return DssRef.lang.BuildingType_Recruitment;
        //        case BuildAndExpandType.Smith:
        //            return DssRef.lang.BuildingType_Smith;
        //        case BuildAndExpandType.Statue_ThePlayer:
        //            return DssRef.lang.DecorType_Statue;
        //        case BuildAndExpandType.Tavern:
        //            return DssRef.lang.BuildingType_Tavern;
        //        case BuildAndExpandType.WheatFarm:
        //            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat);
        //        case BuildAndExpandType.WorkBench:
        //            return DssRef.lang.BuildingType_WorkBench;
        //        case BuildAndExpandType.WorkerHuts:
        //            return DssRef.lang.BuildingType_WorkerHut;

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
                            return DssRef.lang.BuildingType_Barracks;
                        case TerrainBuildingType.Bank:
                            return DssRef.todoLang.BuildingType_Bank;
                        case TerrainBuildingType.Brewery:
                            return DssRef.lang.BuildingType_Brewery;
                        case TerrainBuildingType.Carpenter:
                            return DssRef.lang.BuildingType_Carpenter;
                        case TerrainBuildingType.Work_CoalPit:
                            return DssRef.lang.BuildingType_CoalPit;
                        case TerrainBuildingType.Work_Cook:
                            return DssRef.lang.BuildingType_Cook;
                        case TerrainBuildingType.HenPen:
                            return DssRef.lang.BuildingType_HenPen;
                        case TerrainBuildingType.Nobelhouse:
                            return DssRef.lang.Building_NobleHouse;
                        case TerrainBuildingType.PigPen:
                            return DssRef.lang.BuildingType_PigPen;
                        case TerrainBuildingType.Postal:
                            return DssRef.lang.BuildingType_Postal;
                        case TerrainBuildingType.Recruitment:
                            return DssRef.lang.BuildingType_Recruitment;
                        case TerrainBuildingType.Work_Smith:
                            return DssRef.lang.BuildingType_Smith;                       
                        case TerrainBuildingType.Storehouse:
                            return DssRef.lang.BuildingType_Storage;
                        case TerrainBuildingType.Tavern:
                            return DssRef.lang.BuildingType_Tavern;
                        case TerrainBuildingType.Work_Bench:
                            return DssRef.lang.BuildingType_WorkBench;
                        case TerrainBuildingType.WorkerHut:
                            return DssRef.lang.BuildingType_WorkerHut;

                        default:
                            return DssRef.lang.BuildingType_DefaultName;
                    }

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subType)
                    {
                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            return DssRef.lang.Resource_TypeName_Wood;

                        case TerrainSubFoilType.StoneBlock:
                        case TerrainSubFoilType.Stones:
                            return DssRef.lang.Resource_TypeName_Stone;

                        case TerrainSubFoilType.LinenFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen);

                        case TerrainSubFoilType.WheatFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat);

                        case TerrainSubFoilType.RapeSeedFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Rapeseed);
                        case TerrainSubFoilType.HempFarm:
                            return string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.todoLang.Resource_TypeName_Hemp);
                    }
                    break;

                case TerrainMainType.Mine:
                    switch ((TerrainMineType)subType)
                    {
                        case TerrainMineType.IronOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron);
                        case TerrainMineType.Coal:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Coal);
                        case TerrainMineType.GoldOre:
                            return string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.ResourceType_Gold);
                        
                    }
                    break;

                case TerrainMainType.Decor:
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Pavement:
                            return DssRef.lang.DecorType_Pavement + " A";
                        case TerrainDecorType.PavementFlower:
                            return DssRef.lang.DecorType_Pavement + " B";
                        case TerrainDecorType.Statue_ThePlayer:
                            return DssRef.lang.DecorType_Statue;
                    }
                    break;
            }


            return TextLib.Error + " (" + mainType.ToString() + " " + subType.ToString()+ ")";
        }

        public static string BuildingDescription(TerrainBuildingType buildingType)
        {
            switch (buildingType)
            { 
                case TerrainBuildingType.Tavern:
                    return DssRef.lang.BuildingType_Tavern_Description;
                case TerrainBuildingType.Storehouse:
                    return DssRef.todoLang.BuildingType_Storehourse_Description;
                case TerrainBuildingType.PigPen:
                    return DssRef.lang.BuildingType_PigPen_Description;
                case TerrainBuildingType.HenPen:
                    return DssRef.lang.BuildingType_HenPen_Description;
                case TerrainBuildingType.WorkerHut:
                    return string.Format(DssRef.lang.BuildingType_WorkerHut_DescriptionLimitX, GameObject.CityDetail.WorkersPerHut);
                case TerrainBuildingType.Postal:
                    return DssRef.lang.BuildingType_Postal_Description;
                case TerrainBuildingType.Recruitment:
                    return DssRef.lang.BuildingType_Recruitment_Description;
                case TerrainBuildingType.Barracks:
                    return DssRef.lang.BuildingType_Barracks_Description;
                case TerrainBuildingType.Work_Cook:
                    return DssRef.lang.BuildingType_Cook_Description;
                case TerrainBuildingType.Work_Bench:
                    return DssRef.lang.BuildingType_Bench_Description;
                case TerrainBuildingType.Work_Smith:
                    return DssRef.lang.BuildingType_Smith_Description;
                case TerrainBuildingType.Carpenter:
                    return DssRef.lang.BuildingType_Carpenter_Description;
                case TerrainBuildingType.Nobelhouse:
                    return DssRef.lang.BuildingType_Nobelhouse_Description;
                case TerrainBuildingType.Brewery:
                    return DssRef.lang.BuildingType_Tavern_Brewery;
                case TerrainBuildingType.Work_CoalPit:
                    return DssRef.lang.BuildingType_CoalPit_Description;
                case TerrainBuildingType.Bank:
                    return DssRef.todoLang.BuildingType_Bank_Description;

                default:
                    return TextLib.Error;
            }
        }


        public static string Item(ItemResourceType item)
        {
            switch (item)
            {
                case ItemResourceType.GoldOre: return DssRef.lang.Resource_TypeName_GoldOre;
                case ItemResourceType.Gold: return DssRef.lang.ResourceType_Gold;

                case ItemResourceType.Water_G: return DssRef.lang.Resource_TypeName_Water;
                case ItemResourceType.Beer: return DssRef.lang.Resource_TypeName_Beer;
                case ItemResourceType.IronOre_G: return DssRef.lang.Resource_TypeName_IronOre;
                case ItemResourceType.Iron_G: return DssRef.lang.Resource_TypeName_Iron;
                case ItemResourceType.Food_G: return DssRef.lang.Resource_TypeName_Food;
                case ItemResourceType.Stone_G: return DssRef.lang.Resource_TypeName_Stone;

                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group: 
                    return DssRef.lang.Resource_TypeName_Wood;

                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G: 
                    return DssRef.lang.Resource_TypeName_Fuel;
                
                case ItemResourceType.Pig:
                case ItemResourceType.Hen:
                case ItemResourceType.Egg:
                case ItemResourceType.Wheat:
                case ItemResourceType.RawFood_Group:
                    return DssRef.lang.Resource_TypeName_RawFood;

                case ItemResourceType.Linen:
                case ItemResourceType.SkinLinen_Group: 
                    return DssRef.lang.Resource_TypeName_Linen;

                case ItemResourceType.Rapeseed:
                    return DssRef.todoLang.Resource_TypeName_Rapeseed;
                case ItemResourceType.Hemp:
                    return DssRef.todoLang.Resource_TypeName_Hemp;

                case ItemResourceType.SharpStick:
                    return DssRef.lang.Resource_TypeName_SharpStick;
                case ItemResourceType.Sword:
                    return DssRef.lang.Resource_TypeName_Sword;
                case ItemResourceType.TwoHandSword:
                    return DssRef.lang.Resource_TypeName_TwoHandSword;
                case ItemResourceType.KnightsLance:
                    return DssRef.lang.Resource_TypeName_KnightsLance;
                case ItemResourceType.Bow:
                    return DssRef.lang.Resource_TypeName_Bow;
                case ItemResourceType.LongBow:
                    return DssRef.todoLang.Resource_TypeName_Longbow;
                case ItemResourceType.Ballista:
                    return DssRef.lang.UnitType_Ballista;

                case ItemResourceType.LightArmor:
                    return DssRef.lang.Resource_TypeName_LightArmor;
                case ItemResourceType.MediumArmor:
                    return DssRef.lang.Resource_TypeName_MediumArmor;
                case ItemResourceType.HeavyArmor:
                    return DssRef.lang.Resource_TypeName_HeavyArmor;

                default:
                    return TextLib.Error;
            }
        }

        public static string CityCulture(CityCulture cityCulture, bool title)
        {
            switch (cityCulture)
            {
                case DSSWars.CityCulture.AnimalBreeder: return title ? DssRef.lang.CityCulture_AnimalBreeder : DssRef.lang.CityCulture_AnimalBreeder_Description;
                case DSSWars.CityCulture.Archers: return title ? DssRef.lang.CityCulture_Archers : DssRef.lang.CityCulture_Archers_Description;
                case DSSWars.CityCulture.Builders: return title ? DssRef.lang.CityCulture_Builders : DssRef.lang.CityCulture_Builders_Description;
                case DSSWars.CityCulture.CrabMentality: return title ? DssRef.lang.CityCulture_CrabMentality : DssRef.lang.CityCulture_CrabMentality_Description;
                case DSSWars.CityCulture.DeepWell: return title ? DssRef.lang.CityCulture_DeepWell : DssRef.lang.CityCulture_DeepWell_Description;
                case DSSWars.CityCulture.FertileGround: return title ? DssRef.lang.CityCulture_FertileGround : DssRef.lang.CityCulture_FertileGround_Description;
                case DSSWars.CityCulture.LargeFamilies: return title ? DssRef.lang.CityCulture_LargeFamilies : DssRef.lang.CityCulture_LargeFamilies_Description;
                case DSSWars.CityCulture.Miners: return title ? DssRef.lang.CityCulture_Miners : DssRef.lang.CityCulture_Miners_Description;
                case DSSWars.CityCulture.Warriors: return title ? DssRef.lang.CityCulture_Warriors : DssRef.lang.CityCulture_Warriors_Description;
                case DSSWars.CityCulture.Woodcutters: return title ? DssRef.lang.CityCulture_Woodcutters : DssRef.lang.CityCulture_Woodcutters_Description;
                case DSSWars.CityCulture.Networker: return title ? DssRef.lang.CityCulture_Networker : DssRef.lang.CityCulture_Networker_Description;
                case DSSWars.CityCulture.PitMasters: return title ? DssRef.lang.CityCulture_PitMasters : DssRef.lang.CityCulture_PitMasters_Description;


                default: return TextLib.Error;
            }
        }

        public static string UnitFilterName(UnitFilterType filterType)
        {
            switch (filterType)
            {
                case UnitFilterType.SharpStick:
                    return DssRef.lang.Resource_TypeName_SharpStick;
                case UnitFilterType.Sword:
                    return DssRef.lang.Resource_TypeName_Sword;
                case UnitFilterType.TwohandSword:
                    return DssRef.lang.UnitType_FootKnight;
                case UnitFilterType.Knight:
                    return DssRef.lang.UnitType_CavalryKnight;
                case UnitFilterType.Bow:
                    return DssRef.lang.UnitType_Archer;
                case UnitFilterType.Ballista:
                    return DssRef.lang.UnitType_Ballista;

                case UnitFilterType.GreenSoldier:
                    return DssRef.lang.FactionName_Greenwood;
                case UnitFilterType.HonourGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case UnitFilterType.Viking:
                    return DssRef.lang.UnitType_Viking;

                default:
                    return TextLib.Error;
            }
        }
    }
}
