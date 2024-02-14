using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    static class BlockLib
    {
        public const int NumVerticesPerFace = 4;
        public const int VertexCornerTopLeftIndex = 0;
        public const int VertexCornerTopRightIndex = 1;
        public const int VertexCornerLowLeftIndex = 2;
        public const int VertexCornerLowRightIndex = 3;

        static readonly Dictionary<GroundType, SpriteName> BlockTextures = new Dictionary<GroundType, SpriteName>
        {
            { GroundType.Bricks, SpriteName.NO_IMAGE },
            { GroundType.Desert, SpriteName.NO_IMAGE },
            { GroundType.Grass, SpriteName.NO_IMAGE },
            { GroundType.Mountain, SpriteName.NO_IMAGE }

        };
        public static Graphics.PolygonsAndTrianglesNormal[] BlockPolys;
        public const float BlockRadius = 0.5f;
        
        public static void Init()
        {
            //Skapa en mall för alla blocken, för kopiering
            BlockPolys = new Graphics.PolygonsAndTrianglesNormal[(int)GroundType.NUM];

            
            Vector3[] topCorners = new Vector3[PolygonNormal.NumCorners];

            topCorners[PolygonNormal.CornerTopLeft].X = BlockRadius;
            topCorners[PolygonNormal.CornerTopLeft].Y = BlockRadius;
            topCorners[PolygonNormal.CornerTopLeft].Z = -BlockRadius;

            topCorners[PolygonNormal.CornerTopRight].X = -BlockRadius;
            topCorners[PolygonNormal.CornerTopRight].Y = BlockRadius;
            topCorners[PolygonNormal.CornerTopRight].Z = -BlockRadius;

            topCorners[PolygonNormal.CornerLowLeft].X = BlockRadius;
            topCorners[PolygonNormal.CornerLowLeft].Y = BlockRadius;
            topCorners[PolygonNormal.CornerLowLeft].Z = BlockRadius;

            topCorners[PolygonNormal.CornerLowRight].X = -BlockRadius;
            topCorners[PolygonNormal.CornerLowRight].Y = BlockRadius;
            topCorners[PolygonNormal.CornerLowRight].Z = BlockRadius;


            Vector3[] bottomCorners = new Vector3[PolygonNormal.NumCorners];

            bottomCorners[PolygonNormal.CornerTopLeft].X = BlockRadius;
            bottomCorners[PolygonNormal.CornerTopLeft].Y = -BlockRadius;
            bottomCorners[PolygonNormal.CornerTopLeft].Z = -BlockRadius;

            bottomCorners[PolygonNormal.CornerTopRight].X = -BlockRadius;
            bottomCorners[PolygonNormal.CornerTopRight].Y = -BlockRadius;
            bottomCorners[PolygonNormal.CornerTopRight].Z = -BlockRadius;

            bottomCorners[PolygonNormal.CornerLowLeft].X = BlockRadius;
            bottomCorners[PolygonNormal.CornerLowLeft].Y = -BlockRadius;
            bottomCorners[PolygonNormal.CornerLowLeft].Z = BlockRadius;

            bottomCorners[PolygonNormal.CornerLowRight].X = -BlockRadius;
            bottomCorners[PolygonNormal.CornerLowRight].Y = -BlockRadius;
            bottomCorners[PolygonNormal.CornerLowRight].Z = BlockRadius;


            List<GroundType> createBlocks = new List<GroundType>
            {
                GroundType.Bricks,
                GroundType.Desert,
                GroundType.Grass,
                GroundType.Mountain
            };

            foreach (GroundType type in createBlocks)
            {
                PolygonsAndTrianglesNormal polygons = PolygonsAndTrianglesNormal.Empty;
                ////TOP
                SpriteName SpriteName = BlockTextures[type];
                int tile = (int)SpriteName;//DataLib.SpriteCollection.Sprites[(int)SpriteName];
                
                //bottom behövs ej, man ser den aldrig, ej heller back
                //TOP
                polygons.Polygons.Add(new PolygonNormal(new Vector3[] {
                    topCorners[PolygonNormal.CornerTopRight], topCorners[PolygonNormal.CornerTopLeft], 
                    topCorners[PolygonNormal.CornerLowRight], topCorners[PolygonNormal.CornerLowLeft]}, tile, VectorRect.ZeroOne, Dir4.N));
                //Front
                polygons.Polygons.Add(new PolygonNormal(new Vector3[] {
                    topCorners[PolygonNormal.CornerLowRight], topCorners[PolygonNormal.CornerLowLeft], 
                    bottomCorners[PolygonNormal.CornerLowRight], bottomCorners[PolygonNormal.CornerLowLeft]}, tile, VectorRect.ZeroOne, Dir4.N));
                //Left
                polygons.Polygons.Add(new PolygonNormal(new Vector3[] {
                    topCorners[PolygonNormal.CornerTopRight], topCorners[PolygonNormal.CornerLowRight], 
                    bottomCorners[PolygonNormal.CornerTopRight], bottomCorners[PolygonNormal.CornerLowRight]}, tile, VectorRect.ZeroOne, Dir4.N));
                
                BlockPolys[(int)type] = polygons;
            }

            
        }

        
    }
    enum GroundType : byte
    {
        NOTHING = 0,
        Bricks,
        Grass,
        Mountain,
        Desert,
        NUM,
        REPEATE,
    }
}
