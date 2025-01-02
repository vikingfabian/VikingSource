using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    interface IPolygonsAndTriangles
    {
        int NumPolygons { get; }
        int NumTriangles { get; }

        PolygonType Type { get; }
        //object GetPolygonVertex(ref int polyIndex, ref int vertticeIx);
        object GetPolygonVertex0(ref int polyIndex);
        object GetPolygonVertex1(ref int polyIndex);
        object GetPolygonVertex2(ref int polyIndex);
        object GetPolygonVertex3(ref int polyIndex);
        object GetTriangleVertex(int triangleIndex, int vertticeIx);
        void AddRange(IPolygonsAndTriangles add);
    }

    
    interface IVerticeData
    {
        PolygonType Type { get; }
        VerticeDrawOrderData DrawData { get; }
        VertexDeclaration VertexDeclaration { get; }
        void SetVertice(int index, object vertice);
        
        void SetVertexBuffer(VertexBuffer VB);
    }
    struct VerticeDrawOrderData
    {
        public ushort[] indexDrawOrder16;
        public uint[] indexDrawOrder32;
        public int numVertices;
        public int numTriangles;
        public int indexDrawOrderLength;

        public IndexElementSize bitSize;

        public VerticeDrawOrderData(StaticCountingList<int> indexDrawOrder, ushort[] recycledIndexDrawOrder16, uint[] recycledIndexDrawOrder32)
            //: this(indexDrawOrder.Array, indexDrawOrder.CountingLength)
        {
            indexDrawOrderLength = indexDrawOrder.CountingLength;
            numTriangles = indexDrawOrder.CountingLength / 3;
            numVertices = numTriangles * 2;

            bitSize = drawOrderLengthToBitSz(indexDrawOrderLength);
            if (bitSize == IndexElementSize.SixteenBits)
            {
                indexDrawOrder16 = recycledIndexDrawOrder16;
                indexDrawOrder32 = null;

                for (int i = 0; i < indexDrawOrderLength; ++i)
                {
                    indexDrawOrder16[i] = (ushort)indexDrawOrder[i];
                }
            }
            else
            {
                indexDrawOrder16 = null;
                indexDrawOrder32 = recycledIndexDrawOrder32;

                for (int i = 0; i < indexDrawOrderLength; ++i)
                {
                    indexDrawOrder32[i] = (uint)indexDrawOrder[i];
                }
            }
        }

        public VerticeDrawOrderData(List<int> indexDrawOrder)
            :this(indexDrawOrder.ToArray(), indexDrawOrder.Count)
        {
        }

        public void SetDrawOrder(int index, int order)
        {
            if (bitSize == IndexElementSize.SixteenBits)
            {
                indexDrawOrder16[index] = (ushort)order;
            }
            else
            {
                indexDrawOrder32[index] = (uint)order;
            }
        }

        public VerticeDrawOrderData(int[] indexDrawOrder, int useLength)
        {
            indexDrawOrderLength = useLength;
            numTriangles = useLength / 3;
            numVertices = numTriangles * 2;
            
            bitSize = drawOrderLengthToBitSz(indexDrawOrderLength);
            if (bitSize == IndexElementSize.SixteenBits)
            {
                indexDrawOrder16 = new UInt16[indexDrawOrderLength];
                indexDrawOrder32 = null;

                for (int i = 0; i < indexDrawOrderLength; ++i)
                {
                    indexDrawOrder16[i] = (ushort)indexDrawOrder[i];
                }
            }
            else
            {
                indexDrawOrder16 = null;
                indexDrawOrder32 = new uint[indexDrawOrderLength];

                for (int i = 0; i < indexDrawOrderLength; ++i)
                {
                    indexDrawOrder32[i] = (uint)indexDrawOrder[i];
                }
            }
        }

        public VerticeDrawOrderData(int polyCount, int triangleCount)
        {
            numVertices = polyCount * PolygonLib.NumCornersPolygon + triangleCount * PolygonLib.NumCornersTriangle;
            numTriangles = polyCount * 2 + triangleCount;
            indexDrawOrderLength = PolygonLib.NumDrawIxPerPoly * polyCount + PolygonLib.NumDrawIxPerTriangle * triangleCount;
            
            //indexDrawOrder = new UInt16[indexDrawOrderLength];

            bitSize = drawOrderLengthToBitSz(indexDrawOrderLength);
            if (bitSize == IndexElementSize.SixteenBits)
            {
                indexDrawOrder16 = new UInt16[indexDrawOrderLength];
                indexDrawOrder32 = null;
            }
            else
            {
                indexDrawOrder16 = null;
                indexDrawOrder32 = new uint[indexDrawOrderLength];
            }
        }

        static IndexElementSize drawOrderLengthToBitSz(int drawOrderLength)
        {
            return drawOrderLength < ushort.MaxValue ? IndexElementSize.SixteenBits : IndexElementSize.ThirtyTwoBits;
        }
    }

    class VerticeDataNormal : IVerticeData
    {
        VertexPositionNormalTexture[] Vertices;
        VerticeDrawOrderData drawOrderData;

        public VerticeDataNormal(StaticCountingList<VertexPositionNormalTexture> vertices, StaticCountingList<int> indexDrawOrder, UInt16[] recycledIndexDrawOrder16, uint[] recycledIndexDrawOrder32)
        {
            this.Vertices = vertices.Array;
            drawOrderData = new VerticeDrawOrderData(indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
        }


        public VerticeDataNormal(List<VertexPositionNormalTexture> vertices, List<int> indexDrawOrder)
        {
            Vertices = vertices.ToArray();
            drawOrderData = new VerticeDrawOrderData(indexDrawOrder);
        }

        public VerticeDataNormal(int polyCount, int triangleCount)
        {
            drawOrderData = new VerticeDrawOrderData(polyCount, triangleCount);
            Vertices = new VertexPositionNormalTexture[drawOrderData.numVertices];

        }
        public void SetVertice(int index, object vertice)
        {
            //VertexPositionColorNormal
            Vertices[index] = (VertexPositionNormalTexture)vertice;
        }
        public PolygonType Type { get { return PolygonType.Normal; } }
        public VerticeDrawOrderData DrawData { get { return drawOrderData; } }
        public VertexDeclaration VertexDeclaration { get { return VertexPositionNormalTexture.VertexDeclaration; } }
        public void SetVertexBuffer(VertexBuffer VB)
        {
            VB.SetData(Vertices, 0, DrawData.numVertices);
        }
    }

    class VerticeDataColorTexture : IVerticeData
    {
        public ArrayExposedList<VertexPositionColorTexture> Vertices;
        VerticeDrawOrderData drawOrderData;

        public VerticeDataColorTexture(List<VertexPositionColorTexture> vertices, List<int> indexDrawOrder)
        {
            Vertices = new ArrayExposedList<VertexPositionColorTexture>(vertices);
            drawOrderData = new VerticeDrawOrderData(indexDrawOrder);
        }

        public VerticeDataColorTexture(int polyCount, int triangleCount)//pooling
        {
            drawOrderData = new VerticeDrawOrderData(polyCount, triangleCount);
            Vertices = new ArrayExposedList<VertexPositionColorTexture>(drawOrderData.numVertices);
            Vertices.setLenght(drawOrderData.numVertices);
        }

        public void recycle(int polyCount, int triangleCount)//pooling
        {
            drawOrderData = new VerticeDrawOrderData(polyCount, triangleCount);
            Vertices.setLenght(drawOrderData.numVertices);
        }
        public void SetVertice(int index, object vertice)
        {
            Vertices.array[index] = (VertexPositionColorTexture)vertice;
        }

        public void SetVertice(int index, VertexPositionColorTexture vertice)
        {
            Vertices.array[index] = vertice;
        }


        public PolygonType Type { get { return PolygonType.Color; } }
        public VerticeDrawOrderData DrawData { get { return drawOrderData; } }
        public VertexDeclaration VertexDeclaration { get { return VertexPositionColorTexture.VertexDeclaration; } }
        public void SetVertexBuffer(VertexBuffer VB)
        {
            VB.SetData(Vertices.array, 0, Vertices.count);
        }
    }

    class VerticeDataColorNormal : IVerticeData
    {
        public VertexPositionColorNormal[] Vertices;
        VerticeDrawOrderData drawOrderData;

        public VerticeDataColorNormal(List<VertexPositionColorNormal> vertices, List<int> indexDrawOrder)
        {
            Vertices = vertices.ToArray();
            drawOrderData = new VerticeDrawOrderData(indexDrawOrder);
        }

        public VerticeDataColorNormal(int polyCount, int triangleCount)
        {
            drawOrderData = new VerticeDrawOrderData(polyCount, triangleCount);
            Vertices = new VertexPositionColorNormal[drawOrderData.numVertices];
        }
        public void SetVertice(int index, object vertice)
        {
            Vertices[index] = (VertexPositionColorNormal)vertice;
        }
        public PolygonType Type { get { return PolygonType.ColorAndNormal; } }
        public VerticeDrawOrderData DrawData { get { return drawOrderData; } }
        public VertexDeclaration VertexDeclaration { get { return VertexPositionColorNormal.VertexDeclaration; } }
        public void SetVertexBuffer(VertexBuffer VB)
        {
            VB.SetData(Vertices);
        }
    }


    class VerticeDataColor : IVerticeData
    {
        public VertexPositionColor[] Vertices;
        VerticeDrawOrderData drawOrderData;

        public VerticeDataColor(int polyCount, int triangleCount)
        {
            drawOrderData = new VerticeDrawOrderData(polyCount, triangleCount);
            Vertices = new VertexPositionColor[drawOrderData.numVertices];
        }

        public VerticeDataColor(StaticCountingList<VertexPositionColor> vertices, StaticCountingList<int> indexDrawOrder, UInt16[] recycledIndexDrawOrder16, uint[] recycledIndexDrawOrder32)
        {
            this.Vertices = vertices.ToArray();
            drawOrderData = new VerticeDrawOrderData(indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
        }

        public void SetVertice(int index, object vertice)
        {
            Vertices[index] = (VertexPositionColor)vertice;
        }
        public PolygonType Type { get { return PolygonType.Color; } }
        public VerticeDrawOrderData DrawData { get { return drawOrderData; } }
        public VertexDeclaration VertexDeclaration { get { return VertexPositionColor.VertexDeclaration; } }
        public void SetVertexBuffer(VertexBuffer VB)
        {
            VB.SetData(Vertices);
        }
    }

    struct Face
    {
        public Vector3 Corner1;
        public Vector3 Corner2;
        public Vector3 Corner3;
        public Vector3 Corner4;

        public Vector3 Normal;


        // Constructor with individual corners and auto-calculated normal
        public Face(Vector3 corner1, Vector3 corner2, Vector3 corner3, Vector3 corner4)
        {
            Corner1 = corner1;
            Corner2 = corner2;
            Corner3 = corner3;
            Corner4 = corner4;

            Normal = PolygonLib.CalculateNormals(ref Corner1, ref Corner2, ref Corner3);
        }

        // Constructor with individual corners and provided normal
        public Face(Vector3 corner1, Vector3 corner2, Vector3 corner3, Vector3 corner4, Vector3 normal)
        {
            Corner1 = corner1;
            Corner2 = corner2;
            Corner3 = corner3;
            Corner4 = corner4;

            Normal = normal;
        }


        public Face(Vector3[] corners)
        {
            Corner1 = corners[0];
            Corner2 = corners[1];
            Corner3 = corners[2];
            Corner4 = corners[3];

            Normal = PolygonLib.CalculateNormals(ref Corner1, ref Corner2, ref Corner3);

        }

        public Face(Vector3[] corners, Vector3 normal)
        {
            Corner1 = corners[0];
            Corner2 = corners[1];
            Corner3 = corners[2];
            Corner4 = corners[3];

            Normal = normal;//Graphics.GeneratedObj.CalculateNormals(ref Corner1, ref Corner2, ref Corner3);

        }

        public Face Clone(Vector3 move)
        {
            Face result = this;
            result.Move(move);
            return result;
        }
        public void Move(Vector3 pos)
        {

            Corner1.X += pos.X;
            Corner1.Y += pos.Y;
            Corner1.Z += pos.Z;

            Corner2.X += pos.X;
            Corner2.Y += pos.Y;
            Corner2.Z += pos.Z;

            Corner3.X += pos.X;
            Corner3.Y += pos.Y;
            Corner3.Z += pos.Z;

            Corner4.X += pos.X;
            Corner4.Y += pos.Y;
            Corner4.Z += pos.Z;

        }
        public void Move(IntVector3 pos)
        {

            Corner1.X += pos.X;
            Corner1.Y += pos.Y;
            Corner1.Z += pos.Z;

            Corner2.X += pos.X;
            Corner2.Y += pos.Y;
            Corner2.Z += pos.Z;

            Corner3.X += pos.X;
            Corner3.Y += pos.Y;
            Corner3.Z += pos.Z;

            Corner4.X += pos.X;
            Corner4.Y += pos.Y;
            Corner4.Z += pos.Z;

        }

        public void Move(IntVector3 pos, float blockScale)
        {

            Corner1.X += pos.X * blockScale;
            Corner1.Y += pos.Y * blockScale;
            Corner1.Z += pos.Z * blockScale;

            Corner2.X += pos.X * blockScale;
            Corner2.Y += pos.Y * blockScale;
            Corner2.Z += pos.Z * blockScale;

            Corner3.X += pos.X * blockScale;
            Corner3.Y += pos.Y * blockScale;
            Corner3.Z += pos.Z * blockScale;

            Corner4.X += pos.X * blockScale;
            Corner4.Y += pos.Y * blockScale;
            Corner4.Z += pos.Z * blockScale;

        }
    }
    static class PolygonLib
    {
        public static ConcurrentStack<VerticeDataColorTexture> VerticeDataPool = new ConcurrentStack<VerticeDataColorTexture>();

        static readonly int[] BasicIndexDrawOrder = new int[] { 0, 1, 2, 2, 1, 3 };
        public const int NumCornersPolygon = 4;
        public const int NumCornersTriangle = 3;
        public const int NumDrawIxPerPoly = 6;
        public const int NumDrawIxPerTriangle = 6; //??

        public static VerticeDataColorTexture BuildVDFromPolygons(PolygonsAndTrianglesColor polygonsAndTriangles)
        {
            VerticeDataColorTexture verticeData;
            if (VerticeDataPool.TryPop(out verticeData))
            {
                verticeData.recycle(polygonsAndTriangles.NumPolygons, polygonsAndTriangles.NumTriangles);
            }
            else
            {
                verticeData = new VerticeDataColorTexture(polygonsAndTriangles.NumPolygons, polygonsAndTriangles.NumTriangles);//VerticeDataColorTexture(polygonsAndTriangles.NumPolygons, polygonsAndTriangles.NumTriangles);
            }

            VerticeDrawOrderData drawOrder = verticeData.DrawData;
            int totalVerticeIx = 0;
            int indexDrawOrderPointer = 0;

            for (int polyIx = 0; polyIx < polygonsAndTriangles.NumPolygons; polyIx++)
            {
                verticeData.SetVertice(totalVerticeIx, polygonsAndTriangles.GetPolygonVertex0_coltex(ref polyIx));
                totalVerticeIx++;
                verticeData.SetVertice(totalVerticeIx, polygonsAndTriangles.GetPolygonVertex1_coltex(ref polyIx));
                totalVerticeIx++;
                verticeData.SetVertice(totalVerticeIx, polygonsAndTriangles.GetPolygonVertex2_coltex(ref polyIx));
                totalVerticeIx++;
                verticeData.SetVertice(totalVerticeIx, polygonsAndTriangles.GetPolygonVertex3_coltex(ref polyIx));
                totalVerticeIx++;

                for (int drawOrderIx = 0; drawOrderIx < NumDrawIxPerPoly; drawOrderIx++)
                {
                    drawOrder.SetDrawOrder(indexDrawOrderPointer, BasicIndexDrawOrder[drawOrderIx] + polyIx * PolygonLib.NumCornersPolygon);
                    indexDrawOrderPointer++;
                }
            }
            for (int triIx = 0; triIx < polygonsAndTriangles.NumTriangles; triIx++)
            {
                for (int corner = 0; corner < PolygonLib.NumCornersTriangle; corner++)
                {
                    verticeData.SetVertice(totalVerticeIx, polygonsAndTriangles.GetTriangleVertex(triIx, corner));
                    drawOrder.SetDrawOrder(indexDrawOrderPointer, totalVerticeIx);
                    totalVerticeIx++;
                    indexDrawOrderPointer++;
                }

            }
            return verticeData;
        }
        
        //    else
        //    {
        //        //används inte
        //        VerticeDataNormal verticeData = new VerticeDataNormal(polygonsAndTriangles.NumPolygons, polygonsAndTriangles.NumTriangles);
        //        throw new NotImplementedException();
        //    }
            
        //}
        public static Vector3 CalculateNormals(ref Vector3 vertexA, ref Vector3 vertexB, ref Vector3 vertexC)
        { //vectexA to C is the three corners of the surface triangle
            if (vertexA.Y == vertexB.Y)
            {
                if (vertexA.Y == vertexC.Y)
                {
                    return Vector3.Down;
                }
            }

            //Vector3 SurfaceVec1 = vertexB - vertexA;
            Vector3 SurfaceVec1 = vertexB;
            SurfaceVec1.X -= vertexA.X;
            SurfaceVec1.Y -= vertexA.Y;
            SurfaceVec1.Z -= vertexA.Z;

            Vector3 SurfaceVec2 = vertexC;
            SurfaceVec2.X -= vertexA.X;
            SurfaceVec2.Y -= vertexA.Y;
            SurfaceVec2.Z -= vertexA.Z;
            //Vector3 SurfaceVec2 = vertexC - vertexA;

            Vector3 normal = Vector3.Zero;
            normal.X = (SurfaceVec1.Y * SurfaceVec2.Z) - (SurfaceVec1.Z * SurfaceVec2.Y);
            normal.Y = -((SurfaceVec2.Z * SurfaceVec1.X) - (SurfaceVec2.X * SurfaceVec1.Z));
            normal.Z = (SurfaceVec1.X * SurfaceVec2.Y) - (SurfaceVec1.Y * SurfaceVec2.X);
            normal.Normalize();

            return normal;
        }

        public static AbsVertexAndIndexBuffer BuildVBFromPolygons(PolygonsAndTrianglesColor polygonsAndTriangles)
        {
            IVerticeData verticeData = BuildVDFromPolygons(polygonsAndTriangles);
            VertexAndIndexBuffer result = new VertexAndIndexBuffer(verticeData);
            
            return result;
        }
        public static AbsVertexAndIndexBuffer BuildVBAnimatedFromPolygons(List<PolygonsAndTrianglesColor> polygonsAndTriangles)
        {
            PolygonsAndTrianglesColor polyColl = polygonsAndTriangles[0];
            List<int> numPolys = new List<int> { polyColl.NumPolygons };
            for (int i = 1; i < polygonsAndTriangles.Count; i++)
            {
                polyColl.AddRange(polygonsAndTriangles[i]);
                numPolys.Add(polygonsAndTriangles[i].NumPolygons);
            }
            IVerticeData verticeData = BuildVDFromPolygons(polyColl);
            return new VertexAndIndexBufferAnimated(verticeData, numPolys);
        }

        public static Face[] createFaces(float scale)
        {
            Face[] faces = new Face[(int)CubeFace.NUM];
            float radius = scale * PublicConstants.Half;

            // Define top corners
            Vector3 topCornerTopLeft = new Vector3(radius, -radius, -radius);
            Vector3 topCornerTopRight = new Vector3(-radius, -radius, -radius);
            Vector3 topCornerLowLeft = new Vector3(radius, -radius, radius);
            Vector3 topCornerLowRight = new Vector3(-radius, -radius, radius);

            // Define bottom corners
            Vector3 bottomCornerTopLeft = new Vector3(radius, radius, -radius);
            Vector3 bottomCornerTopRight = new Vector3(-radius, radius, -radius);
            Vector3 bottomCornerLowLeft = new Vector3(radius, radius, radius);
            Vector3 bottomCornerLowRight = new Vector3(-radius, radius, radius);

            // Create faces using the new constructor
            faces[(int)CubeFace.Ynegative] = new Face(
                topCornerTopLeft, topCornerTopRight, topCornerLowLeft, topCornerLowRight,
                new Vector3(0, -1, 0)
            );

            faces[(int)CubeFace.Ypositive] = new Face(
                bottomCornerTopRight, bottomCornerTopLeft, bottomCornerLowRight, bottomCornerLowLeft,
                new Vector3(0, 1, 0)
            );

            faces[(int)CubeFace.Zpositive] = new Face(
                bottomCornerLowRight, bottomCornerLowLeft, topCornerLowRight, topCornerLowLeft,
                new Vector3(0, 0, 1)
            );

            faces[(int)CubeFace.Znegative] = new Face(
                bottomCornerTopLeft, bottomCornerTopRight, topCornerTopLeft, topCornerTopRight,
                new Vector3(0, 0, -1)
            );

            faces[(int)CubeFace.Xnegative] = new Face(
                bottomCornerTopRight, bottomCornerLowRight, topCornerTopRight, topCornerLowRight,
                new Vector3(-1, 0, 0)
            );

            faces[(int)CubeFace.Xpositive] = new Face(
                bottomCornerLowLeft, bottomCornerTopLeft, topCornerLowLeft, topCornerTopLeft,
                new Vector3(1, 0, 0)
            );

            return faces;
        }



    }

    enum PolygonType
    {
        Color,
        Normal,
        ColorAndNormal
    }

    enum AssignNormals
    {
        DontAssign,
        AsPlane,
        Idividual,
    }
}