using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    class BaseClass : IUpdateable, IDeleteable
    {
        bool isDeleted = false;
        bool inUpdate = false;
        bool runDuringPause = false;
        protected UpdateType updateType = UpdateType.Full;

        protected Graphics.ImageGroup images = null;

        public BaseClass()
        {

        }

        virtual public bool IsDeleted
        {
            get { return isDeleted; }
        }
        virtual public void Time_Update(float time_ms) { }

        virtual public void AddToUpdateList()
        { 
            this.AddToOrRemoveFromUpdateList(true); 
        }

        virtual public void AddToOrRemoveFromUpdateList(bool add)
        {
            Ref.update.AddToOrRemoveFromUpdate(this, add);
            inUpdate = add;
        }
        virtual public UpdateType UpdateType { get { return updateType; } }
        virtual public void DeleteMe()
        {
            if (inUpdate)
            {
                AddToOrRemoveFromUpdateList(false);
            }

            if (images != null)
            {
                images.DeleteAll();
                images = null;
            }
        }
        //virtual public bool SavingThread { get { return false; } }
        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }
        public bool RunDuringPause { get { return runDuringPause; } }
    }
}
