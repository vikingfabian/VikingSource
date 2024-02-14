using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class ChangeColor : AbsMotion
    {
        AbsDraw2D image;
        Vector3 color;

        public ChangeColor(AbsDraw2D objImage, Color toColor, float timeMS, bool addToUpdateList)
            : base(MotionType.COLOR, toColor.ToVector3() - objImage.Color.ToVector3(),
            MotionRepeate.NO_REPEAT, timeMS, addToUpdateList)
        {
            image = objImage;
            color = objImage.Color.ToVector3();
        }
        public override bool In3d
        {
            get { return false; }
        }
        protected override bool inRenderList
        {
            get { return image.InRenderList; }
        }
        protected override bool UpdateMotion(float time)
        {
            color.X += Stepping.X * time;
            color.Y += Stepping.Y * time;
            color.Z += Stepping.Z * time;
            image.Color = new Color(color);
            
            return false;
        }
        public override AbsMotion CloneMe(AbsDraw newTargetObj, bool addToUpdate)
        {
            throw new NotImplementedException();
        }
        protected override void resetOriginalPosition()
        {
            throw new NotImplementedException();
        }
    }
}
