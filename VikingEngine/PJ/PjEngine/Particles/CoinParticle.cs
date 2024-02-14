using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.Joust.DropItem;

namespace VikingEngine.PJ.Particles
{
    class CoinParticle: AbsUpdateable
    {
        public static void Create(Vector2 center, float coinSz, SpriteName color = SpriteName.birdCoinParticleYellow)
        {
            const int ParticleCount = 4;
            float velocity = coinSz * 0.1f;
            Rotation1D dir = Rotation1D.D45;

            for (int i = 0; i < ParticleCount; ++i)
            {
                new CoinParticle(center, coinSz, dir.Direction(velocity), color);
                dir.Add(MathHelper.TwoPi / ParticleCount);
            }
        }

        Graphics.Image image;
        Vector2 speed;

        public CoinParticle(Vector2 pos, float sz, Vector2 speed, SpriteName color)
            :base(true)
        {
            image = new Graphics.Image(color, pos, new Vector2(sz), ImageLayers.Background5, true);
            this.speed = speed;
        }

        public override void Time_Update(float time_ms)
        {
            image.Position += speed;

            image.Opacity -= 0.06f;
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
