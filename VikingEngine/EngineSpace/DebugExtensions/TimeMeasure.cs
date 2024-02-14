using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DebugExtensions
{
    struct TimeMeasure
    {
        string name;
        DateTime time;
        public TimeMeasure(string name)
        {
            this.name = name;
            time = DateTime.Now;
        }
        public void EndMeasure()
        { 
            Debug.Log(DebugLogType.MSG, "TIME, " + name + ": " + DateTime.Now.Subtract(time).TotalMilliseconds);
        }
    }
}
