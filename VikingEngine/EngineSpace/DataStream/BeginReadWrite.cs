
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    static class BeginReadWrite
    {
        
        public static void ByteArray(bool save, FilePath filePath, DataLib.ISaveByteArrayObj obj)
        { ByteArray(save, filePath, obj, null);}

        public static void ByteArray(bool save, FilePath filePath, DataLib.ISaveByteArrayObj obj, IStreamIOCallback callbackObj)
        {
            if (save)
                new WriteByteArray(filePath, obj, callbackObj);
            else
                new ReadByteArrayObj(filePath, obj, callbackObj);
        }

        public static void BinaryIO(bool save, FilePath filePath, IBinaryIOobj obj)
        { BinaryIO(save, filePath, obj, null); }


        public static void BinaryIO(bool save, FilePath filePath, WriteBinaryStream w, ReadBinaryStream r, IStreamIOCallback callbackObj)
        {
            if (save)
                new WriteBinaryIO(filePath, w, callbackObj);
            else
                new ReadBinaryIO(filePath, r, callbackObj);
        }

        public static void BinaryIO(bool save, FilePath filePath, IBinaryIOobj obj, IStreamIOCallback callbackObj)
        {
            BinaryIO(save, filePath, obj.write, obj.read, callbackObj);
        }

        public static void BinaryIO(bool save, FilePath filePath, IBinaryIOobj obj, IStreamIOCallback callbackObj, bool startThread)
        {
            BinaryIO(save, filePath, obj.write, obj.read, callbackObj, startThread);
        }

        public static void BinaryIO(bool save, FilePath filePath, WriteBinaryStream w, ReadBinaryStream r, IStreamIOCallback callbackObj, bool startThread)
        {
            if (startThread)
                BinaryIO(save, filePath, w, r, callbackObj);
            else
            {
                if (save)
                    DataStreamHandler.Write(filePath, w);
                else
                {
                    System.IO.BinaryReader br = DataStreamHandler.ReadBinaryIO(filePath);
                    if (br != null)
                    {
                        r(br);
                    }
                }
            }
        }
    }
}
