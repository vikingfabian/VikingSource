using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class DetailMapTile
    {
        static readonly IntervalF FoliageCenterRange = 
            IntervalF.FromCenter(0.5f * WorldData.SubTileSz.X, 0.2f * WorldData.SubTileSz.X);

        static readonly Vector2 GrassSize = new Vector2(0.03f, 0.11f) * WorldData.SubTileSz;

        static readonly Vector2 SandSize = new Vector2(0.03f) * WorldData.SubTileSz;

        static readonly IntervalF GrassCenterRange =
            IntervalF.FromCenter(0.5f * WorldData.SubTileSz.X, 0.45f * WorldData.SubTileSz.X);

        const LootFest.VoxelModelName TreeFoliage = LootFest.VoxelModelName.fol_tree_hard;
        const LootFest.VoxelModelName StoneFoliage = LootFest.VoxelModelName.fo_stone1;

        public static List<LootFest.VoxelModelName> LoadModel()
        {
            return new List<LootFest.VoxelModelName>
            {
                TreeFoliage,
                StoneFoliage,
                 LootFest.VoxelModelName.fol_sprout,
            };
        }

        const LoadedTexture Texture = LoadedTexture.SpriteSheet;
        public static readonly Graphics.CustomEffect ModelEffect =
            new Graphics.CustomEffect("FlatVerticeColor", false);

        public IntVector2 pos;
        IVerticeData verticeData;
        Graphics.VoxelModel model;
        List<Foliage> foliage;

        public bool add = true;
        static PcgRandom rnd = new PcgRandom();

        public DetailMapTile(IntVector2 pos)
        {
            this.pos = pos;
            
            var tile = DssRef.world.tileGrid.Get(pos);
            if (tile.heightLevel != Height.DeepWaterHeight)
            {
                polygonBlock(tile);
            }
        }

        void polygonBlock(Tile tile)
        {
            model = new Graphics.VoxelModel(false);
            model.Effect = ModelEffect;
            model.position = WP.ToWorldPos(pos);

#if DEBUG
            model.DebugName = "Detail map tile " + pos.ToString();
#endif

            DssRef.state.detailMap.polygons.Clear();

            Vector2 topLeft = VectorExt.V2NegHalf;
            IntVector2 subTileStart = pos * WorldData.SubTileWidth;

            for (int y = 0; y < WorldData.SubTileWidth; ++y)
            {
                for (int x = 0; x < WorldData.SubTileWidth; ++x)
                {
                    int subX = subTileStart.X + x;
                    int subY = subTileStart.Y + y;

                    rnd.SetSeed(subX * 3 + subY * 11);

                    SubTile subTile = DssRef.world.subTileGrid.Get(subX, subY);
                    Vector2 subTopLeft = new Vector2(topLeft.X + x * WorldData.SubTileSz.X, topLeft.Y + y * WorldData.SubTileSz.Y);
                    
                    block(subTopLeft, ref subTile);

                    surfaceTexture(tile, subTile, subTopLeft);

                    if (subTile.mainTerrain == TerrainMainType.Foil)
                    {
                        Vector3 topCenter = new Vector3(
                            pos.X + subTopLeft.X,
                            subTile.groundY,
                            pos.Y + subTopLeft.Y);
                        createFoliage((TerrainSubFoilType)subTile.subTerrain, subTile.terrainValue, topCenter);
                    }

                    DssRef.world.subTileGrid.Set(
                        subTileStart.X + x, subTileStart.Y + y, 
                        subTile);
                }
            }

            verticeData = PolygonLib.BuildVDFromPolygons(
                new Graphics.PolygonsAndTrianglesColor(DssRef.state.detailMap.polygons, null));

            void block(Vector2 subTopLeft, ref SubTile subTile)
            {
                var top = Graphics.PolygonColor.QuadXZ(
                    subTopLeft,
                    WorldData.SubTileSz, false, subTile.groundY,
                    subTile.mainTerrain == TerrainMainType.Foil ? SpriteName.warsFoliageShadow : SpriteName.WhiteArea_LFtiles,
                    Dir4.N,
                    subTile.color);

                var bottom = top;
                Color bottomCol;

                if (tile.IsLand())
                {
                    bottom.Move(VectorExt.V3FromY(-0.4f));
                    bottomCol = ColorExt.VeryDarkGray;
                }
                else
                {
                    bottom.Move(VectorExt.V3FromY(-0.1f));
                    bottomCol = MapSettings.DeepWaterCol1;
                }
                Graphics.PolygonColor left = new Graphics.PolygonColor(
                    bottom.V1nw.Position, bottom.V3ne.Position,
                    top.V1nw.Position, top.V3ne.Position,
                    SpriteName.WhiteArea_LFtiles, Dir4.N,
                    ColorExt.ChangeBrighness(subTile.color, -5));
                left.V1nw.Color = bottomCol;
                left.V3ne.Color = bottomCol;

                Graphics.PolygonColor right = new Graphics.PolygonColor(
                    top.V0sw.Position, top.V2se.Position,
                    bottom.V0sw.Position, bottom.V2se.Position,
                    SpriteName.WhiteArea_LFtiles, Dir4.N,
                    ColorExt.ChangeBrighness(subTile.color, -5));
                right.V0sw.Color = bottomCol;
                right.V2se.Color = bottomCol;

                Graphics.PolygonColor front = new Graphics.PolygonColor(
                    bottom.V0sw.Position, bottom.V1nw.Position,
                    top.V0sw.Position, top.V1nw.Position,
                    SpriteName.WhiteArea_LFtiles, Dir4.N,
                    ColorExt.ChangeBrighness(subTile.color, -10));
                front.V1nw.Color = bottomCol;
                front.V3ne.Color = bottomCol;


                DssRef.state.detailMap.polygons.Add(top);
                DssRef.state.detailMap.polygons.Add(front);
                DssRef.state.detailMap.polygons.Add(left);
                DssRef.state.detailMap.polygons.Add(right);
            }
        }

        void surfaceTexture(Tile tile, SubTile subTile, Vector2 subTopLeft)
        {
            BiomColor biom = DssRef.map.bioms.colors[(int)tile.biom];
            var col = biom.colors_height[tile.heightLevel];

            Vector3 center = new Vector3(
                subTopLeft.X,
                subTile.groundY,
                subTopLeft.Y);

            
            if (subTile.mainTerrain != TerrainMainType.Foil)
            {
                switch (col.Texture)
                {
                    case SurfaceTextureType.Grass:
                        {
                            int count = rnd.Int(5, 20);
                            for (int i = 0; i < count; ++i)
                            {
                                Vector3 pos = center;
                                pos.X += GrassCenterRange.GetRandom(rnd);
                                pos.Z += GrassCenterRange.GetRandom(rnd);

                                Color bottomCol = ColorExt.ChangeBrighness(subTile.color, 4);
                                Color topCol = bottomCol;

                                double rndCol = rnd.Double();
                                if (rndCol < 0.7)
                                {
                                    topCol = ColorExt.ChangeBrighness(topCol, 6);
                                }
                                else if (rndCol < 0.9)
                                {//Red tint
                                    topCol.R = Bound.Byte(topCol.R + 10);
                                }
                                else
                                {//Yellow tint
                                    topCol.G = Bound.Byte(topCol.G + 8);
                                    topCol.B = Bound.Byte(topCol.B + 8);
                                }


                                Graphics.PolygonColor straw = new PolygonColor();
                                //Bottom left
                                straw.V2se.Position = pos;
                                straw.V2se.Position.X -= GrassSize.X * 0.5f;
                                straw.V3ne.Color = bottomCol;

                                //Bottom right
                                straw.V3ne.Position = straw.V2se.Position;
                                straw.V3ne.Position.X += GrassSize.X;
                                straw.V2se.Color = bottomCol;

                                //Top left
                                straw.V0sw.Position = straw.V2se.Position;
                                straw.V0sw.Position.Y += GrassSize.Y;
                                straw.V1nw.Color = topCol;

                                //Top right
                                straw.V1nw.Position = straw.V3ne.Position;
                                straw.V1nw.Position.Y += GrassSize.Y;
                                straw.V0sw.Color = topCol;

                                straw.setSprite(SpriteName.WhiteArea_LFtiles, Dir4.N);

                                DssRef.state.detailMap.polygons.Add(straw);
                            }
                        }
                        break;
                    case SurfaceTextureType.Sand:
                        {
                            int count = rnd.Int(24, 30);
                            for (int i = 0; i < count; ++i)
                            {
                                Vector2 pos = Vector2.Zero;
                                pos.X = center.X + GrassCenterRange.GetRandom(rnd);
                                pos.Y = center.Z + GrassCenterRange.GetRandom(rnd);
                                
                                Color color = ColorExt.ChangeBrighness(subTile.color, rnd.Int(-6, 20));

                                DssRef.state.detailMap.polygons.Add(
                                    PolygonColor.QuadXZ(pos, SandSize, true,
                                    center.Y + 0.001f, SpriteName.WhiteArea_LFtiles, Dir4.N,
                                    color));
                            }
                        }
                        break;
                }
            }
        }

        void createFoliage(TerrainSubFoilType type, int sizeValue, Vector3 wp)
        {
            wp.X += FoliageCenterRange.GetRandom(rnd);
            wp.Z += FoliageCenterRange.GetRandom(rnd);

            LootFest.VoxelModelName modelName;
            float scale = 0.12f;

            switch (type)
            {
                case TerrainSubFoilType.Stones:
                    modelName = StoneFoliage;
                    break;
                case TerrainSubFoilType.TreeHard:
                    modelName = TreeFoliage;
                    scale = 0.03f + 0.0012f * sizeValue;
                    break;
                case TerrainSubFoilType.TreeHardSprout:
                    modelName = LootFest.VoxelModelName.fol_sprout;
                    scale = 0.05f + 0.01f * sizeValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
            //var modelName = foliageModels[lib.SmallestValue(
            //    rnd.Int(foliageModels.Count),
            //    rnd.Int(foliageModels.Count))];

            //var model = LootFest.LfRef.modelLoad.AutoLoadModelInstance(modelName,
            //        0.12f, 0, false, false);

            //model.position = wp;

            if (foliage == null)
            {
                foliage = new List<Foliage>(8);
            }

#if DEBUG
            model.DebugName = "Map foliage " + model.DebugName;
#endif
            foliage.Add(new Foliage(modelName, rnd.Double(), wp, scale));
        }

        public void synchToRender()
        {
            if (add)
            {
                if (model != null)
                {
                    model.BuildFromVerticeData(verticeData,
                        new List<int> { verticeData.DrawData.numTriangles / 2 },
                        Texture);

                    model.AddToRender(DrawGame.UnitDetailLayer);

                    verticeData = null;
                }

                if (foliage != null)
                {
                    foreach (var m in foliage)
                    {
                        //m.AddToRender(DrawGame.UnitDetailLayer);
                        m.addToRender();
                    }
                }
            }
            else
            {
                DeleteMe();
            }
        }

        public void DeleteMe()
        {
            var tile = DssRef.world.tileGrid.Get(pos);
            
            model?.DeleteMe();
            if (foliage != null)
            {
                foreach (var m in foliage)
                {
                    m.DeleteMe();
                }
                foliage = null;
            }
        }
    }
}
