using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    class NetManager
    {
        JoinNetwork joinNetwork;
        NetState netState = NetState.SearchingSessions;
        Time searchAndCreateTimer;
        List<Network.AbsAvailableSession> availableSessionsList = null;

        Timer.Basic loadPicTimer = new Timer.Basic(500, true);

        public NetManager()
        {
            //LfRef.net = this;
            //new Network.Session();
        }

        public void update()
        {
            if (joinNetwork != null)
            {
                //if (joinNetwork.Update())
                //{
                //    joinNetwork.DeleteMe();
                //    joinNetwork = null;
                //}
            }
            else
            {

                if (searchAndCreateTimer.CountDown())
                {
                    if (Ref.gamestate is LootFest.PlayState && LfRef.gamestate.UpdateCount > 20)
                    {

                        if (!Ref.netSession.ableToConnect)
                        {
                            searchAndCreateTimer.Seconds = 1f;
                            return;
                        }

                        //if (availableSessionsList != null && netState != NetState.ManualSearchingSessions)
                        //{
                        //    if (!LfRef.gamestate.lockedInMenu && !LfRef.gamestate.LocalHostingPlayer.inCinamticMode)
                        //    {
                        //        LfRef.gamestate.LocalHostingPlayer.listAvailableSessions(availableSessionsList);
                        //        availableSessionsList = null;
                        //        netState = NetState.WaitingForPlayerChoise;
                        //    }
                        //    else
                        //    {
                        //        searchAndCreateTimer.Seconds = 0.5f;
                        //        return;
                        //    }
                        //}


                        switch (netState)
                        {
                            case NetState.SearchingSessions:
                                findSessions();
                                searchAndCreateTimer.Seconds = 5f;
                                netState = NetState.CreatingSession;
                                break;
                            case NetState.CreatingSession:
                                //createSession();
                                searchAndCreateTimer.Seconds = 20f;
                                break;
                            case NetState.WaitingForPlayerChoise:
                                if (LfRef.gamestate.LocalHostingPlayer.inMenu)
                                {
                                    searchAndCreateTimer.Seconds = 2f;
                                }
                                else
                                {
                                    netState = NetState.CreatingSession;
                                }
                                break;
                            case NetState.JoiningSession:
                                //do non
                                break;
                            //case NetState.ManualSearchingSessions:
                            //    LfRef.gamestate.LocalHostingPlayer.listAvailableSessions(availableSessionsList);
                            //    availableSessionsList = null;
                            //    netState = NetState.WaitingForPlayerChoise;
                            //    break;
                        }
                    }
                    else
                    {
                        searchAndCreateTimer.Seconds = 2f;
                    }
                }
            }

            //if (availableSessionsList != null)
            //{
            //    if (loadPicTimer.Update())
            //    {
            //        foreach (var m in availableSessionsList)
            //        {
            //            m.tryLoadGamerIcon();
            //        }
            //    }
            //}
        }

        public void ManualLobbySearch()
        {
            findSessions();
            netState = NetState.ManualSearchingSessions;
            searchAndCreateTimer.Seconds = 5f;
        }

        void findSessions()
        {
#if PCGAME
            if (Ref.steam.isInitialized)
            {
                Ref.steam.LobbyMatchmaker.FindLobbies();
            }
#endif
        }

        //void createSession()
        //{
        //    //if (PlatformSettings.SteamAPI)
        //    //{
        //    //    //if (!VikingEngine.SteamWrapping.SteamNetwork.IsLobbyOwner)
        //    //    //{
        //    //    //   Ref.Steam.LobbyMatchmaker.CreateLobby();
        //    //    //}
        //    //}
        //    //else
        //    //{
        //    //    if (!Ref.netSession.Connected)
        //    //    {
        //    //        Ref.netSession.CreateSession();
        //    //    }
        //    //}
        //}

        


        public bool waitingForId;
        public void joinSessionLink(Network.AbsAvailableSession available)//Microsoft.Xna.Framework.Net.AvailableNetworkSession session)
        {
            waitingForId = true;
            netState = NetState.JoiningSession;
            joinNetwork = new JoinNetwork(available);
            LfRef.gamestate.LocalHostingPlayer.CloseMenu();
        }

        //public void NetworkAvailableSessionsUpdated(AvailableNetworkSessionCollection availableSessions)
        //{
        //    //if (availableSessions.Count > 0)
        //    //{
        //    //    var list = Ref.netSession.SortAvailableSessions();
        //    //    availableSessionsList = new List<SteamAvailableSession>(list.Count);
        //    //    foreach (var av in list)
        //    //    {
        //    //        availableSessionsList.Add(new XboxAvailableSession(av));
        //    //    }
        //    //    searchAndCreateTimer.MilliSeconds = 0;
        //    //}

        //    //if (netState == NetState.ManualSearchingSessions)
        //    //{
        //    //    searchAndCreateTimer.MilliSeconds = 0;
        //    //}

        //}

        public void NetEvent_SessionsFound(List<Network.AbsAvailableSession> availableSessions)
        {
            this.availableSessionsList = availableSessions;
            searchAndCreateTimer.MilliSeconds = 0;

            Vector3 pos = LfRef.LocalHeroes[0].Position;
            pos.Y += 5;

            foreach (var m in availableSessions)
            {
                pos += VectorExt.V2toV3XZ(Rotation1D.Random().Direction(2f));

                new VikingEngine.LootFest.GO.NPC.LobbyCharacter(new GO.GoArgs(pos), m);
            }
        }
        //public void NetworkAvailableSessionsUpdated_PC(LobbyMatchList_t lobbyMatchList)
        //{
        //    int count = (int)lobbyMatchList.m_nLobbiesMatching;
        //    var list = new List<SteamAvailableSession>(count);
        //    for (int i = 0; i < count; ++i)
        //    {
        //        ulong lobbyID = SteamAPI.SteamMatchmaking().GetLobbyByIndex((int)i);
        //        if (lobbyID != VikingEngine.Ref.instance.LobbyMatchmaker.currentLobbyID)
        //        {
        //            list.Add(new SteamAvailableSession(lobbyID));
        //        }
        //    }


        //    //Sort, clear out doublettes
        //    availableSessionsList = new List<SteamAvailableSession>(count);
        //    foreach (var unsortedMember in list)
        //    {
        //        bool contains = false;
        //        foreach (var sortedMember in availableSessionsList)
        //        {
        //            if (sortedMember.Equals(unsortedMember))
        //            {
        //                contains = true;
        //                break;
        //            }
        //        }

        //        if (!contains)
        //        {
        //            availableSessionsList.Add(unsortedMember);
        //        }
        //    }

        //    //if (netState == NetState.ManualSearchingSessions)
        //    //{
        //        searchAndCreateTimer.MilliSeconds = 0;
        //    //}
        //}

        //public void NetworkStatusMessage(Network.NetworkStatusMessage message)
        //{
        //    if (joinNetwork != null)
        //    {
        //        joinNetwork.NetworkStatusMessage(message);
        //    }
        //}
        public void gotNetworkId()
        {
            waitingForId = false;
        }
        public void gotWorldSeed()
        {
            
            if (joinNetwork != null)
            {
                joinNetwork.DeleteMe();
                joinNetwork = null;
            }
        }
    }

    enum NetState
    {
        SearchingSessions,
        WaitingForPlayerChoise,
        JoiningSession,
        CreatingSession,
        ManualSearchingSessions,
    }
}
