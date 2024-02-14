using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class PegFlashAnimation : Graphics.AbsUpdateableImage
    {
        const SpriteName StartFrame = SpriteName.bagPegFlashAnim1;
        SpriteName tile = StartFrame;
        Timer.Basic frameTime = new Timer.Basic(30, true);
        Graphics.Image peg;

        public PegFlashAnimation(Graphics.Image peg, Color col)
            : base(StartFrame, peg.Position, peg.Size * 2f, ImageLayers.Background4, true, true, true)
        {
            this.peg = peg;
            this.Color = col;
            Opacity = 0.35f;

        }

        public override void Time_Update(float time_ms)
        {
            position = peg.Position;

            if (frameTime.Update())
            {
                tile++;
                if (tile > SpriteName.bagPegFlashAnim6)
                {
                    DeleteMe();
                }
                else
                {
                    SetSpriteName(tile);
                }
            }
        }
    }
}
