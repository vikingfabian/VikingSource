using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Net;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.DSSWars.Players
{
    partial class RemotePlayer : AbsHumanPlayer
    {
        PlayerCullingState playerCulling;
        public bool gotStatus = false;
        public bool newPlayer = true;

        public AbsPlayer previousPlayer;
        public RemotePlayerPointer pointer;

        public RemotePlayer(Network.NetworkInstancePeer peer)
            :base()
        {
            peer.Tag = this;
            this.networkPeer = peer;
            InitData();
            playerCulling = new PlayerCullingState();

            pointer = new RemotePlayerPointer(peer.peer, true);
        }
        public override void AssignFaction(Faction faction)
        {
            previousPlayer = faction.player;
            base.AssignFaction(faction);
            this.profile.StorageIndex = -1;
            this.profile.character = new CharacterProfile(-1);
        }
        // public void AssignFaction(Faction faction)
        //{
            
        //}

        public void Net_readStatus(System.IO.BinaryReader r)
        {
            playerCulling.readNet(r);
            gotStatus = true;
        }

        public override void AutoExpandType(City city, out bool work, out BuildAndExpandType buildType, out bool intelligent)
        {
            work = false;
            buildType = BuildAndExpandType.NUM_NONE;
            intelligent = false;

        }

        public override bool IsBot()
        {
            return false;
        }

        public override string Name => networkPeer.peer.Gamertag;

        public override bool IsLocalPlayer()
        {
            return false;
        }
        public override bool IsRemotePlayer()
        {
            return true;
        }
        public RbTexture FlagTextureToHud()
        {
            return new RbTexture(flagTexture, 1f, 0, 0.2f);
        }

        //public void RemoteToHud(RichBoxContent content)
        //{
        //    if (flagTexture != null)
        //    {
        //        content.Add(new RbBeginTitle(2));
        //        content.Add(FlagTextureToHud());

        //        content.space();
        //    }

        //    if (networkPeer != null)
        //    {
        //        content.Add(new RbText(networkPeer.peer.Gamertag));
        //    }
        //}

        public override bool IsLocal => false;

        public void DeleteMe()
        { 
            pointer.DeleteMe();
        }
    }


}
    
