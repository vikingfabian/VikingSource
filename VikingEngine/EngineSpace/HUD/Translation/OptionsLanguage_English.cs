using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class OptionsLanguage_English : AbsOptionsLanguage
    {
        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Options";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Input: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Keyboard & mouse";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Controller";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Not set";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Vertical screen split";


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => "Music Volume";

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => "Sound Volume";
        
        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => "Resolution";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Fullscreen";

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Oversize width";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Width";
        public override string GraphicsOption_OversizeHeight => "Oversize height";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Height";
        public override string GraphicsOption_Oversize_None => "None";

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Recording presets";

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";
    }
}
