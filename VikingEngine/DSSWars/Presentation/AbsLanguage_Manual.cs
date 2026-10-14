using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Presentation
{
    partial class AbsLanguage
    {
        //QoL Update
        public abstract string GameManual { get; }
        public abstract string GameManualTitle_Work { get; }
        public abstract string[] manual_work { get; }

        public abstract string GameManualTitle_Soldiers { get; }
        public abstract string[] manual_soldiers { get; }
        public abstract string[] manual_food { get; }
    }
}
