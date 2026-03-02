using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_German : AbsOptionsLanguage
    {
        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Ebenen ausblenden";
        public override string SplitScreen_HorizontalFirst => "Horizontal zuerst";
        public override string SplitScreen_VerticalFirst => "Vertikal zuerst";
        public override string SplitScreen_HorizontalOnly => "Nur horizontal";
        public override string SplitScreen_VerticalOnly => "Nur vertikal";
        public override string SplitScreen_Title => "Splitscreen"; // Common term in German gaming
        public override string SplitScreen_AdjustSplit => "Aufteilung anpassen {0}";

        public override string Settings_ControllerVibration => "Controller-Vibration";
        public override string GraphicsOption_IngameMenuWidth => "Breite des Ingame-Menüs";
        public override string DisplayMode => "Anzeigemodus";
        public override string DisplayMode_Windowed => "Fenstermodus";
        public override string DisplayMode_BorderlessFullscreen => "Randloser Vollbildmodus";
        public override string GameSettings_RenderedMouseCursor => "Gerenderter Cursor";
        public override string GameSettings_MuteControllerDisconnect => "Controller-Warnungen stummschalten";
        //--
        public override string GraphicsOption_FarViewDistance => "Sichtweite (Long Distance)";

        public override string Hud_Cancel => "Abbrechen";
        public override string Hud_Back => "Zurück";

        /// <summary>
        /// Untermenü für destruktive Entscheidungen des Spielers
        /// </summary>
        public override string Hud_AreYouSure => "Bist du sicher?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Ja";
        public override string Hud_No => "Nein";

        /// <summary>
        /// Titel des Optionsmenüs
        /// </summary>
        public override string Options_title => "Optionen";

        /// <summary>
        /// Spielsteuerungseingaben, 0: aktuelle Eingabe
        /// </summary>
        public override string InputSelect => "Eingabe: {0}";

        /// <summary>
        /// Art der Spieleingabe
        /// </summary>
        public override string InputKeyboardMouse => "Tastatur & Maus";

        /// <summary>
        /// Art der Spieleingabe
        /// </summary>
        public override string InputController => "Controller";

        /// <summary>
        /// Keine Spieleingabe ausgewählt
        /// </summary>
        public override string InputNotSet => "Nicht festgelegt";

        /// <summary>
        /// Bezeichnung für das Kontrollkästchen. Option für lokales Splitscreen-Gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Vertikale Bildschirmteilung";

        /// <summary>
        /// Bezeichnung für den Lautstärkeregler
        /// </summary>
        public override string SoundOption_MusicVolume => "Musiklautstärke";

        /// <summary>
        /// Bezeichnung für den Lautstärkeregler
        /// </summary>
        public override string SoundOption_SoundVolume => "Soundlautstärke";

        /// <summary>
        /// Bildschirmauflösung
        /// </summary>
        public override string GraphicsOption_Resolution => "Auflösung";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Das Spiel im Vollbild- oder Fenstermodus anzeigen
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Vollbild";

        /// <summary>
        /// Überskalierung vergrößert das Spielfenster über den Monitor hinaus für Multi-Monitor-Unterstützung
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Überskalierte Breite";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Breite";
        public override string GraphicsOption_OversizeHeight => "Überskalierte Höhe";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Höhe";
        public override string GraphicsOption_Oversize_None => "Keine";

        /// <summary>
        /// Spezifische Auflösungen für Aufnahmen auf YouTube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Aufnahmepresets";

        /// <summary>
        /// 0: Auflösungshöhe
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "YouTube {0}p";

        /// <summary>
        /// Ändert die Größe von Text und Symbolen
        /// </summary>
        public override string GraphicsOption_UiScale => "UI-Skalierung";

        public override string ReversedStereo => "Umgekehrtes Stereo";
        public override string Option_Low => "Niedrig";
        public override string Option_Medium => "Mittel";
        public override string Option_High => "Hoch";

        public override string MouseSettings_Title => "Mauseingabe";
        public override string KeyboardSettings_Title => "Tastenzuweisung";

        public override string MouseButtonAction_None => "Keine Aktion";
        public override string MouseButtonAction_Select => "Auswählen";
        public override string MouseButtonAction_Pan => "Verschieben";
        public override string MouseButtonAction_PanAndOrder => "Verschieben und Befehl";
        public override string MouseButtonAction_Order => "Befehl";
        public override string MouseButtonAction_Cancel => "Abbrechen";

        public override string MouseButton_Left => "Linke Maustaste";
        public override string MouseButton_Right => "Rechte Maustaste";
        public override string MouseButton_Middle => "Mittlere Maustaste";
        public override string MouseButton_X1 => "Maus-Taste X1";
        public override string MouseButton_X2 => "Maus-Taste X2";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Schwenken und Abbrechen";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Schwenken, Befehl geben und Abbrechen";

        public override string GraphicsOption_Shadows => "Schatten";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modelle auf Boden";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modelle auf Modelle";

        public override string GraphicsOption_Shadow_MapResolution => "Schattenkartenauflösung";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "{0} Pixel hinzufügen";
        public override string Settings_KeyMapPanSpeed => "Schwenkgeschwindigkeit";
        public override string Settings_StoreCameraPosition => "Kameraposition speichern";
        public override string Settings_LoadCameraPosition => "Position laden";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Wasserschaum";
        public override string Settings_ModelShadow => "Schatten";
        public override string Settings_ModelShadowMapSize => "Shadow-Map-Größe";
        public override string Settings_Brightness => "Helligkeit";
        public override string Settings_Mode_No_Achivements => "Erfolge sind nicht verfügbar.";
        public override string Settings_FrameRate => "Bildrate";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Erfolge für importierte Spielstände blockieren";


    }
}
