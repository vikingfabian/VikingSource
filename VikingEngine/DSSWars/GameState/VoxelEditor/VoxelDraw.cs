using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Engine;

namespace VikingEngine.Core.DSSWars.GameState.VoxelEditor
{
    class VoxelDraw : Engine.Draw
    {
        public VoxelDraw()
            : base()
        {
        }
        protected override void drawEvent()
        {
            Ref.draw.AddToContainer = null;

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d((int)RenderLayer.Layer2);

            Camera.RecalculateMatrices();
            //Camera.updateBillboard();
            DrawGenerated(0, 0);
            DrawGenerated(1, 0);
            Draw3d(0, 0);

            //ParticleHandler.Draw(Camera);

            Draw2d(0);
        }
    }
}
