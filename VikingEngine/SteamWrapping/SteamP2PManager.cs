#if PCGAME
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using VikingEngine.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.SteamWrapping
{
    class SteamP2PManager
    {
        public const int SteamPackageByteLimit = 1200;

        const float NoResponceTimeKickSeconds =
#if DSS
            40;
#else
            10;
#endif
        public bool autoAcceptSessionRequests = false;
        public int PeerCount { get { return remoteGamers.Count; } }

        public SteamNetworkPeer Host;
        public SteamNetworkPeer localPeer;
        public List<AbsNetworkPeer> remoteGamers;
        public List<AbsNetworkPeer> joinHistory;
        public bool hostSession = false;

        public bool IsHostingSession => Ref.steamlobby.InLobby && hostSession;

        Callback<P2PSessionConnectFail_t> connectFailCallback;
        Callback<P2PSessionRequest_t> sessionRequestCallback;
        Callback<SteamNetConnectionStatusChangedCallback_t> connectionChangedCallback;

        const int LobbyTimeRefreshRateSec = 3;
        public const int LobbyTimeOut = LobbyTimeRefreshRateSec + 3;

        Timer.Basic roundtripTimer = new Timer.Basic(500, true);
        Timer.Basic lobbyTimeRefresh = new Timer.Basic(TimeExt.SecondsToMS(LobbyTimeRefreshRateSec), true);
        public Time disconnectTime = 0;

        Time heavyTrafficPause = Time.Zero;

        public SteamP2PManager()
        {
            autoAcceptSessionRequests = true;
            remoteGamers = new List<AbsNetworkPeer>();
            joinHistory = new List<AbsNetworkPeer>();
            
            connectFailCallback = new Callback<P2PSessionConnectFail_t>(OnConnectionFail, false);
            sessionRequestCallback = new Callback<P2PSessionRequest_t>(OnSessionRequest, false);
            //connectionChangedCallback = new Callback<SteamNetConnectionStatusChangedCallback_t>(OnConnectionStatusChanged, false);

            //m_listenSocket = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, null);

            //if (m_listenSocket != HSteamListenSocket.Invalid)
            //{
            //    Debug.Log("P2P Listen Socket created successfully!");
            //}
        }

        

        public void OnSendingLargeDataChunk()
        {
            heavyTrafficPause = new Time(2, TimeUnit.Seconds);
        }

        //public void StartListening()
        //{
        //    m_listenSocket = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, null);
        //}
        //private void OnConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t pCallback)
        //{
        //    // Ignore connections that don't belong to our listen socket
        //    // (Useful if you have multiple sockets running)
        //    if (pCallback.m_info.m_hListenSocket != m_listenSocket && pCallback.m_info.m_hListenSocket != HSteamListenSocket.Invalid)
        //    {
        //        return;
        //    }

        //    // Handle the connection state
        //    switch (pCallback.m_info.m_eState)
        //    {
        //        case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting:

        //            // HERE IS YOUR HSteamNetConnection!
        //            HSteamNetConnection incomingConnection = pCallback.m_hConn;

        //            var id = pCallback.m_info.m_identityRemote.GetSteamID();
        //            Debug.Log($"Incoming connection from {pCallback.m_info.m_identityRemote.GetSteamID()}!");

        //            // You must accept the connection to establish it
        //            EResult result = SteamNetworkingSockets.AcceptConnection(incomingConnection);

        //            if (result == EResult.k_EResultOK)
        //            {
        //                Debug.Log("Connection accepted.");
        //                // You can now store 'incomingConnection' in a List/Dictionary 
        //                // to send messages to this specific user later.
        //                //connection = incomingConnection;
        //                (getOrCreatePeer(id) as SteamNetworkPeer).connection = incomingConnection;
        //            }
        //            else
        //            {
        //                Debug.LogError("Failed to accept connection.");
        //                SteamNetworkingSockets.CloseConnection(incomingConnection, 0, "Failed to accept", false);
        //            }
        //            break;

        //        case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected:
        //            Debug.Log("Client has fully connected.");
        //            break;

        //        case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer:
        //        case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally:
        //            Debug.Log("Connection closed or dropped.");
        //            // Clean up the connection handle
        //            SteamNetworkingSockets.CloseConnection(pCallback.m_hConn, 0, null, false);
        //            break;
        //    }
        //}

       

        public void update()
        {
            

            if (disconnectTime.CountDown())
            {
                heavyTrafficPause.CountDown();
                ReadAllPackets();

                if (roundtripTimer.Update() && Ref.netSession != null)
                {
                    var w = Ref.netSession.BeginWritingPacket(PacketType.Steam_SendRoundtrip, PacketReliability.Reliable);
                    w.Write(Ref.TotalTimeSec);

                    for (int i = remoteGamers.Count - 1; i >= 0; --i)
                    {
                        if (Ref.TotalTimeSec - remoteGamers[i].lastHeardFrom > NoResponceTimeKickSeconds)
                        {
                            bool useTimeoutKick = PlatformSettings.Debug_UseNetworkTimeout || PlatformSettings.DebugLevel > BuildDebugLevel.Dev;
                            bool lostHost = Host == remoteGamers[i];
                            bool isHostingNetworkOrLostHost = Ref.netSession.IsHost || lostHost;

                            if (useTimeoutKick && isHostingNetworkOrLostHost)
                            {
                                Debug.Log("Session TIMEOUT kick");
                                if (lostHost)
                                {
                                    Debug.Log("Lost host");
                                    Ref.netSession.Disconnect("Lost host (timeout)");
                                }
                                else
                                {
                                    //Unresponsive, kick player
                                    Debug.CrashIfThreaded();
                                    RemovePeer(remoteGamers[i]);
                                }
                            }
                        }
                    }
                }

                if (Ref.steamlobby.InLobby)
                {
                    if (lobbyTimeRefresh.Update())
                    {
                        if (hostSession)
                        {
                            Ref.steamlobby.updateLobbyTime(true);
                        }
                    }
                }
            }
        }

        void ReadAllPackets()
        {
            uint msgSize = 0;
            while (SteamNetworking.IsP2PPacketAvailable(out msgSize, 0))
            {
                byte[] data = new byte[msgSize];
                uint bytesRead;
                CSteamID senderId;

                if (SteamNetworking.ReadP2PPacket(data, msgSize, out bytesRead, out senderId, 0))
                {
                    DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();

                    if (data.Length <= 1)
                    {
                        continue;
                    }
                    stream.SetByteArray(data);

                    AbsNetworkPeer peer = getOrCreatePeer(senderId);

                    peer.lastHeardFrom = Ref.TotalTimeSec;

                    var packet = new Network.ReceivedPacket(peer, stream.GetReader());

                    if (peer.approved)
                    {
                        switch (packet.type)
                        {
                            default:
                                Ref.NetUpdateReciever().NetworkReadPacket(packet);
                                break;

                            case PacketType.Steam_SendRoundtrip:
                                {
                                    float timestamp = packet.r.ReadSingle();
                                    var w = Ref.netSession.BeginWritingPacket(PacketType.Steam_ReturnRoundtrip, senderId.m_SteamID, PacketReliability.Reliable, null);
                                    w.Write(timestamp);
                                }
                                break;
                            case PacketType.Steam_ReturnRoundtrip:
                                {
                                    float timestamp = packet.r.ReadSingle();
                                    float timePassed = TimeExt.SecondsToMS(Ref.TotalTimeSec - timestamp);
                                    packet.sender.roundTripTime = packet.sender.roundTripTime * 0.5f + timePassed * 0.5f;

                                    int packetCount = 32;

                                    if (heavyTrafficPause.TimeOut)
                                    {
                                        if (packet.sender.roundTripTime < 40)
                                        {
                                            packetCount = 512;
                                        }
                                        else if (packet.sender.roundTripTime < 140)
                                        {
                                            packetCount = 256;
                                        }
                                    }

                                    packet.sender.packetLoad *= 0.2f;
                                    packet.sender.potensialLoad = 0;
                                    packet.sender.maxPacketCount = Bound.Min(packetCount / remoteGamers.Count, 1);
                                    Ref.NetUpdateReciever().NetEvent_PingReturned(packet.sender);
                                }
                                break;
                            case PacketType.Steam_SuccesfulJoinPing:
                                {
                                    var w = Ref.netSession.BeginWritingPacket(PacketType.Steam_ReturnRoundtrip, senderId.m_SteamID, PacketReliability.Reliable, packet.sender.id);
                                    w.Write(0f);
                                }
                                break;
                            case PacketType.Steam_AssignClientId:
                                if (hostSession)
                                {
                                    Ref.NetUpdateReciever().NetworkStatusMessage(NetworkStatusMessage.Double_join_error);
                                    return;
                                }

                                bool myId = readAssignedId(packet);
                                if (myId)
                                {
                                    Ref.NetUpdateReciever().NetEvent_GotNetworkId();
                                }
                                break;
                            case Network.PacketType.PlayerDisconnected:
                                RemovePeer(packet.sender.SteamID);
                                break;
                            //case PacketType.Basic_MapLoadedAndReady:
                            //    packet.sender.mapLoadedAndReady = true;
                            //    Debug.Log(packet.sender.Gamertag + ":: Map Loaded And Ready");
                            //    break;
                            case Network.PacketType.KickPlayer:
                                ulong fullId = packet.r.ReadUInt64();
                                if (localPeer.fullId == fullId)
                                {
                                    //ReceiveKick();
                                    Ref.netSession.Disconnect("Removed by host");
                                    //Ref.steam.LobbyMatchmaker.CreateLobby();
                                }
                                break;
                            case PacketType.Steam_LargePacket:
                                {
                                    int id = packet.r.ReadInt32();
                                    if (Ref.netSession.largePackets.TryGetValue(id, out var largePacketWriter))
                                    {
                                        largePacketWriter.readNext(packet);
                                    }
                                    else
                                    {
                                        new SteamLargePacketWriter(packet, id);
                                    }
                                }
                                break;
                            case PacketType.Steam_LargePacket_Recieved:
                                {
                                    int id = packet.r.ReadInt32();
                                    if (Ref.netSession.largePackets.TryGetValue(id, out var largePacketWriter))
                                    {
                                        largePacketWriter.sendNext();
                                    }
                                }
                                break;
                            //case PacketType.VoiceChat:
                            //    Ref.steam.readChat(packet.r);
                            case PacketType.VoiceChat:
                                Ref.steam.readVoice(packet.sender, packet.r);
                                break;
                        }
                    }
                    else
                    {
                        //RemovePeer(peer);
                        Ref.netSession.writeKick(peer);//peer.kickFromNetwork();
                    }
                }
            }
        }

        public AbsNetworkPeer getOrCreatePeer(CSteamID playerId)
        {
            AbsNetworkPeer peer = GetPeer(playerId);
            if (peer == null)
            {
                peer = AddPeer(playerId);
            }

            return peer;
        }

        public AbsNetworkPeer AddPeer(CSteamID peerId)
        {
            if (localPeer == null)
            {
                createLocalPeer();
            }

            if (peerId == CSteamID.Nil)
            {
                throw new Exception();
            }

            if (peerId == localPeer.SteamID)
            {
                //local gamer, never add
                return null;
            }

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                if (peerId == remoteGamers[i].SteamID) // already added
                    return remoteGamers[i];
            }
            
            var gamer = new SteamNetworkPeer(peerId, false);
            remoteGamers.Add(gamer);

            gamer.approved = approveNewPeer(gamer);

            if (gamer.approved)
            {
                assignIdToGamer(gamer);
                netWriteGamerIds();

                Ref.NetUpdateReciever().NetEvent_PeerJoined(gamer);
            }
            else
            {

            }
            return gamer;
        }

        bool approveNewPeer(SteamNetworkPeer peer)
        {
            var stored = Ref.netsett.getStoredGamer(peer.fullId);
            peer.storedData = stored;
            stored.name = peer.Gamertag;
            Ref.netsett.setUpdatedStoredGamer(stored);
            
            if (hostSession)
            {      
                return Ref.netSession.joinableStatus &&
                    remoteGamers.Count <= Ref.netsett.maxPlayerCount &&
                    stored.ban < BanStatus.Banned;// .bannedPeers.isBanned(peer) == false;
            }
            else
            {
                peer.mapLoadedAndReady = true;
                return true;
            }
        }

        public SteamNetworkPeer GetLocalPeer()
        {
            if (localPeer == null)
            {
                createLocalPeer();
            }
            
            return localPeer;
        }

        void createLocalPeer()
        {
            localPeer = new SteamNetworkPeer(SteamUser.GetSteamID(), true);
            localPeer.mapLoadedAndReady = hostSession;
        }

        public void RemovePeer(AbsNetworkPeer peer)
        {
            peer.approved = false;

            RemovePeer(peer.SteamID);
        }

        public void RemovePeer(CSteamID steamId)
        {
            SteamNetworking.CloseP2PSessionWithUser(steamId);

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                var peer = remoteGamers[i];
                if (peer.SteamID == steamId)
                {
                    Ref.netsett.setUpdatedStoredGamer(peer.storedData);
                    peer.unload();
                    joinHistory.Add(peer);
                    remoteGamers.RemoveAt(i);
                    Ref.NetUpdateReciever().NetEvent_PeerLost(peer);
                    break;
                }
            }

            if (Host == null || Host.SteamID == steamId)
            {
                Ref.netSession.Disconnect("Lost host");
            }
        }

        //public void ReceiveKick()
        //{
        //    for (int i = 0; i < remoteGamers.Count; ++i)
        //    {
        //        Ref.NetUpdateReciever().NetEvent_PeerLost(remoteGamers[i]);
        //        remoteGamers.RemoveAt(i--);
        //    }
        //}

        void assignIdToGamer(AbsNetworkPeer gamer)
        {
            if (hostSession)
            {
                Host = localPeer;
                localPeer.id = 0;

                //Host = 0-3
                //Error = 255
                //Clients = Var fjärde index från 4, får tre reserverade id för split screen medlemmar

                for (byte nextId = 4; nextId < byte.MaxValue; nextId += 4)
                {
                    bool available = true;
                    foreach (SteamNetworkPeer peer in remoteGamers)
                    {
                        if (peer.id == nextId)
                        {
                            available = false;
                            break;
                        }
                    }

                    if (available)
                    {
                        gamer.id = nextId;
                        //var w = Ref.netSession.BeginWritingPacket(Network.PacketType.Steam_AssignClientId, Network.PacketReliability.Reliable);
                        //w.Write(gamer.steamId);
                        //w.Write(nextId);
                        return;
                    }
                }

                throw new Exception("Out of network id's");
            }
        }

        void netWriteGamerIds()
        {
            if (hostSession)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.Steam_AssignClientId, Network.PacketReliability.Reliable);
                w.Write((byte)(remoteGamers.Count + 1));
                
                foreach (var r in remoteGamers)
                {
                    w.Write(r.fullId);
                    w.Write(r.id);
                }

                w.Write(localPeer.fullId);
                w.Write(localPeer.id);
            }
        }

        /// <returns>My id</returns>
        public bool readAssignedId(Network.ReceivedPacket packet)
        {
            var r = packet.r;
            Host = packet.sender as SteamNetworkPeer;

            int count = r.ReadByte();
            bool myId = false;

            for (int i = 0; i < count; ++i)
            {
                ulong steamId = r.ReadUInt64();
                byte id = r.ReadByte();

                if (localPeer.fullId == steamId)
                {
                    localPeer.id = id;
                    myId = true;
                }
                else
                {
                    foreach (SteamNetworkPeer peer in remoteGamers)
                    {
                        if (peer.fullId == steamId)
                        {
                            peer.id = id;
                            break;
                        }
                    }
                }
            }

            return myId;
        }

        public AbsNetworkPeer GetPeer(CSteamID peerId)
        {
            foreach (var peer in remoteGamers)
            {
                if (peer.SteamID == peerId)
                    return peer;
            }

            if (localPeer != null && localPeer.SteamID == peerId)
                return localPeer;

            return null;
        }

        public static void CrashOnTooLargePacket(System.IO.BinaryWriter w)
        {
#if DEBUG
            if (w.BaseStream.Length > SteamPackageByteLimit)
            {
                throw new Exception("Passed steam package limit");
            }
#endif
        }


        public void Send(byte[] data, uint dataLength, VikingEngine.Network.PacketReliability rely, SendPacketTo to, CSteamID specificGamerID)
        {
#if DEBUG
            if (dataLength > SteamPackageByteLimit)
            {
                var packet = (PacketType)data[1];
                throw new Exception("Passed steam package limit: " + packet);
            }
#endif
            EP2PSend sendType;
            float load = data.Length / 1000f + 0.2f;
            if (rely == Network.PacketReliability.Unrelyable)
            {
                //SendUnreliable(data);
                sendType = EP2PSend.k_EP2PSendUnreliable;
                load *= 0.5f;
            }
            else
            {
                //SendReliable(data);
                sendType = EP2PSend.k_EP2PSendReliable;
            }


            switch (to)
            {
                default:
                    foreach (SteamNetworkPeer peer in remoteGamers)
                    {
                        send(peer);
                    }
                    break;

                case SendPacketTo.Ready:
                    foreach (SteamNetworkPeer peer in remoteGamers)
                    {
                        if (peer.mapLoadedAndReady)
                        {
                            send(peer);
                        }
                    }
                    break;

                case SendPacketTo.OneSpecific:
                    send(getOrCreatePeer(specificGamerID));
                    break;

                case SendPacketTo.Host:
                    if (Host != null)
                    {
                        send(Host);
                    }
                    break;

            }

           

            void send(AbsNetworkPeer peer)
            {
                peer.packetLoad += load;
                SteamNetworking.SendP2PPacket(peer.SteamID, data, dataLength, sendType, 0);
            }

        }

        public void SendUnreliable(byte[] data)
        {
            if (data.Length > SteamPackageByteLimit)
            {
                throw new IndexOutOfRangeException("MTU size is 1200 bytes. Please split the data into smaller packets.");
            }
            foreach (var peer in remoteGamers)
            {
                SteamNetworking.SendP2PPacket(peer.SteamID, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliable, 0);
            }
        }

        public void SendUnreliableNoDelay(byte[] data)
        {
            if (data.Length > SteamPackageByteLimit)
            {
                throw new IndexOutOfRangeException("MTU size is 1200 bytes. Please split the data into smaller packets.");
            }
            foreach (var peer in remoteGamers)
            {
                SteamNetworking.SendP2PPacket(peer.SteamID, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
            }
        }

        public void SendReliable(byte[] data)
        {
            foreach (var peer in remoteGamers)
            {
                SteamNetworking.SendP2PPacket(peer.SteamID, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable, 0);
            }
        }

        public void SendReliableWithBuffering(byte[] data)
        {
            foreach (var peer in remoteGamers)
            {
                SteamNetworking.SendP2PPacket(peer.SteamID, data, (uint)data.Length, EP2PSend.k_EP2PSendReliableWithBuffering, 0);
            }
        }

        public void CreateSession()
        {
            hostSession = true;

            Ref.steam.LobbyMatchmaker.RefreshLobbyVisibility();

            if (Ref.steamlobby.InLobby)
            {
                Ref.NetUpdateReciever().NetworkStatusMessage(NetworkStatusMessage.Created_session);
            }
            else
            {
                Ref.steamlobby.CreateLobby();
            }
        }

        public LobbyPublicity SessionLobbyPublicity()
        {
            if (hostSession)
            {
                if (Ref.netsett.lobbyPublicity == LobbyPublicity.Private)
                {
                    return LobbyPublicity.FriendsOnly;
                }
                else
                {
                    return LobbyPublicity.Public;//hostSession ? Ref.netsett.lobbyPublicity : LobbyPublicity.Public;
                }
            }
            else
            {
                return LobbyPublicity.Private;
            }
        }

        public void disconnectSession()
        {
            Debug.CrashIfThreaded();

            for (int i = remoteGamers.Count - 1; i >= 0; --i)
            {
                RemovePeer(remoteGamers[i]);
            }

            localPeer = null;
            Host = null;
            endSession();
            //SteamAPI.clearMem();

            disconnectTime.Seconds = 6f;
        }

        public void endSession()
        {
            Debug.CrashIfThreaded();

            foreach (var gamer in remoteGamers)
            {
                Ref.netsett.setUpdatedStoredGamer(gamer.storedData);
                gamer.unload();
                joinHistory.Add(gamer);
            }
            remoteGamers.Clear();
            
            hostSession = false;
            Ref.steam.LobbyMatchmaker.RefreshLobbyVisibility();
        }

        public void OnSessionRequest(P2PSessionRequest_t sessionRequestInfo)
        {
            if (autoAcceptSessionRequests && disconnectTime.TimeOut)
            {
                CSteamID peerID = sessionRequestInfo.m_steamIDRemote;
                SteamNetworking.AcceptP2PSessionWithUser(peerID);

                for (int i = 0; i < remoteGamers.Count; ++i)
                {
                    if (remoteGamers[i].SteamID == peerID)
                    {
                        return; // already added
                    }
                }
                AddPeer(peerID);
                //remoteGamers.Add(new SteamNetworkPeer(peerID));
            }
        }

        public void OnConnectionFail(P2PSessionConnectFail_t connectionFailInfo)
        {
            CSteamID peerID = connectionFailInfo.m_steamIDRemote;
            EP2PSessionError error = (EP2PSessionError)connectionFailInfo.m_eP2PSessionError;
            switch (error)
            {
                case EP2PSessionError.k_EP2PSessionErrorNone:
                    break;
                //case EP2PSessionError.k_EP2PSessionErrorNotRunningApp:
                //    Debug.LogWarning("The remote user isn't running the same game (appID) as you are.");
                //    break;
                case EP2PSessionError.k_EP2PSessionErrorNoRightsToApp:
                    Debug.LogWarning("The local user doesn't own this game.");
                    break;
                //case EP2PSessionError.k_EP2PSessionErrorDestinationNotLoggedIn:
                //    Debug.LogWarning("The remote user doesn't have a connection to Steam.");
                //    break;
                case EP2PSessionError.k_EP2PSessionErrorTimeout:
                    Debug.LogWarning("The remote user isn't responding. This could be because no physical connection could be made, or the remote end isn't calling AcceptP2PSessionWithUser()");
                    break;
                default:
                    Debug.LogWarning("The remote user didn't answer, but we got no failure reason. Maybe you are not connected to the internet?");
                    break;
            }
            Debug.CrashIfThreaded();
            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                if (remoteGamers[i].SteamID == peerID)
                {
                    var gamer = remoteGamers[i];
                    gamer.unload();
                    joinHistory.Add(gamer);
                    remoteGamers.RemoveAt(i--);
                    Ref.NetUpdateReciever().NetEvent_PeerLost(gamer);
                    
                }
            }
        }
    }
}
#endif