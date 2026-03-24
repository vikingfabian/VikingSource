using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    abstract partial class AbsLanguage
    {
        public abstract string Help_Work_Automatic { get; }
        public abstract string Tutorial_SecondCity { get; }
//Spring patch

        public abstract string InputAction_SkipAutomated { get; }

        public abstract string Resource_WaterReason { get; }
        public abstract string BuildingType_Orchard { get; }
        public abstract string BuildingType_ManorLord { get; }
        public abstract string BuildingType_ManorLord_Description { get; }

        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public abstract string Diplomacy_EndRelations { get; }

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public abstract string ItemSource { get; }

        public abstract string ItemSource_Terrain { get; }
        public abstract string ItemSource_Farm { get; }
        public abstract string ItemSource_CraftStation { get; }
        public abstract string ItemSource_Gathering { get; }

        public abstract string CityCulture_Nomad { get; }

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public abstract string Hud_ChangeFactor { get; }

        public abstract string Hud_Purchase_LowXCost { get; }

        public abstract string WorkQueue_Title { get; }
        public abstract string WorkQueue_Length { get; }
        public abstract string WorkQueue_ActiveWorkers { get; }
        public abstract string WorkQueue_IdleWorkers { get; }

        public abstract string WorkTeam_Size { get; }

        public abstract string ObjectUi_ViewOnMap { get; }
        public abstract string ObjectUi_StuckBuildOrders { get; }
        public abstract string Hud_AllArmies { get; }

        public abstract string Hud_CurrentPage { get; }
        public abstract string Hud_AllPages { get; }
        public abstract string Hud_ToAllCities { get; }
        public abstract string Hud_ToFaction { get; }
        public abstract string Hud_FromFaction { get; }
        public abstract string Hud_FactionWide { get; }

        /// <summary>
        /// This start a new city
        /// </summary>
        public abstract string Action_PlaceSettlement { get; }

        public abstract string Editor_Animation_RemoveAllFramesButThis { get; }


        //Winter patch 3
        public abstract string Hud_Purchase_AllBuildings { get; }
        public abstract string Hud_Purchase_AllTech { get; }
        public abstract string BuildingType_CasualBarracks_Description { get; }


        //Winter update patch + spring

        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public abstract string Hud_Purchase_ResourceCost { get; }

        //public abstract string DisplayMode { get; }
        //public abstract string DisplayMode_Windowed { get; }
        //public abstract string DisplayMode_BorderlessFullscreen { get; }

        //public abstract string GameSettings_RenderedMouseCursor { get; }
        //public abstract string GameSettings_MuteControllerDisconnect { get; }

        public abstract string Delivery_MaxDistance { get; }
        public abstract string Tutorial_WillTakeAWhile { get; }

        /// <summary>
        /// 0: name of building
        /// </summary>
        public abstract string Tutorial_WaitFor { get; }
        public abstract string GameOverResults { get; }

        public abstract string UnitType_UnclaimedLand { get; }
        public abstract string UnitType_Settler { get; }
        public abstract string UnitType_Settler_Description { get; }
        public abstract string Resource_ConsumedProduced { get; }
        public abstract string InputActionName_PlaceTarget { get; }

        public abstract string FactionStartSize { get; }
        public abstract string FactionStartSize_Full { get; }
        public abstract string FactionStartSize_OneCity { get; }
        public abstract string FactionStartSize_Settler { get; }

        //Winter update
        public abstract string Error_SoundInitFailure { get; }
        public abstract string Resource_StockpileLimit { get; }
        public abstract string GameMode_QuickMatch { get; }
        public abstract string GameMode_QuickMatch_Description { get; }
        public abstract string Lobby_PlayerCount { get; }
        public abstract string Lobby_TwoTeams { get; }
        public abstract string Hud_Produce { get; }
        public abstract string Tutorial_WaitForWorkerLevel { get; }

        /// <summary>
        /// 0: Production item, 1: School
        /// </summary>
        public abstract string Tutorial_PracticeOrSchool { get; }
        public abstract string Tutorial_AddTag { get; }
        public abstract string Tutorial_AddPin { get; }
        public abstract string Tutorial_SelectMostTrees { get; }
        public abstract string Tutorial_SelectACityWithX { get; }

        /// <summary>
        /// Will continue on another sentence "Select a city"
        /// </summary>
        public abstract string Tutorial_Select_NotCapital { get; }

        public abstract string Tutorial_HighPriority { get; }

        public abstract string Tutorial_SetXPriorityToY { get; }
        public abstract string Tutorial_AdvisorMission { get; }

        public abstract string Tutorial_AdvisorDescription { get; }

        public abstract string Tutorial_EndAdvisor { get; }

        public abstract string Tutorial_AdvisorCompleteTitle { get; }
        public abstract string Tutorial_AdvisorCompleteMessage { get; }

        public abstract string Hud_Search { get; }

        public abstract string DifficultyDescription_ExtremeAggression { get; }

        public abstract string MapFilter { get; }

        public abstract string Settings_TechMultiplier { get; }

        public abstract string EndScreen_MatchComplete { get; }

        /// <summary>
        /// Theme: Four headed dragon symbol. Known for having an unpenetrable castle.
        /// </summary>
        public abstract string FactionName_DragonGem { get; }

        /// <summary>
        /// Theme: Easter egg for december. "Tomten" is an old nordic name for father christmas
        /// </summary>
        public abstract string FactionName_Tomten { get; }

        /// <summary>
        /// Theme: The blessed folk. A horde like farmers faction.
        /// </summary>
        public abstract string FactionName_Hælfolc { get; }

        /// <summary>
        /// The Iron Saints, people who guard a mountain pass against evil.
        /// </summary>
        public abstract string FactionName_AerimAngren { get; }

        public abstract string HUD_NotAvailbleInX { get; }

        public abstract string InputActionName_MiniMap { get; }
        //-
        public abstract string GameMenu_ControllerDisconnected { get; }
        public abstract string BuildingType_Wall_Description { get; }
        public abstract string BuildingType_Wall_Siege { get; }
        public abstract string Conscript_BlockChance { get; }
        public abstract string Battle_DeclarWarReminder { get; }
        //--

        public abstract string MyLanguage { get; }

        public abstract string Language_ItemCountPresentation { get; }

        public abstract string Lobby_Language { get; }
        public abstract string Lobby_Start { get; }
        public abstract string Lobby_LocalMultiplayerEdit { get; }

        public abstract string Lobby_LocalMultiplayerTitle { get; }
        public abstract string Lobby_LocalMultiplayerControllerRequired { get; }
        public abstract string Lobby_NextScreen { get; }

        public abstract string Lobby_FlagSelectTitle { get; }
        public abstract string Lobby_FlagNumbered { get; }
        public abstract string Lobby_FlagEdit { get; }


        public abstract string Lobby_WarningTitle { get; }
        public abstract string Lobby_IgnoreWarning { get; }
        public abstract string Lobby_PlayerWithoutInputWarning { get; }

        public abstract string Lobby_Extra { get; }
        public abstract string Lobby_Extra_NoSupportWarning { get; }

        public abstract string Lobby_MapSizeTitle { get; }
        public abstract string Lobby_MapSizeOptTiny { get; }
        public abstract string Lobby_MapSizeOptSmall { get; }
        public abstract string Lobby_MapSizeOptMedium { get; }
        public abstract string Lobby_MapSizeOptLarge { get; }
        public abstract string Lobby_MapSizeOptHuge { get; }
        public abstract string Lobby_MapSizeOptEpic { get; }
        public abstract string Lobby_MapSizeDesc { get; }
        public abstract string Lobby_ExitGame { get; }

        //public abstract string Lobby_GameVersion { get; }

        public abstract string Player_DefaultName { get; }

        public abstract string FlagEditor_Description { get; }
        public abstract string FlagEditor_Bucket { get; }
        public abstract string ProfileEditor_OptionsMenu { get; }
        public abstract string ProfileEditor_FlagColorsTitle { get; }
        public abstract string ProfileEditor_MainColor { get; }
        public abstract string ProfileEditor_Detail1Color { get; }
        public abstract string ProfileEditor_Detail2Color { get; }
        public abstract string ProfileEditor_PeopleColorsTitle { get; }
        public abstract string ProfileEditor_SkinColor { get; }
        public abstract string ProfileEditor_HairColor { get; }
        public abstract string ProfileEditor_PickColor { get; }
        public abstract string ProfileEditor_MoveImage { get; }
        public abstract string ProfileEditor_MoveImageLeft { get; }
        public abstract string ProfileEditor_MoveImageRight { get; }
        public abstract string ProfileEditor_MoveImageUp { get; }
        public abstract string ProfileEditor_MoveImageDown { get; }
        public abstract string ProfileEditor_DiscardAndExit { get; }
        public abstract string ProfileEditor_DiscardAndExitDescription { get; }
        public abstract string Hud_SaveAndExit { get; }
        public abstract string ProfileEditor_Hue { get; }
        public abstract string ProfileEditor_Lightness { get; }
        public abstract string ProfileEditor_NextColorType { get; }


        public abstract string Hud_GameSpeedLabel { get; }
        public abstract string Input_GameSpeed { get; }

        public abstract string Hud_TotalIncome { get; }

        public abstract string Hud_Upkeep { get; }

        public abstract string Hud_ArmyUpkeep { get; }

        public abstract string Hud_GuardCount { get; }

        public abstract string Hud_IncreaseMaxGuardCount { get; }

        public abstract string Hud_GuardCount_MustExpandCityMessage { get; }

        public abstract string Hud_SoldierCount { get; }

        public abstract string Hud_SoldierGroupsCount { get; }

        public abstract string Hud_StrengthRating { get; }

        public abstract string Hud_TotalStrengthRating { get; }

        public abstract string Hud_Immigrants { get; }

        public abstract string Hud_CityCount { get; }
        public abstract string Hud_ArmyCount { get; }


        public abstract string Hud_XTimes { get; }

        public abstract string Hud_PurchaseTitle_Requirement { get; }
        public abstract string Hud_PurchaseTitle_Cost { get; }
        public abstract string Hud_PurchaseTitle_Gain { get; }

        public abstract string Hud_Purchase_ResourceCostOfAvailable { get; }

        public abstract string Hud_Purchase_CostWillIncreaseByX { get; }

        public abstract string Hud_Purchase_MaxCapacity { get; }

        public abstract string Hud_CompareMilitaryStrength_YourToOther { get; }

        public abstract string Hud_Date { get; }

        public abstract string Hud_TimeSpan { get; }

        public abstract string Hud_Battle { get; }

        //public abstract string Input_NextCity { get; }
        //public abstract string Input_NextArmy { get; }
        //public abstract string Input_NextBattle { get; }

        public abstract string Input_Pause { get; }
        public abstract string Input_ResumePaused { get; }

        public abstract string ResourceType_Gold { get; }
        public abstract string ResourceType_Workers { get; }
        public abstract string ResourceType_Workers_Description { get; }
        public abstract string ResourceType_DiplomacyPoints { get; }
        public abstract string ResourceType_DiplomacyPoints_WithSoftAndHardLimit { get; }

        public abstract string Building_NobleHouse { get; }
        public abstract string Building_NobleHouse_DiplomacyPointsAdd { get; }
        public abstract string Building_NobleHouse_DiplomacyPointsLimit { get; }
        public abstract string Building_NobleHouse_UnlocksKnight { get; }

        public abstract string Building_BuildAction { get; }
        public abstract string Building_IsBuilt { get; }

        public abstract string Building_DarkFactory { get; }

        public abstract string Settings_TotalDifficulty { get; }
        public abstract string Settings_DifficultyLevel { get; }

        public abstract string Settings_GenerateMaps { get; }
        public abstract string Settings_GenerateMaps_SlowDescription { get; }
        public abstract string Settings_AllowPause { get; }
        public abstract string Settings_BossEvents { get; }
        public abstract string Settings_BossEvents_SandboxDescription { get; }

        public abstract string Automation_Title { get; }
        public abstract string Automation_InfoLine_MaxWorkforce { get; }
        public abstract string Automation_InfoLine_NegativeIncome { get; }
        public abstract string Automation_InfoLine_Priority { get; }
        public abstract string Automation_InfoLine_PurchaseSpeed { get; }

        public abstract string HudAction_BuyItem { get; }


        public abstract string Diplomacy_RelationType { get; }
        public abstract string Diplomacy_RelationToOthers { get; }
        public abstract string Diplomacy_RelationType_Servant { get; }
        public abstract string Diplomacy_RelationType_Ally { get; }
        public abstract string Diplomacy_RelationType_Good { get; }
        public abstract string Diplomacy_RelationType_Peace { get; }
        public abstract string Diplomacy_RelationType_Neutral { get; }
        public abstract string Diplomacy_RelationType_Truce { get; }
        public abstract string Diplomacy_RelationType_War { get; }
        public abstract string Diplomacy_RelationType_TotalWar { get; }

        public abstract string Diplomacy_SpeakTermIs { get; }
        public abstract string Diplomacy_SpeakTerms_Good { get; }
        public abstract string Diplomacy_SpeakTerms_Normal { get; }
        public abstract string Diplomacy_SpeakTerms_Bad { get; }
        public abstract string Diplomacy_SpeakTerms_None { get; }

        public abstract string Diplomacy_ForgeNewRelationTo { get; }
        public abstract string Diplomacy_OfferPeace { get; }
        public abstract string Diplomacy_OfferAlliance { get; }
        public abstract string Diplomacy_PlayerOfferAlliance { get; }
        public abstract string Diplomacy_AcceptRelationOffer { get; }

        public abstract string Diplomacy_NewRelationOffered { get; }

        public abstract string Diplomacy_AbsorbServant { get; }

        public abstract string Diplomacy_LightSide { get; }

        public abstract string Diplomacy_TruceTimeLength { get; }
        public abstract string Diplomacy_ExtendTruceAction { get; }
        public abstract string Diplomacy_TruceExtendTimeLength { get; }

        public abstract string Diplomacy_BreakingRelationCost { get; }

        public abstract string Diplomacy_AllyDescription { get; }
        public abstract string Diplomacy_GoodRelationDescription { get; }

        public abstract string Diplomacy_ServantRequirement_XStrongerMilitary { get; }
        public abstract string Diplomacy_ServantRequirement_HopelessWar { get; }
        public abstract string Diplomacy_ServantRequirement_MaxCities { get; }

        public abstract string Diplomacy_ServantPriceWillRise { get; }
        public abstract string Diplomacy_ServantGainAbsorbFaction { get; }

        public abstract string Diplomacy_WarDeclarationTitle { get; }
        public abstract string Diplomacy_TruceEndTitle { get; }

        public abstract string Statistics_Title { get; }
        public abstract string EndGameStatistics_Time { get; }
        public abstract string EndGameStatistics_SoldiersRecruited { get; }
        public abstract string EndGameStatistics_FriendlySoldiersLost { get; }
        public abstract string EndGameStatistics_EnemySoldiersKilled { get; }
        public abstract string EndGameStatistics_SoldiersDeserted { get; }
        public abstract string EndGameStatistics_CitiesCaptured { get; }
        public abstract string EndGameStatistics_CitiesLost { get; }
        public abstract string EndGameStatistics_BattlesWon { get; }
        public abstract string EndGameStatistics_BattlesLost { get; }
        public abstract string EndGameStatistics_WarsStartedByYou { get; }
        public abstract string EndGameStatistics_WarsStartedByEnemy { get; }
        public abstract string EndGameStatistics_AlliedFactions { get; }
        public abstract string EndGameStatistics_ServantFactions { get; }


        public abstract string UnitType_Army { get; }
        public abstract string UnitType_SoldierGroup { get; }
        public abstract string UnitType_City { get; }

        public abstract string UnitType_ArmyCollectionAndCount { get; }

        public abstract string UnitType_Soldier { get; }
        public abstract string UnitType_Sailor { get; }
        public abstract string UnitType_Folkman { get; }
        public abstract string UnitType_Spearman { get; }
        public abstract string UnitType_HonorGuard { get; }
        public abstract string UnitType_Pikeman { get; }
        public abstract string UnitType_Knight { get; }
        public abstract string UnitType_Archer { get; }
        public abstract string UnitType_Crossbow { get; }
        public abstract string UnitType_Ballista { get; }
        public abstract string UnitType_Trollcannon { get; }
        public abstract string UnitType_GreenSoldier { get; }
        public abstract string UnitType_Viking { get; }
        public abstract string UnitType_DarkLord { get; }
        public abstract string UnitType_Bannerman { get; }
        public abstract string UnitType_WarshipWithUnit { get; }

        public abstract string UnitType_Description_Soldier { get; }
        public abstract string UnitType_Description_Sailor { get; }
        public abstract string UnitType_Description_Folkman { get; }
        public abstract string UnitType_Description_HonorGuard { get; }
        public abstract string UnitType_Description_Knight { get; }
        public abstract string UnitType_Description_Archer { get; }
        public abstract string UnitType_Description_Crossbow { get; }
        public abstract string UnitType_Description_Ballista { get; }
        public abstract string UnitType_Description_GreenSoldier { get; }
        public abstract string UnitType_Description_DarkLord { get; }

        public abstract string SoldierStats_Title { get; }
        public abstract string SoldierStats_GroupCountAndSoldierCount { get; }
        public abstract string SoldierStats_AttackStrengthLandSeaCity { get; }
        public abstract string SoldierStats_Health { get; }

        public abstract string SoldierStats_SpeedBonusLand { get; }
        public abstract string SoldierStats_SpeedBonusSea { get; }
        public abstract string SoldierStats_RecruitTrainingTimeMinutes { get; }

        public abstract string ArmyOption_Halt { get; }
        public abstract string ArmyOption_Disband { get; }
        public abstract string ArmyOption_Divide { get; }
        public abstract string ArmyOption_RemoveX { get; }
        public abstract string ArmyOption_DisbandAll { get; }
        public abstract string ArmyOption_XGroupsOfType { get; }

        public abstract string ArmyOption_MergeAllArmies { get; }

        public abstract string ArmyOption_SendToX { get; }
        public abstract string ArmyOption_SendToNewArmy { get; }
        public abstract string ArmyOption_SendX { get; }
        public abstract string ArmyOption_SendAll { get; }
        public abstract string ArmyOption_DivideHalf { get; }
        public abstract string ArmyOption_MergeArmies { get; }


        public abstract string UnitType_Recruit { get; }
        public abstract string CityOption_RecruitType { get; }
        public abstract string CityOption_XMercenaries { get; }
        public abstract string Hud_MercenaryMarket { get; }


        public abstract string CityOption_BuyXMercenaries { get; }

        public abstract string CityOption_Mercenaries_Description { get; }
        public abstract string CityOption_ExpandWorkForce { get; }
        public abstract string CityOption_ExpandWorkForce_IncreaseMax { get; }
        public abstract string CityOption_ExpandGuardSize { get; }

        public abstract string CityOption_Damages { get; }
        public abstract string CityOption_Repair { get; }
        public abstract string CityOption_RepairGain { get; }
        public abstract string CityOption_Repair_Description { get; }


        public abstract string CityOption_BurnItDown { get; }
        public abstract string CityOption_BurnItDown_Description { get; }

        public abstract string FactionName_DarkLord { get; }
        public abstract string FactionName_DarkFollower { get; }
        public abstract string FactionName_UnitedKingdom { get; }
        public abstract string FactionName_Greenwood { get; }
        public abstract string FactionName_EasternEmpire { get; }
        public abstract string FactionName_NordicRealm { get; }
        public abstract string FactionName_BearClaw { get; }
        public abstract string FactionName_NordicSpur { get; }
        public abstract string FactionName_IceRaven { get; }
        public abstract string FactionName_Dragonslayer { get; }
        public abstract string FactionName_SouthHara { get; }
        public abstract string FactionName_GenericAi { get; }
        public abstract string FactionName_Player { get; }

        public abstract string EventMessage_HaraMercenaryTitle { get; }
        public abstract string EventMessage_HaraMercenaryText { get; }
        public abstract string EventMessage_ProphesyTitle { get; }
        public abstract string EventMessage_ProphesyText { get; }
        public abstract string EventMessage_FinalBossEnterTitle { get; }
        public abstract string EventMessage_FinalBossEnterText { get; }
        public abstract string EventMessage_FinalBattleTitle { get; }
        public abstract string EventMessage_FinalBattleText { get; }

        public abstract string EventMessage_DesertersTitle { get; }
        public abstract string EventMessage_DesertersText_Money { get; }

        public abstract string DifficultyDescription_AiAggression { get; }
        public abstract string DifficultyDescription_BossSize { get; }
        public abstract string DifficultyDescription_BossEnterTime { get; }
        public abstract string DifficultyDescription_AiEconomy { get; }
        public abstract string DifficultyDescription_AiDelay { get; }
        public abstract string DifficultyDescription_DiplomacyDifficulty { get; }
        public abstract string DifficultyDescription_MercenaryCost { get; }
        public abstract string DifficultyDescription_HonorGuards { get; }


        public abstract string EndScreen_VictoryTitle { get; }

        public abstract List<string> EndScreen_VictoryQuotes { get; }

        public abstract string EndScreen_DominationVictoryQuote { get; }

        public abstract string EndScreen_FailTitle { get; }

        public abstract List<string> EndScreen_FailureQuotes { get; }

        public abstract string EndScreen_WatchEpilogue { get; }

        public abstract string EndScreen_Epilogue_Title { get; }
        public abstract string EndScreen_Epilogue_Text { get; }


        public abstract string GameMenu_WatchPrologue { get; }
        public abstract string Prologue_Title { get; }
        public abstract List<string> Prologue_TextLines { get; }

        public abstract string GameMenu_Title { get; }
        public abstract string GameMenu_ContinueGame { get; }
        public abstract string GameMenu_Resume { get; }

        public abstract string GameMenu_ExitGame { get; }

        public abstract string Hud_Save { get; }
        public abstract string GameMenu_SaveStateWarnings { get; }
        public abstract string GameMenu_LoadState { get; }
        public abstract string GameMenu_ContinueFromSave { get; }

        public abstract string GameMenu_AutoSave { get; }

        public abstract string GameMenu_Load_PlayerCountError { get; }

        public abstract string Progressbar_MapLoadingState { get; }

        public abstract string Progressbar_ProgressComplete { get; }

        public abstract string Progressbar_MapLoadingState_GeneratingPercentage { get; }

        public abstract string Progressbar_MapLoadingState_LoadPart { get; }

        public abstract string Progressbar_SaveProgress { get; }
        public abstract string Progressbar_LoadProgress { get; }
        public abstract string Progressbar_PressAnyKey { get; }

        public abstract string Tutorial_MenuOption { get; }
        public abstract string Tutorial_MissionsTitle { get; }
        public abstract string Tutorial_Mission_BuySoldier { get; }
        public abstract string Tutorial_Mission_MoveArmy { get; }

        public abstract string Tutorial_CompleteTitle { get; }
        public abstract string Tutorial_CompleteMessage { get; }

        public abstract string Tutorial_SelectInput { get; }
        public abstract string Tutorial_MoveInput { get; }


        public abstract string Hud_Versus { get; }

        public abstract string Hud_WardeclarationTitle { get; }

        public abstract string ArmyOption_Attack { get; }

        public abstract string Settings_ButtonMapping { get; }
        //public abstract string Input_ToggleHudDetail { get; }
        //public abstract string Input_ToggleHudFocus { get; }
        //public abstract string Input_ClickMessage { get; }
        //public abstract string Input_Up { get; }
        //public abstract string Input_Down { get; }
        //public abstract string Input_Left { get; }
        //public abstract string Input_Right { get; }
        public abstract string Input_Source_Keyboard { get; }
        public abstract string Input_Source_Controller { get; }


        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */


        public abstract string CityMenu_SalePricesTitle { get; }
        public abstract string Blueprint_Title { get; }
        public abstract string Resource_Tab_Overview { get; }
        public abstract string Resource_Tab_Stockpile { get; }

        public abstract string Resource { get; }
        public abstract string Resource_StockPile_Info { get; }
        public abstract string Resource_TypeName_Water { get; }
        public abstract string Resource_TypeName_Wood { get; }
        public abstract string Resource_TypeName_Fuel { get; }
        public abstract string Resource_TypeName_Stone { get; }
        public abstract string Resource_TypeName_RawFood { get; }
        public abstract string Resource_TypeName_Food { get; }
        public abstract string Resource_TypeName_Beer { get; }
        public abstract string Resource_TypeName_Wheat { get; }
        public abstract string Resource_TypeName_Linen { get; }
        //public abstract string Resource_TypeName_SkinAndLinen { get; }
        public abstract string Resource_TypeName_IronOre { get; }
        public abstract string Resource_TypeName_GoldOre { get; }
        public abstract string Resource_TypeName_Iron { get; }

        public abstract string Resource_TypeName_SharpStick { get; }
        public abstract string Resource_TypeName_Sword { get; }
        public abstract string Resource_TypeName_KnightsLance { get; }
        public abstract string Resource_TypeName_TwoHandSword { get; }
        public abstract string Resource_TypeName_Bow { get; }

        public abstract string Resource_TypeName_LightArmor { get; }
        public abstract string Resource_TypeName_MediumArmor { get; }
        public abstract string Resource_TypeName_HeavyArmor { get; }

        public abstract string ResourceType_Children { get; }

        public abstract string BuildingType_DefaultName { get; }
        public abstract string BuildingType_WorkerHut { get; }
        public abstract string BuildingType_Tavern { get; }
        public abstract string BuildingType_Brewery { get; }
        public abstract string BuildingType_Postal { get; }
        public abstract string BuildingType_Recruitment { get; }
        public abstract string BuildingType_Barracks { get; }
        public abstract string BuildingType_PigPen { get; }
        public abstract string BuildingType_HenPen { get; }
        public abstract string BuildingType_WorkBench { get; }
        public abstract string BuildingType_Carpenter { get; }
        public abstract string BuildingType_CoalPit { get; }
        public abstract string DecorType_Statue { get; }
        public abstract string DecorType_Pavement { get; }
        public abstract string BuildingType_Smith { get; }
        public abstract string BuildingType_Cook { get; }
        public abstract string BuildingType_Storage { get; }

        public abstract string BuildingType_ResourceFarm { get; }

        public abstract string BuildingType_WorkerHut_DescriptionLimitX { get; }
        public abstract string BuildingType_Tavern_Description { get; }
        public abstract string BuildingType_Tavern_Brewery { get; }
        public abstract string BuildingType_Postal_Description { get; }
        public abstract string BuildingType_Recruitment_Description { get; }
        public abstract string BuildingType_Barracks_Description { get; }
        public abstract string BuildingType_PigPen_Description { get; }
        public abstract string BuildingType_HenPen_Description { get; }
        public abstract string BuildingType_Decor_Description { get; }
        public abstract string BuildingType_Farm_Description { get; }

        public abstract string BuildingType_Cook_Description { get; }
        public abstract string BuildingType_Bench_Description { get; }

        public abstract string BuildingType_Smith_Description { get; }
        public abstract string BuildingType_Carpenter_Description { get; }

        public abstract string BuildingType_Nobelhouse_Description { get; }
        public abstract string BuildingType_CoalPit_Description { get; }
        public abstract string BuildingType_Storage_Description { get; }

        public abstract string MenuTab_Info { get; }
        public abstract string MenuTab_Work { get; }
        public abstract string MenuTab_Recruit { get; }
        public abstract string MenuTab_Resources { get; }
        public abstract string MenuTab_Trade { get; }
        public abstract string MenuTab_Build { get; }
        public abstract string MenuTab_Economy { get; }
        public abstract string MenuTab_Delivery { get; }

        public abstract string MenuTab_Build_Description { get; }
        public abstract string MenuTab_BlackMarket_Description { get; }
        public abstract string MenuTab_Resources_Description { get; }
        public abstract string MenuTab_Work_Description { get; }
        public abstract string MenuTab_Automation_Description { get; }

        public abstract string BuildHud_OutsideCity { get; }
        public abstract string BuildHud_OutsideFaction { get; }

        public abstract string BuildHud_OccupiedTile { get; }

        public abstract string Build_PlaceBuilding { get; }
        public abstract string Build_DestroyBuilding { get; }
        public abstract string Build_ClearTerrain { get; }

        public abstract string Build_ClearOrders { get; }
        public abstract string Build_Order { get; }
        public abstract string Build_OrderQue { get; }
        public abstract string Build_AutoPlace { get; }

        public abstract string Work_OrderPrioTitle { get; }
        public abstract string Work_OrderPrioDescription { get; }

        public abstract string Work_OrderPrio_No { get; }
        public abstract string Work_OrderPrio_Min { get; }
        public abstract string Work_OrderPrio_Max { get; }

        public abstract string Work_Move { get; }

        public abstract string Work_GatherXResource { get; }
        public abstract string Work_CraftX { get; }
        public abstract string Work_Farming { get; }
        public abstract string Work_Mining { get; }
        public abstract string Work_Trading { get; }

        public abstract string Work_AutoBuild { get; }

        public abstract string WorkerHud_WorkType { get; }
        public abstract string WorkerHud_Carry { get; }
        public abstract string WorkerHud_Energy { get; }
        public abstract string WorkerStatus_Exit { get; }
        public abstract string WorkerStatus_Eat { get; }
        public abstract string WorkerStatus_Till { get; }
        public abstract string WorkerStatus_Plant { get; }
        public abstract string WorkerStatus_Gather { get; }
        public abstract string WorkerStatus_PickUpResource { get; }
        public abstract string WorkerStatus_DropOff { get; }
        public abstract string WorkerStatus_BuildX { get; }
        public abstract string WorkerStatus_TrossReturnToArmy { get; }

        public abstract string Hud_ToggleFollowFaction { get; }
        public abstract string Hud_FollowFaction_Yes { get; }
        public abstract string Hud_FollowFaction_No { get; }

        public abstract string Hud_Idle { get; }
        public abstract string Hud_NoLimit { get; }

        public abstract string Hud_None { get; }
        public abstract string Hud_ProductionQueue { get; }

        public abstract string Hud_EmptyList { get; }

        public abstract string Hud_RequirementOr { get; }

        public abstract string Hud_BlackMarket { get; }

        public abstract string Language_CollectProgress { get; }
        public abstract string Hud_SelectCity { get; }
        public abstract string Conscription_Title { get; }
        public abstract string Conscript_WeaponTitle { get; }
        public abstract string Conscript_ArmorTitle { get; }
        public abstract string Conscript_TrainingTitle { get; }

        public abstract string Conscript_SpecializationTitle { get; }
        public abstract string Conscript_SpecializationDescription { get; }
        public abstract string Conscript_SelectBuilding { get; }

        public abstract string Conscript_WeaponDamage { get; }
        public abstract string Conscript_ArmorHealth { get; }
        public abstract string Conscript_AttackSpeed { get; }
        public abstract string Conscript_TrainingTime { get; }

        public abstract string Conscript_Training_Minimal { get; }
        public abstract string Conscript_Training_Basic { get; }
        public abstract string Conscript_Training_Skillful { get; }
        public abstract string Conscript_Training_Professional { get; }

        public abstract string Conscript_Specialization_Field { get; }
        public abstract string Conscript_Specialization_Sea { get; }
        public abstract string Conscript_Specialization_Siege { get; }
        public abstract string Conscript_Specialization_Traditional { get; }
        public abstract string Conscript_Specialization_AntiCavalry { get; }

        public abstract string Conscription_Status_CollectingEquipment { get; }
        public abstract string Conscription_Status_CollectingMen { get; }
        public abstract string Conscription_Status_Training { get; }

        public abstract string ArmyHud_Food_Reserves_X { get; }
        public abstract string ArmyHud_Food_Upkeep_X { get; }
        public abstract string ArmyHud_Food_Costs_X { get; }

        public abstract string Deliver_WillSendXInfo { get; }
        public abstract string Delivery_ListTitle { get; }
        public abstract string Delivery_DistanceX { get; }
        public abstract string Delivery_DeliveryTimeX { get; }
        public abstract string Delivery_SenderMinimumCap { get; }
        public abstract string Delivery_RecieverMaximumCap { get; }
        public abstract string Delivery_ItemsReady { get; }
        public abstract string Delivery_RecieverReady { get; }
        public abstract string Hud_ThisCity { get; }
        public abstract string Hud_RecieveingCity { get; }

        public abstract string Info_ButtonIcon { get; }

        public abstract string Info_ResourcePerSecond { get; }

        public abstract string Info_MinuteAverage { get; }

        public abstract string Message_OutOfFood_Title { get; }
        public abstract string Message_CityOutOfFood_Text { get; }

        public abstract string Hud_EndSessionIcon { get; }

        public abstract string TerrainType { get; }

        public abstract string Hud_EnergyUpkeepX { get; }

        public abstract string Hud_EnergyAmount { get; }

        public abstract string Hud_CopySetup { get; }
        public abstract string Hud_Paste { get; }

        public abstract string Hud_Available { get; }

        public abstract string WorkForce_ChildBirthRequirements { get; }
        public abstract string WorkForce_AvailableHomes { get; }
        public abstract string WorkForce_Peace { get; }
        public abstract string WorkForce_ChildToManTime { get; }

        public abstract string Economy_TaxIncome { get; }
        public abstract string Economy_ImportCostsForResource { get; }
        public abstract string Economy_BlackMarketCostsForResource { get; }
        public abstract string Economy_GuardUpkeep { get; }

        public abstract string Economy_LocalCityTrade_Export { get; }
        public abstract string Economy_LocalCityTrade_Import { get; }

        public abstract string Economy_ResourceProduction { get; }
        public abstract string Economy_ResourceSpending { get; }

        public abstract string Economy_TaxDescription { get; }

        public abstract string Economy_SoldResources { get; }

        public abstract string UnitType_Cities { get; }
        public abstract string UnitType_Armies { get; }
        public abstract string UnitType_Worker { get; }

        public abstract string UnitType_FootKnight { get; }
        public abstract string UnitType_CavalryKnight { get; }

        public abstract string CityCulture_LargeFamilies { get; }
        public abstract string CityCulture_FertileGround { get; }
        public abstract string CityCulture_Archers { get; }
        public abstract string CityCulture_Warriors { get; }
        public abstract string CityCulture_AnimalBreeder { get; }
        public abstract string CityCulture_Miners { get; }
        public abstract string CityCulture_Woodcutters { get; }
        public abstract string CityCulture_Builders { get; }
        public abstract string CityCulture_CrabMentality { get; }
        public abstract string CityCulture_DeepWell { get; }
        public abstract string CityCulture_Networker { get; }
        public abstract string CityCulture_PitMasters { get; }

        public abstract string CityCulture_CultureIsX { get; }
        public abstract string CityCulture_LargeFamilies_Description { get; }
        public abstract string CityCulture_FertileGround_Description { get; }
        public abstract string CityCulture_Archers_Description { get; }
        public abstract string CityCulture_Warriors_Description { get; }
        //public abstract string CityCulture_AnimalBreeder_Description { get; }
        public abstract string CityCulture_Miners_Description { get; }
        public abstract string CityCulture_Woodcutters_Description { get; }
        public abstract string CityCulture_Builders_Description { get; }
        public abstract string CityCulture_CrabMentality_Description { get; }
        public abstract string CityCulture_DeepWell_Description { get; }
        public abstract string CityCulture_Networker_Description { get; }
        public abstract string CityCulture_PitMasters_Description { get; }

        public abstract string CityOption_AutoBuild_Work { get; }
        public abstract string CityOption_AutoBuild_Farm { get; }

        public abstract string Hud_PurchaseTitle_Resources { get; }
        public abstract string Hud_PurchaseTitle_CurrentlyOwn { get; }

        public abstract string Tutorial_EndTutorial { get; }
        public abstract string Tutorial_MissionX { get; }
        public abstract string Tutorial_CollectXAmountOfY { get; }
        public abstract string Tutorial_SelectTabX { get; }
        public abstract string Tutorial_IncreasePriorityOnX { get; }
        public abstract string Tutorial_PlaceBuildOrder { get; }
        public abstract string Tutorial_ZoomInput { get; }

        public abstract string Tutorial_SelectACity { get; }
        public abstract string Tutorial_ZoomInWorkers { get; }
        public abstract string Tutorial_CreateSoldiers { get; }
        public abstract string Tutorial_ZoomOutOverview { get; }
        public abstract string Tutorial_ZoomOutDiplomacy { get; }
        public abstract string Tutorial_ImproveRelations { get; }
        public abstract string Tutorial_MissionComplete_Title { get; }
        public abstract string Tutorial_MissionComplete_Unlocks { get; }

        //patch1
        public abstract string Resource_ReachedStockpile { get; }
        public abstract string BuildingType_ResourceMine { get; }
        public abstract string Resource_TypeName_BogIron { get; }
        public abstract string Resource_TypeName_Coal { get; }
        public abstract string Language_XUpkeepIsY { get; }
        public abstract string Language_XCountIsY { get; }
        public abstract string Message_ArmyOutOfFood_Text { get; }
        public abstract string Info_ArmyFood1 { get; }
        public abstract string Info_ArmyFood2 { get; }
        public abstract string Info_ArmyFood3 { get; }
        public abstract string FactionName_Monger { get; }
        public abstract string FactionName_Hatu { get; }
        public abstract string FactionName_Destru { get; }

        //Patch2
        public abstract string Tutorial_BuildSomething { get; }
        public abstract string Tutorial_BuildCraft { get; }
        public abstract string Tutorial_IncreaseBufferLimit { get; }

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public abstract string Tutorial_CollectItemStockpile { get; }
        public abstract string Tutorial_LookAtFoodBlueprint { get; }
        public abstract string Tutorial_CollectFood_Info1 { get; }
        public abstract string Tutorial_CollectFood_Info2 { get; }
        public abstract string Tutorial_CollectFood_Info0 { get; }

        public abstract string EndGameStatistics_DecorsBuilt { get; }
        public abstract string EndGameStatistics_StatuesBuilt { get; }



        /* #### --------------- ##### */
        /* #### XMAS UPDATE ##### */
        /* #### --------------- ##### */

        public abstract string Info_FoodAndDeliveryLocation { get; }
        public abstract string GameMenu_UseSpeedX { get; }
        public abstract string GameMenu_LongerBuildQueue { get; }

        public abstract string Diplomacy_RelationWithOthers { get; }
        public abstract string Automation_queue_description { get; }

        public abstract string BuildingType_Storehouse_Description { get; }

        public abstract string Resource_TypeName_Longbow { get; }
        public abstract string Resource_TypeName_Rapeseed { get; }
        public abstract string Resource_TypeName_Hemp { get; }

        public abstract string Resource_BogIronDescription { get; }

        public abstract string Resource_FoodSafeGuard_Description { get; }
        public abstract string Resource_FoodSafeGuard_Active { get; }

        public abstract string GameMenu_NextSong { get; }

        public abstract string BuildingType_Bank { get; }
        public abstract string BuildingType_GoldDelivery_Description { get; }

        public abstract string BuildingType_Logistics { get; }
        public abstract string BuildingType_Logistics_Description { get; }
        public abstract string BuildingType_Logistics_NationSizeRequirement { get; }
        public abstract string Requirements_XItemStorageOfY { get; }

        public abstract string XP_UnlockBuildQueue { get; }
        public abstract string XP_UnlockBuilding { get; }
        public abstract string XP_Upgrade { get; }
        public abstract string XP_UpgradeBuildingX { get; }

        public abstract string BuildHud_PerCycle { get; }
        public abstract string BuildHud_MayCraft { get; }
        public abstract string BuildHud_WorkTime { get; }
        public abstract string BuildHud_GrowTime { get; }
        public abstract string BuildHud_Produce { get; }

        public abstract string BuildHud_Queue { get; }

        public abstract string LandType_Flatland { get; }
        public abstract string LandType_Water { get; }
        public abstract string BuildingType_Wall { get; }
        public abstract string Delivery_AutoReciever_Description { get; }

        //public abstract string Hud_NoLimit { get; }
        public abstract string Hud_On { get; }
        public abstract string Hud_Off { get; }
        public abstract string Hud_Time_Seconds { get; }
        public abstract string Hud_Time_Minutes { get; }
        public abstract string Hud_Undo { get; }
        public abstract string Hud_Redo { get; }

        public abstract string Tag_ViewOnMap { get; }
        public abstract string MenuTab_Tag { get; }

        public abstract string Input_Build { get; }

        public abstract string FlagEditor_ClearAll { get; }

        public abstract string CityCulture_Stonemason { get; }
        public abstract string CityCulture_Stonemason_Description { get; }

        public abstract string CityCulture_Brewmaster { get; }
        public abstract string CityCulture_Brewmaster_Description { get; }

        public abstract string CityCulture_Weavers { get; }
        public abstract string CityCulture_Weavers_Description { get; }

        public abstract string CityCulture_SiegeEngineer { get; }
        public abstract string CityCulture_SiegeEngineer_Description { get; }

        public abstract string CityCulture_Armorsmith { get; }
        public abstract string CityCulture_Armorsmith_Description { get; }

        public abstract string CityCulture_Noblemen { get; }
        public abstract string CityCulture_Noblemen_Description { get; }

        public abstract string CityCulture_Seafaring { get; }
        public abstract string CityCulture_Seafaring_Description { get; }

        public abstract string CityCulture_Backtrader { get; }
        public abstract string CityCulture_Backtrader_Description { get; }

        public abstract string CityCulture_LawAbiding { get; }
        public abstract string CityCulture_LawAbiding_Description { get; }

        //##2##

        public abstract string Hud_Advanced { get; }
        public abstract string Hud_Loading { get; }

        public abstract string CityOption_LowerGuardSize { get; }
        public abstract string Hud_Purchase_MinCapacity { get; }
        public abstract string Settings_ResetToDefault { get; }
        public abstract string Settings_NewGame { get; }

        public abstract string Settings_AdvancedGameSettings { get; }
        public abstract string Settings_FoodMultiplier { get; }
        public abstract string Settings_FoodMultiplier_Description { get; }

        public abstract string Settings_GameMode { get; }

        public abstract string Settings_Mode_Story { get; }
        public abstract string Settings_Mode_IncludeBoss { get; }
        public abstract string Settings_Mode_IncludeAttacks { get; }
        public abstract string Settings_Mode_Sandbox { get; }
        public abstract string Settings_Mode_Peaceful { get; }
        public abstract string Settings_Mode_Peaceful_Description { get; }

        public abstract string Lobby_ImportSave { get; }

        public abstract string Lobby_ExportSave { get; }
        public abstract string Lobby_ExportSave_Description { get; }

        public abstract string Resource_CurrentAmount { get; }
        public abstract string Resource_MaxAmount_Soft { get; }
        public abstract string Resource_MaxAmount { get; }
        public abstract string Resource_AddPerSec { get; }

        public abstract string Resource_WaterAddLimit { get; }

        public abstract string Tutorial_Select_SubTab { get; }


        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        // Tutorial & Demo
        public abstract string Tutorial_OpenGuardSubTab { get; }
        public abstract string Tutorial_GuardToWall { get; }
        public abstract string Demo_MissionObjective_Title { get; }
        public abstract string Demo_MissionObjective_Description { get; }
        public abstract string Demo_Complete_Title { get; }
        public abstract string Demo_TimesUp_Title { get; }
        public abstract string Demo_EndInOneMinuteDescription { get; }

        // Army & Profile
        public abstract string ArmyOption_NewArmy { get; }
        public abstract string ProfileEditor_AltMain { get; }

        // Automation
        public abstract string Automation_CheckBoxTitle { get; }

        // Army Structure
        public abstract string ArmyStructure_ColumnWidth { get; }
        public abstract string ArmyStructure_ArmyPlacement { get; }
        public abstract string ArmyStructure_Row_Front { get; }
        public abstract string ArmyStructure_Row_Body { get; }
        public abstract string ArmyStructure_Row_Second { get; }
        public abstract string ArmyStructure_Row_Behind { get; }

        // Diplomacy
        public abstract string Diplomacy_RelationType_Enemies { get; }

        // Events
        public abstract string EventMessage_EnemyAlliance_Title { get; }
        public abstract string EventMessage_EnemyAlliance { get; }

        // Settings - Gameplay
        public abstract string Settings_CentralGold { get; }
        public abstract string Settings_CentralGold_Description { get; }

        // Input Actions
        public abstract string InputActionName_StopStart { get; }
        public abstract string InputActionName_ToggleHudDetail { get; }
        public abstract string InputActionName_NextCity { get; }
        public abstract string InputActionName_NextArmy { get; }
        public abstract string InputActionName_NextBattle { get; }
        public abstract string InputActionName_Build { get; }
        public abstract string InputActionName_Copy { get; }
        public abstract string InputActionName_Paste { get; }
        public abstract string InputActionName_Menu { get; }
        public abstract string InputActionName_FlagDesign_ToggleColor_Prev { get; }
        public abstract string InputActionName_FlagDesign_ToggleColor_Next { get; }
        public abstract string InputActionName_FlagDesign_PaintBucket { get; }
        public abstract string InputActionName_Controller_FlagDesign_Colorpicker { get; }
        public abstract string InputActionName_ControllerFocus { get; }
        public abstract string InputActionName_ControllerCancel { get; }
        public abstract string InputActionName_ControllerMessageClick { get; }
        public abstract string InputActionName_ControllerSelect { get; }
        public abstract string InputActionName_WASD_UP { get; }
        public abstract string InputActionName_WASD_DOWN { get; }
        public abstract string InputActionName_WASD_LEFT { get; }
        public abstract string InputActionName_WASD_RIGHT { get; }
        public abstract string InputActionName_CameraTiltLeft { get; }
        public abstract string InputActionName_CameraTiltRight { get; }
        public abstract string InputActionName_CameraTiltUp { get; }
        public abstract string InputActionName_ZoomInKey { get; }
        public abstract string InputActionName_ZoomOutKey { get; }

        // Settings UI
        public abstract string Settings_Title_Monitor { get; }
        public abstract string Settings_Title_Graphics { get; }
        public abstract string Settings_Title_Input { get; }
        public abstract string Settings_Title_Gameplay { get; }
        public abstract string Settings_PanOnZoom { get; }
        public abstract string Settings_ScrollSensitivity_Game { get; }
        public abstract string Settings_ScrollSensitivity_Menu { get; }
        public abstract string Settings_Blood { get; }

        public abstract string Settings_MasterVolume { get; }
        public abstract string Settings_AmbienceVolume { get; }
        public abstract string Settings_BattleMelody { get; }

        public abstract string Settings_ModelLight { get; }
        public abstract string Settings_Particles { get; }
        public abstract string Settings_MapLoadSpeed { get; }

        // Lobby
        public abstract string Lobby_Category_Options { get; }
        public abstract string Lobby_Category_Editor { get; }
        public abstract string Lobby_Category_ExtraModes { get; }

        public abstract string Lobby_Editor_MapEditor { get; }
        public abstract string Lobby_Editor_VoxelEditor { get; }

        public abstract string Lobby_Mode_BattleLab { get; }
        public abstract string Lobby_Mode_BattleLab_Description { get; }
        public abstract string Lobby_Mode_Commander { get; }
        public abstract string Lobby_Mode_Commander_Description { get; }
        public abstract string Lobby_MusicPlayList { get; }

        public abstract string Lobby_GameSetup { get; }
        public abstract string Lobby_PlayerSetup { get; }
        public abstract string LobbyDemoMode_Demo { get; }

        public abstract string Lobby_Tutorial { get; }
        public abstract string LobbyDemoMode_ShortTutorial { get; }
        public abstract string LobbyDemoMode_LongTutorial { get; }
        public abstract string LobbyDemoMode_WishlistOn { get; }

        public abstract string BattleLab_StartHere { get; }
        public abstract string BattleLab_Start { get; }
        public abstract string BattleLab_Attacker { get; }

        public abstract string MapGenerator_Name { get; }
        public abstract string MapType_CustomMap { get; }
        public abstract string MapType_GenerateNewMap { get; }
        public abstract string MapGenerator_GenerateAction { get; }

        public abstract string MapGenerator_Terrain_CustomSize { get; }
        public abstract string MapGenerator_Terrain_StartAs { get; }
        public abstract string MapGenerator_Terrain_ClearPass { get; }
        public abstract string MapGenerator_Terrain_BuildPass { get; }
        public abstract string MapGenerator_Terrain_DigPass { get; }
        public abstract string MapGenerator_Terrain_BuildDigLoops { get; }
        public abstract string MapGenerator_Terrain_BuildStrokes { get; }
        public abstract string MapGenerator_Terrain_BuildStrokes_Description { get; }
        public abstract string MapGenerator_Terrain_DigStrokes { get; }
        public abstract string MapGenerator_Terrain_CleanUp_Option { get; }
        public abstract string MapGenerator_Terrain_CleanUpPass { get; }
        public abstract string Economy_ServicemenUpkeep { get; }
        public abstract string Economy_ServicemenUpkeep_Description { get; }
        public abstract string Economy_GuardUpkeep_Description { get; }
        public abstract string EndScreen_TimeHasEndedTitle { get; }
        public abstract string Hud_AdvancedSettings { get; }
        public abstract string Hud_Vector_X { get; }
        public abstract string Hud_Vector_Y { get; }
        public abstract string Hud_Cancel { get; }
        public abstract string Hud_Delete { get; }
        public abstract string Hud_Next { get; }
        //public abstract string Hud_None { get; }
        public abstract string Hud_Apply { get; }
        public abstract string Hud_AllCities { get; }
        public abstract string Hud_Time_Hours { get; }
        public abstract string Hud_AddX { get; }
        public abstract string Hud_Both { get; }
        public abstract string Hud_Direction { get; }
        //public abstract string MusicIsBroken { get; }
        public abstract string Hud_ObjectsAndCount { get; }
        public abstract string Hud_EffectDoesNotStack { get; }
        public abstract string Work_SmeltX { get; }
        public abstract string Info_TotalFoodProduction { get; }
        public abstract string Info_TotalFoodSpending { get; }

        public abstract string BuildingType_StoneWall { get; }
        public abstract string BuildingType_StoneTower { get; }
        public abstract string BuildingType_StoneGate { get; }
        public abstract string BuildingType_StoneHouse { get; }

        public abstract string VariantType_A { get; }
        public abstract string VariantType_B { get; }
        public abstract string VariantType_C { get; }
        public abstract string VariantType_D { get; }
        public abstract string VariantType_E { get; }
        public abstract string VariantType_F { get; }
        public abstract string VariantType_G { get; }
        public abstract string VariantType_H { get; }

        public abstract string BuildingToolShape_Free { get; }
        public abstract string BuildingToolShape_Area { get; }
        public abstract string BuildingToolShape_Line { get; }
        public abstract string BuildingToolShape_LShape { get; }

        public abstract string CityHall_Upgrade { get; }
        public abstract string CityHall_MaxSupportedWorkers { get; }

        public abstract string CityHall_Size_Small { get; }
        public abstract string CityHall_Size_Medium { get; }
        public abstract string CityHall_Size_Large { get; }

        public abstract string GuardHousingCount { get; }
        public abstract string ServicemenCount { get; }

        public abstract string Work_MiningResource { get; }

        public abstract string MenuTab_Progress { get; }

        public abstract string Automation_AutomateCity { get; }
        public abstract string Automation_AutomationFocus { get; }
        public abstract string Automation_AutomationFocus_Grow { get; }
        public abstract string Automation_AutomationFocus_Export { get; }
        public abstract string Automation_AutomationFocus_War { get; }

        public abstract string CityCulture_Smelters_Description { get; }
        public abstract string CityCulture_Smelters { get; }

        public abstract string CityCulture_Apprentices_Description { get; }
        public abstract string CityCulture_Apprentices { get; }

        public abstract string CityCulture_BronzeCasters_Description { get; }
        public abstract string CityCulture_BronzeCasters { get; }

        public abstract string Info_FooodAndDeliveryLocation { get; }
        //public abstract string GameMenu_UseSpeedX { get; }

        public abstract string Delivery_SendChunk { get; }
        public abstract string Delivery_SpeedBonus { get; }

        public abstract string Delivery_AutoResourceDescription { get; }

        public abstract string Conscript_Soldiers_ArmyType { get; }
        public abstract string Conscript_Soldiers_ArmyType_Description { get; }
        public abstract string Conscript_Soldiers_GuardType { get; }
        public abstract string Conscript_Soldiers_GuardType_Description { get; }

        public abstract string Defence_Title { get; }
        public abstract string Defence_GuardPost { get; }

        public abstract string Defence_WallDescription_Movement { get; }
        public abstract string Defence_WallDescription_GuardPost { get; }
        public abstract string Defence_AutoAssign { get; }
        public abstract string Defence_AutoAssign_Description { get; }

        public abstract string Conscript_SplashDamage { get; }
        public abstract string Conscript_HighSplashDamage { get; }

        public abstract string Conscript_Training_Champion { get; }
        public abstract string Conscript_Training_Legendary { get; }

        public abstract string Experience_Title { get; }
        public abstract string Experience_TopExperience { get; }

        public abstract string Experience_TimeReductionDescription { get; }

        public abstract string ExperienceType_Farm { get; }
        public abstract string ExperienceType_AnimalCare { get; }
        public abstract string ExperienceType_HouseBuilding { get; }
        public abstract string ExperienceType_WoodWork { get; }
        public abstract string ExperienceType_StoneCutter { get; }
        public abstract string ExperienceType_Mining { get; }
        public abstract string ExperienceType_Transport { get; }
        public abstract string ExperienceType_Cook { get; }
        public abstract string ExperienceType_Fletcher { get; }
        public abstract string ExperienceType_RefineOre { get; }
        public abstract string ExperienceType_Casting { get; }
        public abstract string ExperienceType_CraftMetal { get; }
        public abstract string ExperienceType_CraftArmor { get; }
        public abstract string ExperienceType_CraftWeapon { get; }
        public abstract string ExperienceType_CraftFuel { get; }
        public abstract string ExperienceType_Chemist { get; }

        public abstract string ExperienceLevel_1 { get; }
        public abstract string ExperienceLevel_2 { get; }
        public abstract string ExperienceLevel_3 { get; }
        public abstract string ExperienceLevel_4 { get; }
        public abstract string ExperienceLevel_5 { get; }

        public abstract string ExperenceOrDistancePrio_Title { get; }
        public abstract string ExperenceOrDistancePrio_Description { get; }

        public abstract string Technology_Description { get; }
        public abstract string Experience_Description { get; }

        public abstract string Technology_Title { get; }
        public abstract string Technology_ShareField { get; }

        public abstract string Technology_GainByNeigborRelation { get; }
        public abstract string Technology_ForEachMaster { get; }
        public abstract string Technology_CitySpread { get; }
        public abstract string Technology_CityCapture { get; }

        public abstract string Technology_AdvancedBuildings { get; }
        public abstract string Technology_AdvancedFarming { get; }
        public abstract string Technology_AdvancedCasting { get; }

        public abstract string Help_Title { get; }
        public abstract string Help_Work_Title { get; }
        public abstract string Help_Work_Resources { get; }
        public abstract string Help_Work_Skill { get; }
        public abstract string Help_Work_Stockpile { get; }
        public abstract string Help_Work_Priority { get; }

        public abstract string Help_Soldiers_Title { get; }
        public abstract string Help_Soldiers_PlaceBuildingX { get; }
        public abstract string Help_Soldiers_Workers { get; }
        public abstract string Help_Soldiers_Weapon { get; }
        public abstract string Help_Soldiers_StartX { get; }

        public abstract string Hud_SelectHistory { get; }

        public abstract string Hud_PointsPerMinute { get; }
        public abstract string Hud_PercentValueCost { get; }

        public abstract string Hud_Mixed { get; }
        public abstract string Hud_Distance { get; }

        public abstract string Hud_Unlock { get; }
        public abstract string Hud_category { get; }

        /// <summary>
        /// Sets the game speed to one frame at a time
        /// </summary>
        public abstract string Input_StepOneFrame { get; }

        public abstract string Resource_TypeName_Wagon2Wheel { get; }
        public abstract string Resource_TypeName_Wagon4Wheel { get; }
        public abstract string Resource_TypeName_Tin { get; }
        public abstract string Resource_TypeName_TinOre { get; }

        public abstract string Resource_TypeName_Copper { get; }
        public abstract string Resource_TypeName_CopperOre { get; }
        public abstract string Resource_TypeName_SilverOre { get; }
        public abstract string Resource_TypeName_Silver { get; }

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public abstract string Resource_TypeName_RawMithril { get; }
        public abstract string Resource_TypeName_Mithril { get; }

        public abstract string Resource_TypeName_BronzeSword { get; }
        public abstract string Resource_TypeName_ShortSword { get; }
        public abstract string Resource_TypeName_LongSword { get; }
        public abstract string Resource_TypeName_HandSpear { get; }
        public abstract string Resource_TypeName_Warhammer { get; }
        public abstract string Resource_TypeName_MithrilSword { get; }
        public abstract string Resource_TypeName_SlingShot { get; }
        public abstract string Resource_TypeName_ThrowingSpear { get; }
        public abstract string Resource_TypeName_Crossbow { get; }
        public abstract string Resource_TypeName_MithrilBow { get; }

        public abstract string Resource_TypeName_CoolingFluid { get; }
        public abstract string Resource_TypeName_Palisade { get; }
        public abstract string Resource_TypeName_Toolkit { get; }

        public abstract string Resource_TypeName_Sulfur { get; }
        public abstract string Resource_TypeName_LeadOre { get; }
        public abstract string Resource_TypeName_Lead { get; }
        public abstract string Resource_TypeName_Bronze { get; }
        public abstract string Resource_TypeName_BloomIron { get; }
        public abstract string Resource_TypeName_Steel { get; }
        public abstract string Resource_TypeName_CastIron { get; }

        public abstract string Resource_TypeName_BlackPowder { get; }
        public abstract string Resource_TypeName_GunPowder { get; }
        public abstract string Resource_TypeName_LedBullet { get; }

        public abstract string Resource_TypeName_HandCannon { get; }
        public abstract string Resource_TypeName_HandCulverin { get; }
        public abstract string Resource_TypeName_Rifle { get; }
        public abstract string Resource_TypeName_Blunderbuss { get; }

        public abstract string Resource_TypeName_Manuballista { get; }
        public abstract string Resource_TypeName_Catapult { get; }
        public abstract string Resource_TypeName_BatteringRam { get; }
        public abstract string Resource_TypeName_SiegeCannonBronze { get; }
        public abstract string Resource_TypeName_ManCannonBronze { get; }
        public abstract string Resource_TypeName_SiegeCannonIron { get; }
        public abstract string Resource_TypeName_ManCannonIron { get; }

        public abstract string Resource_TypeName_PaddedArmor { get; }
        public abstract string Resource_TypeName_HeavyPaddedArmor { get; }

        public abstract string Resource_TypeName_IronArmor { get; }
        public abstract string Resource_TypeName_HeavyIronArmor { get; }

        public abstract string Resource_TypeName_BronzeArmor { get; }

        public abstract string Resource_TypeName_LightPlateArmor { get; }
        public abstract string Resource_TypeName_FullPlateArmor { get; }
        public abstract string Resource_TypeName_MithrilArmor { get; }
        public abstract string Resource_TypeName_Coin { get; }

        public abstract string UnitType_Warhammer { get; }
        //public abstract string UnitType_MithrilKnight { get; }
        public abstract string UnitType_MithrilArcher { get; }
        public abstract string UnitType_SpearAndShield { get; }

        public abstract string UnitType_CollectionOfSoldiers { get; }
        public abstract string UnitType_CollectionOfArmies { get; }

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public abstract string UnitId { get; }

        public abstract string BuildHud_AreaEffectTitle { get; }
        public abstract string BuildHud_BonusRadius { get; }

        public abstract string BuildHud_BuildTime { get; }
        public abstract string SchoolHud_ToLevel { get; }
        public abstract string SchoolHud_TimeDescription { get; }
        public abstract string SchoolHud_SelectSchool { get; }
        public abstract string Upgrade_Order { get; }

        public abstract string Building_ListDescription { get; }

        public abstract string BuildingType_IsUpgraded { get; }
        public abstract string BuildingType_WoodCutter { get; }
        public abstract string BuildingType_Workshop_Description { get; }

        public abstract string BuildingType_WoodCutter_AreaAffect { get; }

        public abstract string BuildingType_StoneCutter_AreaAffect { get; }

        public abstract string BuildingType_StoneCutter { get; }

        public abstract string BuildingType_Embassy { get; }
        public abstract string BuildingType_Embassy_Description { get; }

        public abstract string BuildingType_SoldierBarracks { get; }
        public abstract string BuildingType_ArcherBarracks { get; }
        public abstract string BuildingType_WarmachineBarracks { get; }
        public abstract string BuildingType_GunBarracks { get; }
        public abstract string BuildingType_CannonBarracks { get; }
        public abstract string BuildingType_KnightsBarracks { get; }

        public abstract string BuildingType_WaterResovoir { get; }
        public abstract string BuildingType_WaterResovoir_Description { get; }

        public abstract string BuildingType_SmeltingFurnace { get; }
        public abstract string BuildingType_SmeltingFurnace_Description { get; }

        public abstract string BuildingType_Foundry { get; }
        public abstract string BuildingType_Foundry_Description { get; }

        public abstract string BuildingType_Armory { get; }
        public abstract string BuildingType_Armory_Description { get; }
        public abstract string BuildingType_Chemist { get; }
        public abstract string BuildingType_Chemist_Description { get; }
        public abstract string BuildingType_CoinMaker { get; }
        public abstract string BuildingType_CoinMaker_Description { get; }
        public abstract string BuildingType_Gunmaker { get; }
        public abstract string BuildingType_Gunmaker_Description { get; }

        public abstract string BuildingType_School_Tab { get; }
        public abstract string BuildingType_School { get; }
        public abstract string BuildingType_School_Description { get; }

        public abstract string BuildingType_GoldDelivery { get; }
        public abstract string BuildingType_Bank_Description { get; }

        public abstract string DecorType_CobbleStones { get; }
        public abstract string DecorType_Square { get; }

        public abstract string DecorType_Garden { get; }
        public abstract string DecorType_Flag { get; }
        public abstract string DecorType_Banner { get; }

        public abstract string BuildingType_DirtRoad { get; }
        public abstract string BuildingType_Palisade { get; }

        public abstract string ResourceType_ServiceMen { get; }
        public abstract string BuildingType_ServiceHouse { get; }
        public abstract string BuildingType_ServiceHouse_DescriptionAddX { get; }

        public abstract string BuildingType_GuardOffice { get; }
        public abstract string BuildingType_GuardOffice_DescriptionAddX { get; }

        public abstract string BuildingType_DirtWall { get; }
        public abstract string BuildingType_DirtTower { get; }
        public abstract string BuildingType_WoodWall { get; }
        public abstract string BuildingType_WoodTower { get; }


        //DEMO PATCH 1
        public abstract string FactionName_Barbarian { get; }
        public abstract string Tutorial_AttackAndDestroyX { get; }
        public abstract string Resource_TypeName_Pike { get; }

        public abstract string BattleTrials_Title { get; }
        public abstract string BattleTrials_Description { get; }

        //DEMO PATCH 2
        public abstract string Conscript_BlockReducingAttack { get; }
        public abstract string Conscript_BlockPerSecond { get; }
        public abstract string Conscript_BlockDescription { get; }

        public abstract string Map_CustomSeed { get; }

        public abstract string Settings_Mode_Spectator { get; }
        public abstract string Settings_Mode_Spectator_Description { get; }

        public abstract string Automation_AutomationFocus_NoFocus_Description { get; }
        public abstract string Automation_AutomationFocus_WillProduce { get; }

        public abstract string Help_Food_WhoEats { get; }
        public abstract string Help_Food_BigArmy { get; }
        public abstract string Help_Food_DontBuild { get; }
        public abstract string Help_Food_UseWater { get; }
        public abstract string Help_Food_Postal { get; }

        public abstract string Message_LostCity { get; }

        public abstract string Demo_Description { get; }


        //DEMO PATCH 3
        public abstract string Demo_EndInXMinuteDescription { get; }
        public abstract string Experience_Required { get; }
        public abstract string InputActionName_ToggleMenu { get; }

        //DEMO PATCH 4
        public abstract string Work_BadValueDescription { get; }
        public abstract string Work_SelectCategory { get; }

        public abstract string Hud_RemoveFromList { get; }
        public abstract string Hud_ReturnToPrevious { get; }
        public abstract string Hud_Close { get; }

        public abstract string Hud_Low { get; }
        public abstract string Hud_Medium { get; }
        public abstract string Hud_High { get; }

        public abstract string Hud_Copy { get; }
        //public abstract string Hud_Paste { get; }
        public abstract string Hud_Cut { get; }
        public abstract string Hud_SaveCompleted { get; }

        public abstract string Settings_WaterMultiplier { get; }
        public abstract string Settings_WaterMultiplier_Description { get; }

        public abstract string Settings_ChildMultiplier { get; }
        public abstract string Settings_CraftMultiplier { get; }
        public abstract string Settings_CraftMultiplier_Description { get; }

        public abstract string FastProduction { get; }
        public abstract string SlowProduction { get; }

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public abstract string BlocksProduction { get; }

        public abstract string Automation_AutomationFocus_NoFocus { get; }
        public abstract string CityAutomation_SoldierQuality { get; }
        public abstract string CityAutomation_SoldierWeaponType { get; }

        public abstract string WarsResourceGroup_Resources { get; }
        public abstract string WarsResourceGroup_Weapons { get; }
        public abstract string WarsResourceGroup_AllWeaponTypes { get; }
        public abstract string WarsResourceGroup_MeleeHandWeapons { get; }
        public abstract string WarsResourceGroup_RangedHandWeapons { get; }
        public abstract string WarsResourceGroup_Warmachines { get; }

        public abstract string FactionSettings_Titel { get; }
        public abstract string FactionSettings_Description { get; }

        public abstract string Conscript_MaxPopulation { get; }
        public abstract string Conscript_MaxPopulation_Description { get; }

        public abstract string Conscript_FoodAbundance { get; }
        public abstract string Conscript_FoodAbundance_Description { get; }

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public abstract string GeneralSetting_On { get; }
        public abstract string GeneralSetting_Off { get; }
        public abstract string GeneralSetting_AllBuildingsDescription { get; }
        public abstract string GeneralSetting_ApplyMessage { get; }

        public abstract string MustTurnOffSteamInput { get; }

        public abstract string Technology_GainTitle { get; }
        public abstract string Technology_LevelUp { get; }
        public abstract string Technology_ForEachLevelUp { get; }

        public abstract string VoxelEditor_Description { get; }

        public abstract string Editor_Tool { get; }
        public abstract string Editor_SelectOptionsMenu { get; }
        public abstract string Editor_Continous { get; } // "Continuous"

        public abstract string Editor_Tool_PencilSize { get; }
        public abstract string Editor_Tool_SizeTolerance { get; }
        public abstract string Editor_Tool_RoundPencil { get; }
        public abstract string Editor_Tool_EdgeSize { get; }
        public abstract string Editor_Tool_PercentFill { get; }
        public abstract string Editor_Tool_ClearAbove { get; }
        public abstract string Editor_Tool_FillBelow { get; }

        public abstract string Editor_UserModels { get; }
        public abstract string Editor_UserModels_Description { get; }

        public abstract string Editor_RetailModels { get; }
        public abstract string Editor_RetailModels_Description { get; }

        public abstract string Editor_ModTemplates { get; }
        public abstract string Editor_ExportAsOBJ { get; }
        public abstract string Editor_SelectAll { get; }

        public abstract string Editor_Canvas_Title { get; }
        public abstract string Editor_Canvas_Size { get; }
        public abstract string Editor_Canvas_Dimension_X { get; }
        public abstract string Editor_Canvas_Dimension_Y { get; }
        public abstract string Editor_Canvas_Dimension_Z { get; }
        public abstract string Editor_Canvas_SizePresets { get; }
        public abstract string Editor_Canvas_Move { get; }
        public abstract string Editor_Canvas_Move_Up { get; }
        public abstract string Editor_Canvas_Move_Down { get; }
        public abstract string Editor_Canvas_RotateClockwise { get; }
        public abstract string Editor_Canvas_RotateCounterClockwise { get; }
        public abstract string Editor_Canvas_Mirror { get; }

        public abstract string Editor_Canvas_RotateFlip_Title { get; }
        public abstract string Editor_Canvas_FlipVertical { get; }
        public abstract string Editor_Canvas_FlipOrientation { get; }
        public abstract string Editor_Canvas_ClearAll_Description { get; }

        public abstract string Editor_Animation { get; }
        public abstract string Editor_Animation_RemoveCurrentFrame { get; }
        public abstract string Editor_Animation_AddFrameCopy { get; }
        public abstract string Editor_Animation_AddEmptyFrame { get; }
        public abstract string Editor_Animation_MoveDescription { get; }
        public abstract string Editor_Animation_AllFrames { get; }
        public abstract string Editor_Animation_AllFrames_ActionDescription { get; }

        public abstract string Editor_SettingsMenu { get; }
        public abstract string Hud_Exit { get; }
        public abstract string Editor_Canvas_Clear { get; }

        public abstract string Editor_Stamp { get; }
        public abstract string Editor_StampOtherFrames { get; }
        public abstract string Editor_StampOtherFrames_Description { get; }
        public abstract string Editor_PasteToFrame { get; }
        public abstract string Editor_ClearAllFrames { get; }
        public abstract string Editor_ClearOtherFrames { get; }

        public abstract string Editor_Settings_MoveSpeed { get; }
        public abstract string Editor_Settings_BackgroundColor { get; }
        public abstract string Editor_Settings_HideHUD { get; }

        public abstract string Editor_Color { get; }
        public abstract string Editor_ColorsInUseLabel { get; }
        public abstract string Editor_Color_BrighterPlus { get; }
        public abstract string Editor_Color_Brighter { get; }
        public abstract string Editor_Color_Darker { get; }
        public abstract string Editor_Color_DarkerPlus { get; }
        public abstract string Editor_Color_RedTint { get; }
        public abstract string Editor_Color_Tint { get; }
        public abstract string Editor_Color_GreenTint { get; }
        public abstract string Editor_Color_BlueTint { get; }
        public abstract string Editor_Color_YellowTint { get; }
        public abstract string Editor_Color_PurpleTint { get; }
        public abstract string Editor_NoColor { get; }

        public abstract string Editor_Material { get; }

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public abstract string Editor_Color_Recolor { get; }
        public abstract string Editor_Color_RecolorTo { get; }

        public abstract string Editor_Material_Set { get; }

        public abstract string Editor_Preview { get; }
        public abstract string Editor_CombineWithCurrent { get; }

        public abstract string Editor_PickedColor { get; }
        public abstract string Editor_ColorRGBvalues { get; }

        public abstract string BuildingType_ImmigrationTent { get; }
        public abstract string BuildingType_ImmigrationTent_Description { get; }
        public abstract string BuildingType_ReseachCenter { get; } // spelling kept to match original usage
        public abstract string BuildingType_Bookpress { get; }
        public abstract string BuildingType_Bookpress_Description { get; }

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public abstract string Technology_ReseachExample { get; }

        public abstract string BuildingType_Research_BaseDescription { get; }
        public abstract string BuildingType_ResearchCenter_Description { get; }


        //DEMO PATCH 5


        public abstract string Editor_CropSelection { get; }

        public abstract string Immigrants_DisbandedSoldiers { get; }
        public abstract string Immigrants_RefillWorkers { get; }
        public abstract string Immigrants_UnhousedAreLost { get; }
        public abstract string Editor_VoxelCount { get; }

        public abstract string Editor_Layers_Titel { get; }
        public abstract string Editor_Layers_All { get; }
        public abstract string Editor_LayerNumber { get; }

        public abstract string Editor_Layer_AddEmpty { get; }
        public abstract string Editor_Layer_AddCopy { get; }
        public abstract string Editor_Layer_Remove { get; }
        public abstract string Editor_Layer_MergeDown { get; }
        public abstract string Editor_IsAnimated { get; }
        public abstract string Editor_ToggleVisible { get; }
        public abstract string Editor_ToggleAnimatedLayer { get; }
        public abstract string Editor_Projects { get; }
        public abstract string ProfileEditor_ReplaceMaterial { get; }

        public abstract string ProfileEditor_ProfileColors_Label { get; }
        public abstract string ProfileEditor_TunicColor { get; }
        public abstract string ProfileEditor_PantsColor { get; }
        public abstract string ProfileEditor_LeaderColor { get; }

        public abstract string MapStartAs_Water { get; }
        public abstract string MapStartAs_Land { get; }
        public abstract string MapStartAs_Circle { get; }

        public abstract string Hud_NeedToBeAssigned { get; }
        public abstract string Hud_CommitAssignment { get; }
        public abstract string Technology_NoAvailableResearch { get; }

        public abstract string Research_Tab { get; }

        //5.2
        public abstract string BuildCategory_General { get; }
        public abstract string BuildCategory_Military { get; }
        public abstract string BuildCategory_Decoration { get; }
        public abstract string BuildCategory_Upgrade { get; }

        public abstract string Work_NoMines { get; }

        //NEXT FEST DEMO
         
        public abstract string HUD_DisplayName { get; }
        public abstract string HUD_Filter { get; }
        public abstract string HUD_Scale { get; }
        public abstract string HUD_Tags { get; }
        public abstract string HUD_ClickToCancel { get; }

        public abstract string ObjectTag_Description { get; }
        public abstract string HudPins { get; }
        public abstract string HudPins_Description { get; }


        public abstract string Lobby_PlayerProfileNumbered { get; }
        public abstract string Lobby_CharacterCreationNumbered { get; }
        public abstract string Lobby_PlayerProfileEdit { get; }

  
        public abstract string Editor_ConvertAnimationToLayers { get; }
        public abstract string Editor_StampAllFrames { get; }
        public abstract string Editor_DisplayOptions { get; }
        public abstract string Editor_CharacterCreator { get; }
        public abstract string Editor_CharacterCreator_Description { get; }
        public abstract string Editor_HatGenre { get; }
        public abstract string Editor_HatGenre_FollowWeapon { get; }
        public abstract string Editor_HatGenre_Uniform { get; }
        public abstract string Editor_CopyPasteSelectedColor { get; }

  
        public abstract string Character_Accessories { get; }
        public abstract string Character_Hat { get; }
        public abstract string Character_Head { get; }
        public abstract string Character_Body { get; }
        public abstract string Character_Arms { get; }
        public abstract string Character_Back { get; }
        public abstract string Character_Face { get; }

        public abstract string Settings_ChildMultiplier_Description { get; }
        public abstract string Settings_CasualControls { get; }
        public abstract string Settings_CasualControls_Description { get; }
        public abstract string Settings_AdvancedControls { get; }
        public abstract string Settings_AdvancedControls_Description { get; }

        public abstract string WarsResourceGroup_Metal { get; }
        public abstract string Work_Craft { get; }
        public abstract string Work_OnlyCraftOnFullStock { get; }

 
        public abstract string ExperienceType_Smelting { get; }
        public abstract string Category_Optimize { get; }
        public abstract string BuildCategory_Road { get; }
        public abstract string XP_UnlockBuildPrio { get; }
        public abstract string Technology_ModernFarming { get; }

        public abstract string ExportImportDescription { get; }
        public abstract string CityCultureDescription { get; }

        public abstract string UnitType_CloseRangeRifle { get; }
        public abstract string UnitType_LongRangeRifle { get; }
        public abstract string UnitType_Skirmisher { get; }
        public abstract string UnitType_MithrilSwordsman { get; }

        public abstract string Defence_AutoAssign_Towers { get; }

        public abstract string EventMessage_DesertersText_Food { get; }

        public abstract string Tutorial_CasualRecruitSoldiers { get; }

        //Shadow update
        public abstract string Technology_CannotReassign { get; }

        public abstract string Diplomacy_DeclareWarAgainst { get; }
        public abstract string Diplomacy_AllyCount { get; }
        public abstract string Diplomacy_CostPerAlly { get; }

        public abstract string Event_ChanceOfFailure { get; }
        public abstract string EventMessage_Event_Title { get; }
        public abstract string EventMessage_TheCohalition { get; }

        public abstract string EventMessage_DarkHorde { get; }
        public abstract string EventMessage_DarkHordeKiller_Title { get; }
        public abstract string EventMessage_DarkHordeKiller_Message { get; }

        public abstract string GodPower { get; }

        public abstract string Building_TreeSprout_Description { get; }
        public abstract string Building_TreeSprout_Soft { get; }
        public abstract string Building_TreeSprout_Hard { get; }

        public abstract string GeneralSetting_SetAll { get; }

        public abstract string Hud_All { get; }
        public abstract string Hud_Previous { get; }
        public abstract string Hud_EffectWillStack { get; }

        public abstract string Info_WhenFoodRunsOut { get; }

        //Launch test
        

        public abstract string InputActionName_NextWar { get; }

        public abstract string EngineHud_SymbolFor100 { get; }
        public abstract string EngineHud_SymbolFor1000 { get; }
        public abstract string EngineHud_SymbolFor10000 { get; }

        public abstract string GameMenu_BlockImportAchievements { get; }

        public abstract string EndScreen_PeaceVictoryQuote { get; }

        public abstract string VictoryType_DefeatBoss { get; }
        public abstract string VictoryType_Domination { get; }
        public abstract string VictoryType_WorldPeace { get; }

    }
}
