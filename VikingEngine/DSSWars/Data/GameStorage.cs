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
        //public bool verticalScreenSplit = true;

        DataStream.FilePath path = new DataStream.FilePath(Ref.steam.UserCloudPath, "DSS_gameoptions", ".sav");
        
        public bool autoSave = true;
        public bool runTutorial = true;
        public bool speed5x = false;
        public bool blockImportAchievements = true;
        
        public LocalPlayerStorage[] localPlayers = null;
        public int selectedPlayer = 0;
        public ProfileStorage profileStorage;
        public FlagStorage flagStorage;
        public CharacterStorage characterStorage;
        public SaveMeta meta = null;
        public float multiplayerGameSpeed = 1;
        public bool generateNewMaps = true;

        public MapSettingsStorage mapSettings = MapSettingsStorage.Default;
        public MetaProgression metaProgression = new MetaProgression(); 
        public GameRuleset gameRuleset = new GameRuleset();

        public List<int> mutedSongs = new List<int>();

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
           gameRuleset.defaultGameSettings();
#endif
        }

        void demoSetup()
        {
            gameRuleset.demoSetup();
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

            if (!metaProgression.unlockedDangerousSettings)
            {
                DssRef.difficulty.setting_foodMulti = 1;
                DssRef.difficulty.setting_waterMulti = 1;
                DssRef.difficulty.setting_childMulti = 1;
                DssRef.difficulty.setting_craftMulti = 1;
            }
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
        public const int Version = 33;
        public void writeGameSetup(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            gameRuleset.write(w);
            DssRef.difficulty.write(w);
        }
        public void readGameSetup(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            gameRuleset.read(r);
            DssRef.difficulty.read(r, version);
        }

        
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);
                        
            metaProgression.write(w);
            writeGameSetup(w);

            //w.Write(verticalScreenSplit);
            for (int i = 0; i < MaxLocalPlayerCount; ++i)
            {
                localPlayers[i].write(w);
            }


            w.Write(generateNewMaps);
            w.Write(autoSave);
            w.Write(multiplayerGameSpeed);
            //DssRef.difficulty.write(w);

            //w.Write((byte)runTutorial_1short_2normal);
            w.Write(runTutorial);

            w.Write(speed5x);

            gameRuleset.write(w);
            mapSettings.write(w);

            w.Write(mutedSongs.Count);
            foreach (var s in mutedSongs)
            {
                w.Write(s);
            }
            w.Write(blockImportAchievements);

            Debug.WriteCheck(w);
            
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

                if (version > Version || version == 32)
                {
                    return;
                }


                fileCheck.start(version, Version);

                if (version <= 27)
                {
                    readOld(r, version, gamestate);
                    return;
                }

                metaProgression.read(r);

                if (version > Version)
                {
                    return;
                }

                readGameSetup(r);

                if (!gamestate)
                {
                    if (version < 31)
                    {
                        bool verticalScreenSplit = r.ReadBoolean();
                    }

                    for (int i = 0; i < MaxLocalPlayerCount; ++i)
                    {
                        localPlayers[i].read(r, version);
                    }
                }

                generateNewMaps = r.ReadBoolean();
                autoSave = r.ReadBoolean();

                multiplayerGameSpeed = r.ReadSingle();

                
                

                if (version < 33)
                {
                    DssRef.difficulty.read(r, version);
                    runTutorial = r.ReadByte() > 0;
                }
                else
                {
                    runTutorial = r.ReadBoolean();
                }

                speed5x = r.ReadBoolean();

                gameRuleset.read(r);
                mapSettings.read(r, version);

                generateNewMaps = true;

                if (version >= 29)
                {
                    mutedSongs.Clear();
                    int mutedSongsCount = r.ReadInt32();
                    for (int i = 0; i < mutedSongsCount; ++i)
                    { 
                        mutedSongs.Add(r.ReadInt32());
                    }
                }

                if (version >= 30)
                {
                    blockImportAchievements = r.ReadBoolean();
                }

                Debug.ReadCheck(r);

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

        

        public void readOld(System.IO.BinaryReader r, int version, bool gamestate)
        {
            FileCheck fileCheck = new FileCheck();
            try
            {
                //int version = r.ReadInt32();
                fileCheck.start(version, Version);
                
                if (version > Version || version <= 4)
                {
                    return;
                }

                gameRuleset.mapSize = (MapSize)r.ReadInt32();

                if (!gamestate || version < 16)
                {
                    bool verticalScreenSplit = r.ReadBoolean();

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
                    runTutorial = r.ReadByte() > 0;
                }

                if (version >= 18)
                {
                    speed5x = r.ReadBoolean();
                }
                if (version >= 19)
                {
                    var longerBuildQueue = r.ReadBoolean();
                }
                if (version >= 21)
                {
                    gameRuleset.centralGold = r.ReadBoolean();
                }
                mapSettings.read(r, version);

                generateNewMaps = true;//temp


                if (version == 26)
                {
                    metaProgression.totalGameTimeMinutes = r.ReadInt64();
                    metaProgression.unlockedDangerousSettings = r.ReadBoolean();
                }

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
