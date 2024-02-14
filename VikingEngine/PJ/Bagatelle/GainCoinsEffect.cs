//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using VikingEngine.PJ.Display;

//namespace VikingEngine.PJ.Bagatelle
//{
//    class GainCoinsEffect : AbsInGameUpdateable
//    {
//        Time lifeTime = new Time(0.5f, TimeUnit.Seconds);
//        float speedY, gravity;
//        Graphics.Image image;
//        Timer.Basic nextFrameTimer = new Timer.Basic(45, true);
//        CirkleCounterUp frame = new CirkleCounterUp(0, 5);
//        SpriteText numberText = null;

//        public GainCoinsEffect(AbsGameObject obj, int value, BagatellePlayState state)
//            :base(true)
//        {
//            Vector2 startPos = obj.bound.Center;
//            startPos.Y -= obj.bound.HalfScale.Y * 0.7f;

//            float scale = value < 10 ? 0.5f : 0.7f;

//            image = new Graphics.Image(lib.SumSpriteName(SpriteName.birdCoin1, frame.Next()), startPos,
//                new Vector2(state.BallScale * scale), BagLib.PointEffectLayer, true);
//            speedY = -state.Gravity * 50;
//            gravity = state.Gravity * 2.4f;

//            //value = 5;
//            if (value > 1)
//            {
//                image.Xpos += image.Width * 0.5f;
//                numberText = new SpriteText(value.ToString(), image.Position - VectorExt.V2FromX(image.Width * 0.5f),
//                    image.Height * 1.4f, BagLib.PointEffectLayer, new Vector2(1f, 0.5f), Color.Yellow, true);

//            }
//        }

//        public override void Time_Update(float time_ms)
//        {
//            speedY += gravity;
//            image.Ypos += speedY;

//            if (numberText != null)
//            {
//                numberText.SetY(image.Ypos);
//            }

//            if (nextFrameTimer.Update())
//            {
//                image.SetSpriteName(lib.SumSpriteName(SpriteName.birdCoin1, frame.Next()));
//            }
//            if (lifeTime.CountDown())
//            {
//                image.Opacity -= 0.1f;

//                if (numberText != null)
//                {
//                    numberText.SetOpacity(image.Opacity);
//                }

//                if (image.Opacity <= 0)
//                {
//                    this.DeleteMe();
//                }
//            }
//        }

//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            image.DeleteMe();
//            if (numberText != null)
//            {
//                numberText.DeleteMe();
//            }
//        }
//    }
//}
