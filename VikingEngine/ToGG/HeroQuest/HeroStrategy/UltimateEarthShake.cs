using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class UltimateEarthShake : AbsHeroStrategy
    {
        const int StrengthAdd = 2;

        public UltimateEarthShake()
        {
            cardSprite = SpriteName.hqStrategyEarthShake;
            name = UnltimateTitle;

            description = "Move once. Make one " + TextLib.ValuePlusMinus(StrengthAdd) + 
                " attack. Units around the defender will be stunned.";
            useBloodRage = true;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return true;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.AttackerSetup.modAttackStrength(StrengthAdd);
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(this);
            label.attackModifier(StrengthAdd);
        }

        //public override void battleModifiers(BattleSetup coll)
        //{
        //    coll.AttackerSetup.modAttackStrength(StrengthAdd);

        //    var label = coll.attackerSetup.beginModLabel();
        //    label.modSource(this);
        //    label.attackModifier(StrengthAdd);

        //    base.battleModifiers(coll);
        //}

        public override void onAttackCommit(AttackDisplay attack)
        {
            IntVector2 center = attack.attackRoll.defenders.First.target.position;

            var stunTargets = ToggEngine.Map.AreaFilter.AdjacentUnits(center, ToggEngine.Data.PlayerFilter.Bad);
            foreach (var m in stunTargets)
            {
                m.hq().condition.Set(Data.Condition.ConditionType.Stunned, 1, false, true, true);
            }
        }

        protected override int modifiedAttackStrength(int baseStrength)
        {
            return baseStrength + StrengthAdd;
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.UltimateEarthShake; } }
    }
}
