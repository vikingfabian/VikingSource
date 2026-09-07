using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Translation;

namespace VikingEngine.EngineSpace.Translation.OptionLanguages
{
    class OptionsLanguage_Spanish : AbsOptionsLanguage
    {

        public override string GameSettings_UltraWide => "Ultrawide";
        public override string GameSettings_UltraWide_LeftEdge => "Borde izquierdo de la UI";
        public override string GameSettings_UltraWide_RightEdge => "Borde derecho de la UI";
        //-------
        public override string GameSettings_WideScrollbar => "Barra de desplazamiento ancha";
        public override string GameSettings_DisplayInputHelp => "Ayuda de controles";
        public override string GameSettings_InputSmoothing => "Suavizado de controles";

        //Mounts
        public override string InputSteam => "Steam Input";
        public override string Input_SimulateMouse => "Simular ratón";
        public override string Input_LockMouseToWindow => "Bloquear ratón en la ventana";
        public override string Input_MouseEdgePush_Title => "Desplazamiento en los bordes";
        public override string Input_NoControl => "Ninguno";
        public override string Input_ActiveControl => "Activo";
        public override string Input_PassiveControl => "Pasivo";
        public override string Setting_MinimapScale => "Escala del minimapa";

        //##Settings
        public override string Settings_Particles_FadeMapLayers => "Desvanecer capas";
        public override string SplitScreen_HorizontalFirst => "Horizontal primero";
        public override string SplitScreen_VerticalFirst => "Vertical primero";
        public override string SplitScreen_HorizontalOnly => "Solo horizontal";
        public override string SplitScreen_VerticalOnly => "Solo vertical";
        public override string SplitScreen_Title => "Pantalla dividida";
        public override string SplitScreen_AdjustSplit => "Ajustar división {0}";

        public override string Settings_ControllerVibration => "Vibración del mando";
        public override string GraphicsOption_IngameMenuWidth => "Ancho del menú del juego";
        public override string DisplayMode => "Modo de visualización";
        public override string DisplayMode_Windowed => "Ventana";
        public override string DisplayMode_BorderlessFullscreen => "Pantalla completa sin bordes";
        public override string GameSettings_RenderedMouseCursor => "Cursor renderizado";
        public override string GameSettings_MuteControllerDisconnect => "Silenciar desconexión del mando";
        //--
        public override string GraphicsOption_FarViewDistance => "Vista de larga distancia";
        public override string Hud_Cancel => "Cancelar";
        public override string Hud_Back => "Atrás";

        /// <summary>
        /// Submenú para cuando el jugador tomará decisiones destructivas
        /// </summary>
        public override string Hud_AreYouSure => "¿Estás seguro?";

        public override string Hud_OK => "Aceptar";
        public override string Hud_Yes => "Sí";
        public override string Hud_No => "No";

        /// <summary>
        /// Título del menú de opciones
        /// </summary>
        public override string Options_title => "Opciones";

        /// <summary>
        /// Opciones de entrada del control del juego, 0: entrada actual
        /// </summary>
        public override string InputSelect => "Entrada: {0}";

        /// <summary>
        /// Tipo de entrada del juego
        /// </summary>
        public override string InputKeyboardMouse => "Teclado y ratón";

        /// <summary>
        /// Tipo de entrada del juego
        /// </summary>
        public override string InputController => "Controlador";

        /// <summary>
        /// No se ha seleccionado ninguna entrada del juego
        /// </summary>
        public override string InputNotSet => "No configurado";

        /// <summary>
        /// Etiqueta para la casilla de verificación. Opción para el juego en pantalla dividida local.
        /// </summary>
        public override string VerticalSplitScreen => "Pantalla dividida vertical";

        /// <summary>
        /// Etiqueta para el control deslizante de sonido
        /// </summary>
        public override string SoundOption_MusicVolume => "Volumen de la música";

        /// <summary>
        /// Etiqueta para el control deslizante de sonido
        /// </summary>
        public override string SoundOption_SoundVolume => "Volumen del sonido";

        /// <summary>
        /// Resolución de la pantalla
        /// </summary>
        public override string GraphicsOption_Resolution => "Resolución";
        public override string GraphicsOption_Resolution_PercentageOption => "{0}%";

        /// <summary>
        /// Mostrar el juego en pantalla completa o en modo ventana
        /// </summary>
        public override string GraphicsOption_Fullscreen => "Pantalla completa";

        /// <summary>
        /// El tamaño excesivo hará que la ventana del juego sea más grande que el monitor, para soporte de múltiples monitores
        /// </summary>
        public override string GraphicsOption_OversizeWidth => "Ancho excesivo";
        public override string GraphicsOption_PercentageOversizeWidth => "{0}% Ancho";
        public override string GraphicsOption_OversizeHeight => "Altura excesiva";
        public override string GraphicsOption_PercentageOversizeHeight => "{0}% Altura";
        public override string GraphicsOption_Oversize_None => "Ninguno";

        /// <summary>
        /// Resoluciones específicas para cuando se graba para Youtube
        /// </summary>
        public override string GraphicsOption_RecordingPresets => "Preajustes de grabación";

        /// <summary>
        /// 0: resolución de altura
        /// </summary>
        public override string GraphicsOption_YoutubePreset => "Youtube {0}p";

        /// <summary>
        /// Cambiar el tamaño del texto y los iconos
        /// </summary>
        public override string GraphicsOption_UiScale => "Escala de la interfaz";

        public override string ReversedStereo => "Estéreo invertido";
        public override string Option_Low => "Bajo";
        public override string Option_Medium => "Medio";
        public override string Option_High => "Alto";

        public override string MouseSettings_Title => "Entrada del ratón";
        public override string KeyboardSettings_Title => "Asignación de teclas";

        public override string MouseButtonAction_None => "Sin acción";
        public override string MouseButtonAction_Select => "Seleccionar";
        public override string MouseButtonAction_Pan => "Desplazar";
        public override string MouseButtonAction_PanAndOrder => "Desplazar y ordenar";
        public override string MouseButtonAction_Order => "Ordenar";
        public override string MouseButtonAction_Cancel => "Cancelar";

        public override string MouseButton_Left => "Botón izquierdo";
        public override string MouseButton_Right => "Botón derecho";
        public override string MouseButton_Middle => "Botón central";
        public override string MouseButton_X1 => "Botón X1 del ratón";
        public override string MouseButton_X2 => "Botón X2 del ratón";

        //DEMO PATCH 4
        public override string MouseButtonAction_PanAndCancel => "Desplazar y cancelar";
        public override string MouseButtonAction_PanAndOrderAndCancel => "Desplazar, ordenar y cancelar";

        public override string GraphicsOption_Shadows => "Sombras";
        public override string GraphicsOption_ShadowType_ModelsToGround => "Modelos al suelo";
        public override string GraphicsOption_ShadowType_ModelsToModels => "Modelos a modelos";

        public override string GraphicsOption_Shadow_MapResolution => "Resolución del mapa de sombras";

        //DEMO PATCH 5
        public override string GraphicsOption_RecordingPresets_AddXPixels => "Agregar {0} píxeles";
        public override string Settings_KeyMapPanSpeed => "Velocidad de desplazamiento";
        public override string Settings_StoreCameraPosition => "Guardar posición de la cámara";
        public override string Settings_LoadCameraPosition => "Cargar posición";


        //Shadow update
        public override string Settings_ModelWaterFoam => "Espuma del agua";
        public override string Settings_ModelShadow => "Sombras";
        public override string Settings_ModelShadowMapSize => "Tamaño del Shadow Map";
        public override string Settings_Brightness => "Brillo";
        public override string Settings_Mode_No_Achivements => "Logros no disponibles.";
        public override string Settings_FrameRate => "Tasa de fotogramas";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public override string Settings_ImportNoAchievement => "Bloquear logros para archivos de guardado importados";


    }
}
