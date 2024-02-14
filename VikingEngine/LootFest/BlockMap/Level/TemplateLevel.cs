//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.GO;

//namespace VikingEngine.LootFest.BlockMap.Level
//{
//    abstract class AbsTemplateLevel : AbsLevel
//    { 
//        const byte CenterAreaLockId = 1;
//        const byte EndAreaLockId = 2;
//        const byte BossLockId = 3;

//        const int MiniBossLockGroupId = 1;
//        const int BossLockGroupId = 2;


//        protected SuggestedSpawns spawnMonstersPart1;
//        protected SuggestedSpawns spawnMonstersPart2;

//        MapSegmentPointer bossRoom;
//        VoxelModelName[] terrainModels;

//        public AbsTemplateLevel(LevelEnum lvl)
//            : base(lvl)
//        {
//        }

//        protected override List<VoxelModelName> loadTerrainModelsList()
//        {
//            if (terrain is NormalTerrain)
//            {
//                terrainModels = NormalDefaultModels;
//            }
//            else
//            {
//                terrainModels = DefaultDungeonModels;
//            }

//            List<VoxelModelName> result = new List<VoxelModelName>(terrainModels);
//            result.Add(VoxelModelName.DoorToLobby);
//            return result;
//        }

//        protected override void generateMapAsynch()
//        {
//            IntVector2 pos = standardStartPos();

//            SegmentHeadType enterType, roomType, largeRoomType;

//            if (terrain is CastleTerrain)
//            {
//                enterType = SegmentHeadType.CastleEnter;
//                roomType = SegmentHeadType.Castle;
//                largeRoomType = SegmentHeadType.CastleLarge;
//            }
//            else
//            {
//                enterType = SegmentHeadType.NormalEnter;
//                roomType = SegmentHeadType.Normal;
//                largeRoomType = SegmentHeadType.NormalLarge;
//            }

//            MapSegmentPointer entrance = standardEntrance(pos, enterType, TeleportLocationId.Lobby, terrainModels);
//            //{
//            //    BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//            //        new SegmentHeader(enterType, 0, 0, 1, 0), rnd);
//            //    var pointer = placeSegment(segment, pos, OpenAreaLockId);
//            //    addTerrainModels(pointer.terrainModels, terrainModels, rnd);

//            //    IntVector2 spawnPos = pointer.spawnPositions[0].position;
//            //    spawnPos.Y += 1;
//            //    playerSpawn = toWorldXZ(spawnPos);

//            //    spawnPos.Y -= 2;
//            //    teleport(spawnPos, BlockMap.LevelEnum.Lobby, VoxelModelName.DoorToLobby);

//            //    SuitType suit1, suit2;
//            //    switch (rnd.Int(5))
//            //    {
//            //        case 0: suit1 = SuitType.SpearMan; break;
//            //        default: suit1 = SuitType.Swordsman; break; 
//            //    }

//            //    switch (rnd.Int(5))
//            //    {
//            //        case 0: suit2 = SuitType.BarbarianDane; break;
//            //        case 1: suit2 = SuitType.BarbarianDual; break;
//            //        default: suit2 = SuitType.Archer; break;
//            //    }

//            //    spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.items[0].position), null,
//            //        new SpawnPointData(GameObjectType.SuitBox, (int)suit1), SpawnImportance.Must_0, true, 1, true));
//            //    spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.items[1].position), null,
//            //        new SpawnPointData(GameObjectType.SuitBox, (int)suit2), SpawnImportance.Must_0, true, 1, true));

//            //    entrance = pointer;
//            //}

//            MapSegmentPointer entranceGuard;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(roomType, 1, 1, 1, 0), rnd);
//                var pointer = placeSegment(segment, entrance.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);
//                createLockSpawn(pointer, Dir4.S, doorToCenterArea, 0);
//                placeMonsterSpawns(pointer, spawnMonstersPart1);

//                entranceGuard = pointer;
//            }

//            MapSegmentPointer entranceKeyRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(roomType, 0, 0, 0, 1), rnd);
//                var pointer = placeSegment(segment, entranceGuard.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);
//                placeMonsterSpawns(pointer, spawnMonstersPart1);
//                createKeySpawn(toWorldXZ( pointer.spawnPositions[0].position), false);

//                entranceKeyRoom = pointer;
//            }

//            //--
//            MapSegmentPointer largeCenterArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(largeRoomType, 1, 1, 1, 1), rnd);
//                var pointer = placeSegment(segment, entranceGuard.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, CenterAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);
//                placeMonsterSpawns(pointer, spawnMonstersPart2);
//                createLockSpawn(pointer, Dir4.S, doorToEndArea, MiniBossLockGroupId);

//                largeCenterArea = pointer;
//            }

//            MapSegmentPointer miniBossRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(roomType, 0, 0, 0, 1), rnd);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, CenterAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);
//                placeMonsterSpawns(pointer, spawnMonstersPart2);
//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ( pointer.spawnPositions[0].position), createMiniBoss, SpawnPointData.Empty, SpawnImportance.Must_0, true));

//                miniBossRoom = pointer;
//            }

//            MapSegmentPointer salesmanRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(10);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, CenterAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), null,
//                  new SpawnPointData(GameObjectType.Salesman), SpawnImportance.Must_0, true, 1, true));

//                salesmanRoom = pointer;
//            }

//            //--
//            MapSegmentPointer preBossArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(roomType, 1, 0, 1, 0), rnd);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, EndAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);
//                placeMonsterSpawns(pointer, spawnMonstersPart2);

//                preBossArea = pointer;
//            }

//            //--
//            MapSegmentPointer bossGuard;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(roomType, 1, 0, 1, 0), rnd);
//                var pointer = placeSegment(segment, preBossArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, EndAreaLockId);
//                addTerrainModels(pointer.terrainModels, terrainModels, rnd);

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ( pointer.spawnPositions[0].position), createBossGuard, SpawnPointData.Empty, SpawnImportance.Must_0, true));
//                createLockSpawn(pointer, Dir4.S, doorToBoss, BossLockGroupId);

//                bossGuard = pointer;
//            }

//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.BossRoom, 1, 0, 0, 0), rnd);
//                var pointer = placeSegment(segment, bossGuard.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, BossLockId);
                
//                bossRoom = pointer;
//            }

//            //new Timer.Action0ArgTrigger(createDebugTexture);
//        }

        

        

//        void doorToCenterArea(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, CenterAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
//                0, null, VoxelModelName.locklvl1, Dir4.N);
//        }

//        void createMiniBoss(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            GO.Characters.AbsCharacter miniBoss = new VikingEngine.LootFest.GO.Characters.Boss.MidOrcBoss(goArgs);

//            miniBoss.levelCollider.SetLockedToArea();
//            miniBoss.SetAsManaged();

//            AddLockConnectedGo(miniBoss, MiniBossLockGroupId); //Borde lägga in restriction till segment

//            goArgs.startWp.Z -= 4;
//            for (int i = 0; i < 2; ++i)
//            {
//                var guard = new VikingEngine.LootFest.GO.Characters.OrcArcher(goArgs);

//                guard.levelCollider.SetLockedToArea();
//                guard.SetAsManaged();

//                AddLockConnectedGo(guard, MiniBossLockGroupId);

//                goArgs.startWp.Z += 8;
//            }
//        }

//        void createBossGuard(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            goArgs.startWp.X -= 2;

//            for (int i = 0; i < 3; ++i)
//            {
//                GO.Characters.AbsCharacter guard = new VikingEngine.LootFest.GO.Characters.OrcSoldier(goArgs);

//                guard.levelCollider.SetLockedToArea();
//                guard.managedGameObject = true;

//                AddLockConnectedGo(guard, BossLockGroupId); //Borde lägga in restriction till segment

//                goArgs.startWp.X += 6;
//            }
//        }

//        void doorToEndArea(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, EndAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
//                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
//        }
//        void doorToBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, BossLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
//                goArgs.characterLevel, startBossFight, VoxelModelName.locklvl2, Dir4.N);
//        }

//        void startBossFight()
//        {
//            spawnBoss().bossmanager.babySpawnPos = new Map.WorldPosition(toWorldXZ(bossRoom.spawnPositions[0].position), Map.WorldPosition.ChunkStandardHeight);
//            //new GO.Characters.Boss.GoblinKing(new GoArgs(bossSpawnPos), this);
//        }

//        abstract protected AbsUpdateObj spawnBoss();
//    }
//}
