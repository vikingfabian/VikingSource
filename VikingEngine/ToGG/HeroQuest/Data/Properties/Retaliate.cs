using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class Parry : Retaliate
    {
        public Parry()
        { }

        public Parry(int value)
            :base(value)
        { }

        public override void onAttackEnded(Unit unit, AttackDisplay attack, bool attacker)
        {
            if (!attacker)
            {
                var target = attack.attackRoll.targets.Get(unit);
                if (target != null &&
                    (target.recordedDamage == null || target.recordedDamage.DamageRecieved == 0))
                {
                    base.onAttackEnded(unit, attack, attacker);
                }
            }
        }

        public override SpriteName Icon => SpriteName.cmdParry;
        
        public override string Desc => "Defending against a Melee attacks that don't damage: gains Retaliate " +
            damage.value.BellValueToString();

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new RetaliateXTooltip() };
        }

        public override UnitPropertyType Type => UnitPropertyType.Parry;
    }

    class HeavyShield : AbsUnitProperty
    {
        public int shieldCount;

        public HeavyShield(int shieldCount)
        {
            this.shieldCount = shieldCount;
        }

        public override void collectDefence(DefenceData defence, bool onCommit)
        {
            base.collectDefence(defence, onCommit);

            defence.add(hqLib.HeavyArmorDie, shieldCount);
        }

        public override SpriteName Icon => SpriteName.cmdDiceArmorHeavy;

        public override string Name => "Heavy Shield " + shieldCount.ToString();

        public override string Desc => "Gain " + shieldCount.ToString() + " " + LanguageLib.DefenceHeavyArmor;

        public override UnitPropertyType Type => UnitPropertyType.HeavyShield;
    }

    class Retaliate : AbsUnitProperty
    {
        protected Damage damage;

        public Retaliate()
        { }

        public Retaliate(int value)
        {
            damage = Damage.BellDamage(value);
        }

        public override void onAttackEnded(HeroQuest.Unit unit, AttackDisplay attack, bool attacker)
        {
            base.onAttackEnded(unit, attack, attacker);

            if (!attacker)
            {
                if (unit.canTargetUnit(attack.attackRoll.attacker) &&
                    unit.InMeleeRange(attack.attackRoll.attacker.squarePos))
                {
                    new PerformRetaliate(unit, attack.attackRoll.attacker, this,
                        damage.NextDamage());
                }
            }
        }

        public override void writeData(BinaryWriter w)
        {
            base.writeData(w);
            w.Write((byte)damage.Value);
        }

        public override void readData(BinaryReader r)
        {
            base.readData(r);
            damage = Damage.BellDamage(r.ReadByte());
        }

        public override SpriteName Icon => SpriteName.cmdRetaliate;

        public override string Name => "Retaliate " + damage.value.BellValueToString();

        public override string Desc => LanguageLib.RetaliateDesc + " " + damage.description();

        protected override int Level => damage.Value;

        public override UnitPropertyType Type => UnitPropertyType.Retaliate;
    }

    class PerformRetaliate : AbsPerformUnitAction
    {
        Damage damage;
        

        public PerformRetaliate(System.IO.BinaryReader r)
            : base(r)
        { }

        public PerformRetaliate(AbsUnit parentUnit, AbsUnit target,
            AbsProperty parentAction, Damage damage)
            : base(parentUnit, parentAction)
        {
            this.damage = damage;
            this.target = target;
            lockTargetInAnimation(target);
        }

        public override void onBegin()
        {
            sayAction();
        }

        public override bool update()
        {
            base.update();

            if (timeStamp.event_ms(400))
            {
                spectatorPos = target.squarePos;

                if (isLocalAction)
                {
                    RecordedDamageEvent rec;
                    target.hq().TakeDamage(damage, DamageAnimationType.AttackSlash, null, out rec);

                    rec.NetSend();
                }
                //sendStatusEffect(target, new StatusEffect.Grappled());

                return true;
            }

            return false;
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);

            target.hq().netWriteUnitId(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            target = HeroQuest.Unit.NetReadUnitId(r);
        }

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.PerformRetaliate;
    }
}
