using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Strategy
{
    class GamerDisplay
    {
        Graphics.Image playerIcon, playerButton, coinsIcon, vpIcon;
        Display.SpriteText coinsText, vpText;

        public GamerDisplay()
        {
            StrategyLib.SetHudLayer();

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, 
                new Vector2(Engine.Screen.Width, Engine.Screen.SafeArea.Y + Engine.Screen.IconSize * 1.2f), 
                ImageLayers.Background5);
            bg.Color = ColorExt.VeryDarkGray;

            playerIcon = new Graphics.Image(SpriteName.pigP1Dead, Engine.Screen.SafeArea.Position, new Vector2(Engine.Screen.IconSize), ImageLayers.Lay4);
            playerButton = new Graphics.Image(SpriteName.Key0, playerIcon.RightTop, new Vector2(Engine.Screen.IconSize * 0.6f), ImageLayers.Lay4);

            coinsIcon = new Graphics.Image(SpriteName.birdCoin1, playerIcon.Position + new Vector2(Engine.Screen.IconSize * 3f, 0f), new Vector2(Engine.Screen.IconSize), ImageLayers.Lay4);
            coinsText = new Display.SpriteText("0", coinsIcon.RightTop, coinsIcon.Height, ImageLayers.Lay3, Vector2.Zero, Color.Yellow, true);

            vpIcon = new Graphics.Image(SpriteName.winnerParticle, coinsIcon.Position + new Vector2(Engine.Screen.IconSize * 3f, 0f), new Vector2(Engine.Screen.IconSize), ImageLayers.Lay4);
            vpText = new Display.SpriteText("0", vpIcon.RightTop, coinsIcon.Height, ImageLayers.Lay3, Vector2.Zero, Color.Yellow, true);

            playerIcon.origo = VectorExt.V2Half;
            playerIcon.Position += playerIcon.Size * 0.5f;
            playerIcon.Size *= 1.4f;
        }

        public void refreshGamer(Gamer g)
        {
            StrategyLib.SetHudLayer();

            playerIcon.SetSpriteName(g.animalSetup.wingUpSprite);

            playerButton.SetSpriteName(g.data.button.Icon);

            coinsText.Text(g.Coins.ToString());
            vpText.Text(g.VictoryPoints.ToString());


        }
    }
}
