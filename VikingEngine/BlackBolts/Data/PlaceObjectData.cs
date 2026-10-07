using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Mission;

namespace VikingEngine.Core.BlackBolts.Data
{
    struct PlaceObjectData
    {
        public MapPlacement mapPlacement;
        public ToolSetupComponent component;
        public bool includeItem;
        public ResourceType resourceType;
        public MachineId machineId;
    }
}
