using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class TextFeed : AbsTimer
    {
        string text = TextLib.EmptyString;
        int currentLetter = 0;
        Graphics.AbsTextLine textObject;
        const float TimePerLetter = 100;

        public TextFeed(int startTime, Graphics.AbsTextLine textObj)
            : this(startTime, textObj, textObj.TextString)
        {
            textObject.TextString = TextLib.EmptyString;
        }
        public TextFeed(int startTime, Graphics.AbsTextLine textObj, string text)
            : base(startTime, UpdateType.Full)
        {
            textObject = textObj;
            this.text = text;
            timeLeft = TimePerLetter;
        }

        public override void Time_Update(float time)
        {
            
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                textObject.AddChar(text[currentLetter]);
                currentLetter++;
                if (currentLetter >= text.Length)
                {
                    DeleteMe();
                }
                timeLeft = TimePerLetter;
                //int letters = (int)(PublicConstants.TEXT_FEED_SPEED * -timeLeft);
                //if (letters >= text.Length)
                //{ textObject.TextString = text; DeleteMe(); }
                //else
                //{
                //    textObject.TextString = text.Remove(letters, text.Length - letters);
                //}
            }
            //return false;    
        }
        public override void PreDeleteMe(VikingEngine.Engine.GameState state)
        {
            textObject.TextString = text;
            base.PreDeleteMe(state);
        }
        public override float TimeLeft
        {
            get
            {
                return timeLeft + text.Length * TimePerLetter;
            }
        }
    }
}
