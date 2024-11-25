using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Schema;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Profile;
using VikingEngine.Engine;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.Network;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.Data
{
    class GameStorage
    {        
        public const int MaxLocalPlayerCount = 4;
        public int playerCount = 1;
        public bool verticalScreenSplit = true;

        DataStream.FilePath path = new DataStream.FilePath(Ref.steam.UserCloudPath, "DSS_gameoptions", ".sav");
       
        public MapSize mapSize = MapSize.Medium;
        public bool generateNewMaps = false;
        public bool autoSave = true;
        public bool runTutorial = true;
        public bool speed5x = false;
        public bool longerBuildQueue = false;

        public LocalPlayerStorage[] localPlayers = null;
        public Profile.FlagStorage flagStorage;
        public SaveMeta meta = null;
        public float multiplayerGameSpeed = 2;

        public GameStorage()
        {
            DssRef.storage = this;
            flagStorage = new FlagStorage();
            meta = new SaveMeta();

            localPlayers = new LocalPlayerStorage[MaxLocalPlayerCount];
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i] = new LocalPlayerStorage(i);
            }
        }

        public void Load()
        {
            DataStream.DataStreamHandler.TryReadBinaryIO(path, read);
            meta.Load();
            flagStorage.Load();
        }

        public void Save(DataStream.IStreamIOCallback callBack)
        {
            System.IO.Directory.CreateDirectory(path.CompleteDirectory);
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, callBack, true);
        }

        //public double DifficultyLevelPerc()
        //{
        //    double levelPerc = DssLib.AiEconomyLevel[aiEconomyLevel];
        //    int aggdiff = (int)aiAggressivity - (int)AiAggressivity.Medium;
        //    levelPerc *= 1.0 + aggdiff * 0.25;

        //    double bossTimeDiff = bossTimeSettings - BossTimeSettings.Normal;
        //    levelPerc *= 1.0 - bossTimeDiff * 0.25;

        //    double bossSizeDiff = bossSize - BossSize.Medium;
        //    levelPerc *= 1.0 - bossSizeDiff * 0.25;

        //    double diplomacyDiff = DssRef.storage.diplomacyDifficulty - 1;
        //    levelPerc *= 1.0 + diplomacyDiff * 0.25;

        //    if (!honorGuard)
        //    {
        //        levelPerc *= 1.25;
        //    }

        //    if (!allowPauseCommand)
        //    {
        //        levelPerc *= 1.5;
        //    }

        //    return levelPerc;
        //}
        public void write(System.IO.BinaryWriter w)
        {
            write(w, false);
        }
        public void write(System.IO.BinaryWriter w, bool gamestate = false)
        {
            const int Version = 19;

            w.Write(Version);

            //foreach (var p in profiles)
            //{
            //    p.write(w);
            //}

            w.Write((int)mapSize);

            if (!gamestate)
            {
                w.Write(verticalScreenSplit);
                for (int i = 0; i < MaxLocalPlayerCount; ++i)
                {
                    localPlayers[i].write(w);
                }
            }

            w.Write(generateNewMaps);
            w.Write(autoSave);
            w.Write(multiplayerGameSpeed);
            DssRef.difficulty.write(w);   
            
            w.Write(runTutorial);

            w.Write(speed5x);
            w.Write(longerBuildQueue);
        }

        public void read(System.IO.BinaryReader r)
        {
            read(r, false);
        }
        public void read(System.IO.BinaryReader r, bool gamestate)
        {
            int version = r.ReadInt32();
            if (version <= 4)
            {
                return;
            }

            if (version < 12)
            {
                flagStorage.old_read(r);
                //for (int i = 0; i < ProfilesCount; ++i)
                //{
                //    flagDesigns[i].read(r);
                //}
            }
            mapSize = (MapSize)r.ReadInt32();

            if (!gamestate || version < 16)
            {
                verticalScreenSplit = r.ReadBoolean();

                for (int i = 0; i < MaxLocalPlayerCount; ++i)
                {
                    localPlayers[i].read(r, version);
                }
            }

            if (version >= 11)
            {
                generateNewMaps = r.ReadBoolean();
                if (version >= 13)
                {
                    autoSave = r.ReadBoolean();
                }
                if (version >= 14)
                {
                    multiplayerGameSpeed = r.ReadSingle();
                }
                DssRef.difficulty.read(r, version);
            }

            if (version >= 15)
            {
                runTutorial = r.ReadBoolean();
            }
            
            if (version < 16)
            {
                runTutorial = true;
            }

            if (version >= 18)
            { 
                speed5x = r.ReadBoolean();
            }
            if (version >= 19)
            {
                longerBuildQueue = r.ReadBoolean();
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
