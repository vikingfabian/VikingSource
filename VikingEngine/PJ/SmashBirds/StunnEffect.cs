using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.SmashBirds
{
    class StunnEffect
    {
        Time timer = new Time(2f, TimeUnit.Seconds);
        public Time immortality = new Time(120);
        Graphics.Image image;
        Gamer gamer;

        public StunnEffect(Gamer gamer)
        {
            this.gamer = gamer;
            image = new Graphics.Image(SpriteName.WhiteArea,
                Vector2.Zero, new Vector2(gamer.image.Width * 0.6f),
                SmashLib.LayCharacterEffect, true);
            image.Color = Color.Yellow;
            update();
        }

        public bool update()
        {
            image.position = VectorExt.AddY(gamer.image.position, - gamer.image.Height * 0.8f);
            image.Rotation += 4f * Ref.DeltaGameTimeSec;

            immortality.CountDown();
            return timer.CountDown();
        }

        public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
