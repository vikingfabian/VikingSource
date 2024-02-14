using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.GameObject
{
    class CatAngel : AbsUpdateable
    {
        Graphics.Image image;

        public CatAngel(Graphics.Image animal, int gamerIndex, int facing, SpriteName tile)
            :base(true)
        {
            //BirdLib.SetGameLayer();

            image = new Graphics.Image(tile, animal.Position,
                animal.Size, PjLib.LayerFeather, true);
            image.PaintLayer += gamerIndex * 2f * PublicConstants.LayerMinDiff;
            image.spriteEffects = animal.spriteEffects;
            image.Opacity = 0.5f;
        }

        public override void Time_Update(float time_ms)
        {
            image.Ypos -= Ref.DeltaTimeSec * image.Height * 2f;
        }
    }
}
