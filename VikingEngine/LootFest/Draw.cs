using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.LootFest
{
    class LootfestDraw : Engine.Draw
    {
        /* Methods */
        protected override void drawEvent()
        {
            graphicsDeviceManager.GraphicsDevice.Clear(ClrColor);
            Viewport saveView = graphicsDeviceManager.GraphicsDevice.Viewport;
            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                Engine.PlayerData p = ActivePlayerScreens[cameraIndex];

                ActivePlayerScreens[cameraIndex].view.Camera.RecalculateMatrices();
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                
                Camera = ActivePlayerScreens[cameraIndex].view.Camera;
                Camera.updateBillboard();
                Camera.SetPersonVisible(false);

                graphicsDeviceManager.GraphicsDevice.Viewport = ActivePlayerScreens[cameraIndex].view.Viewport;

                graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                if (p.Tag != null)
                { //Draw skybox
                    Players.Player player = p.Tag as Players.Player;
                    player.scenery.Draw(cameraIndex);
                }

                if (DrawGround)
                {
                    heightmap.Draw(ActivePlayerScreens[cameraIndex].view.DrawAreaPercent, cameraIndex); // ground
                }

                DrawGenerated(0, cameraIndex); // game objects

                Draw3d(0, cameraIndex); // other stuff like block splatter etc

                //DrawGenerated(1, cameraIndex); //water

                // TODO(Martin): Fix this code
                if (DebugSett.Debug3DParticles)
                {
                    // TODO(Martin): This only works for in single client mode
                    instancing.Draw(ref Camera.ViewMatrix, ref Camera.Projection);
                }
                // NOTE(Martin): Ends here

                Engine.ParticleHandler.Draw();
                
                Camera.SetPersonVisible(true);
            }
            graphicsDeviceManager.GraphicsDevice.Viewport = saveView;
            Draw2d(0);
        }
    }
}
