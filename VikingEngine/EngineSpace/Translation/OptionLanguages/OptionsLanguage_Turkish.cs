using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Turkish : AbsOptionsLanguage
    {
        public override string GraphicsOption_IngameMenuWidth => "Oyun içi menü genişliği";
        public override string DisplayMode => "Görüntü modu";
        public override string DisplayMode_Windowed => "Pencere modu";
        public override string DisplayMode_BorderlessFullscreen => "Çerçevesiz tam ekran";
        public override string GameSettings_RenderedMouseCursor => "Render edilmiş imleç";

        //--
        public override string GraphicsOption_FarViewDistance => "Uzun mesafe görüşü";

        public override string Hud_Cancel => "İptal";
        public override string Hud_Back => "Geri";

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => "Mutabık mısın?";

        public override string Hud_OK => "Mutabığım";
        public override string Hud_Yes => "Evet";
        public override string Hud_No => "Hayır";

        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Ayarlar";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Girdi: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Klavye & Fare";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Kontrolcü";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Boş";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Dikey Bölünmüş Ekran";


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => "Müzik Seviyesi";

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => "Ses Seviyesi";
        
        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => "Çözünürlük";
        public override string GraphicsOption_Resolution_PercentageOption => "%{0}";

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Tam Ekran";

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Genişlik ayarı";
        public override string GraphicsOption_PercentageOversizeWidth => "%{0} Genişlik";
        public override string GraphicsOption_OversizeHeight => "Yükseklik ayarı";
        public override string GraphicsOption_PercentageOversizeHeight => "%{0} Yükseklik";
        public override string GraphicsOption_Oversize_None => "Hiç";

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Kayıt alma ön ayarı";

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Arayüz Boyutu";


        //---
        public override string ReversedStereo => "Tersine Stereo";
        public override string Option_Low => "Düşük";
        public override string Option_Medium => "Orta";
        public override string Option_High => "Yüksek";
        public override string MouseSettings_Title => "Fare Girdisi";
        public override string KeyboardSettings_Title => "Tuş Atamaları";
        public override string MouseButtonAction_None => "Eylem Yok";
        public override string MouseButtonAction_Select => "Seç";
        public override string MouseButtonAction_Pan => "Kaydır";
        public override string MouseButtonAction_PanAndOrder => "Kaydır ve Komut Ver";
        public override string MouseButtonAction_Order => "Komut Ver";
        public override string MouseButtonAction_Cancel => "İptal";

        public override string MouseButton_Left => "Sol Tık";
        public override string MouseButton_Right => "Sağ Tık";
        public override string MouseButton_Middle => "Fare Tekerleği";
        public override string MouseButton_X1 => "X1 Fare Tuşu";
        public override string MouseButton_X2 => "X2 Fare Tuşu";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Kaydır ve İptal Et";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Kaydır, Komut Ver, ve İptal Et";

        public override string GraphicsOption_Shadows => "Gölgeler";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Zemine Düşen Gölge";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modele Düşen Gölge";

        public override string GraphicsOption_Shadow_MapResolution => "Gölge Haritası Çözünürlüğü";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "{0} adet piksel ekle";
        public override string Settings_KeyMapPanSpeed => "Kaydırma hızı";
        public override string Settings_StoreCameraPosition => "Kamera Konumunu Kaydet";
        public override string Settings_LoadCameraPosition => "Kaydedilen Konumu Yükle";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Su köpüğü";
        public override string Settings_ModelShadow => "Gölgeler";
        public override string Settings_ModelShadowMapSize => "Shadow Map boyutu";
        public override string Settings_Brightness => "Parlaklık";
        public override string Settings_Mode_No_Achivements => "Başarımlar kullanılamaz.";
        public override string Settings_FrameRate => "Kare hızı";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "İçe aktarılan kayıt dosyaları için başarımları engelle";


    }
}
