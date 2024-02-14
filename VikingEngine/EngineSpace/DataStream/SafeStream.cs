using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    class SafeStream
    {
        const long SafeCheckValue = long.MaxValue - 1103;

        System.IO.MemoryStream ms;
        System.IO.BinaryWriter w;
        System.IO.BinaryWriter mainWriter;
        System.IO.BinaryReader mainReader;

        long endPosition;
        long storeMainWriterPosition;

        public SafeStream(System.IO.BinaryWriter mainWriter)
        {
            this.mainWriter = mainWriter;

            ms = new System.IO.MemoryStream();
            w = new System.IO.BinaryWriter(ms);
        }

        public SafeStream(System.IO.BinaryReader mainReader)
        {
            this.mainReader = mainReader;
        }

        public System.IO.BinaryWriter beginWriteChunk()
        {
            storeMainWriterPosition = mainWriter.BaseStream.Position;

            ms.SetLength(0);
            return w;
        }

        public void endWriteChunk()
        {
            if (storeMainWriterPosition != mainWriter.BaseStream.Position)
            {
                throw new Exception("Main writer was used during a safe chunk, use writer returned by beginWriteChunk()");
            }

            mainWriter.Write(SafeCheckValue);
            mainWriter.Write((int)w.BaseStream.Length);

            mainWriter.Write(ms.ToArray());
        }

        public void beginReadChunk()
        {
            long check = mainReader.ReadInt64();
            if (check != SafeCheckValue)
            {
                throw new Exception("Read stream drift error, at position " + mainReader.BaseStream.Position.ToString());
            }

            int length = mainReader.ReadInt32();
            endPosition = mainReader.BaseStream.Position + length;
        }

        public void endReadChunk(bool crashIfIncorrect)
        {
            if (mainReader.BaseStream.Position != endPosition)
            {
                if (crashIfIncorrect)
                {
                    throw new Exception("Safe stream end chunk error, at position " + mainReader.BaseStream.Position.ToString());
                }
                else
                {
                    mainReader.BaseStream.Position = endPosition;
                }
            }
        }

        public void jumpPastNextChunk()
        {
            beginReadChunk();
            mainReader.BaseStream.Position = endPosition;
        }


        //TEST
        public static void testWrite(System.IO.BinaryWriter w)
        {
            int[] testArray1 = new int[] { 23, 345, 4422, 42, 2134, 1 };
            int[] testArray2 = new int[] { 23, 45, 33, 42, 2134, 10, 23, 23, 4555, 553, 2 };
            int[] testArray3 = new int[] { 23, 34, 77, 42, 3 };

            SafeStream safeStream = new SafeStream(w);

            var sw = safeStream.beginWriteChunk();
            sw.Write(testArray1.Length);
            foreach (var m in testArray1)
            {
                sw.Write(m);
            }
            safeStream.endWriteChunk();


            sw = safeStream.beginWriteChunk();
            sw.Write(testArray2.Length);
            foreach (var m in testArray2)
            {
                sw.Write(m);
            }
            safeStream.endWriteChunk();


            sw = safeStream.beginWriteChunk();
            sw.Write(testArray3.Length);
            foreach (var m in testArray3)
            {
                sw.Write(m);
            }
            safeStream.endWriteChunk();
        }

        public static void testRead(System.IO.BinaryReader r)
        {
            SafeStream safeStream = new SafeStream(r);

            int[] testArray1, testArray3;

            safeStream.beginReadChunk();
            {
                testArray1 = new int[r.ReadInt32()];
                for (int i = 0; i < testArray1.Length; ++i)
                {
                    testArray1[i] = r.ReadInt32();
                }
            } safeStream.endReadChunk(true);


            //Example of ignoring older version of stored data
            safeStream.jumpPastNextChunk();


            safeStream.beginReadChunk();
            {
                testArray3 = new int[r.ReadInt32()];
                for (int i = 0; i < testArray3.Length; ++i)
                {
                    testArray3[i] = r.ReadInt32();
                }
            } safeStream.endReadChunk(true);


            if (testArray1[testArray1.Length - 1] != 1 ||
                testArray3[testArray3.Length - 1] != 3)
            {
                throw new Exception("Test error");
            }

        }
    }
}
