using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PSoldierGroup : IEquatable<PSoldierGroup>
    {
        public PArmy parmy;
        public int groupIndex;
        public static readonly PSoldierGroup Empty = new PSoldierGroup();

        public PSoldierGroup()
        {
            parmy = PArmy.Empty;
            groupIndex = -1;
        }
        public PSoldierGroup(PArmy parmy, int index)
        {
            this.parmy = parmy;
            groupIndex = index;
        }

        public PSoldierGroup(System.IO.BinaryReader r)
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            parmy.write(w);
            if (groupIndex < 0)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)groupIndex);
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            parmy.read(r);
            groupIndex = r.ReadUInt16();
            if (groupIndex == ushort.MaxValue)
            {
                groupIndex = -1;
            }
        }

        public SoldierGroup GetSoldierGroup()
        {
            var army = parmy.GetArmy();
            if (army != null)
            {
                return army.groups.GetIndex_Safe(groupIndex);
            }
            return null;
        }
        public  bool TryGetSoldierGroup(out SoldierGroup soldierGroup)
        {
            var army = parmy.GetArmy();
            if (army != null)
            {
                soldierGroup = army.groups.GetIndex_Safe(groupIndex);
                return soldierGroup != null;
            }
            soldierGroup = null;
            return false;
        }
        public static bool operator ==(PSoldierGroup value1, PSoldierGroup value2)
        {
            return value1.parmy == value2.parmy &&
                value1.groupIndex == value2.groupIndex;
        }
        public static bool operator !=(PSoldierGroup value1, PSoldierGroup value2)
        {
            return value1.parmy != value2.parmy ||
                value1.groupIndex != value2.groupIndex;
        }

        public bool Equals(PSoldierGroup other)
        {
            return parmy == other.parmy && other.groupIndex == groupIndex;
        }
        public override bool Equals(object obj)
        {
            return Equals((PSoldierGroup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(parmy.pfaction.factionIndex, parmy.armyIndex, groupIndex);
        }

        public bool HasValue()
        {
            return groupIndex >= 0;
        }
        public bool IsEmpty()
        {
            return groupIndex < 0;
        }


    }
}
