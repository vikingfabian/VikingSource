using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class ExplosionEffect : AbsUpdateable
    {
        ExplosionParticle[] particles;
        float opacity = 1f;
        Time lifeTime = new Time(300);
 
        public ExplosionEffect(Car car)
            :base(true)
        {
            const int ParticleCount = 4;
            float SpeedL = cballRef.ballScale * 10f;

            particles = new ExplosionParticle[ParticleCount];
            Rotation1D dir = car.image.rotation;
            dir.Add(Rotation1D.D45);
           
            for (int i = 0; i < ParticleCount; ++i)
            {
                particles[i] = new ExplosionParticle(car.bound.Center, dir.Direction(SpeedL), car.image.rotation.Radians);
                dir.Add(MathHelper.TwoPi / ParticleCount);
            }

            cballRef.sounds.bassExplosion.Play(car.bound.Center);
        }

        public override void Time_Update(float time_ms)
        {
            foreach (var m in particles)
            {
                m.Opacity = opacity;
                m.Position += m.speed * Ref.DeltaTimeSec; 
            }

            if (lifeTime.CountDown())
            {
                opacity -= Ref.DeltaTimeSec * 8f;

                if (opacity <= 0f)
                {
                    foreach (var m in particles)
                    {
                        m.DeleteMe();
                    }
                }
            }

        }

        class ExplosionParticle : Graphics.Image
        {
            public Vector2 speed;

            public ExplosionParticle(Vector2 pos, Vector2 speed, float dir)
                : base(SpriteName.WhiteArea, pos, new Vector2(cballRef.ballScale * 0.6f), ImageLayers.Lay2, true)
            {
                this.speed = speed;
                Color = Color.Yellow;
            }
        }
    }
}
