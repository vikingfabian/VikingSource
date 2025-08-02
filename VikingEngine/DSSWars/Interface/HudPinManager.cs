using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Interface
{
    //HUD Pins
    enum HudPinType
    { 
        Resource,
    }

    struct HudPin
    {
        public HudPinType type;
        public int id;

        public HudPin(ItemResourceType itemResource)
        { 
            type = HudPinType.Resource;
            id = (int)itemResource;
        }
    }

    class CityHudPin : List<HudPinType>
    { 
        
    }

    class HudPinManager : Dictionary<int, CityHudPin>
    {
        public HudPinManager() :
            base(8)
        { }

        //public bool 
    }
}
