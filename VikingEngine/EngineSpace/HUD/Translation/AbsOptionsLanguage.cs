using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    abstract class AbsOptionsLanguage
    {
        public abstract string Options_title { get; }

        public abstract string InputSelect { get; }

        public abstract string InputKeyboardMouse { get; }
        public abstract string InputController { get; }
        public abstract string InputNotSet { get; }

        public abstract string VerticalSplitScreen { get; }
    }
}
