using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    class BrickMover
    {
        int fallSpeedLevel = 1;
        public float fallDistPercent = 0f;
        public float fallSpeedMultiplier;

        public BrickMover(int fallSpeedLevel = 1)
        {
            this.fallSpeedLevel = fallSpeedLevel;
        }

        public bool update(ref IntVector2 gridPos)
        {   
            fallDistPercent += LevelToSpeed(fallSpeedLevel) * fallSpeedMultiplier;

            if (fallDistPercent >= 1f)
            {
                fallSpeedLevel++;
                gridPos.Y++;
                fallDistPercent %= 1f;
                return true;
            }

            return false;
        }

        public static float LevelToSpeed(int level)
        {
            float speed;
            if (level == 0)
            {
                speed = 1f;
            }
            else if (level == 1)
            {
                speed = 4f;
            }
            else if (level == 2)
            {
                speed = 10f;
            }
            else
            {
                speed = 20f;
            }

            return speed * Ref.DeltaTimeSec;
        }

        public void onRotate()
        {
            fallSpeedLevel = 0;
        }
    }


}
