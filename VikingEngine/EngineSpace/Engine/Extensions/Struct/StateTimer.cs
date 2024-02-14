using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    struct StateTimer
    {
        Time time;
        Time previuosTime;

        public void update()
        {
            previuosTime = time;
            time.AddTime();
        }

        public bool passedTime(float ms)
        {
            return time.MilliSeconds >= ms &&
                previuosTime.MilliSeconds < ms;

        }
    }
}
