using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine
{
    static class AreaExt
    {
        public static Rectangle[,] Split(Rectangle area, int xDivisions, int yDivisions)
        {
            Rectangle[,] result = new Rectangle[xDivisions, yDivisions];
            area.Width /= xDivisions;
            area.Height /= yDivisions;

            for (int y = 0; y < yDivisions; ++y)
            {
                for (int x = 0; x < xDivisions; ++x)
                {
                    Rectangle split = area;
                    split.X += area.Width * x;
                    result[x, y] = split;
                }

                area.Y += area.Height;
            }

            return result;
        }

        public static VectorRect[,] Split(VectorRect area, int xDivisions, int yDivisions)
        {
            VectorRect[,] result = new VectorRect[xDivisions, yDivisions];
            area.Width /= xDivisions;
            area.Height /= yDivisions;

            for (int y = 0; y < yDivisions; ++y)
            {
                for (int x = 0; x < xDivisions; ++x)
                {
                    VectorRect split = area;
                    split.X += area.Width * x;
                    result[x, y] = split;
                }

                area.Y += area.Height;
            }

            return result;
        }
    }
}
