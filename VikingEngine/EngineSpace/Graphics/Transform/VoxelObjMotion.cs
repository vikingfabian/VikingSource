using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.Graphics
{
    class VoxelObjMotion : AbsMotion
    {
    
        protected AbsVoxelObj mesh;
        Quaternion originalRotation;

        public VoxelObjMotion(MotionType type, AbsVoxelObj objImage,
            Vector3 valueChange, MotionRepeate repeateType,
            float timeMS, bool addToUpdateList)
            : base(type, valueChange, repeateType, timeMS, addToUpdateList)
        {
            mesh = objImage;

            switch (motionType)
            {
                case MotionType.MOVE:
                    originalPose = mesh.position;
                    break;
                case MotionType.PARTICLE:
                    originalPose = mesh.position;
                    break;
                case MotionType.SCALE:
                    originalPose = mesh.scale;
                    break;
                case MotionType.ROTATE:
                    originalRotation = mesh.Rotation.QuadRotation;
                    break;
                case MotionType.ROTATE_AXIS:
                    originalRotation = mesh.Rotation.QuadRotation;
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
            throw new NotImplementedException();
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
                    mesh.position.X += add.X;
                    mesh.position.Y += add.Y;
                    mesh.position.Z += add.Z;
                    break;
                case MotionType.SCALE:
                    mesh.scale += add;
                    break;
                case MotionType.ROTATE:
                    mesh.Rotation.RotateWorld(add); ;
                    break;
                case MotionType.ROTATE_AXIS:
                    mesh.Rotation.RotateAxis(add); ;
                    break;
                case MotionType.OPACITY:
                    mesh.Opacity += add.X;
                    break;
                case MotionType.PARTICLE:
                    mesh.position.X += add.X;
                    mesh.position.Y += add.Y;
                    mesh.position.Z += add.Z;
                    break;

            }
            return false;
        }
    }
}
