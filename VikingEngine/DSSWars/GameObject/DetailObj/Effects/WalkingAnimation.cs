﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    struct WalkingAnimation
    {
        public const float StandardMoveFrames = 0.03f;
        public const float StandardMoveFramesGnome = 0.025f;
        public static readonly WalkingAnimation Standard = new WalkingAnimation(3, 6, StandardMoveFrames);
        public static readonly WalkingAnimation WorkerWalking = new WalkingAnimation(5, 8, StandardMoveFramesGnome);
        public static readonly WalkingAnimation WorkerCarry = new WalkingAnimation(9, 12, StandardMoveFramesGnome);
        public static readonly WalkingAnimation WorkerTrading = new WalkingAnimation(13, 16, StandardMoveFramesGnome);

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
