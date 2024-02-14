using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class ArrowRain : AbsHeroStrategy
    {
        const int StrengthAdd = -2;

        public ArrowRain()
        {
            cardSprite = SpriteName.hqStrategyArrowRain;
            name = "Arrow rain";
            description = "Move and make a " + 
                StrengthAdd.ToString() +" strength attack, in a 3by3 grid area";

            groupAttack = true;
            staminaCost = 1;
            coolDownTurns = new ToggEngine.Data.CooldownCounter(2);
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        public override void collectBoardUiTargets(Unit heroUnit, List<IntVector2> boardUiTargets)
        {
            if (heroUnit.ableToProjectileAttack())
            {
                int fireRange = heroUnit.FireRangeWithModifiers(heroUnit.squarePos);

                Rectangle2 areaInRange = Rectangle2.FromCenterTileAndRadius(heroUnit.squarePos, fireRange);
                ForXYLoop arealoop = new ForXYLoop(areaInRange);
                while (arealoop.Next())
                {
                    List<AttackTarget> validTargets;
                    List<AttackTarget> group = collectAttackGroup(heroUnit, arealoop.Position, out validTargets);
                    if (validTargets.Count > 0)
                    {
                        boardUiTargets.Add(arealoop.Position);
                    }
                }
            }
        }

        public override List<AttackTarget> collectAttackGroup(Unit heroUnit, IntVector2 startPos, out List<AttackTarget> validTargets)
        {
            List<AttackTarget> result = new List<AttackTarget>();
            validTargets = new List<AttackTarget>();

            int fireRange = heroUnit.FireRangeWithModifiers(heroUnit.squarePos);
            if (!heroUnit.lockedInMelee() &&
                heroUnit.InRangeAndSight(startPos, fireRange, false, false))
            {
                ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(startPos, 1));
                while (loop.Next())
                {
                    tryAddTargetToGroup(loop.Position, heroUnit, startPos, result, validTargets, false);
                }
            }
            return result;
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return setup.targets.IsProjectile;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.AttackerSetup.modAttackStrength(StrengthAdd);
        }

        public override void modLabel(ToggEngine.BattleEngine.BattleModifierLabel label)
        {
            label.modSource(this);
            label.attackModifier(StrengthAdd);
        }

        //public override void battleModifiers(BattleSetup coll)
        //{
        //    if (coll.targets.IsProjectile)
        //    {
        //        coll.AttackerSetup.modAttackStrength(StrengthAdd);

        //        var label = coll.attackerSetup.beginModLabel();
        //        label.modSource(this);
        //        label.attackModifier(StrengthAdd);
        //    }
        //    base.battleModifiers(coll);
        //}

        protected override int modifiedAttackStrength(int baseStrength)
        {
            return baseStrength + StrengthAdd;
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.ArrowRain; } }
    }
}
