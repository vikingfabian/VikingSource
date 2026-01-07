using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Path
{
    class RoadPathFinding
    {
        public const int MaxNodeLength = 300000;

        const int TileRadius = 8 * WorldData.TileSubDivitions;

        List<DetailPathNode> open = new List<DetailPathNode>();
        Rectangle2 area;
        //IntVector2 gridOffset;
        IntVector2 nodeUseTopLeft, nodeUseBottomRight;
        DetailPathNode[,] nodeGrid;
        public RoadPathFinding()
        {
            
        }

        public RoadWalkingPath FindPath(IntVector2 center, IntVector2 goal)
        {
            Rectangle2 area = Rectangle2.FromTwoTilePoints(center, goal);
            area.AddRadius(TileRadius);

            nodeGrid = new DetailPathNode[area.Width, area.Height];
            var startDir = conv.ToDir8(goal - center);

            area = Rectangle2.FromCenterTileAndRadius(center, TileRadius);
            area.SetBounds(DssRef.world.subTileGrid.Area);
            //gridOffset = area.pos
            DetailPathNode startNode = new DetailPathNode(center, (int)startDir, false);
            {
                IntVector2 gridPos = center - area.pos;
                nodeGrid[gridPos.X, gridPos.Y] = startNode;
                nodeUseTopLeft = gridPos;
                nodeUseBottomRight = gridPos;
            }
            //bool endAsShip = DssRef.world.subTileGrid.Get(goal).IsWater();
            DetailPathNode currentNode = startNode;

            int numLoops = 0;


            while (true)
            {
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    IntVector2 gridPos = pos - area.pos;
                    if (area.IntersectTilePoint(pos) && !nodeGrid[gridPos.X, gridPos.Y].HasValue)
                    {
                        //add a node to open list
                        DetailPathNode node = new DetailPathNode(pos, dir, DssRef.world, currentNode, goal, false);
                        open.Add(node);

                        nodeGrid[gridPos.X, gridPos.Y] = node;
                        if (gridPos.X < nodeUseTopLeft.X)
                        {
                            nodeUseTopLeft.X = gridPos.X;
                        }
                        else if (gridPos.X > nodeUseBottomRight.X)
                        {
                            nodeUseBottomRight.X = gridPos.X;
                        }

                        if (gridPos.Y < nodeUseTopLeft.Y)
                        {
                            nodeUseTopLeft.Y = gridPos.Y;
                        }
                        else if (gridPos.Y > nodeUseBottomRight.Y)
                        {
                            nodeUseBottomRight.Y = gridPos.Y;
                        }
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
                nodeGrid[currentNode.Position.X - area.pos.X, currentNode.Position.Y - area.pos.Y] = currentNode;

                if (currentNode.Position == goal)
                {
                    break;
                }

                numLoops++;
                if (numLoops > 2000000)
                {
                    break;
                }
            }

            //List<DetailPathNodeResult> result = new List<DetailPathNodeResult>();

            //const int MaxBacknodes = 1;
            var path = new RoadWalkingPath();//DssRef.state.pathUpdates[pathThreadIndex].detailPathFindingPool.GetRes();
            bool blocked = false;
            int totalNodes = 0;

            while (currentNode.Position != startNode.Position)
            {                
                path.nodes.Add(new DetailPathNodeResult(currentNode.Position, currentNode.ship));

                totalNodes++;
                if (totalNodes > MaxNodeLength)
                    throw new EndlessLoopException("");
                
                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid[pos.X - area.pos.X, pos.Y - area.pos.Y];
            }

            path.init(goal, blocked);
            return path;
        }

    }


    class RoadWalkingPath
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
        public int timeStamp;
        public void recycle()
        {
            nodes.Clear();
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
            complete = currentNodeIx < 0;
            if (complete)
            {
                ship = false;
                return WP.SubtileToWorldPosXZ(goal);
            }

            ship = nodes[currentNodeIx].ship;
            IntVector2 to = nodes[currentNodeIx].position;
            Vector3 toWp = WP.SubtileToWorldPosXZ(to);
            Vector2 diff = new Vector2(toWp.X - myPos.X, toWp.Z - myPos.Z);
            if (diff.Length() <= NodeMinDistance)
            {
                --currentNodeIx;
            }


            return toWp;
        }

        public RoadWalkingPath()
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

    //struct PathNode
    //{
    //    const float MoveCostStraight = 10f;
    //    const float MoveCostDiagonal = 14f;

    //    public static readonly PathNode Empty = new PathNode();

    //    public float Value;
    //    float moveCost;

    //    public IntVector2 Position;
    //    public IntVector2 PreviousPosition;

    //    public bool HasValue;
    //    public bool closed;
    //    public bool waterTile;
    //    public bool ship;

    //    int dir8;

    //    public PathNode(IntVector2 pos, int dir8, bool ship)
    //    {
    //        this.Position = pos;
    //        this.dir8 = dir8;
    //        this.ship = ship;
    //        HasValue = true;
    //        closed = true;

    //        moveCost = 0;
    //        Value = 0;
    //        PreviousPosition = pos;
    //        waterTile = ship;
    //    }

    //    public PathNode(IntVector2 pos, int dir8, WorldData world, PathNode parent, IntVector2 goalPos, bool endAsShip)
    //    {
    //        this.Position = pos;
    //        this.dir8 = dir8;
    //        this.PreviousPosition = parent.Position;
    //        closed = false;

    //        moveCost = lib.IsEven(dir8) ? MoveCostStraight : MoveCostDiagonal;
    //        if (dir8 == parent.dir8)
    //        { //Bonus for keeping direction
    //            moveCost -= 1f;
    //        }

    //        Tile tile = world.tileGrid.Get(pos);
    //        waterTile = tile.IsWater();

    //        if (waterTile != parent.waterTile)
    //        {
    //            if (waterTile == endAsShip)
    //            {//wanted convert
    //                moveCost -= 2;
    //            }
    //            else
    //            {
    //                moveCost += MoveCostStraight * 16;
    //            }
    //        }
    //        ship = this.waterTile;

    //        moveCost *= tile.TroupWalkingDistance(ship);

    //        moveCost += parent.moveCost;

    //        Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;

    //        HasValue = true;
    //    }
    //}
}
