using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class ColorFlash : AbsUpdateable
    {
        AbsDraw image;
        bool bColor1 = true;
        Color col1, col2;
        int flashCount;
        Timer.Basic flashTimer;

        public ColorFlash(AbsDraw image, Color col1, Color col2, int flashCount, float flashTime)
            : base(true)
        {
            this.image = image;
            this.col1 = col1;
            this.col2 = col2;
            this.flashCount = flashCount;
            this.flashTimer = new Timer.Basic(flashTime, true);

            image.Color = col1;
        }

        public override void Time_Update(float time_ms)
        {
            if (flashTimer.Update())
            {
                bColor1 = !bColor1;
                image.Color = bColor1 ? col1 : col2;

                if (--flashCount <= 0)
                {
                    DeleteMe();
                }
            }
        }
    }
}