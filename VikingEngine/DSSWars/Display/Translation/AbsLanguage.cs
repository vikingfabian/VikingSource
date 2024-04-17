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

        public abstract string Lobby_ProfilesSelectTitle { get; }
        public abstract string Lobby_ProfileNumbered { get; }
        public abstract string Lobby_ProfileEdit { get; }


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

        public abstract string ProfileEditor_Description { get; }
        public abstract string ProfileEditor_Bucket { get; }
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

       

        public abstract string Hud_GameSpeed { get; }
        public abstract string Hud_TotalIncome { get; }

        public abstract string Hud_Upkeep { get; }

        public abstract string Hud_ArmyUpkeep { get; }

        //public abstract string Hud_WorkForce { get; }

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

        public abstract string Hud_Cancel { get; }

        public abstract string Hud_Back { get; }
        public abstract string Hud_AreYouSure { get; }
        public abstract string Hud_Yes { get; }
        public abstract string Hud_XTimes { get; }

        public abstract string Hud_PurchaseTitle_Requirement { get; }
        public abstract string Hud_PurchaseTitle_Cost { get; }
        public abstract string Hud_PurchaseTitle_Gain { get; }
       
        public abstract string Hud_Purchase_ResourceCostOfAvailable { get; }

        public abstract string Hud_Purchase_CostWillIncreaseByX { get; }

        public abstract string Hud_Purchase_MaxCapacity { get; }

        public abstract string Hud_CompareMilitaryStrength_YourToOther { get; }

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

        public abstract string SoldierStats_Title { get; }
        public abstract string SoldierStats_GroupCountAndSoldierCount { get; }
        public abstract string SoldierStats_AttackStrengthLandSeaCity { get; }
        public abstract string SoldierStats_Health { get; }

        public abstract string SoldierStats_SpeedBonusLand { get; }
        public abstract string SoldierStats_SpeedBonusSea { get; }

        public abstract string ArmyOption_Halt { get; }
        public abstract string ArmyOption_Disband { get; }
        public abstract string ArmyOption_Divide { get; }
        public abstract string ArmyOption_RemoveX { get; }
        public abstract string ArmyOption_DisbandAll { get; }
        public abstract string ArmyOption_XGroupsOfType { get; }
        public abstract string ArmyOption_SendToX { get; }
        public abstract string ArmyOption_SendToNewArmy { get; }
        public abstract string ArmyOption_SendX { get; }
        public abstract string ArmyOption_SendAll { get; }
        public abstract string ArmyOption_DivideHalf { get; }
        public abstract string ArmyOption_MergeArmies { get; }


        public abstract string CityOption_Recruit { get; }
        public abstract string CityOption_RecruitType { get; }
        public abstract string CityOption_XMercenaries { get; }
        public abstract string CityOption_BuyXMercenaries { get; }

        public abstract string CityOption_Mercenaries_Description { get; }
        public abstract string CityOption_ExpandWorkForce { get; }
        public abstract string CityOption_ExpandWorkForce_IncreaseMax { get; }

        public abstract string CityOption_ExpandGuardSize { get; }


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


        public abstract string EndScreen_VictoryTitle { get; }

        public abstract List<string> EndScreen_VictoryQuotes { get; }

        public abstract string EndScreen_FailTitle { get; }

        public abstract List<string> EndScreen_FailureQuotes { get; }

        public abstract string EndScreen_WatchEpilogue { get; }

        public abstract string EndScreen_Epilogue_Title { get; }
        public abstract string EndScreen_Epilogue_Text { get; }


        public abstract string GameMenu_Title { get; }
        public abstract string GameMenu_ContinueGame { get; }
        public abstract string GameMenu_Resume { get; }

        public abstract string GameMenu_ExitGame { get; }
    }
}
