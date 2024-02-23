using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Schema;
using VikingEngine.Engine;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.Network;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.Data
{
    class GameStorage
    {
        const int ProfilesCount = 16;
        public const int MaxLocalPlayerCount = 4;
        public int playerCount = 1;
        public bool verticalScreenSplit = true;

        DataStream.FilePath path = new DataStream.FilePath(null, "DSS_profiles", ".set");
        public List<ProfileData> profiles;
        public MapSize mapSize = MapSize.Medium;
        public AiAggressivity aiAggressivity = AiAggressivity.Medium;


        public int aiEconomyLevel = 1;

        public const int DiplomacyDifficultyCount = 3;
        public int diplomacyDifficulty = 1;
        public bool generateNewMaps = false;
        public LocalPlayerStorage[] localPlayers = null;
        public bool honorGuard = true;

        public BossTimeSettings bossTimeSettings = BossTimeSettings.Normal;

        public GameStorage()
        {
            DssRef.storage = this;
            profiles = new List2<ProfileData>(ProfilesCount);

            for (int i = 0; i < ProfilesCount; ++i)
            {
                profiles.Add(new ProfileData(FactionType.Player, i));
            }

            localPlayers = new LocalPlayerStorage[MaxLocalPlayerCount];
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i] = new LocalPlayerStorage(i);
            }
        }

        public void Load()
        {
            DataStream.DataStreamHandler.TryReadBinaryIO(path, read);
        }

        public void Save(DataStream.IStreamIOCallback callBack)
        {
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, callBack, true);
        }

        public double DifficultyLevelPerc()
        {
            double levelPerc = DssLib.AiEconomyLevel[DssRef.storage.aiEconomyLevel];
            int aggdiff = (int)DssRef.storage.aiAggressivity - (int)AiAggressivity.Medium;
            levelPerc *= 1.0 + aggdiff * 0.5;

            double bossTimeDiff = DssRef.storage.bossTimeSettings - BossTimeSettings.Normal;
            levelPerc *= 1.0 - bossTimeDiff * 0.25;

            double diplomacyDiff = DssRef.storage.diplomacyDifficulty - 1;
            levelPerc *= 1.0 + diplomacyDiff * 0.5;

            if (!DssRef.storage.honorGuard)
            {
                levelPerc *= 1.25;
            }

            return levelPerc;
        }

        public void write(System.IO.BinaryWriter w)
        {
            const int Version = 9;

            w.Write(Version);

            foreach (var p in profiles)
            {
                p.write(w);
            }

            w.Write((int)mapSize);

            w.Write(verticalScreenSplit);

            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i].write(w);
            }

            w.Write((int)aiAggressivity);
            w.Write(aiEconomyLevel);

            w.Write(generateNewMaps);
            w.Write(honorGuard);
            w.Write(diplomacyDifficulty);
            w.Write((int)bossTimeSettings);
        }
        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version < 4)
            {
                return;
            }

            for (int i = 0; i < ProfilesCount; ++i)
            {
                profiles[i].read(r);
            }

            mapSize = (MapSize)r.ReadInt32();

            verticalScreenSplit = r.ReadBoolean();

            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i].read(r, version);
            }

            aiAggressivity = (AiAggressivity)r.ReadInt32();
            aiEconomyLevel = Bound.Set(r.ReadInt32(), 0, DssLib.AiEconomyLevel.Length - 1);

            if (version >= 6)
            {
                generateNewMaps = r.ReadBoolean();
            }
            if (version >= 7)
            {
                honorGuard = r.ReadBoolean();
            }
            if (version >= 8)
            {
                diplomacyDifficulty = r.ReadInt32();
            }
            if (version >= 9)
            {
                bossTimeSettings = (BossTimeSettings)r.ReadInt32();
            }
        }

        public void checkPlayerDoublettes()
        {
            for (int i = 0; i < MaxLocalPlayerCount - 1; ++i)
            {
                checkPlayerDoublettes(i);
            }
        }
        public void checkPlayerDoublettes(int masterIndex)
        {
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                if (i != masterIndex)
                {
                    localPlayers[i].checkDoublette(i, localPlayers);
                }
            }
        }

        public LocalPlayerStorage PlayerFromScreenIndex(int screen)
        {
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                if (localPlayers[i].screenIndex == screen)
                {
                    return localPlayers[i];
                }
            }

            throw new Exception("Missing screen " + screen.ToString());
        }

        public void checkConnected()
        {
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                if (!localPlayers[i].inputSource.Connected)
                {
                    localPlayers[i].inputSource = InputSource.Empty;
                }
            }
        }
    }


}
