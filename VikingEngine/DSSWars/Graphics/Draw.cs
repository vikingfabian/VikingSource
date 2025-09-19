using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.GameState.VoxelEditor;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DeferredRendering;
using VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.Graphics;
using VikingEngine.ToGG.Commander.UnitsData;
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
        
        public const int UnitDetailLayer = 0;
        public const int MidLayer = 1;
        public const int FarLayer = 2;
        public const int WaterEffectLayer = 3;

        ShadowProcessor shadowProcessor = new ShadowProcessor();
        //OceanProcess oceanProcess;
        public DrawGame()
            : base()
        {
            overviewMapTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, MainRenderTarget3D.Width, MainRenderTarget3D.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            drawBatch = new DrawBatchCollection();
        }

        public void initMapShaders()
        {
            //oceanProcess = DssRef.state.detailMap.createOceanProcess();
        }

        public override void OnShaderChange(ShaderChangeType changeType)
        {
            switch (changeType)
            {
                case ShaderChangeType.ShadowMap:
                    shadowProcessor.refreshMapSize();
                    break;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            overviewMapTarget.Dispose();
        }
       
        protected override void drawEvent()
        {
            Viewport saveView = graphicsDeviceManager.GraphicsDevice.Viewport;
            bool hasFadingLayer = false;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            updateLights();

            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
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
                SetRenderTarget(true, overviewMapTarget, Color.White);

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

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(MainRenderTarget3D);
            graphicsDeviceManager.GraphicsDevice.Clear(ClrColor);

            for (int cameraIndex = 0; cameraIndex < ActivePlayerScreens.Count; ++cameraIndex)
            {
                EffectBasicVertexColor.Singleton.basicEffect.DirectionalLight1.DiffuseColor = DssRef.state.localPlayers[cameraIndex].ShaderThemeColor;
                Map.MapLayerManager drawUnits = Map.MapLayerManager.CameraIndexToView[cameraIndex];

                drawDetailLayer(cameraIndex, drawUnits.current, MainRenderTarget3D);
            }


            if (hasFadingLayer)
            { //Draw overview rendertarget
                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;

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
            Draw3DTargetOntoMain();
            Draw2d(0);

            //shadowProcessor.DrawDebug();
        }

        void updateLights()
        {
            if (Ref.gamesett.modelShadow)
            {
                shadowProcessor.light.SunColor = DssRef.time.shadow_SunColor;
                shadowProcessor.light.lightDirection = DssRef.time.shadow_LightDirection;
            }
            else
            {
                EffectBasicVertexColor.Singleton.basicEffect.AmbientLightColor = DssRef.time.ShaderDayLight_Objects;
                Map.MapLayer_Detail.ModelEffect.SetColor(DssRef.time.ShaderDayLight_Map);
            }
        }

        void drawDetailLayer(int cameraIndex, Map.MapLayer lay, RenderTarget2D previousTarget)
        {
            var localPlayer = DssRef.state.localPlayers[cameraIndex];
            Engine.PlayerData p = ActivePlayerScreens[cameraIndex];

            //if (lay.type == Map.MapDetailLayerType.UnitDetail1)
            //{
            //    //DrawShadowMap(cameraIndex, previousTarget);
            //}
            Camera = p.view.Camera;
            graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;

            switch (lay.type)
            {
                case Map.MapDetailLayerType.UnitDetail1:

                    //SHADOW
                    if (Ref.gamesett.modelShadow)
                    {
                        var area = DssRef.state.culling.players[p.localPlayerIndex].GetState().enterArea;
                        shadowProcessor.light.updateScene(Camera, area.Width, area.Height);
                        shadowProcessor.BeginShadowMapPass();
                        {
                            shadowProcessor.DrawRenderListMembersDepthOnly(UnitDetailLayer, DrawObjType.MeshGenerated, cameraIndex);
                            DssRef.state.detailMap.updateAndDraw(true, shadowProcessor.shader, shadowProcessor.light, cameraIndex);
                            drawBatch.DrawDepth(cameraIndex, shadowProcessor.light, shadowProcessor.shader);
                        }
                        graphicsDeviceManager.GraphicsDevice.SetRenderTarget(previousTarget);

                        shadowProcessor.DrawModelsWithShadow(UnitDetailLayer, Graphics.DrawObjType.MeshGenerated, Camera, cameraIndex);
                        DssRef.state.detailMap.drawWithShadow(cameraIndex, Camera, shadowProcessor.shader, shadowProcessor.light);
                        drawBatch.RemoveAndDraw(true, cameraIndex, Camera, shadowProcessor.shader, shadowProcessor.light);
                    }
                    else
                    {
                        DrawGenerated(UnitDetailLayer, cameraIndex);
                        DssRef.state.detailMap.updateAndDraw(false, shadowProcessor.shader, shadowProcessor.light, cameraIndex);
                        drawBatch.RemoveAndDraw(false, cameraIndex, Camera, null, null);
                    }
                    graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    Draw3d(UnitDetailLayer, cameraIndex);
                    //oceanProcess.draw(UnitDetailLayer, Camera, cameraIndex, shadowProcessor.light, shadowProcessor._shadowMap);

                    if (Ref.gamesett.waterFoam)
                    {
                        graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Additive;
                        DssRef.state.detailMap.drawWaterEdges(cameraIndex);
                        //Draw3d(WaterEffectLayer, cameraIndex);
                        WaveXzEffect.GetWaveSingletonSafe().DrawMeshes(WaterEffectLayer, cameraIndex);
                        graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    } 
                    localPlayer.DrawDetalLayer_Mesh(cameraIndex);
                    
                    Engine.ParticleHandler.Draw(p.view.Camera);
                    //Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    
                    break;

                case Map.MapDetailLayerType.TerrainOverview2:
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
                    DssRef.state.detailMap.Update_outOfFocus();
                    DrawGenerated(MidLayer, cameraIndex);
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    Draw3d(MidLayer, cameraIndex);
                    localPlayer.DrawMidLayer_Mesh(cameraIndex);
                    break;

                case Map.MapDetailLayerType.FullOverview4:
                case Map.MapDetailLayerType.FactionColors3:
                    DssRef.state.detailMap.Update_outOfFocus();
                    Draw3d(FarLayer, cameraIndex);
                    break;                    
            }

            
        }

        protected override int renderLayerCount
        {
            get
            {
                return 4;
            }
        }
    }

}
