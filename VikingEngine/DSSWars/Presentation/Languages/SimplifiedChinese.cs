using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class SimplifiedChinese : AbsLanguage
    {
        //Winter patch 3
        public override string Hud_Purchase_AllBuildings => "所有建筑加入队列";
        public override string Hud_Purchase_AllTech => "所有科技加入队列";
        public override string BuildingType_CasualBarracks_Description => "士兵招募时间由所有兵营分摊";
        //Winter update patch + spring
        /// <summary>
        /// How much of a resource that will be used, e.g. "5 gold". There will be a "cost" title above the text. 0: Resource, 1: cost
        /// </summary>
        public override string Hud_Purchase_ResourceCost => "{1} {0}";

        public override string DisplayMode => "显示模式";
        public override string DisplayMode_Windowed => "窗口模式";
        public override string DisplayMode_BorderlessFullscreen => "无边框窗口";

        public override string GameSettings_RenderedMouseCursor => "游戏内光标"; // Implies a cursor rendered by the game
        public override string GameSettings_MuteControllerDisconnect => "屏蔽手柄断开提示";

        public override string Delivery_MaxDistance => "最大运输距离：{0}";
        public override string Tutorial_WillTakeAWhile => "这需要一些时间，请稍后再来。";

        /// <summary>
        /// 0: name of building
        /// </summary>
        public override string Tutorial_WaitFor => "等待 {0} 完成";
        public override string GameOverResults => "游戏历史记录";

        public override string UnitType_UnclaimedLand => "无主之地"; // "Land without a master" - standard gaming term
        public override string UnitType_Settler => "开拓者"; // The standard term in Civ 6 for Settler
        public override string UnitType_Settler_Description => "建立新城市";
        public override string Resource_ConsumedProduced => "消耗 / 产出";
        public override string InputActionName_PlaceTarget => "放置目标";

        public override string FactionStartSize => "势力初始规模";
        public override string FactionStartSize_Full => "完整";
        public override string FactionStartSize_OneCity => "一座城市";
        public override string FactionStartSize_Settler => "一个开拓者";

        //Winter update
        public override string Resource_StockpileLimit => "库存上限";
        public override string GameMode_QuickMatch => "Quick Match";
        public override string GameMode_QuickMatch_Description =>
            "更短的游戏模式。投入与敌对国家之间的全面战争吧。";
        public override string Lobby_PlayerCount => "玩家人数";
        public override string Lobby_TwoTeams => "两支队伍";
        public override string Hud_Produce => "生产:";
        public override string Tutorial_WaitForWorkerLevel => "等待一名工人达到：";

        public override string Tutorial_PracticeOrSchool => "在 {0} 上练习，或使用 {1}";
        public override string Tutorial_AddTag => "添加标签 (tag):";
        public override string Tutorial_AddPin => "添加图钉 (pin):";
        public override string Tutorial_SelectMostTrees => "找到你树木最多的城市";
        public override string Tutorial_SelectACityWithX => "选择一个拥有 {0} 的城市";

        public override string Tutorial_Select_NotCapital => "。不是你的首都。";

        public override string Tutorial_SetXPriorityToY => "将 {0} 的优先级设置为 {1}";
        public override string Tutorial_AdvisorMission => "Advisor 任务";

        public override string Tutorial_AdvisorDescription =>
            "完整游戏已开始。Advisor 将通过更多有用的任务扩展教程。";

        public override string Tutorial_EndAdvisor => "结束 Advisor";

        public override string Tutorial_AdvisorCompleteTitle => "Advisor 完成！";
        public override string Tutorial_AdvisorCompleteMessage => "愿你接下来的一天被祝福！";

        public override string Hud_Search => "搜索";

        public override string DifficultyDescription_ExtremeAggression => "极端侵略性";

        public override string MapFilter => "地图过滤器";

        public override string Settings_TechMultiplier => "Tech 研究速度";

        public override string EndScreen_MatchComplete => "比赛结果";

        public override string FactionName_DragonGem => "Dragon Gem";
        public override string FactionName_Tomten => "Tomten";
        public override string FactionName_Hælfolc => "Hælfolc";
        public override string FactionName_AerimAngren => "Aerim Angren";

        public override string HUD_NotAvailbleInX => "在 {0} 中不可用";

        public override string InputActionName_MiniMap => "Mini-map";

        //--
        public override string Error_SoundInitFailure => "声音初始化失败";

        public override string GameMenu_ControllerDisconnected => "控制器已断开连接";

        public override string Tutorial_HighPriority => "你的士兵会优先完成高优先级的任务。";

        public override string BuildingType_Wall_Description => "城墙可以保护部队免受攻击，并提供少量攻击力加成（Boost）。";

        public override string BuildingType_Wall_Siege => "攻城武器会削弱城墙的防御力。";

        public override string Conscript_BlockChance => "{0}% 几率格挡攻击（Block）。";

        public override string Battle_DeclarWarReminder => "攻击前必须先宣战。";

        //--


        /// <summary>
        /// 本语言的名称
        /// </summary>
        public override string MyLanguage => "英语";

        /// <summary>
        /// 如何显示项目的数量。0: 项目, 1: 数量
        /// </summary>
        public override string Language_ItemCountPresentation => "{0}: {1}";

        /// <summary>
        /// 选择语言选项
        /// </summary>
        public override string Lobby_Language => "语言";

        /// <summary>
        /// 开始游戏
        /// </summary>
        public override string Lobby_Start => "开始";

        /// <summary>
        /// 按钮选择本地多人游戏数量，0:当前玩家数量
        /// </summary>
        public override string Lobby_LocalMultiplayerEdit => "本地多人游戏";

        /// <summary>
        /// 选择分屏玩家数量菜单的标题
        /// </summary>
        public override string Lobby_LocalMultiplayerTitle => "选择玩家数量";

        /// <summary>
        /// 本地多人游戏说明
        /// </summary>
        public override string Lobby_LocalMultiplayerControllerRequired => "多人游戏需要 Xbox 控制器";

        /// <summary>
        /// 移动到下一个分屏位置
        /// </summary>
        public override string Lobby_NextScreen => "下一个屏幕位置";

        /// <summary>
        /// 玩家可以选择视觉外观并将其存储在配置文件中
        /// </summary>
        public override string Lobby_FlagSelectTitle => "选择旗帜";

        /// <summary>
        /// 0: 编号1到16
        /// </summary>
        public override string Lobby_FlagNumbered => "旗帜 {0}";

        /// <summary>
        /// 游戏名称和版本号
        /// </summary>
        public override string Lobby_GameVersion => "DSS 战争派对 - 版本 {0}";

        public override string FlagEditor_Description => "绘制你的旗帜并为你的士兵选择颜色。";

        /// <summary>
        /// 用颜色填充区域的绘画工具
        /// </summary>
        public override string FlagEditor_Bucket => "填充工具";

        /// <summary>
        /// 打开旗帜配置文件编辑器
        /// </summary>
        public override string Lobby_FlagEdit => "编辑旗帜";

        public override string Lobby_WarningTitle => "警告";
        public override string Lobby_IgnoreWarning => "忽略警告";

        /// <summary>
        /// 当一个玩家没有选择输入时的警告。
        /// </summary>
        public override string Lobby_PlayerWithoutInputWarning => "一个玩家没有输入";

        /// <summary>
        /// 包含大多数玩家不会使用的内容的菜单。
        /// </summary>
        public override string Lobby_Extra => "额外内容";

        /// <summary>
        /// 额外内容没有翻译或完整的控制器支持。
        /// </summary>
        public override string Lobby_Extra_NoSupportWarning => "警告！该内容未本地化或预期输入/可访问性支持";

        public override string Lobby_MapSizeTitle => "地图大小";

        /// <summary>
        /// 地图大小1名称
        /// </summary>
        public override string Lobby_MapSizeOptTiny => "极小";

        /// <summary>
        /// 地图大小2名称
        /// </summary>
        public override string Lobby_MapSizeOptSmall => "小";

        /// <summary>
        /// 地图大小3名称
        /// </summary>
        public override string Lobby_MapSizeOptMedium => "中";

        /// <summary>
        /// 地图大小4名称
        /// </summary>
        public override string Lobby_MapSizeOptLarge => "大";

        /// <summary>
        /// 地图大小5名称
        /// </summary>
        public override string Lobby_MapSizeOptHuge => "特大";

        /// <summary>
        /// 地图大小6名称
        /// </summary>
        public override string Lobby_MapSizeOptEpic => "史诗";

        /// <summary>
        /// 地图大小描述X乘Y公里。0: 宽度, 1: 高度
        /// </summary>
        public override string Lobby_MapSizeDesc => "{0}x{1} 公里";

        /// <summary>
        /// 关闭游戏应用
        /// </summary>
        public override string Lobby_ExitGame => "退出";

        /// <summary>
        /// 显示本地多人游戏名称，0: 玩家编号
        /// </summary>
        public override string Player_DefaultName => "玩家 {0}";

        /// <summary>
        /// 在玩家配置文件编辑器中。打开带有编辑器选项的菜单
        /// </summary>
        public override string ProfileEditor_OptionsMenu => "选项";

        /// <summary>
        /// 在玩家配置文件编辑器中。选择旗帜颜色的标题
        /// </summary>
        public override string ProfileEditor_FlagColorsTitle => "旗帜颜色";

        /// <summary>
        /// 在玩家配置文件编辑器中。旗帜颜色选项
        /// </summary>
        public override string ProfileEditor_MainColor => "主颜色";

        /// <summary>
        /// 在玩家配置文件编辑器中。旗帜颜色选项
        /// </summary>
        public override string ProfileEditor_Detail1Color => "细节颜色1";

        /// <summary>
        /// 在玩家配置文件编辑器中。旗帜颜色选项
        /// </summary>
        public override string ProfileEditor_Detail2Color => "细节颜色2";

        /// <summary>
        /// 在玩家配置文件编辑器中。选择士兵颜色的标题
        /// </summary>
        public override string ProfileEditor_PeopleColorsTitle => "人物颜色";

        /// <summary>
        /// 在玩家配置文件编辑器中。士兵颜色选项
        /// </summary>
        public override string ProfileEditor_SkinColor => "肤色";

        /// <summary>
        /// 在玩家配置文件编辑器中。士兵颜色选项
        /// </summary>
        public override string ProfileEditor_HairColor => "头发颜色";

        /// <summary>
        /// 在玩家配置文件编辑器中。打开调色板并选择颜色
        /// </summary>
        public override string ProfileEditor_PickColor => "选择颜色";

        /// <summary>
        /// 在玩家配置文件编辑器中。调整图像位置
        /// </summary>
        public override string ProfileEditor_MoveImage => "移动图像";

        /// <summary>
        /// 在玩家配置文件编辑器中。移动方向
        /// </summary>
        public override string ProfileEditor_MoveImageLeft => "左";

        /// <summary>
        /// 在玩家配置文件编辑器中。移动方向
        /// </summary>
        public override string ProfileEditor_MoveImageRight => "右";

        /// <summary>
        /// 在玩家配置文件编辑器中。移动方向
        /// </summary>
        public override string ProfileEditor_MoveImageUp => "上";

        /// <summary>
        /// 在玩家配置文件编辑器中。移动方向
        /// </summary>
        public override string ProfileEditor_MoveImageDown => "下";

        /// <summary>
        /// 在玩家配置文件编辑器中。不保存并关闭编辑器
        /// </summary>
        public override string ProfileEditor_DiscardAndExit => "放弃并退出";

        /// <summary>
        /// 在玩家配置文件编辑器中。放弃的工具提示
        /// </summary>
        public override string ProfileEditor_DiscardAndExitDescription => "撤销所有更改";

        /// <summary>
        /// 在玩家配置文件编辑器中。保存更改并关闭编辑器
        /// </summary>
        public override string Hud_SaveAndExit => "保存并退出";

        /// <summary>
        /// 在玩家配置文件编辑器中。色调、饱和度和亮度颜色选项的一部分。
        /// </summary>
        public override string ProfileEditor_Hue => "色调";

        /// <summary>
        /// 在玩家配置文件编辑器中。色调、饱和度和亮度颜色选项的一部分。
        /// </summary>
        public override string ProfileEditor_Lightness => "亮度";

        /// <summary>
        /// 在玩家配置文件编辑器中。在旗帜和士兵颜色选项之间切换。
        /// </summary>
        public override string ProfileEditor_NextColorType => "下一个颜色类型";

        /// <summary>
        /// 游戏的当前运行速度，相对于真实时间
        /// </summary>
        public override string Hud_GameSpeedLabel => "游戏速度: {0}倍";

        public override string Input_GameSpeed => "游戏速度";

        /// <summary>
        /// 游戏内显示。单位黄金产量
        /// </summary>
        public override string Hud_TotalIncome => "每秒总收入: {0}";

        /// <summary>
        /// 单位黄金成本。
        /// </summary>
        public override string Hud_Upkeep => "维持费用: {0}";
        public override string Hud_ArmyUpkeep => "军队维持费用: {0}";

        /// <summary>
        /// 游戏内显示。保护建筑的士兵。
        /// </summary>
        public override string Hud_GuardCount => "守卫";

        public override string Hud_IncreaseMaxGuardCount => "最大守卫数量 {0}";

        public override string Hud_GuardCount_MustExpandCityMessage => "你需要扩展城市。";

        public override string Hud_SoldierCount => "士兵数量: {0}";

        public override string Hud_SoldierGroupsCount => "组数量: {0}";

        /// <summary>
        /// 游戏内显示。单位计算的战斗力。
        /// </summary>
        public override string Hud_StrengthRating => "战斗力: {0}";

        /// <summary>
        /// 游戏内显示。整个国家的计算战斗力。
        /// </summary>
        public override string Hud_TotalStrengthRating => "军事力量: {0}";

        /// <summary>
        /// 游戏内显示。来自城邦外的额外人员。
        /// </summary>
        public override string Hud_Immigrants => "移民";

        public override string Hud_CityCount => "城市数量: {0}";
        public override string Hud_ArmyCount => "军队数量: {0}";


        /// <summary>
        /// 重复购买次数的小按钮。例如“x5”
        /// </summary>
        public override string Hud_XTimes => "x{0}";

        public override string Hud_PurchaseTitle_Requirement => "需求";
        public override string Hud_PurchaseTitle_Cost => "成本";
        public override string Hud_PurchaseTitle_Gain => "收益";

        /// <summary>
        /// 使用多少资源，“5金币。（可用：10）”。上方将有一个“成本”标题。0：资源，1：成本，2：可用
        /// </summary>
        public override string Hud_Purchase_ResourceCostOfAvailable => "{1} {0}。（可用: {2}）";

        public override string Hud_Purchase_CostWillIncreaseByX => "成本将增加{0}";

        public override string Hud_Purchase_MaxCapacity => "已达到最大容量";

        public override string Hud_CompareMilitaryStrength_YourToOther => "力量：你的 {0} - 他们的 {1}";

        /// <summary>
        /// 将日期显示为“年、月、日”的简短字符串
        /// </summary>
        public override string Hud_Date => "年{0} 月{1} 日{2}";

        /// <summary>
        /// 将时间跨度显示为“时、分、秒”的简短字符串
        /// </summary>
        public override string Hud_TimeSpan => "时{0} 分{1} 秒{2}";

        /// <summary>
        /// 两支军队之间或军队与城市之间的战斗
        /// </summary>
        public override string Hud_Battle => "战斗";


        /// <summary>
        /// 描述按钮输入。暂停。
        /// </summary>
        public override string Input_Pause => "暂停";

        /// <summary>
        /// 描述按钮输入。从暂停中恢复。
        /// </summary>
        public override string Input_ResumePaused => "恢复";

        /// <summary>
        /// 通用货币资源
        /// </summary>
        public override string ResourceType_Gold => "金币";

        /// <summary>
        /// 劳动力资源
        /// </summary>
        public override string ResourceType_Workers => "工人";

        public override string ResourceType_Workers_Description => "工人提供收入，并被征召为你的军队士兵";

        /// <summary>
        /// 用于外交的资源
        /// </summary>
        public override string ResourceType_DiplomacyPoints => "外交点";

        /// <summary>
        /// 0：你获得的点数，1：软上限（在此之后增加得更慢），2：硬上限
        /// </summary>
        public override string ResourceType_DiplomacyPoints_WithSoftAndHardLimit => "外交点: {0} / {1} ({2})";

        /// <summary>
        /// 城市建筑类型。为骑士和外交官建造的建筑。
        /// </summary>
        public override string Building_NobleHouse => "贵族之家";

        public override string Building_NobleHouse_DiplomacyPointsAdd => "每{0}秒增加1个外交点";
        public override string Building_NobleHouse_DiplomacyPointsLimit => "外交点上限增加{0}";
        public override string Building_NobleHouse_UnlocksKnight => "解锁骑士单位";

        public override string Building_BuildAction => "建造";
        public override string Building_IsBuilt => "已建造";

        /// <summary>
        /// 城市建筑类型。邪恶的大规模生产。
        /// </summary>
        public override string Building_DarkFactory => "黑暗工厂";

        /// <summary>
        /// 游戏内设置菜单。总结所有难度选项的百分比。
        /// </summary>
        public override string Settings_TotalDifficulty => "总难度 {0}%";

        /// <summary>
        /// 游戏内设置菜单。基础难度选项。
        /// </summary>
        public override string Settings_DifficultyLevel => "难度级别 {0}%";

        /// <summary>
        /// 游戏内设置菜单。选择创建新地图而不是加载地图的选项
        /// </summary>
        public override string Settings_GenerateMaps => "生成新地图";

        /// <summary>
        /// 游戏内设置菜单。创建新地图有较长的加载时间
        /// </summary>
        public override string Settings_GenerateMaps_SlowDescription => "生成比加载预建地图要慢";

        /// <summary>
        /// 游戏内设置菜单。难度选项。阻止在暂停时玩游戏的能力。
        /// </summary>
        public override string Settings_AllowPause => "允许暂停和命令";

        /// <summary>
        /// 游戏内设置菜单。难度选项。有BOSS事件进入游戏。
        /// </summary>
        public override string Settings_BossEvents => "BOSS事件";

        /// <summary>
        /// 游戏内设置菜单。无BOSS描述。
        /// </summary>
        public override string Settings_BossEvents_SandboxDescription => "禁用BOSS事件会将游戏置于无结局的沙盒模式。";

        /// <summary>
        /// 自动化游戏机制的选项。菜单标题。
        /// </summary>
        public override string Automation_Title => "自动化";
        /// <summary>
        /// 自动化游戏机制的选项。有关自动化如何工作的信息。
        /// </summary>
        public override string Automation_InfoLine_MaxWorkforce => "将等待劳动力达到最大";
        /// <summary>
        /// 自动化游戏机制的选项。有关自动化如何工作的信息。
        /// </summary>
        public override string Automation_InfoLine_NegativeIncome => "如果收入为负将暂停";
        /// <summary>
        /// 自动化游戏机制的选项。有关自动化如何工作的信息。
        /// </summary>
        public override string Automation_InfoLine_Priority => "大城市优先";
        /// <summary>
        /// 自动化游戏机制的选项。有关自动化如何工作的信息。
        /// </summary>
        public override string Automation_InfoLine_PurchaseSpeed => "每秒最多执行一次购买";

        /// <summary>
        /// 操作按钮标题。为骑士和外交官建造的专用建筑。
        /// </summary>
        public override string HudAction_BuyItem => "购买 {0}";

        /// <summary>
        /// 两国之间的和平或战争状态
        /// </summary>
        public override string Diplomacy_RelationType => "关系";

        /// <summary>
        /// 其他派系之间关系列表的标题
        /// </summary>
        public override string Diplomacy_RelationToOthers => "他们与他人的关系";

        /// <summary>
        /// 外交关系。你直接控制国家资源。
        /// </summary>
        public override string Diplomacy_RelationType_Servant => "仆人";

        /// <summary>
        /// 外交关系。全面合作。
        /// </summary>
        public override string Diplomacy_RelationType_Ally => "盟友";

        /// <summary>
        /// 外交关系。减少战争的可能性。
        /// </summary>
        public override string Diplomacy_RelationType_Good => "良好";

        /// <summary>
        /// 外交关系。和平协议。
        /// </summary>
        public override string Diplomacy_RelationType_Peace => "和平";

        /// <summary>
        /// 外交关系。尚未建立任何联系。
        /// </summary>
        public override string Diplomacy_RelationType_Neutral => "中立";

        /// <summary>
        /// 外交关系。临时和平协议。
        /// </summary>
        public override string Diplomacy_RelationType_Truce => "休战";

        /// <summary>
        /// 外交关系。战争。
        /// </summary>
        public override string Diplomacy_RelationType_War => "战争";

        /// <summary>
        /// 外交关系。无和平可能的战争。
        /// </summary>
        public override string Diplomacy_RelationType_TotalWar => "全面战争";

        /// <summary>
        /// 外交沟通。你能讨论条款的程度。0：条款
        /// </summary>
        public override string Diplomacy_SpeakTermIs => "讨论条款：{0}";

        /// <summary>
        /// 外交沟通。优于正常。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Good => "良好";

        /// <summary>
        /// 外交沟通。正常。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Normal => "正常";

        /// <summary>
        /// 外交沟通。低于正常。
        /// </summary>
        public override string Diplomacy_SpeakTerms_Bad => "差";

        /// <summary>
        /// 外交沟通。不愿沟通。
        /// </summary>
        public override string Diplomacy_SpeakTerms_None => "无";

        /// <summary>
        /// 外交行动。建立新的外交关系。
        /// </summary>
        public override string Diplomacy_ForgeNewRelationTo => "建立关系：{0}";

        /// <summary>
        /// 外交行动。提议新的外交关系。
        /// </summary>
        public override string Diplomacy_OfferPeace => "提议和平";

        /// <summary>
        /// 外交行动。提议新的外交关系。
        /// </summary>
        public override string Diplomacy_OfferAlliance => "提议结盟";

        /// <summary>
        /// 外交标题。另一个玩家提议新的外交关系。0：玩家名
        /// </summary>
        public override string Diplomacy_PlayerOfferAlliance => "{0} 提议建立新关系";

        /// <summary>
        /// 外交行动。接受新的外交关系。
        /// </summary>
        public override string Diplomacy_AcceptRelationOffer => "接受新关系";

        /// <summary>
        /// 外交描述。另一个玩家提议新的外交关系。0：关系类型
        /// </summary>
        public override string Diplomacy_NewRelationOffered => "提议的新关系：{0}";

        /// <summary>
        /// 外交行动。使另一个国家成为你的仆人。
        /// </summary>
        public override string Diplomacy_AbsorbServant => "吸收为仆人";

        /// <summary>
        /// 外交描述。反对邪恶。
        /// </summary>
        public override string Diplomacy_LightSide => "是光明阵营盟友";

        /// <summary>
        /// 外交描述。休战将持续多长时间。
        /// </summary>
        public override string Diplomacy_TruceTimeLength => "将在 {0} 秒后结束";

        /// <summary>
        /// 外交行动。使休战时间更长。
        /// </summary>
        public override string Diplomacy_ExtendTruceAction => "延长休战";

        /// <summary>
        /// 外交描述。休战将延长多长时间。
        /// </summary>
        public override string Diplomacy_TruceExtendTimeLength => "休战延长 {0} 秒";

        /// <summary>
        /// 外交描述。违反已达成的关系将花费外交点。
        /// </summary>
        public override string Diplomacy_BreakingRelationCost => "违反关系将花费 {0} 外交点";

        /// <summary>
        /// 盟友的外交描述。
        /// </summary>
        public override string Diplomacy_AllyDescription => "盟友共享战争声明。";

        /// <summary>
        /// 良好关系的外交描述。
        /// </summary>
        public override string Diplomacy_GoodRelationDescription => "限制宣战能力。";

        /// <summary>
        /// 外交描述。你必须比你的仆人（你将控制的另一个国家）拥有更大的军事力量。
        /// </summary>
        public override string Diplomacy_ServantRequirement_XStrongerMilitary => "军事力量强 {0} 倍";

        /// <summary>
        /// 外交描述。仆人必须陷入无望的战争（你将控制的另一个国家）。
        /// </summary>
        public override string Diplomacy_ServantRequirement_HopelessWar => "仆人必须与更强大的敌人作战";

        /// <summary>
        /// 外交描述。仆人不能拥有太多城市（你将控制的另一个国家）。
        /// </summary>
        public override string Diplomacy_ServantRequirement_MaxCities => "仆人最多可以拥有 {0} 座城市";

        /// <summary>
        /// 外交描述。外交点的花费将增加（你将控制的另一个国家）。
        /// </summary>
        public override string Diplomacy_ServantPriceWillRise => "每个仆人的价格都会上涨";

        /// <summary>
        /// 外交描述。仆人关系的结果，和平接管另一个国家。
        /// </summary>
        public override string Diplomacy_ServantGainAbsorbFaction => "吸收其他派系";

        /// <summary>
        /// 收到战争声明时的消息
        /// </summary>
        public override string Diplomacy_WarDeclarationTitle => "战争宣告！";

        /// <summary>
        /// 休战计时器已结束，你将回到战争状态
        /// </summary>
        public override string Diplomacy_TruceEndTitle => "休战已结束";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。显示标题。
        /// </summary>
        public override string EndGameStatistics_Title => "统计";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。游戏内已过时间。
        /// </summary>
        public override string EndGameStatistics_Time => "游戏时间: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你招募的士兵数量。
        /// </summary>
        public override string EndGameStatistics_SoldiersRecruited => "招募的士兵: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你在战斗中损失的士兵数量。
        /// </summary>
        public override string EndGameStatistics_FriendlySoldiersLost => "战斗中损失的士兵: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你在战斗中击杀的敌方士兵数量。
        /// </summary>
        public override string EndGameStatistics_EnemySoldiersKilled => "战斗中击杀的敌方士兵: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你背叛的士兵数量。
        /// </summary>
        public override string EndGameStatistics_SoldiersDeserted => "叛逃的士兵: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你在战斗中获得的城市数量。
        /// </summary>
        public override string EndGameStatistics_CitiesCaptured => "占领的城市: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你在战斗中失去的城市数量。
        /// </summary>
        public override string EndGameStatistics_CitiesLost => "失去的城市: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你赢得的战斗数量。
        /// </summary>
        public override string EndGameStatistics_BattlesWon => "赢得的战斗: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。你失去的战斗数量。
        /// </summary>
        public override string EndGameStatistics_BattlesLost => "失去的战斗: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。外交。你发起的战争声明数量。
        /// </summary>
        public override string EndGameStatistics_WarsStartedByYou => "发起的战争声明: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。外交。你收到的战争声明数量。
        /// </summary>
        public override string EndGameStatistics_WarsStartedByEnemy => "收到的战争声明: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。通过外交建立的盟友数量。
        /// </summary>
        public override string EndGameStatistics_AlliedFactions => "外交联盟: {0}";

        /// <summary>
        /// 游戏结束画面上显示的统计数据。通过外交建立的仆人数量。仆人的城市和军队变成你的。
        /// </summary>
        public override string EndGameStatistics_ServantFactions => "外交仆人: {0}";

        /// <summary>
        /// 地图上的集体单位类型。士兵的军队。
        /// </summary>
        public override string UnitType_Army => "军队";

        /// <summary>
        /// 地图上的集体单位类型。士兵的军队。
        /// </summary>
        public override string UnitType_SoldierGroup => "队伍";

        /// <summary>
        /// 地图上的集体单位类型。村庄或城市的通用名称。
        /// </summary>
        public override string UnitType_City => "城市";

        /// <summary>
        /// 一组军队选择
        /// </summary>
        public override string UnitType_ArmyCollectionAndCount => "军队组，数量: {0}";

        /// <summary>
        /// 一种专门的士兵类型。标准前线士兵。
        /// </summary>
        public override string UnitType_Soldier => "士兵";

        /// <summary>
        /// 一种专门的士兵类型。海战士兵。
        /// </summary>
        public override string UnitType_Sailor => "水手";

        /// <summary>
        /// 一种专门的士兵类型。征募的农民。
        /// </summary>
        public override string UnitType_Folkman => "民兵";

        /// <summary>
        /// 一种专门的士兵类型。持盾和矛的单位。
        /// </summary>
        public override string UnitType_Spearman => "矛兵";

        /// <summary>
        /// 一种专门的士兵类型。精英部队，国王卫队的一部分。
        /// </summary>
        public override string UnitType_HonorGuard => "荣誉卫队";

        /// <summary>
        /// 一种专门的士兵类型。反骑兵，持长柄双手矛。
        /// </summary>
        public override string UnitType_Pikeman => "枪兵";

        /// <summary>
        /// 一种专门的士兵类型。装甲骑兵单位。
        /// </summary>
        public override string UnitType_Knight => "骑士";

        /// <summary>
        /// 一种专门的士兵类型。弓箭手。
        /// </summary>
        public override string UnitType_Archer => "弓箭手";

        /// <summary>
        /// 一种专门的士兵类型。弩手。
        /// </summary>
        public override string UnitType_Crossbow => "弩手";

        /// <summary>
        /// 一种专门的士兵类型。投掷大型矛的战争机器。
        /// </summary>
        public override string UnitType_Ballista => "弩炮";

        /// <summary>
        /// 一种专门的士兵类型。穿戴大炮的幻想巨魔。
        /// </summary>
        public override string UnitType_Trollcannon => "巨魔炮";

        /// <summary>
        /// 一种专门的士兵类型。来自森林的士兵。
        /// </summary>
        public override string UnitType_GreenSoldier => "绿色士兵";

        /// <summary>
        /// 一种专门的士兵类型。来自北方的海军单位。
        /// </summary>
        public override string UnitType_Viking => "维京人";

        /// <summary>
        /// 一种专门的士兵类型。邪恶的主宰。
        /// </summary>
        public override string UnitType_DarkLord => "黑暗领主";

        /// <summary>
        /// 一种专门的士兵类型。携带大旗的士兵。
        /// </summary>
        public override string UnitType_Bannerman => "旗手";

        /// <summary>
        /// 一种军事单位。运输士兵的战舰。0：运输的单位类型
        /// </summary>
        public override string UnitType_WarshipWithUnit => "{0}战舰";

        public override string UnitType_Description_Soldier => "通用单位。";
        public override string UnitType_Description_Sailor => "在海战中很强。";
        public override string UnitType_Description_Folkman => "廉价的未训练士兵。";
        public override string UnitType_Description_HonorGuard => "无需维护的精英士兵。";
        public override string UnitType_Description_Knight => "在空地战斗中很强。";
        public override string UnitType_Description_Archer => "只有在受到保护时才强大。";
        public override string UnitType_Description_Crossbow => "强大的远程士兵。";
        public override string UnitType_Description_Ballista => "对城市很强。";
        public override string UnitType_Description_GreenSoldier => "令人畏惧的精灵战士。";
        public override string UnitType_Description_DarkLord => "最终头目";
        /// <summary>
        /// 关于士兵类型的信息
        /// </summary>
        public override string SoldierStats_Title => "每单位统计";

        /// <summary>
        /// 有多少组士兵
        /// </summary>
        public override string SoldierStats_GroupCountAndSoldierCount => "{0}组，总共{1}单位";

        /// <summary>
        /// 士兵在空地上、从船上或攻击定居点时会有不同的力量
        /// </summary>
        public override string SoldierStats_AttackStrengthLandSeaCity => "攻击强度：陆地{0} | 海上{1} | 城市{2}";

        /// <summary>
        /// 士兵能承受多少伤害
        /// </summary>
        public override string SoldierStats_Health => "生命值: {0}";

        /// <summary>
        /// 一些士兵会增加军队的移动速度
        /// </summary>
        public override string SoldierStats_SpeedBonusLand => "陆地军队速度加成：{0}";

        /// <summary>
        /// 一些士兵会增加船只的移动速度
        /// </summary>
        public override string SoldierStats_SpeedBonusSea => "海上军队速度加成：{0}";

        /// <summary>
        /// 购买的士兵将以新兵的身份开始，并在几分钟后完成训练。
        /// </summary>
        public override string SoldierStats_RecruitTrainingTimeMinutes => "训练时间：{0}分钟。如果新兵邻近城市，训练时间将加快一倍。";

        /// <summary>
        /// 控制军队的菜单选项。让他们停止移动。
        /// </summary>
        public override string ArmyOption_Halt => "停止";

        /// <summary>
        /// 控制军队的菜单选项。移除士兵。
        /// </summary>
        public override string ArmyOption_Disband => "解散单位";

        /// <summary>
        /// 控制军队的菜单选项。发送士兵到不同的军队。
        /// </summary>
        public override string ArmyOption_Divide => "分割军队";

        /// <summary>
        /// 控制军队的菜单选项。移除士兵。
        /// </summary>
        public override string ArmyOption_RemoveX => "移除 {0}";

        /// <summary>
        /// 控制军队的菜单选项。移除士兵。
        /// </summary>
        public override string ArmyOption_DisbandAll => "全部解散";

        /// <summary>
        /// 控制军队的菜单选项。0：数量，1：单位类型
        /// </summary>
        public override string ArmyOption_XGroupsOfType => "{1} 组: {0}";

        /// <summary>
        /// 控制军队的菜单选项。发送士兵到不同的军队。
        /// </summary>
        public override string ArmyOption_SendToX => "发送单位到 {0}";

        public override string ArmyOption_MergeAllArmies => "合并所有军队";

        /// <summary>
        /// 控制军队的菜单选项。发送士兵到不同的军队。
        /// </summary>
        public override string ArmyOption_SendToNewArmy => "分配单位到新军队";

        /// <summary>
        /// 控制军队的菜单选项。发送士兵到不同的军队。
        /// </summary>
        public override string ArmyOption_SendX => "发送 {0}";

        /// <summary>
        /// 控制军队的菜单选项。发送士兵到不同的军队。
        /// </summary>
        public override string ArmyOption_SendAll => "全部发送";

        /// <summary>
        /// 控制军队的菜单选项。将军队一分为二。
        /// </summary>
        public override string ArmyOption_DivideHalf => "将军队一分为二";

        /// <summary>
        /// 控制军队的菜单选项。合并军队。
        /// </summary>
        public override string ArmyOption_MergeArmies => "合并军队";


        /// <summary>
        /// 招募士兵。
        /// </summary>
        public override string UnitType_Recruit => "招募";

        /// <summary>
        /// 招募某种类型的士兵。0：类型
        /// </summary>
        public override string CityOption_RecruitType => "招募 {0}";

        /// <summary>
        /// 雇佣士兵数量
        /// </summary>
        public override string CityOption_XMercenaries => "雇佣兵: {0}";


        /// <summary>
        /// 表示市场上当前可供雇佣的雇佣兵数量
        /// </summary>
        public override string Hud_MercenaryMarket => "市场雇佣兵可供雇佣";

        /// <summary>
        /// 购买一定数量的雇佣兵
        /// </summary>
        public override string CityOption_BuyXMercenaries => "雇佣 {0} 个雇佣兵";

        public override string CityOption_Mercenaries_Description => "士兵将从雇佣兵中征召，而不是从你的劳动力中征召";

        /// <summary>
        /// 操作按钮标题。为更多工人创建住房。
        /// </summary>
        public override string CityOption_ExpandWorkForce => "扩大劳动力";
        public override string CityOption_ExpandWorkForce_IncreaseMax => "最大劳动力 +{0}";
        public override string CityOption_ExpandGuardSize => "扩大守卫";

        public override string CityOption_Damages => "损坏: {0}";
        public override string CityOption_Repair => "修复损坏";
        public override string CityOption_RepairGain => "修复 {0} 损坏";

        public override string CityOption_Repair_Description => "损坏会降低你能容纳的工人人数。";

        public override string CityOption_BurnItDown => "烧毁它";
        public override string CityOption_BurnItDown_Description => "移除劳动力并应用最大损坏";

        /// <summary>
        /// 主要boss。名字源于其额头上的发光金属石。
        /// </summary>
        public override string FactionName_DarkLord => "末日之眼";

        /// <summary>
        /// 受兽人启发的派系。为黑暗领主工作。
        /// </summary>
        public override string FactionName_DarkFollower => "恐惧仆从";

        /// <summary>
        /// 最大的派系，古老但腐化的王国。
        /// </summary>
        public override string FactionName_UnitedKingdom => "联合王国";

        /// <summary>
        /// 受精灵启发的派系。与森林和谐共处。
        /// </summary>
        public override string FactionName_Greenwood => "绿林";

        /// <summary>
        /// 东方风味的派系
        /// </summary>
        public override string FactionName_EasternEmpire => "东方帝国";

        /// <summary>
        /// 北方的维京风格王国。最大的一个。
        /// </summary>
        public override string FactionName_NordicRealm => "北欧王国";

        /// <summary>
        /// 北方的维京风格王国。使用熊爪作为象征。
        /// </summary>
        public override string FactionName_BearClaw => "熊爪";

        /// <summary>
        /// 北方的维京风格王国。使用公鸡作为象征。
        /// </summary>
        public override string FactionName_NordicSpur => "北欧刺";

        /// <summary>
        /// 北方的维京风格王国。使用黑色渡鸦作为象征。
        /// </summary>
        public override string FactionName_IceRaven => "冰鸦";

        /// <summary>
        /// 因用强大的弩炮杀死龙而闻名的派系。
        /// </summary>
        public override string FactionName_Dragonslayer => "屠龙者";

        /// <summary>
        /// 来自南方的雇佣兵单位。阿拉伯风格。
        /// </summary>
        public override string FactionName_SouthHara => "南哈拉";

        /// <summary>
        /// 中立CPU控制的国家的名称
        /// </summary>
        public override string FactionName_GenericAi => "AI {0}";

        /// <summary>
        /// 显示玩家及其编号
        /// </summary>
        public override string FactionName_Player => "玩家 {0}";

        /// <summary>
        /// 当小boss从南方乘船接近时的消息。
        /// </summary>
        public override string EventMessage_HaraMercenaryTitle => "敌人接近！";
        public override string EventMessage_HaraMercenaryText => "发现哈拉雇佣兵在南方出现";

        /// <summary>
        /// 第一次警告主要boss即将出现。
        /// </summary>
        public override string EventMessage_ProphesyTitle => "黑暗预言";
        public override string EventMessage_ProphesyText => "末日之眼即将出现，你的敌人将加入他！";

        /// <summary>
        /// 第二次警告主要boss即将出现。
        /// </summary>
        public override string EventMessage_FinalBossEnterTitle => "黑暗时刻";
        public override string EventMessage_FinalBossEnterText => "末日之眼已经进入地图！";

        /// <summary>
        /// 当主要boss将在战场上与你相遇时的消息。
        /// </summary>
        public override string EventMessage_FinalBattleTitle => "绝望的攻击";
        public override string EventMessage_FinalBattleText => "黑暗领主已经加入战场。现在是摧毁他的机会！";

        /// <summary>
        /// 当你无法支付士兵的维持费用时士兵离开军队的消息
        /// </summary>
        public override string EventMessage_DesertersTitle => "逃兵！";
        public override string EventMessage_DesertersText_Money => "未支付薪水的士兵正在逃离你的军队";


        public override string DifficultyDescription_AiAggression => "AI攻击性: {0}。";
        public override string DifficultyDescription_BossSize => "Boss规模: {0}。";
        public override string DifficultyDescription_BossEnterTime => "Boss进入时间: {0}。";
        public override string DifficultyDescription_AiEconomy => "AI经济: {0}%。";
        public override string DifficultyDescription_AiDelay => "AI延迟: {0}。";
        public override string DifficultyDescription_DiplomacyDifficulty => "外交难度: {0}。";
        public override string DifficultyDescription_MercenaryCost => "雇佣兵成本: {0}。";
        public override string DifficultyDescription_HonorGuards => "荣誉卫队: {0}。";

        /// <summary>
        /// 游戏成功结束。
        /// </summary>
        public override string EndScreen_VictoryTitle => "胜利！";

        /// <summary>
        /// 你在游戏中扮演的领袖角色的名言
        /// </summary>
        public override List<string> EndScreen_VictoryQuotes => new List<string>
{
    "在和平时期，我们哀悼逝者。",
    "每一次胜利都带着牺牲的阴影。",
    "记住带我们到这里的旅程，充满了勇士的灵魂。",
    "我们的胜利之光照亮了心灵，我们的心因失去的战士而沉重。"
};

        public override string EndScreen_DominationVictoryQuote => "我是被神选中的，来统治世界！";

        /// <summary>
        /// 游戏失败结束。
        /// </summary>
        public override string EndScreen_FailTitle => "失败！";

        /// <summary>
        /// 你在游戏中扮演的领袖角色的名言
        /// </summary>
        public override List<string> EndScreen_FailureQuotes => new List<string>
{
    "随着我们身体的疲惫和夜晚的担忧，我们迎来了终结。",
    "失败可能会使我们的土地黯淡，但无法熄灭我们的决心之光。",
    "熄灭我们心中的火焰，从他们的灰烬中，我们的孩子将铸造新的黎明。",
    "让我们的故事成为明天胜利的火种。"
};

        /// <summary>
        /// 游戏结束时的一个小片段
        /// </summary>
        public override string EndScreen_WatchEpilogue => "观看结尾";

        /// <summary>
        /// 片段标题
        /// </summary>
        public override string EndScreen_Epilogue_Title => "结尾";

        /// <summary>
        /// 片段介绍
        /// </summary>
        public override string EndScreen_Epilogue_Text => "160年前";

        /// <summary>
        /// 序言是关于游戏故事的短诗
        /// </summary>
        public override string GameMenu_WatchPrologue => "观看序言";

        public override string Prologue_Title => "序言";

        /// <summary>
        /// 诗必须是三行，第四行将从名称翻译中提取以展示boss的名字
        /// </summary>
        public override List<string> Prologue_TextLines => new List<string>
{
    "夜晚梦魇缠绕，",
    "预言黑暗的未来",
    "准备迎接他的到来，",
};

        /// <summary>
        /// 暂停时的游戏内菜单
        /// </summary>
        public override string GameMenu_Title => "游戏菜单";

        /// <summary>
        /// 在结束画面后继续游戏
        /// </summary>
        public override string GameMenu_ContinueGame => "继续游戏";

        /// <summary>
        /// 继续游戏
        /// </summary>
        public override string GameMenu_Resume => "继续";

        /// <summary>
        /// 退出到游戏大厅
        /// </summary>
        public override string GameMenu_ExitGame => "退出游戏";

        public override string Hud_Save => "保存";
        public override string GameMenu_SaveStateWarnings => "警告！游戏更新时保存文件将丢失。";
        public override string GameMenu_LoadState => "加载";
        public override string GameMenu_ContinueFromSave => "从保存点继续";

        public override string GameMenu_AutoSave => "自动保存";

        public override string GameMenu_Load_PlayerCountError => "你必须设置与保存文件匹配的玩家数量：{0}";

        public override string Progressbar_MapLoadingState => "地图加载: {0}";

        public override string Progressbar_ProgressComplete => "完成";

        /// <summary>
        /// 0：进度百分比，1：失败次数
        /// </summary>
        public override string Progressbar_MapLoadingState_GeneratingPercentage => "生成中: {0}%。 (失败 {1})";

        /// <summary>
        /// 0：当前部分，1：部分数量
        /// </summary>
        public override string Progressbar_MapLoadingState_LoadPart => "部分 {0}/{1}";

        /// <summary>
        /// 0：百分比或完成
        /// </summary>
        public override string Progressbar_SaveProgress => "保存中: {0}";

        /// <summary>
        /// 0：百分比或完成
        /// </summary>
        public override string Progressbar_LoadProgress => "加载中: {0}";

        /// <summary>
        /// 进度完成，等待玩家输入
        /// </summary>
        public override string Progressbar_PressAnyKey => "按任意键继续";

        /// <summary>
        /// 简短的教程，你需要购买并移动士兵。所有高级控制在教程完成前都将被锁定。
        /// </summary>
        public override string Tutorial_MenuOption => "运行教程";
        public override string Tutorial_MissionsTitle => "教程任务";
        public override string Tutorial_Mission_BuySoldier => "选择一个城市并招募士兵";
        public override string Tutorial_Mission_MoveArmy => "选择一支军队并移动它";

        public override string Tutorial_CompleteTitle => "教程完成！";
        public override string Tutorial_CompleteMessage => "解锁全景缩放和高级游戏选项。";

        /// <summary>
        /// 显示按钮输入
        /// </summary>
        public override string Tutorial_SelectInput => "选择";
        public override string Tutorial_MoveInput => "移动命令";

        /// <summary>
        /// 对战。描述将要进入战斗的两支军队的文字
        /// </summary>
        public override string Hud_Versus => "VS.";

        public override string Hud_WardeclarationTitle => "战争宣言";

        public override string ArmyOption_Attack => "攻击";

        /// <summary>
        /// 游戏设置菜单。更改按下时键和按钮的功能
        /// </summary>
        public override string Settings_ButtonMapping => "按钮映射";



        /// <summary>
        /// 输入类型，标准PC输入
        /// </summary>
        public override string Input_Source_Keyboard => "键盘和鼠标";

        /// <summary>
        /// 输入类型，类似于Xbox使用的手持控制器
        /// </summary>
        public override string Input_Source_Controller => "控制器";

        /* #### --------------- ##### */
        /* #### RESOURCE UPDATE ##### */
        /* #### --------------- ##### */

        public override string CityMenu_SalePricesTitle => "销售价格";
        public override string Blueprint_Title => "蓝图";
        public override string Resource_Tab_Overview => "概览";
        public override string Resource_Tab_Stockpile => "储备";

        public override string Resource => "资源";
        public override string Resource_StockPile_Info => "为资源储备设置目标量，这将通知工人何时开始处理另一种资源。";
        public override string Resource_TypeName_Water => "水";
        public override string Resource_TypeName_Wood => "木材";
        public override string Resource_TypeName_Fuel => "燃料";
        public override string Resource_TypeName_Stone => "石头";
        public override string Resource_TypeName_RawFood => "生食";
        public override string Resource_TypeName_Food => "食物";
        public override string Resource_TypeName_Beer => "啤酒";
        public override string Resource_TypeName_Wheat => "小麦";
        public override string Resource_TypeName_Linen => "亚麻布";
        //public override string Resource_TypeName_SkinAndLinen => "皮革和亚麻";
        public override string Resource_TypeName_IronOre => "铁矿石";
        public override string Resource_TypeName_GoldOre => "金矿石";
        public override string Resource_TypeName_Iron => "铁";

        public override string Resource_TypeName_SharpStick => "尖棍";
        public override string Resource_TypeName_Sword => "剑";
        public override string Resource_TypeName_KnightsLance => "骑士的长矛";
        public override string Resource_TypeName_TwoHandSword => "双手剑";
        public override string Resource_TypeName_Bow => "弓";

        public override string Resource_TypeName_LightArmor => "轻甲";
        public override string Resource_TypeName_MediumArmor => "中甲";
        public override string Resource_TypeName_HeavyArmor => "重甲";

        public override string ResourceType_Children => "儿童";

        public override string BuildingType_DefaultName => "建筑";
        public override string BuildingType_WorkerHut => "工人小屋";
        public override string BuildingType_Brewery => "酿酒厂";
        public override string BuildingType_Postal => "邮政服务";
        public override string BuildingType_Recruitment => "招募中心";
        public override string BuildingType_Barracks => "兵营";
        public override string BuildingType_PigPen => "猪圈";
        public override string BuildingType_HenPen => "鸡舍";
        public override string BuildingType_WorkBench => "工作台";
        public override string BuildingType_Carpenter => "木匠";
        public override string BuildingType_CoalPit => "煤矿";
        public override string DecorType_Statue => "雕像";
        public override string DecorType_Pavement => "铺路";
        public override string BuildingType_Smith => "铁匠铺";
        public override string BuildingType_Cook => "厨师";
        public override string BuildingType_Storage => "仓库";

        public override string BuildingType_ResourceFarm => "{0}农场";

        public override string BuildingType_WorkerHut_DescriptionLimitX => "工人上限增加{0}";
        public override string BuildingType_Tavern_Description => "工人可以在这里用餐";
        public override string BuildingType_Tavern_Brewery => "啤酒生产";
        public override string BuildingType_Postal_Description => "向其他城市发送资源";
        public override string BuildingType_Recruitment_Description => "向其他城市派遣士兵";
        public override string BuildingType_Barracks_Description => "使用人力和装备招募士兵";
        public override string BuildingType_PigPen_Description => "生产猪肉，提供食物和皮革";
        public override string BuildingType_HenPen_Description => "生产鸡肉和鸡蛋，提供食物";
        public override string BuildingType_Decor_Description => "装饰";
        public override string BuildingType_Farm_Description => "种植资源";

        public override string BuildingType_Cook_Description => "食物加工站";
        public override string BuildingType_Bench_Description => "物品制作站";

        public override string BuildingType_Smith_Description => "金属加工站";
        public override string BuildingType_Carpenter_Description => "木材加工站";

        public override string BuildingType_Nobelhouse_Description => "骑士和外交官的住所";
        public override string BuildingType_CoalPit_Description => "高效的燃料生产";
        public override string BuildingType_Storage_Description => "资源储存点";

        public override string MenuTab_Info => "信息";
        public override string MenuTab_Work => "工作";
        public override string MenuTab_Recruit => "招募";
        public override string MenuTab_Resources => "资源";
        public override string MenuTab_Trade => "贸易";
        public override string MenuTab_Build => "建造";
        public override string MenuTab_Economy => "经济";
        public override string MenuTab_Delivery => "配送";

        public override string MenuTab_Build_Description => "在城市中放置建筑物";
        public override string MenuTab_BlackMarket_Description => "在城市中放置建筑物";
        public override string MenuTab_Resources_Description => "在城市中放置建筑物";
        public override string MenuTab_Work_Description => "在城市中放置建筑物";
        public override string MenuTab_Automation_Description => "在城市中放置建筑物";

        public override string BuildHud_OutsideCity => "城市区域外";
        public override string BuildHud_OutsideFaction => "超出你的边界！";

        public override string BuildHud_OccupiedTile => "已占用地块";

        public override string Build_PlaceBuilding => "建造";
        public override string Build_DestroyBuilding => "摧毁";
        public override string Build_ClearTerrain => "清理地形";

        public override string Build_ClearOrders => "清除建筑命令";
        public override string Build_Order => "建筑命令";
        public override string Build_OrderQue => "建筑命令队列: {0}";
        public override string Build_AutoPlace => "自动放置";

        public override string Work_OrderPrioTitle => "工作优先级";
        public override string Work_OrderPrioDescription => "优先级从1（低）到{0}（高）";

        public override string Work_OrderPrio_No => "无优先级。不进行此工作。";
        public override string Work_OrderPrio_Min => "最低优先级。";
        public override string Work_OrderPrio_Max => "最高优先级。";

        public override string Work_Move => "移动物品";

        public override string Work_GatherXResource => "收集{0}";
        public override string Work_CraftX => "制作{0}";
        public override string Work_Farming => "农业";
        public override string Work_Mining => "采矿";
        public override string Work_Trading => "贸易";

        public override string Work_AutoBuild => "自动建造和扩展";

        public override string WorkerHud_WorkType => "工作状态: {0}";
        public override string WorkerHud_Carry => "搬运: {0} {1}";
        public override string WorkerHud_Energy => "能量: {0}";
        public override string WorkerStatus_Exit => "离开劳动力";
        public override string WorkerStatus_Eat => "吃饭";
        public override string WorkerStatus_Till => "耕作";
        public override string WorkerStatus_Plant => "种植";
        public override string WorkerStatus_Gather => "收集";
        public override string WorkerStatus_PickUpResource => "拾取资源";
        public override string WorkerStatus_DropOff => "放下";
        public override string WorkerStatus_BuildX => "建造{0}";
        public override string WorkerStatus_TrossReturnToArmy => "返回军队";

        public override string Hud_ToggleFollowFaction => "切换跟随派系设置";
        public override string Hud_FollowFaction_Yes => "已设置为使用派系的全局设置";
        public override string Hud_FollowFaction_No => "已设置为使用本地设置（全局值为{0}）";

        public override string Hud_Idle => "闲置";
        public override string Hud_NoLimit => "无限制";

        public override string Hud_None => "无";
        public override string Hud_ProductionQueue => "生产队列";

        public override string Hud_EmptyList => "- 空列表 -";

        public override string Hud_RequirementOr => "- 或 -";

        public override string Hud_BlackMarket => "黑市";

        public override string Language_CollectProgress => "{0} / {1}";
        public override string Hud_SelectCity => "选择城市";
        public override string Conscription_Title => "征兵";
        public override string Conscript_WeaponTitle => "武器";
        public override string Conscript_ArmorTitle => "盔甲";
        public override string Conscript_TrainingTitle => "训练";

        public override string Conscript_SpecializationTitle => "专精";
        public override string Conscript_SpecializationDescription => "将提高某个领域的攻击力，同时降低所有其他领域的攻击力，幅度为{0}";
        public override string Conscript_SelectBuilding => "选择兵营";

        public override string Conscript_WeaponDamage => "武器伤害: {0}";
        public override string Conscript_ArmorHealth => "盔甲耐久: {0}";
        public override string Conscript_TrainingSpeed => "攻击速度: {0}";
        public override string Conscript_TrainingTime => "训练时间: {0}";

        public override string Conscript_Training_Minimal => "最低限度";
        public override string Conscript_Training_Basic => "基础";
        public override string Conscript_Training_Skillful => "熟练";
        public override string Conscript_Training_Professional => "专业";

        public override string Conscript_Specialization_Field => "平原";
        public override string Conscript_Specialization_Sea => "海战";
        public override string Conscript_Specialization_Siege => "攻城";
        public override string Conscript_Specialization_Traditional => "传统";
        public override string Conscript_Specialization_AntiCavalry => "反骑兵";

        public override string Conscription_Status_CollectingEquipment => "收集装备: {0}";
        public override string Conscription_Status_CollectingMen => "集结士兵: {0}";
        public override string Conscription_Status_Training => "训练中: {0}";

        public override string ArmyHud_Food_Reserves_X => "食物储备: {0}";
        public override string ArmyHud_Food_Upkeep_X => "食物维持: {0}";
        public override string ArmyHud_Food_Costs_X => "食物成本: {0}";

        public override string Deliver_WillSendXInfo => "每次发送{0}";
        public override string Delivery_ListTitle => "选择配送服务";
        public override string Delivery_DistanceX => "距离: {0}";
        public override string Delivery_DeliveryTimeX => "配送时间: {0}";
        public override string Delivery_SenderMinimumCap => "发送者的最低容量";
        public override string Delivery_RecieverMaximumCap => "接收者的最大容量";
        public override string Delivery_ItemsReady => "物品已准备好";
        public override string Delivery_RecieverReady => "接收者已准备好";
        public override string Hud_ThisCity => "本城市";
        public override string Hud_RecieveingCity => "接收城市";

        public override string Info_ButtonIcon => "i";

        public override string Info_PerSecond => "以每秒资源显示。";

        public override string Info_MinuteAverage => "此值是过去一分钟的平均值。";

        public override string Message_OutOfFood_Title => "食物短缺";
        public override string Message_CityOutOfFood_Text => "将从黑市购买昂贵的食物。当你的钱用完时，工人将会饿死。";

        public override string Hud_EndSessionIcon => "X";

        public override string TerrainType => "地形类型";

        public override string Hud_EnergyUpkeepX => "食物能量维持 {0}";

        public override string Hud_EnergyAmount => "{0} 能量（工作秒数）";

        public override string Hud_CopySetup => "复制设置";
        public override string Hud_Paste => "粘贴";

        public override string Hud_Available => "可用";

        public override string WorkForce_ChildBirthRequirements => "生育条件";
        public override string WorkForce_AvailableHomes => "可用住房: {0}";
        public override string WorkForce_Peace => "和平";
        public override string WorkForce_ChildToManTime => "成年时间: {0} 分钟";

        public override string Economy_TaxIncome => "税收收入: {0}";
        public override string Economy_ImportCostsForResource => "{0}的进口成本: {1}";
        public override string Economy_BlackMarketCostsForResource => "{0}的黑市价格: {1}";
        public override string Economy_GuardUpkeep => "守卫维护费用: {0}";

        public override string Economy_LocalCityTrade_Export => "城市贸易出口: {0}";
        public override string Economy_LocalCityTrade_Import => "城市贸易进口: {0}";

        public override string Economy_ResourceProduction => "{0}生产: {1}";
        public override string Economy_ResourceSpending => "{0}支出: {1}";

        public override string Economy_TaxDescription => "每个工人的税收为{0}金币";

        public override string Economy_SoldResources => "已售资源（金矿石）: {0}";

        public override string UnitType_Cities => "城市";
        public override string UnitType_Armies => "军队";
        public override string UnitType_Worker => "工人";

        public override string UnitType_FootKnight => "长剑骑士";
        public override string UnitType_CavalryKnight => "骑兵骑士";

        public override string CityCulture_LargeFamilies => "大家庭";
        public override string CityCulture_FertileGround => "肥沃的土地";
        public override string CityCulture_Archers => "熟练弓箭手";
        public override string CityCulture_Warriors => "战士";
        public override string CityCulture_AnimalBreeder => "动物饲养者";
        public override string CityCulture_Miners => "矿工";
        public override string CityCulture_Woodcutters => "伐木工";
        public override string CityCulture_Builders => "建筑工人";
        public override string CityCulture_CrabMentality => "蟹心态";
        public override string CityCulture_DeepWell => "深井";
        public override string CityCulture_Networker => "网络专家";
        public override string CityCulture_PitMasters => "燃料大师";

        public override string CityCulture_CultureIsX => "文化: {0}";
        public override string CityCulture_LargeFamilies_Description => "提高出生率";
        public override string CityCulture_FertileGround_Description => "农作物产量增加";
        public override string CityCulture_Archers_Description => "生产熟练的弓箭手";
        public override string CityCulture_Warriors_Description => "生产熟练的近战士兵";
        public override string CityCulture_AnimalBreeder_Description => "动物提供更多资源";
        public override string CityCulture_Miners_Description => "矿石产量增加";
        public override string CityCulture_Woodcutters_Description => "树木产量增加";
        public override string CityCulture_Builders_Description => "建筑速度更快";
        public override string CityCulture_CrabMentality_Description => "工作消耗的能量减少。无法生产高技能的士兵。";
        public override string CityCulture_DeepWell_Description => "水的补充速度更快";
        public override string CityCulture_Networker_Description => "高效的邮政服务";
        public override string CityCulture_PitMasters_Description => "更高的燃料产量";

        public override string CityOption_AutoBuild_Work => "自动扩展劳动力";
        public override string CityOption_AutoBuild_Farm => "自动扩展农场";

        public override string Hud_PurchaseTitle_Resources => "购买资源";
        public override string Hud_PurchaseTitle_CurrentlyOwn => "你拥有";

        public override string Tutorial_EndTutorial => "结束教程";
        public override string Tutorial_MissionX => "任务{0}";
        public override string Tutorial_CollectXAmountOfY => "收集{0}{1}";
        public override string Tutorial_SelectTabX => "选择标签: {0}";
        public override string Tutorial_IncreasePriorityOnX => "提高{0}的优先级";
        public override string Tutorial_PlaceBuildOrder => "下达建筑指令: {0}";
        public override string Tutorial_ZoomInput => "缩放";

        public override string Tutorial_SelectACity => "选择一座城市";
        public override string Tutorial_ZoomInWorkers => "放大查看工人";
        public override string Tutorial_CreateSoldiers => "使用此装备创建两支士兵单位: {0}。{1}。";
        public override string Tutorial_ZoomOutOverview => "缩小，查看地图概况";
        public override string Tutorial_ZoomOutDiplomacy => "缩小，查看外交视图";
        public override string Tutorial_ImproveRelations => "改善与你邻近派系的关系";
        public override string Tutorial_MissionComplete_Title => "任务完成！";
        public override string Tutorial_MissionComplete_Unlocks => "新功能已解锁";

        //patch1
        public override string Resource_ReachedStockpile => "达到库存目标缓冲";

        public override string BuildingType_ResourceMine => "{0}矿";

        public override string Resource_TypeName_BogIron => "沼泽铁";

        public override string Resource_TypeName_Coal => "煤炭";

        public override string Language_XUpkeepIsY => "{0} 维护费用：{1}";
        public override string Language_XCountIsY => "{0} 计数：{1}";

        public override string Message_ArmyOutOfFood_Text => "将从黑市购买昂贵的食物。当你的钱用完时，饥饿的士兵将会逃离。";

        public override string Info_ArmyFood => "军队将从最近的友好城市补充食物。可以从其他派系购买食物。在敌对区域，食物只能从黑市购买。";

        public override string FactionName_Monger => "贩夫";
        public override string FactionName_Hatu => "哈图";
        public override string FactionName_Destru => "德斯特鲁";

        //patch2
        public override string Tutorial_BuildSomething => "建造一个可以生产 {0} 的设施";
        public override string Tutorial_BuildCraft => "为 {0} 建造一个工艺站";
        public override string Tutorial_IncreaseBufferLimit => "增加 {0} 的缓冲区限制";

        /// <summary>
        /// 0: count, 1: item type
        /// </summary>
        public override string Tutorial_CollectItemStockpile => "达到 {0} {1} 的库存量";
        public override string Tutorial_LookAtFoodBlueprint => "查看食物蓝图";
        public override string Tutorial_CollectFood_Info1 => "工人会走到市政厅吃东西";
        public override string Tutorial_CollectFood_Info2 => "军队派遣支援工人收集食物";
        public override string Tutorial_CollectFood_Info0 => "想完全控制工人？将所有工作优先级设为零，然后一次激活一个。";

        public override string EndGameStatistics_DecorsBuilt => "建造的装饰：{0}";
        public override string EndGameStatistics_StatuesBuilt => "建造的雕像：{0}";



        //############
        // XMAS UPDATE
        //############
        public override string Info_FoodAndDeliveryLocation => "默认情况下，工人会去市政厅吃饭或放置物品";
        public override string GameMenu_UseSpeedX => "{0} 速度选项";
        public override string GameMenu_LongerBuildQueue => "扩展建造队列";

        public override string Diplomacy_RelationWithOthers => "与他人的关系";
        public override string Automation_queue_description => "队列空前将持续重复";

        public override string BuildingType_Storehouse_Description => "工人可以在这里放置物品";

        public override string Resource_TypeName_Longbow => "长弓";
        public override string Resource_TypeName_Rapeseed => "油菜籽";
        public override string Resource_TypeName_Hemp => "大麻";

        public override string Resource_BogIronDescription => "采矿比使用沼泽铁更有效率。";

        public override string Resource_FoodSafeGuard_Description => "安全防护。如果食品生产链的优先级降低至 {0} 以下，将最大化优先级。";
        public override string Resource_FoodSafeGuard_Active => "安全防护已激活。";

        public override string GameMenu_NextSong => "下一首歌";

        public override string BuildingType_Bank => "银行";
        public override string BuildingType_GoldDelivery_Description => "向其他城市发送金币";

        public override string BuildingType_Logistics => "物流";
        public override string BuildingType_Logistics_Description => "提升您的建筑指令能力";

        public override string BuildingType_Logistics_NationSizeRequirement => "国家总劳动力：{0}";
        public override string Requirements_XItemStorageOfY => "{0} 城市存储：{1}";

        public override string XP_UnlockBuildQueue => "解锁建造队列至：{0}";
        public override string XP_UnlockBuilding => "解锁建筑：";
        public override string XP_Upgrade => "升级";

        public override string XP_UpgradeBuildingX => "升级建筑：{0}";

        public override string BuildHud_PerCycle => "每周期";
        public override string BuildHud_MayCraft => "可能制作";
        public override string BuildHud_WorkTime => "工作时间：{0}";
        public override string BuildHud_GrowTime => "成长时间：{0}";
        public override string BuildHud_Produce => "生产：";

        public override string BuildHud_Queue => "允许建造队列：{0}/{1}";

        public override string LandType_Flatland => "平原";
        public override string LandType_Water => "水域";
        public override string BuildingType_Wall => "墙";
        public override string Delivery_AutoReciever_Description => "将发送至资源最少的城市";

        public override string Hud_On => "开";
        public override string Hud_Off => "关";

        public override string Hud_Time_Seconds => "{0} 秒";
        public override string Hud_Time_Minutes => "{0} 分钟";
        public override string Hud_Undo => "撤销";
        public override string Hud_Redo => "重做";

        public override string Tag_ViewOnMap => "在地图上查看标签";

        public override string MenuTab_Tag => "标签";

        public override string Input_Build => "建造";

        public override string FlagEditor_ClearAll => "清除所有";

        public override string CityCulture_Stonemason => "石匠";
        public override string CityCulture_Stonemason_Description => "改善石材采集";

        public override string CityCulture_Brewmaster => "酿酒师";
        public override string CityCulture_Brewmaster_Description => "增强啤酒生产";

        public override string CityCulture_Weavers => "织工";
        public override string CityCulture_Weavers_Description => "增强轻甲生产";

        public override string CityCulture_SiegeEngineer => "攻城工程师";
        public override string CityCulture_SiegeEngineer_Description => "更强大的攻城机器";

        public override string CityCulture_Armorsmith => "铠甲匠";
        public override string CityCulture_Armorsmith_Description => "改善铁甲生产";

        public override string CityCulture_Noblemen => "贵族";
        public override string CityCulture_Noblemen_Description => "更强大的骑士";

        public override string CityCulture_Seafaring => "航海";
        public override string CityCulture_Seafaring_Description => "海上特化的士兵拥有更强的船只";

        public override string CityCulture_Backtrader => "背后交易者";
        public override string CityCulture_Backtrader_Description => "更便宜的黑市";

        public override string CityCulture_LawAbiding => "遵纪守法";
        public override string CityCulture_LawAbiding_Description => "增加税收。没有黑市。";




        public override string Hud_Advanced => "高级";
        public override string Hud_Loading => "加载中...";

        public override string CityOption_LowerGuardSize => "释放守卫";
        public override string Hud_Purchase_MinCapacity => "已达到最小容量";
        public override string Settings_ResetToDefault => "恢复默认设置";
        public override string Settings_NewGame => "新游戏";

        public override string Settings_AdvancedGameSettings => "高级游戏设置";
        public override string Settings_FoodMultiplier => "食物倍数";
        public override string Settings_FoodMultiplier_Description => "工人或士兵在饱腹状态下能持续的时间。较高的值会降低计算机性能。";

        public override string Settings_GameMode => "游戏模式";

        public override string Settings_Mode_Story => "完整故事";
        public override string Settings_Mode_IncludeBoss => "包括Boss事件。";
        public override string Settings_Mode_IncludeAttacks => "包括随机攻击。";
        public override string Settings_Mode_Sandbox => "沙盒";
        public override string Settings_Mode_Peaceful => "和平";
        public override string Settings_Mode_Peaceful_Description => "所有战争都由玩家发起";

        public override string Lobby_ImportSave => "导入存档";

        public override string Lobby_ExportSave => "导出存档";
        public override string Lobby_ExportSave_Description => "创建文件的副本，并将其放置在导入文件夹中：{0}";

        public override string Resource_CurrentAmount => "当前数量：{0}";
        public override string Resource_MaxAmount_Soft => "软上限（最大限制）：{0}";
        public override string Resource_MaxAmount => "最大限制：{0}";
        public override string Resource_AddPerSec => "增加速率：每秒{0}";

        public override string Resource_WaterAddLimit => "水的增加速率不能改变";

        public override string Tutorial_Select_SubTab => "并选择分类：{0}";



        /* #### --------------- ##### */
        /* #### DSS 2 DEMO      ##### */
        /* #### --------------- ##### */

        public override string Tutorial_OpenGuardSubTab => "打开兵营并选择类别：{0}";
        public override string Tutorial_GuardToWall => "将守卫移动到城墙上";
        public override string Demo_MissionObjective_Title => "任务目标";
        public override string Demo_MissionObjective_Description => "抵御来自南方的进攻";
        public override string Demo_Complete_Title => "演示完成";
        public override string Demo_TimesUp_Title => "时间到！";
        public override string Demo_EndInOneMinuteDescription => "演示将在一分钟后结束";

        public override string ArmyOption_NewArmy => "新军队";
        public override string ProfileEditor_AltMain => "备用主角";
        public override string Automation_CheckBoxTitle => "自动化";

        public override string ArmyStructure_ColumnWidth => "军队列宽";
        public override string ArmyStructure_ArmyPlacement => "军队中的位置";
        public override string ArmyStructure_Row_Front => "前排";
        public override string ArmyStructure_Row_Body => "中排";
        public override string ArmyStructure_Row_Second => "第二排";
        public override string ArmyStructure_Row_Behind => "后排";

        public override string Diplomacy_RelationType_Enemies => "敌人";

        public override string EventMessage_EnemyAlliance_Title => "对统治的恐惧";
        public override string EventMessage_EnemyAlliance => "由于担心你日益增长的力量，各国联合组成了反对你的联盟。";

        public override string Settings_CentralGold => "中央金币";
        public override string Settings_CentralGold_Description => "开启：所有金币集中共享，可立即使用。关闭：金币为实体资源，需要运输。";

        public override string InputActionName_StopStart => "开始/停止";
        public override string InputActionName_ToggleHudDetail => "切换 HUD 详情";
        public override string InputActionName_NextCity => "下一个城市";
        public override string InputActionName_NextArmy => "下一支军队";
        public override string InputActionName_NextBattle => "下一场战斗";
        public override string InputActionName_Build => "建造";
        public override string InputActionName_Copy => "复制";
        public override string InputActionName_Paste => "粘贴";
        public override string InputActionName_Menu => "菜单";
        public override string InputActionName_FlagDesign_ToggleColor_Prev => "上一个颜色";
        public override string InputActionName_FlagDesign_ToggleColor_Next => "下一个颜色";
        public override string InputActionName_FlagDesign_PaintBucket => "油漆桶";
        public override string InputActionName_Controller_FlagDesign_Colorpicker => "颜色选择器";
        public override string InputActionName_ControllerFocus => "聚焦";
        public override string InputActionName_ControllerCancel => "取消";
        public override string InputActionName_ControllerMessageClick => "点击消息";
        public override string InputActionName_ControllerSelect => "选择";
        public override string InputActionName_WASD_UP => "上";
        public override string InputActionName_WASD_DOWN => "下";
        public override string InputActionName_WASD_LEFT => "左";
        public override string InputActionName_WASD_RIGHT => "右";
        public override string InputActionName_CameraTiltLeft => "摄像机向左倾斜";
        public override string InputActionName_CameraTiltRight => "摄像机向右倾斜";
        public override string InputActionName_CameraTiltUp => "摄像机向上倾斜";
        public override string InputActionName_ZoomInKey => "放大";
        public override string InputActionName_ZoomOutKey => "缩小";

        public override string Settings_Title_Monitor => "显示器设置";
        public override string Settings_Title_Graphics => "图形设置";
        public override string Settings_Title_Input => "输入设置";
        public override string Settings_Title_Gameplay => "游戏设置";
        public override string Settings_PanOnZoom => "缩放时平移";
        public override string Settings_ScrollSensitivity_Game => "滚动灵敏度：游戏";
        public override string Settings_ScrollSensitivity_Menu => "滚动灵敏度：菜单";
        public override string Settings_Blood => "血液效果";

        public override string Settings_MasterVolume => "主音量";
        public override string Settings_AmbienceVolume => "环境音量";
        public override string Settings_BattleMelody => "战斗旋律";

        public override string Settings_ModelLight => "模型光照效果";
        public override string Settings_Particles => "粒子效果";
        public override string Settings_MapLoadSpeed => "地图加载速度";
        public override string Lobby_Category_Options => "选项";
        public override string Lobby_Category_Editor => "编辑器";
        public override string Lobby_Category_ExtraModes => "额外模式";

        public override string Lobby_Editor_MapEditor => "地图编辑器";
        public override string Lobby_Editor_VoxelEditor => "体素编辑器";

        public override string Lobby_Mode_BattleLab => "战斗实验室";
        public override string Lobby_Mode_BattleLab_Description => "让任意士兵互相对抗";
        public override string Lobby_Mode_Commander => "指挥官模式";
        public override string Lobby_Mode_Commander_Description => "一个小型战术桌游模式";
        public override string Lobby_MusicPlayList => "音乐播放列表";

        public override string Lobby_GameSetup => "游戏设置";
        public override string Lobby_PlayerSetup => "玩家设置";
        public override string LobbyDemoMode_Demo => "演示";

        public override string Lobby_Tutorial => "教程";

        public override string LobbyDemoMode_ShortTutorial => "快速教程";
        public override string LobbyDemoMode_LongTutorial => "扩展教程";

        /// <summary>
        /// Says wishlist on, followed by the STEAM logo
        /// </summary>
        public override string LobbyDemoMode_WishlistOn => "已加入愿望单";

        public override string BattleLab_StartHere => "从这里开始战斗";
        public override string BattleLab_Start => "开始战斗";
        public override string BattleLab_Attacker => "进攻方";

        public override string MapGenerator_Name => "地图编辑器 - 生成";

        public override string MapType_CustomMap => "自定义地图";
        public override string MapType_GenerateNewMap => "生成新地图";
        public override string MapGenerator_GenerateAction => "生成";
        public override string MapGenerator_Terrain_CustomSize => "自定义大小";
        public override string MapGenerator_Terrain_StartAs => "开始为";
        public override string MapGenerator_Terrain_ClearPass => "执行清除流程";
        public override string MapGenerator_Terrain_BuildPass => "执行建造流程";
        public override string MapGenerator_Terrain_DigPass => "执行挖掘流程";
        public override string MapGenerator_Terrain_BuildDigLoops => "建造-挖掘循环次数";
        public override string MapGenerator_Terrain_BuildStrokes => "建造笔触数量";
        public override string MapGenerator_Terrain_BuildStrokes_Description => "以每100格的笔触次数计量";
        public override string MapGenerator_Terrain_DigStrokes => "挖掘笔触数量";
        public override string MapGenerator_Terrain_CleanUp_Option => "清理单个格子";
        public override string MapGenerator_Terrain_CleanUpPass => "执行清理流程";

        public override string Economy_ServicemenUpkeep => "服务人员维护费：{0}";
        public override string Economy_ServicemenUpkeep_Description => "每位服务人员的维护费用为 {0} 金币";
        public override string Economy_GuardUpkeep_Description => "每位守卫的维护费用为 {0} 金币";

        public override string EndScreen_TimeHasEndedTitle => "时间到";
        public override string Hud_AdvancedSettings => "高级设置";
        public override string Hud_Vector_X => "X";
        public override string Hud_Vector_Y => "Y";
        public override string Hud_Cancel => "取消";
        public override string Hud_Delete => "删除";
        public override string Hud_Next => "下一步";
        //public override string Hud_None => "无";
        public override string Hud_Apply => "应用";
        public override string Hud_AllCities => "所有城市";
        public override string Hud_Time_Hours => "{0} 小时";
        public override string Hud_AddX => "添加 {0}";
        public override string Hud_Both => "两个";
        public override string Hud_Direction => "方向";
        
        /// <summary>
        /// 0: 对象类型名称，1: 对象数量
        /// </summary>
        public override string Hud_ObjectsAndCount => "{0}，数量：{1}";

        public override string Hud_EffectDoesNotStack => "该效果不可叠加";

        public override string Work_SmeltX => "熔炼 {0}";

        public override string Info_TotalFoodProduction => "总食物产量";
        public override string Info_TotalFoodSpending => "总食物消耗";

        public override string Info_FooodAndDeliveryLocation => "默认情况下，工人会前往市政厅用餐或投递物品";

        public override string Delivery_SendChunk => "每次配送的物品数量";
        public override string Delivery_SpeedBonus => "速度加成：{0}%";

        public override string Delivery_AutoResourceDescription => "将达到库存上限的物资送往有需求的城市";

        public override string Conscript_Soldiers_ArmyType => "军队士兵";
        public override string Conscript_Soldiers_ArmyType_Description => "招募士兵到相邻军队";
        public override string Conscript_Soldiers_GuardType => "城市守卫";
        public override string Conscript_Soldiers_GuardType_Description => "守卫用于加固城墙";

        public override string Defence_Title => "防御";
        public override string Defence_GuardPost => "警戒哨所";

        public override string Defence_WallDescription_Movement => "阻碍敌人移动。";
        public override string Defence_WallDescription_GuardPost => "可在此驻守守卫。";
        public override string Defence_AutoAssign => "自动分配";
        public override string Defence_AutoAssign_Description => "新守卫将自动前往此岗位";

        public override string Conscript_SplashDamage => "范围伤害";
        public override string Conscript_HighSplashDamage => "高范围伤害";

        public override string Conscript_Training_Champion => "冠军";
        public override string Conscript_Training_Legendary => "传奇";

        public override string Experience_Title => "经验";
        public override string Experience_TopExperience => "最高经验等级";

        public override string Experience_TimeReductionDescription => "每提升一级，工作时间减少 {0}%";

        public override string ExperienceType_Farm => "农夫";
        public override string ExperienceType_AnimalCare => "畜牧";
        public override string ExperienceType_HouseBuilding => "建房工";
        public override string ExperienceType_WoodWork => "木工";
        public override string ExperienceType_StoneCutter => "石匠";
        public override string ExperienceType_Mining => "矿工";
        public override string ExperienceType_Transport => "运输工";
        public override string ExperienceType_Cook => "厨师";
        public override string ExperienceType_Fletcher => "制箭匠";
        public override string ExperienceType_RefineOre => "炼矿工";
        public override string ExperienceType_Casting => "铸造工";
        public override string ExperienceType_CraftMetal => "铁匠";
        public override string ExperienceType_CraftArmor => "护甲匠";
        public override string ExperienceType_CraftWeapon => "武器匠";
        public override string ExperienceType_CraftFuel => "制炭工";
        public override string ExperienceType_Chemist => "炼金术士";

        public override string ExperienceLevel_1 => "初学者";
        public override string ExperienceLevel_2 => "熟练者";
        public override string ExperienceLevel_3 => "专家";
        public override string ExperienceLevel_4 => "大师";
        public override string ExperienceLevel_5 => "传奇";

        public override string ExperenceOrDistancePrio_Title => "工人选择";
        public override string ExperenceOrDistancePrio_Description => "空闲工人将根据距离或经验进行分配";

        public override string Technology_Description => "每座城市拥有一个科技树。每项科技都能解锁建筑和物品。";
        public override string Experience_Description => "工人将获得经验并逐渐成长";

        public override string Technology_Title => "科技";
        public override string Technology_ShareField => "共享科技领域";

        public override string Technology_GainByNeigborRelation => "每个拥有该科技的邻近城市，且关系为 {0}：{1}";
        public override string Technology_ForEachMaster => "当一名 {0} 达到经验等级 {1} 时，在科技领域：{2}";
        public override string Technology_CitySpread => "当城市相邻时，它们将共享科技：{0}";
        public override string Technology_CityCapture => "城市在战斗中被攻陷时，大部分科技将会丢失";

        public override string Technology_AdvancedBuildings => "高级建筑技术";
        public override string Technology_AdvancedFarming => "高级农业技术";
        public override string Technology_AdvancedCasting => "高级铸造技术";

        public override string Help_Title => "帮助";
        public override string Help_Work_Title => "工作未开始";
        public override string Help_Work_Resources => "建筑需要可用资源";
        public override string Help_Work_Skill => "工人需要具备相应或更高的技能等级";
        public override string Help_Work_Stockpile => "仓库已满会阻碍资源收集";
        public override string Help_Work_Priority => "该任务可能优先级过低或为零";

        public override string Help_Soldiers_Title => "生产士兵";
        public override string Help_Soldiers_PlaceBuildingX => "建造建筑：{0}";
        public override string Help_Soldiers_Workers => "可供招募的工人";
        public override string Help_Soldiers_Weapon => "每个士兵需要一件武器";
        public override string Help_Soldiers_StartX => "开始：{0}";

        public override string Hud_SelectHistory => "选择历史记录";

        public override string Hud_PointsPerMinute => "每分钟 {0} 分";
        public override string Hud_PercentValueCost => "该服务花费相当于价值的 {0}%";

        public override string Hud_Mixed => "混合";
        public override string Hud_Distance => "距离";

        public override string Hud_Unlock => "解锁";
        public override string Hud_category => "类别";


        /// <summary>
        /// 将游戏速度设置为单帧模式
        /// </summary>
        public override string Input_StepOneFrame => "前进1帧";

        public override string Resource_TypeName_Wagon2Wheel => "小型货车";
        public override string Resource_TypeName_Wagon4Wheel => "大型货车";
        public override string Resource_TypeName_Tin => "锡";
        public override string Resource_TypeName_TinOre => "锡矿石";

        public override string Resource_TypeName_Copper => "铜";
        public override string Resource_TypeName_CopperOre => "铜矿石";
        public override string Resource_TypeName_SilverOre => "银矿石";
        public override string Resource_TypeName_Silver => "银";

        /// <summary>
        /// 秘银是一种幻想金属
        /// </summary>
        public override string Resource_TypeName_RawMithril => "未精炼秘银";
        public override string Resource_TypeName_Mithril => "秘银";

        public override string Resource_TypeName_BronzeSword => "青铜剑";
        public override string Resource_TypeName_ShortSword => "短剑";
        public override string Resource_TypeName_LongSword => "长剑";
        public override string Resource_TypeName_HandSpear => "手持长矛";
        public override string Resource_TypeName_Warhammer => "战锤";
        public override string Resource_TypeName_MithrilSword => "秘银剑";
        public override string Resource_TypeName_SlingShot => "弹弓";
        public override string Resource_TypeName_ThrowingSpear => "投矛";
        public override string Resource_TypeName_Crossbow => "弩";
        public override string Resource_TypeName_MithrilBow => "秘银弓";

        public override string Resource_TypeName_CoolingFluid => "冷却液";
        public override string Resource_TypeName_Palisade => "木栅";
        public override string Resource_TypeName_Toolkit => "工具包";

        public override string Resource_TypeName_Sulfur => "硫磺";
        public override string Resource_TypeName_LeadOre => "铅矿石";
        public override string Resource_TypeName_Lead => "铅";
        public override string Resource_TypeName_Bronze => "青铜";
        public override string Resource_TypeName_BloomIron => "熟铁";
        public override string Resource_TypeName_Steel => "钢";
        public override string Resource_TypeName_CastIron => "生铁";

        public override string Resource_TypeName_BlackPowder => "黑火药";
        public override string Resource_TypeName_GunPowder => "火药";
        public override string Resource_TypeName_LedBullet => "子弹";

        public override string Resource_TypeName_HandCannon => "手持火炮";
        public override string Resource_TypeName_HandCulverin => "手持长管火枪";
        public override string Resource_TypeName_Rifle => "步枪";
        public override string Resource_TypeName_Blunderbuss => "火铳";

        public override string Resource_TypeName_Manuballista => "手动弩炮";
        public override string Resource_TypeName_Catapult => "投石机";
        public override string Resource_TypeName_BatteringRam => "攻城锤";
        public override string Resource_TypeName_SiegeCannonBronze => "青铜巨炮";
        public override string Resource_TypeName_ManCannonBronze => "青铜火炮";
        public override string Resource_TypeName_SiegeCannonIron => "铁质重炮";
        public override string Resource_TypeName_ManCannonIron => "铁制火炮";

        public override string Resource_TypeName_PaddedArmor => "软甲";
        public override string Resource_TypeName_HeavyPaddedArmor => "重型软甲";

        public override string Resource_TypeName_IronArmor => "锁子甲";
        public override string Resource_TypeName_HeavyIronArmor => "重型锁子甲";

        public override string Resource_TypeName_BronzeArmor => "青铜甲";

        public override string Resource_TypeName_LightPlateArmor => "板甲";
        public override string Resource_TypeName_FullPlateArmor => "全身板甲";
        public override string Resource_TypeName_MithrilArmor => "秘银甲";
        public override string Resource_TypeName_Coin => "金币";

        public override string UnitType_Warhammer => "战锤骑士";
        public override string UnitType_SpearAndShield => "枪盾兵";

        public override string UnitType_CollectionOfSoldiers => "士兵集群";
        public override string UnitType_CollectionOfArmies => "军队集群";

        /// <summary>
        /// ID标签为唯一编号
        /// </summary>
        public override string UnitId => "（编号 {0}）";

        public override string BuildHud_AreaEffectTitle => "区域效果";
        public override string BuildHud_BonusRadius => "加成范围：{0}";

        public override string BuildHud_BuildTime => "建造时间";
        public override string SchoolHud_ToLevel => "升级所需";
        public override string SchoolHud_TimeDescription => "时间基于零经验，经验越高所需时间越短。";
        public override string SchoolHud_SelectSchool => "选择学院";
        public override string Upgrade_Order => "升级顺序";

        public override string Building_ListDescription => "该类别中所有建筑的列表";

        public override string BuildingType_IsUpgraded => "{0} - 已升级";
        public override string BuildingType_WoodCutter => "伐木场";
        public override string BuildingType_Workshop_Description => "提升周围地区的工作效率";

        public override string BuildingType_WoodCutter_AreaAffect => "从树木获取木材提升 {0}%";

        public override string BuildingType_StoneCutter_AreaAffect => "获取石材提升 {0}%";

        public override string BuildingType_StoneCutter => "采石场";

        public override string BuildingType_Embassy => "大使馆";
        public override string BuildingType_Embassy_Description => "用于处理外交关系";

        public override string BuildingType_SoldierBarracks => "士兵兵营";
        public override string BuildingType_ArcherBarracks => "弓兵兵营";
        public override string BuildingType_WarmachineBarracks => "战争机器兵营";
        public override string BuildingType_GunBarracks => "火枪兵兵营";
        public override string BuildingType_CannonBarracks => "火炮兵兵营";
        public override string BuildingType_KnightsBarracks => "骑士兵营";

        public override string BuildingType_WaterResovoir => "蓄水池";
        public override string BuildingType_WaterResovoir_Description => "提高水的储存量";

        public override string BuildingType_SmeltingFurnace => "冶炼炉";
        public override string BuildingType_SmeltingFurnace_Description => "将矿石提炼为金属";

        public override string BuildingType_Foundry => "铸造厂";
        public override string BuildingType_Foundry_Description => "金属铸造车间";

        public override string BuildingType_Armory => "护甲工坊";
        public override string BuildingType_Armory_Description => "用于制造盔甲的设施";

        public override string BuildingType_Chemist => "炼金工坊";
        public override string BuildingType_Chemist_Description => "用于制造化学材料的设施";

        public override string BuildingType_CoinMaker => "造币厂";
        public override string BuildingType_CoinMaker_Description => "将金属转换为货币";

        public override string BuildingType_Gunmaker => "火器工坊";
        public override string BuildingType_Gunmaker_Description => "制造火枪与大炮的场所";

        public override string BuildingType_School_Tab => "学院";
        public override string BuildingType_School => "工匠公会";
        public override string BuildingType_School_Description => "提高工人的技能等级";

        public override string BuildingType_GoldDelivery => "金币运送站";
        public override string BuildingType_Bank_Description => "金币管理中心";

        public override string DecorType_CobbleStones => "鹅卵石路";
        public override string DecorType_Square => "城市广场";

        public override string DecorType_Garden => "花园";
        public override string DecorType_Flag => "旗帜";
        public override string DecorType_Banner => "横幅";

        public override string BuildingType_DirtRoad => "泥土路";
        public override string BuildingType_Palisade => "木栅要塞";

        public override string ResourceType_ServiceMen => "服务人员";
        public override string BuildingType_ServiceHouse => "服务小屋";
        public override string BuildingType_ServiceHouse_DescriptionAddX => "增加服务人员：{0}";

        public override string BuildingType_GuardOffice => "守卫室";
        public override string BuildingType_GuardOffice_DescriptionAddX => "守卫上限增加：{0}";

        public override string BuildingType_DirtWall => "土墙";
        public override string BuildingType_DirtTower => "土塔";
        public override string BuildingType_WoodWall => "木墙";
        public override string BuildingType_WoodTower => "木塔";
        public override string BuildingType_StoneWall => "石墙";
        public override string BuildingType_StoneTower => "石塔";
        public override string BuildingType_StoneGate => "石门";
        public override string BuildingType_StoneHouse => "石屋";


        /// <summary>
        /// 用于列出轻微变化的名称，例如“灯A”“灯B”
        /// </summary>
        public override string VariantType_A => "{0} A";
        public override string VariantType_B => "{0} B";
        public override string VariantType_C => "{0} C";
        public override string VariantType_D => "{0} D";
        public override string VariantType_E => "{0} E";
        public override string VariantType_F => "{0} F";
        public override string VariantType_G => "{0} G";
        public override string VariantType_H => "{0} H";

        public override string BuildingToolShape_Free => "自由绘制";
        public override string BuildingToolShape_Area => "矩形";
        public override string BuildingToolShape_Line => "直线";
        public override string BuildingToolShape_LShape => "L形";

        public override string CityHall_Upgrade => "升级市政厅";

        /// <summary>
        /// 城市可支持的最大工人数上限
        /// </summary>
        public override string CityHall_MaxSupportedWorkers => "最大可支持工人数量：{0}";

        public override string CityHall_Size_Small => "村庄";
        public override string CityHall_Size_Medium => "城镇";
        public override string CityHall_Size_Large => "都城";

        public override string GuardHousingCount => "守卫宿舍容量";
        public override string ServicemenCount => "服务人员：{0}";

        public override string Work_MiningResource => "开采 {0}";

        public override string MenuTab_Progress => "发展进度";

        public override string Automation_AutomateCity => "自动化城市";
        public override string Automation_AutomationFocus => "自动化方向";
        public override string Automation_AutomationFocus_Grow => "发展";
        public override string Automation_AutomationFocus_Export => "出口";
        public override string Automation_AutomationFocus_War => "战争";

        public override string CityCulture_Smelters_Description => "提升矿石的冶炼效率";
        public override string CityCulture_Smelters => "炼金工";

        public override string CityCulture_Apprentices_Description => "新工人可从现有工人获得经验";
        public override string CityCulture_Apprentices => "学徒";

        public override string CityCulture_BronzeCasters_Description => "提高青铜及其制品的产量";
        public override string CityCulture_BronzeCasters => "青铜铸造师";

        //DEMO PATCH 1
        /// <summary>
        /// 在地图上游荡的邪恶兽人
        /// </summary>
        public override string FactionName_Barbarian => "黑暗部落";
        public override string Tutorial_AttackAndDestroyX => "攻击并摧毁：{0}";
        public override string Resource_TypeName_Pike => "长矛";

        public override string BattleTrials_Title => "战斗试炼";
        public override string BattleTrials_Description => "在正面对决中检验你的战术策略。";
        //DEMO PATCH 2
        public override string Conscript_BlockReducingAttack => "这些攻击会降低格挡几率";

        public override string Conscript_BlockPerSecond => "每秒最多可格挡 {0} 次";

        public override string Conscript_BlockDescription => "士兵会格挡来自前方扇形区域的大部分攻击";

        public override string Map_CustomSeed => "地图种子";

        public override string Settings_Mode_Spectator => "观战模式";

        //public override string Settings_Mode_Spectator_Description => "仅观看";

        public override string Automation_AutomationFocus_NoFocus_Description => "会平均建造各种设施";

        public override string Automation_AutomationFocus_WillProduce => "主要生产：";

        public override string Help_Food_WhoEats => "所有士兵和工人都会消耗食物";

        public override string Help_Food_BigArmy => "庞大的军队可能会让所在区域的城市陷入饥荒";

        public override string Help_Food_DontBuild => "建造更多农场并不会自动增加食物产量；你需要有空闲工人和烹饪站来收集和处理食物";

        public override string Help_Food_UseWater => "食物生产需要水";

        public override string Help_Food_Postal => "确保城市之间通过运送食物相互支援";

        public override string Message_LostCity => "城市已失守";

        public override string Demo_Description => "简短场景：守住你的城市 {0} 分钟";

        //DEMO PATCH 3
        public override string Demo_EndInXMinuteDescription => "演示将在 {0} 分钟后结束";

        public override string Experience_Required => "所需经验";

        public override string InputActionName_ToggleMenu => "切换菜单";

        //DEMO PATCH 4
        public override string Work_BadValueDescription => "资源可能会低于零，并略微超过库存上限。这些限制仅在创建工作队列时生效。";

        public override string Work_SelectCategory => "选择物品类别";
        public override string Hud_RemoveFromList => "从列表中移除";

        public override string Hud_ReturnToPrevious => "返回";
        public override string Hud_Close => "关闭";

        public override string Hud_Low => "低";
        public override string Hud_Medium => "中";
        public override string Hud_High => "高";

        public override string Hud_Copy => "复制";
        //public override string Hud_Paste => "粘贴";
        public override string Hud_Cut => "剪切";
        public override string Hud_SaveCompleted => "保存完成";

        public override string Settings_WaterMultiplier => "水量倍率";
        public override string Settings_WaterMultiplier_Description => "决定城市生产和储存水的数量。数值越高，性能越差。";

        public override string Settings_ChildMultiplier => "生育倍率";
        public override string Settings_CraftMultiplier_Description => "数值越低，生产越快。";

        public override string FastProduction => "快速生产";
        public override string SlowProduction => "缓慢生产";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public override string BlocksProduction => "不生产";

        public override string Automation_AutomationFocus_NoFocus => "全部";
        public override string CityAutomation_SoldierQuality => "士兵质量";
        public override string CityAutomation_SoldierWeaponType => "武器类型";

        public override string WarsResourceGroup_Resources => "资源";
        public override string WarsResourceGroup_Weapons => "武器";

        public override string WarsResourceGroup_AllWeaponTypes => "混合";
        public override string WarsResourceGroup_MeleeHandWeapons => "近战";
        public override string WarsResourceGroup_RangedHandWeapons => "远程";
        public override string WarsResourceGroup_Warmachines => "战争机器";

        public override string FactionSettings_Titel => "阵营设置";
        public override string FactionSettings_Description => "适用于你所有的城市";

        public override string Conscript_MaxPopulation => "最大人口";
        public override string Conscript_MaxPopulation_Description => "仅当人口达到上限时征兵";

        public override string Conscript_FoodAbundance => "最大食物储量";
        public override string Conscript_FoodAbundance_Description => "仅当食物达到最大库存时征兵";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public override string GeneralSetting_On => "设置为：开";
        public override string GeneralSetting_Off => "设置为：关";
        public override string GeneralSetting_AllBuildingsDescription => "将应用于所有建筑";

        public override string GeneralSetting_ApplyMessage => "更改已应用于 {0} 个建筑";

        public override string MustTurnOffSteamInput => "要使用控制器，必须关闭 Steam Input。";

        public override string Technology_GainTitle => "获取科技的方式";
        public override string Technology_LevelUp => "升级";
        public override string Technology_ForEachLevelUp => "当工人在科技领域升级时：{0}";

        public override string VoxelEditor_Description => "创建像素风模型";

        public override string Editor_Tool => "工具";
        public override string Editor_SelectOptionsMenu => "选择选项";
        public override string Editor_Continous => "连续";

        public override string Editor_Tool_PencilSize => "画笔大小";
        public override string Editor_Tool_SizeTolerance => "尺寸容差";
        public override string Editor_Tool_RoundPencil => "圆形画笔";
        public override string Editor_Tool_EdgeSize => "边缘大小";
        public override string Editor_Tool_PercentFill => "填充百分比";
        public override string Editor_Tool_ClearAbove => "清除上方";
        public override string Editor_Tool_FillBelow => "填充下方";

        public override string Editor_UserModels => "用户模型";
        public override string Editor_UserModels_Description => "浏览你保存的模型";

        public override string Editor_RetailModels => "游戏模型";
        public override string Editor_RetailModels_Description => "从游戏中加载模型";

        public override string Editor_ModTemplates => "Mod 模板";
        public override string Editor_ExportAsOBJ => "导出为 .OBJ";
        public override string Editor_SelectAll => "全选";

        public override string Editor_Canvas_Title => "画布";
        public override string Editor_Canvas_Size => "尺寸";
        public override string Editor_Canvas_Dimension_X => "X";
        public override string Editor_Canvas_Dimension_Y => "Y";
        public override string Editor_Canvas_Dimension_Z => "Z";
        public override string Editor_Canvas_SizePresets => "尺寸预设";
        public override string Editor_Canvas_Move => "移动";
        public override string Editor_Canvas_Move_Up => "上移";
        public override string Editor_Canvas_Move_Down => "下移";
        public override string Editor_Canvas_RotateClockwise => "顺时针旋转";
        public override string Editor_Canvas_RotateCounterClockwise => "逆时针旋转";
        public override string Editor_Canvas_Mirror => "镜像";

        public override string Editor_Canvas_RotateFlip_Title => "旋转/翻转";
        public override string Editor_Canvas_FlipVertical => "上下翻转";
        public override string Editor_Canvas_FlipOrientation => "切换横/竖方向";
        public override string Editor_Canvas_ClearAll_Description => "清除所有方块和帧";

        public override string Editor_Animation => "动画";
        public override string Editor_Animation_RemoveCurrentFrame => "删除当前帧";
        public override string Editor_Animation_AddFrameCopy => "添加帧副本";
        public override string Editor_Animation_AddEmptyFrame => "添加空帧";
        public override string Editor_Animation_MoveDescription => "更改帧位置";
        public override string Editor_Animation_AllFrames => "全部帧";
        public override string Editor_Animation_AllFrames_ActionDescription => "对所有帧执行相同操作";

        public override string Editor_SettingsMenu => "设置";
        public override string Hud_Exit => "退出";
        public override string Editor_Canvas_Clear => "清空";

        public override string Editor_Stamp => "图章";
        public override string Editor_StampOtherFrames => "应用于其他帧";
        public override string Editor_StampOtherFrames_Description => "将体素粘贴到其他帧中";
        public override string Editor_PasteToFrame => "粘贴到当前帧";
        public override string Editor_ClearAllFrames => "清除所有帧";
        public override string Editor_ClearOtherFrames => "清除其他帧";

        public override string Editor_Settings_MoveSpeed => "移动速度";
        public override string Editor_Settings_BackgroundColor => "背景颜色";
        public override string Editor_Settings_HideHUD => "隐藏 HUD";

        public override string Editor_Color => "颜色";
        public override string Editor_ColorsInUseLabel => "正在使用的颜色";
        public override string Editor_Color_BrighterPlus => "更亮 +";
        public override string Editor_Color_Brighter => "更亮";
        public override string Editor_Color_Darker => "更暗";
        public override string Editor_Color_DarkerPlus => "更暗 +";
        public override string Editor_Color_RedTint => "红色色调";
        public override string Editor_Color_Tint => "色调";
        public override string Editor_Color_GreenTint => "绿色色调";
        public override string Editor_Color_BlueTint => "蓝色色调";
        public override string Editor_Color_YellowTint => "黄色色调";
        public override string Editor_Color_PurpleTint => "紫色色调";
        public override string Editor_NoColor => "空";

        public override string Editor_Material => "材质";

        /// <summary>
        /// User may change one color to another across the model
        /// </summary>
        public override string Editor_Color_Recolor => "重新上色";
        public override string Editor_Color_RecolorTo => "替换为";

        public override string Editor_Material_Set => "设置材质";

        public override string Editor_Preview => "预览";
        public override string Editor_CombineWithCurrent => "与当前模型合并";

        public override string Editor_PickedColor => "已选颜色";
        public override string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";

        public override string BuildingType_ImmigrationTent => "移民帐篷";
        public override string BuildingType_ImmigrationTent_Description => "可容纳 {0} 名移民";
        public override string BuildingType_ReseachCenter => "研究中心";
        public override string BuildingType_Bookpress => "印刷机";
        public override string BuildingType_Bookpress_Description => "在一个研究领域中获得的所有点数将与你其他城市中的所有 {0} 建筑共享。";

        public override string Technology_ReseachExample => "示例：当工人生产 {0} 时，他们的 {1} 技能将提升。升级时将为 {2} 技术添加点数，因为它们共享 {1} 领域。";

        public override string BuildingType_Research_BaseDescription => "提高科技研究速度。";

        public override string BuildingType_ResearchCenter_Description => "当工人在同一领域升级时，额外增加 {0} 点科技研究值。";


        //DEMO PATCH 5
        public override string Editor_CropSelection => "裁剪到选区";

        public override string Immigrants_DisbandedSoldiers => "解散的士兵将会移民";
        public override string Immigrants_RefillWorkers => "快速补充劳动力";
        public override string Immigrants_UnhousedAreLost => "没有住所的移民将在一段时间后消失";
        public override string Editor_VoxelCount => "{0} 体素";

        public override string Editor_Layers_Titel => "图层";
        public override string Editor_Layers_All => "所有图层";
        public override string Editor_LayerNumber => "图层 {0}";

        public override string Editor_Layer_AddEmpty => "添加空图层";
        public override string Editor_Layer_AddCopy => "复制图层";
        public override string Editor_Layer_Remove => "删除图层";
        public override string Editor_Layer_MergeDown => "向下合并";
        public override string Editor_IsAnimated => "已动画化";
        public override string Editor_ToggleVisible => "切换可见性";
        public override string Editor_ToggleAnimatedLayer => "切换动画图层";
        public override string Editor_Projects => "项目文件";
        public override string ProfileEditor_ReplaceMaterial => "轮廓颜色：{0}";

        public override string ProfileEditor_ProfileColors_Label => "轮廓颜色";
        public override string ProfileEditor_TunicColor => "上衣颜色";
        public override string ProfileEditor_PantsColor => "裤子颜色";
        public override string ProfileEditor_LeaderColor => "领袖颜色";

        public override string MapStartAs_Water => "水域";
        public override string MapStartAs_Land => "陆地";
        public override string MapStartAs_Circle => "圆形";

        public override string Hud_NeedToBeAssigned => "需要分配";
        public override string Hud_CommitAssignment => "分配";
        public override string Technology_NoAvailableResearch => "没有可用的研究";

        public override string Research_Tab => "研究";

        //5.2
        public override string BuildCategory_General => "通用";
        public override string BuildCategory_Military => "军事";
        public override string BuildCategory_Decoration => "装饰";
        public override string BuildCategory_Upgrade => "升级";
        public override string Work_NoMines => "没有矿场";

        //NEXT FEST DEMO
        public override string HUD_DisplayName => "显示名称";
        public override string HUD_Filter => "筛选器";
        public override string HUD_Scale => "缩放";
        public override string HUD_Tags => "标签";
        public override string HUD_ClickToCancel => "点击取消";

        public override string ObjectTag_Description => "在地图上添加一个符号";
        public override string HudPins => "HUD 固定项";
        public override string HudPins_Description => "将信息固定在屏幕上";

        public override string Lobby_PlayerProfileNumbered => "档案 {0}";
        public override string Lobby_CharacterCreationNumbered => "角色 {0}";
        public override string Lobby_PlayerProfileEdit => "编辑玩家档案";

        public override string Editor_ConvertAnimationToLayers => "将动画转换为图层";
        public override string Editor_StampAllFrames => "应用到所有帧";

        public override string Editor_DisplayOptions => "显示选项";
        public override string Editor_CharacterCreator => "角色创建器";
        public override string Editor_CharacterCreator_Description => "军队模型外观编辑器";
        public override string Editor_HatGenre => "帽子显示模式";
        public override string Editor_HatGenre_FollowWeapon => "跟随武器";
        public override string Editor_HatGenre_Uniform => "制服";
        public override string Editor_CopyPasteSelectedColor => "从选中颜色复制";

        public override string Character_Accessories => "配件";
        public override string Character_Hat => "帽子";
        public override string Character_Head => "头部";
        public override string Character_Body => "身体";
        public override string Character_Arms => "手臂";
        public override string Character_Back => "背部";
        public override string Character_Face => "脸部";

        public override string BuildingType_Tavern => "公共大厅";

        public override string Settings_CraftMultiplier => "制作时间倍率";
        public override string Settings_ChildMultiplier_Description => "加快新工人加入的速度";

        public override string Settings_CasualControls => "休闲玩家控制";
        public override string Settings_CasualControls_Description => "通过减少选项简化玩法。仅使用金钱作为资源。";

        public override string Settings_AdvancedControls => "高级控制";
        public override string Settings_AdvancedControls_Description => "完整的资源管理体验。";

        public override string WarsResourceGroup_Metal => "金属";
        public override string Work_Craft => "制作";
        public override string Work_OnlyCraftOnFullStock => "库存满时才进行制作";

        public override string ExperienceType_Smelting => "冶炼";
        public override string Category_Optimize => "优化";
        public override string BuildCategory_Road => "道路";
        public override string XP_UnlockBuildPrio => "解锁建造优先级：{0}";
        public override string Technology_ModernFarming => "现代农业";

        public override string ExportImportDescription => "要与其他玩家共享存档，所有文件都位于此文件夹中：{0}";

        public override string CityCultureDescription => "文化将为城市提供特殊加成";

        public override string UnitType_CloseRangeRifle => "火绳枪手";
        public override string UnitType_LongRangeRifle => "滑膛枪手";
        public override string UnitType_Skirmisher => "散兵";

        //From lumen (light)
        public override string UnitType_MithrilArcher => "露娜精灵弓箭手";
        public override string UnitType_MithrilSwordsman => "露娜精灵骑士";

        public override string Defence_AutoAssign_Towers => "分配防御塔";

        public override string EventMessage_DesertersText_Food => "饥饿的士兵正在逃离你的军队";

        public override string Tutorial_CasualRecruitSoldiers => "招募一个士兵小队";


        //Shadow update

        public override string Technology_CannotReassign => "在研究完成之前，Tech 不能重新分配";
        public override string Diplomacy_DeclareWarAgainst => "你将向以下对象宣战";
        public override string Diplomacy_AllyCount => "盟友数量";
        public override string Diplomacy_CostPerAlly => "每个盟友成本增加 {0}";

        public override string Event_ChanceOfFailure => "{0}% 失败几率";
        public override string EventMessage_Event_Title => "事件";
        public override string EventMessage_TheCohalition => "联盟";

        public override string EventMessage_DarkHorde => "黑暗部落";
        public override string EventMessage_DarkHordeKiller_Title => "黑暗部落杀手";
        public override string EventMessage_DarkHordeKiller_Message => "冠军骑士加入了你的阵营";

        public override string Settings_Mode_Spectator_Description => "只观看，或者使用 God Powers 进行干预。";
        public override string GodPower => "God Power";

        public override string Building_TreeSprout_Description => "种下一棵树";
        public override string Building_TreeSprout_Soft => "软木树苗";
        public override string Building_TreeSprout_Hard => "硬木树苗";

        public override string GeneralSetting_SetAll => "应用到全部";

        public override string Hud_All => "全部";

        public override string Hud_Previous => "上一个";

        public override string Hud_EffectWillStack => "效果会叠加";

        public override string Info_WhenFoodRunsOut => "当食物耗尽时，城市和军队会自动从黑市购买。";

        //Launch test
        

        public override string InputActionName_NextWar => "下一个交战中的阵营";

        /// <summary>
        /// These symbols are needed to fit large numbers on the HUD,
        /// there will be a tooltip to explain what number it represents
        /// </summary>
        public override string EngineHud_SymbolFor100 => "百";
        public override string EngineHud_SymbolFor1000 => "千";
        public override string EngineHud_SymbolFor10000 => "万";

        /// <summary>
        /// When loading files from other players, you won’t get their achievement progress
        /// </summary>
        public override string GameMenu_BlockImportAchievements => "在导入的存档中禁用成就";

        public override string EndScreen_PeaceVictoryQuote => "让我们放下宝剑，迎接更美好的未来";

        public override string VictoryType_DefeatBoss => "Boss 击败";
        public override string VictoryType_Domination => "统治";
        public override string VictoryType_WorldPeace => "世界和平";

    }
}
