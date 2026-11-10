using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.GO.ObjectPointer;

namespace VikingEngine.Core.Wand.Map
{
    static class RevealRoom
    {
        public static void Reveal(IntVector2 enterPos)
        {
            var tileGrid = WandRef.world.map.tileGrid; 

            RoomSpawner roomSpawner = new RoomSpawner();

            // Standard flood-fill algorithm to reveal contiguous floor tiles and walls
            Queue<IntVector2> q = new Queue<IntVector2>();
            revealTile(enterPos);
            tileGrid.Get(enterPos).explored = false;

            while (q.Count > 0)
            {
                var curr = q.Dequeue();

                if (curr.X < 0 || curr.Y < 0 || curr.X >= tileGrid.Size.X || curr.Y >= tileGrid.Size.Y)
                    continue;

                Tile t = tileGrid.Get(curr.X, curr.Y);

                if (t.explored || t.tileType == TileType.UnbreakableWall)
                    continue;

                t.explored = true;

                // Stop expanding the flood fill once we hit a thick wall (so it only reveals this room and its boundary)
                if (t.tileType != TileType.ThickWall)
                {
                    // Check East (+X)
                    if (t.eastWall != ThinWall.Door && t.eastWall != ThinWall.LockedDoor)
                        revealTile(new IntVector2(curr.X + 1, curr.Y));

                    // Check West (-X): requires checking the east wall of the tile to the left
                    if (curr.X > 0)
                    {
                        var westTile = tileGrid.Get(curr.X - 1, curr.Y);
                        if (westTile.eastWall != ThinWall.Door && westTile.eastWall != ThinWall.LockedDoor)
                            revealTile(new IntVector2(curr.X - 1, curr.Y));
                    }

                    // Check South (+Y): requires checking the north wall of the tile below
                    if (curr.Y < tileGrid.Size.Y - 1)
                    {
                        var southTile = tileGrid.Get(curr.X, curr.Y + 1);
                        if (southTile.northWall != ThinWall.Door && southTile.northWall != ThinWall.LockedDoor)
                            revealTile(new IntVector2(curr.X, curr.Y + 1));
                    }

                    // Check North (-Y)
                    if (t.northWall != ThinWall.Door && t.northWall != ThinWall.LockedDoor)
                        revealTile(new IntVector2(curr.X, curr.Y - 1));
                }
            }

            roomSpawner.Spawn();

            void revealTile(IntVector2 pos)
            {
                q.Enqueue(pos);

                var tile = tileGrid.Get(pos);
                if (tile.item.itemType == TileItemType.SpawnPos)
                {
                    roomSpawner.Add(tile.item, pos);
                }
                if (tile.creature.creatureType == CreatureType.SpawnPos)
                {
                    roomSpawner.Add(tile.creature, pos);
                }
            }
        }
    }
}
