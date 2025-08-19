using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Schema;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
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
        public bool centralGold = true;
        public bool generateNewMaps = true;
        public bool autoSave = true;
        public int runTutorial_1short_2normal = 2;
        public bool speed5x = false;
        public bool longerBuildQueue = false;

        public LocalPlayerStorage[] localPlayers = null;
        public int selectedPlayer = 0;
        public ProfileStorage profileStorage;
        public FlagStorage flagStorage;
        public CharacterStorage characterStorage;
        public SaveMeta meta = null;
        public float multiplayerGameSpeed = 1;

        public MapSettingsStorage mapSettings = MapSettingsStorage.Default;
        

        public GameStorage()
        {
            //DssRef.storage = this;

            flagStorage = new FlagStorage();
            characterStorage = new CharacterStorage();
            profileStorage = new ProfileStorage();
            meta = new SaveMeta();

            localPlayers = new LocalPlayerStorage[MaxLocalPlayerCount];
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i] = new LocalPlayerStorage(i);
            }

#if DEMO
            demoSetup();

#else
            defaultGameSettings();
#endif
        }

        public void defaultGameSettings()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
        }

        void demoSetup()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
            mapSettings = MapSettingsStorage.Default;
            mapSettings.customSeed = true;
            mapSettings.seed = 1;
        }

        public void multiplayerGameSpeedToMenu(RichBoxContent content, RichMenu menu)
        {
            var options = new List<float>
            {
                1.0f,
                1.5f,
                2f,
                3f,
                4f,
            };


            DropDownBuilder dropDown = new DropDownBuilder("mp speed");
            foreach (var item in options)
            {
                dropDown.AddOption(TextLib.OneDecimal(item), item == DssRef.storage.multiplayerGameSpeed, item == 1f, new RbAction1Arg<float>((float value) =>
                {
                    DssRef.storage.multiplayerGameSpeed = value;
                    Ref.SetGameSpeed(value);
                    DssRef.storage.Save(null);
                }, item), null);
            }
            dropDown.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Input_GameSpeed, menu);
            //new GuiOptionsList<float>(SpriteName.NO_IMAGE, DssRef.lang.Input_GameSpeed, options, multiplayerGameSpeedProperty, layout);
        }

        //float multiplayerGameSpeedProperty(bool set, float value)
        //{
        //    if (set)
        //    {
        //        DssRef.storage.multiplayerGameSpeed = value;
        //        Ref.SetGameSpeed(value);
        //        DssRef.storage.Save(null);
        //    }
        //    return DssRef.storage.multiplayerGameSpeed;
        //}

        public void Load()
        {
            DataStream.FileToDiskManager.TryReadBinaryIO(path, read);
            if (StartupSettings.Saves)
            {
                meta.Load();
            }
            flagStorage.Load();
            characterStorage.Load();
            profileStorage.Load();
        }

        public void Save(IStreamIOCallback callBack)
        {
            try
            {
                System.IO.Directory.CreateDirectory(path.CompleteDirectory);
            }
            catch (Exception ex)
            {
                IOLib.fileCheck_gamestorage.createFolderFail = false;
                IOLib.fileCheck_gamestorage.exception = ex;
                return;
            }
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

        const int Version = 25;
        public void write(System.IO.BinaryWriter w, bool gamestate = false)
        {
           

            w.Write(Version);

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
            
            w.Write((byte)runTutorial_1short_2normal);

            w.Write(speed5x);
            w.Write(longerBuildQueue);
            w.Write(centralGold);

            mapSettings.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            read(r, false);
        }
        public void read(System.IO.BinaryReader r, bool gamestate)
        {
            FileCheck fileCheck = new FileCheck();
            try
            {
                int version = r.ReadInt32();
                fileCheck.start(version, Version);
                if (version > Version || version <= 4)
                {
                    return;
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


                generateNewMaps = r.ReadBoolean();
                autoSave = r.ReadBoolean();

                multiplayerGameSpeed = r.ReadSingle();

                DssRef.difficulty.read(r, version);

                if (version >= 15)
                {
                    runTutorial_1short_2normal = r.ReadByte();
                }

                if (version >= 18)
                {
                    speed5x = r.ReadBoolean();
                }
                if (version >= 19)
                {
                    longerBuildQueue = r.ReadBoolean();
                }
                if (version >= 21)
                {
                    centralGold = r.ReadBoolean();
                }
                mapSettings.read(r, version);

                generateNewMaps = true;//temp
                fileCheck.end();

#if DEMO
                demoSetup();
#endif
            }
            catch (Exception e)
            {
                fileCheck.exception = e;
            }

            IOLib.fileCheck_gamestorage = fileCheck;
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

        public PlayerProfile GetHostProfile()
        {
            return profileStorage.profiles[localPlayers[0].profileIndex];
        }
        public void SetHostProfile(PlayerProfile profile)
        {
            profileStorage.profiles[localPlayers[0].profileIndex] = profile;
        }
    }

    
}
