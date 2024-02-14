using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class SpawnManager
    {
        List<MonsterGroupType> groups = new List<MonsterGroupType>();
        List<HqUnitType> groupMembers = new List<HqUnitType>();
        List<MonsterGroupType> spawnedGroups = new List<MonsterGroupType>(4);
        List<IntVector2> positions = new List<IntVector2>(4);
        public PcgRandom rnd;
        BalancedRandom bRnd;
        MonsterGroupType previousMonsterGroup = MonsterGroupType.NUM_NONE;
        public float playerCountDifficulty;

        public static string SpawnResult; 

        public SpawnManager()
        {
            rnd = new PcgRandom(toggRef.Seed + 17);
            bRnd = new BalancedRandom(rnd);

            playerCountDifficulty = hqRef.setup.playerCountDifficulty;            
        }

        public void spawn()
        {
            SpawnResult = "Spw:" + playerCountDifficulty.ToString() + "|" +
                toggRef.Seed.ToString() + "| S" + rnd.Int(1000);

            if (toggRef.board.metaData != null)
            {
                foreach (var m in toggRef.board.metaData.flags)
                {
                    flagSpawn(m);
                }
            }

            SpawnResult += "| E" + rnd.Int(1000);
        }

        void flagSpawn(RoomFlag flag)
        {
            spawnedGroups.Clear();

            flag.collectTiles();
            int areaTileCount = flag.tileArea.Area;
            int realTileCount = flag.tiles.Count;

            int coveredTiles = areaTileCount - realTileCount;

            float areas = (areaTileCount - (coveredTiles * 0.5f)) / (float)MapSpawnLib.AreaTileCount;
            areas *= flag.settings.spawnCountMultiply;

            if (flag.settings.Type == SpawnSettingsType.Specific)
            {
                monsterSpawn(flag, areas);
            }
            else
            {
                campSpawn(flag, areas);
                lootSpawn(flag, areas);

                monsterSpawn(flag, areas);
                trapSpawn(flag, areas);

                rubbleSpawn(flag, areas);
            }
        }

        void campSpawn(RoomFlag flag, float areas)
        {
            if (!flag.heroStartArea)
            {
                double spawnChance;

                if (areas < 1)
                {
                    spawnChance = 0.4;
                }
                else if (areas < 4)
                {
                    spawnChance = 0.7;
                }
                else
                {
                    spawnChance = 0.9;
                }

                if (bRnd.Chance(spawnChance, false))
                {
                    IntVector2 pos;

                    if (pullWallTile(flag, out pos))
                    {
                        ToggEngine.TileObjLib.CreateObject(ToggEngine.TileObjectType.Campfire,
                            pos, null, false);
                    }
                }
            }
        }

        void lootSpawn(RoomFlag flag, float areas)
        {
            IntVector2 pos;
            float rndModify = 1f + bRnd.Plus_MinusF(0.5f, false);
            int lootCount = MathExt.CeilingToInt(areas * playerCountDifficulty * rndModify * MapSpawnLib.LootSpawnsPerArea);

            for (int i = 0; i < lootCount; ++i)
            {
                if (bRnd.Chance(0.9, false))
                {
                    int tier = flag.settings.GetTier(rnd);
                    if (pullWallTile(flag, out pos))
                    {
                        ToggEngine.TileObjLib.CreateObject(ToggEngine.TileObjectType.ItemCollection, 
                            pos, new Gadgets.TileItemCollData((LootLevel)tier), false);
                    }
                }

                //Coins
                if (hqRef.setup.conditions.EnemyLootDrop)
                {
                    if (bRnd.Chance(0.2, false))
                    {
                        int coinCount = rnd.Int(1, 4);

                        for (int cIndex = 0; cIndex < coinCount; ++cIndex)
                        {
                            if (pullWallTile(flag, out pos))
                            {
                                new Gadgets.Lootdrop(pos, rnd.Chance(0.3) ? 2 : 1);
                            }
                        }
                    }
                }
            }            
        }
        
        void rubbleSpawn(RoomFlag flag, float areas)
        {
            int max = MathExt.CeilingToInt(areas * 1.2f);
            int count = rnd.Int(0, max + 1);
            IntVector2 pos;

            for (int i = 0; i < count; ++i)
            {
                if (pullDecorationTile(flag, out pos))
                {
                    ToggEngine.TileObjLib.CreateObject(ToggEngine.TileObjectType.Furnishing,
                        pos, ToggEngine.Map.MapFurnishType.Box, false);
                }
            }            
        }

        bool pullDecorationTile(RoomFlag flag, out IntVector2 tile)
        {
            if (rnd.Chance(0.7))
            {
                return pullWallTile(flag, out tile);
            }
            else
            {
                while (flag.tiles.Count > 0)
                {
                    tile = arraylib.RandomListMemberPop(flag.tiles, rnd);

                    if (toggRef.board.IsEmptyFloorSquare(tile) &&
                        !inFrontOfDoorOpening(tile))
                    {
                        return true;
                    }
                }

                tile = IntVector2.NegativeOne;
                return false;
            }
        }

        bool inFrontOfDoorOpening(IntVector2 pos)
        {
            foreach (var dir in IntVector2.Dir4Array)
            {
                var sq = toggRef.Square(pos + dir);
                if (sq != null &&
                    (sq.tag.tagType == ToggEngine.Map.SquareTagType.RoomDivider ||
                    sq.tileObjects.HasObject(ToggEngine.TileObjectType.Door)))
                {
                    return true;
                }
            }

            return false;
        }

        bool pullWallTile(RoomFlag flag, out IntVector2 tile)
        {
            while (flag.wallAdjTiles.Count > 0)
            {
                tile = arraylib.RandomListMemberPop(flag.wallAdjTiles, rnd);
                if (toggRef.board.IsEmptyFloorSquare(tile))
                {
                    return true;
                }
            }

            tile = IntVector2.NegativeOne;
            return false;
        }

        bool pullEmptyTile(RoomFlag flag, out IntVector2 tile)
        {
            while (flag.tiles.Count > 0)
            {
                tile = arraylib.RandomListMemberPop(flag.tiles, rnd);

                if (toggRef.board.IsEmptyFloorSquare(tile))
                {
                    return true;
                }
            }

            tile = IntVector2.NegativeOne;
            return false;
        }

        void monsterSpawn(RoomFlag flag, float areas)
        {
            Range groupCountInterval = new Range(
                Bound.Min((int)(areas / 4f), 1),
                Bound.Min((int)(areas / 1f), 1));

            int monsterGroupCount = groupCountInterval.GetRandom(rnd);
            groups.Clear();
            flag.settings.getSpawnGroups(rnd, monsterGroupCount, ref previousMonsterGroup, groups);

            float flagDifficulty = flag.settings.DifficultyValue(rnd);

            //TODO player count

            float goalDifficulty = monsterSpawnGoalDif(areas, flagDifficulty, true);

            float currentSpawnDifficulty = 0;
            RoomFlag connected = flag.connectedFlag();
            if (connected != null)
            {
                currentSpawnDifficulty = connected.currrentSpawnDifficulty;
            }

            if (flag.settings.Type == SpawnSettingsType.Specific)
            {
                spawnSpecific(flag, ref currentSpawnDifficulty);
            }
            else
            {
                spawnGrops(flag, goalDifficulty, ref currentSpawnDifficulty);
            }

            flag.currrentSpawnDifficulty = currentSpawnDifficulty;
        }

        public float monsterSpawnGoalDif(float areaCount, float flagDifficulty, bool randomMod)
        {
            float rndDifficultyModify = 1f;
            if (randomMod)
            {
                rndDifficultyModify += bRnd.Plus_MinusF(0.3f, true);
            }
            float goalDifficulty = areaCount * playerCountDifficulty * MapSpawnLib.AreaDefaultSpawnDifficulty *
                flagDifficulty * rndDifficultyModify;

            return goalDifficulty;
        }

        void spawnSpecific(RoomFlag flag, ref float currentSpawnDifficulty)
        {
            var specific = (SpecificSpawn)flag.settings;
            foreach (var m in specific.monsters)
            {
                IntVector2 tile;
                if (pullEmptyTile(flag, out tile))
                {
                    placeMonster(tile, m, ref currentSpawnDifficulty, null);
                }
                else
                {
                    return;
                }
            }
        }

        void spawnGrops(RoomFlag flag, float goalDifficulty, ref float currentSpawnDifficulty)
        {
            bool reachedGoal = false;

            while (!reachedGoal)
            {
                //ERR spawna på fireplace
                MonsterGroupType groupType = arraylib.RandomListMember(groups, rnd);
                MonsterGroupSetup groupSetup = MapSpawnLib.GroupSetups[(int)groupType];

                if (spawnedGroups.Contains(groupType) == false)
                {
                    spawnedGroups.Add(groupType);
                }

                reachedGoal = spawnGroupsOnTiles(groupSetup, flag.tiles, goalDifficulty,
                    ref currentSpawnDifficulty, null);
            }
        }

        public void spawnGropsAfterGoal(MonsterGroupSetup groupSetup,float goalDifficulty, 
            List<IntVector2> tiles, List<Unit> unitsPointer)
        {
            bool reachedGoal = false;
            float currentSpawnDifficulty = 0;

            while (!reachedGoal)
            {
                reachedGoal = spawnGroupsOnTiles(groupSetup, tiles, goalDifficulty,
                    ref currentSpawnDifficulty, unitsPointer);
            }
        }

        public bool spawnGroupsOnTiles(MonsterGroupSetup groupSetup, List<IntVector2> tiles, 
            float goalDifficulty, ref float currentSpawnDifficulty, 
            List<Unit> unitsPointer)
        {
            groupMembers.Clear();
            groupSetup.getOneGroup(rnd, groupMembers);

            foreach (var member in groupMembers)
            {
                bool spawnMore = currentSpawnDifficulty < goalDifficulty &&
                    tiles.Count > 0;

                if (spawnMore)
                {
                    IntVector2 tile = arraylib.RandomListMemberPop(tiles, rnd);

                    if (toggRef.board.IsEmptyFloorSquare(tile, false))
                    {
                        placeMonster(tile, member, ref currentSpawnDifficulty, unitsPointer);
                    }
                }
                else
                {
                    //REACHED SPAWN GOAL
                    return true;
                }
            }

            return false;
        }


        public void spawnGroupOnTags(MonsterGroupType groupType, List<IntVector2> tags, float difficulty,
            List<Unit> unitsPointer)
        {
            //float areas = tags.Count / (float)MapSpawnLib.AreaTileCount;

            float goalDifficulty = playerCountDifficulty * difficulty;//monsterSpawnGoalDif(areas, difficulty, true);
            float currentSpawnDifficulty = 0;

            MonsterGroupSetup groupSetup = MapSpawnLib.GroupSetups[(int)groupType];

            while (currentSpawnDifficulty < goalDifficulty)
            {
                spawnGroupsOnTiles(groupSetup, tags, goalDifficulty, ref currentSpawnDifficulty,
                    unitsPointer);
            }
        }

        void placeMonster(IntVector2 pos, HqUnitType unitType, ref float currentSpawnDifficulty, 
            List<Unit> unitsPointer)
        {
            var data = hqRef.unitsdata.Get(unitType);
            currentSpawnDifficulty += data.UnitDifficulty;

            var u = new Unit(pos, data, hqRef.players.dungeonMaster);

            if (unitsPointer != null)
            {
                unitsPointer.Add(u);
            }
        }

        void trapSpawn(RoomFlag flag, float areas)
        {
            float rndModify = 1f + bRnd.Plus_MinusF(1f, true);
            int trapCount = MathExt.CeilingToInt(areas * playerCountDifficulty * rndModify * 0.25f);

            bool webb = spawnedGroups.Contains(MonsterGroupType.Spiders);
            double spawnChance;
            ToggEngine.TileObjectType trap;

            if (webb)
            {
                spawnChance = 0.8;
                trap = ToggEngine.TileObjectType.NetTrap;
            }
            else
            {
                spawnChance = 0.3;
                trap = ToggEngine.TileObjectType.DamageTrap;
            }

            for (int i = 0; i < trapCount; ++i)
            {
                if (bRnd.Chance(spawnChance, true))
                {
                    int tailCount = rnd.Int(2, 6);

                    tailOfPositions(flag, tailCount);

                    foreach (var pos in positions)
                    {
                        ToggEngine.TileObjLib.CreateObject(trap,
                            pos, null, false);
                    }
                }   
            }
        }

        void tailOfPositions(RoomFlag flag, int count)
        {
            positions.Clear();

            IntVector2 pos;
            if (pullWallTile(flag, out pos))
            {
                nextTail(pos);
            }

            void nextTail(IntVector2 position)
            {
                positions.Add(position);

                if (--count > 0)
                {
                    int dirIx = rnd.Int(8);
                    for (Dir8 di = 0; di < Dir8.NUM; ++di)
                    {
                        IntVector2 adj = position + IntVector2.Dir8Array[dirIx];
                        if (toggRef.board.IsEmptyFloorSquare(adj) &&
                            positions.Contains(adj) == false)
                        {
                            nextTail(adj);
                            break;
                        }
                        else
                        {
                            if (++dirIx >= IntVector2.Dir8Array.Length)
                            {
                                dirIx = 0;
                            }
                        }
                    }
                }
            }
        }

        public IntVector2? randomAdjacentSpawnAvailable(IntVector2 center)
        {
            int dirIx = rnd.Int(8);
            for (Dir8 di = 0; di < Dir8.NUM; ++di)
            {
                IntVector2 adj = center + IntVector2.Dir8Array[dirIx];

                if (toggRef.board.IsSpawnAvailableSquare(adj))
                {
                    return adj;
                }

                if (++dirIx >= IntVector2.Dir8Array.Length)
                {
                    dirIx = 0;
                }
            }

            return null;
        }
    }
}
