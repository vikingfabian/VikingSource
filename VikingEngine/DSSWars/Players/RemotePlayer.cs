using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG;
using VikingEngine.DSSWars.Net;

namespace VikingEngine.DSSWars.Players
{
    partial class RemotePlayer
    {
        PlayerCullingState playerCulling;

        public RemotePlayer(Network.AbsNetworkPeer peer)
        {
            peer.Tag = this;
            InitData();
            playerCulling = new PlayerCullingState();
        }

        

        public void Net_readStatus(System.IO.BinaryReader r)
        {
            playerCulling.readNet(r);
        }
    }


}
    
