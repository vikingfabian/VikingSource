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

        public Worker Get()
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
    }

    enum GameObjectType
    {
        Worker,
        Belt,
        Spin_plate,
        NUM
    }

    enum ObjectListType
    {
        Unknown,
        Creature,
        Static,
    }
}
