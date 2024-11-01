using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights;
using VikingEngine.EngineSpace.Graphics.DeferredRendering;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars
{
    class DrawMenu : Engine.Draw
    {
        public DrawMenu()
            : base()
        {
        }

        protected override void drawEvent()
        {
            spriteBatch.GraphicsDevice.Clear(Ref.draw.ClrColor);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Draw2d(0);
            Draw2d(1, BlendState.Additive);
        }
        protected override int renderLayerCount
        {
            get
            {
                return 2;
            }
        }
    }

    class DrawGame : Engine.Draw
    {
        RenderTarget2D overviewMapTarget;
        RenderTarget2D shadowMapRenderTarget;
        public const int UnitDetailLayer = 0;
        public const int TerrainLayer = 1;
        public const int MinimapLayer = 2;

        //static Effect depthWriter, shadowEffect;
        //Graphics.ImageAdvanced viewDepth=null;
        public DrawGame()
            : base()
        {
            overviewMapTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, MainRenderTarget.Width, MainRenderTarget.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            //TODO SurfaceFormat.Single
            shadowMapRenderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

            
        }

        //public static void LoadContent()
        //{
        //    depthWriter = Engine.LoadContent.LoadShader("DeferredRenderer\\DepthWriter");//"DeferredRenderer\\DepthWriter");//"ShadowMap");
        //    depthWriter.CurrentTechnique = depthWriter.Techniques[0];

        //    shadowEffect = Engine.LoadContent.LoadShader("VoxelShadows");
        //}

        protected override void drawEvent()
        {
            Viewport saveView = graphicsDeviceManager.GraphicsDevice.Viewport;
            bool hasFadingLayer = false;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;


            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer = false;
                Engine.PlayerData p = ActivePlayerScreens[cameraIndex];
                //p.view.Viewport
                //p.view.Camera.RecalculateMatrices();

                Map.MapDetailLayerManager drawUnits = Map.MapDetailLayerManager.CameraIndexToView[cameraIndex];
                if (drawUnits.prevLayer != null)
                {
                    hasFadingLayer = true;
                }
            }

            //SWADOW
            //DrawShadowMap(0);


            if (hasFadingLayer)
            { //Draw overview to rendertarget
                graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                SetRenderTarget(true, overviewMapTarget, ColorExt.Empty);
                
                for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
                {
                    Map.MapDetailLayerManager drawUnits = Map.MapDetailLayerManager.CameraIndexToView[cameraIndex];
                    if (drawUnits.prevLayer != null)
                    {
                        drawDetailLayer(cameraIndex, drawUnits.prevLayer);
                    }
                }                
            }

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(MainRenderTarget);
            graphicsDeviceManager.GraphicsDevice.Clear(ClrColor);
            
            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                Map.MapDetailLayerManager drawUnits = Map.MapDetailLayerManager.CameraIndexToView[cameraIndex];

                drawDetailLayer(cameraIndex, drawUnits.current);
            }
            

            if (hasFadingLayer)
            { //Draw overview rendertarget
                
                for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
                {
                    Map.MapDetailLayerManager drawUnits = Map.MapDetailLayerManager.CameraIndexToView[cameraIndex];
                    if (drawUnits.prevLayer != null)
                    {
                        Engine.PlayerData p = ActivePlayerScreens[cameraIndex];
                        graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;
                        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, renderList[0].TransformMatrix);
                        spriteBatch.Draw(overviewMapTarget,
                            new Rectangle(-p.view.DrawArea.X, -p.view.DrawArea.Y, Engine.Screen.Width, Engine.Screen.Height),
                            new Color(drawUnits.prevLayer.opacity, drawUnits.prevLayer.opacity, drawUnits.prevLayer.opacity, drawUnits.prevLayer.opacity));
                        spriteBatch.End();
                    }
                }
                
            }


            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer = DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer;
            }

            
            graphicsDeviceManager.GraphicsDevice.Viewport = saveView;
            Draw2d(0);
        }

        void drawDetailLayer(int cameraIndex, Map.DetailLayer lay)
        {
            Engine.PlayerData p = ActivePlayerScreens[cameraIndex];
            Camera = p.view.Camera;
            graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;

            switch (lay.type)
            {
                case Map.MapDetailLayerType.UnitDetail1:
                    
                    DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer = true;
                    
                    
                    DrawGenerated(UnitDetailLayer, cameraIndex);
                    
                    
                    Draw3d(UnitDetailLayer, cameraIndex);
                    Engine.ParticleHandler.Draw();
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    break;

                case Map.MapDetailLayerType.TerrainOverview2:
                    DrawGenerated(TerrainLayer, cameraIndex);
                    Draw3d(TerrainLayer, cameraIndex);
                    break;

                case Map.MapDetailLayerType.FullOverview4:
                case Map.MapDetailLayerType.FactionColors3:
                    Draw3d(MinimapLayer, cameraIndex);
                    break;                    
            }
        }

        //public void DrawShadowMap(int cameraIndex)
        //{
        //    //GraphicsDevice device
        //    graphicsDeviceManager.GraphicsDevice.SetRenderTarget(shadowMapRenderTarget);
        //    graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        //    //graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
        //    graphicsDeviceManager.GraphicsDevice.Clear(Color.Transparent);

        //    Engine.PlayerData p = ActivePlayerScreens[cameraIndex];
        //    Camera = p.view.LightCamera;
        //    //Camera.position = p.view.Camera.position;
        //    Camera.LookTarget = p.view.Camera.LookTarget;
        //    Camera.CurrentZoom = 50;
        //    Camera.instantMoveToTarget();
        //    Camera.RecalculateMatrices();


        //    //Vector3 lightPos = Camera.LookTarget + new Vector3(0, 20, 0);
        //    //Matrix lightView = Matrix.CreateLookAt(Camera.LookTarget,
        //    //            lightPos,
        //    //            Vector3.Up);

        //    //Matrix lightProjection = Matrix.CreateOrthographic(20, 20, 0.1f, 40);
        //    //Matrix lightViewProjection = lightView * lightProjection;

        //    //depthWriter.Parameters["View"].SetValue(lightView);//Camera.ViewMatrix);
        //    //depthWriter.Parameters["Projection"].SetValue(lightProjection);//Camera.Projection);

        //    //depthWriter.Parameters["LightPosition"].SetValue(Camera.position);

        //    depthWriter.Parameters["View"].SetValue(Camera.ViewMatrix);
        //    depthWriter.Parameters["Projection"].SetValue(Camera.Projection);
        //    depthWriter.Parameters["LightPosition"].SetValue(Camera.position);
        //    depthWriter.Parameters["FloatingPointPrecisionModifier"].SetValue(1f);

        //    //DrawGenerated(UnitDetailLayer, cameraIndex);
        //    DrawRenderListMembersDepthOnly(depthWriter, UnitDetailLayer, DrawObjType.MeshGenerated, cameraIndex);


        //    if (viewDepth == null)
        //    { 
        //        viewDepth = new ImageAdvanced(SpriteName.WhiteArea, Engine.Screen.CenterScreen, Engine.Screen.Area.Size * VectorExt.V2Half, ImageLayers.Top0, false);
        //        viewDepth.Texture = shadowMapRenderTarget;
        //        viewDepth.SetFullTextureSource();
        //    }
        //    //DrawModels(device, depthWriter);
        //}

        //public void DrawRenderListMembersDepthOnly(Effect shader, int layer, DrawObjType objType, int cameraIndex)
        //{
        //    SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(objType));
        //    while (counter.Next())
        //    {
        //        Abs3DModel model = counter.sel as Abs3DModel;
        //        if (model != null)
        //        {
        //            model.DrawDeferredDepthOnly(shader, cameraIndex);
        //        }
        //    }
        //}

        //public void DrawGenerated_Shadows(int layer, int cameraIndex)
        //{
        //    graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        //    SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.MeshGenerated));
        //    while (drawList.Next())
        //    {
        //        drawList.sel.Draw(cameraIndex);
        //    }

        //}

        protected override int renderLayerCount
        {
            get
            {
                return 3;
            }
        }
    }

}
