using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
    class Draw : Engine.Draw
    {
        protected override void drawEvent()
        {
            GraphicsDevice.Clear(ClrColor);
            Viewport saveView = GraphicsDevice.Viewport;
            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                Engine.PlayerData p = ActivePlayerScreens[cameraIndex];

                ActivePlayerScreens[cameraIndex].view.Camera.UpdateCamera();

                Engine.Draw.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                Camera = ActivePlayerScreens[cameraIndex].view.Camera;
                Camera.updateBillboard();
                Camera.Draw(true);

                GraphicsDevice.Viewport = ActivePlayerScreens[cameraIndex].view.Viewport;

                SkyDome.X = Camera.Target.X; SkyDome.Z = Camera.Target.Z;
                Draw3d((int)RenderLayer.Layer2, cameraIndex);
                DrawGenerated(0, cameraIndex);

                if (DrawGeneratedMesh)
                {
                    heightmap.Draw(ActivePlayerScreens[cameraIndex].view.DrawAreaPercent, cameraIndex);
                }

                Draw3d(0, cameraIndex);

                //DrawGenerated(1, cameraIndex); //water

                Engine.ParticleHandler.Draw();
                Camera.Draw(false);
            }
            GraphicsDevice.Viewport = saveView;
            Draw2d(0);
        }
    }
}
