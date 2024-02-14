using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    class RenderTargetImage : AbsRenderTargetImage
    {
        /* Properties */
        protected override BlendState blendState { get { return BlendState.AlphaBlend; } }

        /* Fields */
        public RenderTarget2D renderTarget;

        /* Constructors */

        public RenderTargetImage(Vector2 pos, Vector2 size, ImageLayers layer)
        : this(pos, size, layer, true)
        { }

        public RenderTargetImage(Vector2 pos, Vector2 size, ImageLayers layer, bool addToRender)
            :base(pos, size, addToRender)
        {
            Layer = layer;
            renderTarget = new RenderTarget2D(Engine.Draw.graphicsDeviceManager.GraphicsDevice, drawSource.Width, drawSource.Height, false, 
                SurfaceFormat.Color, DepthFormat.Depth24);
        }
        

        /* Methods */
        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                destination.X = (int)position.X;
                destination.Y = (int)position.Y;
                destination.Width = (int)size.X;
                destination.Height = (int)size.Y;

                drawOrigin.X = origo.X * renderTarget.Width;
                drawOrigin.Y = origo.Y * renderTarget.Height;

                Ref.draw.spriteBatch.Draw(renderTarget, destination, drawSource, DrawColor(), 0, drawOrigin, 
                    SpriteEffects.None, this.layer);
            }
        }

        public override void SetAsTarget(bool set, CubeMapFace face)
        {
            if (set)
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);
            else
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
        }

        public override bool ViewArea(VectorRect area, bool dimOut)
        {
            return true;
        }
    }
}
