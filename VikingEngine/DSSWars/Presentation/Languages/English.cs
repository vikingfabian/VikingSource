using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ;
using VikingEngine.ToGG.HeroQuest.Players.Ai;

namespace VikingEngine.DSSWars.Presentation
{
    partial class English : AbsLanguage
    {
        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "English";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

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
        public override string Lobby_LocalMultiplayerEdit => "Local multiplayer";

        /// <summary>
        /// Title for menu where you select split screen player count
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
        public override string Lobby_FlagSelectTitle => "Select flag";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => "Flag {0}";

        /// <summary>
        /// Game name and version number
        /// </summary>
        public override string Lobby_GameVersion => "DSS war party - ver {0}";

        public override string FlagEditor_Description => "Paint your flag and select colors for your army men.";

        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => "Bucket";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "Edit flag";


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
        public override string Lobby_Extra_NoSupportWarning => "Warning! This content is not covered by localization or expected input/accessibility support";


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
        /// Map size description X by Y kilometers. 0: Width, 1: Height
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
        public override string Hud_SaveAndExit => "Save and Exit";

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
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => "Game speed: {0}x";

        public override string Input_GameSpeed => "Game speed";

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "Total income/second: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "Upkeep: {0}";
        public override string Hud_ArmyUpkeep => "Army upkeep: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "Guards";

        public override string Hud_IncreaseMaxGuardCount => "Max guard size {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "You need to expand the city.";

        public override string Hud_SoldierCount => "Soldier count: {0}";

        public override string Hud_SoldierGroupsCount => "Group count: {0}";

        /// <summary>
        /// Ingame display. Unit caculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "Strength rating: {0}";

        /// <summary>
        /// Ingame display. Caculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => "Military strength: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "Immigrants";


        public override string Hud_CityCount => "City count: {0}";
        public override string Hud_ArmyCount => "Army count: {0}";


        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "Requirement";
        public override string Hud_PurchaseTitle_Cost => "Cost";
        public override string Hud_PurchaseTitle_Gain => "Gain";

        /// <summary>
        /// How much of a resource that will be used, "5 gold. (Available: 10)". There will be a "cost" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (Available: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "Cost will increase by {0}";

        public override string Hud_Purchase_MaxCapacity => "Has reached maximum capacity";

        public override string Hud_CompareMilitaryStrength_YourToOther => "Strength: Your {0} - Their {1}";

        /// <summary>
        /// Display a short string of date as Year, Month, Day
        /// </summary>
        public override string Hud_Date => "Y{0} M{1} D{2}";
        
        /// <summary>
        /// Display a short string of timespan as Hour, Minutes, Seconds
        /// </summary>
        public override string Hud_TimeSpan => "H{0} M{1} S{2}";

        /// <summary>
        /// Battle between two armies, or army and city
        /// </summary>
        public override string Hud_Battle => "Battle";



        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => "Pause";

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => "Resume";

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => "Gold";

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => "Workers";


        public override string ResourceType_Workers_Description => "Workers provide income. And are drafted as soldiers for your armies";

        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "Diplomacy points";

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "Diplomatic points: {0} / {1} ({2})";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => "Noble house";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "1 diplomacy point per {0} seconds";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "+{0} to diplomacy point max limit";
        public override string Building_NobleHouse_UnlocksKnight => "Unlocks Knight unit";

        public override string Building_BuildAction => "Build";
        public override string Building_IsBuilt => "Built";

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
        public override string Settings_BossEvents_SandboxDescription => "Disabling boss events will put the game in a sandbox mode with no ending.";


        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "Automation";
        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "Will wait for the workforce to max out";
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
        public override string Automation_InfoLine_PurchaseSpeed => "Performs a maximum of one purchase per second";


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "Buy {0}";

        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => "Relation";

        /// <summary>
        /// Titel for list of relations other factions have with eachother
        /// </summary>
        public override string Diplomacy_RelationToOthers => "Their relations with others";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nations resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "Servant";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "Ally";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "Good";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "Peace";

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "Neutral";
        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "Truce";
        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => "War";
        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "Total war";

        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "Speaking terms: {0}";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "Good";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "Normal";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "Bad";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "None";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "Forge relations to: {0}";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => "Offer peace";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "Offer alliance";

        /// <summary>
        /// Diplomatic title. Another player Suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} offers new relations";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "Accept new relation";

        /// <summary>
        /// Diplomatic description. Another player Suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "New relation offered: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation to serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "Absorb as servant";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => "Is light side ally";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "Ends in {0} seconds";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "Extend truce";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "Extends truce by {0} seconds";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "Breaking the relation will cost {0} diplomacy points";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => "Allies share war declarations.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "Limits the ability to declare war.";

        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "{0}x stronger military power";

        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "Servant must be in war against a stronger foe";

        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "Servant can have max {0} cities";

        /// <summary>
        /// Diplomatic description. Const in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "Price will rise for each servant";

        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "Absorb the other faction";

        /// <summary>
        /// Messaage when you recieve a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "War declared!";

        /// <summary>
        /// The truce timer har run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "The truce has ended";

        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string EndGameStatistics_Title => "Stats";
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
        public override string EndGameStatistics_WarsStartedByEnemy => "War declarations received: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "Diplomatic alliances: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "Diplomatic servants: {0}";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => "Army";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => "Group";

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => "City";

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "Army group, count: {0}";

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => "Soldier";

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => "Sailor";

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => "Folkman";

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => "Spearman";

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the Kings guard.
        /// </summary>
        public override string UnitType_HonorGuard => "Honor Guard";

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => "Pikeman";

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => "Knight";

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => "Archer";

        /// <summary>
        /// Name for a specialized type of soldier. 
        /// </summary>
        public override string UnitType_Crossbow => "Crossbow";

        /// <summary>
        /// Name for a specialized type of soldier. Warmashine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => "Ballista";

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => "Trollcannon";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => "Green Soldier";

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => "Viking";

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => "Dark Lord";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => "Bannerman";

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0} warship";

        public override string UnitType_Description_Soldier => "A general purpose unit.";
        public override string UnitType_Description_Sailor => "Strong during sea warfare";
        public override string UnitType_Description_Folkman => "Cheap untrained soldiers";
        public override string UnitType_Description_HonorGuard => "Elite soldiers with no upkeep";
        public override string UnitType_Description_Knight => "Strong in open field battles";
        public override string UnitType_Description_Archer => "Only strong when protected.";
        public override string UnitType_Description_Crossbow => "Powerful ranged soldier";
        public override string UnitType_Description_Ballista => "Strong against cities";
        public override string UnitType_Description_GreenSoldier => "Feared elf warrior";

        public override string UnitType_Description_DarkLord => "The final boss";

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => "Stats per unit";

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0} groups, a total of {1} units";

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "Attack strength: Land {0} | Sea {1} | City {2}";

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => "Health: {0}";

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "Army speed bonus on land: {0}";

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "Army speed bonus on sea: {0}";

        /// <summary>
        /// Purchased soliders will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "Training time: {0} minutes. Will be twice as fast if the recruits are adjacent to a city.";

        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => "Halt";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => "Disband units";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => "Divide army";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => "Remove {0}";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => "Disband all";

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} groups: {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => "Send units to {0}";

        public override string ArmyOption_MergeAllArmies => "Merge all armies";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "Divide units to a new army";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendX => "Send {0}";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => "Send All";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => "Divide army in half";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => "Merge armies";



        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => "Recruit";

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => "Recruit {0}";

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => "Mercenaries: {0}";


        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => "Market mercenaries for hire";

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => "Import {0} mercenaries";

        public override string CityOption_Mercenaries_Description => "Soldiers will be drafted from mercenaries instead of your workforce";

        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "Expand workforce";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "Max workforce +{0}";
        public override string CityOption_ExpandGuardSize => "Expand guard";

        public override string CityOption_Damages => "Damages: {0}";
        public override string CityOption_Repair => "Repair damages";
        public override string CityOption_RepairGain => "Repair {0} damages";

        public override string CityOption_Repair_Description => "Damages lower the number of workers you can fit.";


        public override string CityOption_BurnItDown => "Burn it down";
        public override string CityOption_BurnItDown_Description => "Remove the workforce and apply max damages";

        /// <summary>
        /// The main boss. Named after a glowing metal stone stuck in their forehead.
        /// </summary>
        public override string FactionName_DarkLord => "Eye of Doom";

        /// <summary>
        /// Orc inspired faction. Works for the dark lord.
        /// </summary>
        public override string FactionName_DarkFollower => "Servants of Dread";

        /// <summary>
        /// The largest faction, the old but corrupted kingdom.
        /// </summary>
        public override string FactionName_UnitedKingdom => "United Kingdoms";

        /// <summary>
        /// Elf inspired faction. Lives in harmony with the forest.
        /// </summary>
        public override string FactionName_Greenwood => "Greenwood";

        /// <summary>
        /// Asian flavored faction to the east 
        /// </summary>
        public override string FactionName_EasternEmpire => "Eastern Empire";

        /// <summary>
        /// Viking flavored kingdom in the north. The largest one.
        /// </summary>
        public override string FactionName_NordicRealm => "Nordic Realms";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a bear claw symbol.
        /// </summary>
        public override string FactionName_BearClaw => "Bear claw";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Nordic spur";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a black raven symbol.
        /// </summary>
        public override string FactionName_IceRaven => "Ice Raven";

        /// <summary>
        /// Faction famous for killing dragons with powerful ballistas.
        /// </summary>
        public override string FactionName_Dragonslayer => "Dragonslayer";

        /// <summary>
        /// A mercenary unit from the south. Arabic flavored.
        /// </summary>
        public override string FactionName_SouthHara => "South Hara";

        /// <summary>
        /// Name for neutral CPU controlled nations
        /// </summary>
        public override string FactionName_GenericAi => "AI {0}";

        /// <summary>
        /// Display name for players and their numbers
        /// </summary>
        public override string FactionName_Player => "Player {0}";

        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "Enemy approaching!";
        public override string EventMessage_HaraMercenaryText => "Hara mercenaries have been spotted in the south";

        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "A dark prophesy";
        public override string EventMessage_ProphesyText => "The Eye of Doom will appear soon, and your enemies will join him!";

        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "Dark times";
        public override string EventMessage_FinalBossEnterText => "The Eye of Doom has entered the map!";

        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "A desperate attack";
        public override string EventMessage_FinalBattleText => "The dark lord has joined the battlefield. Now is your chance to destroy him!";

        /// <summary>
        /// Message when soldiers leave the army when you can't pay thier upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => "Deserters!";
        public override string EventMessage_DesertersText_Money => "Unpaid soldiers are deserting from your armies";

        public override string DifficultyDescription_AiAggression => "Ai aggressivity: {0}.";
        public override string DifficultyDescription_BossSize => "Boss size: {0}.";
        public override string DifficultyDescription_BossEnterTime => "Boss enter time: {0}.";
        public override string DifficultyDescription_AiEconomy => "Ai Economy: {0}%.";
        public override string DifficultyDescription_AiDelay => "Ai delay: {0}.";
        public override string DifficultyDescription_DiplomacyDifficulty => "Diplomacy difficulty: {0}.";
        public override string DifficultyDescription_MercenaryCost => "Mercenary cost: {0}.";
        public override string DifficultyDescription_HonorGuards => "Honor guards: {0}.";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "Victory!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
        {
            "In times of peace, we mourn the dead.",
            "Every triumph carries a shadow of sacrifice.",
            "Remember the journey that brought us here, dotted with the souls of the brave.",
            "Our minds are light from victory, our hearts are heavy from the weight of the fallen"
        };

        public override string EndScreen_DominationVictoryQuote => "I was chosen by the Gods to dominate the world!";

        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => "Failure!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
        {
            "With our bodies torn from marching and nights of worry, we welcome the end.",
            "Defeat may darken our lands, but they cannot extinguish the light of our determination.",
            "Extinguish the flames in our hearts, from their ashes, our children shall forge a new dawn.",
            "Let our tales be the ember that kindles tomorrow's victory.",
        };

        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => "Watch epilogue";

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => "Epilogue";

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160 years ago";

        /// <summary>
        /// The Prologue is a short poem about the game's stroy
        /// </summary>
        public override string GameMenu_WatchPrologue => "Watch Prologue";

        public override string Prologue_Title => "Prologue";

        /// <summary>
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
        {
            "Dreams haunt you at night,",
            "A prophecy of a dark future",
            "Prepare for his arrival,",
        };

        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => "Game menu";

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => "Continue";

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => "Resume";

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => "Exit game";

        public override string Hud_Save => "Save";
        public override string GameMenu_SaveStateWarnings => "Warning! Save files will be lost when the game is updated.";
        public override string GameMenu_LoadState => "Load";
        public override string GameMenu_ContinueFromSave => "Continue from save";

        public override string GameMenu_AutoSave => "Auto save";

        public override string GameMenu_Load_PlayerCountError => "You must setup a matching player count to the save file: {0}";

        public override string Progressbar_MapLoadingState => "Map loading: {0}";

        public override string Progressbar_ProgressComplete => "complete";

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "Generating: {0}%. (Fails {1})";


        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "part {0}/{1}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => "Saving: {0}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => "Loading: {0}";

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => "Press any key to continue";


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => "Run tutorial";
        public override string Tutorial_MissionsTitle => "Tutorial missions";
        public override string Tutorial_Mission_BuySoldier => "Select a city and recruit a soldier";
        public override string Tutorial_Mission_MoveArmy => "Select an army and move it";

        public override string Tutorial_CompleteTitle => "Tutorial completed!";
        public override string Tutorial_CompleteMessage => "Unlocked full zoom and advanced game options.";

        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => "Select";
        public override string Tutorial_MoveInput => "Move command";


        
        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "War declaration";

        public override string ArmyOption_Attack => "Attack";



        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "Key bindings";

       

        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "Keyboard & Mouse";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "Controller";


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "Sale prices";
        public override string Blueprint_Title => "Blueprint";
        public override string Resource_Tab_Overview => "Overview";
        public override string Resource_Tab_Stockpile => "Stockpile";

        public override string Resource => "Resource";
        public override string Resource_StockPile_Info => "Set a goal amount for storage of resources; this will inform the workers when to work on other resources.";
        public override string Resource_TypeName_Water => "water";
        public override string Resource_TypeName_Wood => "wood";
        public override string Resource_TypeName_Fuel => "fuel";
        public override string Resource_TypeName_Stone => "stone";
        public override string Resource_TypeName_RawFood => "raw food";
        public override string Resource_TypeName_Food => "food";
        public override string Resource_TypeName_Beer => "beer";
        public override string Resource_TypeName_Wheat => "wheat";
        public override string Resource_TypeName_Linen => "linen";
        //public override string Resource_TypeName_SkinAndLinen => "skin and linen";
        public override string Resource_TypeName_IronOre => "iron ore";
        public override string Resource_TypeName_GoldOre => "gold ore";
        public override string Resource_TypeName_Iron => "iron";

        public override string Resource_TypeName_SharpStick => "Sharp stick";
        public override string Resource_TypeName_Sword => "Sword";
        public override string Resource_TypeName_KnightsLance => "Knight's lance";
        public override string Resource_TypeName_TwoHandSword => "Zweihänder";
        public override string Resource_TypeName_Bow => "Bow";

        public override string Resource_TypeName_LightArmor => "Light armor";
        public override string Resource_TypeName_MediumArmor => "Medium armor";
        public override string Resource_TypeName_HeavyArmor => "Heavy armor";

        public override string ResourceType_Children => "Children";

        public override string BuildingType_DefaultName => "Building";
        public override string BuildingType_WorkerHut => "Worker hut";
        public override string BuildingType_Brewery => "Brewery";
        public override string BuildingType_Postal => "Postal service";
        public override string BuildingType_Recruitment => "Recruitment center";
        public override string BuildingType_Barracks => "Barracks";
        public override string BuildingType_PigPen => "Pig pen";
        public override string BuildingType_HenPen => "Hen pen";
        public override string BuildingType_WorkBench => "Work bench";
        public override string BuildingType_Carpenter => "Carpenter";
        public override string BuildingType_CoalPit => "Charcoal pit";
        public override string DecorType_Statue => "Statue";
        public override string DecorType_Pavement => "Pavement";
        public override string BuildingType_Smith => "Smith";
        public override string BuildingType_Cook => "Cook";
        public override string BuildingType_Storage => "Storehouse";

        public override string BuildingType_ResourceFarm => "{0} farm";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "Expands worker limit by {0}";
        public override string BuildingType_Tavern_Description => "Workers may eat here";
        public override string BuildingType_Tavern_Brewery => "Beer production";
        public override string BuildingType_Postal_Description => "Send resources to other cities";
        public override string BuildingType_Recruitment_Description => "Send men to other cities";
        public override string BuildingType_Barracks_Description => "Uses men and equipment to recruit soldiers";
        public override string BuildingType_PigPen_Description => "Produces pigs, which give food and skin";
        public override string BuildingType_HenPen_Description => "Produces hens and eggs, which give food";
        public override string BuildingType_Decor_Description => "Decoration";
        public override string BuildingType_Farm_Description => "Grow a resource";

        public override string BuildingType_Cook_Description => "Food crafting station";
        public override string BuildingType_Bench_Description => "Item crafting station";

        public override string BuildingType_Smith_Description => "Metal crafting station";
        public override string BuildingType_Carpenter_Description => "Wood crafting station";

        public override string BuildingType_Nobelhouse_Description => "Home for knights and diplomats";
        public override string BuildingType_CoalPit_Description => "Efficient fuel production";
        public override string BuildingType_Storage_Description => "Dropoff point for resources";

        public override string MenuTab_Info => "Info";
        public override string MenuTab_Work => "Work";
        public override string MenuTab_Recruit => "Recruit";
        public override string MenuTab_Resources => "Resources";
        public override string MenuTab_Trade => "Trade";
        public override string MenuTab_Build => "Build";
        public override string MenuTab_Economy => "Economy";
        public override string MenuTab_Delivery => "Delivery";

        public override string MenuTab_Build_Description => "Place buildings in your city";
        public override string MenuTab_BlackMarket_Description => "Place buildings in your city";
        public override string MenuTab_Resources_Description => "Place buildings in your city";
        public override string MenuTab_Work_Description => "Place buildings in your city";
        public override string MenuTab_Automation_Description => "Place buildings in your city";

        public override string BuildHud_OutsideCity => "Outside city region";
        public override string BuildHud_OutsideFaction => "Outside your borders!";

        public override string BuildHud_OccupiedTile => "Occupied tile";

        public override string Build_PlaceBuilding => "Building";
        public override string Build_DestroyBuilding => "Destroy";
        public override string Build_ClearTerrain => "Clear terrain";

        public override string Build_ClearOrders => "Clear build orders";
        public override string Build_Order => "Build order";
        public override string Build_OrderQue => "Build order queue: {0}";
        public override string Build_AutoPlace => "Auto place";

        public override string Work_OrderPrioTitle => "Work priority";
        public override string Work_OrderPrioDescription => "Priority goes from 1 (low) to {0} (high)";

        public override string Work_OrderPrio_No => "No priority. Will not be worked on.";
        public override string Work_OrderPrio_Min => "Minimum priority.";
        public override string Work_OrderPrio_Max => "Maximum priority.";

        public override string Work_Move => "Move items";

        public override string Work_GatherXResource => "Gather {0}";
        public override string Work_CraftX => "Craft {0}";
        public override string Work_Farming => "Farming";
        public override string Work_Mining => "Mining";
        public override string Work_Trading => "Tradeing";

        public override string Work_AutoBuild => "Auto build and expand";

        public override string WorkerHud_WorkType => "Work status: {0}";
        public override string WorkerHud_Carry => "Carry: {0} {1}";
        public override string WorkerHud_Energy => "Energy: {0}";
        public override string WorkerStatus_Exit => "Leave workforce";
        public override string WorkerStatus_Eat => "Eat";
        public override string WorkerStatus_Till => "Till";
        public override string WorkerStatus_Plant => "Plant";
        public override string WorkerStatus_Gather => "Gather";
        public override string WorkerStatus_PickUpResource => "Pick up resource";
        public override string WorkerStatus_DropOff => "Drop off";
        public override string WorkerStatus_BuildX => "Build {0}";
        public override string WorkerStatus_TrossReturnToArmy => "Return to army";

        public override string Hud_ToggleFollowFaction => "Toggle follow faction settings";
        public override string Hud_FollowFaction_Yes => "Is set to use faction global settings";
        public override string Hud_FollowFaction_No => "Is set to use local settings (Global value is {0})";

        public override string Hud_Idle => "Idle";
        public override string Hud_NoLimit => "No limit";

        public override string Hud_None => "None";
        public override string Hud_ProductionQueue => "Production Queue";

        public override string Hud_EmptyList => "- Empty list -";

        public override string Hud_RequirementOr => "- or -";

        public override string Hud_BlackMarket => "Black market";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "Select City";
        public override string Conscription_Title => "Conscription";
        public override string Conscript_WeaponTitle => "Weapon";
        public override string Conscript_ArmorTitle => "Armor";
        public override string Conscript_TrainingTitle => "Training";

        public override string Conscript_SpecializationTitle => "Specialization";
        public override string Conscript_SpecializationDescription => "Will increase attack in one area, and reduce all others, by {0}";
        public override string Conscript_SelectBuilding => "Select barracks";

        public override string Conscript_WeaponDamage => "Weapon damage: {0}";
        public override string Conscript_ArmorHealth => "Armor health: {0}";
        public override string Conscript_TrainingSpeed => "Attack speed: {0}";
        public override string Conscript_TrainingTime => "Training time: {0}";

        public override string Conscript_Training_Minimal => "Minimal";
        public override string Conscript_Training_Basic => "Basic";
        public override string Conscript_Training_Skillful => "Skillful";
        public override string Conscript_Training_Professional => "Professional";

        public override string Conscript_Specialization_Field => "Open field";
        public override string Conscript_Specialization_Sea => "Ship";
        public override string Conscript_Specialization_Siege => "Siege";
        public override string Conscript_Specialization_Traditional => "Traditional";
        public override string Conscript_Specialization_AntiCavalry => "Anti cavalry";

        public override string Conscription_Status_CollectingEquipment => "Collecting equipment: {0}";
        public override string Conscription_Status_CollectingMen => "Collecting men: {0}";
        public override string Conscription_Status_Training => "Training: {0}";

        public override string ArmyHud_Food_Reserves_X => "Food reserves: {0}";
        public override string ArmyHud_Food_Upkeep_X => "Food upkeep: {0}";
        public override string ArmyHud_Food_Costs_X => "Food costs: {0}";

        public override string Deliver_WillSendXInfo => "Will send {0} at a time";
        public override string Delivery_ListTitle => "Select delivery service";
        public override string Delivery_DistanceX => "Distance: {0}";
        public override string Delivery_DeliveryTimeX => "Delivery time: {0}";
        public override string Delivery_SenderMinimumCap => "Sender minimum cap";
        public override string Delivery_RecieverMaximumCap => "Receiver maximum cap";
        public override string Delivery_ItemsReady => "Items ready";
        public override string Delivery_RecieverReady => "Receiver ready";
        public override string Hud_ThisCity => "This city";
        public override string Hud_RecieveingCity => "Receiving city";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "Displayed in Resource Per Second.";

        public override string Info_MinuteAverage => "The value is an average from the last minute";

        public override string Message_OutOfFood_Title => "Out of food";
        public override string Message_CityOutOfFood_Text => "Expensive food will be purchased from the black market. Workers will starve when your money runs out.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "Terrain type";

        public override string Hud_EnergyUpkeepX => "Food energy upkeep {0}";

        public override string Hud_EnergyAmount => "{0} energy (seconds of work)";

        public override string Hud_CopySetup => "Copy setup";
        public override string Hud_Paste => "Paste";

        public override string Hud_Available => "Available";

        public override string WorkForce_ChildBirthRequirements => "Child birth requirements";
        public override string WorkForce_AvailableHomes => "Available homes: {0}";
        public override string WorkForce_Peace => "Peace";
        public override string WorkForce_ChildToManTime => "Grown up age: {0} minutes";

        public override string Economy_TaxIncome => "Tax income: {0}";
        public override string Economy_ImportCostsForResource => "Import costs for {0}: {1}";
        public override string Economy_BlackMarketCostsForResource => "Black market costs for {0}: {1}";
        public override string Economy_GuardUpkeep => "Guard upkeep: {0}";

        public override string Economy_LocalCityTrade_Export => "City trade export: {0}";
        public override string Economy_LocalCityTrade_Import => "City trade import: {0}";

        public override string Economy_ResourceProduction => "{0} production: {1}";
        public override string Economy_ResourceSpending => "{0} spending: {1}";

        public override string Economy_TaxDescription => "Tax is {0} gold per worker";

        public override string Economy_SoldResources => "Sold resources (gold ore): {0}";

        public override string UnitType_Cities => "Cities";
        public override string UnitType_Armies => "Armies";
        public override string UnitType_Worker => "Worker";

        public override string UnitType_FootKnight => "Longsword knight";
        public override string UnitType_CavalryKnight => "Cavalry knight";

        public override string CityCulture_LargeFamilies => "Large families";
        public override string CityCulture_FertileGround => "Fertile grounds";
        public override string CityCulture_Archers => "Skilled archers";
        public override string CityCulture_Warriors => "Warriors";
        public override string CityCulture_AnimalBreeder => "Animal breeders";
        public override string CityCulture_Miners => "Miners";
        public override string CityCulture_Woodcutters => "Lumbermen";
        public override string CityCulture_Builders => "Builders";
        public override string CityCulture_CrabMentality => "Crab mentality";
        public override string CityCulture_DeepWell => "Deep well";
        public override string CityCulture_Networker => "Networker";
        public override string CityCulture_PitMasters => "Pit masters";

        public override string CityCulture_CultureIsX => "Culture: {0}";
        public override string CityCulture_LargeFamilies_Description => "Increased child birth";
        public override string CityCulture_FertileGround_Description => "Crops give more";
        public override string CityCulture_Archers_Description => "Produces skilled archers";
        public override string CityCulture_Warriors_Description => "Produces skilled melee fighters";
        public override string CityCulture_AnimalBreeder_Description => "Animals give more resources";
        public override string CityCulture_Miners_Description => "Mines more ore";
        public override string CityCulture_Woodcutters_Description => "Trees give more wood";
        public override string CityCulture_Builders_Description => "Fast at building";
        public override string CityCulture_CrabMentality_Description => "Work costs less energy. Cannot produce high-skill soldiers.";
        public override string CityCulture_DeepWell_Description => "Water replenishes faster";
        public override string CityCulture_Networker_Description => "Efficient postal service";
        public override string CityCulture_PitMasters_Description => "Higher fuel production";

        public override string CityOption_AutoBuild_Work => "Auto expand workforce";
        public override string CityOption_AutoBuild_Farm => "Auto expand farms";

        public override string Hud_PurchaseTitle_Resources => "Buy resources";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "You own";

        public override string Tutorial_EndTutorial => "End tutorial";
        public override string Tutorial_MissionX => "Mission {0}";
        public override string Tutorial_CollectXAmountOfY => "Collect {0} {1}";
        public override string Tutorial_SelectTabX => "Select tab: {0}";
        public override string Tutorial_IncreasePriorityOnX => "Increase the priority on: {0}";
        public override string Tutorial_PlaceBuildOrder => "Place build order: {0}";
        public override string Tutorial_ZoomInput => "Zoom";

        public override string Tutorial_SelectACity => "Select a city";
        public override string Tutorial_ZoomInWorkers => "Zoom in to see the workers";
        public override string Tutorial_CreateSoldiers => "Create two soldier units with this equipment: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "Zoom out, to map overview";
        public override string Tutorial_ZoomOutDiplomacy => "Zoom out, to diplomacy view";
        public override string Tutorial_ImproveRelations => "Improve your relations with a neighbor faction";
        public override string Tutorial_MissionComplete_Title => "Mission complete!";
        public override string Tutorial_MissionComplete_Unlocks => "New controls have been unlocked";

        //patch1
        public override string Resource_ReachedStockpile => "Reached stockpile goal buffer";

        public override string BuildingType_ResourceMine => "{0} mine";

        public override string Resource_TypeName_BogIron => "Bog iron";

        public override string Resource_TypeName_Coal => "Coal";

        public override string Language_XUpkeepIsY => "{0} upkeep: {1}";
        public override string Language_XCountIsY => "{0} count: {1}";

        public override string Message_ArmyOutOfFood_Text => "Expensive food will be purchased from the black market. Hungry soldiers will desert when your money runs out.";

        public override string Info_ArmyFood => "Armies will restock food from the closest friendly city. Food can be purchased from other factions. In hostile regions, food can only be purchased from the black market.";

        public override string FactionName_Monger => "Monger";
        public override string FactionName_Hatu => "Hatu";
        public override string FactionName_Destru => "Destru";

        //patch2
        public override string Tutorial_BuildSomething => "Build something that produces {0}";
        public override string Tutorial_BuildCraft => "Build a crafting station for: {0}";
        public override string Tutorial_IncreaseBufferLimit => "Increase buffer limit for: {0}";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "Reach a stockpile of {0} {1}";
        public override string Tutorial_LookAtFoodBlueprint => "Look at the food blueprint";
        public override string Tutorial_CollectFood_Info1 => "The workers will walk to the city hall to eat";
        public override string Tutorial_CollectFood_Info2 => "The army sends tross workers to collect food";
        public override string Tutorial_CollectFood_Info0 => "Want full control of the workers? Set all work priorities to zero, and then just activate one at a time.";

        public override string EndGameStatistics_DecorsBuilt => "Decorations built: {0}";
        public override string EndGameStatistics_StatuesBuilt => "Statues built: {0}";


        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "By default, workers go to the city hall to eat or drop off items";
        public override string GameMenu_UseSpeedX => "{0} speed option";
        public override string GameMenu_LongerBuildQueue => "Extended build queue";

        public override string Diplomacy_RelationWithOthers => "Their relations with others";
        public override string Automation_queue_description => "Will keep repeating until the queue is empty";

        public override string BuildingType_Storehouse_Description => "Workers may drop items here";

        public override string Resource_TypeName_Longbow => "longbow";
        public override string Resource_TypeName_Rapeseed => "rapeseed";
        public override string Resource_TypeName_Hemp => "hemp";

        public override string Resource_BogIronDescription => "Mining iron is more efficient than using bog iron.";


        public override string Resource_FoodSafeGuard_Description => "Safe guard. Will maximize the priority of the food production chain, if it falls below {0}.";
        public override string Resource_FoodSafeGuard_Active => "Safe guard is active.";

        public override string GameMenu_NextSong => "Next song";

        public override string BuildingType_Bank => "Bank";
        public override string BuildingType_GoldDelivery_Description => "Send gold to other cities";

        public override string BuildingType_Logistics => "Logistics";
        public override string BuildingType_Logistics_Description => "Upgrade your ability to order buildings";

        public override string BuildingType_Logistics_NationSizeRequirement => "Nation total workforce: {0}";
        public override string Requirements_XItemStorageOfY => "City {0} storage of: {1}";


        public override string XP_UnlockBuildQueue => "Unlock build queue to: {0}";
        public override string XP_UnlockBuilding => "Unlock building: ";
        public override string XP_Upgrade => "Upgrade";

        public override string XP_UpgradeBuildingX => "Upgrade building: {0}";

        /// <summary>
        /// Title for describing the production cycle of farms
        /// </summary>
        public override string BuildHud_PerCycle => "Per cycle";
        public override string BuildHud_MayCraft => "May craft";
        public override string BuildHud_WorkTime => "Work time: {0}";
        public override string BuildHud_GrowTime => "Grow time: {0}";
        public override string BuildHud_Produce => "Produce:";

        public override string BuildHud_Queue => "Allowed build queue: {0}/{1}";

        public override string LandType_Flatland => "Flat land";
        public override string LandType_Water => "Water";
        public override string BuildingType_Wall => "Wall";
        public override string Delivery_AutoReciever_Description => "Will send to the city with lowest amount of resources";

        public override string Hud_On => "On";
        public override string Hud_Off => "Off";

        public override string Hud_Time_Seconds => "{0} seconds";
        public override string Hud_Time_Minutes => "{0} minutes";
        public override string Hud_Undo => "Undo";
        public override string Hud_Redo => "Redo";

        public override string Tag_ViewOnMap => "View tags on map";

        public override string MenuTab_Tag => "Tag";

        public override string Input_Build => "Build";

        public override string FlagEditor_ClearAll => "Clear all";


        public override string CityCulture_Stonemason => "Stonemason";
        public override string CityCulture_Stonemason_Description => "Improved stone collecting";

        public override string CityCulture_Brewmaster => "Brewmaster";
        public override string CityCulture_Brewmaster_Description => "Enhanced beer production";

        public override string CityCulture_Weavers => "Weavers";
        public override string CityCulture_Weavers_Description => "Enhanced light armor production";

        public override string CityCulture_SiegeEngineer => "Siege engineer";
        public override string CityCulture_SiegeEngineer_Description => "More powerful warmachines";

        public override string CityCulture_Armorsmith => "Armorsmith";
        public override string CityCulture_Armorsmith_Description => "Improved iron armor production";

        public override string CityCulture_Noblemen => "Noblemen";
        public override string CityCulture_Noblemen_Description => "More powerful knights";

        public override string CityCulture_Seafaring => "Seafaring";
        public override string CityCulture_Seafaring_Description => "Soldiers with sea specialzation, have stronger ships";

        public override string CityCulture_Backtrader => "Backtrader";
        public override string CityCulture_Backtrader_Description => "Cheaper black market";

        public override string CityCulture_LawAbiding => "Law-abiding";
        public override string CityCulture_LawAbiding_Description => "Gain more tax. No black market.";

        //##2##

        public override string Hud_Advanced => "Advanced";
        public override string Hud_Loading => "Loading...";

        public override string CityOption_LowerGuardSize => "Release guard";
        public override string Hud_Purchase_MinCapacity => "Minimum capacity reached";
        public override string Settings_ResetToDefault => "Reset to default";
        public override string Settings_NewGame => "New game";

        public override string Settings_AdvancedGameSettings => "Advanced Game Settings";
        public override string Settings_FoodMultiplier => "Food multiplier";
        public override string Settings_FoodMultiplier_Description => "How long a worker or soldier lasts on a full stomach. A high value will lower computer performance.";

        public override string Settings_GameMode => "Game mode";

        public override string Settings_Mode_Story => "Full story";
        public override string Settings_Mode_IncludeBoss => "Include Boss Events.";
        public override string Settings_Mode_IncludeAttacks => "Include Random Attacks.";
        public override string Settings_Mode_Sandbox => "Sandbox";
        public override string Settings_Mode_Peaceful => "Peaceful";
        public override string Settings_Mode_Peaceful_Description => "All wars are initiated by the player";

        public override string Lobby_ImportSave => "Import save";

        public override string Lobby_ExportSave => "Export save";
        public override string Lobby_ExportSave_Description => "Creates a copy of the file and places it in the import folder: {0}";

        public override string Resource_CurrentAmount => "Current amount: {0}";
        public override string Resource_MaxAmount_Soft => "Soft Cap (Max Limit): {0}";
        public override string Resource_MaxAmount => "Max limit: {0}";
        public override string Resource_AddPerSec => "Increase Rate: {0} per second";

        public override string Resource_WaterAddLimit => "Water increase rate can't be altered";

        public override string Tutorial_Select_SubTab => "And select category: {0}";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */


        public override string Tutorial_OpenGuardSubTab => "Open a barracks and select category: {0}";
        public override string Tutorial_GuardToWall => "Move a guard to a wall";
        public override string Demo_MissionObjective_Title => "Mission Objective";
        public override string Demo_MissionObjective_Description => "Defend against the attack from the south";
        public override string Demo_Complete_Title => "Demo complete";
        public override string Demo_TimesUp_Title => "Time's up!";
        public override string Demo_EndInOneMinuteDescription => "The demo will end in one minute";

        public override string ArmyOption_NewArmy => "New army";
        public override string ProfileEditor_AltMain => "Alternative main";
        public override string Automation_CheckBoxTitle => "Automated";

        public override string ArmyStructure_ColumnWidth => "Army column width";
        public override string ArmyStructure_ArmyPlacement => "Placement in army";
        public override string ArmyStructure_Row_Front => "Front";
        public override string ArmyStructure_Row_Body => "Body";
        public override string ArmyStructure_Row_Second => "Second";
        public override string ArmyStructure_Row_Behind => "Behind";

        public override string Diplomacy_RelationType_Enemies => "Enemies";

        public override string EventMessage_EnemyAlliance_Title => "Fear of Domination";
        public override string EventMessage_EnemyAlliance => "The nations, fearing your growing power, unite in an alliance against you.";

        public override string Settings_CentralGold => "Central gold";
        public override string Settings_CentralGold_Description => "On: all your gold is in a shared pool for instant use. Off: gold is physical and needs to be transported.";





        public override string InputActionName_StopStart => "Stop/Start";
        public override string InputActionName_ToggleHudDetail => "Toggle HUD Detail";
        public override string InputActionName_NextCity => "Next City";
        public override string InputActionName_NextArmy => "Next Army";
        public override string InputActionName_NextBattle => "Next Battle";
        public override string InputActionName_Build => "Build";
        public override string InputActionName_Copy => "Copy";
        public override string InputActionName_Paste => "Paste";
        public override string InputActionName_Menu => "Menu";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "Previous Color";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "Next Color";
        public override string InputActionName_FlagDesign_PaintBucket => "Paint Bucket";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "Color Picker";
        public override string InputActionName_ControllerFocus => "Focus";
        public override string InputActionName_ControllerCancel => "Cancel";
        public override string InputActionName_ControllerMessageClick => "Message Click";
        public override string InputActionName_ControllerSelect => "Select";
        public override string InputActionName_WASD_UP => "Up";
        public override string InputActionName_WASD_DOWN => "Down";
        public override string InputActionName_WASD_LEFT => "Left";
        public override string InputActionName_WASD_RIGHT => "Right";
        public override string InputActionName_CameraTiltLeft => "Camera Tilt Left";
        public override string InputActionName_CameraTiltRight => "Camera Tilt Right";
        public override string InputActionName_CameraTiltUp => "Camera Tilt Up";
        public override string InputActionName_ZoomInKey => "Zoom In";
        public override string InputActionName_ZoomOutKey => "Zoom Out";




        public override string Settings_Title_Monitor => "Monitor options";
        public override string Settings_Title_Graphics => "Graphic options";
        public override string Settings_Title_Input => "Input";
        public override string Settings_Title_Gameplay => "Gameplay options";
        public override string Settings_PanOnZoom => "Pan on zoom";
        public override string Settings_ScrollSensitivity_Game => "Scroll sensitivity: game";
        public override string Settings_ScrollSensitivity_Menu => "Scroll sensitivity: menu";
        public override string Settings_Blood => "Blood";

        public override string Settings_MasterVolume => "Master Volume";
        public override string Settings_AmbienceVolume => "Ambience Volume";
        public override string Settings_BattleMelody => "Battle Melody";

        public override string Settings_ModelLight => "Model light effect";
        public override string Settings_Particles => "Particle effects";
        public override string Settings_MapLoadSpeed => "Map loading speed";
        public override string Lobby_Category_Options => "Options";
        public override string Lobby_Category_Editor => "Editor";
        public override string Lobby_Category_ExtraModes => "Extra modes";

        public override string Lobby_Editor_MapEditor => "Map editor";
        public override string Lobby_Editor_VoxelEditor => "Voxel editor";

        public override string Lobby_Mode_BattleLab => "Battle lab";
        public override string Lobby_Mode_BattleLab_Description => "Pit any soldiers against each other";
        public override string Lobby_Mode_Commander => "Play Commander";
        public override string Lobby_Mode_Commander_Description => "A small tactical board game";
        public override string Lobby_MusicPlayList => "Music playlist";

        public override string Lobby_GameSetup => "Game setup";
        public override string Lobby_PlayerSetup => "Player setup";
        public override string LobbyDemoMode_Demo => "Demo";

        public override string Lobby_Tutorial => "Tutorial";
        
        public override string LobbyDemoMode_ShortTutorial => "Quick Tutorial";
        public override string LobbyDemoMode_LongTutorial => "Extended Tutorial";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "Wishlist on";


        public override string BattleLab_StartHere => "Start battle here";
        public override string BattleLab_Start => "Start battle";
        public override string BattleLab_Attacker => "Attacker";



        public override string MapGenerator_Name => "Map editor - generate";

        public override string MapType_CustomMap => "Custom Map";
        public override string MapType_GenerateNewMap => "Generate a new map";
        public override string MapGenerator_GenerateAction => "Generate";
        public override string MapGenerator_Terrain_CustomSize => "Custom size";
        public override string MapGenerator_Terrain_StartAs => "Start as";
        public override string MapGenerator_Terrain_ClearPass => "Run Clear Pass";
        public override string MapGenerator_Terrain_BuildPass => "Run Build Pass";
        public override string MapGenerator_Terrain_DigPass => "Run Dig Pass";
        public override string MapGenerator_Terrain_BuildDigLoops => "Build-Dig loop count";
        public override string MapGenerator_Terrain_BuildStrokes => "Build strokes count";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "Measured in paint strokes per 100 tiles";
        public override string MapGenerator_Terrain_DigStrokes => "Dig strokes count";
        public override string MapGenerator_Terrain_CleanUp_Option => "Cleanup of single tiles";
        public override string MapGenerator_Terrain_CleanUpPass => "Run cleanup Pass";



        public override string Economy_ServicemenUpkeep => "Servicemen upkeep: {0}";
        public override string Economy_ServicemenUpkeep_Description => "Upkeep is {0} gold per serviceman";
        public override string Economy_GuardUpkeep_Description => "Upkeep is {0} gold per guard";

        public override string EndScreen_TimeHasEndedTitle => "Time's up";

        public override string Hud_AdvancedSettings => "Advanced settings";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "Cancel";
        public override string Hud_Delete => "Delete";
        public override string Hud_Next => "Next";
        //public override string Hud_None => "None";
        public override string Hud_Apply => "Apply";
        public override string Hud_AllCities => "All cities";
        public override string Hud_Time_Hours => "{0} hours";
        public override string Hud_AddX => "Add {0}";
        public override string Hud_Both => "Both";
        public override string Hud_Direction => "Direction";
        public override string MusicIsBroken => "Music is currently broken";


        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, count: {1}";

        public override string Hud_EffectDoesNotStack => "This effect does not stack";

        public override string Work_SmeltX => "Smelt {0}";

        public override string Info_TotalFoodProduction => "Total food production";
        public override string Info_TotalFoodSpending => "Total food spending";

        public override string Info_FooodAndDeliveryLocation => "By default, workers go to the city hall to eat or drop off items";
        
        public override string Delivery_SendChunk => "Items per Delivery";
        public override string Delivery_SpeedBonus => "Speed bonus: {0}%";

        public override string Delivery_AutoResourceDescription => "Delivers items that has reached the stockpile limit, to cities in need.";

        public override string Conscript_Soldiers_ArmyType => "Army men";
        public override string Conscript_Soldiers_ArmyType_Description => "Recruit soldiers to an adjacent army";
        public override string Conscript_Soldiers_GuardType => "City guard";
        public override string Conscript_Soldiers_GuardType_Description => "Guards are used to fortify walls";
        //-
        public override string Defence_Title => "Defence";
        public override string Defence_GuardPost => "Guard post";

        public override string Defence_WallDescription_Movement => "Hinders enemy movement.";
        public override string Defence_WallDescription_GuardPost => "Guard can be posted here.";
        public override string Defence_AutoAssign => "Auto assign";
        public override string Defence_AutoAssign_Description => "New guards will move to this post";
        public override string Conscript_SplashDamage => "Splash damage";
        public override string Conscript_HighSplashDamage => "High splash damage";

        public override string Conscript_Training_Champion => "Champion";
        public override string Conscript_Training_Legendary => "Legendary";


        public override string Experience_Title => "Experience";
        public override string Experience_TopExperience => "Top experience levels";

        public override string Experience_TimeReductionDescription => "Work time is reduced by {0}% per level";

        public override string ExperienceType_Farm => "Farmer";
        public override string ExperienceType_AnimalCare => "Animal care";
        public override string ExperienceType_HouseBuilding => "House builder";
        public override string ExperienceType_WoodWork => "Wood worker";
        public override string ExperienceType_StoneCutter => "Stone cutter";
        public override string ExperienceType_Mining => "Miner";
        public override string ExperienceType_Transport => "Transport";
        public override string ExperienceType_Cook => "Cook";
        public override string ExperienceType_Fletcher => "Fletcher";
        public override string ExperienceType_RefineOre => "Smelter";
        public override string ExperienceType_Casting => "Casting";
        public override string ExperienceType_CraftMetal => "Smith";
        public override string ExperienceType_CraftArmor => "Armorer";
        public override string ExperienceType_CraftWeapon => "Weapon smith";
        public override string ExperienceType_CraftFuel => "Collier";
        public override string ExperienceType_Chemist => "Chemist";

        public override string ExperienceLevel_1 => "Beginner";
        public override string ExperienceLevel_2 => "Practitioner";
        public override string ExperienceLevel_3 => "Expert";
        public override string ExperienceLevel_4 => "Master";
        public override string ExperienceLevel_5 => "Legendary";

        public override string ExperenceOrDistancePrio_Title => "Worker selection";
        public override string ExperenceOrDistancePrio_Description => "Idle workers will be selected to work either by distance or experience";


        public override string Technology_Description => "Each city has a technology tree. Each technology will unlock buildings and items.";
        public override string Experience_Description => "Workers will gain experience and improve";


        public override string Technology_Title => "Technology";
        public override string Technology_ShareField => "Sharing technology field";

        public override string Technology_GainByNeigborRelation => "For each neighbor city with the technology. And your relation is {0}: {1}";
        public override string Technology_ForEachMaster => "When a {0} reaches an experience level of {1}, in the technology field: {2}";
        public override string Technology_CitySpread => "Your cities will share technology when adjacent: {0}";
        public override string Technology_CityCapture => "Most technolgies are destroyed when a city is captured in battle";

        public override string Technology_AdvancedBuildings => "Advanced buildings";
        public override string Technology_AdvancedFarming => "Advanced farming";
        public override string Technology_AdvancedCasting => "Advanced casting";

        public override string Help_Title => "Help";
        public override string Help_Work_Title => "Work doesn't start";
        public override string Help_Work_Resources => "Buildings need available resources";
        public override string Help_Work_Skill => "The worker needs correct skill level (or higher)";
        public override string Help_Work_Stockpile => "Resource collection will be blocked by a full stockpile";
        public override string Help_Work_Priority => "Work may have low or zero priority";


        public override string Help_Soldiers_Title => "Produce soldiers";
        public override string Help_Soldiers_PlaceBuildingX => "Place building: {0}";
        public override string Help_Soldiers_Workers => "Available workers to recruit";
        public override string Help_Soldiers_Weapon => "A weapon for each soldier";
        public override string Help_Soldiers_StartX => "Start: {0}";


        public override string Hud_SelectHistory => "Select history";

        public override string Hud_PointsPerMinute => "{0} points per minute";
        public override string Hud_PercentValueCost => "The service costs {0}% of the value";

        public override string Hud_Mixed => "Mixed";
        public override string Hud_Distance => "Distance";

        public override string Hud_Unlock => "Unlock";
        public override string Hud_category => "Category";

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public override string Input_StepOneFrame => "Step 1 frame";

        public override string Resource_TypeName_Wagon2Wheel => "Small wagon";
        public override string Resource_TypeName_Wagon4Wheel => "Large wagon";
        public override string Resource_TypeName_Tin => "Tin";
        public override string Resource_TypeName_TinOre => "Tin ore";

        public override string Resource_TypeName_Copper => "Copper";
        public override string Resource_TypeName_CopperOre => "Copper ore";
        public override string Resource_TypeName_SilverOre => "Silver ore";
        public override string Resource_TypeName_Silver => "Silver";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "Raw mithril";
        public override string Resource_TypeName_Mithril => "Mithril";

        public override string Resource_TypeName_BronzeSword => "Bronze sword";
        public override string Resource_TypeName_ShortSword => "Shortsword";
        public override string Resource_TypeName_LongSword => "Longsword";
        public override string Resource_TypeName_HandSpear => "Hand spear";
        public override string Resource_TypeName_Warhammer => "Warhammer";
        public override string Resource_TypeName_MithrilSword => "Mithril sword";
        public override string Resource_TypeName_SlingShot => "Slingshot";
        public override string Resource_TypeName_ThrowingSpear => "Javelin";
        public override string Resource_TypeName_Crossbow => "Crossbow";
        public override string Resource_TypeName_MithrilBow => "Mithril bow";

        public override string Resource_TypeName_CoolingFluid => "Cooling fluid";
        public override string Resource_TypeName_Palisade => "Palisade";
        public override string Resource_TypeName_Toolkit => "Tool kit";

        public override string Resource_TypeName_Sulfur => "Sulfur";
        public override string Resource_TypeName_LeadOre => "Lead ore";
        public override string Resource_TypeName_Lead => "Lead";
        public override string Resource_TypeName_Bronze => "Bronze";
        public override string Resource_TypeName_BloomIron => "Bloomery iron";
        public override string Resource_TypeName_Steel => "Steel";
        public override string Resource_TypeName_CastIron => "Cast iron";

        public override string Resource_TypeName_BlackPowder => "Black powder";
        public override string Resource_TypeName_GunPowder => "Gunpowder";
        public override string Resource_TypeName_LedBullet => "Bullet";

        public override string Resource_TypeName_HandCannon => "Hand cannon";
        public override string Resource_TypeName_HandCulverin => "Hand culverin";
        public override string Resource_TypeName_Rifle => "Rifle";
        public override string Resource_TypeName_Blunderbuss => "Blunderbuss";

        public override string Resource_TypeName_Manuballista => "Manuballista";
        public override string Resource_TypeName_Catapult => "Catapult";
        public override string Resource_TypeName_BatteringRam => "BatteringRam";
        public override string Resource_TypeName_SiegeCannonBronze => "Basilic";
        public override string Resource_TypeName_ManCannonBronze => "Bombard";
        public override string Resource_TypeName_SiegeCannonIron => "Haubitz";
        public override string Resource_TypeName_ManCannonIron => "Cannon";

        public override string Resource_TypeName_PaddedArmor => "Padded armor";
        public override string Resource_TypeName_HeavyPaddedArmor => "Heavy padded armor";

        public override string Resource_TypeName_IronArmor => "Mail armor";
        public override string Resource_TypeName_HeavyIronArmor => "Heavy mail armor";

        public override string Resource_TypeName_BronzeArmor => "Bronze armor";

        public override string Resource_TypeName_LightPlateArmor => "Plate armor";
        public override string Resource_TypeName_FullPlateArmor => "Full plate armor";
        public override string Resource_TypeName_MithrilArmor => "Mithril armor";
        public override string Resource_TypeName_Coin => "Coin";

        public override string UnitType_Warhammer => "Hammer knight";
        
        public override string UnitType_SpearAndShield => "Lineman";

        public override string UnitType_CollectionOfSoldiers => "Soldier Bundle";
        public override string UnitType_CollectionOfArmies => "Army Bundle";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(id {0})";

        public override string BuildHud_AreaEffectTitle => "Area effect";
        public override string BuildHud_BonusRadius => "Bonus radius: {0}";

        public override string BuildHud_BuildTime => "Build time";
        public override string SchoolHud_ToLevel => "To level";
        public override string SchoolHud_TimeDescription => "Time assumes zero experience; it decreases with experience.";
        public override string SchoolHud_SelectSchool => "Select school";
        public override string Upgrade_Order => "Upgrade order";

        public override string Building_ListDescription => "A list of all buildings in this category";

        public override string BuildingType_IsUpgraded => "{0} - upgraded";
        public override string BuildingType_WoodCutter => "Lumber mill";
        public override string BuildingType_Workshop_Description => "Improves work in the area";

        public override string BuildingType_WoodCutter_AreaAffect => "Gain {0}% more wood from trees";

        public override string BuildingType_StoneCutter_AreaAffect => "Gain {0}% more stone";

        public override string BuildingType_StoneCutter => "Stone quarry";

        public override string BuildingType_Embassy => "Embassy";
        public override string BuildingType_Embassy_Description => "For diplomatic relations";

        public override string BuildingType_SoldierBarracks => "Soldier barracks";
        public override string BuildingType_ArcherBarracks => "Archer barracks";
        public override string BuildingType_WarmachineBarracks => "Warmachine barracks";
        public override string BuildingType_GunBarracks => "Gun barracks";
        public override string BuildingType_CannonBarracks => "Cannon barracks";
        public override string BuildingType_KnightsBarracks => "Knights barracks";

        public override string BuildingType_WaterResovoir => "Water reservoir";
        public override string BuildingType_WaterResovoir_Description => "Increases water storage";

        public override string BuildingType_SmeltingFurnace => "Smelting furnace";
        public override string BuildingType_SmeltingFurnace_Description => "Purify ore to metal";

        public override string BuildingType_Foundry => "Foundry";
        public override string BuildingType_Foundry_Description => "Metal casting station";

        public override string BuildingType_Armory => "Armory";
        public override string BuildingType_Armory_Description => "Armor crafting station";
        public override string BuildingType_Chemist => "Chemist";
        public override string BuildingType_Chemist_Description => "Chemicals crafting station";
        public override string BuildingType_CoinMaker => "Coin minter";
        public override string BuildingType_CoinMaker_Description => "Turn metals to money";
        public override string BuildingType_Gunmaker => "Gunmaker";
        public override string BuildingType_Gunmaker_Description => "Crafting station for guns and cannons";

        public override string BuildingType_School_Tab => "School";
        public override string BuildingType_School => "Masters guild";
        public override string BuildingType_School_Description => "Increase the skill level of workers";

        public override string BuildingType_GoldDelivery => "Gold courier";
        public override string BuildingType_Bank_Description => "Gold management";

        public override string DecorType_CobbleStones => "Cobble stones";
        public override string DecorType_Square => "City square";

        public override string DecorType_Garden => "Garden";
        public override string DecorType_Flag => "Flag";
        public override string DecorType_Banner => "Banner";

        public override string BuildingType_DirtRoad => "Dirt road";
        public override string BuildingType_Palisade => "Palisade Fort";

        public override string ResourceType_ServiceMen => "Servicemen";
        public override string BuildingType_ServiceHouse => "Service house";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "Add {0} servicemen";

        public override string BuildingType_GuardOffice => "Guards office";
        public override string BuildingType_GuardOffice_DescriptionAddX => "Increase guard limit by {0}";

        public override string BuildingType_DirtWall => "Dirt wall";
        public override string BuildingType_DirtTower => "Dirt tower";
        public override string BuildingType_WoodWall => "Wood wall";
        public override string BuildingType_WoodTower => "Wood tower";
        public override string BuildingType_StoneWall => "Stone wall";
        public override string BuildingType_StoneTower => "Stone tower";
        public override string BuildingType_StoneGate => "Stone gate";
        public override string BuildingType_StoneHouse => "Stone gate";


        /// <summary>
        /// When listing slight variations, like "Lamp A" and "Lamp B"
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "Pen";
        public override string BuildingToolShape_Area => "Rectangle";
        public override string BuildingToolShape_Line => "Line";
        public override string BuildingToolShape_LShape => "L-shape";


        public override string CityHall_Upgrade => "Upgrade city hall";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "Max supported workers: {0}";

        public override string CityHall_Size_Small => "Village";
        public override string CityHall_Size_Medium => "Town";
        public override string CityHall_Size_Large => "Capital";

        public override string GuardHousingCount => "Guard office housing";
        public override string ServicemenCount => "Servicemen: {0}";


        public override string Work_MiningResource => "Mining {0}";

        public override string MenuTab_Progress => "Progress";

        public override string Automation_AutomateCity => "Automate city";
        public override string Automation_AutomationFocus => "Automation focus";
        public override string Automation_AutomationFocus_Grow => "Grow";
        public override string Automation_AutomationFocus_Export => "Export";
        public override string Automation_AutomationFocus_War => "War";

        public override string CityCulture_Smelters_Description => "Improved ore smelting";
        public override string CityCulture_Smelters => "Smelters";

        public override string CityCulture_Apprentices_Description => "New workers will gain experience from active workers";
        public override string CityCulture_Apprentices => "Apprentices";

        public override string CityCulture_BronzeCasters_Description => "Improved production of bronze and bronze items";
        public override string CityCulture_BronzeCasters => "Bronze casters";

        //DEMO PATCH 1

        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "Dark horde";
        public override string Tutorial_AttackAndDestroyX => "Attack and destroy: {0}";
        public override string Resource_TypeName_Pike => "Pike";


        public override string BattleTrials_Title => "Battle Trials";
        public override string BattleTrials_Description => "Test your tactics in a direct army-versus-army encounter.";


        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "These attacks reduce block chance";

        public override string Conscript_BlockPerSecond => "May block {0} times per second";

        public override string Conscript_BlockDescription => "Soldiers will block most attacks coming from their forward arc";

        public override string Map_CustomSeed => "Map seed";

        public override string Settings_Mode_Spectator => "Spectator";

        public override string Settings_Mode_Spectator_Description => "Just watch";

        public override string Automation_AutomationFocus_NoFocus_Description => "Will build a little bit of everything";

        public override string Automation_AutomationFocus_WillProduce => "Will mainly produce:";

        public override string Help_Food_WhoEats => "All soldiers and workers consume food";

        public override string Help_Food_BigArmy => "A large army can starve out the city in its area";

        public override string Help_Food_DontBuild => "Building more farms doesn't automatically increase food; you need available workers and cook stations to gather and process it";

        public override string Help_Food_UseWater => "Food production requires water";

        public override string Help_Food_Postal => "Make sure your cities support each other by sending food";

        public override string Message_LostCity => "City lost";

        public override string Demo_Description => "A short scenario: defend your city for {0} minutes";


        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "The demo will end in {0} minutes";

        public override string Experience_Required => "Required experience";

        public override string InputActionName_ToggleMenu => "Toggle menu";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "Resources can go below zero and slightly exceed the stockpile limit. The bounds are only enforced when the work queue is created.";

        public override string Work_SelectCategory => "Select Item Category";
        public override string Hud_RemoveFromList => "Remove from List";

        public override string Hud_ReturnToPrevious => "Return";
        public override string Hud_Close => "Close";

        public override string Hud_Low => "Low";
        public override string Hud_Medium => "Medium";
        public override string Hud_High => "High";

        public override string Hud_Copy => "Copy";
        //public override string Hud_Paste => "Paste";
        public override string Hud_Cut => "Cut";
        public override string Hud_SaveCompleted => "Save Completed";

        public override string Settings_WaterMultiplier => "Water Multiplier";
        public override string Settings_WaterMultiplier_Description => "Determines how much water cities produce and store. Higher values reduce computer performance.";

        public override string Settings_ChildMultiplier => "Childbirth Multiplier";
        
        public override string Settings_CraftMultiplier_Description => "Lower values result in faster production.";

        public override string FastProduction => "Fast Production";
        public override string SlowProduction => "Slow Production";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "Will Not Produce";

        //public override string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public override string Automation_AutomationFocus_NoFocus => "All";
        public override string CityAutomation_SoldierQuality => "Soldier Quality";
        public override string CityAutomation_SoldierWeaponType => "Weapon Type";

        public override string WarsResourceGroup_Resources => "Resources";
        public override string WarsResourceGroup_Weapons => "Weapons";

        public override string WarsResourceGroup_AllWeaponTypes => "Mixed";
        public override string WarsResourceGroup_MeleeHandWeapons => "Melee";
        public override string WarsResourceGroup_RangedHandWeapons => "Ranged";
        public override string WarsResourceGroup_Warmachines => "Warmachines";

        public override string FactionSettings_Titel => "Faction-Wide Settings";
        public override string FactionSettings_Description => "Applies to all your cities";

        public override string Conscript_MaxPopulation => "Max Population";
        public override string Conscript_MaxPopulation_Description => "Only recruits when population is maxed out";

        public override string Conscript_FoodAbundance => "Max Food Stock";
        public override string Conscript_FoodAbundance_Description => "Only recruits when food reaches maximum stockpile";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "Set to: On";
        public override string GeneralSetting_Off => "Set to: Off";
        public override string GeneralSetting_AllBuildingsDescription => "Applies to all buildings";

        public override string GeneralSetting_ApplyMessage => "Change applied to {0} buildings";

        public override string MustTurnOffSteamInput => "To use controllers, you must turn off Steam Input.";

        public override string Technology_GainTitle => "Ways to Gain Technology";
        public override string Technology_LevelUp => "Level Up";
        public override string Technology_ForEachLevelUp => "When a worker levels up in the technology field: {0}";

        public override string VoxelEditor_Description => "Create blocky models";

        public override string Editor_Tool => "Tool";
        public override string Editor_SelectOptionsMenu => "Selection Options";
        public override string Editor_Continous => "Continuous"; // corrected spelling
        public override string Editor_Tool_PencilSize => "Pencil Size";
        public override string Editor_Tool_SizeTolerance => "Size Tolerance";
        public override string Editor_Tool_RoundPencil => "Round Pencil";
        public override string Editor_Tool_EdgeSize => "Edge Size";
        public override string Editor_Tool_PercentFill => "Percent Fill";
        public override string Editor_Tool_ClearAbove => "Clear Above";
        public override string Editor_Tool_FillBelow => "Fill Below";
        public override string Editor_UserModels => "User Models";
        public override string Editor_UserModels_Description => "Browse models you have saved";

        public override string Editor_RetailModels => "Retail Models";
        public override string Editor_RetailModels_Description => "Load models from the game";

        public override string Editor_ModTemplates => "Modding Templates";
        public override string Editor_ExportAsOBJ => "Export as .OBJ";
        public override string Editor_SelectAll => "Select All";

        public override string Editor_Canvas_Title => "Canvas";
        public override string Editor_Canvas_Size => "Size";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "Size Presets";
        public override string Editor_Canvas_Move => "Move";
        public override string Editor_Canvas_Move_Up => "Up";
        public override string Editor_Canvas_Move_Down => "Down";
        public override string Editor_Canvas_RotateClockwise => "Rotate Clockwise";
        public override string Editor_Canvas_RotateCounterClockwise => "Rotate Counterclockwise"; // combined into one word
        public override string Editor_Canvas_Mirror => "Mirror";

        public override string Editor_Canvas_RotateFlip_Title => "Rotate/Flip";
        public override string Editor_Canvas_FlipVertical => "Flip Up and Down";
        public override string Editor_Canvas_FlipOrientation => "Flip Lying/Standing";
        public override string Editor_Canvas_ClearAll_Description => "Removes all blocks and frames";

        public override string Editor_Animation => "Animation";
        public override string Editor_Animation_RemoveCurrentFrame => "Remove Current Frame";
        public override string Editor_Animation_AddFrameCopy => "Add Frame as Copy";
        public override string Editor_Animation_AddEmptyFrame => "Add Empty Frame";
        public override string Editor_Animation_MoveDescription => "Change Frame Position";
        public override string Editor_Animation_AllFrames => "All Frames";
        public override string Editor_Animation_AllFrames_ActionDescription => "Perform the same action on all frames";

        public override string Editor_SettingsMenu => "Settings";
        public override string Hud_Exit => "Exit";
        public override string Editor_Canvas_Clear => "Clear";

        public override string Editor_Stamp => "Stamp";
        public override string Editor_StampOtherFrames => "Stamp in Other Frames";
        public override string Editor_StampOtherFrames_Description => "Paste the voxels in these frames"; // "this frames" → "these frames"
        public override string Editor_PasteToFrame => "Paste the voxels in this frame";
        public override string Editor_ClearAllFrames => "Clear in All Frames";
        public override string Editor_ClearOtherFrames => "Clear Other Frames";

        public override string Editor_Settings_MoveSpeed => "Move Speed";
        public override string Editor_Settings_BackgroundColor => "Background Color";
        public override string Editor_Settings_HideHUD => "Hide HUD";

        public override string Editor_Color => "Color";
        public override string Editor_ColorsInUseLabel => "Colors in Use";
        public override string Editor_Color_BrighterPlus => "Brighter +";
        public override string Editor_Color_Brighter => "Brighter";
        public override string Editor_Color_Darker => "Darker";
        public override string Editor_Color_DarkerPlus => "Darker +";
        public override string Editor_Color_RedTint => "Red Tint";
        public override string Editor_Color_Tint => "Tint";
        public override string Editor_Color_GreenTint => "Green Tint";
        public override string Editor_Color_BlueTint => "Blue Tint";
        public override string Editor_Color_YellowTint => "Yellow Tint";
        public override string Editor_Color_PurpleTint => "Purple Tint";
        public override string Editor_NoColor => "Empty";

        public override string Editor_Material => "Material";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "Recolor";
        public override string Editor_Color_RecolorTo => "Recolor To";

        public override string Editor_Material_Set => "Set Material";

        public override string Editor_Preview => "Preview";
        public override string Editor_CombineWithCurrent => "Combine with Current Model";

        public override string Editor_PickedColor => "Picked";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "Immigration Tent";
        public override string BuildingType_ImmigrationTent_Description => "Stores {0} immigrants";
        public override string BuildingType_ReseachCenter => "Research Center"; // fixed typo "Reseach"
        public override string BuildingType_Bookpress => "Book Press";
        public override string BuildingType_Bookpress_Description => "In one research field, all points gained will be shared with all {0} in your other cities.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "Example: When a worker produces {0}, they will increase their {1} skill. When leveling up, it will add points towards the {2} technology since they share the {1} field."; // fixed "Reseach" and plural

        public override string BuildingType_Research_BaseDescription => "Increases technology research.";

        public override string BuildingType_ResearchCenter_Description => "Adds {0} extra technology research points when a worker levels up in the same field.";

        //DEMO PATCH 5
        
         public override string Editor_CropSelection => "Crop to selection";

         public override string Immigrants_DisbandedSoldiers => "Disbanded soldiers will immigrate";
         public override string Immigrants_RefillWorkers => "Quickly refills the workforce";
         public override string Immigrants_UnhousedAreLost => "Immigrants without housing will disappear after some time";
         public override string Editor_VoxelCount => "{0} voxels";

         public override string Editor_Layers_Titel => "Layers";
         public override string Editor_Layers_All => "All layers";
         public override string Editor_LayerNumber => "Layer {0}";

         public override string Editor_Layer_AddEmpty => "Add empty layer";
         public override string Editor_Layer_AddCopy => "Duplicate layer";
         public override string Editor_Layer_Remove => "Remove layer";
         public override string Editor_Layer_MergeDown => "Merge down";
         public override string Editor_IsAnimated => "Animated";
         public override string Editor_ToggleVisible => "Toggle visibility";
         public override string Editor_ToggleAnimatedLayer => "Toggle animated layer";
         public override string Editor_Projects => "Project files";
         public override string ProfileEditor_ReplaceMaterial => "Profile color: {0}";

         public override string ProfileEditor_ProfileColors_Label => "Profile colors";
         public override string ProfileEditor_TunicColor => "Tunic color";
         public override string ProfileEditor_PantsColor => "Pants color";
         public override string ProfileEditor_LeaderColor => "Leader color";

         public override string MapStartAs_Water => "Water";
         public override string MapStartAs_Land => "Land";
         public override string MapStartAs_Circle => "Circle";

         public override string Hud_NeedToBeAssigned => "Needs assignment";
         public override string Hud_CommitAssignment => "Assign";
         public override string Technology_NoAvailableResearch => "No available research";

         public override string Research_Tab => "Research";

        //5.2
        public override string BuildCategory_General => "General";
        public override string BuildCategory_Military => "Military";
        public override string BuildCategory_Decoration => "Decoration";
        public override string BuildCategory_Upgrade => "Upgrade";
        public override string Work_NoMines => "No mines";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "Display name";
        public override string HUD_Filter => "Filter";
        public override string HUD_Scale => "Scale";
        public override string HUD_Tags => "Tags";
        public override string HUD_ClickToCancel => "Click to cancel";

        public override string ObjectTag_Description => "Add a symbol on the map";
        public override string HudPins => "HUD pins";
        public override string HudPins_Description => "Stick information to the screen";

        public override string Lobby_PlayerProfileNumbered => "Profile {0}";
        public override string Lobby_CharacterCreationNumbered => "Character {0}";
        public override string Lobby_PlayerProfileEdit => "Edit player profile";

        public override string Editor_ConvertAnimationToLayers => "Convert animation to layers";
        public override string Editor_StampAllFrames => "Stamp on all frames";

        public override string Editor_DisplayOptions => "Display options";
        public override string Editor_CharacterCreator => "Character creator";
        public override string Editor_CharacterCreator_Description => "Military model appearance editor";
        public override string Editor_HatGenre => "Hat display mode";
        public override string Editor_HatGenre_FollowWeapon => "Follow weapon";
        public override string Editor_HatGenre_Uniform => "Uniform";
        public override string Editor_CopyPasteSelectedColor => "Copy from selected color";

        public override string Character_Accessories => "Accessories";
        public override string Character_Hat => "Hat";
        public override string Character_Head => "Head";
        public override string Character_Body => "Body";
        public override string Character_Arms => "Arms";
        public override string Character_Back => "Back";
        public override string Character_Face => "Face";

        public override string BuildingType_Tavern => "Common Hall";

        public override string Settings_CraftMultiplier => "Craft time multiplier";
        public override string Settings_ChildMultiplier_Description => "Increases the speed at which new workers are added";

        public override string Settings_CasualControls => "Casual player controls";
        public override string Settings_CasualControls_Description => "Simplifies gameplay by reducing choices to key decisions. Only money is used as a resource.";

        public override string Settings_AdvancedControls => "Advanced controls";
        public override string Settings_AdvancedControls_Description => "The full resource management experience.";

        public override string WarsResourceGroup_Metal => "Metal";
        public override string Work_Craft => "Craft";
        public override string Work_OnlyCraftOnFullStock => "Only craft on full stockpile";

        public override string ExperienceType_Smelting => "Smelting";
        public override string Category_Optimize => "Optimize";
        public override string BuildCategory_Road => "Road";
        public override string XP_UnlockBuildPrio => "Unlock build priority: {0}";
        public override string Technology_ModernFarming => "Modern farming";

        public override string ExportImportDescription => "For sharing save files with other player, all files are this folder: {0}";

        public override string CityCultureDescription => "Culuture will give it a special bonus to the city";

        public override string UnitType_CloseRangeRifle => "Arquebusier";
        public override string UnitType_LongRangeRifle => "Musketeer";
        public override string UnitType_Skirmisher => "Skirmisher";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "Lunari archer";
        public override string UnitType_MithrilSwordsman => "Lunari knight";

        public override string Defence_AutoAssign_Towers => "Assign towers";

        public override string EventMessage_DesertersText_Food => "Hungry soldiers are deserting from your army";

        public override string Tutorial_CasualRecruitSoldiers => "Purchase one soldier group";
    }
}
