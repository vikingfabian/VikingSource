//#define VISUAL_NODES
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Valve.Steamworks;
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
        //Stack<WalkingPath> poolResOut = new Stack<WalkingPath>();

        
        public PathFinding GetPf()
        {
            if (poolPf.TryPop(out PathFinding path))
            {
                return path;
            }
            else
            {
                return new PathFinding();
            }
        }

        public WalkingPath GetRes()
        {
            if (poolRes.TryDequeue(out WalkingPath path))
            {
                if (path.timeStamp + 2 >= Ref.TotalFrameCount)
                {
                    poolRes.Enqueue(new WalkingPath());
                    poolRes.Enqueue(new WalkingPath());
                    System.Threading.Thread.Sleep(32);
                }
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
                //path.recycle();
                pathresult.timeStamp = Ref.TotalFrameCount;
                poolRes.Enqueue(pathresult);
            }
        }
    }



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

            //List<PathNodeResult> result = new List<PathNodeResult>();
            var path = DssRef.state.pathFindingPool.GetRes();

            while (currentNode.Position != startNode.Position)
            {
                path.nodes.Add(new PathNodeResult(currentNode.Position, currentNode.ship));
                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid[pos.X, pos.Y];

                numLoops++;
                if (numLoops > MaxNodeLength)
                    throw new EndlessLoopException("");

            }
                        
            path.init();
            return path;
        }

        public void recycle()
        {
            open.Clear();

            for (int y = 0; y < DssRef.world.Size.Y; ++y)
            {
                for (int x = 0; x < DssRef.world.Size.X; ++x)
                {
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

        public int timeStamp;
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

        public PathNode(IntVector2 pos, int dir8, WorldData world, PathNode parent, IntVector2 goalPos, bool endAsShip)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.PreviousPosition = parent.Position;
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

            Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;

            HasValue = true;
        }
    }

}

