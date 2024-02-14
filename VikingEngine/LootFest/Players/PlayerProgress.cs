using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.Players
{
    class PlayerProgress
    {
        public static readonly KeyValuePair<BabyLocation, int>[] BabyLocation_Id = new KeyValuePair<BabyLocation, int>[]
        {
            new KeyValuePair<BabyLocation, int>(BabyLocation.Introduction, 100),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Goblin, 200),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Horse, 300),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Hog, 400),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Spider, 500),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Barrels, 600),
            new KeyValuePair<BabyLocation, int>(BabyLocation.Elf, 700),
            new KeyValuePair<BabyLocation, int>(BabyLocation.EmoVsTroll, 800),
            new KeyValuePair<BabyLocation, int>(BabyLocation.FinalBoss, 900),
        };

        static readonly KeyValuePair<ProgressPoint, int>[] ProgressPoint_Id = new KeyValuePair<ProgressPoint, int>[]
        {
            new KeyValuePair<ProgressPoint, int>(ProgressPoint.TutorialLobby, 100),
            new KeyValuePair<ProgressPoint, int>(ProgressPoint.MainLobby, 200),
        };

        public const int BossCastleUnlockBabyCount = 7;

        public bool[] achievements = new bool[(int)Data.AchievementIndex.NUM_ACHIEVEMENTS];
        public CardCaptures[] cardCollection;

        public ArrayEnumWithIdStorage<BabyLocation> StoredBabyLocations = new ArrayEnumWithIdStorage<BabyLocation>(BabyLocation_Id);
        public ArrayEnumWithIdStorage<ProgressPoint> StoredProgressPoints = new ArrayEnumWithIdStorage<ProgressPoint>(ProgressPoint_Id);

        public PlayerProgress()
        {
            cardCollection = new CardCaptures[(int)CardType.NumNon];
            for (int i = 0; i < cardCollection.Length; ++i)
            {
                if (CardCaptures.CardAvailableTypeList[i] == CardAvailableType.StartDeck)
                {
                    cardCollection[i] = new CardCaptures(2);
                }
            }


            //var test = arraylib.DictionaryKeyFromValue(BabyLocation_Id, 200);
            //OLD
            //completedLevels = new CompletedLevel[(int)LevelEnum.NUM_NON];
            //for (int i = 0; i < completedLevels.Length; ++i)
            //{
            //    completedLevels[i] = new CompletedLevel();
            //}
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            //Version 18
        
            w.Write(achievements.Length);
            foreach (var a in achievements)
            {
                w.Write(a);
            }

            w.Write(cardCollection.Length);
            foreach (var c in cardCollection)
            {
                c.Write(w);
            }

            //w.Write(StoredProgressPoints.Length);
            //foreach (var m in StoredProgressPoints)
            //{
            //    w.Write(m);
            //}

            //w.Write(babyRescues.Length);
            //foreach (var m in babyRescues)
            //{
            //    w.Write(m);
            //}
            
            StoredBabyLocations.write(w);
            StoredProgressPoints.write(w);
        }

        public void ReadStream(System.IO.BinaryReader r, int version)
        {
            //if (version <= 17)
            //{
            //    ReadStreamVer17(r, version);
            //}
            //else
            //{ //LATEST
            int achiveCount = r.ReadInt32();
            for (int i = 0; i < achiveCount; ++i)
            {
                achievements[i] = r.ReadBoolean();
            }

            int cardCollCount = r.ReadInt32();
            for (int i = 0; i < cardCollCount; ++i)
            {
                cardCollection[i].Read(r, version, i);
            }

            if (version >= 18)
            {
                StoredBabyLocations.read(r);
                StoredProgressPoints.read(r);
                
            }
        }


        public void readCompletedLevels_old(System.IO.BinaryReader r, int version)
        {
            //Dictionary<int, LevelEnum> idAndLevel = new Dictionary<int, LevelEnum>((int)LevelEnum.NUM_NON);
            //for (LevelEnum lvl = 0; lvl < LevelEnum.NUM_NON; ++lvl)
            //{
            //    idAndLevel.Add(LevelsManager.LevelId(lvl), lvl);
            //}


            int storedLvlCount = r.ReadInt32();

            for (int i = 0; i < storedLvlCount; ++i)
            {
                int id = r.ReadInt32();
                var cl = new CompletedLevel(r, version);
            }
        }

        public int CardTypesCollectionCount(out int totalCollectableTypes)
        {
            totalCollectableTypes = 0;
            int collectedTypes = 0;
            for (int i = 0; i < cardCollection.Length; ++i)
            {
                if (CardCaptures.CardAvailableTypeList[i] == CardAvailableType.Collectable)
                {
                    totalCollectableTypes++;
                    if (cardCollection[i].TotalCount > 0)
                    {
                        collectedTypes++;
                    }
                }
            }
            return collectedTypes;
        }

        public bool canTravelTo(TeleportLocationId loc)
        {
            switch (loc)
            {
                default:
                    return true;
            }
        }

        public static SpriteName BabyLocationIcon(BabyLocation location)
        {
            switch (location)
            {
                case BabyLocation.Introduction: return SpriteName.cgStatueBoss;
                case BabyLocation.Goblin: return SpriteName.cgGoblinKing;
                case BabyLocation.Horse: return SpriteName.cgGoblinWolfrider;
                case BabyLocation.Hog: return SpriteName.cgOldHog;
                case BabyLocation.Spider: return SpriteName.cgSpiderBot;
                case BabyLocation.Barrels: return SpriteName.cgStatueBoss;
                case BabyLocation.Elf: return SpriteName.cgElfKnight;
                case BabyLocation.EmoVsTroll: return SpriteName.cgTrollBoss;
                case BabyLocation.FinalBoss: return SpriteName.cgBigOrcBoss;
            }

            return SpriteName.MissingImage;
        }

        public bool teleportIsLocked(TeleportLocationId toLocation)
        {
            bool locked = false;

            if (toLocation == TeleportLocationId.BossCastle)
            {
                locked = StoredBabyLocations.TrueCount() < Players.PlayerProgress.BossCastleUnlockBabyCount;
            }

            return locked;
        }
    }

    enum ProgressPoint
    {
        TutorialLobby,
        MainLobby,
        NUM
    }

    enum BabyLocation
    {
        Introduction,
        Goblin,
        Horse,
        Hog,
        Spider,
        Barrels,
        Elf,
        EmoVsTroll,
        FinalBoss,
        NUM,
    }

    enum UnlockType
    {
        Cards,
        Cape,
        NUM_NONE
    }
}
