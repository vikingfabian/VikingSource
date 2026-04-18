using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_SimplifiedChinese : AbsOptionsLanguage
    {
        //Mounts
        public override string InputSteam => "Steam输入";
        public override string Input_SimulateMouse => "模拟鼠标";
        public override string Input_LockMouseToWindow => "鼠标锁定至窗口";
        public override string Input_MouseEdgePush_Title => "边缘平移";
        public override string Input_NoControl => "无";
        public override string Input_ActiveControl => "主动";
        public override string Input_PassiveControl => "被动";
        public override string Setting_MinimapScale => "小地图缩放";
        //##Settings
        public override string Settings_Particles_FadeMapLayers => "图层淡出";
        public override string SplitScreen_HorizontalFirst => "水平优先";
        public override string SplitScreen_VerticalFirst => "垂直优先";
        public override string SplitScreen_HorizontalOnly => "仅水平";
        public override string SplitScreen_VerticalOnly => "仅垂直";
        public override string SplitScreen_Title => "分屏";
        public override string SplitScreen_AdjustSplit => "调整分割 {0}";

        public override string Settings_ControllerVibration => "手柄震动";
        public override string GraphicsOption_IngameMenuWidth => "游戏内菜单宽度";
        public override string DisplayMode => "显示模式";
        public override string DisplayMode_Windowed => "窗口模式";
        public override string DisplayMode_BorderlessFullscreen => "无边框全屏";
        public override string GameSettings_RenderedMouseCursor => "渲染光标";

        public override string GameSettings_MuteControllerDisconnect => "屏蔽手柄断开提示";
        //--
        public override string GraphicsOption_FarViewDistance => "远距离视野";
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
        /// 选项菜单标题
        /// </summary>
        public override string Options_title => "选项";

        /// <summary>
        /// 游戏控制输入选项，0: 当前输入
        /// </summary>
        public override string InputSelect => "输入: {0}";

        /// <summary>
        /// 游戏输入类型
        /// </summary>
        public override string InputKeyboardMouse => "键盘和鼠标";

        /// <summary>
        /// 游戏输入类型
        /// </summary>
        public override string InputController => "控制器";

        /// <summary>
        /// 未选择游戏输入
        /// </summary>
        public override string InputNotSet => "未设置";

        /// <summary>
        /// 复选框标签。本地分屏游戏选项。
        /// </summary>
        public override string VerticalSplitScreen => "垂直分屏";

        /// <summary>
        /// 音量滑块标签
        /// </summary>
        public override string SoundOption_MusicVolume => "音乐音量";

        /// <summary>
        /// 音量滑块标签
        /// </summary>
        public override string SoundOption_SoundVolume => "音效音量";

        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public override string GraphicsOption_Resolution => "分辨率";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// 全屏显示游戏或窗口模式
        /// </summary>
        public override string GraphicsOption_Fullscreen => "全屏";

        /// <summary>
        /// 超大尺寸将使游戏窗口大于显示器，支持多显示器
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "超大宽度";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% 宽度";
        public override string GraphicsOption_OversizeHeight => "超大高度";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% 高度";
        public override string GraphicsOption_Oversize_None => "无";

        /// <summary>
        /// 录制Youtube时的特定分辨率
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "录制预设";

        /// <summary>
        /// 0: 高度分辨率
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// 更改文字和图标的大小
        /// </summary>
        public override string GraphicsOption_UiScale => "UI缩放";

        public override string ReversedStereo => "反向立体声";
        public override string Option_Low => "低";
        public override string Option_Medium => "中";
        public override string Option_High => "高";

        public override string MouseSettings_Title => "鼠标输入";
        public override string KeyboardSettings_Title => "键位绑定";

        public override string MouseButtonAction_None => "无操作";
        public override string MouseButtonAction_Select => "选择";
        public override string MouseButtonAction_Pan => "平移";
        public override string MouseButtonAction_PanAndOrder => "平移并下达指令";
        public override string MouseButtonAction_Order => "下达指令";
        public override string MouseButtonAction_Cancel => "取消";

        public override string MouseButton_Left => "鼠标左键";
        public override string MouseButton_Right => "鼠标右键";
        public override string MouseButton_Middle => "鼠标中键";
        public override string MouseButton_X1 => "鼠标 X1 键";
        public override string MouseButton_X2 => "鼠标 X2 键";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "平移并取消";
        public override string MouseButtonAction_PanAndOrderAndCancel => "平移、下达指令并取消";

        public override string GraphicsOption_Shadows => "阴影";
        public override string GraphicsOption_ShadowType_ModelsToGround => "模型投射到地面";
        public override string GraphicsOption_ShadowType_ModelsToModels => "模型投射到模型";

        public override string GraphicsOption_Shadow_MapResolution => "阴影贴图分辨率";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "添加 {0} 像素";
        public override string Settings_KeyMapPanSpeed => "平移速度";
        public override string Settings_StoreCameraPosition => "保存摄像机位置";
        public override string Settings_LoadCameraPosition => "加载位置";


        //Shadow update
        public override string Settings_ModelWaterFoam => "水面泡沫";
        public override string Settings_ModelShadow => "阴影";
        public override string Settings_ModelShadowMapSize => "阴影贴图大小";
        public override string Settings_Brightness => "亮度";
        public override string Settings_Mode_No_Achivements => "成就不可用。";
        public override string Settings_FrameRate => "帧率";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "阻止导入的存档获得成就";


    }
}
