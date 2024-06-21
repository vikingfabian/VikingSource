using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class SimplifiedChinese : AbsLanguage
    {
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
        public override string Lobby_LocalMultiplayerEdit => "本地多人游戏 ({0})";

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
        public override string ProfileEditor_SaveAndExit => "保存并退出";

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

        public override string Hud_GameSpeedButton => "游戏速度";

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

        public override string Hud_IncreaseMaxGuardCount => "最大守卫数量 +{0}";

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
        public override string Hud_Immigrants => "移民: {0}";

        public override string Hud_CityCount => "城市数量: {0}";
        public override string Hud_ArmyCount => "军队数量: {0}";

        public override string Hud_Cancel => "取消";
        public override string Hud_Back => "返回";

        /// <summary>
        /// 玩家进行破坏性选择时的子菜单
        /// </summary>
        public override string Hud_AreYouSure => "你确定吗？";

        public override string Hud_OK => "确定";
        public override string Hud_Yes => "是";
        public override string Hud_No => "否";

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
        /// 描述按钮输入。移动到下一个城市。
        /// </summary>
        public override string Input_NextCity => "下一个城市";

        /// <summary>
        /// 描述按钮输入。移动到下一个军队。
        /// </summary>
        public override string Input_NextArmy => "下一个军队";

        /// <summary>
        /// 描述按钮输入。移动到下一个战斗。
        /// </summary>
        public override string Input_NextBattle => "下一个战斗";

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
        public override string CityOption_Recruit => "招募";

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

        public override string GameMenu_SaveState => "保存";
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


    }
}
