using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        public string InputActionName_NextWar => "Next faction in war";

        /// <summary>
        /// These symbols are needed to fit large numbers on the hud, there will be tooltip to explain what number it represents
        /// </summary>
        public string EngineHud_SymbolFor100 => "c";
        public string EngineHud_SymbolFor1000 => "k";
        public string EngineHud_SymbolFor10000 => "10k";

    }

}