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

        //static SpottedArray<AbsQuedTasks> StorageQue = new SpottedArray<AbsQuedTasks>(32);

        static List<AbsQuedTasks> StorageTaskQue = new List<AbsQuedTasks>();
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
                if (StorageTaskQue.Count > 0)
                {
                    //bool first = true;
                    //var storageTasksCounter = StorageQue.counter();

                    lock (StorageTaskQue)
                    {
                        //while (storageTasksCounter.Next())
                        for (int i = 0; i < StorageTaskQue.Count; ++i)                        
                        {
                            if (StorageTaskQue[i].storagePriority)
                            {
                                ActiveStorageTask = StorageTaskQue[i];
                                StorageTaskQue.RemoveAt(i);
                                break;
                            }                            
                        }

                        if (ActiveStorageTask == null)
                        {
                            ActiveStorageTask = StorageTaskQue[0];
                            StorageTaskQue.RemoveAt(0);
                        }
                    }

                    var activeStorageTask_sp = ActiveStorageTask;
                    if (activeStorageTask_sp != null)
                    {
                        activeStorageTask_sp.storageTaskBeginTime = TimeStamp.Now();

                        activeStorageTask_sp.task = Task.Factory.StartNew(() =>
                        {
                            activeStorageTask_sp.runQuedStorageTask();
                        });
                    }
                }
            }
        }

        public static void AddStorageTask(AbsQuedTasks task)
        {
            lock (StorageTaskQue)
            {
                StorageTaskQue.Add(task);
            }
        }

        public static void ClearStorageQue()
        {
            lock (StorageTaskQue)
            {
                StorageTaskQue.Clear();
            }
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
