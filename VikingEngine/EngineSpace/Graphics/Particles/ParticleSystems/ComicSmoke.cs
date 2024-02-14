//#region File Description
////-----------------------------------------------------------------------------
//// SmokePlumeParticleSystem.cs
////
//// Microsoft XNA Community Game Platform
//// Copyright (C) Microsoft Corporation. All rights reserved.
////-----------------------------------------------------------------------------
//#endregion

//#region Using Statements
//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//#endregion

//namespace VikingEngine.Graphics
//{
//    /// <summary>
//    /// Custom particle system for creating a giant plume of long lasting smoke.
//    /// </summary>
//    class ComicSmoke : ParticleSystem
//    {
//        public ComicSmoke()
//            : base()
//        { }


//        protected override void InitializeSettings(ParticleSettings settings)
//        {
//           // settings.Texture = LoadedTexture.psmoke;

//            settings.MaxParticles = 1200;

//            settings.Duration = TimeSpan.FromSeconds(10);

//            settings.MinHorizontalVelocity = -0.3f;
//            settings.MaxHorizontalVelocity = 0.3f;

//            settings.MinVerticalVelocity = -0.3f;
//            settings.MaxVerticalVelocity = 0.3f;

//            // Create a wind effect by tilting the gravity vector sideways.
//            settings.Gravity = new Vector3(0.8f, 0.16f, -0.3f);

//            settings.EndVelocity = 0.75f;

//            settings.MinRotateSpeed = -1;
//            settings.MaxRotateSpeed = 1;

//            settings.MinStartSize = 0.6f;
//            settings.MaxStartSize = 1f;

//            settings.MinEndSize = 1.2f;
//            settings.MaxEndSize = 2;

//            Color Transparency = Color.White;
//            //Transparency.A = 200;
//            settings.MinColor = Transparency;
//            settings.MaxColor = Transparency;
//        }
//    }
//}
