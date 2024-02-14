using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.PJ.SpaceWar
{
    class Draw : Engine.Draw
    {
        public Draw()
        {
            horizontalSplit = false;
        }

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
                Camera.RecalculateMatrices();

                graphicsDeviceManager.GraphicsDevice.Viewport = ActivePlayerScreens[cameraIndex].view.Viewport;

                graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                DrawGenerated(0, cameraIndex); // game objects

                Draw3d(0, cameraIndex); // other stuff like block splatter etc

            }
            graphicsDeviceManager.GraphicsDevice.Viewport = saveView;
            Draw2d(0);
        }
    }
}
    

