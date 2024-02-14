using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO
{
    class SimpleFlyPath
    {
        static Vector3[] tryPaths = new Vector3[4];

        int[,] pathOrders = new int[,]
        {
            { 0, 2, 3, 1 },
            { 0, 3, 2, 1 },
            { 2, 3, 0, 1 },
            { 3, 2, 0, 1 },
            { 1, 2, 3, 0 },
            { 1, 0, 2, 3 },
        };

        GO.Characters.AbsCharacter parent;
        public Vector3 goalPos, pathDir;
        public float lengthToGoal;

        int boundHeight, boundHalfWidth;

        IntervalF backtrackTimeRange = new IntervalF(600, 1000);
        Time backtrackingTimeLeft = 0;

        public SimpleFlyPath(GO.Characters.AbsCharacter parent)
        {
            this.parent = parent;
            boundHeight = Convert.ToInt32(parent.CollisionAndDefaultBound.MainBound.halfSize.Y * 2f);
            boundHalfWidth = Convert.ToInt32(parent.CollisionAndDefaultBound.MainBound.halfSize.X);
        }

        public void AsynchUpdate(float time)
        {
            Vector3 start = parent.Position;
            Vector3 directPath = goalPos - start;
            lengthToGoal = directPath.Length();
            directPath.Normalize();
            if (!backtrackingTimeLeft.CountDown(time))
            {
                directPath *= -1f;
            }

            Vector3 dir = directPath;
            if (availablePathIndir(dir, start))
            {
                goto foundPath;
            }
            else
            {
                Vector3 left90Degrees = Vector3.Cross(Vector3.Up, directPath);
                Vector3 Right90Degrees = Vector3.Cross(directPath, Vector3.Up);

                Vector3 up90Degress = Vector3.Cross(Right90Degrees, directPath);
                Vector3 down90Degress = Vector3.Cross(directPath, Right90Degrees);

                const float OneThird = 0.33f;
                const float TwoThirds = 0.66f;

                //30 degree angle
                {
                    tryPaths[0] = up90Degress * OneThird + directPath * TwoThirds;
                    tryPaths[1] = down90Degress * OneThird + directPath * TwoThirds;
                    tryPaths[2] = left90Degrees * OneThird + directPath * TwoThirds;
                    tryPaths[3] = Right90Degrees * OneThird + directPath * TwoThirds;

                    int rndOrder = Ref.rnd.Int(pathOrders.GetLength(0));

                    for (int i = 0; i < 4; ++i)
                    {
                       dir = tryPaths[pathOrders[rndOrder, i]];
                        if (availablePathIndir(dir, start))
                        {
                            goto foundPath;
                        }
                    }
                }

                //60 degree angle
                {
                    tryPaths[0] = up90Degress * TwoThirds + directPath * OneThird;
                    tryPaths[1] = down90Degress * TwoThirds + directPath * OneThird;
                    tryPaths[2] = left90Degrees * TwoThirds + directPath * OneThird;
                    tryPaths[3] = Right90Degrees * TwoThirds + directPath * OneThird;

                    int rndOrder = Ref.rnd.Int(pathOrders.GetLength(0));

                    for (int i = 0; i < 4; ++i)
                    {
                        dir = tryPaths[pathOrders[rndOrder, i]];
                        if (availablePathIndir(dir, start))
                        {
                            goto foundPath;
                        }
                    }
                }
                //90 degree angle
                {
                    tryPaths[0] = up90Degress;
                    tryPaths[1] = down90Degress;
                    tryPaths[2] = left90Degrees;
                    tryPaths[3] = Right90Degrees;

                    int rndOrder = Ref.rnd.Int(pathOrders.GetLength(0));

                    for (int i = 0; i < 4; ++i)
                    {
                        dir = tryPaths[pathOrders[rndOrder, i]];
                        if (availablePathIndir(dir, start))
                        {
                            goto foundPath;
                        }
                    }
                }

                //could not find path
                hitObsticle();
                return;
            }

            foundPath:
                pathDir = dir;
        }

        bool availablePathIndir(Vector3 dir, Vector3 start)
        {
            float SteppLength = boundHalfWidth;
            dir *= SteppLength;
            Vector3 pos = start;
            //int ForwardChecks = lib.SmallestOfTwoValues((int)lengthToGoal, (int)(maxForwardCheckLength)) / SteppLength;
            const int ForwardChecks = 8;
            for (int i = 0; i < ForwardChecks; ++i)
            {
                pos += dir;
                Map.WorldPosition wp = new Map.WorldPosition(pos);

                if (parent.levelCollider.isBlocked(wp))
                {
                    return false;
                }
                //SubLevel subLevel = null;//LfRef.levels.GetSubLevelUnsafe(wp);
                //if (subLevel == null)
                //{
                //    return false;
                //}
                //else
                //{
                //    Vector2 outPos;
                //    LevelRoom room;

                //    float distance = subLevel.GetRoomDistance(wp, new VikingEngine.EngineSpace.Maths.EuclideanSquaredNorm(), out outPos, out room);
                //    if (distance > 0)
                //    {
                //        return false;
                //    }
                //}


                wp.WorldGrindex.X -= boundHalfWidth;
                wp.WorldGrindex.Z -= boundHalfWidth;

                IntVector3 checkPos = IntVector3.Zero;
                ForXYZLoop loop = new ForXYZLoop(wp.WorldGrindex, wp.WorldGrindex + new IntVector3(boundHalfWidth * 2, boundHeight, boundHalfWidth * 2) - 1);
                while (loop.Next())
                {
                    wp.WorldGrindex = loop.Position;
                    if (wp.BlockHasColllision())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void hitObsticle()
        {
            //Try backtrack a small bit to get unstuck
            if (backtrackingTimeLeft.TimeOut)
            {
                backtrackingTimeLeft.MilliSeconds = backtrackTimeRange.GetRandom();
            }
        }
    }
}
