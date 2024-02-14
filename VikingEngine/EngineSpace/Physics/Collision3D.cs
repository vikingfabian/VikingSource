using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    class Collision3D //null is no collision
    {
        public IBound3D MyBound;
        public IBound3D OtherBound;
        float intersectionDepth;
        public float IntersectionDepth
        {
            get { return intersectionDepth; }
            set { intersectionDepth = Bound.Max(value, 0.8f); }
        }
        public Vector3 IntersectionCenter;

        public Collision3D(IBound3D MyBound, IBound3D OtherBound, float IntersectionDepth, Vector3 IntersectionCenter)
        {
            this.MyBound = MyBound;
            this.OtherBound = OtherBound;
            intersectionDepth = 0;
            this.IntersectionDepth = IntersectionDepth;
            this.IntersectionCenter = IntersectionCenter;
        }

        public Collision3D(IBound3D MyBound, IBound3D OtherBound, float IntersectionDepth, Vector2 IntersectionCenter)
            : this(MyBound, OtherBound, IntersectionDepth, Vector3.Zero)
        {
            this.IntersectionCenter.X = IntersectionCenter.X;
            this.IntersectionCenter.Y = (OtherBound.Center.Y + MyBound.Center.Y) * PublicConstants.Half;
            this.IntersectionCenter.Z = IntersectionCenter.Y;
        }

        public Collision3D(IBound3D MyBound, IBound3D OtherBound, IntersectDetails2D intersectDetails)
            : this(MyBound, OtherBound, intersectDetails.Depth, intersectDetails.IntersectionCenter)
        { }
        public Collision3D(IBound3D MyBound, IBound3D OtherBound, IntersectDetails3D intersectDetails)
            : this(MyBound, OtherBound, intersectDetails.Depth, intersectDetails.IntersectionCenter)
        { }

        public Collision3D OtherObjIntersection()
        {
            return new Collision3D(OtherBound, MyBound, IntersectionDepth, IntersectionCenter);
        }

        public override string ToString()
        {
            return "Depth: " + IntersectionDepth.ToString() + ", My bound:" + MyBound.ToString() + ", Other bound:" + OtherBound.ToString();
        }
    }

    public class IntersectDetails3D
    {
        public float Depth;
        public Vector3 IntersectionCenter;

        public IntersectDetails3D()
        { }
        public IntersectDetails3D(float depth, Vector3 vertice)
        {
            this.Depth = depth;
            this.IntersectionCenter = vertice;
        }
    }
}
