using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DataStream
{
    class DebugWriteSize
    {
        long startSz;
        long size;
        public void begin(System.IO.BinaryWriter w)
        { 
            startSz=w.BaseStream.Length;
        }

        public void end(System.IO.BinaryWriter w)
        {
            size = w.BaseStream.Length - startSz;
        }

        public override string ToString()
        {
            if (size > 1000000)
            {
                return size / 1000000 + "MB";
            }
            else if (size > 1000)
            {
                return size / 1000 + "kB";
            }
            return size.ToString() + "byte";
        }

    }
}
