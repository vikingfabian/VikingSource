using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class SpriteFlash : AbsUpdateable
    {
        AbsDraw image;
        bool bSprite1 = true;
        SpriteName sprite1, sprite2;
        int flashCount;
        Timer.Basic flashTimer;

        public SpriteFlash(AbsDraw image, SpriteName sprite1, SpriteName sprite2, int flashCount, float flashTime)
            : base(true)
        {
            this.image = image;
            this.sprite1 = sprite1;
            this.sprite2 = sprite2;
            this.flashCount = flashCount;
            this.flashTimer = new Timer.Basic(flashTime, true);

            image.SetSpriteName(sprite1);
        }

        public override void Time_Update(float time_ms)
        {
            if (flashTimer.Update())
            {
                bSprite1 = !bSprite1;
                image.SetSpriteName(bSprite1 ? sprite1 : sprite2);

                if (--flashCount <= 0)
                {
                    DeleteMe();
                }
            }
        }
    }
}
