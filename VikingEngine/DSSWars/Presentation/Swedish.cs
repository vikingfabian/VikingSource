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
        /// Game name and version number
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - ver {0}";

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

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "Hue";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "Lightness";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "Next color type";

        //--NOT TRANSLATED

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

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => "Total svårighet {0}%";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "Svårighetsnivå {0}%";



        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "Automation";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Will wait for the work force to max out";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "Will pause if the income is negative";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => "Large cities are in priority";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "Does max one purchase per second";

        /// <summary>
        /// Button caption for action. Purchase soldiers.
        /// </summary>
        public override string HudAction_Recruit => "Recruit";

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string HudAction_ExpandWorkForce => "Expand work force";

        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "Buy nobel house";

        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => "Ingame time: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "Soldiers recruited: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "Soldiers lost in battle: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "Enemy soldiers killed in battle: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "Soldiers deserted: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "Cities captured: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "Cities lost: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "Battles won: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "Battles lost: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "War declarations made: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen.  Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "War declarations recieved: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Diplomatic alliances: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Diplomatic servants: {0}";




        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Soldier => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Sailor => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Folkman => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Spearman => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_HonorGuard => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Pikeman => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Knight => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Archer => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_CrossBow => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Ballista => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Trollcannon => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_GreenSoldier => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Viking => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_DarkLord => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_BannerMan => "";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Warship => "";
    }
}
