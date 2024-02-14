using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    abstract class AbsVoxelObj : Abs3DModel
    {
        public LoadedTexture texture = LoadedTexture.NO_TEXTURE;
        /* Static */
        static int CurrentVoxelModelIndex = 0;

        static int NextIndex()
        {
            int result = CurrentVoxelModelIndex++;
            if (result == 52)
            {
                lib.DoNothing();
            }
            return result;
        }

        /* Properties */
        public override DrawObjType DrawType { get { return DrawObjType.MeshGenerated; } }
        //public override float GetPositionX { get { return position.X; } }
        //public override float GetPositionZ { get { return position.Z; } }

        public abstract float SizeToScale { get; }

        /// <summary>
        /// Longest side of the grid
        /// </summary>
        public abstract int GridSideLength { get; }
        //public abstract bool Animated { get; }

        public int Frame = 0; //{ get { return 0; } set { lib.DoNothing(); } }
        public abstract int NumFrames { get; }
        //public virtual AnimationsSettings AnimationsSettings
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        public int ModelIndex { get { return modelIndex; } }
        public float OneBlockScale { get { return scale.X; } }
        
        //public float Scale1D
        //{
        //    get { return scale.Y; }
        //    set
        //    {
        //        scale.X = value;
        //        scale.Z = value;
        //        scale.Y = value;
        //    }
        //}

        public float Size1D
        {
            get { return scale.Y * GridSideLength; }
            set
            {
                scale.X = value * SizeToScale;
                scale.Z = scale.X;
                scale.Y = scale.X;
            }
        }

        /* Fields */
        //public Vector3 position = Vector3.Zero;
        //public Vector3 scale = Vector3.One;

        protected bool fullyInitialized = false;
        protected int modelIndex = NextIndex();

        /* Constructors */
        public AbsVoxelObj(bool add)
            : base(add)
        { }

        /* Methods */
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        {
            if (fullyInitialized)
            {
                //Är trådad och krashar om händer innan full init
                boundingSphere.Center = position;
                boundingSphere.Radius = scale.X / SizeToScale;
            }
        }

        public virtual void SetMaster(VoxelModel master) { throw new NotImplementedException(); }
        public virtual VoxelModel GetMaster() { throw new NotImplementedException(); }
        //public virtual void UpdateAnimation(float speed, float time) { }
        public virtual void NextAnimationFrame() { }

        public Vector3 ChildPosition(Vector3 offset)
        {
            return Rotation.TranslateAlongAxis(offset, position);
        }
    }
}
