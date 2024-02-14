using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Engine
{
    class StepBuffert
    {
        float accumulation = 0;
        bool keyDown = false;
        const float FalloverVal = 180;
        const float HspeedFalloverVal = 75;
        float fallOver = FalloverVal;

        public int Update(float joystickVal)
        {
            const float InputBuffer = 0.3f;
            if (Math.Abs(joystickVal) <= InputBuffer)
            {
                PadUp();
                return 0;
            }
            else
            {
                accumulation += joystickVal * Ref.DeltaTimeMs;
                if (!keyDown)
                {
                    keyDown = true;
                    return lib.ToLeftRight(accumulation);
                }
                else if (Math.Abs(accumulation) >= fallOver)
                {
                    fallOver = HspeedFalloverVal;
                    int result = lib.ToLeftRight(accumulation);
                    accumulation = 0;
                    return result;
                }

            }
            return 0;
        }
        public void PadUp()
        {
            keyDown = false;
            accumulation = 0;
            fallOver = FalloverVal;
        }
    }

}
