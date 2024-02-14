using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class DamageFlash : AbsInGameUpdateable
    {
        const int NumFlashes = 6;
        int flashCount = 0;
        Graphics.AbsVoxelObj image;
        Timer.Basic timer;
        public DamageFlash(Graphics.AbsVoxelObj image, float immortalityTime)
            :base(true)
        {
            this.image = image;
            timer = new Timer.Basic(immortalityTime / NumFlashes, true); 
        }

        public override void Time_Update(float time)
        {
            if (timer.Update())
            {
                if (flashCount > NumFlashes)
                    DeleteMe();
                else
                    flash();
            }
        }
        
        void flash()
        {
            image.Color = lib.IsEven(flashCount) ? Color.Gray : Color.LightSalmon;
            flashCount++;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.Color = Color.White;
        }
    }
}
