using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust
{
    class Toasty : AbsUpdateable
    {
        public static bool BeenUsed = false; 

        Time holdTime = new Time(600);
        Graphics.Image image;
        int state_in_hold_out = 0;
        float goalX;
        

        public Toasty()
            :base(true)
        {
            BeenUsed = true;
            Vector2 size = new Vector2(6, 7) * Engine.Screen.Height * 0.06f;
            image = new Graphics.Image(SpriteName.birdToasty, new Vector2(Engine.Screen.Width, Engine.Screen.Height - size.Y),
                size, ImageLayers.Top1);
            goalX = Engine.Screen.Width - size.X * 1.5f;
        }

        public override void Time_Update(float time)
        {
            float MoveSpeed = 0.01f * image.Width * time;

            switch (state_in_hold_out)
            { 
                case 0:
                    image.Xpos -= MoveSpeed;
                    if (image.Xpos <= goalX)
                    {
                        state_in_hold_out++;
                        SoundManager.PlaySound(LoadedSound.birdToasty);
                    }
                    break;
                case 1:
                    if (holdTime.CountDown())
                    {
                        state_in_hold_out++;
                    }
                    break;
                case 2:
                    image.Xpos += MoveSpeed;
                    if (image.Xpos >= Engine.Screen.Width)
                    {
                        DeleteMe();
                    }
                    break;

            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();

            PjRef.achievements.secretToasty.Unlock();
        }
    }
}
