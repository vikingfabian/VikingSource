using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class CarBallTutorialScreen : AbsPJGameState
    {
        Texture2D fieldTexture;

        public CarBallTutorialScreen(List2<GamerData> joinedGamers)
            :base(false)
        {
            Input.Mouse.Visible = false;

            this.joinedLocalGamers = joinedGamers;
            Ref.draw.ClrColor = Color.Black;

            new Timer.AsynchActionTrigger(loadContent_Asynch, true);
        }

        void loadContent_Asynch()
        {
            fieldTexture = Engine.LoadContent.LoadTexture(PjLib.ContentFolder + "carballfield");
            new Sounds();
            new Timer.Action0ArgTrigger(onLoadingComplete);
        }

        void onLoadingComplete()
        {
            if (Ref.gamestate == this)
            {
                new CarBallPlayState(joinedLocalGamers, fieldTexture);
            }
        }
        

    }
}
