using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba
{
    class PowersWheel
    {
        Graphics.Image pointer;
        Graphics.ImageGroup images = new Graphics.ImageGroup(16);
        VectorRect area;

        int powerCount = 4;
        public bool waitingForKeyUp = true;
        float percPosition = 0;
        VectorRect[] squares;
        public int result = -1;

        public PowersWheel(GO.Hero hero)
        {
            float iconWidth = Engine.Screen.SmallIconSize;
            float w = iconWidth * powerCount;

            Vector2 pos = hero.image.Position;
            pos.X -= w * 0.5f;
            pos.Y -= hero.image.Height + iconWidth;

            squares = new VectorRect[powerCount];

            area = new VectorRect(pos.X, pos.Y, w, iconWidth);
            for (int i = 0; i < powerCount; ++i)
            {
                Graphics.Image square = new Graphics.Image(SpriteName.WhiteArea, pos, new Vector2(iconWidth), MobaLib.LayerPowerWheel);
                square.Color = Color.Black;
                var ar = square.Area;
                squares[i] = ar;
                ar.AddRadius(-2f);
                Graphics.Image innerSquare = new Graphics.Image(SpriteName.WhiteArea, ar.Position, ar.Size, ImageLayers.AbsoluteBottomLayer);
                innerSquare.LayerAbove(square);

                images.Add(square); images.Add(innerSquare);

                pos.X += iconWidth;
            }

            pointer = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(8f, 16f), MobaLib.LayerPowerWheel-2);
            pointer.Position = area.Position;
            images.Add(pointer);
        }


        public bool update(Input.IButtonMap button)
        {
            if (waitingForKeyUp)
            {
                if (button.UpEvent)
                {
                    waitingForKeyUp = false;
                }
            }
            else
            {
                if (button.DownEvent)
                {
                    result = (int)(powerCount * percPosition);
                    var ar = squares[result];
                    Graphics.Image square = new Graphics.Image(SpriteName.WhiteArea, ar.Position, ar.Size, MobaLib.LayerPowerWheel);
                    const float ViewTime = 400;
                    new Graphics.Motion2d(Graphics.MotionType.OPACITY, square, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, ViewTime, true);
                    new Timer.Terminator(ViewTime, square);

                    return true;
                }

                percPosition += Ref.DeltaTimeSec * 1f;
                pointer.Xpos = area.X + percPosition * area.Width;
            }
            return percPosition >= 1f;
        }

        public void DeleteMe()
        {
            images.DeleteAll();
        }
        
    }
}
