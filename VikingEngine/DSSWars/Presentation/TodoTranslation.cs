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
        public string DisplayMode => "Display mode";
        public string DisplayMode_Windowed => "Windowed";
        public string DisplayMode_BorderlessFullscreen => "Borderless fullscreen";

        public string GameSettings_RenderedMouseCursor => "Rendered cursor";
        public string GameSettings_MuteControllerDisconnect => "Mute disconnect messages";

        //public string Error_SoundInitFailure => "Sound initialization failed";

        //public string GameMenu_ControllerDisconnected => "Controller disconnected";

        //public string Tutorial_HighPriority => "Your men will complete high-priority tasks first";

        //public string BuildingType_Wall_Description => "Walls protect men from attacks, and gives a slight attack boost";

        //public string BuildingType_Wall_Siege => "Siege weapons reduce wall defences";

        //public string Conscript_BlockChance => "{0}% chance to block an attack";

        //public string Battle_DeclarWarReminder => "Must declare war to attack";
        public string Tutorial_WillTakeAWhile => "This will take a while, come back later.";
        
        /// <summary>
        /// 0: name of building
        /// </summary>
        public string Tutorial_WaitFor => "Wait for {0} to complete";
    }


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}