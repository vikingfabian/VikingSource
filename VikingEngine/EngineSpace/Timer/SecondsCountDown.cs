using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    struct SecondsCountDown
    {
        public float oneSecond;
        public int secondsLeft;
        public float secondLength;

        public SecondsCountDown(int seconds)
            :this()
        {
            set(seconds);
            secondLength = 1f;
        }

        public void set(int seconds)
        {
            this.secondsLeft = seconds;
            oneSecond = 0f;
        }

        public bool update()
        {
            oneSecond += Ref.DeltaTimeSec;
            if (oneSecond >= secondLength)
            {
                oneSecond -= secondLength;
                secondsLeft--;
                return true;
            }

            return false;
        }
    }
}
