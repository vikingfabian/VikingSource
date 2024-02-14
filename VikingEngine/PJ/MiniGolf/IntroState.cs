using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class IntroState : AbsPJGameState
    {   
        public IntroState(List2<GamerData> joinedGamers)
            :base(false)
        {
            Input.Mouse.Visible = false;

            this.joinedLocalGamers = joinedGamers;
            Ref.draw.ClrColor = Color.Black;

            const float MusicVol = 0.6f;

            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
            Ref.music.SetPlaylist(new List<Sound.SongData>
                {
                    new Sound.SongData(PjLib.MusicFolder + "lazy_town", "Lazy Town", true, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "loot", "Loot", true, 1.4f * MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "RM 6 - Auderesne", "Auderesne", true, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "RM 10 - Incubation","Incubation", false, MusicVol),
                    new Sound.SongData(PjLib.MusicFolder + "void", "Void", true, 1.6f *MusicVol),
                }, false);

            new Timer.AsynchActionTrigger(loadContent_Asynch, true);

#if XBOX
            Ref.xbox.presence.Set("golf");
#endif
        }

        void loadContent_Asynch()
        {
            new Sounds();
            //fieldTexture = Engine.LoadContent.LoadTexture(PjLib.ContentFolder + "carballfield");
            new Timer.Action0ArgTrigger(onLoadingComplete);
        }

        void onLoadingComplete()
        {
            if (Ref.gamestate == this)
            {
                new MinigolfState(joinedLocalGamers, 0);
            }
        }


    }
}
