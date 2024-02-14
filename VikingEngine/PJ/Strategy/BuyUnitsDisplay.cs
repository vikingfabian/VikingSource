using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Strategy
{
    class BuyUnitsDisplay
    {
        Graphics.ImageGroup images;
        Gamer gamer;

        public BuyUnitsDisplay(Gamer gamer)
        {
            this.gamer = gamer;
            StrategyLib.SetHudLayer();

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
                new Vector2(Engine.Screen.Width * 0.75f, Engine.Screen.CenterScreen.Y),
                new Vector2(7 * Engine.Screen.IconSize, 2 * Engine.Screen.IconSize), ImageLayers.Lay7, true);
            bg.Color = Color.DarkGray;
            
            Vector2 iconPos = bg.Position;
            iconPos.X += -bg.Width * 0.5f + Engine.Screen.IconSize;

            Graphics.Image button = new Graphics.Image(gamer.data.button.Icon, iconPos,
                new Vector2(Engine.Screen.IconSize), ImageLayers.Lay6, true);
            button.LayerAbove(bg);

            iconPos.X += Engine.Screen.IconSize * 1.2f;
            Display.SpriteText cost = new Display.SpriteText(StrategyLib.SoldierCost.ToString(), iconPos, 
                Engine.Screen.IconSize * 1.4f, ImageLayers.Lay6,
                new Vector2(0f, 0.5f), Color.Yellow, true);
            iconPos.X += Engine.Screen.IconSize * 1.13f;
            Graphics.Image moneyIcon = new Graphics.Image(SpriteName.birdCoin1, iconPos, 
                new Vector2(Engine.Screen.IconSize * 0.9f), ImageLayers.Lay6, true);
            button.LayerAbove(moneyIcon);

            iconPos.X += Engine.Screen.IconSize * 0.8f;
            Graphics.Image arrow = new Graphics.Image(SpriteName.LfMenuMoreMenusArrow, iconPos,  
                new Vector2(Engine.Screen.IconSize * 0.6f), ImageLayers.Lay6, true);
            iconPos.X += Engine.Screen.IconSize * 1f;
            Graphics.Image soldierIcon = new Graphics.Image(gamer.animalSetup.wingUpSprite, iconPos,
                new Vector2(Engine.Screen.IconSize * 2f), ImageLayers.Lay6, true);

            images = new Graphics.ImageGroup(bg, button, moneyIcon, arrow, soldierIcon);
            cost.AddTo(images.images);
        }

        public void DeleteMe()
        {
            StrategyLib.SetHudLayer();
            images.DeleteAll();
        }
    }
}
