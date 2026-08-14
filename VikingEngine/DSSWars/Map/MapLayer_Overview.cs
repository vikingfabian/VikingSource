using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    abstract class AbsMapLayer : Point3D
    {
        Timer.Basic waterAnimTimer = new Timer.Basic(3000, true);
        int waterFrame = 0;
        //int waterEdgeFrame = 0;
        double waterMoveCurve = 0;

        protected Graphics.Mesh waterSurface, waterBottom;
        protected void WaterModel(bool detailLayer)
        {
            //Graphics.Mesh waterBottom;

            var vol = WaterModelVolume();

            waterBottom = new Mesh(LoadedMesh.plane, vol.Position, new Vector3(1f),
                TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.DarkBlue, false);
            waterBottom.Y -= 0.6f;
            waterBottom.Scale = vol.Scale;

            waterSurface = new Mesh(LoadedMesh.plane, vol.Position, new Vector3(1f),
                TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.White,
                false);

            //if (highDetail)
            //{
            waterSurface.texture = WaterTex()[0];
            int repeatCount = detailLayer ? 2 : 1;
            waterSurface.repeatingTextureSource(WaterTex()[1], DssRef.world.Size * repeatCount);
            //}
            //else
            //{
            //    waterSurface.effectType = TextureEffectType.SeaNoise;

            //    waterSurface.Color = WorldData.WaterCol;//new Color(14, 155, 246);
            //    //new Color(4.3f, 48.6f,77.3f);
            //}
            waterSurface.Scale = vol.Scale;
            const float SurfaceTrans = 0.8f;
            waterSurface.Opacity = SurfaceTrans;

            int drawLayer = detailLayer ? DrawGame.UnitDetailLayer : DrawGame.MidLayer;
            //if (!detailLayer)
            {
                waterSurface.AddToRender(drawLayer);
            }
            waterBottom.AddToRender(drawLayer);
            //waterSurface.Visible = true;
        }

        abstract protected Texture2D[] WaterTex();

        public static VectorVolume WaterModelVolume()
        {
            Vector3 surfacePos = new Vector3(DssRef.world.Size.X * 0.5f - 0.5f, Tile.WaterSurfaceY, DssRef.world.Size.Y * 0.5f - 0.5f);
            Vector3 waterScale = new Vector3(DssRef.world.Size.X, 1f, DssRef.world.Size.Y);

            return new VectorVolume(surfacePos, waterScale);
        }

        protected void updateWaterTexture()
        {
            if (waterAnimTimer.Update(Ref.DeltaGameTimeMs))
            {
                if (++waterFrame >= WaterTex().Length)
                {
                    waterFrame = 0;
                }

                //if (++waterEdgeFrame >= DssRef.models.waterEdgeTextures.Length)
                //{ 
                //    waterEdgeFrame = 0;
                //}

                waterSurface.texture = WaterTex()[waterFrame];
            }

            waterMoveCurve += Ref.DeltaGameTimeSec * 0.5f;
            waterSurface.TextureSource.SourceF.X += Ref.DeltaGameTimeSec * -0.05f;
            waterSurface.TextureSource.SourceF.Y = (float)(Math.Sin(waterMoveCurve) * 0.1);
        }

        //public Texture2D waterEdgeTex()
        //{
        //    return DssRef.models.waterEdgeTextures[waterEdgeFrame];
        //}

        #region DRAW

        public override DrawObjType DrawType
        {
            get { return DrawObjType.MeshGenerated; }
        }
        public override void copyAllDataFrom(Graphics.AbsDraw clone)
        {
            throw new NotImplementedException();
        }
        public override Graphics.AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override Color Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void UpdateCulling()
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    class MapLayer_Overview : AbsMapLayer
    {
        public Map.Borders borders;
        public UnitMiniModels unitMiniModels;

        int state_Processing_Sych_Complete = 2;
        Map.MapLayer_Factions factionsMap;
        Graphics.GeneratedObjColor heightMapModel;
        public bool bRefreshTimer = false;
        public bool bRefreshDataRecieved = false;

        public MapLayer_Overview(Map.MapLayer_Factions factionsMap)
        {
            this.factionsMap = factionsMap;
            Ref.draw.CurrentRenderLayer = DrawGame.MidLayer;

            createModel(generateTerrain());

            WaterModel(false);
            
            waterBottom.Y = waterSurface.Y - 0.1f;

            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {
                borders = new Map.Borders();
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

                var polygons = generateTerrain();
                Ref.update.AddSyncAction(new SyncAction1Arg<List<Graphics.PolygonColor>>(createModel, polygons));
            }
        }

        private List<Graphics.PolygonColor> generateTerrain()
        {
            IntVector2 pos = IntVector2.Zero;

            Sprite topTex = Sprite.FromeName(SpriteName.WhiteArea_LFtiles);
            Sprite sideTex = Sprite.FromeName(SpriteName.WhiteArea_LFtiles);

            Sprite citytopTex = Sprite.FromeName(SpriteName.WhiteArea_LFtiles);
            Sprite citysideTex = Sprite.FromeName(SpriteName.WhiteArea_LFtiles);

            //List<Graphics.PolygonColor> billboards = new List<PolygonColor>();
            List<Graphics.PolygonColor> polygons = new List<PolygonColor>();


            Vector3 center = Vector3.Zero;
            Vector3 nw = Vector3.Zero;
            Vector3 ne = Vector3.Zero;
            Vector3 sw = Vector3.Zero;
            Vector3 se = Vector3.Zero;

            Vector3 bbnw, bbne, bbsw, bbse;
             //bool[] edge4Dir = new bool[4];
             Span<bool> edge4Dir = stackalloc bool[4];

            for (pos.Y = 0; pos.Y < DssRef.world.Size.Y; ++pos.Y)
            {
                center.Z = pos.Y;
                nw.Z = pos.Y - 0.5f;
                ne.Z = pos.Y - 0.5f;
                sw.Z = pos.Y + 0.5f;
                se.Z = pos.Y + 0.5f;

                for (pos.X = 0; pos.X < DssRef.world.Size.X; ++pos.X)
                {
                    Tile tile = DssRef.world.tileGrid.Get(pos);
                    if (tile.heightLevel != Height.DeepWaterHeight)
                    {
                        Color terrainCol = tile.BiomColor();//DssRef.map.bioms.bioms[(int)tile.biom].Color(tile).Color;
                        //Tile.TerrainTypes[tile.biom, tile.heightLevel].color;

                        center.X = pos.X;
                        nw.X = pos.X - 0.5f;
                        ne.X = pos.X + 0.5f;
                        sw.X = pos.X - 0.5f;
                        se.X = pos.X + 0.5f;

                        //Height
                        center.Y = tile.GroundY();
                        nw.Y = center.Y;
                        ne.Y = center.Y;
                        sw.Y = center.Y;
                        se.Y = center.Y;

                        for (int i = 0; i < IntVector2.Dir4Array.Length; ++i)
                        {
                            edge4Dir[i] =  DssRef.world.GetTileSafe(pos + IntVector2.Dir4Array[i], out Tile n) && tile.heightLevel > n.heightLevel;
                        }

                        Sprite imgCoords = topTex;

                        Vector3[] topVertices = new Vector3[]
                            {
                                nw,ne,sw,se,
                            };

                        //move out the texture source 
                        imgCoords.UpdateSourcePolygon(false);

                        if (tile.heightLevel > Height.LowerWaterHeight)
                        {
                            float h = Bound.Max(TileSideHeight, center.Y + 0.5f); 

                            if (edge4Dir[3]) //west
                            {
                                polygons.Add(side(nw, sw, sideTex, ColorExt.ChangeBrighness(terrainCol, -5), h));
                            }
                            if (edge4Dir[1]) //east
                            {
                                polygons.Add(side(se, ne, sideTex, ColorExt.ChangeBrighness(terrainCol, -5), h));
                            }
                            if (edge4Dir[2]) //south
                            {
                                polygons.Add(side(sw, se, sideTex, ColorExt.ChangeBrighness(terrainCol, -10), h));
                            }
                        }

                        polygons.Add(new Graphics.PolygonColor(
                            topVertices,
                            imgCoords, terrainCol));


                    }
                }
            }

            //polygons.AddRange(billboards);

            return polygons;

        }

        void createModel(List<Graphics.PolygonColor> polygons)
        {
            heightMapModel?.DeleteMe();
            heightMapModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                polygons, null), LoadedTexture.SpriteSheet, false);
            heightMapModel.AddToRender(DrawGame.MidLayer);
        }


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
            if (state_Processing_Sych_Complete == 2 && DssRef.world.BordersUpdated)
            {
                DssRef.world.BordersUpdated = false;
                state_Processing_Sych_Complete = 0;

                borders?.quedEvent();
                if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
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
