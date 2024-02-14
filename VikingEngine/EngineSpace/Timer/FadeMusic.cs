using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    //class FadeMusic : Update
    //{
    //    float fadeTime;
    //    float timeLeft;
    //    string nextSong;

    //    public FadeMusic(float time, string nextSong)
    //        :base(true)
    //    {
    //        this.fadeTime = time;
    //        timeLeft = time;
    //        this.nextSong = nextSong;
    //    }
    //    public override void Time_Update(float time)
    //    {
    //        timeLeft -= time;

    //        Engine.Sound.Volume = timeLeft / fadeTime * Engine.Sound.TargetMusicVol;
    //        if (timeLeft <= 0)
    //        {
    //            Engine.Sound.StopMusic();
    //            Engine.Sound.Volume = Engine.Sound.TargetMusicVol;
    //            Engine.LoadContent.LoadAndPlaySongThreaded(nextSong);
    //            DeleteMe();
    //        }
    //    }
    //}
}
