using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Event
{
    class MatchResult
    {
        public List<Faction> winner = new List<Faction>(4);
        public List<Faction> loser = new List<Faction>(8);

    }
}
