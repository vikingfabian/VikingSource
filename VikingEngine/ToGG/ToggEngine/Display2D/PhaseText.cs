using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG
{
    class PhaseText : AbsUpdateable
    {
        const float ViewTime = 800;
        const float FadeTime = 600;

        Graphics.TextG title, name;
        Graphics.Image bg;

        Time view = new Time(ViewTime, TimeUnit.Milliseconds);
        Time fade = new Time(FadeTime, TimeUnit.Milliseconds);


        public PhaseText(string text, Vector2 center, string playerName)
            :base(true)
        {
            title = new Graphics.TextG(LoadedFont.Regular, center, new Vector2(Engine.Screen.TextSize * 2f), Graphics.Align.CenterAll,
                    text, Color.White, ImageLayers.Foreground5);
            bg = new Graphics.Image(SpriteName.WhiteArea, title.Position, title.MeasureText() * 1.2f, ImageLayers.Foreground6, true);
            bg.Color = Color.Black;

            name = new Graphics.TextG(LoadedFont.Regular, bg.Position - bg.Size * 0.5f, new Vector2(Engine.Screen.TextSize * 1.6f),
                new Graphics.Align(new Vector2(0, 1)), playerName, Color.Black, ImageLayers.Foreground7);
        }

        public override void Time_Update(float time)
        {
            if (view.CountDown())
            {
                if (fade.CountDown())
                {
                    DeleteMe();
                }
                else
                {
                    title.Opacity = fade.MilliSeconds / FadeTime;
                    bg.Opacity = title.Opacity;
                    name.Opacity = title.Opacity;
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            title.DeleteMe();
            bg.DeleteMe();
            name.DeleteMe();
        }
    }
}
