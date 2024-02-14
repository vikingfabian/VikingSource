using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class Fade : AbsUpdateable
    {
        AbsDraw img; float add; Time lifeTime;
        public Fade(AbsDraw img, float add, Time time, bool toUpdate)
            : base(toUpdate)
        {
            this.img = img;
            this.add = add / time.MilliSeconds;
            this.lifeTime = time;
        }

        public override void Time_Update(float time)
        {
            img.Opacity += add * time;
            if (lifeTime.CountDown(time))
            {
                DeleteMe();
            }
        }
    }
}
