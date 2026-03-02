using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_French : AbsOptionsLanguage
    {
        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Fondu des calques";
        public override string SplitScreen_HorizontalFirst => "Horizontal en premier";
        public override string SplitScreen_VerticalFirst => "Vertical en premier";
        public override string SplitScreen_HorizontalOnly => "Horizontal seulement";
        public override string SplitScreen_VerticalOnly => "Vertical seulement";
        public override string SplitScreen_Title => "Écran scindé";
        public override string SplitScreen_AdjustSplit => "Ajuster la séparation {0}";

        public override string Settings_ControllerVibration => "Vibration de la manette";
        public override string GraphicsOption_IngameMenuWidth => "Largeur du menu en jeu";
        public override string DisplayMode => "Mode d’affichage";
        public override string DisplayMode_Windowed => "Fenêtré";
        public override string DisplayMode_BorderlessFullscreen => "Plein écran sans bordure";
        public override string GameSettings_RenderedMouseCursor => "Curseur rendu";
        public override string GameSettings_MuteControllerDisconnect => "Masquer alertes manette";
        //-
        public override string GraphicsOption_FarViewDistance => "Vue longue distance";

        public override string Hud_Cancel => "Annuler";
        public override string Hud_Back => "Retour";

        /// <summary>
        /// Submenu for when the player will make destructive choices
        /// </summary>
        public override string Hud_AreYouSure => "Êtes-vous sur?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Oui";
        public override string Hud_No => "Non";

        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Options";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Entrée: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Clavier & souris";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Manette";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Indéfini";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Ecran scindé vertical";


        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_MusicVolume => "Volume de la musique";

        /// <summary>
        /// Label for sound slider
        /// </summary>
        public override string SoundOption_SoundVolume => "Volume du son";
        
        /// <summary>
        /// Screen resolution
        /// </summary>
        public override string GraphicsOption_Resolution => "Résolution";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Display the game fullscreen or window mode
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Plein écran";

        /// <summary>
        /// Oversize will make the game window be larger than the monitor, for multi monitor support
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Largeur extra";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% largeur";
        public override string GraphicsOption_OversizeHeight => "Hauteur extra";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% hauteur";
        public override string GraphicsOption_Oversize_None => "Aucun";

        /// <summary>
        /// Specific resolutions for when recording to Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Preset de capture";

        /// <summary>
        /// 0: height resolution
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Change size on text and icons
        /// </summary>
        public override string GraphicsOption_UiScale => "Échelle de l'UI";


        //---
        public override string ReversedStereo => "Stereo inversée";
        public override string Option_Low => "Bas";
        public override string Option_Medium => "Moyen";
        public override string Option_High => "Haut";
        public override string MouseSettings_Title => "Entrée souris";
        public override string KeyboardSettings_Title => "Raccourcis";
        public override string MouseButtonAction_None => "Pas d'action";
        public override string MouseButtonAction_Select => "Sélectionner";
        public override string MouseButtonAction_Pan => "Descendre";
        public override string MouseButtonAction_PanAndOrder => "Descendre et ordonner";
        public override string MouseButtonAction_Order => "Ordonner";
        public override string MouseButtonAction_Cancel => "Annuler";

        public override string MouseButton_Left => "Souris gauche";
        public override string MouseButton_Right => "Souris droite";
        public override string MouseButton_Middle => "Souris centre";
        public override string MouseButton_X1 => "Bouton souris X1";
        public override string MouseButton_X2 => "Bouton souris X2";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Descendre et annuler";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Descendre, ordonner et annuler";

        public override string GraphicsOption_Shadows => "Ombres";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modèles sur sol";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modèles sur modèles";

        public override string GraphicsOption_Shadow_MapResolution => "Résolution des ombres";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Ajoute {0} pixels";
        public override string Settings_KeyMapPanSpeed => "Vitesse de descente";
        public override string Settings_StoreCameraPosition => "Enregistrer la position caméra";
        public override string Settings_LoadCameraPosition => "Charger la position";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Écume de l’eau";
        public override string Settings_ModelShadow => "Ombres";
        public override string Settings_ModelShadowMapSize => "Taille de la Shadow Map";
        public override string Settings_Brightness => "Luminosité";
        public override string Settings_Mode_No_Achivements => "Succès non disponibles.";
        public override string Settings_FrameRate => "Taux de rafraîchissement";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Bloquer les succès pour les fichiers de sauvegarde importés";


    }
}
