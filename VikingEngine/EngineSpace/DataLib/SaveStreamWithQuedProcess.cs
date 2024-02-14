using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.DataLib
{
    abstract class StorageTaskWithQuedProcess : AbsQuedTasks//: AbsSaveWithQuedProcess
    {
        //protected DataLib.ISaveTostorageCallback callBack = null;
        protected DataStream.FilePath path;
        byte[] data = null;
        bool storageStream;
        bool save;
        bool error = false;
        protected bool runSynchTrigger = false;
        bool processComplete = false;

        //public StorageTaskWithQuedProcess()
        //    :base(QuedTasksType.StorageAndProcess)
        //{ }

        public StorageTaskWithQuedProcess(bool save, DataStream.FilePath path, bool storageStream)
            : base(QuedTasksType.StorageAndProcess, false, !save)
        {
            this.storageStream = storageStream;
            this.path = path;
            this.save = save;            
        }

        //protected void queTask(MultiThreadType threadType)
        //{
        //    switch (threadType)
        //    {
        //        case MultiThreadType.Storage:
        //            new StorageTask(runQuedTask, !save, onStorageComplete);
        //            break;
        //        case MultiThreadType.Asynch:
        //            System.Threading.Tasks.Task.Factory.StartNew(() =>
        //            {
        //                runQuedTask(MultiThreadType.Asynch);
        //            }
        //            );

        //            if (runSynchTrigger)
        //            {
        //                AddToUpdateList();//Ref.update.AddToOrRemoveFromUpdate(this, true);
        //            }
        //            break;
        //    }
        //}

        public override void runQuedTask(MultiThreadType threadType)
        {
            switch (threadType)
            {
                case MultiThreadType.Storage:
                    if (save)
                    {
                        DataStream.DataStreamHandler.Write(path, data);
                    }
                    else
                    {
                        data = DataStream.DataStreamHandler.Read(path);

                        if (data == null)
                        {
                            Debug.LogError("Loading empty data:" + this.ToString());
                            error = true;
                        }
                    }
                    break;

                case MultiThreadType.Asynch:
                    if (storageStream)
                    {
                        if (save)
                        {
                            System.IO.MemoryStream s = new System.IO.MemoryStream();
                            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);
                            WriteStream(w);
                            data = s.ToArray();
                        }
                        else
                        {
                            if (data == null)
                            {
                                //System.Diagnostics.Debug.WriteLine("ERR Loading empty data:" + this.ToString());
                            }
                            else
                            {
                                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                                System.IO.BinaryReader r = new System.IO.BinaryReader(s);
                                ReadStream(r);
                            }
                        }
                    }
                    else
                    {
                        if (save)
                        {
                            data = ByteArraySaveData;
                        }
                        else
                        {
                            ByteArraySaveData = data;
                        }
                    }

                    runQuedAsynchTask();

                    processComplete = true;
                    break;

                case MultiThreadType.Main:
                    if (processComplete)
                    {
                        //if (callBack != null)
                        //{
                        //    callBack.SaveComplete(save, -1, null, false);
                        //}
                        runQuedMainTask();
                    }
                    break;

            }
        }

        //protected void beginStorageTask()
        //{
        //    new StorageTask(task, !save, onStorageComplete);
        //}

        //protected void beginProcessTask()
        //{
        //    System.Threading.Tasks.Task.Factory.StartNew(() =>
        //    {
        //        task(MultiThreadType.Asynch);
        //    }
        //    );

        //    if (runSynchTrigger)
        //    {
        //        AddToUpdateList();//Ref.update.AddToOrRemoveFromUpdate(this, true);
        //    }
        //}

        //void readWriteAction(bool non)
        //{
        //    if (save)
        //    {
        //        DataStream.DataStreamHandler.Write(path, data);
        //    }
        //    else
        //    {
        //        data = DataStream.DataStreamHandler.Read(path);
                
        //        if (data == null)
        //        {
        //            Debug.LogError("Loading empty data:" + this.ToString());
        //            error = true;
        //        }
        //    }
        //}

        

        override public void onStorageComplete()
        {
            if (data != null)
            {
                addTaskToQue(MultiThreadType.Asynch);
            }
            else if (error)
            {
                IOFailedEvent();
            }
        }

        virtual protected void IOFailedEvent()
        { }

        //void processAction()
        //{

        //}

        virtual public void WriteStream(System.IO.BinaryWriter w)
        { throw new NotImplementedException("AbsByteArrayQueObj WriteStream"); }
        virtual public void ReadStream(System.IO.BinaryReader r)
        { throw new NotImplementedException("AbsByteArrayQueObj ReadStream"); }

        virtual public byte[] ByteArraySaveData
        {
            get { throw new NotImplementedException("AbsByteArrayQueObj get array"); }
            set { throw new NotImplementedException("AbsByteArrayQueObj set array"); }
        }

        //public void Time_Update(float time)
        //{
        //    if (processComplete)
        //    {
        //        if (callBack != null)
        //        {
        //            callBack.SaveComplete(save, -1, null, false);
        //        }
        //        MainThreadTrigger();

        //        Ref.update.AddToOrRemoveFromUpdate(this, false);
        //    }
        //}

        //protected override void runQuedMainTask()
        //{
        //    if (processComplete)
        //    {
        //        if (callBack != null)
        //        {
        //            callBack.SaveComplete(save, -1, null, false);
        //        }
        //        MainThreadTrigger();
        //    }
        //}

        //virtual protected void MainThreadTrigger() { }

        //public UpdateType UpdateType { get { return UpdateType.Full; } }
        //public bool RunDuringPause { get { return true; }  }

        //public int SpottedArrayMemberIndex { get { throw new NotImplementedException(); } set { } }
        //public bool SpottedArrayUseIndex { get { return false; } }

    }
}
