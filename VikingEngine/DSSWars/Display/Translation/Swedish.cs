using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    class Swedish:AbsLanguage
    {
        // <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "STARTA";

        /// <summary>
        /// Button to select local multiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "Lokal flerspelare ({0})";

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "Välj antal spelare";

        public override string Lobby_LocalMultiplayerControllerRequired => "Flerspelare kräver Xbox-kontroller";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "Nästa skärmposition";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_ProfilesSelectTitle => "Välj profil";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_ProfileNumbered => "Profil {0}";

        /// <summary>
        /// Opens profile editor
        /// </summary>
        public override string Lobby_ProfileEdit => "Redigera profil";

        public override string Lobby_MapSizeTitle => "Kartstorlek";


        public override string Lobby_MapSizeOptTiny => "Mini";

        public override string Lobby_MapSizeOptSmall => "Liten";

        public override string Lobby_MapSizeOptMedium => "Mellan";

        public override string Lobby_MapSizeOptLarge => "Stor";

        public override string Lobby_MapSizeOptHuge => "Enorm";

        public override string Lobby_MapSizeOptEpic => "Episk";

        /// <summary>
        /// Map size description X by Y kilometers
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";

        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "Avsluta";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "Spelare {0}";

        public override string ProfileEditor_Description => "Måla din flagga och välj soldaternas färger.";

        public override string ProfileEditor_Bucket => "Målarspann";

        /// <summary>
        /// Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Alternativ";

        /// <summary>
        /// Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Flaggfärger";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Huvudfärg";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Detaljfärg 1";

        /// <summary>
        /// Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Detaljfärg 2";

        /// <summary>
        /// Title for selecting your soldiers' colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "Människor";

        /// <summary>
        /// Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Hudfärg";

        /// <summary>
        /// Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Hårfärg";

        /// <summary>
        /// Open color palette
        /// </summary>
        public override string ProfileEditor_PickColor => "Välj färg";

        /// <summary>
        /// Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Flytta bild";

        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Vänster";

        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Höger";

        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Upp";

        /// <summary>
        /// Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Ner";

        /// <summary>
        /// Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Kassera och Avsluta";

        /// <summary>
        /// Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Ångra alla ändringar";

        /// <summary>
        /// Save changes and close editor
        /// </summary>
        public override string ProfileEditor_SaveAndExit => "Spara och Avsluta";
    }
}
