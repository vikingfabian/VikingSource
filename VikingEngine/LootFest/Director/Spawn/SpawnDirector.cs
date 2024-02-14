using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest
{
    delegate void CreateGameObjectDelegate(GoArgs args);

    class GenerateChunk
    {
        public bool host;
        public Map.Chunk chunk;
        public GenerateChunk(bool host, Map.Chunk chunk)
        {
            this.host = host; this.chunk = chunk;
        }
    }

    /// <summary>
    /// All spawns must go through and be cleared by this, also for network
    /// </summary>
    class SpawnDirector
    {
        const bool DebugInfo = false;

        List<AbsSpawnPoint> quedSpawns = new List<AbsSpawnPoint>(8);
        Timer.Basic updateDebugRate = new Timer.Basic(2000, true);

        public SpawnDirector()
        {
            LfRef.spawner = this;
        }


        public void update()
        {
            if (DebugInfo && updateDebugRate.Update())
            {
                Debug.Log("SpawnDirector Status::");
                var mainHero = LfRef.gamestate.LocalHostingPlayer.hero;
                Debug.Log("heat:" + mainHero.spawnDirectorHeroData.heat.ToString() + 
                    ", goalHeat:" + mainHero.spawnDirectorHeroData.goalHeat.ToString());
            }
        }
        public void asynchUpdate(float time)
        {
            for (int pIx = 0; pIx < LfRef.LocalHeroes.Count; ++pIx)
            {
                VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero = LfRef.LocalHeroes[pIx];
                hero.spawnDirectorHeroData.heat = 0;
                hero.spawnDirectorHeroData.LevelMonsterCount = 0;

                var counter = LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter;
                while (counter.Next())
                {
                    var go = counter.GetSelection;
                    if (go is AbsEnemy && go.updatesCount > 0)
                    {
                        var chunk = go.WorldPos.ScreenUnsafe;
                        if (chunk != null && chunk.level != null && hero.isInLevel(chunk.level.LevelEnum))
                        {
                            hero.spawnDirectorHeroData.LevelMonsterCount++;
                            hero.spawnDirectorHeroData.heat += EnemyDifficulty(go.Type, go.characterLevel);
                        }
                    }
                }

                int sharedWithPlayers = 0;
                for (int otherPIx = 0; otherPIx < LfRef.AllHeroes.Count; otherPIx++)
                {
                    if (LfRef.AllHeroes[otherPIx] != hero &&
                        hero.distanceToObject(LfRef.AllHeroes[otherPIx]) <= 64)
                    {
                        sharedWithPlayers++;
                    }
                }

                hero.spawnDirectorHeroData.goalHeat = 140 + sharedWithPlayers * 40;
            }


            //Spawn
            for (int pIx = 0; pIx < LfRef.LocalHeroes.Count; ++pIx)
            {
                VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero = LfRef.LocalHeroes[pIx];

                if (hero.Level != null && hero.Level.generated && hero.Level.spawnPoints.Count > 0)
                {
                    float deltaHeat = hero.spawnDirectorHeroData.goalHeat - hero.spawnDirectorHeroData.heat;
                    if (deltaHeat > 0)
                    {

                        for (int i = 0; i < 2; ++i)
                        {

                            FindMaxValuePointer<AbsSpawnPoint> mostRelevantSpawn = new FindMaxValuePointer<AbsSpawnPoint>();

                            foreach (var m in hero.Level.spawnPoints) 
                            {
                                if (!m.inQue && m is SpawnPointMonsterPortal)
                                {
                                    mostRelevantSpawn.Next(m.relevanceValue(), m);
                                }
                            }

                            if (mostRelevantSpawn.maxValue > 0)
                            {
                                mostRelevantSpawn.maxMember.prepareSpawn_Asynch(deltaHeat);
                                lock (quedSpawns)
                                {
                                    quedSpawns.Add(mostRelevantSpawn.maxMember);
                                }
                            }
                        }
                    }
                }
            }
            
        }

        public static int EnemyDifficulty(GameObjectType type, int level)
        {
            return 10;
        }

        public void GeneratingGameobjects(Map.Chunk c, bool hostingChunk)
        {
            if (c.level != null)
            {
                float heatDelta = c.generatedGoHero.spawnDirectorHeroData.goalHeat - c.generatedGoHero.spawnDirectorHeroData.heat;

                foreach(var m in c.level.spawnPoints)
                {
                    if (m.wp.ChunkGrindex == c.Index)
                    {
                        if (m.earMark)
                        {
                            lib.DoNothing();
                        }

                        if (hostingChunk || !m.networkSynchedSpawn)
                        {
                            m.prepareSpawn_Asynch(heatDelta);

                            m.Spawn();
                        }
                    }
                }
            }
        }
    }

    struct SpawnDirectorHeroData
    {
        public int LevelMonsterCount;
        public float heat, goalHeat;
    }

    class SuggestedSpawns
    {
        SpawnPointData onlyOne;
        public List<SpawnPointData> suggestedSpawns;
        public int totalCommoness;
        public bool mix = false;

        public SuggestedSpawns(SpawnPointData spawn)
        {
            onlyOne = spawn;
        }

        public SuggestedSpawns(List<SpawnPointData> suggestedSpawns)
        {
            this.suggestedSpawns = suggestedSpawns;
            foreach (var s in suggestedSpawns)
            {
                totalCommoness += s.commoness;
            }
        }

        public SpawnPointData GetRandom()
        {
            if (suggestedSpawns == null)
            {
                return onlyOne;
            }
            else
            {
                int rndVal = Ref.rnd.Int(totalCommoness);
                foreach (var sp in suggestedSpawns)
                {
                    if (rndVal < sp.commoness)
                        return sp;
                    rndVal -= sp.commoness;
                }

                throw new Exception("SpawnPointData GetRandom()");
            }
        }
    }


    struct SpawnPointData
    {
        public static readonly SpawnPointData Empty = new SpawnPointData();

        public GameObjectType type;
        public int level;
        public int commoness;
        public byte direction;

        public SpawnPointData(GameObjectType type)
        {
            this.type = type;
            this.level = 0;
            this.commoness = 10;
            this.direction = 0;
        }

        public SpawnPointData(GameObjectType type, int level)
        {
            this.type = type;
            this.level = level;
            this.commoness = 10;
            this.direction = 0;
        }

        public SpawnPointData(GameObjectType type, int level, int commoness)
        {
            this.type = type;
            this.level = level;
            this.commoness = commoness;
            this.direction = 0;
        }

        public SpawnPointData(GameObjectType type, int level, int commoness, byte direction)
        {
            this.type = type;
            this.level = level;
            this.commoness = commoness;
            this.direction = direction;
        }

        public static SpawnPointData FromDir(Dir4 direction)
        {
            SpawnPointData result = new SpawnPointData();
            result.direction = (byte)direction;
            return result;
        }
    }

    enum SpawnImportance
    {
        Must_0,
        Should_1,
        HighSuggest_2,
        LowSuggest_3,
        NumNon,
    }


}
