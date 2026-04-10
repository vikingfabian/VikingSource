// Auto-generated pt-BR localization (hybrid) — Options / HUD
// Theme: medieval / low fantasy, accessible gamer-friendly wording
using System;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    // Keep a distinct class name so it can live side-by-side with English
    class OptionsLanguage_Portuguese : AbsOptionsLanguage
    {
        //Mounts
        public override string InputSteam => "Steam Input";
        public override string Input_SimulateMouse => "Simular mouse";
        public override string Input_LockMouseToWindow => "Prender mouse na janela";
        public override string Input_MouseEdgePush_Title => "Rolagem de borda";
        public override string Input_NoControl => "Nenhum";
        public override string Input_ActiveControl => "Ativo";
        public override string Input_PassiveControl => "Passivo";
        public override string Setting_MinimapScale => "Escala do minimapa";

        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Desvanecer camadas";
        public override string SplitScreen_HorizontalFirst => "Horizontal primeiro";
        public override string SplitScreen_VerticalFirst => "Vertical primeiro";
        public override string SplitScreen_HorizontalOnly => "Apenas horizontal";
        public override string SplitScreen_VerticalOnly => "Apenas vertical";
        public override string SplitScreen_Title => "Tela dividida";
        public override string SplitScreen_AdjustSplit => "Ajustar divisão {0}";

        public override string Settings_ControllerVibration => "Vibração do controle";
        public override string GraphicsOption_IngameMenuWidth => "Largura do menu in-game";
        public override string DisplayMode => "Modo de exibição";
        public override string DisplayMode_Windowed => "Janela";
        public override string DisplayMode_BorderlessFullscreen => "Tela cheia sem bordas";
        public override string GameSettings_RenderedMouseCursor => "Cursor renderizado";
        public override string GameSettings_MuteControllerDisconnect => "Silenciar desconexão do controle";
        //--
        public override string GraphicsOption_FarViewDistance => "Visão de longa distância";

        public override string Hud_Cancel => "Cancelar";
        public override string Hud_Back => "Voltar";

        /// <summary>Submenu for when the player will make destructive choices</summary>
        public override string Hud_AreYouSure => "Tem certeza?";

        public override string Hud_OK => "OK";
        public override string Hud_Yes => "Sim";
        public override string Hud_No => "Não";

        /// <summary>Options menu title</summary>
        public override string Options_title => "Opções";

        /// <summary>Game control input options, 0: current input</summary>
        public override string InputSelect => "Entrada: {0}";

        /// <summary>Type of game input</summary>
        public override string InputKeyboardMouse => "Teclado e mouse";

        /// <summary>Type of game input</summary>
        public override string InputController => "Controle";

        /// <summary>No game input is selected</summary>
        public override string InputNotSet => "Não definido";

        /// <summary>Label for checkbox. Option for local split screen gameplay.</summary>
        public override string VerticalSplitScreen => "Dividir tela na vertical";

        /// <summary>Label for sound slider</summary>
        public override string SoundOption_MusicVolume => "Volume da música";

        /// <summary>Label for sound slider</summary>
        public override string SoundOption_SoundVolume => "Volume do som";

        /// <summary>Screen resolution</summary>
        public override string GraphicsOption_Resolution => "Resolução";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>Display the game fullscreen or window mode</summary>
        public override string GraphicsOption_Fullscreen => "Tela cheia";

        /// <summary>Oversize will make the game window be larger than the monitor, for multi monitor support</summary>
        public override string GraphicsOption_OversizeWidth => "Largura extra";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Largura";
        public override string GraphicsOption_OversizeHeight => "Altura extra";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Altura";
        public override string GraphicsOption_Oversize_None => "Nenhum";

        /// <summary>Specific resolutions for when recording to YouTube</summary>
        public override string GraphicsOption_RecordingPresets => "Presets de gravação";

        /// <summary>0: height resolution</summary>
        public override string GraphicsOption_YoutubePreset => "YouTube {0}p";

        /// <summary>Change size on text and icons</summary>
        public override string GraphicsOption_UiScale => "Escala da UI";

        //---
        public override string ReversedStereo => "Estéreo invertido";
        public override string Option_Low => "Baixo";
        public override string Option_Medium => "Médio";
        public override string Option_High => "Alto";
        public override string MouseSettings_Title => "Entrada do mouse";
        public override string KeyboardSettings_Title => "Mapeamento de teclas";
        public override string MouseButtonAction_None => "Sem ação";
        public override string MouseButtonAction_Select => "Selecionar";
        public override string MouseButtonAction_Pan => "Arrastar câmera";
        public override string MouseButtonAction_PanAndOrder => "Arrastar e ordenar";
        public override string MouseButtonAction_Order => "Ordenar";
        public override string MouseButtonAction_Cancel => "Cancelar";

        public override string MouseButton_Left => "Botão esquerdo";
        public override string MouseButton_Right => "Botão direito";
        public override string MouseButton_Middle => "Botão do meio";
        public override string MouseButton_X1 => "Botão X1 do mouse";
        public override string MouseButton_X2 => "Botão X2 do mouse";

        // DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Arrastar e cancelar";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Arrastar, ordenar e cancelar";

        public override string GraphicsOption_Shadows => "Sombras";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modelos no chão";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modelos em modelos";

        public override string GraphicsOption_Shadow_MapResolution => "Resolução do mapa de sombras";

        // DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Adicionar {0} pixels";
        public override string Settings_KeyMapPanSpeed => "Velocidade do pan";
        public override string Settings_StoreCameraPosition => "Salvar posição da câmera";
        public override string Settings_LoadCameraPosition => "Carregar posição";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Espuma da água";
        public override string Settings_ModelShadow => "Sombras";
        public override string Settings_ModelShadowMapSize => "Tamanho do Shadow Map";
        public override string Settings_Brightness => "Brilho";
        public override string Settings_Mode_No_Achivements => "Conquistas não disponíveis.";
        public override string Settings_FrameRate => "Taxa de quadros";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Bloquear conquistas para arquivos de salvamento importados";

    }
}
