using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class AsynchActionTrigger : IQuedObject
    {
        Action action;

        public AsynchActionTrigger(Action action)
            : this(action, false)
        { }

        public AsynchActionTrigger(Action action, bool accessStorage)
        {
            this.action = action;

            TaskExt.AddTask(this, accessStorage);
            //if (accessStorage)
            //    Engine.Storage.AddToSaveQue(StartQuedProcess, false);
            //else
            //    Ref.asynchUpdate.AddThreadQueObj(this);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            action();
        }
    }

    class AsynchActionIndexTrigger : IQuedObject
    {
        ActionIndexEvent action;
        int index;

        public AsynchActionIndexTrigger(ActionIndexEvent action, int index)
            : this(action, index, false)
        { }

        public AsynchActionIndexTrigger(ActionIndexEvent action, int index, bool accessStorage)
        {
            this.action = action;
            this.index = index;


            TaskExt.AddTask(this, accessStorage);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            action(index);
        }
    }

    class AsynchActionDoubleIndexTrigger : IQuedObject
    {
        ActionDoubleIndexEvent action;
        int index1, index2;

        public AsynchActionDoubleIndexTrigger(ActionDoubleIndexEvent action, int index1, int index2)
            : this(action, index1, index2, false)
        { }

        public AsynchActionDoubleIndexTrigger(ActionDoubleIndexEvent action, int index1, int index2, bool accessStorage)
        {
            this.action = action;
            this.index1 = index1;
            this.index2 = index2;

            TaskExt.AddTask(this, accessStorage);
            //if (accessStorage)
            //    Engine.Storage.AddToSaveQue(StartQuedProcess, false);
            //else
            //    Ref.asynchUpdate.AddThreadQueObj(this);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            action(index1, index2);
        }
    }

    class Asynch1ArgTrigger<T> : IQuedObject
    {
        Action<T> action;
        T arg1;

        public Asynch1ArgTrigger(Action<T> action, T arg1)
            : this(action, arg1, false)
        { }

        public Asynch1ArgTrigger(Action<T> action, T arg1, bool accessStorage)
        {
            this.action = action;
            this.arg1 = arg1;

            TaskExt.AddTask(this, accessStorage);
            //if (accessStorage)
            //    Engine.Storage.AddToSaveQue(StartQuedProcess, false);
            //else
            //    Ref.asynchUpdate.AddThreadQueObj(this);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            action(arg1);
        }
    }

    class Asynch2ArgTrigger<T1, T2> : IQuedObject
    {
        Action<T1, T2> action;
        T1 arg1;
        T2 arg2;

        public Asynch2ArgTrigger(Action<T1, T2> action, T1 arg1, T2 arg2)
            : this(action, arg1, arg2, false)
        { }

        public Asynch2ArgTrigger(Action<T1, T2> action, T1 arg1, T2 arg2, bool accessStorage)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;

            TaskExt.AddTask(this, accessStorage);
            //if (accessStorage)
            //    Engine.Storage.AddToSaveQue(StartQuedProcess, false);
            //else
            //    Ref.asynchUpdate.AddThreadQueObj(this);
        }

        public void runQuedTask(MultiThreadType threadType)
        {
            action(arg1, arg2);
        }
    }
}
