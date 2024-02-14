using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Strategy
{
    class ShippingDisplay
    {
        Graphics.ImageGroup images;

        public ShippingDisplay()
        {
            StrategyLib.SetHudLayer();

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
                new Vector2(Engine.Screen.Width * 0.75f, Engine.Screen.CenterScreen.Y),
                new Vector2(7 * Engine.Screen.IconSize, 2 * Engine.Screen.IconSize), ImageLayers.Lay7, true);
            bg.Color = Color.DarkGray;

            Vector2 iconPos = bg.Position;
            iconPos.X += -bg.Width * 0.5f + Engine.Screen.IconSize;

            Graphics.TextG text = new Graphics.TextG(LoadedFont.Regular, iconPos,
                new Vector2(Engine.Screen.TextSize), Graphics.Align.CenterHeight,
                "Shipping costs " + StrategyLib.ShippingCost.ToString(),
                Color.Yellow, ImageLayers.Lay6);
            text.LayerAbove(bg);

            images = new Graphics.ImageGroup(bg, text);
        }

        public void DeleteMe()
        {
            StrategyLib.SetHudLayer();
            images.DeleteAll();
        }
    }
}
