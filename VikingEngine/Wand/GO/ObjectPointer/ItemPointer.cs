using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.GO.ObjectPointer
{
    struct ItemPointer
    {
        public TileItemType itemType;
        public int index;
    }
    enum TileItemType
    {
        NONE,
        SpawnPos,

        Weapon, // Corrected typo from Weeapon
        Chest,
        WaterBarrel,
        DeadBody,
    }

}
