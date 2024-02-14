using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VikingEngine.Graphics
{
    class TorchFire : ParticleSystem
    {
        public TorchFire()
                : base()
            { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.Texture = LoadedTexture.WhiteArea;
            settings.MaxParticles = 3000;

            settings.Duration = TimeSpan.FromSeconds(3f);
            settings.DurationRandomness = 0.5f;

            const float Speed = 0.03f;
            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = Speed * 0.6f;
            settings.MaxVerticalVelocity = Speed;

            settings.Gravity = new Vector3(0, 0.03f, 0);

            settings.EndVelocity = Speed * 0.1f;

            settings.MinRotateSpeed = 0;
            settings.MaxRotateSpeed = 0;
            
            settings.MinColor = Color.Red;
            settings.MaxColor = Color.Orange;

            const float MinSize = 0.01f;
            const float MaxSize = 0.02f;
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;
            
            settings.BlendState = BlendState.Additive;
        }
    }

}


