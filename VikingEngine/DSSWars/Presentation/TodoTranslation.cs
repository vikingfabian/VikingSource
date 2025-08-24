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
        public string EventMessage_Event_Title => "Event";
        public string EventMessage_TheCohalition => "The cohalition";

        public string EventMessage_DarkHorde => "Dark hordes";

        public string EventMessage_DarkHordeKiller_Title => "Dark horde killer";
        public string EventMessage_DarkHordeKiller_Message => "Champion knigts have joined your service";

        public string Settings_Mode_No_Achivements => "Achivements are not available.";

        public string Settings_Mode_Spectator_Description => "Just watch, or interfere with god powers.";
        public string GodPower => "God power";

        public string Building_TreeSprout_Description => "Plant a tree";
        public string Building_TreeSprout_Soft => "Soft tree sprout";
        public string Building_TreeSprout_Hard => "Hard tree sprout";

        public string GeneralSetting_SetAll => "Apply to all";
    }

}