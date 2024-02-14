using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FlapParticlesVoid : AbsUpdateable
    {
        int particleCount = 0;
        int goalCount;
        Joust.Gamer gamer;
        Time nextParticle = 0;
        Vector2 pScale;

        public FlapParticlesVoid(Joust.Gamer gamer)
            :base(true)
        {
            goalCount = Ref.rnd.Int(5, 8);
            pScale = new Vector2(Joust.Gamer.ImageScale * 0.5f);
            this.gamer = gamer;
        }

        public override void Time_Update(float time_ms)
        {
            if (gamer.Alive && !gamer.IsLyingDown)
            {
                SpriteName pTile;
                float bigChance = 0.8f - 0.2f * particleCount;
                float medChance = 0.6f - 0.1f * particleCount;

                float rnd = Ref.rnd.Float();//lib.NextFloat();
                if (rnd < bigChance)
                {
                    pTile = SpriteName.voidParticle3;
                }
                else if ((rnd - bigChance) < medChance)
                {
                    pTile = SpriteName.voidParticle2;
                }
                else
                {
                    pTile = SpriteName.voidParticle1;
                }

                if (nextParticle.CountDown())
                {
                    Rotation1D dir = Rotation1D.D180;
                    const float RndDirAdd = 0.7f;
                    dir.Add(gamer.travelDir * MathHelper.Pi * 0.3f + Ref.rnd.Plus_MinusF(RndDirAdd));
                    Vector2 dirVec = dir.Direction(1f);

                    new VoidParticle(
                        pTile, 
                        gamer.Position + Ref.rnd.Float(0, pScale.X * 0.8f) * dirVec, 
                        pScale, 
                        0.04f * pScale * dirVec);

                    float maxTimeToNext = 20 + 40 * particleCount;
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
