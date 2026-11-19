using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.GO
{
    struct Fluid
    {
        public static readonly Fluid Empty = new Fluid() { amount = 0 };
        public int amount;
        public FluidType fluidType;

        public Fluid()
        { }

        public Fluid(FluidType resourceType)
        {
            this.fluidType = resourceType;
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

    enum FluidType
    { 
        None,
        Blood,
    }
}
