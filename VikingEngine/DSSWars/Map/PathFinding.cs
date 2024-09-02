using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.Map
{
    //TODO heatmap för vatten

    class PathFindingPool
    {
        //Represents a thread-safe last in-first out (LIFO) collection.
        private ConcurrentStack<PathFinding> pool = new ConcurrentStack<PathFinding>();
        //public PathNodePool nodePool = new PathNodePool();

        public PathFinding Get()
        {
            if (pool.TryPop(out PathFinding path))
            {
                return path;
            }
            else
            {
                return new PathFinding();
            }
        }

        public void Return(PathFinding path)
        {
            // Reset the node to a default state

            path.recycle();
            pool.Push(path);
        }
    }

    //class PathNodePool
    //{
    //    private ConcurrentStack<PathNode> pool = new ConcurrentStack<PathNode>();

    //    public PathNode Get()
    //    {
    //        if (pool.TryPop(out PathNode node))
    //        {
    //            return node;
    //        }
    //        else
    //        {
    //            return new PathNode();
    //        }
    //    }

    //    public void Return(PathNode node)
    //    {
    //        // Reset the node to a default state
    //        //if (node != null)
    //        //{
    //        //node.recycle();
    //        if (pool.Count < PathFinding.MaxNodeLength)
    //        {
    //            pool.Push(node);
    //        }
    //        //}           
    //    }
    //}


    class PathFinding
    {
        public const int MaxNodeLength = 30000;

        List<PathNode> open = new List<PathNode>();

        PathNode[,] nodeGrid;
        public PathFinding()
        {
            nodeGrid = new PathNode[DssRef.world.Size.X, DssRef.world.Size.Y];
        }

        public WalkingPath FindPath(IntVector2 center, Rotation1D startDir, IntVector2 goal, bool startAsShip)
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

            PathNode startNode = new PathNode(center, conv.ToDir8_INT(startDir), startAsShip);
            //PathNode startNode = DssRef.state.pathFindingPool.nodePool.Get().init(center, conv.ToDir8_INT(startDir));

            nodeGrid[center.X, center.Y] = startNode;

            bool endAsShip = DssRef.world.tileGrid.Get(goal).IsWater();
            PathNode currentNode = startNode;

            int numLoops = 0;


            while (true)
            {
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    if (DssRef.world.tileBounds.IntersectTilePoint(pos) && !nodeGrid[pos.X, pos.Y].HasValue)
                    {
                        //add a node to open list
                        PathNode node = new PathNode(pos, dir, DssRef.world, currentNode, goal, endAsShip);
                        open.Add(node);
                        nodeGrid[pos.X, pos.Y] = node;
                    }
                }

                var lowValue = float.MaxValue;
                int lowIndex = -1;
                for (int i = 0; i < open.Count; i++)
                {
                    if (open[i].Value < lowValue)
                    {
                        lowValue = open[i].Value;
                        lowIndex = i;
                    }
                }

                if (open.Count > 1)
                {
                    currentNode = open[lowIndex];
                    open.RemoveAt(lowIndex);
                }

                currentNode.closed = true;
                nodeGrid[currentNode.Position.X, currentNode.Position.Y] = currentNode;

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

            List<PathNodeResult> result = new List<PathNodeResult>();


            while (currentNode.Position != startNode.Position)
            {
                result.Add(new PathNodeResult(currentNode.Position, currentNode.ship));
                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid[pos.X, pos.Y];

                numLoops++;
                if (numLoops > MaxNodeLength)
                    throw new EndlessLoopException("");

            }


            return new WalkingPath(result);
        }

        public void recycle()
        {
            open.Clear();

            for (int y = 0; y < DssRef.world.Size.Y; ++y)
            {
                for (int x = 0; x < DssRef.world.Size.X; ++x)
                {
                    //DssRef.state.pathFindingPool.nodePool.Return(nodeGrid[x, y]);
                    nodeGrid[x, y] = PathNode.Empty;
                }
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
    }

    class WalkingPath
    {
#if VISUAL_NODES
        List<Graphics.Mesh> nodeImages;
#endif
        const int IgnoreDirChangeTimes = 10;
        const float NodeMinDistance = 0.3f;

        public int currentNodeIx;
        public List<PathNodeResult> nodes;

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

        public WalkingPath(List<PathNodeResult> nodes)
        {
            this.nodes = nodes;
            currentNodeIx = nodes.Count - 1;

#if VISUAL_NODES
            nodeImages = new List<Graphics.Mesh>();
            foreach (Vector2 n in nodes)
            {
                Vector3 pos = WorldPosition.V2toV3(n);
                WorldPosition wp = new WorldPosition(pos);
                wp.SetFromGroundY(2);
                nodeImages.Add(new Graphics.Mesh(LoadedMesh.cube_repeating, wp.ToWorldPos(), 
                    new Graphics.TextureEffect( Graphics.TextureEffectType.Flat, SpriteName.ControllerB), 0.4f));
            }

#endif
        }

        //public PathNodeResult CurrentNode()
        //{
        //    if (currentNodeIx < 0)
        //    {
        //        return new PathNodeResult(IntVector2.MinValue, false);
        //    }
        //    return nodes[currentNodeIx];
        //}

        public bool TryGetCurrentNode(out PathNodeResult node)
        {
            int ix = currentNodeIx;

            if (ix >= 0 && nodes.Count > 0)
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

    //class PathNodePool
    //{
    //    private Stack<PathNode> pool = new Stack<PathNode>(PathFinding.MaxNodeLength);

    //    public PathNode Get()
    //    {            
    //        if (pool.Count > 0)
    //        {
    //            return pool.Pop();
    //        }
    //        else
    //        {
    //            return new PathNode();
    //        }            
    //    }

    //    public void Return(PathNode node)
    //    {           
    //        // Reset the node to a default state
    //        node.recycle();
    //        if (pool.Count < PathFinding.MaxNodeLength)
    //        {
    //            pool.Push(node);
    //        }           
    //    }
    //}

    struct PathNode //: IComparable<PathNode>
    {
        const float MoveCostStraight = 10f;
        const float MoveCostDiagonal = 14f;

        public static readonly PathNode Empty = new PathNode();

        public float Value;
        float moveCost;

        public IntVector2 Position;
        public IntVector2 PreviousPosition;

        public bool HasValue;
        public bool closed;
        public bool waterTile;
        public bool ship;

        int dir8;

        public PathNode(IntVector2 pos, int dir8, bool ship)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.ship = ship;
            HasValue = true;
            closed = true;

            moveCost = 0;
            Value = 0;
            PreviousPosition = pos;
            waterTile = ship;
        }
        //public PathNode init(IntVector2 pos, int dir8)
        //{
        //    this.Position = pos;
        //    this.dir8 = dir8;

        //    moveCost = 0;
        //    Value = 0;

        //    HasValue = true;
        //    return this;
        //}
        //public PathNode(IntVector2 pos, int dir8)
        //{
        //    this.Position = pos;
        //    this.dir8 = dir8;

        //    moveCost = 0;
        //    Value = 0;
        //}

        public PathNode(IntVector2 pos, int dir8, WorldData world, PathNode parent, IntVector2 goalPos, bool endAsShip)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.PreviousPosition = parent.Position;
            closed = false;

            //Dir8 starts with North = 0 (even)
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

            //if (parent.ship)
            //{
            //    if (!parent.waterTile && !this.waterTile)
            //    {
            //        //Detta är för att armen kan vada igenom en tile med vatten
            //        parent.ship = false;
            //    }
            //}
            //else
            //{
            //    if (parent.waterTile && this.waterTile)
            //    {
            //        parent.ship = true;
            //    }
            //}
            //ship = parent.ship;
            ship = this.waterTile;

            moveCost *= tile.TroupWalkingDistance(ship);

            moveCost += parent.moveCost;

            Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;

            HasValue = true;
        }
    }

}

