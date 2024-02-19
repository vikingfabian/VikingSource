﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
    class DetailMapTile
    {
        static readonly IntervalF FoliageCenterRange = 
            IntervalF.FromCenter(0.5f * UnitDetailMap.SubTileSz.X, 0.2f * UnitDetailMap.SubTileSz.X);

        static readonly Vector2 GrassSize = new Vector2(0.03f, 0.11f) * UnitDetailMap.SubTileSz;

        static readonly Vector2 SandSize = new Vector2(0.03f) * UnitDetailMap.SubTileSz;

        static readonly IntervalF GrassCenterRange =
            IntervalF.FromCenter(0.5f * UnitDetailMap.SubTileSz.X, 0.45f * UnitDetailMap.SubTileSz.X);

        public const LootFest.VoxelModelName TreeFoliage = LootFest.VoxelModelName.fol_tree_hard;
        public const LootFest.VoxelModelName StoneFoliage = LootFest.VoxelModelName.fo_stone1;

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
            if (tile.heightLevel != Tile.DeepWaterHeight)
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

            DssRef.detailMap.polygons.Clear();

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
                    Vector2 subTopLeft = new Vector2(topLeft.X + x * UnitDetailMap.SubTileSz.X, topLeft.Y + y * UnitDetailMap.SubTileSz.Y);
                    
                    block(subTopLeft, ref subTile);

                    surfaceTexture(tile, subTile, subTopLeft);

                    if (subTile.maintype == SubTileMainType.Foil)
                    {
                        Vector3 topCenter = new Vector3(
                            pos.X + subTopLeft.X,
                            subTile.groundY,
                            pos.Y + subTopLeft.Y);
                        createFoliage((SubTileFoilType)subTile.undertype, topCenter);
                    }

                    DssRef.world.subTileGrid.Set(
                        subTileStart.X + x, subTileStart.Y + y, 
                        subTile);
                }
            }

            verticeData = PolygonLib.BuildVDFromPolygons(
                new Graphics.PolygonsAndTrianglesColor(DssRef.detailMap.polygons, null));

            void block(Vector2 subTopLeft, ref SubTile subTile)
            {
                var top = Graphics.PolygonColor.QuadXZ(
                    subTopLeft,
                    UnitDetailMap.SubTileSz, false, subTile.groundY,
                    subTile.maintype == SubTileMainType.Foil ? SpriteName.warsFoliageShadow : SpriteName.WhiteArea_LFtiles,
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
                    bottomCol = TerrainSettings.DeepWaterCol1;
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


                DssRef.detailMap.polygons.Add(top);
                DssRef.detailMap.polygons.Add(front);
                DssRef.detailMap.polygons.Add(left);
                DssRef.detailMap.polygons.Add(right);
            }
        }

        void surfaceTexture(Tile tile, SubTile subTile, Vector2 subTopLeft)
        {
            TerrainSettings terrain = Tile.TerrainTypes[tile.biom, tile.heightLevel];

            Vector3 center = new Vector3(
                subTopLeft.X,
                subTile.groundY,
                subTopLeft.Y);

            
            if (subTile.maintype != SubTileMainType.Foil)
            {
                switch (terrain.textureType)
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

                                DssRef.detailMap.polygons.Add(straw);
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

                                DssRef.detailMap.polygons.Add(
                                    PolygonColor.QuadXZ(pos, SandSize, true,
                                    center.Y + 0.001f, SpriteName.WhiteArea_LFtiles, Dir4.N,
                                    color));
                            }
                        }
                        break;
                }
            }
        }

        void createFoliage(SubTileFoilType type, Vector3 wp)
        {
            wp.X += FoliageCenterRange.GetRandom(rnd);
            wp.Z += FoliageCenterRange.GetRandom(rnd);

            LootFest.VoxelModelName modelName;

            switch (type)
            {
                case SubTileFoilType.Stones:
                    modelName = StoneFoliage;
                    break;
                case SubTileFoilType.TreeHard:
                    modelName = TreeFoliage;
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
            foliage.Add(new Foliage(modelName, rnd.Double(), wp));
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
