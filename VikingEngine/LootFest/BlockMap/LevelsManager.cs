using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap.Level;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.BlockMap
{
    delegate void RequestWorldCallBack2(AbsLevel level, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, object args);

    class LevelsManager
    {
        public Director.ChunkHostDirector chunkHostDirector;
        
        static readonly IntVector2 LevelsTopLeftChunk = new IntVector2(Map.WorldPosition.MaxChunkRadiusGenerating + 8);
        static readonly int WorldDivitionsIntoAreas =
            (Map.WorldPosition.WorldChunkSize - LevelsTopLeftChunk.X * 2) / AbsLevel.ChunkSize.X;
        static readonly int WorldTotalAreas = WorldDivitionsIntoAreas * WorldDivitionsIntoAreas;


        List<AbsLevel> activeLevels = new List<AbsLevel>();
        AbsLevel[] levels = new AbsLevel[(int)LevelEnum.NUM_NON];

        int nextAvailableAreaIx = 0;
        List<WaitingForLevel2> waitingList = new List<WaitingForLevel2>();
        public VikingEngine.LootFest.Data.WorldSeed WorldSeed = new Data.WorldSeed();

        public LevelsManager(VikingEngine.LootFest.Data.WorldData worldData)
        {
            if (worldData.isClient)
            {
                WorldSeed.seed = worldData.seed;
            }

            LfRef.levels2 = this;
            chunkHostDirector = new Director.ChunkHostDirector();
        }

        public void GetLevel(Map.WorldPosition wp, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero,
           RequestWorldCallBack2 callback, object callBackArgs)
        {
            
        }

        public void GetLevel(LevelEnum type, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero,
            RequestWorldCallBack2 callback, object callBackArgs)
        {
            var result = levels[(int)type];
            if (result == null || !result.generated)
            {
                waitingList.Add(new WaitingForLevel2(type, callback, callBackArgs, hero));
                if (Ref.netSession.IsHostOrOffline)
                {
                    createLvlAndClearOld(type);
                }
                else
                {
                    waitingList.Add(new WaitingForLevel2(type, callback, callBackArgs, hero));
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.RequestWorldLevel, Network.PacketReliability.Reliable);
                    w.Write((int)type);
                    return;
                }

            }
            else
            {
                callback(result, hero, callBackArgs);
            }
        }

        public AbsLevel GetLevelUnsafe(IntVector2 chunk)
        {
            foreach (var m in activeLevels)
            {
                if (m.worldArea.IntersectTilePoint(chunk))
                {
                    return m;
                }
            }

            return null;
        }

        public AbsLevel GetLevelUnsafe(LevelEnum type)
        {
            return levels[(int)type];
        }

        AbsLevel createLvlAndClearOld(LevelEnum lvl)
        {
            Debug.CrashIfThreaded();

            if (levels[(int)lvl] != null)
            {
                return levels[(int)lvl];
            }

            //First remove not used levels
            clearOutNotUsedLevels();

            //Find a spot to place the level in
            int areaIx = findAvailableLevelArea();

            //Create level
            return createLevelAt(lvl, areaIx);
        }

        public void clearOutNotUsedLevels()
        {
            for (int i = levels.Length - 1; i >= 0; --i)
            {
                if (levels[i] != null)
                {
                    if (!levels[i].heroInlevel())
                    {
                        destroyLevel(i, true);
                    }
                }
            }
        }

        public void destroyLevel(int index, bool local)
        {
            if (Ref.netSession.IsHostOrOffline || !local)
            {
                //Clear out level
                AbsLevel lvl = levels[index];

                if (lvl != null)
                {
                    var gameObjects = LfRef.gamestate.GameObjCollection.localMembersUpdateCounter.IClone();
                    while (gameObjects.Next())
                    {
                        gameObjects.GetSelection.onLevelDestroyed(lvl);
                    }
                    //LfRef.spawner.RemoveLevel(lvl.LevelEnum);

                    Debug.Log("-destroys level: " + lvl.LevelEnum.ToString());
                    levels[index] = null;
                }

                if (local)
                {
                    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DestroyLevel, Network.PacketReliability.Reliable);
                    w.Write((byte)index);
                }
            }
        }

        int findAvailableLevelArea()
        {
            int loops = 0;
            while (true)
            {
                if (nextAvailableAreaIx >= WorldTotalAreas)
                {
                    nextAvailableAreaIx = 0;
                }

                bool isUsed = false;
                foreach (var lvl in levels)
                {
                    if (lvl != null && lvl.usesAreaIndex == nextAvailableAreaIx)
                    {
                        isUsed = true;
                        break;
                    }
                }

                if (isUsed)
                {
                    nextAvailableAreaIx++;
                    if (++loops > WorldTotalAreas * 2)
                    {
                        throw new EndlessLoopException("find Available LevelArea");
                    }
                }
                else
                {
                    int usesAreaIx = nextAvailableAreaIx;
                    nextAvailableAreaIx++;
            
                    return usesAreaIx;
                }
            }

           

            throw new Exception("No available area to place the level");
        }

        IntVector2 toAreaTopLeftChunk(int usesAreaIx)
        {
            var grindex = new IntVector2(usesAreaIx % WorldDivitionsIntoAreas, usesAreaIx / WorldDivitionsIntoAreas);

            return LevelsTopLeftChunk + grindex * AbsLevel.ChunkSize;
        }

        AbsLevel createLevelAt(LevelEnum lvl, int areaIx)
        {
            Debug.Log("createLevelAt: " + lvl.ToString() + areaIx.ToString());

            if (levels[(int)lvl] != null)
            {
                return levels[(int)lvl];
            }

            AbsLevel newLevel;
            switch (lvl)
            {
                case LevelEnum.Debug:
                    newLevel = new DebugLevl();
                    break;
                case LevelEnum.Creative:
                    newLevel = new CreativeArea();
                    break;
                case LevelEnum.Tutorial:
                    newLevel = new TutorialLevel();
                    break;
                case LevelEnum.IntroductionLevel:
                    newLevel = new IntroductionLevel();
                    break;
                case LevelEnum.Lobby:
                    newLevel = new Lobby();
                    break;
                case LevelEnum.Spider1:
                    newLevel = new SpiderLevel();
                    break;
                case LevelEnum.EndBoss:
                    newLevel = new EndBoss();
                    break;
                
                //case LevelEnum.Mount:
                //    newLevel = new Mounts2();
                //    break;
                //case LevelEnum.EmoVsTroll:
                //    newLevel = new EmoVsTroll();
                //    break;
                //case LevelEnum.Statue:
                //    newLevel = new Statue();
                //    break;
                //case LevelEnum.Birds:
                //    newLevel = new BlockMap.Level.Birds();
                //    break;
                //case LevelEnum.Swine:
                //    newLevel = new Swine();
                //    break;
                //case LevelEnum.Desert1:
                //    newLevel = new Desert1();
                //    break;
                //case LevelEnum.Desert2:
                //    newLevel = new Desert2();
                //    break;
                //case LevelEnum.Barrels:
                //    newLevel = new Barrels();
                //    break;
                //case LevelEnum.Orcs:
                //    newLevel = new Orcs();
                //    break;
                //case LevelEnum.Wolf:
                //    newLevel = new Wolf();
                //    break;
                //case LevelEnum.Elf1:
                //    newLevel = new Elf1();
                //    break;
                //case LevelEnum.SkeletonDungeon:
                //    newLevel = new SkeletonDungeon();
                //    break;
                
                //case LevelEnum.Challenge_Zombies:
                //    newLevel = new Challenge_Zombies();
                //    break;
                //case LevelEnum.Lootfest1:
                //    newLevel = new Lootfest1();
                //    break;

                default:
                    throw new NotImplementedException();
            }

            newLevel.initialize(areaIx, toAreaTopLeftChunk(areaIx));
            Debug.Log("-Create level: " + lvl.ToString());
            levels[(int)lvl] = newLevel;
            activeLevels.Add(newLevel);

            return newLevel;
        }

        public void netReadRequestLevel(System.IO.BinaryReader r)
        {
            if (Ref.netSession.IsHost)
            {
                int lvl = r.ReadByte();
                if (levels[lvl] == null)
                {
                    createLvlAndClearOld((LevelEnum)lvl);
                }
                else
                {
                    var level = levels[lvl];
                    netWriteCreateLevel(level.LevelEnum, level.usesAreaIndex);
                }
            }
        }

        void netWriteCreateLevel(LevelEnum lvl, int areaIx)
        {
            if (Ref.netSession.IsHost)
            {
                Debug.Log("netWriteCreateLevel: " + lvl.ToString() + areaIx.ToString());

                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.CreateWorldLevel, Network.PacketReliability.Reliable);
                w.Write((byte)lvl);
                w.Write((byte)areaIx);
            }
        }

        public void netReadCreateLevel(System.IO.BinaryReader r)
        {
            if (Ref.netSession.IsClient)
            {
                LevelEnum lvl = (LevelEnum)r.ReadByte();
                int areaIx = r.ReadByte();

                createLevelAt(lvl, areaIx);
            }
        }

        public void levelGeneratingComplete(AbsLevel lvl)
        {
            for (int i = waitingList.Count - 1; i >= 0; --i)
            {
                if (waitingList[i].onCreateLevel(lvl))
                {
                    waitingList.RemoveAt(i);
                }
            }
        }

        public void writeLevel(AbsLevel lvl, System.IO.BinaryWriter w)
        {
            w.Write((byte)lvl.LevelEnum);
        }

        public AbsLevel readLevel(System.IO.BinaryReader r)
        {
            return levels[r.ReadByte()];
        }

        public void netWriteStatus()
        {
            //When new player joins
            foreach (var m in activeLevels)
            {
                m.netWriteLevelState();
            }
        }

        /// <summary>
        /// Each level need a unique ID to make the storage work
        /// </summary>
        public static int LevelId(LevelEnum lvl)
        {
            switch (lvl)
            {
                //case LevelEnum.Tutorial:
                //    return -100;
                //case LevelEnum.Intro:
                //    return -90;
                case LevelEnum.Tutorial:
                    return -100;
                case LevelEnum.Lobby:
                    return 0;
                case LevelEnum.Debug:
                    return 1;
                case LevelEnum.Creative:
                    return 5;
                //case LevelEnum.FabiansDebug:
                //    return 2;
                case LevelEnum.IntroductionLevel:
                    return 100;
                //case LevelEnum.Mount:
                //    return 110;
                //case LevelEnum.EmoVsTroll:
                //    return 115;
                //case LevelEnum.Wolf:
                //    return 120;
                //case LevelEnum.Statue:
                //    return 200;
                //case LevelEnum.Birds:
                //    return 250;
                //case LevelEnum.Swine:
                //    return 300;
                //case LevelEnum.Desert1:
                //    return 350;
                //case LevelEnum.Desert2:
                //    return 370;
                //case LevelEnum.Barrels:
                //    return 390;
                //case LevelEnum.Orcs:
                //    return 400;
                //case LevelEnum.Elf1:
                //    return 700;
                ////case LevelEnum.Elf2:
                ////    return 710;
                //case LevelEnum.SkeletonDungeon:
                //    return 800;
                //case LevelEnum.Spider1:
                //    return 900;
                //case LevelEnum.Spider2:
                //    return 910;
                //case LevelEnum.EndBoss:
                //    return 1000;
                //case LevelEnum.Challenge_Zombies:
                //    return 1100;
                //case LevelEnum.Lootfest1:
                //    return 1300;
                //case LevelEnum.Challenge_Future:
                //    return 1200;
                //case LevelEnum.HappyNpcs:
                //    return 10000;

                default:
                    return -1;
                    //throw new NotImplementedException("ID missing for " + lvl.ToString());
            }
        }

        public static void LevelSymbol(LevelEnum lvl, out SpriteName symbol, out int? number)
        {
            symbol = SpriteName.MissingImage;
            number = 0;

            if (lvl == LevelEnum.Tutorial)
            {
                symbol = SpriteName.LFTutorialLevelIcon;
                number = null;
            }
            else if (lvl <= LevelEnum.Lobby)
            {
                symbol = SpriteName.LFLobbyIcon;
                number = null;
            }
            else if (lvl < LevelEnum.EndBoss)
            {
                symbol = SpriteName.LFLevelIcon;
                for (int i = 0; i < LfLib.EnemyLevels.Length; ++i)
                {
                    if (LfLib.EnemyLevels[i] == lvl)
                    {
                        number = i + 1;
                    }
                }
            }
            else if (lvl == LevelEnum.EndBoss)
            {
                symbol = SpriteName.LfFinalLevelIcon;
                number = null;
            }
            else
            {
                symbol = SpriteName.LfChallengeLevelIcon;
                int firstChallengeIx = -1;

                for (int i = 0; i < LfLib.EnemyLevels.Length; ++i)
                {
                    //if (LfLib.EnemyLevels[i] == LevelEnum.Challenge_Zombies)
                    //{
                    //    firstChallengeIx = i;
                    //}

                    if (LfLib.EnemyLevels[i] == lvl)
                    {
                        number = i - firstChallengeIx + 1;
                    }
                }
            }
        }

        public static SpriteName LevelDoorIcon(LevelEnum lvl, bool completed)
        {
            //if (lvl == LevelEnum.EndBoss)
            //{
            //    return completed ? SpriteName.LfFinalLevelCompleteIcon : SpriteName.LfFinalLevelUnCompleteIcon;
            //}
            //else if (lvl == LevelEnum.Challenge_Zombies)
            //{
            //    return completed ? SpriteName.LfChallengeLevelCompleteIcon : SpriteName.LfChallengeLevelUnCompleteIcon;
            //}
            //else
            //{
            //    return completed ? SpriteName.LFLevelCompleteIcon : SpriteName.LFLevelUnCompleteIcon;
            //}
            return SpriteName.MissingImage;
        }
    }

    class WaitingForLevel2
    {
        LevelEnum level;
        RequestWorldCallBack2 callback;
        object args;
        VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero;

        public WaitingForLevel2(LevelEnum level, RequestWorldCallBack2 callback, object args,
            VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
        {
            this.level = level;
            this.callback = callback;
            this.args = args;
            this.hero = hero;
        }

        public bool onCreateLevel(AbsLevel level)
        {
            if (level.LevelEnum == this.level)
            {
                callback(level, hero, args);
                return true;
            }
            return false;
        }
    }

    enum LevelEnum
    {
        Debug,
        Tutorial,
        IntroductionLevel,
        Lobby,
        Spider1,
        EndBoss,
        Creative,
        NUM_NON,

        Lootfest1,
        
        //IntroductionLevel,
        //Mount,
        //EmoVsTroll,
        //Statue,
        //Birds,
        //Swine,
        //Desert1,
        //Desert2,
        //Orcs,
        //Barrels,
        //Wolf,
        //Elf1,
        //SkeletonDungeon,
        //Spider1,
        //Spider2,
        
        //Challenge_Zombies,
        //Lootfest1,
        
    }
}
