using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    abstract partial class AbsLanguage
    {
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

        public abstract string Lobby_GameVersion { get; }

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
        public abstract string ProfileEditor_SaveAndExit { get; }
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

        public abstract string Input_NextCity { get; }
        public abstract string Input_NextArmy { get; }
        public abstract string Input_NextBattle { get; }

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

        public abstract string EndGameStatistics_Title { get; }
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
        public abstract string EventMessage_DesertersText { get; }

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

        public abstract string GameMenu_SaveState { get; }
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
        public abstract string Input_ToggleHudDetail { get; }
        public abstract string Input_ToggleHudFocus { get; }
        public abstract string Input_ClickMessage { get; }
        public abstract string Input_Up { get; }
        public abstract string Input_Down { get; }
        public abstract string Input_Left { get; }
        public abstract string Input_Right { get; }
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
        public abstract string Resource_TypeName_SkinAndLinen { get; }
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
        public abstract string Hud_Queue { get; }

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
        public abstract string Conscript_TrainingSpeed { get; }
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

        public abstract string Info_PerSecond { get; }

        public abstract string Info_MinuteAverage { get; }

        public abstract string Message_CityOutOfFood_Title { get; }
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
        public abstract string CityCulture_AnimalBreeder_Description { get; }
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
    }
}
