//#define VIEW_BOUNDS
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.LF2
{
    abstract class AbsObjBound : IDeleteable
    {

        protected Graphics.Mesh[] boundImages; //for viewing in debug


        //public AbsObjBound()
        //{

        //}

        protected void initBoundImage()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                BoundData2[] bounds = AllBounds;
                boundImages = new Graphics.Mesh[bounds.Length];
                for (int i = 0; i < boundImages.Length; ++i)
                {
                    boundImages[i] = new Graphics.Mesh(LoadedMesh.cube_repeating, bounds[i].Bound.Center, new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.InterfaceBorder),
                        bounds[i].Bound.CenterScale.HalfSize * 2, Vector3.Zero);
                }
            }
        }

        abstract public BoundData2 MainBound
        {
            get;
        }
        abstract public BoundData2[] AllBounds
        {
            get;
        }

        public static BoundData2 QuickRectangleRotatedData(Vector3 Size)
        {
            return new BoundData2(new Physics.Box1axisBound(
                    new VectorVolume(Vector3.Zero, Size), Rotation1D.D0), Vector3.Zero);
        }
        public static BoundData2 QuickBoundingBoxData(float Size)
        {
            return QuickBoundingBoxData(lib.V3(Size));
        }
        public static BoundData2 QuickBoundingBoxData(Vector3 Size)
        {
            return new BoundData2(new Physics.StaticBoxBound(
                    new VectorVolume(Vector3.Zero, Size)), Vector3.Zero);
        }

        /// <returns>Null is no collision</returns>
        public ObjBoundCollData Intersect2(AbsObjBound otherBound) //, Vector3 pos, Vector3 otherPos)
        {
            //Update bound details
            if (otherBound == null)
            {
                Debug.LogError( "Intersect2-Otherbound is null");
                return null;
            }
            //if (PlatformSettings.ViewErrorWarnings && MainBound.Bound.Center == Vector3.Zero)
            //{
            //    Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Object bound not fully initialized");
            //}
            //UpdatePosition2(rotation, pos);
            //otherBound.UpdatePosition2(Rotation1D.D0, otherPos);

            if (this.SingleBound && otherBound.SingleBound)
            {
                Collision3D intersection = otherBound.MainBound.Bound.Intersect2(this.MainBound.Bound);
                if (intersection != null)
                {
                    return new ObjBoundCollData(intersection);
                }
            }
            else
            {
                BoundData2[] OtherBoundsData = otherBound.AllBounds;
                foreach (BoundData2 other in OtherBoundsData)
                {
                    BoundData2[] myBounds = AllBounds;
                    foreach (BoundData2 me in myBounds)
                    {
                        Collision3D intersection = other.Bound.Intersect2(me.Bound);
                        if (intersection != null)
                        {
                            return new ObjBoundCollData(intersection);
                        }
                    }
                }
               
            }

            
            return null;
        }

        public static bool SolidBodyIntersect(GameObjects.AbsUpdateObj myObj, GameObjects.AbsUpdateObj otherObj)
        {
            return SolidBodyIntersect(myObj, myObj.CollisionBound,
                otherObj, otherObj.CollisionBound);
        }
        public static bool SolidBodyIntersect(GameObjects.AbsUpdateObj myObj, AbsObjBound myObjBound, 
            GameObjects.AbsUpdateObj otherObj, AbsObjBound otherObjBound)
        {
            if (otherObjBound != null && myObjBound != null)
            {
                if (myObjBound.SingleBound && otherObjBound.SingleBound)
                {
                    Physics.Collision3D intersection = myObjBound.MainBound.Bound.Intersect2(otherObjBound.MainBound.Bound);
                    if (intersection != null)
                    {
                        Physics.IBound3D myBound = myObjBound.MainBound.Bound;
                        Physics.IBound3D otherBound = otherObjBound.MainBound.Bound;

                        if (PlatformSettings.DebugWindow)
                        {
                            //Physics.Bound3dIntersect intersectiontest = myObjBound.MainBound.Bound.Intersect2(otherObjBound.MainBound.Bound);
                            if (!intersection.MyBound.Equals(myBound))
                            {
                                Debug.LogError( "SolidBodyIntersect bound inv");
                                Physics.Collision3D intersectiontest = myObjBound.MainBound.Bound.Intersect2(otherObjBound.MainBound.Bound);
                            }
                        }

                        if (myObj != null) myObj.HandleColl3D(intersection, otherObj);
                        if (otherObj != null) otherObj.HandleColl3D(intersection.OtherObjIntersection(), myObj);

                        return true;
                    }
                }
                else
                {
                    BoundData2[] OtherBoundsData = otherObjBound.AllBounds;
                    BoundData2[] myBounds = myObjBound.AllBounds;
                    foreach (BoundData2 other in OtherBoundsData)
                    {

                        foreach (BoundData2 me in myBounds)
                        {
                            Physics.Collision3D intersection = me.Bound.Intersect2(other.Bound);
                            if (intersection != null)
                            {
                                if (myObj != null) myObj.HandleColl3D(intersection, otherObj);
                                if (otherObj != null) otherObj.HandleColl3D(intersection.OtherObjIntersection(), myObj);
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "obj has no bounds " + otherObj.ToString());
            }
            return false;
        }

        public Vector3 IntersectionDepthAndDir(Collision3D intersection)
        {
            //if (PlatformSettings.DebugWindow)
            //{
            //    if (intersection.OtherBound.Center == MainBound.Bound.Center)
            //    {
            //        Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "IntersectionDepthAndDir inverted bound");
            //    }
            //}
            Vector3 diff = intersection.OtherBound.Center - intersection.MyBound.Center;
            return lib.SafeNormalizeV3(diff) * intersection.IntersectionDepth;
        }
        public void DebugBoundColor(Color col)
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImages[0].Color = col;
            }    
        }
       


        public Physics.Collision3D ObsticleIntersect(Physics.IBound3D obsticle)
        {
            if (this.SingleBound)
            {
                return MainBound.Bound.Intersect2(obsticle);
            }
            else
            {
                BoundData2[] myBounds = AllBounds;
                foreach (BoundData2 me in AllBounds)
                {
                    Physics.Collision3D intersection = me.Bound.Intersect2(obsticle);
                    if (intersection != null)
                    {
                        return intersection;
                    }
                }
            }
            return null;
        }


        abstract public TerrainColl CollectTerrainObsticles(Vector3 center, Vector3 oldPos, bool Yadj);
        protected void CollectTerrainObsticles(ref TerrainColl result, Vector3 center, Vector3 oldPos, bool Yadj, BoundData2 bound)
        {
            if (Yadj)
                oldPos.Y += 0.45f;//justera höjden lite för att ge Ystuts mer fördel
            Vector3 halfSz = bound.Bound.OuterBound.HalfSize * PublicConstants.Half;
            Vector3 topleft = center - halfSz;

            Map.WorldPosition wp = new Map.WorldPosition(topleft);


            Map.WorldPosition nwp;
            IntVector3 size = new IntVector3(
                Bound.Min(Convert.ToInt32(bound.Bound.OuterBound.HalfSize.X), 1),
                Bound.Min(Convert.ToInt32(bound.Bound.OuterBound.HalfSize.Y), 1),
                Bound.Min(Convert.ToInt32(bound.Bound.OuterBound.HalfSize.Z), 1));


            if (size == IntVector3.Zero)
            {
                size = IntVector3.One;
            }

            IntVector3 pos = IntVector3.Zero;
            for (pos.Y = 0; pos.Y < size.Y; pos.Y++)
            {
                for (pos.Z = 0; pos.Z < size.Z; pos.Z++)
                {
                    for (pos.X = 0; pos.X < size.X; pos.X++)
                    {
                        nwp = wp.GetNeighborPos(pos);
                        if (LfRef.chunks.Get(nwp) != 0)
                        {
                            result.Collition = true;
                            //prova jämför gamla positionen med blocket
                            Vector3 diff = nwp.ToV3() - oldPos;
                            float sideDiff = Map.WorldPosition.V3toV2(diff).Length();

                            if (Math.Abs(diff.Y) >= Math.Abs(sideDiff))
                            {
                                //Ybounce
                                result.CollDir.Y = -lib.FloatToDir(diff.Y);
                            }
                            else if (Math.Abs(diff.X) > Math.Abs(diff.Z))
                            {
                                result.CollDir.X = -lib.FloatToDir(diff.X);
                            }
                            else
                            {
                                result.CollDir.Z = -lib.FloatToDir(diff.Z);
                            }
                            result.BlockPos = nwp.LocalBlockGrindex;
                        }
                    }
                }
            }
        }

        public void UpdatePosition2(GameObjects.AbsUpdateObj Parent)
        {
            this.UpdatePosition2(Parent.Rotation, Parent.Position);
        }
        abstract public void UpdatePosition2(Rotation1D rotation, Vector3 position);

        public void DeleteMe()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                foreach (Graphics.Mesh img in boundImages) img.DeleteMe();
            }
        }
        public bool IsDeleted
        {
            
            get { 
                    if (PlatformSettings.ViewCollisionBounds)
                        return boundImages == null || boundImages[0].IsDeleted; 
                    else
                        return true;

                }

        }
        abstract public bool SingleBound { get; }
    }

    class ObjSingleBound : AbsObjBound
    {
        BoundData2 Bound;

        public ObjSingleBound(BoundData2 bound)
        {
            this.Bound = bound;
            if (PlatformSettings.ViewCollisionBounds)
            {
                initBoundImage();
            }
        }

        override public BoundData2 MainBound
        {
            get { return Bound; }
        }
        override public BoundData2[] AllBounds
        {
            get { return new BoundData2[] { Bound }; }
        }

        public static ObjSingleBound QuickRectangleRotated2(Vector3 Size)
        {
            return new ObjSingleBound(new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, Size), Rotation1D.D0), new Vector3(0, Size.Y, 0)));
        }
        public static ObjSingleBound QuickRectangleRotated(Vector3 Size, float offsetY)
        {
            return new ObjSingleBound(new BoundData2(new Box1axisBound(new VectorVolume(Vector3.Zero, Size), Rotation1D.D0), new Vector3(0, offsetY, 0)));
        }
        public static ObjSingleBound QuickRectangleRotated(Vector3 pos, Vector3 Size, Vector3 offset)
        {
            return new ObjSingleBound(new BoundData2(new Box1axisBound(new VectorVolume(pos, Size), Rotation1D.D0), offset));
        }
        public static ObjSingleBound QuickRectangleRotated(Vector3 Size)
        {
            return new ObjSingleBound(new BoundData2(new Physics.Box1axisBound(
                    new VectorVolume(Vector3.Zero, Size), Rotation1D.D0), Vector3.Zero));
        }

        public static ObjSingleBound QuickRectangleRotatedFromFeet(Vector3 halfSize, float yAdj)
        {
            Vector3 offSet = Vector3.Zero;
            offSet.Y = halfSize.Y + yAdj;

            return new ObjSingleBound(new BoundData2(new Physics.Box1axisBound(
                   new VectorVolume(Vector3.Zero, halfSize), Rotation1D.D0), offSet));
        }
        public static ObjSingleBound QuickRectangleRotatedFromFeet(Vector3 halfSize, Vector3 offSet)
        {  
            offSet.Y += halfSize.Y;

            return new ObjSingleBound(new BoundData2(new Physics.Box1axisBound(
                   new VectorVolume(Vector3.Zero, halfSize), Rotation1D.D0), offSet));
        }

        public static ObjSingleBound QuickBoundingBox(float Size)
        {
            return new ObjSingleBound(AbsObjBound.QuickBoundingBoxData(Size));
        }
        public static ObjSingleBound QuickBoundingBox(Vector3 Size)
        {
            return new ObjSingleBound(AbsObjBound.QuickBoundingBoxData(Size));
        }
        public static ObjSingleBound QuickBoundingBox(Vector3 Size, float offsetY)
        {
            return new ObjSingleBound(new BoundData2(new StaticBoxBound(new VectorVolume(Vector3.Zero, Size)), new Vector3(0, offsetY, 0)));
        }

        public static ObjSingleBound QuickBoundingBoxFromFeetPos(Vector3 halfSize)
        {
            return QuickBoundingBoxFromFeetPos(halfSize, 0);
        }
        public static ObjSingleBound QuickBoundingBoxFromFeetPos(Vector3 halfSize, float offsetY)
        {
            return new ObjSingleBound(new BoundData2(new StaticBoxBound(new VectorVolume(Vector3.Zero, halfSize)), new Vector3(0, offsetY + halfSize.Y, 0)));
        }

        public static ObjSingleBound QuickCylinderBound(Vector3 position, float radius, float halfHeight)
        {
            return new ObjSingleBound(new BoundData2(new CylinderBound(new CylinderVolume(position, halfHeight, radius)), new Vector3(0, halfHeight, 0)));
        }

        public static ObjSingleBound QuickCylinderBound(float radius, float halfHeight)
        {
            return new ObjSingleBound(new BoundData2( new CylinderBound(new CylinderVolume(Vector3.Zero, halfHeight, radius)), new Vector3(0, halfHeight, 0)));
        }
        public static ObjSingleBound QuickCylinderBound(float radius, float halfHeight, float offsetY)
        {
            return new ObjSingleBound(new BoundData2(new CylinderBound(new CylinderVolume(Vector3.Zero, halfHeight, radius)), new Vector3(0, offsetY, 0)));
        }
        public static ObjSingleBound QuickCylinderBoundFromFeetPos(float radius, float halfHeight, float offsetY)
        {
            return new ObjSingleBound(new BoundData2(new CylinderBound(new CylinderVolume(Vector3.Zero, halfHeight, radius)), new Vector3(0, offsetY + halfHeight, 0)));
        }
        public static ObjSingleBound QuickCylinderBound(float radius, float halfHeight, Vector3 offset)
        {
            return new ObjSingleBound(new BoundData2(new CylinderBound(new CylinderVolume(Vector3.Zero, halfHeight, radius)), offset));
        }
        override public void UpdatePosition2(Rotation1D rotation, Vector3 position)
        {
            Bound.Bound.Rotation = rotation.Radians;
            Bound.Bound.Center = position + lib.RotateVector(Bound.Offset, rotation.Radians);
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImages[0].Position = Bound.Bound.Center;
                Map.WorldPosition.Rotation1DToQuaterion(boundImages[0], Bound.Bound.Rotation);
            }
        }

        override public TerrainColl CollectTerrainObsticles(Vector3 center, Vector3 oldPos, bool Yadj)
        {
            TerrainColl result = TerrainColl.NoCollision;
            CollectTerrainObsticles(ref result, center, oldPos, Yadj, Bound);
            return result;
        }

        override public bool SingleBound { get { return true; } }

        public override string ToString()
        {
            return "Single bound(" + Bound.Bound.BoundID.ToString() + ") { " + Bound.ToString() + "}";
        }
    }

    class ObjMultiBound : AbsObjBound
    {
        public BoundData2[] Bounds;

        
        public ObjMultiBound(BoundData2[] bounds)
        {
            Bounds = bounds;

            if (PlatformSettings.ViewCollisionBounds)
            {
                initBoundImage();
            }
        }
        override public BoundData2 MainBound
        {
            get { return Bounds[0]; }
        }
        override public BoundData2[] AllBounds
        {
            get { return Bounds; }
        }
        
        override public void UpdatePosition2(Rotation1D rotation, Vector3 position)
        {
            foreach (BoundData2 b in Bounds)
            {
                b.Bound.Rotation = rotation.Radians;
                b.Bound.Center = position + lib.RotateVector(b.Offset, rotation.Radians); // position + b.Offset;
            }
            if (PlatformSettings.ViewCollisionBounds)
            {
                for (int i = 0; i < boundImages.Length; ++i)
                {
                    boundImages[i].Position = Bounds[i].Bound.Center;
                    Map.WorldPosition.Rotation1DToQuaterion(boundImages[i], Bounds[i].Bound.Rotation);

                }
            }
        }
        override public TerrainColl CollectTerrainObsticles(Vector3 center, Vector3 oldPos, bool Yadj)
        {
            TerrainColl result = TerrainColl.NoCollision;
            foreach (BoundData2 b in Bounds)
            {
                CollectTerrainObsticles(ref result, center, oldPos, Yadj, b);
                if (result.Collition)
                    return result;
            }
            return result;
        }

        override public bool SingleBound { get { return false; } }
        
        public override string ToString()
        {
            string result = "Muti bound(" + Bounds.Length.ToString() + ") { ";
            foreach (BoundData2 b in Bounds)
            {
                result += b.ToString() + ", ";
            }
            return result + " }";
        }
    }

    struct BoundData2
    {
        public Physics.IBound3D Bound;
        public Vector3 Offset;

        public BoundData2(Physics.IBound3D bound, Vector3 offset)
        {
            Bound = bound;
            Offset = offset;
        }

        public override string ToString()
        {
            return Bound.Type.ToString();
        }
    }
    struct TerrainColl
    {
        public static readonly TerrainColl NoCollision = new TerrainColl(); 
        public bool Collition;
        public Vector3 CollDir;
        public IntVector3 BlockPos;
    }
    
}
