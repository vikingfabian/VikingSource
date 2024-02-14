using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    struct Time
    {
        public static readonly Time Zero = new Time(0);

        const float SecondLength = 1000;
        const float MinuteLength = 60 * SecondLength;
        const float MsToSec = 1f / SecondLength;
        public float MilliSeconds;

        public Time(float milliSeconds)
        {
            this.MilliSeconds = milliSeconds;
        }
        public Time(float value, TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Milliseconds:
                    MilliSeconds = value;
                    break;
                case TimeUnit.Seconds:
                    MilliSeconds = value * SecondLength;
                    break;
                case TimeUnit.Minutes:
                    MilliSeconds = value * MinuteLength;
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        public static implicit operator Time(float value)
        {
            return new Time(value);
        }

        public bool setZero()
        {
            bool hadTimeLeft = MilliSeconds > 0;
            MilliSeconds = 0f;
            return hadTimeLeft;
        }

        public bool CountDownGameTime()
        {
            return this.CountDown(Ref.DeltaGameTimeMs);
        }

        public bool CountDownGameTime(float time)
        {
            return this.CountDown(time * Ref.GameTimeSpeed);
        }

        public bool CountDown()
        {
            return this.CountDown(Ref.DeltaTimeMs);
        }
        public bool CountDown(float time)
        {
            MilliSeconds -= time;
            return MilliSeconds <= 0;
        }

        public bool CountDown_IfActive()
        {
            if (MilliSeconds > 0)
            {
                MilliSeconds -= Ref.DeltaTimeMs;
                return MilliSeconds <= 0;
            }
            return false;
        }

        public bool CountDownGameTime_IfActive()
        {
            if (MilliSeconds > 0)
            {
                MilliSeconds -= Ref.DeltaGameTimeMs;
                return MilliSeconds <= 0;
            }
            return false;
        }

        public void AddTime()
        {
            MilliSeconds += Ref.DeltaTimeMs;
        }
        public void AddGameTime()
        {
            MilliSeconds += Ref.DeltaGameTimeMs;
        }

        public bool AddGameTime_ReachGoalSeconds(float goalSeconds)
        {
            AddGameTime();
            return this.Seconds >= goalSeconds;
        }


        public bool TimeOut { get { return MilliSeconds <= 0; } }
        public bool HasTime { get { return MilliSeconds > 0; } }

        public float Seconds
        {
            get
            {
                return MilliSeconds * MsToSec;
            }
            set
            {
                MilliSeconds = value * SecondLength;
            }
        }
        public float Minutes
        {
            get
            {
                return MilliSeconds / MinuteLength;
            }
            set
            {
                MilliSeconds = value * MinuteLength;
            }
        }

        public void AddSeconds(float seconds)
        {
            MilliSeconds += seconds * SecondLength;
        }

        public void AddMinutes(float minutes)
        {
            MilliSeconds += minutes * MinuteLength;
        }

        public void splitToUnits(out int minutes, out int seconds, out int millisec)
        {
            float ms = MilliSeconds;

            minutes = (int)(ms / MinuteLength);
            ms -= minutes * MinuteLength;

            seconds = (int)(ms / SecondLength);
            ms -= seconds * SecondLength;

            millisec = (int)ms;
        }

        public override string ToString()
        {
            return MilliSeconds.ToString() + "ms";
        }        
    }
}
