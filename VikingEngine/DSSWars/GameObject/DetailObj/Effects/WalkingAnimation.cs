using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    struct WalkingAnimation
    {
        public const float StandardMoveFrames = 0.03f;
        public static readonly WalkingAnimation Standard = new WalkingAnimation(3, 6, StandardMoveFrames);
        public static readonly WalkingAnimation WorkerCarry = new WalkingAnimation(7, 10, StandardMoveFrames);

        public int startframe, endFrame;
        int currentFrame;

        float movelengthBetweenFrames;
        float moveLength;

        public WalkingAnimation(int startframe, int endFrame, float movelengthBetweenFrames)
        {
            this.startframe = startframe;
            this.endFrame = endFrame;
            this.movelengthBetweenFrames = movelengthBetweenFrames;

            currentFrame = startframe;
            moveLength = 0;
        }

        public void update(float speed, Graphics.AbsVoxelObj model)
        {
            moveLength += speed;
            if (moveLength >= movelengthBetweenFrames)
            {
                moveLength -= movelengthBetweenFrames;
                if (++currentFrame > endFrame)
                {
                    currentFrame = startframe;
                }

                model.Frame = currentFrame;
            }
        }

        public void randomStartFrame()
        {
            currentFrame = Ref.rnd.Int(startframe, endFrame + 1);
        }
    }
}
