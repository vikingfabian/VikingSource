using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Net;

namespace VikingEngine.DSSWars.Net
{
    class NetLobby : Network.NetLobby
    {
        Timer.Basic checkTimeout = new Timer.Basic(3000);
        //List2<LobbyButton> lobbies = new List2<LobbyButton>();
        List2<TrackedLobby> lobbiesSorted = new List2<TrackedLobby>();
        public NetLobby()
            : base()
        {
            searchLobbies = true;
            autoCreateSession = false;
        }

        public override void update()
        {
            base.update();

        }

        protected override void onEndedSession()
        {
            base.onEndedSession();
            applyNewSettings();
        }


        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);

            switch (message)
            {
                case Network.NetworkStatusMessage.Created_Lobby:
                    applyNewSettings();
                    break;
                case Network.NetworkStatusMessage.Joining_session:
                    if (Ref.gamestate is PlayState == false &&
                        Ref.gamestate is ConnectState == false)
                    {
                        new ConnectState();
                    }
                    break;
            }

            Ref.gamestate.NetworkStatusMessage(message);
        }
        public override void NetEvent_GotNetworkId()
        {
            base.NetEvent_GotNetworkId();
        }

        public override void onNewGameState(Engine.GameState newState)
        {
            //clearLobbies();
            applyNewSettings();
        }

        public override void NetEvent_SessionsFound(List<AbsAvailableSession> availableSessions/*, List<AbsAvailableSession> prevAvailableSessionsList*/)
        {
            Ref.gamestate.NetEvent_SessionsFound(availableSessions);
            
        }
        public override void NetEvent_PingReturned(AbsNetworkPeer gamer)
        {
            Ref.gamestate.NetEvent_PingReturned(gamer);
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);
            DssRef.state?.NetEvent_PeerLost(gamer);
        }

        public override void NetEvent_ErrorMessage(string message, AbsNetworkPeer peer, bool peerIsSender)
        {
            DssRef.state?.NetEvent_ErrorMessage(message, peer, peerIsSender);
        }

        public override AbsLobbyMetaData NetEvent_StartLobbyMetaData()
        {
            return new LobbyMetaData();
        }

        class TrackedLobby
        {
            public bool available = true;
            public AbsAvailableSession session;
        }

    }
}
