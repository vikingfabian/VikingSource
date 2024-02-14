using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//#if CCG
namespace VikingEngine.HUD
{
    class DialogueBoxWithTimer : DialogueBox
    {
        public Time timeLimit;
        Graphics.TextS timeText;
        public Action timeOutEvent;


        public DialogueBoxWithTimer(string title, string text, DialougeBoxOption[] options, VectorRect area, ImageLayers layer, Time timeLimit, Action timeOutEvent)
            :base(title, text, options, area, layer)
        {
            this.timeOutEvent = timeOutEvent;
            this.timeLimit = timeLimit;
           
            background.Height += Engine.Screen.IconSize * 1.5f;

            timeText = new Graphics.TextS(LoadedFont.Regular, new Vector2(textBox.Xpos, background.Bottom - 4), 
                new Vector2(Engine.Screen.TextSize * 1.5f), new Graphics.Align(new Vector2(0, 1)), 
                "--:--", Color.White, contertlayer);

            images.Add(timeText);
        }

        int prevSec = int.MinValue;

        public override DialogueResult Update()
        {
            if (timeLimit.CountDown())
            {
                if (timeOutEvent != null)
                { timeOutEvent(); }
                return DialogueResult.TimeOut;
            }

            int min = (int)timeLimit.Minutes;
            int sec = (int)(timeLimit.Seconds - min * 60);

            if (prevSec != sec)
            {
                prevSec = sec;
                timeText.TextString = min.ToString("00") + ":" + sec.ToString("00");
            }
            return base.Update();
        }
    }
}
//#endif
