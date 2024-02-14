using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DebugExtensions
{
    class CrashTimer : OneTimeTrigger
    {
        public CrashTimer()
            : base(true)
        {

        }
        public override void Time_Update(float time)
        {
            throw new Exception("Crash Testing");
        }
    }
}
