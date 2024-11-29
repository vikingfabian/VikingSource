using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        public string Hud_Advanced => "Advanced";
        public string Hud_Loading => "Loading...";

        public string Settings_ResetToDefault => "Reset to default";
        public string Settings_NewGame => "New game";

        public string Settings_AndvancedGameSettings => "Game settings";
        public string Settings_FoodMultiplier => "Food multiplier";
        public string Settings_FoodMultiplier_Description => "How long a worker or soldier lasts on a full stomach. A high value vill lower computer performance";

        public string Settings_GameMode => "Game mode";

        public string Settings_Mode_Story => "Full story";
        public string Settings_Mode_InclueBoss => "Boss events.";
        public string Settings_Mode_InclueAttacks => "Random attacks.";
        public string Settings_Mode_Sandbox => "Sandbox";
        public string Settings_Mode_Peaceful => "Peaceful";
        public string Settings_Mode_Peaceful_Description => "All wars are initiated by the player";

        public string Lobby_ImportSave=> "Import save";

        public string Lobby_ExportSave => "Export save";
        public string Lobby_ExportSave_Description => "Creates a copy of the file, and will place it in the import folder: {0}";

        //public string BuildHud_Demolish => "Demolish";
    }

}