using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class TutorialLevel : AbsLevel
    {
        public static readonly byte[] TutorialLobbyLockIds = new byte[] { SecondAreaLockId, ThirdAreaLockId, FourthAreaLockId };

        static readonly VoxelModelName[] TerrainModels = new VoxelModelName[]
        { 
            VoxelModelName.BirchTree1,
            VoxelModelName.BirchTree2,
        };

        const int GuardLockGroupId = 2;
        public const byte EndAreaLockId = ThirdAreaLockId +1;

        public TutorialLevel()
            : base(LevelEnum.Tutorial)
        {
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            var result = new List<VoxelModelName>
            {
                VoxelModelName.HomeHouse1,
                VoxelModelName.TutTargetHanger1,
                VoxelModelName.TutTargetHanger2,
                VoxelModelName.TutShootPlatform,
                VoxelModelName.TutJumpToTeleport2,
                VoxelModelName.DoorToLobby,
                VoxelModelName.DoorToFirstLevel,
                VoxelModelName.TownHouse2,
                VoxelModelName.TownHouse3,
                VoxelModelName.TownLamp,
            };
            result.AddRange(NormalDefaultModels);

            return result;
        }

        protected override void generateMapAsynch()
        {
            loadedModels[VoxelModelName.DoorToLobby].Rotate(1, true);

            //PcgRandom rnd = new PcgRandom(0);
            IntVector2 pos = startPos(new Microsoft.Xna.Framework.Vector2(0.2f, 0.5f));//new IntVector2(10);//standardStartPos();



            MapSegmentPointer startArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(39);
                var pointer = placeSegment(segment, pos, OpenAreaLockId);

                levelEntrance = toCenterWorldXZ(pointer.getEntranceSqPos(Dir4.N, 0));
                
                setTeleportLocation(TeleportLocationId.TutorialStart, toWorldXZ(pointer.getEntranceSqPos(Dir4.N, 0)));

                foreach (var m in pointer.terrainModels)
                {
                    addTerrainModel(m.position, TerrainModels, rnd);
                }
                addModel(VoxelModelName.HomeHouse1, 0, pointer.specialModels[0].position, IntVector3.Zero, true, true);
                foreach (var m in pointer.spawnPositions)
                {
                    switch (m.square.specialIndex)
                    {
                        case 0:
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null,
                                new SpawnPointData(GO.GameObjectType.CritterPig), SpawnImportance.Should_1, true, 3, false));
                            break;
                        case 1: //mother
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null,
                                new SpawnPointData(GO.GameObjectType.Mother), SpawnImportance.Should_1, true, 1, false));
                            break;
                        case 2: //attack tut guard
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null,
                                new SpawnPointData(GO.GameObjectType.AttackTutGuard), SpawnImportance.Must_0, true, 1, false));
                            break;
                        //case 3: //granpa, shoot tut
                        //    spawnPoints.Add(new SpawnPointDelegate(m.position, null,
                        //        new SpawnPointData(GO.GameObjectType.AmmoGranPa), SpawnImportance.Must_0, true, 3, false));
                        //    break;
                        //case 4: //jump tut 
                        //    spawnPoints.Add(new SpawnPointDelegate(m.position, null,
                        //        new SpawnPointData(GO.GameObjectType.JumpTutFather), SpawnImportance.Must_0, true, 1, false));
                        //    break;

                    }
                }

                createLockSpawn(pointer, Dir4.S, createAttackTutLock, -1, 0);
                //createLockSpawn(pointer, Dir4.E, createProjTutLock, -1, 0);

                var targetPos = pointer.specialPoints[0].position;
                addModel(VoxelModelName.TutTargetHanger1, 0, targetPos, IntVector3.NegativeY, false, false);

                Map.WorldPosition targetWp = xzToHeightMapPos(toWorldXZ(targetPos), 3);
                targetWp.X += 3;
                spawnPoints.Add(new SpawnPointDelegate(targetWp, createMeleeTarget,
                    SpawnPointData.FromDir(Dir4.N), SpawnImportance.Must_0, false, 1, true));


                startArea = pointer;
            }

            MapSegmentPointer projTutArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(40);
                var pointer = placeSegment(segment, startArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), null,
                    new SpawnPointData(GO.GameObjectType.AmmoGranPa), SpawnImportance.Must_0, true, 1, false));

                createLockSpawn(pointer, Dir4.E, createProjTutLock, -1, 0);

                foreach (var m in pointer.terrainModels)
                {
                    addTerrainModel(m.position, TerrainModels, rnd);
                }

                IntVector2 platformPos = pointer.specialPoints[0].position;
                addModel(VoxelModelName.TutShootPlatform, 0, platformPos, IntVector3.Zero, true, true);

                platformPos.X += 2;
                addModel(VoxelModelName.TutTargetHanger2, 0, platformPos, IntVector3.NegativeY, false, false);

                Map.WorldPosition targetWp = xzToHeightMapPos(toWorldXZ(platformPos), 12);
                targetWp.Z += 2;
                spawnPoints.Add(new SpawnPointDelegate(targetWp, createMeleeTarget,
                    SpawnPointData.FromDir(Dir4.W), SpawnImportance.Must_0, false, 1, true));

                projTutArea = pointer;
            }

            MapSegmentPointer jumpTutArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(41);
                var pointer = placeSegment(segment, projTutArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, ThirdAreaLockId);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), null,
                    new SpawnPointData(GO.GameObjectType.JumpTutFather), SpawnImportance.Must_0, true, 1, false));

                foreach (var m in pointer.terrainModels)
                {
                    addTerrainModel(m.position, TerrainModels, rnd);
                }

                var jumpPlatform = loadedModels[VoxelModelName.TutJumpToTeleport2];
                addModel(VoxelModelName.TutJumpToTeleport2, 0, pointer.getEntranceSqPos(Dir4.E, 0),
                    new IntVector3(-jumpPlatform.Size.X + BlockMapLib.SquareBlockWidth - 3, -1, -jumpPlatform.Size.Z / 2 + BlockMapLib.SquareHalfWidth),
                    false, true);

                jumpTutArea = pointer;
            }

            var spawnMonstersPart1 = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                new SpawnPointData( GameObjectType.SpitChick, 0, 6),
            });

            MapSegmentPointer CheckPointArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, jumpTutArea.getEntranceSqPos(Dir4.E, 1), Dir4.W, 0, ThirdAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null, 
                    new SpawnPointData(GameObjectType.CheckPointNpc), SpawnImportance.Must_0, false));
                
                CheckPointArea = pointer;
            }


            MapSegmentPointer monsterArea1;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(new SegmentHeader(SegmentHeadType.Normal, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, CheckPointArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, ThirdAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                for (int i = 0; i < 3; ++i)
                {
                    placeMonsterSpawns(pointer, spawnMonstersPart1, i);
                }

                monsterArea1 = pointer;
            }

            MapSegmentPointer monsterArea2;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(new SegmentHeader(SegmentHeadType.Normal, 1, 0, 0, 1), rnd);
                var pointer = placeSegment(segment, monsterArea1.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, ThirdAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                placeMonsterSpawns(pointer, spawnMonstersPart1);

                monsterArea2 = pointer;
            }

            //three guard area
            MapSegmentPointer guardArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, monsterArea2.getEntranceSqPos(Dir4.N, 0), Dir4.S, 0, ThirdAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                createLockSpawn(pointer, Dir4.N, createGuardLock, GuardLockGroupId);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), sleepingGoblin,
                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                guardArea = pointer;
            }

            MapSegmentPointer tutorialLobby;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(87);
                var pointer = placeSegment(segment, guardArea.getEntranceSqPos(Dir4.N, 0), Dir4.S, 0, FourthAreaLockId);

                
                //unlockedAreas.Add(FourthAreaLockId);

                const byte HouseIx = 0, LampIx = 1;

                foreach (var m in pointer.specialModels)
                {
                    switch (m.square.specialIndex)
                    {
                        case HouseIx:
                            addModel(rnd.Chance(0.7) ? VoxelModelName.TownHouse2 : VoxelModelName.TownHouse3, 0,
                                m.position, IntVector3.NegativeY, false, true);
                            break;
                        case LampIx:
                            addModel(VoxelModelName.TownLamp, 0, m.position, IntVector3.NegativeY, false, true);
                            break;
                    }
                }

                IntVector2 caveToFirstLevel = pointer.getEntranceSqPos(Dir4.W, 0);
                teleport(caveToFirstLevel, TeleportLocationId.FirstLevel, Dir4.W, VoxelModelName.DoorToFirstLevel);
                caveToFirstLevel.X += 2;
                setTeleportLocation(TeleportLocationId.CaveToIntroLevel, toWorldXZ(caveToFirstLevel));

                var transportXZ = toWorldXZ(pointer.specialPoints[0].position);
                addSpawn(new SpawnPointDelegate(transportXZ, null, new SpawnPointData(GameObjectType.HorseTransport), SpawnImportance.Must_0, false));

                transportXZ.X -= 6;
                setTeleportLocation(TeleportLocationId.TutorialLobby, transportXZ);

                tutorialLobby = pointer;
            }

            //MapSegmentPointer endTeleportArea;
            //{
            //    BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(new SegmentHeader(SegmentHeadType.NormalEnter, 0, 0, 0, 1), rnd);
            //    var pointer = placeSegment(segment, guardArea.getEntrance(Dir4.E, 0), Dir4.W, 0, EndAreaLockId);

            //    teleport(pointer.spawnPositions[0].position, BlockMap.LevelEnum.Lobby, VoxelModelName.DoorToLobby);

            //    endTeleportArea = pointer;
            //}

            //new Timer.Action0ArgTrigger(createDebugTexture);
        }

        void createAttackTutLock(GoArgs goArgs)
        {
            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
                goArgs.characterLevel, null, VoxelModelName.locklvl1, Dir4.N);
        }

        void createProjTutLock(GoArgs goArgs)
        {
            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, ThirdAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
                goArgs.characterLevel, null, VoxelModelName.locklvl1, Dir4.W);
        }

        void createGuardLock(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, FourthAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, onOpenGuardLock, VoxelModelName.locklvl2, Dir4.S);
        }

        void onOpenGuardLock()
        {
            LfRef.progress.UnlockProgressPoint(Players.ProgressPoint.TutorialLobby);
        }

        void createMeleeTarget(GoArgs goArgs)
        {
            new VikingEngine.LootFest.GO.EnvironmentObj.HitTarget(goArgs, Rotation1D.FromDir4((Dir4)goArgs.direction), true, this);
        }

        private void sleepingGoblin(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 2; ++i)
            {
                var guard = new VikingEngine.LootFest.GO.Characters.GoblinLineman(goArgs);
                guard.managedGameObject = true;
                guard.GoToSleep();
                AddLockConnectedGo(guard, GuardLockGroupId);

                goArgs.startWp.X += 4;
            }
        }

        public override void onTeleport(GO.PlayerCharacter.AbsHero hero, LevelEnum toLevel)
        {
            base.onTeleport(hero, toLevel);
            //hero.Player.Storage.completedLevels[(int)LevelEnum.Tutorial].completed = true;
        }
    }

    
}
