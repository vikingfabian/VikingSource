#if PCGAME
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        public bool hostSession = false;

        public bool IsHostingSession => Ref.steamlobby.InLobby && hostSession;

        //Callback<P2PSessionConnectFail_t> connectFailCallback;
        //Callback<P2PSessionRequest_t> sessionRequestCallback;

        const int LobbyTimeRefreshRateSec = 3;
        public const int LobbyTimeOut = LobbyTimeRefreshRateSec + 3;

        Timer.Basic roundtripTimer = new Timer.Basic(500, true);
        Timer.Basic lobbyTimeRefresh = new Timer.Basic(TimeExt.SecondsToMS(LobbyTimeRefreshRateSec), true);
        public Time disconnectTime = 0;

        Time heavyTrafficPause = Time.Zero;


        Callback<SteamNetConnectionStatusChangedCallback_t> connectionStatusCallback;
        HSteamListenSocket listenSocket;
        HSteamNetPollGroup pollGroup;
        public Dictionary<CSteamID, HSteamNetConnection> connectionHandles = new Dictionary<CSteamID, HSteamNetConnection>();

        public SteamP2PManager()
        {
            //autoAcceptSessionRequests = true;
            //remoteGamers = new List<AbsNetworkPeer>();

            //connectFailCallback = new Callback<P2PSessionConnectFail_t>(OnConnectionFail, false);
            //sessionRequestCallback = new Callback<P2PSessionRequest_t>(OnSessionRequest, false);
            remoteGamers = new List<AbsNetworkPeer>();

            // 1. Initialize the single modern callback
            connectionStatusCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnConnectionStatusChanged);

            // 2. Create the Poll Group (we will throw all connections into this basket to read them)
            SteamNetworkingUtils.InitRelayNetworkAccess();
            pollGroup = SteamNetworkingSockets.CreatePollGroup();
        }

        public void OnSendingLargeDataChunk()
        {
            heavyTrafficPause = new Time(2, TimeUnit.Seconds);
        }

        // Helper method to safely pass the byte array to C++
        private void SendDataToHandle(CSteamID targetId, byte[] data, int sendFlags)
        {
            if (connectionHandles.TryGetValue(targetId, out HSteamNetConnection handle))
            {
                unsafe
                {
                    // Pin the byte array in memory so the Garbage Collector doesn't move it while Steam reads it
                    fixed (byte* pData = data)
                    {
                        SteamNetworkingSockets.SendMessageToConnection(
                            handle,
                            (IntPtr)pData,
                            (uint)data.Length,
                            sendFlags,
                            out long msgNum
                        );
                    }
                }
            }
        }
        public bool HasAvailableTrafficSpace(CSteamID targetId)
        {
            // We now look up the specific handle for the user we want to check
            if (connectionHandles.TryGetValue(targetId, out HSteamNetConnection handle))
            {
                SteamNetConnectionRealTimeStatus_t connectionStatus = new SteamNetConnectionRealTimeStatus_t();
                SteamNetConnectionRealTimeLaneStatus_t pLanes = new SteamNetConnectionRealTimeLaneStatus_t();
                EResult result = SteamNetworkingSockets.GetConnectionRealTimeStatus(handle, ref connectionStatus, 0, ref pLanes);

                if (result == EResult.k_EResultOK)
                {
                    int totalPending = connectionStatus.m_cbPendingUnreliable + connectionStatus.m_cbPendingReliable;
                    int estimatedBandwidthBps = connectionStatus.m_nSendRateBytesPerSecond;

                    return totalPending < (estimatedBandwidthBps / 2);
                }
            }
            return false;
        }

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
            // Read up to 32 messages at a time from the Poll Group
            IntPtr[] messages = new IntPtr[32];
            int numMessages = SteamNetworkingSockets.ReceiveMessagesOnPollGroup(pollGroup, messages, messages.Length);

            for (int i = 0; i < numMessages; i++)
            {
                // Marshal the unmanaged C++ pointer into a C# struct
                SteamNetworkingMessage_t netMessage = Marshal.PtrToStructure<SteamNetworkingMessage_t>(messages[i]);
                CSteamID senderId = netMessage.m_identityPeer.GetSteamID();

                // Create a byte array and copy the data from unmanaged memory
                byte[] data = new byte[netMessage.m_cbSize];
                Marshal.Copy(netMessage.m_pData, data, 0, netMessage.m_cbSize);

                // --- YOUR ORIGINAL GAME LOGIC REMAINS HERE ---
                if (data.Length > 1)
                {
                    DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();
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

                                    int packetCount = 2;

                                    if (heavyTrafficPause.TimeOut)
                                    {
                                        if (packet.sender.roundTripTime < 40)
                                        {
                                            packetCount = 32;
                                        }
                                        else if (packet.sender.roundTripTime < 140)
                                        {
                                            packetCount = 8;
                                        }
                                    }

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
                            case PacketType.Basic_MapLoadedAndReady:
                                packet.sender.mapLoadedAndReady = true;
                                Debug.Log(packet.sender.Gamertag + ":: Map Loaded And Ready");
                                break;
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
                                Ref.steam.readVoice(packet.r);
                                break;
                        }
                    }
                    else
                    {
                        //RemovePeer(peer);
                        Ref.netSession.writeKick(peer);//peer.kickFromNetwork();
                    }
                }
                // VERY IMPORTANT: Free the unmanaged memory to prevent memory leaks!
                SteamNetworkingMessage_t.Release(messages[i]);
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
            if (hostSession)
            {
                return Ref.netSession.joinableStatus &&
                    remoteGamers.Count <= SteamLobbyMatchmaker.MAX_LOBBY_MEMBERS &&
                    Ref.gamesett.bannedPeers.isBanned(peer) == false;
            }
            else
            {
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

        public void Send(byte[] data, VikingEngine.Network.PacketReliability rely, SendPacketTo to, CSteamID specificGamerID)
        {
#if DEBUG
            if (data.Length > SteamPackageByteLimit)
            {
                var packet = (PacketType)data[1];
                throw new Exception("Passed steam package limit: " + packet);
            }
#endif
            int sendFlags = rely == Network.PacketReliability.Unrelyable ?
                Constants.k_nSteamNetworkingSend_Unreliable :
                Constants.k_nSteamNetworkingSend_Reliable;

            if (to == SendPacketTo.OneSpecific)
            {
                SendDataToHandle(specificGamerID, data, sendFlags);
            }
            else if (to == SendPacketTo.Host && Host != null)
            {
                SendDataToHandle(Host.SteamID, data, sendFlags);
            }
            else
            {
                // Broadcast to all
                foreach (SteamNetworkPeer peer in remoteGamers)
                {
                    SendDataToHandle(peer.SteamID, data, sendFlags);
                }
            }
        }
        //        public void Send(byte[] data, VikingEngine.Network.PacketReliability rely, SendPacketTo to, CSteamID specificGamerID)
        //        {
        //#if DEBUG
        //            if (data.Length > SteamPackageByteLimit)
        //            {
        //                var packet = (PacketType)data[1];
        //                throw new Exception("Passed steam package limit: " + packet);
        //            }
        //#endif
        //            EP2PSend sendType;

        //            if (rely == Network.PacketReliability.Unrelyable)
        //            {
        //                //SendUnreliable(data);
        //                sendType = EP2PSend.k_EP2PSendUnreliable;
        //            }
        //            else
        //            {
        //                //SendReliable(data);
        //                sendType = EP2PSend.k_EP2PSendReliable;
        //            }

        //            if (to == SendPacketTo.OneSpecific)
        //            {

        //                bool result = SteamNetworking.SendP2PPacket(specificGamerID, data, (uint)data.Length, sendType, 0);
        //            }
        //            else if (to == SendPacketTo.Host)
        //            {
        //                if (Host != null)
        //                {
        //                    SteamNetworking.SendP2PPacket(Host.SteamID, data, (uint)data.Length, sendType, 0);
        //                }
        //            }
        //            else
        //            {
        //                foreach (SteamNetworkPeer peer in remoteGamers)
        //                {
        //                    SteamNetworking.SendP2PPacket(peer.SteamID, data, (uint)data.Length, sendType, 0);
        //                }
        //            }

        //        }

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

            if (Ref.steamlobby.InLobby)
            {
                Ref.NetUpdateReciever().NetworkStatusMessage(NetworkStatusMessage.Created_session);
            }
            else
            {
                Ref.steamlobby.CreateLobby();
            }
        }

        public void disconnectSession()
        {
            // Clean up handles and sockets properly
            foreach (var kvp in connectionHandles)
            {
                SteamNetworkingSockets.CloseConnection(kvp.Value, 0, "Disconnecting", false);
            }
            connectionHandles.Clear();

            if (listenSocket != HSteamListenSocket.Invalid)
            {
                SteamNetworkingSockets.CloseListenSocket(listenSocket);
                listenSocket = HSteamListenSocket.Invalid;
            }

            localPeer = null;
            Host = null;
            endSession();
            disconnectTime.Seconds = 6f;
        }

        public void endSession()
        {
            remoteGamers.Clear();            
            hostSession = false;
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
        // --- NEW: SERVER INITIALIZATION ---
        public void StartListening()
        {
            listenSocket = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, null);
            Debug.Log("Server Listen Socket Created.");
        }

        // --- NEW: CLIENT INITIALIZATION ---
        public void ConnectToServer(CSteamID hostId)
        {
            SteamNetworkingIdentity identity = new SteamNetworkingIdentity();
            identity.SetSteamID(hostId);

            HSteamNetConnection clientHandle = SteamNetworkingSockets.ConnectP2P(ref identity, 0, 0, null);

            // Add to our dictionary immediately so we can track it
            connectionHandles[hostId] = clientHandle;

            // Assign this connection to our poll group so we can read from it
            SteamNetworkingSockets.SetConnectionPollGroup(clientHandle, pollGroup);
            Debug.Log($"Attempting to connect to Host: {hostId}");
        }
        // --- NEW: THE MASTER CONNECTION CALLBACK ---
        void OnConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t callbackData)
        {
            HSteamNetConnection handle = callbackData.m_hConn;
            CSteamID remoteSteamId = callbackData.m_info.m_identityRemote.GetSteamID();

            switch (callbackData.m_info.m_eState)
            {
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting:
                    // A client is trying to join us!
                    if (hostSession)
                    {
                        // Accept them, assign them to the PollGroup, and store their handle
                        SteamNetworkingSockets.AcceptConnection(handle);
                        SteamNetworkingSockets.SetConnectionPollGroup(handle, pollGroup);
                        connectionHandles[remoteSteamId] = handle;
                        Debug.Log($"Incoming connection accepted from {remoteSteamId}");
                    }
                    break;

                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected:
                    // The handshake is complete. Safe to add as a peer and send data.
                    AddPeer(remoteSteamId);
                    Debug.Log($"Successfully connected to {remoteSteamId}");
                    break;

                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer:
                case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally:
                    // Player left or crashed
                    Debug.Log($"Connection closed with {remoteSteamId}. Reason: {callbackData.m_info.m_eEndReason}");
                    SteamNetworkingSockets.CloseConnection(handle, 0, "Closing", false);
                    connectionHandles.Remove(remoteSteamId);
                    RemovePeer(remoteSteamId);
                    break;
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

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                if (remoteGamers[i].SteamID == peerID)
                {
                    Ref.NetUpdateReciever().NetEvent_PeerLost(remoteGamers[i]);
                    remoteGamers.RemoveAt(i--);
                }
            }
        }
    }
}
#endif