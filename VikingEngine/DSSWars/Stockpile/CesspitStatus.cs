using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Stockpile;

namespace VikingEngine.DSSWars.Stockpile
{
    struct CesspitStatus
    {
        public int idAndPosition;
        public ItemResourceType type;
    }
}

namespace VikingEngine.DSSWars.GameObject
{
   
    partial class City
    {
        List<CesspitStatus> cesspits = null;
    }
}
