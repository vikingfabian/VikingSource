using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class WoodSplint : ParticleSystem
    {
        public WoodSplint()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
           // settings.Texture = LoadedTexture.pwood;
            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(1.2);
            settings.DurationRandomness = 0.4f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, -4, 0);

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.White;

            const float MinSize = 0.08f;
            const float MaxSize = 0.22f;
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;

            settings.MinRotateSpeed = -3;
            settings.MaxRotateSpeed = 3;
            
        }
    }
}
