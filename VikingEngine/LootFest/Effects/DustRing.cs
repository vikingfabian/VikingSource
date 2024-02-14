using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class DustRing : AbsInGameUpdateable
    {
        Vector3 center; IntervalF radius;
        float currentRadius;

        public DustRing(Vector3 center, IntervalF radius)
            : base(true)
        {
            this.center = center;
            this.radius = radius;
            currentRadius = radius.Min;
        }
        public override void Time_Update(float time)
        {
            Graphics.ParticleInitData p = new Graphics.ParticleInitData();
            p.StartSpeed = Vector3.Up * 0.6f;
            Map.WorldPosition wp;
            for (int i = 0; i < 12; i++)
            {
                p.Position = center + VectorExt.V2toV3XZ(Rotation1D.Random().Direction(currentRadius + Ref.rnd.Plus_MinusF(0.6f)));
                wp = new Map.WorldPosition(p.Position);
                wp.SetAtClosestFreeY(1);
                p.Position.Y = wp.WorldGrindex.Y;
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Dust, p);
            }

            currentRadius += 0.05f * time;
            if (currentRadius > radius.Max)
            {
                DeleteMe();
            }
        }
    }
}
