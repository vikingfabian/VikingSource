
#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    class Fire : ParticleSystem
    {
        public Fire()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.TextureName = "fire";
            //settings.Texture = LoadedTexture.pfire;
            settings.Texture = LoadedTexture.particle3;
            settings.MaxParticles = 2400;

            // Set gravity upside down, so the flames will 'fall' upward.

           
            float Speed;
            

            settings.Duration = TimeSpan.FromSeconds(1);

            settings.DurationRandomness = 1;            

            

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                settings.Gravity = new Vector3(0, 0.005f, 0);
                settings.MinStartSize = 0.004f;
                settings.MaxStartSize = 0.006f;

                settings.MinEndSize = 0.010f;//0.4f;
                settings.MaxEndSize = 0.022f;//0.6f;
                Speed = 0.01f;
                settings.EndVelocity = 0.05f; //n
            }
            else 
            {
                settings.Gravity = new Vector3(0, 0.3f, 0);
                settings.MinStartSize = 0.05f;
                settings.MaxStartSize = 0.4f;

                settings.MinEndSize = 0.1f;
                settings.MaxEndSize = 0.8f;
                Speed = 0.1f;
                settings.EndVelocity = 0.75f; //n
            }

            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = Speed;
            settings.MinColor = new Color(200, 0, 0, 120);
            settings.MaxColor = new Color(255, 245, 104, 160);
            
            // Use additive blending.
            settings.BlendState = BlendState.Additive;

            
        }
    }
}
