using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Bounds
{
    class Box1axisBound : AbsBound
    {
        //VectorVolume outerBound;
        // VectorVolume innerBound;
        //VectorVolume volume;
        //public VectorVolume CenterScale { get { return volume; } set { volume = value; } }
        //public RectangleCentered2 PlaneCenterScale { get { return volume.PlaneRect(); } }
        //public Rotation1D rotation;

        public Box1axisBound(Vector3 center, Vector3 scale, Vector3 offset)
            : base(center, scale, offset)
        {
            //boundID = PhysicsLib3D.NextID();
            //volume = _area;
            //outerBound = volume;
            ////innerBound = volume;
            //rotation = _rotation;
            //updateBoundingBox();
        }

        override public bool VerticeIntersect(Vector2[] vertices)
        {
            Rotation1D negativRot = new Rotation1D(-rotation);
            Vector2 pos = VectorExt.V3XZtoV2(center);

            foreach (Vector2 v in vertices)
            {
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(pos, v, negativRot.Radians);
                if (VikingEngine.Physics.PhysicsLib2D.Vertice2DInsideVolume(adjustedVertice, outerBound))
                    return true;
            }
            return false;
        }
        override public VikingEngine.Physics.IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Rotation1D negativRot = new Rotation1D(-rotation);
            Vector2 pos = VectorExt.V3XZtoV2(center);

            foreach (Vector2 v in vertices)
            {
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(pos, v, negativRot.Radians);
                if (VikingEngine.Physics.PhysicsLib2D.Vertice2DInsideVolume(adjustedVertice, outerBound))
                    return new VikingEngine.Physics.IntersectDetails2D(VikingEngine.Physics.PhysicsLib2D.Vertice2DInsideVolumeDepth(adjustedVertice, outerBound), v);
            }
            return null;
        }
        override public Vector2[] Vertices()
        {
            Vector2 planeCenter = VectorExt.V3XZtoV2(center);
            staticBoxVertices();

            if (rotation != 0 && rotation != MathHelper.Pi)
            { //Rotate the vertices
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = lib.RotatePointAroundCenter(planeCenter, vertices[i], rotation);
                }
            }
            //if (PlatformSettings.ViewCollisionBounds)
            //{
            //    PhysicsLib2D.DebugViewCollisionVertices(vertices, volume.center);
            //}
            return vertices;

        }
        public override void refreshBound()
        {
            //Vector2 diff = Vector2.Zero;
            //diff.X = volume.HalfSizeX;
            //diff.Y = volume.HalfSizeZ;

            float largestRadius = VectorExt.Length(halfSize.X, halfSize.Z);//diff.Length();
            //outerBound. = largestRadius;
            outerBound.HalfSize.X = largestRadius;
            outerBound.HalfSize.Y = halfSize.Y;
            outerBound.HalfSize.Z = largestRadius;

        }

        //public VectorVolume outerBound
        //{
        //    get
        //    {
        //        return outerBound;
        //    }
        //}

        //public CylinderVolume InnerCirkle { get { return new CylinderVolume(volume.center, volume.HalfSizeY, volume.HalfSizeX); } }

        //public bool Intersect(AbsBound otherBound)
        //{
        //    if (this.outerBound.Intersect(otherBound.outerBound))
        //    {
        //        if (this.InnerCirkle.Intersect(otherBound.InnerCirkle)) return true;

        //        if (otherBound.Type == BoundShape.Cylinder)
        //            return PhysicsLib3D.CollRectAndCyliner(this, otherBound);
        //        else
        //            return PhysicsLib3D.CollBoxes(otherBound, this);
        //    }
        //    return false;
        //}

        /// <returns>null is no collision</returns>
        override public BoundCollisionResult Intersect2(AbsBound otherBound)
        {
            if (this.outerBound.Intersect(otherBound.outerBound))
            {
                if (otherBound.Type == BoundShape.Cylinder)
                    return CollRectAndCyliner2(this, otherBound, this, otherBound);
                else
                {
                    if (InnerSphereCollision(this, otherBound))//this.InnerCirkle.Intersect(otherBound.InnerCirkle))
                        return new BoundCollisionResult(this, otherBound, IntersectLength2D(this, otherBound));
                    return CollBoxes2(this, otherBound);
                }
            }
            return null;
        }


        override public BoundShape Type { get { return BoundShape.Box1axisRotation; } }
        //public override bool Equals(object obj)
        //{
        //    if (obj is Box1axisBound)
        //    {
        //        Box1axisBound other = (Box1axisBound)obj;
        //        return other.volume.center == volume.center && other.volume.HalfSize == volume.HalfSize;
        //    }
        //    return false;
        //}
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        public override bool UsesRotation { get { return true; } }
    }
}
