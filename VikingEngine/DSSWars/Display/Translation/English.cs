using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class English : AbsLanguage
    {
        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "START";

        /// <summary>
        /// Button to select local mutiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Local multiplayer ({0})";

        /// <summary>
        /// Titel for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Select player count";

        public override string Lobby_LocalMultiplayerControllerRequired => "Multiplayer requires Xbox controllers";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "Next screen position";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_ProfilesSelectTitle => "Select profile";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_ProfileNumbered => "Profile {0}";

        public override string ProfileEditor_Description => "Paint your flag and select colors for your army men.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string ProfileEditor_Bucket => "Bucket";

        /// <summary>
        /// Opens profile editor
        /// </summary>
        public override string Lobby_ProfileEdit => "Edit profile";

        public override string Lobby_MapSizeTitle => "Map size";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "Tiny";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "Small";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "Medium";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "Large";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "Huge";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "Epic";

        /// <summary>
        /// Map size description X by Y kilometers
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";
        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Exit";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Player {0}";
        //--

        /// <summary>
        /// Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Options";

        /// <summary>
        /// Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Flag colors";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Main color";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Detail color 1";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Detail color 2";

        /// <summary>
        /// Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "People";

        /// <summary>
        /// Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Skin color";
        
        /// <summary>
        /// Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Hair color";

        /// <summary>
        /// Open color palette
        /// </summary>
        public override string ProfileEditor_PickColor => "Pick color";

        /// <summary>
        /// Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Move image";

        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Left";
        
        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Right";
        
        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Up";
        
        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Down";

        /// <summary>
        /// Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Discard and Exit";

        /// <summary>
        /// Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Undo all changes";

        /// <summary>
        /// Save changes and close editor
        /// </summary>
        public override string ProfileEditor_SaveAndExit => "Save and Exit";



        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "Total Income: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "Upkeep: {0}";

        /// <summary>
        /// Ingame display. Unit gold cost.
        /// </summary>
        public override string Hud_WorkForce => "Work force: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "Guard count: {0}";

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "Strength rating: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "Immigrants: {0}";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobelHouse => "Nobel house";

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => "Dark factory";
    }
}
