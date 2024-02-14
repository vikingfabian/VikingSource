using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Bounds
{
    class CylinderBound : AbsBound
    {
        //VectorVolume outerBound;
        //CylinderVolume volume;

        public CylinderBound(Vector3 center, Vector3 scale, Vector3 offset)
            : base(center, scale, offset)
        {
            //this.volume = volume;
            //outerBound = VectorVolume.ZeroOne;
            //updateouterBound();
        }
        //public CylinderVolume InnerCirkle { get { return volume; } }
        //public VectorVolume outerBound
        //{
        //    get
        //    {
        //        return outerBound;

        //    }
        //}

        public override void refreshBound()
        {
            base.refreshBound();
            inneSphereRadius = lib.SmallestValue(halfSize.X, halfSize.Y);
            outerBound.HalfSize.X = halfSize.X;
            outerBound.HalfSize.Y = halfSize.Y;
            outerBound.HalfSize.Z = halfSize.X;
        }

        //public bool Intersect(AbsBound otherBound)
        //{
        //    return Intersect2(otherBound) != null;
        //}

        /// <returns>null is no collision</returns>
        override public BoundCollisionResult Intersect2(AbsBound otherBound)
        {
            if (outerBound.Intersect(otherBound.outerBound))
            {
                switch (otherBound.Type)
                {
                    case BoundShape.BoundingBox:
                        return CollRectAndCyliner2(otherBound, this, this, otherBound);
                    case BoundShape.Box1axisRotation:
                        return CollRectAndCyliner2(otherBound, this, this, otherBound);
                    case BoundShape.Cylinder:
                        if (IntersectCylinders(this, otherBound))//volume.Intersect(otherBound.InnerCirkle))
                            return new BoundCollisionResult(this, otherBound, IntersectLength2D(this, otherBound));//volume.IntersectLength2D(otherBound.InnerCirkle));
                        return null;

                }
            }
            return null;
        }

        override public VikingEngine.Physics.IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Circle flatCirkle = this.flatCirkle();//volume.PlaneCirkle();
            foreach (Vector2 v in vertices)
            {
                if (flatCirkle.IntersectPoint(v)) return flatCirkle.IntersectPointDepth(v);
            }
            return null;
        }

        override public bool VerticeIntersect(Vector2[] vertices)
        {
            Circle flatCirkle = this.flatCirkle();
            foreach (Vector2 v in vertices)
            {
                if (flatCirkle.IntersectPoint(v)) return true;
            }
            return false;
        }

        override public Vector2[] Vertices() { throw new NotImplementedException(); }

        //void updateouterBound()
        //{
        //    outerBound = new VectorVolume(volume.center, new Vector3(volume.Radius, volume.HalfHeight, volume.Radius));
        //}

        //public Vector3 Center { get { return volume.center; } set { volume.center = value; outerBound.center = value; } }
        //public float Rotation { get { return 0; } set { } }
        //public VectorVolume CenterScale
        //{
        //    get { return new VectorVolume(volume.center, new Vector3(volume.Radius, volume.HalfHeight, volume.Radius)); }
        //    set { volume.center = value.center; volume.Radius = value.HalfSizeX; volume.HalfHeight = value.HalfSizeY; }
        //}

        //public RectangleCentered2 PlaneCenterScale { get { throw new NotImplementedException(); } }

        override public BoundShape Type { get { return BoundShape.Cylinder; } }

        //public override bool Equals(object obj)
        //{
        //    if (obj is CylinderBound)
        //    {
        //        CylinderBound other = (CylinderBound)obj;
        //        return other.volume.center == volume.center && other.volume.HalfHeight == volume.HalfHeight && other.volume.Radius == volume.Radius;
        //    }
        //    return false;
        //}
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //int boundID;
        //public int BoundID { get { return boundID; } }
        public override bool UsesRotation { get { return false; } }
    }
}
