using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FlapParticlesZombie : AbsUpdateable
    {
        int particleCount = 0;
        int goalCount;
        Joust.Gamer gamer;
        Time nextParticle = 0;
        IntervalF pScale;

        public FlapParticlesZombie(Joust.Gamer gamer)
            :base(true)
        {
            goalCount = Ref.rnd.Int(5, 8);
            pScale = new IntervalF(Joust.Gamer.ImageScale * 0.04f, Joust.Gamer.ImageScale * 0.1f);
            this.gamer = gamer;
        }

        public override void Time_Update(float time_ms)
        {
            if (gamer.Alive && !gamer.IsLyingDown)
            {
                if (nextParticle.CountDown())
                {
                    Rotation1D dir = Rotation1D.D180;
                    const float RndDirAdd = 0.7f;
                    dir.Add(gamer.travelDir * MathHelper.Pi * 0.3f + Ref.rnd.Plus_MinusF(RndDirAdd));
                    Vector2 dirVec = dir.Direction(1f);

                    new FallingParticle(gamer.Position, SpriteName.WhiteArea, PjLib.ZombieParticleColor, pScale.GetRandom(), 0.1f,
                        0.016f * Joust.Gamer.ImageScale, Joust.Gamer.Gravity * 5f, 1f); 

                    float maxTimeToNext = 10 + 20 * particleCount;
                    nextParticle.MilliSeconds = Ref.rnd.Float(maxTimeToNext * 0.4f, maxTimeToNext);

                    if (++particleCount >= goalCount)
                    {
                        DeleteMe();
                    }
                }
            }
            else
            {
                DeleteMe();
            }
        }
    }

    
}