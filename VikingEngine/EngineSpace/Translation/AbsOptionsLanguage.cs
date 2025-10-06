using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;

namespace VikingEngine.EngineSpace.Translation
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


        //
        public abstract string ReversedStereo { get; }
        public abstract string Option_Low { get; }
        public abstract string Option_Medium { get; }
        public abstract string Option_High { get; }
        public abstract string MouseSettings_Title { get; }
        public abstract string KeyboardSettings_Title { get; }
        public abstract string MouseButtonAction_None { get; }
        public abstract string MouseButtonAction_Select { get; }
        public abstract string MouseButtonAction_Pan { get; }
        public abstract string MouseButtonAction_PanAndOrder { get; }
        public abstract string MouseButtonAction_Order { get; }
        public abstract string MouseButtonAction_Cancel { get; }
        public abstract string MouseButton_Left { get; }
        public abstract string MouseButton_Right { get; }
        public abstract string MouseButton_Middle { get; }
        public abstract string MouseButton_X1 { get; }
        public abstract string MouseButton_X2 { get; }

        //DEMO PATCH 4
        public abstract string MouseButtonAction_PanAndCancel { get; }
        public abstract string MouseButtonAction_PanAndOrderAndCancel { get; }

        public abstract string GraphicsOption_Shadows { get; }
        public abstract string GraphicsOption_ShadowType_ModelsToGround { get; }
        public abstract string GraphicsOption_ShadowType_ModelsToModels { get; }

        public abstract string GraphicsOption_Shadow_MapResolution { get; }

        //DEMO PATCH 5
        public abstract string GraphicsOption_RecordingPresets_AddXPixels { get; }
        public abstract string Settings_KeyMapPanSpeed { get; }
        public abstract string Settings_StoreCameraPosition { get; }
        public abstract string Settings_LoadCameraPosition { get; }

        //Shadow update
        public abstract string Settings_ModelWaterFoam { get; }
        public abstract string Settings_ModelShadow { get; }
        public abstract string Settings_ModelShadowMapSize { get; }
        public abstract string Settings_Brightness { get; }
        public abstract string Settings_Mode_No_Achivements { get; }
        public abstract string Settings_FrameRate { get; }
        /// <summary>
        /// Steam Achievements
        /// </summary>
        public abstract string Settings_ImportNoAchievement { get; }
        public abstract string GraphicsOption_FarViewDistance { get; }

        public string ThreeOption(ThreeOptions option)
        {
            switch (option)
            {
                case ThreeOptions.Low:
                    return Ref.langOpt.Option_Low;
                case ThreeOptions.Medium:
                    return Ref.langOpt.Option_Medium;
                case ThreeOptions.High:
                    return Ref.langOpt.Option_High;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
