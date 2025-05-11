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
        public string Conscript_BlockReducingAttack => "These attacks reduces block chance";

        public string Conscript_BlockPerSecond => "May block {0} times per second";

        public string Conscript_BlockDescription => "Soldiers will block most attacks coming in their forward arc";

        public string Map_CustomSeed => "Map seed";

        public string Settings_Mode_Spectator => "Spectator";

        public string Settings_Mode_Spectator_Description => "Just watch";

        public string Automation_AutomationFocus_NoFocus_Description => "Will build a little bit of everything";

        public string Automation_AutomationFocus_WillProduce => "Will mainly produce:";

        public string Help_Food_WhoEats => "All soldiers and workers consume food";

        public string Help_Food_BigArmy => "A large army can starve out the city in its area";

        public string Help_Food_DontBuild => "Building more farms doesn't automatically increase food; you need available workers and cook stations, to gather and process it";

        public string Help_Food_UseWater => "Food production requires water";

        public string Help_Food_Postal => "Make sure your cities support each other by sending food";


        public string Message_LostCity => "City lost";

        public string Demo_Description => "A short scenario; defend your city for {0} minutes";
    }

}