using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    abstract class AbsReadWriteFile : StorageTask
    {
        protected byte[] data;
        protected IStreamIOCallback callbackObj;

        public AbsReadWriteFile()
        { }

        public AbsReadWriteFile(FilePath filePath, IStreamIOCallback callbackObj)
            :base()
        {
            this.filePath = filePath;
            this.callbackObj = callbackObj;
        }
    }

    abstract class AbsWriteFile : AbsReadWriteFile
    {
       public AbsWriteFile(FilePath filePath, IStreamIOCallback callbackObj)
            : base(filePath, callbackObj)
        {

        }
        public override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
           DataStreamHandler.Write(this.filePath, data);
          // return base.quedEvent();
       }    
        public override void onStorageComplete()
        {
            base.onStorageComplete();
            callbackObj?.SaveComplete(true, -1, true, data);
       }
       public override string ToString()
       {
           return "Writing:" + filePath.ToString();
       }
    }

    class WriteByteArray : AbsWriteFile
    {
        public WriteByteArray(FilePath filePath, DataLib.ISaveByteArrayObj writerObj, IStreamIOCallback callbackObj)
            : this(filePath, writerObj.ByteArraySaveData, callbackObj)
        { }
        public WriteByteArray(FilePath filePath, byte[] data, IStreamIOCallback callbackObj)
            :base(filePath, callbackObj)
        {
            this.data = data;
            beginStorageTask();//start();
        }

        public override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
        }
    }

    class WriteBinaryIO : AbsWriteFile
    {
        public WriteBinaryIO(FilePath filePath, IBinaryIOobj writer, IStreamIOCallback callbackObj)
            :this(filePath, writer.write , callbackObj)
        {  }

        public WriteBinaryIO(FilePath filePath,WriteBinaryStream writer , IStreamIOCallback callbackObj)
            : base(filePath, callbackObj)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);
            writer(w);
            data = s.ToArray();
            beginStorageTask();//start();
        }
    }

    class CreateFolderToQue : StorageTask
    {
        string path;
        public CreateFolderToQue(string path)
            :base(null, true, null, false)
        {
            this.path = path;

            beginStorageTask();

            //Engine.Storage.AddToSaveQue(StartQuedProcess, false); //as load be course its high prio to create
        }
        public override void runQuedStorageTask()
        {
            //base.runQuedStorageTask();
            DataStream.FilePath.CreateStorageFolder(path);
        }
    }
}
