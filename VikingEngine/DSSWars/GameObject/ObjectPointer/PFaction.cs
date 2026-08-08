using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PFaction : IEquatable<PFaction>, IBinaryIOobj
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
            read(r);
        }

        public void write(System.IO.BinaryWriter w)
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
        public void read(System.IO.BinaryReader r)
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

            return DssRef.world.factions.GetIndex_Safe(factionIndex); ;
        }

        public bool TryGetFaction(out Faction faction)
        {
            var ix_sp = factionIndex;
            if (ix_sp >= 0 && ix_sp < DssRef.world.factions.Count)
            {
                faction = DssRef.world.factions.Array[ix_sp];
                return true;
            }
            faction = null;
            return false;
        }

        public bool TryGetFactionAndPlayer(out Faction faction, out AbsPlayer player)
        {
            var ix_sp = factionIndex;
            if (ix_sp >= 0 && ix_sp < DssRef.world.factions.Count)
            {
                faction = DssRef.world.factions.Array[ix_sp];
                player = faction.player;
                return player != null;
            }
            faction = null;
            player = null;
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

        public bool HasValue()
        {
            return factionIndex >= 0;
        }
        public bool IsEmpty()
        {
            return factionIndex < 0;
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

        public bool TryGetLocalPlayer(out Players.LocalPlayer player)
        {
            if (TryGetPlayer(out var aplayer))
            {
                player = aplayer.GetLocalPlayer();
                return player != null;
            }
            player = null;
            return false;
        }
        public bool TryGetHumanPlayer(out Players.AbsHumanPlayer player)
        {
            if (TryGetPlayer(out var aplayer))
            {
                player = aplayer.GetHumanPlayer();
                return player != null;
            }
            player = null;
            return false;
        }
        public bool TryGetRemotePlayer(out Players.RemotePlayer player)
        {
            if (TryGetPlayer(out var aplayer))
            {
                player = aplayer.GetRemotePlayer();
                return player != null;
            }
            player = null;
            return false;
        }

        public bool TryGetAiPlayer(out Players.AiPlayer player)
        {
            if (TryGetPlayer(out var aplayer))
            {
                player = aplayer.GetAiPlayer();
                return player != null;
            }
            player = null;
            return false;
        }

        public override string ToString()
        {
            if (TryGetFaction(out var faction))
            { 
                return faction.ToString();
            }
            return $"faction ({factionIndex})";
        }
    }

}
