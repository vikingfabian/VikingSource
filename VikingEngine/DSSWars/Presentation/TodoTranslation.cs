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
        /// <summary>
        /// How much of a resource that will be used, "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public string Hud_Purchase_ResourceCost => "{1} {0}";
        
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public string Diplomacy_EndRelations => "End relations";
        public string DisplayMode => "Display mode";
        public string DisplayMode_Windowed => "Windowed";
        public string DisplayMode_BorderlessFullscreen => "Borderless fullscreen";

        public string GameSettings_RenderedMouseCursor => "Rendered cursor";
        public string GameSettings_MuteControllerDisconnect => "Mute disconnect messages";

        public string Delivery_MaxDistance => "Delivery max distance: {0}";
        //public string Error_SoundInitFailure => "Sound initialization failed";
        public string Tutorial_WillTakeAWhile => "This will take a while, come back later.";
        
        //##SPRING - settings##
        public string Settings_ControllerVibration = "Controller vibration";
        /// <summary>
        /// 0: name of building
        /// </summary>
        public string Tutorial_WaitFor => "Wait for {0} to complete";


        public string GameOverResults => "Game history log";



        //##SPRING##

        public string UnitType_UnclaimedLand => "Unclaimed land";
        public string UnitType_Settler => "Settler";
        public string UnitType_Settler_Description => "Found a new city";
        public string Resource_ConsumedProduced => "Consumed/Produced";
        public string InputActionName_PlaceTarget => "Place target";

        public string FactionStartSize => "Faction start size";
        public string FactionStartSize_Full => "Full";
        public string FactionStartSize_OneCity => "One city";
        public string FactionStartSize_Settler => "One settler";


    }


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}