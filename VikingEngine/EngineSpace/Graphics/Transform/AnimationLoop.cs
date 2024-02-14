using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class AnimationLoop : AbsUpdateable
    {
        AbsDraw image;
        bool frameOffsetAnimation;
        SpriteName startFrame;
        int currentIx = 0;
        int frameCount;
        Timer.Basic frameTime;
        MotionRepeate repeateType;

        bool bGameTime = true;
        public LeftRight dir = LeftRight.Right;

        public AnimationLoop(AbsDraw image, SpriteName startFrame, 
            int frameCount, float frameTime,
            MotionRepeate repeateType, bool addToUpdate = true)
            : base(addToUpdate)
        {
            this.image = image;
            this.startFrame = startFrame;
            this.frameCount = frameCount;
            this.repeateType = repeateType;
            this.frameTime = new Timer.Basic(frameTime, true); 
            frameOffsetAnimation = true;
        }

        public override void Time_Update(float time_ms)
        {
            if (bGameTime)
            {
                if (frameTime.UpdateInGame())
                {
                    nextFrame();
                }
            }
            else
            {
                if (frameTime.Update())
                {
                    nextFrame();
                }
            }

            if (image.IsDeleted)
            {
                DeleteMe();
            }
        }

        void nextFrame()
        {
            currentIx += dir.Value;

            if (currentIx < 0)
            {
                currentIx = frameCount - 1;
            }
            else if (currentIx >= frameCount)
            {
                currentIx = 0;
            }

            if (frameOffsetAnimation)
            {
                image.SetSpriteName(startFrame, currentIx);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
