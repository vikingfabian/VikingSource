using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    struct PolygonsAndTrianglesColor : IPolygonsAndTriangles
    {
        public List<PolygonColor> Polygons;
        public List<TriangleColor> Triangles;
        public PolygonsAndTrianglesColor(List<PolygonColor> polygon)
            : this(polygon, new List<TriangleColor>())
        {  }
        public void AddRange(IPolygonsAndTriangles add)
        {
            PolygonsAndTrianglesColor conv = (PolygonsAndTrianglesColor)add;
            Polygons.AddRange(conv.Polygons);
        }

        public object GetPolygonVertex0(ref int polyIndex)
        {
            return Polygons[polyIndex].V0sw;
        }
        public object GetPolygonVertex1(ref int polyIndex)
        {
            return Polygons[polyIndex].V1nw;
        }
        public object GetPolygonVertex2(ref int polyIndex)
        {
            return Polygons[polyIndex].V2se;
        }
        public object GetPolygonVertex3(ref int polyIndex)
        {
            return Polygons[polyIndex].V3ne;
        }

        public VertexPositionColorTexture GetPolygonVertex0_coltex(ref int polyIndex)
        {
            return Polygons[polyIndex].V0sw;
        }
        public VertexPositionColorTexture GetPolygonVertex1_coltex(ref int polyIndex)
        {
            return Polygons[polyIndex].V1nw;
        }
        public VertexPositionColorTexture GetPolygonVertex2_coltex(ref int polyIndex)
        {
            return Polygons[polyIndex].V2se;
        }
        public VertexPositionColorTexture GetPolygonVertex3_coltex(ref int polyIndex)
        {
            return Polygons[polyIndex].V3ne;
        }

        public object GetTriangleVertex(int triangleIndex, int vertticeIx)
        {
            return Triangles[triangleIndex].VerticeData[vertticeIx];
        }
        public PolygonsAndTrianglesColor(List<PolygonColor> polygons, List<TriangleColor> triangles)
        { Polygons = polygons; Triangles = triangles; }
        public void Add(PolygonsAndTrianglesColor add)
        {
            Polygons.AddRange(add.Polygons);
            Triangles.AddRange(add.Triangles);
        }
        public void AddPosition(Vector3 add)
        {
            for (int i = 0; i < Polygons.Count; i++)
            {
                PolygonColor p = Polygons[i];
                p.V0sw.Position += add;
                p.V1nw.Position += add;
                p.V2se.Position += add;
                p.V3ne.Position += add;

                Polygons[i] = p;
            }
        }
        public static PolygonsAndTrianglesColor Empty
        { get { return new PolygonsAndTrianglesColor(new List<PolygonColor>(), new List<TriangleColor>()); } }
        public PolygonsAndTrianglesColor Clone()
        {
            List<PolygonColor> polys = new List<PolygonColor>();
            polys.AddRange(Polygons);
            return new PolygonsAndTrianglesColor(polys, new List<TriangleColor>());
        }
        public PolygonType Type { get { return PolygonType.Color; } }
        public int NumPolygons { get { return Polygons.Count; } }
        public int NumTriangles { get { return Triangles == null? 0 : Triangles.Count; } }
    }
    struct PolygonColor
    {
        
        public VertexPositionColorTexture V0sw;
        public VertexPositionColorTexture V1nw;
        public VertexPositionColorTexture V2se;
        public VertexPositionColorTexture V3ne;


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

        public PolygonColor(CubeFace facetype, SpriteName sprite,
            Color color, Vector3 blockPos)
        {

            Graphics.Sprite file = DataLib.SpriteCollection.Get(sprite);// LootFest.LootfestLib.Images.TileIxToImgeFile[tileIx];
            Face data = LootFest.Data.Block.GetVoxelObjFace(blockPos, facetype);

            V0sw = new VertexPositionColorTexture(data.Corner3, color, file.SourcePolygonLowLeft);
            V1nw = new VertexPositionColorTexture(data.Corner1, color, file.SourcePolygonTopLeft);
            V2se = new VertexPositionColorTexture(data.Corner4, color, file.SourcePolygonLowRight);
            V3ne = new VertexPositionColorTexture(data.Corner2, color, file.SourcePolygonTopRight);
        }

        public PolygonColor(int facetype, int tileIx, 
            Voxels.FaceCornerColorYS colors, IntVector3 blockPos)
        {

            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];

            Face data = LootFest.Data.Block.GetTerrainFace(blockPos, (CubeFace)facetype);

            //VerticeData = new VertexPositionColorTexture[NumCorners]
            //{ 
              V0sw=  new VertexPositionColorTexture(data.Corner3, colors.Col2,  file.SourcePolygonLowLeft);
              V1nw=  new VertexPositionColorTexture(data.Corner1, colors.Col0,  file.SourcePolygonTopLeft);
              V2se=  new VertexPositionColorTexture(data.Corner4, colors.Col3,  file.SourcePolygonLowRight);
              V3ne = new VertexPositionColorTexture(data.Corner2, colors.Col1, file.SourcePolygonTopRight);
            //};
        }

        public PolygonColor(int facetype, int tileIx,
            byte c0, byte c1, byte c2, byte c3, IntVector3 blockPos)
        {

            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];

            //opta bort förflyttning av blockpos
            Face data = LootFest.Data.Block.GetTerrainFace(blockPos, (CubeFace)facetype);
            //VerticeData = new VertexPositionColorTexture[NumCorners]
            //{ 
            V0sw= new VertexPositionColorTexture(data.Corner3, 
                Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c2],  file.SourcePolygonLowLeft);
            V1nw=new VertexPositionColorTexture(data.Corner1, 
                Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c0],  file.SourcePolygonTopLeft);
            V2se=new VertexPositionColorTexture(data.Corner4, 
                Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c3],  file.SourcePolygonLowRight);
            V3ne = new VertexPositionColorTexture(data.Corner2,
                Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c1], file.SourcePolygonTopRight);
            //};
        }
        public void SetTileAndCorners(int facetype, int tileIx,
            byte c0, byte c1, byte c2, byte c3)
        {
            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];

            V0sw.Color = Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c2];
            V0sw.TextureCoordinate = file.SourcePolygonLowLeft;

            V1nw.Color = Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c0];
            V1nw.TextureCoordinate = file.SourcePolygonTopLeft;

            V2se.Color = Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c3];
            V2se.TextureCoordinate = file.SourcePolygonLowRight;

            V3ne.Color = Voxels.FaceCornerColorYS.PreCalculatedColor_Face_CornerVal[facetype, c1];
            V3ne.TextureCoordinate = file.SourcePolygonTopRight;


        }

        public PolygonColor(Voxels.FaceCornerColorYS colors)
        { 
            V0sw=   new VertexPositionColorTexture(Vector3.Zero, colors.Col2, Vector2.Zero); //file.SourcePolygonLowLeft),
            V1nw=  new VertexPositionColorTexture(Vector3.Zero, colors.Col0, Vector2.Zero); //file.SourcePolygonTopLeft),
            V2se=  new VertexPositionColorTexture(Vector3.Zero, colors.Col3, Vector2.Zero); //file.SourcePolygonLowRight),
            V3ne = new VertexPositionColorTexture(Vector3.Zero, colors.Col1, Vector2.Zero); //file.SourcePolygonTopRight),
        }
        public void ComplementPositionAndTileIx(IntVector3 blockPos, int tileIx, int facetype)
        {
            Graphics.Sprite file = LootFest.LfRef.Images.TileIxToImgeFile[tileIx];
            Face data = LootFest.Data.Block.GetTerrainFace(blockPos, (CubeFace)facetype);

             V0sw.TextureCoordinate = file.SourcePolygonLowLeft;
            V1nw.TextureCoordinate = file.SourcePolygonTopLeft;
            V2se.TextureCoordinate = file.SourcePolygonLowRight;
            V3ne.TextureCoordinate = file.SourcePolygonTopRight;

             V0sw.Position = data.Corner3;
            V1nw.Position = data.Corner1;
            V2se.Position = data.Corner4;
            V3ne.Position = data.Corner2;
        }

        public PolygonColor(Face face, Graphics.Sprite file,
            Voxels.FaceCornerColor colors, IntVector3 pos)
        {
            face.Move(pos.Vec);
            //VerticeData = new VertexPositionColorTexture[NumCorners]
            //{ 
               V0sw=  new VertexPositionColorTexture(face.Corner3, colors.Col2,  file.SourcePolygonLowLeft);
               V1nw=  new VertexPositionColorTexture(face.Corner1, colors.Col0,  file.SourcePolygonTopLeft);
               V2se=  new VertexPositionColorTexture(face.Corner4, colors.Col3,  file.SourcePolygonLowRight);
               V3ne = new VertexPositionColorTexture(face.Corner2, colors.Col1, file.SourcePolygonTopRight);
            //};
        }

        public PolygonColor(Face face, Graphics.Sprite imageFile, Color color)
        { 
            V0sw=  new VertexPositionColorTexture(face.Corner3, color,  imageFile.SourcePolygonLowLeft);
            V1nw=  new VertexPositionColorTexture(face.Corner1, color,  imageFile.SourcePolygonTopLeft);
            V2se=  new VertexPositionColorTexture(face.Corner4, color,  imageFile.SourcePolygonLowRight);
            V3ne = new VertexPositionColorTexture(face.Corner2, color, imageFile.SourcePolygonTopRight);          
        }

        /// <param name="corners">Corner positions in the order NW, NE, SW, SE</param>
        public PolygonColor(Vector3[] corners, Sprite imageFile, Color color)
        {
            V0sw = new VertexPositionColorTexture(corners[CornerLowLeft], color, imageFile.SourcePolygonLowLeft);
            V1nw = new VertexPositionColorTexture(corners[CornerTopLeft], color, imageFile.SourcePolygonTopLeft);
            V2se = new VertexPositionColorTexture(corners[CornerLowRight], color, imageFile.SourcePolygonLowRight);
            V3ne = new VertexPositionColorTexture(corners[CornerTopRight], color, imageFile.SourcePolygonTopRight);
        }

        public PolygonColor(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se, Sprite imageFile, Color color)
        {
            V0sw = new VertexPositionColorTexture(sw, color, imageFile.SourcePolygonLowLeft);
            V1nw = new VertexPositionColorTexture(nw, color, imageFile.SourcePolygonTopLeft);
            V2se = new VertexPositionColorTexture(se, color, imageFile.SourcePolygonLowRight);
            V3ne = new VertexPositionColorTexture(ne, color, imageFile.SourcePolygonTopRight);
        }

        public PolygonColor(Vector3 nw, Vector3 ne, Vector3 sw, Vector3 se, SpriteName sprite, Dir4 spriteRotation, Color color)
        {
            V0sw = new VertexPositionColorTexture(sw, color, Vector2.Zero);
            V1nw = new VertexPositionColorTexture(nw, color, Vector2.Zero);
            V2se = new VertexPositionColorTexture(se, color, Vector2.Zero);
            V3ne = new VertexPositionColorTexture(ne, color, Vector2.Zero);

            setSprite(sprite, spriteRotation);
        }

        public static PolygonColor QuadXZ(Vector2 pos, Vector2 size, bool centered, float height, SpriteName sprite, Dir4 spriteRot, Color color)
        {
            Graphics.PolygonColor poly = new Graphics.PolygonColor();

            //size *= 0.5f;
            pos.Y += size.Y;
            if (centered)
            {
                pos -= size * 0.5f;
            }

            Vector3 topLeft = VectorExt.V2toV3XZ(pos, height);

            //Top left
            poly.V1nw.Position = topLeft;
            //Top right
            poly.V0sw.Position = topLeft;
            poly.V0sw.Position.X += size.X;
            //Bottom left
            poly.V3ne.Position = topLeft;
            poly.V3ne.Position.Z -= size.Y;
            //Bottom right
            poly.V2se.Position = topLeft;
            poly.V2se.Position.X += size.X;
            poly.V2se.Position.Z -= size.Y;

            poly.setSprite(sprite, spriteRot);

            poly.V0sw.Color = color;
            poly.V1nw.Color = color;
            poly.V2se.Color = color;
            poly.V3ne.Color = color;

            return poly;
        }

        public static PolygonColor QuadXZ(Vector3 center, Vector2 halfsize, float rotation, SpriteName sprite, Dir4 spriteRot, Color color)
        {
            Graphics.PolygonColor poly = new Graphics.PolygonColor();
           // rotation = 0;
            Vector2 brRot = -VectorExt.RotateVector(halfsize, rotation);
            //Vector3 topLeft = VectorExt.V2toV3XZ(-brRot) + center;

            //TODO rotera bara offset

            //Top left
            poly.V0sw.Position = VectorExt.V2toV3XZ(brRot) + center;
            //Top right
            brRot = VectorExt.RotateVector90DegreeRight(brRot);
            poly.V1nw.Position = VectorExt.V2toV3XZ(brRot) + center;
            //Bottom right
            brRot = VectorExt.RotateVector90DegreeRight(brRot);
            poly.V3ne.Position = VectorExt.V2toV3XZ(brRot) + center;
            //Bottom left
            brRot = VectorExt.RotateVector90DegreeRight(brRot);
            poly.V2se.Position = VectorExt.V2toV3XZ(brRot) + center;

            poly.setSprite(sprite, spriteRot);

            poly.V0sw.Color = color;
            poly.V1nw.Color = color;
            poly.V2se.Color = color;
            poly.V3ne.Color = color;

            return poly;
        }


        static readonly int[,] Rotation_VerticeCorner = new int[4, 4]
        {
            { CornerLowLeft, CornerTopLeft, CornerLowRight, CornerTopRight, },
            { CornerLowRight, CornerLowLeft, CornerTopRight, CornerTopLeft,},
            { CornerTopRight,CornerLowRight,  CornerTopLeft, CornerLowLeft, },
            { CornerTopLeft, CornerTopRight,CornerLowLeft, CornerLowRight,},
        };

        public void SetColor(Color col)
        {
            V0sw.Color = col;
            V1nw.Color = col;
            V2se.Color = col;
            V3ne.Color = col;
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
                    V1nw.TextureCoordinate = tileSource.Position;
                    V3ne.TextureCoordinate = tileSource.RightTop;
                    V0sw.TextureCoordinate = tileSource.LeftBottom;
                    V2se.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.E:
                    V0sw.TextureCoordinate = tileSource.Position;
                    V1nw.TextureCoordinate = tileSource.RightTop;
                    V2se.TextureCoordinate = tileSource.LeftBottom;
                    V3ne.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.S:
                    V2se.TextureCoordinate = tileSource.Position;
                    V0sw.TextureCoordinate = tileSource.RightTop;
                    V3ne.TextureCoordinate = tileSource.LeftBottom;
                    V1nw.TextureCoordinate = tileSource.RightBottom;
                    break;
                case Dir4.W:
                    V3ne.TextureCoordinate = tileSource.Position;
                    V2se.TextureCoordinate = tileSource.RightTop;
                    V1nw.TextureCoordinate = tileSource.LeftBottom;
                    V0sw.TextureCoordinate = tileSource.RightBottom;
                    break;
            }
        }

        /// <param name="corners">Corner positions in the order NW, NE, SW, SE</param>
        /// 
        static readonly VertexPositionColorTexture EmptyVertex = new VertexPositionColorTexture(Vector3.Zero, Color.White, Vector2.Zero);
        public PolygonColor(Vector3[] corners, int tileIx, VectorRect tileSource, Dir4 tileRotation)
        {           
          
            V0sw = EmptyVertex;
            V1nw = EmptyVertex;
            V2se = EmptyVertex;
            V3ne = EmptyVertex;

            V0sw.Position = corners[CornerLowLeft];
            V1nw.Position = corners[CornerTopLeft];
            V2se.Position = corners[CornerLowRight];
            V3ne.Position = corners[CornerTopRight];
            
            

            //Graphics.Sprite file = DataLib.SpriteCollection.Sprites[tileIx];
            VectorRect source = DataLib.SpriteCollection.Sprites[tileIx].SourceF;
            Vector2 start = source.Position;
            start.X += tileSource.Position.X * source.Width;
            start.Y += tileSource.Position.Y * source.Height;
            Vector2 steps = source.Size;
            steps.X = steps.X * tileSource.Width;
            steps.Y = steps.Y * tileSource.Height;


            int[] TextureCoords = FacesTextureCoords[tileRotation];

            this.SetTextureCoord(TextureCoords[CornerTopLeft], start);
            this.SetTextureCoord(TextureCoords[CornerTopRight], new Vector2(start.X + steps.X, start.Y));
            this.SetTextureCoord(TextureCoords[CornerLowLeft], new Vector2(start.X, start.Y + steps.Y));
            this.SetTextureCoord(TextureCoords[CornerLowRight], new Vector2(start.X + steps.X, start.Y + steps.Y));

           
        }

        public Vector3 Center()
        {
            return (V0sw.Position + V1nw.Position + V2se.Position + V3ne.Position) / 4f;
        }

        public float CenterZ()
        {
            return (V0sw.Position.Z + V1nw.Position.Z + V2se.Position.Z + V3ne.Position.Z) / 4f;
        }

        public Vector3 Normal()
        {
            return PolygonLib.CalculateNormals(ref V0sw.Position, ref V1nw.Position, ref V2se.Position);
        }
        

        public void Move(Vector3 move)
        {
            V0sw.Position += move;
            V1nw.Position += move;
            V2se.Position += move;
            V3ne.Position += move;
        }

        public VertexPositionColorTexture Get(int index)
        {
            switch (index)
            {
                default:
                    return V0sw;
                case 1:
                    return V1nw;
                case 2:
                    return V2se;
                case 3:
                    return V3ne;
            }
        }
        public void Set(int index, VertexPositionColorTexture vertex)
        {
            switch (index)
            {
                default:
                    V0sw = vertex;
                    break;
                case 1:
                    V1nw = vertex;
                    break;
                case 2:
                    V2se = vertex;
                    break;
                case 3:
                    V3ne = vertex;
                    break;
            }
        }
        public void SetTextureCoord(int index, Vector2 value)
        {
            switch (index)
            {
                default:
                    V0sw.TextureCoordinate = value;
                    break;
                case 1:
                    V1nw.TextureCoordinate = value;
                    break;
                case 2:
                    V2se.TextureCoordinate = value;
                    break;
                case 3:
                    V3ne.TextureCoordinate = value;
                    break;
            }
        }

        public bool hasValue()
        {
            return VectorExt.HasValue(V1nw.Position) || VectorExt.HasValue(V2se.Position);
        }

        //All vertices HAVE TO lie in one Plane
        public static List<PolygonColor> PlaneWithDivitions(Vector3[] corners, int tileIx, VectorRect tileSource, 
            Dir4 tileRotation, IntVector2 divitions)
        {
            Vector3 topLeft = corners[CornerTopLeft];
            Vector3 stepX = corners[CornerTopRight] - topLeft;
            Vector3 stepY = corners[CornerLowLeft] - topLeft;

            stepX /= divitions.X;
            stepY /= divitions.Y;

            List<PolygonColor> polygons = new List<PolygonColor>();
            for (int y = 0; y < divitions.Y; y++)
            {
                for (int x = 0; x < divitions.X; x++)
                {
                    polygons.Add(new PolygonColor(new Vector3[] {
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
    }
    
    
    struct TriangleColor
    {
        public VertexPositionColorTexture[] VerticeData;
        public const int NumCorners = 3;
        
        /// <param name="corners">Corner positions in the order NW, NE, SW, SE</param>
        public TriangleColor(Vector3[] corners, Vector2[] textureCoords, Color color)
        {
            VerticeData = new VertexPositionColorTexture[NumCorners];
            VerticeData[0].Position = corners[0];
            VerticeData[1].Position = corners[1];
            VerticeData[2].Position = corners[2];
            VerticeData[0].TextureCoordinate = textureCoords[0];
            VerticeData[1].TextureCoordinate = textureCoords[1];
            VerticeData[2].TextureCoordinate = textureCoords[2];
            VerticeData[0].Color = color;
            VerticeData[1].Color = color;
            VerticeData[2].Color = color;

       }
       
    }
}

