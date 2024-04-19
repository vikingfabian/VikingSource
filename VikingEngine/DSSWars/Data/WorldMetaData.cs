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
        public MapSize mapSize;
        public int saveIndex = -1;
        public bool IsGenerated => saveIndex < 0;

        public WorldMetaData(ushort seed, MapSize mapSize, int saveIndex)
        {
            this.seed = seed;
            this.mapSize = mapSize;
            this.saveIndex = saveIndex;
        }

        public WorldMetaData(System.IO.BinaryReader r)
        { 
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(seed);
            w.Write((byte)mapSize);
            w.Write((short)saveIndex);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
             
            seed = r.ReadUInt16();
            mapSize = (MapSize)r.ReadByte();
            saveIndex = r.ReadInt16();
        }
    }
}
