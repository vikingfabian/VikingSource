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
        ShadowProcessor shadowProcessor = new ShadowProcessor();

        public DrawGame()
            : base()
        {
            overviewMapTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, MainRenderTarget.Width, MainRenderTarget.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            drawBatch = new DrawBatchCollection();
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
            
            EffectBasicVertexColor.Singleton.basicEffect.AmbientLightColor = DssRef.time.ShaderDayLight_Objects;
            Map.MapLayer_Detail.ModelEffect.SetColor(DssRef.time.ShaderDayLight_Map);

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
                    //if (Ref.gamesett.modelShadow)
                    //{
                    //    shadowProcessor.BeginShadowMapPass();
                    //    {
                    //        shadowProcessor.DrawRenderListMembersDepthOnly(UnitDetailLayer, DrawObjType.MeshGenerated, cameraIndex);

                    //    }
                    //    shadowProcessor.EndShadowMapPass();
                    //}
                    //else
                    //{
                        DrawGenerated(UnitDetailLayer, cameraIndex);
                        DssRef.state.detailMap.updateAndDraw(cameraIndex);
                        drawBatch.RemoveAndDraw(cameraIndex);
                    //}
                    Draw3d(UnitDetailLayer, cameraIndex);
                    localPlayer.DrawDetalLayer_Mesh(cameraIndex);
                    
                    Engine.ParticleHandler.Draw(p.view.Camera);
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                    
                    break;

                case Map.MapDetailLayerType.TerrainOverview2:
                    DssRef.state.detailMap.Update_outOfFocus();
                    DrawGenerated(TerrainLayer, cameraIndex);
                    Draw3d(TerrainLayer, cameraIndex);
                    localPlayer.DrawMidLayer_Mesh(cameraIndex);
                    break;

                case Map.MapDetailLayerType.FullOverview4:
                case Map.MapDetailLayerType.FactionColors3:
                    DssRef.state.detailMap.Update_outOfFocus();
                    Draw3d(MinimapLayer, cameraIndex);
                    break;                    
            }
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
