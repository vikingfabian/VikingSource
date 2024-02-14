using VikingEngine.EngineSpace.Maths;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class TargetFade : AbsMotion
    {
        AbsDraw image;

        float startOpacity;
        float targetOpacity;

        float timeMS;

        public TargetFade(AbsDraw objImage, float targetOpacity, float timeMS)
            : base(MotionType.MOVE, Vector3.Zero, MotionRepeate.NO_REPEAT, timeMS, true)
        {
            image = objImage;

            startOpacity = image.Opacity;
            this.targetOpacity = targetOpacity;

            this.timeMS = timeMS;
        }

        protected override void resetOriginalPosition()
        {
            //image.Position = VectorExt.V3XYtoV2(originalPose);
        }

        public override bool In3d
        {
            get { return image.DrawType != DrawObjType.Texture2D; }
        }
        protected override bool inRenderList
        {
            get { return image.InRenderList; }
        }
        public override AbsMotion CloneMe(AbsDraw newTargetObj, bool addToUpdate)
        {
            throw new NotImplementedException();
            //Vector2 valueChange = VectorExt.V3XYtoV2(Stepping);
            //valueChange.X *= timeLeftSave;
            //valueChange.Y *= timeLeftSave;

            //return new Motion2d(motionType, (AbsDraw2D)newTargetObj, valueChange, repeate,
            //    timeLeftSave, addToUpdate);
        }
        protected override bool UpdateMotion(float time)
        {
            float t = MathHelper.Clamp(1 - (timeLeft / timeMS), 0, 1);
            image.Opacity = MathExt.Lerp(startOpacity, targetOpacity, t);
            return false;
        }
    }
}
