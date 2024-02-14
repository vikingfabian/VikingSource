using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Graphics
{
    class Motion2d : AbsMotion
    {
        AbsDraw2D image;

        public Motion2d(MotionType type, AbsDraw2D objImage,
            Vector2 valueChange, MotionRepeate repeateType,
            float timeMS, bool addToUpdateList)
            : base(type, VectorExt.V2toV3XY(valueChange), repeateType, timeMS, addToUpdateList)
        {
            image = objImage;

            switch (motionType)
            {
                case MotionType.MOVE:
                    originalPose = VectorExt.V2toV3XY(image.Position);
                    break;
                case MotionType.SCALE:
                    originalPose = VectorExt.V2toV3XY(image.Size);
                    break;
                case MotionType.ROTATE:
                    originalPose.X = image.Rotation;
                    break;
                case MotionType.OPACITY:
                    originalPose.X = image.Opacity;
                    break;
            }
               
        }

        protected override void resetOriginalPosition()
        {
            switch (motionType)
            {
                case MotionType.MOVE:
                    image.Position = VectorExt.V3XYtoV2(originalPose);
                    break;
                case MotionType.SCALE:
                    image.Size = VectorExt.V3XYtoV2(originalPose);
                    break;
                case MotionType.ROTATE:
                    image.Rotation= originalPose.X;
                    break;
                case MotionType.OPACITY:
                    image.Opacity = originalPose.X;
                    break;
            }
        }

        public override bool In3d
        {
            get { return false; }
        }
        protected override bool inRenderList
        {
            get { return image.InRenderList; }
        }
        public override AbsMotion CloneMe(AbsDraw newTargetObj, bool addToUpdate)
        {
            Vector2 valueChange = VectorExt.V3XYtoV2(Stepping);
            valueChange.X *= timeLeftSave;
            valueChange.Y *= timeLeftSave;

            return new Motion2d(motionType, (AbsDraw2D)newTargetObj, valueChange, repeate,
                timeLeftSave, addToUpdate);
        }
        protected override bool UpdateMotion(float time)
        {
            Vector2 add = Vector2.Zero;
            add.X = Stepping.X * time;
            add.Y = Stepping.Y * time;
            
            switch (motionType)
            {
                case MotionType.MOVE:
                    image.Position += add;
                    break;
                case MotionType.SCALE:
                    image.Size += add;
                    break;
                case MotionType.ROTATE:
                    image.Rotation += add.X;
                    break;
                case MotionType.OPACITY:
                    image.Opacity += add.X;
                    break;
            }
            return false;
        }
    }
}
