using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class MapData
    {
        public IntVector2 playerEnterPos;
        public Grid2D_L<Tile> tileGrid;

        private Random rnd = new Random();


        public MapData()
        {
            WandRef.MapData = this;
        }
        
    }

    class Tile
    {
        public TileType tileType;
        public bool explored = false;
        public ThinWall northWall, eastWall;
        public ItemPointer item;
        public CreaturePointer creature;
    }

    enum TileType
    {
        Floor,
        ExitLevel,
        ThickWall,
        UnbreakableWall,
    }

    /// <summary>/// Walls between tiles/// </summary>
    enum ThinWall
    {
        None,
        Door,
        /// <summary>
        /// Locked doors can only open rooms not required to exit the level
        /// </summary>
        LockedDoor,
        Wall,
    }

    struct ItemPointer
    {
        public TileItemType itemType;
        public int index;
    }

    struct CreaturePointer
    {
        public CreatureType creatureType;
        public int index;
    }

    enum TileItemType
    {
        NONE,
        Weapon, // Corrected typo from Weeapon
        Chest,
        WaterBarrel,
        DeadBody,
    }

    enum CreatureType
    {
        NONE,
        Hen,
        Goblin,
        Orc,
        Spider,
    }
}
