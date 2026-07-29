using Microsoft.Xna.Framework.Content;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Network;
using VikingEngine.ToGG;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Data
{
    class SaveMeta
    {
        const int Version = 3;

        const int SaveStateCount = 10;
        const int AutoSaveCount = 10;
        public const string ImportSaveFolder = "Import Save";
        SaveIterations saves = new SaveIterations(SaveStateCount);
        SaveIterations autosaves = new SaveIterations(AutoSaveCount);
       

        DataStream.FilePath importSavePath = new DataStream.FilePath(ImportSaveFolder, null, null);
        DataStream.FilePath path = new DataStream.FilePath(Ref.steam.UserCloudPath, $"DSS_savemeta_v{SaveGamestate.Version}", ".mta");

        public GameOverResultCollection gameOverResultCollection = null;

        const int SaveClientStateCount = 20;
        SaveClientIterations clientSaves = new SaveClientIterations(SaveClientStateCount);


        public void CreateImportFolders()
        {
            System.IO.Directory.CreateDirectory(importSavePath.CompleteDirectory);
        }

        public List<string> ListSaveImports()
        {
            var files = System.IO.Directory.GetFiles(importSavePath.CompleteDirectory);
            List<string> list = new List<string>();
            foreach (var f in files)
            {
                if (f.Contains(SaveStateMeta.FileEnd))
                { 
                    list.Add(f);
                }
            }

            return list;
        }

        public void Save(IStreamIOCallback callBack)
        {
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, callBack, true);
        }

        public void Load()
        {
            DataStream.FileToDiskManager.TryReadBinaryIO(path, read);
        }

        public bool LoadClient(PFaction faction)
        {
            double latestTimeDiff = double.MaxValue;
            ClientSaveMeta latest = null;

            foreach (var save in clientSaves.saves)
            {
                if (save != null &&
                    save.host == Ref.netSession.Host().fullId &&
                    save.World.MapId() == DssRef.world.metaData.worldId.MapId() &&
                    save.faction == faction)
                {
                    double timeDiff = DssRef.time.TotalIngameTime().TotalSeconds - save.playTime.TotalSeconds;
                    if (timeDiff < 0)
                    {
                        timeDiff = Math.Abs(timeDiff) * 2f;
                    }

                    if (latest == null || timeDiff < latestTimeDiff)
                    {
                        latest = save;
                    }
                }
            }

            if (latest != null)
            {
                var saveGamestate = new ClientSaveState(latest);
                saveGamestate.load();
                return true;
            }

            return false;
        }


        public List<SaveStateMeta> listSaves()
        {
            List<SaveStateMeta> allSaves = new List<SaveStateMeta>();

            foreach (var state in saves.saves)
            {
                if (state != null)
                {
                    allSaves.Add(state);
                }
            }
            foreach (var state in autosaves.saves)
            {
                if (state != null)
                {
                    allSaves.Add(state);
                }
            }

            var sortedSaveStates = allSaves.OrderBy(state => state.saveDate).Reverse().ToList();

            return sortedSaveStates;
        }

        public int NextSaveIndex(bool auto)
        {
           return (auto ? autosaves : saves).nextIndex;
        }

        public int NextClientSaveIndex()
        {
            return clientSaves.nextIndex;
        }

        public void AddSave(SaveStateMeta save, IStreamIOCallback callback)
        {
            (save.autosave ? autosaves : saves).AddSave(save);
            Save(callback);
        }
        public void AddSave(ClientSaveMeta save)
        {
            clientSaves.AddSave(save);
            Save(null);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            saves.write(w);

            autosaves.write(w); 

            clientSaves.write(w);

            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            FileCheck fileCheck = new FileCheck();
            try
            {
                int version = r.ReadInt32();
                fileCheck.start(version, Version);
                if (version > Version) { return; }

                //if (version == 1)
                //{
                //    if (r.ReadBoolean())
                //    {
                //        var state = new SaveStateMeta(r);
                //        if (state.stateVersion == SaveGamestate.Version)
                //        {
                //            saves.saves[0] = state;
                //        }
                //    }
                //}
                //else
                //{
                    saves.read(r, version);
                    autosaves.read(r, version);

                    if (version >= 3)
                    { 
                        clientSaves.read(r, version);

                        Debug.ReadCheck(r);
                    }
                //}
                fileCheck.end();
            }
            catch (Exception e)
            {
                fileCheck.exception = e;
            }

            IOLib.fileCheck_savemeta = fileCheck;
        }
    }

    class SaveIterations
    {
        public int nextIndex = 0;
        public SaveStateMeta[] saves;

        public SaveIterations(int length)
        {
            saves = new SaveStateMeta[length];
        }

        public void AddSave(SaveStateMeta save)
        {
            saves[Bound.Set(save.index, 0, saves.Length -1)] = save;
            nextIndex = save.index + 1;
            if (nextIndex >= saves.Length)
            { 
                nextIndex = 0;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)nextIndex);
            w.Write((byte)saves.Length);
            
            foreach (var state in saves)
            {
                w.Write(state != null);
                if (state != null)
                {
                    state.write(w);
                }
            }
        }
        public void read(System.IO.BinaryReader r, int version)
        {
            nextIndex = r.ReadByte();
            int length = r.ReadByte();
            
            for (int i = 0; i < length; i++)
            {
                if (r.ReadBoolean())
                {
                    var state = new SaveStateMeta(r);
                    if (state.stateVersion == SaveGamestate.Version)
                    {
                        saves[i] = state;
                    }
                }
            }

        }
    }

    class SaveStateMeta : IStreamIOCallback
    {
        public static readonly string PlayMapDir = DssLib.ContentDir + "PlayMap" + DataStream.FilePath.Dir;

        const int Version = 6;
        public const string FileEnd = ".sav";
        public DateTime saveDate;
        public TimeSpan playTime;
        public int localPlayerCount = 1;
        int difficulty;
        public GameModeMainType gameMode = GameModeMainType.NUM;
        public float setting_foodMulti = -1;
        public float setting_waterMulti = -1;
        public float setting_childMulti = -1;
        public float setting_craftMulti = -1;

        public int metaVersion = Version;
        public int stateVersion= SaveGamestate.Version;
        
        public bool autosave;
        public int index;
        public string playmap = null;
        public string import = null;
        public bool importedWorld = false;

        public WorldMetaData worldmeta = null;

        DataStream.FilePath filepath(bool auto, int index)
        {
            if (import != null)
            {
                return new DataStream.FilePath(SaveMeta.ImportSaveFolder, TextLib.RemoveEnding(import, FileEnd.Length), FileEnd);
            }
            else if (playmap != null)
            {
                return new DataStream.FilePath(PlayMapDir, playmap, FileEnd, false);
            }
            else
            {
                return new DataStream.FilePath(Ref.steam.UserCloudPath, string.Format("DSS_{0}savestate{1}_v{2}", auto ? "auto_" : string.Empty, index, stateVersion), FileEnd);
            }
        }

        public DataStream.FilePath Path => filepath(autosave, index);


        public string TitleString()
        { 
            return (autosave? DssRef.lang.GameMenu_AutoSave : DssRef.lang.Hud_Save) + " " + index.ToString();
        }
        public string InfoString()
        {
            string playTime = HudLib.TimeSpan_LongText(this.playTime);//Engine.LoadContent.CheckCharsSafety(this.playTime.ToString(), LoadedFont.Regular);
            string result = string.Empty;
            if (gameMode != GameModeMainType.NUM)
            {
                LangLib.GameModeText(gameMode, out string caption, out _);
                result += string.Format(DssRef.lang.Language_ItemCount_Colon,DssRef.lang.Settings_GameMode, caption) + Environment.NewLine;
            }
            result += string.Format(DssRef.lang.EndGameStatistics_Time, playTime) + Environment.NewLine;
            if (autosave)
            {
                result += DssRef.lang.GameMenu_AutoSave + Environment.NewLine;
            }

            if (worldmeta != null)
            {
                result += string.Format(DssRef.lang.Settings_TotalDifficulty, difficulty) + Environment.NewLine +
                    DssRef.lang.Lobby_MapSizeTitle + ": " + WorldData.SizeString(worldmeta.mapSize) + Environment.NewLine;
            }

            if (setting_foodMulti > 0)
            {
                result += string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_FoodMultiplier, TextLib.OneDecimal(setting_foodMulti)) + Environment.NewLine +
                    string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_WaterMultiplier, TextLib.OneDecimal(setting_waterMulti)) + Environment.NewLine +
                    string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_ChildMultiplier, TextLib.OneDecimal(setting_childMulti)) + Environment.NewLine +
                    string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_CraftMultiplier, TextLib.OneDecimal(setting_craftMulti)) + Environment.NewLine;

            }

            if (localPlayerCount > 1)
            {
                result += string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Lobby_LocalMultiplayerEdit, localPlayerCount) + Environment.NewLine;
            }

            result += " [" + HudLib.Date(saveDate) + "]";
            
            return result;
        }

        public string ExportString()
        {
            return FilePath.SanitizeFileName( Path.FileName + "_" + string.Format(DssRef.lang.EndGameStatistics_Time, playTime) + "_" + string.Format(DssRef.lang.Settings_DifficultyLevel, difficulty) + "_seed" + worldmeta.worldId.seed);
        }

        public SaveStateMeta()
        {            
        }

        public void netSetup()
        {
            worldmeta = DssRef.world.metaData;
        }

        public void storageSetup()
        {
            playTime = DssRef.time.TotalIngameTime();
            localPlayerCount = DssRef.state.localPlayers.Count;
            difficulty = DssRef.difficulty.TotalDifficulty();
            gameMode = DssRef.difficulty.setting_gameMode;

            setting_foodMulti = DssRef.storage.ruleset.setting_foodMulti;
            setting_waterMulti = DssRef.storage.ruleset.setting_waterMulti;
            setting_childMulti = DssRef.storage.ruleset.setting_childMulti;
            setting_craftMulti = DssRef.storage.ruleset.setting_craftMulti;
            worldmeta = DssRef.world.metaData;
        }

        public SaveStateMeta(bool autosave)
        {
            saveDate = DateTime.Now;
            playTime = DssRef.time.TotalIngameTime();
            localPlayerCount = DssRef.state.localPlayers.Count;
            difficulty = DssRef.difficulty.TotalDifficulty();
            gameMode = DssRef.difficulty.setting_gameMode;
            worldmeta = DssRef.world.metaData;

            this.autosave = autosave;
            this.index = DssRef.storage.meta.NextSaveIndex(autosave);
        }
        public SaveStateMeta(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {        
            
            w.Write(metaVersion);
            w.Write(stateVersion);

            w.Write((byte)gameMode);

            w.Write(setting_foodMulti);
            w.Write(setting_waterMulti);
            w.Write(setting_childMulti);
            w.Write(setting_craftMulti);

            w.Write(autosave);
            w.Write((byte)index);
            w.Write(saveDate.Ticks); 
            w.Write(playTime.Ticks);
            w.Write(localPlayerCount);
            w.Write((short)difficulty);

            if (worldmeta == null)
            {
                worldmeta = DssRef.world.metaData;
            }
            worldmeta.write(w);
            w.Write(importedWorld);

            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            
            metaVersion = r.ReadInt32();
            if (metaVersion > Version) { return; }

            stateVersion = r.ReadInt32();


            if (metaVersion >= 4)
            {
                gameMode = (GameModeMainType)r.ReadByte();
            }

            if (metaVersion >= 5)
            {
                setting_foodMulti = r.ReadSingle();
                setting_waterMulti = r.ReadSingle();
                setting_childMulti = r.ReadSingle();
                setting_craftMulti = r.ReadSingle();
            }

            if (metaVersion == 1)
            {
                autosave = false;
            }
            else
            {
                autosave = r.ReadBoolean();
                index = r.ReadByte();
            }

            saveDate = new DateTime(r.ReadInt64());
            playTime = new TimeSpan(r.ReadInt64());
            localPlayerCount = r.ReadInt32();
            difficulty = r.ReadInt16();

            worldmeta = new WorldMetaData(r);

            if (metaVersion >= 6)
            {
                importedWorld = r.ReadBoolean();
            }

            if (metaVersion >= 5)
            {
                Debug.ReadCheck(r);
            }
        }

        public int CompareTo(SaveStateMeta other)
        {
            if (other == null)
                return 1;
            return saveDate.CompareTo(other.saveDate);
        }

        public void loadImportMeta()
        {
            DataStream.BeginReadWrite.BinaryIO(false, Path, null, readMetaOnly, this, true);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value) 
        {
            ((MainMenuState)Ref.gamestate).continueFromSave(this);
        }

        public void readMetaOnly(System.IO.BinaryReader r)
        {
            SaveVersion version = new SaveVersion();
            version.read(r);

            //META            
            read(r);
            Debug.ReadCheck(r);
        }
    }
}
