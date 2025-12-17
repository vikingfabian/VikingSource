using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
{
    struct StorageSize
    {
        public static readonly int COUNT = (int)StorageType.NUM_NONE;

        const int StartSize = 500;
        const int BuildingSizeAdd = 500;

        public int size;

        public StorageSize()
        {
            size = StartSize;
        }

        const int SaveDiv = 100;

        public void write(System.IO.BinaryWriter w)
        { 
            w.Write((ushort)(size / SaveDiv));
        }
        public void read(System.IO.BinaryReader r, int subVersion)
        {
            size = r.ReadUInt16() * SaveDiv;
        }
    }

    enum StorageType
    {
        MaterialStorage, FoodStorage, WeaponStorage, ArmorStorage, AnimalStorage, NUM_NONE
    }
}
