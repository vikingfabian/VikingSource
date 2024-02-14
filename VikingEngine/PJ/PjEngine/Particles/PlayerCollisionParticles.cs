using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Particles
{
    class PlayerCollisionParticle : AbsUpdateable
    {
        public static void Create(Vector2 center)
        {
            const int ParticleCount = 16;
            float velocity = Joust.Gamer.ImageScale * 0.05f;
            Rotation1D dir = Rotation1D.D0;

            for (int i = 0; i < ParticleCount; ++i)
            {
                new PlayerCollisionParticle(center, dir.Direction(velocity));
                dir.Add(MathHelper.TwoPi / ParticleCount);
            }
        }

        Graphics.Image image;
        Vector2 speed;

        public PlayerCollisionParticle(Vector2 pos, Vector2 speed)
            :base(true)
        {
            image = new Graphics.Image(SpriteName.WhiteArea, pos, new Vector2(Joust.Gamer.ImageScale * 0.09f), ImageLayers.Background5, true);
            this.speed = speed;
        }

        public override void Time_Update(float time_ms)
        {
            image.Position += speed;

            image.Opacity -= 0.04f;
            if (image.Opacity <= 0f)
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
