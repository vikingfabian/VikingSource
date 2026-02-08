using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Thai : AbsOptionsLanguage
    {
        public override string Settings_Particles_FadeMapLayers => TextLib.ThaiConv("เลือน|เลเยอร์|แผนที่");
        public override string SplitScreen_HorizontalFirst => TextLib.ThaiConv("แนวนอน|ก่อน");
        public override string SplitScreen_VerticalFirst => TextLib.ThaiConv("แนวตั้ง|ก่อน");
        public override string SplitScreen_HorizontalOnly => TextLib.ThaiConv("เฉพาะ|แนวนอน");
        public override string SplitScreen_VerticalOnly => TextLib.ThaiConv("เฉพาะ|แนวตั้ง");
        public override string SplitScreen_Title => TextLib.ThaiConv("แบ่ง|หน้าจอ");
        public override string SplitScreen_AdjustSplit => TextLib.ThaiConv("ปรับ|การ|แบ่ง|หน้าจอ| {0}");
        public override string Settings_ControllerVibration => TextLib.ThaiConv("การ|สั่น|ของ|คอนโทรลเลอร์");

        //Winter update settings
        public override string GraphicsOption_IngameMenuWidth => TextLib.ThaiConv("ความ|กว้าง|เมนู|ใน|เกม");
        public override string DisplayMode => TextLib.ThaiConv("โหมด|การ|แสดง|ผล");
        public override string DisplayMode_Windowed => TextLib.ThaiConv("แบบ|หน้าต่าง");
        public override string DisplayMode_BorderlessFullscreen => TextLib.ThaiConv("เต็ม|จอ|ไร้|ขอบ");
        public override string GameSettings_RenderedMouseCursor => TextLib.ThaiConv("เรนเดอร์|เคอร์เซอร์");

        //--
        public override string GraphicsOption_FarViewDistance => TextLib.ThaiConv("ระยะ|การ|มอง|เห็น|ไกล");

        public override string Hud_Cancel => TextLib.ThaiConv("ยกเลิก");
        public override string Hud_Back => TextLib.ThaiConv("ย้อน|กลับ");

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => TextLib.ThaiConv("แน่|ใจ|หรือ|ไม่?");

        public override string Hud_OK => TextLib.ThaiConv("ตกลง");
        public override string Hud_Yes => TextLib.ThaiConv("ใช่");
        public override string Hud_No => TextLib.ThaiConv("ไม่");

        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => TextLib.ThaiConv("ตั้งค่า");

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => TextLib.ThaiConv("อินพุต: {0}");

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => TextLib.ThaiConv("คีย์บอร์ด|และ|เมาส์");

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => TextLib.ThaiConv("คอนโทรลเลอร์");

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => TextLib.ThaiConv("ไม่|ได้|ตั้ง|ค่า");

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => TextLib.ThaiConv("แบ่ง|หน้าจอ|แนวตั้ง");


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => TextLib.ThaiConv("ระดับ|เสียง|เพลง");

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => TextLib.ThaiConv("ระดับ|เสียง|เอฟเฟกต์");

        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => TextLib.ThaiConv("ความ|ละเอียด|หน้าจอ");
        public override string GraphicsOption_Resolution_PercentageOption => TextLib.ThaiConv("{0}%");

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => TextLib.ThaiConv("เต็ม|จอ");

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => TextLib.ThaiConv("ความ|กว้าง|เกิน|ขนาด");
        public override string GraphicsOption_PercentageOversizeWidth => TextLib.ThaiConv("{0}% | ความ|กว้าง");
        public override string GraphicsOption_OversizeHeight => TextLib.ThaiConv("ความ|สูง|เกิน|ขนาด");
        public override string GraphicsOption_PercentageOversizeHeight => TextLib.ThaiConv("{0}% | ความ|สูง");
        public override string GraphicsOption_Oversize_None => TextLib.ThaiConv("ไม่มี");

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => TextLib.ThaiConv("พรีเซ็ต|สำหรับ|บันทึก|วิดีโอ");

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => TextLib.ThaiConv("Youtube {0}p");

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => TextLib.ThaiConv("ขนาด|ของ|UI");


        //---
        public override string ReversedStereo => TextLib.ThaiConv("สลับ|เสียง|ซ้าย|ขวา");
        public override string Option_Low => TextLib.ThaiConv("ต่ำ");
        public override string Option_Medium => TextLib.ThaiConv("กลาง");
        public override string Option_High => TextLib.ThaiConv("สูง");
        public override string MouseSettings_Title => TextLib.ThaiConv("การ|ตั้ง|ค่า|เมาส์");
        public override string KeyboardSettings_Title => TextLib.ThaiConv("การ|ตั้ง|ค่า|ปุ่ม|กด");
        public override string MouseButtonAction_None => TextLib.ThaiConv("ไม่|มี|คำ|สั่ง");
        public override string MouseButtonAction_Select => TextLib.ThaiConv("เลือก");
        public override string MouseButtonAction_Pan => TextLib.ThaiConv("เลื่อน|กล้อง");
        public override string MouseButtonAction_PanAndOrder => TextLib.ThaiConv("เลื่อน|กล้อง|และ|สั่ง|การ");
        public override string MouseButtonAction_Order => TextLib.ThaiConv("สั่ง|การ");
        public override string MouseButtonAction_Cancel => TextLib.ThaiConv("ยกเลิก");

        public override string MouseButton_Left => TextLib.ThaiConv("เมาส์|ซ้าย");
        public override string MouseButton_Right => TextLib.ThaiConv("เมาส์|ขวา");
        public override string MouseButton_Middle => TextLib.ThaiConv("เมาส์|กลาง");
        public override string MouseButton_X1 => TextLib.ThaiConv("ปุ่ม|เมาส์|X1");
        public override string MouseButton_X2 => TextLib.ThaiConv("ปุ่ม|เมาส์|X2");

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => TextLib.ThaiConv("เลื่อน|กล้อง|และ|ยกเลิก");
        public override string MouseButtonAction_PanAndOrderAndCancel => TextLib.ThaiConv("เลื่อน|กล้อง, สั่ง|การ, และ|ยกเลิก");

        public override string GraphicsOption_Shadows => TextLib.ThaiConv("เงา");
        public override string GraphicsOption_ShadowType_ModelsToGround => TextLib.ThaiConv("เงา|โมเดล|ลง|พื้น");
        public override string GraphicsOption_ShadowType_ModelsToModels => TextLib.ThaiConv("เงา|โมเดล|ทับ|โมเดล");

        public override string GraphicsOption_Shadow_MapResolution => TextLib.ThaiConv("ความ|ละเอียด|แผนที่|เงา");

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => TextLib.ThaiConv("เพิ่ม| {0} |พิกเซล");
        public override string Settings_KeyMapPanSpeed => TextLib.ThaiConv("ความ|เร็ว|การ|เลื่อน|กล้อง");
        public override string Settings_StoreCameraPosition => TextLib.ThaiConv("บันทึก|ตำแหน่ง|กล้อง");
        public override string Settings_LoadCameraPosition => TextLib.ThaiConv("โหลด|ตำแหน่ง|กล้อง");


        //Shadow update
        public override string Settings_ModelWaterFoam => TextLib.ThaiConv("ฟอง|คลื่น|น้ำ");
        public override string Settings_ModelShadow => TextLib.ThaiConv("เงา");
        public override string Settings_ModelShadowMapSize => TextLib.ThaiConv("ขนาด|แผนที่|เงา");
        public override string Settings_Brightness => TextLib.ThaiConv("ความ|สว่าง");
        public override string Settings_Mode_No_Achivements => TextLib.ThaiConv("ไม่|สามารถ|เก็บ|ความ|สำเร็จ| (Achievements) |ได้");
        public override string Settings_FrameRate => TextLib.ThaiConv("เฟรมเรต");
        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => TextLib.ThaiConv("ปิด|การ|เก็บ|ความ|สำเร็จ|สำหรับ|ไฟล์|เซฟ|ที่|นำ|เข้า");

    }
}
