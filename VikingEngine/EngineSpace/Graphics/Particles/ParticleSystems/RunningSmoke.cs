#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Custom particle system for creating a giant plume of long lasting smoke.
    /// </summary>
    class RunningSmoke : ParticleSystem
    {
        public RunningSmoke()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.psmoke;
            settings.Texture = LoadedTexture.square_particle;

            settings.MaxParticles = 2000;


            const float XZSpeed = 0.3f;
            settings.MinHorizontalVelocity = -XZSpeed;
            settings.MaxHorizontalVelocity = XZSpeed;

            settings.MinVerticalVelocity = 0f;
            settings.MaxVerticalVelocity = 0.6f;

            // Create a wind effect by tilting the gravity vector sideways.
           

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            
                settings.DurationRandomness = 2;
                settings.Duration = TimeSpan.FromSeconds(1);

                settings.Gravity = new Vector3(0.1f, 0.3f, -0.1f);

                settings.MinStartSize = 0.4f;
                settings.MaxStartSize = 0.6f;

                settings.MinEndSize = 0.6f;
                settings.MaxEndSize = 1f;
            
            //Color Transparency = Color.White;
            //Transparency.A = 200;
            const byte BlackSmoke = 255;
            const byte WhiteSmoke = 255;
            settings.MinColor = new Color(BlackSmoke, BlackSmoke, BlackSmoke, (byte)120);
            settings.MaxColor = new Color(WhiteSmoke, WhiteSmoke, WhiteSmoke, (byte)180);
        }
    }
}
