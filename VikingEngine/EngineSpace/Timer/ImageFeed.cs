using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class ImageFeed : AbsTimer
    {

        Graphics.Image image;
        public ImageFeed(int startTime, Graphics.Image img)
            :base(startTime, UpdateType.Full)
        {
            image = img;
            //this.baseInit(startTime);
            img.Opacity = 0;
        }

        public override void Time_Update(float time)
        {
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                float transparensy = -(float)timeLeft / PublicConstants.ImageFeedTime;
                if (transparensy >= 1)
                { image.Opacity = 1; DeleteMe(); }
                else
                {
                    image.Opacity = transparensy; ;
                }
            }
            //return false;    
        }
        public override void PreDeleteMe(VikingEngine.Engine.GameState state)
        {
            image.Opacity = 1;
            base.PreDeleteMe(state);
        }
        public override float TimeLeft
        {
            get
            {
                return timeLeft + PublicConstants.ImageFeedTime;
            }
        }
    }
}
