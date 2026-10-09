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
        public bool locked;
        public ResourceType resourceType;
        public MachineId machineId;
        public FactoryObjectType spawn;

        public void write(System.IO.BinaryWriter w)
        {
            mapPlacement.write(w);
            component.write(w);
            w.Write(includeItem);
            w.Write(locked);
            w.Write((byte)resourceType);
            machineId.write(w);
            w.Write((byte)spawn);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            mapPlacement.read(r, version);
            component.read(r, version);
            includeItem = r.ReadBoolean();
            locked = r.ReadBoolean();
            resourceType = (ResourceType)r.ReadByte();
            machineId.read(r, version);
            spawn = (FactoryObjectType)r.ReadByte();
        }
    }
}
