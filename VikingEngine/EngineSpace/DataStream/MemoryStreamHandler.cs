using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    class MemoryStreamHandler
    {
        System.IO.MemoryStream s;
        System.IO.BinaryWriter w;
        
        public MemoryStreamHandler()
        { }
        public MemoryStreamHandler(System.IO.BinaryReader r)
        {
            ReadSaveFile(r);
        }

        public MemoryStreamHandler(byte[] data)
        {
            SetByteArray(data);
        }

        public void readToMemory(System.IO.BinaryReader r)
        {
            int dataLength = (int)(r.BaseStream.Length - r.BaseStream.Position);
            var data = r.ReadBytes(dataLength);
            s = new System.IO.MemoryStream(data);
        }

        public System.IO.BinaryReader CloneReader(System.IO.BinaryReader r)
        {
            readToMemory(r);

            return GetReader();
        }

        public System.IO.BinaryWriter GetWriter()
        {
            s = new System.IO.MemoryStream();
            w = new System.IO.BinaryWriter(s);
            return w;
        }

        public System.IO.BinaryReader GetReader()
        {
            if (s == null)
            {
                throw new Exception("Has no data");
            }
            System.IO.BinaryReader result = new System.IO.BinaryReader(s);
            result.BaseStream.Position = 0;
            return result;
        }

        public void WriteSaveFile(System.IO.BinaryWriter w)
        {
            w.Write((int)s.Length);
            w.Write(s.ToArray());
        }
        public void ReadSaveFile(System.IO.BinaryReader r)
        {
            int length = r.ReadInt32();
            s = new System.IO.MemoryStream(r.ReadBytes(length));
        }

        public void WriteDataArray(System.IO.BinaryWriter w)
        {
            w.Write(s.ToArray());
        }

        public byte[] ByteArray()
        {
            return s.ToArray();
        }

        public void SetByteArray(byte[] data)
        {
            s = new System.IO.MemoryStream(data);
        }


        static readonly int[] Primes = new int[]
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97
        };
        
        public void WriteChecksum()
        {
            byte[] data = s.ToArray();

            ulong sum = dataSumValue(data, data.Length);

            w.Write(sum);
        }
        public void ReadCheckSum()
        {
            const int CheckSumLength = 8;
            byte[] data = s.ToArray();
            if (data.Length > CheckSumLength)
            {
                int sumPos = data.Length - CheckSumLength;
                ulong sum = dataSumValue(data, sumPos);

                System.IO.BinaryReader r = GetReader();
                r.BaseStream.Position = sumPos;
                ulong readSum = r.ReadUInt64();

                if (sum == readSum)
                {
                    //correct!
                    return;
                }
            }

            //check sum error, clear data
            s = new System.IO.MemoryStream();
            if (PlatformSettings.ViewErrorWarnings)
            {
                throw new Exception("Save Checksum error");
            }
        }

        public void Save(FilePath path)
        {
            DataStreamHandler.Write(path, s.ToArray());
        }

        ulong dataSumValue(byte[] data, int dataLenght)
        {
            ulong sum = 0;
            CirkleCounterUp primIx = new CirkleCounterUp(0, Primes.Length - 1);
            for (int i = 0; i < dataLenght; ++i)
            {
                sum += (ulong)(data[i] * Primes[primIx.Next()]);
            }

            return sum;
        }


        public bool HasData
        {
            get
            {
                return s != null && s.Length > 0;
            }
        }
    }
}
