using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.HUD
{
    class OptionsLanguage_German : AbsOptionsLanguage
    {
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

    }
}
