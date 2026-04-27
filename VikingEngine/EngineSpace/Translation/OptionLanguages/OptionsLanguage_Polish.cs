using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Polish : AbsOptionsLanguage
    {
        //Mounts
        public override string InputSteam => "Steam Input";
        public override string Input_SimulateMouse => "Symulacja myszy";
        public override string Input_LockMouseToWindow => "Zablokuj mysz w oknie";
        public override string Input_MouseEdgePush_Title => "Skrolowanie przy krawędzi";
        public override string Input_NoControl => "Brak";
        public override string Input_ActiveControl => "Aktywne";
        public override string Input_PassiveControl => "Pasywne";
        public override string Setting_MinimapScale => "Skala minimapy";

        public override string Settings_Particles_FadeMapLayers => "Wygaszanie warstw";
        public override string SplitScreen_HorizontalFirst => "Najpierw poziomo";
        public override string SplitScreen_VerticalFirst => "Najpierw pionowo";
        public override string SplitScreen_HorizontalOnly => "Tylko poziomo";
        public override string SplitScreen_VerticalOnly => "Tylko pionowo";
        public override string SplitScreen_Title => "Podzielony ekran (Split-screen)";
        public override string SplitScreen_AdjustSplit => "Dostosuj podział {0}";
        public override string Settings_ControllerVibration => "Wibracje kontrolera";

        //Winter update settings
        public override string GraphicsOption_IngameMenuWidth => "Szerokość menu gry";
        public override string DisplayMode => "Tryb wyświetlania";
        public override string DisplayMode_Windowed => "W oknie";
        public override string DisplayMode_BorderlessFullscreen => "Pełny ekran bez ramek";
        public override string GameSettings_RenderedMouseCursor => "Renderowany kursor";
        public override string GameSettings_MuteControllerDisconnect => "Wycisz komunikaty o rozłączeniu";
        //--
        public override string GraphicsOption_FarViewDistance => "Zasięg widzenia";

        public override string Hud_Cancel => "Anuluj";
        public override string Hud_Back => "Wstecz";

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => "Czy na pewno?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Tak";
        public override string Hud_No => "Nie";

        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Opcje";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Sterowanie: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Klawiatura i mysz";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Kontroler";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Nie ustawiono";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Pionowy podział ekranu";


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => "Głośność muzyki";

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => "Głośność dźwięków";

        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => "Rozdzielczość";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Pełny ekran";

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Szerokość ponadwymiarowa";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Szerokości";
        public override string GraphicsOption_OversizeHeight => "Wysokość ponadwymiarowa";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Wysokości";
        public override string GraphicsOption_Oversize_None => "Brak";

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Presety nagrywania";

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Skala interfejsu (UI)";


        //---
        public override string ReversedStereo => "Odwrócone stereo";
        public override string Option_Low => "Niska";
        public override string Option_Medium => "Średnia";
        public override string Option_High => "Wysoka";
        public override string MouseSettings_Title => "Mysz";
        public override string KeyboardSettings_Title => "Skróty klawiszowe";
        public override string MouseButtonAction_None => "Brak akcji";
        public override string MouseButtonAction_Select => "Wybierz";
        public override string MouseButtonAction_Pan => "Przesuń widok (Pan)";
        public override string MouseButtonAction_PanAndOrder => "Przesuń i wydaj rozkaz";
        public override string MouseButtonAction_Order => "Rozkaz";
        public override string MouseButtonAction_Cancel => "Anuluj";

        public override string MouseButton_Left => "LPM";
        public override string MouseButton_Right => "PPM";
        public override string MouseButton_Middle => "ŚPM";
        public override string MouseButton_X1 => "Mysz: Przycisk X1";
        public override string MouseButton_X2 => "Mysz: Przycisk X2";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Przesuń i anuluj";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Przesuń, rozkaz i anuluj";

        public override string GraphicsOption_Shadows => "Cienie";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modele na podłoże";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modele na modele";

        public override string GraphicsOption_Shadow_MapResolution => "Rozdzielczość mapy cieni";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Dodaj {0} pikseli";
        public override string Settings_KeyMapPanSpeed => "Szybkość przesuwania";
        public override string Settings_StoreCameraPosition => "Zapisz pozycję kamery";
        public override string Settings_LoadCameraPosition => "Wczytaj pozycję";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Piana wodna";
        public override string Settings_ModelShadow => "Cienie";
        public override string Settings_ModelShadowMapSize => "Rozmiar mapy cieni";
        public override string Settings_Brightness => "Jasność";
        public override string Settings_Mode_No_Achivements => "Osiągnięcia są niedostępne.";
        public override string Settings_FrameRate => "Liczba klatek (FPS)";
        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Blokuj osiągnięcia dla importowanych zapisów";

    }
}
