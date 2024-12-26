using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine
{
    class StorageTask : AbsQuedTasks
    {
        public Action<MultiThreadType> action;
        public Action onComplete;
        protected FilePath filePath;
        public bool hasStartedTask = false;

        public StorageTask()
            : base()
        {
        }

        public StorageTask(Action<MultiThreadType> action, bool storagePriority = false, 
            Action onComplete = null, bool bBeginQue = true)
            :base()
        {
            Debug.CrashIfThreaded();

            this.action = action;
            this.onComplete = onComplete;
            this.storagePriority = storagePriority;

            if (bBeginQue)
            {
                beginStorageTask();
            }
        }

        protected void beginStorageTask()
        {
            addTaskToQue(MultiThreadType.Storage);
        }

        public override void runQuedStorageTask()
        {
            hasStartedTask = true;
            base.runQuedStorageTask();
            action?.Invoke(MultiThreadType.Storage);
        }

        public override void onStorageComplete()
        {
            base.onStorageComplete();
            onComplete?.Invoke();
        }
        
    }
}
