using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace VikingEngine.LootFest.GO.Bounds
{
    class StaticBoxBound : AbsBound
    {
        //VectorVolume volume;
        //public VectorVolume CenterScale { get { return volume; } set { volume = value; } }
        //public RectangleCentered2 PlaneCenterScale { get { return volume.PlaneRect(); } }
        public StaticBoxBound(Vector3 center, Vector3 scale, Vector3 offset)
            : base(center, scale, offset)
        {
        }

        public override void refreshBound()
        {
            base.refreshBound();
            inneSphereRadius = lib.SmallestValue(halfSize.X, halfSize.Y, halfSize.Z);
            outerBound.HalfSize = halfSize;
        }
        override public Vector2[] Vertices()
        {
            staticBoxVertices();
            return vertices;
        }
        override public bool VerticeIntersect(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                if (Math.Abs(v.X - center.X) < halfSize.X &&
                    Math.Abs(v.Y - center.Z) < halfSize.Z)
                {
                    return true;
                }
            }
            return false;
        }
        override public VikingEngine.Physics.IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                if (VikingEngine.Physics.PhysicsLib2D.Vertice2DInsideVolume(v, outerBound))
                    return new VikingEngine.Physics.IntersectDetails2D(VikingEngine.Physics.PhysicsLib2D.Vertice2DInsideVolumeDepth(v, outerBound), v);

            }
            return null;
        }

        //public bool Intersect(AbsBound otherBound)
        //{
        //    if (outerBound.Intersect(otherBound.outerBound))
        //    {
        //        switch (otherBound.Type)
        //        {
        //            case BoundShape.BoundingBox:
        //                return true;
        //            case BoundShape.Cylinder:
        //                return CollRectAndCyliner(this, otherBound);
        //            case BoundShape.Box1axisRotation:
        //                return CollBoxes(this, otherBound);

        //        }
        //    }
        //    return false;
        //}


        /// <returns>null is no collision</returns>
        override public BoundCollisionResult Intersect2(AbsBound otherBound)
        {
            if (outerBound.Intersect(otherBound.outerBound))
            {
                switch (otherBound.Type)
                {
                    case BoundShape.BoundingBox:
                        return new BoundCollisionResult(this, otherBound, outerBound.IntersectDepth(otherBound.outerBound));
                    case BoundShape.Cylinder:
                        return CollRectAndCyliner2(this, otherBound, this, otherBound);
                    case BoundShape.Box1axisRotation:
                        return CollBoxes2(this, otherBound);

                }
            }
            return null;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is StaticBoxBound)
        //    {
        //        StaticBoxBound other = (StaticBoxBound)obj;
        //        return other.volume.center == volume.center && other.volume.HalfSize == volume.HalfSize;
        //    }
        //    return false;
        //}
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //int boundID;
        //public int BoundID { get { return boundID; } }
        //public override string ToString()
        //{
        //    return Type.ToString() + "(" + boundID.ToString() + ") " + volume.ToString();
        //}

        override public BoundShape Type { get { return BoundShape.BoundingBox; } }

        public override bool UsesRotation { get { return false; } }
    }
}
