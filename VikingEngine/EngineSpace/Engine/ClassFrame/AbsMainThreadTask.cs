using System;

namespace VikingEngine
{
    abstract class AbsMainThreadTask : IUpdateable, IQuedObject, IDeleteable
    {
        public bool isMainThreadTaskReady = false;

        public void AddToUpdateList()
        {
            isMainThreadTaskReady = false;
            this.AddToOrRemoveFromUpdateList(true);
        }

        virtual public void Time_Update(float time_ms)
        {
            if (isMainThreadTaskReady)
            {
                runQuedTask(MultiThreadType.Main);;
                this.AddToOrRemoveFromUpdateList(false);
            }
        }

        abstract public void runQuedTask(MultiThreadType threadType);

        private bool inUpdateList = false;

        virtual public void AddToOrRemoveFromUpdateList(bool add)
        {
            Ref.update.AddToOrRemoveFromUpdate(this, add);
            inUpdateList = add;
        }

        virtual public bool IsDeleted
        {
            get { return !inUpdateList; }
        }

        virtual public UpdateType UpdateType { get { return UpdateType.Full; } }
        virtual public void DeleteMe()
        {
            AddToOrRemoveFromUpdateList(false);
        }

        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }

        virtual public bool RunDuringPause { get { return true; } }

    }
}
