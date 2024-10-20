using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.LootFest.Editor;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Data
{
    


    class SaveMeta
    {
        const int Version = 2;

        const int SaveStateCount = 10;
        const int AutoSaveCount = 10;

        //public SaveStateMeta[] saveStates = new SaveStateMeta[SaveStateCount];
        //public SaveStateMeta[] autoSaveStates = new SaveStateMeta[AutoSaveCount];
        SaveIterations saves = new SaveIterations(SaveStateCount);
        SaveIterations autosaves = new SaveIterations(AutoSaveCount);


        DataStream.FilePath path = new DataStream.FilePath(Ref.steam.UserCloudPath, "DSS_savemeta", ".sav");

        public void Save(DataStream.IStreamIOCallback callBack)
        {
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, callBack, true);
        }

        public void Load()
        {
            DataStream.DataStreamHandler.TryReadBinaryIO(path, read);
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

        public void AddSave(SaveStateMeta save, IStreamIOCallback callback)
        {
            (save.autosave ? autosaves : saves).AddSave(save);
            Save(callback);
        }

        public void write(System.IO.BinaryWriter w)
        {            
            w.Write(Version);

            saves.write(w);
            autosaves.write(w); 
            //writeStates(saveStates);
            //writeStates(autoSaveStates);

            //void writeStates(SaveStateMeta[] states)
            //{ 
            //    w.Write((byte)states.Length);
            //    foreach (var state in states)
            //    {
            //        w.Write(state != null);
            //        if (state != null)
            //        {
            //            state.write(w);
            //        }
            //    }
            //}
        }



        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            if (version == 1)
            {
                if (r.ReadBoolean())
                {
                    var state = new SaveStateMeta(r);
                    if (state.stateVersion == SaveGamestate.Version)
                    {
                        saves.saves[0] = state;
                    }
                }
            }
            else
            {
                saves.read(r, version);
                autosaves.read(r, version);

            }

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
            saves[save.index] = save;
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

    class SaveStateMeta
    {
        const int Version = 2;

        public DateTime saveDate;
        public TimeSpan playTime;
        public int localPlayerCount;
        int difficulty;

        int metaVersion = Version;
        public int stateVersion= SaveGamestate.Version;
        
        public bool autosave;
        public int index;
        

        public WorldMetaData world = null;

        DataStream.FilePath filepath(bool auto, int index)
        {
           return new DataStream.FilePath(Ref.steam.UserCloudPath, string.Format("DSS_{0}savestate{1}_v{2}", auto? "auto_":string.Empty, index, stateVersion), ".sav");
        }

        public DataStream.FilePath Path => filepath(autosave, index);


        public string TitleString()
        { 
            return (autosave? DssRef.lang.GameMenu_AutoSave : DssRef.lang.GameMenu_SaveState) + " " + index.ToString();
        }
        public string InfoString()
        {
            string playTime = HudLib.TimeSpan(this.playTime);//Engine.LoadContent.CheckCharsSafety(this.playTime.ToString(), LoadedFont.Regular);
            string result = string.Format(DssRef.lang.EndGameStatistics_Time, playTime) + Environment.NewLine;
            if (autosave)
            {
                result += DssRef.lang.GameMenu_AutoSave + Environment.NewLine;
            }
            result += string.Format(DssRef.lang.Settings_TotalDifficulty, difficulty) + Environment.NewLine +
                DssRef.lang.Lobby_MapSizeTitle + ": " + WorldData.SizeString(world.mapSize) + Environment.NewLine +
                string.Format(DssRef.lang.Lobby_LocalMultiplayerEdit, localPlayerCount) + Environment.NewLine +
                " [" + HudLib.Date(saveDate) + "]";
            //" [" + Engine.LoadContent.CheckCharsSafety(saveDate.ToLongDateString(), LoadedFont.Regular) + "]";



            return result;
        }

        public SaveStateMeta(bool autosave)
        {
            saveDate = DateTime.Now;
            playTime = DssRef.time.TotalIngameTime();
            localPlayerCount = DssRef.state.localPlayers.Count;
            difficulty = DssRef.difficulty.TotalDifficulty();
            world = DssRef.world.metaData;

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

            w.Write(autosave);
            w.Write((byte)index);
            w.Write(saveDate.Ticks); 
            w.Write(playTime.Ticks);
            w.Write(localPlayerCount);
            w.Write((short)difficulty);

            world.write(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            
            metaVersion = r.ReadInt32();
            stateVersion = r.ReadInt32();

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

            world = new WorldMetaData(r);
        }

        public int CompareTo(SaveStateMeta other)
        {
            if (other == null)
                return 1;
            return saveDate.CompareTo(other.saveDate);
        }
    }
}
