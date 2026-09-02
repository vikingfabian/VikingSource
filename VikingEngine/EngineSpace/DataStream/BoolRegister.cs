using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine.EngineSpace.DataStream
{
    /// <summary>
    /// Uses bits to compress repeating data
    /// </summary>
    class BoolRegister
    {
        public MemoryStreamHandler memory;
        int currentBool = 0;
        List<bool> bools;
        public System.IO.BinaryWriter writer;

        public BoolRegister(int capacity)
        { 
            bools = new List<bool>(capacity);
            memory = new MemoryStreamHandler();
            writer = memory.GetWriter();
        }
        public void finalizeWrite(System.IO.BinaryWriter w)
        {
            int bytes = MathExt.Div_Ceiling(bools.Count, 8);
            w.Write((byte)bytes);

            int listIndex = 0;
            while (listIndex < bools.Count)
            {
                EightBit eightBit = new EightBit();
                int bitIx = 0;
                while (bitIx < 8 && listIndex < bools.Count)
                {
                    eightBit.Set(bitIx, bools[listIndex]);
                    bitIx++;
                    listIndex++;
                }

                eightBit.write(w);
            }

            memory.WriteDataArray(w);
        }
        public BoolRegister(System.IO.BinaryReader r)
        { 
            int bytes = r.ReadByte();

            bools = new List<bool>(bytes * 8);

            for (int i = 0; i < bytes; i++)
            {
                EightBit eightBit = new EightBit(r);
                for (int bitIx = 0; bitIx < 8; bitIx++)
                {
                    bools.Add(eightBit.Get(bitIx));
                }       
            }
        }
        
        public bool SetNext(bool value)
        {
            bools.Add(value);
            return value;
        }
        public bool GetNext()
        {
            return bools[currentBool++];
        }
    }
}
