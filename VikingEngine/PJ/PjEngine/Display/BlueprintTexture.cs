using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ.PjEngine.Display
{
    class BlueprintTexture
    {
        public BlueprintTexture()
        {
            const float OutlineOpacity = 0.1f;
            const float GridOpacity = OutlineOpacity * 0.5f;
            
            Graphics.Image bgTex = new Graphics.Image(SpriteName.BlueprintBg, VectorExt.V2NegOne,
                    Engine.Screen.ResolutionVec + new Vector2(2), ImageLayers.Background4);

            VectorRect outlineArea = Engine.Screen.SafeArea;

            Vector2 squareSize = new Vector2(MathExt.Round(Engine.Screen.MinWidthHeight * 0.06f));
            VectorRect gridArea = outlineArea;
            gridArea.AddRadius(-squareSize.X * 0.5f);
            IntVector2 gridCount = new IntVector2(gridArea.Size / squareSize);
            gridArea = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, gridCount.Vec * squareSize);
            outlineArea = gridArea;
            outlineArea.AddRadius(squareSize.X * 0.5f);

            Graphics.RectangleLines outlines = new Graphics.RectangleLines(outlineArea,
                Engine.Screen.BorderWidth, 1f, ImageLayers.Background3);
            outlines.setColor(Color.White, OutlineOpacity);

            ForXYLoop loop = new ForXYLoop(gridCount);
            SpriteName sprite;

            Vector2 scale;
            Vector2 fullSquareScale = squareSize / 48 * 52;

            while (loop.Next())
            {
                if (loop.AtRight || loop.AtBottom)
                {
                    sprite = SpriteName.BluePrintSquareFull;
                    scale = fullSquareScale;
                }
                else
                {
                    sprite = SpriteName.BluePrintSquareHalf;
                    scale = squareSize;
                }

                Graphics.Image sq = new Graphics.Image(sprite, 
                    gridArea.Position + squareSize * loop.Position.Vec,
                    scale, ImageLayers.Background3);
                sq.Opacity = GridOpacity;
            }
        }

    }
}
