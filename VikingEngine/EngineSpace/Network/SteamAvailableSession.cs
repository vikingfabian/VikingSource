#if PCGAME
using Microsoft.CodeAnalysis;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //Ref.steam.LobbyMatchmaker.getMetaData(available, out publicity);

            //this.name = SteamMatchmaking.GetLobbyData(available, LobbyDatas.LobbyName.ToString());
            var keys = metaData.GetKeys();
            metaData.Values = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                metaData.Values[i] = SteamMatchmaking.GetLobbyData(available, keys[i]);
            }
            metaData.OnDataRecieved();

            //this.lobbyHost = SteamMatchmaking.GetLobbyOwner(available).m_SteamID;

            if (Ref.steam.LobbyMatchmaker.lobbyIsFriend(metaData.host))
            {
                friend = true;
                //lobbyHost = steamIDFriend.m_SteamID;
            }
        }

        public override bool refreshAvailable()
        {
            long lobbyTimeStamp = Ref.steamlobby.GetLobbyTimeStamp(new CSteamID(lobbyId));

            return Math.Abs(lobbyTimeStamp - ServerTime) < SteamP2PManager.LobbyTimeOut;
        }

        public SteamImageLoadData tryLoadGamerIcon()
        {
            SteamImageLoadData steamImage = SteamNetworkPeer.GetAvatarImage(metaData.host/*new CSteamID( lobbyHost)*/);
            return steamImage;
        }

        override public void join()
        {
            
            Ref.steam.LobbyMatchmaker.JoinLobby(new CSteamID( lobbyId));
        }
    }
}
#endif