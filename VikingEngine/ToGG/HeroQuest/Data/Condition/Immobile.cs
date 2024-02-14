using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class Immobile : AbsCondition
    {
        public const SpriteName ImmobileIcon = SpriteName.cmdImmobile;
        public const string ImmobileName = "Immobile";
        public const string CantMoveDesc = "The unit cannot move.";
        
        public static string ImmobileDesc()
        {
            return LanguageLib.FormatSentences(
               CantMoveDesc,
               "At end of each turn: Reduce Immobile by 1");
        }

        int level;

        public Immobile(int level)
        {
            this.level = level;
        }

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            if (eventType == EventType.TurnEnd)
            {
                if (--level <= 0)
                {
                    return PropertyEventAction.Remove;
                }
            }
            return base.OnEvent(unit, eventType, tag);
        }

        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.Immobile;
        }

        public override SpriteName Icon => ImmobileIcon;

        public override string Name => ImmobileName + " " + level.ToString();

        public override string Desc => ImmobileDesc();

        public override int Level { get => level; set => level = value; }

        public override ConditionType ConditionType => ConditionType.Immobile;

        public override int StatusIsPositive => -1;
    }
}
