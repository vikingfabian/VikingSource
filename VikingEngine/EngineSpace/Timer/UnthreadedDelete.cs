using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Timer
{
    class UnthreadedDelete : OneTimeTrigger
    {
        IDeleteable obj;
        public UnthreadedDelete(IDeleteable obj)
            :base(false)
        {
            this.obj = obj;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            if (!obj.IsDeleted)
                obj.DeleteMe();
        }
    }
}
