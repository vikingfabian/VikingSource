using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players.Ai
{
    //class AiTarget
    //{
    //    public bool unitType;
    //    public AbsUnit unit;
    //    public IntVector2 position;
    //    public WalkingPath path = null;

    //    public AiTarget(AbsUnit unit)
    //    {
    //        unitType = true;
    //        this.unit = unit;
    //        position = unit.squarePos;
    //    }

    //    public AiTarget(AbsUnit unit, WalkingPath path)
    //    {
    //        unitType = true;
    //        this.unit = unit;
    //        this.path = path;
    //        position = unit.squarePos;
    //    }

    //    public AiTarget(IntVector2 position)
    //    {
    //        unitType = false;
    //        this.position = position;
    //    }

    //    public bool canTarget(AbsUnit attacker)
    //    {
    //        if (unitType)
    //        {
    //            return attacker.canTargetUnit(unit);
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }

    //    public float StrengthValue()
    //    {
    //        if (unitType)
    //        {
    //            return unit.StrengthValue();
    //        }
    //        else
    //        {
    //            return 0;
    //        }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return ((AiTarget)obj).position == this.position;
    //    }
    //}
}
