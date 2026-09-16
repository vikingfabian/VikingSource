using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map.MapLayer
{
    

    class MapLayer_Overview : AbsMapLayer
    {
        public Borders borders;
        public UnitMiniModels unitMiniModels;

        int state_Processing_Sych_Complete = 2;
        MapLayer_Factions factionsMap;
        //Graphics.GeneratedObjColor heightMapModel;
        CrossHeightMap crossHeightMap;
        public bool bRefreshTimer = false;
        public bool bRefreshDataRecieved = false;

        public MapLayer_Overview(MapLayer_Factions factionsMap, CrossHeightMap crossHeightMap)
        {
            this.factionsMap = factionsMap;
            this.crossHeightMap = crossHeightMap;
            Ref.draw.CurrentRenderLayer = DrawGame.MidLayer;

            //createModel(generateTerrain());
            crossHeightMap.init();

            WaterModel(false);
            
            waterBottom.Y = waterSurface.Y - 0.1f;

            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                borders = new Borders();
            }
            Ref.draw.CurrentRenderLayer = 0;

            unitMiniModels = new UnitMiniModels();
        }
        public void update()
        {
            updateWaterTexture();
        }

        public void refresh_async()
        {
            if (bRefreshTimer && bRefreshDataRecieved)
            {
                bRefreshTimer = false;
                bRefreshDataRecieved = false;

                var polygons = crossHeightMap.generateTerrain();
                Ref.update.AddSyncAction(new SyncAction1Arg<List<Graphics.CrossPolygonColor>>(crossHeightMap.createModel, polygons));
            }
        }

        //private List<Graphics.PolygonColor> generateTerrain()
        //{
        //    IntVector2 pos = IntVector2.Zero;

        //    Sprite topTex = Sprite.FromName(SpriteName.WhiteArea_LFtiles);
        //    Sprite sideTex = Sprite.FromName(SpriteName.WhiteArea_LFtiles);

        //    Sprite citytopTex = Sprite.FromName(SpriteName.WhiteArea_LFtiles);
        //    Sprite citysideTex = Sprite.FromName(SpriteName.WhiteArea_LFtiles);

        //    //List<Graphics.PolygonColor> billboards = new List<PolygonColor>();
        //    List<Graphics.PolygonColor> polygons = new List<PolygonColor>();
        //    /*

        //    Vector3 center = Vector3.Zero;
        //    Vector3 nw = Vector3.Zero;
        //    Vector3 ne = Vector3.Zero;
        //    Vector3 sw = Vector3.Zero;
        //    Vector3 se = Vector3.Zero;

        //    Vector3 bbnw, bbne, bbsw, bbse;
        //     //bool[] edge4Dir = new bool[4];
        //     Span<bool> edge4Dir = stackalloc bool[4];

        //    for (pos.Y = 0; pos.Y < DssRef.world.Size.Y; ++pos.Y)
        //    {
        //        center.Z = pos.Y;
        //        nw.Z = pos.Y - 0.5f;
        //        ne.Z = pos.Y - 0.5f;
        //        sw.Z = pos.Y + 0.5f;
        //        se.Z = pos.Y + 0.5f;

        //        for (pos.X = 0; pos.X < DssRef.world.Size.X; ++pos.X)
        //        {
        //            SumTile4_4 tile = DssRef.world.tileGrid.Get(pos);
        //            if (tile.heightLevel != ColorHeight.DeepWaterHeight)
        //            {
        //                Color terrainCol = tile.BiomColor();//DssRef.map.bioms.bioms[(int)tile.biom].Color(tile).Color;
        //                //Tile.TerrainTypes[tile.biom, tile.heightLevel].color;

        //                center.X = pos.X;
        //                nw.X = pos.X - 0.5f;
        //                ne.X = pos.X + 0.5f;
        //                sw.X = pos.X - 0.5f;
        //                se.X = pos.X + 0.5f;

        //                //Height
        //                center.Y = tile.GroundY();
        //                nw.Y = center.Y;
        //                ne.Y = center.Y;
        //                sw.Y = center.Y;
        //                se.Y = center.Y;

        //                for (int i = 0; i < IntVector2.Dir4Array.Length; ++i)
        //                {
        //                    edge4Dir[i] =  DssRef.world.GetTileSafe(pos + IntVector2.Dir4Array[i], out SumTile4_4 n) && tile.heightLevel > n.heightLevel;
        //                }

        //                Sprite imgCoords = topTex;

        //                Vector3[] topVertices = new Vector3[]
        //                    {
        //                        nw,ne,sw,se,
        //                    };

        //                //move out the texture source 
        //                imgCoords.UpdateSourcePolygon(false);

        //                if (tile.heightLevel > ColorHeight.LowerWaterHeight)
        //                {
        //                    float h = Bound.Max(TileSideHeight, center.Y + 0.5f); 

        //                    if (edge4Dir[3]) //west
        //                    {
        //                        polygons.Add(side(nw, sw, sideTex, ColorExt.ChangeBrighness(terrainCol, -5), h));
        //                    }
        //                    if (edge4Dir[1]) //east
        //                    {
        //                        polygons.Add(side(se, ne, sideTex, ColorExt.ChangeBrighness(terrainCol, -5), h));
        //                    }
        //                    if (edge4Dir[2]) //south
        //                    {
        //                        polygons.Add(side(sw, se, sideTex, ColorExt.ChangeBrighness(terrainCol, -10), h));
        //                    }
        //                }

        //                polygons.Add(new Graphics.PolygonColor(
        //                    topVertices,
        //                    imgCoords, terrainCol));


        //            }
        //        }
        //    }
        //    */
        //    //polygons.AddRange(billboards);

        //    return polygons;

        //}

        //void createModel(List<Graphics.PolygonColor> polygons)
        //{
        //    heightMapModel?.DeleteMe();
        //    heightMapModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
        //        polygons, null), LoadedTexture.SpriteSheet, false);
        //    heightMapModel.AddToRender(DrawGame.MidLayer);
        //}


        const float TileSideHeight = 1.5f;
       
        Graphics.PolygonColor side(Vector3 v1, Vector3 v2, Sprite sideTex, Color col, float height)
        {
            
            Vector3 sw = v1;
            sw.Y -= height;

            Vector3 se = v2;
            se.Y -= height;


            return new Graphics.PolygonColor(
                new Vector3[]
                {
                    v1, v2,  sw,se,
                },
                sideTex, col);
        }

        public void runAsyncTask()
        {
            if (state_Processing_Sych_Complete == 2 && DssRef.world.BordersUpdated || StartupSettings.AlwaysRefreshMap)
            {
                DssRef.world.BordersUpdated = false;
                state_Processing_Sych_Complete = 0;

                borders?.quedEvent();
                if (DssRef.state.PlayType() != GameState.PlayStateType.BattleLab)
                {
                    factionsMap.asyncTask();
                }
                state_Processing_Sych_Complete = 1;
            }
        }
        public void HalfSecondUpdate()
        {
            if (DssLib.UpdateBorders && 
                state_Processing_Sych_Complete == 1)
            {
                borders?.SetNewModel();
                state_Processing_Sych_Complete = 2;
            }

            unitMiniModels.update();
        }
        protected override Texture2D[] WaterTex()
        {
            return DssRef.models.seaTextures;
        }

    }
}
