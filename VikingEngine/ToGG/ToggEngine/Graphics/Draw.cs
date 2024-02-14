using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG
{
    class Draw : Engine.Draw
    {
        public const int Draw3dOverlay_Layer = 1;
        public List<Graphics.RenderTargetDraw3dContainer> drawContainers = new List<Graphics.RenderTargetDraw3dContainer>();

        protected override void drawInContainersEvent()
        {
            foreach (var m in drawContainers)
            {
                m.DrawModelsToTarget();
            }
        }

        protected override void drawEvent()
        {
            Ref.draw.AddToContainer = null;
           

            if (toggRef.board.model != null && toggRef.board.model.drawready())
            {
                spriteBatch.GraphicsDevice.Clear(ClrColor);
                Camera.RecalculateMatrices();
                if (toggRef.board.model != null)
                {
                    toggRef.board.model.draw();
                }
                DrawGenerated(0, 0);
                Draw3d(0, 0);
                ParticleHandler.Draw();

                clearDepthBuffer();
                Draw3d(Draw3dOverlay_Layer, 0);
                Draw2d(0);
            }
            else
            {
                spriteBatch.GraphicsDevice.Clear(Color.Black);
            }
        }
    }
}
