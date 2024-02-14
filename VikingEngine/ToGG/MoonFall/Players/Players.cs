using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall.Players
{
    class Players
    {
        public List<AbsPlayer> players;

        public Players()
        {
            moonRef.players = this;

            List<Faction> availableFactions = new List<Faction>((int)FactionType.NUM_NON);
            for (FactionType factionType = 0; factionType < FactionType.NUM_NON; ++factionType)
            {
                availableFactions.Add(new Faction(factionType));
            }

            var allAreas = new List<MapArea>(moonRef.map.areas);
            players = new List<AbsPlayer>(moonLib.MaxPlayerCount);

            for (int i = 0; i < moonLib.MaxPlayerCount; ++i)
            {
                House house = new House(i, arraylib.PullLastMember(availableFactions));
                house.startArea(arraylib.PullLastMember(allAreas));

                players.Add(new AiPlayer(house));
            }
        }
    }
}
