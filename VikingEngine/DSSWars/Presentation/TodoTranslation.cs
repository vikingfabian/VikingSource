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
        public string GraphicsOption_IngameMenuWidth => "Game menu width";
        public string DisplayMode => "Display mode";
        public string DisplayMode_Windowed => "Windowed";
        public string DisplayMode_BorderlessFullscreen => "Borderless fullscreen";
        public string GameSettings_RenderedMouseCursor => "Rendered cursor";

        //public string Error_SoundInitFailure => "Sound initialization failed";

        //public string GameMenu_ControllerDisconnected => "Controller disconnected";

        public string Error_SoundInitFailure => "Sound initialization failed";

        public string Resource_StockpileLimit => "Stockpile limit";

        public string GameMode_QuickMatch => "Quick Match";
        public string GameMode_QuickMatch_Description => "A shorter game format. Enter a full-scale war against rival nations.";

        public string Lobby_PlayerCount => "Player count";
        public string Lobby_TwoTeams => "Two teams";

        public string Hud_Produce => "Produce:";

        public string Tutorial_WaitForWorkerLevel => "Wait for a worker to reach:";

        /// <summary>
        /// 0: Production item, 1: School
        /// </summary>
        public string Tutorial_PracticeOrSchool => "Practice on {0}, or use a {1}";
        public string Tutorial_AddTag => "Add tag:";
        public string Tutorial_AddPin => "Add pin:";
        public string Tutorial_SelectMostTrees => "Find your city with the most trees";
        public string Tutorial_SelectACityWithX => "Select a city with {0}";

        /// <summary>
        /// Will continue on another sentence "Select a city"
        /// </summary>
        public string Tutorial_Select_NotCapital => ". Not your capital.";

        public string Tutorial_HighPriority => "Your men will complete high-priority tasks first";

        public string Tutorial_SetXPriorityToY => "Set {0} priority to {1}";
        public string Tutorial_AdvisorMission => "Advisor mission";

        public string Tutorial_AdvisorDescription => "The full game has started. The advisor will extend the tutorial with helpful missions";

        public string Tutorial_EndAdvisor => "End advisor";


        public string Tutorial_AdvisorCompleteTitle => "Advisor completed!";
        public string Tutorial_AdvisorCompleteMessage => "May your next day be blessed!";

        public string Hud_Search => "Search";

        public string DifficultyDescription_ExtremeAggression = "Extreme aggression";

        public string MapFilter => "Map filter";

        public string Settings_TechMultiplier => "Tech research speed";

        public string EndScreen_MatchComplete => "Match result";

        /// <summary>
        /// Theme: Four headed dragon symbol. Known for having an unpenetrable castle.
        /// </summary>
        public string FactionName_DragonGem => "Dragon gem";

        /// <summary>
        /// Theme: Easter egg for december. "Tomten" is an old nordic name for father christmas
        /// </summary>
        public string FactionName_Tomten => "Tomten";

        /// <summary>
        /// Theme: The blessed folk. A horde like farmers faction.
        /// </summary>
        public string FactionName_Hælfolc => "Hælfolc";

        /// <summary>
        /// The Iron Saints, people who guard a mountain pass against evil.
        /// </summary>
        public string FactionName_AerimAngren => "Aerim Angren";

        public string HUD_NotAvailbleInX => "No available in {0}";

        public string InputActionName_MiniMap => "Mini-map";

        //public string UnitType_Faction => "Faction";
    }


    //Not in use
    //public string Settings_Render3dScale_Title => "3D render scale";
    //public string Settings_Render3dScale_UpX => "Upscale {0}X";
    //public string Settings_Render3dScale_DownX => "Downscale {0}X";
}