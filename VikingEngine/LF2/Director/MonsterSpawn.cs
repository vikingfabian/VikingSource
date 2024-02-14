using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.Characters;

namespace VikingEngine.LF2.Director
{
    class MonsterSpawn
    {
        const float SafeRadius = 0.15f;
        const float Level1SafeRadius = ((Map.WorldPosition.WorldChunksX - Map.WorldOverview.WorldChunkEdge * 2) * SafeRadius);
        const float Level2Radius = ((Map.WorldPosition.WorldChunksX - Map.WorldOverview.WorldChunkEdge * 2) * (0.7f - SafeRadius));
        

        const float StartUpWaiting = 10000;//give the player some time after starting
        static readonly IntervalF MonsterWaveIntervals = new IntervalF(lib.SecondsToMS(8), lib.SecondsToMS(12));
        static readonly IntervalF DropMonsterIntervals = new IntervalF(lib.SecondsToMS(4), lib.SecondsToMS(8));
        static readonly Range IntervalSpawnValue = new Range(6, 10);
        const int Lvl2SpawnValueAdd = 10;
        int spawnValue = 0;
        Vector2 lastHeroPos = Vector2.Zero;

        const float MonsterAreaRadius = Map.WorldPosition.ChunkWidth * 4;
        Vector2 monsterArea = Vector2.Zero;//a larger area that canbe drained from monsters
        static readonly Range MonsterWavesRange = new Range(2, 5);
        int monsterWavesInAreaLeft = 1;

        Timer.Basic checkRate;
        List<GameObjects.Characters.AbsCharacter> enemies;
        Map.ChunkData chunkData;

        bool smallMovement = false;
        bool safeEnvironment = false;
        bool clearedArea = false;

        public override string ToString()
        {
            return "MonsterSpawn: time to next" + checkRate.TimeLeft.ToString() + ", spawnVal:" + spawnValue.ToString() + 
                ", smallMov:" + smallMovement.ToString() + ", waves left: " + monsterWavesInAreaLeft.ToString() + 
                ", safe env:" + safeEnvironment.ToString() + ", cleared area:" + clearedArea.ToString();
        }

        public MonsterSpawn()
        {
            checkRate = new Timer.Basic(StartUpWaiting); 
            enemies = new List<GameObjects.Characters.AbsCharacter>();
        }

        public void Update(float time, GameObjects.Characters.Hero hero)
        {

            if (checkRate.Update(time))
            {
                checkRate.Set(DropMonsterIntervals.GetRandom());
                if (PlatformSettings.SpawnEnemies)
                {
                    monsterWavesInAreaLeft--;
                    if (monsterWavesInAreaLeft <= 0)
                    {
                        if ((monsterArea - hero.PlanePos).Length() >= MonsterAreaRadius)
                        {
                            monsterArea = hero.PlanePos;
                            monsterWavesInAreaLeft = MonsterWavesRange.GetRandom();
                            clearedArea = false;
                        }
                        else
                        {
                            checkRate.Set(MonsterWaveIntervals.GetRandom() + lib.SecondsToMS(20));
                            clearedArea = true;
                            return;
                        }
                    }

                    //calc monster level

                    Percent chanceForLvl2 = PositionToLevel2Chance(hero);
                    if (spawnValue <= 0)
                    {//check how big the next group of enemies will be
                        for (int i = enemies.Count - 1; i >= 0; i--)
                        {
                            if (!enemies[i].Alive)
                            {
                                enemies.RemoveAt(i);
                            }
                        }

                        chunkData = LfRef.worldOverView.GetChunkData(hero.ScreenPos);
                        if (enemies.Count >= LootfestLib.MaxEnemiesSpawn ||
                            !LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject() ||
                            (
                                chunkData.AreaType == Map.Terrain.AreaType.Castle ||
                                chunkData.AreaType == Map.Terrain.AreaType.Village ||
                                chunkData.AreaType == Map.Terrain.AreaType.City || 
                                chunkData.AreaType == Map.Terrain.AreaType.HomeBase ||
                                chunkData.AreaType == Map.Terrain.AreaType.PrivateHome ||
                                chunkData.AreaType == Map.Terrain.AreaType.FlatEmptyAndMonsterFree)
                            )
                        {//safe area
                            monsterWavesInAreaLeft = lib.SmallestOfTwoValues(1, monsterWavesInAreaLeft);
                            checkRate.Set(MonsterWaveIntervals.GetRandom());
                            safeEnvironment = true;
                            return;
                        }
                        else
                        {
                            safeEnvironment = false;
                        }

                        spawnValue = IntervalSpawnValue.GetRandom() - enemies.Count / 2;
                        spawnValue += (int)(Lvl2SpawnValueAdd * chanceForLvl2.Value);

                        if ((lastHeroPos - hero.PlanePos).Length() < 5)
                        {//Small movement lower the spawn rate
                            smallMovement = true;
                            spawnValue /= 4;
                        }
                        else
                        {
                            smallMovement = false;
                        }
                        lastHeroPos = hero.PlanePos;
                    }

                    
                    Vector3 pos = spawnArea(hero);

                    int rnd = Ref.rnd.Int(100);
                    if (rnd < 2 && spawnValue >= 6)
                    {
                        //giant

                    }
                    else if (rnd < 30 && spawnValue >= 4)
                    {
                        //humanoid
                        List<GameObjects.Characters.Orc> group = new List<GameObjects.Characters.Orc>();

                        int numGrunts = Ref.rnd.Int(3);
                        int numSwords = Ref.rnd.Int(4) + 1;
                        int numArchers = 0;
                        int numBrutes = 0;
                        if (chanceForLvl2.DiceRoll())
                            numArchers = Ref.rnd.Int(3) + 1;
                        if (chanceForLvl2.DiceRoll())
                            numBrutes = Ref.rnd.Int(2);
                        if (chanceForLvl2.DiceRoll())
                            numSwords += 2;

#if !LOWQUAL
                        for (int i = 0; i < numGrunts; i++)
                        {
                            new GameObjects.Characters.Grunt(new Map.WorldPosition(spawnStartPos(pos)), percentToLvl(chanceForLvl2));
                        }
#endif
                        for (int i = 0; i < numBrutes; i++)
                        {
                            group.Add(new GameObjects.Characters.Orc(new Map.WorldPosition(spawnStartPos(pos)), percentToLvl(chanceForLvl2), GameObjects.Characters.HumanoidType.Brute, null));
                        }
                        for (int i = 0; i < numSwords; i++)
                        {
                            group.Add(new GameObjects.Characters.Orc(new Map.WorldPosition(spawnStartPos(pos)), percentToLvl(chanceForLvl2), GameObjects.Characters.HumanoidType.SwordsMan, null));
                        }
                        for (int i = 0; i < numArchers; i++)
                        {
                            group.Add(new GameObjects.Characters.Orc(new Map.WorldPosition(spawnStartPos(pos)), percentToLvl(chanceForLvl2), GameObjects.Characters.HumanoidType.Archer, null));
                        }

                        new GameObjects.Characters.Orc(new Map.WorldPosition(spawnStartPos(pos)), percentToLvl(chanceForLvl2), GameObjects.Characters.HumanoidType.Leader, group);
                        spawnValue -= group.Count + 3;
                    }
                    else
                    {
                        //monster
                        int numMonsters = 1 + Ref.rnd.Int(4);



                        spawnValue -= numMonsters;
                        bool type1 = Ref.rnd.RandomChance(60);
                        GameObjects.Characters.Monster2Type type;

                        int typernd = Ref.rnd.Int(100);
                        switch (chunkData.Environment)
                        {
                            default:
                                if (typernd < 12)
                                {
                                    type = GameObjects.Characters.Monster2Type.Ent;
                                }
                                else if (typernd < 30)
                                {
                                    type = GameObjects.Characters.Monster2Type.Squig;
                                }
                                else if (typernd < 60)
                                {
                                    type = GameObjects.Characters.Monster2Type.Wolf;
                                }
                                else
                                {
                                    type = GameObjects.Characters.Monster2Type.Hog;
                                }
                                break;
                            case Map.Terrain.EnvironmentType.Burned:
                                if (typernd < 12)
                                {
                                    type = GameObjects.Characters.Monster2Type.Lizard;
                                }
                                else if (typernd < 30)
                                {
                                    type = GameObjects.Characters.Monster2Type.Spider;
                                }
                                else if (typernd < 60)
                                {
                                    type = GameObjects.Characters.Monster2Type.Harpy;
                                }
                                else
                                {
                                    type = GameObjects.Characters.Monster2Type.FireGoblin;
                                }
                                break;
                            case Map.Terrain.EnvironmentType.Desert:
                                if (typernd < 12)
                                {
                                    type = GameObjects.Characters.Monster2Type.Wolf;
                                }
                                else if (typernd < 30)
                                {
                                    type = GameObjects.Characters.Monster2Type.FireGoblin;
                                }
                                else if (typernd < 60)
                                {
                                    type = GameObjects.Characters.Monster2Type.Scorpion;
                                }
                                else
                                {
                                    type = GameObjects.Characters.Monster2Type.Lizard;
                                }
                                break;
                            case Map.Terrain.EnvironmentType.Forest:
                                if (typernd < 12)
                                {
                                    type = GameObjects.Characters.Monster2Type.Hog;
                                }
                                else if (typernd < 30)
                                {
                                    type = GameObjects.Characters.Monster2Type.Frog;
                                }
                                else if (typernd < 60)
                                {
                                    type = GameObjects.Characters.Monster2Type.Ent;
                                }
                                else
                                {
                                    type = GameObjects.Characters.Monster2Type.Spider;
                                }
                                break;
                            case Map.Terrain.EnvironmentType.Swamp:
                                if (typernd < 12)
                                {   
                                    type = GameObjects.Characters.Monster2Type.Harpy;
                                }
                                else if (typernd < 30)
                                {
                                    type = GameObjects.Characters.Monster2Type.Squig;
                                }
                                else if (typernd < 60)
                                {
                                    type = GameObjects.Characters.Monster2Type.Frog;
                                }
                                else
                                {
                                    type = GameObjects.Characters.Monster2Type.Crocodile;
                                }
                                break;

                        }

                        if (type == GameObjects.Characters.Monster2Type.Squig)
                        {
                            numMonsters *= 2;
                        }
                        

                        for (int i = 0; i < numMonsters; i++)
                        {
                            Map.WorldPosition wp = new Map.WorldPosition(spawnStartPos(pos));
                            SpawnMonsterType(type, wp, percentToLvl(chanceForLvl2));
                           
                        }
                    }

                    if (spawnValue <= 0)
                    { //all monsters been spawned, wait for next interval
                        checkRate.Set(MonsterWaveIntervals.GetRandom());
                    }

                }
            }
        }

        public static void SpawnMonsterType(GameObjects.Characters.Monster2Type type, Map.WorldPosition wp, int level)
        {
            switch (type)
            {
                case GameObjects.Characters.Monster2Type.Crocodile:
                    new GameObjects.Characters.Monsters.Crocodile(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Ent:
                    new GameObjects.Characters.Monsters.Ent(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.FireGoblin:
                    new GameObjects.Characters.Monsters.FireGoblin(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Frog:
                    new GameObjects.Characters.Monsters.Frog(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Harpy:
                    new GameObjects.Characters.Monsters.Harpy(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Hog:
                    new GameObjects.Characters.Monsters.Hog(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Lizard:
                    new GameObjects.Characters.Monsters.Lizard(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Scorpion:
                    new GameObjects.Characters.Monsters.Scorpion(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Spider:
                    new GameObjects.Characters.Monsters.Spider(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Squig:
                    new GameObjects.Characters.Monsters.Squig(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.SquigSpawn:
                    new GameObjects.Characters.Monsters.SquigSpawn(wp);
                    break;
                case GameObjects.Characters.Monster2Type.Wolf:
                    new GameObjects.Characters.Monsters.Wolf(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Mummy:
                    new GameObjects.Characters.CastleEnemy.Mommy2(wp, level);
                    break;
                case GameObjects.Characters.Monster2Type.Bee:
                    new GameObjects.Characters.Monsters.Bee(wp, level);
                    break;
                default:
                    throw new NotImplementedException("SpawnMonsterType: " + type.ToString());
            }
                        
        }

        public static Percent PositionToLevel2Chance(Hero hero)
        {
            float lengthFromHome = (hero.ChunkUpdateCenter.Vec - LfRef.worldOverView.StartChunk.Vec).Length();
            Percent chanceForLvl2 = new Percent(Bound.Set((lengthFromHome - Level1SafeRadius) / Level2Radius, 0, 1));
            return chanceForLvl2;
        }

        static int percentToLvl(Percent chanceForLvl2)
        {
            return chanceForLvl2.DiceRoll() ? 1 : 0;
        }

        int areaLevelToLevel(int areaLvl)
        {
            switch (areaLvl)
            {
                case 0:
                    return 0;
                case 1:
                    return lib.RandomBool() ? 0 : 1;
                case 2:
                    return 1;
            }
            throw new NotImplementedException();
        }

        Vector3 spawnStartPos(Vector3 area)
        {
            const float Range = 10;
            area.X += Ref.rnd.Plus_MinusF(Range);
            area.Z += Ref.rnd.Plus_MinusF(Range);
            return area;
        }

        const float MinSpawnRange = 32;
        static readonly IntervalF SpawnRange = new IntervalF(50, 70);
        
        Vector3 spawnArea(GameObjects.Characters.Hero hero)
        {
            const int MaxLoops = 10;
            Vector3 result = Vector3.Zero;
            //List<GameObjects.Characters.Hero> allHeroes = LfRef.AllHeroes;//.gamestate.AllHeroes();

            for (int i = 0; i < MaxLoops; i++)
            {
                Rotation1D dir;
                if (Ref.rnd.RandomChance(30))
                {
                    dir = hero.Rotation;
                    dir.Degrees += Ref.rnd.Plus_MinusF(30);
                }
                else
                {
                    dir = Rotation1D.Random();
                }

                result = hero.Position + Map.WorldPosition.V2toV3(dir.Direction(SpawnRange.GetRandom()));

                //make sure they dont show up to close to another hero
                bool goodPosition = true;
                for (int j = 0; j < LfRef.AllHeroes.Count; ++j)//foreach (GameObjects.Characters.Hero h in LfRef.AllHeroes)
                {
                    GameObjects.Characters.Hero h = LfRef.AllHeroes[j];
                    if (h != hero && (h.Position - result).Length() <= MinSpawnRange)
                    {
                        goodPosition = false;
                        break;
                    }
                }
                if (goodPosition)
                {
                    return result;
                }
            }
            System.Diagnostics.Debug.WriteLine("ERR spawnpos exceded max loops");
            return result;
        }
    }
}
