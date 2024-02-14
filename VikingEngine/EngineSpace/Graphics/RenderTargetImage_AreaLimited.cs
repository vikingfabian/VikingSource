using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class RenderTargetImage_AreaLimited : RenderTargetImage
    {
        VectorRect viewArea;
        Rectangle drawRect;
        bool insideView;


        public RenderTargetImage_AreaLimited(Vector2 pos, Vector2 size, ImageLayers layer, VectorRect viewArea)
            : base(pos, size, layer)
        {
            this.viewArea = viewArea;
            //updateDrawSettings();
        }

        void updateDrawSettings()
        {
            VectorRect drawSource = new VectorRect(0, 0, renderTarget.Width, renderTarget.Height);
            VectorRect myArea = new VectorRect(position, size);
            insideView = viewArea.IntersectRect(myArea);
            Vector2 sizeToTextureScale = drawSource.Size / size;

            if (insideView)
            {
                if (myArea.X < viewArea.X)
                {
                    float instersection = viewArea.X - myArea.X;
                    myArea.X = viewArea.X;
                    myArea.Width -= instersection;
                    drawSource.AddToLeftSide(-instersection * sizeToTextureScale.X);
                }
                if (myArea.Y < viewArea.Y)
                {
                    float instersection = viewArea.Y - myArea.Y;
                    myArea.Y = viewArea.Y;
                    myArea.Height -= instersection;
                    drawSource.AddToTopSide(-instersection * sizeToTextureScale.Y);
                }

                if (myArea.Right > viewArea.Right)
                {
                    float instersection = myArea.Right - viewArea.Right;
                    myArea.Width -= instersection;
                    drawSource.Width -= instersection * sizeToTextureScale.X;
                }

                if (myArea.Bottom > viewArea.Bottom)
                {
                    float instersection = myArea.Bottom - viewArea.Bottom;
                    myArea.Height -= instersection;
                    drawSource.Height -= instersection * sizeToTextureScale.Y;
                }
            }

            drawRect = myArea.Rectangle;
            this.drawSource = drawSource.Rectangle;
        }

        public override void Draw(int cameraIndex)
        {
            if (insideView && visible)
            {
                Ref.draw.spriteBatch.Draw(renderTarget, drawRect, drawSource, DrawColor(), 0, Vector2.Zero,
                    SpriteEffects.None, this.layer);
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                updateDrawSettings();
            }
        }
        public override Vector2 Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                updateDrawSettings();
            }
        }
    }
}
