using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.GameState.ShaderLab
{
    class LabDraw : Engine.Draw
    {
        public LabDraw():base() 
        { 
            ClrColor = ColorExt.VeryDarkGray;
        }

        protected override void drawEvent()
        {
            Ref.draw.AddToContainer = null;

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d((int)RenderLayer.Layer2);

            Camera.RecalculateMatrices();
            Camera.updateBillboard();
            DrawGenerated(0, 0);

            Draw3d(0, 0);

            ParticleHandler.Draw(Camera);

            Draw2d(0);
        }
    }
}
