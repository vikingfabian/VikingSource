using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    struct GameTimer
    {
        public float totalGameTimeStampSec;
        public float goalTimeSec;
        public bool autoReset;
        public bool active;

        public GameTimer(float goalTimeSec, bool autoReset = false, bool startNow = true)
        {
            totalGameTimeStampSec = 0;
            active = false;

            this.goalTimeSec = goalTimeSec;
            this.autoReset = autoReset;

            if (startNow)
            {
                Reset();
            }
        }

        public void Reset()
        {
            totalGameTimeStampSec = Ref.TotalGameTimeSec;
            active = true;
        }

        public float GetPerc()
        {
            if (active)
            {
                float timePass = Ref.TotalGameTimeSec - totalGameTimeStampSec;
                return timePass / goalTimeSec;
            }
            return 1f;
        }

        public void SetPerc(float perc)
        {
            float timePass = goalTimeSec * perc;
            totalGameTimeStampSec = Ref.TotalGameTimeSec - timePass;
        }

        void onTimeOut()
        {
            if (autoReset)
            {
                Reset();
            }
            else
            {
                active = false;
            }
        }

        public bool HasTime
        {
            get
            {
                if (active)
                {
                    bool timeOut = totalGameTimeStampSec + goalTimeSec <= Ref.TotalGameTimeSec;
                    if (timeOut)
                    {
                        onTimeOut();
                    }
                    return !timeOut;
                }

                return false;
            }
        }

        public bool TimeOut
        {
            get
            {
                if (active)
                {
                    bool timeOut = totalGameTimeStampSec + goalTimeSec <= Ref.TotalGameTimeSec;
                    if (timeOut)
                    {
                        onTimeOut();
                    }
                    return timeOut;
                }

                return true;
            }
        }

        /// <summary>
        /// Time out this frame
        /// </summary>
        public bool TimeOut_Event
        {
            get
            {
                if (active)
                {
                    bool timeOutEvent = totalGameTimeStampSec + goalTimeSec <= Ref.TotalGameTimeSec &&
                        totalGameTimeStampSec + goalTimeSec > Ref.PrevTotalGameTimeSec;
                    if (timeOutEvent)
                    {
                        onTimeOut();
                    }
                    return timeOutEvent;
                }

                return false;
            }
        }

        public bool timeOut(out bool timeOutEvent)
        {
            
            //if (active)
            //{
            bool timeOut = !active || 
                totalGameTimeStampSec + goalTimeSec <= Ref.TotalGameTimeSec;

            if (timeOut)
            {
                timeOutEvent = totalGameTimeStampSec + goalTimeSec > Ref.PrevTotalGameTimeSec;
                if (timeOutEvent)
                {
                    onTimeOut();
                }
            }
            else
            {
                timeOutEvent = false;
            }

            return timeOut;
            //}

            //timeOutEvent = false;
            //return false;
        }
    }
}
