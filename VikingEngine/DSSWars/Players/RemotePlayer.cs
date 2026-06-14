using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.DSSWars.Players
{
    partial class RemotePlayer : AbsHumanPlayer
    {
        PlayerCullingState playerCulling;
        public bool gotStatus = false;
        public bool ready = false;
        public bool newPlayer = true;
        public bool waitingForHandover = false;

        public AbsPlayer previousPlayer;
        public int assignedFaction = ushort.MaxValue;
        public FactionType previousFactionType;
        public RemotePlayerPointer pointer;
        public GamerCommunicationSetting communicationSetting; //not implemented

        public NetSharedClientSettings netClientSettings;

        public RemotePlayer(Network.NetworkInstancePeer peer)
            :base()
        {
            peer.Tag = this;
            this.networkPeer = peer;
            InitData();
            playerCulling = new PlayerCullingState();
            this.profile.StorageIndex = -1;
            this.profile.character = new CharacterProfile(-1);
            pointer = new RemotePlayerPointer(peer.peer, true);
        }
        public void UpdateClient(LocalPlayer playerView)
        {
            base.Update();
            pointer.Update(playerView);
            updatePlayer();
        }

        public void FirstEnterSetup()
        {
            if (netClientSettings.clientSettings.useHandicap)
            {
                switch (netClientSettings.clientSettings.handicap_botAggression)
                {
                    case HandicapLevel.None:
                        localAiAggressivity = AiAggressivity.Peaceful;
                        break;
                    case HandicapLevel.Low:
                        if (DssRef.difficulty.aiAggressivity > AiAggressivity.Peaceful)
                        {
                            localAiAggressivity = DssRef.difficulty.aiAggressivity -1;
                        }
                        break;

                    case HandicapLevel.Default:
                        localAiAggressivity = AiAggressivity.UseDefault;
                        break;

                    case HandicapLevel.High:
                        if (DssRef.difficulty.aiAggressivity < AiAggressivity.Extreme)
                        {
                            localAiAggressivity = DssRef.difficulty.aiAggressivity + 1;
                        }
                        break;
                }
            }

            if (localAiAggressivity == AiAggressivity.UseDefault)
            {
                localAiAggressivity = DssRef.difficulty.aiAggressivity;
            }

            warManagerGear = new WarManagerGear(WarManagerGear.StartGear, localAiAggressivity);

            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
            while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
            {
                citySel.money.copper = Math.Max(citySel.money.copper, Resource.Money.GoldToCopper * 100);
            }

            startingResources();

            ((PlayState)DssRef.state).startingArmySizes(out double unitCountMulti, out bool settlerGuard);

            playerStartUnits(unitCountMulti, settlerGuard);
        }

        public override void addNetGamerToHud(RichBoxContent content, bool factionBanner, bool addStatus)
        {
            base.addNetGamerToHud(content, factionBanner, addStatus);
            if (addStatus && pointer.statusIcon != SpriteName.NO_IMAGE)
            {
                content.space();
                if (pointer.itemIcon != SpriteName.NO_IMAGE)
                {
                    content.Add(new RbOverlapImage( new RbImage(pointer.itemIcon), pointer.statusIcon, Vector2.Zero, 0.7f ));
                }
                else
                {
                    content.Add(new RbImage(pointer.statusIcon));
                }
            }
        }

        public void addNetPingToHud(RichBoxContent content)
        {
            content.text(networkPeer.peer.NetPingString(), HudLib.SecondaryTextColor);
        }

        public override void AssignFaction(Faction faction)
        {
            assignedFaction = faction.myIndex;
            previousPlayer = faction.player;
            previousFactionType = faction.factiontype;
            base.AssignFaction(faction);
            
        }

        public void Net_readStatus(System.IO.BinaryReader r)
        {
            playerCulling.readNet(r);
            gotStatus = true;
            ready = true;
        }

        public LocationPin netReadPin(int index, BinaryReader r)
        {
            if (index == ushort.MaxValue || faction == null)
            {
                return null;
            }
            else
            {
                var pin = pins.GetIndex_Safe(index);
                if (pin == null)
                {
                    pin = new LocationPin(this);
                    pin.myIndex = pins.Add(pin);
                    pin.readGameState(r, int.MaxValue);
                    pin.basicInit();
                }
                else
                {
                    pin.readGameState(r, int.MaxValue);
                }
                return pin;
            }
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
        public override RemotePlayer GetRemotePlayer()
        {
            return this;
        }
        public RbTexture FlagTextureToHud()
        {
            return new RbTexture(flagTexture, 1f, 0, 0.2f);
        }

        public override bool IsLocal => false;

        public void DeleteMe()
        {
            clearPins(DeleteReason.LostHost);
            pointer.DeleteMe();
        }
    }
}
    
