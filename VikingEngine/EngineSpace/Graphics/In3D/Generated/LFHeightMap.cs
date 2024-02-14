using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.Terrain;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;


namespace VikingEngine.Graphics
{
    class LFHeightMap : Abs3DModel
    {
        public const float ChunkRadius = LootFest.Map.WorldPosition.ChunkHalfHeight * 1.2f;
        static Effect customEffectGround;
        
        //List<AddVBData> addList = new List<AddVBData>();

        public static void InitEffect()
        {
            customEffectGround = LoadContent.LoadShader("VoxelTerrain2");

            customEffectGround.CurrentTechnique = customEffectGround.Techniques["Flat"];
        }

        /* Properties */
        protected override bool drawable { get { return false; } }
        public override DrawObjType DrawType { get { return DrawObjType.NotDrawable; } }
        public override float Opacity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override Color Color
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /* Fields */
        public SpottedArrayCounter<Chunk> counter;
        //public SpottedArrayCounter<Chunk> asynchDegenerateCounter;
        //SpottedArray<Chunk> vertexBuffers = new SpottedArray<Chunk>(64);
        UInt16[] recycledIndexDrawOrder16 = new ushort[ushort.MaxValue];
        uint[] recycledIndexDrawOrder32 = new uint[VikingEngine.LootFest.Map.HDvoxel.MeshBuilder.MaxArraySize + VikingEngine.LootFest.Map.HDvoxel.MeshBuilder.MaxArraySize / 2];
        //VertexAndIndexBuffer[] lowDetailSides = new VertexAndIndexBuffer[4];
        VertexAndIndexBuffer lowRes = null;

        /* Constructors */
        public LFHeightMap()
            : base(true)
        {
            visible = false;
            Ref.draw.heightmap = this;
            counter = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
            //asynchDegenerateCounter = new SpottedArrayCounter<Chunk>(vertexBuffers);
        }

        /* Family methods */
        public override void UpdateCulling()
        {
            SpottedArrayCounter<Chunk> counter = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
            while (counter.Next())
            {
                VertexAndIndexBuffer mesh = counter.sel.Mesh;
                if (mesh != null)
                {
                    mesh.UpdateCulling();
                }
            }
        }


        public override void Draw(int cameraIndex) { throw new NotImplementedException(); }
        public override AbsDraw CloneMe() { throw new NotImplementedException(); }
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere) { throw new NotImplementedException(); }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
            shader.Parameters["SourceSize"].SetValue(Vector2.One);
            shader.Parameters["Texture"].SetValue(LoadContent.Texture(LfLib.BlockTexture));

            counter.Reset();
            while (counter.Next())
            {
                VertexAndIndexBuffer buf = counter.sel.Mesh;
                if (buf.VisibleInCamera(cameraIndex))
                {
                    buf.SetBuffer();

                    Matrix world = Matrix.CreateTranslation(buf.Position);
                    shader.Parameters["World"].SetValue(world);
                    shader.Parameters["WorldViewIT"].SetValueTranspose(Matrix.Invert(world * view));

                    shader.CurrentTechnique.Passes[0].Apply();
                    buf.Draw();
                }
            }
        }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        {
            counter.Reset();
            while (counter.Next())
            {
                VertexAndIndexBuffer buf = counter.sel.Mesh;
                if (buf.VisibleInCamera(cameraIndex))
                {
                    buf.SetBuffer();

                    Matrix world = Matrix.CreateTranslation(buf.Position);
                    shader.Parameters["World"].SetValue(world);

                    shader.CurrentTechnique.Passes[0].Apply();
                    buf.Draw();
                }
            }
        }
        public override void copyAllDataFrom(AbsDraw master) { throw new NotImplementedException(); }

        /* Novelty methods */
        public VertexAndIndexBuffer BuildFromPolygons(StaticCountingList<VertexPositionNormalTexture> vertices, StaticCountingList<int> indexDrawOrder, 
            LoadedTexture spriteSheet,IntVector2 chunkGrindex, IntVector3 unitPos, bool reload)
        {
            // Comes from ScreenMeshBuilder
            VerticeDataNormal verticeData = new VerticeDataNormal(vertices, indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
            VertexAndIndexBuffer result = new VertexAndIndexBuffer(verticeData);
            if (result.Correct)
            {
                result.Position = unitPos.Vec;
                visible = true;
                //AddVBAsynch(result, true, reload, chunkGrindex);
            }
            return result;
        }



        public VertexAndIndexBuffer BuildFromPolygons(StaticCountingList<VertexPositionColor> vertices, StaticCountingList<int> indexDrawOrder,
            LoadedTexture spriteSheet, IntVector2 chunkGrindex, IntVector3 unitPos)
        {
            // Comes from ScreenMeshBuilder
            VerticeDataColor verticeData = new VerticeDataColor(vertices, indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
            VertexAndIndexBuffer result = new VertexAndIndexBuffer(verticeData);
            if (result.Correct)
            {
                result.Position = unitPos.Vec;
                visible = true;
                return result;
                //AddVBAsynch(result, true, reload, chunkGrindex);
            }
            return null;
        }

        //public void LowDetailEdgeFromPolygons(StaticCountingList<VertexPositionColor> vertices, StaticCountingList<int> indexDrawOrder,
        //    LoadedTexture spriteSheet, IntVector3 unitPos, Dir4 side, int blockWidth)
        //{
        //     VerticeDataColor verticeData = new VerticeDataColor(vertices, indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
        //    VertexAndIndexBuffer result = new VertexAndIndexBuffer(verticeData);
        //    if (result.Correct)
        //    {
        //        result.Position = unitPos.Vec;
        //        result.Position.Y -= blockWidth;
        //        visible = true;

        //        lowDetailSides[(int)side] = result;
        //    }
        //}

        public void LowDetailFromPolygons(StaticCountingList<VertexPositionColor> vertices, StaticCountingList<int> indexDrawOrder,
            LoadedTexture spriteSheet)
        {
            VerticeDataColor verticeData = new VerticeDataColor(vertices, indexDrawOrder, recycledIndexDrawOrder16, recycledIndexDrawOrder32);
            VertexAndIndexBuffer result = new VertexAndIndexBuffer(verticeData);
            if (result.Correct)
            {
                result.Position.Y -= 8;
                visible = true;
                lowRes = result;
            }
        }

        //public void AddVBAsynch(VertexAndIndexBuffer vb, bool add, bool reload, IntVector2 chunkGrindex)
        //{
        //    lock (addList)
        //    {
        //        addList.Add(new AddVBData(this, vb, chunkGrindex, add, reload));
        //    }
        //}

        //public void AddVB(VertexAndIndexBuffer vb, bool add, bool reload, IntVector2 chunkGrindex)
        //{
        //    Debug.CrashIfThreaded();
        //    if (add)
        //    {
        //        if (!reload)
        //        {
        //            vb.Opacity = 0;

        //            new Graphics.Fade(vb, 1, new Time(1, TimeUnit.Seconds), true);
        //        }

        //        Chunk c = recycleBin.PullFromBin();
        //        if (c == null)
        //        {
        //            c = new Chunk(vb, chunkGrindex);
        //        }
        //        else
        //        {
        //            c.recycle(vb, chunkGrindex);
        //        }

        //        vertexBuffers.Add(c);
        //        vb.CullingBound = new BoundingSphere(vb.Position + LootFest.Map.WorldPosition.ChunkHalfV3Sz, ChunkRadius);
        //    }
        //    else
        //    {
        //        counter.Reset();
        //        while (counter.Next())
        //        {
        //            if (counter.Member.Mesh == vb)
        //            {
        //                recycleBin.PushToBin(counter.Member);
        //                counter.RemoveAtCurrent();
        //                return;
        //            }
        //        }
        //    }

        //}



        public double[,] Transpose(double[,] matrix)
        {
            int w = matrix.GetLength(0);
            int h = matrix.GetLength(1);

            double[,] result = new double[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }



        public void Draw(VectorRect area, int cameraIndex)
        {
            if (visible)
            {
                const string World = "world";
                const string WVP = "wvp";
                const string Transparentsy = "Opacity";

                customEffectGround.Parameters[Transparentsy].SetValue(1f);

                //customEffectGround.Parameters["ScreenDim"].SetValue(Engine.Screen.MonitorTargetResolution.Vec);
                //customEffectGround.Parameters["InvScreenDim"].SetValue(-Engine.Screen.MonitorTargetResolution.Vec);
                //customEffectGround.Parameters["FOV"].SetValue(Ref.draw.ActivePlayerScreens[cameraIndex].view.Camera.FieldOfView);
                //customEffectGround.Parameters["matViewInverse"].SetValue(Matrix.Invert( Ref.draw.ActivePlayerScreens[cameraIndex].view.Camera.ViewMatrix));

                //Matrix transposedWorld = Matrix.Transpose(Ref.draw.worldMatrix);

                if (lowRes != null)
                {
                    Ref.draw.worldMatrix = Matrix.CreateTranslation(Vector3.Zero);
                    customEffectGround.Parameters[World].SetValue(Ref.draw.worldMatrix);
                    Ref.draw.wvpMatrix = Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection;
                    customEffectGround.Parameters[WVP].SetValue(Ref.draw.wvpMatrix);

                    lowRes.SetBuffer();
                    //for (int pass = 0; pass < customEffectGround.CurrentTechnique.Passes.Count; pass++)
                    //{
                        customEffectGround.CurrentTechnique.Passes[0].Apply();
                        lowRes.Draw();
                    //}
                }

                counter.Reset();
                while (counter.Next())
                {
                    //if (counter.Member.Index == LfRef.AllHeroes[0].WorldPos.ChunkGrindex)
                    //{
                    //    lib.DoNothing();
                    //}

                    var mesh = counter.sel.Mesh;

                    if (mesh != null && mesh.VisibleInCamera(cameraIndex))
                    {
                        counter.sel.UpdateShadows(customEffectGround);
                        Ref.draw.worldMatrix = Matrix.CreateTranslation(mesh.Position);
                        customEffectGround.Parameters[World].SetValue(Matrix.Transpose(Ref.draw.worldMatrix));
                        Ref.draw.wvpMatrix = Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection;
                        customEffectGround.Parameters[WVP].SetValue(Ref.draw.wvpMatrix);
                        customEffectGround.Parameters[Transparentsy].SetValue(counter.sel.Mesh.Opacity);

                        mesh.SetBuffer();
                        //for (int pass = 0; pass < customEffectGround.CurrentTechnique.Passes.Count; pass++)
                        //{
                        customEffectGround.CurrentTechnique.Passes[0].Apply();
                        mesh.Draw();
                        //}
                    }
                }

                
            }
        }

        public bool inDrawList(IntVector2 chunk)
        {
            var c = counter.IClone();
            while (c.Next())
            {
                if (c.GetSelection.Index == chunk)
                    return true;
            }
            return false;
        }

        //public void update()
        //{
        //    lock (addList)
        //    {
        //        foreach (var m in addList)
        //        {
        //            AddVB(m.vb, m.add, m.reload, m.chunkGrindex);
        //        }

        //        addList.Clear();
        //    }
        //}
       
        public void UpdateLighting()
        {
            SpottedArrayCounter<Chunk> counter = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
            while (counter.Next())
            {
                //VertexAndIndexBuffer mesh = counter.Member.Mesh;
                //if (mesh != null && mesh.InCameraView)
                //{
                    counter.sel.UpdateLighting();
                //}
            }
        }

        //struct AddVBData //Timer : OneTimeTrigger
        //{
        //    /* Fields */
        //    public LFHeightMap parent;
        //    public VertexAndIndexBuffer vb;
        //    public bool reload;
        //    public bool add;
        //    public IntVector2 chunkGrindex;

        //    /* Constructors */
        //    public AddVBData(Graphics.LFHeightMap parent, VertexAndIndexBuffer vb, IntVector2 chunkGrindex, bool add, bool reload)
        //    //:base(false)
        //    {
        //        this.chunkGrindex = chunkGrindex;
        //        this.reload = reload;
        //        this.add = add;
        //        this.parent = parent; this.vb = vb;
        //        // AddToOrRemoveFromUpdateList(true);
        //    }

        //    /* Methods */
        //    //public override void Time_Update(float time)
        //    //{
        //    //    parent.AddVB(vb, add, reload, chunkGrindex);
        //    //}
        //}
    }

    

    //class HeightMapChunk
    //{
        
        
    //    public VertexAndIndexBuffer Mesh;

    //    public const int MaxLightPoints = 16;
    //    public StaticList<Graphics.ILightSource> LightPoints;
    //    public StaticList<Graphics.ILightSource> LightPointsBuffer;

    //    Vector3[] positionToShader;
    //    float[] radiusToShader;
    //    int[] typeToShader;
    //    //public IntVector2 chunkGrindex;
    //    public int removeDelay = 0;


    //    public HeightMapChunk(VertexAndIndexBuffer mesh, IntVector2 chunkGrindex)
    //    {
    //        this.Mesh = mesh;
    //        positionToShader = new Vector3[MaxLightPoints];
    //        radiusToShader = new float[MaxLightPoints];
    //        typeToShader = new int[MaxLightPoints];

    //        LightPoints = new StaticList<ILightSource>(MaxLightPoints);
    //        LightPointsBuffer = new StaticList<ILightSource>(MaxLightPoints);
    //        UpdateLighting();

    //        this.chunkGrindex = chunkGrindex;
    //    }


    //    public void recycle(VertexAndIndexBuffer mesh, IntVector2 chunkGrindex)
    //    {
    //        this.Mesh = mesh;
    //        this.chunkGrindex = chunkGrindex;
    //    }

    //    public void UpdateLighting()
    //    {
    //        LootFest.Director.LightsAndShadows.Instance.GroupToChunk(this);
    //        for (int i = 0; i < LightPoints.Count; ++i)
    //        {
    //            radiusToShader[i] = LightPoints.Array[i].LightSourceRadius;
    //            typeToShader[i] = (int)LightPoints.Array[i].LightSourceType;
    //        }
    //    }

    //    public void UpdateShadows(Effect customEffectGround)
    //    {
    //        for (int i = 0; i < LightPoints.Count; ++i)
    //        {
    //            positionToShader[i] = LightPoints.Array[i].LightSourcePosition;
    //        }

    //        customEffectGround.Parameters["LightSourcePosition"].SetValue(positionToShader);
    //        customEffectGround.Parameters["LightSourceRadius"].SetValue(radiusToShader);
    //        customEffectGround.Parameters["LightSourceType"].SetValue(typeToShader);
    //        customEffectGround.Parameters["ShadowQty"].SetValue(LightPoints.Count);
    //    }

    //    public override string ToString()
    //    {
    //        return " HeightMapChunk " + chunkGrindex.ToString();
    //    }
    //}
}


