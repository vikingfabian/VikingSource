using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars
{
    class GameTime
    {
        const float Quarter = 0.25f;

        public bool oneSecond = false;
        public bool oneSecond_part2 = false;
        public bool halfSecond = false;
        float second = 0;
        int quarter = 0;

        public GameTime()
        {
            DssRef.time = this;
        }

        public void update()
        {
            second += Ref.DeltaGameTimeSec;
            oneSecond = false;
            oneSecond_part2 = false;
            halfSecond = false;

            if (second >= Quarter)
            {
                second -= Quarter;
                ++quarter;
                if (quarter >= 4)
                { 
                    quarter = 0;
                }

                switch (quarter)
                { 
                    case 0: oneSecond = true; break;
                    case 1: halfSecond = true; break;
                    case 2: oneSecond_part2 = true; break;
                    case 3: halfSecond = true; break;
                }                
            }
        }
    }
}
