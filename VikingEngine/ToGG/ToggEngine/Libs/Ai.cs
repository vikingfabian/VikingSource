using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    static class Ai
    {
        const int DefaultMove1Cost = 10;
        const int TargetValue = 1000;
        const int KeepRangeDistanceValue = 50;

        public static int AttackValue(bool melee, int fireDistance, bool fartherIsBetter, int walkingDistance, int targetCount)
        {
            int result = targetCount * TargetValue;
            result -= walkingDistance * DefaultMove1Cost;

            if (!melee)
            {
                result += lib.BoolToLeftRight(fartherIsBetter) * (fireDistance * KeepRangeDistanceValue);
            }

            return result;
        }

        public static AbsUnit WalkToRangedAttack(AbsUnit activeUnit, bool fartherIsBetter, out WalkingPath walkingPath)
        {
            walkingPath = null;
            AbsUnit bestTarget = null;
            int bestValue = int.MinValue;

            var opponents = HeroQuest.hqRef.players.CollectEnemyUnits(activeUnit.Player);
            while (opponents.Next())
            {
                if (activeUnit.canTargetUnit(opponents.sel))
                {
                    int value;
                    WalkingPath path;
                    bool willReach;
                    Ai.BestWalkToRangedTarget(activeUnit, opponents.sel.squarePos, false, fartherIsBetter, 
                        out value, out path, out willReach);

                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestTarget = willReach? opponents.sel : null;
                        walkingPath = path;
                    }
                }
            }
            
            return bestTarget;
        }

        public static void BestWalkToRangedTarget(
            AbsUnit activeUnit, 
            IntVector2 target, 
            bool terrainTarget, 
            bool fartherIsBetter, 
            out int bestValue, 
            out WalkingPath bestPath, 
            out bool willReach)
        {
            bestPath = null;
            bestValue = int.MinValue;
            willReach = false;


            int moveLength = activeUnit.MoveLengthWithModifiers(true);

            int range = activeUnit.FireRangeWithModifiers(activeUnit.squarePos);

            ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(target, range));
            //IntVector2 non;

            while (loop.Next())
            {
                if (toggRef.board.CanEndMovementOn(loop.Position, activeUnit) &&
                    activeUnit.lockedInMelee(loop.Position) == false &&
                    activeUnit.InRangeAndSight(loop.Position, target, range, false, terrainTarget))
                {
                    WalkingPath path = activeUnit.FindPath(loop.Position, false);
                    int fireDistance = (target - loop.Position).SideLength();

                    int value;
                    bool inRange;
                    if (path.Length > moveLength)
                    { //Will not reach target
                        value = -fireDistance * 10 -path.squaresEndToStart.Count * DefaultMove1Cost;
                        inRange = false;
                    }
                    else
                    {
                        value = AttackValue(false, fireDistance, fartherIsBetter, path.squaresEndToStart.Count, 1);//= fireDistance * DefaultMove1Cost - path.squaresEndToStart.Count;
                        inRange = true;
                    }

                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestPath = path;
                        willReach = inRange;
                    }
                }
            }

            bestPath?.applyMoveLengthRestrictions(activeUnit, moveLength);
        }

        public static AttackTarget ClosestAndWeakestTarget(HeroQuest.Unit unit, IntVector2 fromPos, 
            bool pathFindDistance, List<AttackTarget> targets)
        {
            AttackTarget closestTarget = null;
            List<AttackTarget> bestTarget = new List<AttackTarget>(2);
            int shortestDistance = int.MaxValue;
            
            foreach (var m in targets)
            {
                int birdDist = fromPos.SideLength(m.position);

                if (birdDist <= shortestDistance)
                {
                    bool addable;
                    int length;
                    if (pathFindDistance)
                    {
                        m.path = unit.FindPath(m.position, true);
                        addable = m.path != null && m.path.success;
                        length = m.path.Length;
                    }
                    else
                    {
                        addable = true;
                        length = birdDist;
                    }

                    if (addable)
                    {
                        if (length == shortestDistance)
                        {
                            bestTarget.Add(m);
                        }
                        else if (length < shortestDistance)
                        {
                            bestTarget.Clear();
                            bestTarget.Add(m);//new AiTarget(opponents.sel, path));

                            shortestDistance = length;
                        }
                    }
                }

            }

            if (bestTarget.Count > 0)
            {
                closestTarget = ToGG.Ai.WeakestTarget(bestTarget);
            }
            else
            {
                //TODO, försök gå närmare även om path är blockat
                closestTarget = null;
            }

            return closestTarget;

            //List<AttackTarget> closest = new List<AttackTarget>(2);
            //int closestDist = int.MaxValue;

            //foreach (var m in targets)
            //{
            //    int dist = fromPos.SideLength(m.position);
            //    if (dist == closestDist)
            //    {
            //        closest.Add(m);
            //    }
            //    else if (dist < closestDist)
            //    {
            //        closest.Clear();
            //        closest.Add(m);
            //    }
            //}

            //return WeakestTarget(closest);
        }

        public static AbsUnit ClosestAndWeakestUnit(IntVector2 fromPos, List<AbsUnit> units)
        {
            List<AbsUnit> closest = new List<AbsUnit>(2);
            int closestDist = int.MaxValue;

            foreach (var m in units)
            {
                int dist = fromPos.SideLength(m.squarePos);
                if (dist == closestDist)
                {
                    closest.Add(m);
                }
                else if (dist < closestDist)
                {
                    closest.Clear();
                    closest.Add(m);
                }
            }

            return WeakestUnit(closest);
        }

        public static bool CanMoveTo(AbsUnit unit, IntVector2 pos, 
            out WalkingPath path, bool avoidDamage = true)
        {
            path = null;

            if (toggRef.board.CanEndMovementOn(pos, unit))
            {
                int moveLength = unit.MoveLengthWithModifiers(true);

                int minLength = (pos - unit.squarePos).SideLength();

                if (minLength <= moveLength)
                {
                    path = unit.FindPath(pos, false);
                    bool acceptableDamage = path.aiAcceptableDamage(unit);

                    if (path.success &&
                        path.Length <= moveLength &&
                        (!avoidDamage || acceptableDamage))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static int AdjacentEnemiesCount(AbsUnit activeUnit, IntVector2 fromPos)
        {
            int count = 0;

            foreach (var m in IntVector2.Dir8Array)
            {
                var sqUnit = toggRef.board.getUnit(fromPos + m);
                if (activeUnit.IsTargetOpponent(sqUnit))
                {
                    ++count;
                }
            }

            return count;
        }

        public static List<AbsUnit> CollectAdjacentEnemies(AbsUnit activeUnit, IntVector2 fromPos)
        {
            List<AbsUnit> result = null;
            
            foreach (var m in IntVector2.Dir8Array)
            {
                var sqUnit = toggRef.board.getUnit(fromPos + m);
                if (activeUnit.IsTargetOpponent(sqUnit))
                {
                    arraylib.AddOrCreate<AbsUnit>(ref result, sqUnit);    
                }
            }

            return result;
        }

        public static AbsUnit StrongestMeeleUnit(List<AbsUnit> units)
        {
            if (units.Count == 1)
            {
                return units[0];
            }

            FindMaxValuePointer<AbsUnit> strongest = new FindMaxValuePointer<AbsUnit>();

            foreach (var m in units)
            {
                strongest.Next(m.Data.WeaponStats.meleeStrength, m);
            }

            return randomIfNullTarget(units, strongest.maxMember);
        }

        public static AttackTarget WeakestTarget(List<AttackTarget> targets)
        {
            FindMinValuePointer<AttackTarget> weakest = new FindMinValuePointer<AttackTarget>();

            foreach (var m in targets)
            {
                weakest.Next(m.StrengthValue(), m);
            }

            return weakest.minMember;
        }

        public static AbsUnit WeakestUnit(List<AbsUnit> units)
        {
            if (units.Count == 1)
            {
                return units[0];
            }

            FindMinValuePointer<AbsUnit> weakest = new FindMinValuePointer<AbsUnit>();

            foreach (var m in units)
            {
                weakest.Next(m.StrengthValue(), m);
            }

            return randomIfNullTarget(units, weakest.minMember);
        }

        public static void WeakestUnits(List<AbsUnit> units, int keepCount)
        {
            while (units.Count > keepCount)
            {
                float weakest = float.MaxValue;
                int weakestIx = -1;

                for (int i = 0; i < units.Count; ++i)
                {
                    var strength = units[i].StrengthValue();
                    if (strength < weakest)
                    {
                        weakest = strength;
                        weakestIx = i;
                    }
                }

                units.RemoveAt(weakestIx);
            }
        }

        static AbsUnit randomIfNullTarget(List<AbsUnit> units, AbsUnit target)
        {
            if (target == null)
            {
                return arraylib.RandomListMember(units);
            }
            else
            {
                return target;
            }
        }


        public static AttackTarget HasTargetFromPos(AbsUnit activeUnit, IntVector2 fromPos, List<AttackTarget> opponents)
        {
            var melee = HasMeleeTargetFromPos(activeUnit, fromPos);
            if (melee != null)
            {
                return melee;
            }

            if (activeUnit.Data.WeaponStats.HasProjectileAttack)
            {
                return HasRangedTargetFromPos(activeUnit, fromPos, null, opponents);
            }

            return null;
        }

        public static AttackTarget HasMeleeTargetFromPos(AbsUnit activeUnit, IntVector2 fromPos)
        {
            var adj = activeUnit.adjacentOpponents(fromPos);
            if (adj != null)
            {
                var targets = AttackTarget.Conv(adj);
                var target = WeakestTarget(targets);
                target.attackType = AttackType.Melee;
                return target;
            }

            return null;
        }

        public static AttackTarget HasRangedTargetFromPos(AbsUnit activeUnit, IntVector2 fromPos,
            AttackTarget prioTarget, List<AttackTarget> opponents)
        {
            List<AttackTarget> targets = new List<AttackTarget>(2);
            int range = activeUnit.FireRangeWithModifiers(activeUnit.squarePos);

            foreach (var t in opponents)
            {
                if (activeUnit.InRangeAndSight(fromPos, t.position, range, false, t.LosTerrainTarget))
                {
                    if (prioTarget != null && t.Equals(prioTarget))
                    {
                        return prioTarget;
                    }
                    targets.Add(t);
                }
            }

            if (targets.Count > 0)
            {
                var target = ClosestAndWeakestTarget(null, fromPos, false, targets);
                target.attackType = AttackType.Ranged;  
                return target;
            }
            else
            {
                return null;
            }
        }
    }
}
