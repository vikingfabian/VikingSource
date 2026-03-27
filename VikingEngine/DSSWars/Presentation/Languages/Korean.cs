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
    partial class Korean : AbsLanguage
    {
        public override string Help_Work_Automatic => "작업은 자동으로 진행됩니다";
        public override string Tutorial_SecondCity => "두 번째 도시 확보";
        //## Spring update

        public override string InputAction_SkipAutomated => "자동화 건너뛰기";

        public override string Resource_WaterReason => "물은 지원 가능한 유닛 수와 생산 규모를 제한합니다";
        public override string BuildingType_Orchard => "과수원";
        public override string BuildingType_ManorLord => "영주 관저"; // "Lord's Manor/Residence"
        public override string BuildingType_ManorLord_Description => "식품 가공 잠금 해제";
        /// <summary>
        /// Will end diplomatic relations like alliance
        /// </summary>
        public override string Diplomacy_EndRelations => "관계 단절";

        /// <summary>
        /// Where a resource is produced or found
        /// </summary>
        public override string ItemSource => "아이템 출처";

        public override string ItemSource_Terrain => "지형";
        public override string ItemSource_Farm => "농장";
        public override string ItemSource_CraftStation => "제작소";
        public override string ItemSource_Gathering => "채집";

        public override string CityCulture_Nomad => "유목민";

        /// <summary>
        /// A generalized display of buffs and boons, example "+100%" or "Doubled"
        /// </summary>
        public override string Hud_ChangeFactor => "변동 계수: {0}";

        public override string Hud_Purchase_LowXCost => "낮은 {0} 비용";

        public override string WorkQueue_Title => "작업 대기열";
        public override string WorkQueue_Length => "남은 작업 목표";
        public override string WorkQueue_ActiveWorkers => "활동 중인 작업팀";
        public override string WorkQueue_IdleWorkers => "대기 중인 작업팀";

        public override string WorkTeam_Size => "주민들은 {0}명 단위로 일합니다";

        public override string ObjectUi_ViewOnMap => "지도에서 보기";
        public override string ObjectUi_StuckBuildOrders => "중단된 건설 명령";
        public override string Hud_AllArmies => "모든 군대";

        public override string Hud_CurrentPage => "현재 페이지";
        public override string Hud_AllPages => "전체 페이지";
        public override string Hud_ToAllCities => "모든 도시로";
        public override string Hud_ToFaction => "세력으로"; // "To Faction/Power"
        public override string Hud_FromFaction => "세력에서";
        public override string Hud_FactionWide => "세력 전체 설정 사용";
        /// <summary>
        /// This start a new city
        /// </summary>
        public override string Action_PlaceSettlement => "정착지 배치";

        public override string Editor_Animation_RemoveAllFramesButThis => "다른 프레임 모두 제거";
        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => "모든 건물 대기열 등록";
        public override string Hud_Purchase_AllTech => "모든 기술 대기열 등록";
        public override string BuildingType_CasualBarracks_Description => "병사 모집 시간이 병영 간에 분배됩니다";
        //Winter update patch + spring
        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Hud_Purchase_ResourceCost => "{1} {0}";

        //public override string DisplayMode => "화면 모드";
        //public override string DisplayMode_Windowed => "창 모드";
        //public override string DisplayMode_BorderlessFullscreen => "테두리 없는 전체 화면";

        //public override string GameSettings_RenderedMouseCursor => "소프트웨어 커서";
        //public override string GameSettings_MuteControllerDisconnect => "컨트롤러 연결 해제 알림 끄기";

        public override string Delivery_MaxDistance => "최대 배송 거리: {0}";
        public override string Tutorial_WillTakeAWhile => "시간이 좀 걸립니다. 나중에 다시 확인해 주세요.";

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => "{0} 완료 대기 중";
        public override string GameOverResults => "게임 기록";

        public override string UnitType_UnclaimedLand => "미점령지";
        public override string UnitType_Settler => "개척자";
        public override string UnitType_Settler_Description => "새 도시 건설";
        public override string Resource_ConsumedProduced => "소비 / 생산";
        public override string InputActionName_PlaceTarget => "목표 지점 설정";

        public override string FactionStartSize => "세력 시작 규모";
        public override string FactionStartSize_Full => "최대";
        public override string FactionStartSize_OneCity => "도시 1개";
        public override string FactionStartSize_Settler => "개척자 1기";

        //Winter update
        public override string Resource_StockpileLimit => "비축 한도";
        public override string GameMode_QuickMatch => "Quick Match";
        public override string GameMode_QuickMatch_Description =>
            "짧은 게임 형식입니다. 경쟁 국가들과의 본격적인 전쟁에 돌입하세요.";
        public override string Lobby_PlayerCount => "플레이어 수";
        public override string Lobby_TwoTeams => "2팀";
        public override string Hud_Produce => "생산:";
        public override string Tutorial_WaitForWorkerLevel => "작업자가 해당 레벨에 도달할 때까지 기다리기:";

        public override string Tutorial_PracticeOrSchool => "{0}에서 연습하거나 {1}을(를) 사용하기";
        public override string Tutorial_AddTag => "태그 추가:";
        public override string Tutorial_AddPin => "핀 추가:";
        public override string Tutorial_SelectMostTrees => "가장 나무가 많은 자신의 도시 찾기";
        public override string Tutorial_SelectACityWithX => "{0}이(가) 있는 도시 선택";

        public override string Tutorial_Select_NotCapital => ". 수도는 제외.";

        public override string Tutorial_SetXPriorityToY => "{0}의 우선순위를 {1}(으)로 설정";
        public override string Tutorial_AdvisorMission => "Advisor 임무";

        public override string Tutorial_AdvisorDescription =>
            "본편 게임이 시작되었습니다. Advisor가 유용한 임무로 튜토리얼을 확장합니다.";

        public override string Tutorial_EndAdvisor => "Advisor 종료";

        public override string Tutorial_AdvisorCompleteTitle => "Advisor 완료!";
        public override string Tutorial_AdvisorCompleteMessage => "내일도 축복이 가득하길!";

        public override string Hud_Search => "검색";

        public override string DifficultyDescription_ExtremeAggression => "극도 공격성";

        public override string MapFilter => "지도 필터";

        public override string Settings_TechMultiplier => "Tech 연구 속도";

        public override string EndScreen_MatchComplete => "매치 결과";

        public override string FactionName_DragonGem => "Dragon Gem";
        public override string FactionName_Tomten => "Tomten";
        public override string FactionName_Hælfolc => "Hælfolc";
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "{0}에서는 사용할 수 없습니다";

        public override string InputActionName_MiniMap => "Mini-map";

        //--
        public override string Error_SoundInitFailure => "사운드 초기화에 실패했습니다";

        public override string GameMenu_ControllerDisconnected => "컨트롤러가 연결 해제되었습니다";

        public override string Tutorial_HighPriority => "병사들은 우선순위가 높은 작업부터 수행합니다.";

        public override string BuildingType_Wall_Description => "벽은 부대를 공격으로부터 보호하고 약간의 공격 부스트를 제공합니다.";

        public override string BuildingType_Wall_Siege => "공성 무기는 벽의 방어력을 약화시킵니다.";

        public override string Conscript_BlockChance => "공격을 블록할 확률: {0}%";

        public override string Battle_DeclarWarReminder => "공격하기 전에 전쟁을 선포해야 합니다.";

        //--


        /// <summary>
        /// Name of this language
        /// </summary>
        public override string MyLanguage => "한국어";

        /// <summary>
        /// How to display a number of items. 0: item, 1:Number
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// Select language option
        /// </summary>
        public override string Lobby_Language => "언어";

        /// <summary>
        /// Start playing the game
        /// </summary>
        public override string Lobby_Start => "시작";

        /// <summary>
        /// Button to select local multiplayer count, 0:current player count
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "로컬 멀티플레이어";

        /// <summary>
        /// Title for menu where you select split screen player count
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "플레이어 수 선택";

        /// <summary>
        /// Description for local multiplayer
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "멀티플레이에는 Xbox 컨트롤러가 필요합니다";

        /// <summary>
        /// Move to next split screen position
        /// </summary>
        public override string Lobby_NextScreen => "다음 화면 위치";

        /// <summary>
        /// Players can select visual appearance and store them in a profile
        /// </summary>
        public override string Lobby_FlagSelectTitle => "깃발 선택";

        /// <summary>
        /// 0: Numbered 1 to 16
        /// </summary>
        public override string Lobby_FlagNumbered => "깃발 {0}";

        /// <summary>
        /// Game name and version number
        /// </summary>
        //public override string Lobby_GameVersion => "DSS War Party - 버전 {0}";

        public override string FlagEditor_Description => "자신의 깃발을 꾸미고 병사의 색상을 선택하세요.";


        /// <summary>
        /// Paint tool that fills an area with a color
        /// </summary>
        public override string FlagEditor_Bucket => "버킷";

        /// <summary>
        /// Opens flag profile editor
        /// </summary>
        public override string Lobby_FlagEdit => "깃발 편집";

        public override string Lobby_WarningTitle => "경고";
        public override string Lobby_IgnoreWarning => "무시";

        /// <summary>
        /// Warning when one player has no input selected.
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "플레이어 중 입력 장치가 없는 사람이 있습니다";

        /// <summary>
        /// Menu with content that are outside what most players will use.
        /// </summary>
        public override string Lobby_Extra => "추가 콘텐츠";

        /// <summary>
        /// The extra content is not translated or have full controller support.
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "경고! 이 콘텐츠는 현지화나 입력/접근성 지원이 완전하지 않습니다";

        public override string Lobby_MapSizeTitle => "맵 크기";

        /// <summary>
        /// Map size 1 name
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "매우 작음";

        /// <summary>
        /// Map size 2 name
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "작음";

        /// <summary>
        /// Map size 3 name
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "보통";

        /// <summary>
        /// Map size 4 name
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "큼";

        /// <summary>
        /// Map size 5 name
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "매우 큼";

        /// <summary>
        /// Map size 6 name
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "에픽";

        /// <summary>
        /// Map size description X by Y kilometers. 0: Width, 1: Height
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} km";

        /// <summary>
        /// Close game application
        /// </summary>
        public override string Lobby_ExitGame => "게임 종료";

        /// <summary>
        /// Display local multiplayer name, 0: player number
        /// </summary>
        public override string Player_DefaultName => "플레이어 {0}";

        /// <summary>
        /// In player profile editor. Opens menu with editor options
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "옵션";

        /// <summary>
        /// In player profile editor. Title for selecting flag colors
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "깃발 색상";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_MainColor => "메인 색상";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail1Color => "디테일 색상 1";

        /// <summary>
        /// In player profile editor. Flag color option
        /// </summary>
        public override string ProfileEditor_Detail2Color => "디테일 색상 2";

        /// <summary>
        /// In player profile editor. Title for selecting your soldiers’ colors
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "병사 색상";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_SkinColor => "피부 색상";

        /// <summary>
        /// In player profile editor. Soldier color option
        /// </summary>
        public override string ProfileEditor_HairColor => "머리 색상";

        /// <summary>
        /// In player profile editor. Open color palette and select color
        /// </summary>
        public override string ProfileEditor_PickColor => "색상 선택";

        /// <summary>
        /// In player profile editor. Adjust image position
        /// </summary>
        public override string ProfileEditor_MoveImage => "이미지 이동";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "왼쪽";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "오른쪽";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "위";

        /// <summary>
        /// In player profile editor. Move direction
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "아래";

        /// <summary>
        /// In player profile editor. Close editor without saving
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "저장 안 하고 나가기";

        /// <summary>
        /// In player profile editor. Tooltip for discarding
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "모든 변경 사항 취소";
        /// <summary>
        /// In player profile editor. Save changes and close editor
        /// </summary>
        public override string Hud_SaveAndExit => "저장하고 나가기";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Hue => "색상 (Hue)";

        /// <summary>
        /// In player profile editor. Part of the Hue, Saturation and Lightness color options.
        /// </summary>
        public override string ProfileEditor_Lightness => "밝기 (Lightness)";

        /// <summary>
        /// In player profile editor. Move between flag and soldier color options.
        /// </summary>
        public override string ProfileEditor_NextColorType => "다음 색상 타입";

        /// <summary>
        /// Current running speed of the game, compared to real time
        /// </summary>
        public override string Hud_GameSpeedLabel => "게임 속도: {0}x";

        public override string Input_GameSpeed => "게임 속도";

        /// <summary>
        /// Ingame display. Unit gold production
        /// </summary>
        public override string Hud_TotalIncome => "총 수입/초: {0}";

        /// <summary>
        /// Unit gold cost.
        /// </summary>
        public override string Hud_Upkeep => "유지비: {0}";
        public override string Hud_ArmyUpkeep => "군대 유지비: {0}";

        /// <summary>
        /// Ingame display. Soldiers protecting a building.
        /// </summary>
        public override string Hud_GuardCount => "경비병";

        public override string Hud_IncreaseMaxGuardCount => "최대 경비병 수 {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "도시를 확장해야 합니다.";

        public override string Hud_SoldierCount => "병사 수";

        public override string Hud_SoldierGroupsCount => "부대 수";

        /// <summary>
        /// Ingame display. Unit calculated battle strength.
        /// </summary>
        public override string Hud_StrengthRating => "전투력";

        /// <summary>
        /// Ingame display. Calculated battle strength for the whole nation.
        /// </summary>
        public override string Hud_TotalStrengthRating => "국가 전투력: {0}";

        /// <summary>
        /// Ingame display. Extra men coming from outside the city state.
        /// </summary>
        public override string Hud_Immigrants => "이민자";

        public override string Hud_CityCount => "도시 수: {0}";
        public override string Hud_ArmyCount => "군대 수: {0}";

        /// <summary>
        /// Mini button to repeat a purchase a number of times. E.G. "x5"
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "요구 조건";
        public override string Hud_PurchaseTitle_Cost => "비용";
        public override string Hud_PurchaseTitle_Gain => "획득";

        /// <summary>
        /// How much of a resource that will be used, "5 gold. (Available: 10)". There will be a "cost" title above the text. 0: Resource, 1: cost, 2: available
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}. (보유: {2})";

        public override string Hud_Purchase_CostWillIncreaseByX => "비용이 {0}만큼 증가합니다";

        public override string Hud_Purchase_MaxCapacity => "최대 수용량에 도달했습니다";

        public override string Hud_CompareMilitaryStrength_YourToOther => "전투력: 아군 {0} - 적군 {1}";

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
        public override string Hud_Battle => "전투";

        /// <summary>
        /// Describes button input. Pause.
        /// </summary>
        public override string Input_Pause => "일시 정지";

        /// <summary>
        /// Describes button input. Resume from paused.
        /// </summary>
        public override string Input_ResumePaused => "재개";

        /// <summary>
        /// Generic money resource
        /// </summary>
        public override string ResourceType_Gold => "골드";

        /// <summary>
        /// Working men resource
        /// </summary>
        public override string ResourceType_Workers => "일꾼";

        public override string ResourceType_Workers_Description => "일꾼은 수입을 제공합니다. 또한 군대에 징집되어 병사로 사용됩니다.";


        /// <summary>
        /// The resource used in diplomacy
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "외교 포인트";

        /// <summary>
        /// 0: How many points you got, 1: Soft max value (will increase much slower after this), 2: Hard limit
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "외교 포인트: {0} / {1} ({2})";

        /// <summary>
        /// City building type. Building for knights and diplomats.
        /// </summary>
        public override string Building_NobleHouse => "귀족의 집";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "{0}초마다 외교 포인트 +1";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "외교 포인트 최대치 +{0}";
        public override string Building_NobleHouse_UnlocksKnight => "기사 유닛 해금";

        public override string Building_BuildAction => "건설";
        public override string Building_IsBuilt => "건설 완료";

        /// <summary>
        /// City building type. Evil mass production.
        /// </summary>
        public override string Building_DarkFactory => "다크 팩토리";

        /// <summary>
        /// In game settings menu. Sums all difficulty options in percentage.
        /// </summary>
        public override string Settings_TotalDifficulty => "총 난이도 {0}%";

        /// <summary>
        /// In game settings menu. Base difficulty option.
        /// </summary>
        public override string Settings_DifficultyLevel => "난이도 {0}%";

        /// <summary>
        /// In game settings menu. Option for creating new maps instead of loading one.
        /// </summary>
        public override string Settings_GenerateMaps => "새 맵 생성";

        /// <summary>
        /// In game settings menu. Creating new maps has a longer loading time
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "새 맵을 생성하면 기존 맵을 불러오는 것보다 로딩이 느립니다.";


        /// <summary>
        /// In game settings menu. Difficulty option. Block the ability to play the game while paused.
        /// </summary>
        public override string Settings_AllowPause => "일시 정지 중 명령 허용";

        /// <summary>
        /// In game settings menu. Difficulty option. Have bosses that enter the game.
        /// </summary>
        public override string Settings_BossEvents => "보스 이벤트";

        /// <summary>
        /// In game settings menu. Difficulty option. No Boss description.
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "보스 이벤트를 비활성화하면 엔딩이 없는 샌드박스 모드로 전환됩니다.";
        /// <summary>
        /// Options for automating game mechanics. Menu title.
        /// </summary>
        public override string Automation_Title => "자동화";

        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "노동력이 최대치에 도달할 때까지 대기합니다";

        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "수입이 마이너스일 경우 자동화를 일시 정지합니다";

        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_Priority => "대도시가 우선순위를 가집니다";

        /// <summary>
        /// Options for automating game mechanics. Information about how the automation works.
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "초당 최대 한 번의 구매만 수행합니다";


        /// <summary>
        /// Button caption for action. A specialized building for knights and diplomats.
        /// </summary>
        public override string HudAction_BuyItem => "{0} 구매";


        /// <summary>
        /// The state of peace or war between two nations
        /// </summary>
        public override string Diplomacy_RelationType => "관계";

        /// <summary>
        /// Title for list of relations other factions have with each other
        /// </summary>
        public override string Diplomacy_RelationToOthers => "다른 세력과의 관계";

        /// <summary>
        /// Diplomatic relation. You are in direct control over the nation’s resources.
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "속국";

        /// <summary>
        /// Diplomatic relation. Full co-operation.
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "동맹";

        /// <summary>
        /// Diplomatic relation. Reduced chance of war.
        /// </summary>
        public override string Diplomacy_RelationType_Good => "우호";

        /// <summary>
        /// Diplomatic relation. Peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "평화";

        /// <summary>
        /// Diplomatic relation. Have not yet made any contact.
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "중립";

        /// <summary>
        /// Diplomatic relation. Temporary peace agreement.
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "휴전";

        /// <summary>
        /// Diplomatic relation. War.
        /// </summary>
        public override string Diplomacy_RelationType_War => "전쟁";

        /// <summary>
        /// Diplomatic relation. War with no chance of peace.
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "총력전";


        /// <summary>
        /// Diplomatic communication. How well you can discuss terms. 0: SpeakTerms
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "협상 상태: {0}";

        /// <summary>
        /// Diplomatic communication. Better than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "양호";

        /// <summary>
        /// Diplomatic communication. Normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "보통";

        /// <summary>
        /// Diplomatic communication. Worse than normal.
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "악화";

        /// <summary>
        /// Diplomatic communication. Will not communicate.
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "단절";

        /// <summary>
        /// Diplomatic action. Make a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "{0}와(과) 새로운 외교 관계 수립";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferPeace => "평화 제안";

        /// <summary>
        /// Diplomatic action. Suggest a new diplomatic relation.
        /// </summary>
        public override string Diplomacy_OfferAlliance => "동맹 제안";

        /// <summary>
        /// Diplomatic title. Another player suggested a new diplomatic relation. 0: player name
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0}이(가) 새로운 외교 관계를 제안했습니다";

        /// <summary>
        /// Diplomatic action. Accept new diplomatic relation.
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "제안 수락";

        /// <summary>
        /// Diplomatic description. Another player suggested a new diplomatic relation. 0: relation type
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "새로운 관계 제안: {0}";

        /// <summary>
        /// Diplomatic action. Make another nation serve you.
        /// </summary>
        public override string Diplomacy_AbsorbServant => "속국으로 흡수";

        /// <summary>
        /// Diplomatic description. Is against evil.
        /// </summary>
        public override string Diplomacy_LightSide => "정의 진영의 동맹국";

        /// <summary>
        /// Diplomatic description. How long the truce will last.
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "{0}초 후에 종료";

        /// <summary>
        /// Diplomatic action. Make the truce last longer.
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "휴전 연장";

        /// <summary>
        /// Diplomatic description. How long the truce will be extended.
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "휴전이 {0}초 연장됩니다";

        /// <summary>
        /// Diplomatic description. Going against an agreed relation will cost diplomatic points.
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "관계를 파기하면 외교 포인트 {0}이(가) 소모됩니다";

        /// <summary>
        /// Diplomatic description for allies.
        /// </summary>
        public override string Diplomacy_AllyDescription => "동맹국은 전쟁 선포를 공유합니다.";

        /// <summary>
        /// Diplomatic description for good relation.
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "전쟁 선포가 제한됩니다.";


        /// <summary>
        /// Diplomatic description. You must have a larger military force than your servant (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "군사력이 {0}배 이상 강해야 합니다";


        /// <summary>
        /// Diplomatic description. Servant must be stuck in a hopeless war (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "속국은 더 강한 적과의 전쟁에서 궁지에 몰려 있어야 합니다";


        /// <summary>
        /// Diplomatic description. A servant can't own too many cities (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "속국은 최대 {0}개의 도시만 보유할 수 있습니다";


        /// <summary>
        /// Diplomatic description. Cost in diplomatic points will increase (another nation that you will control).
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "속국 수가 늘어날수록 외교 포인트 비용이 증가합니다";


        /// <summary>
        /// Diplomatic description. The result of servant relation, peaceful take over of another nation.
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "상대 세력을 평화적으로 흡수합니다";


        /// <summary>
        /// Message when you receive a war declaration
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "전쟁이 선포되었습니다!";


        /// <summary>
        /// The truce timer has run out, and you go back to war
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "휴전이 종료되었습니다";
        /// <summary>
        /// Stats that are shown on the end game screen. Display title.
        /// </summary>
        public override string Statistics_Title => "통계";

        /// <summary>
        /// Stats that are shown on the end game screen. Total ingame time passed.
        /// </summary>
        public override string EndGameStatistics_Time => "게임 진행 시간: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. How many soldiers you bought.
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "모집한 병사 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that died in battle.
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "전사한 병사 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of opponent soldiers you killed in battle.
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "적군 사살 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of your soldiers that have left you.
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "탈영병 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities won in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "점령한 도시 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of cities lost in battle.
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "잃은 도시 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle win results.
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "승리한 전투 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Count of battle lost results.
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "패배한 전투 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made by you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "선전포고 횟수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Diplomacy. War declarations made toward you.
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "적의 선전포고 횟수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Allies made through diplomacy.
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "외교 동맹 수: {0}";

        /// <summary>
        /// Stats that are shown on the end game screen. Servants made through diplomacy. Servants’ cities and armies become yours.
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "속국 수: {0}";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_Army => "군대";

        /// <summary>
        /// Collective unit type on the map. Army of soldiers.
        /// </summary>
        public override string UnitType_SoldierGroup => "부대";

        /// <summary>
        /// Collective unit type on the map. Common name for village or city.
        /// </summary>
        public override string UnitType_City => "도시";

        /// <summary>
        /// A group selection of armies
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "군대 그룹, 수: {0}";

        /// <summary>
        /// Name for a specialized type of soldier. Standard front line soldier.
        /// </summary>
        public override string UnitType_Soldier => "보병";

        /// <summary>
        /// Name for a specialized type of soldier. Naval battle soldier.
        /// </summary>
        public override string UnitType_Sailor => "해병";

        /// <summary>
        /// Name for a specialized type of soldier. Drafted peasants.
        /// </summary>
        public override string UnitType_Folkman => "농민병";

        /// <summary>
        /// Name for a specialized type of soldier. Shield and spear unit.
        /// </summary>
        public override string UnitType_Spearman => "창병";

        /// <summary>
        /// Name for a specialized type of soldier. Elite force, part of the King's guard.
        /// </summary>
        public override string UnitType_HonorGuard => "근위대";

        /// <summary>
        /// Name for a specialized type of soldier. Anti cavalry, wears long two-handed spears.
        /// </summary>
        public override string UnitType_Pikeman => "장창병";

        /// <summary>
        /// Name for a specialized type of soldier. Armored cavalry unit.
        /// </summary>
        public override string UnitType_Knight => "나이트";

        /// <summary>
        /// Name for a specialized type of soldier. Bow and arrow.
        /// </summary>
        public override string UnitType_Archer => "궁수";

        /// <summary>
        /// Name for a specialized type of soldier.
        /// </summary>
        public override string UnitType_Crossbow => "석궁병";

        /// <summary>
        /// Name for a specialized type of soldier. Warmachine that slings large spears.
        /// </summary>
        public override string UnitType_Ballista => "발리스타";

        /// <summary>
        /// Name for a specialized type of soldier. A fantasy troll wearing a cannon.
        /// </summary>
        public override string UnitType_Trollcannon => "트롤 캐논";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier from the forest.
        /// </summary>
        public override string UnitType_GreenSoldier => "숲의 병사";

        /// <summary>
        /// Name for a specialized type of soldier. Naval unit from the north.
        /// </summary>
        public override string UnitType_Viking => "바이킹";

        /// <summary>
        /// Name for a specialized type of soldier. The evil master boss.
        /// </summary>
        public override string UnitType_DarkLord => "다크 로드";

        /// <summary>
        /// Name for a specialized type of soldier. Soldier that carries a large flag.
        /// </summary>
        public override string UnitType_Bannerman => "기수";

        /// <summary>
        /// Name for a military unit. Soldier carrying ship. 0: unit type it carries
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0} 전함";

        public override string UnitType_Description_Soldier => "범용 병사 유닛입니다.";
        public override string UnitType_Description_Sailor => "해상 전투에서 강합니다.";
        public override string UnitType_Description_Folkman => "훈련되지 않은 값싼 병사들입니다.";
        public override string UnitType_Description_HonorGuard => "유지비가 없는 정예 병사입니다.";
        public override string UnitType_Description_Knight => "평지 전투에서 강합니다.";
        public override string UnitType_Description_Archer => "보호받을 때만 강력합니다.";
        public override string UnitType_Description_Crossbow => "강력한 원거리 병사입니다.";
        public override string UnitType_Description_Ballista => "도시 공격에 강합니다.";
        public override string UnitType_Description_GreenSoldier => "모두가 두려워하는 엘프 전사입니다.";

        public override string UnitType_Description_DarkLord => "최종 보스입니다.";

        /// <summary>
        /// Information about a soldier type
        /// </summary>
        public override string SoldierStats_Title => "유닛별 능력치";

        /// <summary>
        /// How many groups of soldiers
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "부대 {0}개, 총 병력 {1}명";

        /// <summary>
        /// Soldiers will have different strengths depending if the attack on open field, from ships or attacking a settlement
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "공격력: 육상 {0} | 해상 {1} | 도시 {2}";

        /// <summary>
        /// How many wounds a soldier can endure
        /// </summary>
        public override string SoldierStats_Health => "체력";

        /// <summary>
        /// Some soldiers will increase the army movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "육상 이동 속도 보너스: {0}";

        /// <summary>
        /// Some soldiers will increase the ship movement speed
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "해상 이동 속도 보너스: {0}";

        /// <summary>
        /// Purchased soldiers will start as recruits and complete their training after a few minutes.
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "훈련 시간: {0}분. 도시 인접 지역에서는 두 배 빠르게 훈련됩니다.";
        /// <summary>
        /// Menu option to control an army. Make them stop moving.
        /// </summary>
        public override string ArmyOption_Halt => "정지";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_Disband => "부대 해산";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_Divide => "부대 분할";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_RemoveX => "{0} 제거";

        /// <summary>
        /// Menu option to control an army. Remove soldiers.
        /// </summary>
        public override string ArmyOption_DisbandAll => "전부 해산";

        /// <summary>
        /// Menu option to control an army. 0: Count, 1: Unit type
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} 부대: {0}개";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToX => "{0}(으)로 병력 파견";

        public override string ArmyOption_MergeAllArmies => "모든 군대 합류";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "새 군대로 분할 배치";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendX => "{0} 파견";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_SendAll => "전부 파견";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_DivideHalf => "부대를 절반으로 분할";

        /// <summary>
        /// Menu option to control an army. Options to send soldiers between armies.
        /// </summary>
        public override string ArmyOption_MergeArmies => "군대 합류";
        /// <summary>
        /// Purchase soldiers.
        /// </summary>
        public override string UnitType_Recruit => "신병 모집";

        /// <summary>
        /// Purchase soldiers of type. 0:type
        /// </summary>
        public override string CityOption_RecruitType => "{0} 모집";

        /// <summary>
        /// Number of paid soldiers
        /// </summary>
        public override string CityOption_XMercenaries => "용병 수: {0}";

        /// <summary>
        /// Indicates the number of mercenaries currently available for hire from the market
        /// </summary>
        public override string Hud_MercenaryMarket => "고용 가능한 시장 용병";

        /// <summary>
        /// Purchase a number of paid soldiers
        /// </summary>
        public override string CityOption_BuyXMercenaries => "용병 {0}명 고용";

        public override string CityOption_Mercenaries_Description => "용병을 고용하여 일꾼 대신 병력을 확보합니다.";


        /// <summary>
        /// Button caption for action. Create housing for more workers.
        /// </summary>
        public override string CityOption_ExpandWorkForce => "노동력 확장";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "최대 노동력 +{0}";
        public override string CityOption_ExpandGuardSize => "경비병 확장";

        public override string CityOption_Damages => "피해: {0}";
        public override string CityOption_Repair => "피해 복구";
        public override string CityOption_RepairGain => "{0} 피해 복구";

        public override string CityOption_Repair_Description => "피해가 많을수록 수용 가능한 일꾼 수가 줄어듭니다.";

        public override string CityOption_BurnItDown => "불태워 버리기";
        public override string CityOption_BurnItDown_Description => "노동력을 제거하고 최대 피해를 입힙니다.";

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
        public override string FactionName_BearClaw => "Bear Claw";

        /// <summary>
        /// Viking flavored kingdom in the north. Uses a cock symbol.
        /// </summary>
        public override string FactionName_NordicSpur => "Nordic Spur";

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
        public override string FactionName_Player => "플레이어 {0}";
        /// <summary>
        /// Message for when a miniboss is approaching on ships from the south.
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "적이 접근 중!";
        public override string EventMessage_HaraMercenaryText => "남쪽에서 하라 용병들이 나타났습니다.";


        /// <summary>
        /// First warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_ProphesyTitle => "어둠의 예언";
        public override string EventMessage_ProphesyText => "‘Eye of Doom’이 곧 나타납니다. 적들이 그와 손을 잡을 것입니다!";


        /// <summary>
        /// Second warning that the main boss will appear.
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "어둠의 시대";
        public override string EventMessage_FinalBossEnterText => "‘Eye of Doom’이 전장에 나타났습니다!";


        /// <summary>
        /// Message when the main boss will meet you on the battlefield.
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "절망의 전투";
        public override string EventMessage_FinalBattleText => "다크 로드가 전장에 합류했습니다. 지금이 그를 끝낼 마지막 기회입니다!";


        /// <summary>
        /// Message when soldiers leave the army when you can't pay their upkeep
        /// </summary>
        public override string EventMessage_DesertersTitle => "탈영병 발생!";
        public override string EventMessage_DesertersText_Money => "급여를 받지 못한 병사들이 군대를 이탈하고 있습니다.";


        public override string DifficultyDescription_AiAggression => "AI 공격성: {0}";
        public override string DifficultyDescription_BossSize => "보스 규모: {0}";
        public override string DifficultyDescription_BossEnterTime => "보스 등장 시간: {0}";
        public override string DifficultyDescription_AiEconomy => "AI 경제력: {0}%";
        public override string DifficultyDescription_AiDelay => "AI 행동 지연: {0}";
        public override string DifficultyDescription_DiplomacyDifficulty => "외교 난이도: {0}";
        public override string DifficultyDescription_MercenaryCost => "용병 비용: {0}";
        public override string DifficultyDescription_HonorGuards => "근위대 수: {0}";


        /// <summary>
        /// Game has ended in success.
        /// </summary>
        public override string EndScreen_VictoryTitle => "승리!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "평화의 시대가 찾아오면, 우리는 죽은 자들을 애도하리라.",
    "모든 승리에는 희생의 그림자가 깃든다.",
    "이 여정의 끝은 용사들의 넋으로 물들어 있다.",
    "우리의 마음은 승리로 빛나지만, 쓰러진 이들의 무게로 무겁다."
};

        public override string EndScreen_DominationVictoryQuote => "신들께서 나를 선택하셨다. 세상을 지배하라!";


        /// <summary>
        /// Game has ended in failure.
        /// </summary>
        public override string EndScreen_FailTitle => "패배!";

        /// <summary>
        /// Quotes from the leader character you play in the game
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "지친 육신과 불면의 밤 끝에, 우리는 이 종말을 받아들인다.",
    "패배는 우리의 땅을 어둡게 하지만, 결의의 불빛까지는 꺼뜨리지 못한다.",
    "우리의 심장 속 불꽃이 사라져도, 그 재에서 아이들이 새벽을 일으킬 것이다.",
    "우리의 이야기가 내일의 승리를 피워낼 불씨가 되리라."
};


        /// <summary>
        /// A small cutscene at the end of the game
        /// </summary>
        public override string EndScreen_WatchEpilogue => "에필로그 보기";

        /// <summary>
        /// Cutscene title
        /// </summary>
        public override string EndScreen_Epilogue_Title => "에필로그";

        /// <summary>
        /// Cutscene introduction
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160년 전의 이야기";
        /// <summary>
        /// The Prologue is a short poem about the game's story
        /// </summary>
        public override string GameMenu_WatchPrologue => "프롤로그 보기";

        public override string Prologue_Title => "프롤로그";

        /// <summary>
        /// The poem must be three lines, the fourth line will be pulled from the names translations to present the name of the boss
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "밤마다 꿈이 그대를 괴롭히고,",
    "어둠의 미래가 예언된다.",
    "그의 등장을 맞이하라,",
};


        /// <summary>
        /// Ingame menu when pausing
        /// </summary>
        public override string GameMenu_Title => "게임 메뉴";

        /// <summary>
        /// Continue playing the game after end screen
        /// </summary>
        public override string GameMenu_ContinueGame => "계속하기";

        /// <summary>
        /// Continue playing the game
        /// </summary>
        public override string GameMenu_Resume => "재개";

        /// <summary>
        /// Exit to game lobby
        /// </summary>
        public override string GameMenu_ExitGame => "게임 종료";

        public override string Hud_Save => "저장";
        public override string GameMenu_SaveStateWarnings => "경고! 게임이 업데이트되면 저장 파일이 삭제됩니다.";
        public override string GameMenu_LoadState => "불러오기";
        public override string GameMenu_ContinueFromSave => "저장된 게임에서 이어하기";

        public override string GameMenu_AutoSave => "자동 저장";

        public override string GameMenu_Load_PlayerCountError => "저장된 파일에 맞는 플레이어 수를 설정해야 합니다: {0}";

        public override string Progressbar_MapLoadingState => "맵 로딩 중: {0}";

        public override string Progressbar_ProgressComplete => "완료";

        /// <summary>
        /// 0: progress in percentage, 1: fail count
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "생성 중: {0}%. (실패 {1})";

        /// <summary>
        /// 0: current part, 1: number of parts
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "파트 {0}/{1}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_SaveProgress => "저장 중: {0}";

        /// <summary>
        /// 0: Percentage or Complete
        /// </summary>
        public override string Progressbar_LoadProgress => "불러오는 중: {0}";

        /// <summary>
        /// Progress done, waiting for player input
        /// </summary>
        public override string Progressbar_PressAnyKey => "아무 키나 눌러 계속하기";


        /// <summary>
        /// A short tutorial where you are supposed to buy and move a soldier. All advanced controls are locked away until the tutorial is complete.
        /// </summary>
        public override string Tutorial_MenuOption => "튜토리얼 실행";
        public override string Tutorial_MissionsTitle => "튜토리얼 임무";
        public override string Tutorial_Mission_BuySoldier => "도시를 선택하고 병사를 모집하세요.";
        public override string Tutorial_Mission_MoveArmy => "군대를 선택하고 이동 명령을 내리세요.";

        public override string Tutorial_CompleteTitle => "튜토리얼 완료!";
        public override string Tutorial_CompleteMessage => "전체 줌 및 고급 게임 옵션이 해금되었습니다.";


        /// <summary>
        /// Displays the button input
        /// </summary>
        public override string Tutorial_SelectInput => "선택";
        public override string Tutorial_MoveInput => "이동 명령";



        /// <summary>
        /// Versus. Text describing the two armies that will go into battle
        /// </summary>
        public override string Hud_Versus => "VS";

        public override string Hud_WardeclarationTitle => "전쟁 선포";

        public override string ArmyOption_Attack => "공격";


        //----
        /// <summary>
        /// In game settings menu. Change what keys and buttons do when pressed
        /// </summary>
        public override string Settings_ButtonMapping => "키 설정";


        /// <summary>
        /// Input type, standard PC input
        /// </summary>
        public override string Input_Source_Keyboard => "키보드 & 마우스";

        /// <summary>
        /// Input type, handheld controller like the xbox uses
        /// </summary>
        public override string Input_Source_Controller => "컨트롤러";



        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */
        public override string CityMenu_SalePricesTitle => "판매 가격";
        public override string Blueprint_Title => "청사진";
        public override string Resource_Tab_Overview => "개요";
        public override string Resource_Tab_Stockpile => "비축량";

        public override string Resource => "자원";
        public override string Resource_StockPile_Info => "자원 저장 목표량을 설정하면, 일꾼들이 다른 자원 작업 시점을 조정할 수 있습니다.";

        public override string Resource_TypeName_Water => "물";
        public override string Resource_TypeName_Wood => "목재";
        public override string Resource_TypeName_Fuel => "연료";
        public override string Resource_TypeName_Stone => "석재";
        public override string Resource_TypeName_RawFood => "식재료";
        public override string Resource_TypeName_Food => "식량";
        public override string Resource_TypeName_Beer => "맥주";
        public override string Resource_TypeName_Wheat => "밀";
        public override string Resource_TypeName_Linen => "리넨";
        //public override string Resource_TypeName_SkinAndLinen => "가죽과 리넨";
        public override string Resource_TypeName_IronOre => "철광석";
        public override string Resource_TypeName_GoldOre => "금광석";
        public override string Resource_TypeName_Iron => "철";

        public override string Resource_TypeName_SharpStick => "뾰족한 막대기";
        public override string Resource_TypeName_Sword => "검";
        public override string Resource_TypeName_KnightsLance => "기사의 랜스";
        public override string Resource_TypeName_TwoHandSword => "쯔바이헨더";
        public override string Resource_TypeName_Bow => "활";

        public override string Resource_TypeName_LightArmor => "경갑옷";
        public override string Resource_TypeName_MediumArmor => "중갑옷";
        public override string Resource_TypeName_HeavyArmor => "중갑";

        public override string ResourceType_Children => "아이들";
        public override string BuildingType_DefaultName => "건물";
        public override string BuildingType_WorkerHut => "일꾼 오두막";
        public override string BuildingType_Brewery => "양조장";
        public override string BuildingType_Postal => "우편소";
        public override string BuildingType_Recruitment => "모집소";
        public override string BuildingType_Barracks => "병영";
        public override string BuildingType_PigPen => "돼지우리";
        public override string BuildingType_HenPen => "닭장";
        public override string BuildingType_WorkBench => "작업대";
        public override string BuildingType_Carpenter => "목공소";
        public override string BuildingType_CoalPit => "목탄 구덩이";
        public override string DecorType_Statue => "조각상";
        public override string DecorType_Pavement => "포장 도로";
        public override string BuildingType_Smith => "대장간";
        public override string BuildingType_Cook => "요리소";
        public override string BuildingType_Storage => "창고";

        public override string BuildingType_ResourceFarm => "{0} 농장";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "일꾼 최대 수를 {0}만큼 증가시킵니다.";
        public override string BuildingType_Tavern_Description => "일꾼들이 식사할 수 있습니다.";
        public override string BuildingType_Tavern_Brewery => "맥주 생산";
        public override string BuildingType_Postal_Description => "다른 도시로 자원을 보냅니다.";
        public override string BuildingType_Recruitment_Description => "다른 도시로 인력을 보냅니다.";
        public override string BuildingType_Barracks_Description => "인력과 장비를 사용하여 병사를 모집합니다.";
        public override string BuildingType_PigPen_Description => "돼지를 사육해 식량과 가죽을 얻습니다.";
        public override string BuildingType_HenPen_Description => "닭과 달걀을 생산해 식량을 제공합니다.";
        public override string BuildingType_Decor_Description => "장식용 건물입니다.";
        public override string BuildingType_Farm_Description => "자원을 재배합니다.";

        public override string BuildingType_Cook_Description => "식량 제작소입니다.";
        public override string BuildingType_Bench_Description => "아이템 제작소입니다.";

        public override string BuildingType_Smith_Description => "금속 제작소입니다.";
        public override string BuildingType_Carpenter_Description => "목재 제작소입니다.";

        public override string BuildingType_Nobelhouse_Description => "기사와 외교관의 거주지입니다.";
        public override string BuildingType_CoalPit_Description => "효율적인 연료 생산 건물입니다.";
        public override string BuildingType_Storage_Description => "자원 보관소입니다.";

        public override string MenuTab_Info => "정보";
        public override string MenuTab_Work => "작업";
        public override string MenuTab_Recruit => "모집";
        public override string MenuTab_Resources => "자원";
        public override string MenuTab_Trade => "거래";
        public override string MenuTab_Build => "건설";
        public override string MenuTab_Economy => "경제";
        public override string MenuTab_Delivery => "배송";

        public override string MenuTab_Build_Description => "도시에 건물을 배치합니다.";
        public override string MenuTab_BlackMarket_Description => "도시에 건물을 배치합니다.";
        public override string MenuTab_Resources_Description => "도시에 건물을 배치합니다.";
        public override string MenuTab_Work_Description => "도시에 건물을 배치합니다.";
        public override string MenuTab_Automation_Description => "도시에 건물을 배치합니다.";

        public override string BuildHud_OutsideCity => "도시 영역 밖입니다.";
        public override string BuildHud_OutsideFaction => "영토 밖입니다!";

        public override string BuildHud_OccupiedTile => "이미 점유된 지역입니다.";

        public override string Build_PlaceBuilding => "건설";
        public override string Build_DestroyBuilding => "철거";
        public override string Build_ClearTerrain => "지형 정리";

        public override string Build_ClearOrders => "건설 명령 초기화";
        public override string Build_Order => "건설 명령";
        public override string Build_OrderQue => "건설 대기열: {0}";
        public override string Build_AutoPlace => "자동 배치";

        public override string Work_OrderPrioTitle => "작업 우선순위";
        public override string Work_OrderPrioDescription => "우선순위는 1 (낮음)에서 {0} (높음)까지입니다.";

        public override string Work_OrderPrio_No => "우선순위 없음. 작업하지 않습니다.";
        public override string Work_OrderPrio_Min => "최소 우선순위.";
        public override string Work_OrderPrio_Max => "최대 우선순위.";

        public override string Work_Move => "아이템 이동";

        public override string Work_GatherXResource => "{0} 채집";
        public override string Work_CraftX => "{0} 제작";
        public override string Work_Farming => "농사";
        public override string Work_Mining => "채굴";
        public override string Work_Trading => "거래";

        public override string Work_AutoBuild => "자동 건설 및 확장";

        public override string WorkerHud_WorkType => "작업 상태: {0}";
        public override string WorkerHud_Carry => "운반 중: {0} {1}";
        public override string WorkerHud_Energy => "에너지: {0}";
        public override string WorkerStatus_Exit => "노동에서 이탈";
        public override string WorkerStatus_Eat => "식사";
        public override string WorkerStatus_Till => "경작";
        public override string WorkerStatus_Plant => "심기";
        public override string WorkerStatus_Gather => "수확";
        public override string WorkerStatus_PickUpResource => "자원 줍기";
        public override string WorkerStatus_DropOff => "자원 내려놓기";
        public override string WorkerStatus_BuildX => "{0} 건설";
        public override string WorkerStatus_TrossReturnToArmy => "군대로 복귀";
        public override string Hud_ToggleFollowFaction => "세력 설정 연동 전환";
        public override string Hud_FollowFaction_Yes => "세력 전역 설정을 사용 중입니다.";
        public override string Hud_FollowFaction_No => "지역 설정을 사용 중입니다. (전역 값: {0})";

        public override string Hud_Idle => "대기 중";
        public override string Hud_NoLimit => "제한 없음";

        public override string Hud_None => "없음";
        public override string Hud_ProductionQueue => "생산 대기열";

        public override string Hud_EmptyList => "- 비어 있음 -";

        public override string Hud_RequirementOr => "- 또는 -";

        public override string Hud_BlackMarket => "암시장";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "도시 선택";

        public override string Conscription_Title => "징집";
        public override string Conscript_WeaponTitle => "무기";
        public override string Conscript_ArmorTitle => "방어구";
        public override string Conscript_TrainingTitle => "훈련";

        public override string Conscript_SpecializationTitle => "전문화";
        public override string Conscript_SpecializationDescription => "특정 전투 유형의 공격력이 {0}만큼 증가하며, 다른 모든 능력은 감소합니다.";
        public override string Conscript_SelectBuilding => "병영 선택";

        public override string Conscript_WeaponDamage => "무기 공격력";
        public override string Conscript_ArmorHealth => "방어구 체력";
        public override string Conscript_AttackSpeed => "공격 속도";
        public override string Conscript_TrainingTime => "훈련 시간";

        public override string Conscript_Training_Minimal => "최소";
        public override string Conscript_Training_Basic => "기초";
        public override string Conscript_Training_Skillful => "숙련";
        public override string Conscript_Training_Professional => "전문";

        public override string Conscript_Specialization_Field => "평지전";
        public override string Conscript_Specialization_Sea => "해상전";
        public override string Conscript_Specialization_Siege => "공성전";
        public override string Conscript_Specialization_Traditional => "전통전";
        public override string Conscript_Specialization_AntiCavalry => "대기병";

        public override string Conscription_Status_CollectingEquipment => "장비 수집 중: {0}";
        public override string Conscription_Status_CollectingMen => "병력 모집 중: {0}";
        public override string Conscription_Status_Training => "훈련 중: {0}";

        public override string ArmyHud_Food_Reserves_X => "식량 비축량: {0}";
        public override string ArmyHud_Food_Upkeep_X => "식량 소모량: {0}";
        public override string ArmyHud_Food_Costs_X => "식량 비용: {0}";

        public override string Deliver_WillSendXInfo => "한 번에 {0}개씩 보냅니다.";
        public override string Delivery_ListTitle => "배송 서비스 선택";
        public override string Delivery_DistanceX => "거리: {0}";
        public override string Delivery_DeliveryTimeX => "배송 시간: {0}";
        public override string Delivery_SenderMinimumCap => "송신 최소 한도";
        public override string Delivery_RecieverMaximumCap => "수신 최대 한도";
        public override string Delivery_ItemsReady => "보낼 아이템 준비 완료";
        public override string Delivery_RecieverReady => "수신 준비 완료";
        public override string Hud_ThisCity => "이 도시";
        public override string Hud_RecieveingCity => "수신 도시";

        public override string Info_ButtonIcon => "정보";

        public override string Info_ResourcePerSecond => "초당 자원 단위로 표시됩니다.";
        public override string Info_MinuteAverage => "최근 1분간의 평균값입니다.";

        public override string Message_OutOfFood_Title => "식량 부족";
        public override string Message_CityOutOfFood_Text => "식량이 부족하여 암시장에서 비싼 식량을 구매합니다. 금화가 바닥나면 일꾼들이 굶주립니다.";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "지형 종류";

        public override string Hud_EnergyUpkeepX => "식량 에너지 유지비 {0}";
        public override string Hud_EnergyAmount => "{0} 에너지 (작업 가능 시간 초 단위)";

        public override string Hud_CopySetup => "설정 복사";
        public override string Hud_Paste => "붙여넣기";

        public override string Hud_Available => "사용 가능";

        public override string WorkForce_ChildBirthRequirements => "출산 조건";
        public override string WorkForce_AvailableHomes => "거주지 여유: {0}";

        public override string WorkForce_Peace => "평화";
        public override string WorkForce_ChildToManTime => "성인까지 성장 시간: {0}분";

        public override string Economy_TaxIncome => "세금 수입: {0}";
        public override string Economy_ImportCostsForResource => "{0} 수입 비용: {1}";
        public override string Economy_BlackMarketCostsForResource => "{0} 암시장 비용: {1}";
        public override string Economy_GuardUpkeep => "경비 유지비: {0}";

        public override string Economy_LocalCityTrade_Export => "도시 간 수출: {0}";
        public override string Economy_LocalCityTrade_Import => "도시 간 수입: {0}";

        public override string Economy_ResourceProduction => "{0} 생산량: {1}";
        public override string Economy_ResourceSpending => "{0} 소비량: {1}";

        public override string Economy_TaxDescription => "세금은 일꾼 1명당 {0} 금화입니다.";
        public override string Economy_SoldResources => "판매된 자원 (금광석): {0}";

        public override string UnitType_Cities => "도시";
        public override string UnitType_Armies => "군대";
        public override string UnitType_Worker => "일꾼";

        public override string UnitType_FootKnight => "보병 기사";
        public override string UnitType_CavalryKnight => "기병 기사";

        public override string CityCulture_LargeFamilies => "대가족";
        public override string CityCulture_FertileGround => "비옥한 땅";
        public override string CityCulture_Archers => "숙련된 궁수";
        public override string CityCulture_Warriors => "전사";
        public override string CityCulture_AnimalBreeder => "가축 사육자";
        public override string CityCulture_Miners => "광부";
        public override string CityCulture_Woodcutters => "벌목꾼";
        public override string CityCulture_Builders => "건축가";

        public override string CityCulture_CrabMentality => "게 심보";
        public override string CityCulture_DeepWell => "깊은 우물";
        public override string CityCulture_Networker => "연결가";
        public override string CityCulture_PitMasters => "숯굴 장인";

        public override string CityCulture_Culture => "문화";
        public override string CityCulture_LargeFamilies_Description => "출산율이 증가합니다.";
        public override string CityCulture_FertileGround_Description => "작물 생산량이 증가합니다.";
        public override string CityCulture_Archers_Description => "숙련된 궁수를 양성합니다.";
        public override string CityCulture_Warriors_Description => "숙련된 근접 전사를 양성합니다.";
        //public override string CityCulture_AnimalBreeder_Description => "가축에서 더 많은 자원을 얻습니다.";
        public override string CityCulture_Miners_Description => "광석 채굴량이 증가합니다.";
        public override string CityCulture_Woodcutters_Description => "벌목 효율이 증가합니다.";
        public override string CityCulture_Builders_Description => "건설 속도가 빨라집니다.";
        public override string CityCulture_CrabMentality_Description => "작업 에너지 소모가 감소하지만, 고숙련 병사를 생산할 수 없습니다.";
        public override string CityCulture_DeepWell_Description => "물 자원이 더 빨리 회복됩니다.";
        public override string CityCulture_Networker_Description => "우편망이 더욱 효율적입니다.";
        public override string CityCulture_PitMasters_Description => "연료 생산량이 증가합니다.";

        public override string CityOption_AutoBuild_Work => "노동력 자동 확장";
        public override string CityOption_AutoBuild_Farm => "농장 자동 확장";

        public override string Hud_PurchaseTitle_Resources => "자원 구매";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "현재 보유";

        public override string Tutorial_EndTutorial => "튜토리얼 종료";
        public override string Tutorial_MissionX => "임무 {0}";
        public override string Tutorial_CollectXAmountOfY => "{0} {1} 수집";
        public override string Tutorial_SelectTabX => "탭 선택: {0}";
        public override string Tutorial_IncreasePriorityOnX => "{0}의 우선순위 높이기";
        public override string Tutorial_PlaceBuildOrder => "건설 명령 배치: {0}";
        public override string Tutorial_ZoomInput => "줌 조작";

        public override string Tutorial_SelectACity => "도시 선택";
        public override string Tutorial_ZoomInWorkers => "확대하여 일꾼을 확인하세요";
        public override string Tutorial_CreateSoldiers => "다음 장비로 병사 두 부대를 생성하세요: {0}. {1}.";
        public override string Tutorial_ZoomOutOverview => "축소하여 지도 개요 보기";
        public override string Tutorial_ZoomOutDiplomacy => "축소하여 외교 화면 보기";
        public override string Tutorial_ImproveRelations => "이웃 세력과의 관계를 개선하세요";
        public override string Tutorial_MissionComplete_Title => "임무 완료!";
        public override string Tutorial_MissionComplete_Unlocks => "새로운 조작이 해금되었습니다.";
        //patch1
        public override string Resource_ReachedStockpile => "비축 목표치에 도달했습니다";

        public override string BuildingType_ResourceMine => "{0} 광산";

        public override string Resource_TypeName_BogIron => "늪철";
        public override string Resource_TypeName_Coal => "석탄";

        public override string Language_XUpkeepIsY => "{0} 유지비: {1}";
        public override string Language_XCountIsY => "{0} 수량: {1}";

        public override string Message_ArmyOutOfFood_Text => "식량이 부족하여 암시장에서 비싼 식량을 구매합니다. 금화가 바닥나면 병사들이 탈영합니다.";

        public override string Info_ArmyFood1 => "군대는 가장 가까운 우호적인 도시에서 식량을 보충합니다.";
        public override string Info_ArmyFood2 => "식량은 다른 세력에게서 구매할 수 있습니다.";
        public override string Info_ArmyFood3 => "적대적인 지역에서는 암시장에서만 식량을 구매할 수 있습니다.";
        public override string FactionName_Monger => "몽거";
        public override string FactionName_Hatu => "하투";
        public override string FactionName_Destru => "데스트루";

        //patch2
        public override string Tutorial_BuildSomething => "{0}을(를) 생산하는 건물을 지으세요";
        public override string Tutorial_BuildCraft => "{0} 제작소를 지으세요";
        public override string Tutorial_IncreaseBufferLimit => "{0}의 비축 한도를 늘리세요";

        public override string Tutorial_CollectItemStockpile => "{0} {1}을(를) 비축하세요";
        public override string Tutorial_LookAtFoodBlueprint => "식량 설계도를 확인하세요";
        public override string Tutorial_CollectFood_Info1 => "일꾼들은 식사를 위해 시청으로 이동합니다.";
        public override string Tutorial_CollectFood_Info2 => "군대는 식량을 수집하기 위해 보급병을 보냅니다.";
        public override string Tutorial_CollectFood_Info0 => "일꾼을 완전히 제어하고 싶다면 모든 작업 우선순위를 0으로 설정하고, 하나씩만 활성화하세요.";

        public override string EndGameStatistics_DecorsBuilt => "건설된 장식물: {0}";
        public override string EndGameStatistics_StatuesBuilt => "건설된 조각상: {0}";



        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "기본적으로 일꾼들은 시청으로 이동해 식사하거나 물품을 전달합니다.";
        public override string GameMenu_UseSpeedX => "{0} 속도 옵션";
        public override string GameMenu_LongerBuildQueue => "건설 대기열 확장";

        public override string Diplomacy_RelationWithOthers => "그들의 다른 세력과의 관계";
        public override string Automation_queue_description => "대기열이 빌 때까지 반복 실행됩니다.";

        public override string BuildingType_Storehouse_Description => "일꾼들이 물품을 내려놓을 수 있습니다.";

        public override string Resource_TypeName_Longbow => "롱보우";
        public override string Resource_TypeName_Rapeseed => "유채";
        public override string Resource_TypeName_Hemp => "삼";

        public override string Resource_BogIronDescription => "늪철보다 광산에서 철을 채굴하는 것이 더 효율적입니다.";

        public override string Resource_FoodSafeGuard_Description => "식량 보호 기능. 식량 생산이 {0} 이하로 떨어지면 생산 체인의 우선순위를 최대로 설정합니다.";
        public override string Resource_FoodSafeGuard_Active => "식량 보호 기능이 활성화되었습니다.";

        public override string GameMenu_NextSong => "다음 곡";

        public override string BuildingType_Bank => "은행";
        public override string BuildingType_GoldDelivery_Description => "다른 도시로 금을 보냅니다.";

        public override string BuildingType_Logistics => "물류";
        public override string BuildingType_Logistics_Description => "건물 주문 능력을 향상시킵니다.";

        public override string BuildingType_Logistics_NationSizeRequirement => "국가 총 노동력: {0}";
        public override string Requirements_XItemStorageOfY => "도시 {0}의 {1} 저장량";

        public override string XP_UnlockBuildQueue => "건설 대기열 해금: {0}";
        public override string XP_UnlockBuilding => "건물 해금: ";
        public override string XP_Upgrade => "업그레이드";

        public override string XP_UpgradeBuildingX => "건물 업그레이드: {0}";

        public override string BuildHud_PerCycle => "주기당";
        public override string BuildHud_MayCraft => "제작 가능";
        public override string BuildHud_WorkTime => "작업 시간: {0}";
        public override string BuildHud_GrowTime => "성장 시간: {0}";
        public override string BuildHud_Produce => "생산:";

        public override string BuildHud_Queue => "허용된 건설 대기열: {0}/{1}";

        public override string LandType_Flatland => "평지";
        public override string LandType_Water => "물";
        public override string BuildingType_Wall => "성벽";
        public override string Delivery_AutoReciever_Description => "가장 적은 자원을 가진 도시에 자동으로 보냅니다.";

        public override string Hud_On => "켜짐";
        public override string Hud_Off => "꺼짐";

        public override string Hud_Time_Seconds => "{0}초";
        public override string Hud_Time_Minutes => "{0}분";
        public override string Hud_Undo => "되돌리기";
        public override string Hud_Redo => "다시 실행";

        public override string Tag_ViewOnMap => "지도에서 태그 보기";
        public override string MenuTab_Tag => "태그";

        public override string Input_Build => "건설";
        public override string FlagEditor_ClearAll => "모두 지우기";
        public override string CityCulture_Stonemason => "석공";
        public override string CityCulture_Stonemason_Description => "석재 채집 효율이 향상됩니다.";

        public override string CityCulture_Brewmaster => "양조 장인";
        public override string CityCulture_Brewmaster_Description => "맥주 생산량이 증가합니다.";

        public override string CityCulture_Weavers => "직조공";
        public override string CityCulture_Weavers_Description => "경갑옷 생산 효율이 향상됩니다.";

        public override string CityCulture_SiegeEngineer => "공성 기술자";
        public override string CityCulture_SiegeEngineer_Description => "공성 병기의 위력이 강화됩니다.";

        public override string CityCulture_Armorsmith => "갑옷 장인";
        public override string CityCulture_Armorsmith_Description => "철제 갑옷 생산 효율이 향상됩니다.";

        public override string CityCulture_Noblemen => "귀족";
        public override string CityCulture_Noblemen_Description => "기사가 더욱 강력해집니다.";

        public override string CityCulture_Seafaring => "항해 민족";
        public override string CityCulture_Seafaring_Description => "해상 전문 병사들이 더 강력한 함선을 운용합니다.";

        public override string CityCulture_Backtrader => "암시장 상인";
        public override string CityCulture_Backtrader_Description => "암시장 거래 비용이 감소합니다.";

        public override string CityCulture_LawAbiding => "법 순응자";
        public override string CityCulture_LawAbiding_Description => "세금 수입이 증가하지만, 암시장을 사용할 수 없습니다.";

        //##2##

        public override string Hud_Advanced => "고급";
        public override string Hud_Loading => "로딩 중...";

        public override string CityOption_LowerGuardSize => "경비 해제";
        public override string Hud_Purchase_MinCapacity => "최소 용량에 도달했습니다.";
        public override string Settings_ResetToDefault => "기본값으로 초기화";
        public override string Settings_NewGame => "새 게임";

        public override string Settings_AdvancedGameSettings => "고급 게임 설정";
        public override string Settings_FoodMultiplier => "식량 지속 배율";
        public override string Settings_FoodMultiplier_Description => "일꾼이나 병사가 포만 상태로 버티는 시간. 값이 높을수록 성능 저하가 발생할 수 있습니다.";

        public override string Settings_GameMode => "게임 모드";

        public override string Settings_Mode_Story => "전체 스토리";
        public override string Settings_Mode_IncludeBoss => "보스 이벤트 포함";
        public override string Settings_Mode_IncludeAttacks => "무작위 공격 포함";
        public override string Settings_Mode_Sandbox => "샌드박스";
        public override string Settings_Mode_Peaceful => "평화 모드";
        public override string Settings_Mode_Peaceful_Description => "모든 전쟁은 플레이어에 의해만 시작됩니다.";

        public override string Lobby_ImportSave => "세이브 불러오기";

        public override string Lobby_ExportSave => "세이브 내보내기";
        public override string Lobby_ExportSave_Description => "파일 사본을 생성하여 가져오기 폴더에 저장합니다: {0}";

        public override string Resource_CurrentAmount => "현재량: {0}";
        public override string Resource_MaxAmount_Soft => "소프트 캡 (최대 한계): {0}";
        public override string Resource_MaxAmount => "최대 한도: {0}";
        public override string Resource_AddPerSec => "초당 증가량: {0}";

        public override string Resource_WaterAddLimit => "물의 증가 속도는 변경할 수 없습니다.";

        public override string Tutorial_Select_SubTab => "그리고 분류를 선택하세요: {0}";




        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        public override string Tutorial_OpenGuardSubTab => "병영을 열고 다음 분류를 선택하세요: {0}";
        public override string Tutorial_GuardToWall => "경비를 성벽으로 이동시키세요";
        public override string Demo_MissionObjective_Title => "임무 목표";
        public override string Demo_MissionObjective_Description => "남쪽에서 오는 공격을 방어하세요";
        public override string Demo_Complete_Title => "데모 완료";
        public override string Demo_TimesUp_Title => "시간 종료!";
        public override string Demo_EndInOneMinuteDescription => "1분 후 데모가 종료됩니다.";

        public override string ArmyOption_NewArmy => "새 군대";
        public override string ProfileEditor_AltMain => "대체 메인";
        public override string Automation_CheckBoxTitle => "자동화";

        public override string ArmyStructure_ColumnWidth => "군대 열 너비";
        public override string ArmyStructure_ArmyPlacement => "군대 배치 위치";
        public override string ArmyStructure_Row_Front => "전열";
        public override string ArmyStructure_Row_Body => "중앙";
        public override string ArmyStructure_Row_Second => "후열";
        public override string ArmyStructure_Row_Behind => "배후";

        public override string Diplomacy_RelationType_Enemies => "적국";

        public override string EventMessage_EnemyAlliance_Title => "지배의 공포";
        public override string EventMessage_EnemyAlliance => "당신의 세력이 커지는 것을 두려워한 국가들이 동맹을 맺고 당신에게 맞섭니다.";

        public override string Settings_CentralGold => "중앙 금고";
        public override string Settings_CentralGold_Description => "켜짐: 모든 금화가 즉시 사용할 수 있는 공유 금고에 저장됩니다. 꺼짐: 금화는 실물로 존재하며 운송이 필요합니다.";

        public override string InputActionName_StopStart => "정지/시작";
        public override string InputActionName_ToggleHudDetail => "HUD 세부정보 전환";
        public override string InputActionName_NextCity => "다음 도시";
        public override string InputActionName_NextArmy => "다음 군대";
        public override string InputActionName_NextBattle => "다음 전투";
        public override string InputActionName_Build => "건설";
        public override string InputActionName_Copy => "복사";
        public override string InputActionName_Paste => "붙여넣기";
        public override string InputActionName_Menu => "메뉴";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "이전 색상";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "다음 색상";
        public override string InputActionName_FlagDesign_PaintBucket => "페인트 통";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "색상 선택기";
        public override string InputActionName_ControllerFocus => "포커스";
        public override string InputActionName_ControllerCancel => "취소";
        public override string InputActionName_ControllerMessageClick => "메시지 클릭";
        public override string InputActionName_ControllerSelect => "선택";
        public override string InputActionName_WASD_UP => "위로";
        public override string InputActionName_WASD_DOWN => "아래로";
        public override string InputActionName_WASD_LEFT => "왼쪽으로";
        public override string InputActionName_WASD_RIGHT => "오른쪽으로";
        public override string InputActionName_CameraTiltLeft => "카메라 왼쪽 기울이기";
        public override string InputActionName_CameraTiltRight => "카메라 오른쪽 기울이기";
        public override string InputActionName_CameraTiltUp => "카메라 위로 기울이기";
        public override string InputActionName_ZoomInKey => "확대";
        public override string InputActionName_ZoomOutKey => "축소";



        public override string Settings_Title_Monitor => "모니터 옵션";
        public override string Settings_Title_Graphics => "그래픽 옵션";
        public override string Settings_Title_Input => "입력 설정";
        public override string Settings_Title_Gameplay => "게임플레이 옵션";
        public override string Settings_PanOnZoom => "확대 시 화면 이동";
        public override string Settings_ScrollSensitivity_Game => "스크롤 감도: 게임";
        public override string Settings_ScrollSensitivity_Menu => "스크롤 감도: 메뉴";
        public override string Settings_Blood => "피 효과";

        public override string Settings_MasterVolume => "마스터 볼륨";
        public override string Settings_AmbienceVolume => "환경음 볼륨";
        public override string Settings_BattleMelody => "전투 음악";

        public override string Settings_ModelLight => "모델 조명 효과";
        public override string Settings_Particles => "입자 효과";
        public override string Settings_MapLoadSpeed => "지도 로딩 속도";

        public override string Lobby_Category_Options => "옵션";
        public override string Lobby_Category_Editor => "에디터";
        public override string Lobby_Category_ExtraModes => "추가 모드";

        public override string Lobby_Editor_MapEditor => "지도 에디터";
        public override string Lobby_Editor_VoxelEditor => "복셀 에디터";

        public override string Lobby_Mode_BattleLab => "전투 실험실";
        public override string Lobby_Mode_BattleLab_Description => "모든 병사들을 서로 맞붙게 해보세요.";
        public override string Lobby_Mode_Commander => "지휘관 모드";
        public override string Lobby_Mode_Commander_Description => "작은 전술 보드 게임 모드입니다.";
        public override string Lobby_MusicPlayList => "음악 재생 목록";

        public override string Lobby_GameSetup => "게임 설정";
        public override string Lobby_PlayerSetup => "플레이어 설정";
        public override string LobbyDemoMode_Demo => "데모";

        public override string Lobby_Tutorial => "튜토리얼";

        public override string LobbyDemoMode_ShortTutorial => "간단한 튜토리얼";
        public override string LobbyDemoMode_LongTutorial => "확장 튜토리얼";

        public override string LobbyDemoMode_WishlistOn => "위시리스트 등록";

        public override string BattleLab_StartHere => "여기서 전투 시작";
        public override string BattleLab_Start => "전투 시작";
        public override string BattleLab_Attacker => "공격자";

        public override string MapGenerator_Name => "지도 에디터 - 생성";

        public override string MapType_CustomMap => "사용자 정의 지도";
        public override string MapType_GenerateNewMap => "새 지도 생성";
        public override string MapGenerator_GenerateAction => "생성";
        public override string MapGenerator_Terrain_CustomSize => "사용자 지정 크기";
        public override string MapGenerator_Terrain_StartAs => "시작 형태";
        public override string MapGenerator_Terrain_ClearPass => "정리 단계 실행";
        public override string MapGenerator_Terrain_BuildPass => "지형 구축 단계 실행";
        public override string MapGenerator_Terrain_DigPass => "굴착 단계 실행";
        public override string MapGenerator_Terrain_BuildDigLoops => "구축-굴착 반복 횟수";
        public override string MapGenerator_Terrain_BuildStrokes => "구축 획 수";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "100타일당 페인트 획 수 기준";
        public override string MapGenerator_Terrain_DigStrokes => "굴착 획 수";
        public override string MapGenerator_Terrain_CleanUp_Option => "단일 타일 정리";
        public override string MapGenerator_Terrain_CleanUpPass => "정리 단계 실행";

        public override string Economy_ServicemenUpkeep => "근위병 유지비: {0}";
        public override string Economy_ServicemenUpkeep_Description => "근위병 1명당 유지비는 {0}골드입니다.";
        public override string Economy_GuardUpkeep_Description => "경비병 1명당 유지비는 {0}골드입니다.";

        public override string EndScreen_TimeHasEndedTitle => "시간 종료";

        public override string Hud_AdvancedSettings => "고급 설정";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "취소";
        public override string Hud_Delete => "삭제";
        public override string Hud_Next => "다음";
        public override string Hud_Apply => "적용";
        public override string Hud_AllCities => "모든 도시";
        public override string Hud_Time_Hours => "{0}시간";
        public override string Hud_AddX => "{0} 추가";
        public override string Hud_Both => "양쪽";
        public override string Hud_Direction => "방향";

        /// <summary>
        /// 0: object collection type name, 1: number of objects
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}, 개수: {1}";

        public override string Hud_EffectDoesNotStack => "이 효과는 중첩되지 않습니다";

        public override string Work_SmeltX => "{0} 제련";

        public override string Info_TotalFoodProduction => "총 식량 생산량";
        public override string Info_TotalFoodSpending => "총 식량 소비량";

        public override string Info_FooodAndDeliveryLocation => "기본적으로 일꾼들은 시청으로 가서 식사하거나 자원을 내려놓습니다.";

        public override string Delivery_SendChunk => "배송당 물품 수량";
        public override string Delivery_SpeedBonus => "속도 보너스: {0}%";

        public override string Delivery_AutoResourceDescription => "저장 한계에 도달한 자원을 부족한 도시에 자동으로 배송합니다.";

        public override string Conscript_Soldiers_ArmyType => "군대 병력";
        public override string Conscript_Soldiers_ArmyType_Description => "인접한 군대에 병사를 모집합니다.";
        public override string Conscript_Soldiers_GuardType => "도시 경비";
        public override string Conscript_Soldiers_GuardType_Description => "경비병은 성벽을 방어하는 데 사용됩니다.";

        public override string Defence_Title => "방어";
        public override string Defence_GuardPost => "경비 초소";

        public override string Defence_WallDescription_Movement => "적의 이동을 방해합니다.";
        public override string Defence_WallDescription_GuardPost => "경비병을 배치할 수 있습니다.";
        public override string Defence_AutoAssign => "자동 배치";
        public override string Defence_AutoAssign_Description => "새 경비병이 자동으로 이 초소로 이동합니다.";

        public override string Conscript_SplashDamage => "광역 피해";
        public override string Conscript_HighSplashDamage => "강력한 광역 피해";

        public override string Conscript_Training_Champion => "챔피언";
        public override string Conscript_Training_Legendary => "전설";

        public override string Experience_Title => "경험치";
        public override string Experience_TopExperience => "최고 숙련도 등급";

        public override string Experience_TimeReductionDescription => "레벨당 작업 시간이 {0}% 단축됩니다.";

        public override string ExperienceType_Farm => "농부";
        public override string ExperienceType_AnimalCare => "가축 관리";
        public override string ExperienceType_HouseBuilding => "건축가";
        public override string ExperienceType_WoodWork => "목공";
        public override string ExperienceType_StoneCutter => "석공";
        public override string ExperienceType_Mining => "광부";
        public override string ExperienceType_Transport => "운송";
        public override string ExperienceType_Cook => "요리사";
        public override string ExperienceType_Fletcher => "화살 제작자";
        public override string ExperienceType_RefineOre => "제련공";
        public override string ExperienceType_Casting => "주조공";
        public override string ExperienceType_CraftMetal => "대장장이";
        public override string ExperienceType_CraftArmor => "갑옷 제작자";
        public override string ExperienceType_CraftWeapon => "무기 제작자";
        public override string ExperienceType_CraftFuel => "목탄 제작자";
        public override string ExperienceType_Chemist => "연금술사";

        public override string ExperienceLevel_1 => "초보자";
        public override string ExperienceLevel_2 => "숙련자";
        public override string ExperienceLevel_3 => "전문가";
        public override string ExperienceLevel_4 => "장인";
        public override string ExperienceLevel_5 => "전설";

        public override string ExperenceOrDistancePrio_Title => "작업자 선택 기준";
        public override string ExperenceOrDistancePrio_Description => "대기 중인 일꾼은 거리 또는 경험치를 기준으로 선택됩니다.";

        public override string Technology_Description => "각 도시는 고유한 기술 트리를 가지고 있으며, 기술은 새로운 건물과 아이템을 해금합니다.";
        public override string Experience_Description => "일꾼은 경험치를 얻으며 능력이 향상됩니다.";


        public override string Technology_Title => "기술";
        public override string Technology_ShareField => "기술 분야 공유";

        public override string Technology_GainByNeigborRelation => "이웃 도시가 해당 기술을 보유하고 있고, 관계가 {0}일 때: {1}";
        public override string Technology_ForEachMaster => "{0}의 숙련도가 {1}에 도달하면, 기술 분야 {2}에서 효과를 얻습니다.";
        public override string Technology_CitySpread => "인접한 도시끼리 기술을 공유합니다: {0}";
        public override string Technology_CityCapture => "도시가 전투에서 점령되면 대부분의 기술이 파괴됩니다.";

        public override string Technology_AdvancedBuildings => "고급 건축";
        public override string Technology_AdvancedFarming => "고급 농업";
        public override string Technology_AdvancedCasting => "고급 주조";

        public override string Help_Title => "도움말";
        public override string Help_Work_Title => "작업이 시작되지 않음";
        public override string Help_Work_Resources => "건물은 필요한 자원이 있어야 작동합니다.";
        public override string Help_Work_Skill => "일꾼은 해당 작업에 맞는 숙련도가 필요합니다.";
        public override string Help_Work_Stockpile => "저장고가 가득 차면 자원 수집이 중단됩니다.";
        public override string Help_Work_Priority => "작업 우선순위가 낮거나 0일 수 있습니다.";

        public override string Help_Soldiers_Title => "병사 생산";
        public override string Help_Soldiers_PlaceBuildingX => "건물 배치: {0}";
        public override string Help_Soldiers_Workers => "모집 가능한 일꾼이 필요합니다.";
        public override string Help_Soldiers_Weapon => "병사마다 무기가 필요합니다.";
        public override string Help_Soldiers_StartX => "시작: {0}";

        public override string Hud_SelectHistory => "기록 선택";

        public override string Hud_PointsPerMinute => "분당 {0} 포인트";
        public override string Hud_PercentValueCost => "서비스 비용: 가치의 {0}%";

        public override string Hud_Mixed => "혼합";
        public override string Hud_Distance => "거리";

        public override string Hud_Unlock => "해금";
        public override string Hud_category => "분류";

        public override string Input_StepOneFrame => "한 프레임씩 진행";

        public override string Resource_TypeName_Wagon2Wheel => "소형 마차";
        public override string Resource_TypeName_Wagon4Wheel => "대형 마차";
        public override string Resource_TypeName_Tin => "주석";
        public override string Resource_TypeName_TinOre => "주석 광석";

        public override string Resource_TypeName_Copper => "구리";
        public override string Resource_TypeName_CopperOre => "구리 광석";
        public override string Resource_TypeName_SilverOre => "은 광석";
        public override string Resource_TypeName_Silver => "은";

        /// <summary>
        /// Mithril is a fantasy metal
        /// </summary>
        public override string Resource_TypeName_RawMithril => "미스릴 원광석";
        public override string Resource_TypeName_Mithril => "미스릴";

        public override string Resource_TypeName_BronzeSword => "청동검";
        public override string Resource_TypeName_ShortSword => "단검";
        public override string Resource_TypeName_LongSword => "장검";
        public override string Resource_TypeName_HandSpear => "손창";
        public override string Resource_TypeName_Warhammer => "워해머";
        public override string Resource_TypeName_MithrilSword => "미스릴 검";
        public override string Resource_TypeName_SlingShot => "슬링샷";
        public override string Resource_TypeName_ThrowingSpear => "재블린";
        public override string Resource_TypeName_Crossbow => "석궁";
        public override string Resource_TypeName_MithrilBow => "미스릴 활";

        public override string Resource_TypeName_CoolingFluid => "냉각액";
        public override string Resource_TypeName_Palisade => "목책";
        public override string Resource_TypeName_Toolkit => "공구 세트";

        public override string Resource_TypeName_Sulfur => "황";
        public override string Resource_TypeName_LeadOre => "납 광석";
        public override string Resource_TypeName_Lead => "납";
        public override string Resource_TypeName_Bronze => "청동";
        public override string Resource_TypeName_BloomIron => "괴철";
        public override string Resource_TypeName_Steel => "강철";
        public override string Resource_TypeName_CastIron => "주철";

        public override string Resource_TypeName_BlackPowder => "흑색 화약";
        public override string Resource_TypeName_GunPowder => "화약";
        public override string Resource_TypeName_LedBullet => "탄환";

        public override string Resource_TypeName_HandCannon => "휴대 대포";
        public override string Resource_TypeName_HandCulverin => "핸드 컬버린";
        public override string Resource_TypeName_Rifle => "소총";
        public override string Resource_TypeName_Blunderbuss => "블런더버스";

        public override string Resource_TypeName_Manuballista => "매뉴발리스타";
        public override string Resource_TypeName_Catapult => "투석기";
        public override string Resource_TypeName_BatteringRam => "공성추";
        public override string Resource_TypeName_SiegeCannonBronze => "바실리스크";
        public override string Resource_TypeName_ManCannonBronze => "봄바르드";
        public override string Resource_TypeName_SiegeCannonIron => "호비츠";
        public override string Resource_TypeName_ManCannonIron => "대포";

        public override string Resource_TypeName_PaddedArmor => "솜갑옷";
        public override string Resource_TypeName_HeavyPaddedArmor => "중형 솜갑옷";

        public override string Resource_TypeName_IronArmor => "사슬갑옷";
        public override string Resource_TypeName_HeavyIronArmor => "중형 사슬갑옷";

        public override string Resource_TypeName_BronzeArmor => "청동 갑옷";

        public override string Resource_TypeName_LightPlateArmor => "판금 갑옷";
        public override string Resource_TypeName_FullPlateArmor => "전신 판금 갑옷";
        public override string Resource_TypeName_MithrilArmor => "미스릴 갑옷";
        public override string Resource_TypeName_Coin => "동전";
        public override string UnitType_Warhammer => "해머 기사";
        public override string UnitType_SpearAndShield => "전열병";

        public override string UnitType_CollectionOfSoldiers => "병사 묶음";
        public override string UnitType_CollectionOfArmies => "군대 묶음";

        /// <summary>
        /// The id tag will be a unique number
        /// </summary>
        public override string UnitId => "(ID {0})";

        public override string BuildHud_AreaEffectTitle => "영역 효과";
        public override string BuildHud_BonusRadius => "보너스 범위: {0}";

        public override string BuildHud_BuildTime => "건설 시간";
        public override string SchoolHud_ToLevel => "다음 레벨까지";
        public override string SchoolHud_TimeDescription => "시간은 경험치가 0일 때 기준이며, 경험치가 쌓이면 감소합니다.";
        public override string SchoolHud_SelectSchool => "학교 선택";
        public override string Upgrade_Order => "업그레이드 명령";

        public override string Building_ListDescription => "이 범주에 속한 모든 건물 목록";

        public override string BuildingType_IsUpgraded => "{0} - 업그레이드됨";
        public override string BuildingType_WoodCutter => "제재소";
        public override string BuildingType_Workshop_Description => "주변의 작업 효율을 향상시킵니다.";

        public override string BuildingType_WoodCutter_AreaAffect => "나무 채집량이 {0}% 증가합니다.";
        public override string BuildingType_StoneCutter_AreaAffect => "석재 채집량이 {0}% 증가합니다.";

        public override string BuildingType_StoneCutter => "석재 채석장";

        public override string BuildingType_Embassy => "대사관";
        public override string BuildingType_Embassy_Description => "외교 관계를 위한 건물";

        public override string BuildingType_SoldierBarracks => "보병 병영";
        public override string BuildingType_ArcherBarracks => "궁병 병영";
        public override string BuildingType_WarmachineBarracks => "공성 병기 병영";
        public override string BuildingType_GunBarracks => "총기 병영";
        public override string BuildingType_CannonBarracks => "대포 병영";
        public override string BuildingType_KnightsBarracks => "기사 병영";

        public override string BuildingType_WaterResovoir => "저수조";
        public override string BuildingType_WaterResovoir_Description => "물 저장량을 증가시킵니다.";

        public override string BuildingType_SmeltingFurnace => "제련로";
        public override string BuildingType_SmeltingFurnace_Description => "광석을 정제하여 금속을 생산합니다.";

        public override string BuildingType_Foundry => "주조소";
        public override string BuildingType_Foundry_Description => "금속 주조 작업장";

        public override string BuildingType_Armory => "병기고";
        public override string BuildingType_Armory_Description => "갑옷 제작소";
        public override string BuildingType_Chemist => "연금술사 작업실";
        public override string BuildingType_Chemist_Description => "화학 재료를 제작하는 작업장";
        public override string BuildingType_CoinMaker => "주화 주조소";
        public override string BuildingType_CoinMaker_Description => "금속을 돈으로 주조합니다.";
        public override string BuildingType_Gunmaker => "총기 제작소";
        public override string BuildingType_Gunmaker_Description => "총기 및 대포 제작소";

        public override string BuildingType_School_Tab => "학교";
        public override string BuildingType_School => "장인 조합";
        public override string BuildingType_School_Description => "일꾼의 숙련도를 향상시킵니다.";

        public override string BuildingType_GoldDelivery => "금 운송소";
        public override string BuildingType_Bank_Description => "자금 관리소";

        public override string DecorType_CobbleStones => "자갈길";
        public override string DecorType_Square => "도시 광장";

        public override string DecorType_Garden => "정원";
        public override string DecorType_Flag => "깃발";
        public override string DecorType_Banner => "현수막";

        public override string BuildingType_DirtRoad => "흙길";
        public override string BuildingType_Palisade => "목책 요새";

        public override string ResourceType_ServiceMen => "근무 인원";
        public override string BuildingType_ServiceHouse => "근무자 숙소";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "근무 인원 {0}명 추가";

        public override string BuildingType_GuardOffice => "경비 사무소";
        public override string BuildingType_GuardOffice_DescriptionAddX => "경비 한도 {0} 증가";

        public override string BuildingType_DirtWall => "흙담";
        public override string BuildingType_DirtTower => "흙탑";
        public override string BuildingType_WoodWall => "목재 성벽";
        public override string BuildingType_WoodTower => "목재 탑";
        public override string BuildingType_StoneWall => "석벽";
        public override string BuildingType_StoneTower => "석탑";
        public override string BuildingType_StoneGate => "석문";
        public override string BuildingType_StoneHouse => "석조 주택";

        /// <summary>
        /// When listing slight variations, like "Lamp A" and "Lamp B"
        /// </summary>
        public override string VariantType_A => "{0} A형";
        public override string VariantType_B => "{0} B형";
        public override string VariantType_C => "{0} C형";
        public override string VariantType_D => "{0} D형";
        public override string VariantType_E => "{0} E형";
        public override string VariantType_F => "{0} F형";
        public override string VariantType_G => "{0} G형";
        public override string VariantType_H => "{0} H형";

        public override string BuildingToolShape_Free => "펜";
        public override string BuildingToolShape_Area => "사각형";
        public override string BuildingToolShape_Line => "직선";
        public override string BuildingToolShape_LShape => "ㄱ자형";

        public override string CityHall_Upgrade => "시청 업그레이드";

        /// <summary>
        /// A cap on how many workers the city can have
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "지원 가능한 최대 노동자 수: {0}";

        public override string CityHall_Size_Small => "마을";
        public override string CityHall_Size_Medium => "도시";
        public override string CityHall_Size_Large => "수도";

        public override string GuardHousingCount => "경비 숙소 수";
        public override string ServicemenCount => "근무 인원: {0}";

        public override string Work_MiningResource => "{0} 채굴 중";

        public override string MenuTab_Progress => "진행도";

        public override string Automation_AutomateCity => "도시 자동화";
        public override string Automation_AutomationFocus => "자동화 중점";
        public override string Automation_AutomationFocus_Grow => "성장";
        public override string Automation_AutomationFocus_Export => "수출";
        public override string Automation_AutomationFocus_War => "전쟁";

        public override string CityCulture_Smelters_Description => "광석 제련 효율 향상";
        public override string CityCulture_Smelters => "제련공";

        public override string CityCulture_Apprentices_Description => "신규 노동자가 숙련된 일꾼에게서 경험을 얻습니다.";
        public override string CityCulture_Apprentices => "견습생";

        public override string CityCulture_BronzeCasters_Description => "청동 및 청동 제품 생산 향상";
        public override string CityCulture_BronzeCasters => "청동 주조공";


        //DEMO PATCH 1
        /// <summary>
        /// Evil orcs that roam on the map
        /// </summary>
        public override string FactionName_Barbarian => "암흑 오크 부족";
        public override string Tutorial_AttackAndDestroyX => "공격하고 파괴하십시오: {0}";
        public override string Resource_TypeName_Pike => "파이크";

        public override string BattleTrials_Title => "전투 시험장";
        public override string BattleTrials_Description => "군대 대 군대의 직접적인 전투에서 전술을 시험해 보세요.";

        // DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "이 공격은 방어 확률을 감소시킵니다.";
        public override string Conscript_BlockPerSecond => "초당 최대 {0}회 방어 가능";
        public override string Conscript_BlockDescription => "병사들은 전방 호선에서 오는 대부분의 공격을 방어합니다.";

        public override string Map_CustomSeed => "맵 시드";

        public override string Settings_Mode_Spectator => "관전자";
        //public override string Settings_Mode_Spectator_Description => "관전 전용 모드";

        public override string Automation_AutomationFocus_NoFocus_Description => "모든 것을 조금씩 건설합니다.";
        public override string Automation_AutomationFocus_WillProduce => "주로 생산할 항목:";

        public override string Help_Food_WhoEats => "모든 병사와 노동자는 음식을 소비합니다.";
        public override string Help_Food_BigArmy => "거대한 군대는 주변 도시의 식량을 고갈시킬 수 있습니다.";
        public override string Help_Food_DontBuild => "농장을 늘린다고 자동으로 식량이 증가하지 않습니다. 일꾼과 조리장이 있어야 생산됩니다.";
        public override string Help_Food_UseWater => "식량 생산에는 물이 필요합니다.";
        public override string Help_Food_Postal => "도시들이 서로 식량을 지원하도록 우편 시스템을 유지하세요.";

        public override string Message_LostCity => "도시를 잃었습니다.";

        public override string Demo_Description => "짧은 시나리오: {0}분 동안 도시를 방어하십시오.";

        // DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "{0}분 후 데모가 종료됩니다.";

        public override string Experience_Required => "필요한 경험치";

        public override string InputActionName_ToggleMenu => "메뉴 전환";

        // DEMO PATCH 4
        public override string Work_BadValueDescription => "자원은 0 이하로 내려가거나 약간 저장 한도를 초과할 수 있습니다. 이 범위는 작업 대기열이 생성될 때만 적용됩니다.";

        public override string Work_SelectCategory => "아이템 카테고리 선택";
        public override string Hud_RemoveFromList => "목록에서 제거";

        public override string Hud_ReturnToPrevious => "뒤로 가기";
        public override string Hud_Close => "닫기";

        public override string Hud_Low => "낮음";
        public override string Hud_Medium => "보통";
        public override string Hud_High => "높음";

        public override string Hud_Copy => "복사";
        public override string Hud_Cut => "잘라내기";
        public override string Hud_SaveCompleted => "저장 완료";

        public override string Settings_WaterMultiplier => "물 배율";
        public override string Settings_WaterMultiplier_Description => "도시가 생산하고 저장하는 물의 양을 결정합니다. 값이 높을수록 컴퓨터 성능이 저하될 수 있습니다.";

        public override string Settings_ChildMultiplier => "출산 배율";

        public override string Settings_CraftMultiplier_Description => "값이 낮을수록 생산 속도가 빨라집니다.";

        public override string FastProduction => "빠른 생산";
        public override string SlowProduction => "느린 생산";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "생산하지 않음";

        //public override string CityAutomation_WaitForMaxPopulation => "최대 인구까지 대기";
        public override string Automation_AutomationFocus_NoFocus => "전체";
        public override string CityAutomation_SoldierQuality => "병사 품질";
        public override string CityAutomation_SoldierWeaponType => "무기 종류";

        public override string WarsResourceGroup_Resources => "자원";
        public override string WarsResourceGroup_Weapons => "무기";

        public override string WarsResourceGroup_AllWeaponTypes => "혼합형";
        public override string WarsResourceGroup_MeleeHandWeapons => "근접 무기";
        public override string WarsResourceGroup_RangedHandWeapons => "원거리 무기";
        public override string WarsResourceGroup_Warmachines => "공성 병기";

        public override string FactionSettings_Titel => "세력 전체 설정";
        public override string FactionSettings_Description => "모든 도시에 적용됩니다.";

        public override string Conscript_MaxPopulation => "최대 인구 시 모집";
        public override string Conscript_MaxPopulation_Description => "도시 인구가 최대일 때만 모집합니다.";

        public override string Conscript_FoodAbundance => "최대 식량 비축 시 모집";
        public override string Conscript_FoodAbundance_Description => "식량이 최대 비축량에 도달했을 때만 모집합니다.";

        public override string GeneralSetting_On => "설정: 켜기";
        public override string GeneralSetting_Off => "설정: 끄기";
        public override string GeneralSetting_AllBuildingsDescription => "모든 건물에 적용됩니다.";

        public override string GeneralSetting_ApplyMessage => "{0}개의 건물에 변경 사항이 적용되었습니다.";

        public override string MustTurnOffSteamInput => "컨트롤러를 사용하려면 Steam 입력을 꺼야 합니다.";

        public override string Technology_GainTitle => "기술 획득 방법";
        public override string Technology_LevelUp => "레벨 업";
        public override string Technology_ForEachLevelUp => "노동자가 기술 분야에서 레벨업할 때: {0}";

        public override string VoxelEditor_Description => "블록 형태의 모델을 만듭니다.";
        public override string Editor_Tool => "도구";
        public override string Editor_SelectOptionsMenu => "선택 옵션";
        public override string Editor_Continous => "연속";
        public override string Editor_Tool_PencilSize => "연필 크기";
        public override string Editor_Tool_SizeTolerance => "크기 허용치";
        public override string Editor_Tool_RoundPencil => "둥근 연필";
        public override string Editor_Tool_EdgeSize => "모서리 크기";
        public override string Editor_Tool_PercentFill => "채우기 비율";
        public override string Editor_Tool_ClearAbove => "위쪽 지우기";
        public override string Editor_Tool_FillBelow => "아래쪽 채우기";

        public override string Editor_UserModels => "사용자 모델";
        public override string Editor_UserModels_Description => "저장한 모델을 불러옵니다.";

        public override string Editor_RetailModels => "기본 모델";
        public override string Editor_RetailModels_Description => "게임에 포함된 모델을 불러옵니다.";

        public override string Editor_ModTemplates => "모드 템플릿";
        public override string Editor_ExportAsOBJ => ".OBJ로 내보내기";
        public override string Editor_SelectAll => "전체 선택";

        public override string Editor_Canvas_Title => "캔버스";
        public override string Editor_Canvas_Size => "크기";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "크기 프리셋";
        public override string Editor_Canvas_Move => "이동";
        public override string Editor_Canvas_Move_Up => "위로 이동";
        public override string Editor_Canvas_Move_Down => "아래로 이동";
        public override string Editor_Canvas_RotateClockwise => "시계 방향 회전";
        public override string Editor_Canvas_RotateCounterClockwise => "반시계 방향 회전";
        public override string Editor_Canvas_Mirror => "좌우 반전";

        public override string Editor_Canvas_RotateFlip_Title => "회전/뒤집기";
        public override string Editor_Canvas_FlipVertical => "상하 뒤집기";
        public override string Editor_Canvas_FlipOrientation => "가로/세로 전환";
        public override string Editor_Canvas_ClearAll_Description => "모든 블록과 프레임을 제거합니다.";

        public override string Editor_Animation => "애니메이션";
        public override string Editor_Animation_RemoveCurrentFrame => "현재 프레임 삭제";
        public override string Editor_Animation_AddFrameCopy => "현재 프레임 복사 추가";
        public override string Editor_Animation_AddEmptyFrame => "빈 프레임 추가";
        public override string Editor_Animation_MoveDescription => "프레임 위치 변경";
        public override string Editor_Animation_AllFrames => "전체 프레임";
        public override string Editor_Animation_AllFrames_ActionDescription => "모든 프레임에 동일한 동작 수행";

        public override string Editor_SettingsMenu => "설정";
        public override string Hud_Exit => "나가기";
        public override string Editor_Canvas_Clear => "지우기";

        public override string Editor_Stamp => "스탬프";
        public override string Editor_StampOtherFrames => "다른 프레임에 스탬프";
        public override string Editor_StampOtherFrames_Description => "이 프레임의 복셀을 선택된 프레임에 붙여넣습니다.";
        public override string Editor_PasteToFrame => "이 프레임에 복셀 붙여넣기";
        public override string Editor_ClearAllFrames => "모든 프레임 지우기";
        public override string Editor_ClearOtherFrames => "다른 프레임 지우기";

        public override string Editor_Settings_MoveSpeed => "이동 속도";
        public override string Editor_Settings_BackgroundColor => "배경색";
        public override string Editor_Settings_HideHUD => "HUD 숨기기";

        public override string Editor_Color => "색상";
        public override string Editor_ColorsInUseLabel => "사용 중인 색상";
        public override string Editor_Color_BrighterPlus => "더 밝게 ++";
        public override string Editor_Color_Brighter => "밝게";
        public override string Editor_Color_Darker => "어둡게";
        public override string Editor_Color_DarkerPlus => "더 어둡게 ++";
        public override string Editor_Color_RedTint => "붉은색 톤";
        public override string Editor_Color_Tint => "색조";
        public override string Editor_Color_GreenTint => "녹색 톤";
        public override string Editor_Color_BlueTint => "푸른색 톤";
        public override string Editor_Color_YellowTint => "노란색 톤";
        public override string Editor_Color_PurpleTint => "보라색 톤";
        public override string Editor_NoColor => "비어 있음";

        public override string Editor_Material => "재질";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "색상 변경";
        public override string Editor_Color_RecolorTo => "다음 색으로 변경";

        public override string Editor_Material_Set => "재질 설정";

        public override string Editor_Preview => "미리보기";
        public override string Editor_CombineWithCurrent => "현재 모델과 결합";

        public override string Editor_PickedColor => "선택된 색상";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";
        public override string BuildingType_ImmigrationTent => "이주자 천막";
        public override string BuildingType_ImmigrationTent_Description => "이주자 {0}명을 수용합니다.";
        public override string BuildingType_ReseachCenter => "연구 센터";
        public override string BuildingType_Bookpress => "책 인쇄소";
        public override string BuildingType_Bookpress_Description => "하나의 연구 분야에서 얻은 모든 연구 포인트가 다른 도시에 있는 모든 {0}와 공유됩니다.";

        /// <summary>
        /// 0: beer, 1: chemistry, 2: gun powder
        /// </summary>
        public override string Technology_ReseachExample => "예시: 일꾼이 {0}을(를) 생산하면 {1} 기술 숙련도가 상승합니다. 레벨업 시, {1} 분야를 공유하는 {2} 기술에도 연구 포인트가 추가됩니다.";

        public override string BuildingType_Research_BaseDescription => "기술 연구를 향상시킵니다.";
        public override string BuildingType_ResearchCenter_Description => "같은 분야에서 일꾼이 레벨업할 때마다 추가로 {0}의 연구 포인트를 제공합니다.";

        // DEMO PATCH 5
        public override string Editor_CropSelection => "선택 영역 자르기";

        public override string Immigrants_DisbandedSoldiers => "해산된 병사들이 이주자로 전환됩니다.";
        public override string Immigrants_RefillWorkers => "노동력을 빠르게 충원합니다.";
        public override string Immigrants_UnhousedAreLost => "주거지가 없는 이주자는 일정 시간이 지나면 사라집니다.";
        public override string Editor_VoxelCount => "{0} 복셀";

        public override string Editor_Layers_Titel => "레이어";
        public override string Editor_Layers_All => "모든 레이어";
        public override string Editor_LayerNumber => "레이어 {0}";

        public override string Editor_Layer_AddEmpty => "빈 레이어 추가";
        public override string Editor_Layer_AddCopy => "레이어 복제";
        public override string Editor_Layer_Remove => "레이어 삭제";
        public override string Editor_Layer_MergeDown => "아래로 병합";
        public override string Editor_IsAnimated => "애니메이션 있음";
        public override string Editor_ToggleVisible => "가시성 전환";
        public override string Editor_ToggleAnimatedLayer => "애니메이션 레이어 전환";
        public override string Editor_Projects => "프로젝트 파일";

        public override string ProfileEditor_ReplaceMaterial => "프로필 색상: {0}";
        public override string ProfileEditor_ProfileColors_Label => "프로필 색상";
        public override string ProfileEditor_TunicColor => "상의 색상";
        public override string ProfileEditor_PantsColor => "하의 색상";
        public override string ProfileEditor_LeaderColor => "지도자 색상";

        public override string MapStartAs_Water => "물";
        public override string MapStartAs_Land => "육지";
        public override string MapStartAs_Circle => "원형";

        public override string Hud_NeedToBeAssigned => "할당이 필요합니다.";
        public override string Hud_CommitAssignment => "할당";
        public override string Technology_NoAvailableResearch => "가능한 연구가 없습니다.";

        public override string Research_Tab => "연구";

        //5.2
        public override string BuildCategory_General => "일반";
        public override string BuildCategory_Military => "군사";
        public override string BuildCategory_Decoration => "장식";
        public override string BuildCategory_Upgrade => "업그레이드";
        public override string Work_NoMines => "광산이 없습니다.";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "표시 이름";
        public override string HUD_Filter => "필터";
        public override string HUD_Scale => "크기 조정";
        public override string HUD_Tags => "태그";
        public override string HUD_ClickToCancel => "클릭하여 취소";

        public override string ObjectTag_Description => "지도에 기호를 추가합니다.";
        public override string HudPins => "HUD 고정";
        public override string HudPins_Description => "정보를 화면에 고정합니다.";

        public override string Lobby_PlayerProfileNumbered => "프로필 {0}";
        public override string Lobby_CharacterCreationNumbered => "캐릭터 {0}";
        public override string Lobby_PlayerProfileEdit => "플레이어 프로필 편집";

        public override string Editor_ConvertAnimationToLayers => "애니메이션을 레이어로 변환";
        public override string Editor_StampAllFrames => "모든 프레임에 스탬프";

        public override string Editor_DisplayOptions => "표시 옵션";
        public override string Editor_CharacterCreator => "캐릭터 제작기";
        public override string Editor_CharacterCreator_Description => "군사 모델 외형 편집기";
        public override string Editor_HatGenre => "모자 표시 모드";
        public override string Editor_HatGenre_FollowWeapon => "무기 따라가기";
        public override string Editor_HatGenre_Uniform => "통일된 모자";
        public override string Editor_CopyPasteSelectedColor => "선택한 색상 복사";

        public override string Character_Accessories => "장신구";
        public override string Character_Hat => "모자";
        public override string Character_Head => "머리";
        public override string Character_Body => "몸통";
        public override string Character_Arms => "팔";
        public override string Character_Back => "등";
        public override string Character_Face => "얼굴";

        public override string BuildingType_Tavern => "공용 홀";

        public override string Settings_CraftMultiplier => "제작 시간 배율";
        public override string Settings_ChildMultiplier_Description => "새로운 일꾼이 추가되는 속도를 높입니다.";

        public override string Settings_CasualControls => "간단한 조작 모드";
        public override string Settings_CasualControls_Description => "선택지를 줄여 핵심 결정만 내리도록 단순화합니다. 자원은 오직 금화만 사용됩니다.";

        public override string Settings_AdvancedControls => "고급 조작 모드";
        public override string Settings_AdvancedControls_Description => "전체 자원 관리 경험을 제공합니다.";

        public override string WarsResourceGroup_Metal => "금속";
        public override string Work_Craft => "제작";
        public override string Work_OnlyCraftOnFullStock => "재고가 가득 찼을 때만 제작";

        public override string ExperienceType_Smelting => "제련";
        public override string Category_Optimize => "최적화";
        public override string BuildCategory_Road => "도로";
        public override string XP_UnlockBuildPrio => "건설 우선순위 해제: {0}";
        public override string Technology_ModernFarming => "현대 농업";

        public override string ExportImportDescription => "다른 플레이어와 세이브 파일을 공유하려면, 모든 파일은 다음 폴더에 있습니다: {0}";

        public override string CityCultureDescription => "문화는 도시에 특별한 보너스를 제공합니다.";

        public override string UnitType_CloseRangeRifle => "아르케부지어";
        public override string UnitType_LongRangeRifle => "머스킷티어";
        public override string UnitType_Skirmisher => "척후병";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "루나리 궁수";
        public override string UnitType_MithrilSwordsman => "루나리 기사";

        public override string Defence_AutoAssign_Towers => "탑 자동 배치";

        public override string EventMessage_DesertersText_Food => "군대에서 굶주린 병사들이 탈영하고 있습니다.";

        public override string Tutorial_CasualRecruitSoldiers => "병사 부대를 구매하세요.";

        // Shadow update
        public override string Technology_CannotReassign => "연구가 완료될 때까지 기술을 재배정할 수 없습니다.";
        public override string Diplomacy_DeclareWarAgainst => "다음 세력에 선전포고합니다:";
        public override string Diplomacy_AllyCount => "동맹 수";
        public override string Diplomacy_CostPerAlly => "동맹 하나당 비용이 {0}만큼 증가합니다.";

        public override string Event_ChanceOfFailure => "실패 확률: {0}%";
        public override string EventMessage_Event_Title => "이벤트";
        public override string EventMessage_TheCohalition => "연합군";

        public override string EventMessage_DarkHorde => "암흑 군단";
        public override string EventMessage_DarkHordeKiller_Title => "암흑 군단의 처단자";
        public override string EventMessage_DarkHordeKiller_Message => "용맹한 기사들이 당신의 군대에 합류했습니다.";

        public override string Settings_Mode_Spectator_Description => "관전자 모드 — 바라보기만 하거나 신의 힘으로 개입할 수도 있습니다.";
        public override string GodPower => "신의 힘";

        public override string Building_TreeSprout_Description => "나무를 심습니다.";
        public override string Building_TreeSprout_Soft => "부드러운 나무 묘목";
        public override string Building_TreeSprout_Hard => "단단한 나무 묘목";

        public override string GeneralSetting_SetAll => "모두 적용";

        public override string Hud_All => "전체";
        public override string Hud_Previous => "이전";
        public override string Hud_EffectWillStack => "이 효과는 중첩됩니다.";

        public override string Info_WhenFoodRunsOut => "식량이 바닥나면 도시와 군대는 자동으로 암시장에 식량을 구매합니다.";

        // Launch test
        public override string InputActionName_NextWar => "다음 전쟁 중인 세력";

        /// <summary>
        /// These symbols are needed to fit large numbers on the HUD, there will be a tooltip to explain what number it represents
        /// </summary>
        public override string EngineHud_SymbolFor100 => "c";
        public override string EngineHud_SymbolFor1000 => "k";
        public override string EngineHud_SymbolFor10000 => "10k";

        /// <summary>
        /// When loading files from other players, you won’t get their achievement progress. Use the word for Steam Achievements.
        /// </summary>
        public override string GameMenu_BlockImportAchievements => "불러온 파일에서 도전 과제 진행이 차단됩니다.";

        public override string EndScreen_PeaceVictoryQuote => "칼을 내려놓고, 더 나은 미래를 향해 나아갑시다.";

        public override string VictoryType_DefeatBoss => "보스 격파";
        public override string VictoryType_Domination => "지배";
        public override string VictoryType_WorldPeace => "세계 평화";

    }
}
