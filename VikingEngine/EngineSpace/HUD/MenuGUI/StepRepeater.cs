using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    /// <summary>
    /// This class serves to make analog gamepad stick input act like arrow keys in windows, which use stepping.
    /// </summary>
    class StepRepeater
    {
        const float DEAD_ZONE = 0.1f;
        const int MS_DELAY = 300;
        const int MS_REPEAT = 120;
        
        float timeHeld;
        float timeSinceRepeat;

        public IntVector2 GetStepping(Vector2 direction)
        {
            bool stepThisFrame = false;

            // If moving along any axis, normalize along that axis.
            if (direction != Vector2.Zero)
            {
                if (direction.X < -DEAD_ZONE)
                {
                    direction.X = -1;
                }
                else if (direction.X > DEAD_ZONE)
                {
                    direction.X = 1;
                }

                if (direction.Y < -DEAD_ZONE)
                {
                    direction.Y = -1;
                }
                else if (direction.Y > DEAD_ZONE)
                {
                    direction.Y = 1;
                }
            }

            // If we have input outside the dead zone
            if (direction != Vector2.Zero)
            {
                if (timeHeld == 0)
                {
                    // If this is new input, we go
                    stepThisFrame = true;
                }

                timeHeld += Ref.DeltaTimeMs;

                if (timeHeld > MS_DELAY - MS_REPEAT)
                {
                    // start counting up to the first repetetion
                    timeSinceRepeat += Ref.DeltaTimeMs;

                    if (timeSinceRepeat > MS_REPEAT)
                    {
                        timeSinceRepeat -= MS_REPEAT;
                        stepThisFrame = true;
                    }
                }
            }
            else
            {
                // If there is completely new input, we reset the time held.
                timeHeld = 0;
                timeSinceRepeat = 0;
            }

            if (stepThisFrame)
            {
                return new IntVector2(direction);
            }

            return IntVector2.Zero;
        }
    }
}
