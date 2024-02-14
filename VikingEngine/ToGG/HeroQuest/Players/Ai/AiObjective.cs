using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Players.Ai
{
    abstract class AbsAiObjectiveGoal
    {
        abstract public List<IntVector2> objectiveGoal(Unit unit, out bool adjacentToGoal, out AiObjectiveType objectiveType);

        public bool onObjective(Unit unit, out AiObjectiveType objectiveType)
        {
            bool adjacentToGoal;
            List<IntVector2> goal = objectiveGoal(
                unit, out adjacentToGoal, out objectiveType);

            if (goal != null)
            {
                foreach (var goalPos in goal)
                {
                    bool onPos;

                    if (adjacentToGoal)
                    {
                        onPos = unit.AdjacentTo(goalPos);
                    }
                    else
                    {
                        onPos = unit.squarePos == goalPos;
                    }

                    if (onPos)
                    {
                        //Debug.Log("Ai actions, On Objective " + unit.ToString() + ", " + objectiveType.ToString());
                        if (objectiveType == AiObjectiveType.DeliverItem)
                        {
                            lib.DoNothing();
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }

    class AiObjectiveCollect : AbsAiObjectiveGoal
    {
        public List<IntVector2> collect;
        public List<IntVector2> bringTo;

        override public List<IntVector2> objectiveGoal(Unit unit, out bool adjacentToGoal, out AiObjectiveType objectiveType)
        {
            adjacentToGoal = false;

            List<IntVector2> goal;
            if (unit.HasProperty(UnitPropertyType.CarryItem))
            {
                objectiveType = AiObjectiveType.DeliverItem;
                goal = bringTo;
            }
            else
            {
                objectiveType = AiObjectiveType.CollectItem;
                goal = collect;
            }

            if (arraylib.HasMembers(goal))
            {
                return goal;
            }
            
            objectiveType = AiObjectiveType.None;
            return null;
        }
    }

    class AiObjectiveInteract : AbsAiObjectiveGoal
    {
        public List<IntVector2> pos;

        override public List<IntVector2> objectiveGoal(Unit unit, out bool adjacentToGoal, out AiObjectiveType objectiveType)
        {
            adjacentToGoal = false;

            //List<IntVector2> goal;
            //if (unit.HasProperty(UnitPropertyType.CarryItem))
            //{
            //    objectiveType = AiObjectiveType.DeliverItem;
            //    goal = bringTo;
            //}
            //else
            //{
                //objectiveType = AiObjectiveType.CollectItem;
                //goal = pos;
            //}

            if (arraylib.HasMembers(pos))
            {
                objectiveType = AiObjectiveType.MoveTo;
                return pos;
            }

            objectiveType = AiObjectiveType.None;
            return null;
        }
    }

    class AiObjectiveMoveToUnit : AbsAiObjectiveGoal
    {
        public Unit targetUnit;
        List<IntVector2> pos;

        public AiObjectiveMoveToUnit(Unit unit)
        {
            this.targetUnit = unit;
            pos = new List<IntVector2> { IntVector2.Zero };
        }

        override public List<IntVector2> objectiveGoal(Unit unit, 
            out bool adjacentToGoal, out AiObjectiveType objectiveType)
        {
            adjacentToGoal = true;

            if (unit.Alive)
            {
                objectiveType = AiObjectiveType.MoveTo;
                pos[0] = targetUnit.squarePos;

                return pos;
            }
            else
            {
                objectiveType = AiObjectiveType.None;
                return null;
            }
        }
    }

    class AiObjectiveAttackObject : AbsAiObjectiveGoal
    {
        public List<IntVector2> target;

        public AiObjectiveAttackObject(List<IntVector2> target)
        {
            this.target = target;
        }

        override public List<IntVector2> objectiveGoal(Unit unit, out bool adjacentToGoal, out AiObjectiveType objectiveType)
        {
            adjacentToGoal = true;
            objectiveType = AiObjectiveType.None;
            return null;
        }
    }
}
