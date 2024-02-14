using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class LevelLayout
    {
       public  List<BrickBox> boxes;

        public LevelLayout(int playerCount)
        {
            //playerCount = 8;
            
            Ref.draw.ClrColor = Color.CornflowerBlue;
            float boxWidth = Engine.Screen.SafeArea.Width * 0.18f;

            //if (playerCount <= 5)
            //{
            //    boxWidth = Engine.Screen.SafeArea.Width * 0.18f;
            //}
            //else
            if (boxWidth * playerCount > Engine.Screen.SafeArea.Width)
            {
                //totalW = (w + space) * count - space
                //(totalW + space) / count = w
                boxWidth = Engine.Screen.SafeArea.Width / playerCount;
                //boxWidth = cellW - cellSpacing;

                //Engine.Screen.SafeArea.Width * (playerCount > 4 ? 0.1f : 0.16f);
            }
            m3Ref.TileWidth = Convert.ToInt32(boxWidth / (BrickBox.BrickCountSz.X + 1));
            boxWidth = MathExt.RoundAndEven(boxWidth - m3Ref.TileWidth);
            
            float cellSpacing = m3Ref.TileWidth;
            m3Ref.ParticleSpeed = m3Ref.TileWidth * 0.01f;
            m3Ref.Gravity = m3Ref.ParticleSpeed * 0.2f;

            float totalW = Table.TotalWidth(playerCount, boxWidth, cellSpacing);

            Vector2 pos = Engine.Screen.CenterScreen;
            pos.X -= totalW * 0.5f;
            pos.Y -= boxWidth - m3Ref.TileWidth;

            boxes = new List<BrickBox>(playerCount);
            for (int i = 0; i < playerCount; ++i)
            {
                boxes.Add(new BrickBox(i, pos));

                pos.X += boxWidth + cellSpacing;
            }
        }                
    }
}
