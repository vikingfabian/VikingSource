using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class Poision : AbsCondition
    {
        public const string PoisionName = "Poision";
        static readonly BellValue poisionReductionPerTurn = new BellValue(1);
        public const SpriteName PoisionIcon = SpriteName.cmdPoisionIcon;
        public static string PoisionDesc(int? level)
        {
            string levelTxt = level == null ? "X" : level.Value.ToString();

            return LanguageLib.FormatSentences(
            //"Prevents any healing",
                "Recieved damage is increased by " + levelTxt,
                "At end of each turn: the poision will be reduced by " + poisionReductionPerTurn.IntervalToString());
        }

        int level;

        public Poision(int level)
        {
            this.level = level;
        }

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.TurnEnd:
                    level -= poisionReductionPerTurn.Next();
                    if (level <= 0)
                    {
                        return PropertyEventAction.Remove;
                    }
                    break;
            }
            return base.OnEvent(unit, eventType, tag);
        }

        public override SpriteName Icon => PoisionIcon;

        public override string Name => PoisionName + " " + level.ToString();

        public override string Desc => PoisionDesc(level);

        public override int Level { get => level; set => level = value; }

        public override ConditionType ConditionType => ConditionType.Poision;

        public override int StatusIsPositive => -1;
    }

    class PoisionSurgeOption : AbsSurgeOption
    {
        //int poision;
        public PoisionSurgeOption(int surgeCost, int poision)
            : base(surgeCost, poision)
        {
        }

        public override SpriteName Icon => SpriteName.cmdPoisionIcon;

        public override string Name => Poision.PoisionName + " " + resultCount.ToString();

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new PoisionedUnitAffectTooltip() };
        }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            throw new NotImplementedException();
        }

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Poision;
    }

    class PoisionedUnitAffectTooltip : AbsExtToolTip
    {       
        public override SpriteName Icon => SpriteName.cmdPoisionIcon;
        public override string Title => Poision.PoisionName + " X";
        public override string Text => Poision.PoisionDesc(null);
    }
}
