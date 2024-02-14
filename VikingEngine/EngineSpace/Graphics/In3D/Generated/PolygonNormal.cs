using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    struct PolygonsAndTrianglesNormal : IPolygonsAndTriangles
    {
        public List<PolygonNormal> Polygons;
        public List<TriangleNormal> Triangles;
        public PolygonsAndTrianglesNormal(List<PolygonNormal> polygons, List<TriangleNormal> triangles)
        { Polygons = polygons; Triangles = triangles; }
        
        public void AddRange(IPolygonsAndTriangles add)
        {
            PolygonsAndTrianglesNormal conv = (PolygonsAndTrianglesNormal)add;
            Polygons.AddRange(conv.Polygons);
        }
      
        public object GetPolygonVertex0(ref int polyIndex)
        {
            return Polygons[polyIndex].Vertex0sw;
        }
        public object GetPolygonVertex1(ref int polyIndex)
        {
            return Polygons[polyIndex].Vertex1nw;
        }
        public object GetPolygonVertex2(ref int polyIndex)
        {
            return Polygons[polyIndex].Vertex2se;
        }
        public object GetPolygonVertex3(ref int polyIndex)
        {
            return Polygons[polyIndex].Vertex3ne;
        }

        public object GetTriangleVertex(int triangleIndex, int vertticeIx)
        {
            return Triangles[triangleIndex].VerticeData[vertticeIx];
        }
        public void Add(PolygonsAndTrianglesNormal add)
        {
            Polygons.AddRange(add.Polygons);
            Triangles.AddRange(add.Triangles);
        }

        public static PolygonsAndTrianglesNormal Empty
        { get { return new PolygonsAndTrianglesNormal(new List<PolygonNormal>(), new List<TriangleNormal>()); } }

        public PolygonType Type { get { return PolygonType.Normal; } }
        public int NumPolygons { get { return Polygons.Count; } }
        public int NumTriangles { get { return  (Triangles == null?  0 : Triangles.Count); } }
    }
    struct PolygonNormal
    {
        public VertexPositionNormalTexture Vertex0sw;
        public VertexPositionNormalTexture Vertex1nw;
        public VertexPositionNormalTexture Vertex2se;
        public VertexPositionNormalTexture Vertex3ne;

        public const int NumCorners = 4;
        const int NW = 1;
        const int NE = 3;
        const int SW = 0;
        const int SE = 2;
        public const int CornerTopLeft = 0;
        public const int CornerTopRight = 1;
        public const int CornerLowLeft = 2;
        public const int CornerLowRight = 3;

        static Dictionary<Dir4, int[]> FacesTextureCoords = new Dictionary<Dir4, int[]>  {
            { Dir4.N, new int[] { NW, NE, SW, SE} },
            { Dir4.E, new int[] { NE, SE, NW, SW} },
            { Dir4.S, new int[] { SW, SE, NW, NE} },
            { Dir4.W, new int[] { SW,NW,SE,NE} }
        };

        public PolygonNormal(LootFest.Map.PreparedFace face, Vector3 pos)
        {            
            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[face.tileIx];
            Graphics.Face data = LootFest.Data.Block.GetVoxelObjFace(pos, face.faceType);

            Vertex0sw = new VertexPositionNormalTexture(data.Corner3, data.Normal, file.SourcePolygonLowLeft);
            Vertex1nw = new VertexPositionNormalTexture(data.Corner1, data.Normal, file.SourcePolygonTopLeft);
            Vertex2se = new VertexPositionNormalTexture(data.Corner4, data.Normal, file.SourcePolygonLowRight);
            Vertex3ne = new VertexPositionNormalTexture(data.Corner2, data.Normal, file.SourcePolygonTopRight);
        }

        /// <param name="corners">Corner positions in the order NW, NE, SW, SE</param>
        public PolygonNormal(Vector3[] corners, int tileIx, VectorRect tileSource, Dir4 spriteRotation)
        {
            Vertex0sw = new VertexPositionNormalTexture(corners[CornerLowLeft], Vector3.Up, Vector2.Zero);
            Vertex1nw = new VertexPositionNormalTexture(corners[CornerTopLeft], Vector3.Up, Vector2.Zero);
            Vertex2se = new VertexPositionNormalTexture(corners[CornerLowRight], Vector3.Up, Vector2.Zero);
            Vertex3ne = new VertexPositionNormalTexture(corners[CornerTopRight], Vector3.Up, Vector2.Zero);

            setSprite(tileSource, spriteRotation);
        }

        public PolygonNormal(Graphics.Face face)
        {
            Vertex0sw = new VertexPositionNormalTexture(face.Corner3, face.Normal, Vector2.Zero);
            Vertex1nw = new VertexPositionNormalTexture(face.Corner1, face.Normal, Vector2.Zero);
            Vertex2se = new VertexPositionNormalTexture(face.Corner4, face.Normal, Vector2.Zero);
            Vertex3ne = new VertexPositionNormalTexture(face.Corner2, face.Normal, Vector2.Zero);
        }

        public void setSprite(SpriteName sprite, Dir4 spriteRotation)
        {
            Graphics.Sprite file = DataLib.SpriteCollection.Get(sprite);
            setSprite(file.SourceF, spriteRotation);
        }

        public void setSprite(VectorRect tileSource, Dir4 spriteRotation)
        {
            switch (spriteRotation)
            {
                case Dir4.N:
                    Vertex1nw.TextureCoordinate = tileSource.Position;
                    Vertex3ne.TextureCoordinate = tileSource.RightTop;
                    Vertex0sw.TextureCoordinate = tileSource.LeftBottom;
                    Vertex2se.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.E:
                    Vertex0sw.TextureCoordinate = tileSource.Position;
                    Vertex1nw.TextureCoordinate = tileSource.RightTop;
                    Vertex2se.TextureCoordinate = tileSource.LeftBottom;
                    Vertex3ne.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.S:
                    Vertex2se.TextureCoordinate = tileSource.Position;
                    Vertex0sw.TextureCoordinate = tileSource.RightTop;
                    Vertex3ne.TextureCoordinate = tileSource.LeftBottom;
                    Vertex1nw.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.W:
                    Vertex3ne.TextureCoordinate = tileSource.Position;
                    Vertex2se.TextureCoordinate = tileSource.RightTop;
                    Vertex1nw.TextureCoordinate = tileSource.LeftBottom;
                    Vertex0sw.TextureCoordinate = tileSource.RightBottom;
                    break;
            }
        }

        public Vector3 CalcNormals()
        {
            SetNormals(PolygonLib.CalculateNormals(ref Vertex0sw.Position, ref Vertex1nw.Position, ref Vertex2se.Position));
            return Vertex0sw.Normal;
        }
        public void SetNormals(Vector3 normal)
        {
            Vertex0sw.Normal = normal;
            Vertex1nw.Normal = normal;
            Vertex2se.Normal = normal;
            Vertex3ne.Normal = normal;
        }
        //All vertices HAVE TO lie in one Plane
        public static List<PolygonNormal> PlaneWithDivitions(Vector3[] corners, int tileIx, VectorRect tileSource, 
            Dir4 tileRotation, IntVector2 divitions)
        {
            Vector3 topLeft = corners[CornerTopLeft];
            Vector3 stepX = corners[CornerTopRight] - topLeft;
            Vector3 stepY = corners[CornerLowLeft] - topLeft;

            stepX /= divitions.X;
            stepY /= divitions.Y;

            List<PolygonNormal> polygons = new List<PolygonNormal>();
            for (int y = 0; y < divitions.Y; y++)
            {
                for (int x = 0; x < divitions.X; x++)
                {
                    polygons.Add(new PolygonNormal(new Vector3[] {
                        //NW,NE,SW,SE
                        topLeft + stepX * x + stepY * y,
                        topLeft + stepX * (x +1) + stepY * y,
                        topLeft + stepX * x + stepY * (y + 1),
                        topLeft + stepX * (x +1) + stepY * (y + 1)
                        
                    }, tileIx, tileSource, tileRotation));
                }
            }
            return polygons;

        }
        //All vertices DON'T have to lie in one Plane
        public static List<PolygonNormal> PolygonWithDivitions(Vector3[] corners, int tileIx, VectorRect tileSource, 
            Dir4 tileRotation, IntVector2 divitions, AssignNormals assignNormals)
        {
            
            Vector3[,] vertices = new Vector3[divitions.X+1, divitions.Y+1];
            
            float partX;
            float partY;

            //sätta punkter i yled på båda sidlinjerna
            //dra en horisontal linje mellan punkterna
            //dela linjen i antal X 
            for (int y = 0; y <= divitions.Y; y++)
            {
                partY = y / (float)divitions.Y;
                Vector3 leftSideY = (1 - partY) * corners[CornerTopLeft] + partY * corners[CornerLowLeft];
                Vector3 rightSideY = (1 - partY) * corners[CornerTopRight] + partY * corners[CornerLowRight];

                for (int x = 0; x <= divitions.X; x++)
                {
                    partX = x / (float)divitions.X;
                    vertices[x, y] = (1 - partX) * leftSideY + partX * rightSideY; 
                    
                }
            }

            List<PolygonNormal> polygons = new List<PolygonNormal>();
            for (int y = 0; y < divitions.Y; y++)
            {
                for (int x = 0; x < divitions.X; x++)
                {
                    polygons.Add(new PolygonNormal(new Vector3[] {
                        //NW,NE,SW,SE
                        vertices[x,y], vertices[x+1,y], vertices[x,y+1], vertices[x+1,y+1]}, 
                        tileIx, tileSource, tileRotation));
                }
            }
            if (assignNormals == AssignNormals.AsPlane)
            {
                Vector3 normal = polygons[0].CalcNormals();
                for (int i = 1; i < polygons.Count; i++)
                {
                    polygons[i].SetNormals(normal);
                }
            }
            else if (assignNormals == AssignNormals.Idividual)
            {
                foreach (PolygonNormal p in polygons)
                {
                    p.CalcNormals();
                }
            }
            return polygons;

        }
    }
    
    //enum CubeFace
    //{

    //}
    struct TriangleNormal
    {
        public VertexPositionNormalTexture[] VerticeData;
        public const int NumCorners = 3;
        /// <param name="corners">Corner positions in the order NW, NE, SW, SE</param>
        public TriangleNormal(Vector3[] corners, Vector2[] textureCoords)
        {
            VerticeData = new VertexPositionNormalTexture[NumCorners];
            VerticeData[0].Position = corners[0];
            VerticeData[1].Position = corners[1];
            VerticeData[2].Position = corners[2];
            VerticeData[0].TextureCoordinate = textureCoords[0];
            VerticeData[1].TextureCoordinate = textureCoords[1];
            VerticeData[2].TextureCoordinate = textureCoords[2];

        }
        public Vector3 CalcNormals()
        {
            SetNormals(PolygonLib.CalculateNormals(ref VerticeData[0].Position, ref VerticeData[1].Position, ref VerticeData[2].Position));
            return VerticeData[0].Normal;
        }
        public void SetNormals(Vector3 normal)
        {
            for (int i = 0; i < NumCorners; i++)
            {
                VerticeData[i].Normal = normal;
            }
        }
    }
}
