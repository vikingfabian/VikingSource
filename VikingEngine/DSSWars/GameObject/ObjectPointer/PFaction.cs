using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PFaction : IEquatable<PFaction>
    {
        public int factionIndex;
        public static readonly PFaction Empty = new PFaction();

        public PFaction()
        {
            factionIndex = -1;
        }
        public PFaction(int index)
        {
            factionIndex = index;
        }

        public PFaction(System.IO.BinaryReader r)
        {
            NetRead(r);
        }

        public void NetWrite(System.IO.BinaryWriter w)
        {
            if (factionIndex < 0)
            {
                w.Write(ushort.MaxValue);
            }
            else
            {
                w.Write((ushort)factionIndex);
            }
        }
        public void NetRead(System.IO.BinaryReader r)
        {
            factionIndex = r.ReadUInt16();
            if (factionIndex == ushort.MaxValue)
            {
                factionIndex = -1;
            }
        }

        public Faction GetFaction()
        {

            if (factionIndex < 0)
            {
                return null;
            }

            return DssRef.world.faction(factionIndex);
        }

        public bool TryGetFaction(out Faction faction)
        {
            if (factionIndex >= 0 && factionIndex < DssRef.world.factions.Count)
            {
                faction = DssRef.world.factions.Array[factionIndex];
                return true;
            }
            faction = null;
            return false;
        }

        public Faction GetFaction_Safe()
        {
            return DssRef.world?.faction(factionIndex);
        }

        public static bool operator ==(PFaction value1, PFaction value2)
        {
            return value1.factionIndex == value2.factionIndex;
        }
        public static bool operator !=(PFaction value1, PFaction value2)
        {
            return value1.factionIndex != value2.factionIndex;
        }

        public bool Equals(PFaction other)
        {
            return other.factionIndex == factionIndex;
        }
        public override bool Equals(object obj)
        {
            return ((PFaction)obj).factionIndex == factionIndex;
        }

        public override int GetHashCode()
        {
            return factionIndex;
        }

        public Players.AbsPlayer GetPlayer()
        {

            if (factionIndex < 0)
            {
                return null;
            }

            return DssRef.world.factions.Array[factionIndex]?.player;
        }

        public bool TryGetPlayer(out Players.AbsPlayer player)
        {

            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Array.Length)
            {
                player = null;
            }
            else
            {
                player = DssRef.world.factions.Array[factionIndex]?.player;
            }
            return player != null;
        }
    }

}
