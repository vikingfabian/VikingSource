using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class UpdateTrigger: AbsTimer
    {
        AbsUpdateable updateObj;
        bool addToList;

        public UpdateTrigger(float timeDelay, AbsUpdateable _updateObj, bool addToUpdateList)
            : this(timeDelay, _updateObj, addToUpdateList, UpdateType.Lazy)
        {
        }
        public UpdateTrigger(float timeDelay, AbsUpdateable _updateObj, bool addToUpdateList, UpdateType lasyUpdate)
            : base(timeDelay, lasyUpdate)
        {
            updateObj = _updateObj;
            addToList = addToUpdateList;
        }
        protected override void timeTrigger()
        {
            if (addToList)
                //Ref.update.AddToUpdate(updateObj, true);
                updateObj.AddToUpdateList();
            else
                updateObj.DeleteMe();
            DeleteMe();
        }

    }
}
