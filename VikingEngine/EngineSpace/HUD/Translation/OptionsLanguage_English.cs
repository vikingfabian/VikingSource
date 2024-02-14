using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class OptionsLanguage_English : AbsOptionsLanguage
    {
        /// <summary>
        /// Options menu title
        /// </summary>
        public override string Options_title => "Options";

        /// <summary>
        /// Game control input options, 0: current input
        /// </summary>
        public override string InputSelect => "Input: {0}";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputKeyboardMouse => "Keyboard & mouse";

        /// <summary>
        /// Type of game input
        /// </summary>
        public override string InputController => "Controller";

        /// <summary>
        /// No game input is selected
        /// </summary>
        public override string InputNotSet => "Not set";

        /// <summary>
        /// Label for checkbox. Option for local split screen gameplay.
        /// </summary>
        public override string VerticalSplitScreen => "Vertical screen split";
    }
}
