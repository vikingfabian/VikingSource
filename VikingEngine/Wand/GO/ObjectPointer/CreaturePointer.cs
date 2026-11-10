using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.GO.ObjectPointer
{

    struct CreaturePointer
    {
        public CreatureType creatureType;
        public int index;
    }



    enum CreatureType
    {
        NONE,
        SpawnPos,

        Hen,
        Goblin,
        Orc,
        Spider,
    }
}
