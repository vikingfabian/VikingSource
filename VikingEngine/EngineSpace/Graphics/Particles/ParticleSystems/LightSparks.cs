#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    class LightSparks : ParticleSystem
    {
        public LightSparks()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.TextureName = "fire";
            settings.Texture = LoadedTexture.WhiteArea;
            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(1.2);
            settings.DurationRandomness = 0.4f;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, -4, 0);

            settings.MinColor = Color.Blue;
            settings.MaxColor = Color.White;

            const float Size = 0.2f;
            settings.MinStartSize = Size;
            settings.MaxStartSize = 2 * Size;

            settings.MinEndSize = Size;
            settings.MaxEndSize = 2 * Size;

        }
    }
}
