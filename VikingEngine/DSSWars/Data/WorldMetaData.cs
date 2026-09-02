using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct WorldMetaId
    {
        public ushort seed;
        public ushort objSeed;

        public int MapId()
        {
            //adding two max 16bit values to one 
            return (seed << 16) | Bound.UShort(objSeed);
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(seed);
            w.Write(objSeed);
        }

        public void read(System.IO.BinaryReader r)
        {
            seed = r.ReadUInt16();
            objSeed = r.ReadUInt16();
        }
    }

    class WorldMetaData
    {
        const int Version = 1;
        public WorldMetaId worldId;

        public MapSize mapSize;
        public int saveIndex = -1;
        public bool IsGenerated => saveIndex < 0;
        public PcgRandom objRnd;
        public bool customEditorMap = false;

        public WorldMetaData(ushort seed, MapSize mapSize, int saveIndex)
        {
            //this.seed = seed;
            //objSeed = Ref.rnd.Ushort();

            worldId = new WorldMetaId() { seed = seed, objSeed = Ref.rnd.Ushort() };
            objRnd = new PcgRandom(worldId.objSeed);
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

            //w.Write(seed);
            //w.Write(objSeed);
            worldId.write(w);
            w.Write((byte)mapSize);
            w.Write((short)saveIndex);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
             
            //seed = r.ReadUInt16();
            //objSeed = r.ReadUInt16();
            worldId.read(r);
            objRnd = new PcgRandom(worldId.objSeed);
            mapSize = (MapSize)r.ReadByte();
            saveIndex = r.ReadInt16();
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            write(w);
        }
        public void readNet(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void setObjSeed(int id)
        {
            objRnd.SetSeed(id + worldId.objSeed);
        }
    }
}
