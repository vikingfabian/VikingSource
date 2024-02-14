using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    class RunAndHide : Run
    {
        public RunAndHide()
        {
            name = "Run and hide";
            description += " Gain: " + Gadgets.SmokeBomb.SmokeBombName;
            coolDownTurns = new ToggEngine.Data.CooldownCounter(1);
        }

        public override void payForStrategy(bool pay_notReturn, Unit heroUnit)
        {
            base.payForStrategy(pay_notReturn, heroUnit);

            if (pay_notReturn)
            {
                heroUnit.PlayerHQ.add(new Gadgets.SmokeBomb(), true, true);
            }
            else
            {
                heroUnit.PlayerHQ.Backpack().RemoveItem(new Gadgets.ItemFilter(
                    Gadgets.ItemMainType.Potion, (int)Gadgets.PotionType.SmokeBomb), 1);
            }
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.RunAndHide; } }
    }

    class FromTheShadows : Advance
    {
        const int HiddenBonus = 2;
        bool hidden = false;
        public FromTheShadows()
        {
            name = "From the shadows";
            description = LanguageLib.FormatSentences(description, 
                "If hidden at turn start: Gain +" + HiddenBonus.ToString() + LanguageLib.BattleDie);
        }
        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            base.ApplyToHero(heroUnit, commit);
            hidden = heroUnit.condition.Get(Data.Condition.ConditionType.Hidden) != null;
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return hidden;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.AttackerSetup.modAttackStrength(HiddenBonus);
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(this);
            label.attackModifier(HiddenBonus);
        }

        //public override void battleModifiers(BattleSetup coll)
        //{
        //    if (hidden)
        //    {
        //        coll.AttackerSetup.modAttackStrength(HiddenBonus);

        //        var label = coll.attackerSetup.beginModLabel();
        //        label.modSource(this);
        //        label.attackModifier(HiddenBonus);
        //    }
        //    base.battleModifiers(coll);
        //}

        protected override int modifiedAttackStrength(int baseStrength)
        {
            if (hidden)
            {
                return baseStrength + HiddenBonus;
            }
            else
            {
                return base.modifiedAttackStrength(baseStrength);
            }
        }

        public override HeroStrategyType Type => HeroStrategyType.FromTheShadows;
    }

    class KnifeDance : AbsHeroStrategy
    {
        const int StrengthAdd = -1;

        public KnifeDance()
        {   
            cardSprite = SpriteName.hqStrategyKnifeDance;
            name = "Knife dance";
            description = "Move and make a " +
                StrengthAdd.ToString() + " strength attack, in all eight directions.";

            groupAttack = true;
            staminaCost = 2;
            coolDownTurns = new ToggEngine.Data.CooldownCounter(2); 
        }

        public override void specialRequirements(AbsUnit unit, List<AbsRichBoxMember> richbox)
        {
            richbox.Add(new RichBoxNewLine(false));
            richbox.Add(new RichBoxImage(SpriteName.cmdThiefDaggersTier1));
            richbox.Add(new RichBoxText(Gadgets.AbsWeapon.MixedMeleeProjecSpriteName, 
                hasWeaponRequirement(unit) ? Color.White : HudLib.UnavailableRedCol));            
        }

        bool hasWeaponRequirement(AbsUnit unit)
        {
            var mainWeapon = unit.hq().data.hero.equipment.mainhand.item as Gadgets.AbsWeapon;
            return mainWeapon != null && 
                mainWeapon.stats.weaponRange() == WeaponRange.MixedMeleeProjectile;
        }

        public override List<StrategyCardResource> Resources(object tag, out bool available)
        {
            var result = base.Resources(tag, out available);
            available &= hasWeaponRequirement((Unit)tag);

            return result;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        public override void collectBoardUiTargets(Unit heroUnit, List<IntVector2> boardUiTargets)
        {
            if (hasWeaponRequirement(heroUnit))
            {               
                List<AttackTarget> validTargets;
                List<AttackTarget> group = collectAttackGroup(heroUnit, IntVector2.NegativeOne, out validTargets);

                foreach (var m in validTargets)
                {
                    boardUiTargets.Add(m.position);
                }
            }
        }

        public override List<AttackTarget> collectAttackGroup(Unit heroUnit, IntVector2 startPos, 
            out List<AttackTarget> validTargets)
        {
            bool foundTarget = false;
            List<AttackTarget> result = new List<AttackTarget>();
            validTargets = new List<AttackTarget>();

            int fireRange = heroUnit.FireRangeWithModifiers(heroUnit.squarePos);
            //if (heroUnit.InRangeAndSight(startPos, fireRange))
            {
                foreach (var dir in IntVector2.Dir8Array)
                {
                    for (int dist = 1; dist <= fireRange; ++dist)
                    {
                        var pos = heroUnit.squarePos + dir * dist;

                        var sq = toggRef.Square(pos);
                        if (sq == null || sq.BlocksLOS())
                        {
                            break;
                        }
                        else
                        {
                            //var target = sq.unit;
                            if (sq.unit != null && heroUnit.canTargetUnit(sq.unit))
                            {
                                var attack = new AttackTarget(sq.unit, dist == 1);
                                validTargets.Add(attack);
                                result.Add(attack);

                                if (pos == startPos)
                                { foundTarget = true; }

                                break;
                            }
                            else
                            {
                                result.Add(new AttackTarget(pos));
                            }
                        }
                    }
                }
            }

            if (startPos != IntVector2.NegativeOne &&
                !foundTarget)
            {
                result.Clear();
                validTargets.Clear();
            }
            else if (validTargets.Count == 0)
            {
                result.Clear();
            }
            
            return result;
            
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return setup.targets.Count > 1;
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
        //    if (coll.targets.Count > 1)
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

        override public HeroStrategyType Type { get { return HeroStrategyType.KnifeDance; } }
    }

    class Trapper : AbsHeroStrategy
    {
        RemoveTrap removeTrapAction = new RemoveTrap();

        public Trapper()
        {
            cardSprite = SpriteName.hqStrategyTrapper;
            name = "Trapper";
            description = "Move. Gain items: " + RougeTrap.Name + " and " + Data.TrapDecoy.TrapDecoyName +
                ". Gain action: " + removeTrapAction.Name + ". No attacks.";
            coolDownTurns = new ToggEngine.Data.CooldownCounter(2);
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            removeTrapAction.useCount.reset();
            setMoveAttackCount(heroUnit, 1, 0);
        }

        public override void payForStrategy(bool pay_notReturn, Unit heroUnit)
        {
            base.payForStrategy(pay_notReturn, heroUnit);

            if (pay_notReturn)
            {
                heroUnit.PlayerHQ.add(new Gadgets.RougeTrapItem(), true, true);
                heroUnit.PlayerHQ.add(new Gadgets.TrapDecoy(), true, true);
            }
            else
            {
                heroUnit.PlayerHQ.Backpack().RemoveItem(new Gadgets.ItemFilter(
                    Gadgets.ItemMainType.Spawn, (int)Gadgets.ItemSpawnType.RougeTrap), 1);
                heroUnit.PlayerHQ.Backpack().RemoveItem(new Gadgets.ItemFilter(
                   Gadgets.ItemMainType.Spawn, (int)Gadgets.ItemSpawnType.TrapDecoy), 1);
            }
        }

        public override void collectActions(List<AbsUnitAction> unitActions)
        {
            base.collectActions(unitActions);
            unitActions.Add(removeTrapAction);
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.Trapper; } }
    }

    abstract class AbsConditionBomb : AbsHeroStrategy
    {
        public Data.Condition.ConditionType condition;
        ThrowConditionBomb throwAction;
        public string actionDesc;
        public AbsConditionBomb(Data.Condition.ConditionType condition)
        {
            this.condition = condition;
            name = condition + " bomb";

            actionDesc = "Throw a 3x3 area bomb. Gives all enemies " + condition.ToString();
            description = "Move. " + actionDesc;
            coolDownTurns = new ToggEngine.Data.CooldownCounter(2);

            throwAction = new ThrowConditionBomb(this);
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            throwAction.useCount.reset();
            setMoveAttackCount(heroUnit, 1, 0);
        }

        public override void collectActions(List<AbsUnitAction> unitActions)
        {
            base.collectActions(unitActions);
            unitActions.Add(throwAction);
        }
    }

    class PoisionBomb : AbsConditionBomb
    {
        public PoisionBomb()
            : base(Data.Condition.ConditionType.Poision)
        {
            cardSprite = SpriteName.hqStrategyPoisionBomb;
        }
        override public HeroStrategyType Type { get { return HeroStrategyType.PoisionBomb; } }
    }

    class StunBomb : AbsConditionBomb
    {
        public StunBomb()
            : base(Data.Condition.ConditionType.Stunned)
        {
            cardSprite = SpriteName.hqStrategyStunBomb;
        }
        override public HeroStrategyType Type { get { return HeroStrategyType.StunBomb; } }
    }



    class UltimateLootrun : AbsHeroStrategy
    {
        LootX lootAction = new LootX(6);

        public UltimateLootrun()
        {
            cardSprite = SpriteName.hqStrategyLootRun;
            name = UnltimateTitle;
            description = "Move x3. Gain action: " + lootAction.Name;
            useBloodRage = true;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            lootAction.useCount.reset();
            setMoveAttackCount(heroUnit, 3, 0);
        }

        public override void collectActions(List<AbsUnitAction> unitActions)
        {
            base.collectActions(unitActions);
            unitActions.Add(lootAction);
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.UltimateLootrun; } }
    }
}
