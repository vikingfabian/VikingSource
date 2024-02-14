using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.Effects
{
    class OrderUnitEffect : AbsUpdateable
    {
        Time lifeTime = 5000;
        Graphics.Motion3d bumpMoition;
        Graphics.Mesh checkMark, outline;
        bool add;

        public OrderUnitEffect(Graphics.Mesh checkMark, bool add)
            :base(true)
        {
            this.add = add;
            this.checkMark = checkMark;
            if (add)
            {
                bumpMoition = new Graphics.Motion3d(Graphics.MotionType.SCALE,
                    checkMark, checkMark.Scale * 0.8f, Graphics.MotionRepeate.BackNForwardOnce,
                    120, true);
                //outline = (Graphics.Mesh)checkMark.CloneMe();
                //outline.Z -= 0.05f;
                //outline.SetSpriteName(SpriteName.cmdCheckOutline);
            }

        }

        public override void Time_Update(float time)
        {
            if (add)
            {
                if (bumpMoition != null && !checkMark.Visible)
                {
                    bumpMoition.DeleteMe();
                    bumpMoition = null;
                    //DeleteMe();
                }

                //if (outline != null)
                //{
                //    outline.Scale += outline.Scale * 0.01f * Ref.DeltaTimeMs;
                //    outline.Opacity -= 0.005f * Ref.DeltaTimeMs;

                //    if (outline.Opacity <= 0.01f)
                //    {
                //        outline.DeleteMe();
                //        outline = null;
                //    }
                //}
            }

            if (lifeTime.CountDown())
            {
                DeleteMe();
            }
        }
    }
}
