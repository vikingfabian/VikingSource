using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    /// <summary>
    /// Cloud that moves in the background of main menu
    /// </summary>
    class CheckerCloud : AbsUpdateable
    {
        Timer.Basic moveTimer = new Timer.Basic(Ref.rnd.Int(3000, 20000), true);
        float moveSpeed = lib.RandomFloat(0.02f, 1f);
        List<Graphics.Image> imageGroup;

        public CheckerCloud(int layer)
            :base(true)
        {
            Vector2 posAdd = new Vector2(lib.RandomFloat(MainMenuState.CheckerSize), lib.RandomFloat(MainMenuState.CheckerSize));
            Vector2 sqSz = new Vector2(MainMenuState.CheckerSize);
            ColorRange colorRange = new ColorRange(Color.DarkGray, Color.LightGray);
            IntervalF transparentsyRange = new IntervalF(0.1f, 0.04f);
            //MainMenuState.CheckerSize
            //MainMenuState.NumCheckerSquares
            Rectangle2 area = new Rectangle2(
                Ref.rnd.Int(MainMenuState.NumCheckerSquares.X), Ref.rnd.Int(-6, (int)(MainMenuState.NumCheckerSquares.Y * 0.7f)),
                Ref.rnd.Int(8, 14), Ref.rnd.Int(4, 6));

            imageGroup = new List<Graphics.Image>();
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                int edgeDist = Math.Abs(area.LengthToClosestTileEdgeX(loop.Position.X)) + Math.Abs(area.LengthToClosestTileEdgeY(loop.Position.Y));
                int chance = 80;
                const int Edge = 3;
                if (edgeDist <= Edge)
                {
                    chance -= (Edge - edgeDist) * 20;
                }

                if (Ref.rnd.RandomChance(chance))
                {
                    Graphics.Image tile = new Graphics.Image(SpriteName.WhiteArea, (loop.Position * MainMenuState.CheckerSize).Vec + posAdd,
                        sqSz, ImageLayers.Background5);
                    tile.PaintLayer -= PublicConstants.LayerMinDiff * layer;
                    tile.Color = colorRange.GetRandomPercentPos();
                    tile.Transparentsy = transparentsyRange.GetRandom();
                    imageGroup.Add(tile);
                }
            }


        }

        public override void Time_Update(float time)
        {
            //Slowly move the clound, one block at a time
            //if (moveTimer.Update())
            //{
                foreach (Graphics.Image img in imageGroup)
                {
                    img.Xpos += moveSpeed;//MainMenuState.CheckerSize;
                    if (img.Position.X > MainMenuState.NumCheckerSquares.X * MainMenuState.CheckerSize)
                    {
                        img.Xpos = -MainMenuState.CheckerSize;
                    }
                }
            //}
        }
    }
}
