using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    class ArrayEnumWithIdStorage<TEnum>
    {
        KeyValuePair<TEnum, int>[] Enum_Id;
        bool[] stored;

        public ArrayEnumWithIdStorage(KeyValuePair<TEnum, int>[] Enum_Id)
        {
            this.Enum_Id = Enum_Id;
            stored = new bool[Enum_Id.Length];
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(stored.Length);
            for (int i = 0; i < stored.Length; ++i)
            {
                w.Write(Enum_Id[i].Value);
                w.Write(stored[i]);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            int length = r.ReadInt32();
            for (int i = 0; i < length; ++i)
            {
                int id = r.ReadInt32();
                bool value = r.ReadBoolean();
                
                if (value)
                {
                    int ix = arraylib.IndexFromValue(Enum_Id, id);
                    arraylib.TrySet(stored, ix, true);
                }
            }
        }

        public void SetTrue(TEnum enumValue)
        {
            int ix = arraylib.IndexFromKey(Enum_Id, enumValue);
            stored[ix] = true;
        }

        public bool Get(TEnum enumValue)
        {
            int ix = arraylib.IndexFromKey(Enum_Id, enumValue);
            return stored[ix];
        }

        public int TrueCount()
        {
            int count = 0;
            for (int i = 0; i < stored.Length; ++i)
            {
                if (stored[i])
                {
                    count++;
                }
            }

            return count;
        }
    }
}
