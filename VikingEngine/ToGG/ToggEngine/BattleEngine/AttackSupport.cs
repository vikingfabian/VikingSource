using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class AttackSupport
    {
        public int total;
        public List<AttackSupportMember> units;

        public AttackSupport()
        {
            total = 0;
            units = new List<AttackSupportMember>();
        }

        public void add(BattleEngine.AttackType attack, AbsUnit unit)
        {
            int givesSupport = 1;
            if (attack.IsNot(AttackUnderType.BackStab))// != Battle.AttackType.Backstab)
            {
                if (unit.HasProperty(UnitPropertyType.Flank_support))
                {
                    givesSupport = 2;
                }
            }
            total += givesSupport;
            units.Add(new AttackSupportMember(unit, givesSupport));
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)units.Count);
            foreach (var m in units)
            {
                m.unit.writeIndex(w);
            }
        }

        public void read(System.IO.BinaryReader r, BattleEngine.AttackType attack)
        {
            int unitsCount = r.ReadByte();
            for (int i = 0; i < unitsCount; ++i)
            {
                AbsUnit unit = toggRef.gamestate.GetUnit(r);

                if (unit != null)
                {
                    add(attack, unit);
                }
            }
        }
    }

    class AttackSupportMember
    {
        public int support;
        public AbsUnit unit;

        public AttackSupportMember(AbsUnit unit, int support)
        {
            this.support = support;
            this.unit = unit;
        }
    }
}
