using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class IntroductionLevel : AbsLevel
    {
        const int FirstLockGroup = 1;

        public IntroductionLevel()
            : base(LevelEnum.IntroductionLevel)
        {
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
        }

        protected override void generateMapAsynch()
        {
            /*
             * 1.Sovande vakter
             * 2.Patrullerande runt fortress//53
             * 3.Stort rum med två patrul murar //rum 49, spec0: mur, spec1: post
             * 4.hund gård //N to E
             * 5.Borg med boss //52
             */
            //MapSegmentPointer entrance = standardEntrance(standardStartPos(), SegmentHeadType.NormalEnter, TeleportLocationId.CaveToFirstLevel, NormalDefaultModels);
            MapSegmentPointer entrance;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalEnter, 0, 0, 0, 1), rnd);
                var pointer = placeSegment(segment, standardStartPos(), OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                IntVector2 spawnPos = pointer.spawnPositions[0].position;
                spawnPos.Y += 1;
                levelEntrance = toWorldXZ(spawnPos);

                spawnPos.Y -= 2;
                teleport(spawnPos, TeleportLocationId.CaveToIntroLevel, Dir4.E, VoxelModelName.DoorToLobby);
                //LfLib.Location(TeleportLocationId.FirstLevel).wp = new Map.WorldPosition(new Vector2toV3(levelEntrance));
                setTeleportLocation(TeleportLocationId.FirstLevel, levelEntrance);

                entrance = pointer;
            }


            MapSegmentPointer checkPoint;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 1, 0), rnd);
                var pointer = placeSegment(segment, entrance.getEntranceSqPos(Dir4.W), Dir4.E, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                    new SpawnPointData(GameObjectType.CheckPointNpc), SpawnImportance.Must_0, false));

                SuitType suit1 = SuitType.Swordsman, suit2 = SuitType.Archer;

                spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.specialPoints[1].position), null,
                    new SpawnPointData(GameObjectType.SuitBox, (int)suit1), SpawnImportance.Must_0, true, 1, true));
                spawnPoints.Add(new SpawnPointDelegate(toCenterWorldXZ(pointer.specialPoints[2].position), null,
                    new SpawnPointData(GameObjectType.SuitBox, (int)suit2), SpawnImportance.Must_0, true, 1, true));

                checkPoint = pointer;
            }

            MapSegmentPointer sleepingGuardSegment;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, checkPoint.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                IntVector2 guardPos = toWorldXZ(pointer.spawnPositions[0].position);
                spawnPoints.Add(new SpawnPointDelegate(guardPos, createLockGuard,
                    SpawnPointData.Empty, SpawnImportance.Must_0, true));

                guardPos.Y += 4;
                addModelOnWorldXZ(VoxelModelName.GoblinGate1, 0, guardPos, IntVector3.NegativeY, true, false);

                createLockSpawn(pointer, Dir4.S, createLock, FirstLockGroup);

                sleepingGuardSegment = pointer;
            }

            MapSegmentPointer smallFortress;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(53);
                var pointer = placeSegment(segment, sleepingGuardSegment.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

                var fortWp = addModel(VoxelModelName.GoblinFortress1, 0, pointer.specialPoints[0].position, IntVector3.NegativeY, true, true);
                var route = jointsToPatrolroute(getModelJointsSorted(VoxelModelName.GoblinFortress1, fortWp, true, new Range(0, 3)));

                {//Guard 1
                    VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 0);
                    SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[0].wp, null,
                        new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.Should_1, true);
                    spawn.spawnArgs = patrol;

                    spawnPoints.Add(spawn);
                }
                {//Guard 2
                    VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 2);
                    SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[2].wp, null,
                        new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.Should_1, true);
                    spawn.spawnArgs = patrol;

                    spawnPoints.Add(spawn);
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                smallFortress = pointer;
            }

            MapSegmentPointer guardWalls;
            { //54
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(54);
                var pointer = placeSegment(segment, smallFortress.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

                //Special0 = walls, spec1 = post
                foreach (var m in pointer.specialPoints)
                {
                    if (m.square.specialIndex == 0)
                    {
                        var modelWp = addModel(VoxelModelName.GoblinPatrolWall1, 0, m.position, IntVector3.NegativeY, false, true);
                        var route = jointsToPatrolroute(getModelJointsSorted(VoxelModelName.GoblinPatrolWall1, modelWp, false, new Range(0, 1)));
                        VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, false, 0);

                        SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[0].wp, null,
                            new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.Should_1, true);
                        spawn.spawnArgs = patrol;

                        spawnPoints.Add(spawn);
                    }
                    else
                    {
                        var modelWp = addModel(VoxelModelName.GoblinPost1, 0, m.position, IntVector3.NegativeY, true, false);
                        modelWp.Z -= 4;
                        var spawn = new SpawnPointDelegate(modelWp, null,
                            new SpawnPointData(GameObjectType.GoblinBerserk), SpawnImportance.LowSuggest_3, true);
                        spawn.spawnArgs = SleepingSpawnArg.ins;
                        spawnPoints.Add(spawn);
                    }
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                guardWalls = pointer;
            }

            MapSegmentPointer dogPark;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                    new SpawnPointData( GameObjectType.SpitChick, 0, 6),
                });

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 1, 1, 0), rnd);
                var pointer = placeSegment(segment, guardWalls.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                addModel(VoxelModelName.GoblinDogHouse1, 0, pointer.landMarkIx0, IntVector3.NegativeY, false, false);
                placeMonsterSpawns(pointer, spawns);


                createLockSpawn(pointer, Dir4.S, doorToBoss, -1);

                dogPark = pointer;
            }

            MapSegmentPointer entranceKeyRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 0, 0, 0, 1), rnd);
                var pointer = placeSegment(segment, dogPark.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

                //Key = joint9
                var modelWp = addModel(VoxelModelName.GoblinKeyGuard1, 0, pointer.spawnPositions[0].position, IntVector3.Zero, true, true);

                var route = jointsToPatrolroute(getModelJointsSorted(VoxelModelName.GoblinKeyGuard1, modelWp, true, new Range(0, 3)));

                {//Guard 1
                    VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 0);
                    SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[0].wp, null,
                        new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.Should_1, true);
                    spawn.spawnArgs = patrol;

                    spawnPoints.Add(spawn);
                }
                {//Guard 2
                    VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, true, 2);
                    SpawnPointDelegate spawn = new SpawnPointDelegate(jointsSorted[2].wp, null,
                        new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.Should_1, true);
                    spawn.spawnArgs = patrol;

                    spawnPoints.Add(spawn);
                }

                spawnPoints.Add(new SpawnPointDelegate(getModelJoint(9).wp, constructGO_Key, new SpawnPointData(GameObjectType.NUM_NON,
                    1, 0), SpawnImportance.Must_0, false));

                entranceKeyRoom = pointer;
            }


            MapSegmentPointer endFortress;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, dogPark.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, ThirdAreaLockId);


                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createBoss,
                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));
                //joints
                //0: boss
                //1: ground troops
                //2: tower troops
                //var fortWp = addModel(VoxelModelName.GoblinEndFortress1, 0, pointer.specialPoints[0].position, IntVector3.NegativeY, true, true);

                //getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 0);
                //spawnPoints.Add(new SpawnPointDelegate(joints[0].wp, createBoss, SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                //getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 1);
                //foreach (var m in joints)
                //{
                //    spawnPoints.Add(new SpawnPointDelegate(m.wp, null, new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.HighSuggest_2, true, 1, true));
                //}

                //getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 2);
                //foreach (var m in joints)
                //{
                //    spawnPoints.Add(new SpawnPointDelegate(m.wp, null, new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.LowSuggest_3, true, 1, true));
                //}

                //addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                endFortress = pointer;
            }

        }

        protected void constructGO_Key(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;
            VoxelModelName model = VoxelModelName.key_lvl2;

            new GO.PickUp.Key(goArgs, false, this, model);
        }


        void createLockGuard(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 2; ++i)
            {
                GO.Characters.AbsMonster3 guard = new VikingEngine.LootFest.GO.Characters.GoblinScout(goArgs);

                guard.GoToSleep();
                guard.levelCollider.SetLockedToArea();
                guard.managedGameObject = true;

                AddLockConnectedGo(guard, FirstLockGroup); //Borde lägga in restriction till segment

                goArgs.startWp.X += 6;
            }
        }

        void createLock(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
        }

        void doorToBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, ThirdAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
                goArgs.characterLevel, null, VoxelModelName.locklvl2, Dir4.N);
        }

        void createBoss(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            GO.Characters.AbsCharacter boss = new VikingEngine.LootFest.GO.Characters.Boss.StatueBoss(goArgs, this);
            boss.managedGameObject = true;

            //bossSpawnPos = goArgs.startWp;
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            var result = new List<VoxelModelName>
                {
                    VoxelModelName.DoorToLobby,
                    VoxelModelName.GoblinGate1,
                    VoxelModelName.GoblinPatrolWall1,
                    VoxelModelName.GoblinFortress1,
                    VoxelModelName.GoblinEndFortress1,
                    VoxelModelName.GoblinTent,
                    VoxelModelName.GoblinKeyGuard1,
                    VoxelModelName.GoblinPost1,
                    VoxelModelName.goblin_hut,
                    VoxelModelName.GoblinDogHouse1,
                };
            result.AddRange(NormalDefaultModels);
            return result;
        }
    }
}
