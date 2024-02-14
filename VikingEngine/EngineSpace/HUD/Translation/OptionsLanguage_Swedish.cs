using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine.HUD
{
    class OptionsLanguage_Swedish : AbsOptionsLanguage
    {
        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Alternativ";

        /// <summary>
        /// Open input options, 0: current input
        /// </summary>
        public override string InputSelect => "Inmatning: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Tangentbord & mus";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Kontroller";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Inte inställd";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Vertikal skärmdelning";
    }
}
