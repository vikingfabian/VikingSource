using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine
{
    class UpdateableFunction :AbsUpdateable
    {
        Func<bool> function;

        public UpdateableFunction(Func<bool> function)
            :base(true)
        {
            this.function = function;
        }

        public override void Time_Update(float time_ms)
        {
            if (function())
            {
                DeleteMe();
            }
        }
    }
}
