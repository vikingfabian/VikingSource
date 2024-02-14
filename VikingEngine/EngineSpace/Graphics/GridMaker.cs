using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class GridMaker
    {
        public VectorRect Area
        {
            get { return area; }
        }
        VectorRect area;
        public IntVector2 NumCells
        {
            get { return numCells; }
        }
        IntVector2 numCells; 
        Vector2 borderOuter; 
        Vector2 borderInner;

        bool dominantWidth;

        public const float NoScaleRatio = 0;
        public const float SquareRatio = 1;
        float WidthHeightRatio;// = NoScaleRatio;

        public GridMaker(VectorRect area, IntVector2 numCells, Vector2 borderOuter, Vector2 borderInner, float widthHeightRatio)
        {
            numCells.X = Bound.Min(numCells.X, 1);
            numCells.Y = Bound.Min(numCells.Y, 1);
            this.WidthHeightRatio = widthHeightRatio;

            this.area = area;
            this.numCells = numCells;
            this.borderOuter = borderOuter;
            this.borderInner = borderInner;
            
            //Use the shortest length
            float w = area.Width / numCells.X;
            float h = area.Height / numCells.Y;
            dominantWidth = w <= h;
        }
        public VectorRect Cell(IntVector2 cellIndex)
        {
            
            Vector2 pos = area.Position;
            Vector2 size = Vector2.One;
            if (WidthHeightRatio == NoScaleRatio)
            {
                size.X = area.Width / numCells.X;
                size.Y = area.Height / numCells.Y;
            }
            else
            {//just nu förutsätts kvadratisk form
                if (dominantWidth)
                {
                    size = VectorExt.V2((area.Width - borderOuter.X) / numCells.X);
                }
                else
                {
                    size = VectorExt.V2((area.Height - borderOuter.Y) / numCells.Y);
                }
            }
            pos.X += size.X * cellIndex.X + borderOuter.X + borderInner.X;
            pos.Y += size.Y * cellIndex.Y + borderOuter.Y + borderInner.Y;
            size -= borderInner * 2;
            return new VectorRect(pos, size);
        }
    }
}
