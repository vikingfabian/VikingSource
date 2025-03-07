using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Defence
{
    struct DefenceStatus
    {
        public const int NoSoldiers = ushort.MaxValue;
        public int soldierGroupId;
        public int idAndPosition;
        public bool autoAssign;
        public void init(IntVector2 subtilepos)
        {
            soldierGroupId = NoSoldiers;
            idAndPosition = conv.IntVector2ToInt(subtilepos);
        }        
    }
}
