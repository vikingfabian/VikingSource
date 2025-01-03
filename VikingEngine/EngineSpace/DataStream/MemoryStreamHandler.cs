using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    class MemoryStreamHandler: DataLib.ISaveByteArrayObj
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

        public byte[] ByteArraySaveData { get { return ByteArray(); } set { SetByteArray(value); } }

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

        public bool WritePartialDataToWriter(int startIndex, int length, System.IO.BinaryWriter writer)
        {
            if (s == null)
            {
                throw new InvalidOperationException("MemoryStream is null. Ensure it contains data before attempting to write.");
            }

            if (startIndex < 0 || length < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex or length cannot be negative.");
            }

            // Adjust the length if it exceeds the available data
            int availableLength = (int)(s.Length - startIndex);
            if (availableLength <= 0)
            {
                throw new InvalidOperationException("Can't write zero length");
            }

            length = Math.Min(length, availableLength);

            writer.Write((int)s.Length);
            writer.Write(startIndex);
            writer.Write(length);


            // Save the current position of the MemoryStream
            long originalPosition = s.Position;

            try
            {
                // Set the position of the MemoryStream to the start index
                s.Position = startIndex;

                // Create a buffer to hold the data
                byte[] buffer = new byte[length];

                // Read the data into the buffer
                int bytesRead = s.Read(buffer, 0, length);

                // Write the data to the BinaryWriter
                writer.Write(buffer, 0, bytesRead);

                // Check if we have reached the end of the stream
                return s.Position >= s.Length;
            }
            finally
            {
                // Restore the original position of the MemoryStream
                s.Position = originalPosition;
            }
        }


        public bool ReadPartialDataToMemory(System.IO.BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader), "BinaryReader cannot be null.");
            }

            // Read the full length of the data (used for validation or reference)
            int fullLength = reader.ReadInt32();

            // Read the start index and length of the data to be read
            int startIndex = reader.ReadInt32();
            int length = reader.ReadInt32();

            if (startIndex < 0 || length < 0 || startIndex + length > fullLength)
            {
                throw new ArgumentOutOfRangeException("Invalid startIndex or length specified in the BinaryReader.");
            }

            // Create a buffer for the partial data
            byte[] buffer = reader.ReadBytes(length);

            // Initialize or update the MemoryStream with the read data
            if (s == null)
            {
                s = new MemoryStream(fullLength);
            }

            // Set the position to startIndex and write the partial data
            long originalPosition = s.Position;
            try
            {
                s.Position = startIndex;
                s.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                // Restore the original position of the MemoryStream
                s.Position = originalPosition;
            }

            return startIndex + length >= fullLength;
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

        public long memoryLength => s !=null? s.Length : 0;
    }
}
