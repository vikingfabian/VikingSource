using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Detail: AbsMapLayer
    {
        AutoResetEvent pauseEvent = new AutoResetEvent(false);
        ConcurrentStack<DetailMapTile> tilePool = new ConcurrentStack<DetailMapTile>();
        SpottedArray<DetailMapTile> tiles;

        //List<DetailMapTile> processingTiles_Add = new List<DetailMapTile>(400);
        //List<DetailMapTile> processingTiles_Remove = new List<DetailMapTile>(400);
        //List<DetailMapTile> synchToRender = new List<DetailMapTile>(400);
        //int sychToRenderCurrentIndex = 0;
        int MaxSychToRenderCount;
        //List<DetailMapTile> synchDelete = new List<DetailMapTile>(400);
        public List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(256);

        //Graphics.Mesh waterSurface;

       

        public static Graphics.CustomEffect_NoColor ModelEffect = new Graphics.CustomEffect_NoColor("FlatVerticeColor", false);
        

        /// <summary>
        /// Trigger a reload of the map
        /// </summary>
        public bool oneSecondUpdate = false;

        public MapLayer_Detail()
        {
            DssRef.state.detailMap = this;
            tiles = new SpottedArray<DetailMapTile>(1024);//new List<DetailMapTile>(128);

            WaterModel(true);

            refreshLoadSpeed();

        }

        public void refreshLoadSpeed()
        {
            switch (Ref.gamesett.MapLoadingSpeed)
            {
                case ThreeOptions.Low:
                    MaxSychToRenderCount = 40;
                    break;
                default:
                    MaxSychToRenderCount = 200;
                    break;
                case ThreeOptions.High:
                    MaxSychToRenderCount = 600;
                    break;

            }
        }

        public void updateAndDraw(int cameraIndex)
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
                    tilesC.sel.model.Draw(cameraIndex);
                } 
            }

            // Signal the thread (each call resumes the thread *once*)
            pauseEvent.Set();
        }

        //public void update()
        //{
        //    updateWaterTexture();
        //    //if (waterAnimTimer.Update(Ref.DeltaGameTimeMs))
        //    //{
        //    //    if (++waterFrame >= DssRef.models.waterTextures.Length)
        //    //    { 
        //    //        waterFrame = 0;
        //    //    }

        //    //    waterSurface.texture = DssRef.models.waterTextures[waterFrame];
        //    //}

        //    //waterMoveCurve += Ref.DeltaGameTimeSec * 0.5f;
        //    //waterSurface.TextureSource.SourceF.X += Ref.DeltaGameTimeSec * -0.05f;
        //    //waterSurface.TextureSource.SourceF.Y = (float)(Math.Sin(waterMoveCurve) * 0.1);

        //    if (synchToRender.Count > 0)
        //    {
        //        bool addingComplete;
        //        lock (synchToRender)
        //        {
        //            int end_ex = sychToRenderCurrentIndex + MaxSychToRenderCount;
        //            addingComplete = end_ex >= synchToRender.Count;

        //            if (addingComplete)
        //            {
        //                end_ex = synchToRender.Count;
        //            }

        //            for (; sychToRenderCurrentIndex < end_ex; sychToRenderCurrentIndex++)
        //            {
        //                synchToRender[sychToRenderCurrentIndex].synchToRender();
        //            }

        //            if (addingComplete)
        //            {
        //                sychToRenderCurrentIndex = 0;
        //                synchToRender.Clear();
        //            }
        //        }

        //        if (addingComplete)
        //        {
        //            lock (synchDelete)
        //            {
        //                foreach (var m in synchDelete)
        //                {
        //                    m.recycle();
        //                    tilePool.Push(m);
        //                }

        //                synchDelete.Clear();
        //            }
        //        }
        //    }
        //}

        public void asynchUpdate()
        {
            //if (sychToRenderCurrentIndex > 0)
            //{
            //    return;
            //}

            var tileC = tiles.counter();
            while (tileC.Next())//for (int i = tiles.Count - 1; i >= 0; --i)
            {
                if (tileC.sel.renderState == DetailMapTileState.InRender)
                {
                    //var tilePos = tiles[i].pos;
                    ref var worldtile = ref DssRef.world.tileGrid.GetRef(tileC.sel.pos);
                    byte render = DssRef.state.culling.cullingStateA ? worldtile.bits_renderStateA : worldtile.bits_renderStateB;
                    if (render == Culling.NoRender || oneSecondUpdate)
                    {
                        worldtile.hasTileInRender = false;
                        worldtile.exitRenderTimeStamp_TotSec = Ref.TotalGameTimeSec;
                        tileC.sel.exitRender = DetailMapTileExitState.Prepare;
                        //processingTiles_Remove.Add(tiles[i]);
                        //tiles.RemoveAt(i);                    
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
                if (DssRef.state.localPlayers[pIx].bUnitDetailLayer)
                {
                    var p = DssRef.state.culling.players[pIx];

                    var state = DssRef.state.culling.cullingStateA ? p.stateA : p.stateB;
                    var loopArea = state.enterArea;
                    loopArea.size += 1;

                    loopArea.SetTileBounds(DssRef.world.tileBounds);

                    if (loopArea.Width > 0 && loopArea.Height > 0)
                    {
                        ForXYLoop loop = new ForXYLoop(loopArea);

                        while (loop.Next())
                        {
                            var tile = DssRef.world.tileGrid.Get(loop.Position);

                            if (!tile.hasTileInRender)
                            {
                                tile.hasTileInRender = true;
                                DssRef.world.tileGrid.Set(loop.Position, tile);

                                DetailMapTile maptile;
                                if (!tilePool.TryPop(out maptile))
                                {
                                    maptile = new DetailMapTile();
                                }
                                //maptile.add = true;
                                maptile.generateModel_async(loop.Position, tile);
                                maptile.renderState = DetailMapTileState.AddToRender;
                               
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

            tileC.Reset();
            while (tileC.Next())
            {
                if (tileC.sel.exitRender == DetailMapTileExitState.Prepare)
                {
                    tileC.sel.exitRender = DetailMapTileExitState.ExitRender;
                }
            }
            //lock (synchToRender)
            //{
            //    synchToRender.AddRange(processingTiles_Add);
            //}
            //processingTiles_Add.Clear();

            //lock (synchDelete)
            //{
            //    synchDelete.AddRange(processingTiles_Remove);
            //}
            //processingTiles_Remove.Clear();
        }

        protected override Texture2D[] WaterTex()
        {
            return DssRef.models.waterTextures;
        }
        
    }
}
