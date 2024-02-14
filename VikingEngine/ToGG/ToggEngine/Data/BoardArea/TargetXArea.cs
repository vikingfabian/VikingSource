using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.ToggEngine.Data
{
    class TargetXArea : AbsBoardArea
    {
        public int targetCount;
        public TargetXArea(int targetCount)
        {
            this.targetCount = targetCount;
        }

        public override bool pathFindBestTargetCount(AbsUnit activeUnit, 
            out List<IntVector2> targetSquares, out List<AttackTarget> attackTargets, out WalkingPath path)
        {
            List<AbsUnit> mostAttackTargets = new List<AbsUnit>(targetCount);
            List<AbsUnit> attackTargetsProgress = new List<AbsUnit>(targetCount);
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
                        if (m == new IntVector2(-1, 1))
                        {
                            lib.DoNothing();
                        }
                        IntVector2 adjPosition = m + opponents.sel.squarePos;

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
                targetSquares = toggLib.ToPositions(mostAttackTargets);

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
                    attackTargetsProgress.Clear();

                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        IntVector2 nPos = pos + dir;
                        
                        
                        var unit = toggRef.board.getUnit(nPos);

                        if (unit != null &&
                            activeUnit.canTargetUnit(unit))
                        {
                            attackTargetsProgress.Add(unit);
                        }
                    }

                    Ai.WeakestUnits(attackTargetsProgress, targetCount);

                    if (attackTargetsProgress.Count > mostAttackTargets.Count ||
                        (attackTargetsProgress.Count == mostAttackTargets.Count &&
                        pathToPos.Length < shortestMoveLength))
                    {
                        //Found better target
                        shortestMoveLength = pathToPos.Length;

                        mostAttackTargets.Clear();
                        mostAttackTargets.AddRange(attackTargetsProgress);

                        pathToBest = pathToPos;
                    }
                }

            }
        }
    }
}
