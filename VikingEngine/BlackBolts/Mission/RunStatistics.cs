using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.Mission
{
    class RunStatistics
    {
        public int[] itemDelivered = new int[(int)ResourceType.NUM_NONE];
        public int[] killsBy = new int[(int)FactoryObjectType.NUM_NONE];
        public int[] destroyedCount = new int[(int)FactoryObjectType.NUM_NONE];

        public HashSet<int> workersUsed = new HashSet<int>();

        public int poopStomps = 0;

    }
}
