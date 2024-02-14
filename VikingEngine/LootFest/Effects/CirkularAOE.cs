using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    /// <summary>
    /// Views that an area is being affected by something, like magic AOE damage/healing
    /// </summary>
    class CirkularAOE : AbsInGameUpdateable
    {
        const int NumBlocks = 8;
        const float AngleDiff = MathHelper.TwoPi / NumBlocks;

        Vector3 center; Color blockColor; Graphics.ParticleSystemType traceParticles;
        float totalRadius;
        float percentTravel = 0;
        Graphics.Mesh[] blocks;
        Rotation1D rot = Rotation1D.D0;

        public CirkularAOE(Vector3 center, float radius, Color blockColor, Graphics.ParticleSystemType traceParticles)
            :base(true)
        {
            this.totalRadius = radius;
            this.center = center; this.blockColor = blockColor; this.traceParticles = traceParticles;
            blocks = new Graphics.Mesh[NumBlocks];
            //Graphics.TextureEffect tex =  new Graphics.TextureEffect(Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, blockColor);
            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i] = new Graphics.Mesh(LoadedMesh.cube_repeating, center, new Vector3( 0.6f), 
                    Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, blockColor);
            }
        }

        public override void Time_Update(float time)
        {
            const float ParticleChance = 0.3f;
            percentTravel += (1 - percentTravel) * 0.004f * time;

            float radius = totalRadius * percentTravel;
            float rotSpeed = (percentTravel * percentTravel) * 0.002f * time;

            rot += rotSpeed;

            Rotation1D r = rot;
            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i].Position = VectorExt.V2toV3XZ(r.Direction(radius)) + center;
                r.Radians += AngleDiff;
                if (Ref.rnd.Chance(ParticleChance))
                    Engine.ParticleHandler.AddParticles(traceParticles, blocks[i].Position);
            }

            if (percentTravel >= 0.99f)
            {
                this.DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            for (int i = 0; i < blocks.Length; ++i)
            {
                blocks[i].DeleteMe();
            }
        }
    }
}
