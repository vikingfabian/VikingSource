using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class WaterSplash : ParticleSystem
    {
        public WaterSplash()
            : base()
        { }
        protected override void InitializeSettings(ParticleSettings settings)
        {
           // settings.Texture = LoadedTexture.pwater;

            settings.MaxParticles = 500;

            settings.Duration = TimeSpan.FromSeconds(4);

            settings.MinHorizontalVelocity = 0.3f;
            settings.MaxHorizontalVelocity = 0.6f;

            settings.MinVerticalVelocity = -2f;
            settings.MaxVerticalVelocity = 2f;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = new Vector3(14f, -4f, -4f);

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -2;
            settings.MaxRotateSpeed = 10;

            
            const float SizeMin = 0.05f;
            //const float SizeMax = 0.6f;
            settings.MinStartSize = SizeMin;
            settings.MaxStartSize = SizeMin;

            settings.MinEndSize = SizeMin;
            settings.MaxEndSize = SizeMin;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.White;
        }
    }
}
