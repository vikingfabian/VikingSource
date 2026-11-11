using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VikingEngine.Core.BlackBolts.Map;
    using VikingEngine.Core.Wand.GO.ObjectPointer;

    class MapData
    {
        public IntVector2 playerEnterPos;
        public Grid2D_L<Tile> tileGrid;
        public List<BSPNode> leaves = new List<BSPNode>();
        
        public bool CanMove_WallCheck(IntVector2 from, IntVector2 to, out ThinWallData door)
        {   
            if (tileGrid.Get(to).tileType == TileType.Floor)
            {
                door = GetWall(from, to);
                return door.wallType != ThinWall.Wall;
            }
            door = ThinWallData.None;
            return false;
        }

        public bool CanMove_CreatureCheck(IntVector2 to, out CreaturePointer pcreature)
        {
            pcreature = tileGrid.Get(to).creature;
            return !pcreature.HasGameObject();
        }

        public ThinWallData GetWall(IntVector2 from, IntVector2 to)
        {
            IntVector2 dir = to - from;
            if (dir.X > 0)
            {
                return ThinWallData.Pull(from, false);
            }
            else if (dir.X < 0)
            {
                return ThinWallData.Pull(to, false);
            }
            else if (dir.Y > 0)
            {
                return ThinWallData.Pull(to, true);
            }
            else
            {
                return ThinWallData.Pull(from, true);
            }
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

    struct ThinWallData
    {
        public static ThinWallData None = new ThinWallData() { wallType = ThinWall.None };

        public ThinWall wallType;
        public ThinWallPos wallPos;

        public ThinWallData(ThinWall wallType, ThinWallPos wallPos)
        {
            this.wallType = wallType;
            this.wallPos = wallPos;
        }

        public static ThinWallData Pull(IntVector2 tilePos, bool north)
        {
            ThinWallData result = ThinWallData.None;
            result.wallPos = new ThinWallPos(tilePos, north);
            var tile = WandRef.world.map.tileGrid.Get(tilePos);
            result.wallType = north ? tile.northWall : tile.eastWall;

            return result;
        }

        
    }
    struct ThinWallPos
    {
        public IntVector2 tilePos;
        public bool north;

        public ThinWallPos(IntVector2 tilePos, bool north)
        {
            this.tilePos = tilePos;
            this.north = north;
        }

        public void RemoveWall()
        {
            var tile = WandRef.world.map.tileGrid.Get(tilePos);
            if (north)
            {
                tile.northWall = ThinWall.None;
            }
            else
            { 
                tile.eastWall = ThinWall.None;
            }
            RevealRoom.Reveal(tilePos);
            WandRef.PlayScene.mapmodel.floorNeedsUpdate = true;
        }
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

    
}
