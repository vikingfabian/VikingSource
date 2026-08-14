using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.Battle
{
    struct InBattleWith
    {
        public bool attackingCity;
        public FlatArray_Three<PFaction> factions;
        //public PFaction faction1;
        //public int faction2;
        //public int faction3;
        public int groupsInBattle = 0;

        public InBattleWith()
        {
            factions = new FlatArray_Three<PFaction>();
        }

        //public void add(int faction)
        //{
        //    if (faction1 == faction)
        //    {
        //        return;
        //    }
        //    else if (faction1 == -1)
        //    {
        //        faction1 = faction;
        //    }
        //    else if (faction2 == faction)
        //    {
        //        return;
        //    }
        //    else if (faction2 == -1)
        //    {
        //        faction2 = faction;
        //    }
        //    else if (faction3 == faction)
        //    {
        //        return;
        //    }
        //    else if (faction3 == -1)
        //    {
        //        faction3 = faction;
        //    }
        //}

        //public bool ContainsFaction(int faction)
        //{ 
        //    return faction1 == faction || faction2 == faction || faction3 == faction; 
        //}
        public bool ContainsFaction(FactionType factionType)
        {
            for (int i = 0; i < factions.count; ++i)
            {
                var f = factions[i].GetFaction();
                if (f != null && f.factiontype == factionType)
                {
                    return true;
                }
            }
            //if (faction1 >= 0)
            //{
            //    var f = DssRef.world.faction(faction1);
            //    if (f != null && f.factiontype == factionType)
            //    {
            //        return true;
            //    }
            //}
            //if (faction2 >= 0)
            //{
            //    var f = DssRef.world.faction(faction2);
            //    if (f != null && f.factiontype == factionType)
            //    {
            //        return true;
            //    }
            //}
            //if (faction3 >= 0)
            //{
            //    var f = DssRef.world.faction(faction3);
            //    if (f != null && f.factiontype == factionType)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
