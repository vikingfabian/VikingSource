using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna

namespace VikingEngine.Network
{
    interface INetworkUpdateReviever
    {
        void NetworkStatusMessage(Network.NetworkStatusMessage message);
        void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer);
        void NetEvent_JoinedLobby(string name, ulong lobbyHost, bool fromInvite);
        void NetEvent_GotNetworkId();
        void NetEvent_PeerLost(Network.AbsNetworkPeer gamer);
        void NetworkReadPacket(Network.ReceivedPacket packet);
        void NetEvent_PingReturned(Network.AbsNetworkPeer gamer);
        void NetEvent_ConnectionLost(string reason);
        void NetEvent_SessionsFound(
            List<AbsAvailableSession> availableSessions, 
            List<AbsAvailableSession> prevAvailableSessionsList);

        void NetEvent_LargePacket(Network.ReceivedPacket packet);
    }

    enum NetLobbyState
    {
        Offline,
        CreatingSession,
        Lobby,
        TryingToJoin,
        LockedInSession,
        Closing,
    }

    class NetLobby : INetworkUpdateReviever
    {
        public bool autoCreateSession = true;
        public bool searchLobbies = false;
        protected NetLobbyState state = NetLobbyState.Offline;

        const float SearchTimerSec = 4f;
        Time updateTimer = new Time(SearchTimerSec, TimeUnit.Seconds);
        Time closeTimer;

        Time createLobbyFailTime;

        Timer.Basic joinFailTimer = new Timer.Basic(TimeExt.SecondsToMS(10), true); 

        public NetLobby()
        {
            Ref.lobby = this;

            if (Ref.netSession == null)
            {
                new Network.Session();
            }
        }

        virtual public void EnterLobby(bool enter)
        { }

        public void createSession()
        {
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
            {
                if (Ref.p2p.IsHostingSession)
                {
                    state = NetLobbyState.LockedInSession;
                }
                else
                {
                    state = NetLobbyState.CreatingSession;
                    createLobbyFailTime = new Time(8, TimeUnit.Seconds);

                    Ref.steam.P2PManager.CreateSession();//.LobbyMatchmaker.CreateLobbyIfNotInOne();
                }
            }
#endif
        }

        virtual public void update()
        {
            if (Ref.netSession.ableToConnect)
            {
                switch (state)
                {
                    case NetLobbyState.Offline:
                        if (autoCreateSession)
                        {
                            createSession();
                        }
                        else if (searchLobbies)
                        {
                            state = NetLobbyState.Lobby;
                        }
                        break;
                    case NetLobbyState.CreatingSession:
                        if (createLobbyFailTime.CountDown())
                        {
#if PCGAME
                            Ref.steam.LobbyMatchmaker.LeaveCurrentLobby();
#endif
                            state = NetLobbyState.Offline;
                        }
                        break;
                    case NetLobbyState.Lobby:
                        if (updateTimer.CountDown())
                        {
                            updateTimer.Seconds = 8;
                            if (searchLobbies)
                            {
                                findSessions();
                            }
                        }
                        break;
                    case NetLobbyState.Closing:
                        if (closeTimer.CountDown())
                        {
                            state = NetLobbyState.Offline;
                            onEndedSession();
                        }
                        break;
                    case NetLobbyState.TryingToJoin:
                        if (joinFailTimer.Update())
                        {
                            this.NetworkStatusMessage(Network.NetworkStatusMessage.Joining_failed);
                            disconnect("Join trial timeout");
                        }
                        break;

                }
            }
        }

        public void tryJoin(AbsAvailableSession session)
        {
            joinFailTimer.Reset();
            session.join();
            state = NetLobbyState.TryingToJoin;
        }

        public void lockSession()
        {
            state = NetLobbyState.LockedInSession;
        }

        void findSessions()
        {
#if PCGAME
            Ref.steam.LobbyMatchmaker.FindLobbies();
#endif
            updateTimer.Seconds = 20;
        }

        public void startSearchLobbies(bool search)
        {
            if (search)
            {
#if PCGAME
                if (Ref.steam.isInitialized)
                {
                    if (state != NetLobbyState.Closing)
                    {
                        state = NetLobbyState.Lobby;
                        Ref.steamlobby.CreateLobbyIfNotInOne();
                    }
                    updateTimer.Seconds = SearchTimerSec;
                    
                }
#endif
            }

            searchLobbies = search;
        }

        public void startCreateLobby(bool joinable = true)
        {
            autoCreateSession = true;
            Ref.netSession.joinableStatus = joinable;

            if (state != NetLobbyState.Closing)
            {
                state = NetLobbyState.Offline;
                createSession();
            }
        }

        public void disconnect(string reason)
        {
            state = NetLobbyState.Closing;
            autoCreateSession = false;
            searchLobbies = false;
            Ref.netSession.Disconnect(reason);
            closeTimer.Seconds = 8;
        }

        virtual public void NetworkStatusMessage(Network.NetworkStatusMessage message)
        {
            Ref.gamestate.NetworkStatusMessage(message);

            switch (message)
            {
                case Network.NetworkStatusMessage.Created_session:
                    state = NetLobbyState.LockedInSession;
                    break;
                case Network.NetworkStatusMessage.Created_Lobby:
                    state = NetLobbyState.Lobby;
                    break;
                case Network.NetworkStatusMessage.Double_join_error:
                    disconnect("Double join error");
                    break;
            }
            
        }
        virtual public void NetEvent_PeerJoined(Network.AbsNetworkPeer gamer)
        {
            Ref.gamestate.NetEvent_PeerJoined(gamer);
            if (state != NetLobbyState.Closing)
            {
                state = NetLobbyState.LockedInSession;
            }
        }
        virtual public void NetEvent_JoinedLobby(string name, ulong lobbyHost, bool fromInvite)
        {
            Ref.gamestate.NetEvent_JoinedLobby(name, lobbyHost, fromInvite);
        }
        virtual public void NetEvent_GotNetworkId()
        {
            Ref.gamestate.NetEvent_GotNetworkId();
        }
        virtual public void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {  
            Ref.gamestate.NetEvent_PeerLost(gamer);
        }
        virtual public void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            if (state != NetLobbyState.Closing)
            {
                Ref.gamestate.NetworkReadPacket(packet);
            }
        }

        virtual public void NetEvent_LargePacket(Network.ReceivedPacket packet) { }

        virtual public void NetEvent_PingReturned(Network.AbsNetworkPeer gamer)
        {
            Ref.gamestate.NetEvent_PingReturned(gamer);
        }
        virtual public void NetEvent_ConnectionLost(string reason)
        {
            Ref.gamestate.NetEvent_ConnectionLost(reason);
            state = NetLobbyState.Offline;
        }
        virtual public void NetEvent_SessionsFound(
            List<AbsAvailableSession> availableSessions, 
            List<AbsAvailableSession> prevAvailableSessionsList)
        {
            updateTimer.Seconds = SearchTimerSec;
            Ref.gamestate.NetEvent_SessionsFound(availableSessions, prevAvailableSessionsList);
        }

        protected void filterNewAndOldLobbies(
            List<AbsAvailableSession> availableSessions,
            List<AbsAvailableSession> prevAvailableSessionsList,
            out List<AbsAvailableSession> newLobbies,
            out List<AbsAvailableSession> lostLobbies)
        {
            newLobbies = new List<AbsAvailableSession>(arraylib.CountSafe(availableSessions));
            lostLobbies = new List<AbsAvailableSession>();

            if (arraylib.HasMembers(prevAvailableSessionsList) == false)
            {
                if (arraylib.HasMembers(availableSessions))
                {
                    newLobbies.AddRange(availableSessions);
                }
                return;
            }

            if (arraylib.HasMembers(availableSessions) == false)
            {
                if (arraylib.HasMembers(prevAvailableSessionsList))
                {
                    lostLobbies.AddRange(prevAvailableSessionsList);
                }
                return;
            }

            foreach (var av in availableSessions)
            {
                if (!prevAvailableSessionsList.Contains(av))
                {
                    newLobbies.Add(av);
                }
            }

            foreach (var prev in prevAvailableSessionsList)
            {
                if (!availableSessions.Contains(prev))
                {
                    lostLobbies.Add(prev);
                }
            }
        }

        virtual public void onNewGameState(VikingEngine.Engine.GameState newState)
        {
            searchLobbies = false;
        }

        virtual public void applyNewSettings()
        { }

        virtual protected void onEndedSession()
        { }  
    }
}
