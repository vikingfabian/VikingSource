using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VikingEngine.Core.Wand.GO.ObjectPointer;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.Core.Wand.Map
{
    class BSPNode
    {
        public int Index;
        public Rectangle2 space;
        public BSPNode left, right;
        public bool splitH;
        public int splitPos;

        public int doorsCount = 0;
        public Tile doorTile = null;

        public bool treasureRoom;
    }

    class GenerateMap
    {
        static readonly IntervalF LevelSizeMinMax = new IntervalF(30, 40);
        static readonly IntervalF RoomSizeMinMax = new IntervalF(4, 9);

        const int ThickWallLayers = 3;
        MapData mapData;
        

        public GenerateMap(MapData mapData)
        {
            this.mapData = mapData;
        }

        /// <param name="enterRoomSize">First room must be this size</param>
        public void Generate(/*IntVector2? enterRoomSize*/)
        {
            var leaves = mapData.leaves;
            newLevelSetup();

            for (int y = 0; y < mapData.tileGrid.Size.Y; y++)
            {
                for (int x = 0; x < mapData.tileGrid.Size.X; x++)
                {
                    Tile t = mapData.tileGrid.Get(x, y);
                    if (x == 0 || y == 0 || x == mapData.tileGrid.Size.X - 1 || y == mapData.tileGrid.Size.Y - 1)
                        t.tileType = TileType.UnbreakableWall;
                    else
                        t.tileType = TileType.ThickWall;
                }
            }

            // 2 & 3. Generate big rooms using a BSP tree (guarantees unhindered path & fills most of level)
            int playableW = mapData.tileGrid.Size.X - ThickWallLayers * 2;
            int playableH = mapData.tileGrid.Size.Y - ThickWallLayers * 2;

            BSPNode root = new BSPNode
            {
                space = new Rectangle2(ThickWallLayers, ThickWallLayers, playableW, playableH)
            };

            
            SplitBSP(root, leaves);

            // Sort rooms positionally to grab reliable corners
            var sortedLeaves = leaves.OrderBy(l => l.space.X + l.space.Y).ToList();
            //var enterNode = sortedLeaves.First();
            //var exitNode = sortedLeaves.Last()
            // Find all four corners using diagonal math
            var sortedByAdd = leaves.OrderBy(l => l.space.X + l.space.Y).ToList();
            var sortedBySub = leaves.OrderBy(l => l.space.X - l.space.Y).ToList();

            BSPNode topLeft = sortedByAdd.First();
            BSPNode bottomRight = sortedByAdd.Last();
            BSPNode bottomLeft = sortedBySub.First();
            BSPNode topRight = sortedBySub.Last();

            // Collect unique corners (Distinct prevents picking the same room twice in very small maps)
            var corners = new List<BSPNode> { topLeft, bottomRight, bottomLeft, topRight }.Distinct().ToList();

            // Pick a random corner for the entrance
            int enterIndex = Ref.rnd.Int(corners.Count);
            var enterNode = corners[enterIndex];
            corners.RemoveAt(enterIndex);

            // Pick a random corner from the remaining options for the exit
            var exitNode = corners[Ref.rnd.Int(corners.Count)];

            // Carve floor for all generated rooms
            foreach (var leaf in leaves)
            {
                int w = leaf.space.Width;
                int h = leaf.space.Height;

                // Enforce forced size on the enter room if supplied
                //if (leaf == enterNode && enterRoomSize.HasValue)
                //{
                //    w = Math.Min(w, enterRoomSize.Value.X);
                //    h = Math.Min(h, enterRoomSize.Value.Y);
                //}

                for (int y = leaf.space.Y; y < leaf.space.Y + h; y++)
                {
                    for (int x = leaf.space.X; x < leaf.space.X + w; x++)
                    {
                        mapData.tileGrid.Get(x, y).tileType = TileType.Floor;
                    }
                }
            }

            // Setup Player Start
            mapData.playerEnterPos = new IntVector2(
                enterNode.space.X + enterNode.space.Width / 2,
                enterNode.space.Y + enterNode.space.Height / 2);

            // Setup Level Exit
            IntVector2 exitPos = new IntVector2(
                exitNode.space.X + exitNode.space.Width / 2,
                exitNode.space.Y + exitNode.space.Height / 2);

            mapData.tileGrid.Get(exitPos.X, exitPos.Y).tileType = TileType.ExitLevel;

            // Connect rooms with 1-tile door openings to guarantee pathing
            ConnectBSP(root);

            // Rarely lock a dead-end room that isn't required for progression
            int treasureRoomCount = Ref.rnd.Int(1, 4);
            var deadEnds = leaves.Where(l => l.doorsCount == 1 && l != enterNode && l != exitNode).ToList();
            treasureRoomCount = Math.Min(deadEnds.Count, treasureRoomCount);

            for (int i = 0; i < treasureRoomCount; ++i)
            {
                var lockedLeaf = arraylib.RandomListMemberPop(deadEnds);//deadEnds[Ref.rnd.Int(deadEnds.Count)];
                if (lockedLeaf.doorTile != null)
                {
                    lockedLeaf.treasureRoom = true;
                    if (lockedLeaf.doorTile.northWall == ThinWall.Door) lockedLeaf.doorTile.northWall = ThinWall.LockedDoor;
                    if (lockedLeaf.doorTile.eastWall == ThinWall.Door) lockedLeaf.doorTile.eastWall = ThinWall.LockedDoor;
                }
            }

            // 4. Populate with random thin walls, items, and creatures
            for (int i = 0; i < leaves.Count; ++i)//each (var leaf in leaves)
            {
                var leaf = leaves[i];
                leaf.Index = i;
                DecorateRoom(leaf, leaf == enterNode);
            }
        }

        void newLevelSetup()
        {
            // 1. Fill all tiles with thick wall, outermost unbreakable
            mapData.tileGrid = new Grid2D_L<Tile>(new IntVector2((int)LevelSizeMinMax.GetRandom(), (int)LevelSizeMinMax.GetRandom()) + ThickWallLayers * 2);
            for (int y = 0; y < mapData.tileGrid.Size.Y; y++)
            {
                for (int x = 0; x < mapData.tileGrid.Size.X; x++)
                {
                    var t = new Tile();
                    if (x == 0 || y == 0 || x == mapData.tileGrid.Size.X - 1 || y == mapData.tileGrid.Size.Y - 1)
                    { t.tileType = TileType.UnbreakableWall; }
                    else
                    { t.tileType = TileType.ThickWall; }

                    mapData.tileGrid.Set(x, y, t);
                }
            }
        }

       

        // --- BSP Procedural Generation Helpers ---

        //private struct Rect
        //{
        //    public int X, Y, Width, Height;
        //    public Rect(int x, int y, int w, int h) { X = x; Y = y; Width = w; Height = h; }
        //}

        

        private void SplitBSP(BSPNode node, List<BSPNode> leaves)
        {
            int minSize = (int)RoomSizeMinMax.Min;
            int maxSize = (int)RoomSizeMinMax.Max;

            bool mustSplitH = node.space.Width > maxSize;
            bool mustSplitV = node.space.Height > maxSize;

            bool canSplitH = node.space.Width >= (minSize * 2) + 1;
            bool canSplitV = node.space.Height >= (minSize * 2) + 1;

            if (mustSplitH || mustSplitV || ((canSplitH || canSplitV) && Ref.rnd.Int(2) == 0))
            {
                bool splitH = false;
                if (mustSplitH && !mustSplitV) splitH = true;
                else if (mustSplitV && !mustSplitH) splitH = false;
                else if (canSplitH && canSplitV) splitH = Ref.rnd.Int(2) == 0;
                else if (canSplitH) splitH = true;
                else splitH = false;

                if (splitH)
                {
                    int split = Ref.rnd.Int(node.space.X + minSize, node.space.X + node.space.Width - minSize);
                    node.splitH = true;
                    node.splitPos = split;

                    node.left = new BSPNode { space = new Rectangle2(node.space.X, node.space.Y, split - node.space.X, node.space.Height) };
                    node.right = new BSPNode { space = new Rectangle2(split + 1, node.space.Y, node.space.X + node.space.Width - split - 1, node.space.Height) };
                }
                else
                {
                    int split = Ref.rnd.Int(node.space.Y + minSize, node.space.Y + node.space.Height - minSize);
                    node.splitH = false;
                    node.splitPos = split;

                    node.left = new BSPNode { space = new Rectangle2(node.space.X, node.space.Y, node.space.Width, split - node.space.Y) };

                    // FIX: Added node.space.Y to calculate the remaining bounds correctly
                    node.right = new BSPNode { space = new Rectangle2(node.space.X, split + 1, node.space.Width, (node.space.Y + node.space.Height) - split - 1) };
                }

                SplitBSP(node.left, leaves);
                SplitBSP(node.right, leaves);
            }
            else
            {
                leaves.Add(node);
            }
        }

        private void ConnectBSP(BSPNode node)
        {
            if (node.left == null || node.right == null) return;

            ConnectBSP(node.left);
            ConnectBSP(node.right);

            List<IntVector2> validDoorSpots = new List<IntVector2>();

            // Search the dividing line for a tile where both adjacent cells are floor tiles
            if (node.splitH)
            {
                int x = node.splitPos;
                for (int y = node.space.Y; y < node.space.Y + node.space.Height; y++)
                {
                    if (mapData.tileGrid.Get(x - 1, y).tileType == TileType.Floor &&
                        mapData.tileGrid.Get(x + 1, y).tileType == TileType.Floor)
                    {
                        validDoorSpots.Add(new IntVector2(x, y));
                    }
                }
            }
            else
            {
                int y = node.splitPos;
                for (int x = node.space.X; x < node.space.X + node.space.Width; x++)
                {
                    if (mapData.tileGrid.Get(x, y - 1).tileType == TileType.Floor &&
                        mapData.tileGrid.Get(x, y + 1).tileType == TileType.Floor)
                    {
                        validDoorSpots.Add(new IntVector2(x, y));
                    }
                }
            }

            if (validDoorSpots.Count > 0)
            {
                IntVector2 doorPos = validDoorSpots[Ref.rnd.Int(validDoorSpots.Count)];
                Tile t = mapData.tileGrid.Get(doorPos.X, doorPos.Y);

                // Carve the thick wall to make a 1-tile size opening
                t.tileType = TileType.Floor;

                // Place the conceptual thin door on the correct axis
                if (node.splitH) t.eastWall = ThinWall.Door;
                else t.northWall = ThinWall.Door;

                // Track connections so we can find safe dead-end rooms later
                var leftLeaf = GetLeafAt(node.left, doorPos);
                var rightLeaf = GetLeafAt(node.right, doorPos);
                if (leftLeaf != null) { leftLeaf.doorsCount++; leftLeaf.doorTile = t; }
                if (rightLeaf != null) { rightLeaf.doorsCount++; rightLeaf.doorTile = t; }
            }
        }

        private BSPNode GetLeafAt(BSPNode node, IntVector2 pos)
        {
            if (node.left == null && node.right == null) return node;
            if (node.splitH)
                return pos.X <= node.splitPos ? GetLeafAt(node.left, pos) : GetLeafAt(node.right, pos);
            else
                return pos.Y <= node.splitPos ? GetLeafAt(node.left, pos) : GetLeafAt(node.right, pos);
        }

        private void DecorateRoom(BSPNode node, bool isEnterRoom)//Rectangle2 space, bool isEnterRoom)
        {
            // 0. List all enter/exit spaces (doorways) connected to this room
            HashSet<IntVector2> roomDoors = new HashSet<IntVector2>();

            // Check North and South perimeters
            for (int x = node.space.X; x < node.space.X + node.space.Width; x++)
            {
                // North (Y - 1)
                if (mapData.tileGrid.Get(x, node.space.Y - 1).tileType == TileType.Floor)
                    roomDoors.Add(new IntVector2(x, node.space.Y));

                // South (Y + Height)
                if (mapData.tileGrid.Get(x, node.space.Y + node.space.Height).tileType == TileType.Floor)
                    roomDoors.Add(new IntVector2(x, node.space.Y + node.space.Height - 1));
            }

            // Check West and East perimeters
            for (int y = node.space.Y; y < node.space.Y + node.space.Height; y++)
            {
                // West (X - 1)
                if (mapData.tileGrid.Get(node.space.X - 1, y).tileType == TileType.Floor)
                    roomDoors.Add(new IntVector2(node.space.X, y));

                // East (X + Width)
                if (mapData.tileGrid.Get(node.space.X + node.space.Width, y).tileType == TileType.Floor)
                    roomDoors.Add(new IntVector2(node.space.X + node.space.Width - 1, y));
            }

            if (!isEnterRoom)
            {
                // 1. Place items and creatures anywhere in the room (including edge tiles)
                int numItems = Ref.rnd.Int(3, 5);
                int numCreatures = Ref.rnd.Int(2, 5);

                int maxLoops = 1000;
                while (numItems + numCreatures > 0 && maxLoops > 0)
                {
                    // Full bounds used so they can spawn against the perimeter
                    //int rx = Ref.rnd.Int(node.space.X, node.space.X + node.space.Width);
                    //int ry = Ref.rnd.Int(node.space.Y, node.space.Y + node.space.Height);
                    var rPos = node.space.RandomTile();

                    if (roomDoors.Contains(rPos) == false)
                    {
                        Tile t = mapData.tileGrid.Get(rPos);

                        if (numItems > 0)
                        {
                            if (t.item.itemType == TileItemType.NONE)
                                t.item = new ItemPointer { itemType = TileItemType.SpawnPos, index = node.Index };

                            numItems--;
                        }
                        else
                        {
                            if (t.creature.creatureType == CreatureType.NONE)
                                t.creature = new CreaturePointer { creatureType = CreatureType.SpawnPos, index = node.Index };

                            numCreatures--;
                        }
                    }

                    maxLoops--;
                }

                nodeLabyrinth(node);
            }
           
        }

        void nodeLabyrinth(BSPNode node)
        {
            // 2. Generate labyrinth thin walls with linked probability
            // We only place walls strictly inside the room to prevent blocking external doors on the boundary.
            List<(IntVector2 pos, bool isNorth)> placedWalls = new List<(IntVector2, bool)>();

            for (int y = node.space.Y; y < node.space.Y + node.space.Height; y++)
            {
                for (int x = node.space.X; x < node.space.X + node.space.Width; x++)
                {
                    Tile t = mapData.tileGrid.Get(x, y);

                    // Internal North Wall
                    if (y > node.space.Y && t.northWall == ThinWall.None)
                    {
                        double chance = 0.25; // Base chance
                                              // Higher probability if linked to the previous wall on this axis
                        if (x > node.space.X && mapData.tileGrid.Get(x - 1, y).northWall == ThinWall.Wall) chance = 0.70;

                        if (Ref.rnd.Chance(chance))
                        {
                            t.northWall = ThinWall.Wall;
                            placedWalls.Add((new IntVector2(x, y), true));
                        }
                    }

                    // Internal East Wall
                    if (x < node.space.X + node.space.Width - 1 && t.eastWall == ThinWall.None)
                    {
                        double chance = 0.25;
                        // Higher probability if linked to the previous wall on this axis
                        if (y > node.space.Y && mapData.tileGrid.Get(x, y - 1).eastWall == ThinWall.Wall) chance = 0.70;

                        if (Ref.rnd.Chance(chance))
                        {
                            t.eastWall = ThinWall.Wall;
                            placedWalls.Add((new IntVector2(x, y), false));
                        }
                    }
                }
            }

            // 3. Cut away walls that block passage using Union-Find (Kruskal's algorithm)
            int tileCount = node.space.Width * node.space.Height;
            int[] parent = new int[tileCount];
            for (int i = 0; i < tileCount; i++) parent[i] = i;

            // Local functions for region tracking
            int Find(int i)
            {
                if (parent[i] == i) return i;
                return parent[i] = Find(parent[i]);
            }
            void Union(int i, int j)
            {
                int rootI = Find(i);
                int rootJ = Find(j);
                if (rootI != rootJ) parent[rootI] = rootJ;
            }

            int GetIndex(int x, int y) => (x - node.space.X) + (y - node.space.Y) * node.space.Width;

            // First pass: link together all tiles that are NOT separated by a ThinWall.Wall
            for (int y = node.space.Y; y < node.space.Y + node.space.Height; y++)
            {
                for (int x = node.space.X; x < node.space.X + node.space.Width; x++)
                {
                    int curr = GetIndex(x, y);
                    Tile t = mapData.tileGrid.Get(x, y);

                    // Connect North
                    if (y > node.space.Y && t.northWall != ThinWall.Wall)
                        Union(curr, GetIndex(x, y - 1));

                    // Connect East
                    if (x < node.space.X + node.space.Width - 1 && t.eastWall != ThinWall.Wall)
                        Union(curr, GetIndex(x + 1, y));
                }
            }

            // Shuffle the walls we placed to knock them down randomly
            for (int i = 0; i < placedWalls.Count; i++)
            {
                int swapIndex = Ref.rnd.Int(placedWalls.Count);
                var temp = placedWalls[i];
                placedWalls[i] = placedWalls[swapIndex];
                placedWalls[swapIndex] = temp;
            }

            // Second pass: iterate shuffled walls and knock down any that isolate regions
            foreach (var wall in placedWalls)
            {
                int curr = GetIndex(wall.pos.X, wall.pos.Y);
                int neighbor = wall.isNorth ? GetIndex(wall.pos.X, wall.pos.Y - 1) : GetIndex(wall.pos.X + 1, wall.pos.Y);

                // If the two tiles are in different sets, this wall is blocking a necessary path
                if (Find(curr) != Find(neighbor))
                {
                    Union(curr, neighbor); // Merge the regions
                    Tile t = mapData.tileGrid.Get(wall.pos.X, wall.pos.Y);

                    if (wall.isNorth) t.northWall = ThinWall.None; // Cut it away
                    else t.eastWall = ThinWall.None;
                }
            }
        }
    }
}
