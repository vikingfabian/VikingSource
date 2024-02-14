using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class Lobby : AbsLevel
    {
        static readonly VoxelModelName[] EmoForestModels = new VoxelModelName[]
        { 
            VoxelModelName.ForestTree1,
            VoxelModelName.ForestTree2,
            VoxelModelName.ForestTree3,
            VoxelModelName.ForestTree4,
            VoxelModelName.ForestTree5,

            VoxelModelName.ForestStone1,
//            VoxelModelName.ForestStone2,
            VoxelModelName.ForestStone3,
        };

        static readonly VoxelModelName[] EmoOpenAreaModels = new VoxelModelName[]
        { 
            VoxelModelName.ForestTree1,
            VoxelModelName.ForestTree2,
            VoxelModelName.ForestStone1,

            VoxelModelName.ForestBurnedTree1,
            VoxelModelName.ForestBurnedTree2,
            VoxelModelName.TrollRuin1,
            VoxelModelName.TrollRuin2,
            VoxelModelName.TrollRuin3,
        };

        MapSegmentPointer pointer;
        const int GoblinsFirstLockGroup = 1;
        const int EmoFirstLockGroup = 2;
        
        const int HorseFirstLockGroup = 3;
        const int SquigBossLockGroup = 4;

        const byte ElfBossGuardLockGroup = 5;
        const byte HogBossGuardLockGroup = 6;


        const byte GoblinsBossAreaLockId = 11;

        const byte EmoSecondAreaLockId = 21;

        const byte ElfBossAreaLockId = 31;

        const byte HogBossAreaLockId = 41;

        const byte HorseAreaId2 = 51;
        const byte HorseAreaId3 = 52;
        const byte HorseAreaId4 = 53;

        const byte BarrelsAreaId2 = 61;
        const byte BarrelsAreaId3 = 62;


        public static Map.WorldPosition spiderBossWp;

        public Lobby()
            : base(LevelEnum.Lobby)
        {
            collect = new CollectItem(VikingEngine.LootFest.GO.NPC.EmoSuitSmith.CraftingIngotCount, SpriteName.LfMithrilIngot);
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
        }

        protected override void generateMapAsynch()
        {
            //64 stor
            IntVector2 startPos = new IntVector2(22, 8) * BlockMapLib.SquaresPerChunkW;
            MapSegmentPointer townArea = lobbyTownArea(startPos);

            MapSegmentPointer goblinGuard;
            {
                //Spec0: mur 7 bred, Spec1: vertical vakt, Spec2: Hori Vakt, Landmark0: skylt till gob king, Spawns: Hundar

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(91);
                var pointer = placeSegment(segment, townArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

                foreach (var m in pointer.specialPoints)
                {
                    switch (m.square.specialIndex)
                    {
                        case 0:
                            {
                                addModel(VoxelModelName.GoblinGate2, 0,
                                    m.position, IntVector3.NegativeY, true, true);

                                IntVector2 guardPos = m.position + new IntVector2(1, -2);
                                addSpawn(new SpawnPointDelegate(toWorldXZ(guardPos), null,
                                    new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.Should_1, true), SleepingSpawnArg.ins);
                            }
                            break;
                        case 1:
                            {
                                Vector3 startpos = VectorExt.V2toV3XZ(toWorldXZ(m.position).Vec, Map.WorldPosition.ChunkStandardHeight);
                                var route = new List<Vector3> { startpos, VectorExt.AddZ(startpos, 16) };
                                VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, false, 0);

                                SpawnPointDelegate spawn = new SpawnPointDelegate(new Map.WorldPosition(route[0]), null,
                                   new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.Should_1, true);
                                spawn.spawnArgs = patrol;

                                spawnPoints.Add(spawn);
                            }
                            break;
                        case 2:
                            {
                                Vector3 startpos = VectorExt.V2toV3XZ(toWorldXZ(m.position).Vec, Map.WorldPosition.ChunkStandardHeight);
                                var route = new List<Vector3> { startpos, VectorExt.AddX(startpos, 30) };
                                VikingEngine.LootFest.GO.Characters.AI.PatrolRoute patrol = new GO.Characters.AI.PatrolRoute(route, false, 0);

                                SpawnPointDelegate spawn = new SpawnPointDelegate(new Map.WorldPosition(route[0]), null,
                                   new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.Should_1, true);
                                spawn.spawnArgs = patrol;

                                spawnPoints.Add(spawn);
                            }
                            break;
                    }
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                var spawns = new SuggestedSpawns(new SpawnPointData(GameObjectType.Pitbull));
                placeMonsterSpawns(pointer, spawns);
                goblinGuard = pointer;
            }

            goblinsArea(goblinGuard.getEntranceSqPos(Dir4.E, 0));

            MapSegmentPointer mazeEntrance;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
                    new SpawnPointData( GameObjectType.GoblinBerserk, 0, 8),
                    new SpawnPointData( GameObjectType.Pitbull, 0, 15),

                });

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, goblinGuard.getEntranceSqPos(Dir4.S, 0),
                    Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                mazeEntrance = pointer;


            }

            {//maze entrance NPC room
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, mazeEntrance.getEntranceSqPos(Dir4.W, 0),
                    Dir4.E, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
            }

            MapSegmentPointer mazeEntranceRoad;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalRoad, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, mazeEntrance.getEntranceSqPos(Dir4.S, 0),
                    Dir4.N, 0, OpenAreaLockId);

                mazeEntranceRoad = pointer;
            }

            IntVector2 mazeNorthExitSqPos;
            IntVector2 mazeSouthExitSqPos;
            IntVector2 mazeWestExitSqPos;
            mazeArea(mazeEntranceRoad.getEntranceSqPos(Dir4.S), out mazeNorthExitSqPos, out mazeSouthExitSqPos, out mazeWestExitSqPos);

            horseArea(mazeNorthExitSqPos);
            hogArea(mazeWestExitSqPos);

            IntVector2 farmWestExit;
            farmVillageArea(mazeSouthExitSqPos, out farmWestExit);

            IntVector2 barrelsNorthExit;
            barrelsArea(farmWestExit, out barrelsNorthExit);

            elfArea(barrelsNorthExit);

            //EAST
            emoArea(townArea.getEntranceSqPos(Dir4.E));


        }

        MapSegmentPointer hogBossRoom;

        void hogArea(IntVector2 startPos)
        {
            MapSegmentPointer bossGuard;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, startPos, Dir4.E, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createHogBossGuard, 
                    SpawnPointData.Empty, SpawnImportance.Must_0, true));
                createLockSpawn(pointer, Dir4.W, hogDoorToBoss, HogBossGuardLockGroup);

                bossGuard = pointer;
            }

            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, bossGuard.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, HogBossAreaLockId);

                hogBossRoom = pointer;
            }
        }

        void createHogBossGuard(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 3; ++i)
            {
                GO.Characters.AbsCharacter guard = new VikingEngine.LootFest.GO.Characters.OrcSoldier(goArgs);

                guard.levelCollider.SetLockedToArea();
                guard.managedGameObject = true;

                AddLockConnectedGo(guard, HogBossGuardLockGroup); //Borde lägga in restriction till segment

                goArgs.startWp.X += 6;
            }
        }

        void hogDoorToBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, HogBossAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, startHogBossFight, VoxelModelName.locklvl2, Dir4.W);
        }

        void startHogBossFight()
        {
            var bossSpawnPos = new Map.WorldPosition(toWorldXZ(hogBossRoom.spawnPositions[0].position), Map.WorldPosition.ChunkStandardHeight);
            new VikingEngine.LootFest.GO.Characters.Monsters.OldSwineBoss(new GoArgs(bossSpawnPos), this);
            //new GO.Characters.Boss.GoblinKing(new GoArgs(bossSpawnPos), this);
        }

        

        MapSegmentPointer elfBossRoom;

        void elfArea(IntVector2 startPos)
        {
            MapSegmentPointer bossGuard;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, startPos, Dir4.S, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createElfBossGuard, SpawnPointData.Empty, SpawnImportance.Must_0, true));
                createLockSpawn(pointer, Dir4.N, elfDoorToBoss, ElfBossGuardLockGroup);

                bossGuard = pointer;
            }

            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 0, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, bossGuard.getEntranceSqPos(Dir4.N, 0), Dir4.S, 0, ElfBossAreaLockId);

                elfBossRoom = pointer;
            }
        }

        void createElfBossGuard(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 4; ++i)
            {
                GO.Characters.AbsCharacter guard;

                if (lib.IsEven(i))
                {
                    guard = new VikingEngine.LootFest.GO.Characters.HumanionEnemy.ElfArcher(goArgs);
                }
                else
                {
                    guard = new VikingEngine.LootFest.GO.Characters.HumanionEnemy.ElfWardancer(goArgs);
                }

                guard.levelCollider.SetLockedToArea();
                guard.managedGameObject = true;

                AddLockConnectedGo(guard, ElfBossGuardLockGroup); //Borde lägga in restriction till segment

                goArgs.startWp.X += 6;
            }
        }

        void elfDoorToBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, ElfBossAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, startElfBossFight, VoxelModelName.locklvl2, Dir4.N);
        }

        void startElfBossFight()
        {
            var bossSpawnPos = new Map.WorldPosition(toWorldXZ(elfBossRoom.spawnPositions[0].position), Map.WorldPosition.ChunkStandardHeight);
            new VikingEngine.LootFest.GO.Characters.Boss.ElfKing(new GoArgs(bossSpawnPos), this);
            //new GO.Characters.Boss.GoblinKing(new GoArgs(bossSpawnPos), this);
        }


        void emoArea(IntVector2 startPos)
        {
            var goblinsSpawn = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                new SpawnPointData( GameObjectType.GoblinLineman, 0, 15),
                new SpawnPointData( GameObjectType.GoblinBerserk, 0, 4),
            });

            var monsterSpawn = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.Hog, 0, 10),
                new SpawnPointData( GameObjectType.Pitbull, 0, 5),
                new SpawnPointData( GameObjectType.SpitChick, 0, 10),
            });

            var spiderDenSpawn = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.MiniSpider, 0, 4),
                new SpawnPointData( GameObjectType.Spider, 0, 10),
            });


            MapSegmentPointer road1;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalRoad, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, startPos,
                    Dir4.W, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                road1 = pointer;
            }

            MapSegmentPointer road2;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalRoad, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, road1.getEntranceSqPos(Dir4.E),
                    Dir4.W, 0, OpenAreaLockId);

                placeMonsterSpawns(pointer, monsterSpawn);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                road2 = pointer;
            }

            MapSegmentPointer road3;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.Normal, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, road2.getEntranceSqPos(Dir4.E),
                    Dir4.W, 0, OpenAreaLockId);

                placeMonsterSpawns(pointer, monsterSpawn);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                road3 = pointer;
            }

            MapSegmentPointer road4;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalRoad, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, road3.getEntranceSqPos(Dir4.E),
                    Dir4.W, 0, OpenAreaLockId);

                placeMonsterSpawns(pointer, monsterSpawn);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                road4 = pointer;
            }

            MapSegmentPointer minerAreaFork;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, road4.getEntranceSqPos(Dir4.E),
                    Dir4.W, 0, OpenAreaLockId);

                //placeMonsterSpawns(pointer, monsterSpawn);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                minerAreaFork = pointer;
            }
            

            MapSegmentPointer minerHome;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(24);
                var pointer = placeSegment(segment, minerAreaFork.getEntranceSqPos(Dir4.N), Dir4.S, 0, OpenAreaLockId);

                IntVector2 spawnPos = pointer.getEntranceSqPos(Dir4.N, 0);
                spawnPos.Y += 1;
                //levelEntrance = toWorldXZ(spawnPos);
                addSpawn(new SpawnPointDelegate(toWorldXZ(spawnPos), null, new SpawnPointData(GameObjectType.CheckPointNpc), 
                    SpawnImportance.Must_0, false));
                //spawnPos.Y -= 2;
                //teleport(spawnPos, TeleportLocationId.Lobby, VoxelModelName.DoorToLobby);


                //critter spawn 0, miner spawn 1
                GO.GameObjectType[] critterTypes = new GameObjectType[]
                {
                    GO.GameObjectType.CritterMiningPig,
                    GO.GameObjectType.CritterMiningCow,
                };
                int critterTypeIx = 0;

                foreach (var m in pointer.spawnPositions)
                {
                    if (m.square.specialIndex == 1)
                    {
                        placeMiningSpot(toCenterWorldXZ(m.position), true);
                    }
                    else
                    {
                        if (critterTypeIx < critterTypes.Length)
                        {
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null,//createCritter,
                                new SpawnPointData(critterTypes[critterTypeIx++]), SpawnImportance.Should_1, true, 3, false));
                        }
                    }
                }

                foreach (var m in pointer.terrainModels)
                {
                    switch (m.square.specialIndex)
                    {
                        case 0:
                            addModel(rnd.Chance(0.6) ? VoxelModelName.MinerTree1 : VoxelModelName.MinerTree2, 0,
                                m.position, IntVector3.NegativeY, true, true);
                            break;
                        case 1:
                            addModel(VoxelModelName.MinerHouse, 0, m.position, IntVector3.Zero, false, true);
                            break;
                        case 2:
                            addModel(VoxelModelName.minerFence, 0, m.position, IntVector3.NegativeY, false, false);
                            break;
                        case 3:
                            spiderBossWp = addModel(VoxelModelName.MinerMine, 0, m.position, IntVector3.Zero, false, true);
                            break;
                    }
                }

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.items[0].position), createEmoStartSuits,
                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                minerHome = pointer;
            }

            MapSegmentPointer secondAreaGuard;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, minerAreaFork.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, EmoForestModels, rnd);
                addModel(VoxelModelName.TrollWarningSign, 0, pointer.landMarkIx0, IntVector3.NegativeY, false, false);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createEmoLockGuard, SpawnPointData.Empty, SpawnImportance.Must_0, true));
                createLockSpawn(pointer, Dir4.S, createEmoLock, EmoFirstLockGroup);

                secondAreaGuard = pointer;
            }

            MapSegmentPointer largeCenterArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(18);
                var pointer = placeSegment(segment, secondAreaGuard.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

                //Forest trees: 0, Ruins: 1
                foreach (var m in pointer.terrainModels)
                {
                    switch (m.square.specialIndex)
                    {
                        case 0:
                            addTerrainModel(m.position, EmoForestModels, rnd);
                            break;
                        case 1:
                            addTerrainModel(m.position, EmoOpenAreaModels, rnd);
                            break;
                        case 2:

                            break;
                    }
                }

                //Boss: Special 0, mines: spec 1
                foreach (var m in pointer.specialPoints)
                {
                    if (m.square.specialIndex == 0)
                    {
                        spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), createEmoBoss,
                            SpawnPointData.Empty, SpawnImportance.Must_0, true));
                    }
                    else if (m.square.specialIndex == 1)
                    {
                        placeMiningSpot(toCenterWorldXZ(m.position), false);
                    }
                }

                foreach (var m in pointer.items)
                {
                    Rotation1D dir = Rotation1D.FromDegrees(45 + 90 * m.square.specialDir);

                    IntVector2 xz = toWorldXZ(m.position);
                    spawnPoints.Add(new SpawnPointDelegate(xz, null,
                        new SpawnPointData(GameObjectType.GoldChest, dir.ByteDir, 0), SpawnImportance.Must_0, true, 1, true));

                    xz.Y -= 2;
                    spawnPoints.Add(new SpawnPointDelegate(xz, createGoldChestGuard_emo,
                        SpawnPointData.Empty, SpawnImportance.HighSuggest_2, true, 1, true));

                }

                //Spawns
                placeMonsterSpawns(pointer, monsterSpawn, 0);
                placeMonsterSpawns(pointer, spiderDenSpawn, 1);
                placeMonsterSpawns(pointer, goblinsSpawn, 2);

                //LANDMARKS
                addModel(VoxelModelName.troll_stoneship, 0, pointer.landMarkIx0, IntVector3.NegativeY, true, true);
                addModel(VoxelModelName.troll_damagedtower, 0, pointer.landMarkIx1, IntVector3.NegativeY, true, true);
                addModel(VoxelModelName.Troll_SpiderCave, 0, pointer.landMarkIx2, IntVector3.NegativeY, true, true);

                largeCenterArea = pointer;
            }

            MapSegmentPointer granpaArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, SecondAreaLockId);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                   new SpawnPointData(GameObjectType.TrollTutorialGranpa), SpawnImportance.Must_0, true));

                addTerrainModels(pointer.terrainModels, EmoForestModels, rnd);

                granpaArea = pointer;
            }

            MapSegmentPointer salesmanArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(44);
                //new SegmentHeader(SegmentHeadType.NormalNpcRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

                IntVector2 salesmanPosXZ = toCenterWorldXZ(pointer.getSpecialPointIndex(0).position);
                spawnPoints.Add(new SpawnPointDelegate(salesmanPosXZ, null,
                  new SpawnPointData(GameObjectType.MinerSalesman), SpawnImportance.Must_0, true, 1, true));

                //salesmanPosXZ.X -= 8;
                placeMiningSpot(toCenterWorldXZ(pointer.getSpecialPointIndex(1).position), false);

                addTerrainModels(pointer.terrainModels, EmoForestModels, rnd);

                salesmanArea = pointer;
            }

            MapSegmentPointer smithArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.S, 1), Dir4.N, 0, SecondAreaLockId);

                IntVector2 smithPos = pointer.specialPoints[0].position;
                addModel(VoxelModelName.weaponsmith_station, 0, smithPos, IntVector3.NegativeY, true, true);

                smithPos.Y -= 1;

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(smithPos), null,
                   new SpawnPointData(GameObjectType.EmoSuitSmith), SpawnImportance.Must_0, true));

                addTerrainModels(pointer.terrainModels, EmoForestModels, rnd);

                smithArea = pointer;
            }
        }

        void horseArea(IntVector2 startPos)
        {
            MapSegmentPointer horseFarm;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 0, 1, 1, 0), rnd);
                var pointer = placeSegment(segment, startPos, Dir4.S, 0, OpenAreaLockId);

                int horseCount = 0;
                foreach (var m in pointer.spawnPositions)
                {
                    spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Horse),
                       horseCount == 0 ? SpawnImportance.Must_0 : SpawnImportance.HighSuggest_2, true));

                    if (++horseCount >= 3)
                    {
                        break;
                    }
                }

                horseFarm = pointer;
            }

            MapSegmentPointer ridingCorridor;
            {
                //:57 - spawn0 wolf, spawn1 goblin, spawn2 dogs
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(57);
                var pointer = placeSegment(segment, horseFarm.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

                foreach (var m in pointer.spawnPositions)
                {
                    switch (m.square.specialIndex)
                    {
                        case 0:
                            IntVector2 xz = toWorldXZ(m.position);
                            addSpawn(new SpawnPointDelegate(xz, null, new SpawnPointData(GameObjectType.GreatWolf),
                                SpawnImportance.HighSuggest_2, true), SleepingSpawnArg.ins);

                            xz.Y += rnd.LeftRight() * 4;
                            spawnPoints.Add(new SpawnPointDelegate(xz, null, new SpawnPointData(GameObjectType.GoblinBerserk),
                                SpawnImportance.HighSuggest_2, true));
                            break;
                        case 1:
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.GoblinScout),
                                SpawnImportance.LowSuggest_3, true));
                            break;
                        case 2:
                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Pitbull),
                                SpawnImportance.LowSuggest_3, true));
                            break;
                    }
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                ridingCorridor = pointer;
            }

            MapSegmentPointer firstGuardSegment;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 0, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, ridingCorridor.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                int guardCount = 0;
                foreach (var m in pointer.spawnPositions)
                {
                    if (guardCount < 2)
                    {
                        addSpawn(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.GoblinLineman),
                            SpawnImportance.Must_0, true, 1, true),

                            new LockGroupSpawnArgs(this, HorseFirstLockGroup));

                        guardCount++;
                    }
                    else
                    {
                        addSpawn(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Pitbull),
                            SpawnImportance.LowSuggest_3, true, 1, false));
                    }
                }

                createLockSpawn(pointer, Dir4.S, createHorseLock1, HorseFirstLockGroup);

                firstGuardSegment = pointer;
            }

            //58
            MapSegmentPointer threeCastlesArea;
            {

                var squigSpawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
                    new SpawnPointData( GameObjectType.SquigRedBaby, 0, 8),
                });
                squigSpawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(58);
                var pointer = placeSegment(segment, firstGuardSegment.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, HorseAreaId2);

                //spec 0 = castle, spec 1 = squigs
                foreach (var m in pointer.specialPoints)
                {
                    if (m.square.specialIndex == 0)
                    {
                        //Key = joint9
                        var modelWp = addModel(VoxelModelName.GoblinKeyGuard2, 0, m.position, IntVector3.Zero, true, true);

                        var route = jointsToPatrolroute(getModelJointsSorted(VoxelModelName.GoblinKeyGuard2, modelWp, true, new Range(0, 3)));

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
                    }
                    else
                    {
                        placeMonsterSpawn(m.position, squigSpawns);
                    }
                }

                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
                    new SpawnPointData( GameObjectType.GoblinBerserk, 0, 8),
                    new SpawnPointData( GameObjectType.Pitbull, 0, 15),

                });

                placeMonsterSpawns(pointer, spawns);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                createLockSpawn(pointer, Dir4.S, createHorseLock2, -1);

                threeCastlesArea = pointer;
            }

            MapSegmentPointer smallSquigArea;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 1, 1, 1), rnd);
                var pointer = placeSegment(segment, threeCastlesArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, HorseAreaId3);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
                    new SpawnPointData( GameObjectType.SquigGreen, 0, 8),
                });
                spawns.mix = true;

                placeMonsterSpawns(pointer, spawns);

                smallSquigArea = pointer;
            }

            MapSegmentPointer checkPointRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, smallSquigArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, HorseAreaId3);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                  new SpawnPointData(GameObjectType.CheckPointNpc), SpawnImportance.Must_0, false));

                checkPointRoom = pointer;
            }

            MapSegmentPointer salesmanRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 0, 0, 1), rnd);
                var pointer = placeSegment(segment, smallSquigArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, HorseAreaId3);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                  new SpawnPointData(GameObjectType.Salesman), SpawnImportance.Must_0, true, 1, true));

                salesmanRoom = pointer;
            }

            MapSegmentPointer squigBossRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalLarge, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, smallSquigArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, HorseAreaId3);
                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.SquigRed, 0, 10),
                    new SpawnPointData( GameObjectType.SquigRedBaby, 0, 5),
                    new SpawnPointData( GameObjectType.SquigGreen, 0, 8),
                    new SpawnPointData( GameObjectType.SquigGreenBaby, 0, 4),
                });
                spawns.mix = true;
                placeMonsterSpawns(pointer, spawns);

                IntVector2 southtExit = pointer.getEntranceSqPos(Dir4.S, 0);
                southtExit.Y -= 2;

                addSpawn(new SpawnPointDelegate(toWorldXZ(southtExit), null, new SpawnPointData(GameObjectType.SquigHorned),
                            SpawnImportance.Must_0, true, 3, true),

                            new LockGroupSpawnArgs(this, SquigBossLockGroup));

                squigBossRoom = pointer;

                createLockSpawn(pointer, Dir4.S, createHorseLock3, SquigBossLockGroup);
            }


            MapSegmentPointer horseFarm2;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, squigBossRoom.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, HorseAreaId4);

                int horseCount = 0;
                foreach (var m in pointer.spawnPositions)
                {
                    spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null, new SpawnPointData(GameObjectType.Horse),
                       horseCount == 0 ? SpawnImportance.Must_0 : SpawnImportance.HighSuggest_2, true));

                    if (++horseCount >= 3)
                    {
                        break;
                    }
                }

                horseFarm2 = pointer;
            }

            MapSegmentPointer bossRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, horseFarm2.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, HorseAreaId4);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createHorseBoss,
                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                bossRoom = pointer;
            }
        }

        void createHorseLock1(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, HorseAreaId2, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
        }

        void createHorseLock2(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, HorseAreaId3, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
                goArgs.characterLevel, null, VoxelModelName.threelock, Dir4.N);
        }

        void createHorseLock3(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, HorseAreaId4, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
        }

        protected void createHorseBoss(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var boss = new VikingEngine.LootFest.GO.Characters.GoblinWolfRiderBoss(goArgs, this);
        }

        MapSegmentPointer barrelBossRoom;

        void barrelsArea(IntVector2 startPos, out IntVector2 northExit)
        {
            MapSegmentPointer centerArea;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
                    new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
                    new SpawnPointData( GameObjectType.OrcKnight, 0, 10),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(82);
                var pointer = placeSegment(segment, startPos, Dir4.E, 0, OpenAreaLockId);

                foreach (var m in pointer.items)
                {
                    createKeySpawn(toWorldXZ(m.position), false);
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);
                createLockSpawn(pointer, Dir4.W, doorToCastle_barrels, -1);

                centerArea = pointer;
            }

            MapSegmentPointer bossGuard;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                    new SpawnPointData( GameObjectType.OrcKnight, 0, 15),
                });
                spawns.mix = true;


                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Castle, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, centerArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, BarrelsAreaId2);

                placeMonsterSpawns(pointer, spawns);
                createLockSpawn(pointer, Dir4.W, doorToBarrelBoss, -1);

                bossGuard = pointer;
            }

            {//BOSS
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, bossGuard.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, BarrelsAreaId3);

                barrelBossRoom = pointer;
            }

            northExit = centerArea.getEntranceSqPos(Dir4.N);
        }

        void doorToCastle_barrels(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, BarrelsAreaId2, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
                0, null, VoxelModelName.threelock, Dir4.E);
        }

        void doorToBarrelBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, BarrelsAreaId3, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
                0, startBarrelBossFight, VoxelModelName.threelock, Dir4.E);
        }

        void startBarrelBossFight()
        {
            var bossSpawnPos = new Map.WorldPosition(toWorldXZ(barrelBossRoom.spawnPositions[0].position), Map.WorldPosition.ChunkStandardHeight);
            new GO.Characters.Boss.StatueBoss(new GoArgs(bossSpawnPos, 1), this);
        }

        void farmVillageArea(IntVector2 startPos, out IntVector2 westExit)
        {
            MapSegmentPointer startArea;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.GoblinBerserk, 0, 10),
                    new SpawnPointData( GameObjectType.GoblinScout, 0, 20),
                    new SpawnPointData( GameObjectType.Hog, 0, 8),
                });

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(67);
                var pointer = placeSegment(segment, startPos, Dir4.N, 0, OpenAreaLockId);

                IntVector2 spawnPos = pointer.getEntranceSqPos(Dir4.N, 0);
                spawnPos.Y += 3;
                
                setTeleportLocation(TeleportLocationId.FarmEntrance, toWorldXZ(spawnPos));
                spawnPos.X += 2;

                addSpawn(new SpawnPointDelegate(toWorldXZ(spawnPos), null,
                    new SpawnPointData(GameObjectType.HorseTransport), SpawnImportance.Must_0, false));
                //teleport(spawnPos, TeleportLocationId.Lobby, VoxelModelName.DoorToLobby);

                placeMonsterSpawns(pointer, spawns);

                //Model0 house, Model1 pens
                foreach (var m in pointer.specialModels)
                {
                    if (m.square.specialIndex == 0)
                    {
                        addModel(VoxelModelName.farmhouse1, 0, m.position, IntVector3.NegativeY, false, true);
                    }
                    else if (m.square.specialIndex == 1)
                    {
                        addModel(VoxelModelName.FenceYardWide, 0, m.position, IntVector3.NegativeY, false, false);

                        addSpawn(new SpawnPointDelegate(toCenterWorldXZ(m.position), null,
                            new SpawnPointData(GameObjectType.CritterPig), SpawnImportance.LowSuggest_3, true, 3, false));
                    }
                }

                startArea = pointer;
            }

            IntVector2 mainDownpath;

            MapSegmentPointer village;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
                    new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
                    new SpawnPointData( GameObjectType.GoblinScout, 0, 8),
                    new SpawnPointData( GameObjectType.Hog, 0, 8),

                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(68);
                var pointer = placeSegment(segment, startArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

                foreach (var m in pointer.specialModels)
                {
                    if (m.square.specialIndex == 0)
                    {
                        addModel(VoxelModelName.FarmTownHouse1, 0, m.position, IntVector3.Zero, false, true);
                    }
                }

                //Spec0 = vakter, spec1 = healer(?)

                mainDownpath = pointer.getEntranceSqPos(Dir4.E, 0);
                placeUpDownPath(mainDownpath, TeleportLocationId.FarmToCaveMainPath, TeleportLocationId.CaveToFarmMainPath, Dir4.E, false);
                //secretDownPath = pointer.getEntranceSqPos(Dir4.E, 1);


                placeMonsterSpawns(pointer, spawns);

                village = pointer;
            }

            westExit = village.getEntranceSqPos(Dir4.W);
        }

        MapSegmentPointer lobbyTownArea(IntVector2 startPos)
        {
            var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                    new SpawnPointData( GameObjectType.SpitChick, 0, 10),
                });

            BlockMapSegment segment = LfRef.blockmaps.loadSegment(4);
            pointer = placeSegment(segment, startPos, OpenAreaLockId);

            levelEntrance = toWorldXZ(pointer.spawnPositions[0].position);



            //const byte HouseIx = 10, LampIx = 11;
            //wall 0:wall, 1:corner, 2:midtower, 3:gate
            foreach (var m in pointer.specialModels)
            {
                switch (m.square.specialIndex)
                {
                    default://0
                        addModel(VoxelModelName.townwall_wall, m.square.specialDir, m.position,
                            IntVector3.NegativeY, false, true);
                        break;
                    case 1:
                        addModel(VoxelModelName.townwall_corner, m.square.specialDir, m.position,
                            IntVector3.NegativeY, true, true);
                        break;
                    case 2:
                        addModel(VoxelModelName.townwall_walltower, m.square.specialDir, m.position,
                            IntVector3.NegativeY, true, true);
                        break;
                    case 3:
                        addModel(VoxelModelName.townwall_gate, m.square.specialDir, m.position,
                            IntVector3.NegativeY, true, true);
                        break;

                    case 10:
                        addModel(rnd.Chance(0.7) ? VoxelModelName.TownHouse2 : VoxelModelName.TownHouse3, 0,
                            m.position, IntVector3.NegativeY, false, true);
                        break;
                    case 11:
                        addModel(VoxelModelName.TownLamp, 0, m.position, IntVector3.NegativeY, false, true);
                        break;
                    case 12:
                        addModel(VoxelModelName.TownStatue, 0, m.position, IntVector3.NegativeY, true, true);
                        break;
                }


            }

            IntVector2 transportXZ = toCenterWorldXZ(pointer.specialPoints[0].position);


            addSpawn(new SpawnPointDelegate(transportXZ, null,
                new SpawnPointData(GameObjectType.HorseTransport), SpawnImportance.Must_0, false));

            addSpawn(new SpawnPointDelegate(VectorExt.AddX(transportXZ, 5), null,
                new SpawnPointData(GameObjectType.ProgressNPC), SpawnImportance.Must_0, false));

            transportXZ.Y += 6;
            setTeleportLocation(TeleportLocationId.Lobby, transportXZ);

            foreach (var m in pointer.spawnPositions)
            {
                if (m.square.specialIndex == 0)
                {
                    placeMonsterSpawn(m.position, spawns);
                }
                else
                {
                    spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), CreateNPC,
                        SpawnPointData.Empty, SpawnImportance.HighSuggest_2, true));
                }
            }

            IntVector2 bossEntrance = pointer.getEntranceSqPos(Dir4.W);
            teleport(bossEntrance, TeleportLocationId.BossCastle, Dir4.W, VoxelModelName.DoorToFinalLevel2);
            bossEntrance.X += 2;
            //LfLib.Location(TeleportLocationId.DoorToBossCastle).wp = new Map.WorldPosition(new Vector2toV3(toWorldXZ(bossEntrance)));
            setTeleportLocation(TeleportLocationId.DoorToBossCastle, toWorldXZ(bossEntrance));

            addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

            return pointer;
        }

        void mazeArea(IntVector2 entranceSqPos, out IntVector2 northExitSqPos, out IntVector2 southExitSqPos, out IntVector2 westExitSqPos)
        {
            var spawns = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.Hog, 0, 20),
                new SpawnPointData( GameObjectType.Pitbull, 0, 20),
                new SpawnPointData( GameObjectType.SpitChick, 0, 20),
                new SpawnPointData( GameObjectType.Scorpion, 0, 10),
                new SpawnPointData( GameObjectType.Spider, 0, 5),
                new SpawnPointData( GameObjectType.PoisionSpider, 0, 5),
                new SpawnPointData( GameObjectType.SquigGreen, 0, 3),
                new SpawnPointData( GameObjectType.SquigHorned, 0, 3),
                new SpawnPointData( GameObjectType.SquigRed, 0, 3),
                //new SpawnPointData( GameObjectType.GreenSlime, 0, 10),
                new SpawnPointData( GameObjectType.Beetle1, 0, 5),
            });

            //maze start 103, end: 104
            MapSegmentPointer northEntrance;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(105);
                var pointer = placeSegment(segment, entranceSqPos, Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                northEntrance = pointer;
            }

            MapSegmentPointer midTop;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 1, 1, 1, 0), rnd);
                var pointer = placeSegment(segment, northEntrance.getEntranceSqPos(Dir4.S, 0),
                    Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                midTop = pointer;
            }

            MapSegmentPointer rightTop;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 1, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, midTop.getEntranceSqPos(Dir4.E, 0),
                    Dir4.W, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                rightTop = pointer;
            }
            northExitSqPos = rightTop.getEntranceSqPos(Dir4.N);

            MapSegmentPointer rightBottom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, rightTop.getEntranceSqPos(Dir4.S, 0),
                    Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                rightBottom = pointer;
            }

            MapSegmentPointer midBottom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 1, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, midTop.getEntranceSqPos(Dir4.S, 0),
                    Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                midBottom = pointer;
            }

            MapSegmentPointer leftBottom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 1, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, midBottom.getEntranceSqPos(Dir4.W, 0),
                    Dir4.E, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                leftBottom = pointer;
            }

            MapSegmentPointer leftTop;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalMazeSegment, 0, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, leftBottom.getEntranceSqPos(Dir4.N, 0),
                    Dir4.S, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                leftTop = pointer;
            }

            westExitSqPos = leftTop.getEntranceSqPos(Dir4.W);

            MapSegmentPointer southEntrance;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(104);
                var pointer = placeSegment(segment, midBottom.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                southEntrance = pointer;
            }

            southExitSqPos = southEntrance.getEntranceSqPos(Dir4.S);
        }

        void goblinsArea(IntVector2 entranceSqPos)
        {
            MapSegmentPointer goblinGuardWalls;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(92);
                var pointer = placeSegment(segment, entranceSqPos, Dir4.W, 0, OpenAreaLockId);

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

                goblinGuardWalls = pointer;
            }
            MapSegmentPointer goblinCheckPoint;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                   new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, goblinGuardWalls.getEntranceSqPos(Dir4.N, 0),
                    Dir4.S, 0, OpenAreaLockId);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                    new SpawnPointData(GameObjectType.CheckPointNpc), SpawnImportance.Must_0, false));

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);

                goblinCheckPoint = pointer;
            }

            MapSegmentPointer dogPark;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Pitbull, 0, 10),
                    new SpawnPointData( GameObjectType.SpitChick, 0, 6),
                });

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 0, 1, 1, 1), rnd);
                var pointer = placeSegment(segment, goblinGuardWalls.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, OpenAreaLockId);

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                addModel(VoxelModelName.GoblinDogHouse1, 0, pointer.landMarkIx0, IntVector3.NegativeY, false, false);
                placeMonsterSpawns(pointer, spawns);


                createLockSpawn(pointer, Dir4.E, doorToGoblinBoss, -1);

                dogPark = pointer;
            }

            MapSegmentPointer entranceKeyRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, dogPark.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

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
                BlockMapSegment segment = LfRef.blockmaps.loadSegment(93);
                var pointer = placeSegment(segment, dogPark.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, GoblinsBossAreaLockId);

                //joints
                //0: boss
                //1: ground troops
                //2: tower troops
                var fortWp = addModel(VoxelModelName.GoblinEndFortress1, 0, pointer.specialPoints[0].position, IntVector3.NegativeY, true, true);

                getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 0);
                spawnPoints.Add(new SpawnPointDelegate(joints[0].wp, createGoblinBoss, SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 1);
                foreach (var m in joints)
                {
                    spawnPoints.Add(new SpawnPointDelegate(m.wp, null, new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.HighSuggest_2, true, 1, true));
                }

                getModelJoints(VoxelModelName.GoblinEndFortress1, fortWp, true, 2);
                foreach (var m in joints)
                {
                    spawnPoints.Add(new SpawnPointDelegate(m.wp, null, new SpawnPointData(GameObjectType.GoblinScout), SpawnImportance.LowSuggest_3, true, 1, true));
                }

                addTerrainModels(pointer.terrainModels, NormalDefaultModels, rnd);
                endFortress = pointer;
            }
        }//END GoblinsArea

        void CreateNPC(GoArgs goArgs)
        {
            if (PlatformSettings.ReleaseBuild || DebugSett.SpawnNPCs)
            {
                new VikingEngine.LootFest.GO.NPC.BasicNPC(goArgs);
            }
        }

        void createGobinFirstLockGuard(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 2; ++i)
            {
                GO.Characters.AbsMonster3 guard = new VikingEngine.LootFest.GO.Characters.GoblinScout(goArgs);

                guard.GoToSleep();
                guard.levelCollider.SetLockedToArea();
                guard.managedGameObject = true;

                AddLockConnectedGo(guard, GoblinsFirstLockGroup); //Borde lägga in restriction till segment

                goArgs.startWp.X += 6;
            }
        }

        void doorToGoblinBoss(VikingEngine.LootFest.GO.GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, GoblinsBossAreaLockId, GO.EnvironmentObj.AreaUnLockType.Key,
                goArgs.characterLevel, null, VoxelModelName.locklvl2, Dir4.W);
        }

        void createGoblinBoss(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            GO.Characters.AbsCharacter boss = new VikingEngine.LootFest.GO.Characters.Boss.GoblinKing(goArgs, this);
            boss.managedGameObject = true;

            //bossSpawnPos = goArgs.startWp;
        }

        void createEmoStartSuits(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            new VikingEngine.LootFest.GO.PickUp.SuitBox(goArgs, SuitType.Swordsman);
            goArgs.startWp.WorldGrindex.Z += 4;
            goArgs.updatePosV3();
            new VikingEngine.LootFest.GO.PickUp.SuitBox(goArgs, SuitType.Archer);
        }

        void createEmoLockGuard(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            goArgs.startWp.X -= 2;

            for (int i = 0; i < 3; ++i)
            {
                GO.Characters.AbsCharacter guard = new VikingEngine.LootFest.GO.Characters.OrcSoldier(goArgs);

                guard.levelCollider.SetLockedToArea();
                guard.managedGameObject = true;

                AddLockConnectedGo(guard, EmoFirstLockGroup); //Borde lägga in restriction till segment

                goArgs.startWp.X += 6;
            }
        }

        void createEmoLock(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
        }

        void createEmoBoss(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            GO.Characters.AbsCharacter boss = new VikingEngine.LootFest.GO.Characters.Boss.TrollBoss(goArgs, this);
            boss.managedGameObject = true;

            //bossSpawnPos = goArgs.startWp;
        }

        void createGoldChestGuard_emo(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            VikingEngine.LootFest.GO.Characters.OrcKnight guard = new VikingEngine.LootFest.GO.Characters.OrcKnight(goArgs);
            guard.SetPatrolRoute(new GO.Characters.AI.PatrolRoute(
                new List<Microsoft.Xna.Framework.Vector3>
                {
                    goArgs.startPos, goArgs.startPos + Vector3.UnitZ * -14f
                },
                false, 1));
            guard.SetAsManaged();
        }

        void placeMiningSpot(IntVector2 xz, bool minerNPC)
        {
            // mining_pileground
            addModelOnWorldXZ(VoxelModelName.mining_pileground, 0, xz, IntVector3.NegativeY, true, true);

            var spawn = new SpawnPointDelegate(xz, createMiningSpot,
                new SpawnPointData(GameObjectType.MiningSpot, lib.BoolToInt01(minerNPC)), SpawnImportance.Must_0, true, 1, true);

            spawn.earMark = true;

            spawnPoints.Add(spawn);
        }

        void createMiningSpot(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var miningSpot = new VikingEngine.LootFest.GO.EnvironmentObj.MiningSpot(goArgs);
            miningSpot.SetAsManaged();

            if (lib.ToBool(goArgs.characterLevel))
            {
                goArgs.startWp.WorldGrindex.X -= 3;
                goArgs.updatePosV3();
                var miner = new VikingEngine.LootFest.GO.NPC.Miner(goArgs);
                miner.SetAsManaged();
                miner.miningSpot = miningSpot;
            }
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            var result = new List<VoxelModelName>
                {
                    VoxelModelName.TownHouse2,
                    VoxelModelName.TownHouse3,

                    VoxelModelName.TownStatue,
                    VoxelModelName.TownLamp,

                    VoxelModelName.townwall_corner,
                    VoxelModelName.townwall_gate,
                    VoxelModelName.townwall_wall,
                    VoxelModelName.townwall_walltower,
                    //VoxelModelName.DoorToLevelGoblins,
                    //VoxelModelName.DoorToLevelMount,
                    //VoxelModelName.DoorToLevelTroll,
                    //VoxelModelName.DoorToLevelWolf,
                    //VoxelModelName.DoorToLevelStatue,
                    //VoxelModelName.DoorToLevelBird,
                    //VoxelModelName.DoorToLevelSwine,
                    //VoxelModelName.DoorToLevelOrcs,
                    //VoxelModelName.DoorToLevelElf,
                    //VoxelModelName.DoorToLevelSkeletonDungeon,
                    //VoxelModelName.DoorToLevelSpider,
                    VoxelModelName.DoorToFinalLevel2,
                    //VoxelModelName.DoorToChallengeZombies,
                    //VoxelModelName.DoorToChallengeFuture,

                    //VoxelModelName.DoorToLobby,
                    VoxelModelName.GoblinGate1,
                    VoxelModelName.GoblinGate2,
                    VoxelModelName.GoblinPatrolWall1,
                    VoxelModelName.GoblinFortress1,
                    VoxelModelName.GoblinEndFortress1,
                    VoxelModelName.GoblinTent,
                    VoxelModelName.GoblinKeyGuard1,
                    VoxelModelName.GoblinKeyGuard2,
                    VoxelModelName.GoblinPost1,
                    VoxelModelName.goblin_hut,
                    VoxelModelName.GoblinDogHouse1,
                
                    VoxelModelName.weaponsmith_station,
                    VoxelModelName.MinerHouse,
                    VoxelModelName.MinerMine,
                    VoxelModelName.MinerTree1,
                    VoxelModelName.MinerTree2,
                    VoxelModelName.minerFence,
                    VoxelModelName.TrollWarningSign,

                    VoxelModelName.ForestTree1,
                    VoxelModelName.ForestTree2,
                    VoxelModelName.ForestTree3,
                    VoxelModelName.ForestTree4,
                    VoxelModelName.ForestTree5,

                    VoxelModelName.ForestStone1,
                    VoxelModelName.ForestStone2,
                    VoxelModelName.ForestStone3,

                    VoxelModelName.ForestBurnedTree1,
                    VoxelModelName.ForestBurnedTree2,
                    VoxelModelName.TrollRuin1,
                    VoxelModelName.TrollRuin2,
                    VoxelModelName.TrollRuin3,
                    VoxelModelName.troll_damagedtower,
                    VoxelModelName.Troll_SpiderCave,
                    VoxelModelName.troll_stoneship,
                    VoxelModelName.mining_pileground,

                    //VoxelModelName.DoorToLobby,
                    VoxelModelName.uppath,
                    VoxelModelName.downpath,
                    VoxelModelName.farmhouse1,
                    VoxelModelName.FarmTownHouse1,
                    VoxelModelName.FenceYardWide,

                };
            result.AddRange(NormalDefaultModels);

            return result;
        }
    }
}
