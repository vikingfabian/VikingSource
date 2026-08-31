using System;
using System.Collections.Generic;
using VikingEngine;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.Benchmarks.Pathfinding.Legacy
{
    class LegacyPathFinding
    {
        public const int MaxNodeLength = 30000;

        List<LegacyPathNode> open = new List<LegacyPathNode>();
        Grid2D_L<LegacyPathNode> nodeGrid;

        public LegacyPathFinding()
        {
            nodeGrid = new Grid2D_L<LegacyPathNode>(DssRef.world.Size);//new PathNode[DssRef.world.Size.X, DssRef.world.Size.Y];
        }

        //conv.ToDir8_INT(startDir)
        public WalkingPath FindPath(int pathThreadIndex, IntVector2 center, int startDir, IntVector2 goal, bool startAsShip)
        {
            /*
            * Path finding algorithm
            * ruta in världen, kanske var fjärde ruta
            * 1. Kolla 8riktingar
            * 2. Ge värde till rutorna
            * G - kostnad att gå dit, 10 rakt, 14 diagonalt
            * H - Avståndet till målet X + Y
            * F - totalt värde G+H
            * Parent - håll reda på parent ruta
            * -värdet ska vara oändligt om det finns hinder
            * -en liten bonus (2poäng) om man behåller riktingen, checka mot parentDir
            * 3.Varje kollad center ruta ska till en sluten lista
            * 4.Varje ny ruta ska till en öppen lista
            */

            if (!DssRef.world.tileGrid.InBounds(center) ||
                !DssRef.world.tileGrid.InBounds(goal))
            {
                return new WalkingPath();
            }

            LegacyPathNode startNode = new LegacyPathNode(center, startDir, startAsShip);

            //nodeGrid[center.X, center.Y] = startNode;
            nodeGrid.Set(center, startNode);

            bool endAsShip = DssRef.world.tileGrid.Get(goal).IsWater();
            LegacyPathNode currentNode = startNode;

            int numLoops = 0;

            while (true)
            {
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    if (DssRef.world.tileBounds.IntersectTilePoint(pos) && !nodeGrid.Get(pos).HasValue)
                    {
                        //add a node to open list
                        LegacyPathNode node = new LegacyPathNode(pos, dir, DssRef.world, currentNode, goal, endAsShip);
                        open.Add(node);
                        nodeGrid.Set(pos, node);
                    }
                }

                var lowValue = float.MaxValue;
                var lowHeuristic = float.MaxValue;
                int lowIndex = -1;
                for (int i = 0; i < open.Count; i++)
                {
                    if (Math.Abs(open[i].Value - lowValue) < 0.5f)
                    {
                        // Pick the node that is closer to the goal
                        if (open[i].Heuristic < lowHeuristic)
                        {
                            lowHeuristic = open[i].Heuristic;
                            lowIndex = i;
                        }
                    }
                    else if (open[i].Value < lowValue)
                    {
                        lowValue = open[i].Value;
                        lowHeuristic = open[i].Heuristic; // Store H
                        lowIndex = i;
                    }
                }

                if (open.Count > 1)
                {
                    currentNode = open[lowIndex];
                    open.RemoveAt(lowIndex);
                }

                currentNode.closed = true;
                nodeGrid.Set(currentNode.Position, currentNode);

                if (currentNode.Position == goal)
                {
                    break;
                }

                numLoops++;
                if (numLoops > 20000)
                {
                    break;
                }
            }

            //List<PathNodeResult> result = new List<PathNodeResult>();
            WalkingPath path;

            if (pathThreadIndex < 0)
            {
                path = new WalkingPath();
            }
            else
            {
                path = DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.GetRes();
            }

            while (currentNode.Position != startNode.Position)
            {
                path.nodes.Add(new PathNodeResult(currentNode.Position, currentNode.ship));
                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid.Get(pos);

                numLoops++;
                if (numLoops > MaxNodeLength)
                {
                    throw new EndlessLoopException("");
                }
            }

            path.init();
            return path;
        }

        public void recycle()
        {
            open.Clear();
            nodeGrid.Clear();
            //for (int y = 0; y < DssRef.world.Size.Y; ++y)
            //{
            //    for (int x = 0; x < DssRef.world.Size.X; ++x)
            //    {
            //        nodeGrid[x, y] = PathNode.Empty;
            //    }
            //}
        }
    }
}
