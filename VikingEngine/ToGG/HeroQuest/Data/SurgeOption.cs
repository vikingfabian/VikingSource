using System;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    abstract class AbsSurgeOption : AbsProperty
    {
        public int surgeCost;
        public int resultCount;

        public AbsSurgeOption(int surgeCost, int resultCount = 1)
        {
            this.surgeCost = surgeCost;
            this.resultCount = resultCount;
        }

        public override string Desc => surgeCost.ToString() + " surges: " + Name;

        public override List<AbsRichBoxMember> AdvancedCardDisplay()
        {
            return base.AdvancedCardDisplay();
        }

        public void addConvertIcons(List<AbsRichBoxMember> richbox)
        {
            for (int i = 0; i < surgeCost; ++i)
            {
                richbox.Add(new RbImage(SpriteName.cmdIconSurge, 1f, -0.2f, -0.2f));                        
            }

            richbox.Add(new RbImage(SpriteName.cmdConvertArrow, HudLib.ConvertArrowScale));

            for (int i = 0; i < resultCount; ++i)
            {
                richbox.Add(new RbImage(Icon));
            }
        }

        abstract public void onSurge(int useCount, AttackDisplay attack);

        public int UseCount(ref int unusedSurges)
        {
            return MathExt.DivRest(ref unusedSurges, surgeCost);
        }

        abstract public SurgeOptionType SurgeOptionType { get; }
    }

    class WebbSurgeOption : AbsSurgeOption
    {
        public WebbSurgeOption(int surgeCost)
            : base(surgeCost)
        { }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            throw new NotImplementedException();
        }

        public override SpriteName Icon => SpriteName.cmdWebbed;

        public override string Name => "Web";

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new WebUnitAffectTooltip() };
        }

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Webb;

        //public override string Desc => "The unit cannot move, until it spends one attack action to cut free from the web";
    }

    class PierceSurgeOption : AbsSurgeOption
    {
        public PierceSurgeOption(int surgeCost, int pierceCount)
            : base(surgeCost, pierceCount)
        { }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            attack.attackRoll.attackRollResult.pierce += useCount * resultCount;
        }

        public override SpriteName Icon => SpriteName.cmdPierce;

        public override string Name => "Pierce " + resultCount.ToString();
        

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new PierceXTooltip() };
        }

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Pierce;
    }

    class DamageSurgeOption : AbsSurgeOption
    {
        public DamageSurgeOption(int surgeCost, int damage)
            : base(surgeCost, damage)
        { }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            attack.attackRoll.addHits(useCount * resultCount, false);
        }

        public override SpriteName Icon => SpriteName.cmdSlotMashineHit;

        public override string Name => "Damage " + resultCount.ToString();

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Damage;
    }

    class IncreaseMaxHealthSurgeOption : AbsSurgeOption
    {
        public IncreaseMaxHealthSurgeOption(int surgeCost, int increaseHealth)
            : base(surgeCost, increaseHealth)
        {
        }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            throw new NotImplementedException();
        }

        public override string Name => "Max health +" + resultCount.ToString();

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.IncreaseMaxHealth;
    }

    class RageSurgeOption : AbsSurgeOption
    {
        public RageSurgeOption(int surgeCost, int addRage)
            : base(surgeCost, addRage)
        {
        }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            attack.attackRoll.attacker.addBloodrage(useCount * resultCount);
            attack.options?.refreshAttackerStats();
        }

        public override SpriteName Icon => SpriteName.cmdBloodRage;

        public override string Name =>  LanguageLib.BloodRage + " " + resultCount.ToString();

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Rage;
    }

    class StaminaSurgeOption : AbsSurgeOption
    {
        public StaminaSurgeOption(int surgeCost, int addStamina)
            : base(surgeCost, addStamina)
        {
        }

        public override void onSurge(int useCount, AttackDisplay attack)
        {
            attack.attackRoll.attacker.addStamina(useCount * resultCount);
            attack.options?.refreshAttackerStats();
        }

        public override SpriteName Icon => SpriteName.cmdStamina;

        public override string Name => LanguageLib.Stamina + " " + resultCount.ToString();

        public override SurgeOptionType SurgeOptionType => SurgeOptionType.Stamina;
    }

    enum SurgeOptionType
    {
        Pierce,
        Damage,
        Rage,
        Stamina,
        IncreaseMaxHealth,
        Webb,
        Poision,
        Bleed,
    }
}
