using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;

namespace VikingEngine.DSSWars.GameState.ShaderLab
{
    class LabDraw : Engine.Draw
    {
        ShadowProcessor shadowProcessor;
        public LabDraw():base() 
        { 
            ClrColor = ColorExt.VeryDarkGray;
            shadowProcessor = new ShadowProcessor();
            Camera.Tilt = new Vector2(MathHelper.PiOver2 - 0.6f, MathHelper.PiOver4 + 0.1f);
            Camera.CurrentZoom = 20;
        }

        protected override void drawEvent()
        {
            
            
            
            Camera.Time_Update(Ref.DeltaTimeMs);
            Camera.FieldOfView = 20f;
            Camera.FarPlane = 500;
            Camera.NearPlane = 0.01f;
            //Camera.FarPlane = Camera.CurrentZoom + 4;
            //Camera.NearPlane = Camera.CurrentZoom - 10;
            Camera.RecalculateMatrices();

            //Camera.updateBillboard();

            shadowProcessor.light.distance = 300;
            shadowProcessor.light.refresh();


            Ref.draw.AddToContainer = null;

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            Draw2d((int)RenderLayer.Layer2);

            shadowProcessor.BeginShadowMapPass();
            {
                shadowProcessor.DrawRenderListMembersDepthOnly(0, Graphics.DrawObjType.Mesh, 0);
                shadowProcessor.DrawRenderListMembersDepthOnly(0, Graphics.DrawObjType.MeshGenerated, 0);
                
            }
            shadowProcessor.EndShadowMapPass();

            
            //Draw3d(0, 0);
            shadowProcessor.DrawModelsWithShadow(0, Graphics.DrawObjType.Mesh, Camera, 0);

            shadowProcessor.DrawModelsWithShadow(0, Graphics.DrawObjType.MeshGenerated, Camera, 0);

            //ParticleHandler.Draw(Camera);

            Draw2d(0);

            shadowProcessor.DrawDebug();

        }
    }
}
