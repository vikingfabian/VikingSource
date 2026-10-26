using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    class MapLayer1_Near: AbsMapLayer
    {
        AutoResetEvent pauseEvent = new AutoResetEvent(false);
        ConcurrentStack<DetailModelTile> tilePool = new ConcurrentStack<DetailModelTile>();
        SpottedArray<DetailModelTile> tiles;

        const int MaxRemoveCount = 32;
        int MaxSychToRenderCount;
        public List<Graphics.PolygonColor> terrainPolygons = new List<Graphics.PolygonColor>(256);
        public List<Graphics.PolygonColor> waterEdgePolygons = new List<Graphics.PolygonColor>(64);

        public static Graphics.CustomEffect_NoColor ModelEffect = new Graphics.CustomEffect_NoColor("FlatVerticeColor", false);

        CrossHeightMap crossHeightMap;
        /// <summary>
        /// Trigger a reload of the map
        /// </summary>
        public bool oneSecondUpdate = false;

        public MapLayer1_Near(CrossHeightMap crossHeightMap)
            : base(DrawGame.NearLayer)
        {
            DssRef.state.mapLayerNear = this;
            tiles = new SpottedArray<DetailModelTile>(1024);

            //WaterModel(DrawGame.UnitDetailLayer);

            refreshLoadSpeed();
            this.crossHeightMap = crossHeightMap;
        }

        //public OceanProcess createOceanProcess()
        //{
        //    return new OceanProcess(waterSurface);
        //}

        public void refreshLoadSpeed()
        {
            switch (Ref.gamesett.MapLoadingSpeed)
            {
                case ThreeOptions.Low:
                    MaxSychToRenderCount = 40;
                    break;
                default:
                    MaxSychToRenderCount = 300;
                    break;
                case ThreeOptions.High:
                    MaxSychToRenderCount = 800;
                    break;

            }
        }

        public void drawCrossMap(int cameraIndex, LocalPlayer player)
        {
            crossHeightMap.Draw(cameraIndex, player, -MapHeight2.HeightY * 28f);
        }

        public void updateAndDraw(bool depth, Effect shader, LightProjection light, int cameraIndex, LocalPlayer player)
        {
            

            updateWaterTexture();

            var tilesC = tiles.counter();
            while (tilesC.Next())
            {
                if (tilesC.sel.exitRender == DetailMapTileExitState.ExitRender)
                {
                    tilesC.sel.DeleteMe();
                }
                else if (tilesC.sel.renderState == DetailMapTileState.AddToRender)
                {
                    tilesC.sel.synchToRender();
                }

                if (tilesC.sel.renderState == DetailMapTileState.InRender)
                {
                    tilesC.sel.model.DrawDepthOnly(depth, shader, light, cameraIndex);
                }
            }

            // Signal the thread (each call resumes the thread *once*)
            pauseEvent.Set();
        }

        public void drawWithShadow(int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            shader.CurrentTechnique = shader.Techniques["RenderVertexColorTexture"];
            var spriteSheet = Engine.LoadContent.Texture(LoadedTexture.SpriteSheet);
            shader.Parameters["Texture"]?.SetValue(spriteSheet);
            shader.Parameters["TerrainTexture"]?.SetValue(spriteSheet);

            var tilesC = tiles.counter();
            while (tilesC.Next())
            {
                if (tilesC.sel.renderState == DetailMapTileState.InRender)
                {
                    tilesC.sel.model.DrawWithShadow(cameraIndex, camera, shader, light);
                }
            }
        }

        public void drawWaterEdges(int cameraIndex)
        {
            //LoadContent.Textures[(int)LoadedTexture.waterEdge] = texture;
            WaveXzEffect.GetWaveSingletonSafe().beginDraw();

            //ModelEffect.SetColor(Vector4.One);

            var tilesC = tiles.counter();
            while (tilesC.Next())
            {
                if (tilesC.sel.renderState == DetailMapTileState.InRender)
                {
                    var model = tilesC.sel.waterEdgeModel;
                    if (model != null)
                    {
                        
                        model.Draw(cameraIndex);
                    }
                }
            }
        }

        public void Update_outOfFocus()
        {
            pauseEvent.Set();
        }

        public void asynchUpdate()
        {
            var world_sp = DssRef.world;
            if (world_sp != null)
            {
                var tileC = tiles.counter();
                while (tileC.Next())
                {
                    if (tileC.sel.renderState == DetailMapTileState.InRender)
                    {
                        ref var chunkData = ref world_sp.chunkGrid.GetRef(tileC.sel.chunkGrindex);
                        byte render = DssRef.state.culling.cullingStateA ? chunkData.bits_renderStateA : chunkData.bits_renderStateB;
                        if (render == Culling.NoRender || (oneSecondUpdate && chunkData.subtileVisualEdits > 0))
                        {
                            chunkData.hasTileInRender = false;
                            chunkData.exitRenderTimeStamp_TotSec = Ref.TotalGameTimeSec;
                            tileC.sel.exitRender = DetailMapTileExitState.Prepare;
                        }
                    }
                    else if (tileC.sel.renderState == DetailMapTileState.None)
                    {
                        tilePool.Push(tileC.sel);
                        tileC.RemoveAtCurrent();
                    }
                }

                int toRenderCount = 0;

                for (int pIx = 0; pIx < DssRef.state.culling.players.Length; ++pIx)
                {
                    if (DssRef.state.localPlayers[pIx].mapLayersManager.DoUpdateDetailLayer())
                    {
                        var p = DssRef.state.culling.players[pIx];

                        var state = DssRef.state.culling.cullingStateA ? p.stateA : p.stateB;
                        Rectangle2 loopArea = state.enterArea / Map.MapData.MapChunkData8_8.TileWidth;
                        loopArea.size += 1;

                        loopArea.SetBounds(world_sp.chunkBounds);

                        if (loopArea.Width > 0 && loopArea.Height > 0)
                        {
                            ForXYLoop loop = new ForXYLoop(loopArea);

                            while (loop.Next())
                            {
                                var tile = world_sp.chunkGrid.Get(loop.Position);

                                if (!tile.hasTileInRender)
                                {
                                    tile.hasTileInRender = true;
                                    tile.subtileVisualEdits = 0;
                                    world_sp.chunkGrid.Set(loop.Position, tile);

                                    DetailModelTile maptile;
                                    if (!tilePool.TryPop(out maptile))
                                    {
                                        maptile = new DetailModelTile();
                                    }
                                    //maptile.add = true;
                                    maptile.generateModel_async(loop.Position, tile);
                                    maptile.renderState = DetailMapTileState.AddToRender;
                                    //maptile.synchToRender();
                                    tiles.Add(maptile);

                                    if (++toRenderCount > MaxSychToRenderCount)
                                    {
                                        pauseEvent.WaitOne(); // Wait until signaled
                                        toRenderCount = 0;
                                    }
                                }
                            }
                        }
                    }
                }

                oneSecondUpdate = false;
                int removeCount = 0;

                tileC.Reset();
                while (tileC.Next())
                {
                    if (tileC.sel.exitRender == DetailMapTileExitState.Prepare)
                    {
                        tileC.sel.exitRender = DetailMapTileExitState.ExitRender;

                        if (++removeCount > MaxRemoveCount)
                        {
                            pauseEvent.WaitOne(); // Wait until signaled
                            removeCount = 0;
                        }
                    }
                }
            }
        }

        protected override Texture2D[] WaterTex()
        {
            return DssRef.models.waterTextures;
        }
        
    }
}
