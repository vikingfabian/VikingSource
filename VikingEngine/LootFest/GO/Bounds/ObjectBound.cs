using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.GO.Bounds
{
    class ObjectBound : IDeleteable
    {

        public AbsBound[] Bounds;

        public bool createdFromBoundManager = false;
        public bool use3DRotationOffset = false;
        protected Graphics.Mesh[] boundImages; //for viewing in debug

        public ObjectBound(AbsBound[] bounds)
        {
            Bounds = bounds;

            if (PlatformSettings.ViewCollisionBounds)
            {
                initBoundImage();
            }
        }
        public ObjectBound(BoundData2 bound)
            : this(new AbsBound[]{ AbsBound.CreateBound(bound) })
        { }

        public ObjectBound(BoundData2[] bounds)
        {
            Bounds = new AbsBound[bounds.Length];

            for (int i = 0; i < bounds.Length; ++i)
            {
                Bounds[i] = AbsBound.CreateBound(bounds[i]);
                Bounds[i].index = i;
            }

            if (PlatformSettings.ViewCollisionBounds)
            {
                initBoundImage();
            }
        }

        public ObjectBound(BoundShape type, Vector3 center, Vector3 halfSize, Vector3 offset)
            : this(new AbsBound[] { AbsBound.CreateBound(type, center, halfSize, offset) })
        { }

        public ObjectBound(BoundShape type, Vector3 halfSize, Vector3 offset)
            : this(new AbsBound[] { AbsBound.CreateBound(type, Vector3.Zero, halfSize, offset) })
        { }

        public ObjectBound(BoundSaveData data, GO.AbsUpdateObj go)
          :  this(new AbsBound[] { AbsBound.CreateBound(data, go) })
        {
        }

        protected void initBoundImage()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                //BoundData2[] bounds = AllBounds;
                boundImages = new Graphics.Mesh[Bounds.Length];
                for (int i = 0; i < boundImages.Length; ++i)
                {
                    boundImages[i] = new Graphics.Mesh(LoadedMesh.cube_repeating, Bounds[i].center, Bounds[i].halfSize * 2,
                        TextureEffectType.Flat, SpriteName.InterfaceBorder, Color.White);
                        //new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.InterfaceBorder),
                        //Bounds[i].halfSize * 2, Vector3.Zero);
                    if (Bounds[i].Type == BoundShape.Cylinder)
                    {
                        boundImages[i].ScaleZ = boundImages[i].ScaleX;
                    }
                }
            }
        }

        public void refreshVisualBounds()
        {
            DeleteMe();
            initBoundImage();
        }

        /// <returns>Null is no collision</returns>
        public BoundCollisionResult Intersect2(ObjectBound otherBound) //, Vector3 pos, Vector3 otherPos)
        {
            //Update bound details
            if (otherBound == null)
            {
                Debug.LogError("Intersect2-Otherbound is null");
                return null;
            }
            foreach (var other in otherBound.Bounds)
                {
                    //BoundData2[] myBounds = Bounds;
                    foreach (var myBound in Bounds)
                    {
                        BoundCollisionResult intersection = myBound.Intersect2(other);
                        if (intersection != null)
                        {
                            return intersection;
                        }
                    }
                }

            //}


            return null;
        }

        public static bool SolidBodyIntersect(GO.AbsUpdateObj myObj, GO.AbsUpdateObj otherObj)
        {
            return SolidBodyIntersect(myObj, myObj.CollisionAndDefaultBound,
                otherObj, otherObj.CollisionAndDefaultBound);
        }
        public static bool SolidBodyIntersect(GO.AbsUpdateObj myObj, ObjectBound myObjBound,
            GO.AbsUpdateObj otherObj, ObjectBound otherObjBound)
        {
            if (otherObjBound != null && myObjBound != null)
            {
                foreach (AbsBound other in otherObjBound.Bounds)
                    {

                        foreach (AbsBound me in myObjBound.Bounds)
                        {
                            BoundCollisionResult intersection = me.Intersect2(other);
                            if (intersection != null)
                            {
                                if (myObj != null) myObj.HandleColl3D(intersection, otherObj);
                                if (otherObj != null) otherObj.HandleColl3D(intersection.OtherObjIntersection(), myObj);
                                return true;
                            }
                        }
                    }
                //}
            }
            else
            {
                Debug.LogWarning("obj has no bounds " + otherObj.ToString());
            }
            return false;
        }

        public Vector3 IntersectionDepthAndDir(BoundCollisionResult intersection)
        {
            Vector3 diff = intersection.OtherBound.center - intersection.MyBound.center;
            return VectorExt.SafeNormalizeV3(diff) * intersection.IntersectionDepth;
        }
        public void DebugBoundColor(Color col)
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                foreach (var img in boundImages)
                {
                    img.Color = col;
                }
            }
        }

        public void setBoundIndexes()
        {
            for (int i = 0; i < Bounds.Length; ++i)
            {
                Bounds[i].index = i;
            }
        }


        public BoundCollisionResult ObsticleIntersect(AbsBound obsticle)
        {
            foreach (AbsBound b in Bounds)
            {
                BoundCollisionResult intersection = b.Intersect2(obsticle);
                if (intersection != null)
                {
                    return intersection;
                }
            }
         
            return null;
        }

        protected void CollectTerrainObsticles(ref TerrainColl result, Vector3 center, Vector3 oldPos, bool Yadj, AbsBound bound)
        {
            if (Yadj)
                oldPos.Y += 0.45f;//justera höjden lite för att ge Ystuts mer fördel
            Vector3 halfSz = bound.outerBound.HalfSize * PublicConstants.Half;
            Vector3 topleft = center - halfSz;

            Map.WorldPosition wp = new Map.WorldPosition(topleft);


            Map.WorldPosition nwp;
            IntVector3 size = new IntVector3(
                VikingEngine.Bound.Min(Convert.ToInt32(bound.outerBound.HalfSize.X), 1),
                VikingEngine.Bound.Min(Convert.ToInt32(bound.outerBound.HalfSize.Y), 1),
                VikingEngine.Bound.Min(Convert.ToInt32(bound.outerBound.HalfSize.Z), 1));


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
                        if (nwp.BlockHasColllision())//LfRef.chunks.Get(nwp) != 0)
                        {
                            result.Collition = true;
                            //prova jämför gamla positionen med blocket
                            Vector3 diff = nwp.PositionV3 - oldPos;
                            float sideDiff = VectorExt.V3XZtoV2(diff).Length();

                            if (Math.Abs(diff.Y) >= Math.Abs(sideDiff))
                            {
                                //Ybounce
                                result.CollDir.Y = -lib.ToLeftRight(diff.Y);
                            }
                            else if (Math.Abs(diff.X) > Math.Abs(diff.Z))
                            {
                                result.CollDir.X = -lib.ToLeftRight(diff.X);
                            }
                            else
                            {
                                result.CollDir.Z = -lib.ToLeftRight(diff.Z);
                            }
                            result.BlockPos = nwp.LocalBlockGrindex;
                        }
                    }
                }
            }
        }

        public void UpdatePosition2(GO.AbsUpdateObj Parent)
        {
            if (use3DRotationOffset)
                this.UpdatePosition2(Parent.RotationQuarterion, Parent.Position);
            else
                this.UpdatePosition2(Parent.Rotation, Parent.Position);
        }

        public void DeleteMe()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                foreach (Graphics.Mesh img in boundImages) img.DeleteMe();
            }
        }
        public bool IsDeleted
        {

            get
            {
                if (PlatformSettings.ViewCollisionBounds)
                    return boundImages == null || boundImages[0].IsDeleted;
                else
                    return true;

            }

        }

        public AbsBound MainBound
        {
            get { return Bounds[0]; }
        }
       
        
        public void UpdatePosition2(Rotation1D rotation, Vector3 position)
        {
            foreach (AbsBound b in Bounds)
            {
                b.updatePosition(position + VectorExt.RotateVector(b.offset, rotation.Radians), rotation.Radians);
            }
            updateVisualBounds();
        }

        public void UpdatePosition2(RotationQuarterion rotation, Vector3 position)
        {
            foreach (AbsBound b in Bounds)
            {
                b.updatePosition(rotation.TranslateAlongAxis(b.offset, position), 0);
                //Bound.Bound.Center = rotation.TranslateAlongAxis(Bound.Offset, position);
            }
            updateVisualBounds();
        }

        public void updateVisualBounds()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                for (int i = 0; i < boundImages.Length; ++i)
                {
                    boundImages[i].Position = Bounds[i].center;
                    boundImages[i].Scale = Bounds[i].halfSize * 2f;
                    if (Bounds[i].UsesRotation)
                    {
                        Map.WorldPosition.Rotation1DToQuaterion(boundImages[i], Bounds[i].rotation);
                    }
                }
            }
        }

        public TerrainColl CollectTerrainObsticles(Vector3 center, Vector3 oldPos, bool Yadj)
        {
            TerrainColl result = TerrainColl.NoCollision;
            foreach (AbsBound b in Bounds)
            {
                CollectTerrainObsticles(ref result, center, oldPos, Yadj, b);
                if (result.Collition)
                    return result;
            }
            return result;
        }

        public override string ToString()
        {
            string result = "Object bound(" + Bounds.Length.ToString() + ") { ";
            foreach (AbsBound b in Bounds)
            {
                result += b.ToString() + ", ";
            }
            return result + " }";
        }

        public static ObjectBound FromSaveData(List<BoundSaveData> data, GO.AbsUpdateObj go)
        {
            if (data == null || data.Count == 0)
                return null;

            ObjectBound result;

            AbsBound[] bounds = new AbsBound[data.Count];
            for (int i = 0; i < data.Count; ++i)
            {
                bounds[i] = AbsBound.CreateBound(data[i], go);
            }
            result = new ObjectBound(bounds);

            result.createdFromBoundManager = true;
            return result;
        }

        public bool SingleBound { get { return Bounds.Length == 1; } }
    }
}
