using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class WeaponSparks: ParticleSystem
    {
        public WeaponSparks()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.pwater;
            settings.Texture = LoadedTexture.WhiteArea;
            settings.MaxParticles = 3000;

            settings.Duration = TimeSpan.FromSeconds(0.7f);
            settings.DurationRandomness = 1;

            settings.Gravity = new Vector3(0.1f, 0.6f, -0.1f);

            const float Speed = 8;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = -Speed;
            settings.MaxVerticalVelocity = Speed;

            settings.EndVelocity = 0;

            settings.MinColor = Color.White;//new Color(102, 66, 26, 120);
            settings.MaxColor = Color.White;//new Color(198, 156, 109);
            

            const float Rotate = 1.6f;
            settings.MinRotateSpeed = -Rotate;
            settings.MaxRotateSpeed = Rotate;

            const float MinSize = 0.2f;
            const float MaxSize = MinSize * 1.4f;
            settings.MinStartSize = MinSize;
            settings.MaxStartSize = MaxSize;

            settings.MinEndSize = MinSize;
            settings.MaxEndSize = MaxSize;

        }
        
    }
}
