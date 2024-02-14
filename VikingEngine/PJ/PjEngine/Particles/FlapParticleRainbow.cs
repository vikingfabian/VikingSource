using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ
{
    class FlapParticleRainbow : AbsUpdateable
    {
        Graphics.Image image;
        Time viewTime = new Time(400);
        //Vector2 velocity;
        const int AnimationLength = 6;
        int animation = 0;
        CirkleCounterUp updateCount = new CirkleCounterUp(0, 4);

        public FlapParticleRainbow(Vector2 pos, float gamerImageScale, int dir)
            : base(true)
        {
           // BirdLib.SetGameLayer();
            image = new Graphics.Image(SpriteName.rainbowParticle1, pos, new Vector2(gamerImageScale * 0.6f), ImageLayers.Lay2, true);
        }

        public override void Time_Update(float time)
        {
            if (animation < AnimationLength - 1)
            {
                if (updateCount.Next_IsReset())
                {
                    animation++;
                    image.SetSpriteName((SpriteName)((int)SpriteName.rainbowParticle1 + animation));
                }
            }
            else
            {
                if (viewTime.CountDown())
                {
                    image.Opacity -= 0.001f * time;
                    if (image.Opacity <= 0)
                    {
                        DeleteMe();
                    }
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
