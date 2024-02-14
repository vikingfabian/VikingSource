using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.PJ
{
    class FadeInMusic : AbsUpdateable
    {
        Time fadeTime;
        Time muteTime;
        float timePassed = 0;
        float maxVol;

        public FadeInMusic(Time muteTime, Time fadeTime, float maxVol)
            : base(true)
        {
            MediaPlayer.Volume = 0f;
            this.fadeTime = fadeTime;
            this.muteTime = muteTime;
            this.maxVol = maxVol;
        }

        public override void Time_Update(float time)
        {
            if (muteTime.CountDown())
            {
                timePassed += time;
                if (timePassed >= fadeTime.MilliSeconds)
                {
                    MediaPlayer.Volume = maxVol;
                    this.DeleteMe();
                }
                else
                {
                    MediaPlayer.Volume = (timePassed / fadeTime.MilliSeconds) * maxVol;
                }
            }
        }
    }

    
}
