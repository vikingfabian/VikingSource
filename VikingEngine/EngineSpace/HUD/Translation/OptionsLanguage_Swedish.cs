using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.HUD
{
    class OptionsLanguage_Swedish : AbsOptionsLanguage
    {
        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Alternativ";

        /// <summary>
        /// Open input options, 0: current input
        /// </summary>
        public override string InputSelect => "Inmatning: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Tangentbord & mus";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Kontroller";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Inte inställd";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Vertikal skärmdelning";


        public override string SoundOption_MusicVolume => "Music Volume";
        public override string SoundOption_SoundVolume => "Sound Volume";
        public override string GraphicsOption_Resolution => "Resolution";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";
        public override string GraphicsOption_Fullscreen => "Fullscreen";
        public override string GraphicsOption_OversizeWidth => "Oversize width";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Width";
        public override string GraphicsOption_OversizeHeight => "Oversize height";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Height";
        public override string GraphicsOption_Oversize_None => "None";
        public override string GraphicsOption_RecordingPresets => "Recording presets";
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Ui Scale";
    }
}
