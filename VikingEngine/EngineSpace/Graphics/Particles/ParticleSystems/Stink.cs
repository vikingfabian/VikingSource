#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    class Stink : ParticleSystem
    {
        public Stink()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.TextureName = "fire";
           // settings.Texture = LoadedTexture.pstink;

            settings.MaxParticles = 1000;

            settings.Duration = TimeSpan.FromSeconds(10);

            settings.MinHorizontalVelocity = -0.3f;
            settings.MaxHorizontalVelocity = 0.3f;

            settings.MinVerticalVelocity = -0.3f;
            settings.MaxVerticalVelocity = 0.3f;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = new Vector3(0.8f, 0.4f, -0.3f);

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 0.15f;
            settings.MaxStartSize = 0.2f;

            settings.MinEndSize = 3;
            settings.MaxEndSize = 4;

            const byte MaxCol = 255;
            const byte MinCol = 160;

            settings.MinColor = new Color(MaxCol,MaxCol, MaxCol, (byte)160);
            settings.MaxColor = new Color(MinCol, MinCol, MinCol, (byte)80);
        }
    }
}
