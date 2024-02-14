using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    static class Table
    {
        public static float TotalWidth(int cellCount, float cellSize, float spacing)
        {
            return cellSize * cellCount + spacing * (cellCount - 1);
        }

        public static float CenterTableWidth(float centerPos, float cellSize, float spacing,  int cellCount)
        {
            float tableStartPos = centerPos - TotalWidth(cellCount, cellSize, spacing) * 0.5f;
            return tableStartPos;
        }

        public static IntVector2 IndexToGrindex_LeftToRight(int cellIndex, int tableCellCountX)
        {
            IntVector2 grindex = IntVector2.Zero;
            grindex.Y = cellIndex / tableCellCountX;
            grindex.X = cellIndex - grindex.Y * tableCellCountX;

            return grindex;
        }

        public static VectorRect CellPlacement(Vector2 startPosition, bool startFromCenter, int cellIndex, int tableCellCountX, Vector2 cellSize, Vector2 spacing)
        {
            if (startFromCenter)
            {
                startPosition.X -= TotalWidth(tableCellCountX, cellSize.X, spacing.X) * 0.5f;
                startPosition.Y -= cellSize.Y * 0.5f;
            }

            IntVector2 grindex = IndexToGrindex_LeftToRight(cellIndex, tableCellCountX);

            return new VectorRect(startPosition + grindex.Vec * (spacing + cellSize), cellSize);
        }

        public static int FitCellCount(float cellSize, float spacing,  float totalWidth)
        {
            //spacing * (x-1) + cellSize * x = totalWidth
            //(spacing + cellSize) * x - spacing = totalWidth
            //x = (totalWidth + spacing) / (spacing + cellSize)

            return (int)((totalWidth + spacing) / (spacing + cellSize));
        }
    }

}
