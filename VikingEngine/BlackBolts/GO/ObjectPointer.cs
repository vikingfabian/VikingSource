using System;
using System.Collections.Generic;
using System.Text;

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

        public AbsMachine GetStaticItem()
        {
            if (hasValue)
            {
                return BlackRef.mapData.staticObjectList.GetIndex_Safe(objIndex);
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

    enum GameObjectType
    {
        Worker,
        Belt,
        Spin_plate,

        Table,
        Dispencer,
        Delivery_point,
        NUM_NONE
    }

    enum ObjectListType
    {
        Unknown,
        Creature,
        Static,
        SolidResource,
    }

    enum ResourceType
    { 
        Box,
        Flesh,
        Bone,
        Grilled_meat,
        NUM
    }
}
