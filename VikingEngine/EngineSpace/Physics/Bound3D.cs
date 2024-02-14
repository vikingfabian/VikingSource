//#define VIEW_BOUNDS

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Physics
{
    //this version only have 1d rotation
    interface IBound3D
    {
        /// <summary>
        /// The radius of a square that surronds the whole bound shape, this is to make a quick and CPU cheap coll check
        /// </summary>
        VectorVolume OuterBound { get; }

        //VectorVolume VolumeBound { get; }
        /// <summary>
        /// The radius of a cirkle that fits inside the shape, this is to make a quick and CPU cheap coll check
        /// </summary>
        CylinderVolume InnerCirkle { get; }
        //VectorVolume InnerBoundingBox { get; }
        float Rotation { get; set; }
        /// <summary>
        /// Check if this bound shape collides with another
        /// </summary>
        /// /// <returns>
        /// A boolean flag indicating if the bounds intersect.
        /// </returns>
        bool Intersect(IBound3D otherBound);
        Collision3D Intersect2(IBound3D otherBound);
        bool VerticeIntersect(Vector2[] vertices);
        IntersectDetails2D VerticeIntersect2(Vector2[] vertices);
        Vector2[] Vertices();

        Bound3DType Type { get; }

        VectorVolume CenterScale { get; set; }
        RectangleCentered PlaneCenterScale { get; }
        Vector3 Center { get; set; }
        int BoundID { get; }
    }

    

    struct CylinderBound : IBound3D
    {
        VectorVolume outerBound;
        CylinderVolume volume;

        public CylinderBound(CylinderVolume volume)
        {
            boundID = PhysicsLib3D.NextID();
            this.volume = volume;
            outerBound = VectorVolume.ZeroOne;
            updateOuterBound();
        }
        public CylinderVolume InnerCirkle { get { return volume; } }
        public VectorVolume OuterBound
        {
            get
            {
                return outerBound;
                
            }
        }
        
        public bool Intersect(IBound3D otherBound)
        {
            return Intersect2(otherBound) != null;
        }

        /// <returns>null is no collision</returns>
        public Collision3D Intersect2(IBound3D otherBound)
        {
            if (outerBound.Intersect(otherBound.OuterBound))
            {
                switch (otherBound.Type)
                {
                    case Bound3DType.BoundingBox:
                        return PhysicsLib3D.CollRectAndCyliner2(otherBound, this, this, otherBound);
                    case Bound3DType.Box1axisRotation:
                        return PhysicsLib3D.CollRectAndCyliner2(otherBound, this, this, otherBound);
                    case Bound3DType.Cylinder:
                        if (volume.Intersect(otherBound.InnerCirkle))
                            return new Collision3D(this, otherBound, volume.IntersectLength2D(otherBound.InnerCirkle));
                        return null;

                }
            }
            return null;
        }

        public IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Circle flatCirkle = volume.PlaneCirkle();
            foreach (Vector2 v in vertices)
            {
                if (flatCirkle.IntersectPoint(v)) return flatCirkle.IntersectPointDepth(v);
            }
            return null;
        }

        public bool VerticeIntersect(Vector2[] vertices)
        {
            Circle flatCirkle = volume.PlaneCirkle();
            foreach (Vector2 v in vertices)
            {
                if (flatCirkle.IntersectPoint(v)) return true;
            }
            return false;
        }

        public Vector2[] Vertices(){ throw new NotImplementedException(); }

        void updateOuterBound()
        {
            outerBound = new VectorVolume(volume.Center, new Vector3(volume.Radius, volume.HalfHeight, volume.Radius));
        }

        public Vector3 Center { get { return volume.Center; } set { volume.Center = value; outerBound.Center = value; } }
        public float Rotation { get { return 0; } set { } }
        public VectorVolume CenterScale { 
            get { return new VectorVolume(volume.Center, new Vector3(volume.Radius, volume.HalfHeight, volume.Radius)); }
            set { volume.Center = value.Center; volume.Radius = value.HalfSizeX; volume.HalfHeight = value.HalfSizeY; }
        }

        public RectangleCentered PlaneCenterScale { get { throw new NotImplementedException(); } }

        public Bound3DType Type { get { return Bound3DType.Cylinder; } }

        public override bool Equals(object obj)
        {
            if (obj is CylinderBound)
            {
                CylinderBound other = (CylinderBound)obj;
                return other.volume.Center == volume.Center && other.volume.HalfHeight == volume.HalfHeight && other.volume.Radius == volume.Radius;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        int boundID;
        public int BoundID { get { return boundID; } }
        public override string ToString()
        {
            return Type.ToString() + "(" + boundID.ToString() + ") " + volume.ToString();
        }
    }

    struct StaticBoxBound : IBound3D
    {
        VectorVolume volume;
        public VectorVolume CenterScale { get { return volume; } set{ volume = value;} }
        public RectangleCentered PlaneCenterScale { get { return volume.PlaneRect(); } }
        public StaticBoxBound(VectorVolume _area)
        {
            boundID = PhysicsLib3D.NextID();
            volume = _area;
        }
        public Vector2[] Vertices()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                PhysicsLib2D.DebugViewCollisionVertices(PhysicsLib2D.StaticBoxVertices(volume), volume.Center);
            }
            return PhysicsLib2D.StaticBoxVertices(volume);

        }
        public bool VerticeIntersect(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                if (PhysicsLib2D.Vertice2DInsideVolume(v, volume))
                    return true;

            }
            return false;
        }
        public IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Vector2 diff = Vector2.Zero;
            foreach (Vector2 v in vertices)
            {
                if (PhysicsLib2D.Vertice2DInsideVolume(v, volume))
                    return new IntersectDetails2D(PhysicsLib2D.Vertice2DInsideVolumeDepth(v, volume), v);

            }
            return null;
        }

        public bool Intersect(IBound3D otherBound)
        {
            if (volume.Intersect(otherBound.OuterBound))
            {
                switch (otherBound.Type)
                {
                    case Bound3DType.BoundingBox:
                        return true;
                    case Bound3DType.Cylinder:
                        return PhysicsLib3D.CollRectAndCyliner(this, otherBound);
                    case Bound3DType.Box1axisRotation:
                        return PhysicsLib3D.CollBoxes(otherBound, this);

                }
            }
            return false;
        }


        /// <returns>null is no collision</returns>
        public Collision3D Intersect2(IBound3D otherBound)
        {
            if (volume.Intersect(otherBound.OuterBound))
            {
                switch (otherBound.Type)
                {
                    case Bound3DType.BoundingBox:
                        return new Collision3D(this, otherBound,volume.IntersectDepth(otherBound.OuterBound));
                    case Bound3DType.Cylinder:
                        return PhysicsLib3D.CollRectAndCyliner2(this, otherBound, this, otherBound);
                    case Bound3DType.Box1axisRotation:
                        return PhysicsLib3D.CollBoxes2(otherBound, this);

                }
            }
            return null;
        }

        
        public VectorVolume OuterBound
        {
            get
            {
                return volume;
            }
        }

        public CylinderVolume InnerCirkle { get { return new CylinderVolume(volume.Center, volume.HalfSizeY, volume.HalfSizeX); } }
        public float Rotation { get { return 0; } set { } }
        public Vector3 Center { get { return volume.Center; } set { volume.Center = value; } }
        public Bound3DType Type { get { return Bound3DType.BoundingBox; } }
        public override bool Equals(object obj)
        {
            if (obj is StaticBoxBound)
            {
                StaticBoxBound other = (StaticBoxBound)obj;
                return other.volume.Center == volume.Center && other.volume.HalfSize == volume.HalfSize;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        int boundID;
        public int BoundID { get { return boundID; } }
        public override string ToString()
        {
            return Type.ToString() + "(" + boundID.ToString() + ") " + volume.ToString();
        }
    }

    struct Box1axisBound : IBound3D
    {
        VectorVolume outerBound;
       // VectorVolume innerBound;
        VectorVolume volume;
        public VectorVolume CenterScale { get { return volume; } set { volume = value; } }
        public RectangleCentered PlaneCenterScale { get { return volume.PlaneRect(); } }
        public Rotation1D rotation;

        public Box1axisBound(VectorVolume _area, Rotation1D _rotation)
        {
            boundID = PhysicsLib3D.NextID();
            volume = _area;
            outerBound = volume;
            //innerBound = volume;
            rotation = _rotation;
            updateBoundingBox();
        }
        
        public bool VerticeIntersect(Vector2[] vertices)
        {
            Rotation1D negativRot = new Rotation1D(-rotation.Radians);
            Vector2 pos = Vector2.Zero;
            pos.X = volume.X;
            pos.Y = volume.Z;
            foreach (Vector2 v in vertices)
            {
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(pos, v, negativRot.Radians);
                if (PhysicsLib2D.Vertice2DInsideVolume(adjustedVertice, volume))
                    return true;
            }
            return false;
        }
        public IntersectDetails2D VerticeIntersect2(Vector2[] vertices)
        {
            Rotation1D negativRot = new Rotation1D(-rotation.Radians);
            Vector2 pos = Vector2.Zero;
            pos.X = volume.X;
            pos.Y = volume.Z;
            foreach (Vector2 v in vertices)
            {
                Vector2 adjustedVertice = lib.RotatePointAroundCenter(pos, v, negativRot.Radians);
                if (PhysicsLib2D.Vertice2DInsideVolume(adjustedVertice, volume))
                    return new IntersectDetails2D(PhysicsLib2D.Vertice2DInsideVolumeDepth(adjustedVertice, volume), v);
            }
            return null;
        }
        public Vector2[] Vertices()
        {
            Vector2 center = Vector2.Zero;
            center.X = volume.X;
            center.Y = volume.Z;
            
            Vector2[] vertices = PhysicsLib2D.StaticBoxVertices(volume);
            if (rotation.Radians != 0 && rotation.Radians != MathHelper.Pi)
            { //Rotate the vertices
                for (int i = 0; i < 4; i++)
                {
                    vertices[i] = lib.RotatePointAroundCenter(center, vertices[i], rotation.Radians);
                }
            }
            if (PlatformSettings.ViewCollisionBounds)
            {
                PhysicsLib2D.DebugViewCollisionVertices(vertices, volume.Center);
            }
            return vertices;

        }
        void updateBoundingBox()
        {
            Vector2 diff = Vector2.Zero;
            diff.X = volume.HalfSizeX;
            diff.Y = volume.HalfSizeZ;

            float largestRadius = diff.Length();
            outerBound.HalfSizeX = largestRadius;
            outerBound.HalfSizeZ = largestRadius;
        }

        public VectorVolume OuterBound
        {
            get
            {
                return outerBound;
            }
        }

        public CylinderVolume InnerCirkle { get { return new CylinderVolume(volume.Center, volume.HalfSizeY, volume.HalfSizeX); } }

        public bool Intersect(IBound3D otherBound)
        {
            if (this.outerBound.Intersect(otherBound.OuterBound))
            {
                if (this.InnerCirkle.Intersect(otherBound.InnerCirkle)) return true;

                if (otherBound.Type == Bound3DType.Cylinder)
                    return PhysicsLib3D.CollRectAndCyliner(this, otherBound);
                else
                    return PhysicsLib3D.CollBoxes(otherBound, this);
            }
            return false;
        }

        /// <returns>null is no collision</returns>
        public Collision3D Intersect2(IBound3D otherBound)
        {
            if (this.outerBound.Intersect(otherBound.OuterBound))
            {
                if (otherBound.Type == Bound3DType.Cylinder)
                    return PhysicsLib3D.CollRectAndCyliner2(this, otherBound, this, otherBound);
                else
                {
                    if (this.InnerCirkle.Intersect(otherBound.InnerCirkle))
                        return new Collision3D(this, otherBound, this.InnerCirkle.IntersectLength2D(otherBound.InnerCirkle));
                    return PhysicsLib3D.CollBoxes2(this, otherBound);
                }
            }
            return null;
        }

        public Vector3 Center { get { return volume.Center; } set { volume.Center = value; outerBound.Center = value; } }
        public float Rotation { get { return rotation.Radians; } set { rotation.Radians = value; } }
        public Bound3DType Type { get { return Bound3DType.Box1axisRotation; } }
        public override bool Equals(object obj)
        {
            if (obj is Box1axisBound)
            {
                Box1axisBound other = (Box1axisBound)obj;
                return other.volume.Center == volume.Center && other.volume.HalfSize == volume.HalfSize;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        int boundID;
        public int BoundID { get { return boundID; } }
        public override string ToString()
        {
            return Type.ToString() + "(" + boundID.ToString() + ") " + volume.ToString();
        }
    }

    enum Bound3DType
    {
        Box1axisRotation,
        BoundingBox,
        Cylinder,
    }
}
