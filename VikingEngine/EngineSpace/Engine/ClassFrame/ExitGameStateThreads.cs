using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Engine
{
    class ExitGameStateThreads : AbsUpdateable
    {
        Action exitGameState;
        public int startCount;
        public int currentCount;
        Time timeOut = new Time(5, TimeUnit.Seconds);
        public ExitGameStateThreads(Action exitGameState) 
            :base(true)
        {
            this.exitGameState = exitGameState;
            startCount = Ref.update.AbortThreads();
        }

        public override void Time_Update(float time_ms)
        {
            currentCount = Ref.update.AbortThreads();
            if (timeOut.CountDown() || currentCount == 0)
            {
                exitGameState();
#if DEBUG
                if (currentCount > 0)
                {
                    lib.DoNothing();
                }
#endif
            }
        }

    }
}
