using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    abstract class AbsExtToolTip
    {
        virtual public SpriteName Icon => SpriteName.NO_IMAGE;
        abstract public string Title { get; }
        abstract public string Text { get; }

        virtual public bool EqualType(AbsExtToolTip obj)
        {
            return this.GetType() == obj.GetType();
        }

        virtual public AbsExtToolTip[] DescriptionKeyWords() { return null; } 
    }

    class PropertyTooltip : AbsExtToolTip
    {
        AbsProperty property;
        AbsExtToolTip[] descriptionKeyWords = null;
        public PropertyTooltip(AbsProperty property)
        {
            this.property = property;
            descriptionKeyWords = property.ExtendedTooltipKeyWords();
        }

        public override SpriteName Icon => property.Icon;
        public override string Title => property.Name;
        public override string Text => property.Desc;

        public override bool EqualType(AbsExtToolTip obj)
        {
            var pObj = obj as PropertyTooltip;
            return pObj != null && pObj.property.EqualType(this.property);
        }

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return descriptionKeyWords;
        }
    }

    class StatusEffectTooltip : AbsExtToolTip
    {
        AbsCondition status;
        public StatusEffectTooltip(AbsCondition status)
        {
            this.status = status;
        }

        public override SpriteName Icon => status.Icon;
        public override string Title => status.Name;
        public override string Text => "Status effect: " + status.Desc;

        public override bool EqualType(AbsExtToolTip obj)
        {
            var pObj = obj as StatusEffectTooltip;
            return pObj != null && pObj.status.EqualType(this.status);
        }
    }

    class ArmorDicetip : AbsExtToolTip
    {
        BattleDice dice;
        public ArmorDicetip(BattleDice dice)
        {
            this.dice = dice;
        }

        public override SpriteName Icon => dice.icon;
        public override string Title => dice.name;
        public override string Text
        {
            get
            {
                var result = "Is rolled as defensive dice when attacked.";
                if (dice.type == BattleDiceType.HeavyArmor)
                {
                    result += " Blocks 1 direct damage.";
                }
                return result;
            }
        }

        public override bool EqualType(AbsExtToolTip obj)
        {
            var pObj = obj as ArmorDicetip;
            return pObj != null && pObj.dice.type == this.dice.type;
        }
    }

    abstract class AbsValuebarTooltip : AbsExtToolTip
    {
        abstract public SpriteName IconGrayed { get; }

        public override bool EqualType(AbsExtToolTip obj)
        {
            return this.GetType() == obj.GetType();
        }
    }

    class HealthTooltip : AbsValuebarTooltip
    {
        public override SpriteName Icon => SpriteName.cmdStatsHealth;

        public override SpriteName IconGrayed => SpriteName.cmdHeartGrayed;
        public override string Title => LanguageLib.Health;
        public override string Text => "How much damage the unit can recieve before she is destroyed";
    }

    class StaminaTooltip : AbsValuebarTooltip
    {
        public override SpriteName Icon => SpriteName.cmdStamina;
        public override SpriteName IconGrayed => SpriteName.cmdStaminaGrayed;
        public override string Title => LanguageLib.Stamina;
        public override string Text =>
            "Some actions require that you spend stamina. Can be used to increase attack or take extra movement steps.";
    }

    class BloodRageTooltip : AbsValuebarTooltip
    {
        public override SpriteName Icon => SpriteName.cmdBloodRage;
        public override SpriteName IconGrayed => SpriteName.cmdBloodrageGrayed;
        public override string Title => LanguageLib.BloodRage;
        public override string Text => "Recieve rage from kills. A full bar allows the hero to use her Ultimate.";
    }

    class SpecialActionTooltip : AbsExtToolTip
    {
        public override string Title => "Special Action";
        public override string Text => "The unit may use this action instead of attacking or moving";
    }

    class PushXTooltip : AbsExtToolTip
    {
        public const string PushDesc = "Moves the defender away from the attacker";

        public override string Title => "Push X";
        public override string Text => PushDesc;
    }

    class ThrowXTooltip : AbsExtToolTip
    {
        public override string Title => "Throw X";
        public override string Text => LanguageLib.FormatSentences(
            PushXTooltip.PushDesc, "Will fly over ground obsticles");
    }

    class RetaliateXTooltip : AbsExtToolTip
    {
        public override string Title => "Retaliate X";
        public override string Text => LanguageLib.RetaliateDesc;
    }

    class PierceXTooltip : AbsExtToolTip
    {
        public override string Title => "Pierce X";
        public override string Text => "Reduces the block result from the defender's armor.";
    }

    class NetTooltip : AbsExtToolTip
    {
        public override SpriteName Icon => SpriteName.WebTile;
        public override string Title => "Net";
        public override string Text => "A hero must spend 1 stamina to leave a square with net.";
    }

    class WebUnitAffectTooltip : AbsExtToolTip
    {
        public override SpriteName Icon => SpriteName.cmdWebbed;
        public override string Title => "Web";
        public override string Text => "Unit is "+ Immobile.ImmobileName + 
            ", until it spends one attack action to cut free from the web.";
        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new ImmobileAffectTooltip() };
        }
    }

    class ImmobileAffectTooltip : AbsExtToolTip
    {
        //public const string ImmobileName = Immobile.ImmobileName;

        public override SpriteName Icon => Immobile.ImmobileIcon; 

        public override string Title => Immobile.ImmobileName;
        public override string Text => Immobile.CantMoveDesc;
    }

    class ImmobileConditionTooltip : AbsExtToolTip
    {
        //public const string ImmobileName = Immobile.ImmobileName;
        
        public override SpriteName Icon => Immobile.ImmobileIcon;

        public override string Title => Immobile.ImmobileName + " X";
        public override string Text => Immobile.ImmobileDesc();
    }

    class DefencelessAffectTooltip : AbsExtToolTip
    {
        public const string DefenselessName = "Defenceless";

        public override string Title => DefenselessName;
        public override string Text => "The unit is passive when taking damage";
    }

    

    
}
