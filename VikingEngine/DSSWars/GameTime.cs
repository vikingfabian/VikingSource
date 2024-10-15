using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars
{
    class GameTime
    {
        const float Quarter = 0.25f;

        public bool oneSecond = false;
        public bool oneMinute = false;
        public bool oneSecond_part2 = false;
        public bool halfSecond = false;
        float second = 0;
        int quarter = 0;
        int secondsToMinute = 0;
        int totalMinutes = 0;

        float asyncGameObjects_Seconds = 0;
        float asyncWork_Seconds = 0;

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
                    case 2:
                        oneMinute = false;
                        oneSecond_part2 = true;
                        asyncGameObjects_Seconds += 1f;
                        asyncWork_Seconds  += 1f;
                        if (++secondsToMinute >= 60)
                        {
                            secondsToMinute = 0;
                            ++totalMinutes;
                            oneMinute = true;
                            DssRef.state.OneMinute_Update();
                        }
                        break;
                    case 3: halfSecond = true; break;
                }                
            }
        }

        //public bool oneMinute()
        //{
        //    return secondsToMinute == 59;
        //}

        public float pullAsyncGameObjects_Seconds()
        {
            float result = asyncGameObjects_Seconds;
            asyncGameObjects_Seconds -= result;
            return result;
        }

        public float pullAsyncWork_Seconds()
        {
            float result = asyncWork_Seconds;
            asyncWork_Seconds -= result;
            return result;
        }

        public bool pullMinute(ref int totalMinutes)
        {
            if (this.totalMinutes > totalMinutes)
            { 
                totalMinutes = this.totalMinutes;
                return true;
            }

            return false;
        }

        public TimeSpan TotalIngameTime()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(totalMinutes);
            timeSpan = timeSpan.Add(TimeSpan.FromSeconds(secondsToMinute));

            return timeSpan;
        }
    }
}
