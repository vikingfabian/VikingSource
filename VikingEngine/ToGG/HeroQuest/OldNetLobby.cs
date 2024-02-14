using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;

namespace VikingEngine.ToGG.HeroQuest
{
    class OldNetLobby : VikingEngine.Network.NetLobby
    {
        public const int MaxPlayers = 4;
        HUD.TextFeed textFeed;
        public bool host;

        public OldNetLobby(HUD.TextFeed textFeed, bool host)
            : base()
        {
            this.host = host;
            this.textFeed = textFeed;
            searchLobbies = !host;

            autoCreateSession = host;
        }

        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);
            print(TextLib.EnumName(message.ToString()));
        }

        public override void NetEvent_SessionsFound(List<AbsAvailableSession> availableSessions, 
            List<AbsAvailableSession> prevAvailableSessionsList)
        {
            if (availableSessions != null)
            {
                AbsAvailableSession session = availableSessions[0];
                tryJoin(session);
                print("Joining " + session.name);
            }
            base.NetEvent_SessionsFound(availableSessions, prevAvailableSessionsList);
        }

        public override void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerJoined(gamer);

            print(gamer.Gamertag + " joined");
            printLobbyStatus();
        }

        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);

            print(gamer.Gamertag + " lost");
            printLobbyStatus();
        }

        public override void NetEvent_GotNetworkId()
        {
            base.NetEvent_GotNetworkId();
            printLobbyStatus();
        }

        void printLobbyStatus()
        {
#if PCGAME
            if (Ref.steam.P2PManager.Host != null)
            {
                string text = "Lobby members (" + (Ref.netSession.RemoteGamersCount + 1) + "/" + MaxPlayers.ToString() + "): ";
                foreach (var m in Ref.steam.P2PManager.remoteGamers)
                {
                    text += m.Gamertag + ", ";
                }
                            
                print("Host: " + Ref.steam.P2PManager.Host.Gamertag);
                print(text);
            }
#endif
        }

        void print(string text)
        {
            if (Ref.gamestate is Lobby.LobbyState)
            {
                textFeed.print(text);
            }
        }
    }
}
