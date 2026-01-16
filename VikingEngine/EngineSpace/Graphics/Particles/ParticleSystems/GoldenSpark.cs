using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class GoldenSpark : ParticleSystem
    {
        public GoldenSpark()
                : base()
            { }

            protected override void InitializeSettings(ParticleSettings settings)
            {
                settings.Texture = LoadedTexture.WhiteArea;
                settings.MaxParticles = 3000;

                settings.Duration = TimeSpan.FromSeconds(0.9f);
                settings.DurationRandomness = 1;

               

                settings.EndVelocity = 0;

                

                settings.MinRotateSpeed = -1;
                settings.MaxRotateSpeed = 1;

                

                if (PlatformSettings.RunProgram == StartProgram.DSS)
                {
                    settings.Gravity = new Vector3(0.0f, 0.05f, 0.0f);

                    const float Speed = 0.01f;
                    settings.MinHorizontalVelocity = -Speed;
                    settings.MaxHorizontalVelocity = Speed;

                    settings.MinVerticalVelocity = -Speed;
                    settings.MaxVerticalVelocity = Speed;

                    settings.MinColor = new Color(255, 218, 65);
                    settings.MaxColor = new Color(253, 228, 121);

                    const float Size = 0.003f;
                    settings.MinStartSize = Size;
                    settings.MaxStartSize = Size;

                    settings.MinEndSize = Size;
                    settings.MaxEndSize = Size;
                }
                else
                {
                    const float Speed = 2;
                    settings.MinHorizontalVelocity = -Speed;
                    settings.MaxHorizontalVelocity = Speed;

                    settings.MinVerticalVelocity = -Speed;
                    settings.MaxVerticalVelocity = Speed;

                    settings.MinColor = Color.Orange;
                    settings.MaxColor = Color.Yellow;

                    const float Size = 0.3f;
                    settings.MinStartSize = Size;
                    settings.MaxStartSize = Size;

                    settings.MinEndSize = Size;
                    settings.MaxEndSize = Size;
                }

                // Use additive blending.
                settings.BlendState = BlendState.Additive;
            }
        }
    
}

