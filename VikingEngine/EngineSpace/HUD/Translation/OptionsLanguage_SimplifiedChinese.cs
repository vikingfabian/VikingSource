using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class OptionsLanguage_SimplifiedChinese : AbsOptionsLanguage
    {

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
    }
}
