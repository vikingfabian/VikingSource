using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Player
{
    class RemoteGamerData
    {
        Network.AbsNetworkPeer gamer;
        public List<GamerData> joinedGamers;

        public RemoteGamerData(Network.AbsNetworkPeer gamer)
        {
            this.gamer = gamer;
            joinedGamers = new List<GamerData> { new GamerData() };
        }

        public override string ToString()
        {
            return "Remote Gamer, Join count: " + joinedGamers.Count.ToString();
        }
    }
}
