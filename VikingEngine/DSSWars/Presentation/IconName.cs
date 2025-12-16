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
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class IconName
    {
        //new
        public static void Item(ItemResourceType item, out SpriteName itemIcon, out string itemName)
        {
            switch (item)
            {
                // --- Economy & Workers ---
                case ItemResourceType.Gold:
                    itemIcon = SpriteName.rtsMoney;
                    itemName = DssRef.lang.ResourceType_Gold;
                    break;
                case ItemResourceType.Men:
                    itemIcon = SpriteName.WarsWorker;
                    itemName = DssRef.lang.ResourceType_Workers;
                    break;
                case ItemResourceType.ServiceMen:
                    itemIcon = SpriteName.WarsServiceMen;
                    itemName = DssRef.lang.ResourceType_ServiceMen;
                    break;
                case ItemResourceType.Settler:
                    itemIcon = SpriteName.WarsWorker; // Defaulting to worker icon
                    itemName = DssRef.lang.UnitType_Settler;
                    break;

                // --- Resources & Materials ---
                case ItemResourceType.Water_G:
                    itemIcon = SpriteName.WarsResource_Water;
                    itemName = DssRef.lang.Resource_TypeName_Water;
                    break;
                case ItemResourceType.Beer:
                    itemIcon = SpriteName.WarsResource_Beer;
                    itemName = DssRef.lang.Resource_TypeName_Beer;
                    break;
                case ItemResourceType.CoolingFluid:
                    itemIcon = SpriteName.WarsResource_CoolingFluid;
                    itemName = DssRef.lang.Resource_TypeName_CoolingFluid;
                    break;
                case ItemResourceType.Stone_G:
                    itemIcon = SpriteName.WarsResource_Stone;
                    itemName = DssRef.lang.Resource_TypeName_Stone;
                    break;
                case ItemResourceType.DryWood:
                case ItemResourceType.SoftWood:
                case ItemResourceType.HardWood:
                case ItemResourceType.Wood_Group:
                    itemIcon = SpriteName.WarsResource_Wood;
                    itemName = DssRef.lang.Resource_TypeName_Wood;
                    break;
                case ItemResourceType.Coal:
                case ItemResourceType.Fuel_G:
                    itemIcon = SpriteName.WarsResource_Fuel;
                    itemName = DssRef.lang.Resource_TypeName_Fuel;
                    break;
                case ItemResourceType.Sulfur:
                    itemIcon = SpriteName.WarsResource_Sulfur;
                    itemName = DssRef.lang.Resource_TypeName_Sulfur;
                    break;

                // --- Food & Agriculture ---
                case ItemResourceType.Food_G:
                    itemIcon = SpriteName.WarsResource_Food;
                    itemName = DssRef.lang.Resource_TypeName_Food;
                    break;
                case ItemResourceType.Hen:
                case ItemResourceType.Pig:
                    itemIcon = SpriteName.WarsResource_RawMeat;
                    itemName = DssRef.lang.Resource_TypeName_RawFood;
                    break;
                
                case ItemResourceType.Egg:
                case ItemResourceType.Wheat:
                case ItemResourceType.RawFood_Group:
                    itemIcon = SpriteName.WarsResource_RawFood;
                    itemName = DssRef.lang.Resource_TypeName_RawFood;
                    break;

                case ItemResourceType.Rapeseed:
                    itemIcon = SpriteName.WarsResource_Rapeseed;
                    itemName = DssRef.lang.Resource_TypeName_Rapeseed;
                    break;
                case ItemResourceType.Hemp:
                    itemIcon = SpriteName.WarsResource_Hemp;
                    itemName = DssRef.lang.Resource_TypeName_Hemp;
                    break;
                case ItemResourceType.Linen:
                case ItemResourceType.SkinLinen_Group:
                    itemIcon = SpriteName.WarsResource_Linen;
                    itemName = DssRef.lang.Resource_TypeName_Linen;
                    break;

                // --- Ores & Metals ---
                case ItemResourceType.GoldOre:
                    itemIcon = SpriteName.WarsResource_GoldOre;
                    itemName = DssRef.lang.Resource_TypeName_GoldOre;
                    break;
                case ItemResourceType.IronOre_G:
                    itemIcon = SpriteName.WarsResource_IronOre;
                    itemName = DssRef.lang.Resource_TypeName_IronOre;
                    break;
                case ItemResourceType.BogIron:
                    itemIcon = SpriteName.WarsBogIron;
                    itemName = DssRef.lang.Resource_TypeName_BogIron;
                    break;
                case ItemResourceType.TinOre:
                    itemIcon = SpriteName.WarsResource_TinOre;
                    itemName = DssRef.lang.Resource_TypeName_TinOre;
                    break;
                case ItemResourceType.CopperOre:
                    itemIcon = SpriteName.WarsResource_CopperOre;
                    itemName = DssRef.lang.Resource_TypeName_CopperOre;
                    break;
                case ItemResourceType.LeadOre:
                    itemIcon = SpriteName.WarsResource_LeadOre;
                    itemName = DssRef.lang.Resource_TypeName_LeadOre;
                    break;
                case ItemResourceType.SilverOre:
                    itemIcon = SpriteName.WarsResource_SilverOre;
                    itemName = DssRef.lang.Resource_TypeName_SilverOre;
                    break;

                // --- Refined Metals ---
                case ItemResourceType.Iron_G:
                    itemIcon = SpriteName.WarsResource_Iron;
                    itemName = DssRef.lang.Resource_TypeName_Iron;
                    break;
                case ItemResourceType.BloomeryIron:
                    itemIcon = SpriteName.WarsResource_BloomeryIron;
                    itemName = DssRef.lang.Resource_TypeName_BloomIron;
                    break;
                case ItemResourceType.CastIron:
                    itemIcon = SpriteName.WarsResource_CastIron;
                    itemName = DssRef.lang.Resource_TypeName_CastIron;
                    break;
                case ItemResourceType.Steel:
                    itemIcon = SpriteName.WarsResource_Steel;
                    itemName = DssRef.lang.Resource_TypeName_Steel;
                    break;
                case ItemResourceType.Tin:
                    itemIcon = SpriteName.WarsResource_Tin;
                    itemName = DssRef.lang.Resource_TypeName_Tin;
                    break;
                case ItemResourceType.Copper:
                    itemIcon = SpriteName.WarsResource_Copper;
                    itemName = DssRef.lang.Resource_TypeName_Copper;
                    break;
                case ItemResourceType.Bronze:
                    itemIcon = SpriteName.WarsResource_Bronze;
                    itemName = DssRef.lang.Resource_TypeName_Bronze;
                    break;
                case ItemResourceType.Silver:
                    itemIcon = SpriteName.WarsResource_Silver;
                    itemName = DssRef.lang.Resource_TypeName_Silver;
                    break;
                case ItemResourceType.Lead:
                    itemIcon = SpriteName.WarsResource_Lead;
                    itemName = DssRef.lang.Resource_TypeName_Lead;
                    break;
                case ItemResourceType.RawMithril:
                    itemIcon = SpriteName.WarsResource_Mithril;
                    itemName = DssRef.lang.Resource_TypeName_RawMithril;
                    break;
                case ItemResourceType.Mithril:
                    itemIcon = SpriteName.WarsResource_MithrilAlloy;
                    itemName = DssRef.lang.Resource_TypeName_Mithril;
                    break;

                // --- Weapons (Melee) ---
                case ItemResourceType.SharpStick:
                    itemIcon = SpriteName.WarsResource_Sharpstick;
                    itemName = DssRef.lang.Resource_TypeName_SharpStick;
                    break;
                case ItemResourceType.Sword:
                    itemIcon = SpriteName.WarsResource_Sword;
                    itemName = DssRef.lang.Resource_TypeName_Sword;
                    break;
                case ItemResourceType.BronzeSword:
                    itemIcon = SpriteName.WarsResource_BronzeSword;
                    itemName = DssRef.lang.Resource_TypeName_BronzeSword;
                    break;
                case ItemResourceType.ShortSword:
                    itemIcon = SpriteName.WarsResource_ShortSword;
                    itemName = DssRef.lang.Resource_TypeName_ShortSword;
                    break;
                case ItemResourceType.LongSword:
                    itemIcon = SpriteName.WarsResource_Longsword;
                    itemName = DssRef.lang.Resource_TypeName_LongSword;
                    break;
                case ItemResourceType.TwoHandSword:
                    itemIcon = SpriteName.WarsResource_TwoHandSword;
                    itemName = DssRef.lang.Resource_TypeName_TwoHandSword;
                    break;
                case ItemResourceType.MithrilSword:
                    itemIcon = SpriteName.WarsResource_MithrilSword;
                    itemName = DssRef.lang.Resource_TypeName_MithrilSword;
                    break;
                case ItemResourceType.HandSpear:
                    itemIcon = SpriteName.WarsResource_HandSpear;
                    itemName = DssRef.lang.Resource_TypeName_HandSpear;
                    break;
                case ItemResourceType.Pike:
                    itemIcon = SpriteName.WarsResource_Pike; // Assumed SpriteName
                    itemName = DssRef.lang.Resource_TypeName_Pike;
                    break;
                case ItemResourceType.Warhammer:
                    itemIcon = SpriteName.WarsResource_Warhammer;
                    itemName = DssRef.lang.Resource_TypeName_Warhammer;
                    break;
                case ItemResourceType.KnightsLance:
                    itemIcon = SpriteName.WarsResource_KnightsLance;
                    itemName = DssRef.lang.Resource_TypeName_KnightsLance;
                    break;

                // --- Weapons (Ranged) ---
                case ItemResourceType.SlingShot:
                    itemIcon = SpriteName.WarsResource_Slingshot;
                    itemName = DssRef.lang.Resource_TypeName_SlingShot;
                    break;
                case ItemResourceType.ThrowingSpear:
                    itemIcon = SpriteName.WarsResource_ThrowSpear;
                    itemName = DssRef.lang.Resource_TypeName_ThrowingSpear;
                    break;
                case ItemResourceType.Bow:
                    itemIcon = SpriteName.WarsResource_Bow;
                    itemName = DssRef.lang.Resource_TypeName_Bow;
                    break;
                case ItemResourceType.LongBow:
                    itemIcon = SpriteName.WarsResource_Longbow;
                    itemName = DssRef.lang.Resource_TypeName_Longbow;
                    break;
                case ItemResourceType.Crossbow:
                    itemIcon = SpriteName.WarsResource_Crossbow;
                    itemName = DssRef.lang.Resource_TypeName_Crossbow;
                    break;
                case ItemResourceType.MithrilBow:
                    itemIcon = SpriteName.WarsResource_Mithrilbow;
                    itemName = DssRef.lang.Resource_TypeName_MithrilBow;
                    break;

                // --- Siege & Components ---
                case ItemResourceType.Ballista:
                    itemIcon = SpriteName.WarsResource_Ballista;
                    itemName = DssRef.lang.UnitType_Ballista;
                    break;
                case ItemResourceType.Manuballista:
                    itemIcon = SpriteName.WarsResource_Manuballista;
                    itemName = DssRef.lang.Resource_TypeName_Manuballista;
                    break;
                case ItemResourceType.Catapult:
                    itemIcon = SpriteName.WarsResource_Catapult;
                    itemName = DssRef.lang.Resource_TypeName_Catapult;
                    break;
                case ItemResourceType.SiegeCannonBronze:
                    itemIcon = SpriteName.WarsResource_BronzeSiegeCannon;
                    itemName = DssRef.lang.Resource_TypeName_SiegeCannonBronze;
                    break;
                case ItemResourceType.ManCannonBronze:
                    itemIcon = SpriteName.WarsResource_BronzeManCannon;
                    itemName = DssRef.lang.Resource_TypeName_ManCannonBronze;
                    break;
                case ItemResourceType.SiegeCannonIron:
                    itemIcon = SpriteName.WarsResource_IronSiegeCannon;
                    itemName = DssRef.lang.Resource_TypeName_SiegeCannonIron;
                    break;
                case ItemResourceType.ManCannonIron:
                    itemIcon = SpriteName.WarsResource_IronManCannon;
                    itemName = DssRef.lang.Resource_TypeName_ManCannonIron;
                    break;
                case ItemResourceType.Palisade:
                    itemIcon = SpriteName.WarsResource_Palisade;
                    itemName = DssRef.lang.Resource_TypeName_Palisade;
                    break;
                case ItemResourceType.Toolkit:
                    itemIcon = SpriteName.WarsResource_Toolkit;
                    itemName = DssRef.lang.Resource_TypeName_Toolkit;
                    break;
                case ItemResourceType.Wagon2Wheel:
                    itemIcon = SpriteName.WarsResource_Wagon2Wheel;
                    itemName = DssRef.lang.Resource_TypeName_Wagon2Wheel;
                    break;
                case ItemResourceType.Wagon4Wheel:
                    itemIcon = SpriteName.WarsResource_Wagon4Wheel;
                    itemName = DssRef.lang.Resource_TypeName_Wagon4Wheel;
                    break;

                // --- Firearms & Explosives ---
                case ItemResourceType.BlackPowder:
                    itemIcon = SpriteName.WarsResource_BlackPowder;
                    itemName = DssRef.lang.Resource_TypeName_BlackPowder;
                    break;
                case ItemResourceType.GunPowder:
                    itemIcon = SpriteName.WarsResource_GunPowder;
                    itemName = DssRef.lang.Resource_TypeName_GunPowder;
                    break;
                case ItemResourceType.LedBullet:
                    itemIcon = SpriteName.WarsResource_Bullets;
                    itemName = DssRef.lang.Resource_TypeName_LedBullet;
                    break;
                case ItemResourceType.HandCannon:
                    itemIcon = SpriteName.WarsResource_BronzeRifle;
                    itemName = DssRef.lang.Resource_TypeName_HandCannon;
                    break;
                case ItemResourceType.HandCulverin:
                    itemIcon = SpriteName.WarsResource_BronzeShotgun;
                    itemName = DssRef.lang.Resource_TypeName_HandCulverin;
                    break;
                case ItemResourceType.Rifle:
                    itemIcon = SpriteName.WarsResource_IronRifle;
                    itemName = DssRef.lang.Resource_TypeName_Rifle;
                    break;
                case ItemResourceType.Blunderbuss:
                    itemIcon = SpriteName.WarsResource_IronShotgun;
                    itemName = DssRef.lang.Resource_TypeName_Blunderbuss;
                    break;

                // --- Armor ---
                case ItemResourceType.PaddedArmor:
                    itemIcon = SpriteName.WarsResource_PaddedArmor;
                    itemName = DssRef.lang.Resource_TypeName_PaddedArmor;
                    break;
                case ItemResourceType.HeavyPaddedArmor:
                    itemIcon = SpriteName.WarsResource_HeavyPaddedArmor;
                    itemName = DssRef.lang.Resource_TypeName_HeavyPaddedArmor;
                    break;
                case ItemResourceType.BronzeArmor:
                    itemIcon = SpriteName.WarsResource_BronzeArmor;
                    itemName = DssRef.lang.Resource_TypeName_BronzeArmor;
                    break;
                case ItemResourceType.IronArmor:
                    itemIcon = SpriteName.WarsResource_IronArmor;
                    itemName = DssRef.lang.Resource_TypeName_IronArmor;
                    break;
                case ItemResourceType.HeavyIronArmor:
                    itemIcon = SpriteName.WarsResource_HeavyIronArmor;
                    itemName = DssRef.lang.Resource_TypeName_HeavyIronArmor;
                    break;
                case ItemResourceType.LightPlateArmor:
                    itemIcon = SpriteName.WarsResource_LightPlateArmor;
                    itemName = DssRef.lang.Resource_TypeName_LightPlateArmor;
                    break;
                case ItemResourceType.FullPlateArmor:
                    itemIcon = SpriteName.WarsResource_FullPlateArmor;
                    itemName = DssRef.lang.Resource_TypeName_FullPlateArmor;
                    break;
                case ItemResourceType.MithrilArmor:
                    itemIcon = SpriteName.WarsResource_MithrilArmor;
                    itemName = DssRef.lang.Resource_TypeName_MithrilArmor;
                    break;

                // --- Currency ---
                case ItemResourceType.CopperCoin:
                    itemIcon = SpriteName.WarsResource_CopperCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.BronzeCoin:
                    itemIcon = SpriteName.WarsResource_BonzeCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.SilverCoin:
                    itemIcon = SpriteName.WarsResource_SilverCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;
                case ItemResourceType.ElfCoin:
                    itemIcon = SpriteName.WarsResource_ElfCoin;
                    itemName = DssRef.lang.Resource_TypeName_Coin;
                    break;

                // --- Misc ---
                case ItemResourceType.AutomatedItem:
                    itemIcon = SpriteName.AutomationGearIcon;
                    itemName = TextLib.Error; // No name provided in original
                    break;

                case ItemResourceType.NONE:
                    itemIcon = SpriteName.BluePrintSquareFull;
                    itemName = DssRef.lang.Hud_None;
                    break;

                default:
                    itemIcon = SpriteName.NO_IMAGE;
                    itemName = TextLib.Error;
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
                    categoryIcon = SpriteName.MenuPixelIconManual;
                    tabIcon = SpriteName.WarsResource_IronArmor;
                    category = DssRef.lang.Resource_Tab_Overview;
                    tabName = DssRef.lang.Conscript_ArmorTitle;
                    break;

                case ResourcesSubTab.Stockpile_Resources:
                    categoryIcon = SpriteName.WarsStockpileAdd;
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
