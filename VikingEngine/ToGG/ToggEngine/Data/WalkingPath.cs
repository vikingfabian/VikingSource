using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class WalkingPath
    {
        const int IgnoreDirChangeTimes = 10;
        const float NodeMinDistance = 0.3f;

        public bool success;
        int currentNode;
        public List<IntVector2> squaresEndToStart;
        public float expectedDamage;

        public WalkingPath(List<IntVector2> squaresEndToStart)
        {
            //this.Backstabs = Backstabs;
            this.squaresEndToStart = squaresEndToStart;
            currentNode = squaresEndToStart.Count - 1;
        }

        public Vector2 DirToNextNode(Vector2 myPos, out bool complete)
        {
            IntVector2 to = squaresEndToStart[currentNode];
            Vector2 diff = (to.Vec + VectorExt.V2Half) - myPos;
            if (diff.Length() <= NodeMinDistance)
            {
                --currentNode;
            }
            complete = currentNode < 0;
            diff.Normalize();
            return diff;
        }

        public void applyMoveLengthRestrictions(AbsUnit unit, int moveLength)
        {
            setMaxLength(moveLength);
            checkEndSquareRestrictions(unit);
        }

        public void setMaxLength(int max)
        {
            if (squaresEndToStart.Count > max)
            {
                squaresEndToStart.RemoveRange(0, squaresEndToStart.Count - max);
                currentNode = squaresEndToStart.Count - 1;
            }
        }

        public void checkEndSquareRestrictions(AbsUnit unit)
        {
            while (squaresEndToStart.Count > 0 &&
                toggRef.board.CanEndMovementOn(squaresEndToStart[0], unit) == false)
            {
                //Check if the unit can stand next to the square
                const int SecondLastSquareIx = 1;
                IntVector2 from;
                if (arraylib.TryGet(squaresEndToStart, SecondLastSquareIx, out from) == false)
                {
                    from = unit.squarePos;
                }

                IntVector2 lastStep = squaresEndToStart[0] - from;
                Dir8 dir = conv.ToDir8(lastStep);

                TwoDirLoop loop = new TwoDirLoop();
                while (loop.Next())
                {
                    Dir8 sideDir = lib.Rotate(dir, loop.dir);
                    IntVector2 checkPos = IntVector2.FromDir8(sideDir) + from;

                    if (toggRef.board.CanEndMovementOn(checkPos, unit))
                    {
                        squaresEndToStart[0] = checkPos;
                        //Took side step
                        return;
                    }
                }

                //Found no side steps, end movement earlier
                squaresEndToStart.RemoveAt(0);
            }
        }

        public IntVector2 positionAfterMoveLength(int moveLength)
        {
            int startIx = squaresEndToStart.Count - 1;
            int endIx = Bound.Min(startIx - moveLength, 0);

            return squaresEndToStart[endIx];
        }

        public int Length => squaresEndToStart.Count;

        public bool aiAcceptableDamage(AbsUnit unit)
        {
            return expectedDamage <= unit.health.Value / 2;
        }

        public override string ToString()
        {
            string result = "Path(" + squaresEndToStart.Count.ToString() + "): ";
            if (squaresEndToStart.Count >= 1)
            {
                result += arraylib.Last(squaresEndToStart).ToString() + " to " + squaresEndToStart[0].ToString();
            }

            return result;
        }

    }
}
