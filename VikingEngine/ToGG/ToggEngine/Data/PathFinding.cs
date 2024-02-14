using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class PathFinding
    {
        //TODO: polera pathfinding genom att veta vilket det sista steget är - för passthrough tiles

        static readonly IntVector2[] EightDirs_OrtoFirst = new IntVector2[] {
            new IntVector2(0, -1),
            new IntVector2(1, 0),
            new IntVector2(0, 1),
            new IntVector2(-1, 0),
            
            new IntVector2(1, -1),
            new IntVector2(1, 1),
            new IntVector2(-1, 1),
            new IntVector2(-1, -1),
        };
        PathNode[,] nodes;
       

        public PathFinding()
        {
            nodes = new PathNode[toggRef.worldsize.X, toggRef.worldsize.Y];
        }

        public WalkingPath FindPath(AbsUnit unit, IntVector2 center, IntVector2 goal, bool adjacentToGoal)
        {
            if (alreadyAtGoal(center, goal, adjacentToGoal))
            {
                var noPath = new WalkingPath(new List<IntVector2>());
                noPath.success = true;
                return noPath;
            }
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
            List<PathNode> open = new List<PathNode>();
            PathNode startNode = new PathNode(unit, center);
            startNode.Closed = true;
            nodes[center.X, center.Y] = startNode;

            PathNode currentNode = startNode;
            int numLoops = 0;
            bool foundGoal = false;

            while (true)
            {
                foreach (var dir in EightDirs_OrtoFirst)
                {   
                    IntVector2 pos = dir + currentNode.Position;
                    if (availableSquare(pos, unit) && nodes[pos.X, pos.Y] == null)
                    {
                        //add a node to open list
                        PathNode node = new PathNode(unit, pos, currentNode, goal);
                        if (!node.Closed)
                            open.Add(node);
                        nodes[pos.X, pos.Y] = node;
                    }
                }

                FindMinValue lowest = new FindMinValue(false);
                for (int i = 0; i < open.Count; i++)
                {
                    lowest.Next(open[i].Value, i);
                }

                if (open.Count > 0)
                {
                    currentNode = open[lowest.minMemberIndex];
                    open.RemoveAt(lowest.minMemberIndex);
                }
                currentNode.Closed = true;

                //bool foundGoal;
                if (adjacentToGoal)
                {
                    foundGoal = currentNode.Position.SideLength(goal) == 1;
                }
                else
                {
                    foundGoal = currentNode.Position == goal;
                }

                if (foundGoal && toggRef.board.MovementRestriction(currentNode.Position, unit) == MovementRestrictionType.WalkThroughCantStop)
                {
                    foundGoal = false;
                }

                if (foundGoal)
                {
                    break;
                }

                //#if PCGAME
                numLoops++;
                if (numLoops > 1000)
                {
                    lib.DoNothing();
                    break;
                }
            }

            List<IntVector2> squaresEndToStart = new List<IntVector2>(16);

            float expectedDamage = startNode.expectedDamage;

            while (currentNode != startNode)
            {                
                squaresEndToStart.Add(currentNode.Position);
                expectedDamage += currentNode.expectedDamage;
                
                IntVector2 pos = currentNode.PreviousPosition;
                currentNode = nodes[pos.X, pos.Y];

                numLoops++;

                if (numLoops > 10000)
                    throw new EndlessLoopException("path finding");
            }

            var result = new WalkingPath(squaresEndToStart);
            result.success = foundGoal;
            result.expectedDamage = expectedDamage;
            //if (result.squaresEndToStart.Count > 0)
            //{
            //    result.success = result.squaresEndToStart[0] == goal;
            //}
            return result;
        }

        bool alreadyAtGoal(IntVector2 center, IntVector2 goal, bool adjacentToGoal)
        {
            int l = (goal - center).SideLength();

            if (adjacentToGoal)
            {
                return l == 1;
            }
            else
            {
                return l == 0;
            }
        }

        bool availableSquare(IntVector2 pos, AbsUnit unit)
        {
            if (toggRef.board.MovementRestriction(pos, unit) != MovementRestrictionType.Impassable)
            {
                return true;
            }

            return false;
        }
    }
    
    

    class PathNode
    {
        const float OneSquareBaseCost = 10f;
        const float MoveCostImpassable = 100000f;
        const float MoveCostAdd_MustStop = 4;
        const float MoveCostAdd_SittingDuck = 8;
        
        public float Value;
        public float moveCost = 0;
        public bool Closed;
        public IntVector2 Position;
        public IntVector2 PreviousPosition;
        public float expectedDamage = 0;
        public MovementRestrictionType moveRestriction;
        

        public PathNode(AbsUnit unit, IntVector2 pos)
        {
            this.Position = pos;
            //checkAdjacentEnemies(unit, pos);
        }
        public PathNode(AbsUnit unit, IntVector2 pos, PathNode parent, IntVector2 goalPos)
        {
            this.Position = pos;
            this.PreviousPosition = parent.Position;

            //checkAdjacentEnemies(unit, pos);

            moveCost = OneSquareBaseCost;
            var tile = toggRef.board.tileGrid.Get(pos);

            bool toGoalPos = pos == goalPos;

            moveRestriction = toggRef.board.MovementRestriction(pos, unit);
            
            if (moveRestriction == MovementRestrictionType.MustStop)
            {
                if (!toGoalPos)
                { 
                    moveCost = MoveCostImpassable;
                }
            }

            
            expectedDamage = unit.expectedMoveDamage(PreviousPosition, Position);

            moveCost = OneSquareBaseCost * expectedDamage;
            
            moveCost += parent.moveCost;

            Value = moveCost + (Math.Abs(pos.X - goalPos.X) + Math.Abs(pos.Y - goalPos.Y)) * OneSquareBaseCost;
        }

    }
}
