using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class BallExplosionEffect
    {
        public BallExplosionEffect(Graphics.Image original)
        {
            IntVector2 splits = new IntVector2(32);

            var tex = original.GetSprite();
            tex.Source.Width /= splits.X;
            tex.Source.Height /= splits.Y;

            Vector2 topLeft = original.RealTopLeft;
            Vector2 pScale = original.size / splits.Vec;
            topLeft += pScale * 0.5f;

            Vector2 explodeCenter = original.position;

            ForXYLoop loop = new ForXYLoop(splits);
            while (loop.Next())
            {
                Vector2 pos = new Vector2(topLeft.X + loop.Position.X * pScale.X, topLeft.Y + loop.Position.Y * pScale.Y);
                Graphics.ParticleImage p = new Graphics.ParticleImage(SpriteName.MissingImage, pos, pScale * 2f, ImageLayers.AbsoluteBottomLayer,
                    (pos - explodeCenter) * 0.027f);
                p.ImageSource = tex.Source;
                p.ImageSource.X += tex.Source.Width * loop.Position.X;
                p.ImageSource.Y += tex.Source.Height * loop.Position.Y;
                p.LayerAbove(original);

                p.particleData.realTime = true;
                p.particleData.setFadeout(Ref.rnd.Float(60, 120), 200);
            }
        }
    }
}
