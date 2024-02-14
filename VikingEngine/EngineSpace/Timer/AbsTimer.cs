using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    abstract class AbsTimer : AbsUpdateable
    {
        protected float timeLeft;
        protected UpdateType updateType;

        public AbsTimer(float lifeTime, UpdateType updateType)
            :base(false)
        {
            this.updateType = updateType;
            timeLeft = lifeTime;
            AddToOrRemoveFromUpdateList(true);
        }

        public override void  Time_Update(float time)
        {
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                timeTrigger();
                DeleteMe();
            }
        }
        virtual protected void timeTrigger()
        { }
        virtual public void Reset() { }

        virtual public void PreDeleteMe(Engine.GameState state)
        { this.AddToOrRemoveFromUpdateList(false); } 

        virtual public float TimeLeft
        {
            get { return timeLeft; }
        }
        public override UpdateType UpdateType
        {
            get { return updateType; }
        }
    }

    abstract class AbsRepeatingTrigger : AbsTimer
    {
        float triggerTime;
        public AbsRepeatingTrigger(float lifeTime, UpdateType updateType)
            : base(lifeTime, updateType)
        {
            triggerTime = lifeTime;
        }
        public override void Time_Update(float time)
        {
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                timeTrigger();
                timeLeft += triggerTime;
            }
        }
    }

    class RepeatingActionTrigger : AbsUpdateable
    {
        Action triggerEvent;
        float timeBetweenTriggers;
        int numTriggerTimes;
        Time timer;

        public RepeatingActionTrigger(Action triggerEvent, float timeToFirst, float timeBetweenTriggers, int numTriggerTimes)
            :base(true)
        {
            this.triggerEvent = triggerEvent;
            this.timeBetweenTriggers = timeBetweenTriggers;
            this.numTriggerTimes = numTriggerTimes;
            timer = new Time(timeToFirst);
        }

        public override void Time_Update(float time)
        {
            if (timer.CountDown(time))
            {
                triggerEvent();
                timer.MilliSeconds = timeBetweenTriggers;
                --numTriggerTimes;
                if (numTriggerTimes <= 0)
                {
                    DeleteMe();
                }
            }
        }
    }
}
