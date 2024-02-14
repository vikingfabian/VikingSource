using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class VoidParticle : AbsUpdateable
    {
        Graphics.Image image;
        Time viewTime = new Time(400);
        Vector2 velocity;

        public VoidParticle(SpriteName tile, Vector2 pos, Vector2 scale, Vector2 velocity)
            : base(true)
        {
            //BirdLib.SetGameLayer();
            image = new Graphics.Image(tile, pos, scale, ImageLayers.Lay2, true);
            image.Opacity = 0.8f;
            this.velocity = velocity;
        }

        public override void Time_Update(float time)
        {
            image.Position += velocity;
            velocity *= 0.9f;

            if (viewTime.CountDown())
            {
                image.Opacity -= 0.001f * time;
                if (image.Opacity <= 0)
                {
                    DeleteMe();
                }
            }
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
