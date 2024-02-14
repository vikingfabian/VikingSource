using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class SideBorders
    {
        VectorRect playersHudArea;

        public SideBorders(VectorRect activeArea, VectorRect safeArea)
        {
            Color bgCol = ColorExt.VeryDarkGray;

            playersHudArea = VectorRect.FromTwoPoints(safeArea.Position,
                new Vector2(activeArea.X, safeArea.Bottom));
            Graphics.Image leftBg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(playersHudArea.Right, Engine.Screen.Height), ImageLayers.Top7);
            leftBg.Color = bgCol;

            Graphics.Image rightBg = new Graphics.Image(SpriteName.WhiteArea, new Vector2(activeArea.Right, 0), 
                new Vector2(Engine.Screen.Width - activeArea.Right, Engine.Screen.Height), ImageLayers.Top7);
            rightBg.Color = bgCol;

            //playersHudArea.Y = Engine.Screen.SafeArea.Y;
            playersHudArea.Height = safeArea.Height / PjLib.SharedControllerMaxPlayers;

        }

        public VectorRect getPlayerHudArea(int index)
        {
            VectorRect result = playersHudArea;
            result.Y += playersHudArea.Height * index;

            return result;
        }
    }
}
