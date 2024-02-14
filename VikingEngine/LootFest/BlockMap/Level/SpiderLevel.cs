using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class SpiderLevel : AbsLevel
    {
        /*
         * TODO
         * -Blanda terräng typer, ovan/under mark
         * -Teleport trappor
         * -Checkpoint npc, sover och vaknar med flagga
         * -Kistor med nycklar
         * -Automatiskt tracka gångar mellan rum
         * (healer npc)
         */

        public SpiderLevel()
            : base(LevelEnum.Spider1)
        {
            terrain = new AbsLevelTerrain[] { new CaveTerrain(this) };
        }

        protected override void generateMapAsynch()
        {
            IntVector2 startpos = startPos(new Vector2(0.1f, 0.05f));
            //IntVector2 mainDownpath, secretDownPath;

            //loadedModels[VoxelModelName.uppath].Rotate(3, true);
            //loadedModels[VoxelModelName.downpath].Rotate(1, true);


            //MapSegmentPointer startArea;
            //{
            //    var spawns = new SuggestedSpawns(new List<SpawnPointData>
            //    {
            //        new SpawnPointData( GameObjectType.GoblinBerserk, 0, 10),
            //        new SpawnPointData( GameObjectType.GoblinScout, 0, 20),
            //        new SpawnPointData( GameObjectType.Hog, 0, 8),
            //    });

            //    BlockMapSegment segment = LfRef.blockmaps.loadSegment(67);
            //    var pointer = placeSegment(segment, startpos, OpenAreaLockId);

            //    IntVector2 spawnPos = pointer.getEntranceSqPos(Dir4.N, 0);
            //    spawnPos.Y += 1;
            //    levelEntrance = toWorldXZ(spawnPos);

            //    spawnPos.Y -= 2;
            //    teleport(spawnPos, TeleportLocationId.Lobby, VoxelModelName.DoorToLobby);

            //    placeMonsterSpawns(pointer, spawns);

            //    //Model0 house, Model1 pens
            //    foreach (var m in pointer.specialModels)
            //    {
            //        if (m.square.specialIndex == 0)
            //        {
            //            addModel(VoxelModelName.farmhouse1, 0, m.position, IntVector3.NegativeY, false, true);
            //        }
            //        else if (m.square.specialIndex == 1)
            //        {
            //            addModel(VoxelModelName.FenceYardWide, 0, m.position, IntVector3.NegativeY, false, false);

            //            addSpawn(new SpawnPointDelegate(toCenterWorldXZ(m.position), null, 
            //                new SpawnPointData( GameObjectType.CritterPig), SpawnImportance.LowSuggest_3, true, 3, false));
            //        }
            //    }

            //    startArea = pointer;
            //}

            //MapSegmentPointer village;
            //{
            //    var spawns = new SuggestedSpawns(new List<SpawnPointData>
            //    {
            //        new SpawnPointData( GameObjectType.OrcSoldier, 0, 10),
            //        new SpawnPointData( GameObjectType.OrcArcher, 0, 10),
            //        new SpawnPointData( GameObjectType.GoblinScout, 0, 8),
            //        new SpawnPointData( GameObjectType.Hog, 0, 8),

            //    });
            //    spawns.mix = true;

            //    BlockMapSegment segment = LfRef.blockmaps.loadSegment(68);
            //    var pointer = placeSegment(segment, startArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);

            //    foreach (var m in pointer.specialModels)
            //    {
            //        if (m.square.specialIndex == 0)
            //        {
            //            addModel(VoxelModelName.FarmTownHouse1, 0, m.position, IntVector3.Zero, false, true);
            //        }
            //    }

            //    //Spec0 = vakter, spec1 = healer(?)

            //    mainDownpath = pointer.getEntranceSqPos(Dir4.E, 0);
            //    secretDownPath = pointer.getEntranceSqPos(Dir4.E, 1);


            //    placeMonsterSpawns(pointer, spawns);

            //    village = pointer;
            //}

            //currentTerrainIndex = 1;

            //CAVE AREA
            IntVector2 caveStartpos = startPos(new Vector2(0.5f, 0.45f));

            IntVector2 terrainStart = new IntVector2(0, caveStartpos.Y - 20);
            placeTerrainArea(Rectangle2.FromTwoPoints(terrainStart, squares.Size - 1), currentTerrainIndex);

            MapSegmentPointer caveEntranceRoad;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.MiniSpider, 0, 20),
                    new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(69);
                var pointer = placeSegment(segment, caveStartpos, Dir4.W, 0, OpenAreaLockId);

                placeMonsterSpawns(pointer, spawns);

                caveEntranceRoad = pointer;

                var uppath = pointer.getEntranceSqPos(Dir4.W, 0);
                placeUpDownPath(uppath, TeleportLocationId.CaveToFarmMainPath, TeleportLocationId.FarmToCaveMainPath, Dir4.W, true);
                //placeDownPath(mainDownpath, uppath, Dir4.E);
            }

            MapSegmentPointer tempFillerRoom;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Spider, 0, 10),
                    new SpawnPointData( GameObjectType.MiniSpider, 0, 20),
                    new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 1, 0), rnd);
                var pointer = placeSegment(segment, caveEntranceRoad.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);

                placeMonsterSpawns(pointer, spawns);

                tempFillerRoom = pointer;
            }

            MapSegmentPointer caveCenterArea;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Spider, 0, 15),
                    new SpawnPointData( GameObjectType.MiniSpider, 0, 10),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadSegment(70);
                var pointer = placeSegment(segment, tempFillerRoom.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                createLockSpawn(pointer, Dir4.E, createBossLock, -1);

                caveCenterArea = pointer;
            }

            MapSegmentPointer salesmanRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalNpcRoom, 0, 0, 1, 1), rnd);
                var pointer = placeSegment(segment, caveCenterArea.getEntranceSqPos(Dir4.N, 1), Dir4.S, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);

                spawnPoints.Add(new SpawnPointDelegate(toWorldXZ(pointer.specialPoints[0].position), null,
                  new SpawnPointData(GameObjectType.Salesman), SpawnImportance.Must_0, true, 1, true));

                var uppath = pointer.getEntranceSqPos(Dir4.W, 0);
                uppath.X += 1;
               // placeDownPath(secretDownPath, uppath, Dir4.E);

                salesmanRoom = pointer;
            }

            //NORTH ENTRANCES

            MapSegmentPointer batCave;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Bat, 0, 10),
                    new SpawnPointData( GameObjectType.Bat, 1, 4),
                    new SpawnPointData( GameObjectType.Skeleton, 0, 12),

                });
                spawns.mix = false;

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, caveCenterArea.getEntranceSqPos(Dir4.S, 0), Dir4.N, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                createKeySpawn(toWorldXZ(pointer.landMarkIx0), false);

                batCave = pointer;
            }

            MapSegmentPointer miniBoss;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.NormalGuardRoom, 1, 0, 0, 0), rnd);
                var pointer = placeSegment(segment, caveCenterArea.getEntranceSqPos(Dir4.S, 1), Dir4.N, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), null,
                    new SpawnPointData(GameObjectType.GoblinSpiderRiderMiniBoss), SpawnImportance.HighSuggest_2, true, 1, true));

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[1].position), null,
                    new SpawnPointData(GameObjectType.GoblinLineman), SpawnImportance.HighSuggest_2, true, 2, false));

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[2].position), null,
                    new SpawnPointData(GameObjectType.Spider), SpawnImportance.HighSuggest_2, true, 2, false));

                createKeySpawn(toWorldXZ(pointer.landMarkIx0), false);

                miniBoss = pointer;
            }

            //WEST
            MapSegmentPointer bigSpiderEntrance;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Spider, 0, 10),
                    new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
                    new SpawnPointData( GameObjectType.BullSpider, 0, 10),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 0, 1, 0, 1), rnd);
                var pointer = placeSegment(segment, caveCenterArea.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                bigSpiderEntrance = pointer;
            }

            MapSegmentPointer bigSpiderArea;
            {
                var spawns = new SuggestedSpawns(new List<SpawnPointData>
                {
                    new SpawnPointData( GameObjectType.Spider, 0, 10),
                    new SpawnPointData( GameObjectType.PoisionSpider, 0, 10),
                    new SpawnPointData( GameObjectType.BullSpider, 0, 30),
                });
                spawns.mix = true;

                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.Normal, 0, 1, 0, 0), rnd);
                var pointer = placeSegment(segment, bigSpiderEntrance.getEntranceSqPos(Dir4.W, 0), Dir4.E, 0, OpenAreaLockId);
                addTerrainModels(pointer.terrainModels, DefaultDungeonModels, rnd);
                placeMonsterSpawns(pointer, spawns);

                createKeySpawn(toWorldXZ(pointer.landMarkIx0), false);

                bigSpiderArea = pointer;
            }

            //EAST
            MapSegmentPointer bossRoom;
            {
                BlockMapSegment segment = LfRef.blockmaps.loadRandomMatchingSegment(
                    new SegmentHeader(SegmentHeadType.BossRoom, 0, 0, 0, 1), rnd);
                var pointer = placeSegment(segment, caveCenterArea.getEntranceSqPos(Dir4.E, 0), Dir4.W, 0, BossAreaLockId);

                addSpawn(new SpawnPointDelegate(toWorldXZ(pointer.spawnPositions[0].position), createBoss,
                   SpawnPointData.Empty, SpawnImportance.Must_0, true, 1, true));

                bossRoom = pointer;
            }
        }

        void createBossLock(GoArgs goArgs)
        {
            goArgs.fromSpawn.spawnLock = 1;

            var lockGo = new GO.EnvironmentObj.AreaLock(goArgs, this, BossAreaLockId, GO.EnvironmentObj.AreaUnLockType.ThreeKeys,
                goArgs.characterLevel, null, VoxelModelName.threelock, Dir4.W);
        }

        protected void createBoss(GoArgs goArgs)
        {
            //bossSpawnPos = goArgs.startWp;
            goArgs.fromSpawn.spawnLock = 1;

            var boss = new VikingEngine.LootFest.GO.Characters.Boss.SpiderBot(goArgs, this);
        }

        //void placeDownPath(IntVector2 fromSquare, IntVector2 toSquare, Dir4 walkingDir)
        //{
        //    StoredPath path = new StoredPath();

        //    path.fromWp = addModel(VoxelModelName.downpath, 0, fromSquare, IntVector3.FromHeight(-6), true, false);
        //    path.fromWp.Y += 8;
        //    path.toWp = addModel(VoxelModelName.uppath, 0, toSquare, IntVector3.Zero, true, true);

        //    path.walkingDir = walkingDir;
        //    path.from = true;

        //    addSpawn(new SpawnPointDelegate(path.fromWp, createTeleportWithin,
        //        SpawnPointData.Empty, SpawnImportance.Must_0, false, 1, false), path);

        //    addSpawn(new SpawnPointDelegate(path.toWp, createTeleportWithin,
        //        SpawnPointData.Empty, SpawnImportance.Must_0, false, 1, false), path.Clone(false));
        //}

        protected void placeTerrainArea(Rectangle2 area, byte terrainIx)
        {
            ForXYLoop loop = new ForXYLoop(area);
            BlockMapSquare sq = BlockMapSquare.Empty;
            sq.terrain = terrainIx;
            
            while (loop.Next())
            {
                squares.Set(loop.Position, sq);
            }
        }

        void createTeleportWithin(GoArgs goArgs)
        {
            //StoredPath path = (StoredPath)goArgs.linkedSpawnArgs;

            //new VikingEngine.LootFest.GO.EnvironmentObj.TeleportWithin(goArgs, path.teleportTo());
        }

        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            var result = new List<VoxelModelName>
                {
                    VoxelModelName.DoorToLobby,
                    VoxelModelName.uppath,
                    VoxelModelName.downpath,
                    VoxelModelName.farmhouse1,
                    VoxelModelName.FarmTownHouse1,
                    VoxelModelName.FenceYardWide,
                };
            result.AddRange(NormalDefaultModels);
            result.AddRange(DefaultDungeonModels);
           
            return result;
        }
    }

    class StoredPath : AbsSpawnArgument
    {
        public Map.WorldPosition fromWp, toWp;
        public Dir4 walkingDir;
        public bool from;

        public StoredPath Clone(bool from)
        {
            StoredPath clone = new StoredPath();
            clone.fromWp = this.fromWp;
            clone.toWp = this.toWp;
            clone.walkingDir = this.walkingDir;

            clone.from = from;

            return clone;
        }

        public Map.WorldPosition teleportTo()
        {
            Map.WorldPosition result;

            if (from)
            {
                result = toWp;
                result.WorldXZ += IntVector2.FromDir4(walkingDir) * 4;
            }
            else
            {
                result = fromWp;
                result.WorldXZ += IntVector2.FromDir4(walkingDir) * -4;
            }

            return result;
        }
    }
}
