using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Display
{
    class TimesUpDisplay : AbsUpdateable
    {
        Graphics.Image clock;
        SpriteText time;
        float goalScale;

        bool firstFrame = true;
        int animationTime;

        public TimesUpDisplay(int timeLeft)
            : base(false)
        {
            clock = new Graphics.Image(SpriteName.BirdTimesUp1, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.IconSize), ImageLayers.Foreground2, true);
            clock.Xpos -= clock.Width * 0.2f;
            goalScale = 3f * clock.Size1D;

            if (timeLeft > 0)
            {
                time = new SpriteText(timeLeft.ToString(), Engine.Screen.CenterScreen + new Vector2(clock.Width * 0.2f, 0f), clock.Height * 1f, ImageLayers.Foreground3, new Vector2(0, 0.5f), Color.White, true);
            }
            else
            {
                onTimeup();
            }
        }

        int prevTimeLeft = int.MinValue;
        public void updateTimeLeft(int timeLeft)
        {
            if (timeLeft != prevTimeLeft)
            {
                prevTimeLeft = timeLeft;
                if (timeLeft == 0)
                {
                    time.DeleteMe();
                    onTimeup();
                }
                else
                {
                    time.Text(timeLeft.ToString());
                }
            }
        }

        void onTimeup()
        {
            new Sound.SoundSettings(LoadedSound.birdTimesUp, 1f).PlayFlat();
            this.AddToUpdateList();
        }

        public override void Time_Update(float time_ms)
        {
            if (clock.Size1D < goalScale)
            {
                clock.Size1D += Engine.Screen.IconSize * 0.1f;
            }

            if (++animationTime >= 2)
            {
                animationTime = 0;
                firstFrame = !firstFrame;

                clock.SetSpriteName(firstFrame ? SpriteName.BirdTimesUp1 : SpriteName.BirdTimesUp2);
            }

            clock.Rotation = (float)Math.Sin(Ref.TotalTimeSec * 100f) * 0.06f;
        }

        public void fadeOut()
        {
            new Graphics.Motion2d(Graphics.MotionType.OPACITY, clock, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, 200, true);
        }
    }
}
