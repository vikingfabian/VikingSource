using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.Wand.Map
{
    using VikingEngine.EngineSpace;

    struct PathNode
    {
        public IntVector2 pos;
        public int parentIndex; // Index in the allNodes array (-1 for none)
        public int gCost;
        public int hCost;

        public int FCost => gCost + hCost;
    }

    /// <summary>
    /// Zero-allocation A* pathfinder. 
    /// Caches grid and arrays internally so multiple path requests generate no garbage.
    /// </summary>
    class AStarPathfinder
    {
        // Cached collections to prevent allocations
        private StructList<PathNode> allNodes = new StructList<PathNode>(256);
        private StructList<int> openList = new StructList<int>(128);

        // Tracks node presence. 0 = unvisited. > 0 is (index + 1) in allNodes.
        private Grid2D_L<int> visitedGrid = new Grid2D_L<int>();

        // Cached directions to avoid allocating new arrays
        private readonly IntVector2[] DirsXFirst = { new IntVector2(1, 0), new IntVector2(-1, 0), new IntVector2(0, 1), new IntVector2(0, -1) };
        private readonly IntVector2[] DirsYFirst = { new IntVector2(0, 1), new IntVector2(0, -1), new IntVector2(1, 0), new IntVector2(-1, 0) };

        /// <param name="outPath">path goes from start to target, first step is index 1</param>
        public bool FindPath(MapData map, IntVector2 start, IntVector2 target, ref StructList<IntVector2> outPath)
        {
            outPath.Clear();

            if (start == target) return true;
            if (!map.tileGrid.InBounds(start) || !map.tileGrid.InBounds(target)) return false;

            // Ensure our tracking grid matches the current map size
            if (visitedGrid.Size != map.tileGrid.Size)
                visitedGrid.initGrid(map.tileGrid.Size);
            else
                visitedGrid.Clear(); // Quickly zeroes out the array

            allNodes.Clear();
            openList.Clear();

            // Randomize prioritization of X or Y axis for this specific path
            bool prioritizeX = Ref.rnd.Bool();
            IntVector2[] dirs = prioritizeX ? DirsXFirst : DirsYFirst;

            // Initialize Start Node
            allNodes.Add(new PathNode
            {
                pos = start,
                parentIndex = -1,
                gCost = 0,
                hCost = GetManhattan(start, target)
            });

            openList.Add(0);
            visitedGrid.Set(start, 1); // 1-based index (0 means unvisited)

            while (openList.Count > 0)
            {
                // Find node with lowest F cost (or lowest H cost on a tie)
                int bestOpenIndex = 0;
                int bestNodeIndex = openList[0];
                int bestFCost = allNodes[bestNodeIndex].FCost;
                int bestHCost = allNodes[bestNodeIndex].hCost;

                for (int i = 1; i < openList.Count; i++)
                {
                    int nodeIndex = openList[i];
                    ref PathNode node = ref allNodes[nodeIndex];
                    int fCost = node.FCost;

                    if (fCost < bestFCost || (fCost == bestFCost && node.hCost < bestHCost))
                    {
                        bestFCost = fCost;
                        bestHCost = node.hCost;
                        bestOpenIndex = i;
                        bestNodeIndex = nodeIndex;
                    }
                }

                // Pop best node
                openList.RemoveAtSwapBack(bestOpenIndex);
                ref PathNode current = ref allNodes[bestNodeIndex];

                // Check if target reached
                if (current.pos == target)
                {
                    BuildPath(bestNodeIndex, ref outPath);
                    return true;
                }

                // Expand Neighbors
                for (int i = 0; i < 4; i++)
                {
                    IntVector2 neighborPos = current.pos + dirs[i];

                    if (!map.tileGrid.InBounds(neighborPos))
                        continue;

                    ThinWallData door;
                    if (!map.CanMove_WallCheck(current.pos, neighborPos, false, out door))
                        continue;

                    int moveCost = current.gCost + 10;

                    if (map.tileGrid.Get(neighborPos).creature.HasGameObject())
                    {
                        //Another creature in the path
                        moveCost += 4;
                    }

                    int visitedVal = visitedGrid.Get(neighborPos);

                    if (visitedVal == 0) // Not visited yet
                    {
                        int newNodeIndex = allNodes.Count;
                        allNodes.Add(new PathNode
                        {
                            pos = neighborPos,
                            parentIndex = bestNodeIndex,
                            gCost = moveCost,
                            hCost = GetManhattan(neighborPos, target)
                        });

                        openList.Add(newNodeIndex);
                        visitedGrid.Set(neighborPos, newNodeIndex + 1);
                    }
                    else // Already visited
                    {
                        int existingIndex = visitedVal - 1;
                        ref PathNode existingNode = ref allNodes[existingIndex];

                        if (moveCost < existingNode.gCost)
                        {
                            existingNode.gCost = moveCost;
                            existingNode.parentIndex = bestNodeIndex;

                            // Re-evaluate in open list if it was already processed
                            if (!ListContains(openList, existingIndex))
                            {
                                openList.Add(existingIndex);
                            }
                        }
                    }
                }
            }

            return false; // No path found
        }

        private bool ListContains(StructList<int> list, int value)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == value) return true;
            return false;
        }

        private void BuildPath(int targetNodeIndex, ref StructList<IntVector2> outPath)
        {
            int currIndex = targetNodeIndex;
            while (currIndex != -1)
            {
                outPath.Add(allNodes[currIndex].pos);
                currIndex = allNodes[currIndex].parentIndex;
            }

            // Path is built backwards (from target to start), so we reverse it in-place
            int half = outPath.Count / 2;
            for (int i = 0; i < half; i++)
            {
                int swapIdx = outPath.Count - 1 - i;
                IntVector2 temp = outPath[i];
                outPath[i] = outPath[swapIdx];
                outPath[swapIdx] = temp;
            }
        }

        private int GetManhattan(IntVector2 a, IntVector2 b)
        {
            // Manhattan distance calculation multiplied by movement cost (10)
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y)) * 10;
        }
    }
}