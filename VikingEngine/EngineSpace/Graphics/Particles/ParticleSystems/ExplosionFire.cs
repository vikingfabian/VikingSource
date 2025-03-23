using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class ExplosionFire: ParticleSystem
    {
        public ExplosionFire()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            
            settings.Texture = LoadedTexture.particle3;
            settings.MaxParticles = 2400;

            settings.Duration = TimeSpan.FromSeconds(1.2f);

            settings.DurationRandomness = 1;

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                settings.MinStartSize = 0.0008f;
                settings.MaxStartSize = 0.0024f;

                settings.MinEndSize = 0.008f;
                settings.MaxEndSize = 0.016f;

                settings.Gravity = new Vector3(0, 0.003f, 0);
                settings.EndVelocity = 0.03f; //n

                const float Speed = 0.05f;
                settings.MinHorizontalVelocity = -Speed;
                settings.MaxHorizontalVelocity = Speed;

                settings.MinVerticalVelocity = 0;
                settings.MaxVerticalVelocity = Speed;
            }
            else
            {
                settings.MinStartSize = 0.2f;
                settings.MaxStartSize = 0.8f;

                settings.MinEndSize = 2f;
                settings.MaxEndSize = 3.6f;
                // Set gravity upside down, so the flames will 'fall' upward.
                settings.Gravity = new Vector3(0, 0.3f, 0);
                settings.EndVelocity = 0.75f; //n

                const float Speed = 0.1f;
                settings.MinHorizontalVelocity = -Speed;
                settings.MaxHorizontalVelocity = Speed;

                settings.MinVerticalVelocity = 0;
                settings.MaxVerticalVelocity = Speed;
            }
            const float Rotation = 2;
            settings.MinRotateSpeed = -Rotation;
            settings.MaxRotateSpeed = Rotation;
            // Use additive blending.
            settings.BlendState = BlendState.Additive;
            //settings.SourceBlend = Blend.SourceAlpha;
            //settings.DestinationBlend = Blend.One;

            settings.MinColor = new Color(200, 0, 0, 120);
            settings.MaxColor = new Color(255, 245, 104, 160);
        }
    
    }
}
