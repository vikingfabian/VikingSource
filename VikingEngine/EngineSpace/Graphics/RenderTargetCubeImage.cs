using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class RenderTargetCubeImage : AbsRenderTargetImage
    {
        public RenderTargetCube target;
        //RenderTarget2D MainRenderTarget;
        //RenderTarget MainRenderTarget = this.GraphicsDevice.GetRenderTarget(0);

        public RenderTargetCubeImage(int size)
            :base(Vector2.Zero, VectorExt.V2(size))
        {
            target = new RenderTargetCube(Engine.Draw.graphicsDeviceManager.GraphicsDevice, size, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public void TestRenderTexture(SpriteName img)
        {
            const int NumFaces = 6;
            List<AbsDraw> drawList = new List<AbsDraw>{
                new Image(img, Vector2.Zero, size, ImageLayers.AbsoluteTopLayer)};

            for (int i = 0; i < NumFaces; i++)
            {
                //SetAsTarget(true, (CubeMapFace)i);
                DrawImagesToTarget(drawList, true);
                //SetAsTarget(false);
            }
            drawList[0].DeleteMe();

        }

        override public void SetAsTarget(bool set, CubeMapFace face)
        {
            if (set)
            {
             
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(target, face);
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.Clear(ClearColor);
            }
            else
            {
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
            }
                
        }
        override protected BlendState blendState { get { return BlendState.NonPremultiplied; } }
        public override void Draw(int cameraIndex)
        {
            throw new NotImplementedException();
        }
        protected override bool drawable
        {
            get
            {
                return false;
            }
        }
    }
}
