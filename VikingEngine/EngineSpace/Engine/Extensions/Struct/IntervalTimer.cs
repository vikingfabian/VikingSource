using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{    
    struct IntervalTimer
    {
        public Time time, goalTime;
        bool active;
        bool autoReset;

        public IntervalTimer(float milliSeconds, bool autoReset = true, bool setAtGoalTime = false)
        {
            goalTime = new Time(milliSeconds);
            this.autoReset = autoReset;

            if (setAtGoalTime)
            {
                active = autoReset;
                time = goalTime;
            }
            else
            {
                active = true;
                time = Time.Zero;
            }
        }

        public void Set(float milliSeconds)
        {
            goalTime = new Time(milliSeconds);
            Reset();
        }

        public void Reset()
        {
            time = Time.Zero;
            active = true;
        }

        public bool CountDown()
        {
            return this.CountDown(Ref.DeltaTimeMs);
        }
        public bool CountDownGameTime()
        {
            return this.CountDown(Ref.DeltaGameTimeMs);
        }

        public bool CountDown_IfActive()
        {
            if (active)
            {
                return this.CountDown(Ref.DeltaTimeMs);
            }
            return false;
        }

        public bool CountDown(float deltaTime)
        {
            if (active)
            {
                time.MilliSeconds += deltaTime;
                if (time.MilliSeconds >= goalTime.MilliSeconds)
                {
                    if (autoReset)
                    {
                        time.MilliSeconds -= goalTime.MilliSeconds;
                    }
                    else
                    {
                        active = false;
                    }
                    return true;
                }
            }
            return false;
        }
        public float TimeLeft
        {
            get { return goalTime.MilliSeconds - time.MilliSeconds; }
            set { time.MilliSeconds = goalTime.MilliSeconds - value; }
        }
        public void Stop()
        {
            active = false;
        }
        public void SetZeroTimeLeft()
        {
            time = goalTime;
        }
        public float PercentTimePassed
        {
            get
            {
                return time.MilliSeconds / goalTime.MilliSeconds;
            }
        }
        public bool Active => active;
        //{ get { return active; } }
        public bool TimeOut => !active;
        //{ get { return !active; } }

        public override string ToString()
        {
            return time.MilliSeconds.ToString() + "/" + goalTime.MilliSeconds.ToString() + "ms";
        }
    }
}
