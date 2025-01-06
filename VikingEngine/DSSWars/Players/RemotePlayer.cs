using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG;
using VikingEngine.DSSWars.Net;
using VikingEngine.Network;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Build;

namespace VikingEngine.DSSWars.Players
{
    partial class RemotePlayer : AbsHumanPlayer
    {
        PlayerCullingState playerCulling;

        public RemotePlayer(Network.NetworkInstancePeer peer)
            :base()
        {
            peer.Tag = this;
            InitData();
            playerCulling = new PlayerCullingState();
        }


        public void Net_readStatus(System.IO.BinaryReader r)
        {
            playerCulling.readNet(r);
        }

        public override void AutoExpandType(City city, out bool work, out BuildAndExpandType buildType, out bool intelligent)
        {
            work = false;
            buildType = BuildAndExpandType.NUM_NONE;
            intelligent = false;

        }

        public override bool IsAi()
        {
            return false;
        }

        public override string Name => networkPeer.peer.Gamertag;

        public override bool IsLocalPlayer()
        {
            return false;
        }

        public override bool IsLocal => false;
    }


}
    
