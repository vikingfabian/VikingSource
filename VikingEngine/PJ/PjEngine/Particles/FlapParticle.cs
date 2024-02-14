using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FlapParticle : AbsUpdateable
    {
        Graphics.Image image;
        Time viewTime = new Time(400);
        Vector2 velocity;

        public FlapParticle(Vector2 pos, float gamerImageScale, int dir)
            : base(true)
        {
            //BirdLib.SetGameLayer();
            image = new Graphics.Image(SpriteName.birdFlapParticle, pos, new Vector2(gamerImageScale * 0.5f), ImageLayers.Lay2, true);
            image.Opacity = 0.6f;

            velocity = new Vector2(-image.Width * dir, image.Width) * 0.06f;
        }

        public override void Time_Update(float time)
        {
            image.Position += velocity;
            velocity *= 0.7f;

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
