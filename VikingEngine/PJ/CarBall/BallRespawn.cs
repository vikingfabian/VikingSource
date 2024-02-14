using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class BallRespawn
    {
        Ball ball;
        Time respawnDelay = new Time(1f, TimeUnit.Seconds);
        Graphics.Image shpere;

        public BallRespawn(Ball ball)
        {
            this.ball = ball;
            ball.image.Position = cballRef.state.field.area.Center;
            shpere = new Graphics.Image(SpriteName.WhiteCirkle, ball.image.position, Vector2.Zero, ImageLayers.AbsoluteBottomLayer, true);
            shpere.LayerAbove(ball.image);
        }

        public bool update()
        {
            if (respawnDelay.CountDown())
            {
                shpere.size += ball.image.size * (1.2f * Ref.DeltaGameTimeSec);

                if (shpere.Size1D >= cballRef.ballScale)
                {
                    shpere.DeleteMe();

                    Graphics.Image wave = new Graphics.Image(SpriteName.ClickCirkleEffect, ball.image.position, ball.image.size, ImageLayers.AbsoluteBottomLayer, true);
                    wave.LayerBelow(ball.image);
                    wave.Opacity = 0.5f;

                    const float WaveTime = 800;
                    new Graphics.Motion2d(Graphics.MotionType.SCALE, wave,
                        new Vector2(cballRef.ballScale * 6f), Graphics.MotionRepeate.NO_REPEAT, WaveTime, true);
                    new Graphics.Motion2d(Graphics.MotionType.OPACITY, wave, new Vector2(-wave.Opacity), Graphics.MotionRepeate.NO_REPEAT,
                        WaveTime, true);
                    new Timer.Terminator(WaveTime, wave);

                    ball.image.Visible = true;
                    return true;
                }
            }
            return false;
        }
    }
}
