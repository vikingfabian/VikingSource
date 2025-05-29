using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        //Option language

        public string MouseButtonAction_PanAndCancel => "Pan and Cancel";
        public string MouseButtonAction_PanAndOrderAndCancel => "Pan, Order and Cancel";

        //##

        public string Work_BadValueDescription => "Resources can go below zero and slightly exceed the stockpile limit. The bounds are only enforced when the work queue is created.";
        public string Hud_RemoveFromList => "Remove from list";

        public string Hud_Low => "Low";
        public string Hud_Medium => "Medium";
        public string Hud_High => "High";

        public string Settings_WaterMultiplier => "Water multiplier";
        public string Settings_WaterMultiplier_Description => "How much water cities produce and store. A high value will lower computer performance.";

        public string Settings_ChildMultiplier => "Child birth multiplier";
        public string Settings_CraftMultiplier => "Craft speed multiplier";
        public string Settings_CraftMultiplier_Description => "Low speed will give fast production";

        public string FastProduction => "Fast production";
        public string SlowProduction => "Slow production";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public string BlocksProduction => "Will not produce";

        //public string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public string Automation_AutomationFocus_NoFocus => "All";
        public string CityAutomation_SoldierQuality => "Soldier quality";
        public string CityAutomation_SoldierWeaponType => "Weapon type";
        public string WarsResourceGroup_Resources => "Resources";
        public string WarsResourceGroup_Weapons => "Weapons";

        public string WarsResourceGroup_AllWeaponTypes => "Mixed";
        public string WarsResourceGroup_MeleeHandWeapons => "Melee";
        public string WarsResourceGroup_RangedHandWeapons => "Ranged";
        public string WarsResourceGroup_Warmashines => "Warmashine";


        public string FactionSettings_Titel => "Faction wide settings";
        public string FactionSettings_Description => "Will include all your cities";

        public string Conscript_MaxPopulation => "Max population";
        public string Conscript_MaxPopulation_Description => "Will only recruit when the population is maxed out";

        public string Conscript_FoodAbundance => "Max food stock";
        public string Conscript_FoodAbundance_Description => "Will only recruit when the food has reached maximum stockpile";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public string GeneralSetting_On = "Set to: On";
        public string GeneralSetting_Off = "Set to: Off";
        public string GeneralSetting_AllBuildingsDescription => "Will apply to all buildings";

        public string GeneralSetting_ApplyMessage => "Change applied to {0} buildings";

        public string MustTurnOffSteamInput => "To use controllers, you must turn off Steam Input";

        public string Technology_GainTitle => "Ways to gain technology";
        public string Technology_LevelUp => "Level up";
        public string Technology_ForEachLevelUp => "When a worker level up, in the technology field: {0}";
    }

}