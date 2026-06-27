using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;

namespace VikingEngine.DSSWars.Data
{
    class ClientSaveState : AbsUpdateable, IStreamIOCallback
    {
        MemoryStreamHandler memoryStream = new MemoryStreamHandler();

        bool dataReady = false;
        public bool complete = false;
        ClientSaveMeta meta;

        public ClientSaveState(ClientSaveMeta meta)
             : base(false)
        {
            this.meta = meta;
        }

        public void save()
        {
            AddToUpdateList();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var w = memoryStream.GetWriter();
                    writeGameState(w);
                    dataReady = true;
                }
                catch (Exception e)
                {
                    DebugExtensions.BlueScreen.ThreadException = e;
                }
            });
        }

        public void load()
        {
            DataStream.BeginReadWrite.BinaryIO(false, meta.Path, null, readGameState, this, true);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            //TODO error handling
            if (save)
            {
                DssRef.storage.meta.AddSave(meta);
            }

            complete = true;
        }
                
        public void writeGameState(System.IO.BinaryWriter w)
        {
            new SaveVersion(SaveGamestate.Version, SaveGamestate.SubVersion).write(w);

            //Save build orders and pins
            w.Write(DssRef.state.localPlayers.Count);

            foreach (var p in DssRef.state.localPlayers)
            {
                p.orders.writeGameState(w);

                p.writePins(w);

                Debug.WriteCheck(w);
            }
        }

        public void readGameState(System.IO.BinaryReader r)
        {           
            SaveVersion version = new SaveVersion();
            version.read(r);

            int localPlayersCount = lib.SmallestValue(r.ReadInt32(), DssRef.state.localPlayers.Count);

            for (int i = 0; i < localPlayersCount; i++)
            {
                DssRef.state.localPlayers[i].orders.readGameState(i, r, version.sub, null);

                DssRef.state.localPlayers[i].readPins(r, version.sub);

                Debug.ReadCheck(r);
            }
        }

        public override void Time_Update(float time_ms)
        {
            if (dataReady)
            {
                dataReady = false;
                var path = meta.Path;
                System.IO.Directory.CreateDirectory(path.CompleteDirectory);
                new WriteByteArray(path, memoryStream, this);
            }
        }

    }

}
