using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf.Effect
{
    class FourSplitEffect : AbsUpdateable
    {
        Split[] splits;
        float opacity = 1f;
        Time fadeDelay = new Time(120);

        public FourSplitEffect(Graphics.Image original)
            : base(true)
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;

            float speed = GolfRef.gamestate.BallScale * 4f;
            splits = new Split[4];

            var sprite = original.GetSprite();

            var areas = AreaExt.Split(original.RealArea(), 2, 2);
            var splitSprite = AreaExt.Split(sprite.Source, 2, 2);

            var moveDirs = VectorExt.CircleOfDirections(4, -MathExt.TauOver8, speed);

            {
                //NW
                var ar = areas[0, 0];
                var split = new Split();
                split.image = new Graphics.ImageAdvanced(SpriteName.MissingImage, ar.Center, ar.Size, ImageLayers.AbsoluteBottomLayer, true);
                split.image.PaintLayer = original.PaintLayer;
                split.image.ImageSource = splitSprite[0, 0];

                split.dir = moveDirs[0];

                splits[0] = split;
            }

            {
                //NE
                var ar = areas[1, 0];
                var split = new Split();
                split.image = new Graphics.ImageAdvanced(SpriteName.MissingImage, ar.Center, ar.Size, ImageLayers.AbsoluteBottomLayer, true);
                split.image.PaintLayer = original.PaintLayer;
                split.image.ImageSource = splitSprite[1, 0];

                split.dir = moveDirs[1];

                splits[1] = split;
            }

            {
                //SW
                var ar = areas[0, 1];
                var split = new Split();
                split.image = new Graphics.ImageAdvanced(SpriteName.MissingImage, ar.Center, ar.Size, ImageLayers.AbsoluteBottomLayer, true);
                split.image.PaintLayer = original.PaintLayer;
                split.image.ImageSource = splitSprite[0, 1];

                split.dir = moveDirs[3];

                splits[2] = split;
            }

            {
                //SE
                var ar = areas[1, 1];
                var split = new Split();
                split.image = new Graphics.ImageAdvanced(SpriteName.MissingImage, ar.Center, ar.Size, ImageLayers.AbsoluteBottomLayer, true);
                split.image.PaintLayer = original.PaintLayer;
                split.image.ImageSource = splitSprite[1, 1];

                split.dir = moveDirs[2];

                splits[3] = split;
            }

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        public override void Time_Update(float time_ms)
        {
            if (fadeDelay.CountDown())
            {
                opacity -= Ref.DeltaTimeSec * 8f;

                if (opacity <= 0)
                {
                    foreach (var m in splits)
                    {
                        m.image.DeleteMe();
                    }

                    DeleteMe();
                }
            }

            foreach (var m in splits)
            {
                m.image.Position += m.dir * Ref.DeltaTimeSec;
                m.image.Opacity = opacity;
            }
        }

        class Split
        {
            public Graphics.ImageAdvanced image;
            public Vector2 dir;
        }
    }
}
