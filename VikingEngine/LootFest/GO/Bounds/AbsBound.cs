using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Bounds
{
    abstract class AbsBound
    {
        public Vector3 center;
        public Vector3 halfSize;
        public Vector3 offset;
        public bool ignoresDamage = false;
        /// <summary>
        /// The radius of a square that surronds the whole bound shape, this is to make a quick and CPU cheap coll check
        /// </summary>
        public VectorVolumeC outerBound; //Gör om så det bara finns outer bound scale V3
        public float inneSphereRadius;

        public float rotation;
        protected Vector2[] vertices = null;
        public int index;


        public AbsBound(Vector3 center, Vector3 scale, Vector3 offset)
        {
            this.center = center;
            this.halfSize = scale;
            this.offset = offset;
            this.outerBound.Center = center;
            refreshBound();
        }

        public static AbsBound CreateBound(BoundData2 bound)
        {
            return CreateBound((BoundShape)bound.Bound.Type, bound.Bound.Center, bound.Bound.CenterScale.HalfSize, bound.Offset);
        }

        public static AbsBound CreateBound(BoundSaveData saveData, GO.AbsUpdateObj go)
        { 
            return CreateBound(saveData.type, Vector3.Zero, saveData.scale * go.Scale1D, saveData.offset * go.Scale1D);
        }

        public static AbsBound CreateBound(BoundShape type, Vector3 center, Vector3 halfsize, Vector3 offset)
        {
            switch (type)
            {
                case BoundShape.BoundingBox:
                    return new StaticBoxBound(center, halfsize, offset);
                    //Bound = new Physics.StaticBoxBound(new VectorVolume(center, halfsize));
                    //break;
                case BoundShape.Box1axisRotation:
                    return new Box1axisBound(center, halfsize, offset);
                    //Bound = new Physics.Box1axisBound(new VectorVolume(center, halfsize), Rotation1D.D0);
                    //break;
                case BoundShape.Cylinder:
                    return new CylinderBound(center, halfsize, offset);
                    //Bound = new Physics.CylinderBound(new CylinderVolume(center, halfsize.Y, halfsize.X));
                    //break;
                default:
                    throw new NotImplementedException("Bound data for " + type.ToString());                    
            }

           // this.Offset = offset;
        }


        public bool Intersect(AbsBound otherBound)
        {
            return Intersect2(otherBound) != null;
        }

        /// <summary>
        /// Check if this bound shape collides with another
        /// </summary>
        abstract public BoundCollisionResult Intersect2(AbsBound otherBound);

        abstract public bool VerticeIntersect(Vector2[] vertices);
        abstract public VikingEngine.Physics.IntersectDetails2D VerticeIntersect2(Vector2[] vertices);
        abstract public Vector2[] Vertices();

        

        abstract public BoundShape Type { get; }

        public VectorVolumeC CenterScale {
            get
            {
                return new VectorVolumeC(center, halfSize);
            }
            set
            {
                center = value.Center;
                halfSize = value.HalfSize;
            }
        }
        public RectangleCentered PlaneCenterScale { get { return new RectangleCentered(VectorExt.V3XZtoV2(center), VectorExt.V3XZtoV2(halfSize)); } }

        static int nextId;
        public int BoundID = nextId++;


        

        virtual public void updatePosition(Vector3 pos, float rotation)
        {
            this.rotation = rotation;
            center = pos;
            outerBound.Center = pos;
        }
        virtual public void refreshBound() 
        { }

        //public static bool CollBoxes(AbsBound obj1, AbsBound obj2)
        //{
        //    if (obj1.VerticeIntersect(obj2.Vertices()))
        //        return true;
        //    if (obj2.VerticeIntersect(obj1.Vertices()))
        //        return true;
        //    return false;
        //}
        public static BoundCollisionResult CollBoxes2(AbsBound myBound, AbsBound otherBound)
        {
            VikingEngine.Physics.IntersectDetails2D depth = myBound.VerticeIntersect2(otherBound.Vertices());
            if (depth != null)
                return new BoundCollisionResult(myBound, otherBound, depth.Depth, depth.IntersectionCenter);
            depth = otherBound.VerticeIntersect2(myBound.Vertices());
            if (depth != null)
                return new BoundCollisionResult(myBound, otherBound, depth);
            return null;
        }
        public static BoundCollisionResult CollRectAndCyliner2(AbsBound rect, AbsBound cylinder, AbsBound myBound, AbsBound otherBound)
        {
            if (InnerSphereCollision(rect, cylinder))//rect.InnerCirkle.Intersect(cylinder.InnerCirkle))
                return new BoundCollisionResult(myBound, otherBound, IntersectLength2D(rect, cylinder));

            //Treat the cirkle as a point and check is it is inside the box
            Circle flatCirkle = cylinder.flatCirkle();//new Circle(VectorExt.V3XZtoV2(cylinder.center), cylinder.scale.X);//cylinder.InnerCirkle.PlaneCirkle();
            Circle orgFlatCirkle = flatCirkle;
            RectangleCentered flatRect = rect.PlaneCenterScale;
            if (rect.rotation != 0)
            {
                flatCirkle.Center = lib.RotatePointAroundCenter(flatRect.Center, flatCirkle.Center, -rect.rotation);
            }
            if (flatRect.IntersectCirkle(flatCirkle))
            {
                return new BoundCollisionResult(myBound, otherBound, flatRect.IntersectCirkleDepth(flatCirkle, rect.rotation, orgFlatCirkle));
            }
            return null;
        }
        //public static bool CollRectAndCyliner(AbsBound rect, AbsBound cylinder)
        //{
        //    if (InnerSphereCollision(rect, cylinder))//rect.InnerCirkle.Intersect(cylinder.InnerCirkle))
        //        return true;
        //    return cylinder.VerticeIntersect(rect.Vertices());
        //}

        public static bool InnerSphereCollision(AbsBound bound1, AbsBound bound2)
        {
            return (bound1.center - bound2.center).Length() < (bound1.inneSphereRadius + bound2.inneSphereRadius);
        }

        public static VikingEngine.Physics.IntersectDetails2D IntersectLength2D(AbsBound bound1, AbsBound bound2)
        {
            VikingEngine.Physics.IntersectDetails2D result = new VikingEngine.Physics.IntersectDetails2D();
            float combRadius = bound1.inneSphereRadius + bound2.inneSphereRadius;
            Vector2 centerDiff = new Vector2(bound1.center.X - bound2.center.X, bound1.center.Z - bound2.center.Z);
            result.Depth = Math.Abs(centerDiff.Length() - combRadius);
            float collRadius = bound1.inneSphereRadius - result.Depth * PublicConstants.Half;
            centerDiff.Normalize();
            result.IntersectionCenter = new Vector2(bound1.center.X + centerDiff.X * collRadius, bound1.center.Z + centerDiff.Y * collRadius);
            return result;
        }

        public static bool IntersectCylinders(AbsBound cylinder1, AbsBound cylinder2)
        {
            return Math.Abs(cylinder2.center.Y - cylinder1.center.Y) <= cylinder1.halfSize.Y + cylinder2.halfSize.Y &&
                VectorExt.Length(cylinder2.center.X - cylinder1.center.X, cylinder2.center.Z - cylinder1.center.Z) <= cylinder1.halfSize.X + cylinder2.halfSize.X;
        }

        //public static Vector2[] StaticBoxVertices(AbsBound box)
        //{
        //    float left = box.center.X - box.scale.X;
        //    float right = box.center.X + box.scale.X;
        //    float top = box.center.Z - box.scale.Z;
        //    float bottom = box.center.Z + box.scale.Z;

        //    return new Vector2[]
        //        { 
        //            new Vector2(left, top), 
        //            new Vector2(right, top), 
        //            new Vector2(left, bottom), 
        //            new Vector2(right, bottom)
        //        };
        //}
        protected void staticBoxVertices()
        {
            float left = center.X - halfSize.X;
            float right = center.X + halfSize.X;
            float top = center.Z - halfSize.Z;
            float bottom = center.Z + halfSize.Z;

            if (vertices == null)
            {
                vertices = new Vector2[4];
            }

            vertices[0] = new Vector2(left, top);
            vertices[1] = new Vector2(right, top);
            vertices[2] = new Vector2(left, bottom);
            vertices[3] = new Vector2(right, bottom);
        }

        protected Circle flatCirkle()
        {
            return new Circle(VectorExt.V3XZtoV2(center), halfSize.X);
        }

        public override string ToString()
        {
            return Type.ToString() + "(" + BoundID.ToString() + ") center" + center.ToString() + " halfSz" + halfSize.ToString();
        }

        public abstract bool UsesRotation { get; }
    }

    class BoundCollisionResult //null is no collision
    {
        public AbsBound MyBound;
        public AbsBound OtherBound;
        float intersectionDepth;
        public float IntersectionDepth
        {
            get { return intersectionDepth; }
            set { intersectionDepth = Bound.Max(value, 0.8f); }
        }
        public Vector3 IntersectionCenter;

        public BoundCollisionResult(AbsBound MyBound, AbsBound OtherBound, float IntersectionDepth, Vector3 IntersectionCenter)
        {
            this.MyBound = MyBound;
            this.OtherBound = OtherBound;
            intersectionDepth = 0;
            this.IntersectionDepth = IntersectionDepth;
            this.IntersectionCenter = IntersectionCenter;
        }

        public BoundCollisionResult(AbsBound MyBound, AbsBound OtherBound, float IntersectionDepth, Vector2 IntersectionCenter)
            : this(MyBound, OtherBound, IntersectionDepth, Vector3.Zero)
        {
            this.IntersectionCenter.X = IntersectionCenter.X;
            this.IntersectionCenter.Y = (OtherBound.center.Y + MyBound.center.Y) * PublicConstants.Half;
            this.IntersectionCenter.Z = IntersectionCenter.Y;
        }

        public BoundCollisionResult(AbsBound MyBound, AbsBound OtherBound, VikingEngine.Physics.IntersectDetails2D intersectDetails)
            : this(MyBound, OtherBound, intersectDetails.Depth, intersectDetails.IntersectionCenter)
        { }
        public BoundCollisionResult(AbsBound MyBound, AbsBound OtherBound, VikingEngine.Physics.IntersectDetails3D intersectDetails)
            : this(MyBound, OtherBound, intersectDetails.Depth, intersectDetails.IntersectionCenter)
        { }

        public BoundCollisionResult OtherObjIntersection()
        {
            return new BoundCollisionResult(OtherBound, MyBound, IntersectionDepth, IntersectionCenter);
        }

        public override string ToString()
        {
            return "Depth: " + IntersectionDepth.ToString() + ", My bound:" + MyBound.ToString() + ", Other bound:" + OtherBound.ToString();
        }
    }

    

    enum BoundShape
    {
        Box1axisRotation,
        BoundingBox,
        Cylinder,
    }
}
