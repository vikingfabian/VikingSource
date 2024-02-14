using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.TutVideo
{
    abstract class AbsVideo : AbsUpdateable
    {
        protected const float BlackScreenFadeTime = 300;

        protected VideoScreen screen;
        protected Graphics.Image blackScreen;

        public AbsVideo()
            :base(true)
        {
            screen = new VideoScreen();
        }

        protected void createBlackScreen()
        {
            blackScreen = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, screen.drawContainer.Size, ImageLayers.Top1, false, false);
            blackScreen.Color = Color.Black;
            screen.drawContainer.AddImage(blackScreen);
        }

        virtual protected void blackScreenFade(bool fadeIn)
        {
            new Graphics.Motion2d(Graphics.MotionType.OPACITY, blackScreen, 
                fadeIn? Vector2.One : VectorExt.V2NegOne, 
                Graphics.MotionRepeate.NO_REPEAT, BlackScreenFadeTime, true).ignoreImageRender();
        }

        public override void Time_Update(float time_ms)
        { }

        public override void DeleteMe()
        {
            base.DeleteMe();
            screen.DeleteMe();
        }
    }
}
