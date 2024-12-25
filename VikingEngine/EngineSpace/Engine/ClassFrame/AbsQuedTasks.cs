using System;

namespace VikingEngine
{
    abstract class AbsQuedTasks : ISyncAction
    {
        public bool bStorageTask, bAsynchTask, bMainThreadTask;
        public bool storagePriority;
        protected bool autoRun;

        public bool bStorageTaskComplete = false, bAsynchTaskComplete = false, bMainThreadTaskComplete = false;
        public System.Threading.Tasks.Task task;
        public float taskTime;
        //public TimeStamp storageTaskBeginTime;

        public AbsQuedTasks()
        { }

        public AbsQuedTasks(bool bStorageTask, bool bAsynchTask, bool bMainThreadTask, bool autoRun = false, bool storagePriority = false)
        {
            this.bStorageTask = bStorageTask;
            this.bAsynchTask = bAsynchTask;
            this.bMainThreadTask = bMainThreadTask;
            this.storagePriority = storagePriority;

            this.autoRun = autoRun;

            if (autoRun)
            {
                nextTask();
            }
        }

        public void beginAutoTasksRun()
        {
            autoRun = true;
            nextTask();
        }

        public AbsQuedTasks(QuedTasksType type, bool autoRun = false, bool storagePriority = false)
        {
            switch (type)
            {
                case QuedTasksType.QueAndSynch:
                    bStorageTask = false;
                    bAsynchTask = true;
                    bMainThreadTask = true;
                    break;
                case QuedTasksType.StorageAndSynch:
                    bStorageTask = true;
                    bAsynchTask = false;
                    bMainThreadTask = true;
                    break;
                case QuedTasksType.StorageAndProcess:
                    bStorageTask = true;
                    bAsynchTask = true;
                    bMainThreadTask = true;
                    break;
            }
            this.storagePriority = storagePriority;

            this.autoRun = autoRun;

            if (autoRun)
            {
                nextTask();
            }
        }

        void nextTask()
        {
            if (bStorageTask && !bStorageTaskComplete)
            {
                addTaskToQue(MultiThreadType.Storage);
            }
            else if (bAsynchTask && !bAsynchTaskComplete)
            {
                addTaskToQue(MultiThreadType.Asynch);
            }
            else if (bMainThreadTask && !bMainThreadTaskComplete)
            {
                addTaskToQue(MultiThreadType.Main);
            }
        }

        protected void addTaskToQue(MultiThreadType threadType)
        {
            switch (threadType)
            {
                case MultiThreadType.Storage:
                    TaskExt.AddStorageTask(this);
                    TaskExt.CheckStorageQue();
                    break;

                case MultiThreadType.Asynch:
                    task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            runQuedAsynchTask();//runQuedTask(MultiThreadType.Asynch);
                            if (bMainThreadTask)
                            {
                                addToSyncedUpdate();//isMainThreadTaskReady = true;
                            }
                        }
                    );

                    //if (bMainThreadTask)
                    //{
                    //    AddToUpdateList();
                    //}
                    break;

                case MultiThreadType.Main:
                    //AddToUpdateList();
                    break;
            }
        }

        //public override void runQuedTask(MultiThreadType threadType)
        //{
        //    switch (threadType)
        //    {
        //        case MultiThreadType.Storage: runQuedStorageTask(); break;
        //        case MultiThreadType.Asynch: runQuedAsynchTask(); break;
        //        case MultiThreadType.Main: runSyncAction(); break;
        //    }
        //}

        virtual public void runQuedStorageTask()
        { }

        virtual protected void runQuedAsynchTask()
        { }

        virtual public void runSyncAction()
        { }



        virtual public void onStorageComplete()
        {
            bStorageTaskComplete = true;
            //isMainThreadTaskReady = !bAsynchTask;

            if (autoRun)
            {
                nextTask();
            }
        }

        virtual public bool asynchActionComplete()
        {
            return task == null || task.IsCompleted;
        }

        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }


        public void addToSyncedUpdate()
        { 
            Ref.update.AddSyncAction(this);
        }
    }

    class QueAndSynchTask : AbsQuedTasks
    {
        Action asynchAction, synchedAction;

        public QueAndSynchTask(Action asynchAction, Action synchedAction)
            :base(false, true, true, false)
        {
            this.asynchAction = asynchAction;
            this.synchedAction = synchedAction;

            beginAutoTasksRun();
        }

        protected override void runQuedAsynchTask()
        {
            base.runQuedAsynchTask();
            asynchAction();
        }

        public override void runSyncAction()
        {
            base.runSyncAction();
            synchedAction();
        }
    }

    enum QuedTasksType
    {
        QueAndSynch,
        StorageAndSynch,
        StorageAndProcess
    }
}
