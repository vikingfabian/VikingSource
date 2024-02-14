using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class ParticleImage : AbsUpdateableImage
    {
        public ParticleData particleData;

        public ParticleImage()
            : base(true, true)
        { }

        public ParticleImage(SpriteName sprite, Vector2 pos, Vector2 sz, ImageLayers layer, Vector2 velocity)
            : base(sprite, pos, sz, layer, true, true, true)
        {
            particleData.Velocity2D = velocity;
        }

        public override void Draw(int cameraIndex)
        {
            base.Draw(cameraIndex);
        }

        public override void Time_Update(float time_ms)
        {
            if (particleData.update(this))
            {
                DeleteMe();
            }
        }
    }

    class ParticlePlaneXZ : AbsUpdateableModel
    {
        public ParticleData particleData;

        public ParticlePlaneXZ(SpriteName sprite, Vector3 pos, Vector2 sz, Vector3 velocity)
            : base(LoadedMesh.plane, pos, VectorExt.V2toV3XZ(sz), TextureEffectType.Flat, sprite, Color.White, true, true)
        {
            particleData.velocity = velocity;
        }

        public override void Time_Update(float time_ms)
        {
            if (particleData.update(this))
            {
                DeleteMe();
            }
        }
    }
}
