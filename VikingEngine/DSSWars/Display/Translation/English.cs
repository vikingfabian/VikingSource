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
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "English";

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => "Language";

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

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
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

        /// <summary>
        /// Game name and version number
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string ProfileEditor_Description => "Paint your flag and select colors for your army men.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string ProfileEditor_Bucket => "Bucket";

        /// <summary>
        /// Opens profile editor
        /// </summary>
        public override string Lobby_ProfileEdit => "Edit profile";


        public override string Lobby_WarningTitle => "Warning";
        public override string Lobby_IgnoreWarning => "Ignore warning";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "One player has no input";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "Extra";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "Warning! This content is not covered by localization or expected input/accessiblity support";


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
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "Options";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "Flag colors";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "Main color";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "Detail color 1";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "Detail color 2";

        /// <summary>
        /// In player profile editor. Title for selecting you soldiers colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "People";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "Skin color";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "Hair color";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "Pick color";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "Move image";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "Left";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "Right";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "Up";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "Down";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "Discard and Exit";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "Undo all changes";

        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string ProfileEditor_SaveAndExit => "Save and Exit";

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


        public override string Hud_Cancel => "Cancel";
        public override string Hud_Back => "Back";

        public override string Hud_PurchaseTitle_Requirement => "Requirement";
        public override string Hud_PurchaseTitle_Cost => "Cost";
        public override string Hud_PurchaseTitle_Gain => "Gain";
        public override string Hud_CompareMilitaryStrength_YourToOther => "Strength: Your {0} - Their {1}";

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
        public override string Settings_TotalDifficulty => "Total Difficulty {0}%";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "Difficulty level {0}%";


        /// <summary>
        ///  In game settings menu.Option for creating new maps instead of loading one
        /// </summary>
        public override string Settings_GenerateMaps => "Generate new maps";

        /// <summary>
        ///  In game settings menu.Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "Generating is slower than loading the pre-built maps";

        /// <summary>
        ///  In game settings menu.Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => "Allow pause and command";

        /// <summary>
        ///  In game settings menu.Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => "Boss events";

        /// <summary>
        ///  In game settings menu.Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "Turning off the boss will put the game in a sandbox mode with no ending.";


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
        public override string HudAction_BuyItem => "Buy {0}";

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string DiplomacyPoints=> "Diplomacy points";

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType=> "Relation";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant=> "Servant";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally=> "Ally";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good=> "Good";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace=> "Peace";
        
        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral=> "Neutral";
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce=> "Truce";
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War=> "War";
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar=> "Total war";

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms.
        /// </summary>
        public override string Diplomacy_SpeakTermIs=> "Speaking terms: {0}";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good=> "Good";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal=> "Normal";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad=> "Bad";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None=> "None";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo=> "Forge relations to: {0}";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace=> "Offer peace";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance=> "Offer alliance";

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} offers new relations";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer=> "Accept new relation";

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered=> "New relation offered: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant=> "Absorb as servant";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide=> "Is light side ally";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength=> "Ends in {0} seconds";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction=> "Extend truce";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength=> "Extends truce by {0} seconds";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost=> "Breaking the relation will cost {0} diplomacy";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription=> "Allies share war declarations.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription=> "Limits the ability to declare war.";

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary=> "{0}x stronger military power";

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar=> "Servant must be in war against a stronger foe";

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities=> "Servant can have max {0} citites";

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRaise=> "Price will raise for each servant";

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction=> "Absorb the other faction";


        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time =>"Ingame time: {0}";

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
    }
}
