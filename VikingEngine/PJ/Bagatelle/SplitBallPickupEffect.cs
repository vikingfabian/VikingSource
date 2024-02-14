using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class SplitBallPickupEffect : Graphics.AbsUpdateableImage
    {
        Time viewTime = new Time(800);
        Ball ball;

        public SplitBallPickupEffect(Ball ball, BagatellePlayState state)
            :base(SpriteName.bagSplitPickupEffect, ball.image.Position,
                            0.4f * state.BallScale * new Vector2(2, 1), ImageLayers.Top6, true, true, true)
        {
            this.ball = ball;

            Graphics.Motion2d grow = new Graphics.Motion2d(Graphics.MotionType.SCALE, this,
                this.Size * 1.2f, Graphics.MotionRepeate.BackNForwardOnce,
                200, true);
        }

        public override void Time_Update(float time_ms)
        {
            position = ball.position;

            if (viewTime.CountDown())
            {
                DeleteMe();
            }
        }

         //{
         //               float EffectTime = 800;
         //               Graphics.Image pickEffect = new Graphics.Image(SpriteName.bagSplitPickupEffect, box.image.Position,
         //                   0.4f * new Vector2(2, 1) * box.image.Size, ImageLayers.Top6, true);
         //               Graphics.Motion2d grow = new Graphics.Motion2d(Graphics.MotionType.SCALE, pickEffect, 
         //                   pickEffect.Size * 3f, Graphics.MotionRepeate.NO_REPEAT,
         //                   EffectTime, true);
         //               new Timer.Terminator(EffectTime, pickEffect); 
         //           }
    }
}
