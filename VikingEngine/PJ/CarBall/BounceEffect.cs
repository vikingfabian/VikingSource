using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class BounceEffect : AbsUpdateable
    {
        Vector2 startScale;
        float time = MathHelper.Pi;
        Vector2 scaleAdd;
        Vector2 minScaleAdd;
        Graphics.Image image;

        public BounceEffect(Graphics.Image image)
            :base(true)
        {
            this.image = image;

            startScale = image.size;
            scaleAdd = 0.2f * startScale;
            scaleAdd.Y *= -1f;
            minScaleAdd = 0.01f * startScale;
        }

        public override void Time_Update(float time_ms)
        {
            time += Ref.DeltaTimeMs * 0.02f;

            if (scaleAdd.X <= minScaleAdd.X)
            {
                image.size = startScale;
                DeleteMe();
            }
            else
            {
                image.size = scaleAdd * MathExt.Sinf(time) + startScale;
                if (Ref.TimePassed16ms)
                {
                    scaleAdd *= 0.86f;
                }
            }
        }
    }
}
