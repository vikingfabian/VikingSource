using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    abstract class AbsUnitCondition : AbsUnitProperty
    {
        abstract public int StatusIsPositive { get; }

        public void RemoveCondition(AbsUnit parentUnit)
        {
            parentUnit.cmd().conditions.Remove(this);
        }

        //override public SpriteName HudBackgroundTexture
        //{
        //    get { 
        //        if (StatusIsPositive > 0) return SpriteName.toggConditionPositiveTex;
        //        if (StatusIsPositive < 0) return SpriteName.toggConditionNegativeTex;
        //        else return SpriteName.toggConditionNeutralTex;
        //    }
        //}
    }

    class OpenTargetCondition : AbsUnitCondition
    {
        public const float AddChanceToHit = 0.2f;

        public override string Name => "Open taget";
        public override UnitPropertyType Type => UnitPropertyType.OpenTarget;

        public override string Desc => string.Format("+{0} chance to hit. Removed after one turn.", TextLib.PercentText(AddChanceToHit)); //Open target efter attack
             
        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.TurnStart)
            {
                RemoveCondition(parentUnit);
            }
        }

        public override int StatusIsPositive => -1;
    }
}
