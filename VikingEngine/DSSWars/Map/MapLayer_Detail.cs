using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Detail
    {
        ConcurrentStack<DetailMapTile> tilePool = new ConcurrentStack<DetailMapTile>();
        List<DetailMapTile> tiles;


        List<DetailMapTile> processingTiles_Add = new List<DetailMapTile>(400);
        List<DetailMapTile> processingTiles_Remove = new List<DetailMapTile>(400);
        List<DetailMapTile> synchToRender = new List<DetailMapTile>(400);
        List<DetailMapTile> synchDelete = new List<DetailMapTile>(400);
        public List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(256);

        Graphics.Mesh waterSurface;

        Timer.Basic waterAnimTimer = new Timer.Basic(3000,true);
        int waterFrame = 0;
        double waterMoveCurve = 0;

        public static Graphics.CustomEffect_NoColor ModelEffect = new Graphics.CustomEffect_NoColor("FlatVerticeColor", false);
        

        /// <summary>
        /// Trigger a reload of the map
        /// </summary>

        public bool oneSecondUpdate = false;
        //public bool needReload = false;
        public MapLayer_Detail()
        {
            DssRef.state.detailMap = this;
            tiles = new List<DetailMapTile>(128);

            Graphics.Mesh waterBottom;
            MapLayer_Overview.WaterModel(out waterSurface, out waterBottom, true);
            waterSurface.AddToRender(DrawGame.UnitDetailLayer);
            waterBottom.AddToRender(DrawGame.UnitDetailLayer);

            //ModelEffect.SetColor(Color.Gray.ToVector4());   
            //ModelEffect.TerrainShader();
        }

        public void update()
        {
            if (waterAnimTimer.Update(Ref.DeltaGameTimeMs))
            {
                if (++waterFrame >= DssRef.models.waterTextures.Length)
                { 
                    waterFrame = 0;
                }

                waterSurface.texture = DssRef.models.waterTextures[waterFrame];
            }

            waterMoveCurve += Ref.DeltaGameTimeSec * 0.5f;
            waterSurface.TextureSource.SourceF.X += Ref.DeltaGameTimeSec * -0.05f;
            waterSurface.TextureSource.SourceF.Y = (float)(Math.Sin(waterMoveCurve) * 0.1);

            //while (synchTiles.TryPop(out var tile))
            //{
            //    if (!tile.synchToRender())
            //    {
            //        tilePool.Push(tile);
            //    }
            //}
            if (synchToRender.Count > 0)
            {
                lock (synchToRender)
                {
                    foreach (var m in synchToRender)
                    {
                        m.synchToRender();
                    }

                    synchToRender.Clear();
                }

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

        public void asynchUpdate()
        {           
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
    }
}
