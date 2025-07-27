using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.ToGG.ToggEngine;

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
            Camera.CurrentZoom = 10;
            Camera.Tilt = new Vector2(MathHelper.PiOver2 - 0.6f, MathHelper.PiOver4 + 0.1f);
            Camera.Time_Update(Ref.DeltaTimeMs);
            Camera.FieldOfView = 20f;
            Camera.FarPlane = 400;
            Camera.NearPlane = 0.01f;
            Camera.RecalculateMatrices();
            //Camera.updateBillboard();

            Ref.draw.AddToContainer = null;

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d((int)RenderLayer.Layer2);

           
            DrawGenerated(0, 0);

            Draw3d(0, 0);

            ParticleHandler.Draw(Camera);

            Draw2d(0);
        }
    }
}
