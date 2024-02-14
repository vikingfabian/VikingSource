//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;


//namespace VikingEngine.Graphics
//{
//    class DynamiteSpark: ParticleSystem
//    {
//            public DynamiteSpark()
//                : base()
//            { }

//            protected override void InitializeSettings(ParticleSettings settings)
//            {
//                settings.Texture = LoadedTexture.square_particle;
//                settings.MaxParticles = 3000;

//                settings.Duration = TimeSpan.FromSeconds(1f);
//                settings.DurationRandomness = 1;

//                const float Speed = 2;
//                settings.MinHorizontalVelocity = -Speed;
//                settings.MaxHorizontalVelocity = Speed;

//                settings.MinVerticalVelocity = -Speed;
//                settings.MaxVerticalVelocity = Speed;

//                settings.EndVelocity = 0;

//                settings.MinColor = Color.Orange;
//                settings.MaxColor = Color.Yellow;

//                settings.MinRotateSpeed = -1;
//                settings.MaxRotateSpeed = 1;

//                const float Size = 0.1f;
//                settings.MinStartSize = Size;
//                settings.MaxStartSize = Size;

//                settings.MinEndSize = Size;
//                settings.MaxEndSize = Size;

//                // Use additive blending.
//                settings.BlendState = BlendState.Additive;
//            }
//        }
    
//}

