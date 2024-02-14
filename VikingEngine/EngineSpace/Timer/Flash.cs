using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class Flash : AbsFlash
    {//Will turn on and off the image a few times to give attention to it
        //const int FLASH_TIME = 50;
        //const int FLASHES = 4;
        //int flashCounting = 0;
        //Graphics.Mesh obj;

        public Flash(Graphics.Mesh flashObj)
            : base(flashObj)
        {

            //obj = flashObj;
            //baseInit(FLASH_TIME);
        }
        protected override void flash()
        {
            base.flash();
            obj.Visible = !flashOn;
        }
        //public override void Time_Update(float time)
        //{
        //    timeLeft -= time;
        //    if (timeLeft <= 0)
        //    {
        //        flashCounting++;
        //        obj.Visible = !obj.Visible;
        //        if (flashCounting == FLASHES)
        //        {
        //            return true;
        //        }
        //        timeLeft = FLASH_TIME;
        //    }
        //    return false;
        //}
    }
}
