using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using System.Threading;

namespace VikingEngine.Network
{
    class Session
    {
        const int VersionPropertyIndex = 0;
        const int WhoCanJoinPropertyIndex = 1;
        public const int NumReservedProperties = 2;

        public static int relyableBytesSent = 0;
        public static int unrelyableBytesSent = 0;

        TextG debugText;
        public TextG DebugText
        {
            set { debugText = value; }
        }
        Timer.Basic oneSecUpdate = new Timer.Basic(1000);

        public const int SelectRemoteGamerDialogue = 181918;
        public int maxGamers = 5;
        public bool AllowVoiceChat = true;
        public int maxLocalGamers = 4;

        

        /// <summary>
        /// Äger lobbyn, false om man är client.
        /// Notera att man som edge case kan bli lobby owner om host lämnar lobbyn samtidigt som man joinar.
        /// </summary>
        public bool IsLobbyOwner
        {
            get
            {
#if PCGAME
                ulong lobbyId = Ref.steam.LobbyMatchmaker.currentLobbyID;
                if (lobbyId != 0 && Ref.steam.P2PManager.localHost != null)
                {
                    return Valve.Steamworks.SteamAPI.SteamMatchmaking().GetLobbyOwner(lobbyId) == Ref.steam.P2PManager.localHost.fullId;
                }
#endif
                return false;
            }
        }

        public int RemoteGamersCount
        {
            get
            {
#if PCGAME
                if (Ref.steam.isNetworkInitialized)
                {
                    return Ref.steam.P2PManager.remoteGamers.Count;
                }
#endif
                return 0;

            }
        }

        public bool HasInternet
        {
            get
            {
#if PCGAME
                return Ref.steam.isNetworkInitialized && (Ref.steam.LobbyMatchmaker.currentLobbyID != 0 || InMultiplayerSession);
#else
                return false;
#endif

            }
        }

        public bool ableToConnect
        {
            get
            {
#if PCGAME
                return Ref.steam.isNetworkInitialized;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Håller i eller har joinat ett nätverk med någon annan i
        /// </summary>
        public bool InMultiplayerSession
        {
            get
            {
#if PCGAME
                return Ref.steam.isNetworkInitialized && Ref.steam.P2PManager.remoteGamers.Count > 0;
#else
                return false;
#endif
            }
        }

        public bool GotSessionId
        {
            get
            {
#if PCGAME
                if (Ref.steam.isNetworkInitialized)
                {
                    if (Ref.steam.LobbyMatchmaker.hostLobby)
                    {
                        return true;
                    }
                    else
                    {
                        return Ref.steam.P2PManager.localHost != null &&
                            Ref.steam.P2PManager.localHost.GotAssignedId;
                    }
                }
#endif
                return false;
            }
    }

    /// <summary>
    /// Håller i en MP grupp
    /// </summary>
    public bool IsHost { get {
#if PCGAME
                return  Ref.steam.isNetworkInitialized && Ref.steam.LobbyMatchmaker.hostLobby;
#else
                return false;
#endif
            } }

        /// <summary>
        /// Har joinat MP grupp
        /// </summary>
        public bool IsClient { get { return InMultiplayerSession && !IsHost; } }

        public void Disconnect(string reason)
        {
            // Fabian skriv ett paket och skicka det över reliable, ta emot det på andra sidan och ta bort dem från P2Plistan.
            // Skicka med om det var host som lämnade för då måste någon ny få host-rollen.
            // Använd boolen Ref.Steam.LobbyMatchmaker.isHost så slipper vi klydd med state desynch-buggar.
            //Ref.steam.peerToPeerManager.SendReliable(/*din byte[] här :)*/);

//Exit current session
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
            {
                BeginWritingPacket(PacketType.PlayerDisconnected, PacketReliability.Reliable);
                SendAllQuedPackets();

                Ref.steam.LobbyMatchmaker.disconnect();
                Ref.steam.P2PManager.disconnectSession();

                Ref.NetUpdateReciever().NetEvent_ConnectionLost(reason);
            }
#endif
        }

        public void kickFromNetwork(AbsNetworkPeer peer)
        {
            writeKick(peer);
            peer.approved = false;

            Ref.NetUpdateReciever().NetEvent_PeerLost(peer);
        }

        public void writeKick(AbsNetworkPeer kickPeer)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.KickPlayer,
                Network.PacketReliability.Reliable);
            w.Write(kickPeer.fullId);
        }

        public void Invite()
        {
#if PCGAME
            if (Ref.steam.LobbyMatchmaker.currentLobbyID != 0)
            {
                Valve.Steamworks.SteamAPI.SteamFriends().ActivateGameOverlayInviteDialog(
                    Ref.steam.LobbyMatchmaker.currentLobbyID);
            }
#endif
        }

        public AbsNetworkPeer Host()
        {
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
                return Ref.steam.P2PManager.Host;
#endif
            return null;
        }
        public AbsNetworkPeer LocalHost()
        {
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
            {
                return Ref.steam.P2PManager.GetLocalHost();
            }
#endif
            return null;
        }

        public AbsNetworkPeer EmptyToOfflinePeer(AbsNetworkPeer peer)
        {
            if (peer == null)
            {
                return new OfflinePeer();
            }
            else
            {
                return peer;
            }
        }

        public List<AbsNetworkPeer> RemoteGamers()
        {
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
                return Ref.steam.P2PManager.remoteGamers;
#endif
            return null;
        }

        public AbsNetworkPeer GetPeer(ulong fullId)
        {
#if PCGAME
            return Ref.steam.P2PManager.GetPeer(fullId);
#else
            return null;
#endif
            

        }

        public LobbyPublicity LobbyPublicity
        {
            get
            {
#if PCGAME
                if (Ref.steam.isNetworkInitialized)
                    return Ref.steam.LobbyMatchmaker.lobbyPublicity;
                else
                    return LobbyPublicity.ERROR;
#else
                return LobbyPublicity.ERROR;
#endif
            }

            set
            {
#if PCGAME
                Ref.steam.LobbyMatchmaker.SetLobbyPublicity(value);
#else
                throw new NotImplementedException();
#endif
            }
        }

        public bool joinableStatus = true;

        public void setLobbyJoinable(bool canJoin)
        {
            joinableStatus = canJoin;
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
            {
                Ref.steam.LobbyMatchmaker.setJoinable(joinableStatus);
            }
#else
            throw new NotImplementedException();
#endif
        }

        //PacketReliability currentRely;
        public Settings settings;

       public bool IsHostOrOffline
        { get {
            return !InMultiplayerSession || IsHost;
        } }
        
        //PacketType currentWritingType;

        //public PacketType CurrentWritingType
        //{ get { return currentWritingType; } }
        
        public System.IO.BinaryWriter BeginWritingPacket_Asynch(PacketType type, PacketReliability relyability, out SteamWrapping.SteamWriter packet)
        {
            packet = new SteamWrapping.SteamWriter(relyability, false, SendPacketTo.All, 0);
            return packet.writeHead(type, null);
        }
        
        public System.IO.BinaryWriter BeginWritingPacket(PacketType Type, PacketReliability relyability)
        {
            return BeginWritingPacket(Type, relyability, null);
        }


        public System.IO.BinaryWriter BeginWritingPacket(PacketType type, SendPacketToOptions to, PacketReliability relyability, int? player)
        {
            return BeginWritingPacket(type, to.To, to.SpecificGamerID, relyability, player);
        }

        public System.IO.BinaryWriter BeginWritingPacket(PacketType type, ulong? to, PacketReliability relyability, int? player)
        {
            Network.SendPacketTo sendToType;
            ulong toGamer = 0;
            if (to == null)
            {
                sendToType = Network.SendPacketTo.All;
            }
            else
            {
                sendToType = Network.SendPacketTo.OneSpecific;
                toGamer = to.Value;
            }
            return BeginWritingPacket(type, sendToType, toGamer, relyability, player);
        }


        public System.IO.BinaryWriter BeginWritingPacket(PacketType type, PacketReliability relyability, int? player)
        {
            return BeginWritingPacket(type, SendPacketTo.All, 0, relyability, player);
        }
        public System.IO.BinaryWriter BeginWritingPacket(PacketType type, SendPacketTo to, ulong specificGamerID, 
            PacketReliability relyability, int? sender)
        {
            SteamWrapping.SteamWriter stream = new SteamWrapping.SteamWriter(relyability, true, to, specificGamerID);
            return stream.writeHead(type, sender);
        }

        public System.IO.BinaryWriter BeginWritingPacketToHost(PacketType Type, PacketReliability relyability, int? player)
        {
            return BeginWritingPacket(Type, SendPacketTo.Host, 0, relyability, player);
        }

        public void SendAllQuedPackets()
        {
            Ref.update.TriggerAllSteamWriters();
        }

        public void setLobbyData(string key, int data)
        {
#if PCGAME
            Valve.Steamworks.SteamAPI.SteamMatchmaking().SetLobbyData(Ref.steamlobby.currentLobbyID, key, data.ToString());
#endif
        }

        public int getLobbyIntData(string key, ulong lobbyID)
        {
#if PCGAME
            string data = Valve.Steamworks.SteamAPI.SteamMatchmaking().GetLobbyData(lobbyID, key);
            return Convert.ToInt32(data);
#endif
            return int.MinValue;
        }
        
        float nextNetUpdateTime = 0;
        public float netUpdateRate = 1000; 

        public void Time_Update(float time)
        {
            if (Ref.netSession.InMultiplayerSession)
            {
                nextNetUpdateTime += Ref.DeltaTimeMs;
                if (nextNetUpdateTime >= netUpdateRate)
                {
                    nextNetUpdateTime = 0;
                    int count = RemoteGamersCount;

                    if (count >= 32)
                    {
                        netUpdateRate = 300;
                    }
                    else if (count >= 8)
                    {
                        netUpdateRate = 200;
                    }
                    else
                    {
                        netUpdateRate = 120;
                    }
                    Ref.gamestate.NetUpdate();
                }
            }
        }

        public void GracefulExitSession()
        {
            if (Ref.netSession.InMultiplayerSession)
            {
#if PCGAME
                if (PlatformSettings.SteamAPI)
                {
                    var update = Ref.update.updateCounter.IClone();
                    while (update.Next())
                    {
                        var member = update.GetSelection;
                        if (member is VikingEngine.SteamWrapping.SteamWriter)
                        {
                            member.Time_Update(0f);
                            Ref.update.AddToOrRemoveFromUpdate(member, false);
                        }
                    }
                }
#endif
            }
            
        }
        
        int localHostingPlayer = -1;
        public int LocalHostingPlayer
        { get { return localHostingPlayer; } }
       
        public Session()
        {
            if (Ref.netSession != null)
            {
                throw new Exception();
            }
            Ref.netSession = this;
        }

        bool searchingSessions = false;
        public bool SearchingSessions { get { return searchingSessions; } }
       
        void StatusMessage(NetworkStatusMessage message, bool threaded)
        {
            if (threaded)
                new Timer.Action1ArgTrigger<NetworkStatusMessage>(StatusMessage, message);
            else
                StatusMessage(message);
        }

        void StatusMessage(NetworkStatusMessage message)
        {
            Debug.CrashIfThreaded();
            
            Ref.NetUpdateReciever().NetworkStatusMessage(message);
        }
        
        public void sortFriendsOnlyLobbies(List<Network.AbsAvailableSession> availableSessions)
        {
            if (availableSessions != null)
            {
                for (int i = availableSessions.Count - 1; i >= 0; --i)
                {
                    if (availableSessions[i].friend == false)
                    {
                        availableSessions.RemoveAt(i);
                    }
                }
            }
        }
    }
    
    enum NetworkStatusMessage
    {
        Need_to_sign_in,
        Need_Gold_membership,
        Need_Online_Privilege,
        All_players_need_gold,
        Searching_Session,
        Found_Session,
        Found_No_Session,
        Created_Lobby,
        Create_Lobby_Failed,
        Created_session,
        Joining_session,
        Joining_failed,
        Joining_timed_out,
        Double_join_error,
        Session_ended,
        NUM_NON,
    }
    enum PacketReliability
    {
        Reliable,
        ReliableLasy,
        Unrelyable,
        Chat,
        NUM
    }
}
