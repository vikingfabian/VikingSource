//#define VISUAL_NODES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.GameObject
{
    static class PathFinder
    {
        public static SoldierWalkingPath PathFinding(IntVector2 from, IntVector2 goal, GameObject.SoldierGroup group)
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

            bool[,] checkedPathsFrom = new bool[DssRef.world.Size.X, DssRef.world.Size.Y];//warsRef.board.areaGrid.Width, warsRef.board.areaGrid.Height];


            List <PathNode> open = new List<PathNode>();
            PathNode startNode = new PathNode(from, Dir8.NO_DIR);
            startNode.Closed = true;
           
            PathNode currentNode = startNode;
            int numLoops = 0;

            while (true)
            {
                Dir8 backDir = Dir8.NO_DIR;
                if (currentNode.parentDir != Dir8.NO_DIR)
                {
                    backDir = lib.Invert(currentNode.parentDir);
                }
                checkedPathsFrom[currentNode.Position.X, currentNode.Position.Y] = true;
                for (Dir8 dir = (Dir8)0; dir < Dir8.NUM; dir++)
                {
                    if (dir != backDir)
                    {
                        //if (dir == Dir8.E)
                        //{
                        //    lib.DoNothing();
                        //}
                        IntVector2 moveToPos = IntVector2.FromDir8(dir) + currentNode.Position;
                        if (DssRef.world.tileGrid.InBounds(moveToPos) &&
                            checkedPathsFrom[moveToPos.X, moveToPos.Y] == false)
                        {
                           if (group.canMoveTo(currentNode.Position, moveToPos))
                            {
                                PathNode node = new PathNode(currentNode, moveToPos, dir, currentNode, goal, group);
                                open.Add(node);
                            }
                        }
                    }
                }

                FindMinValue lowest = new FindMinValue(false);
                for (int i = 0; i < open.Count; i++)
                {
                    lowest.Next(open[i].Value, i);
                }


                if (open.Count > 1)
                {
                    currentNode = open[lowest.minMemberIndex];
                    open.RemoveAt(lowest.minMemberIndex);
                }
                currentNode.Closed = true;

                if (currentNode.Position == goal)
                {
                    break;
                }
                if (currentNode.steps >= 64)
                {
                    Debug.LogWarning("Too long path");
                    break;
                }

                numLoops++;
                if (numLoops > 10000)
                {
                    break;
                }
            }

            List<IntVector2> result = new List<IntVector2>();


            while (currentNode != null)
            {
                result.Add(currentNode.Position);
                currentNode = currentNode.parentNode;
            }

            return new SoldierWalkingPath(result);
        }
    }

    struct WalkingPathInstance
    {
        SoldierWalkingPath path;
        int currentNode;
        public Vector3 currentGoalPos;

        public WalkingPathInstance(SoldierWalkingPath path, IntVector2 inArea)
        {
            this.path = path;
            currentNode = path.nodes.Count - 1;
            currentGoalPos = Vector3.Zero;

            refreshGoal();
            asynchUpdate(inArea);
        }

        public void asynchUpdate(IntVector2 inArea)
        {
            if (currentNode > 0)
            {
                if (inArea == path.nodes[currentNode])
                {
                    currentNode--;
                    refreshGoal();
                }
            }
        }

        void refreshGoal()
        {
            currentGoalPos = WP.ToWorldPos(path.nodes[currentNode]);//warsRef.board.areaToWorldPos(path.nodes[currentNode]);
        }
    }

    class SoldierWalkingPath
    {
#if VISUAL_NODES
        List<Graphics.Mesh> nodeImages;
#endif
        const int IgnoreDirChangeTimes = 10;
        //int numIngoreDirChange = 0;

       // bool firstTimeUse = true;

        int currentNodeIx;
        public List<IntVector2> nodes;
        public SoldierWalkingPath(List<IntVector2> nodes)
        {
            this.nodes = nodes;
            currentNodeIx = nodes.Count - 1;

#if VISUAL_NODES

            new Timer.Action0ArgTrigger(createVisualNodes);
#endif
        }

        public IntVector2 currentNodePos()
        {
            return nodes[currentNodeIx];
        }

        void createVisualNodes()
        {
#if VISUAL_NODES
            nodeImages = new List<Graphics.Mesh>();
            foreach (IntVector2 n in nodes)
            {
                Vector3 pos = stupRef.board.areaPos(n);
                pos.Y += 1f;
                nodeImages.Add(new Graphics.Mesh(LoadedMesh.cube_repeating, pos,
                    new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.White), 0.4f));
            }

            new Timer.TimedAction0ArgTrigger(DeleteMe, 2000);
#endif
        }

//        public Vector2 WalkTowardsNode(Vector3 currentPos, Rotation1D currentDir, float turnSpeedAndTime, float walkingSpeed)
//        {
//            if (currentNode < 0 || nodes.Count == 0)
//                return Vector2.Zero;

//            Vector2 planePos = lib.V3toV2(currentPos);
//            if (firstTimeUse)
//            {
//                firstTimeUse = false;
//                float closest = (nodes[0] - planePos).Length();
//                for (int i = nodes.Count - 2; i >= 0; i--)
//                {
//                    float l = (nodes[i] - planePos).Length();
//                    if (l < closest)
//                    {
//                        closest = l;
//                        currentNode = i;
//                    }
//                    else
//                    {
//                        break;
//                    }
//                }
//            }



//            Vector2 diff = nodes[currentNode] - planePos;
//            float length = diff.Length();
//            //check next node to make sure it isnt closer


//            const float MinLenght = 1f;
//            if (length < MinLenght)
//            {
//                currentNode--;
//            }

//            diff.Normalize();
//            Rotation1D dir = Rotation1D.FromDirection(diff);
//            float dirDiff = dir.AngleDifference(currentDir);
//            if (dirDiff > turnSpeedAndTime)
//            {
//                numIngoreDirChange++;
//                if (numIngoreDirChange >= IgnoreDirChangeTimes)
//                {
//                    currentDir.Add(dirDiff);

//                }
//                return currentDir.Direction(walkingSpeed);
//            }
//            else
//            {
//                numIngoreDirChange = 0;
//            }

//            return diff * walkingSpeed;
//        }

        public void DeleteMe()
        {
#if VISUAL_NODES
            foreach (Graphics.Mesh m in nodeImages)
            {
                m.DeleteMe();
            }
#endif
        }
    }

    class PathNode
    {
        public Dir8 parentDir; //the direction the parent node walked in
        const int MoveCostStraight = 10;
        const int MoveCostDiagonal = 14;
        public int Value;
        public int moveCost;
        public bool Closed;
        public int steps = 0;
        public IntVector2 Position;
        public PathNode parentNode;

        public PathNode(IntVector2 pos, Dir8 parentDir)
        {
            this.Position = pos;
            this.parentDir = parentDir;
        }
        public PathNode(PathNode parentNode, IntVector2 pos, Dir8 parentDir, PathNode parent, IntVector2 goalPos, SoldierGroup group)
        {
            this.Position = pos;
            this.parentDir = parentDir;
            this.parentNode = parentNode;
            this.steps = parentNode.steps +1;
            
            if (parent.parentDir == Dir8.NE || parent.parentDir == Dir8.NW || 
                parent.parentDir == Dir8.SE || parent.parentDir == Dir8.SW)
            { 
                moveCost = MoveCostDiagonal; 
            }
            else
            {
                moveCost = MoveCostStraight;
            }

            moveCost += parent.moveCost;
            if (parentDir == parent.parentDir)
                moveCost -= 2;

            Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * MoveCostStraight;
        }

        public override string ToString()
        {
            return "Node " + Position.ToString() + ", Dir:" + parentDir.ToString() + ", value:" + Value.ToString();
        }

        
    }
}
