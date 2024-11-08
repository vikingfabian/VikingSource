
#define VISUAL_NODES
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
        private ConcurrentStack<DetailPathFinding> pool = new ConcurrentStack<DetailPathFinding>();

        public DetailPathFinding Get()
        {
            if (pool.TryPop(out DetailPathFinding path))
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

            path.recycle();
            pool.Push(path);
        }
    }

    class DetailPathFinding
    {
        public const int MaxNodeLength = 30000;
        const int MaxTileRadius = 64 * WorldData.TileSubDivitions;

        List<DetailPathNode> open = new List<DetailPathNode>();
        Rectangle2 area;
        //IntVector2 gridOffset;
        DetailPathNode[,] nodeGrid;
        public DetailPathFinding()
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(IntVector2.Zero, MaxTileRadius);
            nodeGrid = new DetailPathNode[area.Width, area.Height];
        }

        public DetailWalkingPath FindPath(IntVector2 center, Rotation1D startDir, IntVector2 goal, bool startAsShip, bool endAsShip)
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

            
            area = Rectangle2.FromCenterTileAndRadius(center, MaxTileRadius);
            area.SetBounds(DssRef.world.subTileGrid.Area);
            //gridOffset = area.pos

            DetailPathNode startNode = new DetailPathNode(center, conv.ToDir8_INT(startDir), startAsShip);

            nodeGrid[center.X - area.pos.X, center.Y - area.pos.Y] = startNode;

            //bool endAsShip = DssRef.world.subTileGrid.Get(goal).IsWater();
            DetailPathNode currentNode = startNode;

            int numLoops = 0;


            while (true)
            {
                for (int dir = 0; dir < 8; dir++)
                {
                    IntVector2 pos = IntVector2.Dir8Array[dir] + currentNode.Position;
                    if (area.IntersectTilePoint(pos) && !nodeGrid[pos.X - area.pos.X, pos.Y - area.pos.Y].HasValue)
                    {
                        //add a node to open list
                        DetailPathNode node = new DetailPathNode(pos, dir, DssRef.world, currentNode, goal, endAsShip);
                        open.Add(node);
                        nodeGrid[pos.X - area.pos.X, pos.Y - area.pos.Y] = node;
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
                if (numLoops > 20000)
                {
                    break;
                }
            }

            List<DetailPathNodeResult> result = new List<DetailPathNodeResult>();
            bool blocked = false;

            while (currentNode.Position != startNode.Position)
            {
                if (currentNode.ship == startAsShip || currentNode.ship == endAsShip)
                {
                    result.Add(new DetailPathNodeResult(currentNode.Position, currentNode.ship));
                    
                    numLoops++;
                    if (numLoops > MaxNodeLength)
                        throw new EndlessLoopException("");
                }
                else
                {
                    result.Clear();
                    blocked = true;
                }

                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodeGrid[pos.X - area.pos.X, pos.Y - area.pos.Y];
            }


            return new DetailWalkingPath(goal, result, blocked);
        }

        public void recycle()
        {
            open.Clear();

            for (int y = 0; y < area.size.Y; ++y)
            {
                for (int x = 0; x < area.size.X; ++x)
                {
                    nodeGrid[x, y] = DetailPathNode.Empty;
                }
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
        public List<DetailPathNodeResult> nodes;
        public bool blockedPath;

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
                return  WP.SubtileToWorldPosXZ(goal);
            }

            ship = nodes[currentNodeIx].ship;
            IntVector2 to = nodes[currentNodeIx].position;
            Vector3 toWp = WP.SubtileToWorldPosXZ(to);
            Vector2 diff = new Vector2( toWp.X - myPos.X, toWp.Z - myPos.Z);
            if (diff.Length() <= NodeMinDistance)
            {
                --currentNodeIx;
            }
            
            
            return toWp;
        }

        public DetailWalkingPath(IntVector2 goal, List<DetailPathNodeResult> nodes, bool blockedPath)
        {
            this.goal = goal;
            this.nodes = nodes;
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

        public float Value;
        float moveCost;

        public IntVector2 Position;
        public IntVector2 PreviousPosition;

        public bool HasValue;
        public bool closed;
        public bool waterTile;
        public bool ship;

        int dir8;

        public DetailPathNode(IntVector2 pos, int dir8, bool ship)
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

        public DetailPathNode(IntVector2 pos, int dir8, WorldData world, DetailPathNode parent, IntVector2 goalPos, bool endAsShip)
        {
            this.Position = pos;
            this.dir8 = dir8;
            this.PreviousPosition = parent.Position;
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

            Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;

            HasValue = true;
        }
    }
}
