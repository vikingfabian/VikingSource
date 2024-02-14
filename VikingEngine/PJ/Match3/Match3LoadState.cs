using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class Match3LoadState : AbsPJGameState
    {
        Texture2D bgTexture;

        public Match3LoadState(List2<GamerData> joinedGamers)
            :base(false)
        {
            Input.Mouse.Visible = false;

            this.joinedLocalGamers = joinedGamers;
            Ref.draw.ClrColor = Color.Black;
            
            new Timer.AsynchActionTrigger(loadContent_Asynch, true);
        }

        void loadContent_Asynch()
        {
            bgTexture = Engine.LoadContent.LoadTexture(PjLib.ContentFolder + "m3bg");
            new Sounds();
            new Timer.Action0ArgTrigger(onLoadingComplete);
        }

        void onLoadingComplete()
        {
           if (Ref.gamestate == this)
            {
                new Match3PlayState(joinedLocalGamers, bgTexture);
            }
        }

    }
}
