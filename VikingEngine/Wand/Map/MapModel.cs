using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.DataLib;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.Map
{
    class MapModel
    {
        const float WallHeight = 0.6f;
        const float ThinWallThickness = 0.1f;

        public bool decalsNeedsUpdate = false;
        public bool floorNeedsUpdate = false;
        public bool isGeneratingFloor = false;
        public bool isGeneratingDecals = false;
        GeneratedObjColor floormodel, newFloorModel, decalsModel, newdecalsModel;

        public MapModel()
        {
            floorNeedsUpdate = true;
            //beginGenerateFloorModel();
        }

        public void update()
        {
            if (decalsNeedsUpdate && !isGeneratingDecals)
            {
                beginGenerateDecalModel();
            }

            if (floorNeedsUpdate && !isGeneratingFloor)
            {
                beginGenerateFloorModel();
            }
        }
        public void beginGenerateDecalModel()
        {
            decalsNeedsUpdate = false;
            isGeneratingDecals = true;

            new QueAndSynchTask(generateDecalsAsynch, onGenerateDecalsComplete);
        }
        public void beginGenerateFloorModel()
        {
            floorNeedsUpdate = false;
            isGeneratingFloor = true;

            new QueAndSynchTask(generateAsynch, onGenerateComplete);
        }

        void generateAsynch()
        {
            var tileGrid = WandRef.world.map.tileGrid;

            PcgRandom rnd = new PcgRandom(1);
            IntVector2 pos = IntVector2.Zero;
            List<PolygonColor> polygons = new List<PolygonColor>();
            //Tiles
            for (pos.Y = 0; pos.Y < tileGrid.Size.Y; ++pos.Y)
            {
                for (pos.X = 0; pos.X < tileGrid.Size.X; ++pos.X)
                {
                    rnd.SetSeed(pos.GetHashCode());

                    var tile = tileGrid.Get(pos);
                    if (tile.explored)
                    {

                        switch (tile.tileType)
                        {
                            case TileType.Floor:
                                SpriteName sprite = (SpriteName)((int)SpriteName.bb_floor1 + rnd.Int(4));
                                floorTile(sprite, lib.IsEven(pos.X + pos.Y) ? Color.White : Color.LightGray);
                                break;
                            case TileType.ExitLevel:
                                floorTile(SpriteName.WhiteArea_LFtiles, Color.Green);
                                break;
                        }

                        void floorTile(SpriteName sprite, Color color)
                        {
                            polygons.Add(PolygonColor.QuadXZ(pos.Vec, Vector2.One, true, 0f,
                                sprite, Dir4.N, color));
                        }
                    }

                    if (tile.northWall != ThinWall.None)
                    {
                        if (tile.explored ||
                            (tileGrid.TryGet(VectorExt.AddY(pos, -1), out var nTile) && nTile.explored))
                        {
                            thinwall(tile.northWall, VectorExt.AddY(pos.Vec, -0.5f), new Vector2(1, ThinWallThickness));
                        }
                    }
                    if (tile.eastWall != ThinWall.None)
                    {
                        if (tile.explored ||
                            (tileGrid.TryGet(VectorExt.AddX(pos, 1), out var nTile) && nTile.explored))
                        {
                            thinwall(tile.eastWall, VectorExt.AddX(pos.Vec, 0.5f), new Vector2(ThinWallThickness, 1));
                        }
                    }
                    
                    void thinwall(ThinWall wallType, Vector2 quadPos, Vector2 quadSize)
                    {
                        SpriteName sprite;
                        Color color;
                        switch (wallType)
                        {
                            default:
                                sprite = SpriteName.bb_floor1;
                                color = Color.SaddleBrown;
                                break;
                            case ThinWall.Door:
                                sprite = SpriteName.WhiteArea_LFtiles;
                                color = Color.Beige;
                                break;
                            case ThinWall.LockedDoor:
                                sprite = SpriteName.WhiteArea_LFtiles;
                                color = Color.DarkBlue;
                                break;


                        }
                        var topQuad = PolygonColor.QuadXZ(quadPos, quadSize, true, WallHeight,
                            sprite, Dir4.N, color);
                        polygons.Add(topQuad);
                        topQuad.quadXZSides(WallHeight, sprite, Dir4.N, color, polygons);
                    }
                    
                }
            }

            newFloorModel = new Graphics.GeneratedObjColor(
                new Graphics.PolygonsAndTrianglesColor(polygons),
                LoadedTexture.SpriteSheet, false);

        }

        void generateDecalsAsynch()
        {
            
            IntVector2 pos = IntVector2.Zero;
            List<PolygonColor> polygons = new List<PolygonColor>();
            /*
            //Tiles
            for (pos.Y = 0; pos.Y < WandRef.mapData.Size.Y; ++pos.Y)
            {
                for (pos.X = 0; pos.X < WandRef.mapData.Size.X; ++pos.X)
                {
                    var tile = WandRef.mapData.tileGrid.Get(pos);
                    if (tile.fluid.HasValue)
                    {
                        SpriteName sprite = SpriteName.MissingImage;
                        switch (tile.fluid.resourceType)
                        {
                            case Data.ResourceType.FluidPoopStain:
                                sprite = SpriteName.bb_stain_shit;
                                break;
                            case Data.ResourceType.FluidBlood:
                                sprite = SpriteName.bb_stain_blood;
                                break;

                        }
                        polygons.Add(PolygonColor.QuadXZ(pos.Vec, Vector2.One, true, 0.01f,
                            sprite, Dir4.N, Color.White));
                    }
                }
            }
            */
            newdecalsModel = new Graphics.GeneratedObjColor(
                new Graphics.PolygonsAndTrianglesColor(polygons),
                LoadedTexture.SpriteSheet, false);

        }
        void onGenerateComplete()
        {
            isGeneratingFloor = false;
            floormodel?.DeleteMe();
            floormodel = newFloorModel;
            newFloorModel = null;
            floormodel.AddToRender();
        }
        void onGenerateDecalsComplete()
        {
            isGeneratingDecals = false;
            decalsModel?.DeleteMe();
            decalsModel = newdecalsModel;
            newdecalsModel = null;
            decalsModel.AddToRender();
        }
    }
}
