using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class Dust: ParticleSystem
    {
        public Dust()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.pwater;
            settings.Texture = LoadedTexture.particle3;
            

            float MinSize = 0.1f;
            float MaxSize = MinSize * 1.4f;
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                settings.MaxParticles = 20000;
                MaxSize = 0.01f;
                MinSize = MaxSize * 0.1f;

                settings.Gravity = new Vector3(0.004f, 0.02f, -0.004f);

                const float Speed = 0.01f;
                settings.MinHorizontalVelocity = -Speed;
                settings.MaxHorizontalVelocity = Speed;

                settings.MinVerticalVelocity = -Speed;
                settings.MaxVerticalVelocity = Speed;

                settings.Duration = TimeSpan.FromSeconds(3f);
                settings.MinColor = new Color(102, 66, 26, 20);
                settings.MaxColor = new Color(198, 156, 10);
            }
            else
            {
                settings.MaxParticles = 3000;
                MinSize = 0.1f;
                MaxSize = MinSize * 1.4f;

                settings.Gravity = new Vector3(0.1f, 0.6f, -0.1f);

                const float Speed = 2;
                settings.MinHorizontalVelocity = -Speed;
                settings.MaxHorizontalVelocity = Speed;

                settings.MinVerticalVelocity = -Speed;
                settings.MaxVerticalVelocity = Speed;

                settings.Duration = TimeSpan.FromSeconds(0.7f);
                settings.MinColor = new Color(102, 66, 26, 120);
                settings.MaxColor = new Color(198, 156, 109);
            }

            
            settings.DurationRandomness = 1;
                       

            settings.EndVelocity = 0;

            settings.MinColor = new Color(102, 66, 26, 120);
            settings.MaxColor = new Color(198, 156, 109);
            

            const float Rotate = 1.6f;
            settings.MinRotateSpeed = -Rotate;
            settings.MaxRotateSpeed = Rotate;

            
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;

        }
        
    }
}
