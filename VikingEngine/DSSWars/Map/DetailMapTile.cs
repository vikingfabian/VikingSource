using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Map
{
   
    class DetailMapTile
    {
        static readonly IntervalF FoliageCenterRange = 
            IntervalF.FromCenter(0.5f * WorldData.SubTileWidth, 0.2f * WorldData.SubTileWidth);

        static readonly Vector2 GrassSize = new Vector2(0.03f, 0.11f) * WorldData.SubTileWidth;

        static readonly Vector2 SandSize = new Vector2(0.03f) * WorldData.SubTileWidth;

        static readonly IntervalF GrassCenterRange =
            IntervalF.FromCenter(0.5f * WorldData.SubTileWidth, 0.45f * WorldData.SubTileWidth);

        public static List<LootFest.VoxelModelName> LoadModel()
        {
            return new List<LootFest.VoxelModelName>
            {
                LootFest.VoxelModelName.fol_tree_hard,
                LootFest.VoxelModelName.fol_tree_soft,
                LootFest.VoxelModelName.fol_tree_dry,

                LootFest.VoxelModelName.fo_stone1,
                LootFest.VoxelModelName.fol_sprout,
                LootFest.VoxelModelName.fol_tallgrass,
                LootFest.VoxelModelName.fol_herbs,
                LootFest.VoxelModelName.fol_bush1,
                LootFest.VoxelModelName.fol_stoneblock,
                LootFest.VoxelModelName.fol_farmculture,

                LootFest.VoxelModelName.resource_tree,
                LootFest.VoxelModelName.resource_rubble,

            };
        }

        const LoadedTexture Texture = LoadedTexture.SpriteSheet;
        public static readonly Graphics.CustomEffect ModelEffect =
            new Graphics.CustomEffect("FlatVerticeColor", false);

        public IntVector2 pos;
        IVerticeData verticeData;
        Graphics.VoxelModel model;
        List<Foliage> foliage;
        List<AnimalData> animalData;
        

        public bool add = true;
        static PcgRandom rnd = new PcgRandom();
        //public bool isDeleted = false;

        public DetailMapTile(IntVector2 pos, Tile tile)
        {
            this.pos = pos;
            
            //var tile = DssRef.world.tileGrid.Get(pos);
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
            IntVector2 subTileStart = pos * WorldData.TileSubDivitions;

            for (int y = 0; y < WorldData.TileSubDivitions; ++y)
            {
                for (int x = 0; x < WorldData.TileSubDivitions; ++x)
                {
                    int subX = subTileStart.X + x;
                    int subY = subTileStart.Y + y;

                    rnd.SetSeed(subX * 3 + subY * 11);

                    SubTile subTile = DssRef.world.subTileGrid.Get(subX, subY);
                    Vector2 subTopLeft = new Vector2(topLeft.X + x * WorldData.SubTileWidth, topLeft.Y + y * WorldData.SubTileWidth);
                    
                    block(subTopLeft, ref subTile);

                    surfaceTexture(tile, subTile, subTopLeft);

                    switch (subTile.mainTerrain)
                    {
                        case TerrainMainType.Foil:
                            createFoliage((TerrainSubFoilType)subTile.subTerrain, subTile.terrainAmount,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
                        case TerrainMainType.Resourses:
                            createResoursePile((TerrainResourcesType)subTile.subTerrain,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
                        case TerrainMainType.Building:
                            createBuilding(tile, ref subTile, (TerrainBuildingType)subTile.subTerrain,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
                        case TerrainMainType.Wall:
                            createWall(tile, ref subTile, (TerrainWallType)subTile.subTerrain,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
                        case TerrainMainType.Mine:
                            createMine((TerrainMineType)subTile.subTerrain,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
                        case TerrainMainType.Decor:
                            createDecor(tile, ref subTile, (TerrainDecorType)subTile.subTerrain,
                                topCenter(ref subTile, ref subTopLeft));
                            break;
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
                    WorldData.SubTileWidthV2, false, subTile.groundY,
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

            Vector3 topCenter(ref SubTile subTile, ref Vector2 subTopLeft)
            {
               return new Vector3(
                    pos.X + subTopLeft.X,
                    subTile.groundY,
                    pos.Y + subTopLeft.Y);
            }
        }

        void surfaceTexture(Tile tile, SubTile subTile, Vector2 subTopLeft)
        {
            Biom biom = DssRef.map.bioms.bioms[(int)tile.biom];
            var col = biom.colors_height[tile.heightLevel];

            Vector3 center = new Vector3(
                subTopLeft.X,
                subTile.groundY,
                subTopLeft.Y);

            
            if (subTile.mainTerrain != TerrainMainType.Foil &&
                subTile.mainTerrain != TerrainMainType.Building &&
                subTile.mainTerrain != TerrainMainType.Decor)
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

            //LootFest.VoxelModelName modelName;
            //float scale = 0.12f;

            switch (type)
            {
                case TerrainSubFoilType.TallGrass:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_tallgrass, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.StoneBlock:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_stoneblock, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Bush:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_bush1, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Herbs:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_herbs, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.Stones:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fo_stone1, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.TreeHard:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_tree_hard, rnd, wp, 0.03f + 0.0012f * sizeValue));
                    break;
                case TerrainSubFoilType.TreeSoft:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_tree_soft, rnd, wp, 0.03f + 0.0012f * sizeValue));
                    break;
                case TerrainSubFoilType.DryWood:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_tree_dry, rnd, wp, 0.12f));
                    break;
                case TerrainSubFoilType.TreeSoftSprout:
                case TerrainSubFoilType.TreeHardSprout:
                    addFoliage(new Foliage(LootFest.VoxelModelName.fol_sprout, rnd, wp, 0.05f + 0.01f * sizeValue));
                    break;
                case TerrainSubFoilType.WheatFarm:
                    farm(3);                    
                    break;
                case TerrainSubFoilType.LinenFarm:
                    farm(4);                    
                    break;
                case TerrainSubFoilType.HempFarm:
                    farm(6);
                    break;
                case TerrainSubFoilType.RapeSeedFarm:
                    farm(5);
                    break;

                case TerrainSubFoilType.BogIron:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 3, wp, 0.14f));
                    break;
                default:
                    throw new NotImplementedException();
            }


            void farm(int readyFrame)
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
                addFoliage(new Foliage(LootFest.VoxelModelName.fol_farmculture, frame, wp, 0.1f));
            }
            
        }

        void createWall(Tile tile, ref SubTile subTile, TerrainWallType buildingType, Vector3 wp)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;

            switch (buildingType)
            {
                case TerrainWallType.DirtWall:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_dirtwall, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainWallType.DirtTower:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_dirttower, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainWallType.WoodWall:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_woodwall, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainWallType.WoodTower:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_woodtower, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainWallType.StoneWall:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_stonewall, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainWallType.StoneTower:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_stonetower, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                default:
                    throw new NotImplementedException();

            }

        }
    

    void createBuilding(Tile tile, ref SubTile subTile, TerrainBuildingType buildingType, Vector3 wp)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;


            switch (buildingType)
            {
                case TerrainBuildingType.PigPen:
                    animals(tile, ref subTile, ref wp, AnimalType.Pig, TerrainContent.PigMaxSize);
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.HenPen:
                    animals(tile, ref subTile, ref wp, AnimalType.Hen, TerrainContent.HenMaxSize);
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_pen, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.WorkerHut:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workerhut, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Tavern:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_tavern, rnd, wp, WorldData.SubTileWidth * 0.9f));
                    break;
                case TerrainBuildingType.Storehouse:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_storehouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.Postal:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_postal, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.Recruitment:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_recruitment, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.SoldierBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 1, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.ArcherBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 2, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WarmashineBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 3, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.KnightsBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 5, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.GunBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 6, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.CannonBarracks:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_barracks, 7, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.StoneHall:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_stonehall, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.SmallHouse:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_smallhouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.BigHouse:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_bighouse, rnd, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.CobbleStones:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_cobblestone, rnd, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Square:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_square, rnd, wp, WorldData.SubTileWidth * 1.1f));
                    break;
                case TerrainBuildingType.Work_Cook:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 1, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.Work_Bench:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 3, wp, WorldData.SubTileWidth * 1.4f));
                    break;

                case TerrainBuildingType.Work_CoalPit:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 4, wp, WorldData.SubTileWidth * 1.2f));
                    break;

                case TerrainBuildingType.Work_Smith:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 0, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Smelter:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 5, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Foundry:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 6, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Armory:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 0, wp, WorldData.SubTileWidth * 1.4f));//saknar
                    break;
                case TerrainBuildingType.Chemist:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 7, wp, WorldData.SubTileWidth * 1.4f));
                    break;
                case TerrainBuildingType.Gunmaker:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 0, wp, WorldData.SubTileWidth * 0.4f));
                    break;

                case TerrainBuildingType.Brewery:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 2, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WaterResovoir:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_water, 0, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.Carpenter:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_carpenter, 0, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.WoodCutter:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_quarry, 0, wp, WorldData.SubTileWidth * 1f));
                    break;
                case TerrainBuildingType.StoneCutter:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_quarry, 1, wp, WorldData.SubTileWidth * 1f));
                    break;

                case TerrainBuildingType.Nobelhouse:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_nobelhouse, 0, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainBuildingType.Embassy:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_nobelhouse, 1, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainBuildingType.Logistics:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_logistic, subTile.terrainAmount -1, wp, WorldData.SubTileWidth * 1.0f));
                    break;

                case TerrainBuildingType.Bank:
                    addFoliage(new Foliage(LootFest.VoxelModelName.Bank, 0, wp, WorldData.SubTileWidth * 1.0f));
                    break;
                case TerrainBuildingType.CoinMinter:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_workstation, 0, wp, WorldData.SubTileWidth * 0.5f));
                    break;

                case TerrainBuildingType._RESERVE1:
                case TerrainBuildingType._RESERVE2:
                case TerrainBuildingType._RESERVE3:
                case TerrainBuildingType._RESERVE4:
                case TerrainBuildingType._RESERVE5:
                case TerrainBuildingType._RESERVE6:
                    createWall(tile, ref subTile, (TerrainWallType)buildingType, wp);
                    break;
                
                default:
                    throw new NotImplementedException();
            }
        }

        void createDecor(Tile tile, ref SubTile subTile, TerrainDecorType decorType, Vector3 wp)
        {
            wp.X += WorldData.SubTileHalfWidth;
            wp.Z += WorldData.SubTileHalfWidth;

            switch (decorType) {
                case TerrainDecorType.Pavement:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_pavement, 0, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.PavementFlower:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_pavement, 1, wp, WorldData.SubTileWidth * 1.3f));
                    break;
                case TerrainDecorType.Statue_ThePlayer:
                    addFoliage(new Foliage(LootFest.VoxelModelName.decor_statue, 0, wp, WorldData.SubTileWidth * 1f));
                    break;
               
                default:
                    throw new NotImplementedException();
            }
        }

        void animals(Tile tile, ref SubTile subTile, ref Vector3 wp, AnimalType animalType, int animalSize)
        {
            if (tile.OutOfRenderTimeOut())
            {
                if (animalData == null)
                {
                    animalData = new List<AnimalData>(8);
                }

                int count = (subTile.terrainAmount + animalSize - 1) / animalSize;
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
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 0, wp, scale));
                    break;
                case TerrainMineType.Coal:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 2, wp, scale));
                    break;
                case TerrainMineType.TinOre:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 6, wp, scale));
                    break;
                
                case TerrainMineType.CupperOre:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 9, wp, scale));
                    break;
                
                case TerrainMineType.LeadOre:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 7, wp, scale));
                    break;
                case TerrainMineType.SilverOre:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 4, wp, scale));
                    break;
                case TerrainMineType.GoldOre:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 1, wp, scale));
                    break;
                case TerrainMineType.Mithril:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 8, wp, scale));
                    break;
                case TerrainMineType.Sulfur:
                    addFoliage(new Foliage(LootFest.VoxelModelName.city_mine, 5, wp, scale));
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

            //if (foliage == null)
            //{
            //    foliage = new List<Foliage>(8);
            //}
//#if DEBUG
//            model.DebugName = "Resource pile " + model.DebugName;
//#endif
            addFoliage(new Foliage(modelName, rnd, wp, scale));

        }

        void addFoliage(Foliage f)
        {
            if (foliage == null)
            {
                foliage = new List<Foliage>(8);
            }
            foliage.Add(f);
        }

        public void synchToRender()
        {
            if (add)
            {
                //var tile = DssRef.world.tileGrid.Get(pos);

                if (model != null)
                {
                    model.BuildFromVerticeData(verticeData,
                        new List<int> { verticeData.DrawData.numTriangles / 2 },
                        Texture);

                    model.AddToRender(DrawGame.UnitDetailLayer);

                    verticeData = null;
                }

                //foliage?.addToRender();
                if (foliage != null)
                {
                    foreach (var m in foliage)
                    {
                        //m.AddToRender(DrawGame.UnitDetailLayer);
                        m.addToRender();
                    }
                }

                if (animalData != null)
                {
                    foreach (var m in animalData)
                    {
                        m.create(pos);
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
            model?.DeleteMe();
            if (foliage != null)
            {
                foreach (var m in foliage)
                {
                    m.DeleteMe();
                }
                foliage = null;
            }

            //isDeleted = true;
        }
    }
}
