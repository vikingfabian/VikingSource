using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Strategy
{
    class GainResourceAnimationEmitter : AbsUpdateable
    {
        Graphics.Image resourceIcon;
        int countLeft;
        Time timer = 0;

        public GainResourceAnimationEmitter(Graphics.Image resourceIcon, int count)
            :base(true)
        {
            this.resourceIcon = resourceIcon;
            this.countLeft = count;
        }

        public override void Time_Update(float time_ms)
        {
            if (timer.CountDown())
            {
                new GainResourceAnimation(resourceIcon);
                timer.MilliSeconds = 220;
                
                if (--countLeft <= 0)
                {
                    DeleteMe();
                }
            }
        }
    }

    class GainResourceAnimation : AbsUpdateable
    {
        const float Gravity = 0.4f;
        Graphics.Image image;
        float speedY = -10f;

        public GainResourceAnimation(Graphics.Image resourceIcon)
            :base(true)
        {
            StrategyLib.SetMapLayer();
            image = (Graphics.Image)resourceIcon.CloneMe();
            image.Layer = ImageLayers.Foreground3;
        }

        public override void Time_Update(float time_ms)
        {
            speedY += Gravity;
            image.Ypos += speedY;

            if (speedY > 0)
            {
                image.Opacity -= 0.06f;
                if (image.Opacity <= 0f)
                {
                    DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            image.DeleteMe();
            base.DeleteMe();
        }
    }
}
