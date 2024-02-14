using System;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    class RectangleBound : AbsBound2D
    {
        public RectangleCentered area;
        float radius;

        public RectangleBound()
        { }

        public RectangleBound(Vector2 halfSz)
        {
            this.area = new RectangleCentered(Vector2.Zero, halfSz);
            updateRadius();
        }

        public RectangleBound(Vector2 center, Vector2 halfSize)
        {
            this.area = new RectangleCentered(center, halfSize);
            radius = 0;
            updateRadius();
        }

        public RectangleBound(RectangleCentered _area)
        {
            this.area = _area;
            radius = 0;
            updateRadius();
        }

        override public void baseUpdate(Vector2 center, float rotation)
        {
            area.Center = center;
        }

        override public Vector2[] Vertices()
        {
            area.UpdateVertices(ref vertices);
            return vertices;
        }
        override public bool VerticeIntersect(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                if (area.IntersectPoint(v))
                    return true;
            }
            return false;
        }

        override public float ExtremeRadius { get { return radius; } }

        override public float InnerCirkleRadius
        { get { return (area.HalfSizeX < area.HalfSizeY ? area.HalfSizeX : area.HalfSizeY); } }

        protected void updateRadius()
        {
            const float Diagonal = 1.414f;
            radius = VectorExt.SideLength(area.HalfSizeX, area.HalfSizeY) * Diagonal;

        }
        //void updateRadius()
        //{
        //    radius = (area.HalfSizeX > area.HalfSizeY ? area.HalfSizeX : area.HalfSizeY);
        //}

        override public bool Intersect(AbsBound2D otherBound)
        {
            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                if (PhysicsLib2D.MinRadiusColl(this, otherBound)) return true;

                switch (otherBound.Type)
                {
                    case Bound2DType.Rectangle:
                        RectangleBound otherRect = (RectangleBound)otherBound;

                        Vector2 collisionDiff = area.Center - otherRect.Center;
                        Vector2 radius = area.HalfSize + otherRect.HalfSize;
                        return (Math.Abs(collisionDiff.X) <= radius.X && Math.Abs(collisionDiff.Y) <= radius.Y);
                    case Bound2DType.RectangleRotated:
                        return PhysicsLib2D.CollRectangles(otherBound, this);
                    case Bound2DType.Circle:
                        return PhysicsLib2D.CollCirkleAndRectangle((CircleBound)otherBound, this);
                }
            }
            return false;
        }

        override public bool Intersect(Vector2 point)
        {
            return area.IntersectPoint(point);
        }

        override public Collision2D Intersect2(AbsBound2D otherBound)
        {
            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                switch (otherBound.Type)
                {
                    case Bound2DType.Circle:
                        return PhysicsLib2D.CollCirkleAndRectangle_v2(otherBound, this, false);

                    case Bound2DType.Rectangle:
                        return RectangleIntersection((RectangleBound)otherBound);

                    case Bound2DType.RectangleRotated:
                        return PhysicsLib2D.CollRectangles_V2(this, otherBound);
                        
                    default:
                        throw new System.NotImplementedException();
                }
            }
            return Collision2D.NoCollision;
        }

        public Collision2D RectangleIntersection(RectangleBound other)
        {
            //if (PhysicsLib2D.ExtremeRadiusColl(this, other))
            //{
            Vector2 collisionDiff = area.Center - other.Center;
            Vector2 sumHalfWidth = area.HalfSize + other.HalfSize;
            if (Math.Abs(collisionDiff.X) <= sumHalfWidth.X && Math.Abs(collisionDiff.Y) <= sumHalfWidth.Y)
            {
                Collision2D result = new Collision2D();
                result.IsCollision = true;
                result.intersectionSize = sumHalfWidth - lib.AbsVector2(collisionDiff);

                if (result.intersectionSize.X < result.intersectionSize.Y)
                {
                    result.direction.X = lib.ToLeftRight(collisionDiff.X);
                    result.depth = result.intersectionSize.X;
                }
                else
                {
                    result.direction.Y = lib.ToLeftRight(collisionDiff.Y);
                    result.depth = result.intersectionSize.Y;
                }
                return result;
            }
            //}
            return Collision2D.NoCollision;
        }

        override public Vector2 Center { get { return area.Center; } set { area.Center = value; } }
        override public Vector2 HalfSize { get { return area.HalfSize; } set { area.HalfSize = value; updateRadius(); } }
        override public float Rotation { get { return 0; } set { } }
        override public Bound2DType Type { get { return Bound2DType.Rectangle; } }
        override public VectorRect Area { get { return area.VectorRect(); } }

        public override RectangleCentered AreaCentered => area;

        public override AbsBound2D Clone()
        {
            return clone();
        }
        public RectangleBound clone()
        {
            RectangleBound clone = new RectangleBound();
            clone.offset = this.offset;
            clone.rotationAdd = this.rotationAdd;
            clone.area = this.area;
            clone.updateRadius();

            return clone;
        }

        public override string ToString()
        {
            return "Rectangle " + area.ToString();
        }
    }
}
