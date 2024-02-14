using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

namespace VikingEngine
{
    static class TaskExt
    {
        #region StorageQue
        static AbsQuedTasks ActiveStorageTask = null;
       
        static SpottedArray<AbsQuedTasks> StorageQue = new SpottedArray<AbsQuedTasks>(32);
        //
        public static void CheckStorageQue()
        {   
            if (ActiveStorageTask != null)
            {
                if (ActiveStorageTask.asynchActionComplete())
                {
                    var prevTask = ActiveStorageTask;
                    ActiveStorageTask = null;
                    prevTask?.onStorageComplete();
                }
                else if (ActiveStorageTask.storageTaskBeginTime.secPassed(30f))
                {
                    BlueScreen.ThreadException = new Exception("Storage timeout " + ActiveStorageTask.ToString());
                }
            }

            if (ActiveStorageTask == null)
            {
                if (StorageQue.Count > 0)
                {
                    bool first = true;
                    var storageTasksCounter = StorageQue.counter();
                    while (storageTasksCounter.Next())
                    {
                        if (storageTasksCounter.sel.storagePriority)
                        {
                            ActiveStorageTask = storageTasksCounter.sel;
                            break;
                        }
                        else if (first)
                        {
                            ActiveStorageTask = storageTasksCounter.sel;
                        }
                        first=false;
                    }

                    var activeStorageTask_sp = ActiveStorageTask;
                    if (activeStorageTask_sp != null)
                    {
                        StorageQue.Remove(activeStorageTask_sp);

                        activeStorageTask_sp.storageTaskBeginTime = TimeStamp.Now();

                        activeStorageTask_sp.task = Task.Factory.StartNew(() =>
                        {
                            activeStorageTask_sp.runQuedTask(MultiThreadType.Storage);
                        });
                    }
                }
            }
        }

        public static void AddStorageTask(AbsQuedTasks task)
        {
            StorageQue.Add(task);
        }

        public static void ClearStorageQue()
        {
            StorageQue.Clear();
        }
        #endregion

        public static void AddTask(IQuedObject obj, bool storageTask = false, bool storagePriority = false)
        {
            AddTask(obj.runQuedTask, storageTask, storagePriority);
        }

        public static void AddTask(Action<MultiThreadType> action, bool storageTask, bool storagePriority = false)
        {
            if (storageTask)
            {
                new StorageTask(action, storagePriority);
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        action(MultiThreadType.Asynch);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                        Debug.LogError(e.StackTrace);
                    }
                });
            }
        }

        public static void Update()
        {
            CheckStorageQue();
        }
    }
    

    
}
