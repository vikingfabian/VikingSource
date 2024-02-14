using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace VikingEngine.Graphics
{
    class Point2D : AbsDraw2D
    {
        //Just an empty point
        public override CamObjType Type
        {
            get
            {
                return CamObjType.Point;
            }
        }
        override protected bool drawable
        { get { return false; } }
        
        public Point2D()
            : this(Vector2.Zero, Vector2.One)
        { }
        public Point2D(Vector2 pos, Vector2 sz)
        {
            position = pos;
            size = sz;
        }

        public void Initpoint(Vector2 pos, Vector2 sz)
        {
            position = pos;
            size = sz;
        }
        public override void Draw(int cameraIndex)
        {
        }
        public override AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);
        }
        public override string ToString()
        {
            return "Point2D";
        }
    }
}
