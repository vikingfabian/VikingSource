using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SmashBirds
{
    class Shield
    {
        const float Delay = 60;
        const float ActiveTime = 120;

        Graphics.Image icon;//, outline;
        public bool active = false;
        Gamer gamer;
        Time stateTime;

        public Shield(Gamer gamer)
        {
            this.gamer = gamer;
            stateTime.MilliSeconds = Delay;
        }

        public bool update()
        {
            if (!active)
            {
                if (stateTime.CountDown())
                {
                    active = true;
                    icon = new Graphics.Image(SpriteName.BirdShieldIcon, gamer.Center, new Vector2(gamer.image.Height * 0.6f),
                        SmashLib.LayShieldIcon, true);
                    //outline = new Graphics.Image(SpriteName.birdBumpWave, gamer.Center, new Vector2(gamer.image.Height),
                    //    SmashLib.LayShield, true);
                    //outline.Opacity = 0.2f;

                    stateTime.MilliSeconds = ActiveTime;
                }
            }

            if (active)
            {
                icon.position = gamer.Center;
                //outline.position = icon.position;

                return stateTime.CountDown();
            }

            return false;
        }

        public void DeleteMe()
        {
            if (icon != null)
            {
                icon.DeleteMe();
                //outline.DeleteMe();
            }
        }
    }
}
