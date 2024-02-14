//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;
//using VikingEngine.GameSpace;

//namespace VikingEngine.Graphics
//{
//    class WaterEdgeColl : ParticleSystem
//    {
//        public WaterEdgeColl()
//            : base()
//        { }
//        protected override void InitializeSettings(ParticleSettings settings)
//        {
//            settings.Texture = LoadedTexture.pwater;

//            settings.MaxParticles = 2000;

//            settings.Duration = TimeSpan.FromSeconds(3);

//            settings.MinHorizontalVelocity = 0f;
//            settings.MaxHorizontalVelocity = 0f;

//            settings.MinVerticalVelocity = 0f;
//            settings.MaxVerticalVelocity =0f;

//            // Create a wind effect by tilting the gravity vector sideways.
//            settings.Gravity = Vector3.Zero;

//            settings.EndVelocity = 0;

//            settings.MinRotateSpeed = 0;
//            settings.MaxRotateSpeed = 0;

            
//            const float SizeMin = 0.04f;
//            const float SizeMax = 0.06f;
//            const float Grow = 3;
//            settings.MinStartSize = SizeMin;
//            settings.MaxStartSize = SizeMax;

//            settings.MinEndSize = SizeMin * Grow;
//            settings.MaxEndSize = SizeMax * Grow;

//            settings.MinColor = Color.LightGray;
//            settings.MaxColor = Color.White;
//        }
//    }
//}
