#define GENERATE_MESH

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using System.IO;

using VikingEngine.LootFest.Map.HDvoxel;


namespace VikingEngine.LootFest.Map
{
    class Chunk : ISpottedArrayMember
    {
        /* Constants */
        //public const int RecycleBinSize = 128;
        public const string SaveFileEnding = ".chk";
        //const string ChunkStorageFolder = "ChunksStorage";
        const ushort ChunkDataVersion = 1;
        
        /* Static Readonly */
        //static readonly IntervalF AboutCenter = IntervalF.FromCenter(PublicConstants.Half, 0.1f);
        //static readonly List<IntVector2> SourroundingSides = new List<IntVector2> 
        //    { new IntVector2(0, -1), new IntVector2(0, 1), 
        //      new IntVector2(-1, 0), new IntVector2(1, 0), };
        
        
        /* Static Fields */
        //static RecycleInstances<Chunk> recycleBin = new RecycleInstances<Chunk>(RecycleBinSize);

        /* Static Methods */
        //public static Chunk CreateOrRecycleChunk(IntVector2 index)
        //{
        //    Chunk c = recycleBin.PullFromBin();
        //    if (c == null)
        //    {
        //        c = new Chunk(index);
        //    }
        //    else
        //    {
        //        c.recycled(index);
        //    }

        //    return c;
        //}
        
        /* Properties */
        public BlockMap.AbsLevel level;
        
        /* Fields */
        public IntVector2 Index; //Chunk placement in the world gridslee

        //public VoxelsChunk voxels;
        //List<ushort> compressedData;
        public ushort[, ,] DataGrid;

        public Graphics.VertexAndIndexBuffer Mesh = null;
        public float meshGenerateTimeStamp;
        public PcgRandom prng;

        public bool hasLowResChunk = false;
        public bool needToUpdateLowResChunk = false;
#if PCGAME
        public bool ClientEditingFlag = false;
#endif
        /// </summary>Cant be closed<summary>
        //public bool LockScreen = false;
        
       
        //List<FluidSystem> fluidSystems = null;

        public const int MaxLightPoints = 16;
        public StaticList<Graphics.ILightSource> LightPoints;
        public StaticList<Graphics.ILightSource> LightPointsBuffer;

        Vector3[] positionToShader;
        float[] radiusToShader;
        int[] typeToShader;
        //public IntVector2 chunkGrindex;
        public int removeDelay = 0;
        int chunkIsRequested = 0;
        float creationTimeStamp;

        /* Constructors */
        public Chunk(IntVector2 index)
        {
            this.Index = index;
            
            prng = new PcgRandom(index.X + index.Y * 256);
            DataGrid = new ushort[Map.WorldPosition.ChunkWidth, Map.WorldPosition.ChunkHeight, Map.WorldPosition.ChunkWidth];

            creationTimeStamp = Ref.TotalTimeSec;
        }

        public float lifeTimeSec()
        {
            return  Ref.TotalTimeSec - creationTimeStamp;
        }

        //public void OnDestroy()
        //{
        //    //Debug.Log("--Chunk Ondestroy binL" + recycleBin.Count.ToString());
        //    //if (!recycleBin.PushToBin(this))
        //    //{
        //    //    // Debug.Log("--Chunk Push to bin, count: " + dataGridBin.Count.ToString());
        //    //    for (int y = 0; y < WorldPosition.ChunkHeight; ++y)
        //    //    {
        //    //        for (int z = 0; z < WorldPosition.ChunkWidth; ++z)
        //    //        {
        //    //            for (int x = 0; x < WorldPosition.ChunkWidth; ++x)
        //    //            {
        //    //                DataGrid[x, y, z] = byte.MinValue;
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //}


        //void createEmptyGrid()
        //{
        //    //DataGrid = new byte[WorldPosition.ChunkWidth, WorldPosition.ChunkHeight, WorldPosition.ChunkWidth];
        //}


        public void neighborStatusUpdate_Asynch()
        {
            if (generatedGameObjects)
            {
                neighbor_generatedGameObjects = true;
            }
            else
            {
                if (Index.X > 1 && Index.Y > 1)
                {
                    foreach (IntVector2 dir in IntVector2.Dir8Array)
                    {
                        var ns = LfRef.chunks.GetScreenUnsafe(dir + Index);
                        if (ns != null && ns.generatedGameObjects)
                        {
                            neighbor_generatedGameObjects = true;
                            return;
                        }
                    }
                }
                neighbor_generatedGameObjects = false;
            }
        }

        
        public bool MeshNeedsReload = false;
        public ChunkDataOrigin preparedDataOrigin = ChunkDataOrigin.NON;
        //public ChunkDataOrigin PreparedDataOrigin { get { return preparedDataOrigin; } }


        ChunkDataOrigin completedDataOrigin = ChunkDataOrigin.NON;
        public ChunkDataOrigin DataOrigin { get { return completedDataOrigin; } }

        public bool generatedGameObjects = false, neighbor_generatedGameObjects = true;
        public VikingEngine.LootFest.GO.PlayerCharacter.AbsHero generatedGoHero;
        public ScreenOpenStatus openstatus = ScreenOpenStatus.Closed_0;
       
        public bool CorrectLoaded { get { return completedDataOrigin != ChunkDataOrigin.NON; } }
       
        public bool DataGridLoadingComplete { get { return openstatus >= ScreenOpenStatus.Detail_2; } }

        bool writeProtected = false;
        public bool WriteProtected
        {
            get { return writeProtected; }
            set 
            {
                
                writeProtected = value;
                if (value)
                {
                    unsavedChanges = false;
                }
                
            }
        }
       
        ChunkModifiableType ModifiableType
        {
            get
            {
              return ChunkModifiableType.Modifiable_WillSave; //default
            }
        }

        ChunkSaveType SaveType
        {
            get
            {
                ChunkModifiableType mod = ModifiableType;
                switch (mod)
                {
                    case ChunkModifiableType.Modifiable_NoSave: return ChunkSaveType.NoSave;
                    case ChunkModifiableType.Modifiable_WillSave: return ChunkSaveType.Save;
                    case ChunkModifiableType.PrivateOwner: return ChunkSaveType.PrivateStorage;
                    default: throw new NotImplementedException();
                }
            }
        }

       
       //public void SaveComplete(bool save, int player, DataLib.AbsSaveToStorage obj, bool failed)
       //{
       //    if (!save)
       //    {
       //        needToUpdateLowResChunk = true;
       //    }

       //    if (!save && failed)
       //    {
       //        Debug.LogError("LOAD CHUNK FAILED: " + Index.ToString());
       //    }
       //}
      
        public bool PathFindingPass(WorldPosition wp, ref int CurrentY)
        {
            const int MaxHeight = 3;
            for (int i = 0; i < MaxHeight; i++)
            {
                if (!wp.BlockHasColllision())//hasCollision(wp.LocalBlockX, CurrentY, wp.LocalBlockZ))
                {
                    return true;
                }
                CurrentY++;
            }
            return false;
        }


        //public void RemoveVoxels(WorldPosition startPos, WorldPosition endPos)
        //{
        //    IntVector3 start = startPos.LocalBlockGrindex;
        //    if (startPos.ChunkGrindex.X < Index.X)
        //    {
        //        start.X = 0;
        //    }
        //    if (startPos.ChunkGrindex.Y < Index.Y)
        //    {
        //        start.Z = 0;
        //    }
        //    IntVector3 end = endPos.LocalBlockGrindex;
        //    if (endPos.ChunkGrindex.X > Index.X)
        //    {
        //        end.X = WorldPosition.ChunkWidth - 1;
        //    }
        //    if (endPos.ChunkGrindex.Y > Index.Y)
        //    {
        //        end.Z = WorldPosition.ChunkWidth - 1;
        //    }

        //    IntVector3 blockPos = IntVector3.Zero;
        //    for (blockPos.Y = 0; blockPos.Y < WorldPosition.ChunkHeight; blockPos.Y++)
        //    {
        //        for (blockPos.Z = start.Z; blockPos.Z <= end.Z; blockPos.Z++)
        //        {
        //            for (blockPos.X = start.X; blockPos.X <= end.X; blockPos.X++)
        //            {
        //                DataGrid[blockPos.X, blockPos.Y, blockPos.Z] = 0;
        //            }
        //        }
        //    }
        //}

        //public List<Voxels.Voxel> CollectVoxels(WorldPosition startPos, WorldPosition endPos, IntVector3 addPos)
        //{
        //    List<Voxels.Voxel> result = new List<Voxels.Voxel>();

        //    IntVector3 start = startPos.LocalBlockGrindex;
        //    if (startPos.ChunkGrindex.X < Index.X)
        //    {
        //        start.X = 0;
        //    }
        //    if (startPos.ChunkGrindex.Y < Index.Y)
        //    {
        //        start.Z = 0;
        //    }
        //    IntVector3 end = endPos.LocalBlockGrindex;
        //    if (endPos.ChunkGrindex.X > Index.X)
        //    {
        //        end.X = WorldPosition.ChunkWidth -1;
        //    }
        //    if (endPos.ChunkGrindex.Y > Index.Y)
        //    {
        //        end.Z = WorldPosition.ChunkWidth - 1;
        //    }

        //    IntVector3 blockPos = IntVector3.Zero;
        //    for (blockPos.Y = 0; blockPos.Y < WorldPosition.ChunkHeight; blockPos.Y++)
        //    {
        //        for (blockPos.Z = start.Z; blockPos.Z <= end.Z; blockPos.Z++)
        //        {
        //            for (blockPos.X = start.X; blockPos.X <= end.X; blockPos.X++)
        //            {
        //                if (DataGrid[blockPos.X, blockPos.Y, blockPos.Z] != 0)
        //                {
        //                    IntVector3 pos = blockPos;
        //                    pos.Add(addPos);
        //                    result.Add(new Voxels.Voxel(pos, DataGrid[blockPos.X, blockPos.Y, blockPos.Z]));
        //                }
        //            }
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// For placement of NPCs
        /// </summary>
        public float GetClosestFreeY(WorldPosition pos)
        {
            const float AdjustResult = 0.5f;

            IntVector3 localPos = pos.LocalBlockGrindex;
            localPos.Y = Bound.Set(localPos.Y, 0, Map.WorldPosition.MaxChunkY);

            //check if inside block 
            bool insideBlock =hasCollision(localPos.X, localPos.Y, localPos.Z) || 
                hasCollision(localPos.X, Bound.Max(localPos.Y + 1,  WorldPosition.MaxChunkY), localPos.Z);

            if (insideBlock)
            { //check upward
                for (; localPos.Y < WorldPosition.ChunkHeight; ++localPos.Y)
                {
                    if (!hasCollision(localPos.X, localPos.Y, localPos.Z) && 
                        !hasCollision(localPos.X, Bound.Max(localPos.Y + 1, WorldPosition.MaxChunkY), localPos.Z))//make sure its a two block gap
                    {
                        return localPos.Y - 1 + AdjustResult;
                    }
                }
                return localPos.Y - 1 + AdjustResult;
            }
            else
            { //only check downward
                for (; localPos.Y >= 0; --localPos.Y)
                {
                    if (hasCollision(localPos.X, localPos.Y, localPos.Z))
                    {
                        return localPos.Y + AdjustResult;
                    }
                }
                return localPos.Y + AdjustResult;
            }
        }


        public bool HasFreeSpaceUp(WorldPosition pos, int spaceNeeded)
        {
            IntVector3 localPos = pos.LocalBlockGrindex;
            int endY = lib.SmallestValue(localPos.Y + spaceNeeded, WorldPosition.ChunkHeight);

            for (; localPos.Y < endY; ++localPos.Y)
            {
                if (hasCollision(localPos.X, localPos.Y, localPos.Z))
                    return false;
            }
            return true;
        }

        public int GetHighestYpos(WorldPosition pos)
        {
            if (!pos.CorrectGridPos)
            {
                return 0;
            }

            IntVector3 localPos = pos.LocalBlockGrindex;
            for (int y = WorldPosition.ChunkHeight - 1; y >= 0; y--)
            {
                if (hasCollision(localPos.X, y, localPos.Z))
                {
                    return y;
                }
            }
            return 0;
        }

       
        public int GetFirstBlockBelow(WorldPosition pos)
        {
            //throw new Exception("Behöver uppdateras"); //används bara av flying toy
            IntVector3 localPos = pos.LocalBlockGrindex;
            for (localPos.Y = Bound.Max(localPos.Y + 1, WorldPosition.MaxChunkY); localPos.Y >= 0; localPos.Y--)
            {
                if (hasCollision(localPos.X, localPos.Y, localPos.Z))
                {
                    return localPos.Y;
                }
            }
           
            return 0;
        }
       

        /// <returns>if you will collide to this square</returns>
        //public bool Check2DCollision(WorldPosition wp)
        //{
        //    if (DataGrid == null)
        //    { 
        //        return false;
        //    }

        //    return (DataGrid[wp.LocalBlockX, WorldPosition.PassableGroundY, wp.LocalBlockZ] == WorldPosition.EmptyBlock) ||
        //        (DataGrid[wp.LocalBlockX, WorldPosition.PassableGroundY + 1, wp.LocalBlockZ] != WorldPosition.EmptyBlock);
        //}
     

        public bool unsavedChanges = false;
        //public bool UnSavedChanges
        //{ 
        //    get { return unsavedChanges; }
        //}
        

        public void CompleteRemoval()
        {
            DataStream.DataStreamHandler.BeginUserRemoveFile(Path);
        }
       
        public static DataStream.FilePath SavePath(IntVector2 index)
        {
            return new DataStream.FilePath(
                VikingEngine.LootFest.Players.PlayerStorageGroup.FileFolderName(LfRef.gamestate.LocalHostingPlayer.Storage.StorageGroupIx, LfRef.WorldHost), 
                lib.IntVec2Text(index), SaveFileEnding, true, NumBackupVersions, true);
        }

        public const int NumBackupVersions = 3;
        public DataStream.FilePath Path
        {
            get
            {
                return SavePath(Index);
            }
        }

        public void SaveData2(bool save, BlockMap.DesignAreaStorage area)
        {
            if (save)
            {
                unsavedChanges = false;
                area.onSavingChunk(Index);
                
                needToUpdateLowResChunk = true;
            }
            new ScreenSaveQuer2(save, this, area);
        }

        //public void SaveData(bool save, DataLib.ISaveTostorageCallback callBack, bool saveBackup)
        //{
        //    if (save)
        //    {
        //        unsavedChanges = false;

        //    }

        //    if ((!save || LfRef.WorldHost) && !WriteProtected && SaveType == ChunkSaveType.Save)
        //    {
        //        new ScreenSaveQuer(save, this, callBack, false);
        //    }
        //}

       
        public void WriteChunk(System.IO.BinaryWriter w)
        {
            w.Write(ChunkDataVersion);
            writeDataGrid(w);
        }

        public void writeDataGrid(System.IO.BinaryWriter w)
        {
            Voxels.VoxelLib.CompressGridHD(DataGrid, w);
        }

        public void ReadChunk(System.IO.BinaryReader r)
        {
            Debug.CrashIfThreaded();

            int version = r.ReadInt16();
            readDataGrid(r);
        }

        public void readDataGrid(System.IO.BinaryReader r)
        {
            Voxels.VoxelLib.DeCompressGridHD(DataGrid, r);

            if (openstatus < ScreenOpenStatus.Detail_2)
                openstatus = ScreenOpenStatus.Detail_2;

            completedDataOrigin = ChunkDataOrigin.Loaded;

            needToUpdateLowResChunk = true;
        }


        public void BeginReadChunk(System.IO.BinaryReader r)
        {
            new DataLib.SafeMainThreadReader(r, this.ReadChunk);
        }


        public static void NetworkSaveChunk(System.IO.BinaryReader r, IntVector2 pos)
        {
            byte[] data = r.ReadBytes((int)r.BaseStream.Length);
            new DataStream.WriteByteArray( SavePath(pos), data, null);
        }

        //void writeDataGrid(System.IO.BinaryWriter w)
        //{
        //    Voxels.VoxelLib.CompressGridHD(DataGrid, w);
        //    //if (DataGrid != null)
        //    //{
        //    //    List<IntVector2> materialAndReps = new List<IntVector2>();
        //    //    byte[] usedMaterials = new byte[PublicConstants.ByteSize];

        //    //    Voxels.VoxelLib.GridToMaterialAndReps(DataGrid, materialAndReps, usedMaterials);

        //    //    List<byte> usedMaterialsList = Voxels.VoxelLib.GetUsedMaterialsList(usedMaterials);
        //    //    Voxels.VoxelLib.WriteUsedMaterials(w, usedMaterialsList);
        //    //    Voxels.VoxelLib.WriteMaterialAndReps(w, materialAndReps, usedMaterials, usedMaterialsList);
        //    //}
        //}

        //void readDataGrid(System.IO.BinaryReader r, int version)
        //{
        //    Voxels.VoxelLib.DeCompressGridHD(DataGrid, r);
        //        //List<VikingEngine.Voxels.Voxel> non;
        //        //byte[] usedMaterials = Voxels.VoxelLib.ReadUsedMaterials(r);
        //        //DataGrid = Voxels.VoxelLib.ReadMaterialAndReps(r, Map.WorldPosition.ChunkSize, usedMaterials, false, out non);
           
        //}

        public void netWrite(System.IO.BinaryWriter w)
        {
            //Head
            Map.WorldPosition.WriteChunkGrindex_Static(Index, w);

            //Body
            WriteChunk(w);
            //writeDataGrid(w);
        }

        public void netRead(System.IO.BinaryReader r)
        {
            Debug.Log(">Receives chunk from host " + Index.ToString());
            try
            {
                ReadChunk(r);
                //readDataGrid(r);
                completedDataOrigin = ChunkDataOrigin.RecievedByNet;
                MeshNeedsReload = true;
                unsavedChanges = true;

                IntVector2 localPos;
                BlockMap.DesignAreaStorage area = level.designAreas.getArea(Index, out localPos);
                area.Set(localPos, true, BlockMap.DesignAreaStorage.NetRecieved_BitIndex);//.Get(localPos, out edited, out netRecieved);
                //Map.World.hasWaitingReloads = true;
            }
            catch (IOException e)
            {
                Debug.LogError("NetworkReceiveChunk, " + e.Message);
            }
        }


        //public void GeneratePart(ScreenOpenStatus setStatus)
        //{
        //    switch (openstatus)
        //    {
        //        case ScreenOpenStatus.Closed_0:
        //            generate2_HeightMap();
        //            break;
        //        case ScreenOpenStatus.HeightMap_1a:
        //            generate3_Detail();
        //            break;
        //    }

        //    System.Threading.Thread.Sleep(1000);
    
        //}


        public void generate4_Mesh(Graphics.LFHeightMap heightMapMesh,
            MeshBuilder meshBuilder)
        {
            if (openstatus >= ScreenOpenStatus.Detail_2)
            {
                setupLights();
                MeshNeedsReload = false;

                if (openstatus < ScreenOpenStatus.Mesh_3)
                    openstatus = ScreenOpenStatus.Mesh_3;

#if GENERATE_MESH
                
                WorldPosition wp = WorldPosition.EmptyPos;
                wp.ChunkGrindex = Index;

                
                 Map.World.ChunkGeneratingSleep();
                

                //bool isReload = Mesh != null;

                Graphics.VertexAndIndexBuffer newVB = null;

                if (!Ref.update.exitApplication)
                {
                    newVB = meshBuilder.BuildScreen(wp, heightMapMesh);

                    if (newVB == null)
                    {
                        return;
                    }

                    newVB.CullingBound = new BoundingSphere(newVB.Position + LootFest.Map.WorldPosition.ChunkHalfV3Sz, LFHeightMap.ChunkRadius);
                }

                // We queue the remove-chunk AFTER queuing the add-chunk, so that the meshes don't blink in and out of existence.
                //if (isReload)
                //{
                //    heightMapMesh.AddVBAsynch(Mesh, false, true, Index);
                //    //new AddVBTimer(heightMapMesh, vbPointer, Index, false, false);
                //}

                Mesh = newVB;
                meshGenerateTimeStamp = Ref.TotalTimeSec;
#endif
            }
        }

        void setupLights()
        {
            if (positionToShader == null)
            {
                positionToShader = new Vector3[MaxLightPoints];
                radiusToShader = new float[MaxLightPoints];
                typeToShader = new int[MaxLightPoints];

                LightPoints = new StaticList<ILightSource>(MaxLightPoints);
                LightPointsBuffer = new StaticList<ILightSource>(MaxLightPoints);
                UpdateLighting();
            }
        }
        public void UpdateLighting()
        {
            var meshPointer = Mesh;
            if (meshPointer != null && meshPointer.InCameraView)
            {
                LootFest.Director.LightsAndShadows.Instance.GroupToChunk(this);
                for (int i = 0; i < LightPoints.Count; ++i)
                {
                    radiusToShader[i] = LightPoints.Array[i].LightSourceRadius;
                    typeToShader[i] = (int)LightPoints.Array[i].LightSourceType;
                }
            }
        }

        public void UpdateShadows(Effect customEffectGround)
        {
            customEffectGround.Parameters["ShadowQty"].SetValue(LightPoints.Count);

            for (int i = 0; i < LightPoints.Count; ++i)
            {
                positionToShader[i] = LightPoints.Array[i].LightSourcePosition;

                customEffectGround.Parameters["LightSourcePosition"].Elements[i].SetValue(positionToShader[i]);
                customEffectGround.Parameters["LightSourceRadius"].Elements[i].SetValue(radiusToShader[i]);
                customEffectGround.Parameters["LightSourceType"].Elements[i].SetValue(typeToShader[i]);
                
            }

        }

//        public void RemoveMesh(Graphics.LFHeightMap heightMapMesh)
//        {
//            if (Mesh != null)
//            {
//#if GENERATE_MESH
//                openstatus = ScreenOpenStatus.Detail_2;
//                //AddVBTimer timer = new AddVBTimer(heightMapMesh, vbPointer, Index, false, false);
//                heightMapMesh.AddVBAsynch(Mesh, false, false, Index);
//                Mesh = null;
//                degenerateGameObjects();
//#endif
//            }
//        }

        public void RemoveMeshPointer()
        {
            if (Mesh != null)
            {
                Mesh = null;
                degenerateGameObjects();
            }

            if (openstatus >= ScreenOpenStatus.Mesh_3)
            {
                openstatus = ScreenOpenStatus.Detail_2;
            }
        }

        void degenerateGameObjects()
        {
            generatedGameObjects = false;
        }

        void debugPrintStreamPos(object stream, string position)
        {
            if (PlatformSettings.ViewErrorWarnings)
            {
                string text = "Chunk" + Index.ToString() + ", streaming " + position + ", ";
                long pos, length;
                if (stream is System.IO.BinaryReader)
                {
                    System.IO.BinaryReader r = stream as System.IO.BinaryReader;
                    pos = r.BaseStream.Position;
                    length = r.BaseStream.Length;
                    text += "reader";
                }
                else
                {
                    System.IO.BinaryWriter w = stream as System.IO.BinaryWriter;
                    pos = w.BaseStream.Position;
                    length = w.BaseStream.Length;
                    text += "writer";
                }
                text += " pos" + pos.ToString() + "/" + length.ToString();
                Debug.Log(DebugLogType.MSG, text);
            }
        }

        public void Close(bool save)
        {
            RemoveMeshPointer();

            if (openstatus > ScreenOpenStatus.Closed_0)
            {
                if (save)
                {
                    saveChanges();
                }
                openstatus = ScreenOpenStatus.Closed_0;
            }
        }

        public void saveChanges()
        {
            if (unsavedChanges)
            {
                if (level != null)
                {
                    
                    BlockMap.DesignAreaStorage area = level.designAreas.getArea(Index, out _);
                    if (area != null)
                    {
                        SaveData2(true, area);
                    }
                }
            }
        }

        public void AddBoss(int level)
        {
            Vector3 startPos = Vector3.Zero;
            startPos.X = (float)((Index.X + 1) * WorldPosition.ChunkWidth);
            startPos.Z = (float)((Index.Y + 1) * WorldPosition.ChunkWidth);
        }
        //public bool EmptyScreenCheck()
        //{
        //    return DataGrid[5,1, 5] == 0;
        //}


        /// <summary>
        /// Add smaller objects like trees and stones to the ground
        /// </summary>
       public void generate3_Detail()
       {
           if (openstatus == ScreenOpenStatus.HeightMap_1a)
           {
                var level = LfRef.levels2.GetLevelUnsafe(Index);

                if (level != null)
                {
                    level.generateChunkDetail(this);
                }

               openstatus = ScreenOpenStatus.Detail_2;
           }
       }

       

       
       //public void ReRequest()
       //{
       //    if (Ref.netSession.IsClient)
       //    {
       //        chunkIsRequested = 0;
       //        RequestChunkData();
       //    }
       //}


       public void RequestChunkData()
       {
           preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
           if (chunkIsRequested == 0)
           {
               var w = Ref.netSession.BeginWritingPacket(Network.PacketType.RequestChunk, Network.PacketReliability.Reliable);
                Map.WorldPosition.WriteChunkGrindex_Static(Index, w);
               //System.IO.BinaryWriter writer = Ref.netSession.BeginAsynchPacket();
               //Map.WorldPosition.WriteChunkGrindex_Static(Index, writer);

               //Debug.Log("<Requesting chunk from host " + Index.ToString());

               //Ref.netSession.EndAsynchPacket(writer,
               //    Network.PacketType.RequestChunk,
               //     Network.SendPacketTo.OneSpecific, Ref.steam.P2PManager.localHost.FullId,
               //    Network.PacketReliability.ReliableLasy, null);
           }

           if (chunkIsRequested++ > 600)
           {
               chunkIsRequested = 0;
           }
       }
       //public void RequestChunkDataUpdate()
       //{
       //    //when a chunk is undoed
       //    chunkIsRequested++;
       //    System.IO.BinaryWriter writer = Ref.netSession.BeginWritingPacketToHost(Network.PacketType.RequestChunkGroup, 
       //        Network.PacketReliability.ReliableLasy, LfLib.LocalHostIx);
       //    //Index.WriteStream(writer);
       //    Map.WorldPosition.WriteChunkGrindex_Static(Index, writer);
       //}



       //bool loadingOrRequestingData = false;
       //bool generateTerrain = true;
       /// <summary>
       /// Checks if the chunk data is changed before generating it
       /// </summary>
       /// <returns>Should keep generating the chunk</returns>
       public bool LoadOrRequestData(BlockMap.DesignAreaStorage area, IntVector2 localPos)
       {
           bool generateTerrain = true;

           if (level != null && area != null)
           {
               //IntVector2 localPos;
               //BlockMap.DesignAreaStorage area = level.designAreas.getArea(Index, out localPos);

               //if (area != null)
               //{
                   bool edited, netRecieved;
                   area.Get(localPos, out edited, out netRecieved);

                   if (edited)
                   {
                       if (LfRef.WorldHost || netRecieved)
                       {
                           SaveData2(false, area);
                           generateTerrain = false;
                           preparedDataOrigin = ChunkDataOrigin.Loaded;
                       }
                       else if (!LfRef.WorldHost)
                       {
                           generateTerrain = false;
                           new Timer.Action0ArgTrigger(RequestChunkData);
                       }
                   }
               //}
           }
           //if (SaveType == ChunkSaveType.Save)//chunkData.changed && 
           //{ //Chunk has stored changes
           //    if (Map.World.RunningAsHost)
           //    {//locally stored, just load it
           //        SaveData(false, this, true);
           //        generateTerrain = false;
           //        preparedDataOrigin = ChunkDataOrigin.Loaded;
           //    }
           //    else
           //    {//request from host
           //        RequestChunkData();
           //        //preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
           //    }
           //}
           //else if (SaveType == ChunkSaveType.PrivateStorage)
           //{ //Chunk is hosted by one player
           //    Players.AbsPlayer owner = null;//LfRef.worldOverView.ChunkHasOwner(Index);
           //    if (owner != null)
           //    {
           //        if (owner.Local)
           //        {//Pick from local player
           //            //bool hasData = loadPrivateArea((Players.Player)owner);
           //            //generateTerrain = !hasData;

           //            //if (hasData)
           //            //    preparedDataOrigin = ChunkDataOrigin.Loaded;
           //            //else
           //            //    preparedDataOrigin = ChunkDataOrigin.Generated;

           //            /*
           //             * loadPrivateArea always returned false
           //             * and generateTerrain is already set to true above
           //             */
           //            preparedDataOrigin = ChunkDataOrigin.Generated;
           //        }
           //        else
           //        {//Request over net
           //            RequestChunkData(); //måste se till att man frågar rätt host
           //            //preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
           //        }
           //    }
           //    else
           //    {
           //        preparedDataOrigin = ChunkDataOrigin.Generated;
           //    }
           //}
           //else
           //{
           //    preparedDataOrigin = ChunkDataOrigin.Generated;
           //}

           if (!generateTerrain)
           {
               if (openstatus >= ScreenOpenStatus.LoadingOrRecievingData_1b)
                   throw new Exception();
               openstatus = ScreenOpenStatus.LoadingOrRecievingData_1b;
           }

            return generateTerrain;

           //if (PlatformSettings.ViewErrorWarnings && preparedDataOrigin == ChunkDataOrigin.NON)
           //    throw new Exception();
       }


       bool readyToGenerate(out BlockMap.DesignAreaStorage area, out IntVector2 localPos)
       {
           area = null;
           localPos = IntVector2.Zero;

           if (level != null)
           {
               area = level.designAreas.getArea(Index, out localPos);

               if (area != null)
               {
                   return LfRef.WorldHost || level.designAreas.netRecieved;
               }
           }
           
           return true;
       }


        public bool dataOriginOutdated()
        {
            if (openstatus >= ScreenOpenStatus.HeightMap_1a)
            {
                level = LfRef.levels2.GetLevelUnsafe(Index);
                if (level != null)
                {
                    IntVector2 localPos;
                    BlockMap.DesignAreaStorage area = level.designAreas.getArea(Index, out localPos);
                    
                    if (area != null)
                    {
                        bool edited, netRecieved;
                        area.Get(localPos, out edited, out netRecieved);

                        if (edited && completedDataOrigin != ChunkDataOrigin.RecievedByNet)
                        {
                            return true;
                            //return LfRef.WorldHost || level.designAreas.netRecieved;
                        }
                    }
                }
            }

            return false;
       }

       public void generate2_HeightMap()
        {//PART OF OPEN STATUS
            if (openstatus < ScreenOpenStatus.HeightMap_1a)//temp
            {
               // Level = LfRef.levels.GetLevelUnsafe(Index * WorldPosition.ChunkWidth);
                level = LfRef.levels2.GetLevelUnsafe(Index);

                BlockMap.DesignAreaStorage designArea;
                IntVector2 designAreaLocalPos;

                if (readyToGenerate(out designArea, out designAreaLocalPos) == false)
                {
                    return;
                }

                bool generateTerrain = LoadOrRequestData(designArea, designAreaLocalPos);


                if (generateTerrain)
                {
                    completedDataOrigin = ChunkDataOrigin.Generated;

                    //1. start with a 2d grid of the height
                    //2. add height templates (select highest)
                    //3. add detail map

                   // byte addMaterial;

                    IntVector3 pos = IntVector3.Zero;

                    

                    if (level != null)
                    {
                        int height = 0; 
                        HeightMapMaterials materials;// = new HeightMapMaterials();


                        for (pos.Z = 0; pos.Z < Map.WorldPosition.ChunkWidth; ++pos.Z)
                        {
                            for (pos.X = 0; pos.X < Map.WorldPosition.ChunkWidth; ++pos.X)
                            {
                                level.heightmap(Index, pos.X, pos.Z, ref height, out materials);

                                pos.Y = height - 1;
                                SetBlockPattern(pos, materials.topMaterial);
                                pos.Y--;
                                int topLayerMin = pos.Y - materials.firstLayerHeight;

                                for (; pos.Y >= 0; --pos.Y)
                                {
                                    if (pos.Y < topLayerMin)
                                    {
                                        SetBlockPattern(pos, materials.bottomMaterial);
                                    }
                                    else
                                    {
                                        SetBlockPattern(pos, materials.firstLayerMaterial);
                                    }
                                }
                            }
                        }
                    }
                    if (openstatus < ScreenOpenStatus.HeightMap_1a)
                        openstatus = ScreenOpenStatus.HeightMap_1a;
                }
            }//end check openstatus
        }

        /// <summary>
        /// Sets a floor block at a certain height, removing everything above
        /// </summary>
        /// <param name="localPos">The block position in the chunk</param>
        /// <param name="floorMaterial">The material type of the floor</param>
        //public void CreateFlatFloor(IntVector3 localPos, Data.MaterialType floorMaterial)
        //{
        //    DataGrid[localPos.X, localPos.Y, localPos.Z] = (byte)floorMaterial;
        //    int highest = GetHighestYpos(new WorldPosition(localPos));
        //    for (int y = ++localPos.Y; y <= highest; ++y)
        //    {
        //        DataGrid[localPos.X, y, localPos.Z] = byte.MinValue;
        //    }
        //}

        /// <summary>
        /// Takes the material and fills downwards at pos until a cube is found that isn't empty
        /// </summary>
        /// <param name="localPos">The block position in the chunk</param>
        /// <param name="fillMaterial">The material type to fill with</param>
        //public void FillDownwards(IntVector3 localPos, Data.MaterialType fillMaterial)
        //{
        //    while (DataGrid[localPos.X, localPos.Y, localPos.Z] == byte.MinValue && localPos.Y > 0)
        //    {
        //        DataGrid[localPos.X, localPos.Y--, localPos.Z] = (byte)fillMaterial;
        //    }
        //}

        /// <summary>
        /// Fills the entire chunk with a flat floor
        /// </summary>
        /// <param name="floorMaterial">The material type of the floor</param>
        //public void FillFlatFloor(Data.MaterialType floorMaterial)
        //{
        //    IntVector3 pos = IntVector3.Zero;
        //    pos.Y = WorldPosition.ChunkStandardHeight;

        //    for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; pos.Z++)
        //    {
        //        for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; pos.X++)
        //        {
        //            CreateFlatFloor(pos, floorMaterial);
        //        }
        //    }
        //}

        //public void FillLayer(int layerY, Data.MaterialType material)
        //{
        //    IntVector3 pos = IntVector3.Zero;
        //    pos.Y = layerY;

        //    for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; pos.Z++)
        //    {
        //        for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; pos.X++)
        //        {
        //            DataGrid[pos.X, pos.Y, pos.Z] = (byte)(material);
        //        }
        //    }
        //}

       public ushort Get(WorldPosition pos)
       {

           //if (compressedData != null)
           //{
           //    lib.DoNothing();
           //}

           if (pos.CorrectYPos)
               return DataGrid[pos.LocalBlockX, pos.WorldGrindex.Y, pos.LocalBlockZ];

           
           return 0;
       }
       public void Set(WorldPosition pos, ushort value)
       {
           if (pos.CorrectYPos)
               DataGrid[pos.LocalBlockX, pos.WorldGrindex.Y, pos.LocalBlockZ] = value;
       }
        //public void Set(WorldPosition pos, MaterialType material)
        //{
        //    Set(pos, (byte)material);
        //}

        ////public void Set(int localX, int localY, int localZ, byte value)
        ////{
        ////    voxels.Set(new WorldPosition(Index, localX, localY, localZ), new HDvoxel.BlockHD(value));
        ////    //DataGrid[localX, localY, localZ] = value;
        ////}

        ////public void Set(int localX, int localY, int localZ, HDvoxel.BlockHD value)
        ////{
        ////    voxels.Set(new WorldPosition(Index, localX, localY, localZ), value);
        ////}

       bool hasCollision(int localX, int localY, int localZ)
       {
          // return voxels.Get(new WorldPosition(Index, localX, localY, localZ)).HasMaterial;
           return DataGrid[localX, localY, localZ] != 0;
       }

       public void SetBlockPattern(IntVector3 localPos, ushort value)
       {
           if (BlockHD.ToMaterialValue(value) == BlockHD.BlockPatternMaterial)
           {
               value = BlockPatternMaterialsLib.ToBlock(value, localPos);

               BlockHD testb = new BlockHD();
               testb.BlockValue = value;

           }

           DataGrid[localPos.X, localPos.Y, localPos.Z] = value;
       }

       public void SetBlockPattern(WorldPosition pos, ushort value)
       {
           if (pos.CorrectYPos)
           {
               if (BlockHD.ToMaterialValue(value) == BlockHD.BlockPatternMaterial)
               {
                   value = BlockPatternMaterialsLib.ToBlock(value, pos.WorldGrindex);
               }

               DataGrid[pos.LocalBlockX, pos.WorldGrindex.Y, pos.LocalBlockZ] = value;
           }
       }

       public void Set(IntVector3 localPos, ushort value)
       {
           DataGrid[localPos.X, localPos.Y, localPos.Z] = value;
       }

        //public void SetUnsafe(WorldPosition pos, byte value)
        //{
        //    DataGrid[pos.WorldGrindex.X & Map.WorldPosition.ChunkBitsZero, pos.WorldGrindex.Y, pos.WorldGrindex.Z & Map.WorldPosition.ChunkBitsZero] = value;
        //}



        public override string ToString()
        {
            string generated = "";
            if (generatedGameObjects)
            {
                generated = "(GO gen)";
            }

            return "Chk" + Index.ToString() + ":" + openstatus.ToString() + generated + ":" + completedDataOrigin.ToString() +
                (CorrectLoaded ? " (Y)" : " (N)") + ", req" + chunkIsRequested.ToString();
        }
        public bool ReadyForEditing
        {
            get
            {
                return DataGridLoadingComplete;
            }
        }

        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }
        public bool GotMesh { get { return Mesh != null; } }

        public Rectangle2 ToWorldTilePosRect()
        {
            return new Rectangle2(Index * WorldPosition.ChunkWidth, new IntVector2(WorldPosition.MaxChunkXZ));
        }

        public Rectangle2 ToWorldPosRect()
        {
            return new Rectangle2(Index * WorldPosition.ChunkWidth, new IntVector2(WorldPosition.ChunkWidth));
        }
    }

    enum ScreenOpenStatus
    {
        Closed_0,
        HeightMap_1a,
        LoadingOrRecievingData_1b,
        Detail_2,
        Mesh_3,
        //GameObjects_4,
        NUM
    }
    enum ChunkDataOrigin
    {
        NON,
        Generated,
        Loaded,
        RecievedByNet,
    }
    enum ChunkModifiableType
    {
        Modifiable_NoSave,
        Modifiable_WillSave,
        PrivateOwner,
    }
    enum ChunkSaveType
    {
        NoSave,
        Save,
        PrivateStorage,
    }
}
