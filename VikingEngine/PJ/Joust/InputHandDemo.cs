using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust
{
    class InputHandDemo : AbsUpdateable
    {
        Input.IButtonMap button;
        Graphics.Image hand;

        public InputHandDemo(GamerData gamerdata)
            :base(true)
        {
            button = gamerdata.button;
            hand = new Graphics.Image(SpriteName.birdButtonPressUp,
                Engine.Screen.CenterScreen, Engine.Screen.IconSizeV2 * 4f, ImageLayers.Top1);
            hand.OrigoAtCenterWidth();
        }

        public override void Time_Update(float time_ms)
        {
            if (button.DownEvent)
            {
                hand.SetSpriteName(SpriteName.birdButtonPressDown);
            }
            else if (button.UpEvent)
            {
                hand.SetSpriteName(SpriteName.birdButtonPressUp);
            }
        }
    }
}
