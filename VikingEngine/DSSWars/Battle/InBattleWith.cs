using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Battle
{
    struct InBattleWith
    {
        public int faction1;
        public int faction2;
        public int faction3;
        public int groupsInBattle = 0;

        public InBattleWith()
        {
            faction1 = -1;
            faction2 = -1;
            faction3 = -1;
        }

        public void add(int faction)
        {
            if (faction1 == faction)
            {
                return;
            }
            else if (faction1 == -1)
            {
                faction1 = faction;
            }
            else if (faction2 == faction)
            {
                return;
            }
            else if (faction2 == -1)
            {
                faction2 = faction;
            }
            else if (faction3 == faction)
            {
                return;
            }
            else if (faction3 == -1)
            {
                faction3 = faction;
            }
        }

        public bool ContainsFaction(int faction)
        { 
            return faction1 == faction || faction2 == faction || faction3 == faction; 
        }
        public bool ContainsFaction(FactionType factionType)
        {
            if (faction1 >= 0)
            {
                var f = DssRef.world.factions.GetIndex_Safe(faction1);
                if (f != null && f.factiontype == factionType)
                {
                    return true;
                }
            }
            if (faction2 >= 0)
            {
                var f = DssRef.world.factions.GetIndex_Safe(faction2);
                if (f != null && f.factiontype == factionType)
                {
                    return true;
                }
            }
            if (faction3 >= 0)
            {
                var f = DssRef.world.factions.GetIndex_Safe(faction3);
                if (f != null && f.factiontype == factionType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
