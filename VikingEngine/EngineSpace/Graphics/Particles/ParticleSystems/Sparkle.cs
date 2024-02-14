#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    class Sparkle : ParticleSystem
    {
            public Sparkle()
                : base()
            { }

            protected override void InitializeSettings(ParticleSettings settings)
            {
                //settings.TextureName = "fire";
                //settings.Texture = LoadedTexture.psparkle;
                settings.Texture = LoadedTexture.NO_TEXTURE;
                settings.MaxParticles = 2400;

                settings.Duration = TimeSpan.FromSeconds(2);
                settings.DurationRandomness = 1;

                const float Speed = 1;
                settings.MinHorizontalVelocity = -Speed;
                settings.MaxHorizontalVelocity = Speed;

                settings.MinVerticalVelocity = -Speed;
                settings.MaxVerticalVelocity = Speed;

                settings.EndVelocity = 0;

                //const byte StartCol = 255;
                //settings.MinColor = new Color(StartCol, StartCol, StartCol, 140);
                //settings.MaxColor = new Color(255, 255, 255, 220);
                settings.MaxColor = Color.Yellow;
                settings.MinColor = Color.Orange;

                settings.MinRotateSpeed = -1;
                settings.MaxRotateSpeed = 1;

                //const float Size = 0.2f;
                settings.MinStartSize = 0.2f;
                settings.MaxStartSize = 0.3f;

                settings.MinEndSize = 0.4f;
                settings.MaxEndSize = 0.6f;

                //// Use additive blending.
                //settings.SourceBlend = Blend.SourceAlpha;
                //settings.DestinationBlend = Blend.One;
            }
        }
    
}
