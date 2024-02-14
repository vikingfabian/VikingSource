using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust
{
    class OnGroundTimer
    {        
        Display.SpriteText number;
        Gamer gamer;
        int secondsLeft;
        float oneSec = 0f;
        bool autojump;

        public OnGroundTimer(Gamer gamer, bool autojump)
        {
            this.autojump = autojump;
            this.gamer = gamer;

            secondsLeft = autojump ? JoustLib.OnGroundAutojumpSeconds : JoustLib.OnGroundSeconds;

            number = new Display.SpriteText(secondsLeft.ToString(), gamer.Position + new Vector2(0f, gamer.Scale.Y * 0.6f),
                Engine.Screen.IconSize, JoustLib.LayerGroundTimer, VectorExt.V2Half, autojump ? PjLib.RedNumbersColor : Color.White, true);
        }

        public bool update()
        {
            oneSec += Ref.DeltaTimeSec;

            if (oneSec >= 1f)
            {
                oneSec -= 1f;
                secondsLeft--;
                if (secondsLeft <= 0)
                {
                    //gamer.OnGroundTimeOut(autojump);
                    DeleteMe();
                    return true;
                }
                else
                {

                    number.Text( secondsLeft.ToString());
                }
            }

            if (!gamer.Alive)
            {
                DeleteMe();
            }

            return false;
        }

        public void DeleteMe()
        {
            number.DeleteMe();
        }
    }
}
