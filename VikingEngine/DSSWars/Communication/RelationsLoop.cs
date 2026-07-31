using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.Communication
{
    struct RelationsLoop
    {
        PFaction faction;
        public int otherFactionIx = -1;

        public RelationsLoop(PFaction faction) 
        { 
            this.faction = faction;
        }

        public bool Next()
        {
            otherFactionIx++;

            if (otherFactionIx == faction.factionIndex)
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
            DssRef.world.diplomacy.RelationIndex(faction.factionIndex, otherFactionIx, out var result);
            return result;
        }

        public DiplomaticRelation Relation()
        {
            return DssRef.world.diplomacy.GetRelation(faction, OtherFaction_P());
        }

        public bool OtherFaction(out Faction other)
        { 
            other = DssRef.world.factions.Array[otherFactionIx];
            return other != null;
        }

        public PFaction OtherFaction_P()
        { 
            return new PFaction(otherFactionIx);
        }
    }
}
