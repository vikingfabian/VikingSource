#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.Network
{
    class SteamAvailableSession : AbsAvailableSession
    {
        static long ServerTime;
        public static void RefreshServerTime()
        {
            ServerTime = SteamUtils.GetServerRealTime();
        }

        public SteamAvailableSession(CSteamID available)
            :base(available.m_SteamID)
        {
            Ref.steam.LobbyMatchmaker.getMetaData(available, out publicity);

            this.name = SteamMatchmaking.GetLobbyData(available, LobbyDatas.LobbyName.ToString());
            
            this.lobbyHost = SteamMatchmaking.GetLobbyOwner(available).m_SteamID;

            CSteamID steamIDFriend;
            if (Ref.steam.LobbyMatchmaker.lobbyIsFriend(available, out steamIDFriend))
            {
                friend = true;
                lobbyHost = steamIDFriend.m_SteamID;
            }
        }

        public override bool refreshAvailable()
        {
            long lobbyTimeStamp = Ref.steamlobby.GetLobbyTimeStamp(new CSteamID(lobbyId));

            return Math.Abs(lobbyTimeStamp - ServerTime) < SteamP2PManager.LobbyTimeOut;
        }

        public SteamImageLoadData tryLoadGamerIcon()
        {
            SteamImageLoadData steamImage = SteamNetworkPeer.GetAvatarImage(new CSteamID( lobbyHost));
            return steamImage;
        }

        override public void join()
        {
            Ref.steam.LobbyMatchmaker.JoinLobby(new CSteamID( lobbyId));
        }
    }
}
#endif