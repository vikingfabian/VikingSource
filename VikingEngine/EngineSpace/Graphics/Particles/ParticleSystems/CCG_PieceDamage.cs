using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class CCG_PieceDamage : ParticleSystem
    {
        public CCG_PieceDamage()
            : base()
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.Texture = LoadedTexture.psmoke;
            settings.Texture = LoadedTexture.ccg_piece_particle;

            settings.MaxParticles = 2000;



            const float Speed = 0.01f;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = -Speed;
            settings.MaxVerticalVelocity = Speed;

            settings.EndVelocity = Speed; //n


            settings.Gravity = new Vector3(0, -1f, 0);

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.DurationRandomness = 0.5f;
            settings.Duration = TimeSpan.FromSeconds(2);

            settings.MinStartSize = 0.03f;
            settings.MaxStartSize = 0.04f;

            settings.MinEndSize = 0.03f;
            settings.MaxEndSize = 0.04f;

            //Color Transparency = Color.White;
            //Transparency.A = 200;
            //const byte BlackSmoke = 20;
            //const byte WhiteSmoke = 100;
            settings.MinColor = Color.White;
            settings.MaxColor = Color.LightGray;
        }
    }
}
