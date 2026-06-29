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
        public SpriteName voiceState = SpriteName.cmdHudCross;
        public RbImage voiceIcon = null;

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
                    content.space();
                }
            }
            else if (factionBanner && faction.player != null)
            {
                content.Add(faction.FlagTextureToHud());
                content.space();
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
            //content.Add(new RbBeginTitle(2));

            addNetGamerIconsToHud(content, factionBanner);

            if (networkPeer != null)
            {
                //if (Ref.netSession.Host() == networkPeer.peer)
                //{
                //    content.Add(new RbImage(SpriteName.birdRotatingCrown1));
                //    content.space();
                //}
                //content.Add(new RbGamerIcon(networkPeer.peer, 0.8f));
                content.space();
                content.Add(new RbText(networkPeer.peer.Gamertag, IsLocal? HudLib.TitleColor_Self : HudLib.TitleColor_Name));
                               
            }
        }

        public void updatePlayer()
        {
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
                voiceState = SpriteName.pjNum1;
            }
            else if (networkPeer.peer.isRecording)
            {
                voiceState = SpriteName.pjNum0;
            }
            else
            {
                voiceState = SpriteName.cmdHudCross;
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
            //faction.displayInFullOverview = true;
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
    }
}
