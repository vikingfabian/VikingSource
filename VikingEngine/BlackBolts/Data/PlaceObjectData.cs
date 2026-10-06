using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;

namespace VikingEngine.Core.BlackBolts.Data
{
    struct PlaceObjectData
    {
        public MapPlacement mapPlacement;
        public FactoryObjectType factoryObjectType;
        public bool includeItem;
        public ResourceType resourceType;
        public MachineId machineId;
    }
}
