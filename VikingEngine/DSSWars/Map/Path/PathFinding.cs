//#define VISUAL_NODES
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Map
{
    //TODO heatmap för vatten

    class PathFindingPool
    {
        //Represents a thread-safe last in-first out (LIFO) collection.
        ConcurrentStack<PathFinding> poolPf = new ConcurrentStack<PathFinding>();
        ConcurrentQueue<WalkingPath> poolRes = new ConcurrentQueue<WalkingPath>();
        int createdPfCount = 0;

        public int PfCount => poolPf.Count;
        public int ResCount => poolRes.Count;
        public int CreatedPfCount => createdPfCount;
        
        public PathFinding GetPf()
        {
            if (poolPf.TryPop(out PathFinding path))
            {
                return path;
            }
            else
            {
                System.Threading.Interlocked.Increment(ref createdPfCount);
                return new PathFinding();
            }
        }

        public WalkingPath GetRes()
        {
            if (poolRes.TryDequeue(out WalkingPath path))
            {
                path.recycle();
                return path;
            }
            else
            {
                return new WalkingPath();
            }
        }

        public void Return(PathFinding path)
        {
            // Reset the node to a default state
            if (path != null)
            {
                path.recycle();
                poolPf.Push(path);
            }
        }

        public void Return(WalkingPath pathresult)
        {
            // Reset the node to a default state
            if (pathresult != null)
            {
                poolRes.Enqueue(pathresult);
            }
        }

        public void Preallocate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                poolPf.Push(new PathFinding());
                System.Threading.Interlocked.Increment(ref createdPfCount);
            }
        }

        public void Clear()
        {
            poolPf.Clear();
            poolRes.Clear();
        }
    }



    class PathFinding
    {
        public const int MaxNodeLength = 30000;

        // Min-heap open list ordered by (TotalCost, Heuristic)
        PriorityQueue<PathNode, (float Value, float Heuristic)> open = new PriorityQueue<PathNode, (float, float)>();
        Grid2D_L<PathNode> nodeGrid;

        // Generation counter for O(1) visited checks.
        private int _currentRunId = 1;

        public PathFinding()
        {
            nodeGrid = new Grid2D_L<PathNode>(DssRef.world.Size);
        }

        public WalkingPath FindPath(int pathThreadIndex, IntVector2 center, int startDir, IntVector2 goal, bool startAsShip)
        {
            // Short circuit if already at goal or coordinates are out of bounds.
            if (center == goal ||
                !DssRef.world.tileGrid.InBounds(center) ||
                !DssRef.world.tileGrid.InBounds(goal))
            {
                return new WalkingPath();
            }

            PathNode startNode = new PathNode(center, startDir, startAsShip, _currentRunId);
            nodeGrid.Set(center, startNode);

            bool endAsShip = DssRef.world.tileGrid.Get(goal).IsWater();
            PathNode currentNode = startNode;

            int numLoops = 0;

            while (true)
            {
                // Expand neighbours not yet visited in this run.
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    if (DssRef.world.tileBounds.IntersectTilePoint(pos) && nodeGrid.Get(pos).RunId != _currentRunId)
                    {
                        PathNode node = new PathNode(pos, dir, DssRef.world, currentNode, goal, endAsShip, _currentRunId);
                        
                        // Enqueue neighbor with (TotalCost, Heuristic) priority for min-heap ordering.
                        open.Enqueue(node, (node.Value, node.Heuristic));
                        nodeGrid.Set(pos, node);
                    }
                }

                // Break if no reachable path left.
                if (open.Count == 0)
                {
                    break;
                }

                // Dequeue the lowest cost candidate.
                currentNode = open.Dequeue();

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

            WalkingPath path;

            if (pathThreadIndex < 0)
            {
                path = new WalkingPath();
            }
            else
            {
                path = DssRef.state.pathUpdates[pathThreadIndex].pathFindingPool.GetRes();
            }

            // Backtrack path from goal to start.
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
            // O(1) recycle.
            open.Clear();
            _currentRunId++;
            if (_currentRunId == int.MaxValue)
            {
                // Safety guard for integer overflow.
                // Reset counter and clear grid.
                _currentRunId = 1;
                nodeGrid.Clear();
            }
        }
    }

    struct PathNodeResult
    {
        public bool ship;
        public IntVector2 position;

        public PathNodeResult(IntVector2 position, bool ship)
        {
            this.position = position;
            this.ship = ship;
        }

        public bool HasValue()
        {
            return position.X >= 0;
        }

        public override string ToString()
        {
            return position.ToString() + " water {" + ship.ToString() + "}";
        }
    }

    class WalkingPath
    {
#if VISUAL_NODES
        List<Graphics.Mesh> nodeImages;
#endif
        const int IgnoreDirChangeTimes = 10;
        const float NodeMinDistance = 0.3f;

        public int currentNodeIx;
        public List<PathNodeResult> nodes = new List<PathNodeResult>(64);

        public Vector2 DirToNextNode(Vector2 myPos, out bool complete, out bool ship)
        {
            ship = nodes[currentNodeIx].ship;
            IntVector2 to = nodes[currentNodeIx].position;
            Vector2 diff = (to.Vec + VectorExt.V2Half) - myPos;
            if (diff.Length() <= NodeMinDistance)
            {
                --currentNodeIx;
            }
            complete = currentNodeIx < 0;
            diff.Normalize();
            return diff;
        }

        public void recycle()
        {
            nodes.Clear();
            if (nodes.Capacity > 512)
            {
                nodes.Capacity = 256;
            }
        }

//        public WalkingPath(List<PathNodeResult> nodes)
//        {
//            this.nodes = nodes;
//            currentNodeIx = nodes.Count - 1;

        //#if VISUAL_NODES
        //            Ref.update.AddSyncAction(new SyncAction(createVisuals));
        //#endif
        //        }

        public void init(/*List<PathNodeResult> nodes*/)
        {
            //this.nodes = nodes;
            currentNodeIx = nodes.Count - 1;

#if VISUAL_NODES
            Ref.update.AddSyncAction(new SyncAction(createVisuals));
#endif
        }

#if VISUAL_NODES
        void createVisuals()
        {
            nodeImages = new List<Graphics.Mesh>();
            foreach (var n in nodes)
            {
                Vector3 pos = WP.ToSubTileWP_Centered(n.position);
                //WorldPosition wp = new WorldPosition(pos);

                var mesh = new Graphics.Mesh(LoadedMesh.cube_repeating, pos, new Vector3(0.3f), Graphics.TextureEffectType.Flat, SpriteName.ArmourGold, Color.White, false);
                mesh.AddToRender(DrawGame.UnitDetailLayer);
                nodeImages.Add(mesh);
            }
            new TimedAction0ArgTrigger(deleteVisuals, 10000);
        }

        void deleteVisuals()
        {
            foreach (var img in nodeImages)
            {
                img.DeleteMe();
            }
        }
#endif

        public bool TryGetCurrentNode(out PathNodeResult node)
        {
            int ix = currentNodeIx;

            if (ix >= 0 && ix < nodes.Count)
            {
                node = nodes[ix];
                return true;
            }
            node = new PathNodeResult(IntVector2.MinValue, false);
            return false;
        }

        public bool nextTwoNodesAreShip()
        {
            if (currentNodeIx > 0)
            {
                return nodes[currentNodeIx].ship && nodes[currentNodeIx - 1].ship;
            }
            return false;
        }
        public bool nextTwoNodesAreByFeet()
        {
            if (currentNodeIx > 0)
            {
                return !nodes[currentNodeIx].ship && !nodes[currentNodeIx - 1].ship;
            }
            return false;
        }

        public bool nextNodeIsShip()
        {
            if (currentNodeIx >= 0)
            {
                return nodes[currentNodeIx].ship;
            }
            return false;
        }
        public bool nextNodeIsFeet()
        {
            if (currentNodeIx >= 0)
            {
                return !nodes[currentNodeIx].ship;
            }
            return false;
        }

        public void NextNode()
        {
            --currentNodeIx;
        }

        public bool HasMoreNodes()
        {
            return currentNodeIx >= 0 && nodes.Count > 0;
        }

        public IntVector2 LastNode()
        {
            return nodes[0].position;
        }

        public IntVector2 getNodeAhead(int distanceAhead, IntVector2 start, out bool isTravelNode)
        {
            int maxLoops = 100;

            while (--maxLoops > 0)
            {
                if (HasMoreNodes())
                {
                    int dist = nodes[currentNodeIx].position.SideLength(start);
                    if (dist <= 1)
                    {
                        NextNode();
                    }
                    else
                    { 
                        //Next is distance one away
                        int aheadNode = Bound.Min(currentNodeIx - (distanceAhead -1), 0);
                        isTravelNode = aheadNode >= 2;
                        return nodes[aheadNode].position;
                    }
                }
                else
                {
                    isTravelNode = false;
                    return start;
                }
            }
            isTravelNode = false;
            return start;
        }

        public void refreshCurrentNode(IntVector2 tilePos, out bool offTrack)
        {
            int maxLoops = 100;

            while (HasMoreNodes() && --maxLoops > 0)
            {
                int dist = nodes[currentNodeIx].position.SideLength(tilePos);
                if (dist <= 1)
                {
                    NextNode();
                }
                else
                {
                    offTrack = dist > 2;
                    return;
                }
            }

            offTrack = false;
            return;
        }

        public int RemoveLast()
        {
            --currentNodeIx;
            nodes.RemoveAt(0);
            return nodes.Count;
        }

        public int PassedNodeCount()
        {
            return nodes.Count - 1 - currentNodeIx;
        }

        public int NodeCountLeft()
        {
            return currentNodeIx;
        }
    }

    struct PathNode 
    {
        const float MoveCostStraight = 10f;
        const float MoveCostDiagonal = 14f;

        public static readonly PathNode Empty = new PathNode();

        public float Value;

        /// <summary>
        /// Distance to goal
        /// </summary>
        public float Heuristic;
        float moveCost;

        public IntVector2 Position;
        public IntVector2 PreviousPosition;

        public int RunId;
        public bool closed;
        public bool waterTile;
        public bool ship;

        int dir8;

        public PathNode(IntVector2 pos, int dir8, bool ship, int runId)
        {
            Position = pos;
            this.dir8 = dir8;
            this.ship = ship;
            RunId = runId;
            closed = true;

            moveCost = 0;
            Value = 0;
            PreviousPosition = pos;
            waterTile = ship;
        }

        public PathNode(IntVector2 pos, int dir8, WorldData world, PathNode parent, IntVector2 goalPos, bool endAsShip, int runId)
        {
            Position = pos;
            this.dir8 = dir8;
            PreviousPosition = parent.Position;
            RunId = runId;
            closed = false;

            moveCost = lib.IsEven(dir8) ? MoveCostStraight : MoveCostDiagonal;
            if (dir8 == parent.dir8)
            { //Bonus for keeping direction
                moveCost -= 1f;
            }

            Tile tile = world.tileGrid.Get(pos);
            waterTile = tile.IsWater();

            if (waterTile != parent.waterTile)
            {
                if (waterTile == endAsShip)
                {//wanted convert
                    moveCost -= 2;
                }
                else
                {
                    moveCost += MoveCostStraight * 16;
                }
            }
            ship = this.waterTile;

            moveCost *= tile.TroupWalkingDistance(ship);

            moveCost += parent.moveCost;

            Heuristic = (pos - goalPos).Length() * MoveCostStraight;

            const float DistanceToGoalWeight = 1.5f;
            Heuristic *= DistanceToGoalWeight;
            this.Value = moveCost + Heuristic;
        }
    }

}

