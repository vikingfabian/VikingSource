using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class AbsFlash: AbsTimer
    {//Will turn on and off the image a few times to give attention to it
        const int FLASH_TIME = 50;
        const int FLASHES = 4;
        int flashCounting = 0;
        protected Graphics.Mesh obj;
        protected bool flashOn = false;

        public AbsFlash(Graphics.Mesh flashObj)
            : base(FLASH_TIME, UpdateType.Lazy)
        {
            obj = flashObj;
            //baseInit(FLASH_TIME);
        }
        public override void Time_Update(float time)
        {
            timeLeft -= time;
            if (timeLeft <= 0)
            {
                flashCounting++;
                //obj.Visible = !obj.Visible;
                flash();
                if (flashCounting == FLASHES)
                {
                    DeleteMe();
                    //return true;
                }
                timeLeft = FLASH_TIME;
            }
            //return false;
        }
        virtual protected void flash()
        {
            flashOn = !flashOn;
        }
    }
}
