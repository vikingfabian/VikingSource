using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars
{
    static class IconName
    {
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
    }
}
