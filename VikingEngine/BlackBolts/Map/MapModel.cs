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
        public bool isGenerating = false;
        public bool isGeneratingDecals = false;
        GeneratedObjColor floormodel, decalsModel, newdecalsModel;

        public MapModel()
        {
            beginGenerateModel();
        }

        public void update()
        {
            if (decalsNeedsUpdate && !isGeneratingDecals)
            {
                beginGenerateDecalModel();
            }
        }
        public void beginGenerateDecalModel()
        {
            decalsNeedsUpdate = false;
            isGeneratingDecals = true;

            new QueAndSynchTask(generateDecalsAsynch, onGenerateDecalsComplete);
        }
        public void beginGenerateModel()
        {
            isGenerating = true;

            new QueAndSynchTask(generateAsynch, onGenerateComplete);
        }

        void generateAsynch()
        {
            IntVector2 pos = IntVector2.Zero;
            List<PolygonColor> polygons = new List<PolygonColor>();
            //Tiles
            for (pos.Y = 0; pos.Y < BlackRef.mapData.Size.Y; ++pos.Y)
            {
                for (pos.X = 0; pos.X < BlackRef.mapData.Size.X; ++pos.X)
                {
                    polygons.Add(PolygonColor.QuadXZ(pos.Vec, Vector2.One, true, 0f, 
                        (SpriteName)((int)SpriteName.bb_floor1 + Ref.rnd.Int(4)), Dir4.N, lib.IsEven(pos.X + pos.Y)? Color.White : Color.LightGray));
                }
            }

            floormodel = new Graphics.GeneratedObjColor(
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
                        SpriteName sprite = SpriteName.bb_stain_shit;
                        polygons.Add(PolygonColor.QuadXZ(pos.Vec, Vector2.One, true, 0.02f,
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
            isGenerating = false;
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
