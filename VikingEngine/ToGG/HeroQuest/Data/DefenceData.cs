using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest
{
    class DefenceData
    {
        public List<BattleDice> dice = null;
        public List<BattleDiceModification> modifications = null;

        public float value;
        public int directDamageResist;

        public void clear()
        {
            dice?.Clear();
        }

        public void set(BattleDice type, int count = 1)
        {
            dice = new List<BattleDice>(count);
            for (int i = 0; i < count; ++i)
            {
                dice.Add(type);
            }

            refresh();
        }

        public void add(BattleDice type, int count = 1)
        {
            for (int i = 0; i < count; ++i)
            {
               arraylib.AddOrCreate(ref dice, type);
            }

            refresh();
        }

        void refresh()
        {
            value = 0;
            directDamageResist = 0;

            foreach (var m in dice)
            {
                value += m.value;

                if (m.type == BattleDiceType.HeavyArmor)
                {
                    directDamageResist += 1;
                }
            }
        }

        DefenceData Instance()
        {
            DefenceData clone = new DefenceData();

            if (this.dice != null)
            {
                clone.dice = new List<BattleDice>(this.dice.Count + 2);
                for (int i = 0; i < this.dice.Count; ++i)
                {
                    clone.dice.Add(this.dice[i].Clone());
                }
            }

            clone.directDamageResist = directDamageResist;
            clone.value = value;

            return clone;
        }

        public DefenceData collectDefence(Unit attacker, Unit defender, 
            AttackType attackType, bool onCommit)
        {
            DefenceData result;

            if (defender.condition.GetBase(Data.Condition.BaseCondition.Defenseless) != null)
            {
                result = new DefenceData();
            }
            else
            {
                result = Instance();
                if (defender.data.hero != null)
                {
                    defender.PlayerHQ.collectGadgetDefence(defender, result, onCommit);
                }
                defender.data.properties.collectDefence(result, onCommit);

                if (attacker != null)
                {
                    attacker.data.properties.collectOpponentDefence(result, onCommit);
                }

                if (result.dice != null)
                {
                    foreach (var die in result.dice)
                    {
                        if (result.modifications != null)
                        {
                            foreach (var mod in result.modifications)
                            {
                                mod.ApplyTo(die);
                            }
                        }

                        setDisabledDieResults(die, attackType);
                    }
                }
            }

            return result;
        }

        void setDisabledDieResults(BattleDice die, AttackType attackType)
        {
            for (int i = 0; i < die.sides.Count; ++i)
            {
                var m = die.sides[i];
                if (m.result == BattleDiceResult.AvoidRanged)
                {
                    m.enabled = attackType.IsRanged;
                }
                else if (m.result == BattleDiceResult.AvoidLongRange)
                {
                    m.enabled = attackType.Is(AttackUnderType.LongRange);
                }

                die.sides[i] = m;
            }
        }

        public void checkDamageResistance(ref Damage damage)
        {
            if (damage.applyType == DamageApplyType.Direct)
            {
                damage.Value -= directDamageResist;
            }
        }

        public BattleDice[] Array()
        {
            if (dice == null)
            {
                return new BattleDice[0];
            }
            return dice.ToArray();
        }

        public void write(System.IO.BinaryWriter w)
        {
            if (arraylib.HasMembers(dice))
            {
                w.Write((byte)dice.Count);
                foreach (var m in dice)
                {
                    w.Write((byte)m.type);
                }
            }
            else
            {
                w.Write(byte.MinValue);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            int count = r.ReadByte();
            if (count > 0)
            {
                dice = new List<BattleDice>(count);
                for (int i = 0; i < count; ++i)
                {
                    dice.Add(BattleDice.Get((BattleDiceType)r.ReadByte()));
                }
            }
            else
            {
                dice = null;
            }
        }

        public int Count => arraylib.SafeCount(dice);
    }
}
