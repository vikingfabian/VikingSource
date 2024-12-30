using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    abstract class AbsVertexAndIndexBuffer : Abs3DModel
    {
        /* Properties */
        public abstract int NumFrames { get; }
        public override float Opacity
        {
            get { return opacity; }
            set { opacity = Bound.Set(value, 0f, 1f); }
        }
        public override DrawObjType DrawType { get { return DrawObjType.NotDrawable; } }
        public override Color Color
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Correct { get { 
            return vertexBuffer_GPU != null && vertexBuffer_GPU.VertexCount > 0; } }

        protected override bool drawable { get { return false; } }

        /* Fields */
        protected VertexBuffer vertexBuffer_GPU;
        protected IndexBuffer indexBuffer;

        private float opacity = 1;

        /* Constructors */
        public AbsVertexAndIndexBuffer(IVerticeData verticeData)
            :base(false)
        {
            if (verticeData == null || verticeData.DrawData.numVertices == 0 || Ref.update.exitApplication)
            {
                return;
            }
            try
            {
                vertexBuffer_GPU = new VertexBuffer(Engine.Draw.graphicsDeviceManager.GraphicsDevice,
                        verticeData.VertexDeclaration,
                        verticeData.DrawData.numVertices, BufferUsage.WriteOnly);
                verticeData.SetVertexBuffer(vertexBuffer_GPU);

                indexBuffer = new IndexBuffer(Engine.Draw.graphicsDeviceManager.GraphicsDevice, verticeData.DrawData.bitSize,
                    verticeData.DrawData.indexDrawOrderLength, BufferUsage.WriteOnly);

                if (verticeData.DrawData.bitSize == IndexElementSize.SixteenBits)
                {
                    indexBuffer.SetData(verticeData.DrawData.indexDrawOrder16, 0, verticeData.DrawData.indexDrawOrderLength);
                }
                else
                {
                    indexBuffer.SetData(verticeData.DrawData.indexDrawOrder32, 0, verticeData.DrawData.indexDrawOrderLength);
                }

                //lösning på crash
                //System.Threading.Thread.Sleep(2);//Ref.main.TargetElapsedTime)
            }
            catch (Exception e)
            {
                Debug.LogError("AbsVertexAndIndexBuffer, " + e.Message);
                vertexBuffer_GPU = null;
            }
        }

        /* Family methods */
        public override AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        { }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        { }
        public override void copyAllDataFrom(AbsDraw master)
        {
            throw new NotImplementedException();
        }
        public override void SetSpriteName(SpriteName name)
        {
            throw new NotImplementedException();
        }
        /* Novelty methods */
        public void SetBuffer()
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(vertexBuffer_GPU);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = indexBuffer;
        }
    }

    class VertexAndIndexBuffer : AbsVertexAndIndexBuffer
    {
        /* Properties */
        public override int NumFrames { get { return 1; } }

        /* Fields */
        public Vector3 Position;
        public BoundingSphere CullingBound;
        //int numVertices;
        int numTriangles;

        /* Constructors */
        public VertexAndIndexBuffer(IVerticeData verticeData)
            : base(verticeData)
        {
            //numVertices = verticeData.DrawData.numVertices;
            numTriangles = verticeData.DrawData.numTriangles;
        }

        /* Family methods */
        public override void Draw(int frame)
        {
            if (numTriangles > 0)
            {
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                    0, numTriangles);
            }
        }
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        {
            boundingSphere = CullingBound;
        }

        /* Novelty methods */
        public void Draw()
        {
            this.Draw(0);
        }
    }

    class VertexAndIndexBufferAnimated : AbsVertexAndIndexBuffer
    {
        /* Properties */
        override public int NumFrames { get { return frames.Count; } }

        /* Fields */
        List<Frame> frames;
        public Vector3 Position;
        public BoundingSphere CullingBound;

        /* Constructors */
        public VertexAndIndexBufferAnimated(IVerticeData verticeData, List<int> numPolygons)
            : base(verticeData)
        {
            this.frames = new List<Frame>();
            int currentPos = 0;
            foreach (int num in numPolygons)
            {
                Frame frameData = new Frame(currentPos, num);
                this.frames.Add(frameData);
                currentPos += num;
            }
        }

        public VertexAndIndexBufferAnimated(IVerticeData verticeData, List<Frame> frames)
            : base(verticeData)
        {
            this.frames = frames;
        }

        /* Family methods */
        public override void Draw(int frame)
        {
            if (frames.Count > 0)
            {
                if (frame < 0 || frame >= frames.Count)
                {
                    frame = 0;
                }
                Frame f = frames[frame];

                if (f.numVertices > 0)
                {
                    Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, f.startDrawOrderIndex, f.primitiveCount);
                }
            }
        }
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        {
            boundingSphere = CullingBound;
        }
    }
    struct Frame
    {
        /* Constants */
        const int VerticesPerPoly = 4;
        const int DrawOrderIndexesPerPoly = 6;

        /* Fields */
        public int minVertexIndex; //start polys * 4
        public int numVertices;//num polys * 4
        public int startDrawOrderIndex; //start polys * 6, positionen i Index Buffer
        public int primitiveCount; //num triangles, num polys * 2

        /* Constructors */
        public Frame(int firstPolygonIndex, int numPolygons)
            :this()
        {
            minVertexIndex = firstPolygonIndex * VerticesPerPoly;
            numVertices = numPolygons * VerticesPerPoly;
            CalcOrderAndPrimitives(firstPolygonIndex, numPolygons);
        }

        /* Family methods */
        public override string ToString()
        {
            return "Frame Data { vertices Start: " + minVertexIndex.ToString() + ", lenght: " + numVertices.ToString() +
                ", draw Order Start: " + startDrawOrderIndex.ToString() + ", num primitives:" + primitiveCount.ToString() + " }";
        }

        /* Novelty methods */
        public void FromVerticesCount(int minVertexIndex, int numVertices)
        {
            this.minVertexIndex = minVertexIndex;
            this.numVertices = numVertices;
            int firstPolygonIndex = minVertexIndex / VerticesPerPoly;
            int numPolygons= numVertices / VerticesPerPoly;

            CalcOrderAndPrimitives(firstPolygonIndex, numPolygons);
        }

        private void CalcOrderAndPrimitives(int firstPolygonIndex, int numPolygons)
        {
            startDrawOrderIndex = firstPolygonIndex * DrawOrderIndexesPerPoly;
            primitiveCount = numPolygons * 2;
        }
    }
}
