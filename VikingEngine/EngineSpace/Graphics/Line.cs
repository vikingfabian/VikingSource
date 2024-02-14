using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.Graphics
{
    class Line : Image
    {
        Vector2 point1, point2;

        public float Thickness
        {
            get { return this.Height; }
            set { this.Height = value; }
        }

        public Line(float thickness, ImageLayers layer, Color color, Vector2 point1, Vector2 point2)
            : this(thickness, layer, color, point1, point2, true)
        { }

        public Line(float thickness, ImageLayers layer, Color color, Vector2 point1, Vector2 point2, bool addToRender)
            : base(SpriteName.WhiteArea, Vector2.Zero, Vector2.One * thickness, layer, false, addToRender)
        {
            this.point1 = point1;
            this.point2 = point2;

            this.pureColor = color;
            this.origo = new Vector2(0, PublicConstants.Half);
            updateLine();
        }

        public void UpdateLine(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
            updateLine();
        }

        void updateLine()
        {
            this.position = point1;
            Vector2 diff = point2 - point1;

            this.Width = diff.Length();
            this.rotation = (float)Math.Atan2(diff.Y, diff.X);
        }
        public void PointPos(bool firstPoint, Vector2 pos)
        {
            if (firstPoint)
                point1 = pos;
            else
                point2 = pos;
            updateLine();
        }
        
        public bool Intersects(Line other, out Vector2 intersection)
        {
            Vector2 r = point2 - point1;
            Vector2 s = other.point2 - other.point1;
            float rxs = MathExt.V2Cross(r, s);
            
            if (MathExt.NearZero(rxs))
            {
                intersection = Vector2.Zero;
                return false;
            }

            float qpxr = MathExt.V2Cross(other.point1 - point1, r);

            float t = MathExt.V2Cross(other.point1 - point1, s) / rxs;
            float u = qpxr / rxs;

            if ((0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                intersection = point1 + t * r;
                return true;
            }

            intersection = Vector2.Zero;
            return false;
        }

        public Vector2 P1
        {
            get { return point1; }
            set { point1 = value; updateLine(); }
        }
        public Vector2 P2
        {
            get { return point2; }
            set { point2 = value; updateLine(); }
        }
    }
}
