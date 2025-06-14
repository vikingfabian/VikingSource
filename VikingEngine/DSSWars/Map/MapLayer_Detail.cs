using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Detail: AbsMapLayer
    {
        ConcurrentStack<DetailMapTile> tilePool = new ConcurrentStack<DetailMapTile>();
        List<DetailMapTile> tiles;

        List<DetailMapTile> processingTiles_Add = new List<DetailMapTile>(400);
        List<DetailMapTile> processingTiles_Remove = new List<DetailMapTile>(400);
        List<DetailMapTile> synchToRender = new List<DetailMapTile>(400);
        int sychToRenderCurrentIndex = 0;
        int MaxSychToRenderCount;
        List<DetailMapTile> synchDelete = new List<DetailMapTile>(400);
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
            tiles = new List<DetailMapTile>(128);

            WaterModel(true);

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

        public void update()
        {
            updateWaterTexture();
            //if (waterAnimTimer.Update(Ref.DeltaGameTimeMs))
            //{
            //    if (++waterFrame >= DssRef.models.waterTextures.Length)
            //    { 
            //        waterFrame = 0;
            //    }

            //    waterSurface.texture = DssRef.models.waterTextures[waterFrame];
            //}

            //waterMoveCurve += Ref.DeltaGameTimeSec * 0.5f;
            //waterSurface.TextureSource.SourceF.X += Ref.DeltaGameTimeSec * -0.05f;
            //waterSurface.TextureSource.SourceF.Y = (float)(Math.Sin(waterMoveCurve) * 0.1);

            if (synchToRender.Count > 0)
            {
                bool addingComplete;
                lock (synchToRender)
                {
                    int end_ex = sychToRenderCurrentIndex + MaxSychToRenderCount;
                    addingComplete = end_ex >= synchToRender.Count;

                    if (addingComplete)
                    {
                        end_ex = synchToRender.Count;
                    }

                    for (; sychToRenderCurrentIndex < end_ex; sychToRenderCurrentIndex++)
                    {
                        synchToRender[sychToRenderCurrentIndex].synchToRender();
                    }

                    if (addingComplete)
                    {
                        sychToRenderCurrentIndex = 0;
                        synchToRender.Clear();
                    }
                }

                if (addingComplete)
                {
                    lock (synchDelete)
                    {
                        foreach (var m in synchDelete)
                        {
                            m.recycle();
                            tilePool.Push(m);
                        }

                        synchDelete.Clear();
                    }
                }
            }
        }

        public void asynchUpdate()
        {
            if (sychToRenderCurrentIndex > 0)
            {
                return;
            }

            for (int i = tiles.Count - 1; i >= 0; --i)
            {
                var tilePos = tiles[i].pos;
                var tile = DssRef.world.tileGrid.Get(tilePos);
                byte render = DssRef.state.culling.cullingStateA ? tile.bits_renderStateA : tile.bits_renderStateB;
                if (render == Culling.NoRender || oneSecondUpdate)
                {
                    tile.hasTileInRender = false;
                    
                    tile.exitRenderTimeStamp_TotSec = Ref.TotalGameTimeSec; 
                    DssRef.world.tileGrid.Set(tilePos, tile);
                    //tiles[i].add = false;
                    processingTiles_Remove.Add(tiles[i]);
                    tiles.RemoveAt(i);                    
                }
            }
                       

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
                                processingTiles_Add.Add(maptile);
                                tiles.Add(maptile);
                            }
                        }
                    }
                }
            }

            oneSecondUpdate = false;

            lock (synchToRender)
            {
                synchToRender.AddRange(processingTiles_Add);
            }
            processingTiles_Add.Clear();

            lock (synchDelete)
            {
                synchDelete.AddRange(processingTiles_Remove);
            }
            processingTiles_Remove.Clear();
        }

        protected override Texture2D[] WaterTex()
        {
            return DssRef.models.waterTextures;
        }
        
    }
}
