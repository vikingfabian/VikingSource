using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_English : AbsOptionsLanguage
    {


        public override string Hud_Cancel => "Cancel";
        public override string Hud_Back => "Back";

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => "Are you sure?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Yes";
        public override string Hud_No => "No";

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

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Ui Scale";


        //---
        public override string ReversedStereo => "Reversed stereo";
        public override string Option_Low => "Low";
        public override string Option_Medium => "Medium";
        public override string Option_High => "High";
        public override string MouseSettings_Title => "Mouse input";
        public override string KeyboardSettings_Title => "Key mapping";
        public override string MouseButtonAction_None => "No action";
        public override string MouseButtonAction_Select => "Select";
        public override string MouseButtonAction_Pan => "Pan";
        public override string MouseButtonAction_PanAndOrder => "Pan and Order";
        public override string MouseButtonAction_Order => "Order";
        public override string MouseButtonAction_Cancel => "Cancel";

        public override string MouseButton_Left => "Left Mouse";
        public override string MouseButton_Right => "Right Mouse";
        public override string MouseButton_Middle => "Middle Mouse";
        public override string MouseButton_X1 => "X1 Button Mouse";
        public override string MouseButton_X2 => "X2 Button Mouse";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Pan and Cancel";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Pan, Order, and Cancel";

        public override string GraphicsOption_Shadows => "Shadows";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Models to Ground";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Models to Models";

        public override string GraphicsOption_Shadow_MapResolution => "Shadow Map Resolution";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Add {0} pixels";
        public override string Settings_KeyMapPanSpeed => "Pan speed";
        public override string Settings_StoreCameraPosition => "Store camera position";
        public override string Settings_LoadCameraPosition => "Load position";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Water foam";
        public override string Settings_ModelShadow => "Shadows";
        public override string Settings_ModelShadowMapSize => "Shadow map size";
        public override string Settings_Brightness => "Brightness";
        public override string Settings_Mode_No_Achivements => "Achievements are not available.";
        public override string Settings_FrameRate => "Frame rate";
        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Block achievements for imported save files";


    }
}
