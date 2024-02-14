using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.PJ.Joust;

namespace VikingEngine.PJ.GameObject
{
    class EggBallChick : AbsUpdateable
    {
        Graphics.Image image;
        Vector2 speed = Vector2.Zero;
        float gravity = Joust.Gamer.Gravity * 15f;
        public static int ChickCount = 0;
        //Joust.Level level;
        bool faceLeft = Ref.rnd.Bool();//lib.RandomBool();

        Time nextStateTime = new Time(10000);
        bool fallState = true;

        public EggBallChick(Vector2 startPos, float shellH)
            : base(false)
        {
            JoustRef.gamestate.endLevelAchievements.Add(PjRef.achievements.secretChickenEgg);
            //level = ((Joust.JoustGameState)Ref.gamestate).level;
            if (startPos.Y <= JoustRef.level.groundY)
            {
                image = new Graphics.Image(SpriteName.easterChick1, startPos, new Vector2(shellH * 1.5f), Joust.JoustLib.LayerShellChick);
                image.PaintLayer += PublicConstants.LayerMinDiff * ChickCount++;
                image.origo = new Vector2(0.5f, 0.9f);
                
                speed.Y = -gravity * 20f;
                AddToUpdateList();
            }
        }

        public override void Time_Update(float time_ms)
        {
            speed.Y += gravity;
            image.Position += speed;

            if (image.Ypos >= JoustRef.level.groundY)
            {
                image.Ypos = JoustRef.level.groundY;

                speed.Y *= -0.2f;

                if (Math.Abs(speed.Y) < Math.Abs(gravity * 2f))
                {
                    speed.Y = 0f;
                    speed.X = 0f;
                    if (fallState)
                    {
                        nextStateTime.MilliSeconds = Bound.Max(nextStateTime.MilliSeconds, 200);
                        fallState = false;
                    }
                }
            }

            if (nextStateTime.CountDown())
            {
                float random = Ref.rnd.Float(100);

                if (random < 20f)
                {
                    //flip
                    faceLeft = !faceLeft;
                    nextStateTime.MilliSeconds = 1200;
                }
                else if (random < 40f)
                {
                    //eye flash
                    image.SetSpriteName(SpriteName.easterChick2);
                    new Timer.TimedAction0ArgTrigger(openEyes, 400);

                    nextStateTime.MilliSeconds = 600;
                }
                else if (random < 60f)
                {
                    //jump
                    speed = new Vector2(lib.BoolToLeftRight(!faceLeft) * Math.Abs(gravity) * 4f,
                        -gravity * 6f);

                    nextStateTime.MilliSeconds = 10000;
                    fallState = true;
                }
                else
                {
                    //nothing
                    nextStateTime.MilliSeconds = Ref.rnd.Float(400, 600);
                }
            }


            image.spriteEffects = faceLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        void openEyes()
        {
            image.SetSpriteName(SpriteName.easterChick1);
        }
    }
}
