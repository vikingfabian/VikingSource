using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.HeroQuest.Data.Condition;
using VikingEngine.ToGG.ToggEngine.BattleEngine;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    struct Buff
    {
        public static Buff Empty = new Buff();

        public int movement;
        public int attackDice;

        public void Add(Buff add)
        {
            movement += add.movement;
            attackDice += add.attackDice;
        }
        
        public List<AbsUnitStatus> properties()
        {
            List<AbsUnitStatus> properties = null;

            if (movement != 0)
            {
                arraylib.AddOrCreate(ref properties, new BuffProperty(BuffPropertyType.Move, movement));
            }

            if (attackDice != 0)
            {
                arraylib.AddOrCreate(ref properties, new BuffProperty(BuffPropertyType.Attack, attackDice));
            }

            return properties;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((sbyte)movement);
            w.Write((sbyte)attackDice);
        }

        public void read(System.IO.BinaryReader r)
        {
            movement = r.ReadSByte();
            attackDice = r.ReadSByte();
        }

        public override string ToString()
        {
            return "Buff: move" + movement.ToString() + ", attack" + attackDice.ToString();
        }
    }

    class BuffProperty : Condition.AbsUnitStatus
    {
        BuffPropertyType type;
        int value;

        public BuffProperty(BuffPropertyType type, int value)
        {
            this.type = type;
            this.value = value;
        }

        public override SpriteName Icon
        {
            get
            {
                switch (type)
                {
                    case BuffPropertyType.Attack:
                        return value > 0 ? SpriteName.cmdAttackUp : SpriteName.cmdAttackDown;

                    case BuffPropertyType.Move:
                        return value > 0 ? SpriteName.cmdMoveUp : SpriteName.cmdMoveDown;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public override string Name
        {
            get
            {
                string name;

                switch (type)
                {
                    case BuffPropertyType.Attack:
                        name = LanguageLib.BattleDie;
                        break;

                    case BuffPropertyType.Move:
                        name = "Move length";
                        break;

                    default:
                        name = TextLib.Error;
                        break;
                }

                return (value > 0 ? "Buff" : "Debuff") + ": " + 
                    TextLib.ValuePlusMinus(value) + " " + name;
            }
        }

        public override string Desc => "Buff/Debuff";

        public override int StatusIsPositive => value;

        public override BattleModificationType ModificationType => throw new NotImplementedException();
        public override int ModificationUnderType => throw new NotImplementedException();
    }

    class UnitBuffsMember : AbsBattleModification
    {
        public Buff buff;
        public AbsUnitProperty source;
        public bool friendly;
        
        public void Set(Buff buff, AbsUnitProperty source, bool friendly)
        {
            this.buff = buff;
            this.source = source;
            this.friendly = friendly;
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return true;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.attackerSetup.attackStrength += buff.attackDice;
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(source);
            label.attackModifier(buff.attackDice);
        }

        public override void netWriteMod(BinaryWriter w)
        {
            buff.write(w);
            source.writeUnitPropertyType(w);
        }

        public override void netReadMod(BinaryReader r)
        {
            buff.read(r);
            source = AbsUnitProperty.ReadUnitPropertyType(r);
        }

        public override string ToString()
        {
            return "UnitBuffsMember " + buff.ToString() + ", source (" + source.Name + ")";
        }

        public override BattleModificationType ModificationType => BattleModificationType.UnitBuffsMember;
        override public int ModificationUnderType { get { return 0; } }
    }

    enum BuffPropertyType
    {
        Attack,
        Move,
    }
}
