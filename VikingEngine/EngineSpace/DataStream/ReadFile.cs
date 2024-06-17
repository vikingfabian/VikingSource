using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    abstract class AbsReadFile : AbsReadWriteFile
    {
        public AbsReadFile()
        { }

        public AbsReadFile(FilePath filePath, IStreamIOCallback callbackObj)
            :base(filePath, callbackObj)
        {
            storagePriority = true;
        }
        public override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
            data = DataStreamHandler.Read(filePath);
            //return true;
        }
        //public override void Time_Update(float time)
        //{
        //    if (callbackObj != null)
        //        callbackObj.SaveComplete(false, -1, data != null, data);
        //}
        public override void onStorageComplete()
        {
            base.onStorageComplete();
            callbackObj?.SaveComplete(false, -1, data != null, data);
        }

        public override string ToString()
        {
            return "Reading:" + filePath.ToString();
        }
    }

    /// <summary>
    /// Is que and synch
    /// </summary>
    class ReadByteArrayObj : AbsReadFile
    {
        DataLib.ISaveByteArrayObj byteArrayObj;
        public ReadByteArrayObj(FilePath filePath, DataLib.ISaveByteArrayObj byteArrayObj, IStreamIOCallback callbackObj)
            :base(filePath, callbackObj)
        {
            this.byteArrayObj = byteArrayObj;
            beginStorageTask();
        }
        //protected override void SynchedEvent()//public override void Time_Update(float time)
        //{
        public override void onStorageComplete()
        {
            base.onStorageComplete();
            byteArrayObj.ByteArraySaveData = data;
            //base.Time_Update(time);
        }
    }

    /// <summary>
    /// Is que and synch
    /// </summary>
    class ReadBinaryIO : AbsReadFile
    {
        ReadBinaryStream reader;
        //IBinaryIOobj binaryIOobj;

        public ReadBinaryIO(FilePath filePath, IBinaryIOobj binaryIOobj, IStreamIOCallback callbackObj)
            : base(filePath, callbackObj)
        {
            this.reader = binaryIOobj.read;
            beginStorageTask();
        }
        public ReadBinaryIO(FilePath filePath, ReadBinaryStream reader, IStreamIOCallback callbackObj)
            : base(filePath, callbackObj)
        {
            this.reader = reader;
            beginStorageTask();
        }
        //protected override void SynchedEvent()//public override void Time_Update(float time)
        public override void onStorageComplete()
        {
            
            if (data != null)
            {
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                System.IO.BinaryReader r = new System.IO.BinaryReader(s);
                reader(r);
            }

            base.onStorageComplete();
            //base.Time_Update();
        }
    }

    class ReadToMemory : AbsReadFile
    {
        public MemoryStreamHandler memory = null;
        
        public ReadToMemory()
            :base()
        { }

        public ReadToMemory(FilePath filePath, IStreamIOCallback callbackObj)
            : base(filePath, callbackObj)
        {
            beginStorageTask();
        }        

        public override void onStorageComplete()
        {
            if (data != null)
            {
                memory = new MemoryStreamHandler(data);
            }

            base.onStorageComplete();
        }

        public System.IO.BinaryWriter writeToMemory()
        {
            memory = new MemoryStreamHandler();
            return memory.GetWriter();
        }

        public bool Ready => memory != null;
    }


}
