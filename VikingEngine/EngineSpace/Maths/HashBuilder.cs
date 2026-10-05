using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    struct HashBuilder
    {
        int count;
        uint hash;

        public void Add(int value)
        {
            if (count == 0)
            {
                hash = (uint)value;
            }
            else
            {
                //0x9e3779b9 is the golden ratio
                hash ^= (uint)value + 0x9e3779b9 + (hash << 6) + (hash >> 2);
            }
            count++;
        }

        public int GetHash()
        {
            return (int)hash;
        }
    }
}
