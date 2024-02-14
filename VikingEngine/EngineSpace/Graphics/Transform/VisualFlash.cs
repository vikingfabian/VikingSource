using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class VisualFlash : AbsUpdateable
    {
        AbsDraw img;
        Timer.Basic timer;
        int visibleOnOffTimes;

        public VisualFlash(AbsDraw img, int visibleOnOffTimes, float betweenTime)
            :base(true)
        {
            this.img = img;
            timer = new Timer.Basic(betweenTime, true);
            this.visibleOnOffTimes = visibleOnOffTimes;
        }

        public override void Time_Update(float time_ms)
        {
            if (timer.Update())
            {
                img.Visible = !img.Visible;

                if (--visibleOnOffTimes <= 0 || img.IsDeleted)
                {
                    DeleteMe();
                }
            }
        }

        public float TotalTime => visibleOnOffTimes * timer.goalTime;
    }
}
