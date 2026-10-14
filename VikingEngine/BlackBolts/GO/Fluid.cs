using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.GO
{
    struct Fluid
    {
        public static readonly Fluid Empty = new Fluid() { amount = 0 };
        public int amount;
        public ResourceType resourceType;

        public Fluid()
        { }

        public Fluid(ResourceType resourceType)
        {
            this.resourceType = resourceType;
            amount = 1;
        }

        public bool HasValue => amount > 0;

        public void clear()
        {
            amount = 0;
        }

        public Fluid GetOne()
        {
            Fluid result = this;
            result.amount = 1;
            return result;
        }
    }
}
