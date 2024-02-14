using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FlapParticlesBling : AbsUpdateable
    {
        int particleCount = 0;
        int goalCount;
        Joust.Gamer gamer;
        Time nextParticle = 0;
        float pScale;

        public FlapParticlesBling(Joust.Gamer gamer)
            :base(true)
        {
            goalCount = Ref.rnd.Int(5, 8);
            pScale = Joust.Gamer.ImageScale * 0.3f;
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

                    SpriteName tile;
                    float airResistance = 1f;
                    float scale = pScale;

                    switch (Ref.rnd.Int(4))
                    {
                        case 0:
                            tile = SpriteName.blingParticle1;
                            break;
                        case 1:
                            tile = SpriteName.blingParticle2;
                            break;
                        default:
                            tile = SpriteName.blingParticle3;
                            airResistance = 0.96f;
                            scale = pScale * 2f;
                            break;
                    }

                    new FallingParticle(gamer.Position, tile, Color.White, scale, 0.1f,
                        0.016f * Joust.Gamer.ImageScale, Joust.Gamer.Gravity * 5f, airResistance); 

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