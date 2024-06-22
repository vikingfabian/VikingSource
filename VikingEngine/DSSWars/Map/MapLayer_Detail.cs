using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class MapLayer_Detail
    {
        List<DetailMapTile> tiles;
       

        List<DetailMapTile> processingTiles = new List<DetailMapTile>(800);
        List<DetailMapTile> synchTiles = new List<DetailMapTile>(800);
        public List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(256);

        Graphics.Mesh waterSurface;

        Timer.Basic waterAnimTimer = new Timer.Basic(3000,true);
        int waterFrame = 0;
        double waterMoveCurve = 0;

        /// <summary>
        /// Trigger a reload of the map
        /// </summary>
        
        public bool onSecondUpdate = false;
        //public bool needReload = false;
        public MapLayer_Detail()
        {
            DssRef.state.detailMap = this;
            tiles = new List<DetailMapTile>(128);

            Graphics.Mesh waterBottom;
            MapLayer_Overview.WaterModel(out waterSurface, out waterBottom, true);
            waterSurface.AddToRender(DrawGame.UnitDetailLayer);
            waterBottom.AddToRender(DrawGame.UnitDetailLayer);
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

            if (synchTiles.Count > 0)
            {
                lock (synchTiles)
                {
                    foreach (var m in synchTiles)
                    {
                        m.synchToRender();
                    }

                    synchTiles.Clear();
                }
            }
        }

        public void asynchUpdate()
        {
           
            for (int i = tiles.Count - 1; i >= 0; --i)
            {
                var tile = DssRef.world.tileGrid.Get(tiles[i].pos);
                byte render = DssRef.state.culling.cullingStateA ? tile.renderStateA : tile.renderStateB;
                if (render == Culling.NoRender || onSecondUpdate)
                {
                    tile.hasTileInRender = false;
                    tiles[i].add = false;
                    processingTiles.Add(tiles[i]);
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
                                var maptile = new DetailMapTile(loop.Position);
                                processingTiles.Add(maptile);
                                tiles.Add(maptile);
                            }
                        }
                    }
                }
            }

            onSecondUpdate = false;

            lock (synchTiles)
            {
                synchTiles.AddRange(processingTiles);
            }
            processingTiles.Clear();
        }
    }
}
