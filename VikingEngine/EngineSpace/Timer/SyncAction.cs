using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;

namespace VikingEngine
{
    interface ISyncAction
    {
        void execute();
    }
    struct SyncAction : ISyncAction
    {
        public Action action;

        public SyncAction(Action action)
        {
            this.action = action;
        }
        public SyncAction(Action action1, Action action2)
        {
            this.action = action1 + action2;
        }
        public void execute()
        {
            action();
        }
    }

    struct SyncActionIndex : ISyncAction
    {
        public ActionIndexEvent action;
        int index;

        public SyncActionIndex(ActionIndexEvent action, int index)
        {
            this.action = action;
            this.index = index;
        }

        public void execute()
        {
            action(index);
        }
    }

    struct SyncAction1Arg<Arg1> : ISyncAction
    {
        Action<Arg1> action;
        Arg1 arg1;

        public SyncAction1Arg(Action<Arg1> action, Arg1 arg1)
        {
            this.action = action;
            this.arg1 = arg1;
        }
        public void execute()
        {
            action(arg1);
        }
    }

    struct SyncAction2Arg<Arg1, Arg2> : ISyncAction
    {
        Action<Arg1, Arg2> action;
        Arg1 arg1;
        Arg2 arg2;

        public SyncAction2Arg(Action<Arg1, Arg2> action, Arg1 arg1, Arg2 arg2)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        public void execute()
        {
            action(arg1, arg2);
        }
    }

    struct SyncAction2ArgNoReturn<Arg1, Arg2> : ISyncAction
    {
        public delegate object GuiNoReturn2Arg(Arg1 arg1, Arg2 arg2);

        GuiNoReturn2Arg action;
        Arg1 arg1;
        Arg2 arg2;

        public SyncAction2ArgNoReturn(GuiNoReturn2Arg action, Arg1 arg1, Arg2 arg2)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        public void execute()
        {
            action(arg1, arg2);
        }
    }

    struct SyncAction3Arg<Arg1, Arg2, Arg3> : ISyncAction
    {
        Action<Arg1, Arg2, Arg3> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;

        public SyncAction3Arg(Action<Arg1, Arg2, Arg3> action, Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
        public void execute()
        {
            action(arg1, arg2, arg3);
        }
    }

    struct SyncAction4Arg<Arg1, Arg2, Arg3, Arg4> : ISyncAction
    {
        Action<Arg1, Arg2, Arg3, Arg4> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;
        Arg4 arg4;

        public SyncAction4Arg(Action<Arg1, Arg2, Arg3, Arg4> action, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
        }
        public void execute()
        {
            action(arg1, arg2, arg3, arg4);
        }
    }

    struct SyncAction5Arg<Arg1, Arg2, Arg3, Arg4, Arg5> : ISyncAction
    {
        Action<Arg1, Arg2, Arg3, Arg4, Arg5> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;
        Arg4 arg4;
        Arg5 arg5;

        public SyncAction5Arg(Action<Arg1, Arg2, Arg3, Arg4, Arg5> action, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4, Arg5 arg5)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
        }
        public void execute()
        {
            action(arg1, arg2, arg3, arg4, arg5);
        }
    }
}
