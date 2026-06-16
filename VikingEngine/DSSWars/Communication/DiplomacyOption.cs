using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Communication
{
    struct DiplomacyOption
    {
        public RelationType toRelation;
        public int cost;
        public bool available;

        public DiplomacyOption()
        { }

        public DiplomacyOption(RelationType toRelation)
        {
            this.toRelation = toRelation;
            available = true;
        }
    }
}
