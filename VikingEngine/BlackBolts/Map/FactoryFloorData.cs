using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.Map
{
    class FactoryFloorData
    {
        public IntVector2 Size;

        public FactoryFloorData(IntVector2 size)
        {
            BlackRef.floorData = this;
            Size = size;
        }
    }
}
