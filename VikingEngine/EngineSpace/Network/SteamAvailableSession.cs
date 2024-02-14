#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.SteamWrapping;
using Valve.Steamworks;

namespace VikingEngine.Network
{
    class SteamAvailableSession : AbsAvailableSession
    {
        static long ServerTime;
        public static void RefreshServerTime()
        {
            ServerTime = SteamAPI.SteamUtils().GetServerRealTime();
        }

        public SteamAvailableSession(ulong available)
            :base(available)
        {
            Ref.steam.LobbyMatchmaker.getMetaData(lobbyId, out publicity);

            this.name = SteamAPI.SteamMatchmaking().GetLobbyData(lobbyId, LobbyDatas.LobbyName.ToString());
            
            this.lobbyHost = SteamAPI.SteamMatchmaking().GetLobbyOwner(lobbyId);

            ulong steamIDFriend;
            if (Ref.steam.LobbyMatchmaker.lobbyIsFriend(lobbyId, out steamIDFriend))
            {
                friend = true;
                lobbyHost = steamIDFriend;
            }
        }

        public override bool refreshAvailable()
        {
            long lobbyTimeStamp = Ref.steamlobby.GetLobbyTimeStamp(lobbyId);

            return Math.Abs(lobbyTimeStamp - ServerTime) < SteamP2PManager.LobbyTimeOut;
        }

        public SteamImageLoadData tryLoadGamerIcon()
        {
            SteamImageLoadData steamImage = SteamNetworkPeer.GetAvatarImage(lobbyHost);
            return steamImage;
        }

        override public void join()
        {
            Ref.steam.LobbyMatchmaker.JoinLobby(lobbyId);
        }
    }
}
#endif