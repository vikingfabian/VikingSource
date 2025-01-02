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

        public static Graphics.CustomEffect_NoColor ModelEffect = new Graphics.CustomEffect_NoColor("FlatVerticeColor", false);
        //public static Graphics.EffectBasicVertexColor ModelEffect = new EffectBasicVertexColor();



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
                var tilePos = tiles[i].pos;
                var tile = DssRef.world.tileGrid.Get(tilePos);
                //Debug.Log("Tile Get(C) " + tilePos.ToString() + ", " + tile.ToString());
                byte render = DssRef.state.culling.cullingStateA ? tile.bits_renderStateA : tile.bits_renderStateB;
                if (render == Culling.NoRender || onSecondUpdate)
                {
                    tile.hasTileInRender = false;
                    tile.exitRenderTimeStamp_TotSec = Ref.TotalGameTimeSec; 
                    DssRef.world.tileGrid.Set(tilePos, tile);
                    //Debug.Log("Tile Set(C) " + tilePos.ToString() + ", " + tile.ToString());
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
                            //Debug.Log("Tile Get(B) " + loop.Position.ToString() + ", " + tile.ToString());
                            if (!tile.hasTileInRender)
                            {
                                tile.hasTileInRender = true;
                                DssRef.world.tileGrid.Set(loop.Position, tile);
                                var maptile = new DetailMapTile(loop.Position, tile);
                                processingTiles.Add(maptile);
                                tiles.Add(maptile);

                                
                                //Debug.Log("Tile Set(B) " + loop.Position.ToString() + ", " + tile.ToString());
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
