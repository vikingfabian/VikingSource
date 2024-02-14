using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    class GuiLayoutHeadButton
    {
        VectorRect area;
        Graphics.Image image, highlight, input;

        public GuiLayoutHeadButton(SpriteName icon, Vector2 center, float barheight, SpriteName inputIcon, int inputIconDir, float layer)
        {
            area = VectorRect.FromCenterSize(center, new Vector2(barheight * 0.8f));
            image = new Graphics.Image(icon, area.Position, area.Size, ImageLayers.AbsoluteTopLayer);
             
            highlight = new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(barheight * 0.86f), ImageLayers.AbsoluteTopLayer, true);
            highlight.Visible = false;

            input = new Graphics.Image(inputIcon, area.Center, area.Size * 0.9f, ImageLayers.AbsoluteTopLayer, true);
            input.Xpos += inputIconDir * area.Width;

            setLayer(layer);
        }

        /// <returns>Click</returns>
        public bool update()
        {
            if (area.IntersectPoint(Input.Mouse.Position))
            {
                highlight.Visible = true;
                image.Color = Color.Black;
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    return true;
                }
            }
            else
            {
                image.Color = Color.White;
                highlight.Visible = false;
            }

            return false;
        }

        public void setLayer(float lay)
        {
            image.PaintLayer = lay - PublicConstants.LayerMinDiff * 2;
            input.PaintLayer = image.PaintLayer;
            highlight.PaintLayer = lay - PublicConstants.LayerMinDiff * 1;
        }

        public void DeleteMe()
        {
            image.DeleteMe();
            highlight.DeleteMe();
            input.DeleteMe();
        }
    }
}
