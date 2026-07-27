using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Work;

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

        struct CityClientSaveMeta
        {
            public int cityIndex;
            public long memoryStart;
            public long memoryLength;

            public void write(System.IO.BinaryWriter w)
            {
                w.Write((ushort)cityIndex);
                w.Write((ushort)memoryLength);
            }

            public void read(System.IO.BinaryReader r)
            {
                cityIndex = r.ReadUInt16();
                memoryLength = r.ReadUInt16();
            }
        }
        public void writeGameState(System.IO.BinaryWriter w)
        {
            new SaveVersion(SaveGamestate.Version, SaveGamestate.SubVersion).write(w);

            w.Write(DssRef.state.localPlayers.Count);

            foreach (var p in DssRef.state.localPlayers)
            {
                p.orders.writeGameState(w);

                p.writePins(w);
                Debug.WriteCheck(w);

                p.pfaction.GetFaction().writeClientState(w);
                Debug.WriteCheck(w);

                var f = p.pfaction.GetFaction();
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();

                List<CityClientSaveMeta> clientSaveMetas = new List<CityClientSaveMeta>(f.cities.Count);
                MemoryStreamHandler memory = new MemoryStreamHandler();
                var memW = memory.GetWriter();

                while (citiesC.Next(ref f.cities, DssRef.world.cities, out City city))
                {
                    var meta = new CityClientSaveMeta() { cityIndex = city.myIndex, memoryStart = memW.BaseStream.Position };

                    city.writeClientState(memW);
                    meta.memoryLength = memW.BaseStream.Position - meta.memoryStart;
                    clientSaveMetas.Add(meta);
                    //city.workTemplate.onFactionChange(city, workTemplate, true);
                }


                w.Write((ushort)clientSaveMetas.Count);
                foreach (var c in clientSaveMetas)
                {
                    c.write(w);
                }

                memory.WriteSaveFile(w);

                Debug.WriteCheck(w);
            }


        }

        public void readGameState(System.IO.BinaryReader r)
        {
            //Loads after handover

            SaveVersion version = new SaveVersion();
            version.read(r);

            int localPlayersCount = lib.SmallestValue(r.ReadInt32(), DssRef.state.localPlayers.Count);

            for (int i = 0; i < localPlayersCount; i++)
            {
                var p = DssRef.state.localPlayers[i];
                p.orders.readGameState(i, r, version.sub, null);

                p.readPins(r, version.sub);
                Debug.ReadCheck(r);

                if (version.sub >= 131)
                {
                    var f = p.pfaction.GetFaction();
                    f.readClientState(r, version.sub);
                    Debug.ReadCheck(r);

                    int clientSaveMetasCount = r.ReadUInt16();
                    List<CityClientSaveMeta> clientSaveMetas = new List<CityClientSaveMeta>(clientSaveMetasCount);
                    for (int metaIx = 0; metaIx < clientSaveMetasCount; metaIx++)
                    {
                        CityClientSaveMeta meta = new CityClientSaveMeta();
                        meta.read(r);
                        clientSaveMetas.Add(meta);
                    }

                    MemoryStreamHandler memory = new MemoryStreamHandler();
                    memory.ReadSaveFile(r);
                    var memR = memory.GetReader();
                    long readStart = 0;

                    for (int metaIx = 0; metaIx < clientSaveMetasCount; metaIx++)
                    {
                        memR.BaseStream.Position = readStart;
                        CityClientSaveMeta meta = clientSaveMetas[metaIx];
                        var city = DssRef.world.cities[meta.cityIndex];

                        if (city.pfaction == p.pfaction)
                        { 
                            city.readClientState(memR, version.sub);
                        }

                        readStart += meta.memoryLength;
                    }

                    Debug.ReadCheck(r);

                    //Apply faction setup
                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref f.cities, DssRef.world.cities, out City city))
                    {
                        city.workTemplate.onFactionChange(city, f.workTemplate, true);
                    }
                }
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
