using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class AbsIconStripToolTip : AbsToolTip
    {
        protected Vector2 nextPos;
        Graphics.Image bg;
        protected float edge, spacing;
        Vector2 iconSz;

        public AbsIconStripToolTip(MapControls mapControls)
            : base(mapControls)
        {
            edge = Engine.Screen.BorderWidth;
            spacing = (int)(Engine.Screen.IconSize * 0.3f);
            iconSz = Engine.Screen.SmallIconSizeV2;

            startBar(Vector2.Zero);
        }

        Graphics.Image startBar(Vector2 pos)
        {
            nextPos = pos;

            bg = new Graphics.Image(SpriteName.WhiteArea,
              nextPos,
              new Vector2(iconSz.Y + edge * 2f), Layer);
            bg.Color = Color.Black;
            bg.Opacity = BgOpacity;
            Add(bg);
            nextPos += new Vector2(edge);

            return bg;
        }

        protected Graphics.Image addIcon(SpriteName iconImage)
        {
            Graphics.Image img = new Graphics.Image(iconImage, nextPos, iconSz, ImageLayers.AbsoluteBottomLayer);
            img.LayerAbove(bg);
            Add(img);

            nextPos.X += iconSz.X;
            return img;
        }

        protected Graphics.TextG addText(string textString)
        {
            Graphics.TextG img = new Graphics.TextG(LoadedFont.Regular, nextPos, Vector2.One, Graphics.Align.Zero,
                textString, Color.White, ImageLayers.AbsoluteBottomLayer);
            img.SetHeight(iconSz.Y);
            img.LayerAbove(bg);
            Add(img);

            nextPos.X += img.MeasureText().X;
            return img;
        }

        protected Graphics.TextG addValue(int value)
        {
            return addText(value.ToString());
        }

        protected void addSpace()
        {
            nextPos.X += spacing;
        }

        protected Vector2 EndBar()
        {
            bg.SetRight(nextPos.X + edge, true);

            return bg.BottomRight;
        }

    }
}
