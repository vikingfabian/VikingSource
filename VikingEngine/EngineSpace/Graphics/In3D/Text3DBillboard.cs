using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class Text3DBillboard : RenderTargetBillboard
    {
        TextS textImg;

        public Text3DBillboard(LoadedFont font, string text, Color textCol, Color? bgCol, Vector3 pos, 
            float modelScale, float fontScale, bool addToRender)
            :base(pos, modelScale, addToRender)
        {
            textImg = new TextS(font, Vector2.Zero, new Vector2(fontScale), Align.Zero, text, textCol, ImageLayers.Foreground1, false);

            Vector2 textSz = textImg.MeasureText();
            
            createTexture(textSz, new List<AbsDraw> { textImg }, bgCol);

            setModelSizeFromTexHeight();
            //this.ScaleX = modelScale / textSz.Y * textSz.X;
        }

        public String TextString
        {
            get
            {
                return textImg.TextString;
            }
            set
            {
                textImg.TextString = value;
                renderTarget.DrawImagesToTarget(images, true);
            }
        }

        public override float Scale1D
        {
            get
            {
                return scale.Z;
            }
            set
            {
                scale.Z = value;
                setModelSizeFromTexHeight();
                //scale.X = scale.Z / renderTarget.Height * renderTarget.Width; 
            }
        }
    }

    class RenderTargetBillboard : Billboard2D
    {
        protected RenderTargetImage renderTarget;
        public List<AbsDraw> images;

        public RenderTargetBillboard(Vector3 pos, float modelScale, bool addToRender)
            :base(pos, SpriteName.WhiteArea, modelScale, addToRender)
        {

        }

        public void createTexture(Vector2 targetTextureSize, List<AbsDraw> images, Color? clearColor)
        {
            this.images = images;
            renderTarget = new RenderTargetImage(Vector2.Zero, targetTextureSize, ImageLayers.AbsoluteTopLayer, false);

            //if (bgCol != null)
            renderTarget.ClearColor = clearColor == null ? ColorExt.Empty : clearColor.Value;
            renderTarget.DrawImagesToTarget(this.images, true);
            this.setFullTextureSource(renderTarget.renderTarget);
        }

        public void setModelSizeFromTexWidth()
        {
            scale.Z = scale.X / renderTarget.Width * renderTarget.Height;
        }

        public void setModelSizeFromTexHeight()
        {
            scale.X = scale.Z / renderTarget.Height * renderTarget.Width;
        }
    }
}
