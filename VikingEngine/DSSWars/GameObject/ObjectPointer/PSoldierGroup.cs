using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PSoldierGroup : IEquatable<PSoldierGroup>
    {
        public PMapObject pabsarmy;
        public int groupIndex;
        public bool isCityGuard;
        public static readonly PSoldierGroup Empty = new PSoldierGroup();

        public PSoldierGroup()
        {
            pabsarmy = PMapObject.Empty;
            groupIndex = -1;
        }
        public PSoldierGroup(PMapObject parmy, int index)
        {
            this.pabsarmy = parmy;
            groupIndex = index;
        }

        public PSoldierGroup(System.IO.BinaryReader r)
            : this()
        {
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            pabsarmy.write(w);
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
            pabsarmy.read(r);
            groupIndex = r.ReadUInt16();
            if (groupIndex == ushort.MaxValue)
            {
                groupIndex = -1;
            }
        }

        public SoldierGroup GetSoldierGroup(out AbsArmy army)
        {
            army = pabsarmy.Get() as AbsArmy;
            if (army != null)
            {
                return army.GetAbsArmy().groups.GetIndex_Safe(groupIndex);
            }
            return null;
        }
        public  bool TryGetSoldierGroup(out SoldierGroup soldierGroup)
        {
            var army = pabsarmy.Get();
            if (army != null)
            {
                soldierGroup = army.GetAbsArmy().groups.GetIndex_Safe(groupIndex);
                return soldierGroup != null;
            }
            soldierGroup = null;
            return false;
        }
        public static bool operator ==(PSoldierGroup value1, PSoldierGroup value2)
        {
            return value1.pabsarmy == value2.pabsarmy &&
                value1.groupIndex == value2.groupIndex;
        }
        public static bool operator !=(PSoldierGroup value1, PSoldierGroup value2)
        {
            return value1.pabsarmy != value2.pabsarmy ||
                value1.groupIndex != value2.groupIndex;
        }

        public bool Equals(PSoldierGroup other)
        {
            return pabsarmy == other.pabsarmy && other.groupIndex == groupIndex;
        }
        public override bool Equals(object obj)
        {
            return Equals((PSoldierGroup)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(pabsarmy.objectType, pabsarmy.pfaction.factionIndex, pabsarmy.objectIndex, groupIndex);
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
