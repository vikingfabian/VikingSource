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
        public string Error_SoundInitFailure => "Sound initialization failed";
               
        public string GameMenu_ControllerDisconnected => "Controller disconnected";

        public string GameMode_QuickMatch => "Quick Match";
        public string GameMode_QuickMatch_Description => "A shorter game format. Enter a full-scale war against rival nations.";

        public string Lobby_PlayerCount => "Player count";
        public string Lobby_TwoTeams => "Two teams";

        public string Tutorial_HighPriority => "Your men will complete high-priority tasks first";
        public string Hud_Search => "Search";

        public string DifficultyDescription_ExtremeAggression = "Extreme aggression";

        public string MapFilter => "Map filter";

        public string Settings_TechMultiplier => "Tech research speed";

        public string EndScreen_MatchComplete => "Match result";

        public string FactionName_DragonGem => "Dragon gem";
    }


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}