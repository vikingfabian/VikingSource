
//#define VISUAL_NODES
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Map.Path
{

    class DetailPathFindingPool
    {
        //Represents a thread-safe last in-first out (LIFO) collection.
        ConcurrentStack<DetailPathFinding> pfPool = new ConcurrentStack<DetailPathFinding>();
        ConcurrentQueue<DetailWalkingPath> resultPool = new ConcurrentQueue<DetailWalkingPath>();
                
        public DetailPathFinding GetPf()
        {
            if (pfPool.TryPop(out DetailPathFinding path))
            {
                return path;
            }
            else
            {
                return new DetailPathFinding();
            }
        }

        public void Return(DetailPathFinding path)
        {
            // Reset the node to a default state
            if (path != null)
            {
                path.recycle();
                pfPool.Push(path);
            }
        }

        public DetailWalkingPath GetRes()
        {   
            if (resultPool.TryDequeue(out DetailWalkingPath path))
            {
                path.recycle();
                return path;
            }
            else
            {
                return new DetailWalkingPath();
            }
        }

        public void Return(DetailWalkingPath pathresult)
        {
            if (pathresult != null)
            {
                resultPool.Enqueue(pathresult);
            }
        }
    }

    class DetailPathFinding
    {
        public const int MaxNodeLength = 30000;
        const int MaxTileRadius = 64 * WorldData.TileSubDivitions;

        // Min-heap open list ordered by: (TotalCost, Heuristic)
        PriorityQueue<DetailPathNode, (float Value, float Heuristic)> open = new PriorityQueue<DetailPathNode, (float, float)>();
        Rectangle2 area;
        Grid2D_L<DetailPathNode> nodeGrid;

        // Generation counter for O(1) visited checks.
        private int _currentRunId = 1;

        public DetailPathFinding()
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(IntVector2.Zero, MaxTileRadius);
            nodeGrid = new Grid2D_L<DetailPathNode>(area.size);
        }

        public DetailWalkingPath FindPath(int pathThreadIndex, IntVector2 center, Rotation1D startDir, IntVector2 goal, bool startAsShip, bool endAsShip, bool isTravelNode)
        {
            // Short-circuit if already at target, out of bounds, or outside max local search radius.
            if (center == goal ||
                !DssRef.world.subTileGrid.InBounds(center) ||
                !DssRef.world.subTileGrid.InBounds(goal) ||
                center.X <= 0 ||
                (goal - center).SideLength() >= MaxTileRadius)
            {
                return null;
            }

#if !DEBUG
            try
            {
#endif

            area = Rectangle2.FromCenterTileAndRadius(center, MaxTileRadius);
            Rectangle2 subtileLimit = DssRef.world.subTileGrid.Area;
            subtileLimit.AddRadius(-1);
            area.SetTileBounds(subtileLimit);
            
            DetailPathNode startNode = new DetailPathNode(center, conv.ToDir8_INT(startDir), startAsShip, _currentRunId);
            {
                IntVector2 gridPos = center - area.pos;
#if DEBUG
                try
                {
#endif
                    nodeGrid.Set(gridPos, startNode);
#if DEBUG
                }
                catch (Exception ex)
                {
                    lib.DoNothing();
                }
#endif
            }
            
            DetailPathNode currentNode = startNode;
            int numLoops = 0;

            while (true)
            {
                // Expand neighbors not yet visited in this run.
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    IntVector2 gridPos = pos - area.pos;
                    if (area.IntersectTilePoint(pos) && nodeGrid.Get(gridPos).RunId != _currentRunId)
                    {
                        DetailPathNode node = new DetailPathNode(pos, dir, DssRef.world, currentNode, goal, endAsShip, _currentRunId);

                        // Enqueue neighbor with (TotalCost, Heuristic) priority.
                        open.Enqueue(node, (node.Value, node.Heuristic));
                        nodeGrid.Set(gridPos, node);
                    }
                }

                // Break if open is empty.
                if (open.Count == 0)
                {
                    break;
                }

                // Dequeue lowest cost candidate.
                currentNode = open.Dequeue();

                currentNode.closed = true;
                nodeGrid.Set(currentNode.Position.X - area.pos.X, currentNode.Position.Y - area.pos.Y, currentNode);

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

            DetailWalkingPath path;
            if (pathThreadIndex < 0)
            {
                path = new ();
            }
            else
            {
                path = DssRef.state.pathUpdates[pathThreadIndex].detailPathFindingPool.GetRes();
            }

            bool blocked = false;
            int totalNodes = 0;

            // Backtrack path from target to start.
            while (currentNode.Position != startNode.Position)
            {
                path.nodes.Add(new DetailPathNodeResult(currentNode.Position, currentNode.ship));

                totalNodes++;
                if (totalNodes > MaxNodeLength)
                {
                    throw new EndlessLoopException("");
                }

                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid.Get(pos.X - area.pos.X, pos.Y - area.pos.Y);
            }

            path.init(goal, blocked);
            return path;

#if !DEBUG
            }
            catch (Exception ex)
            {
                return null;
            }
#endif
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

    struct DetailPathNodeResult
    {
        public bool ship;
        public IntVector2 position;

        public DetailPathNodeResult(IntVector2 position, bool ship)
        {
            this.position = position;
            this.ship = ship;
        }

        public bool HasValue()
        {
            return position.X >= 0;
        }
    }

    class DetailWalkingPath
    {
#if VISUAL_NODES
        List<Graphics.Mesh> nodeImages;
#endif
        const int IgnoreDirChangeTimes = 10;
        static readonly float NodeMinDistance = 0.3f * WorldData.SubTileWidth;

        public int currentNodeIx;
        public IntVector2 goal;
        public List<DetailPathNodeResult> nodes = new List<DetailPathNodeResult>(64);
        public bool blockedPath;
        public void recycle()
        { 
            nodes.Clear();
            if (nodes.Capacity > 512)
            {
                nodes.Capacity = 256;
            }
        }

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

        public Vector3 NextNodeWp(Vector3 myPos, out bool complete, out bool ship)
        {
            var node_sp = currentNodeIx;
            complete = node_sp < 0 || node_sp >= nodes.Count;
            if (complete)
            {
                ship = false;
                return  WP.SubtileToWorldPosXZ(goal);
            }

            ship = nodes[node_sp].ship;
            IntVector2 to = nodes[node_sp].position;
            Vector3 toWp = WP.SubtileToWorldPosXZ(to);
            Vector2 diff = new Vector2( toWp.X - myPos.X, toWp.Z - myPos.Z);
            if (diff.Length() <= NodeMinDistance)
            {
                --currentNodeIx;
            }            
            
            return toWp;
        }

        public DetailWalkingPath()
        {
            
        }

        public void init(IntVector2 goal,/* List<DetailPathNodeResult> nodes,*/ bool blockedPath)
        {
            this.goal = goal;
            //this.nodes = nodes;
            this.blockedPath = blockedPath;
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
                Vector3 pos = WP.SubtileToWorldPosXZgroundY_Centered(n.position);
                pos.Y += WorldData.SubTileHalfWidth;
                var mesh = new Graphics.Mesh(LoadedMesh.cube_repeating, pos,
                   new Vector3(WorldData.SubTileHalfWidth), Graphics.TextureEffectType.Flat, SpriteName.KeyArrowRight, Color.Pink, false);
                mesh.AddToRender(DrawGame.UnitDetailLayer);
                nodeImages.Add(mesh);
            }

            new TimedAction0ArgTrigger(deleteVisuals, 5000);
        }

        void deleteVisuals()
        {
            foreach (var img in nodeImages)
            { 
                img.DeleteMe();
            }
        }
#endif

        public bool TryGetCurrentNode(out DetailPathNodeResult node)
        {
            int ix = currentNodeIx;

            if (ix >= 0 && nodes.Count > 0)
            {
                node = nodes[ix];
                return true;
            }
            node = new DetailPathNodeResult(IntVector2.MinValue, false);
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

    struct DetailPathNode
    {
        const float MoveCostStraight = 10f;
        const float MoveCostDiagonal = 14f;

        public const float MoveCostWall = 20;
        public const float MoveCostHindering = 3;


        public static readonly DetailPathNode Empty = new DetailPathNode();

        public float Heuristic;
        public float Value;
        float moveCost;

        public IntVector2 Position;
        public IntVector2 PreviousPosition;

        public int RunId;
        public bool closed;
        public bool waterTile;
        public bool ship;

        int dir8;

        public DetailPathNode(IntVector2 pos, int dir8, bool ship, int runId)
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

        public DetailPathNode(IntVector2 pos, int dir8, WorldData world, DetailPathNode parent, IntVector2 goalPos, bool endAsShip, int runId)
        {
            Position = pos;
            this.dir8 = dir8;
            PreviousPosition = parent.Position;
            RunId = runId;
            closed = false;

            if (lib.IsEven(dir8))
            {
                moveCost = MoveCostStraight;
            }
            else
            { 
                moveCost = MoveCostDiagonal;
            }
            
            if (dir8 == parent.dir8)
            { //Bonus for keeping direction
                moveCost -= 1f;
            }

            SubTile subtile = world.subTileGrid.Get(pos);
            moveCost *= subtile.TerrainBlockMultipleValue();

            Tile tile = world.tileGrid.Get(pos / WorldData.TileSubDivitions);
            waterTile = tile.IsWater();

            if (waterTile != parent.waterTile)
            {
                if (waterTile == endAsShip)
                {//wanted convert
                    moveCost -= 2;
                }
                else
                {
                    moveCost += MoveCostStraight * 64;
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
