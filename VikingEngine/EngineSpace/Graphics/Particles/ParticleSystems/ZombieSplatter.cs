//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.Graphics
//{
//    class ZombieSplatter : ParticleSystem
//    {
//        public ZombieSplatter()
//            : base()
//        { }

//        protected override void InitializeSettings(ParticleSettings settings)
//        {
//            settings.Texture = LoadedTexture.WhiteArea;

//            settings.MaxParticles = 500;

//            settings.Duration = TimeSpan.FromSeconds(4);
//            settings.DurationRandomness = 0.2f;

//            settings.MinHorizontalVelocity = 0.3f;
//            settings.MaxHorizontalVelocity = 0.6f;

//            settings.MinVerticalVelocity = -2f;
//            settings.MaxVerticalVelocity = 2f;

//            // Create a wind effect by tilting the gravity vector sideways.
//           settings.Gravity = new Vector3(0, -40f, 0);

//            settings.EndVelocity = 1f;

//            settings.MinRotateSpeed = -10;
//            settings.MaxRotateSpeed = 10;


//            const float SizeMin = 0.3f;
//            const float SizeMax = 0.4f;
//            settings.MinStartSize = SizeMin;
//            settings.MaxStartSize = SizeMax;

//            settings.MinEndSize = SizeMin;
//            settings.MaxEndSize = SizeMax;

//            settings.MinColor = Color.DarkViolet;
//            settings.MaxColor = Color.Violet;
//        }
//    }
//}