using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class GamephaseBorder
    {
        Graphics.ImageGroup2D borders, icons;

        public GamephaseBorder()
        {
            icons = new Graphics.ImageGroup2D();

            VectorRect area = new VectorRect(0, 0, Engine.Screen.Width, Engine.Screen.SafeArea.Y);
            area.AddRadius(1f);

            Graphics.Image topBorder = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background8);
            addIcons(area);

            area.Y = Engine.Screen.SafeArea.Bottom;
            Graphics.Image bottomBorder = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background8);
            addIcons(area);

            borders = new Graphics.ImageGroup2D(topBorder, bottomBorder);

            setVisible(false);
        }

        void addIcons(VectorRect area)
        {
            Vector2 iconSz = new Vector2(area.Height * 0.8f);

            Vector2 pos = area.LeftCenter;
            do
            {
                Graphics.Image icon = new Graphics.Image(SpriteName.cmdStatsMelee,
                    pos, iconSz, ImageLayers.Background7, true);
                icons.Add(icon);

                pos.X += iconSz.X * 1.6f;
            } while (pos.X < area.Right);
        }

        public void setVisible(bool visible)
        {
            borders.SetVisible(visible);
            icons.SetVisible(visible);
        }

        public void SetVisuals(Color bgCol, SpriteName iconTile, Color iconColor)
        {
            borders.SetColor(bgCol);

            icons.SetSpriteName(iconTile);
            icons.SetColor(iconColor);

            setVisible(true);
        }
    }
}
