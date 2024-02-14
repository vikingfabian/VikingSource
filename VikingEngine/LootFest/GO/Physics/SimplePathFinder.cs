using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.LootFest.GO
{
    /// <summary>
    /// A simple pathfinding tool to guide characters, using random alt directions
    /// </summary>
    class SimplePathFinder
    {
        const float PathForwardCheckLength = 10f;
        const float CollCheckForwardCheckLength = 2f;


        GO.Characters.AbsCharacter parent;
        public Vector3 goalPos;
        public Vector2 pathDir;
        public float pathAngle;
        public float lengthToGoal;

       public  int maxClimbHeight = 1;
       public int maxFallHeight = 4;

        public int maxJumpUp = 4;
        public int maxJumpDown = 8;


        IntervalF backtrackTimeRange = new IntervalF(600, 1000);
        Time backtrackingTimeLeft = 0;

        Rotation1D backDir = Rotation1D.Random();

        bool isEvading = false;
        bool evadeRight;
        public bool jump = false;

        public SimplePathFinder(GO.Characters.AbsCharacter parent)
        {
            this.parent = parent;
        }

        public bool PathCollisionCheck(Vector3 position, Velocity direction)
        {
            Vector3 dir = direction.Value;
            dir.Y = 0;
            if (dir == Vector3.Zero)
            {
                return false;
            }

            dir.Normalize();
            bool collision = !availablePathIndir(dir, position, CollCheckForwardCheckLength, false);
            return collision;
        }

        public void hitObsticle()
        {
            //Try backtrack a small bit to get unstuck
            if (backtrackingTimeLeft.TimeOut)
            {
                backtrackingTimeLeft.MilliSeconds = backtrackTimeRange.GetRandom();

                backDir = Rotation1D.FromDirection(pathDir);
                backDir.Add(MathHelper.Pi + Ref.rnd.Plus_MinusF(1.8f));//

                pathDir = backDir.Direction(1f);
                pathAngle = backDir.Radians;
            }
            else
            {
                backDir.Add(Ref.rnd.Plus_MinusF(0.4f));
            }
        }

        public void AsynchUpdate(float time)
        {
            if (backtrackingTimeLeft.CountDown(time))
            {

                Vector3 dir = goalPos - parent.Position;
                dir.Y = 0;
                lengthToGoal = dir.Length();

                if (lengthToGoal < 0.1f)
                {
                    pathDir = Vector2.Zero;
                }
                else
                {
                    Vector3 center = parent.Position;
                    dir.Normalize();

                    if (availablePathIndir(dir, center, PathForwardCheckLength, true))
                    {
                        isEvading = false;
                        pathDir = VectorExt.V3XZtoV2(dir);
                    }
                    else
                    { //Try find alternative path by turning left or right
                        if (!isEvading)
                        { //keep evading in the same dir if started that way
                            evadeRight = Ref.rnd.Bool();
                        }

                        float currentAngle = lib.V2ToAngle(VectorExt.V3XZtoV2(dir));

                        if (findPathOnSide(evadeRight, currentAngle, center, out dir))
                        {
                            pathDir = VectorExt.V3XZtoV2(dir);
                            isEvading = true;

                            return;
                        }

                        evadeRight = !evadeRight; //flip to try the other side
                        if (findPathOnSide(evadeRight, currentAngle, center, out dir))
                        {
                            pathDir = VectorExt.V3XZtoV2(dir);
                            isEvading = true;

                            return;
                        }

                        //Could not find path
                        pathDir = VectorExt.V3XZtoV2(dir);
                        hitObsticle();
                    }


                }
            }
            else
            { //is backtracking
                pathDir = backDir.Direction(1f);
            }

            pathAngle = lib.V2ToAngle(pathDir);
        }


        bool findPathOnSide(bool rightSide, float forwardAngle, Vector3 centerPos, out Vector3 dir)
        {
            const int AngleStepsCount = 2;
            const float AngleStep = MathHelper.PiOver2 / AngleStepsCount;

            dir = Vector3.Zero;

            for (int i = 0; i < AngleStepsCount; ++i)
            {
                forwardAngle += AngleStep * lib.BoolToLeftRight(rightSide);

                dir = VectorExt.V2toV3XZ(lib.AngleToV2(forwardAngle, 0.5f));

                if (availablePathIndir(dir, centerPos, PathForwardCheckLength, true))
                {
                    return true;
                }
            }

            return false;
        }


        bool availablePathIndir(Vector3 dir, Vector3 start, float maxForwardCheckLength, bool allowJump)
        {
            const float SteppLength = 1f;
            float length = 0;

            //The right-hand normal of vector (x, y) is (y, -x), and the left-hand normal is (-y, x).
            Vector3 leftDir = dir; leftDir.X = -dir.Z; leftDir.Z = dir.X;
            Vector3 rightDir = dir; leftDir.X = dir.Z; leftDir.Z = dir.X;


            int ForwardChecks = Convert.ToInt32( lib.SmallestValue(lengthToGoal, maxForwardCheckLength) / SteppLength);

            Map.WorldPosition prevPos = new Map.WorldPosition(start);
            Map.WorldPosition nextPos = prevPos;
            Map.WorldPosition leftSide = Map.WorldPosition.EmptyPos, rightSide = Map.WorldPosition.EmptyPos;

            int prevY = (int)LfRef.chunks.GetScreenSafe(prevPos.ChunkGrindex).GetClosestFreeY(prevPos);//LfRef.chunks.GetHighestYpos(prevPos);

            for (int i = 0; i < ForwardChecks; ++i)
            {
                start.X += dir.X * SteppLength;
                start.Z += dir.Z * SteppLength;
                length += SteppLength;

                nextPos.PositionV3 = start;

                leftSide.PositionV3 = start + leftDir;
                rightSide.PositionV3 = start + rightDir;


                if (checkIfPositionIsImpassable(nextPos, prevPos, length, ref prevY, allowJump))
                {
                    return false;
                }
                int leftPosY = prevY;
                if (checkIfPositionIsImpassable(leftSide, nextPos, length, ref leftPosY, allowJump))
                {
                   // return false;
                    int rightPosY = prevY;
                    if (checkIfPositionIsImpassable(rightSide, nextPos, length, ref rightPosY, allowJump))
                    {
                        return false;
                    }
                }
                
                
                prevPos = nextPos;
            }

            return true;
        }

        bool checkIfPositionIsImpassable(Map.WorldPosition toPos, Map.WorldPosition prevPos, float length, ref int prevY, bool allowJump)
        {
            const float JumpBeforeHinderLength = 1;

            if (parent.boundary != null && !parent.boundary.areaWorldXZ.IntersectPoint(toPos.PlaneCoordinates))
            {
                return true;
            }

            if (parent.levelCollider.isBlocked(toPos))
            {
                return true;
            }
            //Vector2 pos;
            //LevelRoom room;
           // SubLevel subLevel = LfRef.levels.GetSubLevelUnsafe(toPos);
            //if (subLevel == null)
            //{
            //    return true;
            //}
            //else
            //{
            //    float distance = subLevel.GetRoomDistance(toPos, new EuclideanSquaredNorm(), out pos, out room);
            //    if (distance > 0)
            //    {
            //        return true;
            //    }
            //}

            if (toPos.WorldGrindex.X != prevPos.WorldGrindex.X || toPos.WorldGrindex.Z != prevPos.WorldGrindex.Z)
            {
                int y = (int)LfRef.chunks.GetScreenSafe(toPos.ChunkGrindex).GetClosestFreeY(toPos);

                if (y > prevY)
                {
                    int diff = y - prevY;

                    if (diff > maxClimbHeight)
                    {
                        if (!allowJump || diff > maxJumpUp)
                        {
                            return true;
                        }
                        else
                        {
                            jump = length <= JumpBeforeHinderLength;
                        }
                    }
                }
                else 
                {
                    int diff = prevY - y;
                    if (diff > maxFallHeight)
                    {
                        if (!allowJump || diff > maxJumpDown)
                        {
                            return true;
                        }
                        else
                        {
                            jump = length <= JumpBeforeHinderLength;
                        }
                    }
                }

                prevY = y;
            }

            return false;
        }
    }
}
