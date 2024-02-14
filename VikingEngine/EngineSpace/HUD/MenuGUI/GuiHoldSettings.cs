using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.HUD
{
    class GuiHoldSettings
    {
        float startTime;
        float levelSpeedUp;
        int clicksToLevelUp;
        int maxLevel;

        int speedLevel;
        int clickCount;
        float nextClickTime;
        float time;

        public GuiHoldSettings(float startTime = 400, 
            float levelSpeedUp = 0.5f, 
            int clicksToLevelUp = 2, 
            int maxLevel = 3)
        {
            this.startTime = startTime;
            this.levelSpeedUp = levelSpeedUp;
            this.clicksToLevelUp = clicksToLevelUp;
            this.maxLevel = maxLevel;

            speedLevel = 0;
            clickCount = 0;
            nextClickTime = 0;
            time = 0;
        }

        public void reset()
        {
            speedLevel = 0;
            clickCount = 0;
            nextClickTime = startTime;
            time = 0;
        }

        public bool update()
        {
            time += Ref.DeltaTimeMs;
            if (time >= nextClickTime)
            {
                time -= nextClickTime;

                if (speedLevel < maxLevel)
                {
                    if (++clickCount >= clicksToLevelUp)
                    {
                        clicksToLevelUp = 0;
                        speedLevel++;
                        nextClickTime *= levelSpeedUp;
                    }
                }
                return true;
            }

            return false;
        }
    }
}
