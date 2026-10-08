using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO.Creature;

namespace VikingEngine.Core.BlackBolts.GO
{
    struct ObjectPointer
    {
        public static readonly ObjectPointer Empty = new ObjectPointer();
        public bool hasValue;
        public int objIndex;
        public ObjectListType listType;

        public AbsCreature GetCreature()
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

        public bool Equals(ObjectPointer other)
        {
            return hasValue && other.hasValue && objIndex == other.objIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectPointer other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(hasValue, objIndex);
        }

        public static bool operator ==(ObjectPointer left, ObjectPointer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjectPointer left, ObjectPointer right)
        {
            return !left.Equals(right);
        }
    }

}
