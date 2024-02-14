using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.PJ.SpaceWar
{
    class WorldMap
    {
        public const float MapHalfW = 200;
        const float ObjectDespawnLength = 40;
        public static VectorRect DespawnArea;
        public VectorRect PlayerBounds;

        public WorldMap()
        {
            DespawnArea = VectorRect.FromCenterSize(Vector2.Zero, new Vector2((MapHalfW + ObjectDespawnLength) * 2f));
            PlayerBounds = VectorRect.FromCenterSize(Vector2.Zero, new Vector2(MapHalfW * 2f));
            
            generateBackground();

            new WorldSetup(this);
        }

        void generateBackground()
        {
            const int LayerCount = 20;

            MapLayer[] layers = new MapLayer[20];
            IntervalF depthRange = new IntervalF(-150f, -50f);

            float depth = depthRange.Min;
            float add = depthRange.Difference / LayerCount;

            for (int i = 0; i < LayerCount; ++i)
            {
                layers[i] = new MapLayer(depth);
                depth += add;
            }


            const int StarCount = 2000;
            const int CloudCluster = 500;

            const float VisualHalfW = MapHalfW * 1.3f;


            Color red = new Color(255, 209, 223);
            Color blue = new Color(207, 219, 255);
            Color green = new Color(207, 255, 234);
            
            //STARS
            for (int i = 0; i < StarCount; ++i)
            {
                Vector2 pos = new Vector2(Ref.rnd.Plus_MinusF(VisualHalfW), Ref.rnd.Plus_MinusF(VisualHalfW));
                MapLayer lay = arraylib.RandomListMember(layers);


                float StarW = Ref.rnd.Float(0.16f, 0.22f);
                
                Color starCol;
                var rndCol = Ref.rnd.Double();

                if (rndCol < 0.5f)
                {
                    starCol = Color.LightYellow;
                }
                if (rndCol < 0.7f)
                {
                    starCol = blue;
                }
                if (rndCol < 0.9f)
                {
                    starCol = green;
                }
                else
                {
                    starCol = red;
                }

                if (Ref.rnd.Chance(0.1))
                {
                    Graphics.PolygonColor poly = PolygonColor.QuadXZ(pos,
                        new Vector2(StarW * 6f), true, lay.y, SpriteName.spaceWarStarWhite, Dir4.N, starCol);
                    lay.polygons.Add(poly);
                }
                else
                {
                    Graphics.PolygonColor poly = PolygonColor.QuadXZ(pos,
                        new Vector2(StarW), true, lay.y, SpriteName.WhiteArea_LFtiles, Dir4.N, starCol);
                    lay.polygons.Add(poly);
                }
            }

            for (int i = 0; i < CloudCluster; ++i)
            {
                Vector2 pos = new Vector2(Ref.rnd.Plus_MinusF(VisualHalfW), Ref.rnd.Plus_MinusF(VisualHalfW));
                MapLayer lay = layers[Ref.rnd.Int(10, LayerCount)];

                float width = 40f;

                float y = lay.y;

                int clusterCount = Ref.rnd.Int(3, 12);


                float r, g, b;
                var rndCol = Ref.rnd.Double();
                if (rndCol < 0.4f)
                {
                    r = 1f;
                    g = 0.5f;
                    b = 0f;
                }
                else
                {
                    r = 1f;
                    g = 0f;
                    b = 0.4f;
                }

                const float colDiff = 0.05f;

                for (int c = 0; c < clusterCount; ++c)
                {
                    y += 0.1f;

                    Graphics.PolygonColor poly = PolygonColor.QuadXZ(pos + Ref.rnd.vector2_cirkle(Ref.rnd.Float(20f)),
                        new Vector2(width), true, lay.y, lib.SumSpriteName(SpriteName.spaceWarFogCirkle1, Ref.rnd.Int(11)), Ref.rnd.Dir4(), 
                        new Color(r + Ref.rnd.Plus_MinusF(colDiff), g + Ref.rnd.Plus_MinusF(colDiff), b + Ref.rnd.Plus_MinusF(colDiff)));
                    

                    lay.opacityPolygons.Add(poly);
                }
            }

            //BORDERS

            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(StarCount + 4);
            List<Graphics.PolygonColor> opacityPolys = new List<Graphics.PolygonColor>(StarCount + 4);


            //BG PLANE
            {
                float bgplaneY = depthRange.Min - 5;

                Graphics.PolygonColor bgPlane = PolygonColor.QuadXZ(Vector2.Zero,
                    new Vector2(VisualHalfW * 3f), true, bgplaneY, SpriteName.WhiteArea_LFtiles, Dir4.N, new Color(8, 0, 16));
                bgPlane.V3ne.Color = new Color(16, 0, 14);
                bgPlane.V0sw.Color = Color.Black;
                bgPlane.V2se.Color = Color.Black;

                polygons.Add(bgPlane);
            }

            foreach (var m in layers)
            {
                polygons.AddRange(m.polygons);
                opacityPolys.AddRange(m.opacityPolygons);
            }


            const float BordersW = 0.1f;

            Vector3 borderCenter = Vector3.Zero;

            Graphics.PolygonColor leftBorder = PolygonColor.QuadXZ(
                new Vector2(-MapHalfW - BordersW, -MapHalfW), new Vector2(BordersW, MapHalfW * 2f), false, 0f, SpriteName.WhiteArea_LFtiles, Dir4.N, Color.Red);
            polygons.Add(leftBorder);

            Graphics.PolygonColor rightBorder = PolygonColor.QuadXZ(
                new Vector2(MapHalfW, -MapHalfW), new Vector2(BordersW, MapHalfW * 2f), false, 0f, SpriteName.WhiteArea_LFtiles, Dir4.N, Color.Red);
            polygons.Add(rightBorder);

            Graphics.PolygonColor topBorder = PolygonColor.QuadXZ(
                new Vector2(-MapHalfW, -MapHalfW - BordersW), new Vector2((MapHalfW + BordersW) * 2f, BordersW), false, 0f, SpriteName.WhiteArea_LFtiles, Dir4.N, Color.Red);
            polygons.Add(topBorder);

            Graphics.PolygonColor bottomBorder = PolygonColor.QuadXZ(
               new Vector2(-MapHalfW, MapHalfW), new Vector2((MapHalfW + BordersW) * 2f, BordersW), false, 0f, SpriteName.WhiteArea_LFtiles, Dir4.N, Color.Red);
            polygons.Add(bottomBorder);
            
            new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                polygons, null), LoadedTexture.SpriteSheet, true);
 
            var fog = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                opacityPolys, null), LoadedTexture.SpriteSheet, true);
            fog.Opacity = 0.03f;
        }

        class MapLayer
        {
            public List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>(64);
            public List<Graphics.PolygonColor> opacityPolygons = new List<Graphics.PolygonColor>(64);
            public float y;

            public MapLayer(float y)
            {
                this.y = y;
            }
        }
    }
}
