using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.Graphics
{
    class CCG_MagicSparkle: ParticleSystem
    {
        public CCG_MagicSparkle()
            : base()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.psmoke;
            settings.Texture = LoadedTexture.square_particle;

            settings.MaxParticles = 6000;

            const float Speed = 0.01f;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = -Speed;
            settings.MaxVerticalVelocity = Speed;

            settings.EndVelocity = Speed; //n

            settings.DurationRandomness = 0.5f;
            settings.Duration = TimeSpan.FromSeconds(1);

            settings.MinStartSize = 0.04f;
            settings.MaxStartSize = 0.16f;

            settings.MinEndSize = 0.02f;
            settings.MaxEndSize = 0.08f;

            settings.MinColor = Color.Blue;
            settings.MaxColor = Color.White;
            settings.MinColor.A = 100;
            settings.MaxColor.A = 200;

            settings.BlendState = BlendState.Additive;
        }
    }
}
