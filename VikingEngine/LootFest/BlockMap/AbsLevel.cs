using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;
using System.IO;
using VikingEngine.LootFest.Map.Terrain;
using VikingEngine.LootFest.BlockMap.Level;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap
{
    abstract class AbsLevel
    {
        protected static readonly VoxelModelName[] NormalDefaultModels = new VoxelModelName[]
        { 
            VoxelModelName.BirchTree1,
            VoxelModelName.BirchTree2,
            VoxelModelName.PineTree1,
            VoxelModelName.PineTree2,
            VoxelModelName.PinkTree, 
            VoxelModelName.Stone1,
            VoxelModelName.Stone2,
            VoxelModelName.Stone3,
        };
        protected static readonly VoxelModelName[] DefaultDungeonModels = new VoxelModelName[]
        { 
            VoxelModelName.DungeonTerrain1,
            VoxelModelName.DungeonTerrain2,
            VoxelModelName.DungeonTerrain3,
            VoxelModelName.DungeonTerrain4,
        };

        public static readonly IntVector2 ChunkSize = new IntVector2(64, 64);

        protected PcgRandom rnd;
        public int mySeed = 0;
        public VikingEngine.EngineSpace.Maths.SimplexNoise2D heightMap;
        public IntVector2 topLeftChunk = new IntVector2(100);
        public IntVector2 levelEntrance;
        public Grid2D<BlockMapSquare> squares;
        public Grid2D<LevelChunk> chunks;

        public int usesAreaIndex;
        public bool generated = false;
        public Rectangle2 worldArea;
        public List<AbsSpawnPoint> spawnPoints = new List<AbsSpawnPoint>();

        protected Dictionary<VoxelModelNameAndRotation, VikingEngine.Voxels.VoxelObjGridData> loadedModels = new Dictionary<VoxelModelNameAndRotation, Voxels.VoxelObjGridData>();


        static readonly HeightMapMaterials DebugMaterials = HeightMapMaterials.Test; // new HeightMapMaterials(Data.MaterialType.pure_violet, Data.MaterialType.pure_violet_magenta, 1, Data.MaterialType.pure_violet);

        public const byte OpenAreaLockId = 0;
        public const byte SecondAreaLockId = 1;
        public const byte ThirdAreaLockId = 2;
        public const byte FourthAreaLockId = 3;
        public const byte BossAreaLockId = 10;
        public List<byte> unlockedAreas = new List<byte> { OpenAreaLockId };
        
        public IntInLowBound keyCount;
        Dictionary<int, LockGroup> lockGroups = new Dictionary<int, LockGroup>();
        public CollectItem collect;

        protected byte currentTerrainIndex = 0;
        public AbsLevelTerrain[] terrain;
        public AreaDesignStorageCollection designAreas = new AreaDesignStorageCollection();
        

        public AbsLevel(LevelEnum type)
        {   
            this.type = type;
            mySeed = LfRef.levels2.WorldSeed.seed + (int)type;

            squares = new Grid2D<BlockMapSquare>(ChunkSize * BlockMapLib.SquaresPerChunkW);
            chunks = new Grid2D<LevelChunk>(ChunkSize);
        }

        public System.IO.BinaryWriter netWriteLevelState()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LevelStatus, Network.PacketReliability.Reliable);
            LfRef.levels2.writeLevel(this, w);

            w.Write((byte)unlockedAreas.Count);
            foreach (var id in unlockedAreas)
            {
                w.Write(id);
            }

            w.Write((byte)keyCount.Value);

            collect.write(w);

            return w;
        }

        public void netReadLevelState(System.IO.BinaryReader r)
        {
            unlockedAreas.Clear();
            int unlockedAreasCount = r.ReadByte();
            for (int i = 0; i < unlockedAreasCount; ++i)
            {
                unlockedAreas.Add(r.ReadByte());
            }

            keyCount.Value = r.ReadByte();

            var prevCollect = collect;
            collect.read(r);

            if (prevCollect.collectedCount != collect.collectedCount)
            {
                onCollectUpdated();
            }
            VikingEngine.LootFest.GO.EnvironmentObj.AreaLock.RefreshAllLevelLocks(this);
        }

        public void collectAdd(int add)
        {
            if (Ref.netSession.IsHostOrOffline)
            {
                collect.collectedCount += add;
                onCollectUpdated();
                netWriteLevelState();
            }
            else
            {
                var w = Ref.netSession.BeginWritingPacketToHost(Network.PacketType.RequestLevelCollectAdd, Network.PacketReliability.Reliable, null);
                LfRef.levels2.writeLevel(this, w);
                w.Write(add);
            }
        }

        void onCollectUpdated()
        {
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                if (LfRef.LocalHeroes[i].LevelEnum == this.LevelEnum)
                {
                    LfRef.LocalHeroes[i].player.refreshLevelCollectItem();
                }
            }
        }

        public void initialize(int usesAreaIndex, IntVector2 topLeftChunk)
        {
            this.usesAreaIndex = usesAreaIndex;
            this.topLeftChunk = topLeftChunk;

            worldArea = new Rectangle2(topLeftChunk, ChunkSize);

            beginGenerate();
        }

        protected void beginGenerate()
        {
            new Timer.AsynchActionTrigger(generateAndCallback_asynch, true);
        }

        void generateAndCallback_asynch()
        {
            heightMap = new EngineSpace.Maths.SimplexNoise2D(mySeed);
            var loadModels = loadTerrainModelsList();
            foreach (var m in loadModels)
            {
                LoadModel(m);
            }

            rnd = new PcgRandom(mySeed);
            generateMapAsynch();
            
            generated = true;
            new Timer.Action0ArgTrigger(onComplete);
        }

        VoxelModelName loading;
        void LoadModel(VoxelModelName model)
        {
            loading = model;
           
            FilePath filePath = new FilePath(LfLib.ModelsCategoryTerrain, model.ToString(),
                VikingEngine.Voxels.VoxelLib.VoxelObjByteArrayEnding, false, false);

            if (filePath.Storage && !DataStreamHandler.FileExists(filePath))
            {
                emptyGrid();
            }
            else
            {
                DataStream.BeginReadWrite.BinaryIO(false, filePath, null, read,
                    null, false);
            }
        }

        void read(BinaryReader r)
        {
            List<VikingEngine.Voxels.Voxel> joints;
            var grid = new VikingEngine.Voxels.VoxelObjGridData(VikingEngine.Voxels.VoxelLib.ReadVoxelObject(r, true, out joints));
            grid.special = joints;
            loadedModels.Add(loading, grid);
        }

        static readonly byte EmptyMaterial1 = (byte)Data.MaterialType.pink;
        static readonly byte EmptyMaterial2 = (byte)Data.MaterialType.light_violet;
        void emptyGrid()
        {
            var grid = new VikingEngine.Voxels.VoxelObjGridData(new IntVector3(1));
            
            ForXYZLoop loop = new ForXYZLoop(grid.Size);
            while (loop.Next())
            {
                grid.Set(loop.Position, lib.IsEven(loop.Position.X + loop.Position.Y + loop.Position.Z) ? EmptyMaterial1 : EmptyMaterial2);
            }
            loadedModels.Add(loading, grid);
        }

        void onComplete()
        {
            //generateSpawnsPoints();
            LfRef.levels2.levelGeneratingComplete(this);
        }

        //virtual protected void generateSpawnsPoints()
        //{
        //    foreach (var m in spawnPoints)
        //    {
        //        m.addToDirector();
        //    }
        //}


        protected MapSegmentPointer placeSegment(BlockMapSegment seg, IntVector2 sqPos, Dir4 fromEntrance, int fromEntranceIx, byte lockId)
        {
           IntVector2 offset =  seg.getEntranceSqPos(fromEntrance, fromEntranceIx);
           return placeSegment(seg, sqPos - offset, lockId);
        }
        protected MapSegmentPointer placeSegment(BlockMapSegment seg, IntVector2 sqPos, byte lockId)
        {
            MapSegmentPointer pointer = new MapSegmentPointer(sqPos);

            seg.squares.LoopBegin();
            while (seg.squares.LoopNext())
            {
                BlockMapSquare sq = seg.squares.LoopValueGet();
                IntVector2 worldSqPos = seg.squares.LoopPosition + sqPos;

                if (sq.isEmpty())
                {
                    sq = squares.Get(worldSqPos);
                }
                else
                {
                    if (sq.special != MapBlockSpecialType.None)
                    {
                        pointer.addSpecial(worldSqPos, sq, this);
                    }
                    sq.lockId = lockId;
                }

                sq.terrain = currentTerrainIndex;
                squares.Set(worldSqPos, sq);
            }

            return pointer;
        }

        protected IntVector2 standardStartPos()
        {
            return new IntVector2(squares.Size.X / 2, squares.Size.Y / 8);
        }

        protected IntVector2 startPos(Vector2 percPos)
        {
            return new IntVector2(percPos * squares.Size.Vec);
        }

        abstract protected void generateMapAsynch();
        abstract protected List<VoxelModelName> loadTerrainModelsList();

        LevelEnum type;
        public LevelEnum LevelEnum
        {
            get { return type; }
        }

        public IntVector2 toWorldXZ(IntVector2 squarePos)
        {
            if (squarePos.X < 0)
            {
                throw new Exception();
            }
            return toWorldXZ(squarePos.X, squarePos.Y);
        }
        public IntVector2 toWorldXZ(int squareX, int squareY)
        {
            if (squareX < 0)
            {
                throw new Exception();
            }

            return new IntVector2(
                topLeftChunk.X * Map.WorldPosition.ChunkWidth + squareX * BlockMapLib.SquareBlockWidth,
                topLeftChunk.Y * Map.WorldPosition.ChunkWidth + squareY * BlockMapLib.SquareBlockWidth);
        }

        public IntVector2 toSquarePos(IntVector2 xz)
        {
            xz.X = (xz.X - topLeftChunk.X * Map.WorldPosition.ChunkWidth) / BlockMapLib.SquareBlockWidth;
            xz.Y = (xz.Y - topLeftChunk.Y * Map.WorldPosition.ChunkWidth) / BlockMapLib.SquareBlockWidth;
            return xz;
        }

        public IntVector2 toCenterWorldXZ(IntVector2 squarePos)
        {
            return toCenterWorldXZ(squarePos.X, squarePos.Y);
        }
        public IntVector2 toCenterWorldXZ(int squareX, int squareY)
        {
            return new IntVector2(
                topLeftChunk.X * Map.WorldPosition.ChunkWidth + squareX * BlockMapLib.SquareBlockWidth + BlockMapLib.SquareHalfWidth,
                topLeftChunk.Y * Map.WorldPosition.ChunkWidth + squareY * BlockMapLib.SquareBlockWidth + BlockMapLib.SquareHalfWidth);
        }

        protected Map.WorldPosition gridPosToHeightMapPos(IntVector2 xz, int addY, bool center)
        {
            return xzToHeightMapPos(center ? toCenterWorldXZ(xz) : toWorldXZ(xz), addY);
        }

        protected Map.WorldPosition xzToHeightMapPos(IntVector2 xz, int addY)
        {
            var wp = new Map.WorldPosition(xz, mapHeight(xz));
            wp.WorldGrindex.Y += addY;
            return wp;
        }

        public int mapHeight(IntVector2 worldXZ)
        {
            var sq = squares.Get( toSquarePos(worldXZ));

            return terrain[sq.terrain].mapHeight(worldXZ, sq.type);
        }

        public bool getSquare(Map.WorldPosition wp, out BlockMapSquare square)
        {
            IntVector2 pos = toSquarePos(wp.WorldXZ);
            //wp.WorldGrindex.X = (wp.WorldGrindex.X - topLeftChunk.X * Map.WorldPosition.ChunkWidth) / BlockMapLib.SquareBlockWidth;
            //wp.WorldGrindex.Z = (wp.WorldGrindex.Z - topLeftChunk.Y * Map.WorldPosition.ChunkWidth) / BlockMapLib.SquareBlockWidth;
            if (squares.InBounds(pos))
            {
                square = squares.Get(pos);
                return true;
            }
            else
            {
                square = BlockMapSquare.Empty;
                return false;
            }
        }

        IntVector2 toLocalChunkPos(IntVector2 squarePos)
        {
            squarePos.X /= BlockMapLib.SquaresPerChunkW;
            squarePos.Y /= BlockMapLib.SquaresPerChunkW;
            return squarePos;
        }

        protected IntVector2 toChunkPos(IntVector2 squarePos)
        {
            squarePos.X = squarePos.X / BlockMapLib.SquaresPerChunkW + topLeftChunk.X;
            squarePos.Y = squarePos.Y / BlockMapLib.SquaresPerChunkW + topLeftChunk.Y;
            return squarePos;
        }

        LevelChunk getOrCreateLvlChunk(IntVector2 localChunkPos)
        {
            LevelChunk result = chunks.Get(localChunkPos);
            if (result == null)
            {
                result = new LevelChunk();
                chunks.Set(localChunkPos, result);
            }

            return result;
        }
        
        protected void addTerrainModels(List<SquareItem> terrainModels, VoxelModelName[] models, PcgRandom rnd)
        {
            foreach (var m in terrainModels)
            {
                addTerrainModel(m.position, models, rnd);
            }
        }
        protected void addTerrainModel(IntVector2 squarePos, VoxelModelName[] models, PcgRandom rnd)
        {
            IntVector3 adj = new IntVector3(rnd.Int(-2, 2), -1, rnd.Int(-2, 2));
            addModel(models[Ref.rnd.Int(models.Length)], 0, squarePos, adj, true, true);
        }

        protected Map.WorldPosition addModel(VoxelModelName modelName, int rotation, IntVector2 squarePos, IntVector3 adjPos, bool center, bool fillUpTheGround)
        {
            return addModel(modelName, rotation, squarePos, toWorldXZ(squarePos), adjPos, center, center, fillUpTheGround);
        }

        protected Map.WorldPosition addModelOnWorldXZ(VoxelModelName modelName, int rotation, IntVector2 worldXZ, IntVector3 adjPos, bool center, bool fillUpTheGround)
        {
            return addModel(modelName, rotation, toSquarePos(worldXZ), worldXZ, adjPos, center, false, fillUpTheGround);
        }

        protected Map.WorldPosition addModel(VoxelModelName modelName, int rotation, IntVector2 squarePos, IntVector2 worldXZ, IntVector3 adjPos, bool centerModel, bool centerSquare, bool fillUpTheGround)
        {
            if (modelName == VoxelModelName.TutJumpToTeleport2)
            {
                lib.DoNothing();
            }

            IntVector3 modelSize = IntVector3.Zero;
            var nameRot = new VoxelModelNameAndRotation(modelName, rotation);

            var model = getModel(nameRot);

            if (centerModel)
            {
                worldXZ.X -= model.Size.X / 2;
                worldXZ.Y -= model.Size.Z / 2;

                modelSize = model.Size;
            }

            if (centerSquare)
            {
                worldXZ.X += BlockMapLib.SquareHalfWidth;
                worldXZ.Y += BlockMapLib.SquareHalfWidth;
            }

            worldXZ.X += adjPos.X;
            worldXZ.Y += adjPos.Z;

            IntVector2 center = worldXZ;
            center.X += model.Size.X / 2;
            center.Y += model.Size.Z / 2;

            var origo = new Map.WorldPosition(worldXZ, mapHeight(center));
            origo.WorldGrindex.Y += adjPos.Y;

            var chunk = getOrCreateLvlChunk(toLocalChunkPos(squarePos));
            chunk.models.Add(new TerrainModel(nameRot, origo, true));

            if (centerModel)
            { //correct origo so it returns center pos
                origo.WorldGrindex.X += modelSize.X / 2;
                origo.WorldGrindex.Z += modelSize.Z / 2;
            }
            return origo;
        }

        protected Voxels.VoxelObjGridData getModel(VoxelModelNameAndRotation name)
        {
            Voxels.VoxelObjGridData result;
            if (loadedModels.TryGetValue(name, out result) == false)
            {
                if (name.rotation == 0)
                {
                    missingModelException(name); 
                }
                else
                {
                    Voxels.VoxelObjGridData original;
                    if (loadedModels.TryGetValue(new VoxelModelNameAndRotation(name.name, 0), out original))
                    {
                        result = original.Rotate(name.rotation, false);
                        loadedModels.Add(name, result);
                    }
                    else
                    {
                        missingModelException(name); 
                    }
                }
            }

            return result;
        }

        void missingModelException(VoxelModelNameAndRotation name)
        {
            throw new Exception("Missing model: " + name.name.ToString() + ", in lvl: " + this.LevelEnum.ToString()); 
        }

        protected static List<ModelJoint> joints = new List<ModelJoint>();
        protected List<ModelJoint> getModelJoints(VoxelModelName modelName,  Map.WorldPosition pos, bool centered, int jointId)
        {
            joints.Clear();

            var model = loadedModels[modelName];

            if (model.special != null)
            {
                if (centered)
                {
                    pos.WorldGrindex.X -= model.Size.X / 2;
                    pos.WorldGrindex.Z -= model.Size.Z / 2;
                }

                foreach (var m in model.special)
                {
                    if (jointId < 0 || jointId == (m.Material - Voxels.VoxelLib.JointStart))
                    {
                        joints.Add(new ModelJoint(m, pos));
                    }
                }
            }

            return joints;
        }

        protected static List<ModelJoint> jointsSorted = new List<ModelJoint>();

        protected List<ModelJoint> getModelJointsSorted(VoxelModelName modelName, Map.WorldPosition pos, bool centered, Range idRange)
        {
            jointsSorted.Clear();
            getModelJoints(modelName, pos, centered, -1);

            for (int i = idRange.Min; i <= idRange.Max; ++i)
            {
                foreach (var m in joints)
                {
                    if (m.joint == i)
                    {
                        jointsSorted.Add(m);
                        break;
                    }
                }
            }

            return jointsSorted;
        }

        protected ModelJoint getModelJoint(int jointId)
        {
            foreach (var m in joints)
            {
                if (m.joint == jointId)
                {
                    return m;
                }
            }
            return joints[0];
        }

        protected List<Vector3> jointsToPatrolroute(List<ModelJoint> joints)
        {
            List<Vector3> route = new List<Vector3>(joints.Count);
            foreach (var m in joints)
            {
                route.Add(m.wp.PositionV3);
            }

            return route;
        }

        protected void placeMonsterSpawns(MapSegmentPointer sp, SuggestedSpawns spawns)
        {
            Rectangle2 area = BlockMapLib.SquareArea;
            foreach (var m in sp.spawnPositions)
            {
                area.pos = toWorldXZ(m.position);
                spawnPoints.Add(new SpawnPointArea(area, spawns, this.LevelEnum));
            }
        }

        protected void placeMonsterSpawns(MapSegmentPointer sp, SuggestedSpawns spawns, int fromIndex)
        {
            Rectangle2 area = BlockMapLib.SquareArea;
            foreach (var m in sp.spawnPositions)
            {
                if (m.square.specialIndex == fromIndex)
                {
                    area.pos = toWorldXZ(m.position);
                    spawnPoints.Add(new SpawnPointArea(area, spawns, this.LevelEnum));
                }
            }
        }

        protected void placeMonsterSpawn(IntVector2 squarePos, SuggestedSpawns spawns)
        {
            Rectangle2 area = BlockMapLib.SquareArea;
            area.pos = toWorldXZ(squarePos);
            spawnPoints.Add(new SpawnPointArea(area, spawns, this.LevelEnum));
        }

        protected void createLockSpawn(MapSegmentPointer segment, Dir4 onEntrance, CreateGameObjectDelegate createGO, int lockGroup, 
            byte entranceIx = byte.MinValue)
        {
            SpawnPointData spd = SpawnPointData.Empty;
            spd.level = lockGroup;
            spd.direction = (byte)onEntrance;
            var wp = gridPosToHeightMapPos(segment.getEntranceSqPos(onEntrance, entranceIx), 0, true);
            spawnPoints.Add(new SpawnPointDelegate(wp, createGO, spd, SpawnImportance.Must_0, false, 1, true));
        }

        protected void teleport(IntVector2 gridPos, TeleportLocationId toLocation, Dir4 enterDirection, VoxelModelName model)
        {
            var wp = addModel(model, 0, gridPos, IntVector3.Zero, true, true);
            wp.WorldGrindex.Y += 2;
            spawnPoints.Add(new SpawnPointDelegate(wp, createTeleport,
                    new SpawnPointData(GameObjectType.NUM_NON, (int)toLocation, 0, (byte)enterDirection), SpawnImportance.Must_0, false));
        }

        protected void createTeleport(GoArgs args)
        {
            new VikingEngine.LootFest.GO.EnvironmentObj.Teleport(args, (Dir4)args.direction, this.LevelEnum, (TeleportLocationId)args.characterLevel);
        }

        virtual public void onTeleport(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, LevelEnum toLevel)
        { }

        protected void createKeySpawn(IntVector2 worldXZ, bool bossKey)
        {
            spawnPoints.Add(new SpawnPointDelegate(worldXZ, constructGO_Key, new SpawnPointData(GameObjectType.NUM_NON,
                bossKey ? 1 : 0, 0), SpawnImportance.Must_0, false));
        }

        protected void constructGO_Key(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;
            VoxelModelName model;
            if (goArgs.characterLevel == 0)
            {
                model = VoxelModelName.key_lvl1;
            }
            else
            {
                model = VoxelModelName.key_lvl2;
            }

            goArgs.startWp.WorldGrindex += 1;
            new GO.PickUp.Key(goArgs, false, this, model);
        }

        protected MapSegmentPointer standardEntrance(IntVector2 pos, SegmentHeadType enterType, TeleportLocationId returnLocation, VoxelModelName[] terrainModels)
        {
            BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                new SegmentHeader(enterType, 0, 0, 1, 0), rnd);
            var pointer = placeSegment(segment, pos, OpenAreaLockId);
            addTerrainModels(pointer.terrainModels, terrainModels, rnd);

            IntVector2 spawnPos = pointer.spawnPositions[0].position;
            spawnPos.Y += 1;
            levelEntrance = toWorldXZ(spawnPos);

            spawnPos.Y -= 2;
            teleport(spawnPos, returnLocation, Dir4.N, VoxelModelName.DoorToLobby);

            SuitType suit1, suit2;
            switch (rnd.Int(5))
            {
                case 0: suit1 = SuitType.SpearMan; break;
                default: suit1 = SuitType.Swordsman; break;
            }

            switch (rnd.Int(5))
            {
                case 0: suit2 = SuitType.BarbarianDane; break;
                case 1: suit2 = SuitType.BarbarianDual; break;
                default: suit2 = SuitType.Archer; break;
            }

            spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.items[0].position), null,
                new SpawnPointData(GameObjectType.SuitBox, (int)suit1), SpawnImportance.Must_0, true, 1, true));
            spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.items[1].position), null,
                new SpawnPointData(GameObjectType.SuitBox, (int)suit2), SpawnImportance.Must_0, true, 1, true));

            return pointer;
        }

        public void AddLockConnectedGo(GO.AbsUpdateObj go, int lockId)
        {
            getOrCreateLockGroup(lockId).AddConnectedGo(go);
        }
        public void AddConnectedAreaLock(GO.EnvironmentObj.AreaLock arealock, int lockId)
        {
            getOrCreateLockGroup(lockId).AddAreaLock(arealock);
        }
        LockGroup getOrCreateLockGroup(int lockId)
        {
            LockGroup group;
            if (!lockGroups.TryGetValue(lockId, out group))
            {
                group = new LockGroup();
                lockGroups.Add(lockId, group);
            }

            return group;
        }

        protected void addSpawn(AbsSpawnPoint spawn)
        {
            spawnPoints.Add(spawn);
        }
        protected void addSpawn(SpawnPointDelegate spawn, AbsSpawnArgument spawnArgs)
        {
            spawn.spawnArgs = spawnArgs;
            spawnPoints.Add(spawn);
        }

        public void heightmap(IntVector2 chunk, int x, int z, ref int height, out HeightMapMaterials material)
        {
            int squareX = (chunk.X - topLeftChunk.X) * BlockMapLib.SquaresPerChunkW + x / BlockMapLib.SquareBlockWidth;
            int squareY = (chunk.Y - topLeftChunk.Y) * BlockMapLib.SquaresPerChunkW + z / BlockMapLib.SquareBlockWidth;

            var sq = squares.Get(squareX, squareY);

            terrain[sq.terrain].heightmap(chunk, x, z, sq, ref height, out material);
        }

        public void generateChunkDetail(VikingEngine.LootFest.Map.Chunk chunk)
        {
            PcgRandom rnd = new PcgRandom(mySeed + chunk.Index.X * 3 + chunk.Index.Y * 11);

            IntVector2 localChunkPos = chunk.Index - topLeftChunk;
            IntVector2 gridPosStart = localChunkPos * BlockMapLib.SquaresPerChunkW;
            IntVector2 gridPosEnd = gridPosStart + BlockMapLib.SquaresPerChunkW;

            IntVector2 gridPos = IntVector2.Zero;
            for (gridPos.Y = gridPosStart.Y; gridPos.Y < gridPosEnd.Y; ++gridPos.Y)
            {
                for (gridPos.X = gridPosStart.X; gridPos.X < gridPosEnd.X; ++gridPos.X)
                {
                    terrain[squares.Get(gridPos).terrain].generateChunkDetail(chunk, gridPos, rnd);
                }
            }

            var c = chunks.Get(localChunkPos);
            if (c != null)
            {
                foreach (var m in c.models)
                {
                    if (m.name.name == VoxelModelName.townwall_walltower)
                    {
                        lib.DoNothing();
                    }
                    var model = getModel(m.name);//loadedModels[m.name];
                    model.BuildOnTerrain(m.position);
                    if (m.fillUpTheGround)
                    {
                        var squareTerrain = terrain[squares.Get(gridPosStart).terrain];

                        ForXYLoop loop = new ForXYLoop(model.Size.XZplaneCoords);
                        while (loop.Next())
                        {
                            if (model.MaterialGrid[loop.Position.X, 0, loop.Position.Y] != byte.MinValue)
                            {
                                Map.WorldPosition wp = m.position;
                                wp.Add(loop.Position);
                                squareTerrain.fillUpGround(wp, m.position.WorldGrindex.Y - 1);
                            }
                        }
                    }
                }
            }
        }

        public bool heroInlevel()
        {
            for (int h = 0; h < LfRef.AllHeroes.Count; ++h)
            {
                if (LfRef.AllHeroes.Array[h].isInLevel(LevelEnum))
                {
                    return true;
                }
            }
            return false;
        }

        public AbsLevelTerrain terrainFromWp(Map.WorldPosition wp)
        {
            return terrain[squares.Get(toSquarePos(wp.WorldXZ)).terrain];
        }

        public IntVector2 levelCenterInWorldXZ()
        {
            return (topLeftChunk + ChunkSize / 2) * Map.WorldPosition.ChunkWidth;
        }

        public void setTeleportLocation(TeleportLocationId t, IntVector2 worldXZ)
        {
            var wp = new Map.WorldPosition(worldXZ, Map.WorldPosition.ChunkStandardHeight);
            LfLib.Location(t).wp = wp;
        }

        protected void placeOpenArea(Rectangle2 area, double terrainChance)
        {
            ForXYLoop loop = new ForXYLoop(area);
            BlockMapSquare open = new BlockMapSquare();
            open.type = MapBlockType.Open;

            BlockMapSquare terrain = new BlockMapSquare();
            terrain.type = MapBlockType.Open;
            terrain.special = MapBlockSpecialType.TerrainModel;

            while (loop.Next())
            {
                if (rnd.Chance(terrainChance))
                {
                    squares.Set(loop.Position, terrain);
                }
                else
                {
                    squares.Set(loop.Position, open);
                }
            }
        }

        protected void applyOpenArea(Rectangle2 area, Rectangle2 ignoreArea, VoxelModelName[] terrainModels, 
            double monsterChance, SuggestedSpawns spawnMonsters)
        {
            //Place terrain models and spawns
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                if (ignoreArea.X == 0 || ignoreArea.IntersectTilePoint(loop.Position) == false)
                {
                    var sq = squares.Get(loop.Position);
                    if (sq.type == MapBlockType.Open)
                    {
                        if (sq.special == MapBlockSpecialType.TerrainModel)
                        {
                            addTerrainModel(loop.Position, terrainModels, rnd);
                        }
                        else if (rnd.Chance(monsterChance))
                        {
                            placeMonsterSpawn(loop.Position, spawnMonsters);
                        }
                    }
                }
            }
        }

        protected void placeUpDownPath(IntVector2 squarePos, TeleportLocationId fromLocation, TeleportLocationId toLocation, Dir4 walkingDir, bool up)
        {
            Map.WorldPosition wp;
            if (up)
            {
                wp = addModel(VoxelModelName.uppath, (int)walkingDir, squarePos, IntVector3.Zero, true, true);

            }
            else
            {
                
                wp = addModel(VoxelModelName.downpath, (int)walkingDir, squarePos, IntVector3.FromHeight(-6), true, false);
                wp.Y += 8;
            }

            addSpawn(new SpawnPointDelegate(wp, null,
                new SpawnPointData(GameObjectType.TeleportWithin, (int)toLocation), SpawnImportance.Must_0, false, 1, false));

            wp.WorldXZ -= conv.ToIntV2(walkingDir, 6);
            LfLib.Location(fromLocation).wp = wp;
        }
    }

}
