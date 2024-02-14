using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class RenderTargetDraw3dContainer : RenderTargetImage, IDrawContainer
    {
        public List<Graphics.AbsDraw> renderList = new List<AbsDraw>();

        public RenderTargetDraw3dContainer(Vector2 pos, Vector2 size, ImageLayers layer)
           : base(pos, size, layer)
        { }
        
        /// <summary>
        /// Has to be manually called before each render
        /// </summary>
        public void DrawModelsToTarget()
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);
            {
                //Ref.draw.spriteBatch.Begin();
                //Ref.draw.spriteBatch.End();
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState.Default;

                Engine.Draw.graphicsDeviceManager.GraphicsDevice.Clear(ClearColor);

                AbsCamera storeCam = Ref.draw.Camera;
                if (Camera != null)
                {
                    Ref.draw.Camera = Camera;
                }

                foreach (var m in renderList)
                {
                    m.Draw(0);
                }

                Ref.draw.Camera = storeCam;
            }
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
        }

        public void setCamera(AbsCamera cam)
        {
            cam.setAspectRatio(renderTarget.Width, renderTarget.Height);
            this.Camera = cam;
        }

        public void setAsContainer(bool set)
        {
            if (set)
            {
                Ref.draw.AddToContainer = this;
            }
            else
            {
                Ref.draw.AddToContainer = null;
            }
        }

        public void AddImage(Graphics.AbsDraw image)
        {
            renderList.Add(image);
        }
        public void RemoveImage(Graphics.AbsDraw image)
        {
            renderList.Remove(image);
        }
    }
}
