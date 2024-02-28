using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    abstract class AbsRbAction
    {
        public RbSoundProfile sound;

        public bool enabled = true;
        abstract public void actionTrigger();
    }

    class RbAction : AbsRbAction
    {
        public Action action;

        public RbAction(Action action, RbSoundProfile sound = null)
        {
            this.action = action;
            this.sound = sound; 
        }

        public override void actionTrigger()
        {
            sound?.onActionTrigger(enabled);
            if (enabled)
            {
                action.Invoke();
            }
        }
    }

    class RbAction1Arg<Arg1> : AbsRbAction
    {
        public Action<Arg1> action;
        Arg1 arg1;

        public RbAction1Arg(Action<Arg1> action, Arg1 arg1, RbSoundProfile sound = null)
        {
            this.action = action;
            this.arg1 = arg1;
            this.sound = sound;
        }

        public override void actionTrigger()
        {
            sound?.onActionTrigger(enabled);
            if (enabled)
            {
                action.Invoke(arg1);
            }
        }
    }

    class RbAction2Arg<Arg1, Arg2> : AbsRbAction
    {
        public Action<Arg1, Arg2> action;
        Arg1 arg1; 
        Arg2 arg2;

        public RbAction2Arg(Action<Arg1, Arg2> action, Arg1 arg1, Arg2 arg2, RbSoundProfile sound = null)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.sound = sound;
        }

        public override void actionTrigger()
        {
            sound?.onActionTrigger(enabled);
            if (enabled)
            {
                action.Invoke(arg1, arg2);
            }
        }
    }

    class RbAction3Arg<Arg1, Arg2, Arg3> : AbsRbAction
    {
        public Action<Arg1, Arg2, Arg3> action;
        Arg1 arg1;
        Arg2 arg2;
        Arg3 arg3;

        public RbAction3Arg(Action<Arg1, Arg2, Arg3> action, Arg1 arg1, Arg2 arg2, Arg3 arg3, RbSoundProfile sound = null)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.sound = sound;
        }

        public override void actionTrigger()
        {
            sound?.onActionTrigger(enabled);
            if (enabled)
            {
                action.Invoke(arg1, arg2, arg3);
            }
        }
    }

    class RbAction_ChangeInt : AbsRbAction
    {
        Action refreshCAllback;
        IntGetSetIx property;
        int propertyIx;
        int add;
        public RbAction_ChangeInt(IntGetSetIx property, int propertyIx, int add, Action refreshCAllback)
        {
            this.refreshCAllback= refreshCAllback;
            this.property = property;
            this.propertyIx = propertyIx; 
            this.add = add;
        }
        public override void actionTrigger()
        {
            int value = property(propertyIx, false, 0);
            property(propertyIx, true, value + add);

            refreshCAllback.Invoke();
        }
    }
}
