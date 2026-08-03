using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Network
{
    abstract class AbsAvailableSession
    {
        public bool IsAvailable = true;

        public ulong lobbyId;
        //public ulong lobbyHost;
        //public string name;
        public bool friend = false;
        //public LobbyPublicity publicity;
        public AbsLobbyMetaData metaData;

        public AbsAvailableSession(ulong available)
        {
            this.lobbyId = available;

#if DSS
            metaData = new VikingEngine.DSSWars.Net.LobbyMetaData();
#endif
        }

        abstract public bool refreshAvailable();
        //public SteamImageLoadData tryLoadGamerIcon()
        //{
        //    SteamImageLoadData steamImage = SteamNetworkPeer.GetAvatarImage(lobbyHost);
        //    return steamImage;
        //}

        abstract public void join();

        public string hostName
        {
            get { return metaData.name; }
        }

        public override bool Equals(object obj)
        {
            AbsAvailableSession other = (AbsAvailableSession)obj;
            return this.lobbyId == other.lobbyId;
        }

        public override int GetHashCode()
        {
            return (int)lobbyId;
        }
    }
    
}
