using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class Stunned : AbsCondition
    {
        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            if (eventType == EventType.TurnEnd)
            {
                unit.condition.Set(DefaultStunImmune(), true, true, false);
                return PropertyEventAction.Remove;
            }
            return base.OnEvent(unit, eventType, tag);
        }

        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.CantActivate;
        }

        public override SpriteName Icon => SpriteName.cmdStunnIcon;

        public override string Name => "Stunned";

        public override string Desc =>
            "Will not activate during it's turn. Will then gain " + DefaultStunImmune().Name + ".";

        public override ConditionType ConditionType => ConditionType.Stunned;

        public override int StatusIsPositive => -1;

        static StunImmune DefaultStunImmune()
        {
            return new StunImmune(2);
        }
    }

    class StunImmune : AbsCondition
    {
        int level;

        public StunImmune(int level)
        {
            this.level = level;
        }

        //public override void onApply(Unit unit)
        //{
        //    base.onApply(unit);
        //    unit.PlayerHQ.TurnsCount;
        //}

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            if (eventType == EventType.TurnEnd &&
                applyTurn < unit.PlayerHQ.TurnsCount)
            {                    
                level--;
                if (level <= 0)
                {
                    return PropertyEventAction.Remove;
                }
            }
            return base.OnEvent(unit, eventType, tag);
        }

        public override SpriteName Icon => SpriteName.cmdStunnIcon;

        public override string Name => "Stun immune " + level.ToString();

        public override string Desc => "Can't be stunned, for " + level.ToString() + " turns";

        public override ConditionType ConditionType => ConditionType.StunImmune;

        public override int StatusIsPositive => 1;

        public override int Level { get => level; set => level = value; }
    }
}
