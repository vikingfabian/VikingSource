using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    interface IGuiAction
    {
        void guiActionTrigger(Gui menu, GuiMember menuMember);
    }
    delegate void ActionOpenPage(Gui menu);

    struct GuiNoAction : IGuiAction
    {
        public void guiActionTrigger(Gui menu, GuiMember menuMember) { }
    }

    /// <summary>
    /// Will trigger action and go back one step in menu
    /// </summary>
    struct GuiActionAndReturn
    {
        IGuiAction action;
        public GuiActionAndReturn(IGuiAction action)
        {
            this.action = action;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action.guiActionTrigger(menu, menuMember);
            menu.PopLayout();
        }
    }

    struct GuiAction : IGuiAction
    {
        public Action action;

        public GuiAction(Action action)
        {
            this.action = action;
        }
        public GuiAction(Action action1, Action action2)
        {
            this.action = action1 + action2;
        }

        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action();
        }
    }

    struct GuiActionIndex : IGuiAction
    {
        public ActionIndexEvent action;
        int index;

        public GuiActionIndex(ActionIndexEvent action, int index)
        {
            this.action = action;
            this.index = index;
        }

        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(index);
        }
    }

    struct GuiAction1Arg<Arg1> : IGuiAction
    {
        Action<Arg1> action;
        Arg1 arg1;

        public GuiAction1Arg(Action<Arg1> action, Arg1 arg1)
        {
            this.action = action;
            this.arg1 = arg1;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1);
        }
    }

    struct GuiAction2Arg<Arg1, Arg2> : IGuiAction
    {
        Action<Arg1, Arg2> action;
        Arg1 arg1;
        Arg2 arg2;

        public GuiAction2Arg(Action<Arg1, Arg2> action, Arg1 arg1, Arg2 arg2)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1, arg2);
        }
    }

    struct GuiAction2ArgNoReturn<Arg1, Arg2> : IGuiAction
    {
        public delegate object GuiNoReturn2Arg(Arg1 arg1, Arg2 arg2);

        GuiNoReturn2Arg action;
        Arg1 arg1;
        Arg2 arg2;

        public GuiAction2ArgNoReturn(GuiNoReturn2Arg action, Arg1 arg1, Arg2 arg2)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1, arg2);
        }
    }

    struct GuiAction3Arg<Arg1, Arg2, Arg3> : IGuiAction
    {
        Action<Arg1, Arg2, Arg3> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;

        public GuiAction3Arg(Action<Arg1, Arg2, Arg3> action, Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1, arg2, arg3);
        }
    }

    struct GuiAction4Arg<Arg1, Arg2, Arg3, Arg4> : IGuiAction
    {
        Action<Arg1, Arg2, Arg3, Arg4> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;
        Arg4 arg4;

        public GuiAction4Arg(Action<Arg1, Arg2, Arg3, Arg4> action, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1, arg2, arg3, arg4);
        }
    }

    struct GuiAction5Arg<Arg1, Arg2, Arg3, Arg4, Arg5> : IGuiAction
    {
        Action<Arg1, Arg2, Arg3, Arg4, Arg5> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;
        Arg4 arg4;
        Arg5 arg5;

        public GuiAction5Arg(Action<Arg1, Arg2, Arg3, Arg4, Arg5> action, Arg1 arg1, Arg2 arg2, Arg3 arg3, Arg4 arg4, Arg5 arg5)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
        }
        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(arg1, arg2, arg3, arg4, arg5);
        }
    }

    struct GuiActionOpenPage : IGuiAction
    {
        public ActionOpenPage action;

        public GuiActionOpenPage(ActionOpenPage action)
        {
            this.action = action;
        }

        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            action(menu);
        }
    }

    struct GuiActionWithAreYouSureDialogue : IGuiAction
    {
        public Action action;

        public GuiActionWithAreYouSureDialogue(Action action)
        {
            this.action = action;
        }

        public void guiActionTrigger(Gui menu, GuiMember menuMember)
        {
            new GuiLayout().AreYouSure_Layout(menu, menuMember, action);
        }
    }


}
