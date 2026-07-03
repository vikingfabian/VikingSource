using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.ObjectPointer
{
    struct PFaction
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
