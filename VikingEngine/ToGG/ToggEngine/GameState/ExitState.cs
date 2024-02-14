using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.GameState
{
    class ExitState : Engine.GameState
    {
        Time viewTime = new Time(200);
        
        public ExitState()
            : base()
        {
            draw.ClrColor = Color.Black;

            Ref.lobby.disconnect(null);
        }

        public void viewMessage(string text)
        {
            viewTime.MilliSeconds = 1000;
            Graphics.Text2 txtimg = new Graphics.Text2(text, LoadedFont.Bold,
                Engine.Screen.CenterScreen, Engine.Screen.SmallIconSize, Color.Yellow, ImageLayers.Lay0);
            txtimg.OrigoAtCenter();
        }

        public void LostConnectionMessage(string reason)
        {
            viewTime.Seconds = 20;

            float textH = Engine.Screen.TextTitleHeight;

            var lost = new Graphics.Text2("Disconnected", LoadedFont.Regular, Engine.Screen.CenterScreen,
                textH, Color.Yellow, ImageLayers.Lay1);
            lost.Ypos -= textH;
            lost.OrigoAtCenterWidth();

            var reasonTxt = new Graphics.Text2(reason, LoadedFont.Regular, Engine.Screen.CenterScreen,
                textH, Color.White, ImageLayers.Lay1);
            reasonTxt.CheckCharsSafety();
            reasonTxt.OrigoAtCenterWidth();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            
            if (viewTime.CountDown() ||
                (toggRef.inputmap.anyExitKey() && UpdateCount > 16))
            {
                new MainMenuState();
            }
        }
    }
}
