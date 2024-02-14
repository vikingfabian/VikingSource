using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    class Bleed : AbsCondition
    {
        public const SpriteName BleedIcon = SpriteName.cmdBleedIcon;
        public static string BleedDesc(int level)
        {
            return LanguageLib.FormatSentences(
               "At end of each turn: " + Damage.BellDamage(level).description(),
               "Healing will remove the condition");
        }

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.TurnEnd:
                    RecordedDamageEvent rec;
                    var damage = damagePerTurn.NextDamage();
                    unit.hq().TakeDamage(damage, DamageAnimationType.AttackSlash, null, out rec);
                    rec.NetSend();
                    break;
                case EventType.Heal:
                    return PropertyEventAction.Remove;
            }
            return base.OnEvent(unit, eventType, tag);
        }

        Damage damagePerTurn;

        public Bleed()
        {
            damagePerTurn = Damage.BellDamage(1);
            damagePerTurn.applyType = DamageApplyType.Pure;
        }

        public override SpriteName Icon => BleedIcon;

        public override string Name => "Bleed " + damagePerTurn.ValueToString();

        public override string Desc => BleedDesc(damagePerTurn.Value);

        public override int Level { get => damagePerTurn.Value; set => damagePerTurn.Value = value; }

        public override ConditionType ConditionType => ConditionType.Bleed;

        public override int StatusIsPositive => -1;
    }

    class BleedSurgeOption : AbsSurgeOption
    {
        public BleedSurgeOption(int surgeCost, int level)
            : base(surgeCost, level)
        {
        }

        public override SpriteName Icon => Bleed.BleedIcon;

        public override string Name => "Bleed " + resultCount.ToString();

        public override ToggEngine.Display2D.AbsExtToolTip[] DescriptionKeyWords()
        {
            return new ToggEngine.Display2D.AbsExtToolTip[] { new BleedUnitAffectTooltip() };
        }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            foreach (var m in attack.attackRoll.targets)
            {
                m.unit.hq().condition.Set(ConditionType.Bleed, useCount * resultCount, true, true, true);
            }
        }

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Bleed;
    }

    class BleedUnitAffectTooltip : ToggEngine.Display2D.AbsExtToolTip
    {
        public override SpriteName Icon => Bleed.BleedIcon;
        public override string Title => "Bleed X";
        public override string Text => Bleed.BleedDesc(1);        
    }
}
