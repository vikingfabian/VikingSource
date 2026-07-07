using Microsoft.Xna.Framework;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;

namespace VikingEngine.DSSWars.Players
{
    abstract partial class AbsHumanPlayer : AbsPlayer
    {
        public NetworkInstancePeer networkPeer;
        public GiftedAchievementsPlayerCollection giftedAchievements = new GiftedAchievementsPlayerCollection();
        public SpottedArray<LocationPin> pins = new SpottedArray<LocationPin>();
        public TimeSpan timePlayed = TimeSpan.Zero;
        public SpriteName voiceState = SpriteName.VoiceDisabled;
        public RbImage voiceIcon = null;
        //protected SpottedArrayCounter<LocationPin> netSharePinCounter;
        int netSharePinIndex = 0;
        public AbsHumanPlayer(Faction faction, bool newGame)
            : base(faction, newGame)
        { }

        public AbsHumanPlayer()
            : base()
        { }

        protected void startingResources()
        {
            faction.addGold_factionWide(DssRef.difficulty.PlayerBonusGold);
        }

        public void addNetGamerIconsToHud(RichBoxContent content, bool factionBanner)
        {
            if (faction == null)
            {
                if (profile.flag != null)
                {
                    var flagTexture = profile.flag.flagDesign.CreateTexture(profile.flag);
                    content.Add(new RbTexture(flagTexture, 1f, 0, 0.2f));
                    //content.space();
                }
            }
            else if (factionBanner && faction.player != null)
            {
                content.Add(faction.FlagTextureToHud());
                //content.space();
            }

            if (networkPeer != null)
            {
                if (Ref.netSession.Host() == networkPeer.peer)
                {
                    content.Add(new RbImage(SpriteName.birdRotatingCrown1));
                    content.space();
                }
                content.Add(new RbGamerIcon(networkPeer.peer, 0.9f));
            }
        }

        virtual public void addNetGamerToHud(RichBoxContent content, bool factionBanner, bool addStatus)
        {
            addNetGamerIconsToHud(content, factionBanner);

            if (networkPeer != null)
            {
                content.space();
                content.Add(new RbText(networkPeer.peer.Gamertag, IsLocal? HudLib.TitleColor_Self : HudLib.TitleColor_Name));
                               
            }
        }

        public void updatePlayer()
        {
            if (networkPeer == null)
            {
                return;
            }

            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.update();
            }

            if (DssRef.time.oneSecond)
            {
                timePlayed = timePlayed.Add(TimeSpan.FromSeconds(1));
            }

            if (!networkPeer.peer.lastvoice.msPassed(SteamManager.VoiceDisplayTimeMs))
            { 
                voiceState = SpriteName.VoiceSoundOn;
            }
            else if (networkPeer.peer.isRecording)
            {
                voiceState = SpriteName.VoiceSoundOff;
            }
            else
            {
                voiceState = SpriteName.VoiceDisabled;
            }
            voiceIcon?.pointer?.SetSpriteName(voiceState);
        }

        virtual public void asynchCullingUpdate(float time, bool bStateA)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.asynchCullingUpdate(time, bStateA);
            }
        }

        public LocationPin rayCollisionWithPin(Ray ray)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                if (pinsC.sel.rayCollision(ray))
                {
                    return pinsC.sel;
                }
            }

            return null;
        }

        public void writePins(BinaryWriter w)
        {
            w.Write((ushort)pins.Count);
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.writeGameState(w);
            }
        }

        public void readPins(BinaryReader r, int subversion)
        {
            int pinsCount = r.ReadUInt16();
            for (int i = 0; i < pinsCount; ++i)
            {
                LocationPin pin = new LocationPin(this, r, subversion);
                pin.myIndex = pins.Add(pin);
                pin.basicInit();
            }
        }


        const int NetWriteMaxPins = 5;
        public void netWritePinUpdate()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssPinUpdate, Network.PacketReliability.Unrelyable, 
                GetLocalPlayer().playerData.localPlayerIndex);

            int pinsArrayLength = pins.Array.Length;
            w.Write((ushort)pinsArrayLength);
            w.Write((ushort)netSharePinIndex);
            for (int i = 0; i < NetWriteMaxPins; ++i)
            {
                int index = netSharePinIndex + i;

                if (index >= pinsArrayLength)
                {
                    break;
                }
                else
                {
                    var pin = pins.GetIndex_Safe(index);
                    if (pin != null)
                    {
                        w.Write(true);
                        pin.writeGameState(w);
                    }
                    else
                    {
                        w.Write(false);
                    }
                }
            }

            netSharePinIndex += NetWriteMaxPins;
            if (netSharePinIndex >= pins.Array.Length)
            {
                netSharePinIndex = 0;
            }
            /*
            if (pins.Count > 0)
            {
                w.Write(true);

                if (!netSharePinCounter.HasMore())
                {
                    netSharePinCounter.Reset();
                }

                int startIndex = netSharePinCounter.CurrentIndex + 1;
                int end = startIndex + NetWriteMaxPins -1;
                EightBit used = new EightBit();
                while (netSharePinCounter.Next())
                {
                    if (netSharePinCounter.sel.netInteractLevel != Network.NetInteractLevel.Hidden)
                    {
                        used.Set(netSharePinCounter.CurrentIndex - startIndex, true);
                        w.Write((ushort)netSharePinCounter.CurrentIndex);
                        netSharePinCounter.sel.writeGameState(w);

                        if (netSharePinCounter.CurrentIndex >= end)//--maxPins <= 0)
                        {
                            break;
                        }
                    }
                }

                //Mark end
                w.Write(ushort.MaxValue);

                //Write empty pins
                w.Write((ushort)startIndex);
                used.write(w);
            }
            else
            {
                w.Write(false);
            }
            */
        }

        public void netReadPinUpdate(BinaryReader r)
        {
            int pinsArrayLength = r.ReadUInt16();
            int netSharePinIndex = r.ReadUInt16();

            for (int i = 0; i < NetWriteMaxPins; ++i)
            {
                int index = netSharePinIndex + i;

                if (index >= pinsArrayLength)
                {
                    break;
                }
                else
                {
                    if (r.ReadBoolean())
                    {
                        netReadPin(index, r);                        
                    }
                    else
                    {
                        var delPin = pins.PullIndex_Safe(index);
                        delPin?.DeleteMe(DeleteReason.NetworkEvent, false);
                    }
                    //var pin = pins.GetIndex_Safe(index);
                    //if (pin == null)
                    //{
                    //    w.Write(false);
                    //}
                    //else
                    //{
                    //    w.Write(true);
                    //    pin.writeGameState(w);
                    //}
                }
            }
            /*
            if (r.ReadBoolean())
            {
                LocationPin pin;
                do
                {
                    int pinIndex = r.ReadUInt16();
                    pin = netReadPin(pinIndex, r);

                } while (pin != null);

                int startIndex = r.ReadUInt16();
                EightBit used = new EightBit(r);

                for (int i = 0; i < NetWriteMaxPins; ++i)
                { 
                    int index = i + startIndex;
                    if (index >= pins.Array.Length)
                    {
                        break;
                    }
                    else if (!used.Get(i))
                    {
                        var delPin = pins.PullIndex_Safe(index);
                        delPin?.DeleteMe(DeleteReason.NetworkEvent, false);
                    }
                }
            }
            else
            {
                clearPins(DeleteReason.NetworkEvent);
            }
            */
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
                    pin = new LocationPin(this.GetRemotePlayer());
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

        public void clearPins(DeleteReason reason)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                pinsC.sel.DeleteMe(reason, false);
            }

            pins.Clear();
        }

        public LocationPin getPin(string name)
        {
            var pinsC = pins.counter();
            while (pinsC.Next())
            {
                if (pinsC.sel.Name(out _) == name)
                {
                    return pinsC.sel;
                }
            }

            return null;
        }

        public void deletePin(int index)
        {
            var pin = pins.PullIndex_Safe(index);
            pin?.DeleteMe(DeleteReason.Disband, false);
        }

        override public void AssignFaction(Faction faction)
        {
            
            setPlayerFaction(faction);

            base.AssignFaction(faction);
           
        }

        protected void playerStartUnits(double unitCountMulti, bool settlerGuard, int honorguards)
        {
            if (faction.cities.Count > 0)
            {
                if (quickMatchUnits(false))
                {
                    return;
                }
                if (settlerGuard)
                {
                    settlerGuardUnits();
                    return;
                }

                int startStrength = (int)faction.militaryStrength;


                IntVector2 onTile = faction.mainCity.ArmySpawnTilePos();
                var mainArmy = faction.NewArmy(onTile);
                mainArmy.Tag = new MapObjectTag(CityTagBack.Blue, MapObjectTag.Tag_SpecializeTradition);

                for (int i = 0; i < MathExt.MultiplyInt(5, unitCountMulti); ++i)
                {
                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);
                }

                if (honorguards > 0)
                {
                    int guardCount = MathExt.MultiplyInt(honorguards, unitCountMulti) - startStrength;

                    SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                    while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
                    {
                        if (citySel != faction.mainCity)
                        {
                            onTile = citySel.ArmySpawnTilePos();
                            var army = faction.NewArmy(onTile);
                            var cityGuardCount = MathExt.MultiplyInt(4, unitCountMulti);
                            for (int i = 0; i < cityGuardCount; ++i)
                            {
                                new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard, army.position);
                                --guardCount;
                            }

                            army.setAsStartArmy();
                            if (guardCount <= 3)
                            {
                                break;
                            }
                        }
                    }

                    for (int i = 0; i < guardCount; ++i)
                    {
                        new SoldierGroup(mainArmy, DssLib.SoldierProfile_HonorGuard, mainArmy.position);
                    }
                }

                mainArmy.setAsStartArmy();
            }
        }

        override public AbsHumanPlayer GetHumanPlayer()
        {
            return this;
        }

        public override bool IsHumanPlayer()
        {
            return true;
        }

        public int AllianceCount_Humans()
        { 
            int count = 0;

            AllHumansLoop humansLoop = new AllHumansLoop();
            while (humansLoop.Next(out bool ready))
            {
                if (ready &&
                    humansLoop.sel != this &&
                    DssRef.world.diplomacy.GetRelation(faction, humansLoop.sel.faction).Relation >=  RelationType.RelationType3_Ally)
                { 
                     count++;
                }
            }

            return count;
        }

        public void setPlayerFaction(Faction faction)
        {
            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;
        }

        virtual public NetSharedClientSettings NetClientSettings()
        {
            var result = new NetSharedClientSettings();
            result.ApplyHostSettings();
            return result;
        }

        virtual public bool IsFriend()
        {
            return true;
        }

        abstract public bool HasSupportDLC();
    }
}
