//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.GO;
//using VikingEngine.LootFest.Map.Terrain;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest.BlockMap.Level
//{
//    class EmoVsTroll : AbsLevel
//    {
//        static readonly VoxelModelName[] ForestModels = new VoxelModelName[]
//        { 
//            VoxelModelName.ForestTree1,
//            VoxelModelName.ForestTree2,
//            VoxelModelName.ForestTree3,
//            VoxelModelName.ForestTree4,
//            VoxelModelName.ForestTree5,

//            VoxelModelName.ForestStone1,
//            VoxelModelName.ForestStone2,
//            VoxelModelName.ForestStone3,
//        };

//        static readonly VoxelModelName[] OpenAreaModels = new VoxelModelName[]
//        { 
//            VoxelModelName.ForestTree1,
//            VoxelModelName.ForestTree2,
//            VoxelModelName.ForestStone1,

//            VoxelModelName.ForestBurnedTree1,
//            VoxelModelName.ForestBurnedTree2,
//            VoxelModelName.TrollRuin1,
//            VoxelModelName.TrollRuin2,
//            VoxelModelName.TrollRuin3,
//        };

//        const int LockGroup = 1;

//        public static Map.WorldPosition spiderBossWp;

//        public EmoVsTroll()
//            : base(LevelEnum.EmoVsTroll)
//        {
//            collect = new CollectItem(VikingEngine.LootFest.GO.NPC.EmoSuitSmith.CraftingIngotCount, SpriteName.LfMithrilIngot);

//            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };
//        }

//        protected override void generateMapAsynch()
//        {
//            IntVector2 startpos = standardStartPos();
            
//            var goblinsSpawn = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.GoblinScout, 0, 10),
//                new SpawnPointData( GameObjectType.Pitbull, 0, 10),
//                new SpawnPointData( GameObjectType.GoblinLineman, 0, 15),
//                new SpawnPointData( GameObjectType.GoblinBerserk, 0, 4),
//            });

//            var monsterSpawn = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.Hog, 0, 10),
//                new SpawnPointData( GameObjectType.Pitbull, 0, 5),
//                new SpawnPointData( GameObjectType.SpitChick, 0, 10),
//            });

//            var spiderDenSpawn = new SuggestedSpawns(new List<SpawnPointData>
//            {
//                new SpawnPointData( GameObjectType.MiniSpider, 0, 4),
//                new SpawnPointData( GameObjectType.Spider, 0, 10),
//            });

//            MapSegmentPointer minerHome;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(24);
//                var pointer = placeSegment(segment, startpos, OpenAreaLockId);

//                IntVector2 spawnPos = pointer.getEntranceSqPos(Dir4.N, 0);
//                spawnPos.Y += 1;
//                levelEntrance = toWorldXZ(spawnPos);

//                spawnPos.Y -= 2;
//                teleport(spawnPos, TeleportLocationId.Lobby, VoxelModelName.DoorToLobby);

                
//                //critter spawn 0, miner spawn 1
//                GO.GameObjectType[] critterTypes = new GameObjectType[]
//                {
//                    GO.GameObjectType.CritterMiningPig,
//                    GO.GameObjectType.CritterMiningCow,
//                };
//                int critterTypeIx = 0;

//                foreach (var m in pointer.spawnPositions)
//                {
//                    if (m.square.specialIndex == 1)
//                    {
//                        placeMiningSpot(toCenterWorldXZ(m.position), true);
//                    }
//                    else
//                    {
//                        if (critterTypeIx < critterTypes.Length)
//                        {
//                            spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), null,//createCritter,
//                                new SpawnPointData(critterTypes[critterTypeIx++]), SpawnImportance.Should_1, true, 3, false));
//                        }
//                    }
//                }

//                foreach (var m in pointer.terrainModels)
//                {
//                    switch (m.square.specialIndex)
//                    {
//                        case 0:
//                            addModel(rnd.RandomChance(0.6)? VoxelModelName.MinerTree1 : VoxelModelName.MinerTree2, 0, 
//                                m.position, IntVector3.NegativeY, true, true);
//                            break;
//                        case 1:
//                            addModel(VoxelModelName.MinerHouse, 0, m.position, IntVector3.Zero, false, true);
//                            break;
//                        case 2:
//                            addModel(VoxelModelName.minerFence, 0, m.position, IntVector3.NegativeY, false, false);
//                            break;
//                        case 3:
//                            spiderBossWp = addModel(VoxelModelName.MinerMine, 0, m.position, IntVector3.Zero, false, true);
//                            break;
//                    }
//                }

//                spawnPoints.Add( new SpawnPointDelegate(toWorldXZ( pointer.items[0].position), createSuits,
//                    SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

//                minerHome = pointer;
//            }

//            MapSegmentPointer secondAreaGuard;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
//                var pointer = placeSegment(segment, minerHome.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
//                addTerrainModels(pointer.terrainModels, ForestModels, rnd);
//                addModel(VoxelModelName.TrollWarningSign, 0, pointer.landMarkIx0, IntVector3.NegativeY, false, false);

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ( pointer.spawnPositions[0].position), createLockGuard, SpawnPointData.Empty, SpawnImportance.Must_0, true));
//                createLockSpawn(pointer, Dir4.S, createLock, LockGroup);

//                secondAreaGuard = pointer;
//            }

//            MapSegmentPointer largeCenterArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(18);
//                var pointer = placeSegment(segment, secondAreaGuard.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);
                
//                //Forest trees: 0, Ruins: 1
//                foreach (var m in pointer.terrainModels)
//                {
//                    switch (m.square.specialIndex)
//                    {
//                        case 0:
//                            addTerrainModel(m.position, ForestModels, rnd);
//                            break;
//                        case 1:
//                            addTerrainModel(m.position, OpenAreaModels, rnd);
//                            break;
//                        case 2:

//                            break;
//                    }
//                }

//                //Boss: Special 0, mines: spec 1
//                foreach (var m in pointer.specialPoints)
//                {
//                    if (m.square.specialIndex == 0)
//                    {
//                        spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(m.position), createBoss,
//                            SpawnPointData.Empty, SpawnImportance.Must_0, true));
//                    }
//                    else if (m.square.specialIndex == 1)
//                    {
//                        placeMiningSpot(toCenterWorldXZ(m.position), false);
//                    }
//                }

//                foreach (var m in pointer.items)
//                {
//                    Rotation1D dir = Rotation1D.FromDegrees(45 + 90 * m.square.specialDir);

//                    IntVector2 xz = toWorldXZ(m.position);
//                    spawnPoints.Add(new SpawnPointDelegate(xz, null,
//                        new SpawnPointData(GameObjectType.GoldChest, dir.ByteDir, 0), SpawnImportance.Must_0, true, 1, true));

//                    xz.Y -= 2;
//                    spawnPoints.Add(new SpawnPointDelegate(xz, createGoldChestGuard,
//                        SpawnPointData.Empty, SpawnImportance.HighSuggest_2, true, 1, true));

//                }

//                //Spawns
//                placeMonsterSpawns(pointer, monsterSpawn, 0);
//                placeMonsterSpawns(pointer, spiderDenSpawn, 1);
//                placeMonsterSpawns(pointer, goblinsSpawn, 2);

//                //LANDMARKS
//                addModel(VoxelModelName.troll_stoneship, 0, pointer.landMarkIx0, IntVector3.NegativeY, true, true);
//                addModel(VoxelModelName.troll_damagedtower, 0, pointer.landMarkIx1, IntVector3.NegativeY, true, true);
//                addModel(VoxelModelName.Troll_SpiderCave, 0, pointer.landMarkIx2, IntVector3.NegativeY, true, true);

//                largeCenterArea = pointer;
//            }

//            MapSegmentPointer granpaArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 1, 0, 0), rnd);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, SecondAreaLockId);

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
//                   new SpawnPointData(GameObjectType.TrollTutorialGranpa), SpawnImportance.Must_0, true));

//                addTerrainModels(pointer.terrainModels, ForestModels, rnd);

//                granpaArea = pointer;
//            }

//            MapSegmentPointer salesmanArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadSegment(44);
//                    //new SegmentHeader(SegmentHeadType.NormalNpcRoom, 1, 0, 0, 0), rnd);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, SecondAreaLockId);

//                IntVector2 salesmanPosXZ = toCenterWorldXZ(pointer.getSpecialPointIndex(0).position);
//                spawnPoints.Add(new SpawnPointDelegate(salesmanPosXZ, null,
//                  new SpawnPointData(GameObjectType.MinerSalesman), SpawnImportance.Must_0, true, 1, true));

//                //salesmanPosXZ.X -= 8;
//                placeMiningSpot(toCenterWorldXZ(pointer.getSpecialPointIndex(1).position), false);

//                addTerrainModels(pointer.terrainModels, ForestModels, rnd);

//                salesmanArea = pointer;
//            }

//            MapSegmentPointer smithArea;
//            {
//                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
//                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 1, 0, 0, 0), rnd);
//                var pointer = placeSegment(segment, largeCenterArea.getEntranceSqPos(Dir4.S, 1), Dir4.N, 0, SecondAreaLockId);

//                IntVector2 smithPos = pointer.specialPoints[0].position;
//                addModel(VoxelModelName.weaponsmith_station, 0, smithPos, IntVector3.NegativeY, true, true);

//                smithPos.Y -= 1;

//                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(smithPos), null,
//                   new SpawnPointData(GameObjectType.EmoSuitSmith), SpawnImportance.Must_0, true));

//                addTerrainModels(pointer.terrainModels, ForestModels, rnd);

//                smithArea = pointer;
//            }

//           // new Timer.Action0ArgTrigger(createDebugTexture);
//        }

//        protected override List<VoxelModelName> loadTerrainModelsList()
//        {
//            var result = new List<VoxelModelName>
//                {
//                    VoxelModelName.DoorToLobby,
//                    VoxelModelName.weaponsmith_station,
//                    VoxelModelName.MinerHouse,
//                    VoxelModelName.MinerMine,
//                    VoxelModelName.MinerTree1,
//                    VoxelModelName.MinerTree2,
//                    VoxelModelName.minerFence,
//                    VoxelModelName.TrollWarningSign,

//                    VoxelModelName.ForestTree1,
//                    VoxelModelName.ForestTree2,
//                    VoxelModelName.ForestTree3,
//                    VoxelModelName.ForestTree4,
//                    VoxelModelName.ForestTree5,

//                    VoxelModelName.ForestStone1,
//                    VoxelModelName.ForestStone2,
//                    VoxelModelName.ForestStone3,

//                    VoxelModelName.ForestBurnedTree1,
//                    VoxelModelName.ForestBurnedTree2,
//                    VoxelModelName.TrollRuin1,
//                    VoxelModelName.TrollRuin2,
//                    VoxelModelName.TrollRuin3,
//                    VoxelModelName.troll_damagedtower,
//                    VoxelModelName.Troll_SpiderCave,
//                    VoxelModelName.troll_stoneship,
//                    VoxelModelName.mining_pileground,
//                };
//            return result;
//        }

        

//        void createSuits(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            new VikingEngine.LootFest.GO.PickUp.SuitBox(goArgs, SuitType.Swordsman);
//            goArgs.startWp.WorldGrindex.Z += 4;
//            goArgs.updatePosV3();
//            new VikingEngine.LootFest.GO.PickUp.SuitBox(goArgs, SuitType.Archer);
//        }

//        void createLockGuard(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            goArgs.startWp.X -= 2;

//            for (int i = 0; i < 3; ++i)
//            {
//                GO.Characters.AbsCharacter guard = new VikingEngine.LootFest.GO.Characters.OrcSoldier(goArgs);

//                guard.levelCollider.SetLockedToArea();
//                guard.managedGameObject = true;

//                AddLockConnectedGo(guard, LockGroup); //Borde lägga in restriction till segment

//                goArgs.startWp.X += 6;
//            }
//        }

//        void createLock(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, SecondAreaLockId, GO.EnvironmentObj.AreaUnLockType.ConnectedEnemies,
//                goArgs.characterLevel, null, VoxelModelName.groupLock, Dir4.N);
//        }

//        void createBoss(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            GO.Characters.AbsCharacter boss = new VikingEngine.LootFest.GO.Characters.Boss.TrollBoss(goArgs, this);
//            boss.managedGameObject = true;

//            //bossSpawnPos = goArgs.startWp;
//        }

//        void createGoldChestGuard(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;

//            VikingEngine.LootFest.GO.Characters.OrcKnight guard = new VikingEngine.LootFest.GO.Characters.OrcKnight(goArgs);
//            guard.SetPatrolRoute(new GO.Characters.AI.PatrolRoute(
//                new List<Microsoft.Xna.Framework.Vector3>
//                {
//                    goArgs.startPos, goArgs.startPos + Vector3.UnitZ * -14f
//                }, 
//                false, 1));
//            guard.SetAsManaged();
//        }

//        void placeMiningSpot(IntVector2 xz, bool minerNPC)
//        {
//           // mining_pileground
//            addModelOnWorldXZ(VoxelModelName.mining_pileground, 0, xz, IntVector3.NegativeY, true, true);

//            spawnPoints.Add(new SpawnPointDelegate(xz, createMiningSpot,
//                new SpawnPointData( GameObjectType.NUM_NON, lib.BoolToInt01(minerNPC)), SpawnImportance.Must_0, true, 1, true));
//        }

//        void createMiningSpot(GoArgs goArgs)
//        {
//            goArgs.fromSpawn.spawnLock = 1;
         
//            var miningSpot = new VikingEngine.LootFest.GO.EnvironmentObj.MiningSpot(goArgs);
//            miningSpot.SetAsManaged();

//            if (lib.ToBool(goArgs.characterLevel))
//            {
//                goArgs.startWp.WorldGrindex.X -= 3;
//                goArgs.updatePosV3();
//                var miner = new VikingEngine.LootFest.GO.NPC.Miner(goArgs);
//                miner.SetAsManaged();
//                miner.miningSpot = miningSpot;
//            }
//        }

//        //void createMinerNPC(GoArgs goArgs)
//        //{
//        //    goArgs.fromSpawn.spawnLock = 1;
            

//        //    goArgs.startWp.WorldGrindex.X += 3;
//        //    miner.miningSpot = new VikingEngine.LootFest.GO.EnvironmentObj.MiningSpot(goArgs);
//        //    miner.miningSpot.SetAsManaged();
//        //}
//    }
//}
