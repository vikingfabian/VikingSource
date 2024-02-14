using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class BulletTrace : ParticleSystem
    {
        const float StartScale =
#if DSS
            0.1f;//10f * DSSWars.GameObject.AbsDetailUnit.StandardModelScale;
#else
            0.4f;
#endif
        const float EndScale = StartScale * 0.1f;


        public BulletTrace()
            : base()
        { }

        static readonly Color MaxCol = new Color(1, 1, 1, 0.5f);
        static readonly Color MinCol = new Color(1, 1, 1, 0.4f);
        
        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.Texture = LoadedTexture.WhiteArea;

            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(0.45);//0.5

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 0;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = 0;

            settings.Gravity = Vector3.Zero;

            settings.EndVelocity = 0;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            //const float StartScale = 0.2f;
            //const float EndScale = StartScale * 1.5f;
            

            settings.MinStartSize = StartScale;
            settings.MaxStartSize = StartScale;

            settings.MinEndSize = EndScale;
            settings.MaxEndSize = EndScale;

            settings.MinColor = MinCol;
            settings.MaxColor = MaxCol;

            //settings.BlendState = BlendState.Additive;
        }
    }
}
