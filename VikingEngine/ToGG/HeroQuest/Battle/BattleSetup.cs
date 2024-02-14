using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.HeroQuest.Data.Condition;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest
{
    class BattleAttackerSetup : AbsBattleAttackerSetup
    {
        public int pierce = 0;
        public List<Data.UnitBuffsMember> attackerBuffs;
        public List<ConditionType> defendersConditions = new List<ConditionType>();

        public void calcAttackCount(BattleSetup setup, Unit attacker, AttackTargetGroup targets,
            StaminaAttackBoost staminaAttackBoost)
        {
            baseAttackStrength = attacker.data.WeaponStats.Strength(targets.IsMelee);

            attackStrength = baseAttackStrength;

            if (attacker.data.hero != null &&
                attacker.data.hero.availableStrategies.active.HasBattleModifier(setup, true))
            {
                modifications.Add(attacker.data.hero.availableStrategies.active);
            }

            if (targets.HasMods)
            {
                modifications.AddRange(targets.attackSettings.modifications);
            }

            foreach (var m in targets)
            {
                if (m.unit != null)
                {
                    //m.unit.hq().condition.attackCountModifiers(setup, false);

                    var statuses = m.unit.hq().condition.conditions.counter();
                    while (statuses.Next())
                    {
                        if (statuses.sel.HasBattleModifier(setup, false))
                        {
                            modifications.Add(statuses.sel);
                        }
                    }
                }
            }

            attackerBuffs = attacker.condition.buffs.listBuffs(true);
            modifications.AddRange(attackerBuffs);
            
            foreach (var mod in modifications)
            {
                mod.applyMod(setup);
                mod.modLabel(beginModLabel());
            }

           attackStrength += staminaAttackBoost.boostCount;
        }

        public BattleDice[] attackDiceArray(Unit attacker)
        {
            var dice = hqLib.BattleDie.Clone();

            if (attacker.HasProperty(UnitPropertyType.Dull_Weapon))
            {
                dice.removeSide(BattleDiceResult.CriticalHit);
            }

            var diceArray = arraylib.ValueArray(dice, attackStrength);

            return diceArray;
        }

        public override void write(BinaryWriter w)
        {
            //Debug.Log("BATTLE setup netwrite " + modifications.Count.ToString());

            base.write(w);
            w.Write((byte)modifications.Count);
            foreach (var mod in modifications)
            {
                //Debug.Log(mod.ToString());
                AbsBattleModification.NetWrite(mod, w);
            }
        }

        public override void read(BinaryReader r)
        {
            base.read(r);
            int modificationsCount = r.ReadByte();
            for (int i = 0; i < modificationsCount; ++i)
            {
                var mod = AbsBattleModification.NetRead(r);
                mod.modLabel(beginModLabel());
                modifications.Add(mod);
            }
        }
    }

    class BattleDefenderSetup : AbsBattleDefenderSetup
    {
        public DefenceData defDice;
        public void calcDefences(Unit attacker, Unit defender, AttackType attackType)
        {
            defDice = defender.data.defence.collectDefence(attacker, defender, attackType, false);
        }
    }

    class BattleSetup : AbsBattleSetup
    {
        public BattleAttackerSetup attackerSetup;
        public Dictionary<int, BattleDefenderSetup> defendersSetup;

        public BattleSetup(List<ToggEngine.GO.AbsUnit> attacker, AttackTargetGroup targets, 
            StaminaAttackBoost staminaAttackBoost, bool local)
            : base(attacker, targets)
        {
            this.attacker = attacker;

            if (local)
            {
                attackerSetup = new BattleAttackerSetup();
                attackerSetup.calcAttackCount(this, MainAttacker.hq(), targets, staminaAttackBoost);

                defendersSetup = new Dictionary<int, BattleDefenderSetup>(targets.Count);
                foreach (var m in targets)
                {
                    if (m.unit != null)
                    {
                        BattleDefenderSetup def = new BattleDefenderSetup();
                        def.calcDefences(MainAttacker.hq(), m.unit.hq(), targets.AttackType);

                        defendersSetup.Add(m.unit.UnitId, def);
                    }
                }
            }
        }

        public BattleDefenderSetup defenderSetup(ToggEngine.GO.AbsUnit unit)
        {
            BattleDefenderSetup result = null;
            defendersSetup.TryGetValue(unit.UnitId, out result);

            return result;
        }

        public void write(System.IO.BinaryWriter w)
        {
            attackerSetup.write(w);

            w.Write((byte)defendersSetup.Count);
            foreach (var kv in defendersSetup)
            {
                w.Write((ushort)kv.Key);
                kv.Value.defDice.write(w);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            attackerSetup = new BattleAttackerSetup();
            attackerSetup.read(r);

            int defendersSetupCount = r.ReadByte();
            defendersSetup = new Dictionary<int, BattleDefenderSetup>(defendersSetupCount);
            for (int i = 0; i < defendersSetupCount; ++i)
            {
                BattleDefenderSetup def = new BattleDefenderSetup();
                def.defDice = new DefenceData();

                int key = r.ReadUInt16();
                def.defDice.read(r);

                defendersSetup.Add(key, def);
            }
        }

        public override AbsBattleAttackerSetup AttackerSetup => attackerSetup;
    }
}
