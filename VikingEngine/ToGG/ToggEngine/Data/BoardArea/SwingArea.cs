using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    class SwingArea : AbsBoardArea
    {
        public int swingLength;

        public SwingArea(int swingLength)
        {
            this.swingLength = swingLength;
        }

        public override bool pathFindBestTargetCount(AbsUnit activeUnit,
             out List<IntVector2> targetSquares, out List<AttackTarget> attackTargets, out WalkingPath path)
        {
            List<AbsUnit> mostAttackTargets = new List<AbsUnit>(swingLength);
            List<AbsUnit> attackTargetsProgress = new List<AbsUnit>(swingLength);
            List<IntVector2> bestTargetSquares = new List<IntVector2>(swingLength);
            List<IntVector2> bestTargetSquaresProgress = new List<IntVector2>(swingLength);
            int shortestMoveLength = int.MaxValue;
            WalkingPath pathToBest = null;

            CheckedSquares checkedSquares = new CheckedSquares();
            var opponents = hqRef.players.CollectEnemyUnits(activeUnit.Player);
            while (opponents.Next())
            {
                if (activeUnit.canTargetUnit(opponents.sel))
                {
                    foreach (var m in IntVector2.Dir8Array)
                    {
                        IntVector2 adjPosition = m + opponents.GetSelection.squarePos;

                        if (checkedSquares.IsUnchecked(adjPosition))
                        {
                            checkStandingPos(adjPosition);
                        }
                    }
                }
            }

            if (mostAttackTargets.Count >= 2)
            {
                attackTargets = AttackTarget.Conv(mostAttackTargets);
                targetSquares = bestTargetSquares;
                path = pathToBest;

                return true;
            }
            else
            {
                attackTargets = null;
                targetSquares = null;
                path = null;

                return false;
            }

            void checkStandingPos(IntVector2 pos)
            {
                WalkingPath pathToPos;

                if (Ai.CanMoveTo(activeUnit, pos, out pathToPos))
                {
                    Dir8 startDir = toggLib.GeneralDirection(activeUnit.squarePos, pos, true);
                    startDir = lib.WrapDir(startDir - (swingLength / 2));

                    for (int startIx = 0; startIx < lib.Dir8Count; ++startIx)
                    {
                        attackTargetsProgress.Clear();
                        bestTargetSquaresProgress.Clear();

                        for (int swingIx = 0; swingIx < swingLength; ++swingIx)
                        {
                            Dir8 checkDir = lib.WrapDir(startDir + (startIx + swingIx));
                            IntVector2 targetPos = IntVector2.FromDir8(checkDir) + pos;
                            bestTargetSquaresProgress.Add(targetPos);

                            var unit = toggRef.board.getUnit(targetPos);
                            if (unit != null &&
                                activeUnit.canTargetUnit(unit))
                            {
                                attackTargetsProgress.Add(unit);
                            }
                        }

                        if (attackTargetsProgress.Count > mostAttackTargets.Count ||
                            (attackTargetsProgress.Count == mostAttackTargets.Count &&
                            pathToPos.Length < shortestMoveLength))
                        {
                            //Found better target
                            shortestMoveLength = pathToPos.Length;

                            mostAttackTargets.Clear();
                            mostAttackTargets.AddRange(attackTargetsProgress);

                            bestTargetSquares.Clear();
                            bestTargetSquares.AddRange(bestTargetSquaresProgress);

                            pathToBest = pathToPos;
                        }
                    }
                }

            }
        }

        
    }
}
