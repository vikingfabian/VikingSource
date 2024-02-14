using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    class PercentageHoriBar : Image 
    {
        //VectorRect barArea;
        Graphics.Image bar1, bar2;
        float edgeWidth;
        public float Value;

        public PercentageHoriBar(Vector2 position, Vector2 size, float edgeWidth, Color edgeColor, Color color1, Color color2, float value, ImageLayers layer, bool centerMidpoint)
            :base(SpriteName.WhiteArea, position, size, layer, centerMidpoint)
        {
            this.edgeWidth = edgeWidth;
            this.Value = value;
            this.Color = edgeColor;
            
            bar1 = new Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, layer, false, false);
            bar1.Color = color1;
            bar1.PaintLayer -= PublicConstants.LayerMinDiff;
           
            bar2 = new Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, layer, false, false);
            bar2.Color = color2;
            bar2.PaintLayer -= PublicConstants.LayerMinDiff;
        }

        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                base.Draw(cameraIndex);

                VectorRect barArea = new VectorRect(position, size);
                barArea.Position -= size * origo;
                barArea.AddRadius(-edgeWidth);


                bar1.Position = barArea.Position;
                bar1.Size = new Vector2(barArea.Width * Value - edgeWidth * 0.5f, barArea.Height);

                bar2.Xpos = barArea.X + barArea.Width * Value + edgeWidth;
                bar2.Ypos = barArea.Y;
                bar2.Width = (1f - Value) * barArea.Width - edgeWidth * 0.5f;
                bar2.Height = barArea.Height;

                bar1.Draw(cameraIndex);
                bar2.Draw(cameraIndex);
            }
        }
    }
}
