using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    struct BoolStreamer
    {
        int currentBitIndex;
        EightBit bits;

        public void writeNext(bool value, System.IO.BinaryWriter w)
        {
            bits.Set(currentBitIndex, value);
            if (++currentBitIndex >= 8)
            {
                bits.write(w);
                currentBitIndex = 0;
            }
        }

        public void endWrite(System.IO.BinaryWriter w)
        {
            bits.write(w);
        }

        public bool readNext(System.IO.BinaryReader r)
        {
            if (currentBitIndex == 0 || currentBitIndex >= 8)
            {
                bits.read(r);
                currentBitIndex = 0;
            }

            return bits.Get(currentBitIndex++);
        }
    }
}
