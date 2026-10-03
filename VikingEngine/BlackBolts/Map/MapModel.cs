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
        public bool isGenerating = false;
        GeneratedObjColor model;

        public MapModel()
        {
            beginGenerateModel();
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

            model = new Graphics.GeneratedObjColor(
                new Graphics.PolygonsAndTrianglesColor(polygons),
                LoadedTexture.SpriteSheet, false);
            
            
            //newConent.model.Color = ColorExt.GrayScale(10);//Color.Gray;

        }
        void onGenerateComplete()
        {
            isGenerating = false;
            model.AddToRender();
        }
    }
}
