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
        public string Tutorial_WillTakeAWhile => "This will take a while, come back later.";
        
        //##SPRING - settings##
        public string Settings_ControllerVibration = "Controller vibration";
        /// <summary>
        /// 0: name of building
        /// </summary>
        public string Tutorial_WaitFor => "Wait for {0} to complete";



        //##SPRING##

        public string UnitType_UnclaimedLand => "Unclaimed land";
        public string UnitType_Settler => "Settler";
        public string UnitType_Settler_Description => "Found a new city";
        public string Resource_ConsumedProduced => "Consumed/Produced";
    }


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}