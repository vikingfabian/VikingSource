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
        public string Hud_RemoveFromList => "Remove from list";

        public string Hud_Low => "Low";
        public string Hud_Medium => "Medium";
        public string Hud_High => "High";

        public string FastProduction => "Fast production";
        public string SlowProduction => "Slow production";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public string BlocksProduction => "Will not produce";

        //public string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";

        public string CityAutomation_SoldierQuality => "Soldier quality";
        public string WarsResourceGroup_Resources => "Resources";
        public string WarsResourceGroup_Weapons => "Weapons";

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



    }

}