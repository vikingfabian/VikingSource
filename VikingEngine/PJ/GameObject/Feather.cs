using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ
{
    class Feather : AbsUpdateable
    {
        Graphics.Image img;

        float rotateSpeed = Ref.rnd.LeftRight() * Ref.rnd.Float(0.001f, 0.01f);
        Vector2 initialSpeed;

        public Feather(SpriteName tile, float depthAdd, Vector2 pos, Rotation1D dir, float gamerImageScale, Color color, bool move)
            : base(true)
        {
            //BirdLib.SetGameLayer();
            img = new Graphics.Image(tile, pos, new Vector2(gamerImageScale * 0.5f), PjLib.LayerFeather, true);
            img.Color = color;
            img.PaintLayer += depthAdd;
            if (move)
            {
                initialSpeed = dir.Direction(gamerImageScale * 0.004f);
            }
            else
            {
                initialSpeed = Vector2.Zero;
            }
        }

        public override void Time_Update(float time)
        {
            img.Rotation += rotateSpeed * time;

            img.Position += initialSpeed * time;
            initialSpeed *= 0.9f;

            img.Ypos += Engine.Screen.Height * 0.00014f * time;

            if (img.Ypos - img.Height > Engine.Screen.Height)
            {
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
           //BirdLib.SetGameLayer();
            base.DeleteMe();
            img.DeleteMe();
        }
    }
}
