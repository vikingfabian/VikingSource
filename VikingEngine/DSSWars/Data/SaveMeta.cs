using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.LootFest.Editor;

namespace VikingEngine.DSSWars.Data
{
    class SaveMeta
    {
        public SaveStateMeta saveState1 = null;

        const int Version = 1;
        DataStream.FilePath path = new DataStream.FilePath(null, "DSS_savemeta", ".sav");

        public void Save(DataStream.IStreamIOCallback callBack)
        {
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, callBack, true);
        }

        public void Load()
        {
            DataStream.DataStreamHandler.TryReadBinaryIO(path, read);
        }

        public void write(System.IO.BinaryWriter w)
        {            
            w.Write(Version);

            w.Write(saveState1 != null);
            if (saveState1 != null)
            {
                saveState1.write(w);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            if (r.ReadBoolean())
            {
                saveState1 = new SaveStateMeta(r);
            }
        }
    }

    class SaveStateMeta
    {
        const int Version = 1;

        DateTime saveDate;
        TimeSpan playTime;
        int localPlayerCount;
        int difficulty;

        int metaVersion = Version;
        int stateVersion= SaveGamestate.Version;

        public WorldMetaData world = null;

        public DataStream.FilePath Filepath => new DataStream.FilePath(null, "DSS_savestate_v" + stateVersion.ToString(), ".sav");

        public string InfoString()
        {
            return string.Format(DssRef.lang.EndGameStatistics_Time, playTime) + Environment.NewLine +
                string.Format(DssRef.lang.Settings_TotalDifficulty, difficulty) + Environment.NewLine +
                DssRef.lang.Lobby_MapSizeTitle + ": " + WorldData.SizeString(world.mapSize) + Environment.NewLine +
                string.Format(DssRef.lang.Lobby_LocalMultiplayerEdit, localPlayerCount) + Environment.NewLine +
                " [" + saveDate.ToLongDateString() + "]";
        }

        public SaveStateMeta()
        {
            saveDate = DateTime.Now;
            playTime = DssRef.time.TotalIngameTime();
            localPlayerCount = DssRef.state.localPlayers.Count;
            difficulty = DssRef.difficulty.TotalDifficulty();
            world = DssRef.world.metaData;

        }
        public SaveStateMeta(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(metaVersion);
            w.Write(stateVersion);

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
            
            saveDate = new DateTime(r.ReadInt64());
            playTime = new TimeSpan(r.ReadInt64());
            localPlayerCount = r.ReadInt32();
            difficulty = r.ReadInt16();

            world = new WorldMetaData(r);
        }

        
    }
}
