using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PArmy : IEquatable<PArmy>
    {
        public PFaction pfaction;
        public int armyIndex;
        public static readonly PArmy Empty = new PArmy();

        public PArmy()
        {
            pfaction = PFaction.Empty;
            armyIndex = -1;
        }
        public PArmy(PFaction pfaction, int index)
        {
            this.pfaction = pfaction;
            armyIndex = index;
        }

        public PArmy(System.IO.BinaryReader r)
        {            
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            pfaction.write(w);
            if (armyIndex < 0)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)armyIndex);
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            pfaction.read(r);
            armyIndex = r.ReadUInt16();
            if (armyIndex == ushort.MaxValue)
            {
                armyIndex = -1;
            }
        }

        public Army GetArmy()
        {
            if (pfaction.TryGetFaction(out var faction))
            {
                return faction.armies.GetIndex_Safe(armyIndex);
            }
            return null;
        }

        public bool TryGetArmy(out Army army)
        {
            if (pfaction.TryGetFaction(out var faction))
            {
                army = faction.armies.GetIndex_Safe(armyIndex);
                return army != null;
            }
            army = null;
            return false;
        }

        public static bool operator ==(PArmy value1, PArmy value2)
        {
            return value1.pfaction == value2.pfaction &&
                value1.armyIndex == value2.armyIndex;
        }
        public static bool operator !=(PArmy value1, PArmy value2)
        {
            return value1.pfaction != value2.pfaction || 
                value1.armyIndex != value2.armyIndex;
        }

        public bool Equals(PArmy other)
        {
            return pfaction == other.pfaction && other.armyIndex == armyIndex;
        }
        public override bool Equals(object obj)
        {
            return Equals((PArmy)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(pfaction.factionIndex, armyIndex);
        }

        public bool HasValue()
        {
            return armyIndex >= 0;
        }
        public bool IsEmpty()
        {
            return armyIndex < 0;
        }
    }
}
