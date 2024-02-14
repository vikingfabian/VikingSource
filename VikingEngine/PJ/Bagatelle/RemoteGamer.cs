using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Bagatelle
{
    class RemoteGamer : AbsGamer
    {
        Network.AbsNetworkPeer peer;

        public RemoteGamer(int localIx, Network.AbsNetworkPeer peer, VectorRect hudArea, BagatellePlayState state)
            : base(hudArea, localIx, state)
        {
            this.peer = peer;
            initGamer();

            LobbyAvatar.RemoteGamerIconSetup(peer, button);
            refreshHud();
        }

        public void onLostGamer()
        {
            Graphics.Image lostIcon = new Graphics.Image(SpriteName.birdNoNetwork, animal.Position, animal.Size, ImageLayers.AbsoluteBottomLayer, true);
            lostIcon.LayerAbove(lostIcon);

            for (int i = balls.Count - 1; i >= 0; --i)
            {
                RemoveBall(balls[i]);
            }
        }

        public override Network.AbsNetworkPeer NetworkPeer
        {
            get { return peer; }
        }

        public override GamerData GetGamerData()
        {
            Player.RemoteGamerData group = PjLib.GetRemote(peer);
            if (group != null && group.joinedGamers.Count > localGamerIndex)
            {
                return group.joinedGamers[localGamerIndex];
            }

            return new GamerData();
        }
        
    }
}
