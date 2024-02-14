using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players.Ai    
{
    class UnitAiActions
    {
        public WalkingPath walkingPath;
        public AttackTargetGroup targetGroup;

        AttackTarget closestTarget = null;
        //WalkingPath pathToClosest = null;

        public Unit unit;
        UnitActionCount actionCount;
        int moveLength;
        public AiObjectiveType objectiveType;
       // AbsUnitAiProperty aiState;
        List<AttackTarget> allTargets;

        public UnitAiActions(Unit unit)
        {
            this.unit = unit;
            if (unit != null)
            {
                actionCount = UnitActionCount.First;
                moveLength = unit.MoveLengthWithModifiers(true);
               // aiState = unit.data.properties.aiObjective();
            }
        }

        public AiState NextAction_Asynch()
        {
            AiState result;

            allTargets = collectAiTargets();

            do
            {
                bool overridingAiAction = unit.data.properties.overridingAiActions(
                    this, actionCount, out result);

                if (!overridingAiAction)
                {
                    switch (actionCount)
                    {
                        case UnitActionCount.First:
                            result = firstAction();
                            break;
                        case UnitActionCount.Second:
                            result = secondAction();
                            break;

                        default:
                            endAction();
                            result = AiState.UnitActivationComplete;
                            break;
                    }
                }
                actionCount++;

            } while (result == AiState.NextUnitAction);

            return result;
        }

        AiState firstAction()
        {
            //if (aiState != null)
            //{
            //    switch (aiState.AiState)
            //    {
            //        case UnitAiState.Idle:
            //            return AiState.UnitActivationComplete;
            //        case UnitAiState.ObjectiveGoal:
            //            walkToObjective();
            //            if (walkingPath != null)
            //            {
            //                return checkMoveAction(walkingPath);
            //            }
            //            break;
            //    }
            //}            

            findClosestTarget();

            if (closestTarget == null)
            {
                return AiState.NextUnitAction;
            }
            else if (unit.data.IsProjectileAttackMain())
            {
                bool willReach;
                walkToRangedTarget(out willReach);

                if (willReach)
                {
                    createAttackTarget(closestTarget, false);
                }

                return checkMoveAction(walkingPath);
            }
            else if (aoeAttack())
            {
                return checkMoveAction(walkingPath);
            }
            else if (unit.data.HasMeleeAttack &&
                moveLength >= closestTarget.path.Length)//pathToClosest.Length)
            {
                createAttackTarget(closestTarget, true);
                walkingPath = closestTarget.path;
                return checkMoveAction(walkingPath);
            }
            else
            {
                //Just walk towards target
                walkingPath = closestTarget.path;
                return checkMoveAction(walkingPath);
            }            
        }

        AiState secondAction()
        {
            //if (aiState != null)
            //{
            //    if (onObjective())
            //    {
            //        return AiState.Objective;
            //    }
            //}

            if (targetGroup == null)
            {
                //Se om råkar stå och kan nå annat target
                var adj = adjacentTargets(unit.squarePos);
                if (adj != null)
                {
                    var target = ToGG.Ai.WeakestTarget(adj);
                    createAttackTarget(target, true);
                }
                else if (unit.data.WeaponStats.HasProjectileAttack)
                {
                    //Shoot a weaker projectile
                    var target = ToGG.Ai.HasRangedTargetFromPos(unit, unit.squarePos, closestTarget, allTargets);

                    if (target != null)
                    {
                        createAttackTarget(target, false);
                    }
                }
            }
            else
            {
                //double check range
                if (unit.InMeleeRange(targetGroup[0].position) == false)
                {
                    int range = unit.FireRangeWithModifiers(unit.squarePos);

                    if (unit.InRangeAndSight(targetGroup[0].position,
                        range, false, 
                        targetGroup.LosTerrainTarget) == false)
                    {
                        targetGroup = null;
                    }
                }
            }

            if (targetGroup != null)
            {
                return AiState.Attack;
            }


            return AiState.NextUnitAction;
        }

        void endAction()
        {
            SpecialActionPriority priority = unit.MovedThisTurn ? SpecialActionPriority.AfterFullActiviation : SpecialActionPriority.AfterNoMovement;

            hasSpecialAction(priority);
        }

        public AiState checkMoveAction(WalkingPath walkingPath)
        {
            if (walkingPath != null && 
                walkingPath.Length > 0 &&
                unit.ableToMove())
            {                
                walkingPath.applyMoveLengthRestrictions(unit, moveLength);

                return AiState.Move;
            }
            else
            {
                return AiState.NextUnitAction;
            }
        }

        //public AttackTarget HasRangedTargetFromPos(AbsUnit activeUnit, IntVector2 fromPos,
        //    AttackTarget prioTarget)
        //{
        //    List<AttackTarget> targets = new List<AttackTarget>(2);
        //    IntVector2 non;
        //    int range = activeUnit.FireRangeWithModifiers(activeUnit.squarePos);

        //    foreach (var t in allTargets)
        //    {
        //        if (activeUnit.InRangeAndSight(fromPos, t.position, range, false, t.LosTerrainTarget))
        //        {
        //            if (t.Equals(prioTarget))
        //            {
        //                return prioTarget;
        //            }
        //            targets.Add(t);
        //        }
        //    }

        //    if (targets.Count > 0)
        //    {
        //        var target = ToGG.Ai.ClosestAndWeakestTarget(null, fromPos, false, targets);

        //        return target;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        bool aoeAttack()
        {
            AttackSettings attackSettings;
            var area = unit.data.AoeAttack(true, out attackSettings);
            if (area != null)
            {
                List<IntVector2> targetSquares;
                List<AttackTarget> attackTargets;
                if (area.pathFindBestTargetCount(unit, out targetSquares,
                    out attackTargets, out walkingPath))
                {
                    createAttackTarget(attackTargets, true, attackSettings);
                    return true;
                }
            }

            return false;
        }

        void walkToRangedTarget(out bool willReach)
        {
            walkingPath = null;

            bool adjToOpponents;

            if (canRangeAttackFrom(unit.squarePos, out adjToOpponents))
            {
                //stand still and fire
                willReach = true;
                return;
            }

            if (!adjToOpponents && closestTarget.path != null)
            {
                closestTarget.path.applyMoveLengthRestrictions(unit, moveLength);

                //Check the steps towards the opponent, if any has LOS
                for (int i = closestTarget.path.squaresEndToStart.Count - 2; i >= 1; --i)
                {
                    if (canRangeAttackFrom(closestTarget.path.squaresEndToStart[i], out adjToOpponents))
                    {
                        closestTarget.path.setMaxLength(closestTarget.path.squaresEndToStart.Count - i);
                        walkingPath = closestTarget.path;
                        willReach = true;
                        return;
                    }
                }
            }

            //Start a new path finding
            int value;

            ToGG.Ai.BestWalkToRangedTarget(unit, closestTarget.position, 
                closestTarget.LosTerrainTarget, true,
                out value, out walkingPath, out willReach);
        }

        public void walkToObjective(List<IntVector2> goal, bool adjacentToGoal)
        {
            walkingPath = null;
            float bestValue = float.MinValue;

            //bool adjacentToGoal;
            //List<IntVector2> goal = aiState.objectiveGoal.objectiveGoal(
            //    unit, out adjacentToGoal, out objectiveType);

            if (goal != null)
            {
                foreach (var goalPos in goal)
                {
                    WalkingPath path = unit.FindPath(goalPos, adjacentToGoal);
                    if (path.success)
                    {
                        float value = -path.Length - MathExt.Square(path.expectedDamage);

                        if (value > bestValue)
                        {
                            walkingPath = path;
                            bestValue = value;
                        }
                    }
                }
            }
        }

        //bool onObjective()
        //{
        //    bool adjacentToGoal;
        //    List<IntVector2> goal = aiState.objectiveGoal.objectiveGoal(
        //        unit, out adjacentToGoal, out objectiveType);

        //    if (goal != null)
        //    {
        //        foreach (var goalPos in goal)
        //        {
        //            bool onPos;

        //            if (adjacentToGoal)
        //            {
        //                onPos = unit.AdjacentTo(goalPos);
        //            }
        //            else
        //            {
        //                onPos = unit.squarePos == goalPos;
        //            }

        //            if (onPos)
        //            {
        //                //Debug.Log("Ai actions, On Objective " + unit.ToString() + ", " + objectiveType.ToString());
        //                if (objectiveType == AiObjectiveType.DeliverItem)
        //                {
        //                    lib.DoNothing();
        //                }

        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //List<IntVector2> objectiveGoal()
        //{
        //    if (hqRef.players.dungeonMaster.objective != null)
        //    {
        //        List<IntVector2> goal;
        //        if (unit.HasProperty(UnitPropertyType.CarryItem))
        //        {
        //            objectiveType = AiObjectiveType.DeliverItem;
        //            goal = hqRef.players.dungeonMaster.objective.bringTo;
        //        }
        //        else
        //        {
        //            objectiveType = AiObjectiveType.CollectItem;
        //            goal = hqRef.players.dungeonMaster.objective.collect;
        //        }

        //        if (arraylib.HasMembers(goal))
        //        {
                    

        //            return goal;
        //        }
        //    }

        //    return null;
        //}

        bool canRangeAttackFrom(IntVector2 square, out bool adjToOpponents)
        {
            adjToOpponents = unit.lockedInMelee();//unit.bAdjacentOpponents(square);

            if (!adjToOpponents)
            {
                int range = unit.FireRangeWithModifiers(square);

                if (unit.InRangeAndSight(square, closestTarget.position, range, 
                    false, closestTarget.LosTerrainTarget))
                {
                    return true;
                }
            }

            return false;
        } 

        void findClosestTarget()
        {
            List<AttackTarget> bestTarget = new List<AttackTarget>(2);
            int shortestDistance = int.MaxValue;


            foreach (var m in allTargets)
            {
                
                    if (unit.squarePos.SideLength(m.position) <= shortestDistance)
                    {
                        m.path = unit.FindPath(m.position, true);

                        if (m.path != null && m.path.success)
                        {
                            if (m.path.Length == shortestDistance)
                            {
                                bestTarget.Add(m);
                            }
                            else if (m.path.Length < shortestDistance)
                            {
                                bestTarget.Clear();
                                bestTarget.Add(m);//new AiTarget(opponents.sel, path));

                                shortestDistance = m.path.Length;
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
        }

        List<AttackTarget> adjacentTargets(IntVector2 fromPos)
        {
            List<AttackTarget> result = new List<AttackTarget>();
            foreach (var m in allTargets)
            {
                if (fromPos.SideLength(m.position) == 1)
                {
                    result.Add(m);
                }
            }

            return result;
        }

        List<AttackTarget> collectAiTargets()
        {
            List<AttackTarget> targets = new List<AttackTarget>(32);

            var opponents = HeroQuest.hqRef.players.CollectEnemyUnits(unit.Player);
            while (opponents.Next())
            {
                if (unit.canTargetUnit(opponents.sel))
                {
                    targets.Add(new AttackTarget(opponents.sel));
                }
            }

            if (hqRef.setup.conditions.objectiveAttackTargets != null)
            {
                foreach (var pos in hqRef.setup.conditions.objectiveAttackTargets)
                {
                    targets.Add(new AttackTarget(pos, AttackTargetType.Objective));
                }
            }

            return targets;
        }

        bool hasSpecialAction(SpecialActionPriority priority)
        {
            var actions = unit.Data.properties.specialActions(SpecialActionClass.Any);

            if (actions != null)
            {
                foreach (var m in actions)
                {
                    var perform = m.ai_useAction(unit, priority);

                    if (perform != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void createAttackTarget(AttackTarget target, bool melee)
        {
            if (target == null)
            {
                targetGroup = null;
            }
            else
            {
                createAttackTarget(new List<AttackTarget> { target }, melee, null);
            }
        }

        void createAttackTarget(List<AttackTarget> targets, bool melee, AttackSettings attackSettings = null)
        {
            if (targets == null)
            {
                targetGroup = null;
            }
            else
            {
                targetGroup = new AttackTargetGroup(targets, melee);

                if (attackSettings == null)
                {
                    attackSettings = unit.data.attackSettings(melee);
                }

                targetGroup.attackSettings = attackSettings;
            }
        }
    }

    class UnitPathPair
    {
        public AbsUnit unit;
        public WalkingPath path;

        public UnitPathPair(AbsUnit unit, WalkingPath path)
        {
            this.unit = unit;
            this.path = path;
        }
    }

    enum AiObjectiveType
    {
        None,
        CollectItem,
        DeliverItem,
        MoveTo,
        AttackObject,
        Idle,
    }

    enum UnitActionCount
    {
        First,
        Second,
        End
    }
}
