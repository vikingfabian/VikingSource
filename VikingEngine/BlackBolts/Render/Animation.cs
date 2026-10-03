using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.Render
{
    struct Animation
    {
        public int startframe, endFrame;
        int currentFrame;

        float frameTime;

        float timePassed;

        public Animation(int startframe, int endFrame, float frameTime)
        {
            this.startframe = startframe;
            this.endFrame = endFrame;
            this.frameTime = frameTime;
            currentFrame = startframe;
            timePassed = 0;
        }

        public void update(float timeMs, Graphics.AbsVoxelObj model, out bool enterEvenFrame)
        {
            enterEvenFrame = false;
            timePassed += timeMs;
            if (timePassed >= frameTime)
            {
                timePassed -= frameTime;
                if (++currentFrame > endFrame)
                {
                    currentFrame = startframe;
                }

                model.Frame = currentFrame;
                enterEvenFrame = lib.IsEven(currentFrame);
            }
        }
    }
}
