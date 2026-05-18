#if PCGAME
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.PickUp;
using VikingEngine.Network;


namespace VikingEngine.SteamWrapping
{
    //enum LobbyDatas
    //{
    //    LobbyName,
    //    LobbyDebugState,
    //    LobbyGameVersion
    //}

    //enum LobbyDebugState
    //{
    //    DEBUG,
    //    RELEASE
    //}
    /*
        4. Users stay in a lobby until there are enough players ready to launch the game. Data is communicated between the lobby members about which character they want to play, or other per-user settings. If there are some rules that need to be enforced in the lobby (for example, only one user can play as a certain character), there is one and only one lobby owner who you can use to arbritrate that.
        5. There may or may not be a user interface associated with the lobby; if there is, the lobby data communications functions can be used to send chat messages between lobby members. Voice data can also be sent, but needs to sent using the peer-to-peer networking API.
        6. Once the game is ready to launch, the users all join the game server, or connect to the user nominated to host the game, and then leave the lobby. Once all users have left a lobby, it is automatically destroyed.
    */
    class SteamLobbyMatchmaker
    {
        

        public const int MAX_LOBBY_MEMBERS =
#if CCG
            2;
#elif DSS
            64;//16;
#elif PJ
            9;
#else
            4;
#endif

        
        /* Fields */
        public CSteamID currentLobbyID = CSteamID.Nil;
        public CSteamID inviteFromLobby = CSteamID.Nil;
        public bool hostLobby = false;
       
        public bool InLobby { get { return currentLobbyID != CSteamID.Nil; } }
        public bool HostingLobby => currentLobbyID != CSteamID.Nil && hostLobby;

        CallResult<LobbyMatchList_t> callResultLobbyMatchList;
        CallResult<LobbyCreated_t> callResultLobbyCreated;

        Callback<GameLobbyJoinRequested_t> callbackJoinRequest;
        
        CallResult<LobbyEnter_t> callResultLobbyEnter;
        Callback<LobbyChatMsg_t> callbackLobbyChatMsg;
        Callback<LobbyChatUpdate_t> callbackLobbyChatUpdate;
        Callback<LobbyDataUpdate_t> callbackLobbyDataUpdate;

        List<string> unresponsiveLobbies = new List<string>();
       
        

        /* Constructor */
        public SteamLobbyMatchmaker()
        {
            callResultLobbyMatchList = new CallResult<LobbyMatchList_t>(OnLobbyMatchList);
            callResultLobbyCreated = new CallResult<LobbyCreated_t>(OnLobbyCreated);
            callResultLobbyEnter = new CallResult<LobbyEnter_t>(OnLobbyJoined);

            callbackJoinRequest = new Callback<GameLobbyJoinRequested_t>(onInvite, false);
            
            callbackLobbyChatMsg = new Callback<LobbyChatMsg_t>(OnLobbyChatMessage, false);
            callbackLobbyChatUpdate = new Callback<LobbyChatUpdate_t>(OnLobbyChatUpdate, false);
            callbackLobbyDataUpdate = new Callback<LobbyDataUpdate_t>(OnLobbyDataUpdate, false);            
        }

        public void SetLobbyFilters()
        {
            // 1. Usually recommended to let user decide on whom to play with etc and set up some options, but we skip this for now.
            // SteamMatchmaking()->AddRequestLobbyListFilter*() functions would be called here, before a call to FindLobbies

            //SteamMatchmaking.AddRequestLobbyListStringFilter(LobbyDatas.LobbyGameVersion.ToString(), PlatformSettings.SteamNetworkVersion.ToString(), ELobbyComparison.k_ELobbyComparisonEqual);
        }

        public void InviteSteamUserToLobbyDialog()
        {
            SteamFriends.ActivateGameOverlay("LobbyInvite");
        }

        public void SetLobbyPublicity(LobbyPublicity value)
        {
            var oldValue = lobbyPublicity;
            lobbyPublicity = value;

            if (value != oldValue)
            {
                if (IsPublicNetwork(value) != IsPublicNetwork(oldValue))
                {
                    Ref.netSession.Disconnect("Lobby Publicity reset");
                }
                else
                {
                    refreshMetaData();
                }
            }
        }

        static bool IsPublicNetwork(LobbyPublicity publicity)
        {
            return publicity != LobbyPublicity.Private;
        }

        public void CreateLobbyIfNotInOne()
        {
            if (currentLobbyID == CSteamID.Nil && Ref.steam.P2PManager.disconnectTime.TimeOut)
            {
                CreateLobby();
            }
        }

        public void CreateLobby()
        {
            if (currentLobbyID != CSteamID.Nil)
            {
                Debug.LogWarning("Replacing already existing lobby");
                LeaveCurrentLobby();
            }

            ELobbyType type;
            switch (lobbyPublicity)
            {
                default:
                case LobbyPublicity.Private:
                    type = ELobbyType.k_ELobbyTypePrivate;
                    break;
                case LobbyPublicity.FriendsOnly:
                    type = ELobbyType.k_ELobbyTypeFriendsOnly;
                    break;
                case LobbyPublicity.Public:
                    type = ELobbyType.k_ELobbyTypePublic;
                    break;
            }
            //ELobbyType type = IsPublicNetwork(lobbyPublicity) ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypePrivate;

            Debug.Log("Creating lobby...");
            SteamAPICall_t result = SteamMatchmaking.CreateLobby(type, MAX_LOBBY_MEMBERS);
            callResultLobbyCreated.Set(result, OnLobbyCreated);
        }

        public void setJoinable(bool joinable)
        {
            if (currentLobbyID != CSteamID.Nil)
            {
                SteamMatchmaking.SetLobbyJoinable(currentLobbyID, joinable);
            }
        }

        void OnLobbyCreated(LobbyCreated_t lobbyCreated, bool ioFailure)
        {
            Debug.Log("--On Lobby Created(?): " + lobbyCreated.m_eResult.ToString());
            if (ioFailure || lobbyCreated.m_eResult == EResult.k_EResultAccessDenied)
            {
                statusMessage(Network.NetworkStatusMessage.Create_Lobby_Failed);
                return;
            }

            statusMessage(Network.NetworkStatusMessage.Created_Lobby);

            currentLobbyID = new CSteamID( lobbyCreated.m_ulSteamIDLobby);
            hostLobby = true;

            //string userName = SteamFriends.GetPersonaName();

            //SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyDatas.LobbyName.ToString(), userName);
            SteamMatchmaking.SetLobbyOwner(currentLobbyID, SteamUser.GetSteamID());

           
            //SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyDatas.LobbyGameVersion.ToString(), PlatformSettings.SteamNetworkVersion.ToString());
            refreshMetaData();

            setJoinable(Ref.netSession.joinableStatus);

            if (Ref.p2p.hostSession)
            {
                Ref.steamlobby.updateLobbyTime(true);
                statusMessage(Network.NetworkStatusMessage.Created_session);

                // --- ADD THIS (or call a method in your P2PManager that does this) ---
                // Create the Listen Socket so clients can connect to you.
                //Ref.p2p.StartListening();//TODO
            }
        }


        public void updateLobbyTime(bool connected)
        {
            uint time = 0;
            if (connected)
            {
                time = SteamUtils.GetServerRealTime();
            }
            SteamMatchmaking.SetLobbyData(currentLobbyID, AbsLobbyMetaData.LobbyTimeDataKey, time.ToString());
        }

        //public long lobbyTimeDelta()
        //{
        //    long lobbyTime = GetLobbyTimeStamp(currentLobbyID);
        //    long serverTime = SteamUtils.GetServerRealTime();

        //    return lobbyTime - serverTime;
        //}

        void refreshMetaData()
        {
            var meta = Ref.NetUpdateReciever().NetEvent_StartLobbyMetaData();
            meta.CollectValues();
            var keys = meta.GetKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                SteamMatchmaking.SetLobbyData(currentLobbyID, keys[i], meta.Values[i]);
            }

            //    int publicType = (int)lobbyPublicity;

            //    SteamMatchmaking.SetLobbyData(currentLobbyID, LobbyPublicityDataKey, publicType.ToString());
        }

            //Key Length(k_nMaxLobbyKeyLength): Maximum of 255 characters.

            //Value Length(k_cubChatMetadataMax): Maximum of 8,192 bytes.

            //public void getMetaData(CSteamID lobbyID, out LobbyPublicity publicity)
            //{
            //    publicity = LobbyPublicity.ERROR;

            //    string data = SteamMatchmaking.GetLobbyData(lobbyID, LobbyPublicityDataKey);

            //    if (data != null && data.Length > 0)
            //    {
            //        int ptype = Convert.ToInt32(data);
            //        publicity = (LobbyPublicity)ptype;
            //    }
            //}

            void statusMessage(Network.NetworkStatusMessage message)
        {
            if (PlatformSettings.DevBuild)
            {
                Debug.Log("Steam network:: " + message.ToString());
            }
            Ref.NetUpdateReciever().NetworkStatusMessage(message);
        }

        public void FindLobbies()
        {
            if (currentLobbyID == CSteamID.Nil)
            {
                Debug.LogWarning("FindLobbies - you got no lobby");
            }

            SetLobbyFilters();
            statusMessage(Network.NetworkStatusMessage.Searching_Session);

            // this call can take from 300ms to 5 seconds to complete, with a timeout of 20 seconds.
            SteamAPICall_t lobbyRequest = SteamMatchmaking.RequestLobbyList();
            callResultLobbyMatchList.Set(lobbyRequest, OnLobbyMatchList);
        }

        void OnLobbyMatchList(LobbyMatchList_t lobbyMatchList, bool ioFailure)
        {
            if (ioFailure)
            {
                Debug.LogError("Failed getting lobby match list.");
                return;
            }

            Debug.Log("Lobby match list found!");

            sortFoundLobbies(lobbyMatchList);
            
        }

        List<AbsAvailableSession> availableSessionsList = new List<AbsAvailableSession>();
        void sortFoundLobbies(LobbyMatchList_t lobbyMatchList)
        {
            SteamAvailableSession.RefreshServerTime();
            //long serverTime = SteamUtils.GetServerRealTime();
            //List<AbsAvailableSession> availableSessionsList = null;

            int count = (int)lobbyMatchList.m_nLobbiesMatching;
            if (count > 0)
            {
                var rawList = new Dictionary<ulong, AbsAvailableSession>(count);
                for (int i = 0; i < count; ++i)
                {
                    CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex((int)i);

                    if (lobbyID != VikingEngine.Ref.steam.LobbyMatchmaker.currentLobbyID)
                    {
                        SteamAvailableSession session = new SteamAvailableSession(lobbyID);
                        bool canBeListed = true;

                        if (unresponsiveLobbies.Contains(session.metaData.name))
                        {
                            canBeListed = false;
                        }

                        if (session.metaData.lobbyPublicity == LobbyPublicity.FriendsOnly && session.friend == false)
                        { //has locked out anyone that is not friend
                            canBeListed = false;
                        }

                        if (session.refreshAvailable() == false)
                        {
                            canBeListed = false;
                        }

                        if (canBeListed)
                        {
                            rawList.Add(lobbyID.m_SteamID, session);
                        }   
                    }
                }

                for (int i = availableSessionsList.Count -1; i>= 0; --i)//foreach (var prev in availableSessionsList)
                {
                    var prev = availableSessionsList[i];
                    if (rawList.TryGetValue(prev.lobbyId, out var availableSession))
                    {
                        availableSessionsList[i] = availableSession;
                        rawList.Remove(prev.lobbyId);
                    }
                    else
                    {
                        
                        prev.IsAvailable = false;
                        availableSessionsList.RemoveAt(i);
                    }
                }

                foreach (var kv in rawList)
                {
                    availableSessionsList.Add(kv.Value);
                }

                //Sort, clear out doublettes
                //availableSessionsList = new List<AbsAvailableSession>(rawList.Count);
                //foreach (var unsortedMember in rawList)
                //{
                //    bool contains = false;
                //    foreach (var sortedMember in availableSessionsList)
                //    {
                //        if (sortedMember.Equals(unsortedMember))
                //        {
                //            contains = true;
                //            break;
                //        }
                //    }

                //    if (!contains)
                //    {
                //        availableSessionsList.Add(unsortedMember);
                //    }
                //}

                

                Debug.Log("Lobby sorted count: " + availableSessionsList.Count.ToString());
            }


            if (availableSessionsList.Count > 0)
            {
                statusMessage(Network.NetworkStatusMessage.Found_Session);
                
            }
            else
            {
                statusMessage(NetworkStatusMessage.Found_No_Session);
                //Ref.NetUpdateReciever().NetEvent_SessionsFound(null, this.availableSessionsList);
            }
            Ref.NetUpdateReciever().NetEvent_SessionsFound(availableSessionsList);

            this.availableSessionsList = availableSessionsList;
        }

        public bool lobbyIsFriend(CSteamID lobbyId, out CSteamID steamIDFriend)
        {
            steamIDFriend = CSteamID.Nil;

            int cFriends = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
            for (int i = 0; i < cFriends; i++)
            {
                FriendGameInfo_t friendGameInfo;
                steamIDFriend = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);

                SteamFriends.GetFriendGamePlayed(steamIDFriend, out friendGameInfo);

                if (friendGameInfo.m_steamIDLobby == lobbyId)
                {
                    return true;
                }
            }

            return false;
        }

        void onInvite(GameLobbyJoinRequested_t args)
        {
            Debug.Log("Got invite! From: " + args.m_steamIDLobby.ToString());
            inviteFromLobby = args.m_steamIDLobby;
            JoinLobby(args.m_steamIDLobby);
        }

        //public static string lobbyName(CSteamID lobbyID)
        //{
        //    return SteamMatchmaking.GetLobbyData(lobbyID, LobbyDatas.LobbyName.ToString());
        //}
        
        public void JoinLobby(CSteamID lobbyID)
        {
            LeaveCurrentLobby();

            //string name = SteamMatchmaking.GetLobbyData(lobbyID, LobbyDatas.LobbyName.ToString());

            Debug.Log("Attempting to join lobby (" + lobbyID.ToString() + ")");
            SteamAPICall_t result = SteamMatchmaking.JoinLobby(lobbyID);

            //isHost = false;

            callResultLobbyEnter.Set(result, OnLobbyJoined);
        }

        void OnLobbyJoined(LobbyEnter_t lobbyEnter, bool ioFailure)
        {
            if (ioFailure)
            {
                statusMessage(Network.NetworkStatusMessage.Joining_failed);
                return;
            }
            hostLobby = false;
            Ref.steam.P2PManager.endSession();
            //Ref.steam.P2PManager.remoteGamers.Clear();
            //currentLobbyID = lobbyEnter.m_ulSteamIDLobby;
            ConnectToLobbyMembers(new CSteamID( lobbyEnter.m_ulSteamIDLobby));
            CSteamID lobbyHost = SteamMatchmaking.GetLobbyOwner(new CSteamID( lobbyEnter.m_ulSteamIDLobby));
            string name = lobbyEnter.m_ulSteamIDLobby.ToString();//SteamMatchmaking.GetLobbyData(new CSteamID(lobbyEnter.m_ulSteamIDLobby), LobbyDatas.LobbyName.ToString());

            bool fromInvite = lobbyEnter.m_ulSteamIDLobby == inviteFromLobby.m_SteamID;
            Ref.NetUpdateReciever().NetEvent_JoinedLobby(name, lobbyHost.m_SteamID, fromInvite);
            Ref.netSession.BeginWritingPacket(Network.PacketType.Steam_SuccesfulJoinPing, Network.PacketReliability.Reliable);

            currentLobbyID = new CSteamID( lobbyEnter.m_ulSteamIDLobby);
            //refreshHostStatus();
            statusMessage(Network.NetworkStatusMessage.Joining_session);

            if (fromInvite)
            {
                //Ref.steam.P2PManager.getOrCreatePeer(lobbyHost);
                new Timer.TimedAction0ArgTrigger(inviteAccept, 500);       
            }
            Debug.Log("Join Lobby Success: " + name + ", id: " + currentLobbyID.ToString());
        }        

        void inviteAccept()
        {
            if (Ref.steam.P2PManager.remoteGamers.Count == 0)
            {
                Debug.LogError("Accept invite sent to zero receptors");
            }
            Ref.netSession.BeginWritingPacket(PacketType.Steam_InviteAccepted, PacketReliability.Reliable);
        }

        void ConnectToLobbyMembers(CSteamID currentLobbyID)
        {
            //CSteamID lobbyHost = SteamMatchmaking.GetLobbyOwner(currentLobbyID);
            //CSteamID myID = SteamUser.GetSteamID();

            // User info
            int memberCount = SteamMatchmaking.GetNumLobbyMembers(currentLobbyID);
            for (int i = 0; i < memberCount; ++i)
            {
                CSteamID userID = SteamMatchmaking.GetLobbyMemberByIndex(currentLobbyID, i);
                if (userID != CSteamID.Nil)
                {
                    //if (!hostLobby && userID == lobbyHost && userID != myID)
                    //{
                    //    // Here is where you call SteamNetworkingSockets.ConnectP2P()
                    //    // Example: Ref.p2p.ConnectToServer(userID); 
                    //    // That method should return and store your HSteamNetConnection handle.
                    //}
                    Debug.Log("Lobby member: " + SteamFriends.GetFriendPersonaName(userID));
                    Ref.p2p.AddPeer(userID);
                }
            }
        }

        public void disconnect()
        {
            LeaveCurrentLobby();
            hostLobby = false;
           
        }

        public void LeaveCurrentLobby()
        {
            if (currentLobbyID != CSteamID.Nil)
            {
                if (hostLobby)
                {
                    //Ref.steamlobby.updateLobbyTime(false);
                    //Ref.steamlobby.SetLobbyFilters
                    SteamMatchmaking.SetLobbyData(currentLobbyID, AbsLobbyMetaData.LobbyAliveDataKey, bool.FalseString);
                }
                Debug.Log("Leaving current lobby");
                SteamMatchmaking.LeaveLobby(currentLobbyID);
                //Ref.NetUpdateReciever().NetEvent_ConnectionLost();
                currentLobbyID.Clear();

            }
        }

        public void SetLobbyAsUnresponsive()
        {
            if (currentLobbyID != CSteamID.Nil)
            {
                //string lobbyname = SteamMatchmaking.GetLobbyData(currentLobbyID, LobbyDatas.LobbyName.ToString());
                string lobbyname = currentLobbyID.ToString();
                Debug.Log("Adding unresponsive lobby: " + lobbyname);
                unresponsiveLobbies.Add(lobbyname);
            }
        }

        public void SendDbgChat()
        {
            string msg = Ref.rnd.Int().ToString();

            byte[] bytes = EncodeString(msg);

            if (SteamMatchmaking.SendLobbyChatMsg(currentLobbyID, bytes, bytes.Length))
            {
                Debug.Log("Broadcasted MSG:[" + msg + "]");
            }
            else
            {
                Debug.Log("Failed broadcasting MSG:[" + msg + "]");
            }
        }

        void OnLobbyChatMessage(LobbyChatMsg_t lobbyChatMsg)
        {
            CSteamID sendingUserID;
            int maxMsgByteCount = 4096;
            byte[] msgArena = new byte[4096];
            EChatEntryType chatEntryType;
            int bytesUsed = SteamMatchmaking.GetLobbyChatEntry(currentLobbyID, (int)lobbyChatMsg.m_iChatID, out sendingUserID, msgArena, maxMsgByteCount, out chatEntryType);

            string msg = DecodeString(msgArena, bytesUsed);

            Debug.Log("Received message MSG:[" + msg + "]");
        }

        void OnLobbyChatUpdate(LobbyChatUpdate_t lobbyChatUpdate)
        {
            if (lobbyChatUpdate.m_ulSteamIDLobby == currentLobbyID.m_SteamID)
            {
                //ulong changedGamerID = lobbyChatUpdate.m_ulSteamIDUserChanged;
                ////ulong changerID = new ulong(lobbyChatUpdate.m_ulSteamIDMakingChange);
                //uint changeFlags = lobbyChatUpdate.m_rgfChatMemberStateChange;

                //string changedName = SteamFriends.GetFriendPersonaName(changedGamerID);
                ////string changerName = SteamFriends.GetFriendPersonaName(changerID);

                //if (((uint)EChatMemberStateChange.k_EChatMemberStateChangeBanned & changeFlags) != 0)
                //{
                //    Debug.Log(changedName + " was banned (chat)");
                //    Ref.steam.P2PManager.RemovePeer(changedGamerID);
                //}
                //if (((uint)EChatMemberStateChange.k_EChatMemberStateChangeDisconnected & changeFlags) != 0)
                //{
                //    Debug.Log(changedName + " was disconnected (chat)");
                //    Ref.steam.P2PManager.RemovePeer(changedGamerID);
                //}
                //if (((uint)EChatMemberStateChange.k_EChatMemberStateChangeEntered & changeFlags) != 0)
                //{
                //    Ref.steam.P2PManager.AddPeer(changedGamerID);
                //    Debug.Log(changedName + " entered (chat)");
                //}
                //if (((uint)EChatMemberStateChange.k_EChatMemberStateChangeKicked & changeFlags) != 0)
                //{
                //    Debug.Log(changedName + " was kicked (chat)");
                //    Ref.steam.P2PManager.RemovePeer(changedGamerID);
                //}
                //if (((uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft & changeFlags) != 0)
                //{
                //    Debug.Log(changedName + " left (chat)");
                //    Ref.steam.P2PManager.RemovePeer(changedGamerID);
                //}
            }
        }

        public long GetLobbyTimeStamp(CSteamID lobbyId)
        {
            string time = SteamMatchmaking.GetLobbyData(lobbyId, AbsLobbyMetaData.LobbyTimeDataKey);
            if (TextLib.IsEmpty(time))
            {
                return 0;
            }

            long lobbyTimeStamp = Convert.ToInt64(time);
            return lobbyTimeStamp;
        }

        void OnLobbyDataUpdate(LobbyDataUpdate_t lobbyDataUpdate)
        {
            if (lobbyDataUpdate.m_bSuccess != 0)
            {
                //Debug.Log("Received a lobby data update.");
            }
        }

        public static byte[] EncodeString(string msg)
        {
            byte[] bytes = new byte[2 * msg.Length];
            for (int i = 0;
                i < msg.Length;
                ++i)
            {
                bytes[2 * i] = (byte)msg[i];
                bytes[2 * i + 1] = (byte)(((ushort)msg[i]) >> 8);
            }
            return bytes;
        }

        public static string DecodeString(byte[] msgArena, int bytesUsed)
        {
            string msg = "";
            for (int i = 0;
                i < bytesUsed;
                i += 2)
            {
                msg += (char)(((byte)msgArena[i]) |
                             (((byte)msgArena[i + 1]) << 8));
            }
            return msg;
        }
    }

    
}
#endif