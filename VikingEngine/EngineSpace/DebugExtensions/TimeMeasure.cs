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
        public TimeMeasure()
            : this("measure")
        { }
        public TimeMeasure(string name)
        {
            this.name = name;
            time = DateTime.Now;
        }
        public void EndMeasure()
        {
            System.Diagnostics.Debug.WriteLine("TIME, " + name + ": " + DateTime.Now.Subtract(time).TotalMilliseconds);
        }
    }
}
