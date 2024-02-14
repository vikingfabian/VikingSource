using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class Point3D : Abs3DModel, IRotationCallBack
    {
        /* Properties */
       

        /* Fields */
        //protected Vector3 position;
        //protected Vector3 scale = Vector3.Zero;

        /* Constructors */
        public Point3D(Vector3 pos, Vector3 size, bool addToRender)
            : base(addToRender)
        { 
            base.position = pos;
            base.scale = size;
            Rotation.CallBackObj = this;
        }
        public Point3D()
            : this(Vector3.Zero, Vector3.One, true)
        { }
       
        /* Family methods */
        public override AbsDraw CloneMe()
        {
            Point3D clone = new Point3D();
            copyAllDataFrom(clone);
            return clone;
        }
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        {
            throw new NotImplementedException();
        }
        public override void Draw(int cameraIndex)
        { }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        { }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        { }
        public override void copyAllDataFrom(AbsDraw master)
        {
            Point3D p = (Point3D)master;
            p.Position = Position;
            p.Scale = Scale;
            p.QuatRotation = Rotation.QuadRotation;
        }
        public virtual void RotationCallBack() { changedApperance(); }

		/* Novelty methods */
        public void simulateInRenderList()
        {
            inRenderList = true;
        }

        //public override float GetPositionX
        //{
        //    get { return Position.X; }
        //}
        //public override float GetPositionZ
        //{
        //    get { return Position.Z; }
        //}
        //public override float Opacity
        //{
        //    get { return 0; }
        //    set { }
        //}
        public override DrawObjType DrawType { get { return DrawObjType.Mesh; } }

        public virtual Vector3 Position
        {
            get { return base.position; }
            set { base.position = value; }
        }
        public virtual float X
        {
            get { return base.position.X; }
            set { base.position.X = value; }
        }
        public virtual float Y
        {
            get { return base.position.Y; }
            set { base.position.Y = value; }
        }
        public virtual float Z
        {
            get { return base.position.Z; }
            set { base.position.Z = value; }
        }

        public virtual Vector2 XZ
        {
            get { return new Vector2(base.position.X, base.position.Z); }
            set { base.position.X = value.X; base.position.Z = value.Y; }
        }

        public virtual Quaternion QuatRotation
        {
            get { return Rotation.QuadRotation; }
            set { Rotation.QuadRotation = value; }
        }
        public virtual void RotateWorld(Vector3 rot) //Will rotate the object around the world coordinates
        {
            Rotation.RotateWorld(rot);
        }
        public virtual void RotateAxis(Vector3 rot) //Will rotate the object around its own coordinates
        {
            Rotation.RotateAxis(rot);
        }
        public virtual Vector3 Scale
        {
            get { return base.scale; }
            set { base.scale = value; }
        }

        public virtual float ScaleX
        {
            get { return base.scale.X; }
            set { base.scale.X = value; }
        }
        public virtual float ScaleY
        {
            get { return base.scale.Y; }
            set { base.scale.Y = value; }
        }
        public virtual float ScaleZ
        {
            get { return base.scale.Z; }
            set { base.scale.Z = value; }
        }
        public virtual Vector2 ScaleXZ
        {
            get { return new Vector2(base.scale.X, base.scale.Z); }
            set { base.scale.X = value.X; base.scale.Z = value.Y; }
        }
        //public virtual ObjType Type { get { return ObjType.NotSet; } }

        protected override bool drawable { get { return false; } }
    }
}