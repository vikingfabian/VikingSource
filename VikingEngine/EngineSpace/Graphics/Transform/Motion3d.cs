using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;


namespace VikingEngine.Graphics
{
    class Motion3d : AbsMotion
    {
        protected Point3D mesh;
        Quaternion originalRotation;

        public Motion3d(MotionType type, Point3D objImage,
            Vector3 valueChange, MotionRepeate repeateType,
            float timeMS, bool addToUpdateList)
            : base(type, valueChange, repeateType, timeMS, addToUpdateList)
        {
            mesh = objImage;

            switch (motionType)
            {
                case MotionType.MOVE:
                    originalPose = mesh.Position;
                    break;
                case MotionType.PARTICLE:
                    originalPose = mesh.Position;
                    break;
                case MotionType.SCALE:
                    originalPose = mesh.Scale;
                    break;
                case MotionType.ROTATE:
                    originalRotation = mesh.QuatRotation;
                    break;
                case MotionType.ROTATE_AXIS:
                    originalRotation = mesh.QuatRotation;
                    break;

                case MotionType.OPACITY:
                    originalPose.X = mesh.Opacity;
                    break;
                case MotionType.ACCELERATE:
                    originalPose = valueChange;
                    break;

            }
               
        }
        protected override void resetOriginalPosition()
        {
            
            switch (motionType)
            {
                case MotionType.MOVE:
                    mesh.Position = originalPose;
                    break;
                case MotionType.SCALE:
                    mesh.Scale = originalPose;
                    break;
                case MotionType.OPACITY:
                    mesh.Opacity = originalPose.X;
                    break;
                default:
                    throw new NotImplementedException("resetOriginalPosition");
            }
        }
        public override bool In3d
        {
            get { return true; }
        }
        protected override bool inRenderList
        {
            get { return mesh.InRenderList; }
        }
        public override AbsMotion CloneMe(AbsDraw newTargetObj, bool addToUpdate)
        {
            Vector3 valueChange = Stepping;
            valueChange.X *= timeLeftSave;
            valueChange.Y *= timeLeftSave;
            valueChange.Z *= timeLeftSave;

            return new Motion3d(motionType, (Mesh)newTargetObj, valueChange, repeate,
                timeLeftSave, addToUpdate);
        }

        protected override bool UpdateMotion(float time)
        {
            Vector3 add = Vector3.Zero;
            add.X = Stepping.X * time;
            add.Y = Stepping.Y * time;
            add.Z = Stepping.Z * time;
            
            switch (motionType)
            {
                case MotionType.MOVE:
                    mesh.Position += add;
                    //mesh.X += add.X;
                    //mesh.Y += add.Y;
                    //mesh.Z += add.Z;
                    break;
                case MotionType.SCALE:
                    mesh.Scale += add;
                    break;
                case MotionType.ROTATE:
                    mesh.RotateWorld(add); ;
                    break;
                case MotionType.ROTATE_AXIS:
                    mesh.RotateAxis(add); ;
                    break;
                case MotionType.OPACITY:
                    mesh.Opacity += add.X;
                    break;
                case MotionType.PARTICLE:
                    mesh.X += add.X;
                    mesh.Y += add.Y;
                    mesh.Z += add.Z;
                    break;

            }
            return false;
        }
    }
}
