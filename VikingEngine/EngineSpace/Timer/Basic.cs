using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    struct Basic
    {
        public float goalTime;
        float time;
        bool active;
        bool autoReset;
        
        public Basic(float timeset)
            : this(timeset, false)
        {
        }
        public Basic(float timeset, bool autoReset)
        {
            this.autoReset = autoReset;
            time = 0;
            this.goalTime = timeset; active = true; 
        }

        public void Reset()
        {
            time = 0;
            active = true;
        }
        public void Set(IntervalF timeset)
        {
            this.Set(timeset.GetRandom());
        }
        public void Set(float timeset)
        {
            goalTime = timeset;
            Reset();
        }
        public bool Update()
        {
            return this.Update(Ref.DeltaTimeMs);
        }
        public bool UpdateInGame()
        {
            return this.Update(Ref.DeltaGameTimeMs);
        }
        public bool Update(float deltaTime)
        {
            if (active)
            {
                time += deltaTime;
                if (time >= goalTime)
                {
                    if (autoReset)
                    { Reset(); }
                    else
                    { 
                        active = false;
                        time = goalTime;
                    }
                    return true;
                }
            }
            return false;
        }
        public float TimeLeft
        {
            get { return goalTime - time; }
            set { time = goalTime - value; }
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
                return time / goalTime;
            }
        }
        public bool Active => active;
        //{ get { return active; } }
        public bool TimeOut => !active;
        //{ get { return !active; } }

    }
}
