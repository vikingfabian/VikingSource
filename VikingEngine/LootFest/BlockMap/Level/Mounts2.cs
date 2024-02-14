//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.GO;

//namespace VikingEngine.LootFest.BlockMap.Level
//{
//    class Mounts2 : AbsLevel
//    {
//        public Mounts2()
//            : base(LevelEnum.Mount)
//        {
//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void generateMapAsynch()
//        {
//            const int FirstLockGroup = 1;

//            const int SquigBossLockGroup = 2;
//            /*
//             * Start: Bondgård med hästar och bonde
//             * 
//             * Lång korridor med varg-ryttare 
//             * Rum med vakter + lock
//             * Stort område med tre borgar+nyckel, introducerar ägg och squig 
//             * Korridor med mer squigs
//             * sidorum med säljare
//             * 
//             * Miniboss - mängder av squigs och ägg, squig boss med nyckel
//             * 
//             * Vilorum, fler hästar
//             * Slingrande hall med vakter i torn som skjuter neråt
//             * Slutrum för bossfight
//             */

//            MapSegmentPointer entrance = standardEntrance(standardStartPos(), SegmentHeadType.NormalEnter, TeleportLocationId.Lobby, NormalDefaultModels);

//            MapSegmentPointer horseFarm;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 1, 1, 0, 0), rnd);
//                var pointer = placeSegment(segment, entrance.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

//                int horseCount = 0;
//                foreach (var m in pointer.spawnPositions)
//                {
//                    spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Horse),
//                       horseCount == 0 ? SpawnImportance.Must_0 : SpawnImportance.HighSuggest_2, true));

//                    if (++horseCount >= 3)
//                    {
//                        break;
//                    }
//                }

//                horseFarm = pointer;
//            }

//            MapSegmentPointer ridingCorridor;
//            {
//                //:57 - spawn0 wolf, spawn1 goblin, spawn2 dogs
//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(57);
//                var pointer = placeSegment(segment, horseFarm.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

//                foreach (var m in pointer.spawnPositions)
//                {
//                    switch (m.square.specialIndex)
//                    {
//                        case 0:
//                            IntVector2 xz = toWorldXZ(m.position);
//                            addSpawn(new SpawnPointDelegate(xz, null, new SpawnPointData(GameObjectType.GreatWolf),
//                                SpawnImportance.HighSuggest_2, true), SleepingSpawnArg.ins);

//                            xz.Y += rnd.Dir() * 4;
//                            spawnPoints.Add(new SpawnPointDelegate(xz, null, new SpawnPointData(GameObjectType.GoblinBerserk),
//                                SpawnImportance.HighSuggest_2, true));
//                            break;
//                        case 1:
//                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.GoblinScout),
//                                SpawnImportance.LowSuggest_3, true));
//                            break;
//                        case 2:
//                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Pitbull),
//                                SpawnImportance.LowSuggest_3, true));
//                            break;
//                    }
//                }

//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                ridingCorridor = pointer;
//            }

//            MapSegmentPointer firstGuardSegment;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 0, 0, 1, 1), rnd);
//                var pointer = placeSegment(segment, ridingCorridor.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                int guardCount = 0;
//                foreach (var m in pointer.spawnPositions)
//                {
//                    if (guardCount < 2)
//                    {
//                        addSpawn(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.GoblinLineman),
//                            SpawnImportance.Must_0, true, 1, true),

//                            new LockGroupSpawnArgs(this, FirstLockGroup));

//                        guardCount++;
//                    }
//                    else
//                    {
//                        addSpawn(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Pitbull),
//                            SpawnImportance.LowSuggest_3, true, 1, false));
//                    }
//                }

//                createLockSpawn(pointer, Dir4.S, createLock1, FirstLockGroup);

//                firstGuardSegment = pointer;
//            }

//            //58
//            MapSegmentPointer threeCastlesArea;
//            {

//                var squigSpawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
//                    new SpawnPointData( GameObjectType.SquigRedBaby, 0, 8),
//                });
//                squigSpawns.mix = true;

//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(58);
//                var pointer = placeSegment(segment, firstGuardSegment.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

//                //spec 0 = castle, spec 1 = squigs
//                foreach (var m in pointer.specialPoints)
//                {
//                    if (m.square.specialIndex == 0)
//                    {
//                        //Key = joint9
//                        var modelWp = addModel(VoxelModelName.GoblinKeyGuard2, 0, m.position, IntVector3.Zero, true, true);

//                        var route = jointsToPatrolroute(getModelJointsSorted(VoxelModelName.GoblinKeyGuard2, modelWp, true, new Range(0, 3)));

//                        {//Guard 1
//                            VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 0);
//                            SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[0].wp, null,
//                                new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.Should_1, true);
//                            spawn.spawnArgs = patrol;

//                            spawnPoints.Add(spawn);
//                        }
//                        {//Guard 2
//                            VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 2);
//                            SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[2].wp, null,
//                                new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.Should_1, true);
//                            spawn.spawnArgs = patrol;

//                            spawnPoints.Add(spawn);
//                        }

//                        spawnPoints.Add(new SpawnPointDelegate(getModelJoint(9).wp, constructGO_Key, new SpawnPointData(GameObjectType.NUM_NON,
//                            1, 0), SpawnImportance.Must_0, false));
//                    }
//                    else
//                    {
//                        placeMonsterSpawn(m.position, squigSpawns);
//                    }
//                }

//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
//                    new SpawnPointData( GameObjectType.GoblinBerserk, 0, 8),
//                    new SpawnPointData( GameObjectType.Pitbull, 0, 15),

//                });

//                placeMonsterSpawns(pointer, spawns);

//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                createLockSpawn(pointer, Dir4.S, createLock2, -1);

//                threeCastlesArea = pointer;
//            }

//            MapSegmentPointer smallSquigArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 1, 1, 1, 0), rnd);
//                var pointer = placeSegment(segment, threeCastlesArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, ThirdAreaLockId);

//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
//                    new SpawnPointData( GameObjectType.SquigGreen, 0, 8),
//                });
//                spawns.mix = true;

//                placeMonsterSpawns(pointer, spawns);

//                smallSquigArea = pointer;
//            }

//            MapSegmentPointer salesmanRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 0, 0, 1), rnd);
//                var pointer = placeSegment(segment, smallSquigArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, ThirdAreaLockId);
//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
//                  new SpawnPointData(GameObjectType.Salesman), SpawnImportance.Must_0, true, 1, true));

//                salesmanRoom = pointer;
//            }

//            MapSegmentPointer squigBossRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.NormalLarge, 1, 0, 1, 0), rnd);
//                var pointer = placeSegment(segment, smallSquigArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, ThirdAreaLockId);
//                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

//                var spawns = new SuggestedSpawns(new List<SpawnPointData>
//                {
//                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
//                    new SpawnPointData( GameObjectType.SquigRedBaby, 0, 5),
//                    new SpawnPointData( GameObjectType.SquigGreen, 0, 8),
//                    new SpawnPointData( GameObjectType.SquigGreenBaby, 0, 4),
//                });
//                spawns.mix = true;
//                placeMonsterSpawns(pointer, spawns);

//                IntVector2 southtExit = pointer.getEntranceSqPos(Dir4.S, 0);
//                southtExit.Y -= 2;

//                addSpawn(new SpawnPointDelegate(toWorldXZ(southtExit), null, new SpawnPointData(GameObjectType.SquigHorned),
//                            SpawnImportance.Must_0, true, 3, true),

//                            new LockGroupSpawnArgs(this, SquigBossLockGroup));

//                squigBossRoom = pointer;

//                createLockSpawn(pointer, Dir4.S, createLock3, SquigBossLockGroup);
//            }


//            MapSegmentPointer horseFarm2;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
//                var pointer = placeSegment(segment, squigBossRoom.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

//                int horseCount = 0;
//                foreach (var m in pointer.spawnPositions)
//                {
//                    spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Horse),
//                       horseCount == 0 ? SpawnImportance.Must_0 : SpawnImportance.HighSuggest_2, true));

//                    if (++horseCount >= 3)
//                    {
//                        break;
//                    }
//                }

//                horseFarm2 = pointer;
//            }

//            MapSegmentPointer bossRoom;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.BossRoom, 1, 0, 0, 0), rnd);
//                var pointer = placeSegment(segment, horseFarm2.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, FourthAreaLockId);

//                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createBoss,
//                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

//                bossRoom = pointer;
//            }

//            //new Timer.Action0ArgTrigger(createDebugTexture);
//        }

//        protected void constructGO_Key(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;
//            VoxelModelName model = VoxelModelName.key_lvl1;

//            new GO.PickUp.Key(goArgs, false, this, model);
//        }

//        void createLock1(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
//                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
//        }

//        void createLock2(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, ThirdAreaLockId, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
//                goArgs.characterLevel, null, VoxelModelName.threelock, Dir4.N);
//        }

//        void createLock3(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, FourthAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
//                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
//        }

//        protected void createBoss(GoArgs goArgs)
//        {
//            bossSpawnPos = goArgs.startWp;
//            goArgs.fromSpawn.spawnLock = 1;

//            var boss = new VikingEngine.LootFest.GO.Characters.GoblinWolfRiderBoss(goArgs, this);
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
