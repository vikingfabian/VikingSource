using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Threading;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.Map
{
    class World
    {
        #region  OUTDATED
        List<IntVector2> outDatedChunks = new List<IntVector2>();

        public void AddOutDatedChunks(List<IntVector2> chunks)
        {
            lock (outDatedChunks)
            {
                outDatedChunks.AddRange(chunks);
            }
            
        }

        void removeOutdatedChunksUpdate()
        {
            if (outDatedChunks.Count > 0)
            {
                List<IntVector2> outDatedChunksCopy;
                lock (outDatedChunks)
                {
                    outDatedChunksCopy = new List<IntVector2>(outDatedChunks);
                    outDatedChunks.Clear();
                }

                foreach (IntVector2 chunk in outDatedChunksCopy)
                {
                    Chunk s = LfRef.chunks.GetScreenUnsafe(chunk);
                    if (s != null)
                    {
                        //unthread
                        new Timer.Action1ArgTrigger<Chunk>(closeScreen, s);
                    }
                }
            }
        }

        #endregion
        
        const int MeshGeneratingThreadCount = 5;
        List<IntVector2> currentMeshGenerating = new List<IntVector2>();
        List<Editor.EditorPacket> editorPacketQue = new List<Editor.EditorPacket>();
        MeshBuilder[] meshBuilders;

        public bool areaStorageHeaderNeedsUpdate = false;
        

        //public static bool RunningAsHost = true;
        //public Graphics.LFHeightMap mesh;
        LowResChunkCollection lowResChunkCollection = new LowResChunkCollection();

        IntVector2 centerScreen = IntVector2.NegativeOne;
        const int OpenScreenRadius_EnemySpawn = 3;

        const int GenerateGameObjectRadius = 4;
        const int RemoveGameObjectRadius = GenerateGameObjectRadius + 1;

        public const int StandardOpenRadius = 4;
        public static int OpenScreenRadius_Mesh = StandardOpenRadius;
        const int RemoveMeshBufferLength = 1;
        static int OpenScreenRadius_AreaDetail;
        //public static int DecompressScreenRadius;
        public static int OpenScreenWidth;
        public static int LowResChunksRadius;
        
        static int CheckRadius = OpenScreenRadius_Mesh - 1;

        static int FullCloseRadius;
        //int lowDetailEdgeGeneratingLock = 0;

        List<EnvironmentObjectChanged> EnvironmentObjectChangedList = new List<EnvironmentObjectChanged>();
        EnvironmentObjectChanged currentEnvChange = null;

        //public static bool hasWaitingReloads = false;
        bool clearedOutChunkMemory = false;

        public World(VikingEngine.LootFest.Data.WorldData worldData)
        {
            new WorldChunks();

            new BlockMap.LevelsManager(worldData);
            RefreshChunkLoadRadius();
        }

        public static void ReloadChunkMesh(WorldPosition from, IntVector3 size, bool editorChange)
        {
            ReloadChunkMesh(from, from.GetNeighborPos(size), editorChange);
        }

        public static void ReloadChunkMesh(WorldPosition from, WorldPosition to, bool editorChange)
        {
            IntVector2 min = new IntVector2(lib.SmallestValue(from.ChunkX, to.ChunkX), lib.SmallestValue(from.ChunkY, to.ChunkY));
            IntVector2 max = new IntVector2(lib.LargestValue(from.ChunkX, to.ChunkX), lib.LargestValue(from.ChunkY, to.ChunkY));
            //max 
            IntVector2 chunk = IntVector2.Zero;
            for (chunk.Y = min.Y; chunk.Y <= max.Y; chunk.Y++)
            {
                for (chunk.X = min.X; chunk.X <= max.X; chunk.X++)
                {
                    ReloadChunkMesh(chunk, editorChange);
                }
            }
        }

        public static void ReloadChunkMesh(IntVector2 pos, bool editorChange)
        {
            
            Chunk s = LfRef.chunks.GetScreenUnsafe(pos);
            if (s != null)
            {
                s.unsavedChanges = true;
                s.MeshNeedsReload = true;
                //hasWaitingReloads = true;
            }
        }
        

        public void UpdateHeightMapLights()
        {
            Ref.draw.heightmap.UpdateLighting();
        }

        
        public void GameStart()
        {
            new LFHeightMap();

            new Director.FrustumCullingDirector();

            new Timer.TimedAction0ArgTrigger(startGeneratingThreads, 300);
        }

        void startGeneratingThreads()
        {
            //Ref.asynchUpdate.AddUpdateThread(terrainGeneratingDataThread, "terrain Generating Data thread", 0);
            new AsynchUpdateable(terrainGeneratingDataThread, "terrain Generating Data thread");

            meshBuilders = new MeshBuilder[MeshGeneratingThreadCount];
            for (int i = 0; i < MeshGeneratingThreadCount; ++i)
            {
                meshBuilders[i] = new MeshBuilder();
                //Ref.asynchUpdate.AddUpdateThread(meshGeneratingThread, "mesh Generating thread" + i.ToString(), i);
                new AsynchUpdateable(meshGeneratingThread, "mesh Generating thread" + i.ToString(), i);
            }
        }

        public void RefreshChunkLoadRadius()
        {
            int openRadius = StandardOpenRadius;
            if (PlatformSettings.PC_platform)
            {
                openRadius = Ref.gamesett.ChunkLoadRadius;
            }

            OpenScreenRadius_Mesh = (LfRef.LocalHeroes == null || LfRef.LocalHeroes.Count <= 2) ? 
                openRadius : (openRadius - 1);
            OpenScreenRadius_AreaDetail = OpenScreenRadius_Mesh + 1;
            FullCloseRadius = OpenScreenRadius_AreaDetail + 1;

            OpenScreenWidth = OpenScreenRadius_AreaDetail * 2 + 1;
            
            CheckRadius = OpenScreenRadius_Mesh - 1;

            LowResChunksRadius = OpenScreenRadius_Mesh + 12;
        }


        bool terrainGeneratingDataThread(int id, float time)
        {
            removeOutdatedChunksUpdate();
            closeChunksUpdate_asynch();
            areaStorageHeaderUpdate_Asynch();
            readEditorPacketUpdate_asynch();
            openScreensUpdate_asynch();
            neighborStatusUpdate_Asynch();
            lowResChunkCollection.update(this);
            //compressUpdate_asynch(time);
            return false;
        }

        

        void neighborStatusUpdate_Asynch()
        {
            LfRef.chunks.OpenChunksWorldGenCounter.Reset();
            while (LfRef.chunks.OpenChunksWorldGenCounter.Next())
            {
                LfRef.chunks.OpenChunksWorldGenCounter.sel.neighborStatusUpdate_Asynch();
            }
        }

        

        bool meshGeneratingThread(int id, float time)
        {
            Vector3 center = RandomUpdateCenter();
            WorldPosition wp = new WorldPosition(center);
            int numBuiltChunks = 0;
            
            for (int radius = 0; radius <= OpenScreenRadius_Mesh && numBuiltChunks < 4; radius++)
            {
                FrameAroundIntV2 frame = new FrameAroundIntV2(wp.ChunkGrindex, radius);

                while (frame.Next())
                {
                    Chunk c = LfRef.chunks.GetScreenUnsafe(frame.Position);
                    if (c != null)
                    {
                        bool needGenerating = chunkReadyForMeshGenerating(c);

                        if (c.MeshNeedsReload && c.openstatus >= ScreenOpenStatus.Mesh_3)
                        {
                            needGenerating = true;
                        }
                        
                        if (needGenerating)
                        {
                            bool available = false;
                            lock (currentMeshGenerating)
                            {
                                if (!currentMeshGenerating.Contains(c.Index))
                                {
                                    currentMeshGenerating.Add(c.Index);
                                    available = true;
                                }
                            }

                            if (available)
                            {
                                c.generate4_Mesh(Ref.draw.heightmap, meshBuilders[id]);
                                numBuiltChunks++;
                                lock (currentMeshGenerating)
                                {
                                    currentMeshGenerating.Remove(c.Index);
                                }
                            }
                        }
                    }

                }
            }

            if (id == 0)
            {
                lowResChunkCollection.generateMesh(meshBuilders[id]);
            }
            
            return false;
        }


        //void lowResMeshGenerating_asynch()
        //{

        //}

        //bool areaAvailableForMeshGen(IntVector2 topLeft, IntVector2 bottomRight)
        //{
        //    topLeft -= 1;
        //    bottomRight += 1;

        //    IntVector2 pos = IntVector2.Zero;
        //    for (pos.Y = topLeft.Y; pos.Y <= bottomRight.Y; ++pos.Y)
        //    {
        //        for (pos.X = topLeft.X; pos.X <= bottomRight.X; ++pos.X)
        //        {
        //            var chunk = LfRef.chunks.GetScreenUnsafe(pos);
        //            if (chunk == null || chunk.openstatus < ScreenOpenStatus.Detail_2)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        //void compressChunksInMeshGenerating(IntVector2 topLeft, IntVector2 bottomRight, bool compress, bool onlyRemoveLock)
        //{
        //    topLeft -= 1;
        //    bottomRight += 1;

        //    IntVector2 pos = IntVector2.Zero;
        //    for (pos.Y = topLeft.Y; pos.Y <= bottomRight.Y; ++pos.Y)
        //    {
        //        for (pos.X = topLeft.X; pos.X <= bottomRight.X; ++pos.X)
        //        {
        //            var chunk = LfRef.chunks.GetScreenUnsafe(pos);
        //            if (chunk != null)
        //            {
        //                if (compress)
        //                {
        //                    chunk.chunkGeneratingLock--;

        //                    if (!onlyRemoveLock)
        //                    {
        //                        chunk.compressToList();
        //                    }
        //                }
        //                else
        //                {
        //                    chunk.chunkGeneratingLock++;
        //                    chunk.unCompressToGrid();
        //                }
        //            }
        //        }
        //    }
        //}

        //void compressChunksInMeshGenerating(IntVector2 topLeft, IntVector2 bottomRight, bool compress, bool onlyRemoveLock)
        //{
        //    topLeft -= 1;
        //    bottomRight += 1;

        //    IntVector2 pos = IntVector2.Zero;
        //    for (pos.Y = topLeft.Y; pos.Y <= bottomRight.Y; ++pos.Y)
        //    {
        //        for (pos.X = topLeft.X; pos.X <= bottomRight.X; ++pos.X)
        //        {
        //            var chunk = LfRef.chunks.GetScreenUnsafe(pos);
        //            if (chunk != null)
        //            {
        //                if (compress)
        //                {
        //                    chunk.chunkGeneratingLock--;

        //                    if (!onlyRemoveLock)
        //                    {
        //                        chunk.compressToList();
        //                    }
        //                }
        //                else
        //                {
        //                    chunk.chunkGeneratingLock++;
        //                    chunk.unCompressToGrid();
        //                }
        //            }
        //        }
        //    }
        //}

        //void chunkMeshGenCompleted

        const int LowDetaildEdgeThickness = 4;

        //void reloadChunksUpdate(int id)
        //{
        //    if (hasWaitingReloads)
        //    {
        //        //LfRef.chunks.OpenChunksList
        //        LfRef.chunks.OpenChunksMeshGenCounter.Reset();

        //        while (LfRef.chunks.OpenChunksMeshGenCounter.Next())
        //        {
        //            var c = LfRef.chunks.OpenChunksMeshGenCounter.Member;
        //            if (c.MeshNeedsReload && c.openstatus >= ScreenOpenStatus.Mesh_3)
        //            {
        //                c.generate4_Mesh(mesh, meshBuilders[id]);
        //            }
        //           // checkChunkReload(LfRef.chunks.OpenChunksMeshGenCounter.Member);
        //        }
        //    }
        //}

        bool chunkReadyForMeshGenerating(Chunk c)
        {
            if (c.openstatus == ScreenOpenStatus.Detail_2)
            {
                foreach (var dir in IntVector2.Dir4Array)
                {
                    IntVector2 pos = dir + c.Index;
                    var n = LfRef.chunks.GetScreenUnsafe(pos);
                    if (n == null || n.openstatus < ScreenOpenStatus.Detail_2)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }



        //void reloadChunksIfWaiting()
        //{
            
        //        reloadChunksUpdate();
        //}
        

        //void checkChunkReload(Chunk c)
        //{
        //    if (c.MeshNeedsReload && c.openstatus >= ScreenOpenStatus.Mesh_3)
        //    {
        //        c.generate4_Mesh(mesh, meshBuilder);
        //    }
        //}
        

        //void checkEnvObjectChange()
        //{
        //    if (currentEnvChange != null)
        //    {
        //        if (LfRef.chunks.ChunksDataLoaded(currentEnvChange.Position))
        //        {
        //            lock (EnvironmentObjectChangedList)
        //            {
        //                currentEnvChange.RunChange(LfRef.chunks.GetScreen(currentEnvChange.Position), EnvironmentObjectChangedList);
        //            }
        //            currentEnvChange = null;
        //        }
        //    }
        //    else if (EnvironmentObjectChangedList.Count > 0)
        //    {
        //        lock (EnvironmentObjectChangedList)
        //        {
        //            currentEnvChange = EnvironmentObjectChangedList[0];
        //            EnvironmentObjectChangedList.RemoveAt(0);
        //        }
        //        for (int loadPart = LoadScreenOrder.Length - 1; loadPart >= 0; loadPart--)
        //        {
        //            ForXYLoop loop = new ForXYLoop(IntVector2.NegativeOne, IntVector2.One);
        //            while (!loop.Done)
        //            {
        //                LfRef.chunks.GetScreen(loop.Next_Old() + currentEnvChange.Position).GeneratePart(LoadScreenOrder[loadPart], false);
        //            }
        //        }
        //    }
        //}
        public void AddEnvObjectChange(EnvironmentObjectChanged obj)
        {
            lock (EnvironmentObjectChangedList)
            {
                EnvironmentObjectChangedList.Add(obj);
            }
        }

        public int smallestHeroDistanceToChunk(IntVector2 chunk)
        {
            int smallestDist = int.MaxValue;
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)//foreach (GO.PlayerCharacter.AbsHero h in LfRef.LocalHeroes)
            {
                IntVector2 diff = LfRef.LocalHeroes[i].ChunkUpdateCenter;
                diff.Sub(chunk);
                smallestDist = lib.SmallestValue(smallestDist, diff.SideLength());
            }
            //Debug.Log("smallestDist:" + smallestDist.ToString());
            return smallestDist;
        }

        virtual protected bool clientIsUsingChunk(IntVector2 chunk)
        {
            return LfRef.gamestate.ClientIsUsingChunk(chunk); 
        }

        const int MaxCloseChunks = 16;
        //int numOpenScreens = 0;
        bool bReloadAllChunks = false;

        void closeChunksUpdate_asynch()
        {
            //Look for screens that is not in use

            if (bReloadAllChunks)
            {
                bReloadAllChunks = false;
                SpottedArrayCounter<Chunk> open = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
                while (open.Next())
                {
                    open.sel.RemoveMeshPointer();//.RemoveMesh(mesh);
                    open.sel.Close(true);
                }
                LfRef.chunks.OpenChunksList.Clear();
                //numOpenScreens = 0;
                return;
            }

            LfRef.chunks.OpenChunksWorldGenCounter.Reset();
            int closedChunksCount = 0;

            while (LfRef.chunks.OpenChunksWorldGenCounter.Next())
            {
                Chunk chunk = LfRef.chunks.OpenChunksWorldGenCounter.sel;
                if (chunk.lifeTimeSec() > 4f)
                {
                    if (closeDownChunk(chunk.Index, OpenScreenRadius_Mesh + RemoveMeshBufferLength, false))
                    {
                        if (++chunk.removeDelay > 12)
                        {
                            chunk.RemoveMeshPointer();
                        }
                        if (chunk.needToUpdateLowResChunk)
                        {
                            chunk.needToUpdateLowResChunk = false;
                            lowResChunkCollection.add(chunk);
                        }
                    }
                    else
                    {
                        chunk.removeDelay = 0;
                    }

                    if (closeDownChunk(chunk.Index, FullCloseRadius, true))
                    {
                        chunk.Close(true);
                        LfRef.chunks.RemoveChunk(chunk.Index);

                        if (++closedChunksCount >= MaxCloseChunks)
                        {
                            ChunkGeneratingSleep();
                            //numOpenScreens = 0;
                            break;
                        }
                    }
                }
            }

            clearedOutChunkMemory = closedChunksCount <= 2;
        }

        void areaStorageHeaderUpdate_Asynch()
        {
            if (areaStorageHeaderNeedsUpdate)
            {
                areaStorageHeaderNeedsUpdate = false;

                if (LfRef.gamestate.LocalHostingPlayer.hero.Level != null)
                {
                    LfRef.chunks.OpenChunksWorldGenCounter.Reset();

                    while (LfRef.chunks.OpenChunksWorldGenCounter.Next())
                    {
                        Chunk chunk = LfRef.chunks.OpenChunksWorldGenCounter.sel;
                        if (chunk.dataOriginOutdated())
                        {
                            chunk.Close(false);
                        }
                    }
                }
            }
        }

        bool closeDownChunk(IntVector2 chunk, int maxDistance, bool fullClose)
        {
            int smallestDist = smallestHeroDistanceToChunk(chunk);
            if (smallestDist > maxDistance)
            {
                if (fullClose)
                {
                    bool clientIsCloseToChunk = LfRef.gamestate.HeroIsCloseToChunk(chunk, Map.World.OpenScreenRadius_Mesh);

                    if (clientIsCloseToChunk)
                    {
                        return false;
                    }
                    if (clientIsUsingChunk(chunk) ||
                        (currentEnvChange != null && (currentEnvChange == null || currentEnvChange.ProtectedPosition(chunk))))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public void saveUpdate()
        {
            SpottedArrayCounter<Chunk> open = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
            while (open.Next())
            {
                open.sel.saveChanges();
            }
        }

        public void ReloadAllChunks()
        {
            bReloadAllChunks = true;
        }

        public static void ChunkGeneratingSleep()
        {
            //if (PlatformSettings.RunningXbox || Ref.gamesett.DetailLevel == 0)
            //{
            //    if (!Ref.inLoading)
            //    {
            //        System.Threading.Thread.Sleep(Ref.main.TargetElapsedTime.Milliseconds);
            //    }
            //}
        }

        //virtual protected void closingChunk(IntVector2 chunk)
        //{
        //    //LfRef.worldOverView.EnvironmentObjectQue.ClosingChunk(chunk, LfRef.gamestate.LocalHostingPlayer);   
        //    new Director.BeginClosingChunk(chunk, LfRef.gamestate.LocalHostingPlayer);  
        //}

        void closeScreen(Chunk s)
        {
            Debug.CrashIfThreaded();
            s.RemoveMeshPointer();//.RemoveMesh(mesh);
            s.Close(true);
            LfRef.chunks.RemoveChunk(s.Index);
            LfRef.levels2.chunkHostDirector.StopHostingChunk(s.Index, LfRef.gamestate.LocalHostingPlayer);
        }

        public void QuickCloseChunk(IntVector2 index)
        {
            Chunk s = LfRef.chunks.GetScreenUnsafe(index);
            if (s != null && s.openstatus >= Map.ScreenOpenStatus.Mesh_3)
            {
                s.RemoveMeshPointer();//RemoveMesh(mesh);
                s.Close(true);
                LfRef.chunks.RemoveChunk(s.Index);
            }
        }

        

        virtual public Vector3 RandomUpdateCenter()
        {
                //Calculate center
                GO.PlayerCharacter.AbsHero hero = LfRef.LocalHeroes.GetRandom();
                Vector3 center = hero.player.CamTargetPosition();
            
                if (!hero.player.InLoadingScene)
                {
                    center.X = Bound.Min(center.X, WorldPosition.WorldEdgeSafetyDistance);
                    center.Z = Bound.Min(center.Z, WorldPosition.WorldEdgeSafetyDistance);
                }

                return center;
            
        }

        public void addEditorPacket(Editor.EditorPacket packet)
        {
            lock (editorPacketQue)
            {
                editorPacketQue.Add(packet);
            }
        }

        void readEditorPacketUpdate_asynch()
        {
            while (editorPacketQue.Count > 0)
            {
                Editor.EditorPacket packet = editorPacketQue[0];
                
                {//Try open area
                    bool generationHappened, chunkReachedDetailStatus;

                    IntVector2 min = packet.minWp.ChunkGrindex;
                    IntVector2 max = packet.maxWp.ChunkGrindex;

                    IntVector2 chunk = IntVector2.Zero;
                    for (chunk.Y = min.Y; chunk.Y <= max.Y; chunk.Y++)
                    {
                        for (chunk.X = min.X; chunk.X <= max.X; chunk.X++)
                        {
                            generateChunkDetail(chunk, out generationHappened, out chunkReachedDetailStatus);

                            if (!chunkReachedDetailStatus)
                            {
                                return;
                            }
                        }
                    }
                }

                lock (editorPacketQue)
                {
                    editorPacketQue.RemoveAt(0);
                }

                packet.read();
            }
        }

        public IntVector2 currentlyOpeningScreen = IntVector2.Zero;

        void openScreensUpdate_asynch()
        {
            //if (!LfRef.WorldHost && !LfRef.gamestate.HasRecievedWorldOverview)
            //{
            //    Thread.Sleep(400);
            //    return;
            //}

            const int NumBuildChunks = 8;
            
            Vector3 center = RandomUpdateCenter();
            WorldPosition wp = new WorldPosition(center);
            int numBuiltChunks = 0;

            //search for an unfinished chunk, start from center and out
            for (int radius = 0; radius <= OpenScreenRadius_AreaDetail; radius++)
            {
                FrameAroundIntV2 frame = new FrameAroundIntV2(wp.ChunkGrindex, radius);

                while (frame.Next())
                {
                    currentlyOpeningScreen = frame.Position;

                    bool generationHappened, chunkReachedDetailStatus;
                    generateChunkDetail(frame.Position, out generationHappened, out chunkReachedDetailStatus);

                    if (generationHappened)
                    {
                        if (++numBuiltChunks >= NumBuildChunks)
                        {
                            return;
                        }
                    }
                }
            }

            openScreensForLowRes(wp.ChunkGrindex);
        }

        void generateChunkDetail(IntVector2 chunkIx, out bool generationHappened, out bool chunkReachedDetailStatus)
        {
            Chunk chunk = LfRef.chunks.GetScreen(chunkIx);

            if (chunk.openstatus < ScreenOpenStatus.Detail_2 &&
                chunk.openstatus != ScreenOpenStatus.LoadingOrRecievingData_1b)
            {
                IntVector2 pos = IntVector2.Zero;
                for (pos.Y = -1; pos.Y <= 1; pos.Y++)
                {
                    for (pos.X = -1; pos.X <= 1; pos.X++)
                    {
                        var nchunk = LfRef.chunks.GetScreen(chunkIx + pos);
                        nchunk.generate2_HeightMap();
                    }
                }

                chunk.generate3_Detail();
                if (chunk.preparedDataOrigin != ChunkDataOrigin.Loaded)
                {
                    lowResChunkCollection.add(chunk);
                }

                generationHappened = true;
            }
            else
            {
                generationHappened = false;
            }

            chunkReachedDetailStatus = chunk.openstatus >= ScreenOpenStatus.Detail_2;
        }

        

        FrameAroundIntV2 lowResFrame;
        int lowresRadius = -1;
     
        void openScreensForLowRes(IntVector2 center)
        {
            if (clearedOutChunkMemory)
            {
                clearedOutChunkMemory = false;

                const int ChunksWidth = 2;
                const int HalfChunksWidth = ChunksWidth / 2;

                if ((lowResFrame.center - center).SideLength() > 2)
                {
                    //Walked too far, start over
                    lowresRadius = -1;
                }

                int innerRadius = OpenScreenRadius_AreaDetail + HalfChunksWidth;

                if (lowresRadius < 0)
                {
                    lowresRadius = 0;
                    lowResFrame = new FrameAroundIntV2(center, innerRadius);

                    lowResFrame.Next();
                }
                else
                {
                    for (int i = 0; i < ChunksWidth; ++i)
                    {
                        if (!lowResFrame.Next())
                        {
                            lowresRadius++;
                            if (lowresRadius > 4)
                            {
                                lowresRadius = 0;
                            }
                            lowResFrame = new FrameAroundIntV2(center, innerRadius + HalfChunksWidth * lowresRadius);
                            break;
                        }
                    }
                }

                ForXYLoop loop = new ForXYLoop(lowResFrame.Position - HalfChunksWidth, lowResFrame.Position + HalfChunksWidth);
                while (loop.Next())
                {
                    bool generationHappened, chunkReachedDetailStatus;
                    generateChunkDetail(loop.Position, out generationHappened, out chunkReachedDetailStatus);
                }
            }
        }

        //void compressUpdate_asynch(float time)
        //{
        //    SpottedArrayCounter<Chunk> open = new SpottedArrayCounter<Chunk>(LfRef.chunks.OpenChunksList);
        //    while (open.Next())
        //    {
        //        int smallestDist = smallestHeroDistanceToChunk(open.Member.Index);

        //        if (smallestDist < RemoveGameObjectRadius)
        //        {
        //            open.Member.unCompressToGrid();
        //        }
        //        else
        //        {
        //            open.Member.compressUpdate();
        //        }
        //    }
        //}

        public IntVector2 currentGeneratingGoChunk;

        public void GenerateGameObjectsUpdate()
        {
            const float UpdateMoveLength = 20;
            var hero = LfRef.LocalHeroes.GetRandom();
            if (hero != null && hero.WorldPos.InsideSafeArea)
            {
                Vector3 diff = hero.previousGameObjectGeneratingPos - hero.Position;

                if (diff.Length() >= UpdateMoveLength ||
                    hero.gameObjectGeneratingComplete && Ref.rnd.Chance(5))
                {
                    hero.previousGameObjectGeneratingPos = hero.Position;
                    hero.gameObjectGeneratingLoop = new ForXYLoop(hero.ChunkUpdateCenter - GenerateGameObjectRadius, hero.ChunkUpdateCenter + GenerateGameObjectRadius);
                    hero.gameObjectGeneratingComplete = false;
                }

                if (!hero.gameObjectGeneratingComplete)
                {
                    while (hero.gameObjectGeneratingLoop.Next())
                    {
                        Chunk c = LfRef.chunks.GetScreen(hero.gameObjectGeneratingLoop.Position);
                        if (c.openstatus >= ScreenOpenStatus.Mesh_3 &&
                            c.generatedGameObjects == false)
                        {
                            var generateGo = LfRef.levels2.chunkHostDirector.AddHostRequest(c.Index, null);

                            if (generateGo != Director.GenerateOwnerResult.WaitingForReply &&
                                c.level != null &&
                                c.level.generated)
                            {
                                bool hostingGoGeneration = generateGo == Director.GenerateOwnerResult.GenerateNow;
                                currentGeneratingGoChunk = c.Index;
                                c.generatedGoHero = hero;
                               
                                LfRef.spawner.GeneratingGameobjects(c, hostingGoGeneration);
                                c.generatedGameObjects = true;
                                goto closeGoUpdate;
                            }
                        }
                    }
                    hero.gameObjectGeneratingComplete = true;
                }

                closeGoUpdate:

                for (int i = 0; i < 4; ++i)
                {
                    //Close chunks update
                    //Pick one random and see if it is outside active area
                    Chunk c = LfRef.chunks.OpenChunksList.GetRandomUnsafe(Ref.rnd);
                    if (c != null && c.generatedGameObjects)
                    {
                        //for (int j = 0; j < LfRef.AllHeroes.Count; ++j)
                        //{
                        //    if ((c.Index - LfRef.AllHeroes[j].ScreenPos).SideLength() < RemoveGameObjectRadius)
                        //    {
                        //        goto checkedChunkComplete;
                        //    }
                        //}
                        //not close to any hero

                        if (!LfRef.gamestate.HeroIsCloseToChunk(c.Index, RemoveGameObjectRadius))
                        {
                            //c.DegenerateGameObjects();
                            LfRef.levels2.chunkHostDirector.StopHostingChunk(c.Index, hero.player);
                        }
                    }
                }
            }
        }

        public static void FillArea(IntervalIntV3 area, Data.MaterialType material)
        {
            byte val = (byte)material;

            IntVector3 min = new IntVector3();
            min = area.Min; 
            IntVector3 max = new IntVector3();
            max = area.Max;

            for (int z = min.Z; z <= max.Z; z++)
            {
                for (int y = min.Y; y <= max.Y; y++)
                {
                    for (int x = min.X; x <= max.X; x++)
                    {
                        new WorldPosition(new IntVector3(x, y, z)).SetBlock(val);//LfRef.chunks.Set(new WorldPosition(new IntVector3(x, y, z)), val);
                    }
                }
            }

        }
    }

    class EnvironmentObjectChanged
    {
        EnvironmentChangedType type;
        byte[] data;
        IntVector2 pos;
        public IntVector2 Position
        {
            get { return pos; }
        }



        public EnvironmentObjectChanged(IntVector2 pos, System.IO.BinaryReader r, EnvironmentChangedType type)
        {
            this.type = type;
            this.pos = pos;
            this.data = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
        }

        /// <summary>
        /// avoid closing the screen until its done
        /// </summary>
        public bool ProtectedPosition(IntVector2 position)
        {
            if (type == EnvironmentChangedType.ObjectRemoved)
                return pos == position;
            else
                return pos.SideLength() <= 1;
        }


        //public void RunChange(Chunk screen, List<EnvironmentObjectChanged> EnvironmentObjectChangedList)
        //{
        //    //System.IO.BinaryReader r = new System.IO.BinaryReader();
        //    //r.BaseStream.Write(data, 0, data.Length);
        //    //r.Position = 0;
        //    //switch (type)
        //    //{
        //    //    case EnvironmentChangedType.DoorOpenClose:
        //    //        screen.ReadOpenCloseDoor(r);
        //    //        break;
        //    //    case EnvironmentChangedType.ObjectRemoved:
        //    //        screen.NetReadRemoveChunkObject(r);
        //    //        break;
        //    //}
            

        //    //if (EnvironmentObjectChangedList != null)
        //    //{
        //    //    for (int i = 0; i < EnvironmentObjectChangedList.Count; i++)
        //    //    {
        //    //        if (EnvironmentObjectChangedList[i].Position == pos)
        //    //        {
        //    //            EnvironmentObjectChangedList[i].RunChange(screen, null);
        //    //            EnvironmentObjectChangedList.RemoveAt(i);
        //    //            i--;
        //    //        }
        //    //    }
        //    //}
        //}
    }
    enum EnvironmentChangedType
    {
        DoorOpenClose,
        ObjectRemoved,
    }
}
