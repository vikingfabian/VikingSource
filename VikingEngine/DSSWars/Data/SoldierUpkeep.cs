using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct SoldierUpkeep
    {
        public float food;
        public float copper;

        public static SoldierUpkeep operator +(SoldierUpkeep value1, SoldierUpkeep value2)
        {
            value1.food += value2.food;
            value1.copper += value2.copper;
            return value1;
        }
    }
}
