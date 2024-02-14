using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class SmokingArea : AbsInGameUpdateable
    {
        float startDelay = 400;
        float addRate = 0.1f;
        Vector3 center; float radius;

        public SmokingArea(Vector3 center, float radius)
            :base(true)
        {
            this.center = center;
            this.radius = radius;
        }
        public override void Time_Update(float time)
        {
            if (startDelay <= 0)
            {
                addRate -= 0.000002f * time;
                if (Ref.rnd.Double() < addRate)
                {
                    Vector3 pos = center;
                    pos.X += Ref.rnd.Plus_MinusF(radius);
                    pos.Z += Ref.rnd.Plus_MinusF(radius);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, pos);
                }
                else if (addRate <= 0)
                {
                    DeleteMe();
                }
            }
            else
                startDelay -= time;
        }
    }
}
