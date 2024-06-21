using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DataStream
{
    delegate void FileIoCallback(bool save, bool completed, object args);

    /// <summary>
    /// Designed to replace all save/load
    /// </summary>
    class FileIO_2 : AbsQuedTasks
    {
        static readonly AbsPlatformStorage Storage =
#if XBOX
        new XboxWrapping.XboxStorage();
#else
        new PCStorage();
#endif

        public FilePath filePath;
        public FileIoCallback callback;
        public object callbackArgs = null;

        WriteBinaryStream writeAction;
        ReadBinaryStream readAction;

        bool save;
        bool success = false;
        bool synchedRead = false;
        System.IO.MemoryStream data;
        bool storageTaskComplete = false;

        public FileIO_2(FilePath filePath, WriteBinaryStream writeAction,
             bool startThread = true,
            FileIoCallback callback = null, object callbackArgs = null)

            : this(true, filePath, writeAction, null,
                 startThread, false, callback, callbackArgs)
        {
        }

        public FileIO_2(FilePath filePath, ReadBinaryStream readAction,
            bool startThread = true, bool synchedRead = false,
            FileIoCallback callback = null, object callbackArgs = null)
            
            :this(false, filePath, null, readAction, 
                 startThread, synchedRead, callback, callbackArgs)
        {
        }

        public FileIO_2(bool save, FilePath filePath,
            WriteBinaryStream writeAction,
            ReadBinaryStream readAction,
            bool startThread = true, bool synchedRead = false,
            FileIoCallback callback = null, object callbackArgs = null)
        {
            this.writeAction = writeAction;
            this.readAction = readAction;
            this.synchedRead = synchedRead;

            this.filePath = filePath;
            this.callback = callback;
            this.callbackArgs = callbackArgs;

            beginStorageTask(save, startThread);
        }

        protected void beginStorageTask(bool save, bool startThread)
        {
            this.save = save;
            filePath.CheckFileLength();

            if (startThread)
            {
                storagePriority = !save;
                addTaskToQue(MultiThreadType.Storage);
            }
            else
            {
                runQuedStorageTask();
            }
        }

        public override void runQuedStorageTask()
        {
            base.runQuedStorageTask();

            if (save)
            {
                data = new System.IO.MemoryStream();
                System.IO.BinaryWriter w = new System.IO.BinaryWriter(data);
                writeAction(w);

                data.Position = 0;
                Storage.write(filePath, data, onStorageWriteComplete);
            }
            else
            {
                data = new System.IO.MemoryStream();
                Storage.read(filePath, data, onStorageReadComplete);
                
            }
        }

        void onStorageWriteComplete()
        {
            success = data != null;
            storageTaskComplete = true;
        }

        void onStorageReadComplete()
        {
            if (data == null || data.Length == 0)
            {
                success = false;
            }
            else
            {
                if (!synchedRead)
                {
                    InvokeReadAction();
                }
                success = true;
            }

            storageTaskComplete = true;
        }

        public override void onStorageComplete()
        {
            base.onStorageComplete();
            if (!save && synchedRead)
            {
                InvokeReadAction();
            }
            InvokeCallback();
        }

        void InvokeReadAction()
        {
            if (data != null)
            {
                System.IO.BinaryReader r = new System.IO.BinaryReader(data);
                r.BaseStream.Position = 0;
                readAction(r);
            }
            else
            {
                Debug.LogError("Can't read Empty file: " + filePath.ToString());
            }
        }

        void InvokeCallback()
        {
            callback?.Invoke(save, success, callbackArgs); 
        }

        public override bool asynchActionComplete()
        {
            return storageTaskComplete;
        }
    }
}
