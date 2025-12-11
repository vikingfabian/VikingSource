using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class IconName
    {
        public static void CityCulture(CityCulture cityCulture, out string title, out string description)
        {
            switch (cityCulture)
            {
                case DSSWars.CityCulture.Archers:
                    title = DssRef.lang.CityCulture_Archers;
                    description = DssRef.lang.CityCulture_Archers_Description;
                    break;

                case DSSWars.CityCulture.Builders:
                    title = DssRef.lang.CityCulture_Builders;
                    description = DssRef.lang.CityCulture_Builders_Description;
                    break;

                case DSSWars.CityCulture.CrabMentality:
                    title = DssRef.lang.CityCulture_CrabMentality;
                    description = DssRef.lang.CityCulture_CrabMentality_Description;
                    break;

                case DSSWars.CityCulture.DeepWell:
                    title = DssRef.lang.CityCulture_DeepWell;
                    description = DssRef.lang.CityCulture_DeepWell_Description;
                    break;

                case DSSWars.CityCulture.FertileGround:
                    title = DssRef.lang.CityCulture_FertileGround;
                    description = DssRef.lang.CityCulture_FertileGround_Description;
                    break;

                case DSSWars.CityCulture.LargeFamilies:
                    title = DssRef.lang.CityCulture_LargeFamilies;
                    description = DssRef.lang.CityCulture_LargeFamilies_Description;
                    break;

                case DSSWars.CityCulture.Miners:
                    title = DssRef.lang.CityCulture_Miners;
                    description = DssRef.lang.CityCulture_Miners_Description;
                    break;

                case DSSWars.CityCulture.Warriors:
                    title = DssRef.lang.CityCulture_Warriors;
                    description = DssRef.lang.CityCulture_Warriors_Description;
                    break;

                case DSSWars.CityCulture.Woodcutters:
                    title = DssRef.lang.CityCulture_Woodcutters;
                    description = DssRef.lang.CityCulture_Woodcutters_Description;
                    break;

                case DSSWars.CityCulture.Networker:
                    title = DssRef.lang.CityCulture_Networker;
                    description = DssRef.lang.CityCulture_Networker_Description;
                    break;

                case DSSWars.CityCulture.PitMasters:
                    title = DssRef.lang.CityCulture_PitMasters;
                    description = DssRef.lang.CityCulture_PitMasters_Description;
                    break;

                case DSSWars.CityCulture.Stonemason:
                    title = DssRef.lang.CityCulture_Stonemason;
                    description = DssRef.lang.CityCulture_Stonemason_Description;
                    break;

                case DSSWars.CityCulture.Brewmaster:
                    title = DssRef.lang.CityCulture_Brewmaster;
                    description = DssRef.lang.CityCulture_Brewmaster_Description;
                    break;

                case DSSWars.CityCulture.Weavers:
                    title = DssRef.lang.CityCulture_Weavers;
                    description = DssRef.lang.CityCulture_Weavers_Description;
                    break;

                case DSSWars.CityCulture.SiegeEngineer:
                    title = DssRef.lang.CityCulture_SiegeEngineer;
                    description = DssRef.lang.CityCulture_SiegeEngineer_Description;
                    break;

                case DSSWars.CityCulture.Armorsmith:
                    title = DssRef.lang.CityCulture_Armorsmith;
                    description = DssRef.lang.CityCulture_Armorsmith_Description;
                    break;

                case DSSWars.CityCulture.Noblemen:
                    title = DssRef.lang.CityCulture_Noblemen;
                    description = DssRef.lang.CityCulture_Noblemen_Description;
                    break;

                case DSSWars.CityCulture.Seafaring:
                    title = DssRef.lang.CityCulture_Seafaring;
                    description = DssRef.lang.CityCulture_Seafaring_Description;
                    break;

                case DSSWars.CityCulture.Backtrader:
                    title = DssRef.lang.CityCulture_Backtrader;
                    description = DssRef.lang.CityCulture_Backtrader_Description;
                    break;

                case DSSWars.CityCulture.Lawbiding:
                    title = DssRef.lang.CityCulture_LawAbiding;
                    description = DssRef.lang.CityCulture_LawAbiding_Description;
                    break;

                case DSSWars.CityCulture.Smelters:
                    title = DssRef.lang.CityCulture_Smelters;
                    description = DssRef.lang.CityCulture_Smelters_Description;
                    break;

                case DSSWars.CityCulture.BronzeCasters:
                    title = DssRef.lang.CityCulture_BronzeCasters;
                    description = DssRef.lang.CityCulture_BronzeCasters_Description;
                    break;

                case DSSWars.CityCulture.Apprentices:
                    title = DssRef.lang.CityCulture_Apprentices;
                    description = DssRef.lang.CityCulture_Apprentices_Description;
                    break;

                case DSSWars.CityCulture.AnimalBreeder2:
                    title = DssRef.lang.CityCulture_AnimalBreeder;
                    description = DssRef.todoLang.CityCulture_AnimalBreeder2_Description;
                    break;

                case DSSWars.CityCulture.Butchers:
                    title = DssRef.todoLang.CityCulture_Butchers;
                    description = string.Format( DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Meat);
                    break;
               

                case DSSWars.CityCulture.Potters:
                    title = DssRef.todoLang.CityCulture_Potters;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.);
                    break;

                case DSSWars.CityCulture.Wainwright:
                    title = DssRef.todoLang.CityCulture_Wainwright;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Wagon);
                    break;

                case DSSWars.CityCulture.Wheelwright:
                    title = DssRef.todoLang.CityCulture_Wheelwright;
                    description = DssRef.todoLang.CityCulture_Wheelwright_Description;
                    break;

                case DSSWars.CityCulture.ShieldMaker:
                    title = DssRef.todoLang.CityCulture_ShieldMaker;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_Shield);
                    break;

                case DSSWars.CityCulture.Nomads:
                    title = DssRef.todoLang.CityCulture_Nomads;
                    description = DssRef.todoLang.CityCulture_Nomads_Description;
                    break;

                case DSSWars.CityCulture.Coopers:
                    title = DssRef.todoLang.CityCulture_Coopers;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_StorageBox);
                    break;

                case DSSWars.CityCulture.Salters:
                    title = DssRef.todoLang.CityCulture_Salters;
                    description = string.Format(DssRef.todoLang.CityCulture_EnhancedProduction, DssRef.todoLang.Resource_TypeName_ConservedFood);
                    break;

                default:
                    title = TextLib.Error;
                    description = TextLib.Error;
                    break;
            }
        }

        public static void BuildCategory(BuildCategoryTab tab, out SpriteName tabIcon, out string category)
        {
            //string category;
            //SpriteName tabIcon;
            switch (tab)
            {
                case BuildCategoryTab.Filter:
                    tabIcon = SpriteName.warsBuildCategorySearch;
                    category = DssRef.lang.HUD_Filter;
                    break;
                case BuildCategoryTab.General:
                    tabIcon = SpriteName.warsBuildCategoryHouse;
                    category = DssRef.lang.BuildCategory_General;
                    break;
                case BuildCategoryTab.Advanced:
                    tabIcon = SpriteName.warsBuildCategoryAdvanced;
                    category = DssRef.lang.Hud_Advanced;
                    break;
                case BuildCategoryTab.Military:
                    tabIcon = SpriteName.warsBuildCategoryMilitaryWall;
                    category = DssRef.lang.BuildCategory_Military;
                    break;
                case BuildCategoryTab.Decor:
                    tabIcon = SpriteName.warsBuildCategoryDecorTree;
                    category = DssRef.lang.BuildCategory_Decoration;
                    break;
                case BuildCategoryTab.Upgrade:
                    tabIcon = SpriteName.warsBuildCategoryUpgrades;
                    category = DssRef.lang.BuildCategory_Upgrade;
                    break;
                case BuildCategoryTab.GodPower:
                    tabIcon = SpriteName.WarsGodPowerIcon;
                    category = DssRef.lang.GodPower;
                    break;
                default:
                    tabIcon = SpriteName.warsBuildCategoryAutomation;
                    category = DssRef.lang.Automation_Title;
                    break;
            }
        }

        public static void Tab(ResourcesSubTab tab, out SpriteName categoryIcon, out string category, out SpriteName tabIcon, out string tabName)
        {
            switch (tab)
            {
                case ResourcesSubTab.Overview_Resources:
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_Wood;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.WarsResourceGroup_Resources;
                    break;
                case ResourcesSubTab.Overview_Metals:
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_Iron;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.WarsResourceGroup_Metal;
                    break;
                case ResourcesSubTab.Overview_Weapons:
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_Sword;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
                    break;
                case ResourcesSubTab.Overview_Projectile:
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_Bow;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
                    break;
                case ResourcesSubTab.Overview_Armor:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    tabIcon = SpriteName.WarsResource_IronArmor;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.Conscript_ArmorTitle;
                    break;

                case ResourcesSubTab.Stockpile_Resources:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_Wood;
                    category = DssRef.lang.Resource_Tab_Stockpile;
                    tabName = DssRef.lang.WarsResourceGroup_Resources;
                    break;
                case ResourcesSubTab.Stockpile_Metals:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    tabIcon = SpriteName.WarsResource_Iron;
                    category = DssRef.lang.Resource_Tab_Stockpile;
                    tabName = DssRef.lang.WarsResourceGroup_Metal;
                    break;
                case ResourcesSubTab.Stockpile_Weapons:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    tabIcon = SpriteName.WarsResource_Sword;
                    category = DssRef.lang.Resource_Tab_Stockpile;
                    tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
                    break;
                case ResourcesSubTab.Stockpile_Projectile:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    tabIcon = SpriteName.WarsResource_Bow;
                    category = DssRef.lang.Resource_Tab_Stockpile;
                    tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
                    break;
                case ResourcesSubTab.Stockpile_Armor:
                    categoryIcon = SpriteName.WarsStockpileAdd;
                    tabIcon = SpriteName.WarsResource_IronArmor;
                    category = DssRef.lang.Resource_Tab_Stockpile;
                    tabName = DssRef.lang.Conscript_ArmorTitle;
                    break;

                case ResourcesSubTab.Work_Resources:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_Wood;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.WarsResourceGroup_Resources;
                    break;
                case ResourcesSubTab.Work_Metals:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_Iron;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.WarsResourceGroup_Metal;
                    break;
                case ResourcesSubTab.Work_Weapons:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_Sword;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
                    break;
                case ResourcesSubTab.Work_Projectile:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_Bow;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
                    break;
                case ResourcesSubTab.Work_Armor:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_IronArmor;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.Conscript_ArmorTitle;
                    break;

                case ResourcesSubTab.Work_Mint:
                    categoryIcon = SpriteName.WarsHammer;
                    tabIcon = SpriteName.WarsResource_SilverCoin;
                    category = DssRef.lang.MenuTab_Work;
                    tabName = DssRef.lang.BuildingType_CoinMaker;
                    break;

                default:
                    categoryIcon = SpriteName.MissingImage;
                    tabIcon = SpriteName.MissingImage;
                    category = TextLib.Error;
                    tabName = TextLib.Error;
                    break;
            }
        }

        public static void Building(BuildAndExpandType buildingType, out SpriteName icon, out string name)
        {
            var build = BuildLib.BuildOptions[(int)buildingType];
            icon = build.sprite;
            Terrain(build.terrainType.mainTerrain, build.terrainType.subTerrain, out _, out name);
        }

        public static void Terrain(TerrainMainType mainType, int subType, out SpriteName icon, out string name)
        {
            icon = SpriteName.NO_IMAGE;
            name = null;

            switch (mainType)
            {
                case TerrainMainType.Building:
                    switch ((TerrainBuildingType)subType)
                    {
                        case TerrainBuildingType.Logistics:
                            name = DssRef.lang.BuildingType_Logistics;
                            break;
                        case TerrainBuildingType.SoldierBarracks:
                            name = DssRef.lang.BuildingType_SoldierBarracks;
                            break;
                        case TerrainBuildingType.Bank:
                            name = DssRef.lang.BuildingType_Bank;
                            break;
                        case TerrainBuildingType.CoinMinter:
                            name = DssRef.lang.BuildingType_CoinMaker;
                            break;
                        case TerrainBuildingType.Brewery:
                            name = DssRef.lang.BuildingType_Brewery;
                            break;
                        case TerrainBuildingType.Carpenter:
                            name = DssRef.lang.BuildingType_Carpenter;
                            break;
                        case TerrainBuildingType.Work_CoalPit:
                            name = DssRef.lang.BuildingType_CoalPit;
                            break;
                        case TerrainBuildingType.Work_Cook:
                            name = DssRef.lang.BuildingType_Cook;
                            break;
                        case TerrainBuildingType.HenPen:
                            name = DssRef.lang.BuildingType_HenPen;
                            break;
                        case TerrainBuildingType.Nobelhouse:
                            name = DssRef.lang.Building_NobleHouse;
                            break;
                        case TerrainBuildingType.ImmigrationTent:
                            name = DssRef.lang.BuildingType_ImmigrationTent;
                            break;
                        case TerrainBuildingType.PigPen:
                            name = DssRef.lang.BuildingType_PigPen;
                            break;

                        case TerrainBuildingType.Postal:
                            name = DssRef.lang.BuildingType_Postal;
                            break;
                        case TerrainBuildingType.PostalLevel2:
                        case TerrainBuildingType.PostalLevel3:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Postal);
                            break;

                        case TerrainBuildingType.GoldDeliveryLevel1:
                            name = DssRef.lang.BuildingType_GoldDelivery;
                            break;
                        case TerrainBuildingType.GoldDeliveryLevel2:
                        case TerrainBuildingType.GoldDeliveryLevel3:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_GoldDelivery);
                            break;

                        case TerrainBuildingType.Recruitment:
                            name = DssRef.lang.BuildingType_Recruitment;
                            break;
                        case TerrainBuildingType.RecruitmentLevel2:
                        case TerrainBuildingType.RecruitmentLevel3:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded, DssRef.lang.BuildingType_Recruitment);
                            break;

                        case TerrainBuildingType.Work_Smith:
                            name = DssRef.lang.BuildingType_Smith;
                            break;
                        case TerrainBuildingType.Storehouse:
                            name = DssRef.lang.BuildingType_Storage;
                            break;
                        case TerrainBuildingType.Tavern:
                            name = DssRef.lang.BuildingType_Tavern;
                            break;
                        case TerrainBuildingType.Work_Bench:
                            name = DssRef.lang.BuildingType_WorkBench;
                            break;
                        case TerrainBuildingType.WorkerHut:
                        case TerrainBuildingType.WorkerHutLarge:
                            name = DssRef.lang.BuildingType_WorkerHut;
                            break;

                        case TerrainBuildingType.Smelter:
                            name = DssRef.lang.BuildingType_SmeltingFurnace;
                            break;
                        case TerrainBuildingType.WoodCutter:
                            name = DssRef.lang.BuildingType_WoodCutter;
                            break;
                        case TerrainBuildingType.StoneCutter:
                            name = DssRef.lang.BuildingType_StoneCutter;
                            break;
                        case TerrainBuildingType.Embassy:
                            name = DssRef.lang.BuildingType_Embassy;
                            break;
                        case TerrainBuildingType.WaterResovoir:
                            name = DssRef.lang.BuildingType_WaterResovoir;
                            break;

                        case TerrainBuildingType.GuardHouse_Small:
                        case TerrainBuildingType.GuardHouse_Large:
                            name = DssRef.lang.BuildingType_GuardOffice;
                            break;

                        case TerrainBuildingType.ArcherBarracks:
                            name = DssRef.lang.BuildingType_ArcherBarracks;
                            break;
                        case TerrainBuildingType.WarmachineBarracks:
                            name = DssRef.lang.BuildingType_WarmachineBarracks;
                            break;
                        case TerrainBuildingType.GunBarracks:
                            name = DssRef.lang.BuildingType_GunBarracks;
                            break;
                        case TerrainBuildingType.CannonBarracks:
                            name = DssRef.lang.BuildingType_CannonBarracks;
                            break;
                        case TerrainBuildingType.KnightsBarracks:
                            name = DssRef.lang.BuildingType_KnightsBarracks;
                            break;

                        case TerrainBuildingType.Foundry:
                            name = DssRef.lang.BuildingType_Foundry;
                            break;
                        case TerrainBuildingType.Armory:
                            name = DssRef.lang.BuildingType_Armory;
                            break;
                        case TerrainBuildingType.Chemist:
                            name = DssRef.lang.BuildingType_Chemist;
                            break;
                        case TerrainBuildingType.Gunmaker:
                            name = DssRef.lang.BuildingType_Gunmaker;
                            break;
                        case TerrainBuildingType.School:
                            name = DssRef.lang.BuildingType_School;
                            break;
                        case TerrainBuildingType.ResearchCenter:
                            name = DssRef.lang.BuildingType_ReseachCenter;
                            break;
                        case TerrainBuildingType.BookPress:
                            name = DssRef.lang.BuildingType_Bookpress;
                            break;

                        case TerrainBuildingType.ServiceMenHouse_small:
                        case TerrainBuildingType.ServiceMenHouse_Large:
                            name = DssRef.lang.BuildingType_ServiceHouse;
                            break;

                        default:
                            name = DssRef.lang.BuildingType_DefaultName;
                            break;
                    }
                    break;

                case TerrainMainType.Foil:
                    switch ((TerrainSubFoilType)subType)
                    {
                        default:
                            name = DssRef.lang.LandType_Flatland;
                            break;

                        case TerrainSubFoilType.TreeHard:
                        case TerrainSubFoilType.TreeSoft:
                        case TerrainSubFoilType.DryWood:
                            icon = SpriteName.WarsBuild_TreeSoft;
                            name = DssRef.lang.Resource_TypeName_Wood;
                            break;

                        case TerrainSubFoilType.TreeSoftSprout:
                            name = DssRef.lang.Building_TreeSprout_Soft;
                            break;
                        case TerrainSubFoilType.TreeHardSprout:
                            name = DssRef.lang.Building_TreeSprout_Hard;
                            break;

                        case TerrainSubFoilType.StoneBlock:
                        case TerrainSubFoilType.Stones:
                            icon = SpriteName.WarsResource_Stone;
                            name = DssRef.lang.Resource_TypeName_Stone;
                            break;

                        case TerrainSubFoilType.LinenFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen);
                            break;
                        case TerrainSubFoilType.LinenFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case TerrainSubFoilType.WheatFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat);
                            break;
                        case TerrainSubFoilType.WheatFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat));
                            break;

                        case TerrainSubFoilType.RapeSeedFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed);
                            break;
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Rapeseed));
                            break;

                        case TerrainSubFoilType.HempFarm:
                            name = string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp);
                            break;
                        case TerrainSubFoilType.HempFarmUpgraded:
                            name = string.Format(DssRef.lang.BuildingType_IsUpgraded,
                                string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Hemp));
                            break;

                        case TerrainSubFoilType.BogIron:
                            name = DssRef.lang.Resource_TypeName_BogIron;
                            break;
                    }
                    break;

                case TerrainMainType.Mine:
                    icon = SpriteName.WarsWorkMine;
                    switch ((TerrainMineType)subType)
                    {
                        case TerrainMineType.IronOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron);
                            break;
                        case TerrainMineType.Coal:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Coal);
                            break;
                        case TerrainMineType.GoldOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.ResourceType_Gold);
                            break;

                        case TerrainMineType.TinOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Tin);
                            break;
                        case TerrainMineType.CopperOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Copper);
                            break;
                        case TerrainMineType.SilverOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Silver);
                            break;
                        case TerrainMineType.LeadOre:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Lead);
                            break;
                        case TerrainMineType.Mithril:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Mithril);
                            break;
                        case TerrainMineType.Sulfur:
                            name = string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Sulfur);
                            break;
                    }
                    break;

                case TerrainMainType.Road:
                    switch ((TerrainRoadType)subType)
                    {
                        case TerrainRoadType.DirtRoad:
                            name = DssRef.lang.BuildingType_DirtRoad;
                            break;
                    }
                    break;

                case TerrainMainType.Decor:
                    switch ((TerrainDecorType)subType)
                    {
                        case TerrainDecorType.Pavement:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementFlower:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementRectFlower:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavementLamp:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Pavement);
                            break;
                        case TerrainDecorType.PavemenFountain:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Pavement);
                            break;

                        case TerrainDecorType.Statue_ThePlayer:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Statue);
                            break;

                        case TerrainDecorType.GardenFourBushes:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenLongTree:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenWalledBush:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenGrass:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Garden);
                            break;
                        case TerrainDecorType.GardenBird:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Garden);
                            break;

                        case TerrainDecorType.GardenMemoryStone:
                            name = string.Format(DssRef.lang.VariantType_F, DssRef.lang.DecorType_Garden);
                            break;

                        case TerrainDecorType.Statue_Leader:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Lion:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Horse:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Statue);
                            break;
                        case TerrainDecorType.Statue_Pillar:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Statue);
                            break;

                        case TerrainDecorType.FlagPole_LongBanner:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Banner);
                            break;
                        case TerrainDecorType.FlagPole_Banner:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Banner);
                            break;
                        case TerrainDecorType.FlagPole_SlimBanner:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Banner);
                            break;

                        case TerrainDecorType.FlagPole_Flag:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_FlagRound:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_FlagLarge:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_Streamer:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.DecorType_Flag);
                            break;
                        case TerrainDecorType.FlagPole_Triangle:
                            name = string.Format(DssRef.lang.VariantType_E, DssRef.lang.DecorType_Flag);
                            break;

                        case TerrainDecorType.CobbleStones:
                            name = DssRef.lang.DecorType_CobbleStones;
                            break;
                        case TerrainDecorType.Square:
                            name = DssRef.lang.DecorType_Square;
                            break;
                    }
                    break;

                case TerrainMainType.Destroyed:
                case TerrainMainType.DefaultLand:
                    name = DssRef.lang.LandType_Flatland;
                    break;

                case TerrainMainType.DefaultSea:
                    name = DssRef.lang.LandType_Water;
                    break;

                case TerrainMainType.Resourses:
                    name = DssRef.lang.Resource;
                    break;

                case TerrainMainType.Wall:
                    switch ((TerrainWallType)subType)
                    {
                        case TerrainWallType.Palisade:
                            name = DssRef.lang.BuildingType_Palisade;
                            break;
                        case TerrainWallType.DirtWall:
                            name = DssRef.lang.BuildingType_DirtWall;
                            break;
                        case TerrainWallType.DirtTower:
                            name = DssRef.lang.BuildingType_DirtTower;
                            break;
                        case TerrainWallType.WoodWall:
                            name = DssRef.lang.BuildingType_WoodWall;
                            break;
                        case TerrainWallType.WoodTower:
                            name = DssRef.lang.BuildingType_WoodTower;
                            break;
                        case TerrainWallType.StoneWall:
                            name = string.Format(DssRef.lang.VariantType_A, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneTower:
                            name = DssRef.lang.BuildingType_StoneTower;
                            break;
                        case TerrainWallType.StoneWallGreen:
                            name = string.Format(DssRef.lang.VariantType_B, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneWallBlueRoof:
                            name = string.Format(DssRef.lang.VariantType_C, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneWallWoodHouse:
                            name = string.Format(DssRef.lang.VariantType_D, DssRef.lang.BuildingType_StoneWall);
                            break;
                        case TerrainWallType.StoneGate:
                            name = DssRef.lang.BuildingType_StoneGate;
                            break;
                        case TerrainWallType.StoneHouse:
                            name = DssRef.lang.BuildingType_StoneHouse;
                            break;

                        default:
                            name = DssRef.lang.BuildingType_Wall;
                            break;
                    }
                    break;
            }

            if (name == null)
            {
                name = $"UNKNOWN ({mainType} {subType})";
            }
        }

    }
}
