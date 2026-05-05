using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Communication
{
    struct RelationsLoop
    {
        int faction;
        public int otherFactionIx = -1;

        public RelationsLoop(int faction) 
        { 
            this.faction = faction;
        }

        public bool Next()
        {
            otherFactionIx++;

            if (otherFactionIx == faction)
            {
                otherFactionIx++;
            }
            if (otherFactionIx >= DssRef.world.factions.Array.Length)
            { 
                return false;
            }

            return true;
        }

        public bool nextAlly()
        {
            while (Next())
            {
                if (Relation().Relation >= RelationType.RelationType3_Ally)
                {
                    return true;
                }
            }
            return false;
        }

        public int RelationIndex()
        { 
            return DssRef.world.diplomacy.RelationIndex(faction, otherFactionIx);
        }

        public DiplomaticRelation Relation()
        {
            return DssRef.world.diplomacy.GetRelation(faction, otherFactionIx);
        }

        public bool OtherFaction(out Faction other)
        { 
            other = DssRef.world.factions.Array[otherFactionIx];
            return other != null;
        }
    }
}
