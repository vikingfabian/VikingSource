using System;

namespace VikingEngine
{
    interface IUpdateable : ISpottedArrayMember
    {
        UpdateType UpdateType { get; }
        bool RunDuringPause { get; }
        void Time_Update(float time_ms);
        //bool SavingThread { get; }
    }
    abstract class AbsUpdateable : IUpdateable, IDeleteable
    {
        public AbsUpdateable(bool addToUpdate)
        {
            if (addToUpdate)
            {
                AddToOrRemoveFromUpdateList(true);
            }
        }
        virtual public bool IsDeleted
        {
            get { return !inUpdateList; }
        }
        abstract public void Time_Update(float time_ms);

        private bool inUpdateList = false;

        virtual public void AddToUpdateList()
        { this.AddToOrRemoveFromUpdateList(true); }
        virtual public void AddToOrRemoveFromUpdateList(bool add)
        {
            Ref.update.AddToOrRemoveFromUpdate(this, add);
            inUpdateList = add;
        }
        virtual public UpdateType UpdateType { get { return UpdateType.Full; } }
        virtual public void DeleteMe()
        {
            AddToOrRemoveFromUpdateList(false);
        }
        //irtual public bool SavingThread { get { return false; } }
        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }

        virtual public bool RunDuringPause { get { return true; } }

        virtual public void AbortThreads() { }

    }

    abstract class AbsInGameUpdateable : AbsUpdateable
    {
        public AbsInGameUpdateable(bool addToUpdate)
            : base(addToUpdate)
        { }

        public override bool RunDuringPause { get { return false; } }
    }


    abstract class OneTimeTrigger : AbsUpdateable
    {
        public OneTimeTrigger(bool addToUpdate)
            : base(addToUpdate)
        {
        }
        override public UpdateType UpdateType { get { return UpdateType.OneTimeTrigger; } }
    }
    abstract class LazyUpdate : AbsUpdateable
    {
        public LazyUpdate(bool addToUpdate)
            : base(addToUpdate)
        {
        }
        override public UpdateType UpdateType { get { return UpdateType.Lazy; } }
    }

    

    

    //abstract class OneTimeQueTrigger2 : IQuedObject
    //{
    //    public OneTimeQueTrigger2(bool addToUpdate)
    //    {
    //        if (addToUpdate)
    //            AddToUpdate();
    //    }

    //    protected void AddToUpdate()
    //    { Ref.asynchUpdate.AddThreadQueObj(this); }

    //    abstract public void runQuedTask(MultiThreadType threadType);
    //}

    /// <summary>
    /// Will first run a threaded process, followed by a unthreaded event
    /// </summary>
    //abstract class QueAndSynch : AbsMainThreadTask, IQuedObject
    //{
    //    bool storageAccess;
    //    bool save;
    //    Task task;
    //    public bool continueToSynchedUpdate;

    //    public QueAndSynch(bool storageAccess)
    //        : this(storageAccess, true)
    //    { }
    //    public QueAndSynch(bool storageAccess, bool save)
    //        //: base(false)
    //    {
    //        this.storageAccess = storageAccess;
    //        this.save = save;
    //    }

    //    protected void start()
    //    {
    //        if (storageAccess)
    //        {
    //            new StorageTask(runQuedTask, !save);
    //            //Engine.Storage.AddToSaveQue(StartQuedProcess, save);
    //        }
    //        else
    //        {
    //            Task.Factory.StartNew(() =>
    //            {
    //                runQuedTask(MultiThreadType.Asynch);
    //            });
    //            //Ref.asynchUpdate.AddThreadQueObj(this);
    //        }

    //        this.AddToUpdateList();
    //    }

    //    public override void Time_Update(float time_ms)
    //    {
    //        if (isComplete)
    //        {
    //            if (continueToSynchedUpdate)
    //            {
    //                base.Time_Update(time_ms);
    //            }
    //            else
    //            {
    //                AddToOrRemoveFromUpdateList(false);
    //            }
    //        }
    //    }
        
    //    //override public void runQuedTask(MultiThreadType threadType)
    //    //{

    //    //    //continueToSynchedUpdate = quedEvent();
    //    //    isComplete = true;
    //    //}

    //    /// <returns>If the progress should continue (false will abort)</returns>
    //    //abstract protected bool quedEvent();
    //}



    enum UpdateType
    {
        Full,
        Lazy,
        NUM,
        OneTimeTrigger,
    }
}
