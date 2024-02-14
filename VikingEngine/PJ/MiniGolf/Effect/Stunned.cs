using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf.Effect
{
    class Stunned
    {
        Ball ball;
        Graphics.Image image;

        Time time = new Time(4f, TimeUnit.Seconds);

        public Stunned(Ball ball)//SpriteName.golfStunnStars, ball.image.Position, ball.image.Size, 
        {
            Ref.draw.CurrentRenderLayer = Draw.HudLayer;

            this.ball = ball;
            image = new Graphics.Image(SpriteName.golfStunnStars, ball.image.Position, ball.image.Size * 0.7f, GolfLib.BallEffectLayer, true);
        }

        public bool update()
        {
            image.Rotation += Ref.DeltaTimeSec * 4f;
            image.Position = ball.image.Position;

            return time.CountDown();
        }

        public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
