using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class RectangleLines
    {
        public VectorRect rectangle;
        public float thickness;
        public float lineCenter; //0 inside, 0.5 centered, 1 outside 

        public Image[] lines = null;

        public RectangleLines(VectorRect rectangle, float thickness, float lineCenter, ImageLayers layer)
        {
            this.rectangle = rectangle;
            this.thickness = thickness;
            this.lineCenter = lineCenter;

            lines = new Image[4];
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = new Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, layer, false);
            }

            Refresh();
        }

        public void SetRectangle(VectorRect rectangle)
        {
            this.rectangle = rectangle;
            Refresh();
        }
        public void Refresh(VectorRect rectangle)
        {
            this.rectangle = rectangle;
            Refresh();
        }
        public void Refresh()
        {
            VectorRect linesOuterArea = rectangle;
            linesOuterArea.AddRadius(thickness * lineCenter);

            //N
            lines[0].Position = linesOuterArea.Position;
            lines[0].Size = new Vector2(linesOuterArea.Size.X, thickness);

            //S
            lines[1].Position = new Vector2(linesOuterArea.Position.X, linesOuterArea.Bottom - thickness);
            lines[1].Size = lines[0].Size;

            //W
            lines[2].Position = linesOuterArea.Position;
            lines[2].Size = new Vector2(thickness, linesOuterArea.Size.Y);

            //E
            lines[3].Position = new Vector2(linesOuterArea.Right - thickness, linesOuterArea.Position.Y);
            lines[3].Size = lines[2].Size;

        }

        public void DeleteMe()
        {
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; ++i)
                {
                    lines[i].DeleteMe();
                }
                lines = null;
            }
        }

        public bool Visible
        {
            get
            {
                return lines[0].Visible;
            }
            set
            {
                if (lines[0].Visible != value)
                {
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        lines[i].Visible = value;
                    }
                }
            }
        }

        public void setColor(Color col, float alpha = 1f)
        {
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i].ColorAndAlpha(col, alpha);
            }
        }
    }
}
