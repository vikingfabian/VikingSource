using System;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    class RectangleRotatedBound : RectangleBound
    {
        //public RectangleCentered area;
        public Rotation1D rotation;
        //float radius;

        public RectangleRotatedBound()
        { }

        public RectangleRotatedBound(Vector2 halfSz)
        {
            this.area = new RectangleCentered(Vector2.Zero, halfSz);
            updateRadius();
        }

        public RectangleRotatedBound(RectangleCentered area, Rotation1D rotation)
        {
            this.area = area;
            this.rotation = rotation;
            updateRadius();
        }

        override public void baseUpdate(Vector2 center, float rotation)
        {
            area.Center = center;
            this.rotation.Radians = rotation;
        }

        override public bool VerticeIntersect(Vector2[] vertices)
        {
            float negativRot = MathExt.Tau - rotation.Radians;
            foreach (Vector2 v in vertices)
            {
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(area.Center, v, negativRot);
                if (area.IntersectPoint(adjustedVertice))
                    return true;
            }
            return false;
        }
        override public Vector2[] Vertices()
        {
            area.UpdateVertices(ref vertices);
            if (rotation.Radians != 0)
            { //Rotate the vertices
                for (int i = 0; i < vertices.Length; ++i)
                {
                    vertices[i] = lib.RotatePointAroundCenter(area.Center, vertices[i], rotation.Radians);
                }
            }
            return vertices;
        }

        //override public float ExtremeRadius { get { return radius; } }

        override public float InnerCirkleRadius
        { get { return (area.HalfSizeX < area.HalfSizeY ? area.HalfSizeX : area.HalfSizeY); } }

        override public bool Intersect(AbsBound2D otherBound)
        {
            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                if (PhysicsLib2D.MinRadiusColl(this, otherBound)) return true;

                switch (otherBound.Type)
                {
                    case Bound2DType.Rectangle:
                        return PhysicsLib2D.CollRectangles(otherBound, this);
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
            return area.IntersectPoint(point, rotation.Radians);
        }

        override public Collision2D Intersect2(AbsBound2D otherBound)
        {
            if (PhysicsLib2D.ExtremeRadiusColl(this, otherBound))
            {
                switch (otherBound.Type)
                {
                    case Bound2DType.RectangleRotated:
                    case Bound2DType.Rectangle:
                        return PhysicsLib2D.CollRectangles_V2(this, otherBound);
                        
                    default:
                        throw new System.NotImplementedException();
                }
            }
            return Collision2D.NoCollision;
        }
        public override AbsBound2D Clone()
        {
            return clone();
        }

        public RectangleRotatedBound clone()
        {
            RectangleRotatedBound clone = new RectangleRotatedBound();
            clone.offset = this.offset;
            clone.rotationAdd = this.rotationAdd;
            clone.area = this.area;
            clone.rotation = this.rotation;
            clone.updateRadius();

            return clone;
        }

        //override public Vector2 Center { get { return area.Center; } set { area.Center = value; } }
        //override public Vector2 HalfSize { get { return area.HalfSize; } set { area.HalfSize = value; updateRadius(); } }
        override public float Rotation { get { return rotation.Radians; } set { rotation.Radians = value; } }
        override public Bound2DType Type { get { return Bound2DType.RectangleRotated; } }
        //override public VectorRect Area { get { return area.VectorRect(); } }

        public override string ToString()
        {
            return "Rectangle rot " + area.ToString() + ", " + rotation.ToString();
        }
    }

}
