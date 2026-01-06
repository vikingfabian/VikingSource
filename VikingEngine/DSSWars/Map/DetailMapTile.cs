using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;
using VikingEngine.EngineSpace;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Map.Terrain;
using VikingEngine.ToGG.HeroQuest.GO;

namespace VikingEngine.DSSWars.Map
{
   
    class DetailMapTile
    {
        static readonly IntervalF FoliageCenterRange = 
            IntervalF.FromCenter(0.5f * WorldData.SubTileWidth, 0.2f * WorldData.SubTileWidth);

        static readonly Vector2 GrassSize = new Vector2(0.03f, 0.11f) * WorldData.SubTileWidth;
        static readonly Vector2 TuftSize = new Vector2(0.08f, 0.25f) * WorldData.SubTileWidth;

        static readonly Vector2 SandSize = new Vector2(0.03f) * WorldData.SubTileWidth;

        static readonly IntervalF GrassCenterRange =
            IntervalF.FromCenter(0.5f * WorldData.SubTileWidth, 0.45f * WorldData.SubTileWidth);
        static readonly IntervalF GrassTuftCenterRange =
                    IntervalF.FromCenter(0.4f * WorldData.SubTileWidth, 0.35f * WorldData.SubTileWidth);

        //static ConcurrentStack<FoliageModel> foliagePool = new ConcurrentStack<FoliageModel>();

        public static List<LootFest.VoxelModelName> LoadModel()
        {
            return new List<LootFest.VoxelModelName>
            {
                LootFest.VoxelModelName.fol_tree_hard,
                LootFest.VoxelModelName.fol_tree_soft,
                LootFest.VoxelModelName.fol_tree_dry,
                LootFest.VoxelModelName.tree_apple,
                LootFest.VoxelModelName.tree_banana,
                LootFest.VoxelModelName.fol_tree_hard_lava,
                LootFest.VoxelModelName.fol_tree_soft_lava,
                LootFest.VoxelModelName.fol_tree_hard_snow,
                LootFest.VoxelModelName.fol_tree_soft_snow,

                LootFest.VoxelModelName.fo_stone1,
                LootFest.VoxelModelName.fol_sprout,
                LootFest.VoxelModelName.fol_tallgrass,
                LootFest.VoxelModelName.fol_herbs,
                LootFest.VoxelModelName.fol_bush1,
                LootFest.VoxelModelName.fol_stoneblock,
                LootFest.VoxelModelName.fol_farmculture,
                LootFest.VoxelModelName.fol_farmculture2,
                LootFest.VoxelModelName.fol_greenfoliage,

                LootFest.VoxelModelName.resource_tree,
                LootFest.VoxelModelName.resource_rubble,

            };
        }

        const LoadedTexture Texture = LoadedTexture.SpriteSheet;
        
           
        public IntVector2 pos;
        VerticeDataColorTexture verticeData;
        VerticeDataColorTexture waterEdgeVerticeData;
        public Graphics.VoxelModel model = new Graphics.VoxelModel(false);
        public Graphics.VoxelModel waterEdgeModel = new Graphics.VoxelModel(false) { colorAndAlpha = new Vector4(1, 1, 1, WaterEdgeBuilder.Opacity) };
        StructList<FoliageModel> foliageModels = new StructList<FoliageModel>(32);
        List<AnimalData> animalData;
        bool hasPolygons;

        static PcgRandom rnd = new PcgRandom();

        public DetailMapTileState renderState = DetailMapTileState.None;
        public DetailMapTileExitState exitRender =  DetailMapTileExitState.None;
        public DetailMapTile()
        {            
            model.Effect = MapLayer_Detail.ModelEffect;
            model.Visible = false;

            waterEdgeModel.Effect = WaveXzEffect.GetWaveSingletonSafe();
            waterEdgeModel.Visible = false;
        }
        
        public void generateModel_async(IntVector2 pos, Tile tile)
        {
            this.pos = pos;
            hasPolygons = tile.heightLevel != Height.DeepWaterHeight;

            if (hasPolygons)
            {
                model.position = WP.ToWorldPos(pos);
                waterEdgeModel.position = model.position;
                waterEdgeModel.PositionY = Tile.WaterFoamY;

#if DEBUG
                model.DebugName = "Detail map tile " + pos.ToString();
                waterEdgeModel.DebugName = "Detail map - water edge" + pos.ToString();
#endif

                DssRef.state.detailMap.terrainPolygons.Clear();

                Vector2 topLeft = VectorExt.V2NegHalf;
                IntVector2 subTileStart = pos * WorldData.TileSubDivitions;
                Biom biom = DssRef.map.bioms.bioms[(int)tile.biom];
                var col = biom.colors_height[tile.heightLevel];

                for (int y = 0; y < WorldData.TileSubDivitions; ++y)
                {
                    for (int x = 0; x < WorldData.TileSubDivitions; ++x)
                    {
                        int subX = subTileStart.X + x;
                        int subY = subTileStart.Y + y;

                        rnd.SetSeed(subX * 3 + subY * 11);

                        SubTile subTile = DssRef.world.subTileGrid.Get(subX, subY);
                        Vector2 subTopLeft = new Vector2(topLeft.X + x * WorldData.SubTileWidth, topLeft.Y + y * WorldData.SubTileWidth);

                        bool bSurfacePolygonTexture = true;
                        SurfaceTextureType surfacePolygonTexture = col.Texture;
                        SpriteName surfaceSprite = SpriteName.WhiteArea_LFtiles;
                        
                        Color surfaceColor = subTile.color;

                        switch (subTile.mainTerrain)
                        {
                            case TerrainMainType.Destroyed:
                                surfacePolygonTexture = SurfaceTextureType.Sand;
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.2f);
                                break;

                            case TerrainMainType.Foil:
                                bSurfacePolygonTexture = false;
                                
                                createFoliage((TerrainSubFoilType)subTile.subTerrain, subTile.terrainAmount,
                                    topCenter(ref subTile, ref subTopLeft), ref surfaceSprite, biom, out bool manMade);
                                if (manMade)
                                {
                                    surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.2f);
                                }
                                break;
                            case TerrainMainType.Resourses:
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.2f);
                                createResoursePile((TerrainResourcesType)subTile.subTerrain,
                                    topCenter(ref subTile, ref subTopLeft));
                                break;
                            case TerrainMainType.Building:
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.3f);
                                bSurfacePolygonTexture = false;
                                createBuilding(tile, ref subTile, (TerrainBuildingType)subTile.subTerrain,
                                    topCenter(ref subTile, ref subTopLeft), ref surfaceColor);
                                break;
                            case TerrainMainType.Wall:
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.1f);
                                createWall(tile, ref subTile, (TerrainWallType)subTile.subTerrain,
                                    topCenter(ref subTile, ref subTopLeft), ref surfaceColor);
                                break;
                            case TerrainMainType.Mine:
                                bSurfacePolygonTexture = false;
                                createMine((TerrainMineType)subTile.subTerrain,
                                    topCenter(ref subTile, ref subTopLeft));
                                break;
                            case TerrainMainType.Road:
                                bSurfacePolygonTexture = false;
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.3f);
                                createRoad((TerrainRoadType)subTile.subTerrain, ref surfaceSprite, ref surfaceColor);
                                break;
                            case TerrainMainType.Decor:
                                surfaceColor = ColorExt.Mix(biom.mudColor, surfaceColor, 0.2f);
                                bSurfacePolygonTexture = false;
                                createDecor(tile, ref subTile, (TerrainDecorType)subTile.subTerrain,
                                    topCenter(ref subTile, ref subTopLeft), ref bSurfacePolygonTexture, ref surfacePolygonTexture, ref surfaceColor);
                                break;
                        }
#if DEBUG
                        //if (surfaceColor == ColorExt.Empty)
                        //{
                        //    throw new Exception("Empty col");
                        //}
#endif
                        block(subTopLeft, surfaceSprite, surfaceColor, ref subTile);

                        if (bSurfacePolygonTexture)
                        {
                            surfaceTexture(tile, subTile, subTopLeft, surfaceColor, surfacePolygonTexture);
                        }

                        //DssRef.world.subTileGrid.Set(
                        //    subTileStart.X + x, subTileStart.Y + y,
                        //    subTile);
                    }
                }


                verticeData = PolygonLib.BuildVDFromPolygons(
                    new Graphics.PolygonsAndTrianglesColor(DssRef.state.detailMap.terrainPolygons, null));

                if (tile.IsLand())
                {
                    generateWaterEdge_async(pos, tile);
                }
                

                void block(Vector2 subTopLeft, SpriteName texture, Color color, ref SubTile subTile)
                {
                    var top = Graphics.PolygonColor.QuadXZ(
                        subTopLeft,
                        WorldData.SubTileWidthV2, false, subTile.groundY,
                        texture,
                        Dir4.N,
                        color);

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
                        ColorExt.ChangeBrighness(color, -5));
                    left.V1nw.Color = bottomCol;
                    left.V3ne.Color = bottomCol;

                    Graphics.PolygonColor right = new Graphics.PolygonColor(
                        top.V0sw.Position, top.V2se.Position,
                        bottom.V0sw.Position, bottom.V2se.Position,
                        SpriteName.WhiteArea_LFtiles, Dir4.N,
                        ColorExt.ChangeBrighness(color, -5));
                    right.V0sw.Color = bottomCol;
                    right.V2se.Color = bottomCol;

                    Graphics.PolygonColor front = new Graphics.PolygonColor(
                        bottom.V0sw.Position, bottom.V1nw.Position,
                        top.V0sw.Position, top.V1nw.Position,
                        SpriteName.WhiteArea_LFtiles, Dir4.N,
                        ColorExt.ChangeBrighness(color, -10));
                    front.V1nw.Color = bottomCol;
                    front.V3ne.Color = bottomCol;


                    DssRef.state.detailMap.terrainPolygons.Add(top);
                    DssRef.state.detailMap.terrainPolygons.Add(front);
                    DssRef.state.detailMap.terrainPolygons.Add(left);
                    DssRef.state.detailMap.terrainPolygons.Add(right);
                }

               
            }
        }

        public void generateWaterEdge_async(IntVector2 pos, Tile tile)
        {
            //const float WaveModelWidth = 0.25f;
            //const float WaveModelHalSize = WaveModelWidth * 0.5f;

            DssRef.state.detailMap.waterEdgePolygons.Clear();

            for (Dir4 dir = 0; dir < Dir4.NUM_NON; ++dir)//each (var dir in IntVector2.Dir4Array)
            {
                
                if (DssRef.world.tileGrid.TryGet(pos + IntVector2.Dir4Array[(int)dir], out var nTile) && nTile.IsWater())
                {
                    //Vector2 center = dirVec.Vec * (0.5f + WorldData.SubTileHalfWidth);

                    //var top = Graphics.PolygonColor.QuadXZ(
                    //   center - new Vector2(WorldData.SubTileHalfWidth),
                    //   WorldData.SubTileWidthV2, false, 0,
                    //   SpriteName.WarsResource_Food,
                    //   dir,
                    //   Color.White);
                    
                    DssRef.state.detailMap.waterEdgePolygons.AddRange(WaterEdgeBuilder.Get(dir));
                }
            }

            if (DssRef.state.detailMap.waterEdgePolygons.Count > 0)
            {
                waterEdgeVerticeData = PolygonLib.BuildVDFromPolygons(
                   new Graphics.PolygonsAndTrianglesColor(DssRef.state.detailMap.waterEdgePolygons, null));
            }
        }

        Vector3 topCenter(ref SubTile subTile, ref Vector2 subTopLeft)
        {
            return new Vector3(
                 pos.X + subTopLeft.X,
                 subTile.groundY,
                 pos.Y + subTopLeft.Y);
        }

        void surfaceTexture(Tile tile, SubTile subTile, Vector2 subTopLeft, Color tileColor, SurfaceTextureType textureType)
        {
            
            Vector3 center = new Vector3(
                subTopLeft.X,
                subTile.groundY,
                subTopLeft.Y);

            
          
            switch (textureType)
            {
                case SurfaceTextureType.Grass:
                    {
                        bool tuft = rnd.Chance(0.06);
                        IntervalF centerRange;
                        Vector2 sz;
                        if (tuft)
                        {
                            centerRange = GrassTuftCenterRange;
                            sz = TuftSize;
                        }
                        else
                        {
                            centerRange = GrassCenterRange;
                            sz = GrassSize;

                            if (rnd.Chance(0.01))
                            {
                                foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_greenfoliage, rnd, VectorExt.AddXZ(center, pos.X + WorldData.SubTileHalfWidth, pos.Y + WorldData.SubTileHalfWidth), 0.12f));
                            }
                        }


                        int count = rnd.Int(5, 20);
                        for (int i = 0; i < count; ++i)
                        {
                            Vector3 pos = center;
                            pos.X += GrassCenterRange.GetRandom(rnd);
                            pos.Z += GrassCenterRange.GetRandom(rnd);

                            Color bottomCol = ColorExt.ChangeBrighness(tileColor, 4);
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
                            straw.V2se.Position.X -= sz.X * 0.5f;
                            straw.V3ne.Color = bottomCol;

                            //Bottom right
                            straw.V3ne.Position = straw.V2se.Position;
                            straw.V3ne.Position.X += sz.X;
                            straw.V2se.Color = bottomCol;

                            //Top left
                            straw.V0sw.Position = straw.V2se.Position;
                            straw.V0sw.Position.Y += sz.Y;
                            straw.V1nw.Color = topCol;

                            //Top right
                            straw.V1nw.Position = straw.V3ne.Position;
                            straw.V1nw.Position.Y += sz.Y;
                            straw.V0sw.Color = topCol;

                            straw.setSprite(SpriteName.WhiteArea_LFtiles, Dir4.N);

                            DssRef.state.detailMap.terrainPolygons.Add(straw);
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
                                
                            Color color = ColorExt.ChangeBrighness(tileColor, rnd.Int(-6, 20));

                            DssRef.state.detailMap.terrainPolygons.Add(
                                PolygonColor.QuadXZ(pos, SandSize, true,
                                center.Y + 0.001f, SpriteName.WhiteArea_LFtiles, Dir4.N,
                                color));
                        }
                    }
                    break;
            }
            
        }


        //FoliageModel newFoliage()
        //{
        //    FoliageModel result;
        //    if (!foliagePool.TryPop(out result))
        //    {
        //        result = new FoliageModel();
        //    }
        //    foliageModels.Add(result);
        //    return result;
        //}

        void createRoad(TerrainRoadType type, ref SpriteName surfaceSprite, ref Color surfaceColor)
        {
            //surfaceColor.Deconstruct(out byte r, out byte g, out byte b);
            //surfaceColor = new Color(r + 30, g + 30, b + 20);
            surfaceSprite = SpriteName.warsFoliageDirtRoad;
        }

        void createFoliage(TerrainSubFoilType type, int sizeValue, Vector3 wp, ref SpriteName surfaceSprite, Biom biom, out bool manMade)
        {
            wp.X += FoliageCenterRange.GetRandom(rnd);
            wp.Z += FoliageCenterRange.GetRandom(rnd);

            switch (type)
            {
                case TerrainSubFoilType.TallGrass:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_tallgrass, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.StoneBlock:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_stoneblock, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Bush:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_bush1, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Herbs:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_herbs, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Stones:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fo_stone1, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.TreeHard:
                    manMade = false;
                    surfaceSprite = SpriteName.warsFoliageRoundShadow;
                    foliageModels.Add(new FoliageModel(biom.treeHard, rnd, wp, 0.03f + 0.0012f * sizeValue));
                    break;
                case TerrainSubFoilType.TreeSoft:
                    manMade = false;
                    surfaceSprite = SpriteName.warsFoliageRoundShadow;
                    foliageModels.Add(new FoliageModel(biom.treeHard, rnd, wp, 0.03f + 0.0012f * sizeValue));
                    break;

                case TerrainSubFoilType.TreeApple:
                    manMade = true;
                    orchid(LootFest.VoxelModelName.tree_apple);
                    break;
                case TerrainSubFoilType.TreeBanana:
                    manMade = false;
                    orchid(LootFest.VoxelModelName.tree_banana);
                    break;

                case TerrainSubFoilType.DryWood:
                    manMade = false;
                    surfaceSprite = SpriteName.warsFoliageRoundShadow;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_tree_dry, rnd, wp, 0.12f));
                    break;

                case TerrainSubFoilType.TreeSoftSprout:
                case TerrainSubFoilType.TreeHardSprout:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.fol_sprout, rnd, wp, 0.05f + 0.01f * sizeValue));
                    break;

                case TerrainSubFoilType.WheatFarm:
                    manMade = true;
                    farm(3, false);                    
                    break;
                case TerrainSubFoilType.LinenFarm:
                    manMade = true;
                    farm(4, false);                    
                    break;
                case TerrainSubFoilType.HempFarm:
                    manMade = true;
                    farm(6, false);
                    break;
                case TerrainSubFoilType.RapeSeedFarm:
                    manMade = true;
                    farm(5, false);
                    break;

                case TerrainSubFoilType.WheatFarmUpgraded:
                    manMade = true;
                    farm(3, true);
                    break;
                case TerrainSubFoilType.LinenFarmUpgraded:
                    manMade = true;
                    farm(4, true);
                    break;
                case TerrainSubFoilType.HempFarmUpgraded:
                    manMade = true;
                    farm(6, true);
                    break;
                case TerrainSubFoilType.RapeSeedFarmUpgraded:
                    manMade = true;
                    farm(5, true);
                    break;

                case TerrainSubFoilType.BogIron:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 3, wp, 0.14f));
                    break;
                case TerrainSubFoilType.ClayPit:
                    manMade = false;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 10, wp, 0.14f));
                    break;
                case TerrainSubFoilType.SaltPit:
                    manMade = true;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 12, wp, 0.14f));
                    break;
                default:
                    throw new NotImplementedException();
            }

            void orchid(LootFest.VoxelModelName model)
            {
                int frame = 0;

                if (sizeValue >= TerrainContent.OrchardReady)
                {
                    frame = 5;
                }
                else if (sizeValue == TerrainContent.OrchardPlucked)
                {
                    frame = 3;
                }
                else if (sizeValue < TerrainContent.OrchardPlucked)
                {
                    frame = MathExt.MultiplyInt((double)sizeValue / TerrainContent.OrchardPlucked, 3.0);
                }
                else
                {
                    frame = 4;
                }
                foliageModels.Add(new FoliageModel(model, frame, wp, 0.1f));
            }

            void farm(int readyFrame, bool upgraded)
            {
                int frame = TerrainContent.FarmCulture_Empty;
                if (sizeValue >= TerrainContent.FarmCulture_ReadySize)
                {
                    frame = readyFrame;
                }
                else if (sizeValue >= TerrainContent.FarmCulture_HalfSize)
                {
                    frame = 2;
                }
                else if (sizeValue > TerrainContent.FarmCulture_Empty)
                {
                    frame = 1;
                }
                foliageModels.Add(new FoliageModel(upgraded ? LootFest.VoxelModelName.fol_farmculture2 : LootFest.VoxelModelName.fol_farmculture, frame, wp, 0.1f));
            }
            
        }

        void createWall(Tile tile, ref SubTile subTile, TerrainWallType buildingType, Vector3 wp, ref Color surfaceColor)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;

            const float WallSize = 1.6f;
            surfaceColor = ColorExt.ChangeBrighness(surfaceColor, -30);

            switch (buildingType)
            {
                case TerrainWallType.Palisade:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_palisade, 0, wp, WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.DirtWall:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_dirtwall, 0, VectorExt.AddY(wp, -0.02f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.DirtTower:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_dirttower, 0, VectorExt.AddY(wp, -0.02f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.WoodWall:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_woodwall, 0, VectorExt.AddY(wp, -0.02f), WorldData.SubTileWidth * 1.5f));
                    break;
                case TerrainWallType.WoodTower:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_woodtower, 0, VectorExt.AddY(wp, -0.02f), WorldData.SubTileWidth * 1.5f));
                    break;
                case TerrainWallType.StoneWall:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 0, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.StoneTower:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonetower, 0, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.StoneWallGreen:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 1, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.StoneWallBlueRoof:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 2, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.StoneWallWoodHouse:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 3, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;
                case TerrainWallType.StoneGate:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 5, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * 1.5f));
                    break;
                case TerrainWallType.StoneHouse:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonewall, 6, VectorExt.AddY(wp, -0.03f), WorldData.SubTileWidth * WallSize));
                    break;

                default:
                    throw new NotImplementedException();

            }

        }

        static readonly Color SquareGroundCol = new Color(102,102,115);
        void createBuilding(Tile tile, ref SubTile subTile, TerrainBuildingType buildingType, Vector3 wp, ref Color surfaceColor)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;

            switch (buildingType)
            {
                case TerrainBuildingType.PigPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Pig, TerrainContent.PigGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.HenPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Hen, TerrainContent.HenGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.OxenPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Oxen, TerrainContent.OxenGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.KineOxenPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.KineOxen, TerrainContent.KineOxenGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.DogCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Dog, TerrainContent.DogGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.HoundCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Hound, TerrainContent.HoundGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.PonyPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Pony, TerrainContent.PonyGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.HorsePen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Horse, TerrainContent.HorseGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WarHorsePen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WarHorse, TerrainContent.WarHorseGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.DraftHorsePen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.DraftHorse, TerrainContent.DraftHorseGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.WildPigPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WildPig, TerrainContent.WildPigGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WildHogPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WildHog, TerrainContent.WildHogGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WarHogPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WarHog, TerrainContent.WarHogGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.StagHogPen:
                    animals(tile, ref subTile, ref wp, ItemResourceType.StagHog, TerrainContent.StagHogGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.WolfCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Wolf, TerrainContent.WolfGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WargCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Warg, TerrainContent.WargGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.AlphaWargCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.AlphaWarg, TerrainContent.AlphaWargGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.WildCatCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WildCat, TerrainContent.WildCatGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.LionCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Lion, TerrainContent.LionGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WarLionCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WarLion, TerrainContent.WarLionGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.ElephantCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Elephant, TerrainContent.ElephantGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WarElephantCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.WarElephant, TerrainContent.WarElephantGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.OliphantCage:
                    animals(tile, ref subTile, ref wp, ItemResourceType.Oliphant, TerrainContent.OliphantGrowth);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.WorkerTent:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_tenthut, rnd, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.WorkerHut:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workerhut, rnd, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.WorkerHutLarge:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workerhut_long, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
               
                case TerrainBuildingType.GuardHouse_Small:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_guard_house, 0, wp, WorldData.SubTileWidth * 1.0f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(faction, 7, wp + new Vector3(WorldData.SubTileWidth * 0.22f, 0.002f, -0.004f), WorldData.SubTileWidth * 0.8f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.GuardHouse_Large:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_guard_house, 1, wp, WorldData.SubTileWidth * 1.0f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 7, wp + new Vector3(WorldData.SubTileWidth * 0.22f, 0.002f, -0.004f), WorldData.SubTileWidth * 0.8f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.Tavern:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_tavern, rnd, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Storehouse:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storehouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.Postal:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 0, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.PostalLevel2:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 1, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.PostalLevel3:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 2, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Recruitment:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 3, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.RecruitmentLevel2:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 4, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.RecruitmentLevel3:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 5, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.SoldierBarracks:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 1, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.ArcherBarracks:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 2, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WarmachineBarracks:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 3, wp, WorldData.SubTileWidth * 1f));
                    break;
                //case TerrainBuildingType.KnightsBarracks:
                //    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 4, wp, WorldData.SubTileWidth * 1f));
                //    break;
                case TerrainBuildingType.GunBarracks:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 5, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.CannonBarracks:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_barracks, 6, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.CityHall_Unclaimed:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonehall, 0, wp, WorldData.SubTileWidth * 1.4f));
                    }
                    break;
                case TerrainBuildingType.CityHall_Tent:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonehall, 1, wp, WorldData.SubTileWidth * 1.4f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 8, wp + new Vector3(0.013f, -0.008f, 0.07f), WorldData.SubTileWidth * 1.1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.CityHall_Village:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonehall, 2, wp, WorldData.SubTileWidth * 1.4f));
                    
                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 8, wp + new Vector3(0.013f, -0.020f, 0.07f), WorldData.SubTileWidth * 1.1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.CityHall_Town:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonehall, 3, wp, WorldData.SubTileWidth * 1.4f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 8, wp + new Vector3(0.013f, -0.025f, 0.07f), WorldData.SubTileWidth * 1.2f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.CityHall_Capital:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_stonehall, 4, wp, WorldData.SubTileWidth * 1.4f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 8, wp + new Vector3(0.012f, 0.002f, 0.07f), WorldData.SubTileWidth * 1.2f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;
                case TerrainBuildingType.ServiceMenHouse_small:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_smallhouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.ServiceMenHouse_Large:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_bighouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                
                case TerrainBuildingType.Work_Cook:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 1, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.Work_Bench:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 3, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.Work_CoalPit:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 4, wp, WorldData.SubTileWidth * 0.9f));
                    break;

                case TerrainBuildingType.Work_Smith:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 0, wp, WorldData.SubTileWidth * 1.2f));
                    break;
                case TerrainBuildingType.Smelter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 5, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.Foundry:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 6, wp, WorldData.SubTileWidth * 1.2f) );
                    break;
                case TerrainBuildingType.Armory:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 8, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Chemist:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 7, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Gunmaker:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 9, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.Brewery:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 2, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WaterResovoir:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_water, 0, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.Carpenter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_carpenter, 0, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WoodCutter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_quarry, 0, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.StoneCutter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_quarry, 1, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.Nobelhouse:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_nobelhouse, 0, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainBuildingType.Embassy:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_nobelhouse, 1, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainBuildingType.Logistics:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_logistic, subTile.terrainAmount - 1, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.ManorLord:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workerhut_long, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Bank:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_bank, 0, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.CoinMinter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_bank, 1, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.School:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_logistic, 2, wp, WorldData.SubTileWidth * 1.0f));
                    break;

                case TerrainBuildingType.GoldDeliveryLevel1:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 6, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.GoldDeliveryLevel2:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 7, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.GoldDeliveryLevel3:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_postal, 8, wp, WorldData.SubTileWidth * 0.9f));
                    break;

                case TerrainBuildingType.ImmigrationTent:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_tent, 0, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.ResearchCenter:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_research, 1, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.BookPress:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_research, 0, wp, WorldData.SubTileWidth * 0.9f));
                    break;

                case TerrainBuildingType.Smoker:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_meatstation, 2, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Pottery:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 10, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Butcher:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_meatstation, 0, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.ShieldMaker:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_workstation, 11, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Dryer:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_meatstation, 3, wp, WorldData.SubTileWidth * 0.9f));
                    break;

                case TerrainBuildingType.MaterialStorage:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 0, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.FoodStorage:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 1, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.WeaponStorage:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 2, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.ArmorStorage:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 3, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.AnimalStorage:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 4, wp, WorldData.SubTileWidth * 0.9f));
                    break;

                case TerrainBuildingType.Cesspit:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_storage, 5, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        static readonly Color GardenGrassCol = new Color(104, 146, 70);
        static readonly Color GardenGrassColShadow = new Color(90, 135, 60);
        static readonly Color PavementGroundCol = new Color(92, 92, 136);

        void createDecor(Tile tile, ref SubTile subTile, TerrainDecorType decorType, Vector3 wp, ref bool bSurfacePolygonTexture, ref SurfaceTextureType surfacePolygonTexture, ref Color surfaceColor)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;

            switch (decorType) {
                case TerrainDecorType.CobbleStones:
                    surfaceColor = ColorExt.ChangeBrighness(surfaceColor, -8);
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_cobblestone, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainDecorType.Square:
                    surfaceColor = SquareGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_square, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;


                case TerrainDecorType.Pavement:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pavement, 0, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.PavementFlower:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pavement, 4, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.PavementRectFlower:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pavement, 3, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.PavementLamp:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pavement, 1, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.PavemenFountain:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_pavement, 2, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                


                case TerrainDecorType.Statue_ThePlayer:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.decor_statue, 0, wp, WorldData.SubTileWidth * 1f));
                    break;


                
                case TerrainDecorType.GardenFourBushes:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassColShadow;
                    surfacePolygonTexture = SurfaceTextureType.Grass;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_garden, 0, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.GardenLongTree:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassColShadow;
                    surfacePolygonTexture = SurfaceTextureType.Grass;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_garden, 1, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.GardenWalledBush:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassCol;
                    surfacePolygonTexture = SurfaceTextureType.Grass;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_garden, 2, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.GardenGrass:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassCol;
                    surfacePolygonTexture = SurfaceTextureType.Grass;                    
                    break;
                case TerrainDecorType.GardenBird:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassCol;
                    surfacePolygonTexture = SurfaceTextureType.Grass;
                    animals(tile, ref subTile, ref wp, ItemResourceType.Pheasant, TerrainContent.Pheasant);//1);
                    break;

                case TerrainDecorType.GardenMemoryStone:
                    bSurfacePolygonTexture = true;
                    surfaceColor = GardenGrassCol;
                    surfacePolygonTexture = SurfaceTextureType.Grass;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_garden, 3, wp, WorldData.SubTileWidth * 1.3f));
                    break;

                case TerrainDecorType.Statue_Leader:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.decor_statue, 1, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainDecorType.Statue_Lion:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.decor_statue, 2, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainDecorType.Statue_Horse:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.decor_statue, 3, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainDecorType.Statue_Pillar:
                    surfaceColor = PavementGroundCol;
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.decor_statue, 4, wp, WorldData.SubTileWidth * 1f));
                    break;


                case TerrainDecorType.FlagPole_LongBanner:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 0, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 0, wp + new Vector3(0.011f, 0.009f, -0.032f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        } 
                    }
                    break;

                case TerrainDecorType.FlagPole_Banner:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 0, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel( faction, 1, wp + new Vector3(0.011f, 0.009f, -0.032f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_SlimBanner:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 0, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 2, wp + new Vector3(0.011f, 0.009f, -0.032f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_Flag:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 1, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 3, wp + new Vector3(0.001f, 0.009f, -0.038f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_FlagRound:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 1, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 4, wp + new Vector3(0.001f, 0.009f, -0.038f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_FlagLarge:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 1, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 5, wp + new Vector3(0.001f, 0.009f, -0.038f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_Streamer:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 1, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 6, wp + new Vector3(0.001f, 0.009f, -0.038f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;

                case TerrainDecorType.FlagPole_Triangle:
                    {
                        foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_flagpole, 1, wp, WorldData.SubTileWidth * 1f));

                        var faction = tile.Faction_Safe();
                        if (faction != null)
                        {
                            var flag = new FoliageModel(
                            faction, 7, wp + new Vector3(0.001f, 0.009f, -0.038f), WorldData.SubTileWidth * 1f);
                            foliageModels.Add(flag);
                        }
                    }
                    break;


                default:
                    throw new NotImplementedException();
            }
        }

        void animals(Tile tile, ref SubTile subTile, ref Vector3 wp, ItemResourceType animalType, AnimalPenGrowth penGrowth)
        {
            if (tile.OutOfRenderTimeOut())
            {
                if (animalData == null)
                {
                    animalData = new List<AnimalData>(8);
                }

                int count = penGrowth.visualCount(subTile.terrainAmount);//(subTile.terrainAmount + penGrowth.maxSize - 1) / penGrowth.maxSize;
                var animal = new AnimalData(wp, animalType);
                for (int i = 0; i < count; i++)
                {
                    animalData.Add(animal);
                }
            }
        }

        void createMine(TerrainMineType mineType, Vector3 wp)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;
            
            float scale = WorldData.SubTileWidth * 1.4f;

            switch (mineType)
            {
                case TerrainMineType.IronOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 0, wp, scale) );
                    break;
                case TerrainMineType.Coal:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 2, wp, scale));
                    break;
                case TerrainMineType.TinOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 6, wp, scale));
                    break;                
                case TerrainMineType.CopperOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 9, wp, scale));
                    break;                
                case TerrainMineType.LeadOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 7, wp, scale));
                    break;
                case TerrainMineType.SilverOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 4, wp, scale));
                    break;
                case TerrainMineType.GoldOre:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 1, wp, scale));
                    break;
                case TerrainMineType.Mithril:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 8, wp, scale));
                    break;
                case TerrainMineType.Salt:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 11, wp, scale));
                    break;
                case TerrainMineType.Sulfur:
                    foliageModels.Add(new FoliageModel(LootFest.VoxelModelName.city_mine, 5, wp, scale));
                    break;

                default:
                    throw new NotImplementedException();
            }    
        }

        void createResoursePile(TerrainResourcesType resourceType, Vector3 wp)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;
            LootFest.VoxelModelName modelName;
            float scale = WorldData.SubTileWidth * 1.4f;

            switch (resourceType)
            {
               
                case TerrainResourcesType.Wood:
                    scale = 0.1f;
                    modelName = LootFest.VoxelModelName.resource_tree;
                    break;
                case TerrainResourcesType.Rubble:
                    scale = 0.1f;
                    modelName = LootFest.VoxelModelName.resource_rubble;
                    break;

                default:
                    throw new NotImplementedException();
            }

            foliageModels.Add(new FoliageModel(modelName, rnd, wp, scale));

        }

        public void synchToRender()
        {
            model.Visible = false;
            if (verticeData != null)
            {
                model.BuildFromVerticeData(verticeData,
                    new List<int> { verticeData.DrawData.numTriangles / 2 },
                    Texture);
                
                PolygonLib.VerticeDataPool.Push(verticeData);
                verticeData = null;

                if (waterEdgeVerticeData != null && waterEdgeVerticeData.Vertices.count > 0)
                {
                    waterEdgeModel.BuildFromVerticeData(waterEdgeVerticeData,
                    new List<int> { waterEdgeVerticeData.DrawData.numTriangles / 2 },
                     LoadedTexture.waterEdge);

                    PolygonLib.VerticeDataPool.Push(waterEdgeVerticeData);
                    waterEdgeVerticeData = null;

                    waterEdgeModel.Visible = true;
                    //waterEdgeModel.Color = Color.DarkGray;
                }

                model.Visible = true;
            }

            for (int i = 0; i < foliageModels.Count; ++i)
            {
                ref var m = ref foliageModels.array[i];
                m.addToRender();
            }
           
            if (animalData != null)
            {
                foreach (var m in animalData)
                {
                    m.create(pos);
                }
            }


            renderState = DetailMapTileState.InRender;

            //return add;
        }
        public void recycle()
        {
            //add = false;
            DeleteMe();
        }

        public void DeleteMe()
        {
            model.Visible = false;
            if (waterEdgeModel != null)
            {
                waterEdgeModel.Visible = false;
            }

            for (int i = 0; i < foliageModels.Count; ++i)
            {
                foliageModels[i].DeleteMe();
            }
            foliageModels.Clear();

            animalData?.Clear();

            exitRender =  DetailMapTileExitState.None;
            renderState = DetailMapTileState.None;
        }
    }

    enum DetailMapTileState
    { 
        None,
        AddToRender,
        InRender,
        //ExitRender,
    }

    enum DetailMapTileExitState
    { 
        None,
        Prepare,
        ExitRender,
    }
}
