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


    }

}