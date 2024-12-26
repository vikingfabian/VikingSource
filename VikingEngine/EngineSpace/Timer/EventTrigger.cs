using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace VikingEngine.Timer
{
    //interface IEventTriggerCallBack
    //{
    //    void EventTriggerCallBack(int type);
    //}
    //class TimedEventTrigger : AbsTimer
    //{
    //    IEventTriggerCallBack callbackObj;
    //    int type;

    //    public TimedEventTrigger(IEventTriggerCallBack callbackObj, float time, int type)
    //        : base(time, UpdateType.Lasy)
    //    {
    //        this.callbackObj = callbackObj;
    //        this.type = type;
    //    }
    //    protected override void timeTrigger()
    //    {
    //        callbackObj.EventTriggerCallBack(type);
    //    }
    //}

    class TimedAction0ArgTrigger : AbsTimer
    {
        Action method;

        public TimedAction0ArgTrigger(Action method, float time)
            : base(time, UpdateType.Lazy)
        {
            this.method = method;
        }
        protected override void timeTrigger()
        {
            method();
        }
    }

    class TimedAction1ArgTrigger<T> : AbsTimer
    {
        Action<T>  method;
        T arg1;

        public TimedAction1ArgTrigger(Action<T> method, T arg1, float time)
            : base(time, UpdateType.Lazy)
        {
            this.method = method;
            this.arg1 = arg1;
        }
        protected override void timeTrigger()
        {
            method(arg1);
        }
    }

    class TimedAction2ArgTrigger<T1, T2> : AbsTimer
    {
        Action<T1, T2> method;
        T1 arg1;
        T2 arg2;

        public TimedAction2ArgTrigger(Action<T1, T2> method, T1 arg1, T2 arg2, float time)
            : base(time, UpdateType.Lazy)
        {
            this.method = method;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        protected override void timeTrigger()
        {
            method(arg1, arg2);
        }
    }

    class TimedAction2ArgTrigger_InGame<T1, T2> : AbsInGameTrigger
    {
        Action<T1, T2> method;
        T1 arg1;
        T2 arg2;

        public TimedAction2ArgTrigger_InGame(Action<T1, T2> method, T1 arg1, T2 arg2, float timeSec)
            : base(timeSec)
        {
            this.method = method;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        protected override void timeTrigger()
        {
            method(arg1, arg2);
        }
    }

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    //class EventTrigger : OneTimeTrigger
    //{
    //    IEventTriggerCallBack callbackObj;
    //    int type;

    //    public EventTrigger(IEventTriggerCallBack callbackObj, int type)
    //        : base(false)
    //    {
    //        this.callbackObj = callbackObj;
    //        this.type = type;
    //        AddToUpdateList(true);
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        callbackObj.EventTriggerCallBack(type);
    //    }
    //}

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    class Action0ArgTrigger : OneTimeTrigger
    {
        Action eventTrigger;
        
        public Action0ArgTrigger(Action eventTrigger)
            : base(false)
        {
            this.eventTrigger = eventTrigger;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            eventTrigger();
        }
    }

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    class Action1ArgTrigger<T> : OneTimeTrigger
    {
        Action<T> eventTrigger;
        T arg1;

        public Action1ArgTrigger(Action<T> eventTrigger, T arg1)
            : base(false)
        {
            this.arg1 = arg1;
            this.eventTrigger = eventTrigger;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            eventTrigger(arg1);
        }
    }

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    class Action2ArgTrigger<T1, T2> : OneTimeTrigger
    {
        Action<T1, T2> eventTrigger;
        T1 arg1;
        T2 arg2;

        public Action2ArgTrigger(Action<T1, T2> eventTrigger, T1 arg1, T2 arg2)
            : base(false)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.eventTrigger = eventTrigger;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            eventTrigger(arg1, arg2);
        }
    }

    class Action3ArgTrigger<T1, T2, T3> : OneTimeTrigger
    {
        Action<T1, T2, T3> eventTrigger;
        T1 arg1;
        T2 arg2;
        T3 arg3;

        public Action3ArgTrigger(Action<T1, T2, T3> eventTrigger, T1 arg1, T2 arg2, T3 arg3)
            : base(false)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.eventTrigger = eventTrigger;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            eventTrigger(arg1, arg2, arg3);
        }
    }

    /// <summary>
    /// Trigger on next frame
    /// </summary>
    class ActionEventTimedTrigger : LazyUpdate
    {
        Action eventTrigger;
        Time time;
        public ActionEventTimedTrigger(Action eventTrigger, Time time)
            : base(false)
        {
            this.time = time;
            this.eventTrigger = eventTrigger;
            AddToUpdateList();
        }
        public override void Time_Update(float time)
        {
            if (this.time.CountDown(time))
            {
                eventTrigger();
                this.DeleteMe();
            }
        }
    }
}
