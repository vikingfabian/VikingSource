using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class OptionsLanguage_Spanish : AbsOptionsLanguage
    {
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
    }
}
