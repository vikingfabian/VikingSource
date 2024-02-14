using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.PJ.Display;
//using VikingEngine.LootFest.Display;

namespace VikingEngine.PJ.PjEngine.Effect
{
    class GainCoinsEffect : AbsInGameUpdateable
    {
        Time lifeTime = new Time(0.5f, TimeUnit.Seconds);
        float speedY, gravity;
        Graphics.Image coinImage;
        Timer.Basic nextFrameTimer = new Timer.Basic(45, true);
        CirkleCounterUp frame = new CirkleCounterUp(0, 5);
        SpriteText numberText = null;

        public GainCoinsEffect(Vector2 objCenterTop, int value, ImageLayers lay)
            : base(true)
        {
            var iconSz = Engine.Screen.IconSize;
            var Gravity = 0.02f * Engine.Screen.IconSize;

            Vector2 startPos = VectorExt.AddY(objCenterTop, -iconSz * 0.5f);

            float scale = value < 10 ? 0.65f : 0.85f;

            coinImage = new Graphics.Image(lib.SumSpriteName(SpriteName.birdCoin1, frame.Next()), startPos,
                new Vector2(iconSz * scale), lay, true);
            
            gravity = 0.00024f * Engine.Screen.Height;
            speedY = -gravity * 20;

            if (value > 1)
            {
                coinImage.Xpos += coinImage.Width * 0.4f;
                numberText = new SpriteText(value.ToString(), coinImage.Position - VectorExt.V2FromX(coinImage.Width * 0.5f),
                    coinImage.Height * 0.6f, lay, new Vector2(1f, 0.5f), Color.Yellow, true);

            }
        }

        public override void Time_Update(float time_ms)
        {
            speedY += gravity;
            coinImage.Ypos += speedY;

            if (numberText != null)
            {
                numberText.SetY(coinImage.Ypos);
            }

            if (nextFrameTimer.Update())
            {
                coinImage.SetSpriteName(lib.SumSpriteName(SpriteName.birdCoin1, frame.Next()));
            }
            if (lifeTime.CountDown())
            {
                coinImage.Opacity -= 0.1f;

                if (numberText != null)
                {
                    numberText.SetOpacity(coinImage.Opacity);
                }

                if (coinImage.Opacity <= 0)
                {
                    this.DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            coinImage.DeleteMe();
            if (numberText != null)
            {
                numberText.DeleteMe();
            }
        }
    }
}
