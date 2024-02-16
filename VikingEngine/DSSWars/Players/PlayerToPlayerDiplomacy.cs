using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players
{
    class PlayerToPlayerDiplomacy
    {
        public int index;

        public bool suggestingNewRelation = false;
        public RelationType suggestedRelation;
        public int suggestedBy;
    }
}
