using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Data
{
    class WorldData
    {
        public bool hostingWorld;
        public int seed;

        public WorldData(bool hostingWorld)
        {
            this.hostingWorld = hostingWorld;
        }

        public bool isClient { get { return !hostingWorld; } }
    }
}
