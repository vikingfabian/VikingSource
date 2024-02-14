using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Display
{
    class CountDownNumber
    {
        const int CountDownSteps = 3;
        const float CountDownSeconds = 2.832f;
        const float CountDownStepTime = CountDownSeconds / CountDownSteps;
        int countDownSecondsLeft = CountDownSteps;
        float oneCountStep = 0;
        Graphics.Image CountDownText;
        
        public CountDownNumber()
        {
            CountDownText = new Graphics.Image(SpriteName.CountDownNumber3, Engine.Screen.CenterScreen,
                new Vector2(2, 3) * Engine.Screen.IconSize * 1.2f,
                ImageLayers.Foreground4, true);
            CountDownText.Opacity = 0.5f;
            bumpCountDownText();
        }

        public bool update()
        {
            oneCountStep += Ref.DeltaTimeSec;
            if (oneCountStep >= CountDownStepTime)
            {
                oneCountStep -= CountDownStepTime;
                countDownSecondsLeft--;
                if (countDownSecondsLeft <= 0)
                {
                    CountDownText.DeleteMe();
                    return true;
                }
                else
                {
                    CountDownText.SetSpriteName(countDownSecondsLeft == 2 ? SpriteName.CountDownNumber2 : SpriteName.CountDownNumber1);
                    bumpCountDownText();
                }
            }

            return false;
        }

        void bumpCountDownText()
        {
            new Graphics.Motion2d(Graphics.MotionType.SCALE, CountDownText, CountDownText.Size * 0.6f,
                Graphics.MotionRepeate.BackNForwardOnce, 66, true);
        }
    }
}
