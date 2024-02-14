using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    class ButtonMotion : Graphics.Motion2d
    {
        public AbsButtonGui button;

        public ButtonMotion(MotionType type, AbsButtonGui button, AbsDraw2D objImage,
            Vector2 valueChange, MotionRepeate repeateType,
            float timeMS, bool addToUpdateList)
            :base(type, objImage, valueChange, repeateType, timeMS, addToUpdateList)
        {
            this.button = button;
        }

        protected override bool UpdateMotion(float time)
        {
            bool r = base.UpdateMotion(time);
            button.refresh();
            return r;
        }
    }
}
