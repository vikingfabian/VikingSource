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
using VikingEngine.DSSWars.EntityComponent;
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
        public bool supporterDLC = false;
        public bool waitingForHandover = false;

        public AbsPlayer previousPlayer;
        public int assignedFaction = ushort.MaxValue;
        public FactionType previousFactionType;
        public RemotePlayerPointer pointer;
        public GamerCommunicationSetting communicationSetting; //not implemented

        public NetSharedClientSettings netClientSettings;
        public double incomeMultiplier = 1;

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

            if (faction == null)
            {
                faction = DssRef.world.faction(assignedFaction);
            }
            else if (faction.player == null)
            {
                faction.player = this;
            }
        }

        public override NetSharedClientSettings NetClientSettings()
        {
            return netClientSettings;
        }

        override public bool IsFriend()
        {
            return networkPeer.peer.isFriend;
        }

        public void FirstEnterSetup()
        {
            //This happens after faction is assigned, but before the handover starts sending

            localTooPeacefulPercentage = DssRef.difficulty.tooPeacefulPercentage;
            int honorGuard = DssRef.difficulty.honorGuardCount();

            if (netClientSettings.clientSettings.useHandicap)
            {
                switch (netClientSettings.clientSettings.handicap_botAggression)
                {
                    case HandicapLevel.None:
                        localAiAggressivity = AiAggressivity.Peaceful;
                        localTooPeacefulPercentage = 0;
                        break;

                    case HandicapLevel.Low:
                        if (DssRef.difficulty.aiAggressivity > AiAggressivity.Peaceful)
                        {
                            localAiAggressivity = DssRef.difficulty.aiAggressivity -1;
                            localTooPeacefulPercentage *= 0.6f;
                        }
                        break;

                    case HandicapLevel.Default:
                        localAiAggressivity = AiAggressivity.UseDefault;
                        break;

                    case HandicapLevel.High:
                        if (DssRef.difficulty.aiAggressivity < AiAggressivity.Extreme)
                        {
                            localAiAggressivity = DssRef.difficulty.aiAggressivity + 1;
                            localTooPeacefulPercentage *= 2.5f;
                        }
                        break;
                }

                if (netClientSettings.clientSettings.handicap_extraHonorGuards)
                {
                    honorGuard += 4;
                }

                switch (netClientSettings.clientSettings.handicap_taxIncome)
                {
                    case HandicapLevel.Low:
                        incomeMultiplier = 0.75;
                        break;
                    case HandicapLevel.Default:
                        incomeMultiplier = 1;
                        break;
                    case HandicapLevel.High:
                        incomeMultiplier = 1.25;
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

                if (netClientSettings.clientSettings.useHandicap && 
                    netClientSettings.clientSettings.handicap_resourceBoost)
                {
                    //BOOST RESOURCES
                    citySel.resourceAmountSet(CityResourceIndex.food, 500);
                    citySel.resourceAmountSet(CityResourceIndex.wood, 300);
                    citySel.resourceAmountSet(CityResourceIndex.stone, 300);
                    citySel.resourceAmountSet(CityResourceIndex.skinLinnen, 300);
                    citySel.resourceAmountSet(CityResourceIndex.fuel, 200);
                    citySel.resourceAmountSet(CityResourceIndex.Brick, 100);
                    citySel.resourceAmountSet(CityResourceIndex.iron, 200);
                }
            }

            startingResources();

            ((PlayState)DssRef.state).startingArmySizes(out double unitCountMulti, out bool settlerGuard);
                        
            playerStartUnits(unitCountMulti, settlerGuard, honorGuard);
                        
            AllHumansLoop humans = new AllHumansLoop();
            while (humans.Next(out _))
            {
                if (humans.sel != this)
                {
                    DssRef.world.diplomacy.SetRelationType(faction, humans.sel.faction, null, Ref.netsett.hostSettings.startDiplomacy);
                }
            }
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
        public override void SetColor(Color selected, bool netShare)
        {
            base.SetColor(selected, netShare);

            pointer.colorFrame.Color = selected;
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
        public override bool HasSupportDLC()
        {
            return supporterDLC;
        }
        public override bool IsLocal => false;

        public void DeleteMe()
        {
            clearPins(DeleteReason.LostHost);
            pointer.DeleteMe();
        }
    }
}
    
