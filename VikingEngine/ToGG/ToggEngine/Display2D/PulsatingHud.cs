using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class PulsatingHud : AbsUpdateable
    {
        const float Opacity = 0.7f;
        Graphics.RenderTargetImage drawContainer;
        Vector2 startScale;
        bool outwardDir = true;

        public PulsatingHud(VectorRect area, bool thick, float? hudPaintLay = null)
            :base(true)
        {
            float edgeWidth = MathExt.Round(HudLib.ThickBorderEdgeSize * (thick? 2f : 0.8f));

            area.AddRadius(edgeWidth * 0.6f);


            VectorRect imageArea = area;
            imageArea.Position = Vector2.Zero;
            imageArea.AddRadius(-edgeWidth * 0.6f);
            startScale = area.Size;

            var images = new List<Graphics.AbsDraw>(8);

            for (Corner c = 0; c < Corner.NUM; ++c)
            {
                Vector2 pos1 = imageArea.GetCorner(c);
                Vector2 pos2 = imageArea.GetCorner(lib.WrapCorner(c + 1));

                Graphics.Line line = new Graphics.Line(edgeWidth, ImageLayers.Lay0,
                    Color.White, pos1, pos2, false);
                Graphics.Image cornerImg = new Graphics.Image(SpriteName.WhiteCirkle, pos1, new Vector2(edgeWidth), 
                    ImageLayers.Lay0, true, false);

                images.Add(line);
                images.Add(cornerImg);
            }

            drawContainer = new Graphics.RenderTargetImage(area.Center, area.Size, HudLib.BgLayer + 4, true);//HudLib.ContentLayer + 1, true);
            if (hudPaintLay != null)
            {
                drawContainer.PaintLayer = hudPaintLay.Value + PublicConstants.LayerMinDiff;
            }
            drawContainer.DrawImagesToTarget(images);
            drawContainer.Opacity = Opacity;
            drawContainer.OrigoAtCenter();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            drawContainer.DeleteMe();
        }

        public override void Time_Update(float time_ms)
        {
            //float sizeUp = Ref.DeltaTimeSec * Engine.Screen.IconSize * 0.04f;
            float opacityChange = Ref.DeltaTimeSec * 0.2f;

            if (outwardDir)
            {
                //drawContainer.size += new Vector2(sizeUp);
                drawContainer.Opacity -= opacityChange;

                if (drawContainer.Opacity <= 0.3f)
                {
                    outwardDir = false;
                }
            }
            else
            {
                //drawContainer.size -= new Vector2(sizeUp);
                drawContainer.Opacity += opacityChange;

                if (drawContainer.Opacity >= Opacity)
                {
                    outwardDir = true;
                }
            }
        }
    }
}
