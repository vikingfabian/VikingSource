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
    class Smoke : ParticleSystem
    {
        public Smoke()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.psmoke;
            settings.Texture = LoadedTexture.square_particle;

            settings.MaxParticles = 2000;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                const float XZSpeed = 0.01f;

                settings.MinHorizontalVelocity = -XZSpeed;
                settings.MaxHorizontalVelocity = XZSpeed;

                settings.MinVerticalVelocity = 0f;
                settings.MaxVerticalVelocity = XZSpeed * 2f;

                settings.EndVelocity = 0.075f;

                settings.DurationRandomness = 1;
                settings.Duration = TimeSpan.FromSeconds(3);

                settings.Gravity = new Vector3(0.001f, 0.003f, -0.001f);

                settings.MinStartSize = 0.004f;//0.2f;
                settings.MaxStartSize = 0.006f;//0.3f;

                settings.MinEndSize = 0.008f;//0.4f;
                settings.MaxEndSize = 0.012f;//0.6f;
            }
            else
            {
                const float XZSpeed = 0.3f;

                settings.MinHorizontalVelocity = -XZSpeed;
                settings.MaxHorizontalVelocity = XZSpeed;

                settings.MinVerticalVelocity = 0f;
                settings.MaxVerticalVelocity = XZSpeed * 2f;

                settings.EndVelocity = 0.75f;

                settings.DurationRandomness = 2;
                settings.Duration = TimeSpan.FromSeconds(6);

                settings.Gravity = new Vector3(0.1f, 0.3f, -0.1f);

                settings.MinStartSize = 0.4f;
                settings.MaxStartSize = 0.6f;

                settings.MinEndSize = 0.8f;
                settings.MaxEndSize = 1.2f;
            }
            //Color Transparency = Color.White;
            //Transparency.A = 200;
            const byte BlackSmoke = 20;
            const byte WhiteSmoke = 100;
            settings.MinColor = new Color(BlackSmoke, BlackSmoke, BlackSmoke, (byte)120);
            settings.MaxColor = new Color(WhiteSmoke, WhiteSmoke, WhiteSmoke, (byte)180);
        }
    }
}
