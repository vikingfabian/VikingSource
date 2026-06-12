using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Presentation;

namespace VikingEngine.DSSWars.Data
{
    //class ClientSaveMeta
    //{
    //}

    class SaveClientIterations
    {
        public int nextIndex = 0;
        public SaveClientStateMeta[] saves;

        public SaveClientIterations(int length)
        {
            saves = new SaveClientStateMeta[length];
        }

        public void AddSave(SaveClientStateMeta save)
        {
            saves[Bound.Set(save.index, 0, saves.Length - 1)] = save;
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
                    var state = new SaveClientStateMeta(r);
                    if (state.stateVersion == SaveGamestate.Version)
                    {
                        saves[i] = state;
                    }
                }
            }

        }
    }

    class SaveClientStateMeta : IStreamIOCallback
    {
        const int Version = 1;
        public int stateVersion = SaveGamestate.Version;
        public const string FileEnd = ".cls";
        public DateTime saveDate;
        public TimeSpan playTime;

        public int index;
        public int metaVersion = Version;
        public ulong host;
        public WorldMetaId World;
        public int faction;

        DataStream.FilePath filepath(int index)
        {
            return new DataStream.FilePath(Ref.steam.UserCloudPath, $"DSS_clientstate{index}_v{stateVersion}", FileEnd);   
        }

        public DataStream.FilePath Path => filepath(index);

        public SaveClientStateMeta(TimeSpan playTime, WorldMetaId World, int faction)
        {
            saveDate = DateTime.Now;
            this.World = World;
            host = Ref.netSession.Host().FullId;
            this.playTime = playTime;

            this.index = DssRef.storage.meta.NextClientSaveIndex();
            this.faction = faction;
        }

        public SaveClientStateMeta(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(metaVersion);
            w.Write(stateVersion);

            w.Write((byte)index);
            w.Write(saveDate.Ticks);
            w.Write(playTime.Ticks);
          
            w.Write(host);
            World.write(w);
            w.Write((ushort)faction);

            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            metaVersion = r.ReadInt32();
            if (metaVersion > Version) { return; }

            stateVersion = r.ReadInt32();

            index = r.ReadByte();
            saveDate = new DateTime(r.ReadInt64());
            playTime = new TimeSpan(r.ReadInt64());
            
            host = r.ReadUInt64();
            World.read(r);
            faction = r.ReadUInt16();

            Debug.ReadCheck(r);            
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            
        }
    }
}
