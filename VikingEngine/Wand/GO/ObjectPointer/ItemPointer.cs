using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.GO.Item;

namespace VikingEngine.Core.Wand.GO.ObjectPointer
{
    struct ItemPointer
    {
        public static readonly ItemPointer Empty = new ItemPointer();
        public TileItemType itemType;
        public int index;

        public bool HasGameObject()
        {
            return itemType > TileItemType.SpawnPos;
        }

        public AbsItem Get()
        {
            WandRef.world.items.TryGetIndex(index, out var result);
            return result;
        }
    }
    enum TileItemType
    {
        NONE,
        SpawnPos,

        Health_potion,
        Book,
        Magic_wand,
        Altar,
        DeadBody,

        Throwing_knife,
    }

}
