using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    class WorldMetaData
    {
        const int Version = 1;

        public ushort seed;
        public int objSeed;
        public MapSize mapSize;
        public int saveIndex = -1;
        public bool IsGenerated => saveIndex < 0;
        public PcgRandom objRnd;

        public WorldMetaData(ushort seed, MapSize mapSize, int saveIndex)
        {
            this.seed = seed;
            objSeed = Ref.rnd.Int(100000);
            objRnd = new PcgRandom(objSeed);
            this.mapSize = mapSize;
            this.saveIndex = saveIndex;
        }

        public WorldMetaData(System.IO.BinaryReader r)
        { 
            read(r);
        }

        public WorldMetaData()
        {
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(seed);
            w.Write((ushort)objSeed);
            w.Write((byte)mapSize);
            w.Write((short)saveIndex);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
             
            seed = r.ReadUInt16();
            objSeed = r.ReadUInt16();
            objRnd = new PcgRandom(objSeed);
            mapSize = (MapSize)r.ReadByte();
            saveIndex = r.ReadInt16();
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            w.Write((byte)mapSize);
        }
        public void readNet(System.IO.BinaryReader r)
        {
            mapSize = (MapSize)r.ReadByte();
        }

        public void setObjSeed(int id)
        {
            objRnd.SetSeed(id + objSeed);
        }
    }
}
