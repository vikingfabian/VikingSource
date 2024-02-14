using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ
{
    class WinnerParticleEmitter : AbsUpdateable
    {
        int particlesLeft = 120;
        IPosition winner;
        Time delay = new Time(400, TimeUnit.Milliseconds);

        public WinnerParticleEmitter(IPosition winner)
            :base(true)
        {
            this.winner = winner;
        }

        public override void Time_Update(float time_ms)
        {
            if (delay.CountDown() && particlesLeft > 0)
            {
                int particleCount = Ref.rnd.Int(6, 10);

                Vector2 center = winner.PositionXY;

                for (int i = 0; i < particleCount; ++i)
                {
                    new WinnerParticle(center, Rotation1D.Random().Direction(1f));
                    particlesLeft--;
                }
            }
        }
    }

    class WinnerParticle : AbsUpdateable
    {
        Graphics.Image image;
        Vector2 dir;
        Time lifeTime;
        float rotSpeed;

        public WinnerParticle(Vector2 pos, Vector2 dir)
            :base(true)
        {
            image = new Graphics.Image(SpriteName.winnerParticle, pos, Engine.Screen.SmallIconSizeV2, 
                ImageLayers.Foreground1, true);
            image.Rotation = Ref.rnd.Rotation();
            this.dir = dir * Ref.rnd.Float(8f, 9f);

            lifeTime.MilliSeconds = Ref.rnd.Int(1000, 1400);

            rotSpeed = Ref.rnd.Plus_MinusF(0.2f);
        }

        public override void Time_Update(float time_ms)
        {
            image.Position += dir;
            image.Rotation += rotSpeed;

            if (lifeTime.CountDown())
            {
                image.Opacity -= 0.1f;

                if (image.Opacity <= 0f)
                {
                    DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
