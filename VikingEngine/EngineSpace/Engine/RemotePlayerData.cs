using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.SteamWrapping;

namespace VikingEngine.Engine
{
    class RemotePlayerData : AbsPlayerData
    {
        Network.AbsNetworkPeer peer;

        public RemotePlayerData(Network.AbsNetworkPeer peer, int globalIndex = -1)
        {
            this.peer = peer;
            this.globalPlayerIndex = globalIndex;
        }

        override public string PublicName(LoadedFont fontsafe)
        {
            if (peer != null)
            {
                string gamerTag = peer.Gamertag;

                if (fontsafe != LoadedFont.NUM_NON)
                {
                    gamerTag = LoadContent.CheckCharsSafety(gamerTag, fontsafe);
                }

                if (localPlayerIndex > 0)
                {
                    gamerTag += "(" + TextLib.IndexToString(localPlayerIndex) + ")";
                }

                return gamerTag;
            }
            else
            { return "????" + TextLib.IndexToString(localPlayerIndex); }
        }

        public override Network.AbsNetworkPeer netPeer()
        {
            return peer;
        }

        public override string ToString()
        {
            return "remote player data" + "(" + PublicName(LoadedFont.NUM_NON) + ")";
        }

        public override bool equals(AbsPlayerData otherPlayerData)
        {
            return otherPlayerData.Type == this.Type && otherPlayerData.netPeer().FullId == this.peer.FullId;
        }

        override public PlayerType Type { get { return PlayerType.Remote; } }
    }
}
