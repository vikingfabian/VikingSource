using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.PJ.Bagatelle
{
    class LeadingPlayerCrown : AbsUpdateable
    {
        Graphics.Image image;
        //Joust.Gamer gamer;

        CirkleCounterUp frameIndex = new CirkleCounterUp(0, 5);
        //public bool endState = false;
        int frameTime = 0;


        public LeadingPlayerCrown(Vector2 pos)
            : base(true)
        {
            //Vector2 offset = Vector2.Zero;
           // offset.Y = -Joust.Gamer.ImageScale * 0.5f;

            image = new Graphics.Image(SpriteName.birdRotatingCrown1, 
                pos, 
                new Vector2(Engine.Screen.IconSize * 1.4f),
                ImageLayers.Top8, false);
            image.origo = VectorExt.V2HalfY;
        }

        public override void Time_Update(float time_ms)
        {
            if (++frameTime >= 4)
            {
                frameTime = 0;
                image.SetSpriteName((SpriteName)((int)SpriteName.birdRotatingCrown1 + frameIndex.Next()));
            }

            //if (endState)
            //{
            //    image.Ypos -= 3f;
            //    image.Opacity -= 0.02f;
            //    if (image.Opacity <= 0f)
            //    {
            //        DeleteMe();
            //    }
            //}
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}
