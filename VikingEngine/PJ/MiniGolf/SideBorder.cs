using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class SideBorder
    {
        VectorRect playersHudArea;

        public SideBorder(VectorRect playArea, VectorRect safeArea)
        {
            Color bgCol = ColorExt.VeryDarkGray;

            playersHudArea = VectorRect.FromTwoPoints(safeArea.Position,
                new Vector2(playArea.X, safeArea.Bottom));
            Graphics.Image leftBg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, 
                new Vector2(playersHudArea.Right - GolfRef.field.squareSize.X * 0.5f, Engine.Screen.Height), 
                GolfLib.BorderLayer +1);
            leftBg.Color = bgCol;
            
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
