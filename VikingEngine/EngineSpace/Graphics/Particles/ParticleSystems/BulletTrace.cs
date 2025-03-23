using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class BulletTrace : ParticleSystem
    {
//        const float StartScale =
//#if DSS
//            0.1f;
//#else
//            0.4f;
//#endif
//        const float EndScale = StartScale * 0.1f;


        public BulletTrace()
            : base()
        { }

        
        
        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.Texture = LoadedTexture.particle3;
            settings.MaxParticles = 30000;

            settings.Duration = TimeSpan.FromSeconds(3f);
            settings.DurationRandomness = 0.1f;

            settings.Gravity = new Vector3(0.0f, -0.02f, 0.0f);

            const float Speed = 0.001f;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = -Speed;
            settings.MaxVerticalVelocity = Speed;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkRed;//new Color(102, 66, 26, 120);
            settings.MaxColor = Color.Red;//new Color(198, 156, 109);


            const float Rotate = 1.6f;
            settings.MinRotateSpeed = -Rotate;
            settings.MaxRotateSpeed = Rotate;

            const float MinSize = 0.006f;
            const float MaxSize = MinSize * 1.4f;
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;

            //float StartScale;
            //float EndScale;

            //settings.Texture = LoadedTexture.particle3;

            //settings.MaxParticles = 20000;

            //settings.Duration = TimeSpan.FromSeconds(1);//0.45);//0.5

            //settings.MinHorizontalVelocity = 0;
            //settings.MaxHorizontalVelocity = 0;

            //settings.MinVerticalVelocity = 0;
            //settings.MaxVerticalVelocity = 0;

            settings.Gravity = Vector3.Zero;

            //settings.EndVelocity = 0;

            //settings.MinRotateSpeed = -1;
            //settings.MaxRotateSpeed = 1;

            ////const float StartScale = 0.2f;
            ////const float EndScale = StartScale * 1.5f;

            //if (PlatformSettings.RunProgram == StartProgram.DSS)
            //{
            //    StartScale = 0.02f;
            //    EndScale = StartScale * 0.3f;
            //settings.MinColor = Color.White;
            //settings.MaxColor = Color.White;
            //}
            //else
            //{
            //    StartScale = 0.4f;
            //    EndScale = StartScale * 0.1f;
            settings.MinColor = new Color(1, 1, 1, 0.4f);
            settings.MaxColor = new Color(1, 1, 1, 0.5f);
            //}


            //settings.MinStartSize = StartScale;
            //settings.MaxStartSize = StartScale;

            //settings.MinEndSize = EndScale;
            //settings.MaxEndSize = EndScale;



            //settings.BlendState = BlendState.NonPremultiplied;
        }
    }
}
