using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        //options
        public string InputSteam = "Steam input";
        public string Input_SimulateMouse = "Simulate mouse";
        public string Input_LockMouseToWindow => "Lock mouse to window";
        public string Input_MouseEdgePush_Title=> "Mouse edge push";
        public string Input_NoControl => "None";
        public string Input_ActiveControl => "Active";
        public string Input_PassiveControl => "Passive";



        //regular
        public string Tutorial_SeeThisInThat = "See {0} in {1}";


    }
}