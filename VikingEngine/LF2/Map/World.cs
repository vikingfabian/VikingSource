using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Threading;

namespace VikingEngine.LF2.Map
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
                        //closeScreen(s);
                    }
                }
            }
        }

        #endregion

        public static bool RunningAsHost = true;
        Graphics.LFHeightMap mesh;
        Terrain.ScreenMeshBuilder7 meshBuilder = new Terrain.ScreenMeshBuilder7();
        IntVector2 centerScreen = IntVector2.NegativeOne;
        const int OpenScreenRadius_EnemySpawn = 3;


        public const int StanadardOpenRadius = 4;
        public static int OpenScreenRadius_Mesh = StanadardOpenRadius;
        static int OpenScreenRadius_AreaDetail = OpenScreenRadius_Mesh + 1;
        static int OpenScreenRadius_HeightMap = OpenScreenRadius_AreaDetail + 1;
        public static short OpenScreenRadius_TopographData = (short)(OpenScreenRadius_HeightMap + 1);
        public static int DecompressScreenRadius = OpenScreenRadius_Mesh + 1;
        static int OpenScreenWidth = OpenScreenRadius_Mesh * 2 + 1;
        //static int MaxOpenScreens = lib.Square(OpenScreenRadius_HeightMap * PublicConstants.Twice + 1) -50;
        static int CheckRadius = OpenScreenRadius_Mesh - 1;

        public const int InvisibleWallRadius = (Map.World.StanadardOpenRadius + 5) * Map.WorldPosition.ChunkWidth;

        //Thread updateMapThread;
        List<EnvironmentObjectChanged> EnvironmentObjectChangedList = new List<EnvironmentObjectChanged>();
        EnvironmentObjectChanged currentEnvChange = null;

        public static bool hasWaitingReloads = false;

        public static void ReloadChunkMesh(WorldPosition from, IntVector3 size)
        {
            ReloadChunkMesh(from, from.GetNeighborPos(size));
        }

        public static void ReloadChunkMesh(WorldPosition from, WorldPosition to)
        {
            IntVector2 min = new IntVector2(lib.SmallestValue(from.ChunkX, to.ChunkX), lib.SmallestValue(from.ChunkY, to.ChunkY));
            IntVector2 max = new IntVector2(lib.LargestValue(from.ChunkX, to.ChunkX), lib.LargestValue(from.ChunkY, to.ChunkY));
            //max 
            IntVector2 chunk = IntVector2.Zero;
            for (chunk.Y = min.Y; chunk.Y <= max.Y; chunk.Y++)
            {
                for (chunk.X = min.X; chunk.X <= max.X; chunk.X++)
                {
                   Map.Chunk s= LfRef.chunks.GetScreenUnsafe(chunk);//Map.World.ReloadChunkMesh(chunk);
                   if (s != null) s.MeshNeedsReload = true;
                   hasWaitingReloads = true;
                }
            }
        }

        
        public static void ReloadChunkMesh(IntVector2 pos)
        {
            Chunk s = LfRef.chunks.GetScreenUnsafe(pos);
            if (s != null)
            {
                s.MeshNeedsReload = true;
                hasWaitingReloads = true;
            }
        }
        public World(bool generateNewWorld, bool createOverview)
        {
            //måste ligga i en egen tråd
            //reloadChunkMesh = new List<IntVector2>();
            LfRef.chunks = new WorldChunks();
            //Save the areas to storage if this is the first time

            if (createOverview)
                new WorldOverview(this);
            if (RunningAsHost)
            {
                
            }
            else
            {
                Data.WorldsSummaryColl.CurrentWorld.SetVisitorStatus();
                
            }

#if CMODE
            LfRef.worldOverView.FreeMemory();
#endif

        }

        public void UpdateHeightMapLights()
        {
            mesh.UpdateLighting();
        }

        public void GameStart()
        {
            mesh = new LFHeightMap();

            new Director.FrustumCullingDirector(mesh);

            Ref.asynchUpdate.AddUpdateThread(terrainGeneratingDataUpdate, "terrain Generating Data Update");
        }

        public static void UpdateChunkLoadRadius()
        {
            OpenScreenRadius_Mesh = LfRef.LocalHeroes.Count <= 2 ? StanadardOpenRadius : (StanadardOpenRadius - 1);
            OpenScreenRadius_AreaDetail = OpenScreenRadius_Mesh + 1;
            OpenScreenRadius_HeightMap = OpenScreenRadius_AreaDetail + 1;
            OpenScreenRadius_TopographData = (short)(OpenScreenRadius_HeightMap + 1);
            DecompressScreenRadius = OpenScreenRadius_Mesh + 1;
            OpenScreenWidth = OpenScreenRadius_Mesh * 2 + 1;
            //MaxOpenScreens = (OpenScreenRadius_TopographData * PublicConstants.Twice + 1) * (OpenScreenRadius_TopographData * PublicConstants.Twice + 1) + 1;
            CheckRadius = OpenScreenRadius_Mesh - 1;
        }


        void terrainGeneratingDataUpdate(float time)
        {
            removeOutdatedChunksUpdate();
            closeChunksUpdate();
            OpenScreensUpdate();
            checkEnvObjectChange();
            //reloadChunks();
        }

        void reloadChunksIfWaiting()
        {
            if (hasWaitingReloads)
                reloadChunksUpdate();
        }
        void reloadChunksUpdate()
        {
            //LfRef.chunks.OpenChunksList
            LfRef.chunks.OpenChunksWorldGenCounter.Reset();

            while (LfRef.chunks.OpenChunksWorldGenCounter.Next())
            {
                checkChunkReload(LfRef.chunks.OpenChunksWorldGenCounter.Member);
            }
        }

        void checkChunkReload(Chunk c)
        {
            if (c.MeshNeedsReload)
            {
                c.generate4_Mesh(mesh, meshBuilder);
            }
        }
        

        void checkEnvObjectChange()
        {
            if (currentEnvChange != null)
            {
                if (LfRef.chunks.ChunksDataLoaded(currentEnvChange.Position))
                {
                    lock (EnvironmentObjectChangedList)
                    {
                        currentEnvChange.RunChange(LfRef.chunks.GetScreen(currentEnvChange.Position), EnvironmentObjectChangedList);
                    }
                    currentEnvChange = null;
                }
            }
            else if (EnvironmentObjectChangedList.Count > 0)
            {
                lock (EnvironmentObjectChangedList)
                {
                    currentEnvChange = EnvironmentObjectChangedList[0];
                    EnvironmentObjectChangedList.RemoveAt(0);
                }
                for (int loadPart = LoadScreenOrder.Count - 1; loadPart >= 0; loadPart--)
                {
                    ForXYLoop loop = new ForXYLoop(IntVector2.NegativeOne, IntVector2.One);
                    while (!loop.Done)
                    {
                        LfRef.chunks.GetScreen(loop.Next_Old() + currentEnvChange.Position).GeneratePart(LoadScreenOrder[loadPart], false);
                    }
                }
            }
        }
        public void AddEnvObjectChange(EnvironmentObjectChanged obj)
        {
            lock (EnvironmentObjectChangedList)
            {
                EnvironmentObjectChangedList.Add(obj);
            }
        }

        virtual protected int smallestHeroDistanceToChunk(IntVector2 chunk)
        {
            int smallestDist = int.MaxValue;
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)//foreach (GameObjects.Characters.Hero h in LfRef.LocalHeroes)
            {
                IntVector2 diff = LfRef.LocalHeroes[i].ChunkUpdateCenter;
                diff.Sub(chunk);
                smallestDist = lib.SmallestValue(smallestDist, lib.LargestValue(Math.Abs(diff.X), Math.Abs(diff.Y)));
            }
            return smallestDist;
        }

        virtual protected bool clientIsUsingChunk(IntVector2 chunk)
        {
            return LfRef.gamestate.ClientIsUsingChunk(chunk); 
        }


        int numOpenScreens = 0;
        void closeChunksUpdate()
        {
            //CLOSE
            //Look for screens that is not in use

            if (LfRef.chunks.OpenChunksList.Count != numOpenScreens)
            {
                LfRef.chunks.OpenChunksWorldGenCounter.Reset();

                bool close = false;

                while(LfRef.chunks.OpenChunksWorldGenCounter.Next())
                {
                    Chunk s = LfRef.chunks.OpenChunksWorldGenCounter.Member;
                    //Chunk s = LfRef.chunks.OpenChunksList[i];
                    //lock (LfRef.chunks.OpenChunksList)
                    //{
                    //    s = LfRef.chunks.OpenChunksList[Ref.rnd.Int(LfRef.chunks.OpenChunksList.Count)];
                    //}

                    int smallestDist = smallestHeroDistanceToChunk(s.Index);
                    if (smallestDist > OpenScreenRadius_AreaDetail)
                    {
                        bool clientIsCloseToChunk = LfRef.gamestate.ClientIsCloseToChunk(s.Index);

                        //make it unvisible
                        s.RemoveMesh(mesh);
                        if (!clientIsCloseToChunk)
                        {
                            s.beginDegenerateGameObjects();
                        }

                        if (smallestDist > OpenScreenRadius_HeightMap)
                        {
                            //close the screen and store the changes
                            //check if any clients are active close to the chunk
                            //if so, keep it alive
                            close = true;

                            if (clientIsCloseToChunk)
                            {
                                close = false;
                            }
                            if (clientIsUsingChunk(s.Index) ||
                                s.LockScreen ||
                                (currentEnvChange != null && (currentEnvChange == null || currentEnvChange.ProtectedPosition(s.Index))))
                            {
                                close = false;
                            }

                            if (close)
                            {
                                s.CloseAndSave();
                                LfRef.chunks.RemoveChunk(s.Index);
                                closingChunk(s.Index);
                            }
                        }
                    }
                    if (!close)
                    {
                        checkChunkReload(s);
                    }
                }

                numOpenScreens = LfRef.chunks.OpenChunksList.Count;
            }
        }

        virtual protected void closingChunk(IntVector2 chunk)
        {
            //LfRef.worldOverView.EnvironmentObjectQue.ClosingChunk(chunk, LfRef.gamestate.LocalHostingPlayer);   
            new Director.BeginClosingChunk(chunk, LfRef.gamestate.LocalHostingPlayer);  
        }

        void closeScreen(Chunk s)
        {
            Debug.CrashIfThreaded();
            s.CloseAndSave();
            LfRef.chunks.RemoveChunk(s.Index);
            LfRef.worldOverView.EnvironmentObjectQue.ClosingChunk(s.Index, LfRef.gamestate.LocalHostingPlayer);
        }

        public void QuickCloseChunk(IntVector2 index)
        {
            Chunk s = LfRef.chunks.GetScreenUnsafe(index);
            if (s != null && s.Openstatus >= Map.ScreenOpenStatus.MeshGeneratedDone)
            {
                s.RemoveMesh(mesh);
                s.CloseAndSave();
                LfRef.chunks.RemoveChunk(s.Index);
            }
        }

        static readonly List<ScreenOpenStatus> LoadScreenOrder = new List<ScreenOpenStatus>
            {
                //ScreenOpenStatus.DotMapDone,
                ScreenOpenStatus.HeightMapToDataGrid,
                ScreenOpenStatus.GotTopographic,
            };

        virtual public Vector3 RandomUpdateCenter()
        {
                //Calculate center
                GameObjects.Characters.Hero hero = LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
                Vector3 center = hero.Player.CamTargetPosition();//hero.Player.ControllerLink.view.Camera.Position;


                center.X = Bound.Min(center.X, Map.World.InvisibleWallRadius);
                center.Z = Bound.Min(center.Z, Map.World.InvisibleWallRadius);


                const float MaxTargetLength = 48;
                Vector3 targetDiff = hero.Player.localPData.view.Camera.LookTarget - center;

                if (targetDiff.Length() > MaxTargetLength)
                {
                    targetDiff = lib.SafeNormalizeV3(targetDiff);// .Normalize();
                    targetDiff *= MaxTargetLength;
                }
                center += targetDiff;

                const float MoveDirAdd = 48;
                Rotation1D moveDir = hero.Player.CamTargetMoveDir();
                center += WorldPosition.V2toV3(moveDir.Direction(MoveDirAdd));
                return center;
            
        }

        void OpenScreensUpdate()
        {
            if (!Map.World.RunningAsHost && !LfRef.gamestate.HasRecievedWorldOverview)
            {
                Thread.Sleep(400);
                return;
            }

            const int NumBuildChunks = 4;
            //NEW
            /*
             * Centrum ligger i cam fokus
             * skjut center åt speed dir
             * Leta från centrum ut för att hitta en screen utan mesh
             * Hittad screen ska ladda alla omkring liggande i Height och dotmap
             */

            
            Vector3 center = RandomUpdateCenter();
            WorldPosition wp = new WorldPosition(center);
            int numBuiltChunks = 0;

            //search for an unfinished chunk, start from center and out
            for (int radius = 0; radius < OpenScreenRadius_Mesh; radius++)
            {
                FrameAroundIntV2 frame = new FrameAroundIntV2(wp.ChunkGrindex, radius);
                while (frame.Next())
                {
                    if (frame.Position == new IntVector2(167, 167))
                    {
                        lib.DoNothing();
                    }

                    //IntVector2 checkScreen = frame.Position;
                    Chunk s = LfRef.chunks.GetScreen(frame.Position);
                    if ((s.Openstatus < ScreenOpenStatus.MeshGeneratedDone || s.MeshNeedsReload) && s.Openstatus != ScreenOpenStatus.LoadingOrRecievingData)
                    {
                        
                        //Make sure all sorrounding chunks has its data loaded
                        for (int loadRadius = LoadScreenOrder.Count; loadRadius > 0; loadRadius--)
                        {
                            ScreenOpenStatus status = LoadScreenOrder[loadRadius -1];
                            IntVector2 pos = IntVector2.Zero;
                            for (pos.Y = -loadRadius; pos.Y <= loadRadius; pos.Y++)
                            {
                                for (pos.X = -loadRadius; pos.X <= loadRadius; pos.X++)
                                {
                                    LfRef.chunks.GetScreen(frame.Position + pos).GeneratePart(status, false);
                                }
                            }
                        }

                        //mesh generating
                        s.generate4_Mesh(mesh, meshBuilder);
                        if (++numBuiltChunks >= NumBuildChunks)
                        {
                            return;
                        }
                        reloadChunksIfWaiting();
                    }
                    else if (s.WaintingForNetworkData)
                    {
                        s.RequestChunkData();
                    }
                }
            }
        }

        public static void FillArea(RangeIntV3 area, Data.MaterialType material)
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
                        LfRef.chunks.Set(new WorldPosition(new IntVector3(x, y, z)), val);
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
            this.data = r.ReadBytes(r.Length - r.Position);
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


        public void RunChange(Chunk screen, List<EnvironmentObjectChanged> EnvironmentObjectChangedList)
        {
            System.IO.BinaryReader r = new System.IO.BinaryReader();
            r.BaseStream.Write(data, 0, data.Length);
            r.Position = 0;
            switch (type)
            {
                case EnvironmentChangedType.DoorOpenClose:
                    screen.ReadOpenCloseDoor(r);
                    break;
                case EnvironmentChangedType.ObjectRemoved:
                    screen.NetReadRemoveChunkObject(r);
                    break;
            }
            

            if (EnvironmentObjectChangedList != null)
            {
                for (int i = 0; i < EnvironmentObjectChangedList.Count; i++)
                {
                    if (EnvironmentObjectChangedList[i].Position == pos)
                    {
                        EnvironmentObjectChangedList[i].RunChange(screen, null);
                        EnvironmentObjectChangedList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
    enum EnvironmentChangedType
    {
        DoorOpenClose,
        ObjectRemoved,
    }
}
