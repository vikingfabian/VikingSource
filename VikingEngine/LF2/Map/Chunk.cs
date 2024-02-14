using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
//

namespace VikingEngine.LF2.Map
{
    class Chunk : DataLib.ISaveTostorageCallback, ISpottedArrayMember
    {
        static readonly IntervalF AboutCenter = IntervalF.FromRadius(PublicConstants.Half, 0.1f);
        static RecycleInstances<byte[, ,]> dataGridBin = new RecycleInstances<byte[, ,]>(8);

        public IntVector2 Index; //Chunk placement in the world grid
        public byte[, ,] DataGrid = null;
        Graphics.VertexAndIndexBuffer vbPointer = null;


        //bool loading = false;
#if WINDOWS
        public bool ClientEditingFlag = false;
#endif
        /// </summary>Cant be closed<summary>
        public bool LockScreen = false;
        List<GameObjects.EnvironmentObj.MapChunkObject> chunkObjects = null;

        /// <summary>
        /// The object will be removed when the mesh is no longer visual
        /// </summary>
        List<IDeleteable> connectedObjects = null;


        public Chunk(IntVector2 index)
        {
            this.Index = index;
            createEmptyGrid();
            System.Threading.Thread.Sleep(Ref.main.TargetElapsedTime.Milliseconds);
        }

        void createEmptyGrid()
        {
            DataGrid = dataGridBin.PullFromBin();
            if (DataGrid == null)
            {
                DataGrid = new byte[WorldPosition.ChunkWidth, WorldPosition.ChunkHeight, WorldPosition.ChunkWidth];
            }
        }

        public void OnDestroy()
        {
            if (!dataGridBin.PushToBin(DataGrid))
            {
                for (int y = 0; y < WorldPosition.ChunkHeight; ++y)
                {
                    for (int z = 0; z < WorldPosition.ChunkWidth; ++z)
                    {
                        for (int x = 0; x < WorldPosition.ChunkWidth; ++x)
                        {
                            DataGrid[x, y, z] = byte.MinValue;
                        }
                    }
                }
            }
        }

        public void AddConnectedObject(IDeleteable obj)
        {
            if (connectedObjects == null)
                connectedObjects = new List<IDeleteable>(8);
            connectedObjects.Add(obj);

#if WINDOWS
            if (obj is GameObjects.EnvironmentObj.MapChunkObject)
                Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Connected MapChunkObject to chunk: " + obj.ToString());
#endif
        }
        public void beginDegenerateGameObjects()
        {
            if (connectedObjects != null)
                new Timer.Action0ArgTrigger(degenerateGameObjects);
        }
        void degenerateGameObjects()
        {
            if (connectedObjects != null)
            {
                foreach (IDeleteable obj in connectedObjects)
                    obj.DeleteMe();

                connectedObjects = null;
            }
        }
        void beginGenerate5_GameObjects()
        {
            if (!emptyAreaData)
            {
                new Timer.Action0ArgTrigger(generate5_GameObjects);
            }
            else
            {
                completedDataOrigin = preparedDataOrigin;
                //preparedDataOrigin = ChunkDataOrigin.NON;
            }
        }
        void generate5_GameObjects()
        {
            if (connectedObjects == null)
            {
                Terrain.Area.AbsArea area = LfRef.worldOverView.GetArea(Index);
                if (area != null)
                {
                   // area.GenerateChunkGameObjects(this, preparedDataOrigin == Map.ChunkDataOrigin.Generated);
                    area.BuildOnChunk(this, preparedDataOrigin == Map.ChunkDataOrigin.Generated, true);
                }
            }
            completedDataOrigin = preparedDataOrigin;
           // preparedDataOrigin = ChunkDataOrigin.NON;
        }
        
        public bool MeshNeedsReload = false;
        ChunkDataOrigin preparedDataOrigin = ChunkDataOrigin.NON;
        public ChunkDataOrigin PreparedDataOrigin
        { get { return preparedDataOrigin; } }


        ChunkDataOrigin completedDataOrigin = ChunkDataOrigin.NON;
        public ChunkDataOrigin DataOrigin
        {
            get { return completedDataOrigin; }
        }
        ScreenOpenStatus openstatusPrivate = ScreenOpenStatus.Closed;
        ScreenOpenStatus openstatus
        {
            get { return openstatusPrivate; }
            set
            {
                openstatusPrivate = value;
            }
        }
        public bool CorrectLoaded
        {
            get
            {
                return completedDataOrigin != ChunkDataOrigin.NON;
            }
        }
        public ScreenOpenStatus Openstatus
        {
            get { return openstatus; }
            set { openstatus = value; }
        }
        public bool DataGridLoadingComplete
        { get { return openstatus >= ScreenOpenStatus.DataGridComplete; } }

        bool writeProtected = false;
        public bool WriteProtected
        {
            get { return writeProtected || chunkData.AreaType == Terrain.AreaType.PrivateHome; }
            set 
            {
                
                writeProtected = value;
                if (value)
                {
                    unsavedChanges = false;
                    LfRef.worldOverView.WriteProtectedChunk(Index);
                }
                
            }
        }
        //new
       Terrain.ScreenTopographicData topographData;
       public Terrain.ScreenTopographicData TopographData
        {
            get 
            {
                generate1_Topographic();
                return topographData;
            }
        }
       static readonly List<IntVector2> SourroundingSides = new List<IntVector2> 
            { new IntVector2(0, -1), new IntVector2(0, 1), 
              new IntVector2(-1, 0), new IntVector2(1, 0), };

       public ChunkData chunkData
       {
           get { return LfRef.worldOverView.GetChunkData(Index); }
       }

        ChunkModifiableType ModifiableType
        {
            get
            {
                ChunkData data = chunkData;

                if (data.AreaType == Terrain.AreaType.PrivateHome)
                    return ChunkModifiableType.PrivateOwner;

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

       public void ReadOpenCloseDoor(System.IO.BinaryReader r)
       {
           if (DataGridLoadingComplete && chunkObjects != null)
           {
               IntVector3 pos = IntVector3.FromByteSzStream(r);
               foreach (GameObjects.EnvironmentObj.MapChunkObject obj in chunkObjects)
               {
                   if (obj.MapChunkObjectType == GameObjects.EnvironmentObj.MapChunkObjectType.Door &&
                       ((GameObjects.EnvironmentObj.Door)obj).ReadOpenClose(pos, r))
                   {
                       return;
                   }
               }
           }
       }
       public void SaveComplete(bool save, int player, DataLib.AbsSaveToStorage obj, bool failed)
       {
           if (!save && failed)
           {
               System.Diagnostics.Debug.WriteLine("LOAD CHUNK FAILED: " + Index.ToString());
           }
       }
        public void generate1_Topographic()
        {
            if (openstatus < ScreenOpenStatus.GotTopographic)
            {
                if (topographData == null)//temp
                {
                    //check the area type
                    //the first the screen will load
                    //needed to feed sourronding screens with info
                    topographData = new Terrain.ScreenTopographicData(Index, chunkData.AreaType, (int)chunkData.Environment);
                    openstatus = ScreenOpenStatus.GotTopographic;
                }
            }
        }

       
        public void RestoreToGenerated()
        {
            DataStream.DataStreamHandler.BeginUserRemoveFile(Path);
            LfRef.worldOverView.UnChangedChunk(Index);
            unsavedChanges = false;

        }

        public bool PathFindingPass(WorldPosition wp, ref int CurrentY)
        {
            const int MaxHeight = 3;
            for (int i = 0; i < MaxHeight; i++)
            {
                if (DataGrid[wp.LocalBlockX, CurrentY, wp.LocalBlockZ] == byte.MinValue)
                {
                    return true;
                }
                CurrentY++;
            }
            return false;
        }


        public void NetReadRemoveChunkObject(System.IO.BinaryReader r)
        {
            int i = r.ReadByte();
            if (chunkObjects != null && chunkObjects.Count > i)
            {
                chunkObjects[i].ChunkDeleteEvent();
                chunkObjects.RemoveAt(i);
            }
            ChangedData();
        }

        public void AddChunkObject(GameObjects.EnvironmentObj.MapChunkObject obj, bool add)
        {
            if (add)
            {
                if (PlatformSettings.RunningWindows)
                {
                    if (obj == null)
                    {
                        throw new NullReferenceException();
                    }
                }

                if (chunkObjects == null)
                    chunkObjects = new List<GameObjects.EnvironmentObj.MapChunkObject> { obj };
                else if (!chunkObjects.Contains(obj))
                {
                    chunkObjects.Add(obj);
                }
            }
            else
            {
                if (chunkObjects != null)
                {
                    //net share
                    chunkObjects.Remove(obj);
                }
            }


            if (obj.NeedToBeStored && !writeProtected)
                unsavedChanges = true;
        }

        public void RectangleFill(RangeIntV3 area, byte material)
        {
            area.Min.X = Bound.Min(area.Min.X, 0); area.Max.X = lib.SetMaxVal(area.Max.X, Map.WorldPosition.ChunkWidth);
            area.Min.Y = Bound.Min(area.Min.Y, 0); area.Max.Y = lib.SetMaxVal(area.Max.Y, Map.WorldPosition.ChunkHeight);
            area.Min.Z = Bound.Min(area.Min.Z, 0); area.Max.Z = lib.SetMaxVal(area.Max.Z, Map.WorldPosition.ChunkWidth);

            IntVector3 pos = IntVector3.Zero;
            for (pos.Z = area.Min.Z; pos.Z < area.Max.Z; pos.Z++)
            {
                for (pos.Y = area.Min.Y; pos.Y < area.Max.Y; pos.Y++)
                {
                    for (pos.X = area.Min.X; pos.X < area.Max.X; pos.X++)
                    {
                        DataGrid[pos.X, pos.Y, pos.Z] = material;
                    }
                }
            }
        }
        public void RemoveVoxels(WorldPosition startPos, WorldPosition endPos)
        {
            IntVector3 start = startPos.LocalBlockGrindex;
            if (startPos.ChunkGrindex.X < Index.X)
            {
                start.X = 0;
            }
            if (startPos.ChunkGrindex.Y < Index.Y)
            {
                start.Z = 0;
            }
            IntVector3 end = endPos.LocalBlockGrindex;
            if (endPos.ChunkGrindex.X > Index.X)
            {
                end.X = WorldPosition.ChunkWidth - 1;
            }
            if (endPos.ChunkGrindex.Y > Index.Y)
            {
                end.Z = WorldPosition.ChunkWidth - 1;
            }

            IntVector3 blockPos = IntVector3.Zero;
            for (blockPos.Y = 0; blockPos.Y < WorldPosition.ChunkHeight; blockPos.Y++)
            {
                for (blockPos.Z = start.Z; blockPos.Z <= end.Z; blockPos.Z++)
                {
                    for (blockPos.X = start.X; blockPos.X <= end.X; blockPos.X++)
                    {
                        DataGrid[blockPos.X, blockPos.Y, blockPos.Z] = 0;
                    }
                }
            }
        }

        public void AddToPreparedChunk(Facing8Dir dir, ref byte[, ,] preparedChunk)
        {
            int startX;
            int Xlength;

            int startZ;
            int Zlength;

            int xadd;
            int zadd;

            const int MinStartIx = 0;
            const int MaxStartIx = WorldPosition.ChunkWidth - 1;
            const int FollowSameIxAdd = 1;
            const int MinStartAdd = -WorldPosition.ChunkWidth + 1;
            const int MaxStartAdd = WorldPosition.ChunkWidth + 1;

            switch (dir)
            {
                default:
                    startX = MinStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MaxStartIx;
                    Zlength = WorldPosition.ChunkWidth;

                    xadd = FollowSameIxAdd;
                    zadd = MinStartAdd;
                    break;
                case Facing8Dir.SOUTH:
                    startX = MinStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MinStartIx;
                    Zlength = 1;

                    xadd = FollowSameIxAdd;
                    zadd = MaxStartAdd;
                    break;
                case Facing8Dir.WEST:
                    startX = MaxStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MinStartIx;
                    Zlength = WorldPosition.ChunkWidth;

                    xadd = MinStartAdd;
                    zadd = FollowSameIxAdd;
                    break;
                case Facing8Dir.EAST:
                    startX = MinStartIx;
                    Xlength = 1;
                    startZ = MinStartIx;
                    Zlength = WorldPosition.ChunkWidth;

                    xadd = MaxStartAdd;
                    zadd = FollowSameIxAdd;
                    break;

                case Facing8Dir.NW:
                    startX = MaxStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MaxStartIx;
                    Zlength = WorldPosition.ChunkWidth;

                    xadd = MinStartAdd;
                    zadd = MinStartAdd;
                    break;
                case Facing8Dir.NE:
                    startX = MaxStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MinStartIx;
                    Zlength = 1;

                    xadd = MinStartAdd;
                    zadd = MaxStartAdd;
                    break;
                case Facing8Dir.SW:
                    startX = MaxStartIx;
                    Xlength = WorldPosition.ChunkWidth;
                    startZ = MinStartIx;
                    Zlength = 1;

                    xadd = MinStartAdd;
                    zadd = MaxStartAdd;
                    break;
                case Facing8Dir.SE:
                    startX = MinStartIx;
                    Xlength = 1;
                    startZ = MinStartIx;
                    Zlength = 1;

                    xadd = MaxStartAdd;
                    zadd = MaxStartAdd;
                    break;

            }

            IntVector3 pos = IntVector3.Zero;
            IntVector3 preChunkPos = IntVector3.Zero;
            for (pos.Y = 0; pos.Y < WorldPosition.ChunkHeight; pos.Y++)
            {
                preChunkPos.Y = pos.Y + 1;
                for (pos.Z = startZ; pos.Z < Zlength; pos.Z++)
                {
                    preChunkPos.Z = pos.Z + zadd;
                    for (pos.X = startX; pos.X < Xlength; pos.X++)
                    {
                        preChunkPos.X = pos.X + xadd;

                        preparedChunk[preChunkPos.X, preChunkPos.Y, preChunkPos.Z] =
                            DataGrid[pos.X, pos.Y, pos.Z];
                    }
                }
            }
        }

        public List<Voxels.Voxel> CollectVoxels(WorldPosition startPos, WorldPosition endPos, IntVector3 addPos)
        {
            List<Voxels.Voxel> result = new List<Voxels.Voxel>();

            IntVector3 start = startPos.LocalBlockGrindex;
            if (startPos.ChunkGrindex.X < Index.X)
            {
                start.X = 0;
            }
            if (startPos.ChunkGrindex.Y < Index.Y)
            {
                start.Z = 0;
            }
            IntVector3 end = endPos.LocalBlockGrindex;
            if (endPos.ChunkGrindex.X > Index.X)
            {
                end.X = WorldPosition.ChunkWidth -1;
            }
            if (endPos.ChunkGrindex.Y > Index.Y)
            {
                end.Z = WorldPosition.ChunkWidth - 1;
            }

            IntVector3 blockPos = IntVector3.Zero;
            for (blockPos.Y = 0; blockPos.Y < WorldPosition.ChunkHeight; blockPos.Y++)
            {
                for (blockPos.Z = start.Z; blockPos.Z <= end.Z; blockPos.Z++)
                {
                    for (blockPos.X = start.X; blockPos.X <= end.X; blockPos.X++)
                    {
                        if (DataGrid[blockPos.X, blockPos.Y, blockPos.Z] != 0)
                        {
                            IntVector3 pos = blockPos;
                            pos.Add(addPos);
                            result.Add(new Voxels.Voxel(pos, DataGrid[blockPos.X, blockPos.Y, blockPos.Z]));
                        }
                    }
                }
            }

            return result;
        }


        
        //public void InLoadingUpdate()
        //{ loading = true; }
        
        static readonly bool[,] BasicAllowedSpawnBuild = new bool[,]
        { 
            {  true, true, true },
            {  true, true, true },
            {  true, true, true },
        };
        static readonly bool[,] VillageSpawnBuild = new bool[,]
        { 
            {  false, true, false },
            {  true, false, true },
            {  false, true, false },
        };
        static readonly bool[,] CastleSpawnBuild = new bool[,]
        { 
            {  false, false, false },
            {  false, true, false },
            {  false, false, true },
        };

        

        /// <summary>
        /// For placement of NPCs
        /// </summary>
        public float GetGroundY(WorldPosition pos)
        {
            const float AdjustResult = 0.5f;

            IntVector3 localPos = pos.LocalBlockGrindex;
            localPos.Y = Bound.Set(localPos.Y, 0, Map.WorldPosition.MaxChunkY);
            if (localPos.Y < 0) localPos.Y = 0;
            

            //check if inside block 
            bool insideBlock = DataGrid[localPos.X, localPos.Y, localPos.Z] != 0 || 
                DataGrid[localPos.X, lib.SetMaxVal(localPos.Y + 1,  WorldPosition.MaxChunkY), localPos.Z] != 0;

            if (insideBlock)
            { //check upward
                for (; localPos.Y < WorldPosition.ChunkHeight; ++localPos.Y)
                {
                    if (DataGrid[localPos.X, localPos.Y, localPos.Z] == 0 && 
                        DataGrid[localPos.X, lib.SetMaxVal(localPos.Y + 1, WorldPosition.MaxChunkY), localPos.Z] == 0)//make sure its a two block gap
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
                    if (DataGrid[localPos.X, localPos.Y, localPos.Z] != 0)
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
            int endY = lib.SmallestOfTwoValues(localPos.Y + spaceNeeded, WorldPosition.ChunkHeight);

            for (; localPos.Y < endY; ++localPos.Y)
            {
                if (DataGrid[localPos.X, localPos.Y, localPos.Z] != 0)
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
                if (DataGrid[localPos.X, y, localPos.Z] != 0)
                {
                    return y;
                }
            }
            return 0;
        }
        public Voxels.Voxel GetFirstBlockBelow(WorldPosition pos)
        {
            throw new Exception("Behöver uppdateras"); //används bara av flying toy
            IntVector3 localPos = pos.LocalBlockGrindex;
            for (pos.Y =lib.SetMaxVal(pos.Y, WorldPosition.MaxChunkY); pos.Y >= 0; pos.Y--)
            {
                if (DataGrid[localPos.X, pos.Y, localPos.Z] != 0)
                {
                    return new Voxels.Voxel(pos.LocalBlockGrindex, DataGrid[pos.LocalBlockX, pos.LocalBlockY, pos.LocalBlockZ]);
                }
            }
           
            return new Voxels.Voxel(pos.LocalBlockGrindex, 0);
        }
       

        /// <returns>if you will collide to this square</returns>
        public bool Check2DCollision(WorldPosition wp)
        {
            if (DataGrid == null)
            { 
                return false;
            }

            return (DataGrid[wp.LocalBlockX, WorldPosition.PassableGroundY, wp.LocalBlockZ] == WorldPosition.EmptyBlock) ||
                (DataGrid[wp.LocalBlockX, WorldPosition.PassableGroundY + 1, wp.LocalBlockZ] != WorldPosition.EmptyBlock);
        }


        public void ReplaceFirstGroundBlock(IntVector2 pos, byte toMaterial)
        {
            int startY = WorldPosition.ChunkStandardHeight - 1;
            for (int y = startY; y < WorldPosition.ChunkHeight; y++)
            {

                if (DataGrid[pos.X, y + 1, pos.Y] == byte.MinValue)
                {
                    DataGrid[pos.X, y, pos.Y] = (byte)toMaterial;
                    return;
                }
            }
        }
     

        bool unsavedChanges = false;
        public bool UnSavedChanges
        { 
            get { return unsavedChanges; }
        }
        public void ChangedData()
        {
            unsavedChanges = true;
            LfRef.worldOverView.ChangedChunk(Index);
            
        }

        void beginDeleteMembers()
        {
            if (chunkObjects != null)
            {
                new Timer.Action0ArgTrigger(deleteMembers);
            }
        }
        void deleteMembers()
        {
            if (chunkObjects != null)
            {
                foreach (GameObjects.EnvironmentObj.MapChunkObject obj in chunkObjects)
                {
                    obj.ChunkDeleteEvent();
                }
            }
        }


        public void CompleteRemoval()
        {
            DataStream.DataStreamHandler.BeginUserRemoveFile(Path);
        }
        public const string SaveFileEnding = ".chk";
        public const string VisitedFolder = "Visited";

        public static bool GotSaveFile(IntVector2 index)
        {
            return DataLib.SaveLoad.FileExistInStorageDir(Data.WorldsSummaryColl.CurrentWorld.FolderPath, lib.IntVec2Text(index));
        }
        public static DataStream.FilePath SavePath(IntVector2 index)
        {
            return new DataStream.FilePath(Data.WorldsSummaryColl.CurrentWorld.FolderPath, lib.IntVec2Text(index), SaveFileEnding, true, NumBackupVersions, true);
        }

        public const int NumBackupVersions = 3;
        public DataStream.FilePath Path
        {
            get
            {
                return SavePath(Index);
            }
        }


        public static readonly long HoursToTicks = TimeSpan.FromHours(0.5).Ticks;
        public static string BackUpTimeFolder()
        {
           return (DateTime.Now.Ticks / HoursToTicks).ToString();
        }
       
        public static bool ChunkSaveExsist(IntVector2 index)
        {
            return DataLib.SaveLoad.FileExistInStorageDir(Data.WorldsSummaryColl.CurrentWorld.FolderPath, lib.IntVec2Text(index));
        }

        public void SaveData(bool save, DataLib.ISaveTostorageCallback callBack, bool saveBackup)
        {
            if ((!save || Map.World.RunningAsHost) && !WriteProtected && SaveType == ChunkSaveType.Save)
            {
                new ScreenSaveQuer(save, this, callBack, false);
            }
            else if (SaveType == ChunkSaveType.PrivateStorage)
            {//if this unit is the owner, it should be stored with it
                LfRef.worldOverView.SaveChunkToOwner(save, callBack, this);
            }
        }

        /// <returns>Has stored data</returns>
        bool loadPrivateArea(Players.Player owner)
        {
            IntVector2 pos = LfRef.worldOverView.GetArea(Index).ToLocalPos(Index);
            if (owner.Settings.PrivateAreaData[pos.X, pos.Y] != null && owner.Settings.PrivateAreaData[pos.X, pos.Y].HasData)
            {
                this.BeginReadChunk(owner.Settings.PrivateAreaData[pos.X, pos.Y].GetReader());
                return true;
            }
            return false;
        }

        public void DebugCorruptChunk()
        {
            createEmptyGrid();//DataGrid = new byte[WorldPosition.ChunkWidth, WorldPosition.ChunkHeight, WorldPosition.ChunkWidth];
            new DataStream.CorruptFile(Path, true);
            MeshNeedsReload = true;
        }

       


        void writeChunkObjects(System.IO.BinaryWriter w)
        {
            //debugPrintStreamPos(w, "writeChunkObjects_s");
            if (chunkObjects != null)
            {
                foreach (GameObjects.EnvironmentObj.MapChunkObject obj in chunkObjects)
                {
                    if (obj.NeedToBeStored)
                    {
                        //1
                        w.Write(true);
                        //2
                        w.Write((byte)obj.MapChunkObjectType);
                        //3
                        obj.WriteStream(w);
                    }
                }
            }
            w.Write(false);
            //17
           // debugPrintStreamPos(w, "writeChunkObjects_e");
        }

        
        void readChunkObjects(System.IO.BinaryReader r, byte version)
        {
            //är två bytes kort
            //debugPrintStreamPos(r, "readChunkObjects_s");
            clearChunkObjects();
            if (version >= 93)
            {
                while (r.ReadBoolean())
                {
                    //2
                    readObject(r, version);
                }
                #region OLD
            }
            else
            { //OLD
                //w.Write(byte.MinValue);
                int numChunkObjects = r.ReadByte();
                for (int i = 0; i < numChunkObjects; i++)
                {

                    byte control = r.ReadByte();
                    if (control != ControllValue)
                    {
                        Debug.LogError( "Chunk has shifted read order");
                    }
                    readObject(r, version);
                }
            }

            //debugPrintStreamPos(r, "readChunkObjects_e");
                #endregion
        }

        private void clearChunkObjects()
        {
            if (chunkObjects != null)
            {
                Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, "Screen already contains chunkObjects, " + this.ToString());

                foreach (GameObjects.EnvironmentObj.MapChunkObject obj in chunkObjects)
                {
                    obj.ChunkDeleteEvent();
                }
            }
            chunkObjects = null;
        }

        void readObject(System.IO.BinaryReader r, byte version)
        {
            GameObjects.EnvironmentObj.MapChunkObjectType type = (GameObjects.EnvironmentObj.MapChunkObjectType)r.ReadByte();
            //3
            GameObjects.EnvironmentObj.MapChunkObject obj = null;
            switch (type)
            {
                default:
                    Debug.LogError( "Trying to load NPC, " + this.ToString());
                    //obj = 
                       LF2.Data.Characters.NPCdata.LoadNPC(type, r, version,  Index, false); //read only to be backward comp
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Door:
                    obj = new GameObjects.EnvironmentObj.Door(r, version, Index, false);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.CritterSpawn:
                    obj = new GameObjects.EnvironmentObj.CritterSpawn(r, version, Index, false);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.Chest:
                    System.Diagnostics.Debug.WriteLine("CHUNK " + Index.ToString() + ", read Chest");

                    obj = new GameObjects.EnvironmentObj.ChestData(null, Index, r, version, GameObjects.EnvironmentObj.MapChunkObjectType.Chest, LfRef.gamestate.LocalHostingPlayer);
                    break;
                case GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile:
                    obj = new GameObjects.EnvironmentObj.ChestData(null, Index, r, version, GameObjects.EnvironmentObj.MapChunkObjectType.DiscardPile, LfRef.gamestate.LocalHostingPlayer);//obj = new GameObjects.EnvironmentObj.DiscardPile(this, r, version).Data;
                    break;
            }

            //if (obj != null)
            //    AddChunkObject(obj, true);
        }

        const byte ChunkDataVersion = 93;
        const byte ControllValue = byte.MaxValue - 1;

        public void WriteChunk(System.IO.BinaryWriter w)
        {
            w.Write(ChunkDataVersion);
            writeChunkObjects(w);
            writeDataGrid(w);
        }

        public void ReadChunk(System.IO.BinaryReader r)
        {
            Debug.DebugLib.CrashIfThreaded();//i framtiden kanske dela upp på main för gameobjects och resten på tråd

            createEmptyGrid();//DataGrid = new byte[WorldPosition.ChunkWidth, WorldPosition.ChunkHeight, WorldPosition.ChunkWidth];

            byte version = r.ReadByte();
#region OLD
            if (version < 90)
            {
                r.BaseStream.Position = 0;
            }
            else
#endregion
            {
                readChunkObjects(r, version);
            }
            readDataGrid(r, version);
            
            if (openstatus < ScreenOpenStatus.DataGridComplete)
                openstatus = ScreenOpenStatus.DataGridComplete;

            completedDataOrigin = ChunkDataOrigin.Loaded;
            //preparedDataOrigin = ChunkDataOrigin.NON;
        }

        public void BeginReadChunk(System.IO.BinaryReader r)
        {
            new DataLib.SafeMainThreadReader(r, this.ReadChunk);
        }

       
        void checkMultiFatherBug()
        {
            int amountFathers = 0;
            foreach (GameObjects.EnvironmentObj.MapChunkObject obj in chunkObjects)
            {
                if (obj.MapChunkObjectType == GameObjects.EnvironmentObj.MapChunkObjectType.Father)
                    amountFathers++;
            }
            if (amountFathers > 1)
            {
                throw new Exception("Saving multi fathers");
            }
        }


        public static void NetworkSaveChunk(System.IO.BinaryReader r, IntVector2 pos)
        {
            byte[] data = r.ReadBytes(r.Length);
            new DataStream.WriteByteArray( SavePath(pos), data, null);
        }

        void writeDataGrid(System.IO.BinaryWriter w)
        {
            if (DataGrid != null)
            {
                List<IntVector2> materialAndReps = new List<IntVector2>();
                byte[] usedMaterials = new byte[PublicConstants.ByteSize];

                Voxels.VoxelLib.GridToMaterialAndReps(DataGrid, materialAndReps, usedMaterials);

                List<byte> usedMaterialsList = Voxels.VoxelLib.GetUsedMaterialsList(usedMaterials);
                Voxels.VoxelLib.WriteUsedMaterials(w, usedMaterialsList);
                Voxels.VoxelLib.WriteMaterialAndReps(w, materialAndReps, usedMaterials, usedMaterialsList);
            }
        }
        void readDataGrid(System.IO.BinaryReader r, byte version)
        {
            if (version >= 92)
            {
                byte[] usedMaterials = Voxels.VoxelLib.ReadUsedMaterials(r);
                DataGrid = Voxels.VoxelLib.ReadMaterialAndReps(r, Map.WorldPosition.ChunkSize, usedMaterials);
            }
            else
            {
                //Step through all the blocks
                WorldPosition wp = new WorldPosition();
                while (r.BaseStream.Position < r.BaseStream.Length)
                {
                    byte material = r.ReadByte();
                    byte rep = r.ReadByte();
                    if (material == byte.MinValue)
                    {
                        wp.NextBlock(rep);
                    }
                    else
                    {
                        for (byte i = 0; i < rep; i++)
                        {
                            SetUnsafe(wp, material);
                            wp.NextBlock();
                        }
                    }
                }
            }

            
            //new Timer.ActionEventTrigger(meshCompleteEvent);
        }
        void compressChunkData(List<byte> compressedDataArray, bool compress)
        {
            if (compress)
            {
                if (DataGrid != null)
                {
                    byte lastVal = byte.MaxValue - 1;
                    byte numRepetitions = 0;

                    IntVector3 gridPos = IntVector3.Zero;
                    for (gridPos.Y = 0; gridPos.Y < WorldPosition.ChunkHeight; gridPos.Y++)
                    {
                        for (gridPos.Z = 0; gridPos.Z < WorldPosition.ChunkWidth; gridPos.Z++)
                        {
                            for (gridPos.X = 0; gridPos.X < WorldPosition.ChunkWidth; gridPos.X++)
                            {
                                byte value = DataGrid[gridPos.X, gridPos.Y, gridPos.Z];
                                if (value == lastVal)
                                {
                                    numRepetitions++;
                                    if (numRepetitions == byte.MaxValue)
                                    {
                                        compressedDataArray.Add(lastVal);
                                        compressedDataArray.Add(numRepetitions);
                                        numRepetitions = 0;
                                    }
                                }
                                else
                                {
                                    compressedDataArray.Add(lastVal);
                                    compressedDataArray.Add(numRepetitions);
                                    lastVal = value;
                                    numRepetitions = 1;
                                }
                            }
                        }
                    }//end forY
                    compressedDataArray.Add(lastVal);
                    compressedDataArray.Add(numRepetitions);
                }
            }
            else
            {
                //Step through all the blocks
                WorldPosition wp = new WorldPosition();
                //bool isRepetitions
                for (int arrayPos = 0; arrayPos < (compressedDataArray.Count - 1); arrayPos += 2)
                {
                    byte material = compressedDataArray[arrayPos];
                    for (byte i = 0; i < compressedDataArray[arrayPos + 1]; i++)
                    {
                        Set(wp, material);
                        wp.NextBlock();

                    }
                }
            }
        }

        

       

        public bool WaintingForNetworkData
        {
            get
            {
                //if (Ref.netSession.IsClient)
                //{
                    return preparedDataOrigin == ChunkDataOrigin.RecievedByNet && 
                        completedDataOrigin != ChunkDataOrigin.RecievedByNet;
                //}
                //return false;
            }
        }

        public void NetworkReciveChunk(System.IO.BinaryReader r)
        {
            System.Diagnostics.Debug.WriteLine(">Receives chunk from host " + Index.ToString()); 
            try
            {
                
                ReadChunk(r);
                completedDataOrigin = ChunkDataOrigin.RecievedByNet;
                MeshNeedsReload = true;
                Map.World.hasWaitingReloads = true;
            }
            catch (IOException e)
            {
                Debug.LogError("NetworkReciveChunk, " + e.Message);
            }
        }


        public void GeneratePart(ScreenOpenStatus openStatus, bool preGen)
        {
            switch (openstatus)
            {
                case ScreenOpenStatus.Closed:
                    generate1_Topographic();
                    break;
                case ScreenOpenStatus.GotTopographic:
                    generate2_HeightMap();
                    break;
                case ScreenOpenStatus.HeightMapToDataGrid:
                    //if (preGen)
                    //    generate3B_Area();
                    //else
                    generate3_Detail();
                    break;
            }
    
        }

        public void generate4_Mesh(Graphics.LFHeightMap heightMapMesh, 
            Terrain.ScreenMeshBuilder7 meshBuilder)
        {
            if (openstatus >= ScreenOpenStatus.DataGridComplete)
            {
                if (openstatus < ScreenOpenStatus.MeshGeneratedDone)
                    openstatus = ScreenOpenStatus.MeshGeneratedDone;
               
                MeshNeedsReload = false;
                WorldPosition wp = WorldPosition.EmptyPos;
                wp.ChunkGrindex = Index;
                
                Graphics.VertexAndIndexBuffer newVB =
                    meshBuilder.BuildScreen(ref DataGrid, wp, heightMapMesh, vbPointer != null);

                System.Threading.Thread.Sleep(Ref.main.TargetElapsedTime.Milliseconds);

                if (vbPointer != null)
                {
                    new AddVBTimer(heightMapMesh, vbPointer, Index, false, false);
                }
                vbPointer = newVB;
                //completedDataOrigin = preparedDataOrigin;
                beginGenerate5_GameObjects();
            }
        }

        public void RemoveMesh(Graphics.LFHeightMap heightMapMesh)
        {
            if (vbPointer != null)
            {
                openstatus = ScreenOpenStatus.DataGridComplete;
                new AddVBTimer(heightMapMesh, vbPointer, Index, false, false);
                vbPointer = null;

                //beginRemoveConnectedObjects();
            }
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
                Debug.DebugLib.Print(Debug.PrintCathegoryType.Output, text);
            }
        }

        public void CloseAndSave()
        {
            if (openstatus > ScreenOpenStatus.Closed)
            {
#if WINDOWS
                if (Index == new IntVector2(101, 84))
                {
                    lib.DoNothing();
                }
                if (ClientEditingFlag)
                {
                    LfRef.gamestate.LocalHostingPlayerPrint("Client flagged chunk removed");
                }
#endif
                if (unsavedChanges && DataGrid != null)
                {
                    LfRef.worldOverView.ChangedChunk(Index);
                    //save changes
                    if (LfRef.gamestate.SaveMap)
                        SaveData(true, null, true);

                    if (Ref.netSession.IsClient)
                        new Process.NetSendOpenChunk(this, Network.PacketReliability.ReliableLasy, true);
                }
                //deleteMembers();
                beginDeleteMembers();
                beginDegenerateGameObjects();
                openstatus = ScreenOpenStatus.Closed;
            }
        }

        public void AddBoss(int level)
        {
            Vector3 startPos = Vector3.Zero;
            startPos.X = (float)((Index.X + 1) * WorldPosition.ChunkWidth);
            startPos.Z = (float)((Index.Y + 1) * WorldPosition.ChunkWidth);
        }
        public bool EmptyScreenCheck()
        {
            return DataGrid[5,1, 5] == 0;
        }

        bool emptyAreaData
        {
            get
            {
                return chunkData.AreaType == Terrain.AreaType.Empty ||  
                    chunkData.AreaType == Terrain.AreaType.FlatEmptyAndMonsterFree ||
                    chunkData.AreaType == Terrain.AreaType.NUM_NON;
            }
        }

        /// <summary>
        /// Add smaller objects like trees and stones to the ground
        /// </summary>
       public void generate3_Detail()
       {
           
           if (openstatus == ScreenOpenStatus.HeightMapToDataGrid)//temp
           {
                

               if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1 && !Map.World.RunningAsHost && !LfRef.gamestate.HasRecievedWorldOverview)
               {
                   throw new Exception("generating chunk before gotten world overview");
               }


               if (generateTerrain)
               {
                   //check area type
                   if (emptyAreaData)
                   { //for normal areas just randomize objects like trees and stones
                       if (chunkData.AreaType != Terrain.AreaType.FlatEmptyAndMonsterFree)
                       {
                           List<ByteVector2> dots = Terrain.HandmadeTemplates.DotTemplates[topographData.DotMap];
                           Terrain.ObstacleBuilder.Build(Index, DataGrid, dots, chunkData.Environment);
                       }
                   }
                   else
                   {//urban area
                       //1. get link to the urban place
                       LfRef.worldOverView.GetAreaObject(chunkData).BuildOnChunk(this, preparedDataOrigin == Map.ChunkDataOrigin.Generated, false);
                   }

               }
               openstatus = ScreenOpenStatus.DataGridComplete;

               //new Timer.ActionEventTrigger(meshCompleteEvent);
           }
       }

       int chunkIsRequested = 0;
       public void ReRequest()
       {
           if (Ref.netSession.IsClient)
           {
               chunkIsRequested = 0;
               RequestChunkData();
           }
       }

       public void RequestChunkData()
       {
           Players.AbsPlayer owner = LfRef.worldOverView.ChunkHasOwner(Index);
           if (owner != null)
           {
               RequestChunkData(owner.StaticNetworkId);
           }
           else
           {
               RequestChunkData(Ref.netSession.HostStaticId);
           }
       }

       public void RequestChunkData(byte chunkOwner)
       {
           preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
           if (chunkIsRequested == 0)
           {


               System.IO.BinaryWriter writer = Ref.netSession.BeginAsynchPacket();
               Map.WorldPosition.WriteChunkGrindex_Static(Index, writer);

               System.Diagnostics.Debug.WriteLine("<Requesting chunk from host " + Index.ToString()); 

               Ref.netSession.EndAsynchPacket(writer,
                   Network.PacketType.LF2_RequestChunk,
                    Network.SendPacketTo.OneSpecific, chunkOwner,
                   Network.PacketReliability.ReliableLasy, null);
           }

           if (chunkIsRequested++ > 600)
           {
               chunkIsRequested = 0;
           }
       }
       public void RequestChunkDataUpdate()
       {
           //when a chunk is undoed
           chunkIsRequested++;
           System.IO.BinaryWriter writer = Ref.netSession.BeginWritingPacketToHost(Network.PacketType.LF2_RequestChunkGroup, 
               Network.PacketReliability.ReliableLasy, LootfestLib.LocalHostIx);
           //Index.WriteStream(writer);
           Map.WorldPosition.WriteChunkGrindex_Static(Index, writer);
       }

       void createByteGrid()
       {

       }

       static readonly Terrain.EnvironmentMaterials[] EnvironmentMaterials = new Terrain.EnvironmentMaterials[]
        {
            // Grassfield,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.grass, (byte)Data.MaterialType.cobble_stone, (byte)Data.MaterialType.dirt, (byte)Data.MaterialType.sand),
            //Forest,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.forest, (byte)Data.MaterialType.mossy_stone, (byte)Data.MaterialType.dirt, (byte)Data.MaterialType.sand),
            //Swamp,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.dirt, (byte)Data.MaterialType.forest, (byte)Data.MaterialType.water, (byte)Data.MaterialType.sand),
            //Desert,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.desert_sand, (byte)Data.MaterialType.sand_stone, (byte)Data.MaterialType.sand, (byte)Data.MaterialType.sand),
            //Burned,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.burnt_ground, (byte)Data.MaterialType.dark_gray, (byte)Data.MaterialType.lava, (byte)Data.MaterialType.burnt_ground),
            //Mountains,
            new Terrain.EnvironmentMaterials((byte)Data.MaterialType.grass, (byte)Data.MaterialType.cobble_stone, (byte)Data.MaterialType.mossy_stone, (byte)Data.MaterialType.cobble_stone),
        };


        /// <summary>
        /// Check if the chunk should be loaded or requestet over net, and starts the process
        /// </summary>
        /// <returns>Should keep generating the terrain</returns>
       bool checkDataLoadOrRequest()
       {
           if (chunkData.changed && SaveType == ChunkSaveType.Save)
           { //load chunk from storage

               return false;
           }
           if (SaveType == ChunkSaveType.PrivateStorage)
           { 
               Players.AbsPlayer owner = LfRef.worldOverView.ChunkHasOwner(Index);
               if (owner != null)
               { //Request chunk from owner
                   if (owner.Local)
                   {

                   }
                   else
                   { //Send network request

                   }
               }
           }
           return true;
       }


       //bool loadingOrRequestingData = false;
       bool generateTerrain = true;
       /// <summary>
       /// Checks if the chunk data is changed before generating it
       /// </summary>
       /// <returns>Should keep generating the chunk</returns>
       public void LoadOrRequestData()
       {
           generateTerrain = true;

           if (chunkData.changed && SaveType == ChunkSaveType.Save)
           { //Chunk has stored changes
               if (Map.World.RunningAsHost)
               {//locally stored, just load it
                   SaveData(false, this, true);
                   generateTerrain = false;
                   preparedDataOrigin = ChunkDataOrigin.Loaded;
               }
               else
               {//request from host
                   RequestChunkData(Ref.netSession.HostStaticId);
                   //preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
               }
           }
           else if (SaveType == ChunkSaveType.PrivateStorage)
           { //Chunk is hosted by one player
               Players.AbsPlayer owner = LfRef.worldOverView.ChunkHasOwner(Index);
               if (owner != null)
               {
                   if (owner.Local)
                   {//Pick from local player
                       bool hasData = loadPrivateArea((Players.Player)owner);
                       generateTerrain = !hasData;

                       if (hasData)
                           preparedDataOrigin = ChunkDataOrigin.Loaded;
                       else
                           preparedDataOrigin = ChunkDataOrigin.Generated;
                   }
                   else
                   {//Request over net
                       RequestChunkData(owner.StaticNetworkId); //måste se till att man frågar rätt host
                       //preparedDataOrigin = ChunkDataOrigin.RecievedByNet;
                   }
               }
               else
               {
                   preparedDataOrigin = ChunkDataOrigin.Generated;
               }
           }
           else
           {
               preparedDataOrigin = ChunkDataOrigin.Generated;
           }

           if (!generateTerrain)
           {
               if (openstatus >= ScreenOpenStatus.LoadingOrRecievingData)
                   throw new Exception();
               openstatus = ScreenOpenStatus.LoadingOrRecievingData;
           }

           if (PlatformSettings.ViewErrorWarnings && preparedDataOrigin == ChunkDataOrigin.NON)
               throw new Exception();
       }


       static List<Terrain.ScreenTopographicData> topographs = new List<Terrain.ScreenTopographicData>();
       static List<IntVector2> topgraphStartPos = new List<IntVector2>();

       public void generate2_HeightMap()
        {//PART OF OPEN STATUS
            if (openstatus < ScreenOpenStatus.HeightMapToDataGrid)//temp
            {
                LoadOrRequestData();
                if (generateTerrain)
                {
                    createByteGrid();
                    completedDataOrigin = ChunkDataOrigin.Generated;
                    
                    
                    generate1_Topographic();

                    //1. start with a 2d grid of the height
                    //2. add height templates (select highest)
                    //3. add detail map

                    //ver2
                    byte addMaterial;
                    topographs.Clear();
                    topgraphStartPos.Clear();
                    //List<Terrain.ScreenTopographicData> topographs = new List<Terrain.ScreenTopographicData>();
                    //List<IntVector2> topgraphStartPos = new List<IntVector2>();

                    //center
                    IntVector2 cstart;
                    int myEnvironmentType = (int)chunkData.Environment;
                    if (topographData.Height > 0)
                    {
                        topographs.Add(topographData);
                        cstart = Terrain.HeightTemplate.ScreenCenterTemplateStart;
                        cstart.Add(topographData.centerDislocation);
                        topgraphStartPos.Add(cstart);
                    }
                    //sourrounding screens
                    IntVector2 npos = IntVector2.Zero;

                    foreach (IntVector2 dir in SourroundingSides)
                    {
                        npos = Index;
                        npos += dir;

                        Chunk neighbor = LfRef.chunks.GetScreen(npos);
                        Terrain.ScreenTopographicData tg = neighbor.TopographData;
                            
                        if (tg != null)
                        {
                            cstart = Terrain.HeightTemplate.ScreenCenterTemplateStart;
                            cstart.Add(tg.centerDislocation);
                            cstart.X += WorldPosition.ChunkWidth * dir.X;
                            cstart.Y += WorldPosition.ChunkWidth * dir.Y;

                            topographs.Add(tg);
                            topgraphStartPos.Add(cstart);
                        }
                    }
                    IntVector3 pos = IntVector3.Zero;
                    

                    //add detailMap
                    byte[,] detail = Terrain.HandmadeTemplates.DetailTemplates[topographData.detailMap];
                    for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; ++pos.Z)
                    {
                        for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; ++pos.X)
                        {
                            //go through the topographs to find out the height
                            int height = 0;
                            int environmentType = Ref.rnd.RandomChance(60) ? myEnvironmentType : topographs[Ref.rnd.Int(topographs.Count)].TerrainType;

                            for (int graphIx = 0; graphIx < topographs.Count; graphIx++)
                            {
                                //does it affect the position
                                IntVector2 start = topgraphStartPos[graphIx];
                                if (start.X <= pos.X && pos.X < (start.X + Terrain.HeightTemplate.Width) &&
                                    start.Y <= pos.Z && pos.Z < (start.Y + Terrain.HeightTemplate.Width))
                                {
                                   //pick the height of the topgraph and see if it is the largest
                                    if (topographs[graphIx].heightMap[pos.X - start.X, pos.Z - start.Y] > 0)
                                    {
                                        int h = topographs[graphIx].heightMap[pos.X - start.X, pos.Z - start.Y] * topographs[graphIx].Height;
                                        
                                        if (h > height)
                                        {
                                            height = h;
                                            if (Ref.rnd.RandomChance(70))
                                                environmentType = topographs[graphIx].TerrainType;
                                        }
                                        else if (height == h && topographs[graphIx].TerrainType > environmentType && Ref.rnd.RandomChance(40))
                                        {
                                            environmentType = topographs[graphIx].TerrainType;
                                        }
                                    }
                                }
                            }



                            //fill the data grid
                            const int BasicGroundHeight = Map.WorldPosition.ChunkStandardHeight;
                            const int LowLevelMaterialHeight = BasicGroundHeight - 3; //1
                            const int HighLevelMaterialHeight = BasicGroundHeight + 2; //6
                            //topograph with added detail
                            height += BasicGroundHeight + detail[pos.X, pos.Z] * topographData.DetailHeight;
                            int DirtHeight = height - 1;

                            Terrain.EnvironmentMaterials m = EnvironmentMaterials[environmentType];

                            for (pos.Y = 0; pos.Y < height; ++pos.Y)
                            {
#if WINDOWS
                                //if (pos.X == 0 && pos.Z == 0)
                                //{
                                //    addMaterial = (byte)Data.MaterialType.white;
                                //}
                                //else
                                //{
#endif
                                    if (pos.Y < LowLevelMaterialHeight + Terrain.HandmadeTemplates.MaterialHeightChange1[pos.X, pos.Z])
                                    {
                                        addMaterial = m.lowGroundMaterial;
                                    }
                                    else if (pos.Y > HighLevelMaterialHeight + Terrain.HandmadeTemplates.MaterialHeightChange2[pos.X, pos.Z])
                                    {
                                        addMaterial = m.highGroundMaterial;
                                    }
                                    else
                                    {
                                        addMaterial = pos.Y < DirtHeight ? m.belowSurfaceMaterial : m.mainMaterial;
                                    }
#if WINDOWS
                                //}
#endif
                                DataGrid[pos.X, pos.Y, pos.Z] = addMaterial;
                            }
                        }
                    }
                    if (openstatus < ScreenOpenStatus.HeightMapToDataGrid)
                        openstatus = ScreenOpenStatus.HeightMapToDataGrid;
                }
                
            }//end check openstatus
        }

        /// <summary>
        /// Sets a floor block at a certain height, removing everything above
        /// </summary>
        /// <param name="localPos">The block position in the chunk</param>
        /// <param name="floorMaterial">The material type of the floor</param>
        public void CreateFlatFloor(IntVector3 localPos, Data.MaterialType floorMaterial)
        {
            DataGrid[localPos.X, localPos.Y, localPos.Z] = (byte)floorMaterial;
            int highest = GetHighestYpos(new WorldPosition(localPos));
            for (int y = ++localPos.Y; y <= highest; ++y)
            {
                DataGrid[localPos.X, y, localPos.Z] = byte.MinValue;
            }
        }

        public void RemoveOrganicMaterialAbove(IntVector3 localPos)
        {
            int highest = GetHighestYpos(new WorldPosition(localPos));
            if (localPos.Y >= highest)
                return;
            for (int y = ++localPos.Y; y <= highest; ++y)
            {
                byte material = DataGrid[localPos.X, y, localPos.Z];
                bool replace = false;
                for(int i = 0; i < EnvironmentMaterials.Length; ++i)
                {
                    if(EnvironmentMaterials[i].belowSurfaceMaterial == material)
                        replace = true;
                    else if(EnvironmentMaterials[i].highGroundMaterial == material)
                        replace = true;
                    else if(EnvironmentMaterials[i].lowGroundMaterial == material)
                        replace = true;
                    else if(EnvironmentMaterials[i].mainMaterial == material)
                        replace = true;
                }
                if(replace)
                    DataGrid[localPos.X, y, localPos.Z] = byte.MinValue;
            }
        }

        public void RemoveAllOrganicMaterialAbove(int y)
        {
            IntVector3 pos = IntVector3.Zero;
            pos.Y = y;

            for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; pos.Z++)
            {
                for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; pos.X++)
                {
                    RemoveOrganicMaterialAbove(pos);
                }
            }
        }

        /// <summary>
        /// Takes the material and fills downwards at pos until a cube is found that isn't empty
        /// </summary>
        /// <param name="pos">The block position in the chunk</param>
        /// <param name="fillMaterial">The material type to fill with</param>
        public void FillDownwards(IntVector3 pos, Data.MaterialType fillMaterial)
        {
            while (DataGrid[pos.X, pos.Y, pos.Z] == byte.MinValue && pos.Y > 0)
            {
                DataGrid[pos.X, pos.Y--, pos.Z] = (byte)fillMaterial;
            }
        }

        /// <summary>
        /// Fills the entire chunk with a flat floor
        /// </summary>
        /// <param name="floorMaterial">The material type of the floor</param>
        public void FillFlatFloor(Data.MaterialType floorMaterial)
        {
            IntVector3 pos = IntVector3.Zero;
            pos.Y = WorldPosition.ChunkStandardHeight;

            for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; pos.Z++)
            {
                for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; pos.X++)
                {
                    CreateFlatFloor(pos, floorMaterial);
                }
            }
        }

        public void FillLayer(int layerY, Data.MaterialType material)
        {
            IntVector3 pos = IntVector3.Zero;
            pos.Y = layerY;

            for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; pos.Z++)
            {
                for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; pos.X++)
                {
                    DataGrid[pos.X, pos.Y, pos.Z] = (byte)(material);
                }
            }
        }

        public byte Get(WorldPosition pos)
        {
            if (pos.CorrectYPos)
                return DataGrid[pos.WorldGrindex.X & Map.WorldPosition.ChunkBitsZero, pos.WorldGrindex.Y, pos.WorldGrindex.Z & Map.WorldPosition.ChunkBitsZero];
            return 0;
        }
        public void Set(WorldPosition pos, byte value) 
        {
            if (pos.CorrectYPos)
                DataGrid[pos.WorldGrindex.X & Map.WorldPosition.ChunkBitsZero, pos.WorldGrindex.Y, pos.WorldGrindex.Z & Map.WorldPosition.ChunkBitsZero] = value;
        }

        public void Set(int localX, int localY, int localZ, byte value)
        {
            DataGrid[localX, localY, localZ] = value;
        }

        public void Set(IntVector3 localPos, byte value)
        {
            DataGrid[localPos.X, localPos.Y, localPos.Z] = value;
        }

        public void SetUnsafe(WorldPosition pos, byte value)
        {
            DataGrid[pos.WorldGrindex.X & Map.WorldPosition.ChunkBitsZero, pos.WorldGrindex.Y, pos.WorldGrindex.Z & Map.WorldPosition.ChunkBitsZero] = value;
        }



        public override string ToString()
        {
            return "Chk" + Index.ToString() + ":" + openstatus.ToString() + ":" + completedDataOrigin.ToString() +
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
        public bool GotMesh { get { return vbPointer != null; } }

    }

    enum ScreenOpenStatus
    {
        Closed,
        GotTopographic,
        HeightMapToDataGrid,
        LoadingOrRecievingData,
        //DotMapDone,
        DataGridComplete,
        GeneratingMesh,
        MeshGeneratedDone,
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
