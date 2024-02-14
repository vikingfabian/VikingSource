using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class BouncingBlock2 : BouncingBlock2Dummie
    {
        float fadeOut = float.MinValue;
        static readonly IntervalF FadeWaitTime = new IntervalF(1900, 2200);
        const float FadeOutTime = 600;
        const float FalloutSpeed = -1 / FadeOutTime;
        Vector3 oldPos;
        static readonly GO.Bounds.ObjectBound Bound = LootFest.ObjSingleBound.QuickBoundingBox(1);
        GO.Bounds.ObjectBound bound;

        public BouncingBlock2(Vector3 startPos, Data.MaterialType material, float scale)
            : this(startPos, material, scale, Rotation1D.Random())
        { }

        public BouncingBlock2(Vector3 startPos, Data.MaterialType material, float scale, Rotation1D dir)
           : base(startPos, material, scale, dir)
        {
            oldPos = startPos;
            oldPos.Y -= Yadj;
            bound = Bound;
        }

        const float Yadj = -0.3f;
        public override void Time_Update(float time)
        {
            if (!Ref.isPaused)
            {
                if (fadeOut == float.MinValue)
                {
                    base.Time_Update(time);
                    Vector3 pos = block.Position;
                    pos.Y -= Yadj;
                    bound.MainBound.center = pos;
                    LootFest.TerrainColl coll = Bound.CollectTerrainObsticles(pos, oldPos, true);
                    if (coll.Collition)
                    {
                        const float CollisionDamp = 1.8f;
                        speed += speed * -coll.CollDir * CollisionDamp;

                        const float TotalBounceDamp = 0.7f;
                        speed *= TotalBounceDamp;

                        const float PosAdjust = 0.08f;
                        const float MinSpeed = 0.004f;
                        if (coll.CollDir.Y != 0 && speed.Length() <= MinSpeed)
                        {
                            fadeOut = FadeWaitTime.GetRandom();
                        }
                        else
                        {
                            rotationSpeed = new Vector3(rotationBounceSpeed.GetRandom(), rotationBounceSpeed.GetRandom(), rotationBounceSpeed.GetRandom());
                        }
                        block.Position += coll.CollDir * PosAdjust;
                    }
                    oldPos = pos;
                }
                else
                {
                    fadeOut -= time;
                    if (fadeOut <= 0)
                    {
                        DeleteMe();
                    }
                    if (fadeOut < FadeOutTime)
                    {
                        block.Opacity = fadeOut / FadeOutTime;
                        block.Y += FalloutSpeed * time;

                    }
                }
            }
        }
    }
}
