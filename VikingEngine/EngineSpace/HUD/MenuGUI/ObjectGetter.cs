using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    class DelayedGetCall1Arg<GetType>
    {
        public delegate GetType GetDelegate(object argument);

        GetDelegate runMethod;
        object argument;

        public DelayedGetCall1Arg(GetDelegate runMethod, object argument)
        {
            this.runMethod = runMethod;
            this.argument = argument;
        }

        public GetType InvokeGet()
        {
            return runMethod(argument);
        }
    }
}
