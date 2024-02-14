using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players.Ai
{
    abstract class AbsUnitAiProperty : AbsUnitProperty
    {
        public AbsAiObjectiveGoal objectiveGoal;

        public AbsUnitAiProperty(AbsAiObjectiveGoal objectiveGoal)
        {
            this.objectiveGoal = objectiveGoal;
            if (AiState != UnitAiState.Idle && objectiveGoal == null)
            {
                throw new ArgumentNullException();
            }
        }

        public override SpriteName Icon => SpriteName.cmdAiBrain;

        public override SpriteName HoverIcon => Icon;

        protected const string AiPrefix = "Ai - ";

        public override bool IsAiState => true;

        abstract public UnitAiState AiState { get; }
    }

    class UnitAiIdle : AbsUnitAiProperty
    {
        public UnitAiIdle()
            : base(null)
        { }

        public override bool overridingAiActions(UnitAiActions aiActions, 
            UnitActionCount actionCount, out AiState result)
        {
            result = Players.AiState.UnitActivationComplete;
            return true;
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.DamageTarget)
            {
                removeFromUnit(parentUnit);
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override SpriteName Icon => SpriteName.cmdAiBrainObjective;
        public override UnitPropertyType Type => UnitPropertyType.UnitAiIdle;

        public override string Name => AiPrefix + "Idle";

        public override string Desc => "Will wait until being attacked";

        public override UnitAiState AiState => UnitAiState.Idle;
    }    

    class UnitAiObjectiveAlways : AbsUnitAiProperty
    {
        public UnitAiObjectiveAlways(AbsAiObjectiveGoal objective)
            : base(objective)
        {
        }

        public override bool overridingAiActions(UnitAiActions aiActions, 
            UnitActionCount actionCount, out AiState result)
        {
            if (actionCount == UnitActionCount.First)
            {
                bool adjacentToGoal;
                List<IntVector2> goal = objectiveGoal.objectiveGoal(
                    aiActions.unit, out adjacentToGoal, out aiActions.objectiveType);

                aiActions.walkToObjective(goal, adjacentToGoal);
                if (aiActions.walkingPath != null)
                {
                    result = aiActions.checkMoveAction(aiActions.walkingPath);
                    return true;
                }
            }
            else if (actionCount == UnitActionCount.Second)
            {
                if (objectiveGoal.onObjective(aiActions.unit, out aiActions.objectiveType))
                {
                    result = Players.AiState.Objective;
                    return true;
                }
            }

            return base.overridingAiActions(aiActions, actionCount, out result);
        }

        public override SpriteName Icon => SpriteName.cmdAiBrainObjectiveAlways;
        public override UnitPropertyType Type => UnitPropertyType.UnitAiObjectiveAlways;

        public override string Name => AiPrefix + "Objective always";

        public override string Desc => "Will always go for the objective";

        public override UnitAiState AiState => UnitAiState.ObjectiveGoal;
    }

    class UnitAiObjective : UnitAiObjectiveAlways
    {
        public UnitAiObjective(AbsAiObjectiveGoal objective)
            : base(objective)
        {
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.DamageTarget)
            {
                removeFromUnit(parentUnit);
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override SpriteName Icon => SpriteName.cmdAiBrainObjective;
        public override UnitPropertyType Type => UnitPropertyType.UnitAiObjective;

        public override string Name => AiPrefix + "Objective";

        public override string Desc => "Will follow the mission objective until being attacked";

        public override UnitAiState AiState => UnitAiState.ObjectiveGoal;
    }

    enum UnitAiState
    {
        None,
        Idle,
        ObjectiveGoal,
    }

    

    class Bark : AbsUnitProperty
    {
        const int SpotRange = 4;

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (parentUnit.aiAlerted)
            {
                if (eventType == EventType.TurnEnd)
                {
                    removeFromUnit(parentUnit);
                }
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override bool overridingAiActions(UnitAiActions aiActions,
            UnitActionCount actionCount, out AiState result)
        {
            if (aiActions.unit.aiAlerted)
            {
                barkAction(aiActions.unit);
            }

            result = Players.AiState.UnitActivationComplete;
            return true;
        }

        void barkAction(Unit unit)
        {
            {//Spot opponents
                var spottedUnits = ToggEngine.Map.AreaFilter.Units_InRangeAndSight(unit.squarePos, SpotRange, false,
                    ToggEngine.Data.PlayerFilter.OpponentsTo(unit));

                foreach (var m in spottedUnits)
                {
                    m.hq().condition.Set(Data.Condition.ConditionType.Spotted, 1,
                       false, true, true);
                }
            }

            {//Alert friendlies
                var friendly = hqRef.players.CollectFriendlyUnits(unit);
                while (friendly.Next())
                {
                    if (!friendly.sel.aiAlerted &&
                        !toggRef.Square(friendly.sel.squarePos).hidden)
                    {
                        friendly.sel.Alert();
                    }
                }
            }

            unit.textAnimation(SpriteName.NO_IMAGE, "Woof-woof!");
            hqRef.players.dungeonMaster.holdCamera();
        }

        public override SpriteName Icon => SpriteName.cmdBark;

        public override SpriteName HoverIcon => Icon;

        public override UnitPropertyType Type => UnitPropertyType.Bark;

        public override string Desc => "Uses it's first turn to bark. Will alert all visible units. Opponents with get Spotted condition, range: " + SpotRange.ToString();
    }
}
