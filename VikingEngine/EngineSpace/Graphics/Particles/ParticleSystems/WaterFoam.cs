using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class WaterFoam : ParticleSystem
    {
//        const float StartScale =
//#if DSS
//            0.1f;
//#else
//            0.4f;
//#endif
//        const float EndScale = StartScale * 0.1f;


        public WaterFoam()
            : base()
        { }

        
        
        protected override void InitializeSettings(ParticleSettings settings)
        {

            settings.Texture = LoadedTexture.particle3;
            settings.MaxParticles = 30000;

            settings.Duration = TimeSpan.FromSeconds(5f);
            settings.DurationRandomness = 0.1f;

            const float Speed = 0.001f;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = -Speed;
            settings.MaxVerticalVelocity = Speed;

            settings.EndVelocity = 0;

            //const float Rotate = 1.6f;
            //settings.MinRotateSpeed = -Rotate;
            //settings.MaxRotateSpeed = Rotate;

            const float MinSize = 0.009f;
            const float MaxSize = MinSize * 1.4f;
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;


            settings.Gravity = Vector3.Zero;

            settings.MinColor = new Color(1, 1, 1, 0.2f);
            settings.MaxColor = new Color(1, 1, 1, 0.3f);

        }
    }
}
