using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class AiModifier_TargetUnit
    {
        public AbsUnit unit;

        public AiModifier_TargetUnit(AbsUnit unit)
        {
            this.unit = unit;
        }
    }

    class AiModifier_MoveUnit
    {
        public AbsUnit unit;
        public IntVector2 goalPos;

        public AiModifier_MoveUnit(AbsUnit unit, IntVector2 goalPos)
        {
            this.unit = unit;
            this.goalPos = goalPos;
        }

        public bool OnGoal()
        {
            return unit.squarePos == goalPos;
        }
    }
}
