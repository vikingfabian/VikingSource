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
    //interface IDrawLayer
    //{ 
        
    //}

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
        
        public const int UnitDetailLayer = 0;
        public const int TerrainLayer = 1;
        public const int MinimapLayer = 2;

        
        Graphics.ImageAdvanced viewDepth=null;
        EffectVertexColorShadow shadowEffect;
        //static Effect depthWriter;
        public DrawGame()
            : base()
        {
            overviewMapTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, MainRenderTarget.Width, MainRenderTarget.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            //shadowEffect = new EffectVertexColorShadow();
            //TODO SurfaceFormat.Single
            //shadowMapRenderTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

            drawBatch = new DrawBatchCollection();
        }

        //public static void LoadContent()
        //{
        //    depthWriter = Engine.LoadContent.LoadShader("DeferredRenderer\\DepthWriter");
        //    depthWriter.CurrentTechnique = depthWriter.Techniques[0];

        //    //shadowEffect = Engine.LoadContent.LoadShader("VoxelShadows");
        //}

        protected override void drawEvent()
        {
            Viewport saveView = graphicsDeviceManager.GraphicsDevice.Viewport;
            bool hasFadingLayer = false;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            
            EffectBasicVertexColor.Singleton.basicEffect.AmbientLightColor = DssRef.time.ShaderDayLight_Objects;
            Map.MapLayer_Detail.ModelEffect.SetColor(DssRef.time.ShaderDayLight_Map);

            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                //DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer = false;
                Engine.PlayerData p = ActivePlayerScreens[cameraIndex];
                
                Map.MapLayerManager drawUnits = Map.MapLayerManager.CameraIndexToView[cameraIndex];
                if (drawUnits.prevLayer != null)
                {
                    hasFadingLayer = true;
                }
            }

            //DrawShadowMap(0);

            if (hasFadingLayer)
            { //Draw overview to rendertarget
                graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                SetRenderTarget(true, overviewMapTarget, ColorExt.Empty);
                
                for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
                {
                    EffectBasicVertexColor.Singleton.basicEffect.DirectionalLight1.DiffuseColor = DssRef.state.localPlayers[cameraIndex].ShaderThemeColor;

                    Map.MapLayerManager drawUnits = Map.MapLayerManager.CameraIndexToView[cameraIndex];
                    if (drawUnits.prevLayer != null)
                    {
                        drawDetailLayer(cameraIndex, drawUnits.prevLayer, overviewMapTarget);
                    }
                }                
            }

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(MainRenderTarget);
            graphicsDeviceManager.GraphicsDevice.Clear(ClrColor);
            
            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                EffectBasicVertexColor.Singleton.basicEffect.DirectionalLight1.DiffuseColor = DssRef.state.localPlayers[cameraIndex].ShaderThemeColor;
                Map.MapLayerManager drawUnits = Map.MapLayerManager.CameraIndexToView[cameraIndex];

                drawDetailLayer(cameraIndex, drawUnits.current, MainRenderTarget);
            }
            

            if (hasFadingLayer)
            { //Draw overview rendertarget
                
                for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
                {
                    Map.MapLayerManager drawUnits = Map.MapLayerManager.CameraIndexToView[cameraIndex];
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


            //for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            //{
            //    DssRef.state.localPlayers[cameraIndex].bUpdateDetailLayer = DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer;
            //}

            
            graphicsDeviceManager.GraphicsDevice.Viewport = saveView;
            Draw2d(0);
        }

        void drawDetailLayer(int cameraIndex, Map.MapLayer lay, RenderTarget2D previousTarget)
        {
            var localPlayer = DssRef.state.localPlayers[cameraIndex];
            Engine.PlayerData p = ActivePlayerScreens[cameraIndex];

            if (lay.type == Map.MapDetailLayerType.UnitDetail1)
            {
                //DrawShadowMap(cameraIndex, previousTarget);
            }
            Camera = p.view.Camera;
            graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;

            switch (lay.type)
            {
                case Map.MapDetailLayerType.UnitDetail1:

                    //DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer = true;

                    //SHADOW
                    DrawGenerated(UnitDetailLayer, cameraIndex);
                    //DrawGenerated_Shadows(UnitDetailLayer, cameraIndex);
                    DssRef.state.detailMap.updateAndDraw(cameraIndex);
                    drawBatch.RemoveAndDraw(cameraIndex);

                    //DssRef.state.localPlayers[cameraIndex].bUnitDetailLayer_buffer = true;

                    Draw3d(UnitDetailLayer, cameraIndex);
                    localPlayer.DrawDetalLayer(cameraIndex);
                    Engine.ParticleHandler.Draw(p.view.Camera);
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    //time.EndMeasure();
                    break;

                case Map.MapDetailLayerType.TerrainOverview2:
                    DssRef.state.detailMap.Update_outOfFocus();
                    DrawGenerated(TerrainLayer, cameraIndex);
                    Draw3d(TerrainLayer, cameraIndex);
                    localPlayer.DrawMidLayer(cameraIndex);
                    break;

                case Map.MapDetailLayerType.FullOverview4:
                case Map.MapDetailLayerType.FactionColors3:
                    DssRef.state.detailMap.Update_outOfFocus();
                    Draw3d(MinimapLayer, cameraIndex);
                    break;                    
            }
        }

        public void DrawShadowMap(int cameraIndex, RenderTarget2D previousTarget)
        {
            Engine.PlayerData p = ActivePlayerScreens[cameraIndex];

            //GraphicsDevice device
            //graphicsDeviceManager.GraphicsDevice.SetRenderTarget(shadowEffect.shadowMapRenderTarget);

            //graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ////graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            //graphicsDeviceManager.GraphicsDevice.Clear(Color.Transparent);
            ////Camera = p.view.LightCamera;
            //////Camera.position = p.view.Camera.position;
            ////Camera.LookTarget = p.view.Camera.LookTarget;
            ////Camera.CurrentZoom = 50;
            ////Camera.instantMoveToTarget();
            ////Camera.RecalculateMatrices();


            //Vector3 lightPos = p.view.Camera.LookTarget + new Vector3(0, 5, 0);

            ////Matrix lightViewProjection = lightView * lightProjection;
            //Matrix lightView = Matrix.CreateLookAt(lightPos, Camera.LookTarget, Vector3.Forward);
            //float orthoSize = 8f;
            //float zNear = 4f;
            //float zFar = 6f;
            //Matrix lightProjection = Matrix.CreateOrthographic(orthoSize, orthoSize, zNear, zFar);

            //depthWriter.Parameters["View"].SetValue(lightView);
            //depthWriter.Parameters["Projection"].SetValue(lightProjection);
            ////depthWriter.Parameters["LightPosition"].SetValue(lightPos);
            //depthWriter.Parameters["ZNear"].SetValue(zNear);
            //depthWriter.Parameters["ZFar"].SetValue(zFar);
            //depthWriter.Parameters["FloatingPointPrecisionModifier"].SetValue(1f);

            //DrawGenerated(UnitDetailLayer, cameraIndex);









            //shadowEffect.DrawShadowMap(p, cameraIndex);




             graphicsDeviceManager.GraphicsDevice.SetRenderTarget(shadowEffect.shadowMapRenderTarget);
            DrawRenderListMembersDepthOnly(EffectVertexColorShadow.depthWriter, UnitDetailLayer, DrawObjType.MeshGenerated, cameraIndex);
            drawBatch.DrawDepth(cameraIndex, EffectVertexColorShadow.depthWriter);

            if (viewDepth == null)
            {
                viewDepth = new ImageAdvanced(SpriteName.WhiteArea, Engine.Screen.CenterScreen, Engine.Screen.Area.Size * VectorExt.V2Half, ImageLayers.Top0, false);
                viewDepth.Texture = shadowEffect.shadowMapRenderTarget;
                viewDepth.SetFullTextureSource();

                Image bg = new Image(SpriteName.WhiteArea, viewDepth.position, viewDepth.size, ImageLayers.Top1);
            }

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(previousTarget);
            //    //DrawModels(device, depthWriter);
            //}
        }

        public void DrawRenderListMembersDepthOnly(Effect shader, int layer, DrawObjType objType, int cameraIndex)
        {
            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawDeferredDepthOnly(shader, cameraIndex);
                }
            }
        }

        //public void DrawGenerated_Shadows(int layer, int cameraIndex)
        //{
        //    graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        //    SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.MeshGenerated));
        //    while (drawList.Next())
        //    {
        //        drawList.sel.Draw(cameraIndex);
        //    }

        //}
        public void DrawGenerated_Shadows(int layer, int cameraIndex)
        {
            shadowEffect.BeginDrawShadow();

            graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            SpottedArrayCounter<AbsDraw> drawList = new SpottedArrayCounter<AbsDraw>(renderList[layer].GetList(Graphics.DrawObjType.MeshGenerated));
            while (drawList.Next())
            {
                if (drawList.CurrentIndex == 92)
                {
                    lib.DoNothing();
                }
                drawList.sel.DrawShadow(cameraIndex, shadowEffect);
                //drawList.sel.Draw(cameraIndex);
            }
            Engine.Draw.PreviousVertexBuffer = -1;

        }

        protected override int renderLayerCount
        {
            get
            {
                return 3;
            }
        }
    }

}
