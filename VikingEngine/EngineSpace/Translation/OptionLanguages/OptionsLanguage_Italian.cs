using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Italian : AbsOptionsLanguage
    {
        public override string GameSettings_UltraWide => "Ultrawide";
        public override string GameSettings_UltraWide_LeftEdge => "Bordo sinistro della UI";
        public override string GameSettings_UltraWide_RightEdge => "Bordo destro della UI";
        public override string GameSettings_WideScrollbar => "Barra di scorrimento larga";
        public override string GameSettings_DisplayInputHelp => "Guida ai comandi";
        public override string GameSettings_InputSmoothing => "Fluidità dei comandi";
        //Mounts
        public override string InputSteam => "Steam Input";
        public override string Input_SimulateMouse => "Simula mouse";
        public override string Input_LockMouseToWindow => "Blocca il mouse nella finestra";
        public override string Input_MouseEdgePush_Title => "Scorrimento ai bordi";
        public override string Input_NoControl => "Nessuno";
        public override string Input_ActiveControl => "Attivo";
        public override string Input_PassiveControl => "Passivo";
        public override string Setting_MinimapScale => "Scala della minimappa";

        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Dissolvenza livelli";
        public override string SplitScreen_HorizontalFirst => "Orizzontale prima";
        public override string SplitScreen_VerticalFirst => "Verticale prima";
        public override string SplitScreen_HorizontalOnly => "Solo orizzontale";
        public override string SplitScreen_VerticalOnly => "Solo verticale";
        public override string SplitScreen_Title => "Schermo condiviso";
        public override string SplitScreen_AdjustSplit => "Regola divisione {0}";

        public override string Settings_ControllerVibration => "Vibrazione controller";
        public override string GraphicsOption_IngameMenuWidth => "Larghezza del menu di gioco";
        public override string DisplayMode => "Modalità di visualizzazione";
        public override string DisplayMode_Windowed => "Finestra";
        public override string DisplayMode_BorderlessFullscreen => "Schermo intero senza bordi";
        public override string GameSettings_RenderedMouseCursor => "Cursore renderizzato";
        public override string GameSettings_MuteControllerDisconnect => "Silenzia disconnessione controller";
        //--
        public override string GraphicsOption_FarViewDistance => "Vista a lunga distanza";


        public override string Hud_Cancel => "Annulla";
        public override string Hud_Back => "Indietro";

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => "Sei sicuro?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Sì";
        public override string Hud_No => "No";

        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Opzioni";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Input: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Tastiera e mouse";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Controller";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Non impostato";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Schermo diviso verticale";


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => "Volume musica";

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => "Volume effetti";
        
        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => "Risoluzione";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Schermo intero";

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Larghezza extra";
        public override string GraphicsOption_PercentageOversizeWidth => "Larghezza {0}%";
        public override string GraphicsOption_OversizeHeight => "Altezza extra";
        public override string GraphicsOption_PercentageOversizeHeight => "Altezza {0}%";
        public override string GraphicsOption_Oversize_None => "Nessuno";

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Preset di registrazione";

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "YouTube {0}p";

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Scala UI";


        //---
        public override string ReversedStereo => "Stereo invertito";
        public override string Option_Low => "Basso";
        public override string Option_Medium => "Media";
        public override string Option_High => "Alto";
        public override string MouseSettings_Title => "Input mouse";
        public override string KeyboardSettings_Title => "Mappatura tasti";
        public override string MouseButtonAction_None => "Nessuna azione";
        public override string MouseButtonAction_Select => "Seleziona";
        public override string MouseButtonAction_Pan => "Panoramica";
        public override string MouseButtonAction_PanAndOrder => "Panoramica e ordine";
        public override string MouseButtonAction_Order => "Ordine";
        public override string MouseButtonAction_Cancel => "Annulla";

        public override string MouseButton_Left => "Mouse sinistro";
        public override string MouseButton_Right => "Mouse destro";
        public override string MouseButton_Middle => "Mouse centrale";
        public override string MouseButton_X1 => "Pulsante X1 mouse";
        public override string MouseButton_X2 => "Pulsante X2 mouse";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Panoramica e annulla";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Panoramica, ordine e annulla";

        public override string GraphicsOption_Shadows => "Ombre";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modelli su terreno";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modelli su modelli";

        public override string GraphicsOption_Shadow_MapResolution => "Risoluzione mappa ombre";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Aggiungi {0} pixel";
        public override string Settings_KeyMapPanSpeed => "Velocità panoramica";
        public override string Settings_StoreCameraPosition => "Memorizza posizione camera";
        public override string Settings_LoadCameraPosition => "Carica posizione";


        //Shadow update

        public override string Settings_ModelWaterFoam => "Schiuma dell’acqua";
        public override string Settings_ModelShadow => "Ombre";
        public override string Settings_ModelShadowMapSize => "Dimensione della Shadow Map";
        public override string Settings_Brightness => "Luminosità";
        public override string Settings_Mode_No_Achivements => "Obiettivi non disponibili.";
        public override string Settings_FrameRate => "Frame rate";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Blocca gli obiettivi per i salvataggi importati";

    }
}
