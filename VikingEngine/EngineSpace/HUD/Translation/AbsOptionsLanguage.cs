using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;

namespace VikingEngine.HUD
{
    abstract class AbsOptionsLanguage
    {
        public abstract string Hud_Cancel { get; }
        public abstract string Hud_Back { get; }
        public abstract string Hud_AreYouSure { get; }
        public abstract string Hud_OK { get; }
        public abstract string Hud_Yes { get; }
        public abstract string Hud_No { get; }

        public abstract string Options_title { get; }

        public abstract string InputSelect { get; }

        public abstract string InputKeyboardMouse { get; }
        public abstract string InputController { get; }
        public abstract string InputNotSet { get; }

        public abstract string VerticalSplitScreen { get; }

        public abstract string SoundOption_MusicVolume { get; }
        public abstract string SoundOption_SoundVolume { get; }
        public abstract string GraphicsOption_Resolution { get; }
        public abstract string GraphicsOption_Resolution_PercentageOption { get; }
        public abstract string GraphicsOption_Fullscreen { get; }
        public abstract string GraphicsOption_OversizeWidth { get; }
        public abstract string GraphicsOption_PercentageOversizeWidth { get; }
        public abstract string GraphicsOption_OversizeHeight { get; }
        public abstract string GraphicsOption_PercentageOversizeHeight { get; }
        public abstract string GraphicsOption_Oversize_None { get; }
        public abstract string GraphicsOption_RecordingPresets { get; }
        public abstract string GraphicsOption_YoutubePreset { get; }

        public abstract string GraphicsOption_UiScale { get; }

        public string ThreeOption(ThreeOptions option)
        {
            switch (option)
            {
                case ThreeOptions.Low:
                    return DssRef.todoLang.Option_Low;
                case ThreeOptions.Medium:
                    return DssRef.todoLang.Option_Medium;
                case ThreeOptions.High:
                    return DssRef.todoLang.Option_High;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
