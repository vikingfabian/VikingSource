using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    struct AttackType
    {
        public static readonly AttackType None = new AttackType(AttackMainType.NONE, AttackUnderType.None);
        public static readonly AttackType Melee = new AttackType(AttackMainType.Melee, AttackUnderType.None);
        public static readonly AttackType Ranged = new AttackType(AttackMainType.Ranged, AttackUnderType.None);
        public static readonly AttackType CounterAttack = new AttackType(AttackMainType.Melee, AttackUnderType.CounterAttack);

        public AttackMainType mainType;
        public AttackUnderType underType;

        public AttackType(AttackMainType mainType, AttackUnderType underType)
        {
            this.mainType = mainType;
            this.underType = underType;
        }

        public AttackType(System.IO.BinaryReader r)
            :this()
        {
            read(r);
        }

        public bool Is(AttackUnderType isType)
        {
            return this.underType == isType;
        }

        public bool IsNot(AttackUnderType isType)
        {
            return this.underType != isType;
        }

        public bool Is(AttackMainType isType)
        {
            return this.mainType == isType;
        }

        public bool Is(bool isMelee, AttackUnderType isUType)
        {
            return mainType != AttackMainType.Ranged && this.underType == isUType;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)mainType);
            w.Write((byte)underType);
        }
        public void read(System.IO.BinaryReader r)
        {
            mainType = (AttackMainType)r.ReadByte();
            underType = (AttackUnderType)r.ReadByte();
        }

        public override string ToString()
        {
            switch (underType)
            {
                case AttackUnderType.BackStab:
                    return "Back stab";
                case AttackUnderType.CounterAttack:
                    return "Counter attack";
                case AttackUnderType.LongRange:
                    return "Long range attack";
                default:
                    switch (mainType)
                    {
                        case AttackMainType.Melee:
                            return "Melee combat";
                        case AttackMainType.Mixed:
                            return "Mixed combat";
                        case AttackMainType.Ranged:
                            return "Ranged attack";
                    }
                    break;
            }
            return "ERR";
        }

        public bool IsMelee { get { return mainType != AttackMainType.Ranged; } }
        public bool IsRanged { get { return mainType != AttackMainType.Melee; } }

    }

    enum AttackMainType
    {
        Melee,
        Ranged,
        Mixed,
        NONE,
    }

    enum AttackUnderType
    {
        None,
        CounterAttack,
        BackStab,
        LongRange,
    }
}
