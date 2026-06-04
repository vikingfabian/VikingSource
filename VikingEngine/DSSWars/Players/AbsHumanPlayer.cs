using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Players
{
    abstract class AbsHumanPlayer : AbsPlayer
    {
        public NetworkInstancePeer networkPeer;
        public GiftedAchievementsPlayerCollection giftedAchievements = new GiftedAchievementsPlayerCollection();
        protected SpottedArray<LocationPin> pins = new SpottedArray<LocationPin>();

        public AbsHumanPlayer(Faction faction, bool newGame)
            : base(faction, newGame)
        { }

        public AbsHumanPlayer()
            : base()
        { }

        virtual public void addNetGamerToHud(RichBoxContent content, bool addStatus)
        {
            //content.Add(new RbBeginTitle(2));

            if (faction == null)
            {
                if (profile.flag != null)
                {
                    var flagTexture = profile.flag.flagDesign.CreateTexture(profile.flag);
                    content.Add(new RbTexture(flagTexture, 1f, 0, 0.2f));
                    content.space();
                }
            }
            else
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
                content.Add(new RbGamerIcon(networkPeer.peer, 0.8f));
                content.space();
                content.Add(new RbText(networkPeer.peer.Gamertag, IsLocal? HudLib.TitleColor_Self : HudLib.TitleColor_Name));

                
            }
        }

        protected void writePins(BinaryWriter w)
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
        override public void AssignFaction(Faction faction)
        {
            
            setPlayerFaction(faction);
            //faction.displayInFullOverview = true;
            base.AssignFaction(faction);
           
        }

        override public AbsHumanPlayer GetHumanPlayer()
        {
            return this;
        }

        public override bool IsHumanPlayer()
        {
            return true;
        }

        public void setPlayerFaction(Faction faction)
        {
            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;
        }
    }
}
