#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valve.Steamworks;
using VikingEngine.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.SteamWrapping
{
    class SteamP2PManager
    {
        public const int SteamPackageByteLimit = 1200;

        const float NoResponceTimeKickSeconds = 10;
        public bool autoAcceptSessionRequests = false;
        public int PeerCount { get { return remoteGamers.Count; } }

        public SteamNetworkPeer Host;
        public SteamNetworkPeer localHost;
        public List<AbsNetworkPeer> remoteGamers;
        public bool hostSession = false;

        public bool IsHostingSession => Ref.steamlobby.InLobby && hostSession;

        SteamCallback<P2PSessionConnectFail_t> connectFailCallback;
        SteamCallback<P2PSessionRequest_t> sessionRequestCallback;

        const int LobbyTimeRefreshRateSec = 3;
        public const int LobbyTimeOut = LobbyTimeRefreshRateSec + 3;

        Timer.Basic roundtripTimer = new Timer.Basic(500, true);
        Timer.Basic lobbyTimeRefresh = new Timer.Basic(TimeExt.SecondsToMS(LobbyTimeRefreshRateSec), true);
        public Time disconnectTime = 0;

        public SteamP2PManager()
        {
            autoAcceptSessionRequests = true;
            remoteGamers = new List<AbsNetworkPeer>();
            
            connectFailCallback = new SteamCallback<P2PSessionConnectFail_t>(OnConnectionFail, false);
            sessionRequestCallback = new SteamCallback<P2PSessionRequest_t>(OnSessionRequest, false);
        }

        public void update()
        {
            if (disconnectTime.CountDown())
            {
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
                        //else if (!Ref.steamlobby.hostLobby)
                        //{
                        //    if (Math.Abs(Ref.steamlobby.lobbyTimeDelta()) > LobbyTimeOut)
                        //    {
                        //        Debug.Log("Lobby server time out");
                        //        Ref.netSession.Disconnect();
                        //    }
                        //}
                    }
                }
            }
        }

        void ReadAllPackets()
        {
            uint msgSize = 0;
            while (SteamAPI.SteamNetworking().IsP2PPacketAvailable(out msgSize, 0))
            {
                byte[] data = new byte[msgSize];
                uint bytesRead;
                ulong senderId;

                if (SteamAPI.SteamNetworking().ReadP2PPacket(data, msgSize, out bytesRead, out senderId, 0))
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
                                    var w = Ref.netSession.BeginWritingPacket(PacketType.Steam_ReturnRoundtrip, senderId, PacketReliability.Reliable, null);
                                    w.Write(timestamp);
                                }
                                break;
                            case PacketType.Steam_ReturnRoundtrip:
                                {
                                    float timestamp = packet.r.ReadSingle();
                                    float timePassed = TimeExt.SecondsToMS(Ref.TotalTimeSec - timestamp);
                                    packet.sender.roundTripTime = packet.sender.roundTripTime * 0.5f + timePassed * 0.5f;

                                    Ref.NetUpdateReciever().NetEvent_PingReturned(packet.sender);
                                }
                                break;
                            case PacketType.Steam_SuccesfulJoinPing:
                                {
                                    var w = Ref.netSession.BeginWritingPacket(PacketType.Steam_ReturnRoundtrip, senderId, PacketReliability.Reliable, packet.sender.id);
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
                                RemovePeer(packet.sender.fullId);
                                break;
                            case PacketType.Basic_MapLoadedAndReady:
                                packet.sender.mapLoadedAndReady = true;
                                Debug.Log(packet.sender.Gamertag + ":: Map Loaded And Ready");
                                break;
                            case Network.PacketType.KickPlayer:
                                ulong fullId = packet.r.ReadUInt64();
                                if (localHost.fullId == fullId)
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

        public AbsNetworkPeer getOrCreatePeer(ulong playerId)
        {
            AbsNetworkPeer peer = GetPeer(playerId);
            if (peer == null)
            {
                peer = AddPeer(playerId);
            }

            return peer;
        }

        public AbsNetworkPeer AddPeer(ulong peerId)
        {
            if (localHost == null)
            {
                createLocalHost();
            }

            if (peerId == 0)
            {
                throw new Exception();
            }

            if (peerId == localHost.fullId)
            {
                //local gamer, never add
                return null;
            }

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                if (peerId == remoteGamers[i].fullId) // already added
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

        public SteamNetworkPeer GetLocalHost()
        {
            if (localHost == null)
            {
                createLocalHost();
            }

            return localHost;
        }

        void createLocalHost()
        {
            localHost = new SteamNetworkPeer(SteamAPI.SteamUser().GetSteamID(), true);
        }

        public void RemovePeer(AbsNetworkPeer peer)
        {
            peer.approved = false;

            RemovePeer(peer.FullId);
        }

        public void RemovePeer(ulong steamId)
        {
            SteamAPI.SteamNetworking().CloseP2PSessionWithUser(steamId);

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                var peer = remoteGamers[i];
                if (peer.fullId == steamId)
                {
                    remoteGamers.RemoveAt(i);
                    Ref.NetUpdateReciever().NetEvent_PeerLost(peer);
                    break;
                }
            }

            if (Host == null || Host.FullId == steamId)
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
                Host = localHost;
                localHost.id = 0;

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

                w.Write(localHost.fullId);
                w.Write(localHost.id);
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

                if (localHost.fullId == steamId)
                {
                    localHost.id = id;
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

        public AbsNetworkPeer GetPeer(ulong peerId)
        {
            foreach (var peer in remoteGamers)
            {
                if (peer.fullId == peerId)
                    return peer;
            }

            if (localHost.fullId == peerId)
                return localHost;

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


        public void Send(byte[] data, VikingEngine.Network.PacketReliability rely, SendPacketTo to, ulong specificGamerID)
        {
#if DEBUG
            if (data.Length > SteamPackageByteLimit)
            {
                throw new Exception("Passed steam package limit");
            }
#endif
            EP2PSend sendType;

            if (rely == Network.PacketReliability.Unrelyable)
            {
                //SendUnreliable(data);
                sendType = EP2PSend.k_EP2PSendUnreliable;
            }
            else
            {
                //SendReliable(data);
                sendType = EP2PSend.k_EP2PSendReliable;
            }

            if (to == SendPacketTo.OneSpecific)
            {

                bool result = SteamAPI.SteamNetworking().SendP2PPacket(specificGamerID, data, (uint)data.Length, sendType, 0);
            }
            else if (to == SendPacketTo.Host)
            {
                if (Host != null)
                {
                    SteamAPI.SteamNetworking().SendP2PPacket(Host.fullId, data, (uint)data.Length, sendType, 0);
                }
            }
            else
            {
                foreach (SteamNetworkPeer peer in remoteGamers)
                {
                    SteamAPI.SteamNetworking().SendP2PPacket(peer.fullId, data, (uint)data.Length, sendType, 0);
                }
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
                SteamAPI.SteamNetworking().SendP2PPacket(peer.fullId, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliable, 0);
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
                SteamAPI.SteamNetworking().SendP2PPacket(peer.fullId, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliableNoDelay, 0);
            }
        }

        public void SendReliable(byte[] data)
        {
            foreach (var peer in remoteGamers)
            {
                SteamAPI.SteamNetworking().SendP2PPacket(peer.fullId, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable, 0);
            }
        }

        public void SendReliableWithBuffering(byte[] data)
        {
            foreach (var peer in remoteGamers)
            {
                SteamAPI.SteamNetworking().SendP2PPacket(peer.fullId, data, (uint)data.Length, EP2PSend.k_EP2PSendReliableWithBuffering, 0);
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
            for (int i = remoteGamers.Count - 1; i >= 0; --i)
            {
                RemovePeer(remoteGamers[i]);
            }

            localHost = null;
            Host = null;
            endSession();
            SteamAPI.clearMem();

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
                ulong peerID = sessionRequestInfo.m_steamIDRemote;
                SteamAPI.SteamNetworking().AcceptP2PSessionWithUser(peerID);

                for (int i = 0; i < remoteGamers.Count; ++i)
                {
                    if (remoteGamers[i].fullId == peerID)
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
            ulong peerID = connectionFailInfo.m_steamIDRemote;
            EP2PSessionError error = (EP2PSessionError)connectionFailInfo.m_eP2PSessionError;
            switch (error)
            {
                case EP2PSessionError.k_EP2PSessionErrorNone:
                    break;
                case EP2PSessionError.k_EP2PSessionErrorNotRunningApp:
                    Debug.LogWarning("The remote user isn't running the same game (appID) as you are.");
                    break;
                case EP2PSessionError.k_EP2PSessionErrorNoRightsToApp:
                    Debug.LogWarning("The local user doesn't own this game.");
                    break;
                case EP2PSessionError.k_EP2PSessionErrorDestinationNotLoggedIn:
                    Debug.LogWarning("The remote user doesn't have a connection to Steam.");
                    break;
                case EP2PSessionError.k_EP2PSessionErrorTimeout:
                    Debug.LogWarning("The remote user isn't responding. This could be because no physical connection could be made, or the remote end isn't calling AcceptP2PSessionWithUser()");
                    break;
                default:
                    Debug.LogWarning("The remote user didn't answer, but we got no failure reason. Maybe you are not connected to the internet?");
                    break;
            }

            for (int i = 0; i < remoteGamers.Count; ++i)
            {
                if (remoteGamers[i].fullId == peerID)
                {
                    Ref.NetUpdateReciever().NetEvent_PeerLost(remoteGamers[i]);
                    remoteGamers.RemoveAt(i--);
                }
            }
        }
    }
}
#endif