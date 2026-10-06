using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.GO
{
    struct ObjectPointer
    {
        public static readonly ObjectPointer Empty = new ObjectPointer();
        public bool hasValue;
        public int objIndex;
        public ObjectListType listType;

        public Worker GetCreature()
        {
            if (hasValue)
            {
                return BlackRef.mapData.creatureList.GetIndex_Safe(objIndex);
            }
            return null;
        }

        public AbsMachine GetMachine()
        {
            if (hasValue)
            {
                return BlackRef.mapData.machineList.GetIndex_Safe(objIndex);
            }
            return null;
        }
        public SolidResource GetSolidResource()
        {
            if (hasValue)
            {
                return BlackRef.mapData.resourceList.GetIndex_Safe(objIndex);
            }
            return null;
        }
    }

}
