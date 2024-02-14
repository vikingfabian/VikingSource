using VikingEngine.EngineSpace.Maths;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class TargetMove2D : AbsMotion
    {
        AbsDraw2D image;

        Vector2 startPos;
        Vector2 targetPos;

        float timeMS;

        public TargetMove2D(AbsDraw2D objImage, Vector2 target, float timeMS)
            : base(MotionType.MOVE, Vector3.Zero, MotionRepeate.NO_REPEAT, timeMS, true)
        {
            image = objImage;

            startPos = image.Position;
            targetPos = target;

            this.timeMS = timeMS;
        }

        protected override void resetOriginalPosition()
        {
            image.Position = VectorExt.V3XYtoV2(originalPose);
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
            float t = MathHelper.Clamp(1 - (timeLeft / timeMS), 0, 1);
            image.Position = MathExt.Lerp(startPos, targetPos, t);
            return false;
        }
    }
}
