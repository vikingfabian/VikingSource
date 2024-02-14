//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using VikingEngine.LootFest.GO;

//namespace VikingEngine.LootFest.BlockMap.Level
//{
//    class Barrels : AbsLevel
//    {
//        MapSegmentPointer bossRoom;
//        /*
//         * --Explosiva tunnor banan-- 
//         * Start: mur med tunn-spawner
//         * Utanför en borg, bondgårdar, mur till borgen
//         * Borggård med mur mot borgens innerdel
//         * Stor sal med barrel-boss, tar bara skada av tunnor
//         * 
//         * Stor snabb farlig ogre som härjar
//         * Barrel-brute orc med extra hp
//         * 
//         * kan expandera till en bana med två borgar som har jätte-tunnor som ska eskorteras till jätteslott i mitten
//         */

//        public Barrels()
//            : base(LevelEnum.Barrels)
//        {
//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void generateMapAsynch()
//        {
//            IntVector2 startpos = startPos(new Vector2(0.3f, 0.3f));

//            MapSegmentPointer entrance = standardEntrance(startpos, SegmentHeadType.NormalEnter, TeleportLocationId.Lobby, NormalDefaultModels);

//            MapSegmentPointer entranceGuard;
//            {
//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
//                    new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
//                    new SpawnPointData( GameObjectType.Hog, 0, 8),
//                });

//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 1, 1, 1, 0), rnd);
//                var pointer = placeSegment(segment, entrance.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
//                createLockSpawn(pointer, Dir4.S, doorToCenterArea, 0);
//                placeMonsterSpawns(pointer, spawns);

//                entranceGuard = pointer;
//            }

//            MapSegmentPointer entranceKeyRoom;
//            {
//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
//                    new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
//                    new SpawnPointData( GameObjectType.Hog, 0, 8),
//                });

//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 0, 0, 0, 1), rnd);
//                var pointer = placeSegment(segment, entranceGuard.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);
//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
//                placeMonsterSpawns(pointer, spawns);
//                createKeySpawn(toWorldXZ(pointer.landMarkIx0), false);

//                entranceKeyRoom = pointer;
//            }

//            MapSegmentPointer centerArea;
//            {
//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
//                    new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
//                    new SpawnPointData( GameObjectType.OrcKnight, 0, 10),
//                });
//                spawns.mix = true;

//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(82);
//                var pointer = placeSegment(segment, entranceGuard.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

//                foreach (var m in pointer.items)
//                {
//                    createKeySpawn(toWorldXZ(m.position), false);
//                }

//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
//                placeMonsterSpawns(pointer, spawns);
//                createLockSpawn(pointer, Dir4.W, doorToCastle, -1);

//                centerArea = pointer;
//            }

//            MapSegmentPointer bossGuard;
//            {
//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.Pitbull, 0, 10),
//                    new SpawnPointData( GameObjectType.OrcKnight, 0, 15),
//                });
//                spawns.mix = true;


//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Castle, 0, 1, 0, 1), rnd);
//                var pointer = placeSegment(segment, centerArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, SecondAreaLockId);

//                placeMonsterSpawns(pointer, spawns);
//                createLockSpawn(pointer, Dir4.W, doorToBoss, -1);

//                bossGuard = pointer;
//            }

//            {//BOSS
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.BossRoom, 0, 1, 0, 0), rnd);
//                var pointer = placeSegment(segment, bossGuard.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, BossAreaLockId);

//                bossRoom = pointer;
//            }

//           // new Timer.Action0ArgTrigger(createDebugTexture);
//        }

//        void doorToCenterArea(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
//                0, null, VoxelModelName.locklvl1, Dir4.N);
//        }

//        void doorToCastle(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, ThirdAreaLockId, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
//                0, null, VoxelModelName.threelock, Dir4.E);
//        }

//        //void doorToInnerCastle(VikingEngine.LootFest.GO.GoArgs goArgs)
//        //{
//        //    goArgs.fromSpawn.spawnLock = 1;

//        //    var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, FourthAreaLockId, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
//        //        0, null, VoxelModelName.threelock, Dir4.E);
//        //}

//        void doorToBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, BossAreaLockId, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
//                0, startBossFight, VoxelModelName.threelock, Dir4.E);
//        }

//        void startBossFight()
//        {
//            var bossSpawnPos = new Map.WorldPosition(toWorldXZ(bossRoom.spawnPositions[0].position), Map.WorldPosition.ChunkStandardHeight);
//            new GO.Characters.Boss.StatueBoss(new GoArgs(bossSpawnPos), this);
//        }

//        protected override List<VoxelModelName> loadTerrainModelsList()
//        {
//            var result = new List<VoxelModelName>
//                {
//                    VoxelModelName.DoorToLobby,
//                    VoxelModelName.GoblinKeyGuard2,

//                };
//            result.AddRange(NormalDefaultModels);
//            return result;
//        }
//    }
//}
