using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest
{
    class EventManager
    {
        public EventManager()
        {
            hqRef.events = this;
        }

        public void SendToMap(ToGG.Data.EventType eventType, object tag)
        {
            toggRef.board.OnEvent(eventType);
        }

        public void SendToPlayers(ToGG.Data.EventType eventType, object tag)
        {
            var players = hqRef.players.allPlayers.counter();
          
            while (players.Next())
            {
                players.sel.OnEvent(eventType, tag);
            }
        }

        public void SendToAll(ToGG.Data.EventType eventType, object tag)
        {
            toggRef.board.OnEvent(eventType);

            SendToPlayers(eventType, tag);

            hqRef.setup.conditions.OnEvent(eventType, tag);
        }

        public void SendToAllButInactivePlayers(ToGG.Data.EventType eventType, object tag)
        {
            toggRef.board.OnEvent(eventType);

            SendToActivePlayers(eventType, tag);

            hqRef.setup.conditions.OnEvent(eventType, tag);
        }

        public void SendToActivePlayers(ToGG.Data.EventType eventType, object tag)
        {
            var players = hqRef.players.allPlayers.counter();
            while (players.Next())
            {
                if (players.sel.pData.teamIndex == hqRef.players.currentTeam)
                {
                    players.sel.OnEvent(eventType, tag);
                }
            }
        }
    }
}
