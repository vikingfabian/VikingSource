using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.Core.BlackBolts.Map
{
    class MapModel
    {
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
            PcgRandom rnd = new PcgRandom(1);
            IntVector2 pos = IntVector2.Zero;
            List<PolygonColor> polygons = new List<PolygonColor>();
            //Tiles
            for (pos.Y = 0; pos.Y < BlackRef.mapData.Size.Y; ++pos.Y)
            {
                for (pos.X = 0; pos.X < BlackRef.mapData.Size.X; ++pos.X)
                {
                    rnd.SetSeed(pos.GetHashCode());

                    var tile = BlackRef.mapData.tileGrid.Get(pos);
                    SpriteName sprite;
                    switch (tile.tileEffect)
                    {
                        default:
                            sprite = (SpriteName)((int)SpriteName.bb_floor1 + rnd.Int(4));
                            break;
                        case TileEffect.NoBuildZone:
                            sprite = (SpriteName)((int)SpriteName.bb_noBuildZone1 + rnd.Int(2));
                            break;
                        case TileEffect.Spawner:
                            sprite = SpriteName.bb_spawnwarning_texture;
                            break;
                    }
                    polygons.Add(PolygonColor.QuadXZ(pos.Vec, Vector2.One, true, 0f, 
                       sprite, Dir4.N, lib.IsEven(pos.X + pos.Y)? Color.White : Color.LightGray));
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
            //Tiles
            for (pos.Y = 0; pos.Y < BlackRef.mapData.Size.Y; ++pos.Y)
            {
                for (pos.X = 0; pos.X < BlackRef.mapData.Size.X; ++pos.X)
                {
                    var tile = BlackRef.mapData.tileGrid.Get(pos);
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
